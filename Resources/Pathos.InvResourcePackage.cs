#pragma warning disable 0649
namespace Pathos.Official
{
  internal static class Resources
  {
    static Resources()
    {
      global::Inv.Resource.Foundation.Import(typeof(Resources), "Resources.Pathos.InvResourcePackage.rs");
    }

    public static readonly ResourcesQuests Quests;
    public static readonly ResourcesSpecials Specials;
  }

  internal sealed class ResourcesQuests
  {
    public ResourcesQuests() { }

    ///<Summary>(.quest) ~ (121.6KB)</Summary>
    public readonly global::Inv.Resource.BinaryReference Abyss;
    ///<Summary>(.quest) ~ (210.0KB)</Summary>
    public readonly global::Inv.Resource.BinaryReference Chambers;
    ///<Summary>(.quest) ~ (296.0KB)</Summary>
    public readonly global::Inv.Resource.BinaryReference Dhak;
    ///<Summary>(.quest) ~ (385.7KB)</Summary>
    public readonly global::Inv.Resource.BinaryReference Eobi;
    ///<Summary>(.quest) ~ (192.8KB)</Summary>
    public readonly global::Inv.Resource.BinaryReference Kingdom1;
    ///<Summary>(.quest) ~ (208.6KB)</Summary>
    public readonly global::Inv.Resource.BinaryReference Kingdom2;
    ///<Summary>(.quest) ~ (57.2KB)</Summary>
    public readonly global::Inv.Resource.BinaryReference Lair;
    ///<Summary>(.quest) ~ (15.4KB)</Summary>
    public readonly global::Inv.Resource.BinaryReference Market;
    ///<Summary>(.quest) ~ (227.7KB)</Summary>
    public readonly global::Inv.Resource.BinaryReference Phantasie;
    ///<Summary>(.quest) ~ (69.4KB)</Summary>
    public readonly global::Inv.Resource.BinaryReference Tower;
    ///<Summary>(.quest) ~ (456.8KB)</Summary>
    public readonly global::Inv.Resource.BinaryReference Underdeep;
  }

  internal sealed class ResourcesSpecials
  {
    public ResourcesSpecials() { }

