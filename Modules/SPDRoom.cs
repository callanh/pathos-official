using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
    enum SPDFlavour
    {
      None,
      Entrance,
      Exit,
      Connection,
      Empty, 
      Altar, 
      Aquarium, 
      Armory, 
      Artillery, 
      ChestZoo,
      Crypt, 
      DarkShrine, 
      Farm,
      Garden,
      GrassyGrave, 
      Graves, 
      Hive, 
      Hoard, 
      HolyShrine, 
      Honeypot, 
      Laboratory, 
      Larder,
      LeprechaunHall, 
      Library, 
      Pit,
      Pool,
      SacredGrove,
      SecretShop, 
      Shop,
      Statue, 
      Storage, 
      Striped,
      Study, 
      Summoning, 
      SuspiciousChest,
      Traps, 
      Treasury,
      Vault,
      Zoo
    }

    internal sealed class SPDRoom : SPDRect
    {
        public static readonly int ALL = 0;
        public static readonly int LEFT = 1;
        public static readonly int TOP = 2;
        public static readonly int RIGHT = 3;
        public static readonly int BOTTOM = 4;

        public static SPDRoom NewStandard()
        {
            var room = new SPDRoom();
            room.type = 1;
            room.flavor = SelectStandardType();
            return room;
        }
        public static SPDRoom NewSpecial()
        {
            var room = new SPDRoom();
            room.type = 2;
            room.flavor = SelectSpecialType();
            return room;
        }
        public static SPDRoom NewSecret()
        {
            var room = new SPDRoom();
            room.type = 3;
            room.flavor = SelectSecretType();
            return room;
        }
        public static SPDRoom NewConnection()
        {
            var room = new SPDRoom();
            room.type = 4;
            room.flavor = SPDFlavour.Connection;
            return room;
        }

        public int type = 0;
        public SPDFlavour flavor = SPDFlavour.None;
        public readonly Inv.DistinctList<SPDRoom> neighbours = new Inv.DistinctList<SPDRoom>();
        public readonly Dictionary<SPDRoom, SPDPoint> connected = new Dictionary<SPDRoom, SPDPoint>();

        public new int Width() => base.Width() + 1;
        public new int Height() => base.Height() + 1;
        public int MinWidth()
        {
            switch (type)
            {
                case 0:
                    return 5;
                case 1:
                    switch (flavor)
                    {
                        case SPDFlavour.Aquarium: return 8;
                        default: return 5;
                    }
                case 2: return 5;
                case 3: return 5;
                default:
                    return 5;
            }
        }
        public int MinHeight() => MinWidth();
        public int MaxWidth()
        {
            switch (type)
            {
                case 0: return 9;
                case 1: switch (flavor)
                    {
                        case SPDFlavour.LeprechaunHall: return 9;
                        case SPDFlavour.Zoo: return 9;
                        case SPDFlavour.Hive: return 9;
                        case SPDFlavour.DarkShrine: return 9;
                        case SPDFlavour.HolyShrine: return 9;
                        case SPDFlavour.SacredGrove: return 9;
                        default: return 11;
                    }
                case 2: switch (flavor)
                    {
                        case SPDFlavour.Altar: return 7;
                        case SPDFlavour.Pit: return 7;
                        default: return 10;
                    }
                case 3: switch (flavor)
                    {
                        case SPDFlavour.ChestZoo: return 9;
                        case SPDFlavour.Hoard: return 9;
                        case SPDFlavour.Summoning: return 9;
                        default: return 11;
                    }
            }
            return 10;
        }
        public int MaxHeight() => MaxWidth();
        public SPDPoint Random(int m)
        {
            return new SPDPoint(SPDRandom.IntRange(left + m, right - m), SPDRandom.IntRange(top + m, bottom - m));
        }
        public SPDPoint Random()
        {
            return Random(1);
        }
        public IEnumerable<SPDPoint> PointsInside()
        {
            foreach (var point in GetPoints())
            {
                if (!(point.x == left || point.x == right || point.y == top || point.y == bottom)) 
                    yield return point;
            }
        }
        public SPDPoint GetDoorCenter()
        {
            var doorCenter = new SPDPointF(0, 0);
            foreach (var door in connected.Values)
            {
                doorCenter.x += door.x;
                doorCenter.y += door.y;
            }
            var c = new SPDPoint((int)doorCenter.x / connected.Count, (int)doorCenter.y / connected.Count);
            if (SPDRandom.Float() < doorCenter.x % 1) c.x++;
            if (SPDRandom.Float() < doorCenter.y % 1) c.y++;
            c.x = (int)SPDBuilder.Gate(left + 1, c.x, right - 1);
            c.y = (int)SPDBuilder.Gate(top + 1, c.y, bottom - 1);

            return c;
        }
        public SPDRect GetConnectionSpace()
        {
            var c = GetDoorCenter();
            return new SPDRect(c.x, c.y, c.x, c.y);
        }
        public SPDPoint Entrance()
        {
            if (connected.Count == 0)
            {
                return null;
            }
            else
            {
                return connected.Values.ElementAt(0);
            }
        }
        public int MaxConnections(int direction)
        {
            if (type == 2 || type == 3) return 1;
            else if (direction == ALL) return 16;
            else return 4;
        }
        public bool SetSize()
        {
            return SetSize(MinWidth(), MaxWidth(), MinHeight(), MaxHeight());
        }
        public bool SetSize(int minW, int maxW, int minH, int maxH)
        {
            if (minW < MinWidth() || maxW > MaxWidth() || minH < MinHeight() || minW > maxW || minH > maxH) return false;
            else
            {
                Resize(SPDRandom.NormalIntRange(minW, maxW) - 1, SPDRandom.NormalIntRange(minH, maxH) - 1);
                return true;
            }
        }
        public bool SetSizeWithLimit(int w, int h)
        {
            if (w < MinWidth() || h < MinHeight())
            {
                return false;
            }
            else
            {
                SetSize();
                if (Width() > w || Height() > h)
                {
                    Resize(Math.Min(Width(), w) - 1, Math.Min(Height(), h) - 1);
                }
                if (Width() < 0 || Height() < 0) return false;
                return true;
            }
        }
        public bool Connect(SPDRoom room)
        {
            if ((neighbours.Contains(room) || AddNeighbour(room)) && !connected.ContainsKey(room) && CanConnect(room))
            {
                connected.Add(room, null);
                room.connected.Add(this, null);
                return true;
            }
            return false;
        }
        public bool AddNeighbour(SPDRoom other)
        {
            if (neighbours.Contains(other)) return true;

            var i = Intersect(other);
            if ((i.Width() == 0 && i.Height() >= 2) || (i.Height() == 0 && i.Width() >= 2))
            {
                neighbours.Add(other);
                other.neighbours.Add(this);
                return true;
            }
            return false;
        }
        public bool CanConnect(SPDRoom r)
        {
            var i = Intersect(r);

            bool foundPoint = false;
            foreach (var p in i.GetPoints())
            {
                if (CanConnect(p) && r.CanConnect(p))
                {
                    foundPoint = true;
                    break;
                }
            }
            if (!foundPoint) return false;

            if (i.Width() == 0 && i.left == left) return CanConnect(LEFT) && r.CanConnect(RIGHT);
            else if (i.Height() == 0 && i.top == top) return CanConnect(TOP) && r.CanConnect(BOTTOM);
            else if (i.Width() == 0 && i.right == right) return CanConnect(RIGHT) && r.CanConnect(LEFT);
            else if (i.Height() == 0 && i.bottom == bottom) return CanConnect(BOTTOM) && r.CanConnect(TOP);
            else return false;
        }
        public bool CanConnect(int direction)
        {
            return RemConnections(direction) > 0;
        }
        public int CurConnections(int direction)
        {
            if (direction == ALL)
            {
                return connected.Count;
            }
            else
            {
                var total = 0;
                foreach (var r in connected.Keys)
                {
                    var i = Intersect(r);
                    if (direction == LEFT && i.Width() == 0 && i.left == left) total++;
                    else if (direction == TOP && i.Height() == 0 && i.top == top) total++;
                    else if (direction == RIGHT && i.Width() == 0 && i.right == right) total++;
                    else if (direction == BOTTOM && i.Height() == 0 && i.bottom == bottom) total++;
                }
                return total;
            }
        }
        public bool CanConnect(SPDPoint p)
        {
            return (p.x == left || p.x == right) != (p.y == top || p.y == bottom);
        }
        public int RemConnections(int direction)
        {
            if (CurConnections(ALL) >= MaxConnections(ALL)) return 0;
            else return MaxConnections(direction) - CurConnections(direction);
        }
        public void ClearConnections()
        {
            foreach (var r in neighbours)
            {
                r.neighbours.Remove(this);
            }
            neighbours.Clear();
            foreach (var r in connected.Keys)
            {
                r.connected.Remove(this);
            }
            connected.Clear();
        }

        private static SPDFlavour SelectStandardType()
        {
          var chances = new float[] { 32, 7, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
          var type = StandardTypesArray[SPDRandom.Chances(chances)];
        
          if (type == SPDFlavour.DarkShrine && SPDDebug.currentmap.initRoomsStandard.Find(r => r.type == 1 && r.flavor == type) != null)
          {
            chances[10]--;
            return StandardTypesArray[SPDRandom.Chances(chances)];
          }
          else if (type == SPDFlavour.HolyShrine && SPDDebug.currentmap.initRoomsStandard.Find(r => r.type == 1 && r.flavor == type) != null)
          {
            chances[11]--;
            return StandardTypesArray[SPDRandom.Chances(chances)];
          }
          else if (type == SPDFlavour.SacredGrove && SPDDebug.currentmap.initRoomsStandard.Find(r => r.type == 1 && r.flavor == type) != null)
          {
            chances[12]--;
            return StandardTypesArray[SPDRandom.Chances(chances)];
          }
        
          return type;
        }
        private static SPDFlavour SelectSpecialType()
        {
          return SpecialTypeArray[SPDRandom.Int(SpecialTypeArray.Length)];
        }
        private static SPDFlavour SelectSecretType()
        {
          return SecretTypeArray[SPDRandom.Int(SecretTypeArray.Length)];
        }

        private static readonly SPDFlavour[] StandardTypesArray = new SPDFlavour[]
        { 
            SPDFlavour.Empty, SPDFlavour.Striped,
            SPDFlavour.Aquarium, SPDFlavour.LeprechaunHall, SPDFlavour.Zoo, SPDFlavour.Garden,
            SPDFlavour.Hive, SPDFlavour.GrassyGrave, SPDFlavour.Study, SPDFlavour.SuspiciousChest,
            SPDFlavour.DarkShrine, SPDFlavour.HolyShrine, SPDFlavour.SacredGrove 
        };
        private static readonly SPDFlavour[] SpecialTypeArray = new SPDFlavour[]
        {
            SPDFlavour.Crypt, SPDFlavour.Pool,
            SPDFlavour.Library, SPDFlavour.Armory, SPDFlavour.Treasury,
            SPDFlavour.Traps, SPDFlavour.Storage, SPDFlavour.Statue, SPDFlavour.Vault,
            SPDFlavour.Laboratory, SPDFlavour.Altar, SPDFlavour.Pit
        };
        private static readonly SPDFlavour[] SecretTypeArray = new SPDFlavour[]
        {
            SPDFlavour.Garden, SPDFlavour.Laboratory, SPDFlavour.Library, SPDFlavour.Larder,
            SPDFlavour.SecretShop, SPDFlavour.Graves, SPDFlavour.Artillery, SPDFlavour.ChestZoo,
            SPDFlavour.Honeypot, SPDFlavour.Hoard, SPDFlavour.Summoning, SPDFlavour.Farm//, SPDFlavour.Maze
        };
    }
}