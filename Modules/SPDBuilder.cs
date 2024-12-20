using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathos
{
  internal sealed class SPDBuilder
  {
    public bool Success { get; private set; }

    public void BuildLoop()
    {
      Success = false;
      BranchFails = 300;
      SetParametersLoop();
      SetupRooms();
      if (entrance == null) throw new Exception("Entrance not found.");

      entrance.SetSize();
      entrance.SetPos(0, 0);

      var startAngle = SPDRandom.Float(0, 360);

      SPDDebug.currentmap.mainPathRooms.Insert(0, entrance);
      SPDDebug.currentmap.mainPathRooms.Insert((SPDDebug.currentmap.mainPathRooms.Count + 1) / 2, exit);
      float[] pathTunnels = pathTunnelChances.ToArray();
      foreach (var r in SPDDebug.currentmap.mainPathRooms)
      {
        SPDDebug.currentmap.loop.Add(r);

        int tunnels = SPDRandom.Chances(pathTunnels);
        if (tunnels == -1)
        {
          pathTunnels = pathTunnelChances.ToArray();
          tunnels = SPDRandom.Chances(pathTunnels);
        }
        pathTunnels[tunnels]--;

        for (var j = 0; j < tunnels; j++)
          SPDDebug.currentmap.loop.Add(SPDRoom.NewConnection());
      }

      var prev = entrance;
      float targetAngle;
      for (var i = 1; i < SPDDebug.currentmap.loop.Count; i++)
      {
        var r = SPDDebug.currentmap.loop[i];
        targetAngle = startAngle + TargetAngle(i / (float)SPDDebug.currentmap.loop.Count);

        if (PlaceRoom(SPDDebug.currentmap.initRooms, prev, r, targetAngle) != -1)
        {
          prev = r;
          if (!SPDDebug.currentmap.initRooms.Contains(prev))
            SPDDebug.currentmap.initRooms.Add(prev);
        }
        else
        {
          Success = false;
          return;
        }
      }

      while (!prev.Connect(entrance))
      {
        var c = SPDRoom.NewConnection();
        if (PlaceRoom(SPDDebug.currentmap.loop, prev, c, AngleBetweenRooms(prev, entrance)) == -1)
        {
          Success = false;
          return;
        }
        if (!SPDDebug.currentmap.loop.Contains(prev)) SPDDebug.currentmap.loop.Add(prev);
        if (!SPDDebug.currentmap.initRooms.Contains(prev)) SPDDebug.currentmap.initRooms.Add(prev);
      }

      if (shop != null)
      {
        float angle;
        var tries = 10;
        do
        {
          angle = PlaceRoom(SPDDebug.currentmap.loop, entrance, shop, SPDRandom.Float(360f));
          tries--;
        } while (angle == -1 && tries >= 0);
        if (angle == -1)
        {
          Success = false;
          return;
        }
      }

      foreach (var r in SPDDebug.currentmap.loop)
      {
        SPDDebug.currentmap.loopCenter.x += (r.left + r.right) / 2f;
        SPDDebug.currentmap.loopCenter.y += (r.top + r.bottom) / 2f;
      }
      SPDDebug.currentmap.loopCenter.x /= SPDDebug.currentmap.loop.Count;
      SPDDebug.currentmap.loopCenter.y /= SPDDebug.currentmap.loop.Count;

      var branchable = new Inv.DistinctList<SPDRoom>(SPDDebug.currentmap.loop);
      var roomsToBranch = new Inv.DistinctList<SPDRoom>();
      foreach (var multiconn in SPDDebug.currentmap.multiConnections)
      {
        roomsToBranch.Add(multiconn);
      }
      foreach (var singleconn in SPDDebug.currentmap.singleConnections)
      {
        roomsToBranch.Add(singleconn);
      }
      if (!CreateBranches(SPDDebug.currentmap.initRooms, branchable, roomsToBranch, branchTunnelChances)) return;

      FindNeighbours(SPDDebug.currentmap.initRooms);

      foreach (var r in SPDDebug.currentmap.initRooms)
      {
        foreach (var n in r.neighbours)
        {
          if (!n.connected.ContainsKey(r) && SPDRandom.Float() < 0.30f) r.Connect(n);
        }
      }

      foreach (var room in SPDDebug.currentmap.initRooms)
      {
        SPDDebug.currentmap.finalRooms.Add(room);
      }

      Success = true;
    }
    public void BuildFigureEight()
    {
      Success = false;
      SetParametersFE();

      SetupRooms();
      landmarkRoom = null;
      firstLoop = new Inv.DistinctList<SPDRoom>();
      secondLoop = new Inv.DistinctList<SPDRoom>();
      firstLoopCenter = new SPDPointF();
      secondLoopCenter = new SPDPointF();
      if (landmarkRoom == null)
      {
        foreach (var r in SPDDebug.currentmap.mainPathRooms)
        {
          if (r.MaxConnections(SPDRoom.ALL) >= 4 &&
              (landmarkRoom == null || landmarkRoom.MinWidth() * landmarkRoom.MinHeight() < r.MinWidth() * r.MinHeight()))
            landmarkRoom = r;
        }
      }
      if (SPDDebug.currentmap.multiConnections.Count != 0)
      {
        SPDDebug.currentmap.mainPathRooms.Add(SPDDebug.currentmap.multiConnections[0]);
        SPDDebug.currentmap.multiConnections.RemoveAt(0);
      }
      SPDDebug.currentmap.mainPathRooms.Remove(landmarkRoom);
      SPDDebug.currentmap.multiConnections.Remove(landmarkRoom);

      var startAngle = SPDRandom.Float(0, 360);
      var roomsOnFirstLoop = SPDDebug.currentmap.mainPathRooms.Count / 2;
      if (SPDDebug.currentmap.mainPathRooms.Count % 2 == 1) roomsOnFirstLoop += SPDRandom.Int(2);
      var roomsToLoop = new Inv.DistinctList<SPDRoom>();
      foreach (var roomtemp in SPDDebug.currentmap.mainPathRooms)
      {
        roomsToLoop.Add(roomtemp);
      }

      var firstLoopTemp = new Inv.DistinctList<SPDRoom>();
      firstLoopTemp.Add(landmarkRoom);
      for (var i = 0; i < roomsOnFirstLoop; i++)
      {
        firstLoopTemp.Add(roomsToLoop[0]);
        roomsToLoop.RemoveAt(0);
      }
      firstLoopTemp.Insert((firstLoopTemp.Count + 1) / 2, entrance);

      var pathTunnels = pathTunnelChances.ToArray();
      firstLoop = new Inv.DistinctList<SPDRoom>();
      foreach (var r in firstLoopTemp)
      {
        firstLoop.Add(r);
        var tunnels = SPDRandom.Chances(pathTunnels);
        if (tunnels == -1)
        {
          pathTunnels = pathTunnelChances.ToArray();
          tunnels = SPDRandom.Chances(pathTunnels);
        }
        pathTunnels[tunnels]--;
        for (var j = 0; j < tunnels; j++)
        {
          firstLoop.Add(SPDRoom.NewConnection());
        }
      }

      var secondLoopTemp = new Inv.DistinctList<SPDRoom>();
      secondLoopTemp.Add(landmarkRoom);
      secondLoopTemp.AddRange(roomsToLoop);
      secondLoopTemp.Insert((secondLoopTemp.Count + 1) / 2, exit);
      secondLoop = new Inv.DistinctList<SPDRoom>();
      foreach (var r in secondLoopTemp)
      {
        secondLoop.Add(r);
        var tunnels = SPDRandom.Chances(pathTunnels);
        if (tunnels == -1)
        {
          pathTunnels = pathTunnelChances.ToArray();
          tunnels = SPDRandom.Chances(pathTunnels);
        }
        pathTunnels[tunnels]--;
        for (var j = 0; j < tunnels; j++)
        {
          secondLoop.Add(SPDRoom.NewConnection());
        }
      }

      landmarkRoom.SetSize();
      landmarkRoom.SetPos(0, 0);

      var prev = landmarkRoom;
      for (var i = 1; i < firstLoop.Count; i++)
      {
        var r = firstLoop[i];
        var targetAngle = startAngle + TargetAngle(i / (float)firstLoop.Count);
        if (PlaceRoom(SPDDebug.currentmap.initRooms, prev, r, targetAngle) != -1)
        {
          prev = r;
          if (!SPDDebug.currentmap.initRooms.Contains(prev)) SPDDebug.currentmap.initRooms.Add(r);
        }
        else return;
      }

      while (!prev.Connect(landmarkRoom))
      {
        var c = SPDRoom.NewConnection();
        if (PlaceRoom(firstLoop, prev, c, AngleBetweenRooms(prev, landmarkRoom)) == -1) return;
        firstLoop.Add(c);
        SPDDebug.currentmap.initRooms.Add(c);
      }

      prev = landmarkRoom;
      startAngle += 180f;
      for (int i = 1; i < secondLoop.Count; i++)
      {
        var r = secondLoop[i];
        var targetAngle = startAngle + TargetAngle(i / (float)secondLoop.Count);
        if (PlaceRoom(SPDDebug.currentmap.initRooms, prev, r, targetAngle) != -1)
        {
          prev = r;
          if (!SPDDebug.currentmap.initRooms.Contains(r)) SPDDebug.currentmap.initRooms.Add(r);
        }
        else return;
      }

      while (!prev.Connect(landmarkRoom))
      {
        var c = SPDRoom.NewConnection();
        if (PlaceRoom(secondLoop, prev, c, AngleBetweenRooms(prev, landmarkRoom)) == -1) return;
        secondLoop.Add(c);
        SPDDebug.currentmap.initRooms.Add(c);
      }

      if (shop != null)
      {
        float angle;
        int tries = 10;
        do
        {
          angle = PlaceRoom(SPDDebug.currentmap.initRooms, entrance, shop, SPDRandom.Float(360f));
          tries--;
        } while (angle == -1 && tries >= 0);
        if (angle == -1) return;
      }

      firstLoopCenter = new SPDPointF();
      foreach (var r in firstLoop)
      {
        firstLoopCenter.x += (r.left + r.right) / 2f;
        firstLoopCenter.y += (r.top + r.bottom) / 2f;
      }
      firstLoopCenter.x /= firstLoop.Count;
      firstLoopCenter.y /= firstLoop.Count;
      secondLoopCenter = new SPDPointF();
      foreach (var r in secondLoop)
      {
        secondLoopCenter.x += (r.left + r.right) / 2f;
        secondLoopCenter.y += (r.top + r.bottom) / 2f;
      }
      secondLoopCenter.x /= secondLoop.Count;
      secondLoopCenter.y /= secondLoop.Count;

      var branchable = new Inv.DistinctList<SPDRoom>(firstLoop);
      branchable.Remove(landmarkRoom);
      branchable.AddRange(secondLoop);
      var roomsToBranch = new Inv.DistinctList<SPDRoom>();
      roomsToBranch.AddRange(SPDDebug.currentmap.multiConnections);
      roomsToBranch.AddRange(SPDDebug.currentmap.singleConnections);
      if (!CreateBranches(SPDDebug.currentmap.initRooms, branchable, roomsToBranch, branchTunnelChances)) return;
      FindNeighbours(SPDDebug.currentmap.initRooms);
      foreach (var r in SPDDebug.currentmap.initRooms)
      {
        foreach (var n in r.neighbours)
        {
          if (!n.connected.ContainsKey(r) && SPDRandom.Float() < 0.30f) r.Connect(n);
        }
      }

      SPDDebug.currentmap.finalRooms.AddRange(SPDDebug.currentmap.initRooms);

      Success = true;
    }

    private void SetParametersFE()
    {
      curveExponent = 2;
      curveIntensity = SPDRandom.Float(0.3f, 0.8f);
      curveOffset = 0f;
    }
    private void SetParametersLoop()
    {
      curveExponent = 2;
      curveIntensity = SPDRandom.Float(0f, 0.65f);
      curveOffset = SPDRandom.Float(0f, 0.50f);
    }
    private void SetupRooms()
    {
      foreach (var r in SPDDebug.currentmap.initRooms)
        r.SetEmpty();

      entrance = exit = shop = null;
      SPDDebug.currentmap.mainPathRooms.Clear();
      SPDDebug.currentmap.singleConnections.Clear();
      SPDDebug.currentmap.multiConnections.Clear();
      foreach (var r in SPDDebug.currentmap.initRooms)
      {
        if (r.type == 0 && r.flavor == SPDFlavour.Entrance)
        {
          entrance = r;
        }
        else if (r.type == 0 && r.flavor == SPDFlavour.Exit)
        {
          exit = r;
        }
        else if (r.type == 2 && r.flavor == SPDFlavour.Shop && r.MaxConnections(SPDRoom.ALL) == 1)
        {
          shop = r;
        }
        else if (r.MaxConnections(SPDRoom.ALL) > 1)
        {
          SPDDebug.currentmap.multiConnections.Add(r);
        }
        else if (r.MaxConnections(SPDRoom.ALL) == 1)
        {
          SPDDebug.currentmap.singleConnections.Add(r);
        }
      }
      // No weightRooms() because DistinctList<> doesnt allow.
      SPDDebug.currentmap.multiConnections.Shuffle();

      var roomsOnMainPath = (int)(SPDDebug.currentmap.multiConnections.Count * pathLength) + SPDRandom.Chances(pathLenJitterChances);

      while (roomsOnMainPath > 0 && SPDDebug.currentmap.multiConnections.Count > 0)
      {
        var r = SPDDebug.currentmap.multiConnections[0];
        roomsOnMainPath--;
        SPDDebug.currentmap.mainPathRooms.Add(r);
        SPDDebug.currentmap.multiConnections.Remove(r);
      }
    }
    private float PlaceRoom(Inv.DistinctList<SPDRoom> collision, SPDRoom prev, SPDRoom next, float angle)
    {
      angle %= 360f;
      if (angle < 0)
      {
        angle += 360f;
      }

      var prevCenter = new SPDPointF((prev.left + prev.right) / 2f, (prev.top + prev.bottom) / 2f);

      double m = Math.Tan(angle / A + Math.PI / 2.0);
      double b = prevCenter.y - m * prevCenter.x;

      SPDPoint start;
      int direction;
      if (Math.Abs(m) >= 1)
      {
        if (angle < 90 || angle > 270)
        {
          direction = SPDRoom.TOP;
          start = new SPDPoint((int)Math.Round((prev.top - b) / m), prev.top);
        }
        else
        {
          direction = SPDRoom.BOTTOM;
          start = new SPDPoint((int)Math.Round((prev.bottom - b) / m), prev.bottom);
        }
      }
      else
      {
        if (angle < 180)
        {
          direction = SPDRoom.RIGHT;
          start = new SPDPoint(prev.right, (int)Math.Round(m * prev.right + b));
        }
        else
        {
          direction = SPDRoom.LEFT;
          start = new SPDPoint(prev.left, (int)Math.Round(m * prev.left + b));
        }
      }

      if (direction == SPDRoom.TOP || direction == SPDRoom.BOTTOM)
      {
        start.x = (int)Gate(prev.left + 1, start.x, prev.right - 1);
      }
      else
      {
        start.y = (int)Gate(prev.top + 1, start.y, prev.bottom - 1);
      }

      var space = FindFreeSpace(start, collision, Math.Max(next.MaxWidth(), next.MaxHeight()));
      if (!next.SetSizeWithLimit(space.Width() + 1, space.Height() + 1))
      {
        return -1;
      }

      var targetCenter = new SPDPointF();
      if (direction == SPDRoom.TOP)
      {
        targetCenter.y = prev.top - (next.Height() - 1) / 2f;
        targetCenter.x = (float)((targetCenter.y - b) / m);
        next.SetPos(Convert.ToInt32(targetCenter.x - (next.Width() - 1) / 2f), prev.top - (next.Height() - 1));

      }
      else if (direction == SPDRoom.BOTTOM)
      {
        targetCenter.y = prev.bottom + (next.Height() - 1) / 2f;
        targetCenter.x = (float)((targetCenter.y - b) / m);
        next.SetPos(Convert.ToInt32(targetCenter.x - (next.Width() - 1) / 2f), prev.bottom);

      }
      else if (direction == SPDRoom.RIGHT)
      {
        targetCenter.x = prev.right + (next.Width() - 1) / 2f;
        targetCenter.y = (float)(m * targetCenter.x + b);
        next.SetPos(prev.right, Convert.ToInt32(targetCenter.y - (next.Height() - 1) / 2f));

      }
      else if (direction == SPDRoom.LEFT)
      {
        targetCenter.x = prev.left - (next.Width() - 1) / 2f;
        targetCenter.y = (float)(m * targetCenter.x + b);
        next.SetPos(prev.left - (next.Width() - 1), Convert.ToInt32(targetCenter.y - (next.Height() - 1) / 2f));

      }

      if (direction == SPDRoom.TOP || direction == SPDRoom.BOTTOM)
      {
        if (next.right < prev.left + 2) next.Shift(prev.left + 2 - next.right, 0);
        else if (next.left > prev.right - 2) next.Shift(prev.right - 2 - next.left, 0);

        if (next.right > space.right) next.Shift(space.right - next.right, 0);
        else if (next.left < space.left) next.Shift(space.left - next.left, 0);
      }
      else
      {
        if (next.bottom < prev.top + 2) next.Shift(0, prev.top + 2 - next.bottom);
        else if (next.top > prev.bottom - 2) next.Shift(0, prev.bottom - 2 - next.top);

        if (next.bottom > space.bottom) next.Shift(0, space.bottom - next.bottom);
        else if (next.top < space.top) next.Shift(0, space.top - next.top);
      }
      if ((next.Height() <= 0) || (next.Width() <= 0) || prev.Height() <= 0 || prev.Width() <= 0) throw new Exception("room smaller than 0");

      if (next.Connect(prev))
      {
        return AngleBetweenRooms(prev, next);
      }
      else
      {
        return -1;
      }
    }
    private float AngleBetweenRooms(SPDRoom from, SPDRoom to)
    {
      var fromCenter = new SPDPointF((from.left + from.right) / 2f, (from.top + from.bottom) / 2f);
      var toCenter = new SPDPointF((to.left + to.right) / 2f, (to.top + to.bottom) / 2f);
      return AngleBetweenPoints(fromCenter, toCenter);
    }
    private float AngleBetweenPoints(SPDPointF from, SPDPointF to)
    {
      double m = (to.y - from.y) / (to.x = from.x);

      var angle = (float)(A * (Math.Atan(m) + Math.PI / 2.0));
      if (from.x > to.x) angle -= 180f;
      return angle;
    }
    private SPDRect FindFreeSpace(SPDPoint start, Inv.DistinctList<SPDRoom> collision, int maxSize)
    {
      var space = new SPDRect(start.x - maxSize, start.y - maxSize, start.x + maxSize, start.y + maxSize);
      var colliding = new Inv.DistinctList<SPDRoom>(collision);
      do
      {
        foreach (var room in colliding)
        {
          if (room.IsEmpty()
              || Math.Max(space.left, room.left) >= Math.Min(space.right, room.right)
              || Math.Max(space.top, room.top) >= Math.Min(space.bottom, room.bottom))
            colliding.Remove(room);
        }

        SPDRoom closestRoom = null;
        var closestDiff = Int32.MaxValue;
        var inside = true;
        var curDiff = 0;
        foreach (var curRoom in colliding)
        {
          if (start.x <= curRoom.left)
          {
            inside = false;
            curDiff += curRoom.left - start.x;
          }
          else if (start.x >= curRoom.right)
          {
            inside = false;
            curDiff += start.x - curRoom.right;
          }

          if (start.y <= curRoom.top)
          {
            inside = false;
            curDiff += curRoom.top - start.y;
          }
          else if (start.y >= curRoom.bottom)
          {
            inside = false;
            curDiff += start.y - curRoom.bottom;
          }

          if (inside)
          {
            space.Set(start.x, start.y, start.x, start.y);
            return space;
          }

          if (curDiff < closestDiff)
          {
            closestDiff = curDiff;
            closestRoom = curRoom;
          }
        }

        int wDiff, hDiff;
        if (closestRoom != null)
        {
          wDiff = Int32.MaxValue;
          if (closestRoom.left >= start.x)
          {
            wDiff = (space.right - closestRoom.left) * (space.Height() + 1);
          }
          else if (closestRoom.right <= start.x)
          {
            wDiff = (closestRoom.right - space.left) * (space.Height() + 1);
          }

          hDiff = Int32.MaxValue;
          if (closestRoom.top >= start.y)
          {
            hDiff = (space.bottom - closestRoom.top) * (space.Width() + 1);
          }
          else if (closestRoom.bottom <= start.y)
          {
            hDiff = (closestRoom.bottom - space.top) * (space.Width() + 1);
          }

          if (wDiff < hDiff || wDiff == hDiff && SPDRandom.Int(2) == 0)
          {
            if (closestRoom.left >= start.x && closestRoom.left < space.right) space.right = closestRoom.left;
            if (closestRoom.right <= start.x && closestRoom.right > space.left) space.left = closestRoom.right;
          }
          else
          {
            if (closestRoom.top >= start.y && closestRoom.top < space.bottom) space.bottom = closestRoom.top;
            if (closestRoom.bottom <= start.y && closestRoom.bottom > space.top) space.top = closestRoom.bottom;
          }
          colliding.Remove(closestRoom);
        }
        else
        {
          colliding.Clear();
        }
      } while (!(colliding.Count == 0));

      return space;
    }
    private float TargetAngle(float percentAlong)
    {
      percentAlong += curveOffset;
      return 360f * (float)(curveIntensity * CurveEquation(percentAlong)
          + (1 - curveIntensity) * percentAlong - curveOffset);
    }
    private double CurveEquation(double x)
    {
      return Math.Pow(4, 2 * curveExponent) *
          (Math.Pow((x % 0.5f) - 0.25, 2 * curveExponent + 1)) +
          0.25 + 0.5 * Math.Floor(2 * x);
    }
    private static int BranchFails;
    private bool CreateBranches(Inv.DistinctList<SPDRoom> rooms, Inv.DistinctList<SPDRoom> branchable, Inv.DistinctList<SPDRoom> roomsToBranch, float[] connChances)
    {
      int i = 0;
      float angle;
      int tries;
      SPDRoom curr;
      var connectingRoomsThisBranch = new Inv.DistinctList<SPDRoom>();
      float[] connectionChances = connChances.ToArray();

      while (i < roomsToBranch.Count)
      {
        var r = roomsToBranch[i];
        connectingRoomsThisBranch.Clear();

        do
        {
          curr = branchable[SPDRandom.Int(branchable.Count)];
        } while (r.type == 3 && curr.type == 4);

        int connectingRooms = SPDRandom.Chances(connectionChances);
        if (connectingRooms == -1)
        {
          connectionChances = connChances.ToArray();
          connectingRooms = SPDRandom.Chances(connectionChances);
        }
        connectionChances[connectingRooms]--;

        for (var j = 0; j < connectingRooms; j++)
        {
          var t = SPDRoom.NewConnection();
          tries = 3;

          do
          {
            angle = PlaceRoom(rooms, curr, t, RandomBranchAngle(curr));
            tries--;
          } while (angle == -1 && tries > 0);

          if (angle == -1)
          {
            t.ClearConnections();
            foreach (var c in connectingRoomsThisBranch)
            {
              c.ClearConnections();
              rooms.Remove(c);
            }
            connectingRoomsThisBranch.Clear();
            break;
          }
          else
          {
            connectingRoomsThisBranch.Add(t);
            rooms.Add(t);
          }

          curr = t;
        }

        if (connectingRoomsThisBranch.Count != connectingRooms)
        {
          if (BranchFails > 0)
          {
            BranchFails--;
            continue;
          }
          else return false;
        }

        tries = 10;

        do
        {
          angle = PlaceRoom(rooms, curr, r, RandomBranchAngle(curr));
          tries--;
        } while (angle == -1 && tries > 0);

        if (angle == -1)
        {
          r.ClearConnections();
          foreach (var t in connectingRoomsThisBranch)
          {
            t.ClearConnections();
            rooms.Remove(t);
          }
          connectingRoomsThisBranch.Clear();
          continue;
        }

        for (var j = 0; j < connectingRoomsThisBranch.Count; j++)
        {
          if (SPDRandom.Int(3) <= 1)
            branchable.Add(connectingRoomsThisBranch[j]);
        }

        if (r.MaxConnections(SPDRoom.ALL) > 1 && SPDRandom.Int(3) == 0)
          branchable.Add(r);

        i++;
      }

      return true;
    }
    private float RandomBranchAngle(SPDRoom r)
    {
      if (SPDDebug.currentmap.loopCenter == null)
      {
        return SPDRandom.Float(360f);
      }
      else
      {
        var toCenter = AngleBetweenPoints(new SPDPointF((r.left + r.right) / 2f, (r.top + r.bottom) / 2f), SPDDebug.currentmap.loopCenter);
        if (toCenter < 0) toCenter += 360f;

        var currAngle = SPDRandom.Float(360f);
        for (var i = 0; i < 4; i++)
        {
          var newAngle = SPDRandom.Float(360f);
          if (Math.Abs(toCenter - newAngle) < Math.Abs(toCenter - currAngle))
          {
            currAngle = newAngle;
          }
        }
        return currAngle;
      }
    }
    private void FindNeighbours(Inv.DistinctList<SPDRoom> rooms)
    {
      var ra = new Inv.DistinctList<SPDRoom>(rooms);
      for (var i = 0; i < (ra.Count - 1); i++)
      {
        for (var j = i + 1; j < ra.Count; j++)
        {
          ra[i].AddNeighbour(ra[j]);
        }
      }
    }

    private SPDRoom entrance;
    private SPDRoom exit;
    private SPDRoom shop;
    private SPDRoom landmarkRoom;
    private Inv.DistinctList<SPDRoom> firstLoop;
    private Inv.DistinctList<SPDRoom> secondLoop;
    private SPDPointF firstLoopCenter;
    private SPDPointF secondLoopCenter;
    private float curveOffset = 0;
    private float curveIntensity = 1;
    private int curveExponent = 0;
    private readonly float pathLength = 0.25f;
    private readonly double A = 180 / Math.PI;
    private readonly float[] pathLenJitterChances = new float[] { 0, 0, 0, 1 };
    private readonly float[] branchTunnelChances = new float[] { 2, 2, 1 };
    private readonly float[] pathTunnelChances = new float[] { 1, 3, 1 };

    public static float Gate(float min, float value, float max)
    {
      if (value < min)
        return min;
      else if (value > max)
        return max;
      else
        return value;
    }
  }
}