    ///<Summary>(.txt) -------#HHHHH#------- (1.7KB)</Summary>
    public readonly global::Inv.Resource.TextReference AncientStronghold;
    ///<Summary>(.txt)                                          }}}}                            ----  (1.5KB)</Summary>
    public readonly global::Inv.Resource.TextReference AsmodeusLair;
    ///<Summary>(.txt)  ####### (22.0KB)</Summary>
    public readonly global::Inv.Resource.TextReference Attics;
    ///<Summary>(.txt)       }}   }}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}                   (1.5KB)</Summary>
    public readonly global::Inv.Resource.TextReference BaalzebubLair;
    ///<Summary>(.txt)                  .........         }}...          -----  }}}}}}}}}---------}} (1.5KB)</Summary>
    public readonly global::Inv.Resource.TextReference CthulhuSanctum;
    ///<Summary>(.txt)                                   }}}}  ......   }}}    .V}}}  }}}           (1.4KB)</Summary>
    public readonly global::Inv.Resource.TextReference DemogorgonLair;
    ///<Summary>(.txt)  (0.4KB)</Summary>
    public readonly global::Inv.Resource.TextReference DemonLair;
    ///<Summary>(.txt)                }}}}                                (1.0KB)</Summary>
    public readonly global::Inv.Resource.TextReference DispaterLair;
    ///<Summary>(.txt) ------------------------------------------------------ (1.1KB)</Summary>
    public readonly global::Inv.Resource.TextReference FortLudios1;
    ///<Summary>(.txt) ----------------------------------- (1.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference FortLudios2;
    ///<Summary>(.txt) ----------------------------------- (1.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference FortLudios3;
    ///<Summary>(.txt) ----------------------------------- (1.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference FortLudios4;
    ///<Summary>(.txt) ----------------------------------- (1.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference FortLudios5;
    ///<Summary>(.txt) -----------| (0.2KB)</Summary>
    public readonly global::Inv.Resource.TextReference FortLudiosCache;
    ///<Summary>(.txt)              }}}}}}}}}}      }}}}}    }}}}              (1.5KB)</Summary>
    public readonly global::Inv.Resource.TextReference GeryonLair;
    ///<Summary>(.txt) ------------------------------------- (1.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference GnollTown;
    ///<Summary>(.txt)          -----|        (0.7KB)</Summary>
    public readonly global::Inv.Resource.TextReference HallOfGiants;
    ///<Summary>(.txt) |--------------------------------------    -------------------|  ----| (1.2KB)</Summary>
    public readonly global::Inv.Resource.TextReference KoboldTown;
    ///<Summary>(.txt)          ------             (0.7KB)</Summary>
    public readonly global::Inv.Resource.TextReference LostTomb;
    ///<Summary>(.txt) }}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}} (1.5KB)</Summary>
    public readonly global::Inv.Resource.TextReference MedusaIsland1;
    ///<Summary>(.txt) }}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}} (1.5KB)</Summary>
    public readonly global::Inv.Resource.TextReference MedusaIsland2;
    ///<Summary>(.txt) }}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}} (1.5KB)</Summary>
    public readonly global::Inv.Resource.TextReference MedusaIsland3;
    ///<Summary>(.txt) }}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}} (1.6KB)</Summary>
    public readonly global::Inv.Resource.TextReference MedusaIsland4;
    ///<Summary>(.txt) }}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}|}} (6.5KB)</Summary>
    public readonly global::Inv.Resource.TextReference MedusaIslands;
    ///<Summary>(.txt)                          ---------     -----| (1.4KB)</Summary>
    public readonly global::Inv.Resource.TextReference Mines1;
    ///<Summary>(.txt) -------------------------        (1.2KB)</Summary>
    public readonly global::Inv.Resource.TextReference Mines2;
    ///<Summary>(.txt)            ---- (1.1KB)</Summary>
    public readonly global::Inv.Resource.TextReference Mines3;
    ///<Summary>(.txt)   -----         ---------             (1.7KB)</Summary>
    public readonly global::Inv.Resource.TextReference Mines4;
    ///<Summary>(.txt)                  0                   (1.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference Mines5;
    ///<Summary>(.txt) -||||---||||- (0.7KB)</Summary>
    public readonly global::Inv.Resource.TextReference MonsterLair;
    ///<Summary>(.txt)   -----                        --------------------                               (1.5KB)</Summary>
    public readonly global::Inv.Resource.TextReference OrcusTown;
    ///<Summary>(.txt)                                         ----------------- (1.5KB)</Summary>
    public readonly global::Inv.Resource.TextReference PleasantValley;
    ///<Summary>(.txt)                                                                  --------- (1.4KB)</Summary>
    public readonly global::Inv.Resource.TextReference RatInfestation;
    ///<Summary>(.txt)          ----- ----- ----- ----- ----- (0.4KB)</Summary>
    public readonly global::Inv.Resource.TextReference RottingTemple;
    ///<Summary>(.txt)  -----------------------. (0.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference SecretLaboratory;
    ///<Summary>(.txt) Dungeon Tile 10 (5.1KB)</Summary>
    public readonly global::Inv.Resource.TextReference SixBy6;
    ///<Summary>(.txt) -------- ------ (0.2KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban1a;
    ///<Summary>(.txt)  ------  -----  (0.2KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban1b;
    ///<Summary>(.txt)      -------                  (0.4KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban1c;
    ///<Summary>(.txt) ----------------- (0.2KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban1d;
    ///<Summary>(.txt)        -----------  (0.2KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban1e;
    ///<Summary>(.txt)   ----          ----------- (0.4KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban2a;
    ///<Summary>(.txt)  -----------       ----------- (0.4KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban2b;
    ///<Summary>(.txt)  --------        (0.2KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban2c;
    ///<Summary>(.txt)             --------- (0.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban2d;
    ///<Summary>(.txt) -----------------  (0.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban2e;
    ///<Summary>(.txt)           ----       (0.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban2f;
    ///<Summary>(.txt) ----------------     (0.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban2g;
    ///<Summary>(.txt)    --------           (0.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban3a;
    ///<Summary>(.txt)  -------------------- (0.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban3b;
    ///<Summary>(.txt)        ------------ (0.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban3c;
    ///<Summary>(.txt)          ---------- (0.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban3d;
    ///<Summary>(.txt)     ------------------ (0.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban3e;
    ///<Summary>(.txt)         ---------     (0.3KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban3f;
    ///<Summary>(.txt)            ----        (0.4KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban3g;
    ///<Summary>(.txt)  -------------------------- (0.5KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban4a;
    ///<Summary>(.txt)    ------------------------ (0.5KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban4b;
    ///<Summary>(.txt)             ---------  (0.4KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban4c;
    ///<Summary>(.txt) ------------       (0.4KB)</Summary>
    public readonly global::Inv.Resource.TextReference Sokoban4d;
    ///<Summary>(.txt)  (1.5KB)</Summary>
    public readonly global::Inv.Resource.TextReference SpiderCaves;
    ///<Summary>(.txt)                              }}}}}}}}        }}}}}}}}}}              (1.1KB)</Summary>
    public readonly global::Inv.Resource.TextReference SunlessSea;
    ///<Summary>(.txt) ################################ (0.9KB)</Summary>
    public readonly global::Inv.Resource.TextReference Town1Frontier;
    ///<Summary>(.txt) ################################ (0.9KB)</Summary>
    public readonly global::Inv.Resource.TextReference Town2Square;
    ///<Summary>(.txt) ################################ (0.9KB)</Summary>
    public readonly global::Inv.Resource.TextReference Town3Alley;
    ///<Summary>(.txt) ################################ (0.9KB)</Summary>
    public readonly global::Inv.Resource.TextReference Town4College;
    ///<Summary>(.txt) ################################ (0.9KB)</Summary>
    public readonly global::Inv.Resource.TextReference Town5Bustling;
    ///<Summary>(.txt) ################################ (0.9KB)</Summary>
    public readonly global::Inv.Resource.TextReference Town6Bazaar;
    ///<Summary>(.txt) ################################ (0.9KB)</Summary>
    public readonly global::Inv.Resource.TextReference Town7Orcish;
    ///<Summary>(.txt) ################################ (0.9KB)</Summary>
    public readonly global::Inv.Resource.TextReference Town8Waterway;
    ///<Summary>(.txt) ################################ (0.9KB)</Summary>
    public readonly global::Inv.Resource.TextReference Town9Lavaflow;
    ///<Summary>(.txt)                                         ---                           --- (1.6KB)</Summary>
    public readonly global::Inv.Resource.TextReference WyrmCaves;
    ///<Summary>(.txt)         }}}}       .....  GG    ----------- ------------               .Z.        (1.4KB)</Summary>
    public readonly global::Inv.Resource.TextReference YeenoghuLair;
  }
}