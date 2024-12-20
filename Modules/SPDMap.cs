using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  internal sealed class SPDMap
  {
    public int depth;
    public int feeling;

    public int width;
    public int height;

    public Inv.DistinctList<SPDMapPoint> map;

    // Feelings:
    // 0. Normal
    // 1. Large
    // 2. Traps
    // 3. Secrets

    public int standardRooms;
    public int specialRooms;
    public readonly SPDPointF loopCenter = new SPDPointF();
    public readonly Inv.DistinctList<SPDRoom> initRoomsSpecial = new Inv.DistinctList<SPDRoom>();
    public readonly Inv.DistinctList<SPDRoom> initRoomsStandard = new Inv.DistinctList<SPDRoom>();
    public readonly Inv.DistinctList<SPDRoom> initRoomsSecret = new Inv.DistinctList<SPDRoom>();
    public readonly Inv.DistinctList<SPDRoom> initRooms = new Inv.DistinctList<SPDRoom>();
    public readonly Inv.DistinctList<SPDRoom> finalRooms = new Inv.DistinctList<SPDRoom>();

    public readonly Inv.DistinctList<SPDRoom> mainPathRooms = new Inv.DistinctList<SPDRoom>();
    public readonly Inv.DistinctList<SPDRoom> multiConnections = new Inv.DistinctList<SPDRoom>();
    public readonly Inv.DistinctList<SPDRoom> singleConnections = new Inv.DistinctList<SPDRoom>();
    public readonly Inv.DistinctList<SPDRoom> loop = new Inv.DistinctList<SPDRoom>();

    public readonly Inv.DistinctList<SPDMapPoint> monsters = new Inv.DistinctList<SPDMapPoint>();
    public readonly Inv.DistinctList<SPDMapPoint> items = new Inv.DistinctList<SPDMapPoint>();
    public readonly Inv.DistinctList<SPDMapPoint> traps = new Inv.DistinctList<SPDMapPoint>();
    public readonly Inv.DistinctList<SPDMapPoint> facilities = new Inv.DistinctList<SPDMapPoint>();
    public readonly Inv.DistinctList<SPDMapPoint> blocks = new Inv.DistinctList<SPDMapPoint>();

    public Pathos.Map pathosMap;
    public SPDPoint entrance;
    public SPDPoint exit;

    public readonly Inv.DistinctList<SPDItem> itemsToAdd = new Inv.DistinctList<SPDItem>();
    public readonly Inv.DistinctList<Entity> monstersToAdd = new Inv.DistinctList<Entity>();

    public void SetSize(int w, int h)
    {
      this.width = w;
      this.height = h;

      this.map = new Inv.DistinctList<SPDMapPoint>(width * height);
      for (var y = 0; y < height; y++)
      {
        for (var x = 0; x < width; x++)
          map.Add(new SPDMapPoint(x, y, 0));
      }
    }
  }

  internal sealed class SPDItem
  {
    public SPDItem(Item Item, int? Quantity)
    {
      this.Item = Item;
      this.Quantity = Quantity;
    }

    public Item Item { get; }
    public int? Quantity { get; }
  }
}