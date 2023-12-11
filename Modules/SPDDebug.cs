using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathos
{
    internal static class SPDDebug
    {
        /// <summary>
        /// Global status and debug stuff.
        /// </summary>
        public static Generator generator;
        public static Adventure adventure;
        public static Codex codex;
        public static Site mainSite;
        public static Region startRegion;
        public static Zone startZone;
        public static Inv.DistinctList<SPDMap> maps;
        public static SPDMap currentmap;
        public static SPDMap previousmap;

        public const string level_depth = "level//depth";

        public static string CurrentLevelName() => generator.EscapedModuleTerm(level_depth) + " " + currentmap.depth;
        
        private static string FilePath = "";
        private static string DebugContent = "";

        public static void Write()
        {
            System.IO.File.WriteAllText(@FilePath, DebugContent);
            DebugContent = "";
        }
        public static void WriteMapList()
        {
            FilePath = "C:\\LAN\\99.txt";
            DebugContent = "";
            foreach (var map in maps)
            {
                DebugContent += map.depth;
                DebugContent += " ";
                DebugContent += map.feeling;
                DebugContent += " ";
                DebugContent += map.initRooms.Count;
                DebugContent += "\r\n";
            }
            Write();
        }
        public static void WriteRooms()
        {
            FilePath = "C:\\LAN\\" + currentmap.depth + ".txt";
            DebugContent = "";
            if (currentmap.finalRooms.Count > 0)
            {
                DebugContent += "rooms=" + currentmap.finalRooms.Count + "\r\n";
                DebugContent += "allrooms\r\n";
                foreach (var room in currentmap.finalRooms)
                {
                    DebugContent += room.type + " ";
                    DebugContent += room.flavor + " ";
                    DebugContent += room.Height() + "H ";
                    DebugContent += room.Width() + "W ";
                    DebugContent += "LTRB=";
                    DebugContent += room.left + " ";
                    DebugContent += room.top + " ";
                    DebugContent += room.right + " ";
                    DebugContent += room.bottom + " ";
                    DebugContent += "points= ";
                    DebugContent += "(" + room.left + "," + (-room.top) + ") ";
                    DebugContent += "(" + room.left + "," + (-room.bottom) + ") ";
                    DebugContent += "(" + room.right + "," + (-room.top) + ") ";
                    DebugContent += "(" + room.right + "," + (-room.bottom) + ") ";
                    DebugContent += "connected ";
                    foreach (KeyValuePair<SPDRoom, SPDPoint> connedroom in room.connected)
                    {
                        DebugContent += connedroom.Key.flavor + " ";
                    }
                    DebugContent += "\r\n";
                }
                DebugContent += "\r\n";
            }
            if (currentmap.multiConnections.Count > 0)
            {
                DebugContent += "multiconn\r\n";
                foreach (var room in currentmap.multiConnections)
                {
                    DebugContent += room.type;
                    DebugContent += room.flavor;
                    DebugContent += "\r\n";
                }
                DebugContent += "\r\n";
            }
            if (currentmap.singleConnections.Count > 0)
            {
                DebugContent += "singleconn\r\n";
                foreach (var room in currentmap.singleConnections)
                {
                    DebugContent += room.type;
                    DebugContent += room.flavor;
                    DebugContent += "\r\n";
                }
                DebugContent += "\r\n";
            }
            if (currentmap.mainPathRooms.Count > 0)
            {
                DebugContent += "mainpath\r\n";
                foreach (var room in currentmap.mainPathRooms)
                {
                    DebugContent += room.type;
                    DebugContent += room.flavor;
                    DebugContent += "\r\n";
                }
                DebugContent += "\r\n";
            }
            if (currentmap.loop.Count > 0)
            {
                DebugContent += "loop\r\n";
                foreach (var room in currentmap.loop)
                {
                    DebugContent += room.type + " ";
                    DebugContent += room.flavor + " ";
                    DebugContent += room.Height() + "H ";
                    DebugContent += room.Width() + "W ";
                    DebugContent += "LTRB=";
                    DebugContent += room.left + " ";
                    DebugContent += room.top + " ";
                    DebugContent += room.right + " ";
                    DebugContent += room.bottom + " ";
                    DebugContent += "connected ";
                    foreach (var connedroom in room.connected)
                    {
                        DebugContent += connedroom.Key.flavor + " ";
                    }
                    DebugContent += "\r\n";
                }
                DebugContent += "\r\n";
            }
            WriteMap();
            Write();
        }
        public static void WriteMap()
        {
            DebugContent += "map\r\n";
            for (var y = 0; y < currentmap.height; y++)
            {
                for (var x = 0; x < currentmap.width; x++)
                {
                    DebugContent += currentmap.map.Find(p => p.x == x && p.y == y).value;
                }
                DebugContent += "\r\n";
            }
        }
    }
}
