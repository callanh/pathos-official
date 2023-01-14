using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathos
{
    internal sealed class SPDPainter
    {
        public void Paint()
        {
            var leftMost = int.MaxValue;
            var topMost = int.MaxValue;
            foreach (var r in SPDDebug.currentmap.finalRooms)
            {
                if (r.left < leftMost) leftMost = r.left;
                if (r.top < topMost) topMost = r.top;
            }
            leftMost -= 1;
            topMost -= 1;
            var rightMost = 0;
            var bottomMost = 0;
            foreach (var r in SPDDebug.currentmap.finalRooms)
            {
                r.Shift(-leftMost, -topMost);
                if (r.right > rightMost) rightMost = r.right;
                if (r.bottom > bottomMost) bottomMost = r.bottom;
            }
            rightMost += 1;
            bottomMost += 1;

            SPDDebug.currentmap.SetSize(rightMost+1, bottomMost+1);

            foreach (var r in SPDDebug.currentmap.finalRooms)
            {
                if (r.type == 3 || r.type == 2 || r.type == 1)
                {
                    PlaceDoors(r);
                    PaintRoom(r);
                }
            }
            foreach (var r in SPDDebug.currentmap.finalRooms)
            {
                if (r.type == 4)
                {
                    PlaceDoors(r);
                    PaintRoom(r);
                }
            }
            foreach (var r in SPDDebug.currentmap.finalRooms)
            {
                if (r.type == 0)
                {
                    PlaceDoors(r);
                    PaintRoom(r);
                }
            }
            PaintDoors();
        }
        public void Fill(int x, int y, int w, int h, int value)
        {
            var width = SPDDebug.currentmap.width;
            var pos = y * width + x;
            for (var i = y; i < y + h + 1; i++, pos += width)
            {
                for (var j = pos; j < pos + w + 1; j++)
                    SPDDebug.currentmap.map[j].value = value;
            }
        }
        public void Fill(SPDRect rect, int value)
        {
            Fill(rect.left, rect.top, rect.Width(), rect.Height(), value);
        }
        public void Fill(SPDRect rect, int m, int value)
        {
            Fill(rect.left + m, rect.top + m, rect.Width() - m * 2, rect.Height() - m * 2, value);
        }
        public void Fill(SPDRect rect, int l, int t, int r, int b, int value)
        {
            Fill(rect.left + 1, rect.top + t, rect.Width() - (l + r), rect.Height() - (t + b), value);
        }
        public void Set(SPDPoint point, int value)
        {
            SPDDebug.currentmap.map.Find(p => p.x == point.x && p.y == point.y).value = value;
        }
        public void Set(int x, int y, int value)
        {
            SPDDebug.currentmap.map.Find(p => p.x == x && p.y == y).value = value;
        }
        public void DrawLine(SPDPoint from, SPDPoint to, int value)
        {
            float x = from.x;
            float y = from.y;
            float dx = to.x - from.x;
            float dy = to.y - from.y;
            var movingbyX = Math.Abs(dx) >= Math.Abs(dy);
            if (movingbyX)
            {
                dy /= Math.Abs(dx);
                dx /= Math.Abs(dx);
            }
            else
            {
                dx /= Math.Abs(dy);
                dy /= Math.Abs(dy);
            }
            Set(Convert.ToInt32(x), Convert.ToInt32(y), value);
            while ((movingbyX && to.x != x) || (!movingbyX && to.y != y))
            {
                x += dx;
                y += dy;
                Set(Convert.ToInt32(x), Convert.ToInt32(y), value);
            }
        }
        public SPDPoint DrawInside(SPDRoom room, SPDPoint from, int n, int value)
        {
            var step = new SPDPoint();
            if (from.x == room.left)
                step.Set(+1, 0);
            else if (from.x == room.right)
                step.Set(-1, 0);
            else if (from.y == room.top)
                step.Set(0, +1);
            else if (from.y == room.bottom)
                step.Set(0, -1);

            var p = new SPDPoint(from).Offset(step);
            for (var i = 0; i < n; i++)
            {
                if (value != -1)
                    Set(p, value);

                p.Offset(step);
            }
            return p;
        }
        public void AddFacility(SPDPoint point, int value)
        {
            SPDDebug.currentmap.facilities.Add(new SPDMapPoint(point.x, point.y, value));
        }
        public void AddItem(SPDPoint point, int value)
        {
            SPDDebug.currentmap.items.Add(new SPDMapPoint(point.x, point.y, value));
        }
        public void AddMonster(SPDPoint point, int value)
        {
            SPDDebug.currentmap.monsters.Add(new SPDMapPoint(point.x, point.y, value));
        }
        public void AddTrap(SPDPoint point, int value)
        {
            SPDDebug.currentmap.traps.Add(new SPDMapPoint(point.x, point.y, value));
        }
        public void AddBlock(SPDPoint point, int value)
        {
            SPDDebug.currentmap.blocks.Add(new SPDMapPoint(point.x, point.y, value));
        }
        public void ReplaceItem(SPDPoint point, int value)
        {
            SPDDebug.currentmap.items.Find(p => p.x == point.x && p.y == point.y).value = value;
        }
        public void AddItemDelayed(Item Item, int? Quantity = null)
        {
            SPDDebug.currentmap.itemsToAdd.Add(new SPDItem(Item, Quantity));
        }
        public void AddMonsterDelayed(Entity entity)
        {
            SPDDebug.currentmap.monstersToAdd.Add(entity);
        }
        public Inv.DistinctList<SPDPoint> AllStandardPoints()
        {
            var Result = new Inv.DistinctList<SPDPoint>();
            foreach (var room in SPDDebug.currentmap.finalRooms)
            {
                if (IsNormalRoom(room))
                     Result.AddRange(room.PointsInside());
            }
            return Result;
        }
        public bool PoSNeeded()
        {
            var posLeftThisSet = 3 - (posDropped - (SPDDebug.currentmap.depth / 5) * 3);
            if (posLeftThisSet <= 0) return false;
            var floorThisSet = (SPDDebug.currentmap.depth % 5);
            return SPDRandom.Int(5 - floorThisSet) < posLeftThisSet;
        }

        private void PlaceDoors(SPDRoom r)
        {
            for (var j = 0; j < r.connected.Keys.Count; j++) 
            {
                var n = r.connected.ElementAt(j).Key;
                var door = r.connected[n];
                if (door == null)
                {
                    var i = r.Intersect(n);
                    var doorSpots = new Inv.DistinctList<SPDPoint>();
                    foreach (var p in i.GetPoints())
                    {
                        if (r.CanConnect(p) && n.CanConnect(p)) doorSpots.Add(p);
                    }
                    door = new SPDPoint(doorSpots[SPDRandom.Int(doorSpots.Count)]);
                    r.connected[n] = door;
                    n.connected[r] = door;
                }
            }
        }
        private void PaintDoors()
        {
            foreach (var r in SPDDebug.currentmap.finalRooms)
            {
                foreach (var n in r.connected.Keys)
                {
                    var d = r.connected[n];
                    if (r.type == 4 && n.type == 4)
                    {
                        SPDDebug.currentmap.map.Find(p => p.x == d.x && p.y == d.y).value = SPDMapPoint.CORRIDOR;
                    } else if (r.type == 3 || n.type == 3)
                    {
                        if (d.y == r.top || d.y == r.bottom) SPDDebug.currentmap.map.Find(p => p.x == d.x && p.y == d.y).value = SPDMapPoint.DOORSECH;
                        if (d.x == r.left || d.x == r.right) SPDDebug.currentmap.map.Find(p => p.x == d.x && p.y == d.y).value = SPDMapPoint.DOORSECL;
                    } else if (r.type == 2 || n.type == 2)
                    {
                        SPDDebug.currentmap.map.Find(p => p.x == d.x && p.y == d.y).value = SPDMapPoint.DOORLOCK;
                    } else if (r.type == 0 || n.type == 0)
                    {
                        SPDDebug.currentmap.map.Find(p => p.x == d.x && p.y == d.y).value = SPDMapPoint.DOORPLAIN;
                    } else SPDDebug.currentmap.map.Find(p => p.x == d.x && p.y == d.y).value = SPDMapPoint.DOORNORM;
                    if ((r.type == 2 && r.flavor == SPDFlavour.Shop) || (n.type == 2 && n.flavor == SPDFlavour.Shop)) SPDDebug.currentmap.map.Find(p => p.x == d.x && p.y == d.y).value = SPDMapPoint.DOORPLAIN;
                    if ((r.type == 3 && r.flavor == SPDFlavour.ChestZoo) || (n.type == 3 && n.flavor == SPDFlavour.ChestZoo)) SPDDebug.currentmap.map.Find(p => p.x == d.x && p.y == d.y).value = SPDMapPoint.DOORSUPER;
                }
            }
        }
        private void PaintRoom(SPDRoom r)
        {
            switch (r.type)
            {
                case 0:
                    switch (r.flavor)
                    {
                        case SPDFlavour.Entrance:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            SPDDebug.currentmap.entrance = r.Random(2);
                            Set(SPDDebug.currentmap.entrance, SPDMapPoint.ENTRANCE);
                            break;
                        case SPDFlavour.Exit:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            SPDDebug.currentmap.exit = r.Random(2);
                            Set(SPDDebug.currentmap.exit, SPDMapPoint.EXIT);
                            break;
                    }
                    break;
                case 1:
                    switch (r.flavor)
                    {
                        case SPDFlavour.Empty:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            break;
                        case SPDFlavour.Aquarium:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            Fill(r, 2, SPDMapPoint.EMPTY);
                            Fill(r, 3, SPDMapPoint.WATER);
                            var aquaN = SPDRandom.IntRange(1, 2);
                            for (var i = 0; i < aquaN; i++)
                            {
                                SPDPoint aquariumPos;
                                do
                                {
                                    aquariumPos = r.Random();
                                } while (SPDDebug.currentmap.map.Find(p => p.x == aquariumPos.x && p.y == aquariumPos.y).value != SPDMapPoint.WATER || SPDDebug.currentmap.monsters.Exists(m => m.x == aquariumPos.x && m.y == aquariumPos.y));
                                AddMonster(aquariumPos, SPDMapPoint.MONSTER);
                            }
                            break;
                        case SPDFlavour.Garden:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            var gardenentrance = r.Entrance();
                            SPDPoint gardenbarrel = null;
                            if (gardenentrance.x == r.left)
                            {
                                gardenbarrel = new SPDPoint(r.right - 1, SPDRandom.Int(2) == 0 ? r.top + 1 : r.bottom - 1);
                            }
                            else if (gardenentrance.x == r.right)
                            {
                                gardenbarrel = new SPDPoint(r.left + 1, SPDRandom.Int(2) == 0 ? r.top + 1 : r.bottom - 1);
                            }
                            else if (gardenentrance.y == r.top)
                            {
                                gardenbarrel = new SPDPoint(SPDRandom.Int(2) == 0 ? r.left + 1 : r.right - 1, r.bottom - 1);
                            }
                            else if (gardenentrance.y == r.bottom)
                            {
                                gardenbarrel = new SPDPoint(SPDRandom.Int(2) == 0 ? r.left + 1 : r.right - 1, r.top + 1);
                            }

                            if (gardenbarrel != null)
                                AddBlock(gardenbarrel, SPDMapPoint.BARREL);

                            var gardenN = SPDRandom.IntRange(2, 3);
                            for (var i = 0; i < gardenN; i++)
                            {
                                SPDPoint pos;
                                do
                                {
                                    pos = r.Random();
                                } while (SPDDebug.currentmap.blocks.Exists(a => a.x == pos.x && a.y == pos.y) || SPDDebug.currentmap.items.Exists(b => b.x == pos.x && b.y == pos.y));
                                AddItem(pos, SPDMapPoint.MAGICFOOD);
                            }
                            break;
                        case SPDFlavour.Zoo:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            foreach (var pos in r.GetPoints())
                            {
                                if (pos.x != r.left && pos.x != r.right && pos.y != r.top && pos.y != r.bottom)
                                {
                                    if (SPDRandom.Int(2) == 0) AddMonster(pos, SPDMapPoint.MONSTER);
                                    if (SPDRandom.Int(8) < 3) AddItem(pos, SPDMapPoint.GOLDHALF);
                                }
                            }
                            break;
                        case SPDFlavour.LeprechaunHall:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            foreach (var pos in r.GetPoints())
                            {
                                if (pos.x != r.left && pos.x != r.right && pos.y != r.top && pos.y != r.bottom)
                                {
                                    if (SPDRandom.Int(3) == 0) AddMonster(pos, SPDMapPoint.LEPRECHAUN);
                                    if (SPDRandom.Int(2) == 0) AddItem(pos, SPDMapPoint.GOLDHALF);
                                }
                            }
                            SPDPoint LepreWizardPos;
                            do
                            {
                                LepreWizardPos = r.Random();
                            } while (SPDDebug.currentmap.monsters.Exists(m => m.x == LepreWizardPos.x && m.y == LepreWizardPos.y));
                            AddMonster(LepreWizardPos, SPDMapPoint.LEPREWIZARD);
                            break;
                        case SPDFlavour.Hive:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.HIVE);
                            foreach (var pos in r.GetPoints())
                            {
                                if (pos.x != r.left && pos.x != r.right && pos.y != r.top && pos.y != r.bottom)
                                {
                                    if (SPDRandom.Int(3) == 0) AddMonster(pos, SPDMapPoint.BEE);
                                }
                            }
                            SPDPoint beeQueenPos;
                            do
                            {
                                beeQueenPos = r.Random();
                            } while (SPDDebug.currentmap.monsters.Exists(m => m.x == beeQueenPos.x && m.y == beeQueenPos.y));
                            if (SPDDebug.currentmap.depth > 10) AddMonster(beeQueenPos, SPDMapPoint.BEEQUEEN);
                            break;
                        case SPDFlavour.GrassyGrave:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.GRASS);
                            var gravesW = r.Width() - 2;
                            var gravesH = r.Height() - 2;
                            var gravesN = Math.Max(gravesW, gravesH) / 2;
                            var gravesIndex = SPDRandom.Int(gravesN);
                            var gravesShift = SPDRandom.Int(2);
                            for (var i = 0; i < gravesN; i++)
                            {
                                var pos = gravesW > gravesH ?
                                    new SPDPoint(r.left + 1 + gravesShift + i * 2, r.top + 2 + SPDRandom.Int(gravesH - 2)) :
                                    new SPDPoint(r.left + 2 + SPDRandom.Int(gravesW - 2), r.top + 1 + gravesShift + i * 2);
                                AddFacility(pos, SPDMapPoint.GRAVE);
                            }
                            break;
                        case SPDFlavour.Striped:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            if (r.Width() < 10 || r.Height() < 10)
                            {
                                Fill(r, 1, SPDMapPoint.WOODFLOOR);
                                if (r.Width() > r.Height() || (r.Width() == r.Height() && SPDRandom.Int(2) == 0)){
                                    for (var i = r.left + 2; i < r.right; i += 2) { Fill(i, r.top + 1, 0, r.Height() - 3, SPDMapPoint.EMPTY); }
                                } else for (var i = r.top + 2; i < r.bottom; i += 2) { Fill(r.left + 1, i, r.Width() - 3, 0, SPDMapPoint.EMPTY); }
                            } else
                            {
                                var layers = Math.Min(r.Width(), r.Height()) - 1 / 2;
                                for (var i = 1; i <= layers; i++) { Fill(r, i, (i % 2 == 1) ? SPDMapPoint.WOODFLOOR : SPDMapPoint.EMPTY); }
                            }
                            break;
                        case SPDFlavour.Study:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.WOODFLOOR);
                            var studyCenter = r.Center();
                            Set(studyCenter, SPDMapPoint.MARBLE);
                            switch (SPDRandom.Int(4))
                            {
                                case 0: AddItem(studyCenter, SPDMapPoint.RANDPOTION); break;
                                case 1: AddItem(studyCenter, SPDMapPoint.RANDSCROLL); break;
                                case 2: AddItem(studyCenter, SPDMapPoint.RANDWAND); break;
                                case 3: AddItem(studyCenter, SPDMapPoint.RANDTOOL); break;
                            }
                            break;
                        case SPDFlavour.SuspiciousChest:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            var susChestCenter = r.Center();
                            Set(susChestCenter, SPDMapPoint.MARBLE);
                            if (SPDRandom.Int(3) == 0) AddMonster(susChestCenter, SPDMapPoint.SMALMIMIC);
                            else AddItem(susChestCenter, SPDMapPoint.ZOOCHEST);
                            break;
                        case SPDFlavour.HolyShrine:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            AddFacility(r.Center(), SPDMapPoint.HOLYSHRINE);
                            break;
                        case SPDFlavour.DarkShrine:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            AddFacility(r.Center(), SPDMapPoint.DARKSHRINE);
                            break;
                        case SPDFlavour.SacredGrove:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            AddFacility(r.Center(), SPDMapPoint.SACREDGROVE);
                            break;
                        default:
                            Fill(r, SPDMapPoint.WALL);
                            break;
                    }
                    break;
                case 2:
                    switch (r.flavor)
                    {
                        case SPDFlavour.Altar:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            var altarcenter = r.Center();
                            //var altardoor = Entrance();
                            AddFacility(altarcenter, SPDMapPoint.ALTAR);
                            break;
                        case SPDFlavour.Armory:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            var armoryentrance = r.Entrance();
                            SPDPoint armorystatue = null;
                            if (armoryentrance.x == r.left)
                            {
                                armorystatue = new SPDPoint(r.right - 1, SPDRandom.Int(2) == 0 ? r.top + 1 : r.bottom - 1);
                            } else if (armoryentrance.x == r.right)
                            {
                                armorystatue = new SPDPoint(r.left + 1, SPDRandom.Int(2) == 0 ? r.top + 1 : r.bottom - 1);
                            } else if (armoryentrance.y == r.top)
                            {
                                armorystatue = new SPDPoint(SPDRandom.Int(2) == 0 ? r.left + 1 : r.right - 1, r.bottom - 1);
                            } else if (armoryentrance.y == r.bottom)
                            {
                                armorystatue = new SPDPoint(SPDRandom.Int(2) == 0 ? r.left + 1 : r.right - 1, r.top + 1);
                            }
                            if (armorystatue != null) AddBlock(armorystatue, SPDMapPoint.STATUE);
                            var armoryN = SPDRandom.IntRange(2, 3);
                            for (var i = 0; i < armoryN; i++)
                            {
                                SPDPoint pos;
                                do
                                {
                                    pos = r.Random();
                                } while (SPDDebug.currentmap.blocks.Exists(a => a.x == pos.x && a.y == pos.y) || SPDDebug.currentmap.items.Exists(b => b.x == pos.x && b.y == pos.y));
                                AddItem(pos, SPDMapPoint.RANDARMOUR);
                            }
                            break;
                        case SPDFlavour.Crypt:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            var cryptcenter = r.Center();
                            var cryptcx = cryptcenter.x;
                            var cryptcy = cryptcenter.y;
                            var cryptentrance = r.Entrance();
                            if (cryptentrance.x == r.left)
                            {
                                AddBlock(new SPDPoint(r.right - 1, r.top + 1), SPDMapPoint.STATUE);
                                AddBlock(new SPDPoint(r.right - 1, r.bottom - 1), SPDMapPoint.STATUE);
                                cryptcx = r.right - 2;
                            } else if (cryptentrance.x == r.right)
                            {
                                AddBlock(new SPDPoint(r.left + 1, r.top + 1), SPDMapPoint.STATUE);
                                AddBlock(new SPDPoint(r.left + 1, r.bottom - 1), SPDMapPoint.STATUE);
                            }
                            else if (cryptentrance.y == r.top)
                            {
                                AddBlock(new SPDPoint(r.left + 1, r.bottom - 1), SPDMapPoint.STATUE);
                                AddBlock(new SPDPoint(r.right - 1, r.bottom - 1), SPDMapPoint.STATUE);
                            }
                            else if (cryptentrance.y == r.bottom)
                            {
                                AddBlock(new SPDPoint(r.left + 1, r.top + 1), SPDMapPoint.STATUE);
                                AddBlock(new SPDPoint(r.right - 1, r.top + 1), SPDMapPoint.STATUE);
                            }
                            AddItem(new SPDPoint(cryptcx,cryptcy), SPDMapPoint.GOODARMOUR);
                            break;
                        case SPDFlavour.Laboratory:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            var labentrance = r.Entrance();
                            SPDPoint labfountain = null;
                            if (labentrance.x == r.left)
                            {
                                labfountain = new SPDPoint(r.right - 1, SPDRandom.Int(2) == 0 ? r.top + 1 : r.bottom - 1);
                            }
                            else if (labentrance.x == r.right)
                            {
                                labfountain = new SPDPoint(r.left + 1, SPDRandom.Int(2) == 0 ? r.top + 1 : r.bottom - 1);
                            }
                            else if (labentrance.y == r.top)
                            {
                                labfountain = new SPDPoint(SPDRandom.Int(2) == 0 ? r.left + 1 : r.right - 1, r.bottom - 1);
                            }
                            else if (labentrance.y == r.bottom)
                            {
                                labfountain = new SPDPoint(SPDRandom.Int(2) == 0 ? r.left + 1 : r.right - 1, r.top + 1);
                            }
                            AddFacility(labfountain, SPDMapPoint.FOUNTAIN);
                            var labN = SPDRandom.NormalIntRange(2, 3);
                            for (var i = 0; i < labN; i++)
                            {
                                SPDPoint pos;
                                do
                                {
                                    pos = r.Random();
                                } while (SPDDebug.currentmap.facilities.Exists(a => a.x == pos.x && a.y == pos.y) || SPDDebug.currentmap.items.Exists(b => b.x == pos.x && b.y == pos.y));
                                AddItem(pos, SPDMapPoint.RANDPOTION);
                            }
                            int labBookN = SPDRandom.NormalIntRange(0, 2);
                            for (int i = 0; i < labBookN; i++)
                            {
                                SPDPoint pos;
                                do
                                {
                                    pos = r.Random();
                                } while (SPDDebug.currentmap.facilities.Exists(a => a.x == pos.x && a.y == pos.y) || SPDDebug.currentmap.items.Exists(b => b.x == pos.x && b.y == pos.y));
                                AddItem(pos, SPDMapPoint.BOOK);
                            }
                            break;
                        case SPDFlavour.Library:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.WOODFLOOR);
                            Fill(r, 2, SPDMapPoint.EMPTY);
                            DrawInside(r, r.Entrance(), 1, SPDMapPoint.EMPTY);
                            var libraryN = SPDRandom.NormalIntRange(2, 3);
                            for (var i = 0; i < libraryN; i++)
                            {
                                SPDPoint pos;
                                do
                                {
                                    pos = r.Random();
                                } while (SPDDebug.currentmap.map.Find(a => a.x == pos.x || a.y == pos.y).value == SPDMapPoint.WOODFLOOR || SPDDebug.currentmap.items.Exists(b => b.x == pos.x && b.y == pos.y));
                                if (i == 0)
                                {
                                    if (SPDRandom.Int(2) == 0) AddItem(pos, SPDMapPoint.SOID);
                                    else AddItem(pos, SPDMapPoint.SORC);
                                }
                                else AddItem(pos, SPDMapPoint.RANDSCROLL);
                            }
                            var libraryBookN = SPDRandom.NormalIntRange(1, 2);
                            for (var i = 0; i < libraryBookN; i++)
                            {
                                SPDPoint pos;
                                do
                                {
                                    pos = r.Random();
                                } while (SPDDebug.currentmap.map.Find(a => a.x == pos.x || a.y == pos.y).value == SPDMapPoint.WOODFLOOR || SPDDebug.currentmap.items.Exists(b => b.x == pos.x && b.y == pos.y));
                                AddItem(pos, SPDMapPoint.BOOK);
                            }
                            break;
                        case SPDFlavour.Pit:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            var pitEntrance = r.Entrance();
                            SPDPoint pitWell = null;
                            if (pitEntrance.x == r.left)
                            {
                                pitWell = new SPDPoint(r.right - 1, SPDRandom.Int(2) == 0 ? r.top + 1 : r.bottom - 1);
                            }
                            else if (pitEntrance.x == r.right)
                            {
                                pitWell = new SPDPoint(r.left + 1, SPDRandom.Int(2) == 0 ? r.top + 1 : r.bottom - 1);
                            }
                            else if (pitEntrance.y == r.top)
                            {
                                pitWell = new SPDPoint(SPDRandom.Int(2) == 0 ? r.left + 1 : r.right - 1, r.bottom - 1);
                            }
                            else if (pitEntrance.y == r.bottom)
                            {
                                pitWell = new SPDPoint(SPDRandom.Int(2) == 0 ? r.left + 1 : r.right - 1, r.top + 1);
                            }
                            AddFacility(pitWell, SPDMapPoint.BED);
                            var pitRemains = r.Random();
                            pitRemains = r.Random();
                            AddItem(pitRemains, SPDMapPoint.PITCHEST);
                            break;
                        case SPDFlavour.Pool:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.WATER);
                            SPDPoint poolDoor = r.Entrance();
                            int poolX = -1;
                            int poolY = -1;
                            if (poolDoor.x == r.left)
                            {
                                poolX = r.right - 1; poolY = r.top + r.Height() / 2;
                                Fill(r.left + 1, r.top + 1, 0, r.Height() - 3, SPDMapPoint.WOODFLOOR);
                            }
                            else if (poolDoor.x == r.right)
                            {
                                poolX = r.left + 1; poolY = r.top + r.Height() / 2;
                                Fill(r.right - 1, r.top + 1, 0, r.Height() - 3, SPDMapPoint.WOODFLOOR);
                            }
                            else if (poolDoor.y == r.top)
                            {
                                poolX = r.left + r.Width() / 2; poolY = r.bottom - 1;
                                Fill(r.left + 1, r.top + 1, r.Width() - 3, 0, SPDMapPoint.WOODFLOOR);
                            }
                            else if (poolDoor.y == r.bottom)
                            {
                                poolX = r.left + r.Width() / 2; poolY = r.top + 1;
                                Fill(r.left + 1, r.bottom - 1, r.Width() - 3, 0, SPDMapPoint.WOODFLOOR);
                            }
                            var poolPos = new SPDPoint(poolX, poolY);
                            Set(poolPos, SPDMapPoint.WOODFLOOR);
                            AddItem(poolPos, SPDMapPoint.POOLPRIZE);
                            break;
                        case SPDFlavour.Shop:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.WOODFLOOR);
                            SPDPoint shopPos = r.Center();
                            AddFacility(shopPos, SPDMapPoint.PDSHOP);
                            break;
                        case SPDFlavour.Statue:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            SPDPoint statueCenter = r.Center();
                            int statueCX = statueCenter.x;
                            int statueCY = statueCenter.y;
                            SPDPoint statueDoor = r.Entrance();
                            if (statueDoor.x == r.left)
                            {
                                Fill(r.right - 1, r.top + 1, 0, r.Height() - 3, SPDMapPoint.TERRSTATUE);
                                statueCX = r.right - 2;
                            } else if (statueDoor.x == r.right)
                            {
                                Fill(r.left + 1, r.top + 1, 0, r.Height() - 3, SPDMapPoint.TERRSTATUE);
                                statueCX = r.left + 2;
                            } else if (statueDoor.y == r.top)
                            {
                                Fill(r.left + 1, r.bottom - 1, r.Width() - 3, 0, SPDMapPoint.TERRSTATUE);
                                statueCY = r.bottom - 2;
                            } else if (statueDoor.y == r.bottom)
                            {
                                Fill(r.left + 1, r.top + 1, r.Width() - 3, 0, SPDMapPoint.TERRSTATUE);
                                statueCY = r.top + 2;
                            }
                            AddItem(new SPDPoint(statueCX, statueCY), SPDMapPoint.GOODWEAPON);
                            break;
                        case SPDFlavour.Storage:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.WOODFLOOR);
                            var storageN = SPDRandom.IntRange(3, 5);
                            for (var i = 0; i < storageN; i++)
                            {
                                var storagePos = r.Random();
                                switch (SPDRandom.Int(5))
                                {
                                    case 0: AddItem(storagePos, SPDMapPoint.RANDPOTION); break;
                                    case 1: AddItem(storagePos, SPDMapPoint.RANDSCROLL); break;
                                    case 2: AddItem(storagePos, SPDMapPoint.GOLD); break;
                                    case 3: AddItem(storagePos, SPDMapPoint.GOLD); break;
                                    case 4: AddItem(storagePos, SPDMapPoint.BOOK); break;
                                    default: AddItem(storagePos, SPDMapPoint.GOLD); break;
                                }
                            }
                            break;
                        case SPDFlavour.Traps:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.TERRTRAPSP);
                            var trapsDoor = r.Entrance();
                            var trapsX = -1;
                            var trapsY = -1;
                            if (trapsDoor.x == r.left)
                            {
                                trapsX = r.right - 1; trapsY = r.top + r.Height() / 2;
                                Fill(trapsX, r.top + 1, 0, r.Height() - 3, SPDMapPoint.WOODFLOOR);
                            }
                            else if (trapsDoor.x == r.right)
                            {
                                trapsX = r.left + 1; trapsY = r.top + r.Height() / 2;
                                Fill(trapsX, r.top + 1, 0, r.Height() - 3, SPDMapPoint.WOODFLOOR);
                            }
                            else if (trapsDoor.y == r.top)
                            {
                                trapsX = r.left + r.Width() / 2; trapsY = r.bottom - 1;
                                Fill(r.left + 1, trapsY, r.Width() - 3, 0, SPDMapPoint.WOODFLOOR);
                            }
                            else if (trapsDoor.y == r.bottom)
                            {
                                trapsX = r.left + r.Width() / 2; trapsY = r.top + 1;
                                Fill(r.left + 1, trapsY, r.Width() - 3, 0, SPDMapPoint.WOODFLOOR);
                            }
                            var trapsPos = new SPDPoint(trapsX, trapsY);
                            Set(trapsPos, SPDMapPoint.WOODFLOOR);
                            AddItem(trapsPos, SPDMapPoint.POOLPRIZE);
                            break;
                        case SPDFlavour.Treasury:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            AddBlock(r.Center(), SPDMapPoint.STATUE);
                            var treasuryN = SPDRandom.IntRange(1, 3);
                            for (var i = 0; i < treasuryN; i++)
                            {
                                SPDPoint pos;
                                do
                                {
                                    pos = r.Random();
                                } while (pos.x == r.Center().x && pos.y == r.Center().y);
                                AddItem(pos, SPDMapPoint.GOLD);
                            }
                            for (var i = 0; i < 3; i++)
                            {
                                SPDPoint pos;
                                do
                                {
                                    pos = r.Random();
                                } while (pos.x == r.Center().x && pos.y == r.Center().y);
                                AddItem(pos, SPDMapPoint.GOLDHALF);
                            }
                            break;
                        case SPDFlavour.Vault:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.WOODFLOOR);
                            var vaultPos1 = r.Random();
                            SPDPoint vaultPos2;
                            do { vaultPos2 = r.Random(); } while (vaultPos2.x == vaultPos1.x && vaultPos2.y == vaultPos1.y);
                            switch (SPDRandom.Int(3))
                            {
                                case 0: AddItem(vaultPos1, SPDMapPoint.AMULET); break;
                                case 1: AddItem(vaultPos1, SPDMapPoint.RANDRING); break;
                                case 2: AddItem(vaultPos1, SPDMapPoint.BOOK); break;
                            }
                            switch (SPDRandom.Int(3))
                            {
                                case 0: AddItem(vaultPos2, SPDMapPoint.AMULET); break;
                                case 1: AddItem(vaultPos2, SPDMapPoint.RANDRING); break;
                                case 2: AddItem(vaultPos2, SPDMapPoint.BOOK); break;
                            }
                            break;
                        default:
                            Fill(r, SPDMapPoint.WALL);
                            break;
                    }
                    break;
                case 3:
                    switch (r.flavor)
                    {
                        case SPDFlavour.Artillery:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            var artilleryStatue = r.Center();
                            AddBlock(artilleryStatue, SPDMapPoint.STATUE);
                            for (var i = 0; i < 3; i++)
                            {
                                SPDPoint itemPos;
                                do
                                {
                                    itemPos = r.Random();
                                } while (itemPos.x == artilleryStatue.x && itemPos.y == artilleryStatue.y);
                                if (i == 0) AddItem(itemPos, SPDMapPoint.THROWNWEP);
                                else AddItem(itemPos, SPDMapPoint.THROWNFIRE);
                            }
                            break;
                        case SPDFlavour.ChestZoo:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            foreach (var chestChasmPos in r.GetPoints())
                            {
                                if (chestChasmPos.x != r.left && chestChasmPos.x != r.right && chestChasmPos.y != r.top && chestChasmPos.y != r.bottom)
                                AddMonster(chestChasmPos, SPDMapPoint.MONSTER);
                            }
                            var zooChestPos1 = r.Random();
                            SPDPoint zooChestPos2;
                            do { zooChestPos2 = r.Random(); } while (zooChestPos2.x == zooChestPos1.x && zooChestPos2.y == zooChestPos1.y);
                            AddItem(zooChestPos1, SPDMapPoint.ZOOCHEST);
                            AddItem(zooChestPos2, SPDMapPoint.ZOOCHEST);
                            break;
                        case SPDFlavour.Garden:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            var sGardenDoor = r.Entrance();
                            if (sGardenDoor.x == r.left)
                            {
                                Fill(r.right - 1, r.top + 1, 0, r.Height() - 3, SPDMapPoint.TERRBARREL);
                            }
                            else if (sGardenDoor.x == r.right)
                            {
                                Fill(r.left + 1, r.top + 1, 0, r.Height() - 3, SPDMapPoint.TERRBARREL);
                            }
                            else if (sGardenDoor.y == r.top)
                            {
                                Fill(r.left + 1, r.bottom - 1, r.Width() - 3, 0, SPDMapPoint.TERRBARREL);
                            }
                            else if (sGardenDoor.y == r.bottom)
                            {
                                Fill(r.left + 1, r.top + 1, r.Width() - 3, 0, SPDMapPoint.TERRBARREL);
                            }
                            for (int i = 0; i < 4; i++)
                            {
                                SPDPoint pos;
                                do
                                {
                                    pos = r.Random();
                                } while (SPDDebug.currentmap.blocks.Exists(a => a.x == pos.x && a.y == pos.y));
                                if (SPDRandom.Int(4) == 0) AddItem(pos, SPDMapPoint.POH);
                                else AddItem(pos, SPDMapPoint.MAGICFOOD);
                            }
                            break;
                        case SPDFlavour.Hoard:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            var hoardDice1 = SPDRandom.IntRange(1, 2);
                            for (var i = 0; i < hoardDice1; i++)
                                AddItem(r.Random(), SPDMapPoint.GOLD);

                            var hoardDice2 = SPDRandom.IntRange(0, 2);
                            for (var i = 0; i < hoardDice2; i++)
                                AddItem(r.Random(), SPDMapPoint.GEMREAL);

                            foreach (var point in r.GetPoints())
                            {
                                if (point.x != r.left && point.x != r.right && point.y != r.top && point.y != r.bottom) 
                                    AddTrap(point, SPDMapPoint.TRAP);
                            }
                            break;
                        case SPDFlavour.Honeypot:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.EMPTY);
                            for (var i = 0; i < 5; i++)
                            {
                                var pos = r.Random();
                                if (SPDRandom.Int(2) == 0) AddItem(pos, SPDMapPoint.THROWNWEP);
                                else AddItem(pos, SPDMapPoint.THROWNFIRE);
                            }
                            break;
                        case SPDFlavour.Laboratory:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.WOODFLOOR);
                            AddFacility(r.Center(), SPDMapPoint.FOUNTAIN);
                            var secLabN = SPDRandom.IntRange(2, 3);
                            for (var i = 0; i < secLabN; i++)
                            {
                                SPDPoint pos;
                                do
                                {
                                    pos = r.Random();
                                } while (pos.x == r.Center().x && pos.y == r.Center().y);
                                AddItem(pos, SPDMapPoint.RANDPOTION);
                            }
                            break;
                        case SPDFlavour.Larder:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.GRASS);
                            var secLarderN = SPDRandom.IntRange(2, 3);
                            for (var i = 0; i < secLarderN; i++)
                                AddItem(r.Random(), SPDMapPoint.MAGICFOOD);
                            break;
                        case SPDFlavour.Library:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.WOODFLOOR);
                            AddFacility(r.Center(), SPDMapPoint.WORKBENCH);
                            var secLibraryN = SPDRandom.IntRange(2, 3);
                            for (var i = 0; i < secLibraryN; i++)
                                AddItem(r.Random(), SPDMapPoint.RANDSCROLL);
                            var secLibBookN = SPDRandom.NormalIntRange(1, 2);
                            for (var i = 0; i < secLibBookN; i++)
                                AddItem(r.Random(), SPDMapPoint.BOOK);
                            break;
                        case SPDFlavour.Graves:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.DIRT);
                            var graveSpots = new Inv.DistinctList<SPDPoint>();
                            var secGravesN = SPDRandom.IntRange(5, 8);
                            for (var i = 0; i < secGravesN; i++)
                            {
                                SPDPoint pos;
                                do
                                {
                                    pos = r.Random();
                                } while (graveSpots.Exists(p => p.x == pos.x && p.y == pos.y));
                                graveSpots.Add(pos);
                                AddFacility(pos, SPDMapPoint.GRAVE);
                            }
                            break;
                        case SPDFlavour.Summoning:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.TERRSUMMTRAP);
                            AddItem(r.Center(), SPDMapPoint.GOLD);
                            break;
                        case SPDFlavour.SecretShop:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.WOODFLOOR);
                            AddFacility(r.Center(), SPDMapPoint.SHOPPOOR);
                            break;
                        case SPDFlavour.Farm:
                            Fill(r, SPDMapPoint.WALL);
                            Fill(r, 1, SPDMapPoint.GRASS);
                            foreach (var pos in r.GetPoints())
                            {
                                if (pos.x != r.left && pos.x != r.right && pos.y != r.top && pos.y != r.bottom)
                                {
                                    if (SPDRandom.Int(5) == 0) AddMonster(pos, SPDMapPoint.YEOMAN);
                                    else if (SPDRandom.Int(8) == 0) AddMonster(pos, SPDMapPoint.YEOMANWARDER);
                                    if (SPDRandom.Int(8) == 0) AddItem(pos, SPDMapPoint.MAGICFOOD);
                                }
                            }
                            SPDPoint yeomanChiefPos;
                            do
                            {
                                yeomanChiefPos = r.Random();
                            } while (SPDDebug.currentmap.monsters.Exists(m => m.x == yeomanChiefPos.x && m.y == yeomanChiefPos.y));
                            AddMonster(yeomanChiefPos, SPDMapPoint.YEOMANCHIEF);
                            break;
                        default:
                            Fill(r, SPDMapPoint.WALL);
                            break;
                    }
                    break;
                case 4:
                    var connfloor = SPDMapPoint.CORRIDOR;
                    //Painter.Fill(this, MapPoint.WALL);
                    var connspace = r.GetConnectionSpace();

                    foreach (var door in r.connected.Values)
                    {
                        var connstart = new SPDPoint(door);
                        if (connstart.x == r.left) connstart.x++;
                        else if (connstart.y == r.top) connstart.y++;
                        else if (connstart.x == r.right) connstart.x--;
                        else if (connstart.y == r.bottom) connstart.y--;

                        int connrightShift;
                        int conndownShift;
                        if (connstart.x < connspace.left) connrightShift = connspace.left - connstart.x;
                        else if (connstart.x > connspace.right) connrightShift = connspace.right - connstart.x;
                        else connrightShift = 0;
                        if (connstart.y < connspace.top) conndownShift = connspace.top - connstart.y;
                        else if (connstart.y > connspace.bottom) conndownShift = connspace.bottom - connstart.y;
                        else conndownShift = 0;

                        SPDPoint connmid;
                        SPDPoint connend;
                        if (door.x == r.left || door.x == r.right)
                        {
                            connmid = new SPDPoint(connstart.x + connrightShift, connstart.y);
                            connend = new SPDPoint(connmid.x, connmid.y + conndownShift);
                        }
                        else
                        {
                            connmid = new SPDPoint(connstart.x, connstart.y + conndownShift);
                            connend = new SPDPoint(connmid.x + connrightShift, connmid.y);
                        }

                        DrawLine(connstart, connmid, connfloor);
                        DrawLine(connmid, connend, connfloor);
                    }
                    break;
                default:
                    Fill(r, SPDMapPoint.WALL);
                    Fill(r, 1, SPDMapPoint.EMPTY);
                    break;
            }

            if (r.type == 2) AddItemDelayed(SPDDebug.codex.Items.lock_pick, Quantity: 1);
        }
        private bool SoUNeeded()
        {
            var souLeftThisSet = 3 - (souDropped - (SPDDebug.currentmap.depth / 5) * 3);
            if (souLeftThisSet <= 0) return false;
            var floorThisSet = (SPDDebug.currentmap.depth % 5);
            return SPDRandom.Int(5 - floorThisSet) < souLeftThisSet;
        }
        private bool IsNormalRoom(SPDRoom room)
        {
            if (room.type == 0 && room.flavor == SPDFlavour.Exit) return true;
            if (room.type == 1)
            {
                if (room.flavor == SPDFlavour.DarkShrine) return false;
                if (room.flavor == SPDFlavour.HolyShrine) return false;
                if (room.flavor == SPDFlavour.SacredGrove) return false;
                return true;
            }
            return false;
        }

        private int posDropped = 0;
        private int souDropped = 0;
    }
}