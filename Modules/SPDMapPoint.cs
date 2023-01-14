using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathos
{
    class SPDMapPoint
    {
        public int x;
        public int y;
        public int value;

        public SPDMapPoint()
        {
            new SPDMapPoint(0, 0, 0);
        }

        public SPDMapPoint(int x, int y, int value)
        {
            this.x = x;
            this.y = y;
            this.value = value;
        }
        //Terrains
        public static readonly int VOID     = 0;
        public static readonly int WALL     = 1;
        public static readonly int EMPTY    = 2;
        public static readonly int DOORNORM = 22;
        public static readonly int ENTRANCE = 4;
        public static readonly int EXIT     = 5;
        public static readonly int WATER    = 6;
        public static readonly int WOODFLOOR= 7;
        public static readonly int CORRIDOR = 8;
        public static readonly int LAVA     = 9;
        public static readonly int TERRSTATUE = 10;
        public static readonly int TERRBARREL = 11;
        public static readonly int TERRTRAP   = 12;
        public static readonly int TERRTRAPSP = 13;
        public static readonly int GRASS    = 14;
        public static readonly int DIRT     = 15;
        public static readonly int TERRSUMMTRAP = 16;
        public static readonly int HIVE     = 17;
        public static readonly int JADEWALL = 18;
        public static readonly int MARBLE   = 19;
        public static readonly int DOORLOCK = 20;
        public static readonly int DOORSECH = 21;
        public static readonly int DOORPLAIN= 3;
        public static readonly int DOORSECL = 23;
        public static readonly int DOORSUPER= 24;
        //Blocks
        public static readonly int STATUE   = 0;
        public static readonly int BARREL   = 1;
        //Items
        public static readonly int RANDARMOUR   = 0;
        public static readonly int GOODARMOUR   = 1;
        public static readonly int MAGICFOOD    = 2;
        public static readonly int RANDPOTION   = 3;
        public static readonly int RANDSCROLL   = 4;
        public static readonly int SOID         = 5; //Scroll of identify
        public static readonly int SOU          = 6; //Upgrade(Enchantment)
        public static readonly int POS          = 7; //Potion of strength(gain ability)
        public static readonly int SORC         = 8; //Remove curse
        public static readonly int PITCHEST     = 9;
        public static readonly int RANDRING     = 10;
        public static readonly int POOLPRIZE    = 11;
        public static readonly int GOODWEAPON   = 12;
        public static readonly int GOLD         = 13;
        public static readonly int GOLDHALF     = 14;
        public static readonly int AMULET       = 15;
        public static readonly int BOOK         = 16;
        //public static readonly int ARTIFACT     = 17;
        public static readonly int THROWNWEP    = 18;
        public static readonly int THROWNFIRE   = 19;
        public static readonly int ZOOCHEST     = 20;
        public static readonly int POH          = 21;//Potion of healing
        public static readonly int GEMREAL      = 22;
        public static readonly int RANDWAND     = 23;
        public static readonly int RANDTOOL     = 24;
        //Features
        public static readonly int FOUNTAIN     = 0;
        public static readonly int ALTAR        = 1;
        public static readonly int BED          = 2;
        public static readonly int PDSHOP       = 3;
        public static readonly int SACREDGROVE        = 4;
        public static readonly int HOLYSHRINE   = 5;
        public static readonly int DARKSHRINE   = 6;
        public static readonly int WORKBENCH    = 7;
        public static readonly int GRAVE        = 8;
        public static readonly int SHOPPOOR     = 9;
        //Monsters
        public static readonly int MONSTER      = 0;
        public static readonly int LEPRECHAUN   = 1;
        public static readonly int LEPREWIZARD  = 2;
        public static readonly int YEOMAN       = 3;
        public static readonly int YEOMANWARDER = 4;
        public static readonly int YEOMANCHIEF  = 5;
        public static readonly int BEE          = 6;
        public static readonly int BEEQUEEN     = 7;
        public static readonly int SMALMIMIC    = 8;
        //Traps
        public static readonly int TRAP         = 0;
        public static readonly int TRAPSP       = 1;
        public static readonly int TRAPSUMMON   = 2;
    }
}
