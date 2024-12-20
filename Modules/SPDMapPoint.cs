using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathos
{
  class SPDMapPoint
  {
    public SPDMapPoint(int x, int y, int value)
    {
      this.x = x;
      this.y = y;
      this.value = value;
    }

    public int x;
    public int y;
    public int value;

    //Terrains
    public const int VOID = 0;
    public const int WALL = 1;
    public const int EMPTY = 2;
    public const int DOORNORM = 22;
    public const int ENTRANCE = 4;
    public const int EXIT = 5;
    public const int WATER = 6;
    public const int WOODFLOOR = 7;
    public const int CORRIDOR = 8;
    public const int LAVA = 9;
    public const int TERRSTATUE = 10;
    public const int TERRBARREL = 11;
    public const int TERRTRAP = 12;
    public const int TERRTRAPSP = 13;
    public const int GRASS = 14;
    public const int DIRT = 15;
    public const int TERRSUMMTRAP = 16;
    public const int HIVE = 17;
    public const int JADEWALL = 18;
    public const int MARBLE = 19;
    public const int DOORLOCK = 20;
    public const int DOORSECH = 21;
    public const int DOORPLAIN = 3;
    public const int DOORSECL = 23;
    public const int DOORSUPER = 24;
    //Blocks
    public const int STATUE = 0;
    public const int BARREL = 1;
    //Items
    public const int RANDARMOUR = 0;
    public const int GOODARMOUR = 1;
    public const int MAGICFOOD = 2;
    public const int RANDPOTION = 3;
    public const int RANDSCROLL = 4;
    public const int SOID = 5; //Scroll of identify
    public const int SOU = 6; //Upgrade(Enchantment)
    public const int POS = 7; //Potion of strength(gain ability)
    public const int SORC = 8; //Remove curse
    public const int PITCHEST = 9;
    public const int RANDRING = 10;
    public const int POOLPRIZE = 11;
    public const int GOODWEAPON = 12;
    public const int GOLD = 13;
    public const int GOLDHALF = 14;
    public const int AMULET = 15;
    public const int BOOK = 16;
    public const int ARTIFACT = 17; // not used.
    public const int THROWNWEP = 18;
    public const int THROWNFIRE = 19;
    public const int ZOOCHEST = 20;
    public const int POH = 21;//Potion of healing
    public const int GEMREAL = 22;
    public const int RANDWAND = 23;
    public const int RANDTOOL = 24;
    //Features
    public const int FOUNTAIN = 0;
    public const int ALTAR = 1;
    public const int BED = 2;
    public const int PDSHOP = 3;
    public const int SACREDGROVE = 4;
    public const int HOLYSHRINE = 5;
    public const int DARKSHRINE = 6;
    public const int WORKBENCH = 7;
    public const int GRAVE = 8;
    public const int SHOPPOOR = 9;
    //Monsters
    public const int MONSTER = 0;
    public const int LEPRECHAUN = 1;
    public const int LEPREWIZARD = 2;
    public const int YEOMAN = 3;
    public const int YEOMANWARDER = 4;
    public const int YEOMANCHIEF = 5;
    public const int BEE = 6;
    public const int BEEQUEEN = 7;
    public const int SMALMIMIC = 8;
    //Traps
    public const int TRAP = 0;
    public const int TRAPSP = 1;
    public const int TRAPSUMMON = 2;
  }
}