using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  internal sealed class OpusModule : Module
  {
    internal OpusModule(Codex Codex)
      : base(
          Handle: "opus",
          Name: "Kaloi Opus",
          Description: "Explore a vast, procedurally generated overland map containing side dungeons, elaborate set pieces and intriguing encounters.",
          Colour: Inv.Colour.Red.Darken(0.15F),
          Author: "Callan Hodgskin", Email: "hodgskin.callan@gmail.com",
          RequiresMasterMode: false)
    {
      this.Codex = Codex;

      SetIntroduction(Codex.Sonics.introduction);
      SetConclusion(Codex.Sonics.conclusion);
      SetTrack(Codex.Tracks.pixel_title);

      AddTerms(OpusTerms.List);
    }

    public override void Execute(Generator Generator) => new OpusMaker(Codex, Generator).Make();

    private readonly Codex Codex;
  }

  internal static class OpusTerms
  {
    static OpusTerms()
    {
      var Type = typeof(OpusTerms);

      var TermList = new Inv.DistinctList<string>();

      foreach (var FieldInfo in Type.GetReflectionFields())
      {
        if (FieldInfo.IsStatic && FieldInfo.IsPublic && FieldInfo.FieldType == typeof(string))
          TermList.Add((string)FieldInfo.GetValue(Type));
      }

      List = TermList;
    }

    public static readonly IReadOnlyList<string> List;

    // Opus.
    public const string Opus = "Opus";
    public const string Overland = "Overland";
    public const string Underground = "Underground";

    // Sites.
    public const string Station = "Station";
    public const string Ruins = "Ruins";
    public const string Halls = "Halls";
    public const string Cage = "Cage";
    public const string Rupture = "Rupture";
    public const string Respite = "Respite";
    public const string Rebirth = "Rebirth";
    public const string Crypt = "Crypt";
    public const string Furnace = "Furnace";
    public const string Arena = "Arena";
    public const string Prison = "Prison";
    public const string Cemetery = "Cemetery";
    public const string Grotto = "Grotto";
    public const string Ambush = "Ambush";

    // Towns.
    public const string Woodtown = "Woodtown";
    public const string Stonetown = "Stonetown";
    public const string Cavetown = "Cavetown";
    public const string Jadetown = "Jadetown";
    public const string Helltown = "Helltown";

    // Lairs.
    public const string BlackLair = "Black Lair";
    public const string BlueLair = "Blue Lair";
    public const string DeepLair = "Deep Lair";
    public const string GreenLair = "Green Lair";
    public const string OrangeLair = "Orange Lair";
    public const string RedLair = "Red Lair";
    public const string ShimmerLair = "Shimmer Lair";
    public const string WhiteLair = "White Lair";
    public const string YellowLair = "Yellow Lair";

    // Mazes.
    public const string Tree_Maze = "Tree Maze";
    public const string Shroom_Maze = "Shroom Maze";
    public const string Cave_Maze = "Cave Maze";

    // Forts.
    public const string Kobold_Fort = "Kobold Fort";
    public const string Outpost = "Outpost";

    // Pits.
    public const string Snake_Pit = "Snake Pit";
    public const string Pudding_Pit = "Pudding Pit";
    public const string Slime_Pit = "Slime Pit";
    public const string Jelly_Pit = "Jelly Pit";
    public const string Moss_Pit = "Moss Pit";
    public const string Rat_Pit = "Rat Pit";
    public const string Eye_Pit = "Eye Pit";
    public const string Worm_Pit = "Worm Pit";

    // Ravines.
    public const string River = "River";
    public const string Canyon = "Canyon";
    public const string Glacier = "Glacier";
    public const string Lava_Flow = "Lava Flow";

    // Lakes.
    public const string Lake = "Lake";
    public const string Crater = "Crater";
    public const string Ice_Lake = "Ice Lake";
    public const string Lava_Lake = "Lava Lake";

    // Henges.
    public const string Stone_Henge = "Stone Henge";
    public const string Statue_Henge = "Statue Henge";
    public const string Crystal_Henge = "Crystal Henge";

    // Mines.
    public const string RubyMine = "Ruby Mine";
    public const string EmeraldMine = "Emerald Mine";
    public const string SapphireMine = "Sapphire Mine";
    public const string DiamondMine = "Diamond Mine";
    public const string TopazMine = "Topaz Mine";

    // Nests.
    public const string AntNest = "Ant Nest";
    public const string AntCave = "Ant Cave";
    public const string AntLair = "Ant Lair";
    public const string ScorpionNest = "Scorpion Nest";
    public const string ScorpionCave = "Scorpion Cave";
    public const string ScorpionLair = "Scorpion Lair";
    public const string SpiderNest = "Spider Nest";
    public const string SpiderCave = "Spider Cave";
    public const string SpiderLair = "Spider Lair";

    // Secret Farms.
    public const string ChickenFarm = "Chicken Farm";
    public const string CowFarm = "Cow Farm";
    public const string GoatFarm = "Goat Farm";
    public const string HorseFarm = "Horse Farm";
    public const string PigFarm = "Pig Farm";
    public const string SealFarm = "Seal Farm";
    public const string SheepFarm = "Sheep Farm";
    public const string StrangeChicken = "strange chicken";
    public const string StrangeCow = "strange cow";
    public const string StrangeGoat = "strange goat";
    public const string StrangeHorse = "strange horse";
    public const string StrangePig = "strange pig";
    public const string StrangeSeal = "strange seal";
    public const string StrangeSheep = "strange sheep";

    // TODO: future sites.
    //public const string Hospital = "Hospital";
    //public const string Laboratory = "Laboratory";
    //public const string Sanatorium = "Sanatorium";
    //public const string Temple = "Temple";
  }

  internal sealed class OpusMaker
  {
    internal OpusMaker(Codex Codex, Generator Generator)
    {
      this.Codex = Codex;
      this.Generator = Generator;
      this.ShopProbability = new Probability<Shop>();
      this.ShrineProbability = new Probability<Shrine>();
      this.GoodCharacterList = new Inv.DistinctList<Character>();
      this.EvilCharacterList = new Inv.DistinctList<Character>();
      this.UniqueEntityList = new Inv.DistinctList<Entity>();

      this.OccupyList = new[]
      {
        new OpusOccupy(Codex.Kinds.dwarf),
        new OpusOccupy(Codex.Kinds.elf),
        new OpusOccupy(Codex.Kinds.fairy),
        new OpusOccupy(Codex.Kinds.giant),
        new OpusOccupy(Codex.Kinds.gnoll),
        new OpusOccupy(Codex.Kinds.gnome),
        new OpusOccupy(Codex.Kinds.human),
        new OpusOccupy(Codex.Kinds.imp),
        new OpusOccupy(Codex.Kinds.kobold),
        new OpusOccupy(Codex.Kinds.lizardman),
        new OpusOccupy(Codex.Kinds.ogre),
        new OpusOccupy(Codex.Kinds.orc), // contains goblins/hobgoblins.
        new OpusOccupy(Codex.Kinds.troll),
        new OpusOccupy(Codex.Kinds.vampire),
      };

#if DEBUG
      //for (var Difficulty = 1; Difficulty <= 40; Difficulty++)
      //  Debug.WriteLine($"Occupy {Difficulty:D2} = " + GetOccupy(Difficulty).Select(O => O.Kind.Name).OrderBy().AsSeparatedText(", "));
#endif

      this.CornerList = new[]
      {
        new OpusCorner { Closed = new [] { Direction.NorthWest, Direction.North, Direction.West }, Open = new [] { Direction.South, Direction.SouthEast, Direction.East } },
        new OpusCorner { Closed = new [] { Direction.NorthEast, Direction.North, Direction.East }, Open = new [] { Direction.South, Direction.SouthWest, Direction.West } },
        new OpusCorner { Closed = new [] { Direction.SouthWest, Direction.South, Direction.West }, Open = new [] { Direction.North, Direction.NorthEast, Direction.East } },
        new OpusCorner { Closed = new [] { Direction.SouthEast, Direction.South, Direction.East }, Open = new [] { Direction.North, Direction.NorthWest, Direction.West } },
      };

      this.Ambush = new AmbushBuilder(this);
      this.Cage = new CageBuilder(this);
      this.Cemetery = new CemeteryBuilder(this);
      this.Crypt = new CryptBuilder(this);
      this.DragonLair = new DragonLairBuilder(this);
      this.Farm = new FarmBuilder(this);
      this.Finale = new FinaleBuilder(this);
      this.Halls = new HallsBuilder(this);
      this.Henge = new HengeBuilder(this);
      this.Horde = new HordeBuilder(this);
      this.KoboldFort = new KoboldFortBuilder(this);
      this.Lake = new LakeBuilder(this);
      this.Maze = new MazeBuilder(this);
      this.Mine = new MineBuilder(this);
      this.Nest = new NestBuilder(this);
      this.Origin = new OriginBuilder(this);
      this.Outpost = new OutpostBuilder(this);
      this.Pit = new PitBuilder(this);
      this.Print = new PrintBuilder(this);
      this.Prison = new PrisonBuilder(this);
      this.Ravine = new RavineBuilder(this);
      this.Road = new RoadBuilder(this);
      this.Ruins = new RuinsBuilder(this);
      //this.Sanatorium = new SanatoriumBuilder(this);
      this.Station = new StationBuilder(this);
      this.Town = new TownBuilder(this);
      this.Vendor = new VendorBuilder(this);
      this.Zoo = new ZooBuilder(this);

      this.BuildRecordDictionary = new Dictionary<string, BuildRecord>();
    }

    public void Make()
    {
      BuildRecordDictionary.Clear();

      // TODO:
      // * skirmish: mercenary fighting a horde
      // * troll bridge with billy goats
      // * bee hive and queen
      // * beach with sand and water
      // * cat island
      // * circle of flames trick
      // * elemental zone: flame/shock/frost/earth/water/wind plane (volatiles, vortex, elemental, seeker/binder/maker)
      // * dwarf fortress/human castle + moat + bridge (UnderWW?)
      // * tavern
      // * cave entrance - cave bear, sleeping
      // * sewers with sewer rats
      // * gateway (statues & guards)

      var CriticalList = new Action<OpusSection>[]
      {
        (Section) => Origin.Build(Section),
          (Section) => Maze.Build(Section),
          (Section) => Cemetery.Build(Section), // crypt
          (Section) => Ravine.Build(Section),
        (Section) => Town.Build(Section),
          (Section) => Lake.Build(Section),
          (Section) => Ruins.Build(Section), // ruins
          (Section) => Ravine.Build(Section),
          (Section) => Mine.Build(Section), // mine
        (Section) => Town.Build(Section),
          (Section) => Lake.Build(Section),
          (Section) => Farm.Build(Section), // garden
          (Section) => Outpost.Build(Section),
          (Section) => Henge.Build(Section),
          (Section) => Halls.Build(Section), // halls
        (Section) => Town.Build(Section),
          (Section) => Maze.Build(Section),
          (Section) => Ambush.Build(Section),
          (Section) => Ravine.Build(Section),
          (Section) => Nest.Build(Section), // nest
        (Section) => Town.Build(Section),
          (Section) => Lake.Build(Section),
          (Section) => Prison.Build(Section), // prison
          (Section) => Henge.Build(Section),
          (Section) => Cage.Build(Section), // cage
          (Section) => Ravine.Build(Section),
        (Section) => Town.Build(Section),
          (Section) => Lake.Build(Section),
          (Section) => Henge.Build(Section), // furnace
          (Section) => Maze.Build(Section),
          (Section) => DragonLair.Build(Section), // lair
          (Section) => Road.Build(Section),
          (Section) => Finale.Build(Section)
      };

      var SideList = new Action<OpusClearing>[]
      {
        (Clearing) => Vendor.Build(Clearing),
        (Clearing) => Vendor.Build(Clearing),
        (Clearing) => Horde.Build(Clearing),
        (Clearing) => Zoo.Build(Clearing),
        (Clearing) => Print.Build(Clearing)
      };

      var OpusDesign = NewDesign(256, 256, CriticalList.Length - 1); // minus one because origin counts as distance zero.

      var OpusSite = Generator.Adventure.World.AddSite(Generator.EscapedModuleTerm(OpusTerms.Opus));

      this.OverlandMap = OpusSite.World.AddMap(Generator.EscapedModuleTerm(OpusTerms.Overland), OpusDesign.Width, OpusDesign.Height);
      OverlandMap.SetTerminal(false);
      OverlandMap.SetAtmosphere(Codex.Atmospheres.forest);
      var OverlandLevel = OpusSite.AddLevel(1, OverlandMap);

      this.UndergroundMap = OpusSite.World.AddMap(Generator.EscapedModuleTerm(OpusTerms.Underground), OverlandMap.Region.Width, OverlandMap.Region.Height);
      UndergroundMap.SetTerminal(true);
      UndergroundMap.SetAtmosphere(Codex.Atmospheres.cavern);
      var UndergroundLevel = OpusSite.AddLevel(2, UndergroundMap);

      // unassigned unique entities still need to be generated so they can be transported into the finale arena.
      var UnassignedSquare = OverlandMap.Midpoint;
      foreach (var Good in new[] { Codex.Entities.Earendil, Codex.Entities.Thorin, Codex.Entities.Twoflower, Codex.Entities.Pelias, Codex.Entities.Orion, Codex.Entities.Norn, Codex.Entities.Neferet, Codex.Entities.Van_Helsing, Codex.Entities.Lord_Sato, Codex.Entities.Aleax, Codex.Entities.Solar, Codex.Entities.Planetar, Codex.Entities.Archmage_Dirachi, Codex.Entities.Archmage_Flaynn, Codex.Entities.Archpriest_Avvakrum, Codex.Entities.Aphrodite, Codex.Entities.Elwing, Codex.Entities.Hippocrates })
        NewGoodCharacter(UnassignedSquare, SelectUniqueEntity(Good));

      var FinaleCharacter = NewEvilCharacter(UnassignedSquare, SelectUniqueEntity(Codex.Entities.Kaloi_Thrym));
      FinaleCharacter.Inventory.Carried.Add(Generator.NewSpecificAsset(UnassignedSquare, Codex.Items.Stamped_Letter));

      foreach (var Evil in new[] { Codex.Entities.Cthulhu, Codex.Entities.Father_Dagon, Codex.Entities.Mother_Hydra, Codex.Entities.Lycaon, Codex.Entities.Metamorphius, Codex.Entities.Master_Kaen, Codex.Entities.Ashikaga_Takauji, Codex.Entities.Lareth, Codex.Entities.Death, Codex.Entities.Pestilence, Codex.Entities.Famine, Codex.Entities.Charon, Codex.Entities.Cerberus, Codex.Entities.Vecna })
        NewEvilCharacter(UnassignedSquare, SelectUniqueEntity(Evil));

      // town transportal station.
      Station.Build();

      var OverlandInternalRegion = OverlandMap.Region.Reduce(1);

      foreach (var Section in OpusDesign.Sections)
      {
        var LastClearing = (OpusClearing)null;
        foreach (var Clearing in Section.Clearings)
        {
          // fill the clearing with dirt.
          foreach (var ClearingSquare in OverlandMap.GetCircleInnerSquares(Clearing.Circle))
          {
            if (ClearingSquare.Floor == null)
            {
              ClearingSquare.SetLit(true);
              Generator.PlaceFloor(ClearingSquare, Codex.Grounds.dirt);
            }
          }

          if (LastClearing != null)
          {
            // deviate from the last clearing to this clearing.
            var LastSquare = OverlandMap[LastClearing.Circle.X, LastClearing.Circle.Y];
            var NextSquare = OverlandMap[Clearing.Circle.X, Clearing.Circle.Y];

            foreach (var DeviateSquare in DeviatingPath(LastSquare, NextSquare, OverlandInternalRegion))
            {
              if (DeviateSquare.Floor == null)
              {
                DeviateSquare.SetLit(true);
                Generator.PlaceFloor(DeviateSquare, Codex.Grounds.dirt);
              }
            }
          }

          LastClearing = Clearing;
        }

        // connect a random clearing to each join.
        var SourceClearing = Section.Clearings.GetRandom();
        var SourceSquare = OverlandMap[SourceClearing.Circle.X, SourceClearing.Circle.Y];

        foreach (var Join in Section.Joins)
        {
          // find any existing join squares.
          var LinkSquareList = OverlandMap.GetSquares(Join.Region).Where(S => S.Floor != null).ToDistinctList();

          if (LinkSquareList.Count == 0)
          {
            // otherwise, select a random contiguous subset of the join region.
            var LinkLeft = RandomSupport.NextRange(Join.Region.Left, Join.Region.Right);
            var LinkTop = RandomSupport.NextRange(Join.Region.Top, Join.Region.Bottom);
            var LinkRight = RandomSupport.NextRange(LinkLeft, Join.Region.Right);
            var LinkBottom = RandomSupport.NextRange(LinkTop, Join.Region.Bottom);
            var LinkRegion = new Region(LinkLeft, LinkTop, LinkRight, LinkBottom);

            LinkSquareList.AddRange(OverlandMap.GetSquares(LinkRegion));
          }

          // deviate from the clearing source to each link square.
          foreach (var LinkSquare in LinkSquareList)
          {
            foreach (var DeviateSquare in DeviatingPath(SourceSquare, LinkSquare, OverlandInternalRegion))
            {
              if (DeviateSquare.Floor == null)
              {
                DeviateSquare.SetLit(true);
                Generator.PlaceFloor(DeviateSquare, Codex.Grounds.dirt);
              }
            }
          }
        }

        Debug.Assert(Section.Critical, "Non-critical sections are not yet implemented.");

        // somewhat of a hack so that the generation methods always work correctly (eg. tins, eggs).
        OverlandMap.SetDifficulty(Section.MaximumDifficulty);
        UndergroundMap.SetDifficulty(Section.MaximumDifficulty);
        try
        {
          CriticalList[Section.Distance % CriticalList.Length](Section);

          var DecorateList = new Inv.DistinctList<OpusClearing>();

          foreach (var Clearing in Section.Clearings)
          {
            // completely empty clearings can have a side generation.
            if (OverlandMap.GetCircleInnerSquares(Clearing.Circle).All(S => S.Floor?.Ground == Codex.Grounds.dirt && S.Wall == null && S.Boulder == null && S.Fixture == null && S.Trap == null && S.Passage == null && S.Character == null))
              SideList.GetRandom()(Clearing);
            else
              DecorateList.Add(Clearing);
          }

          // decorate plain clearings with some trees at least.. what else?
          foreach (var Clearing in DecorateList)
          {
            bool IsDirt(Square S) => S.Floor?.Ground == Codex.Grounds.dirt;
            foreach (var SpaceSquare in OverlandMap.GetCircleInnerSquares(Clearing.Circle).Where(S => IsDirt(S) && S.Wall == null))
            {
              if (SpaceSquare.GetAroundSquares().All(S => IsDirt(S) && S.Wall == null && S.Boulder == null && S.Fixture == null && S.Trap == null && S.Passage == null && S.Character == null && S.Stash == null))
              {
                if (Chance.OneIn3.Hit())
                  Generator.PlaceSolidWall(SpaceSquare, Codex.Barriers.tree, WallSegment.Pillar);
              }
            }
          }
        }
        finally
        {
          UndergroundMap.SetDifficulty(0);
          OverlandMap.SetDifficulty(0);
        }
      }

      // check overland chasms and descent traps match to underground caves.
      foreach (var OverlandSquare in OverlandMap.GetSquares())
      {
        if (OverlandSquare.Floor?.Ground == Codex.Grounds.chasm)
        {
          var UndergroundSquare = UndergroundMap[OverlandSquare.X, OverlandSquare.Y];

          Debug.Assert(UndergroundSquare.Floor != null, "Overland chasm must be matched to an underground cavern.");
        }
        else if ((OverlandSquare.Trap?.Device.Descent ?? false) || OverlandSquare.Trap?.Device == Codex.Devices.squeaky_board) // squeaky board converts to hole.
        {
          OverlandSquare.SetTrap(null); // erase descent traps for now.

          /* trapdoors/holes do not drop the character to a fixed location.
          var UndergroundSquare = UndergroundMap[OverlandSquare.X, OverlandSquare.Y];

          if (UndergroundSquare.Floor == null)
          {
            Generator.PlaceFloor(UndergroundSquare, Codex.Grounds.cave_floor);
            Generator.PlacePassage(UndergroundSquare, Codex.Portals.wooden_ladder_up, OverlandSquare.GetAdjacentSquares().Where(S => S.Floor != null && S.Wall == null).GetRandomOrNull() ?? OverlandSquare);
          }
          */
        }
      }

      Debug.Assert(OverlandMap.GetFrameSquares(OverlandMap.Region).All(S => S.IsVoid()), "Overland map boundary squares are required to be void so that the permanent trees can be placed.");
      Debug.Assert(UndergroundMap.GetFrameSquares(UndergroundMap.Region).All(S => S.IsVoid()), "Underground map boundary squares are required to be void so that the permanent cave walls can be placed.");

      // repair tree boundary around the overland.
      RepairBoundary(OverlandMap, OverlandMap.Region, Codex.Barriers.tree, Codex.Grounds.dirt, IsLit: true);

      // repair the overland map including zones.
      Generator.RepairMap(OverlandMap, OverlandMap.Region);

      // repair cave boundary around the underground.
      RepairBoundary(UndergroundMap, UndergroundMap.Region, Codex.Barriers.cave_wall, Codex.Grounds.cave_floor, IsLit: true);

      // repair the underground map including zones.
      Generator.RepairMap(UndergroundMap, UndergroundMap.Region);

      // make all characters dormant so their AI doesn't impact on CPU until they are disturbed.
      foreach (var Map in Generator.Adventure.World.Maps)
      {
        foreach (var Character in Map.GetCharacters())
          Character.SetDormant(true);
      }

      // apply area difficulty based on distance from starting section.
      foreach (var Section in OpusDesign.Sections)
      {
        foreach (var Map in new[] { OverlandMap, UndergroundMap })
        {
          var Area = Map.AddArea(Generator.EscapedModuleTerm(Map == OverlandMap ? Section.OverlandAreaName : Section.UndergroundAreaName));

          if (Section.Distance == 0)
            Area.SetSpawnRestricted(true);

          if (Area.Difficulty == 0)
            Area.SetDifficulty(Section.Distance);
          else if (Area.Difficulty > Section.Distance)
            Area.SetDifficulty(Section.Distance);

          foreach (var Square in Map.GetSquares(Section.Region))
            Area.AddZone(Square.Zone);

          if (Area.Zones.Count == 0)
            Map.RemoveArea(Area);
        }
      }

#if DEBUG
      foreach (var UniqueEntity in Codex.Entities.List.Where(E => E.IsUnique).Except(UniqueEntityList))
        Debug.WriteLine($"{UniqueEntity.Name} was not considered in this epic module.");

      void SetSiteSuffixFirstLevelUp(string SiteName) => Generator.Adventure.World.SetStart(Generator.Adventure.World.Sites.Find(S => S.Name.EndsWith(SiteName + "_")).FirstLevel.UpSquare);
      void SetSiteSuffixFirstLevelDown(string SiteName) => Generator.Adventure.World.SetStart(Generator.Adventure.World.Sites.Find(S => S.Name.EndsWith(SiteName + "_")).FirstLevel.DownSquare);
      void SetSiteFirstLevelUp(string SiteName) => Generator.Adventure.World.SetStart(Generator.Adventure.World.GetSite(Generator.EscapedModuleTerm(SiteName)).FirstLevel.UpSquare);
      void SetSiteFirstLevelEntrance(string SiteName) => Generator.Adventure.World.SetStart(Generator.Adventure.World.GetSite(Generator.EscapedModuleTerm(SiteName)).FirstLevel.UpSquare.Passage.Destination);
      void SetSiteLastLevelUp(string SiteName) => Generator.Adventure.World.SetStart(Generator.Adventure.World.GetSite(Generator.EscapedModuleTerm(SiteName)).LastLevel.UpSquare);
      void SetMapLevelUp(string MapName) => Generator.Adventure.World.SetStart(Generator.Adventure.World.GetMap(Generator.EscapedModuleTerm(MapName)).Level.UpSquare);
      void SetMapLevelDown(string MapName) => Generator.Adventure.World.SetStart(Generator.Adventure.World.GetMap(Generator.EscapedModuleTerm(MapName)).Level.DownSquare);
      void SetMapMiddle(string MapName) => Generator.Adventure.World.SetStart(Generator.Adventure.World.GetMap(Generator.EscapedModuleTerm(MapName)).Midpoint);

      SetSiteFirstLevelUp(OpusTerms.Crypt);
      SetMapMiddle(OpusTerms.Station);
      SetSiteSuffixFirstLevelUp("Mine");
      SetSiteSuffixFirstLevelUp("Lair");
      SetSiteFirstLevelUp(OpusTerms.Halls);
      SetSiteFirstLevelUp(OpusTerms.Cage);
      SetSiteFirstLevelUp(OpusTerms.Prison);
      SetMapLevelDown(OpusTerms.Overland);
      SetSiteSuffixFirstLevelDown("Nest");
      Generator.Adventure.World.SetStart(FinaleSquare);
      SetSiteFirstLevelUp(OpusTerms.Ruins);
      SetSiteFirstLevelUp(OpusTerms.Furnace);
#endif

      // write out build records for performance tracking.
#if DEBUG
      Debug.WriteLine($"Total = {BuildRecordDictionary.Values.Sum(V => V.ElapsedMSList.Sum()):N0}ms");
      foreach (var BuildRecord in BuildRecordDictionary.Values)
        Debug.WriteLine($"{BuildRecord.ElapsedMSList.Count} x {BuildRecord.Name} = {BuildRecord.ElapsedMSList.Sum():N0}ms (~avg {BuildRecord.ElapsedMSList.Average():N0}ms) {BuildRecord.ElapsedMSList.Select(E => $"{E:N0}ms").AsSeparatedText(", ")}");
#endif
    }

    private OpusDesign NewDesign(int Width, int Height, int CriticalDistance)
    {
      var Plan = new OpusPlanner()
      {
        StartMinSize = 8,
        StartMaxSize = 12,
        SectorMinSize = 16,
        SectorMaxSize = 32,
        CriticalDistance = CriticalDistance
      }.Generate(Width, Height);

      Plan.PlotCriticalPaths();
      Plan.PruneSidePaths();
      Plan.ClipAndResize();

      var SectionList = new Inv.DistinctList<OpusSection>(Plan.Sectors.Count);

      var SectorSectionDictionary = new Dictionary<OpusSector, OpusSection>(Plan.Sectors.Count);

      foreach (var Sector in Plan.Sectors)
      {
        var Section = new OpusSection(Sector.Region, Sector.Distance);
        SectionList.Add(Section);
        Section.Critical = Sector.Critical;

        SectorSectionDictionary.Add(Sector, Section);
      }

      foreach (var SectorSectionEntry in SectorSectionDictionary)
      {
        var Sector = SectorSectionEntry.Key;
        var Section = SectorSectionEntry.Value;

        foreach (var Link in Sector.Links)
          Section.AddJoin(Link.Region, SectorSectionDictionary[Link.Sector]);
      }

      foreach (var Section in SectionList)
      {
        var SectionRegion = Section.Region;
        var SectionSize = Math.Min(SectionRegion.Width, SectionRegion.Height);

        var ClearingSize = (SectionSize - 4) / 2;

        foreach (var ClearingIndex in RandomSupport.NextRange(3, 6).NumberSeries())
        {
          var ClearingRadius = RandomSupport.NextRange(3, ClearingSize);
          var ClearingX = RandomSupport.NextRange(SectionRegion.Left + ClearingRadius + 1, SectionRegion.Right - ClearingRadius - 1);
          var ClearingY = RandomSupport.NextRange(SectionRegion.Top + ClearingRadius + 1, SectionRegion.Bottom - ClearingRadius - 1);
          var ClearingCircle = new Inv.Circle(ClearingX, ClearingY, ClearingRadius);

          if (!Section.Clearings.Any(C => C.Circle == ClearingCircle))
            Section.AddClearing(ClearingCircle);
        }
      }

      return new OpusDesign(Plan.Width, Plan.Height, SectionList);
    }
    private void PlaceMimics(OpusSection Section, IReadOnlyList<Square> SquareList, int Mimics)
    {
      foreach (var Mimic in Mimics.NumberSeries())
      {
        // mimics should not be directly adjacent to the merchant or any other character.
        var MimicSquare = SquareList.Where(S => Generator.CanPlaceCharacter(S) && S.GetAdjacentSquares().All(A => A.Character == null)).GetRandomOrNull();
        if (MimicSquare != null)
          Generator.PlaceCharacter(MimicSquare, Section.MinimumDifficulty, Section.MaximumDifficulty, E => E.IsEncounter && E.IsMimicking());
      }
    }
    private void RepairBoundary(Map BoundaryMap, Region BoundaryRegion, Barrier BoundaryBarrier, Ground RegularGround, bool IsLit)
    {
      // first pass to surround all edge floor squares with a permanent barrier.
      foreach (var BoundarySquare in BoundaryMap.GetSquares(BoundaryRegion))
      {
        if (BoundarySquare.IsVoid() && BoundarySquare.GetAdjacentSquares().Any(S => S.Floor != null && S.Wall == null))
        {
          BoundarySquare.SetLit(IsLit);
          Generator.PlaceWall(BoundarySquare, BoundaryBarrier, WallStructure.Permanent, WallSegment.Pillar);
        }
      }

      // second pass to convert non-boundary permanent barriers into solid barriers.
      foreach (var BoundarySquare in OverlandMap.GetSquares())
      {
        var BorderWall = BoundarySquare.Wall;

        if (BorderWall != null && BorderWall.Structure == WallStructure.Permanent && BorderWall.Barrier == BoundaryBarrier && (!BoundarySquare.GetAdjacentSquares().Any(S => S.IsVoid()) && !BoundarySquare.IsEdge(BoundaryRegion)))
        {
          BoundarySquare.Wall.SetStructure(WallStructure.Solid);

          if (BoundarySquare.Floor == null && !BoundaryBarrier.Rebound)
            Generator.PlaceFloor(BoundarySquare, RegularGround);
        }
      }
    }
    private void RepairVeranda(Map Map, Region Region, Ground Ground, bool IsLit)
    {
      foreach (var OutsideSquare in Map.GetSquares(Region))
      {
        if (OutsideSquare.IsVoid() && OutsideSquare.GetAdjacentSquares().Any(S => S.Wall != null || S.Door != null))
        {
          OutsideSquare.SetLit(IsLit);
          Generator.PlaceFloor(OutsideSquare, Ground);
        }
      }
    }
    private void RepairGaps(Map Map, Region Region, Barrier Barrier, bool IsLit)
    {
      bool IsWall(Square Square, Direction Direction) => Square.Adjacent(Direction)?.Wall != null;
      bool IsFloor(Square Square, Direction Direction) => Square.Adjacent(Direction)?.Floor != null;
      void RepairGap(Square Square)
      {
        Square.SetFloor(null);
        Generator.PlaceSolidWall(Square, Barrier, WallSegment.Pillar);
        Square.SetLit(IsLit);
      }
      foreach (var Square in Map.GetSquares(Region).Where(S => S.Floor != null))
      {
        var NorthWall = IsWall(Square, Direction.North);
        var SouthWall = IsWall(Square, Direction.South);
        var WestWall = IsWall(Square, Direction.West);
        var EastWall = IsWall(Square, Direction.East);

        if (NorthWall && WestWall && IsFloor(Square, Direction.NorthWest) && IsFloor(Square, Direction.East) && IsFloor(Square, Direction.South) && IsFloor(Square, Direction.SouthEast))
          RepairGap(Square);
        else if (NorthWall && EastWall && IsFloor(Square, Direction.NorthEast) && IsFloor(Square, Direction.West) && IsFloor(Square, Direction.South) && IsFloor(Square, Direction.SouthWest))
          RepairGap(Square);
        else if (SouthWall && WestWall && IsFloor(Square, Direction.SouthWest) && IsFloor(Square, Direction.East) && IsFloor(Square, Direction.North) && IsFloor(Square, Direction.NorthEast))
          RepairGap(Square);
        else if (SouthWall && EastWall && IsFloor(Square, Direction.SouthEast) && IsFloor(Square, Direction.West) && IsFloor(Square, Direction.North) && IsFloor(Square, Direction.NorthWest))
          RepairGap(Square);

        // TODO: there are some cases where the gaps won't get repaired.
      }
    }
    private void ConnectSquares(IEnumerable<Square> SpaceSquares, Square StartSquare, Action<Square> ConnectAction)
    {
      var SpaceSet = SpaceSquares.Where(S => S.Floor != null).ToHashSetX();
      do
      {
        var FlatSet = new HashSet<Square>();
        var PathSet = new HashSet<Square>();
        StartSquare.FloodNeighbour(Square =>
        {
          if (Square.Floor != null && Square.Wall == null)
          {
            if (!SpaceSet.Contains(Square))
              return false;

            return PathSet.Add(Square);
          }

          if (Square.Wall != null && Square.IsFlat())
            FlatSet.Add(Square);

          return false;
        });

        if (PathSet.Count >= SpaceSet.Count)
        {
          // all floor squares are reachable.
          break;
        }
        else
        {
          // choose a flat wall that will open up to a square that is yet to be reached.
          var PunchSquare = FlatSet.Where(F => F.GetNeighbourSquares().Any(S => S.Floor != null && S.Wall == null && SpaceSet.Contains(S) && S.Door == null && !PathSet.Contains(S))).GetRandomOrNull();
          if (PunchSquare == null || PunchSquare.Wall == null)
          {
            //Debug.Fail("Unable to punch a door to all spaces on the map - there will be a disconnected area.");
            break;
          }

          ConnectAction(PunchSquare);

          Debug.Assert(PunchSquare.Floor != null);

          if (PunchSquare.Floor != null)
            SpaceSet.Add(PunchSquare);
        }
      }
      while (true);
    }
    private void PlaceRoomBoulders(IReadOnlyList<Square> SquareList)
    {
      // boulders.
      if (Chance.OneIn25.Hit())
      {
        var BoulderSquare = SquareList.Where(Generator.CanPlaceBoulder).GetRandomOrNull();

        // NOTE: don't place boulders in front of doors to ensure we don't generate maps that are unplayable.
        if (BoulderSquare != null && !BoulderSquare.GetCompassSquares().Any(S => S.Door != null))
        {
          if (Chance.OneIn12.Hit())
          {
            Generator.PlaceBoulder(BoulderSquare, Codex.Blocks.statue, IsRigid: false);
          }
          else if (Chance.OneIn3.Hit())
          {
            Generator.PlaceBoulder(BoulderSquare, Codex.Blocks.wooden_barrel, IsRigid: false);
          }
          else
          {
            Generator.PlaceBoulder(BoulderSquare, BoulderSquare.Floor?.Ground.Block ?? Codex.Blocks.stone_boulder, IsRigid: false);

            // any items under the boulder?
            if (!BoulderSquare.HasAssets())
            {
              if (BoulderSquare.GetAdjacentSquares().Count(S => S.Wall != null || S.Floor == null) >= 5)
              {
                // blocked boulders are more likely to have items under them.
                if (Chance.OneIn2.Hit())
                  Generator.PlaceRandomAsset(BoulderSquare);
              }
              else if (Chance.OneIn5.Hit())
              {
                Generator.PlaceRandomAsset(BoulderSquare);
              }
            }
          }
        }
      }
    }
    private IEnumerable<Square> FindUnreachableSquares(Map ReachMap, Region ReachRegion, Square ReachSquare)
    {
      Debug.Assert(ReachSquare.Map == ReachMap);

      var SpaceSet = ReachMap.GetSquares(ReachRegion).Where(S => S.Floor != null).ToHashSetX();
      var PathSet = new HashSet<Square>();
      ReachSquare.FloodNeighbour(Square =>
      {
        if (Square.Floor != null)
        {
          if (!SpaceSet.Contains(Square))
            return false;

          return PathSet.Add(Square);
        }

        return false;
      });

      return SpaceSet.Except(PathSet);
    }
    private IEnumerable<Region> ScanBoundedRegions(Map ScanMap, Region ScanRegion)
    {
      bool IsSpace(Square Square) => Square.Floor != null;
      bool IsBoundary(Square Square) => Square.Wall != null;

      foreach (var ScanSquare in ScanMap.GetSquares(ScanRegion))
      {
        // if open floor has a top-left corner of walls then this could be a rectangular region.
        if (IsSpace(ScanSquare) && IsBoundary(ScanSquare.Adjacent(Direction.West)) && IsBoundary(ScanSquare.Adjacent(Direction.North)) && IsBoundary(ScanSquare.Adjacent(Direction.NorthWest)))
        {
          // rectangular search.
          var RoomLeft = ScanSquare.X;
          var RoomTop = ScanSquare.Y;

          var RoomX = RoomLeft + 1;
          var RoomY = RoomTop;

          // find the cells in the first row.
          while (IsSpace(ScanMap[RoomX, RoomY]))
            RoomX++;

          var RoomRight = RoomX - 1;

          // find the rest of the rows.
          while (ScanMap.GetSquares(new Region(RoomLeft, RoomY, RoomRight, RoomY)).All(S => IsSpace(S)))
            RoomY++;

          var RoomBottom = RoomY - 1;

          // open space region is expanded to include the expected boundary walls.
          var ResultRegion = new Region(RoomLeft, RoomTop, RoomRight, RoomBottom).Expand(1);
          if (ScanMap.GetFrameSquares(ResultRegion).All(S => IsBoundary(S)))
            yield return ResultRegion;
        }
      }
    }
    private HashSet<Region> RandomConnectedRegions(int GenerateCount, Func<Region> GenerateFunc)
    {
      // it is possible to have two or more distinct groups of regions that are disconnected.
      var ConnectList = new Inv.DistinctList<HashSet<Region>>(1);
      var BoxRegionList = new Inv.DistinctList<Region>(GenerateCount);
      do
      {
        foreach (var BoxIndex in GenerateCount.NumberSeries())
        {
          var BoxAttempt = 0;
          do
          {
            var BoxRegion = GenerateFunc();

            if (!BoxRegionList.Contains(BoxRegion))
            {
              BoxRegionList.Add(BoxRegion);
              break;
            }
          }
          while (BoxAttempt++ < 3);
        }

        while (BoxRegionList.Count > 0)
        {
          var ScanSet = new HashSet<Region>();

          void Scan(Region ScanRegion)
          {
            if (ScanSet.Add(ScanRegion))
            {
              BoxRegionList.Remove(ScanRegion);

              foreach (var IntersectRegion in BoxRegionList.Where(R => ScanRegion.Intersects(R)))
                Scan(IntersectRegion);
            }
          }

          Scan(BoxRegionList[0]);

          ConnectList.Add(ScanSet);
        }

        var DuplicateRegionArray = ConnectList.SelectMany(S => S).Duplicates().ToArray();
        Debug.Assert(DuplicateRegionArray.Length == 0, "Why are there duplicate regions.");

        if (ConnectList.Count > 1)
        {
          ConnectList.Sort((A, B) => B.Count.CompareTo(A.Count));

          // go around again to regenerate the orphaned regions.
          BoxRegionList.AddRange(ConnectList[0]);
          GenerateCount = ConnectList.Skip(1).Sum(S => S.Count);

          ConnectList.Clear();
        }
      }
      while (ConnectList.Count != 1);

      // end up with only one connected group of regions.
      return ConnectList.Single();
    }
    private IEnumerable<Square> WalkingPath(Square SourceSquare, Square TargetSquare)
    {
      var WalkSquare = SourceSquare;

      if (WalkSquare != null)
        yield return WalkSquare;

      var BiasVertical = Chance.OneIn2.Hit();

      while (WalkSquare != null && WalkSquare != TargetSquare)
      {
        var Offset = WalkSquare.AsOffset(TargetSquare);

        var GoWest = Offset.X < 0;
        var GoEast = Offset.X > 0;
        var GoNorth = Offset.Y < 0;
        var GoSouth = Offset.Y > 0;

        if (BiasVertical && (GoNorth || GoSouth) && (GoWest || GoEast))
        {
          GoEast = false;
          GoWest = false;
        }

        if (GoWest)
          WalkSquare = WalkSquare.Adjacent(Direction.West);
        else if (GoEast)
          WalkSquare = WalkSquare.Adjacent(Direction.East);
        else if (GoNorth)
          WalkSquare = WalkSquare.Adjacent(Direction.North);
        else if (GoSouth)
          WalkSquare = WalkSquare.Adjacent(Direction.South);

        if (WalkSquare != null)
          yield return WalkSquare;
      }
    }
    private IEnumerable<Square> DeviatingPath(Square SourceSquare, Square TargetSquare, Region ConstraintRegion)
    {
      var DeviateSquare = SourceSquare;

      if (DeviateSquare != null)
        yield return DeviateSquare;

      while (DeviateSquare != null && DeviateSquare != TargetSquare)
      {
        var Offset = DeviateSquare.AsOffset(TargetSquare);

        var GoWest = Offset.X < 0;
        var GoEast = Offset.X > 0;
        var GoNorth = Offset.Y < 0;
        var GoSouth = Offset.Y > 0;

        if ((GoNorth || GoSouth) && (GoWest || GoEast) && Chance.OneIn2.Hit())
        {
          GoEast = false;
          GoWest = false;
        }
        else if ((GoNorth || GoSouth) && !GoWest && !GoEast && Math.Abs(Offset.Y) > 1)
        {
          GoWest = DeviateSquare.X > ConstraintRegion.Left && Chance.OneIn2.Hit();
          GoEast = !GoWest && DeviateSquare.X < ConstraintRegion.Right;

          if (GoWest || GoEast)
          {
            GoNorth = false;
            GoSouth = false;
          }
        }
        else if ((GoWest || GoEast) && !GoNorth && !GoSouth && Math.Abs(Offset.X) > 1)
        {
          GoNorth = DeviateSquare.Y > ConstraintRegion.Top && Chance.OneIn2.Hit();
          GoSouth = !GoNorth && DeviateSquare.Y < ConstraintRegion.Bottom;

          if (GoNorth || GoSouth)
          {
            GoWest = false;
            GoEast = false;
          }
        }

        if (GoWest)
          DeviateSquare = DeviateSquare.Adjacent(Direction.West);
        else if (GoEast)
          DeviateSquare = DeviateSquare.Adjacent(Direction.East);
        else if (GoNorth)
          DeviateSquare = DeviateSquare.Adjacent(Direction.North);
        else if (GoSouth)
          DeviateSquare = DeviateSquare.Adjacent(Direction.South);

        if (DeviateSquare != null)
          yield return DeviateSquare;
      }
    }
    private void Tunnel(Square TunnelSquare, Barrier TunnelBarrier, Ground TunnelGround, bool IsLit)
    {
      if (TunnelSquare.Wall != null)
        TunnelSquare.SetWall(null);

      if (TunnelSquare.Floor == null)
      {
        Generator.PlaceFloor(TunnelSquare, TunnelGround);

        foreach (var AroundSquare in TunnelSquare.GetAdjacentSquares())
        {
          if (AroundSquare.IsVoid())
          {
            Generator.PlaceSolidWall(AroundSquare, TunnelBarrier, WallSegment.Pillar);

            AroundSquare.SetLit(IsLit);
          }
        }
      }

      TunnelSquare.SetLit(IsLit);
    }
    private void Tunnel(Region ConstraintRegion, Square FromSquare, Square ToSquare, Barrier TunnelBarrier, Ground TunnelGround, bool IsLit)
    {
      foreach (var TunnelSquare in DeviatingPath(FromSquare, ToSquare, ConstraintRegion))
        Tunnel(TunnelSquare, TunnelBarrier, TunnelGround, IsLit);
    }

    private Entity SelectUniqueEntity(params Entity[] EntityArray)
    {
      UniqueEntityList.AddRange(EntityArray);
      return EntityArray.GetRandom();
    }
    private Character NewEvilCharacter(Square Square, Entity Entity)
    {
      Debug.Assert(!Entity.IsGuardian, "Evil entities must NOT be guardians.");
      Debug.Assert(UniqueEntityList.Contains(Entity), "Evil entity must first be selected by NewUniqueEntity.");

      var Result = Generator.NewCharacter(Square, Entity);
      Result.SetNeutral(false);
      EvilCharacterList.Add(Result);

      return Result;
    }
    private Character NewGoodCharacter(Square Square, Entity Entity)
    {
      Debug.Assert(Entity.IsGuardian, "Good entities must be guardians.");
      Debug.Assert(UniqueEntityList.Contains(Entity), "Good entity must first be selected by NewUniqueEntity.");

      var Result = Generator.NewCharacter(Square, Entity);
      Result.SetNeutral(true);
      GoodCharacterList.Add(Result);

      return Result;
    }
    private IEnumerable<OpusOccupy> GetOccupy(int Difficulty)
    {
      return OccupyList.Where(O => O.MinimumDifficulty <= Difficulty && O.MaximumDifficulty >= Difficulty);
    }
    private Trap RandomContainerTrap(Square Square, int Difficulty)
    {
      return Generator.NewTrap(Generator.RandomContainerDevice(Square, Difficulty), Revealed: false);
    }
    private Entity RandomMercenaryEntity(OpusSection Section)
    {
      return Generator.RandomEntity(Section.MinimumDifficulty, Section.MaximumDifficulty, E => E.IsMercenary && E.IsEncounter);
    }
    private OpusOccupy NextRandomOccupy(int Difficulty)
    {
      return GetOccupy(Difficulty).GetRandomOrNull();
    }
    private Shop NextRandomShop()
    {
      if (!ShopProbability.HasChecks())
        ShopProbability.Add(Generator.GetShops(), M => M.Rarity);

      var Result = ShopProbability.GetRandom();
      ShopProbability.Remove(Result);
      return Result;
    }
    private Shrine NextRandomShrine()
    {
      if (!ShrineProbability.HasChecks())
        ShrineProbability.Add(Generator.GetShrines(), M => M.Rarity);

      var Result = ShrineProbability.GetRandom();
      ShrineProbability.Remove(Result);
      return Result;
    }
    private void SnoozeCharacter(Character Character)
    {
      if (Character != null)
        Generator.AcquireCharacterTalent(Character, Codex.Properties.sleeping);
    }
    private bool IsCorner(Square Square, Func<Square, bool> ClosedFunc, Func<Square, bool> OpenFunc)
    {
      // corner example (? doesn't matter, 1=wall, 0=floor, X=boulder).
      // ?00
      // 1X0
      // 11?

      foreach (var Corner in CornerList)
      {
        var IsClosed = true;

        foreach (var Direction in Corner.Closed)
        {
          var AdjacentSquare = Square.Adjacent(Direction);

          if (AdjacentSquare == null || !ClosedFunc(AdjacentSquare))
          {
            IsClosed = false;
            break;
          }
        }

        if (IsClosed)
        {
          var IsOpen = true;

          foreach (var Direction in Corner.Open)
          {
            var AdjacentSquare = Square.Adjacent(Direction);

            if (AdjacentSquare == null || !OpenFunc(AdjacentSquare))
            {
              IsOpen = false;
              break;
            }
          }

          return IsOpen;
        }
      }

      return false;
    }
    private void PostBuild(string Name, long ElapsedMS)
    {
      var BuildRecord = BuildRecordDictionary.GetOrAdd(Name, C => new BuildRecord(C));
      BuildRecord.ElapsedMSList.Add(ElapsedMS);
    }

    private readonly Generator Generator;
    private readonly Codex Codex;
    private readonly AmbushBuilder Ambush;
    private readonly CageBuilder Cage;
    private readonly CemeteryBuilder Cemetery;
    private readonly FinaleBuilder Finale;
    private readonly CryptBuilder Crypt;
    private readonly DragonLairBuilder DragonLair;
    private readonly FarmBuilder Farm;
    private readonly HallsBuilder Halls;
    private readonly HengeBuilder Henge;
    private readonly HordeBuilder Horde;
    private readonly KoboldFortBuilder KoboldFort;
    private readonly LakeBuilder Lake;
    private readonly MazeBuilder Maze;
    private readonly MineBuilder Mine;
    private readonly NestBuilder Nest;
    private readonly OriginBuilder Origin;
    private readonly PitBuilder Pit;
    private readonly PrintBuilder Print;
    private readonly OutpostBuilder Outpost;
    private readonly PrisonBuilder Prison;
    private readonly RavineBuilder Ravine;
    private readonly RoadBuilder Road;
    private readonly RuinsBuilder Ruins;
    //private readonly SanatoriumBuilder Sanatorium;
    private readonly StationBuilder Station;
    private readonly TownBuilder Town;
    private readonly VendorBuilder Vendor;
    private readonly ZooBuilder Zoo;
    private readonly Probability<Shop> ShopProbability;
    private readonly Probability<Shrine> ShrineProbability;
    private readonly IReadOnlyList<OpusOccupy> OccupyList;
    private readonly IReadOnlyList<OpusCorner> CornerList;
    private readonly Dictionary<string, BuildRecord> BuildRecordDictionary;
    private readonly Inv.DistinctList<Character> GoodCharacterList;
    private readonly Inv.DistinctList<Character> EvilCharacterList;
    private readonly Inv.DistinctList<Entity> UniqueEntityList;
    private Map OverlandMap;
    private Map UndergroundMap;
    private Square FinaleSquare;

    private sealed class BuildRecord
    {
      internal BuildRecord(string Name)
      {
        this.Name = Name;
        this.ElapsedMSList = new List<long>();
      }

      public readonly string Name;
      public readonly List<long> ElapsedMSList;
    }

    private abstract class Builder
    {
      public Builder(OpusMaker Maker)
      {
        this.Name = GetType().Name.ExcludeAfter("Builder");
        this.Maker = Maker;
        this.Codex = Maker.Codex;
        this.Generator = Maker.Generator;
      }

      public readonly string Name;

      protected readonly OpusMaker Maker;
      protected readonly Generator Generator;
      protected readonly Codex Codex;

      [Conditional("DEBUG")]
      protected void BuildStart()
      {
        if (Stopwatch.IsRunning)
          throw new Exception("Stopwatch cannot be started if it is already running.");

        Stopwatch.Restart();
      }
      [Conditional("DEBUG")]
      protected void BuildStop()
      {
        if (!Stopwatch.IsRunning)
          throw new Exception("Stopwatch cannot be stopped unless it is running.");

        Stopwatch.Stop();

        Maker.PostBuild(Name, Stopwatch.ElapsedMilliseconds);
      }

      private readonly Stopwatch Stopwatch = new Stopwatch();
    }

    private sealed class Variance<T>
    {
      public Variance(params T[] VariantArray)
        : this(Sequential: false, VariantArray)
      {
      }
      public Variance(bool Sequential, params T[] VariantArray)
      {
        Debug.Assert(VariantArray.Length > 0, "Variance must be provided with one or more variants.");

        this.VariantList = Sequential ? VariantArray : VariantArray.EnumerateRandom();
      }

      public IReadOnlyList<T> List => VariantList;

      public T NextVariant()
      {
        var Result = VariantList[VariantIndex];

        VariantIndex++;
        if (VariantIndex >= VariantList.Count)
          VariantIndex = 0;

        return Result;
      }

      private readonly IReadOnlyList<T> VariantList;
      private int VariantIndex;
    }

    private sealed class OriginBuilder : Builder
    {
      public OriginBuilder(OpusMaker Maker)
        : base(Maker)
      {
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        Section.UndergroundAreaName = OpusTerms.Rebirth;

        var OverlandMap = Maker.OverlandMap;
        var UndergroundMap = Maker.UndergroundMap;
        var OriginClearing = Section.SmallestClearing;
        var OverlandCircle = OriginClearing.Circle;
        var UndergroundCircle = OriginClearing.Circle.Reduce(1);

        var UndergroundSquare = UndergroundMap[OverlandCircle.X, OverlandCircle.Y];
        UndergroundMap.Level.SetTransitions(UndergroundSquare, null);

        var OverlandSquare = OverlandMap[UndergroundCircle.X, UndergroundCircle.Y];
        OverlandMap.Level.SetTransitions(null, OverlandSquare);

        var OverlandZone = OverlandMap.AddZone();
        OverlandZone.SetSpawnRestricted(true);
        foreach (var OriginSquare in OverlandMap.GetCircleInnerSquares(OverlandCircle).Union(OverlandMap.GetCircleOuterSquares(OverlandCircle)))
        {
          if (!OriginSquare.IsVoid())
          {
            OverlandZone.AddSquare(OriginSquare);
            Generator.PlaceFloor(OriginSquare, Codex.Grounds.grass);
          }
        }
        OverlandZone.SetLit(true);
        Debug.Assert(OverlandZone.HasSquares());

        Generator.PlaceFloor(OverlandSquare, Codex.Grounds.cave_floor);

        var UndergroundZone = UndergroundMap.AddZone();
        UndergroundZone.SetSpawnRestricted(true);
        foreach (var OriginSquare in UndergroundMap.GetCircleInnerSquares(UndergroundCircle))
        {
          if (OriginSquare.IsVoid())
          {
            UndergroundZone.AddSquare(OriginSquare);
            Generator.PlaceFloor(OriginSquare, Codex.Grounds.cave_floor);
          }
        }
        foreach (var OriginSquare in UndergroundMap.GetCircleOuterSquares(UndergroundCircle))
        {
          if (OriginSquare.IsVoid())
          {
            UndergroundZone.AddSquare(OriginSquare);
            Generator.PlacePermanentWall(OriginSquare, Codex.Barriers.cave_wall, WallSegment.Pillar);
          }
        }
        UndergroundZone.SetLit(true);
        Debug.Assert(UndergroundZone.HasSquares());

        Generator.PlaceFloor(UndergroundSquare, Codex.Grounds.grass);

        Generator.PlacePassage(OverlandSquare, Codex.Portals.wooden_ladder_down, UndergroundSquare);
        Generator.PlacePassage(UndergroundSquare, Codex.Portals.wooden_ladder_up, OverlandSquare);

        var CoffinSquare = UndergroundSquare.GetNeighbourSquares().GetRandomOrNull() ?? UndergroundSquare;
        Generator.PlaceFixture(CoffinSquare, Codex.Features.sarcophagus, Codex.Sanctities.Cursed, Broken: true);
        Generator.Adventure.World.SetStart(CoffinSquare);

        var OracleSquare = UndergroundSquare.Adjacent(CoffinSquare.AsDirection(UndergroundSquare));
        var OracleCharacter = Maker.NewGoodCharacter(OracleSquare, Maker.SelectUniqueEntity(Codex.Entities.Oracle));
        OracleCharacter.SetNeutral(true);
        OracleCharacter.SetResidentSquare(OracleSquare);
        Generator.PlaceCharacter(OracleSquare, OracleCharacter);

        // assassin will arrive in the overland if the oracle is slain.
        var AssassinSquare = OverlandSquare.Adjacent(CoffinSquare.AsDirection(UndergroundSquare));
        var AssassinCharacter = Maker.NewGoodCharacter(AssassinSquare, Maker.SelectUniqueEntity(Codex.Entities.Assassin_Mortimer));

        var OracleParty = Generator.NewParty(OracleCharacter);
        OracleParty.AddAlly(AssassinCharacter, Clock.Zero, Delay.Zero);

        OracleCharacter.InsertScript().Killed.Sequence.Add(Codex.Tricks.transport_candidate).SetSource(AssassinCharacter).SetTarget(AssassinSquare);

        /* // NOTE: for testing pressure plates.
        var PressurePlateSquare = UndergroundSquare.Adjacent(CoffinSquare.AsDirection(UndergroundSquare));
        Generator.PlaceTrap(PressurePlateSquare, Codex.Devices.pressure_plate, Revealed: true);
        var PressurePlateTrap = PressurePlateSquare.Trap;
        PressurePlateTrap.InsertPressSequence().Add(Codex.Tricks.barred_way, CoffinSquare);
        PressurePlateTrap.InsertReleaseSequence().Add(Codex.Tricks.cleared_way, CoffinSquare);
        */

        BuildStop();

        // TODO:
        // * Oracle dialogue.
      }
    }

    private sealed class StationBuilder : Builder
    {
      public StationBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.ConnectList = new Inv.DistinctList<Square>(StationCount);
      }

      public void Build()
      {
        BuildStart();

        var StationName = Generator.EscapedModuleTerm(OpusTerms.Station);
        var StationSite = Generator.Adventure.World.AddSite(StationName);

        var StationMap = Generator.Adventure.World.AddMap(StationName, StationSize, StationSize);
        StationMap.SetDifficulty(0);
        StationMap.SetSealed(true); // no random spawns.
        StationMap.SetTerminal(true);
        StationMap.SetAtmosphere(Codex.Atmospheres.civilisation);

        StationSite.AddLevel(1, StationMap);

        //StationMap.InsertTrigger().AddSchedule(Delay.Zero, Codex.Tricks.complete_mapping); // TODO: do we really need to auto-map the station?

        void PlaceHub(Inv.Circle Circle)
        {
          foreach (var StationSquare in StationMap.GetCircleInnerSquares(Circle))
          {
            StationSquare.SetLit(true);

            Generator.PlaceFloor(StationSquare, Codex.Grounds.dirt);
          }

          ConnectList.Add(StationMap[Circle.Origin]);
        }

        var HubSquare = StationMap.Midpoint;
        var HubCircle = new Inv.Circle(HubSquare.Point, 1);

        // first hub is at the top.
        var TopSquare = StationMap[HubCircle.Origin.X, HubCircle.Origin.Y - HubCircle.Radius - 2];
        for (var StationIndex = 0; StationIndex < StationCount - 1; StationIndex++)
        {
          var ConnectAngle = StationIndex * StationAngle;
          var ConnectSquare = TopSquare.Rotate(HubSquare.Point, ConnectAngle);
          var ConnectCircle = new Inv.Circle(ConnectSquare.Point, 1);

          PlaceHub(ConnectCircle);
        }

        // central hub is the last one.
        PlaceHub(HubCircle);

        // cave walls that will be repainted when the town portals are connected.
        Maker.RepairBoundary(StationMap, StationMap.Region, Codex.Barriers.cave_wall, Codex.Grounds.dirt, IsLit: true);

        // zones.
        foreach (var ConnectSquare in ConnectList)
        {
          var ConnectCircle = new Inv.Circle(ConnectSquare.Point, 1);

          var ConnectZone = StationMap.AddZone();
          ConnectZone.SetLit(true);
          ConnectZone.SetAccessRestricted(true);
          ConnectZone.SetSpawnRestricted(true);

          foreach (var StationSquare in StationMap.GetCircleOuterSquares(ConnectCircle))
            ConnectZone.AddSquare(StationSquare);

          foreach (var StationSquare in StationMap.GetCircleInnerSquares(ConnectCircle))
            ConnectZone.AddSquare(StationSquare);

          Debug.Assert(ConnectZone.HasSquares());
        }

        StationMap.AddArea(StationName).AddMapZones();

        BuildStop();

        // TODO:
        // * keystone kops?
      }
      public void ConnectTownPortal(Square PortalSquare, Barrier TownBarrier, Ground TownGround)
      {
        Debug.Assert(ConnectIndex < ConnectList.Count, "Unable to connect portals to more than expected number of towns.");

        var ConnectSquare = ConnectList[ConnectIndex++];
        var ConnectCircle = new Inv.Circle(ConnectSquare.Point, 1);

        Generator.PlaceFloor(PortalSquare, TownGround);
        Generator.PlacePassage(PortalSquare, Codex.Portals.transportal, null);
        PortalSquare.InsertTrigger().Add(Delay.Zero, Codex.Tricks.connecting_portal).SetTarget(ConnectSquare);

        // repaint the walls and floors to match the theme of the town.
        foreach (var StationSquare in ConnectSquare.Map.GetCircleOuterSquares(ConnectCircle))
          StationSquare.Wall?.SetBarrier(TownBarrier);

        foreach (var StationSquare in ConnectSquare.Map.GetCircleInnerSquares(ConnectCircle).Where(S => S.Floor != null))
          Generator.PlaceFloor(StationSquare, TownGround);

        Generator.RepairMap(ConnectSquare.Map, ConnectSquare.Map.Region);
      }

      private const int StationCount = 5;
      private const int StationSize = 11;
      private const int StationAngle = 360 / (StationCount - 1);
      private readonly Inv.DistinctList<Square> ConnectList;
      private int ConnectIndex;
    }

    private sealed class AmbushBuilder : Builder
    {
      public AmbushBuilder(OpusMaker Maker)
        : base(Maker)
      {
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        Section.OverlandAreaName = OpusTerms.Grotto;

        var AmbushMap = Maker.OverlandMap;
        var AmbushCircle = Section.LargestClearing.Circle;
        if (AmbushCircle.Radius <= 6)
          AmbushCircle = new Inv.Circle(Section.Region.Midpoint(), Math.Min(Section.Region.Width / 2 - 2, Section.Region.Height / 2 - 2));

        var AmbushRadius = Math.Min(7, Math.Max(5, AmbushCircle.Radius - 1));
        var AmbushX = AmbushCircle.X;
        var AmbushY = AmbushCircle.Y;
        AmbushCircle = new Inv.Circle(AmbushX, AmbushY, AmbushRadius);

        var LureSquare = AmbushMap[AmbushX, AmbushY];
        var FunnelRadius = AmbushRadius - 3;
        var FunnelCircle = new Inv.Circle(AmbushX, AmbushY, FunnelRadius);
        var FunnelSquare = LureSquare.GetNeighbourSquares(AmbushRadius - 1).GetRandomOrNull();
        var FunnelDirection = LureSquare.AsDirection(FunnelSquare);
        var EntranceSquare = FunnelSquare.Adjacent(FunnelDirection)?.Adjacent(FunnelDirection);

        // entrance may be sealed with a boulder.
        var EntranceBoulderChance = Chance.OneIn4;
        if (EntranceBoulderChance.Hit())
          Generator.PlaceBoulder(EntranceSquare, Codex.Blocks.clay_boulder, IsRigid: true);

        // outer tree wall.
        foreach (var AmbushSquare in AmbushMap.GetCircleOuterSquares(AmbushCircle))
        {
          AmbushSquare.SetLit(true);

          if (AmbushSquare.Floor == null)
            Generator.PlaceFloor(AmbushSquare, Codex.Grounds.dirt);

          if (AmbushSquare != EntranceSquare)
            Generator.PlaceWall(AmbushSquare, Codex.Barriers.tree, WallStructure.Solid, WallSegment.Pillar);
        }

        // everything is dark by default.
        foreach (var AmbushSquare in AmbushMap.GetCircleInnerSquares(AmbushCircle))
        {
          AmbushSquare.SetLit(false);

          if (AmbushSquare.Floor == null)
            Generator.PlaceFloor(AmbushSquare, Codex.Grounds.dirt);
        }

        // entrance and funnel into the lure zone.
        var FunnelZone = AmbushMap.AddZone();
        FunnelZone.AddSquare(EntranceSquare);
        foreach (var AmbushSquare in FunnelSquare.GetAroundSquares(2))
          FunnelZone.AddSquare(AmbushSquare);
        FunnelZone.SetLit(true);
        FunnelZone.SetAccessRestricted(true);
        FunnelZone.SetSpawnRestricted(true);
        Debug.Assert(FunnelZone.HasSquares());

        // funnel is the cave entrance.
        foreach (var AmbushSquare in FunnelZone.Squares)
          Generator.PlaceFloor(AmbushSquare, Codex.Grounds.cave_floor);

        // inner cave wall.
        foreach (var AmbushSquare in AmbushMap.GetCircleOuterSquares(FunnelCircle))
        {
          if (AmbushSquare != FunnelSquare && !AmbushSquare.IsAdjacent(FunnelSquare))
          {
            AmbushSquare.SetLit(true);
            AmbushSquare.SetFloor(null);
            Generator.PlaceWall(AmbushSquare, Codex.Barriers.cave_wall, WallStructure.Solid, WallSegment.Pillar);
          }
        }

        // inside cave zone to lure the player.
        var LureZone = AmbushMap.AddZone();
        foreach (var AmbushSquare in AmbushMap.GetCircleInnerSquares(FunnelCircle))
        {
          AmbushSquare.SetLit(true);
          Generator.PlaceFloor(AmbushSquare, Codex.Grounds.cave_floor);

          LureZone.AddSquare(AmbushSquare);
        }
        Debug.Assert(LureZone.HasSquares());

        // rigid boulders that are mobilised once the lure zone is triggered.
        var LureTrigger = LureZone.InsertTrigger();
        foreach (var BoulderSquare in FunnelSquare.GetFrameSquares(2))
        {
          if (BoulderSquare != EntranceSquare && BoulderSquare.Wall == null && BoulderSquare.Floor != null && BoulderSquare.AsRange(LureSquare) > FunnelRadius)
          {
            Generator.PlaceBoulder(BoulderSquare, Codex.Blocks.clay_boulder, IsRigid: true);

            LureTrigger.Add(Delay.Zero, Codex.Tricks.mobilise_boulder).SetTarget(BoulderSquare);
          }
        }

        AmbushMap.AddArea(Generator.EscapedModuleTerm(OpusTerms.Ambush)).AddZone(LureZone);

        var BoulderWater = Chance.OneIn2.Hit();
        void PlaceBoulderFill(Square FillSquare)
        {
          if (BoulderWater)
            Generator.PlaceFloor(FillSquare, Codex.Grounds.water);
          else
            Generator.PlaceTrap(FillSquare, Codex.Devices.pit, Revealed: true);
        }

        // additional boulders for extra attack points.
        foreach (var AdditionalBoulder in RandomSupport.NextRange(0, 3).NumberSeries())
        {
          var AdditionalSquare = LureSquare.GetNeighbourSquares(FunnelRadius + 1).Where(S => S.Wall != null).GetRandomOrNull();

          if (AdditionalSquare != null)
          {
            var AdditionalDirection = LureSquare.AsDirection(AdditionalSquare);

            AdditionalSquare.SetWall(null);
            Generator.PlaceFloor(AdditionalSquare, Codex.Grounds.cave_floor);
            Generator.PlaceBoulder(AdditionalSquare, Codex.Blocks.clay_boulder, IsRigid: true);

            PlaceBoulderFill(AdditionalSquare.Adjacent(AdditionalDirection.Reverse()));

            LureTrigger.Add(Delay.Zero, Codex.Tricks.mobilise_boulder).SetTarget(AdditionalSquare);

            // use illusionary trees to make chambers to hold the lurkers in their ambush spots.
            foreach (var ChamberSquare in AdditionalSquare.Adjacent(AdditionalDirection).GetFrameSquares(2))
            {
              if (ChamberSquare.Wall == null && ChamberSquare.Trap == null && ChamberSquare.AsRange(LureSquare) > FunnelRadius)
              {
                Generator.PlaceWall(ChamberSquare, Codex.Barriers.tree, WallStructure.Illusionary, WallSegment.Pillar);

                LureTrigger.Add(Delay.Zero, Codex.Tricks.dismissed_illusion).SetTarget(ChamberSquare);
              }
            }
          }
        }

        // empty locked large boxes to tempt the player.
        foreach (var Largebox in RandomSupport.NextRange(2, 4).NumberSeries())
        {
          var LargeboxSquare = AmbushMap.GetCircleInnerSquares(FunnelCircle).Where(S => S.Wall == null && !S.HasAssets() && Maker.IsCorner(S, C => C.Wall != null, O => O.Floor != null && O.Wall == null)).GetRandomOrNull();

          if (LargeboxSquare != null)
          {
            var LargeboxAsset = Generator.NewSpecificAsset(LargeboxSquare, Codex.Items.large_box);
            LargeboxAsset.Container.Locked = true;

            LargeboxSquare.PlaceAsset(LargeboxAsset);

            if (Largebox == 0)
              LargeboxAsset.Container.Stash.Add(Generator.GenerateUniqueAsset(LargeboxSquare));
          }
        }

        // decoy trapped chest to keep the player busy (contains actual items).
        var ChestAsset = Generator.NewSpecificAsset(LureSquare, Codex.Items.chest);
        ChestAsset.Container.Locked = true;
        ChestAsset.Container.Trap = Maker.RandomContainerTrap(LureSquare, Section.Distance);
        Generator.StockContainer(LureSquare, ChestAsset);
        LureSquare.PlaceAsset(ChestAsset);

        // corresponding pit for the boulders.
        foreach (var PitSquare in FunnelSquare.GetFrameSquares(1))
        {
          if (PitSquare != EntranceSquare && PitSquare.Wall == null && PitSquare.Floor != null && PitSquare.GetNeighbourSquares().Any(S => S.Boulder != null))
            PlaceBoulderFill(PitSquare);
        }

        // lurking in ambush.
        var EntityProbability = Maker.NextRandomOccupy(Section.MaximumDifficulty)?.GetProbability(Section.MinimumDifficulty, Section.MaximumDifficulty);
        var LurkParty = Generator.NewParty(Leader: null);
        var LurkChance = Chance.OneIn3;

        foreach (var AmbushSquare in AmbushMap.GetCircleOuterSquares(FunnelCircle.Expand(1)).Union(AmbushMap.GetCircleOuterSquares(AmbushCircle.Reduce(1))))
        {
          if (AmbushSquare.Wall == null && AmbushSquare.Boulder == null && AmbushSquare.Trap == null && !AmbushSquare.IsAdjacent(FunnelSquare) && (AmbushSquare.GetAdjacentSquares().Any(S => S.Boulder != null) || LurkChance.Hit()))
          {
            Generator.PlaceCharacter(AmbushSquare, EntityProbability?.GetRandomOrNull() ?? Generator.RandomEntity(AmbushSquare));
            var OccupyCharacter = AmbushSquare.Character;
            if (OccupyCharacter != null)
            {
              // NOTE: aggravation prevents entities from hiding (they need to rush out to ambush).
              OccupyCharacter.AcquireTalent(Codex.Properties.aggravation);
              Generator.RevealCharacter(OccupyCharacter);

              LurkParty.AddAlly(OccupyCharacter, Clock.Zero, Delay.Zero);

              if (OccupyCharacter.HasAcquiredTalent(Codex.Properties.sleeping))
                LureTrigger.Add(Delay.Zero, Codex.Tricks.awaken_slumber).SetTarget(AmbushSquare);
            }
          }
        }

        // actual loot for the victor.
        foreach (var Loot in RandomSupport.NextRange(2, 3).NumberSeries())
        {
          var LootSquare = AmbushMap.GetCircleOuterSquares(AmbushCircle.Reduce(1)).Where(S => S.Wall == null && S.Boulder == null && S.Trap == null && !S.HasAssets()).GetRandomOrNull();

          var LootAsset = Generator.NewSpecificAsset(LureSquare, Codex.Items.chest);
          Generator.StockContainer(LootSquare, LootAsset);
          LootSquare.PlaceAsset(LootAsset);
        }

        // place some scouts.
        foreach (var SpaceSquare in AmbushMap.GetSquares(Section.Region).Where(S => S.Wall == null && S.Floor?.Ground == Codex.Grounds.dirt && S.Character == null))
        {
          if (Chance.OneIn15.Hit())
            Generator.PlaceCharacter(SpaceSquare, EntityProbability?.GetRandomOrNull() ?? Generator.RandomEntity(SpaceSquare));
        }

        // ensure we have carved a dirt veranda around the ambush.
        Maker.RepairVeranda(AmbushMap, new Region(AmbushCircle.Expand(1)).Expand(1), Codex.Grounds.dirt, IsLit: true);

        // TODO:
        // * decoy large boxes could be mimics instead?
        // * mobilise boulders on death of any mob to stop cheaters?
        // * alternate boulder ambush scenarios such as a tree clearing with boulders in the tree line.

        BuildStop();
      }
    }

    private sealed class CageBuilder : Builder
    {
      public CageBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.CageVariance = new Variance<CageVariant>
        (
          new CageVariant
          {
            AboveName = OpusTerms.Rupture,
            BelowName = OpusTerms.Cage,
            Kind = Codex.Kinds.demon,
            MainBarrier = Codex.Barriers.hell_brick,
            DivideBarrier = Codex.Barriers.iron_bars,
            WalkGround = Codex.Grounds.obsidian_floor,
            FancyGround = Codex.Grounds.granite_floor,
            SinkGround = Codex.Grounds.lava,
            PortalDown = Codex.Portals.stone_staircase_down,
            PortalUp = Codex.Portals.stone_staircase_up,
          }
        );
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        var CageVariant = CageVariance.NextVariant();
        Section.OverlandAreaName = CageVariant.AboveName;

        var AboveMap = Maker.OverlandMap;

        // lava pools.
        foreach (var CageClearing in Section.Clearings)
        {
          foreach (var CageSquare in AboveMap.GetCircleInnerSquares(CageClearing.Circle))
            Generator.PlaceFloor(CageSquare, CageVariant.SinkGround);
        }

        foreach (var CageClearing in Section.Clearings)
        {
          var SourceSquare = AboveMap[CageClearing.Circle.Origin];

          foreach (var CageJoin in Section.Joins)
          {
            var TargetSquare = AboveMap.GetSquares(CageJoin.Region).Where(S => S.Floor?.Ground == Codex.Grounds.dirt).GetRandomOrNull();

            if (TargetSquare != null)
            {
              // dirt corridors.
              foreach (var CageSquare in Maker.DeviatingPath(SourceSquare, TargetSquare, Section.Region))
                Generator.PlaceFloor(CageSquare, Codex.Grounds.dirt);
            }
          }

          // clearing circles, full of enemies.
          foreach (var CageSquare in AboveMap.GetCircleInnerSquares(new Inv.Circle(CageClearing.Circle.Origin, 1)))
          {
            Generator.PlaceFloor(CageSquare, Codex.Grounds.dirt);

            if (CageSquare.Character == null)
              Generator.PlaceCharacter(CageSquare, CageVariant.Kind);
          }
        }

        var BelowName = Generator.EscapedModuleTerm(CageVariant.BelowName);

        if (!Generator.Adventure.World.HasSite(BelowName))
        {
          const int BelowSize = 50;

          var AboveClearing = Section.LargestClearing;
          var AboveEntranceSquare = AboveMap[AboveClearing.Circle.X, AboveClearing.Circle.Y];
          Generator.PlaceFloor(AboveEntranceSquare, CageVariant.WalkGround);

          var BelowSite = Generator.Adventure.World.AddSite(BelowName);
          var BelowMap = Generator.Adventure.World.AddMap(BelowName, BelowSize, BelowSize);
          BelowMap.SetDifficulty(Section.Distance + 1);
          BelowMap.SetTerminal(true);
          BelowMap.SetSealed(true);
          BelowMap.SetAtmosphere(Codex.Atmospheres.dungeon);

          var BelowLevel = BelowSite.AddLevel(1, BelowMap);
          var BelowIsLit = true;

          var Plan = new OpusPlanner()
          {
            StartMinSize = 9,
            StartMaxSize = 13,
            SectorMinSize = 9,
            SectorMaxSize = 13,
            CriticalDistance = 9
          }.Generate(BelowSize, BelowSize);
          Plan.PlotCriticalPaths();

          foreach (var Sector in Plan.Sectors)
          {
            var SectorRadius = Math.Min(Sector.Region.Width, Sector.Region.Height) / 2;
            var SectorCircle = new Inv.Circle(Sector.Region.Midpoint(), SectorRadius - 2);

            var SectorZone = BelowMap.AddZone();
            SectorZone.SetDifficulty(BelowMap.Difficulty + Sector.Distance);
            SectorZone.SetAccessRestricted(true);
            //SectorZone.SetSpawnRestricted(true); // map is sealed.

            foreach (var BelowSquare in BelowMap.GetCircleInnerSquares(SectorCircle).Where(S => S.Wall == null))
            {
              BelowSquare.SetLit(BelowIsLit);
              Generator.PlaceFloor(BelowSquare, CageVariant.WalkGround);

              SectorZone.AddSquare(BelowSquare);
            }

            foreach (var BelowSquare in BelowMap.GetCircleOuterSquares(SectorCircle).Where(S => S.Floor == null))
            {
              BelowSquare.SetLit(BelowIsLit);
              Generator.PlacePermanentWall(BelowSquare, CageVariant.MainBarrier, WallSegment.Pillar);

              SectorZone.AddSquare(BelowSquare);
            }

            Debug.Assert(SectorZone.HasSquares());

            if (Chance.OneIn5.Hit())
            {
              foreach (var BelowSquare in BelowMap.GetCircleOuterSquares(new Inv.Circle(SectorCircle.Origin, 1)))
                Generator.PlaceFloor(BelowSquare, CageVariant.FancyGround);
            }
            else if (Chance.OneIn5.Hit())
            {
              foreach (var CageSquare in BelowMap.GetCircleInnerSquares(SectorCircle).Where(S => S.Floor != null))
                Generator.PlaceFloor(CageSquare, CageVariant.FancyGround);

              foreach (var BelowSquare in BelowMap.GetCircleOuterSquares(new Inv.Circle(SectorCircle.Origin, 1)))
                Generator.PlaceFloor(BelowSquare, CageVariant.WalkGround);
            }
            else if (Chance.OneIn5.Hit())
            {
              foreach (var BelowRadius in SectorCircle.Radius.NumberSeries())
              {
                foreach (var BelowSquare in BelowMap[SectorCircle.Origin].GetNeighbourSquares(BelowRadius + 1).Where(S => S.Floor != null))
                  Generator.PlaceFloor(BelowSquare, CageVariant.FancyGround);
              }
            }
            else
            {
              Generator.PlaceFloor(BelowMap[SectorCircle.Origin], CageVariant.FancyGround);
            }
          }

          var TunnelRegionSet = new HashSet<Region>(); // only link two sectors one time.
          var DivideActionList = new Inv.DistinctList<Action>();

          var InitialSector = Plan.Sectors.Where(S => S.Critical).First();
          var FinalSector = Plan.Sectors.Where(S => S.Critical).Last();

          var BacktrackDictionary = new Dictionary<OpusSector, Square>();

          foreach (var Sector in Plan.Sectors)
          {
            var SourceSquare = BelowMap[Sector.Region.Midpoint()];

            if (Sector == InitialSector)
            {
              BelowLevel.SetTransitions(SourceSquare, null);

              Generator.PlacePassage(AboveEntranceSquare, CageVariant.PortalDown, SourceSquare);
              Generator.PlacePassage(SourceSquare, CageVariant.PortalUp, AboveEntranceSquare);
            }
            else if (Sector == FinalSector)
            {
              // boss and artifact.
              var BossCharacter = Maker.NewEvilCharacter(SourceSquare, Maker.SelectUniqueEntity(Codex.Entities.Juiblex, Codex.Entities.Yeenoghu, Codex.Entities.Orcus, Codex.Entities.Demogorgon));
              BossCharacter.Inventory.Carried.Add(Generator.GenerateUniqueAsset(SourceSquare)); // carry but don't outfit.
              Generator.PlaceCharacter(SourceSquare, BossCharacter);

              // TODO: clear all bars that prevent backtracking.
              var BossScript = BossCharacter.InsertScript();
              BossScript.Killed.Sequence.Add(Codex.Tricks.cleared_way).SetTarget(SourceSquare);
              foreach (var DivideSquare in BacktrackDictionary.Values)
                BossScript.Killed.Sequence.Add(Codex.Tricks.cleared_way).SetTarget(DivideSquare);

              Generator.PlacePermanentWall(SourceSquare, CageVariant.DivideBarrier, WallSegment.Pillar);
              Generator.PlacePassage(SourceSquare, Codex.Portals.transportal, AboveEntranceSquare);
            }
            else if (!Sector.Critical && Sector.Links.Count == 1)
            {
              // terminal non-critical path.
              var ChestAsset = Generator.NewSpecificAsset(SourceSquare, Codex.Items.chest);
              ChestAsset.Container.Locked = true;
              ChestAsset.Container.Trap = Maker.RandomContainerTrap(SourceSquare, BelowMap.Difficulty + Sector.Distance);
              Generator.StockContainer(SourceSquare, ChestAsset);
              SourceSquare.PlaceAsset(ChestAsset);

              foreach (var ChestSquare in BelowMap.GetCircleOuterSquares(new Inv.Circle(SourceSquare.Point, 1)))
                Generator.PlaceFloor(ChestSquare, CageVariant.SinkGround);
            }

            foreach (var Link in Sector.Links)
            {
              if (TunnelRegionSet.Add(Link.Region))
              {
                var TargetSquare = BelowMap[Link.Sector.Region.Midpoint()];

                var LinkSquare = BelowMap.GetSquares(Link.Region).Where(S => S.Wall != null || S.Floor == null).GetRandomOrNull();

                var TunnelSquareList = new Inv.DistinctList<Square>();

                if (LinkSquare == null)
                {
                  Debug.Fail("Should not get to this code branch.");
                  TunnelSquareList.AddRange(Maker.WalkingPath(SourceSquare, TargetSquare));
                }
                else
                {
                  TunnelSquareList.AddRange(Maker.WalkingPath(SourceSquare, LinkSquare));
                  TunnelSquareList.AddRange(Maker.WalkingPath(LinkSquare, TargetSquare).Except(TunnelSquareList));
                }

                var TunnelGround = Sector != InitialSector && Chance.OneIn10.Hit() ? CageVariant.SinkGround : CageVariant.WalkGround;

                foreach (var TunnelSquare in TunnelSquareList)
                  Maker.Tunnel(TunnelSquare, CageVariant.MainBarrier, TunnelGround, BelowIsLit);

                var DivideSquare = TunnelSquareList.Where(S => S.Wall == null && S.Passage == null && S.Floor != null && S.IsFlat()).FirstOrDefault() ?? TunnelSquareList.Where(S => S.Wall == null && S.Passage == null && S.Floor != null).FirstOrDefault();
                var BacktrackSquare = TunnelSquareList.Where(S => S.Wall == null && S.Passage == null && S.Floor != null && S.IsFlat()).LastOrDefault() ?? TunnelSquareList.Where(S => S.Wall == null && S.Passage == null && S.Floor != null).LastOrDefault();

                if (DivideSquare == null || BacktrackSquare == null)
                {
                  Debug.Fail("Divide and Backtrack square must be found.");
                }
                else
                {
                  if (Sector.Critical && Link.Sector.Critical)
                  {
                    Debug.Assert(!BacktrackDictionary.Values.Contains(BacktrackSquare), "Backtrack square cannot be designated to more than one sector.");

                    BacktrackDictionary.Add(Sector, BacktrackSquare);
                  }

                  // initial sector has no divider or enemy.
                  if (SourceSquare.Passage == null)
                  {
                    if (Sector.Critical)
                      Generator.PlacePermanentWall(DivideSquare, CageVariant.DivideBarrier, WallSegment.Pillar);

                    DivideActionList.Add(() =>
                    {
                      // walking on this square, after it has been cleared, will trigger bars over the previous area, preventing backtracking.
                      if (Sector.Critical && Link.Sector.Critical)
                      {
                        var BackLink = Sector.Links.Find(L => L.Sector.Critical && L.Sector != Link.Sector);

                        if (BackLink != null)
                          DivideSquare.RequireTrigger().Add(Delay.Zero, Codex.Tricks.barred_way).SetTarget(BacktrackDictionary[BackLink.Sector]);
                      }

                      var EnemyCharacter = SourceSquare.Character;

                      if (EnemyCharacter == null)
                      {
                        Generator.PlaceCharacter(SourceSquare, CageVariant.Kind);
                        EnemyCharacter = SourceSquare.Character;

                        if (EnemyCharacter == null)
                        {
                          Generator.PlaceCharacter(SourceSquare); // can't place the expected kind due to difficulty level probably.
                          EnemyCharacter = SourceSquare.Character;
                        }

                        if (Sector.Critical)
                          EnemyCharacter?.InsertScript();
                      }

                      if (Sector.Critical)
                      {
                        if (EnemyCharacter == null)
                        {
                          Debug.Fail("Enemy must be generated or there will be a permanent wall that cannot be removed.");
                          SourceSquare.InsertTrigger().Add(Delay.Zero, Codex.Tricks.cleared_way).SetTarget(DivideSquare);
                        }
                        else
                        {
                          // clear the dividing barrier when the sector enemy is killed.
                          EnemyCharacter.Script.Killed.Sequence.Add(Codex.Tricks.cleared_way).SetTarget(DivideSquare);

                          // add an item for loot.
                          EnemyCharacter.Inventory.Carried.Add(Generator.NewRandomAsset(SourceSquare));
                          Generator.OutfitCharacter(EnemyCharacter);
                        }
                      }
                    });
                  }
                }
              }
            }
          }

          Debug.Assert(BacktrackDictionary.Count == Plan.Sectors.Count(S => S.Critical) - 1, "All critical path sectors must have an associated divide square (except the final sector).");

          // dividers and enemies.
          DivideActionList.ForEach(A => A());

          // traps.
          foreach (var Sector in Plan.Sectors)
          {
            foreach (var BelowSquare in BelowMap.GetSquares(Sector.Region).Where(S => S.Passage == null && S.Character == null && S.Trap == null && S.Floor != null && S.Floor.Ground != CageVariant.SinkGround && S.GetAdjacentSquares().Count(S => S.Wall?.Barrier == CageVariant.MainBarrier) == 1))
              Generator.PlaceTrap(BelowSquare, Revealed: false);
          }

          // prepare the map.
          Generator.RepairMap(BelowMap, BelowMap.Region);

          BelowMap.AddArea(BelowName).AddMapZones();
        }

        BuildStop();

        // TODO:
        // * 
      }

      private readonly Variance<CageVariant> CageVariance;

      private sealed class CageVariant
      {
        public string AboveName;
        public string BelowName;
        public Kind Kind;
        public Barrier MainBarrier;
        public Barrier DivideBarrier;
        public Ground SinkGround;
        public Ground WalkGround;
        public Ground FancyGround;
        public Portal PortalDown;
        public Portal PortalUp;
      }
    }

    private sealed class CemeteryBuilder : Builder
    {
      public CemeteryBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.CemeteryVariance = new Variance<CemeteryVariant>
        (
          new CemeteryVariant
          {
            Name = OpusTerms.Cemetery
          }
        );
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        var CemeteryVariant = CemeteryVariance.NextVariant();
        Section.OverlandAreaName = CemeteryVariant.Name;

        var CemeteryMap = Maker.OverlandMap;
        var CemeteryClearing = Section.LargestClearing;
        var CemeteryX = CemeteryClearing.Circle.X;
        var CemeteryY = CemeteryClearing.Circle.Y;
        var CemeteryRadius = RandomSupport.NextRange(4, Math.Max(4, CemeteryClearing.Circle.Radius - 2));
        var CemeteryCircle = new Inv.Circle(CemeteryX, CemeteryY, CemeteryRadius);
        var MiddleSquare = CemeteryMap[CemeteryX, CemeteryY];

        // crypt.
        var CryptSize = Math.Max(1, CemeteryRadius / 2);
        var CryptRegion = new Region(CemeteryX - CryptSize, CemeteryY - CryptSize, CemeteryX + CryptSize, CemeteryY + CryptSize);
        var CryptBorder = (CemeteryRadius - CryptSize) > 2;

        Generator.PlaceSolidWallFrame(CemeteryMap, Codex.Barriers.stone_wall, CryptRegion);
        Generator.PlaceFloorFill(CemeteryMap, Codex.Grounds.stone_floor, CryptRegion);
        var CryptZone = CemeteryMap.AddZone();
        CryptZone.AddRegion(CryptRegion);
        CryptZone.SetLit(false);
        Debug.Assert(CryptZone.HasSquares());

        var CryptTrigger = CryptZone.InsertTrigger();

        // outer walls of crypt should be lit.
        foreach (var CryptSquare in MiddleSquare.GetFrameSquares(CryptSize))
          CryptSquare.SetLit(true);

        // four doors into the crypt building.
        foreach (var CryptSquare in MiddleSquare.GetNeighbourSquares(CryptSize))
          Generator.PlaceClosedVerticalDoor(CryptSquare, Codex.Gates.wooden_door, SecretBarrier: Codex.Barriers.stone_wall);

        // decorate the border between the crypt and the graves with statues.
        if (CryptBorder)
        {
          foreach (var BorderSquare in CemeteryMap.GetFrameSquares(CryptRegion.Expand(1)))
          {
            if (BorderSquare.GetAdjacentSquares().Count(S => S.Wall != null) == 1)
            {
              Generator.PlaceBoulder(BorderSquare, Codex.Blocks.stone_boulder, IsRigid: false).SetBlock(Codex.Blocks.statue); // TODO: don't actually want a prisoner statue, Generator API needs to be improved to allow non-character statues.

              foreach (var AlcoveSquare in BorderSquare.GetAdjacentSquares().Where(S => S.AsRange(MiddleSquare) <= CryptSize + 1))
                Generator.PlaceSolidWall(AlcoveSquare, Codex.Barriers.stone_wall, WallSegment.Pillar);
            }
          }
        }

        // grave plot.
        var CryptGraveSquareList = new Inv.DistinctList<Square>();

        foreach (var CemeterySquare in CemeteryMap.GetCircleInnerSquares(CemeteryCircle))
        {
          if (CemeterySquare.Fixture == null && CemeterySquare.Trap == null && CemeterySquare.Door == null && CemeterySquare.Wall == null)
          {
            var MiddleStrip = MiddleSquare.X == CemeterySquare.X || MiddleSquare.Y == CemeterySquare.Y;

            if (CemeterySquare.Floor?.Ground == Codex.Grounds.dirt)
            {
              // outside crypt.
              CemeterySquare.SetLit(true);

              if (CryptBorder && CemeterySquare.AsRange(MiddleSquare) <= CryptSize + 1)
                continue;

              Debug.Assert(Generator.CanPlaceTrap(CemeterySquare));
              Debug.Assert(Generator.CanPlaceFeature(CemeterySquare));

              if (!MiddleStrip && Chance.TwoIn3.Hit())
              {
                Generator.PlaceFloor(CemeterySquare, Codex.Grounds.grass);
                Generator.PlaceFixture(CemeterySquare, Codex.Features.grave);

                CryptGraveSquareList.Add(CemeterySquare);
              }
              else if (!MiddleStrip)
              {
                Generator.PlaceTrap(CemeterySquare, Codex.Devices.pit, Revealed: true);

                if (Chance.OneIn3.Hit())
                {
                  var CorpseEntity = Generator.RandomCorpse(Section.MinimumDifficulty, Section.MaximumDifficulty, Codex.Items.animal_corpse);
                  if (CorpseEntity != null)
                    CemeterySquare.PlaceAsset(Generator.CorpseCharacter(Generator.NewCharacter(CemeterySquare, CorpseEntity)));
                }
              }
            }
          }
        }

        // four sarcophagi around the stairs.
        foreach (var CryptSquare in MiddleSquare.GetAdjacentSquares().Where(S => S.Floor?.Ground == Codex.Grounds.stone_floor && !(MiddleSquare.X == S.X || MiddleSquare.Y == S.Y)))
          Generator.PlaceFixture(CryptSquare, Codex.Features.sarcophagus);

        // waves of returning undead.
        var CryptWaveNumberDice = 1.d3();
        foreach (var CryptWave in CryptWaveNumberDice.Roll().NumberSeries())
        {
          // delay between waves.
          var WaveDelay = CryptWave > 1 ? 60 : 0;

          CryptGraveSquareList.Shuffle(); // so it's not predictable which grave will spawn first.
          foreach (var CryptGraveSquare in CryptGraveSquareList)
            CryptTrigger.Add(Delay.FromTurns(RandomSupport.NextRange(10, 20) + WaveDelay), Codex.Tricks.returning_undead).SetTarget(CryptGraveSquare);
        }

        Maker.Crypt.Build(Section, CemeteryMap);

        // TODO:
        // * 

        BuildStop();
      }

      private readonly Variance<CemeteryVariant> CemeteryVariance;

      private sealed class CemeteryVariant
      {
        public string Name;
      }
    }

    private sealed class CryptBuilder : Builder
    {
      public CryptBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.CryptVariance = new Variance<CryptVariant>
        (
          new CryptVariant
          {
            Name = OpusTerms.Crypt,
            Barrier = Codex.Barriers.stone_wall,
            Ground = Codex.Grounds.stone_floor,
            Gate = Codex.Gates.wooden_door,
            PortalUp = Codex.Portals.stone_staircase_up,
            PortalDown = Codex.Portals.stone_staircase_down,
            Boss = Codex.Entities.Deliarne // TODO: too powerful: Codex.Entities.Vecna, Codex.Entities.Dark_Lord
          }
        );
      }

      public void Build(OpusSection Section, Map CemeteryMap)
      {
        var CryptClearing = Section.LargestClearing;
        var CryptVariant = CryptVariance.NextVariant();
        var CryptName = Generator.EscapedModuleTerm(CryptVariant.Name);

        if (!Generator.Adventure.World.HasSite(CryptName))
        {
          BuildStart();

          const int CryptSize = 24;

          var CryptKindArray = Codex.Kinds.Undead;
          var BossTriangle = new Inv.Triangle(new Inv.Point(CryptSize / 2, CryptSize / 2), new Inv.Point(CryptSize / 4, CryptSize - 2), new Inv.Point(CryptSize / 4 * 3, CryptSize - 2));

          var OverlandEntranceSquare = CemeteryMap[CryptClearing.Circle.X, CryptClearing.Circle.Y];
          var CryptEntranceSquare = OverlandEntranceSquare;

          var CryptCount = RandomSupport.NextNumber(5, 6);
          var CryptSite = Generator.Adventure.World.AddSite(CryptName);
          for (var CryptIndex = 1; CryptIndex <= CryptCount; CryptIndex++)
          {
            var CryptMap = Generator.Adventure.World.AddMap(CryptName + " " + CryptIndex, CryptSize, CryptSize);
            CryptMap.SetDifficulty(Section.Distance + CryptIndex);
            CryptMap.SetSealed(true); // no random spawns.
            CryptMap.SetTerminal(CryptIndex == CryptCount);
            CryptMap.SetAtmosphere(Codex.Atmospheres.dungeon);

            var CryptLevel = CryptSite.AddLevel(CryptIndex, CryptMap);
            var CryptIsLit = !Chance.Of(CryptIndex, CryptCount * 2).Hit();

            var TriangleCount = CryptMap.Region.Height / 4;
            var TriangleList = new Inv.DistinctList<Inv.Triangle>(TriangleCount);

            var CryptRegion = CryptMap.Region.Reduce(1);

            foreach (var TriangleIndex in TriangleCount.NumberSeries())
            {
              var TriangleAttempt = 0;
              do
              {
                var TriangleRadius = RandomSupport.NextRange(2, 4);
                var TriangleX = RandomSupport.NextRange(CryptRegion.Left + TriangleRadius + 1, CryptRegion.Right - TriangleRadius - 1);
                var TriangleY = RandomSupport.NextRange(CryptRegion.Top + TriangleRadius + 1, CryptRegion.Bottom - TriangleRadius - 1);
                var VertexCircle = new Inv.Circle(TriangleX, TriangleY, TriangleRadius - 1); // so we can get the outline of the circle.

                // first vertex of the triangle is a random point on the circle outline, the other two vertices are found by rotating that point around the origin by 90 and 180 degrees.
                var VertexASquare = CryptMap.GetCircleOuterSquares(VertexCircle).GetRandomOrNull();
                var VertexBSquare = VertexASquare.Rotate(VertexCircle.Origin, 90);
                var VertexCSquare = VertexASquare.Rotate(VertexCircle.Origin, 180);

                var Triangle = new Inv.Triangle(new Inv.Point(VertexASquare.X, VertexASquare.Y), new Inv.Point(VertexBSquare.X, VertexBSquare.Y), new Inv.Point(VertexCSquare.X, VertexCSquare.Y));

                if (!TriangleList.Any(C => C == Triangle))
                {
                  TriangleList.Add(Triangle);
                  break;
                }
              }
              while (TriangleAttempt++ < 3);
            }

            // place all the chambers (random order of Triangles should give the most different generation).
            foreach (var Triangle in TriangleList)
            {
              foreach (var RoomSquare in CryptMap.GetTriangleOuterSquares(Triangle).Where(S => S.IsVoid()))
              {
                RoomSquare.SetLit(CryptIsLit);
                Generator.PlaceSolidWall(RoomSquare, CryptVariant.Barrier, WallSegment.Pillar);
              }

              foreach (var RoomSquare in CryptMap.GetTriangleInnerSquares(Triangle).Where(S => S.IsVoid()))
              {
                RoomSquare.SetLit(CryptIsLit);
                Generator.PlaceFloor(RoomSquare, CryptVariant.Ground);
              }
            }

            // establish tunnels to connect all Triangles.
            var PreviousTriangle = TriangleList[0];
            foreach (var NextTriangle in TriangleList.Skip(1))
            {
              var PreviousMidpoint = PreviousTriangle.Midpoint();
              var NextMidpoint = NextTriangle.Midpoint();

              Maker.Tunnel(CryptMap.Region, CryptMap[PreviousMidpoint.X, PreviousMidpoint.Y], CryptMap[NextMidpoint.X, NextMidpoint.Y], CryptVariant.Barrier, CryptVariant.Ground, CryptIsLit);

              PreviousTriangle = NextTriangle;
            }

            // sort the Triangles so the connecting tunnels are more sensible and so the first and last Triangle are far enough apart and diagonally opposite each other.
            var FlipHorizontal = +1;
            var FlipVertical = +1;
            TriangleList.Sort((A, B) =>
            {
              var AMidpoint = A.Midpoint();
              var BMidpoint = B.Midpoint();

              return ((AMidpoint.X * FlipHorizontal) + (AMidpoint.Y * FlipVertical)).CompareTo((BMidpoint.X * FlipHorizontal) + (BMidpoint.Y * FlipVertical));
            });

            // entrance point.
            var StartPoint = TriangleList.First().Midpoint();
            var EnterSquare = CryptMap[StartPoint.X, StartPoint.Y];

            // make connections where possible.
            Maker.ConnectSquares(CryptMap.GetSquares(CryptRegion), EnterSquare, PunchSquare =>
            {
              var PunchBarrier = PunchSquare.Wall.Barrier;
              PunchSquare.SetWall(null);
              Generator.PlaceFloor(PunchSquare, PunchBarrier.Underlay);

              PunchSquare.SetLit(CryptIsLit);
            });

            if (CryptIndex == CryptCount)
            {
              TriangleList.Add(BossTriangle);

              foreach (var CryptSquare in CryptMap.GetTriangleOuterSquares(BossTriangle))
              {
                CryptSquare.SetFloor(null);
                Generator.PlaceSolidWall(CryptSquare, CryptVariant.Barrier, WallSegment.Pillar);

                CryptSquare.SetLit(CryptIsLit);
              }

              foreach (var CryptSquare in CryptMap.GetTriangleInnerSquares(BossTriangle))
              {
                CryptSquare.SetWall(null);
                Generator.PlaceFloor(CryptSquare, CryptVariant.Ground);

                CryptSquare.SetLit(CryptIsLit);
              }

              // throne door.
              var DoorSquare = CryptMap[BossTriangle.VertexA.X, BossTriangle.VertexA.Y];
              Maker.Tunnel(CryptMap.Region, CryptMap[StartPoint.X, StartPoint.Y], DoorSquare, CryptVariant.Barrier, CryptVariant.Ground, CryptIsLit);
              if (DoorSquare.Floor != null && IsWall(DoorSquare, Direction.West) && IsWall(DoorSquare, Direction.East) && IsFloor(DoorSquare, Direction.North) && IsFloor(DoorSquare, Direction.South))
                Generator.PlaceLockedVerticalDoor(DoorSquare, Codex.Gates.gold_door, CryptVariant.Barrier);

              // throne chair.
              var ThroneSquare = CryptMap[BossTriangle.VertexA.X, BossTriangle.VertexC.Y];
              Generator.PlaceFixture(ThroneSquare, Codex.Features.throne);

              var BossCharacter = Maker.NewEvilCharacter(ThroneSquare, Maker.SelectUniqueEntity(CryptVariant.Boss));
              Generator.PlaceCharacter(ThroneSquare, BossCharacter);
              Generator.AcquireUnique(ThroneSquare, BossCharacter, Codex.Qualifications.master);

              // library of books and scrolls.
              void LootChest(Square Square, Stock Stock)
              {
                var ChestAsset = Generator.NewSpecificAsset(Square, Codex.Items.chest);
                ChestAsset.Container.Locked = true;
                ChestAsset.Container.Trap = Maker.RandomContainerTrap(Square, Section.Distance);

                foreach (var LootIndex in RandomSupport.NextRange(2, 3).NumberSeries())
                  ChestAsset.Container.Stash.Add(Generator.NewRandomAsset(Square, Stock));

                Square.PlaceAsset(ChestAsset);
              }

              LootChest(ThroneSquare.Adjacent(Direction.West), Codex.Stocks.book);
              LootChest(ThroneSquare.Adjacent(Direction.East), Codex.Stocks.scroll);
            }

            // exit point.
            var FinishPoint = TriangleList.Last().Midpoint();
            var ExitSquare = CryptMap[FinishPoint.X, FinishPoint.Y];

            // repair weird gaps.
            Maker.RepairGaps(CryptMap, CryptRegion, CryptVariant.Barrier, CryptIsLit);

            // ensure access points.
            if (EnterSquare.Wall != null || EnterSquare.Floor == null)
            {
              EnterSquare.SetWall(null);
              Generator.PlaceFloor(EnterSquare, CryptVariant.Ground);
            }

            if (ExitSquare.Wall != null || ExitSquare.Floor == null)
            {
              ExitSquare.SetWall(null);
              Generator.PlaceFloor(ExitSquare, CryptVariant.Ground);
            }

            // replace unreachable squares with a wall, so the following call to RepairMap will remove them entirely as necessary.
            foreach (var UnreachableSquare in Maker.FindUnreachableSquares(CryptMap, CryptMap.Region, EnterSquare))
            {
              if (UnreachableSquare == EnterSquare || UnreachableSquare == ExitSquare)
                continue;

              UnreachableSquare.SetFloor(null);
              Generator.PlaceSolidWall(UnreachableSquare, CryptVariant.Barrier, WallSegment.Pillar);

              UnreachableSquare.SetLit(CryptIsLit);
            }

            CryptLevel.SetTransitions(EnterSquare, CryptIndex == CryptCount ? null : ExitSquare);

            void EnsurePassage(Square FromSquare, Portal Portal, Square ToSquare)
            {
              Debug.Assert(FromSquare.Floor != null && FromSquare.Wall == null && FromSquare.Fixture == null);
              Debug.Assert(ToSquare.Floor != null && ToSquare.Wall == null && ToSquare.Fixture == null);

              Generator.PlacePassage(FromSquare, Portal, ToSquare);
            }

            EnsurePassage(CryptEntranceSquare, CryptVariant.PortalDown, EnterSquare);
            EnsurePassage(EnterSquare, CryptVariant.PortalUp, CryptEntranceSquare);
            Generator.PlacePassage(ExitSquare, CryptIndex == CryptCount ? Codex.Portals.transportal : CryptVariant.PortalDown, null); // just hold the down passage square.

            if (CryptCount > 1)
            {
              if (CryptIndex == CryptCount)
                Generator.PlacePassage(ExitSquare, Codex.Portals.transportal, OverlandEntranceSquare); // escape portal from the crypt.
              else
                CryptEntranceSquare = ExitSquare;
            }

            // skeletons in closets.
            foreach (var CryptSquare in CryptMap.GetSquares(CryptRegion).Where(S => S.Floor != null && S.GetAdjacentSquares().Count(S => S.Wall != null) == 7))
            {
              var DoorSquare = CryptSquare.GetAdjacentSquares().Where(S => S.Floor != null).GetRandomOrNull();

              if (DoorSquare != null)
              {
                Generator.PlaceCharacter(CryptSquare, Codex.Entities.skeleton);
                Generator.PlaceLockedVerticalDoor(DoorSquare, CryptVariant.Gate, SecretBarrier: CryptVariant.Barrier);
              }
            }

            bool IsWall(Square Square, Direction Direction)
            {
              var AdjacentSquare = Square.Adjacent(Direction);
              return AdjacentSquare != null && AdjacentSquare.Wall != null;
            }
            bool IsFloor(Square Square, Direction Direction)
            {
              var AdjacentSquare = Square.Adjacent(Direction);
              return AdjacentSquare != null && AdjacentSquare.Floor != null && AdjacentSquare.Door == null && AdjacentSquare.Boulder == null && AdjacentSquare.Character == null && AdjacentSquare.Fixture == null && AdjacentSquare.Wall == null;
            }
            bool IsVoid(Square Square, Direction Direction)
            {
              var AdjacentSquare = Square.Adjacent(Direction);
              return AdjacentSquare != null && AdjacentSquare.IsVoid();
            }
            void SurroundWall(Square Square, Barrier Barrier)
            {
              foreach (var WallSquare in Square.GetAdjacentSquares().Where(S => S.IsVoid()))
              {
                WallSquare.SetLit(CryptIsLit);

                Generator.PlaceSolidWall(WallSquare, Barrier, WallSegment.Pillar);
              }
            }
            void Denizen(Square Square)
            {
              Generator.PlaceCharacter(Square, CryptKindArray);

              // 50% of denizens carry an item because there's no items generated on the floor.
              var RecessCharacter = Square.Character;
              if (RecessCharacter != null && Chance.OneIn2.Hit())
              {
                RecessCharacter.Inventory.Carried.Add(Generator.NewRandomAsset(Square));
                Generator.OutfitCharacter(RecessCharacter);
              }
            }

            var RecessBoulderSquareList = new Inv.DistinctList<Square>();
            var RecessSarcophagusSquareList = new Inv.DistinctList<Square>();
            var RecessStatueSquareList = new Inv.DistinctList<Square>();

            void Recess(Square RecessSquare, Direction Direction)
            {
              RecessSquare.SetLit(CryptIsLit);
              RecessSquare.SetWall(null);
              Generator.PlaceFloor(RecessSquare, CryptVariant.Ground);
              SurroundWall(RecessSquare, CryptVariant.Barrier);

              if (Chance.OneIn3.Hit() && !RecessSquare.GetAdjacentSquares().Any(S => S.Trap != null))
              {
                Generator.PlaceTrap(RecessSquare, Revealed: false);
              }
              else if (Chance.OneIn3.Hit())
              {
                var Broken = Chance.OneIn2.Hit();
                Generator.PlaceFixture(RecessSquare, Codex.Features.sarcophagus).SetBroken(Broken);

                if (Broken)
                  Generator.PlaceRandomAsset(RecessSquare);
                else
                  RecessSarcophagusSquareList.Add(RecessSquare);
              }
              else if (Chance.OneIn3.Hit())
              {
                if (Chance.OneIn2.Hit())
                {
                  Generator.PlaceBoulder(RecessSquare, Codex.Blocks.statue, IsRigid: true, Generator.RandomEntity(RecessSquare, CryptKindArray));
                  RecessStatueSquareList.Add(RecessSquare);
                }
                else if (Chance.OneIn2.Hit())
                {
                  Generator.PlaceBoulder(RecessSquare, Codex.Blocks.stone_boulder, IsRigid: true).SetBlock(Codex.Blocks.statue);
                }
                else
                {
                  Generator.PlaceBoulder(RecessSquare, Codex.Blocks.stone_boulder, IsRigid: true).SetBlock(Codex.Blocks.statue);
                  RecessBoulderSquareList.Add(RecessSquare);

                  var SecretSquare = RecessSquare.Adjacent(Direction);
                  if (SecretSquare?.Wall != null && (SecretSquare.Adjacent(Direction)?.IsVoid() ?? false))
                  {
                    Generator.PlaceFloor(SecretSquare, CryptVariant.Ground);
                    SurroundWall(SecretSquare, CryptVariant.Barrier);

                    var BeyondSquare = SecretSquare.Adjacent(Direction);
                    if (BeyondSquare?.Wall != null && (BeyondSquare.Adjacent(Direction)?.IsVoid() ?? false))
                    {
                      SecretSquare.Wall.SetStructure(WallStructure.Illusionary);

                      BeyondSquare.SetWall(null);
                      Generator.PlaceFloor(BeyondSquare, CryptVariant.Ground);
                      SurroundWall(BeyondSquare, CryptVariant.Barrier);

                      Denizen(BeyondSquare);
                      Generator.DropCoins(BeyondSquare, CryptIndex.d100().Roll());
                    }
                    else
                    {
                      SecretSquare.SetWall(null);

                      Denizen(SecretSquare);
                    }
                  }
                }
              }
              else
              {
                Denizen(RecessSquare);
              }
            }

            foreach (var CryptSquare in CryptMap.GetSquares(CryptRegion).Where(S => S.Wall != null))
            {
              if (IsWall(CryptSquare, Direction.North) && IsWall(CryptSquare, Direction.South))
              {
                if (IsVoid(CryptSquare, Direction.West) && IsFloor(CryptSquare, Direction.East))
                  Recess(CryptSquare, Direction.West);
                else if (IsVoid(CryptSquare, Direction.East) && IsFloor(CryptSquare, Direction.West))
                  Recess(CryptSquare, Direction.East);
              }
              else if (IsWall(CryptSquare, Direction.West) && IsWall(CryptSquare, Direction.East))
              {
                if (IsVoid(CryptSquare, Direction.North) && IsFloor(CryptSquare, Direction.South))
                  Recess(CryptSquare, Direction.North);
                else if (IsVoid(CryptSquare, Direction.South) && IsFloor(CryptSquare, Direction.North))
                  Recess(CryptSquare, Direction.South);
              }
            }

            void Pillar(Square Square)
            {
              if (Square.Fixture == null && Square.Passage == null && Square.Boulder == null)
              {
                Square.SetFloor(null);
                Generator.PlaceSolidWall(Square, CryptVariant.Barrier, WallSegment.Pillar);
              }
            }

            var FourSquare = CryptMap.GetSquares(CryptRegion).Where(S => S.GetAroundSquares(2).All(S => S.Floor != null && S.Passage == null)).GetRandomOrNull();
            if (FourSquare == null)
            {
              var ThreeSquare = CryptMap.GetSquares(CryptRegion).Where(S => S.GetAroundSquares(1).All(S => S.Floor != null && S.Passage == null)).GetRandomOrNull();
              if (ThreeSquare != null)
              {
                Generator.PlaceFixture(ThreeSquare, Codex.Features.sarcophagus);
                RecessSarcophagusSquareList.Add(ThreeSquare);

                foreach (var CandleSquare in ThreeSquare.GetNeighbourSquares())
                {
                  if (Chance.OneIn2.Hit())
                    Generator.PlaceSpecificAsset(CandleSquare, Codex.Items.wax_candle);
                }
              }
            }
            else
            {
              if (FourSquare.Fixture == null && FourSquare.Passage == null && FourSquare.Boulder == null)
              {
                Generator.PlaceFixture(FourSquare, Codex.Features.sarcophagus);
                RecessSarcophagusSquareList.Add(FourSquare);
              }

              Pillar(FourSquare.Adjacent(Direction.NorthWest));
              Pillar(FourSquare.Adjacent(Direction.NorthEast));
              Pillar(FourSquare.Adjacent(Direction.SouthWest));
              Pillar(FourSquare.Adjacent(Direction.SouthEast));
            }

            if (RecessSarcophagusSquareList.Count > 0 && (RecessBoulderSquareList.Count > 0 || RecessStatueSquareList.Count > 0))
            {
              foreach (var RecessSarcophagusSquare in RecessSarcophagusSquareList)
              {
                var RecessTrigger = RecessSarcophagusSquare.InsertTrigger();

                foreach (var RecessBoulderSquare in RecessBoulderSquareList)
                  RecessTrigger.Add(Delay.Zero, Codex.Tricks.mobilise_boulder).SetTarget(RecessBoulderSquare);

                foreach (var RecessStatueSquare in RecessStatueSquareList)
                  RecessTrigger.Add(Delay.Zero, Codex.Tricks.living_statue).SetTarget(RecessStatueSquare);
              }
            }

            if (CryptIndex == CryptCount)
            {
              var BossZone = CryptMap.AddZone();
              BossZone.AddTriangle(BossTriangle);
              Debug.Assert(BossZone.HasSquares());

              var BossTrigger = BossZone.InsertTrigger();

              // mercenary corpses for the lich to animate.
              foreach (var CorpseIndex in RandomSupport.NextRange(3, 5).NumberSeries())
              {
                var MercenaryEntity = Maker.RandomMercenaryEntity(Section);
                if (MercenaryEntity != null)
                {
                  var CorpseSquare = CryptMap.GetTriangleInnerSquares(BossTriangle).Where(S => S.Floor != null && S.Stash == null).GetRandomOrNull();
                  if (CorpseSquare != null)
                  {
                    // containing a corpse of a mercenary and their gear.
                    var MercenaryCharacter = Generator.NewCharacter(CorpseSquare, MercenaryEntity);
                    MercenaryCharacter.SetNeutral(false);

                    foreach (var MercenaryAsset in MercenaryCharacter.Inventory.RemoveAllAssets())
                      CorpseSquare.PlaceAsset(MercenaryAsset);

                    CorpseSquare.PlaceAsset(Generator.CorpseCharacter(MercenaryCharacter));

                    BossTrigger.Add(Delay.FromTurns(20), Codex.Tricks.living_dead).SetTarget(CorpseSquare);
                  }
                }
              }
            }

            Generator.RepairMap(CryptMap, CryptMap.Region);

            CryptMap.AddArea(CryptName).AddMapZones();

#if DEBUG
            foreach (var FrameSquare in CryptMap.GetFrameSquares(CryptMap.Region))
              Debug.Assert(FrameSquare.Floor == null, $"{CryptMap.Name} must not have any floor on the outer boundary.");
#endif
          }

          BuildStop();
        }

        // TODO:
        // *
      }

      private readonly Variance<CryptVariant> CryptVariance;

      private sealed class CryptVariant
      {
        public string Name;
        public Barrier Barrier;
        public Ground Ground;
        public Gate Gate;
        public Portal PortalUp;
        public Portal PortalDown;
        public Entity Boss;
      }
    }

    private sealed class DragonLairBuilder : Builder
    {
      public DragonLairBuilder(OpusMaker Maker)
        : base(Maker)
      {
        var Entities = Codex.Entities;

        this.DragonLairVariance = new Variance<DragonLairVariant>
        (
          new[]
          {
            new DragonLairVariant(OpusTerms.BlackLair, Codex.Grounds.moss, Entities.baby_black_dragon, Entities.young_black_dragon, Entities.adult_black_dragon, Entities.ancient_black_dragon),
            new DragonLairVariant(OpusTerms.BlueLair, Codex.Grounds.metal_floor, Entities.baby_blue_dragon, Entities.young_blue_dragon, Entities.adult_blue_dragon, Entities.ancient_blue_dragon),
            new DragonLairVariant(OpusTerms.DeepLair, Codex.Grounds.granite_floor, Entities.baby_deep_dragon, Entities.young_deep_dragon, Entities.adult_deep_dragon, Entities.ancient_deep_dragon),
            new DragonLairVariant(OpusTerms.GreenLair, Codex.Grounds.wooden_floor, Entities.baby_green_dragon, Entities.young_green_dragon, Entities.adult_green_dragon, Entities.ancient_green_dragon),
            new DragonLairVariant(OpusTerms.OrangeLair, Codex.Grounds.sand, Entities.baby_orange_dragon, Entities.young_orange_dragon, Entities.adult_orange_dragon, Entities.ancient_orange_dragon),
            new DragonLairVariant(OpusTerms.RedLair, Codex.Grounds.obsidian_floor, Entities.baby_red_dragon, Entities.young_red_dragon, Entities.adult_red_dragon, Entities.ancient_red_dragon),
            new DragonLairVariant(OpusTerms.ShimmerLair, Codex.Grounds.marble_floor, Entities.baby_shimmering_dragon, Entities.young_shimmering_dragon, Entities.adult_shimmering_dragon, Entities.ancient_shimmering_dragon),
            new DragonLairVariant(OpusTerms.WhiteLair, Codex.Grounds.snow, Entities.baby_white_dragon, Entities.young_white_dragon, Entities.adult_white_dragon, Entities.ancient_white_dragon),
            new DragonLairVariant(OpusTerms.YellowLair, Codex.Grounds.dirt, Entities.baby_yellow_dragon, Entities.young_yellow_dragon, Entities.adult_yellow_dragon, Entities.ancient_yellow_dragon)
            // TODO: include neutral dragons: silver/gold?
          }.OrderBy(V => V.EntityList.Last().Difficulty).ThenBy(V => V.EntityList.Last().Level).ToArray()
        );
      }

      public void Build(OpusSection Section)
      {
        var DragonLairVariant = DragonLairVariance.NextVariant();
        Section.OverlandAreaName = OpusTerms.Kobold_Fort;

        var OverlandMap = Maker.OverlandMap;
        var DragonLairClearing = Section.LargestClearing;
        var DragonLairName = Generator.EscapedModuleTerm(DragonLairVariant.Name);

        // TODO: other types of 'secret entrances'?
        var DragonScalesItem = Codex.Items.DragonScales.Find(S => S.DerivativeEntities.ContainsAny(DragonLairVariant.EntityList));
        Maker.KoboldFort.Build(Section, OverlandMap, DragonLairClearing, DragonScalesItem);

        if (!Generator.Adventure.World.HasSite(DragonLairName))
        {
          BuildStart();

          var OverlandEntranceSquare = OverlandMap[DragonLairClearing.Circle.X, DragonLairClearing.Circle.Y];

          var AlertItemArray = new[] { Codex.Items.brass_bugle, Codex.Items.leather_drum, Codex.Items.wooden_flute, Codex.Items.wooden_harp, Codex.Items.tin_whistle, Codex.Items.bronze_bell, Codex.Items.tooled_horn };

          const int ChamberSize = 24;

          var LairBarrier = Codex.Barriers.cave_wall;
          var SoftGround = DragonLairVariant.NativeGround;
          var HardGround = Codex.Grounds.cave_floor;
          var ChamberEntranceSquare = OverlandEntranceSquare;
          var ChamberCount = DragonLairVariant.EntityList.Count; // 4: baby, young, adult, ancient.

          var LairSite = Generator.Adventure.World.AddSite(DragonLairName);
          var LairMap = Generator.Adventure.World.AddMap(DragonLairName, ChamberSize * ChamberCount, ChamberSize);
          LairMap.SetDifficulty(Section.Distance);
          LairMap.SetTerminal(true);
          LairMap.SetSealed(true); // no random spawns.
          LairMap.SetAtmosphere(Codex.Atmospheres.cavern);
          var LairLevel = LairSite.AddLevel(1, LairMap);

          for (var ChamberIndex = 1; ChamberIndex <= ChamberCount; ChamberIndex++)
          {
            var ChamberEntity = DragonLairVariant.EntityList[ChamberIndex - 1];
            var ChamberRegion = new Region(ChamberSize * (ChamberIndex - 1), LairMap.Region.Top, (ChamberSize * ChamberIndex) - 1, LairMap.Region.Bottom);

            var CircleCount = LairMap.Region.Height / 4;
            var CircleList = new Inv.DistinctList<Inv.Circle>(CircleCount);

            foreach (var CircleIndex in CircleCount.NumberSeries())
            {
              var CircleAttempt = 0;
              do
              {
                var CircleRadius = RandomSupport.NextRange(1, 3);
                var CircleX = RandomSupport.NextRange(ChamberRegion.Left + CircleRadius + 1, ChamberRegion.Right - CircleRadius - 1);
                var CircleY = RandomSupport.NextRange(ChamberRegion.Top + CircleRadius + 1, ChamberRegion.Bottom - CircleRadius - 1);
                var Circle = new Inv.Circle(CircleX, CircleY, CircleRadius);

                if (!CircleList.Any(C => C.Origin == Circle.Origin))
                {
                  CircleList.Add(Circle);
                  break;
                }
              }
              while (CircleAttempt++ < 3);
            }

            // place all the circles (random order of circles should give the most different generation).
            foreach (var Circle in CircleList)
            {
              foreach (var RoomSquare in LairMap.GetCircleOuterSquares(Circle).Where(S => S.IsVoid()))
                Generator.PlaceSolidWall(RoomSquare, LairBarrier, WallSegment.Pillar);

              foreach (var RoomSquare in LairMap.GetCircleInnerSquares(Circle).Where(S => S.IsVoid()))
                Generator.PlaceFloor(RoomSquare, SoftGround);
            }

            // establish tunnels to connect all circles.
            var PreviousCircle = CircleList[0];
            foreach (var NextCircle in CircleList.Skip(1))
            {
              Maker.Tunnel(LairMap.Region, LairMap[PreviousCircle.X, PreviousCircle.Y], LairMap[NextCircle.X, NextCircle.Y], LairBarrier, SoftGround, IsLit: false);

              PreviousCircle = NextCircle;
            }

            // sort the circles so the connecting tunnels are more sensible and so the first and last circle are far enough apart and diagonally opposite each other.
            var FlipHorizontal = +1;
            var FlipVertical = ChamberIndex % 2 == 0 ? -1 : +1;
            CircleList.Sort((A, B) => ((A.X * FlipHorizontal) + (A.Y * FlipVertical)).CompareTo((B.X * FlipHorizontal) + (B.Y * FlipVertical)));

            // entrance to the lair.
            var StartCircle = CircleList.First();
            var EnterSquare = LairMap[StartCircle.X, StartCircle.Y];

            Debug.Assert(EnterSquare.Passage == null && EnterSquare.Floor != null && EnterSquare.Wall == null);

            if (ChamberIndex == 1)
            {
              LairLevel.SetTransitions(EnterSquare, null);
              Generator.PlacePassage(ChamberEntranceSquare, Codex.Portals.clay_staircase_down, EnterSquare);
              Generator.PlacePassage(EnterSquare, Codex.Portals.clay_staircase_up, ChamberEntranceSquare);
            }
            else
            {
              // connect two chambers together with a hard tunnel.
              Maker.Tunnel(LairMap.Region, ChamberEntranceSquare, EnterSquare, LairBarrier, HardGround, IsLit: false);

              var SentrySquare = EnterSquare;
              if (SentrySquare.Character == null)
              {
                // kobold sentries with bugles to awaken dragons (stationary position inside each chamber) - perhaps they can wear dragon scales as armour?
                Generator.PlaceCharacter(SentrySquare, Codex.Entities.large_kobold);
                var SentryCharacter = SentrySquare.Character;
                if (SentryCharacter != null)
                {
                  // walk the chamber tunnel.
                  SentryCharacter.SetResidentRoute(new[] { SentrySquare, ChamberEntranceSquare }, 0);

                  // remove any darts so they will only use their musical instrument at range.
                  SentryCharacter.Inventory.RemoveAllAssets();

                  // alert musical instrument.
                  var AlertItem = AlertItemArray.GetRandom();
                  SentryCharacter.Inventory.Carried.Add(Generator.NewSpecificAsset(SentrySquare, AlertItem));
                  foreach (var SentrySkill in AlertItem.GetSkills())
                    SentryCharacter.ForceCompetency(SentrySkill, Codex.Qualifications.proficient);

                  // chosen kobold gets to wear the scales.
                  if (DragonScalesItem != null && !SentryCharacter.Inventory.HasEquipment(Codex.Slots.suit))
                  {
                    Generator.EquipCharacter(SentryCharacter, Codex.Slots.suit, Generator.NewSpecificAsset(SentrySquare, DragonScalesItem));

                    foreach (var SentrySkill in DragonScalesItem.GetSkills())
                      SentryCharacter.ForceCompetency(SentrySkill, Codex.Qualifications.proficient);
                  }
                }
              }
            }

            if (ChamberCount > 1)
            {
              var FinishCircle = CircleList.Last();
              var ExitSquare = LairMap[FinishCircle.X, FinishCircle.Y];

              Debug.Assert(ExitSquare.Passage == null && ExitSquare.Floor != null && ExitSquare.Wall == null);

              // final chamber has an escape portal to the overland.
              if (ChamberIndex == ChamberCount)
              {
                Generator.PlacePassage(ExitSquare, Codex.Portals.transportal, OverlandEntranceSquare);

                // boss and artifact.
                var BossCharacter = Maker.NewEvilCharacter(ExitSquare, Maker.SelectUniqueEntity(Codex.Entities.Ixoth));
                BossCharacter.Inventory.Carried.Add(Generator.GenerateUniqueAsset(ExitSquare)); // carry but don't outfit.
                Generator.PlaceCharacter(ExitSquare, BossCharacter);
                Maker.SnoozeCharacter(BossCharacter);
              }
              else
              {
                ChamberEntranceSquare = ExitSquare;
              }
            }

            // hard border around the soft inner floors.
            foreach (var HardSquare in LairMap.GetSquares(ChamberRegion).Where(S => S.Floor != null && S.GetAdjacentSquares().Any(S => S.Wall != null)))
              Generator.PlaceFloor(HardSquare, HardGround);

            // place gold and dragons.
            var LairCoinTotal = 0;
            var LairCharacterCount = 1;
            var LairEgg = Codex.Eggs.List.Find(E => E.Layer == ChamberEntity);
            foreach (var SoftSquare in LairMap.GetSquares(ChamberRegion).Where(S => S.Floor?.Ground == SoftGround && S.Passage == null && S.Character == null))
            {
              // NOTE: may still generate on downstairs due to the algorithm (can be fixed by moving the gold/dragon generation out of the for-loop for lair levels).

              if (LairCharacterCount <= 10 && Chance.OneIn(LairCharacterCount).Hit())
              {
                LairCharacterCount++;

                Generator.PlaceCharacter(SoftSquare, ChamberEntity);

                var LairCharacter = SoftSquare.Character;
                if (LairCharacter != null)
                {
                  // NOTE: orange dragons will not be put asleep because they have sleep resistance.
                  Maker.SnoozeCharacter(LairCharacter);

                  if (LairEgg != null && Chance.OneIn3.Hit())
                  {
                    var EggAsset = Generator.NewSpecificAsset(Square: null, Codex.Items.egg);
                    EggAsset.SetEgg(LairEgg);
                    SoftSquare.PlaceAsset(EggAsset);
                  }
                }
              }
              else
              {
                var CoinDice = ChamberIndex.d(20 - LairCharacterCount + 1) + (ChamberIndex * (20 - LairCharacterCount + 1));
                var CoinQuantity = CoinDice.Roll();
                Generator.DropCoins(SoftSquare, CoinQuantity);

                LairCoinTotal += CoinQuantity;
              }
            }

            Debug.WriteLine($"{DragonLairVariant.Name} {ChamberIndex} Coins = {LairCoinTotal}");
          }

          // replace unreachable squares with a wall, so the following call to RepairMap will remove them entirely as necessary.
          foreach (var UnreachableSquare in Maker.FindUnreachableSquares(LairMap, LairMap.Region, LairLevel.UpSquare))
          {
            UnreachableSquare.SetFloor(null);
            Generator.PlaceSolidWall(UnreachableSquare, LairBarrier, WallSegment.Pillar);
          }

          // repair the walls and zones.
          Generator.RepairMap(LairMap, LairMap.Region);

          LairMap.AddArea(DragonLairName).AddMapZones();

          Debug.Assert(!LairMap.Zones.Any(Z => Z.Squares.Count == 0), "Dragon lair should not have any zones without squares.");

          BuildStop();
        }

        // TODO:
        // * ground based traps in the hard tunnels?
        // * flooded/ice/lava filling the hard tunnels?
        // * hidden egg chamber?
        // * huge chunks of meat, corpses, dragon scales, <colour> gems and other treasure - gold items?
        // * objective trigger for each Lair that somehow leads to the opening of Chromatic Lair?
      }

      private readonly Variance<DragonLairVariant> DragonLairVariance;

      private sealed class DragonLairVariant
      {
        internal DragonLairVariant(string Name, Ground NativeGround, params Entity[] EntityArray)
        {
          this.Name = Name;
          this.NativeGround = NativeGround;
          this.EntityList = EntityArray;
        }

        public string Name { get; }
        /// <summary>
        /// The type of ground where dragons and gold are generated.
        /// </summary>
        public Ground NativeGround { get; }
        public IReadOnlyList<Entity> EntityList { get; }

        public override string ToString() => $"{Name}: {NativeGround} | {EntityList.Select(E => E.Name).AsSeparatedText(", ")}";
      }
    }

    private sealed class FarmBuilder : Builder
    {
      public FarmBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.FarmVariance = new Variance<FarmVariant>
        (
          new FarmVariant
          {
            Name = OpusTerms.ChickenFarm,
            StrangeName = OpusTerms.StrangeChicken,
            FieldGround = Codex.Grounds.grass,
            FenceGround = Codex.Grounds.stone_path,
            EntityList = new[] { Codex.Entities.chicken }
          },
          new FarmVariant
          {
            Name = OpusTerms.SheepFarm,
            StrangeName = OpusTerms.StrangeSheep,
            FieldGround = Codex.Grounds.grass,
            FenceGround = Codex.Grounds.stone_path,
            EntityList = new[] { Codex.Entities.sheep, Codex.Entities.lamb }
          },
          new FarmVariant
          {
            Name = OpusTerms.GoatFarm,
            StrangeName = OpusTerms.StrangeGoat,
            FieldGround = Codex.Grounds.grass,
            FenceGround = Codex.Grounds.stone_path,
            EntityList = new[] { Codex.Entities.goat }
          },
          new FarmVariant
          {
            Name = OpusTerms.PigFarm,
            StrangeName = OpusTerms.StrangePig,
            FieldGround = Codex.Grounds.grass,
            FenceGround = Codex.Grounds.stone_path,
            EntityList = new[] { Codex.Entities.pig }
          },
          new FarmVariant
          {
            Name = OpusTerms.HorseFarm,
            StrangeName = OpusTerms.StrangeHorse,
            FieldGround = Codex.Grounds.grass,
            FenceGround = Codex.Grounds.stone_path,
            EntityList = new[] { Codex.Entities.horse, Codex.Entities.pony, Codex.Entities.warhorse }
          },
          new FarmVariant
          {
            Name = OpusTerms.CowFarm,
            StrangeName = OpusTerms.StrangeCow,
            FieldGround = Codex.Grounds.grass,
            FenceGround = Codex.Grounds.stone_path,
            EntityList = new[] { Codex.Entities.cow, Codex.Entities.bull }
          },
          new FarmVariant
          {
            Name = OpusTerms.SealFarm,
            StrangeName = OpusTerms.StrangeSeal,
            FieldGround = Codex.Grounds.water,
            FenceGround = Codex.Grounds.sand,
            EntityList = new[] { Codex.Entities.seal }
          }
        );
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        var FarmVariant = FarmVariance.NextVariant();
        Section.OverlandAreaName = FarmVariant.Name;

        var FarmClearing = Section.LargestClearing;
        var FarmMap = Maker.OverlandMap;

        var FarmCircle = FarmClearing.Circle.Reduce(FarmClearing.Circle.Radius > 4 ? FarmClearing.Circle.Radius - 4 : 2);

        var MiddleSquare = FarmMap[FarmCircle.X, FarmCircle.Y];

        var FarmZone = FarmMap.AddZone();

        // fence.
        foreach (var FarmSquare in FarmMap.GetCircleOuterSquares(FarmCircle))
          Generator.PlaceFloor(FarmSquare, FarmVariant.FenceGround);

        // Strange animals.
        var Strange = Chance.Always.Hit();
        var SuspicousEntity = FarmVariant.EntityList[0]; // first entity is the Strange one.
        var StrangeName = Generator.EscapedModuleTerm(FarmVariant.StrangeName);
        void ActSuspicous(Character StrangeCharacter)
        {
          Debug.Assert(StrangeCharacter.Entity == SuspicousEntity);

          StrangeCharacter.SetName(StrangeName);
          StrangeCharacter.AcquireTalent(Codex.Properties.aggravation);

          // TODO: should we make the Strange animal eat corpses on the ground?
          //StrangeCharacter.AcquireTalent(Codex.Properties.cannibalism);
          //StrangeCharacter.SetPunished(Codex.Punishments.gluttony);

          if (FarmClearing.Section.Distance + 5 > StrangeCharacter.Level)
            Generator.PromoteCharacter(StrangeCharacter, FarmClearing.Section.Distance + 5 - StrangeCharacter.Level);
        }

        // field.
        var FarmParty = Generator.NewParty(Leader: null);

        foreach (var FarmSquare in FarmMap.GetCircleInnerSquares(FarmCircle))
        {
          if (!FarmSquare.IsVoid())
            FarmZone.ForceSquare(FarmSquare);

          if (FarmSquare.Floor != null)
          {
            Generator.PlaceFloor(FarmSquare, FarmVariant.FieldGround);

            var Middle = FarmSquare == MiddleSquare;
            if ((Middle && Strange) || (!Middle && Chance.OneIn3.Hit()))
            {
              if (Middle && Strange)
                Generator.PlaceCharacter(FarmSquare, SuspicousEntity);
              else
                Generator.PlaceCharacter(FarmSquare, FarmClearing.Section.MinimumDifficulty, FarmClearing.Section.MaximumDifficulty, FarmVariant.EntityList);

              var FarmCharacter = FarmSquare.Character;
              if (FarmCharacter != null)
              {
                FarmCharacter.SetNeutral(true);

                if (Middle && Strange)
                {
                  ActSuspicous(FarmCharacter);

                  FarmCharacter.SetResidentSquare(FarmSquare);
                }
                else
                {
                  // random routes within the grassed area. random point on the circle, rotated 90 degrees.
                  var RouteSquare = FarmMap.GetCircleOuterSquares(FarmCircle).GetRandomOrNull();
                  var RouteArray = new[]
                  {
                    RouteSquare,
                    RouteSquare.Rotate(FarmCircle.Origin, 90),
                    RouteSquare.Rotate(FarmCircle.Origin, 180),
                    RouteSquare.Rotate(FarmCircle.Origin, 270)
                  }.EnumerateRandom().ToArray();

                  FarmCharacter.SetResidentRoute(RouteArray, 0);
                }
                FarmParty.AddAlly(FarmCharacter, Clock.Zero, Delay.Zero);
              }
            }
          }
        }

        FarmZone.SetLit(true);
        Debug.Assert(FarmZone.HasSquares());

        // secret level.
        var SecretCharacter = MiddleSquare.Character;
        var SecretName = Generator.EscapedModuleTerm(FarmVariant.Name);
        if (SecretCharacter != null && !Generator.Adventure.World.HasSite(SecretName))
        {
          var SecretSite = Generator.Adventure.World.AddSite(SecretName);

          var SecretCircle = new Inv.Circle(FarmCircle.Radius + 1, FarmCircle.Radius + 1, FarmCircle.Radius);
          var SecretSize = ((SecretCircle.Radius + 1) * 2) + 1;

          var SecretMap = Generator.Adventure.World.AddMap(SecretName, SecretSize, SecretSize);
          SecretMap.SetDifficulty(FarmClearing.Section.Distance + 1);
          SecretMap.SetTerminal(true);
          SecretMap.SetAtmosphere(Codex.Atmospheres.forest);
          var SecretLevel = SecretSite.AddLevel(1, SecretMap);
          SecretLevel.SetTransitions(null, null);

          var SecretZone = SecretMap.AddZone();

          // fence.
          foreach (var SecretSquare in SecretMap.GetCircleOuterSquares(SecretCircle))
          {
            Generator.PlaceWall(SecretSquare, Codex.Barriers.hell_brick, WallStructure.Permanent, WallSegment.Pillar);

            SecretZone.AddSquare(SecretSquare);
          }

          // field.
          var SecretParty = Generator.NewParty(Leader: null);
          var RiftSquare = SecretMap[SecretCircle.X, SecretCircle.Y];

          foreach (var SecretSquare in SecretMap.GetCircleInnerSquares(SecretCircle))
          {
            Generator.PlaceFloor(SecretSquare, Codex.Grounds.moss);

            SecretZone.AddSquare(SecretSquare);

            if (SecretSquare != RiftSquare)
            {
              Generator.PlaceCharacter(SecretSquare, SuspicousEntity);

              var StrangeCharacter = SecretSquare.Character;
              if (StrangeCharacter != null)
              {
                ActSuspicous(StrangeCharacter);

                StrangeCharacter.SetNeutral(false);

                SecretParty.AddAlly(StrangeCharacter, Clock.Zero, Delay.Zero);
              }
            }
          }

          var UniqueSquare = SecretMap.GetCircleInnerSquares(SecretCircle).Where(S => S.Character != null).GetRandomOrNull();
          UniqueSquare?.PlaceAsset(Generator.GenerateUniqueAsset(UniqueSquare));

          SecretZone.SetLit(true);
          Debug.Assert(SecretZone.HasSquares());

          Generator.PlacePassage(RiftSquare, Codex.Portals.rift, MiddleSquare);

          var SecretScript = SecretCharacter.InsertScript();
          SecretScript.TurnedFriendly.Sequence.Add(Codex.Tricks.connecting_portal).SetTarget(RiftSquare);
          SecretScript.TurnedFriendly.Sequence.Add(Codex.Tricks.transport_candidate).SetSource(SecretCharacter).SetTarget(RiftSquare);
          SecretScript.Killed.Sequence.Add(Codex.Tricks.connecting_rift).SetTarget(RiftSquare);

          Generator.RepairMap(SecretMap, SecretMap.Region);
        }

        // TODO:
        // * randomly determined number of farm animals? right now, zero can be generated.
        // * secret level is currently boring, jazz it up - loot pinatas?
        // * neutral farmer/peasant/shepherd?
        // * wooden barn/shed building contains animal food (carrots, etc)?
        // * sleeping animals in the three-wall barn.

        BuildStop();
      }

      private readonly Variance<FarmVariant> FarmVariance;

      private sealed class FarmVariant
      {
        public string Name;
        public string StrangeName;
        public Ground FieldGround;
        public Ground FenceGround;
        public IReadOnlyList<Entity> EntityList;

        public override string ToString() => $"{EntityList.Select(E => E.Name).AsSeparatedText(", ")}";
      }
    }

    private sealed class FinaleBuilder : Builder
    {
      public FinaleBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.FinaleVariance = new Variance<FinaleVariant>
        (
          new FinaleVariant
          {
            Name = OpusTerms.Arena
          }
        );
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        var FinaleVariant = FinaleVariance.NextVariant();
        Section.OverlandAreaName = FinaleVariant.Name;

        var FinaleMap = Maker.OverlandMap;
        var FinaleJoin = Section.Joins.Find(J => J.Section.Critical);
        var FinaleCircle = new Inv.Circle(Section.Region.Midpoint(), Math.Min(6, Math.Min(Section.Region.Width, Section.Region.Height) / 2 - 2));

        Maker.FinaleSquare = FinaleMap[FinaleCircle.Origin];

        var ArenaTrigger = Maker.FinaleSquare.InsertTrigger();

        var FormationFlip = Chance.OneIn2.Hit();
        var LeftOrRight = Chance.OneIn2.Hit();

        // arena.
        var ArenaZone = FinaleMap.AddZone();

        foreach (var FinaleSquare in FinaleMap.GetCircleInnerSquares(FinaleCircle))
        {
          var GoodVsEvil = FormationFlip;

          var DistanceX = FinaleSquare.X - Maker.FinaleSquare.X;
          var DistanceY = FinaleSquare.Y - Maker.FinaleSquare.Y;

          if (LeftOrRight)
          {
            if (DistanceX < 0 && (DistanceX + DistanceY) < 0)
              GoodVsEvil = !GoodVsEvil;
            else if (DistanceX >= 0 && (DistanceX + DistanceY) < 0)
              GoodVsEvil = !GoodVsEvil;
          }
          else
          {
            if (DistanceX < 0 && (DistanceX - DistanceY) > 0)
              GoodVsEvil = !GoodVsEvil;
            else if (DistanceX >= 0 && (DistanceX - DistanceY) > 0)
              GoodVsEvil = !GoodVsEvil;
          }

          FinaleSquare.SetLit(true);
          Generator.PlaceFloor(FinaleSquare, GoodVsEvil ? Codex.Grounds.marble_floor : Codex.Grounds.granite_floor);

          ArenaZone.AddSquare(FinaleSquare);
        }

        Debug.Assert(ArenaZone.HasSquares());

        var FinaleNorthSquare = Maker.FinaleSquare.Adjacent(Direction.North, FinaleCircle.Radius);
        var FinaleSouthSquare = Maker.FinaleSquare.Adjacent(Direction.South, FinaleCircle.Radius);
        var FinaleEastSquare = Maker.FinaleSquare.Adjacent(Direction.East, FinaleCircle.Radius);
        var FinaleWestSquare = Maker.FinaleSquare.Adjacent(Direction.West, FinaleCircle.Radius);
        var FinaleSouthWestSquare = Maker.FinaleSquare.Adjacent(Direction.SouthWest, FinaleCircle.Radius);
        var FinaleNorthEastSquare = Maker.FinaleSquare.Adjacent(Direction.NorthEast, FinaleCircle.Radius);
        var FinaleNorthWestSquare = Maker.FinaleSquare.Adjacent(Direction.NorthWest, FinaleCircle.Radius);
        var FinaleSouthEastSquare = Maker.FinaleSquare.Adjacent(Direction.SouthEast, FinaleCircle.Radius);

        // divider pits.
        foreach (var FinaleSquare in LeftOrRight ? FinaleSouthWestSquare.GetPathSquares(FinaleNorthEastSquare) : FinaleNorthWestSquare.GetPathSquares(FinaleSouthEastSquare))
        {
          if (FinaleSquare.Floor != null && FinaleSquare.Floor.Ground != Codex.Grounds.dirt)
          {
            if (FinaleSquare == Maker.FinaleSquare)
            {
              Generator.PlaceFloor(FinaleSquare, Codex.Grounds.gold_floor);
            }
            else
            {
              Generator.PlaceFloor(FinaleSquare, Codex.Grounds.dirt);
              Generator.PlaceTrap(FinaleSquare, Codex.Devices.pit, Revealed: true);
            }
          }
        }

        // formations.
        IEnumerable<Square> GetFormationSquares(Square LeftSquare, Square RightSquare, Direction AdvanceDirection)
        {
          Square Result = null;
          var Advance = 0;
          while (Result == null && Advance < FinaleCircle.Radius)
          {
            var FirstSquare = LeftSquare.Adjacent(AdvanceDirection, Advance);
            var LastSquare = RightSquare.Adjacent(AdvanceDirection, Advance);

            foreach (var FormationSquare in FirstSquare.GetPathSquares(LastSquare).Where(S => S.Character == null && S.Floor != null && S.Floor.Ground != Codex.Grounds.dirt && S.Wall == null))
              yield return FormationSquare;

            Advance++;
          }
        }

        var FormationDictionary = new Dictionary<Character, Square>();
        var GoodFormationSquareList = GetFormationSquares(LeftOrRight ? FinaleWestSquare : FinaleEastSquare, FinaleNorthSquare, Direction.South).ToDistinctList();
        var EvilFormationSquareList = GetFormationSquares(FinaleSouthSquare, LeftOrRight ? FinaleEastSquare : FinaleWestSquare, Direction.North).ToDistinctList();
        if (FormationFlip)
          (EvilFormationSquareList, GoodFormationSquareList) = (GoodFormationSquareList, EvilFormationSquareList);

        foreach (var GoodCharacter in Maker.GoodCharacterList)
        {
          var GoodSquare = GoodFormationSquareList.RemoveFirst();
          ArenaTrigger.Add(Delay.Zero, Codex.Tricks.transport_candidate).SetSource(GoodCharacter).SetTarget(GoodSquare);

          FormationDictionary.Add(GoodCharacter, GoodSquare);
        }

        foreach (var EvilCharacter in Maker.EvilCharacterList)
        {
          var EvilSquare = EvilFormationSquareList.RemoveFirst();
          ArenaTrigger.Add(Delay.Zero, Codex.Tricks.transport_candidate).SetSource(EvilCharacter).SetTarget(EvilSquare);

          FormationDictionary.Add(EvilCharacter, EvilSquare);
        }

        // resident routes to make both sides engage.
        foreach (var (GoodCharacter, EvilCharacter) in Maker.GoodCharacterList.ZipX(Maker.EvilCharacterList))
        {
          ArenaTrigger.Add(Delay.Zero, Codex.Tricks.change_route).SetSource(GoodCharacter).SetTarget(FormationDictionary[EvilCharacter]);
          ArenaTrigger.Add(Delay.Zero, Codex.Tricks.change_route).SetSource(EvilCharacter).SetTarget(FormationDictionary[GoodCharacter]);
        }

        // good double teams.
        foreach (var GoodCharacter in Maker.GoodCharacterList.Skip(Maker.EvilCharacterList.Count))
          ArenaTrigger.Add(Delay.Zero, Codex.Tricks.change_route).SetSource(GoodCharacter).SetTarget(FormationDictionary[Maker.EvilCharacterList.GetRandom()]);

        // evil double teams.
        foreach (var EvilCharacter in Maker.EvilCharacterList.Skip(Maker.GoodCharacterList.Count))
          ArenaTrigger.Add(Delay.Zero, Codex.Tricks.change_route).SetSource(EvilCharacter).SetTarget(FormationDictionary[Maker.GoodCharacterList.GetRandom()]);

        // end game rift.
        Generator.PlacePassage(Maker.FinaleSquare, Codex.Portals.rift, null);

        // TODO:
        // * 

        BuildStop();
      }

      private readonly Variance<FinaleVariant> FinaleVariance;

      private sealed class FinaleVariant
      {
        public string Name;
      }
    }

    private sealed class GardenBuilder : Builder
    {
      public GardenBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.GardenVariance = new Variance<GardenVariant>
        (
          new GardenVariant
          {
          }
        );
      }

      public void Build(OpusSection Section, Map GardenMap)
      {
        BuildStart();

        var GardenVariant = GardenVariance.NextVariant();

        // TODO:
        // * fairy garden with sleeping nymphs and water fountains, fae dragon.
        // * Elwing, Aphrodite.

        BuildStop();
      }

      private readonly Variance<GardenVariant> GardenVariance;

      private sealed class GardenVariant
      {
      }
    }

    private sealed class HengeBuilder : Builder
    {
      public HengeBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.HengeVariance = new Variance<HengeVariant>
        (
          Sequential: true, // always demon henge last.
          new HengeVariant
          {
            Name = OpusTerms.Stone_Henge,
            Block = Codex.Blocks.stone_boulder,
            Trick = Codex.Tricks.animated_objects,
            MiddleAction = S => Generator.PlaceFixture(S, Codex.Features.altar)
          },
          new HengeVariant
          {
            Name = OpusTerms.Statue_Henge,
            Block = Codex.Blocks.statue,
            Prisoner = Codex.Kinds.troll,
            Trick = Codex.Tricks.living_statue,
            MiddleAction = S =>
            {
              // place a dead halfling in a sack (reference to The Hobbit book).
              var SackAsset = Generator.NewSpecificAsset(S, Codex.Items.sack);
              S.PlaceAsset(SackAsset);

              var HobbitCharacter = Generator.NewCharacter(S, Codex.Entities.proudfoot);

              foreach (var MercenaryAsset in HobbitCharacter.Inventory.RemoveAllAssets())
              {
                if (MercenaryAsset.Container != null)
                  S.PlaceAsset(MercenaryAsset);
                else
                  SackAsset.Container.Stash.Add(MercenaryAsset);
              }

              // The 'One Ring'.
              if (SackAsset.Container.Stash.Count == 0)
                SackAsset.Container.Stash.Add(Generator.NewSpecificAsset(S, Codex.Items.ring_of_invisibility));

              SackAsset.Container.Stash.Add(Generator.CorpseCharacter(HobbitCharacter));
            }
          },
          new HengeVariant
          {
            Name = OpusTerms.Crystal_Henge,
            Block = Codex.Blocks.crystal_boulder,
            Prisoner = Codex.Kinds.demon,
            Ground = Codex.Grounds.obsidian_floor,
            Trick = Codex.Tricks.summoning_demons,
            MiddleAction = S => Generator.PlaceFixture(S, Codex.Features.pentagram),
            FinalTriggerAction = (S, T) =>
            {
              var FurnaceSquare = BuildFurnace(S.Map.Difficulty + 1);

              if (FurnaceSquare != null)
                T.Add(Delay.FromTurns(20), Codex.Tricks.connecting_rift).SetTarget(FurnaceSquare);
            }
          }
        );
      }

      public void Build(OpusSection Section)
      {
        var HengeVariant = HengeVariance.NextVariant();
        Section.OverlandAreaName = HengeVariant.Name;

        var HengeMap = Maker.OverlandMap;

        if (HengeVariant.Prisoner == null || HengeVariant.Prisoner.Entities.Any(E => E.IsEncounter && E.Difficulty >= Section.MinimumDifficulty && E.Difficulty <= Section.MaximumDifficulty))
        {
          BuildStart();

          var HengeClearing = Section.LargestClearing;
          var HengeCircle = HengeClearing.Circle.Reduce(HengeClearing.Circle.Radius > 3 ? HengeClearing.Circle.Radius - 3 : 2);
          var MiddleSquare = HengeMap[HengeClearing.Circle.X, HengeClearing.Circle.Y];

          Debug.Assert(HengeCircle.Radius < HengeClearing.Circle.Radius, "Henge radius must be less than the clearing so we can get the outline squares.");

          var HengeZone = HengeMap.AddZone();

          var HengeTrigger = HengeVariant.Trick != null || HengeVariant.FinalTriggerAction != null ? HengeZone.InsertTrigger() : null;

          foreach (var HengeSquare in HengeMap.GetCircleOuterSquares(HengeCircle))
          {
            HengeZone.AddSquare(HengeSquare);

            if (HengeSquare.Floor == null)
              Generator.PlaceFloor(HengeSquare, Codex.Grounds.dirt);

            // space the boulders around the circle.
            if (!HengeSquare.GetAdjacentSquares().Any(S => S.Boulder != null))
            {
              var PrisonerEntity = HengeVariant.Block.Prison != null && HengeVariant.Prisoner != null ? Generator.RandomEntity(Section.MinimumDifficulty, Section.MaximumDifficulty, E => E.Kind.In(HengeVariant.Prisoner)) : null;

              if (HengeVariant.Ground != null)
                Generator.PlaceFloor(HengeSquare, HengeVariant.Ground);

              Generator.PlaceBoulder(HengeSquare, HengeVariant.Block, IsRigid: true, PrisonerEntity);

              if (HengeVariant.Trick != null)
                HengeTrigger.Add(Delay.FromTurns(20), HengeVariant.Trick).SetTarget(HengeSquare);
            }
          }

          foreach (var HengeSquare in HengeMap.GetCircleInnerSquares(HengeCircle))
          {
            if (HengeVariant.Ground != null)
              Generator.PlaceFloor(HengeSquare, HengeVariant.Ground);
            else if (HengeSquare.Floor == null)
              Generator.PlaceFloor(HengeSquare, Codex.Grounds.dirt);

            HengeZone.AddSquare(HengeSquare);
          }

          HengeZone.SetLit(true);
          Debug.Assert(HengeZone.HasSquares());

          HengeVariant.MiddleAction?.Invoke(MiddleSquare);

          HengeVariant.FinalTriggerAction?.Invoke(MiddleSquare, HengeTrigger);

          if (HengeCircle.Radius < HengeClearing.Circle.Radius - 3)
          {
            foreach (var HengeSquare in HengeMap.GetCircleOuterSquares(HengeClearing.Circle.Reduce(3)))
            {
              if (HengeSquare.Boulder == null && Chance.OneIn3.Hit())
                Generator.PlaceSolidWall(HengeSquare, Codex.Barriers.tree, WallSegment.Pillar);
            }
          }

          // TODO:
          // * 

          BuildStop();
        }
      }

      private Square BuildFurnace(int Difficulty)
      {
        var FurnaceName = Generator.EscapedModuleTerm(OpusTerms.Furnace);
        if (!Generator.Adventure.World.HasSite(FurnaceName))
        {
          var FurnaceSite = Generator.Adventure.World.AddSite(FurnaceName);
          var FurnaceBossSize = (RandomSupport.NextRange(7, 10) * 2) + 1; // must be odd number.
          var FurnaceBossHalfSize = FurnaceBossSize / 2;
          var FurnaceBossQuarterSize = FurnaceBossSize / 4;
          var FurnaceInset = 14;
          var FurnaceFullSize = FurnaceBossSize + ((FurnaceInset - 1) * 2);

          var FurnaceMap = Generator.Adventure.World.AddMap(FurnaceName, FurnaceFullSize, FurnaceFullSize);
          FurnaceMap.SetDifficulty(Difficulty);
          FurnaceMap.SetSealed(true);
          FurnaceMap.SetTerminal(true);
          FurnaceMap.SetAtmosphere(Codex.Atmospheres.dungeon);
          var FurnaceLevel = FurnaceSite.AddLevel(1, FurnaceMap);

          var FurnaceMidpoint = FurnaceMap.Region.Midpoint();

          // boundary circle line.
          var FirstCircle = new Inv.Circle(FurnaceMidpoint, FurnaceFullSize / 2 - 2);
          foreach (var FurnaceSquare in FurnaceMap.GetCircleInnerSquares(FirstCircle).Except(FurnaceMap.GetCircleInnerSquares(FirstCircle.Reduce(4))))
          {
            FurnaceSquare.SetLit(true);

            Generator.PlaceFloor(FurnaceSquare, Codex.Grounds.obsidian_floor);
          }

          var SecondCircle = FirstCircle.Reduce(5);
          foreach (var FurnaceSquare in FurnaceMap.GetCircleInnerSquares(SecondCircle).Except(FurnaceMap.GetCircleInnerSquares(SecondCircle.Reduce(4))))
          {
            FurnaceSquare.SetLit(true);

            Generator.PlaceFloor(FurnaceSquare, Codex.Grounds.obsidian_floor);
          }

          var ThirdCircle = SecondCircle.Reduce(5);

          var TriangleFirstPoint = new Inv.Point(FurnaceMidpoint.X, FurnaceInset);
          var TriangleSecondPoint = new Inv.Point(FurnaceInset, FurnaceMidpoint.Y);
          var TriangleThirdPoint = new Inv.Point(FurnaceFullSize - FurnaceInset, FurnaceMidpoint.Y);
          var ThroneSquare = FurnaceMap[TriangleFirstPoint.X, TriangleFirstPoint.Y + 1];
          var ApproachSquare = FurnaceMap[FurnaceMidpoint.X, FurnaceFullSize - FurnaceInset + 2];
          var LootSquare = FurnaceMap[TriangleFirstPoint];

          // outer circle line.
          var OuterCircle = new Inv.Circle(FurnaceMidpoint, FurnaceBossHalfSize - 2);
          foreach (var FurnaceSquare in FurnaceMap.GetCircleOuterSquares(OuterCircle))
          {
            FurnaceSquare.SetLit(true);

            Generator.PlaceFloor(FurnaceSquare, Codex.Grounds.obsidian_floor);
          }

          // main triangle.
          foreach (var FurnaceSquare in FurnaceMap.GetTriangleInnerSquares(new Inv.Triangle(TriangleFirstPoint, TriangleSecondPoint, TriangleThirdPoint)).Where(S => S.Floor == null))
            Generator.PlaceFloor(FurnaceSquare, Codex.Grounds.obsidian_floor);

          // inner circle area.
          var InnerCircle = OuterCircle.Reduce(FurnaceBossQuarterSize);
          foreach (var FurnaceSquare in FurnaceMap.GetCircleInnerSquares(InnerCircle))
            Generator.PlaceFloor(FurnaceSquare, Codex.Grounds.granite_floor);

          // crystal bridge.
          foreach (var FurnaceSquare in ApproachSquare.GetPathSquares(ThroneSquare.Adjacent(Direction.South)))
          {
            FurnaceSquare.SetLit(true);

            Generator.PlaceFloor(FurnaceSquare, Codex.Grounds.lava);
            Generator.PlaceBridge(FurnaceSquare, Codex.Platforms.crystal_bridge, BridgeOrientation.Vertical);
          }

          // boulder fence.
          foreach (var FurnaceSquare in FurnaceMap.GetCircleInnerSquares(InnerCircle))
          {
            if (FurnaceSquare.Bridge == null && FurnaceSquare.GetAdjacentSquares().Any(S => S.IsVoid()))
              Generator.PlaceBoulder(FurnaceSquare, Codex.Blocks.crystal_boulder, IsRigid: true);
          }

          // fill with lava.
          foreach (var FurnaceSquare in FurnaceMap.GetCircleInnerSquares(OuterCircle))
          {
            FurnaceSquare.SetLit(true);

            if (FurnaceSquare.Floor == null)
              Generator.PlaceFloor(FurnaceSquare, Codex.Grounds.lava);
          }

          // throne.
          Generator.PlaceFixture(ThroneSquare, Codex.Features.throne);

          // flood remaining squares with lava
          foreach (var FurnaceSquare in FurnaceMap.GetCircleInnerSquares(ThirdCircle).Where(S => S.Floor == null))
          {
            FurnaceSquare.SetLit(true);

            Generator.PlaceFloor(FurnaceSquare, Codex.Grounds.lava);
          }

          // artifact.
          Generator.PlacePermanentWall(LootSquare, Codex.Barriers.iron_bars, WallSegment.Pillar);
          LootSquare.PlaceAsset(Generator.GenerateUniqueAsset(LootSquare));
          foreach (var FurnaceSquare in ThroneSquare.GetAdjacentSquares().Except(LootSquare))
            Generator.PlaceFloor(FurnaceSquare, Codex.Grounds.lava);

          // blazes
          foreach (var FurnaceSquare in FurnaceMap.GetCircleOuterSquares(OuterCircle).Where(S => S.Wall == null && S.Floor?.Ground == Codex.Grounds.obsidian_floor))
            Generator.PlaceSpill(FurnaceSquare, Codex.Volatiles.blaze, Clock.Forever);

          // pentagrams.
          var LeftPentagramSquare = FurnaceMap[FurnaceInset + 1, FurnaceMidpoint.Y];
          Generator.PlaceFixture(LeftPentagramSquare, Codex.Features.pentagram);

          var RightPentagramSquare = FurnaceMap[FurnaceFullSize - FurnaceInset - 2, FurnaceMidpoint.Y];
          Generator.PlaceFixture(RightPentagramSquare, Codex.Features.pentagram);

          var FurnaceTrigger = FurnaceMap.InsertTrigger();
          foreach (var SummonIndex in 6.NumberSeries())
          {
            FurnaceTrigger.Add(Delay.FromTurns(600), Codex.Tricks.summoning_demons).SetTarget(LeftPentagramSquare);
            FurnaceTrigger.Add(Delay.Zero, Codex.Tricks.summoning_demons).SetTarget(RightPentagramSquare);
          }

          // boss.
          var FurnaceBossArray = new[] { Codex.Entities.Baalzebub, Codex.Entities.Geryon, Codex.Entities.Dispater, Codex.Entities.Asmodeus };
          Debug.Assert(FurnaceBossArray.All(E => E.Startup.Resistances.Contains(Codex.Elements.fire)), "Boss entities must have fire resistance.");
          var BossCharacter = Maker.NewEvilCharacter(ThroneSquare, Maker.SelectUniqueEntity(FurnaceBossArray));
          Generator.PlaceCharacter(ThroneSquare, BossCharacter);
          BossCharacter.InsertScript().Killed.Sequence.Add(Codex.Tricks.cleared_way).SetTarget(LootSquare);

          Maker.RepairBoundary(FurnaceMap, FurnaceMap.Region, Codex.Barriers.hell_brick, Codex.Grounds.obsidian_floor, IsLit: true);

          Maker.RepairGaps(FurnaceMap, FurnaceMap.Region, Codex.Barriers.hell_brick, IsLit: true);

          var LastRoomMatched = false;

          foreach (var RoomSquare in FurnaceMap.GetCircleOuterSquares(SecondCircle).Where(S => S.Wall != null))
          {
            Debug.Assert(RoomSquare.Floor == null);
            //Generator.PlacePermanentWall(RoomSquare, Codex.Barriers.jade_wall, WallSegment.Pillar);

            if (LastRoomMatched)
            {
              LastRoomMatched = false;
              continue;
            }

            var OptionSquare = RoomSquare.GetNeighbourSquares().Where(S => S.Floor != null).GetRandomOrNull();
            if (OptionSquare != null)
            {
              var NeighbourDirection = RoomSquare.AsDirection(OptionSquare);
              var NeighbourSquare = OptionSquare;

              do
              {
                var LastSquare = NeighbourSquare;

                NeighbourSquare = LastSquare.Adjacent(NeighbourDirection);

                if (NeighbourSquare?.Wall != null)
                  break;

                if (NeighbourSquare != null && NeighbourSquare.GetNeighbourSquares().Any(S => S != LastSquare && S != NeighbourSquare.Adjacent(NeighbourDirection) && S.Wall != null))
                {
                  //Generator.PlaceFloor(NeighbourSquare, Codex.Grounds.metal_floor);
                  NeighbourSquare = null;
                }
              }
              while (NeighbourSquare?.Floor != null && NeighbourSquare?.Bridge == null);

              if (NeighbourSquare?.Wall != null)
              {
                LastRoomMatched = true;

                foreach (var LineSquare in RoomSquare.GetPathSquares(NeighbourSquare))
                {
                  if (LineSquare.Floor != null)
                  {
                    LineSquare.SetFloor(null);
                    Generator.PlacePermanentWall(LineSquare, Codex.Barriers.hell_brick, WallSegment.Pillar);
                  }
                }
              }
            }
          }

          Maker.ConnectSquares(FurnaceMap.GetSquares().Where(S => S.Floor?.Ground == Codex.Grounds.obsidian_floor || S.Bridge != null), ThroneSquare, S =>
          {
            Generator.PlaceFloor(S, Codex.Grounds.obsidian_floor);
            Generator.PlaceDoor(S, Codex.Gates.crystal_door, DoorOrientation.Horizontal, SecretBarrier: Codex.Barriers.hell_brick);
          });

          // irons bars walls over lava for decoration.          
          foreach (var FurnaceSquare in FurnaceMap.GetSquares().Where(S => S.Wall != null && S.IsFlat() && !S.GetNeighbourSquares().Any(S => S.Wall?.Barrier == Codex.Barriers.iron_bars)))
          {
            if (Chance.OneIn2.Hit())
            {
              Generator.PlaceFloor(FurnaceSquare, Codex.Grounds.lava);
              Generator.PlacePermanentWall(FurnaceSquare, Codex.Barriers.iron_bars, WallSegment.Pillar);
            }
          }

          // repair the map.
          Generator.RepairMap(FurnaceMap, FurnaceMap.Region);

          // map details.
          foreach (var DetailZone in FurnaceMap.GetCircleInnerSquares(FirstCircle).Except(FurnaceMap.GetCircleInnerSquares(ThirdCircle)).Where(S => S.Floor != null && S.Wall == null && S.Spill == null).Select(S => S.Zone).Distinct())
          {
            var DetailSquareList = DetailZone.Squares.Where(Generator.CanPlaceTrap).ToDistinctList();
            Generator.PlaceRoomTraps(DetailSquareList, FurnaceMap.Difficulty);
            Generator.PlaceRoomCoins(DetailSquareList);
            Generator.PlaceRoomAssets(DetailSquareList);
            Generator.PlaceRoomHorde(DetailSquareList);
            Generator.PlaceRoomCharacter(DetailSquareList);
            Maker.PlaceRoomBoulders(DetailSquareList);
          }

          FurnaceMap.AddArea(FurnaceName).AddMapZones();

          var ResultSquare = FurnaceMap[FurnaceMidpoint.X, 2];
          if (ResultSquare.Floor?.Ground != Codex.Grounds.obsidian_floor)
            ResultSquare = ResultSquare.GetAdjacentSquares().Where(S => S.Floor?.Ground == Codex.Grounds.obsidian_floor).GetRandomOrNull() ?? FurnaceMap.GetSquares().Where(S => S.Floor?.Ground == Codex.Grounds.obsidian_floor).GetRandomOrNull();

          FurnaceLevel.SetTransitions(ResultSquare, null);

          // TODO:
          // * random orientation of furnace?

          return ResultSquare;
        }

        return null;
      }

      private readonly Variance<HengeVariant> HengeVariance;

      private sealed class HengeVariant
      {
        public string Name;
        public Ground Ground;
        public Block Block;
        public Kind Prisoner;
        public Trick Trick;
        public Action<Square> MiddleAction;
        public Action<Square, Trigger> FinalTriggerAction;

        public override string ToString() => Name;
      }
    }

    private sealed class HallsBuilder : Builder
    {
      public HallsBuilder(OpusMaker Maker)
        : base(Maker)
      {
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        Section.OverlandAreaName = OpusTerms.Halls;

        var AboveMap = Maker.OverlandMap;
        var HallRegion = Section.Region.Reduce(3);

        Square AboveEntranceSquare = null;

        var AboveVariant = new HallVariant()
        {
          MainBarrier = Codex.Barriers.stone_wall,
          MainGround = Codex.Grounds.stone_floor,
          MainGate = Codex.Gates.wooden_door,
          FocusGroundArray = new Ground[] { Codex.Grounds.wooden_floor },
          MinimumDifficulty = Section.MinimumDifficulty,
          MaximumDifficulty = Section.MaximumDifficulty,
          CriticalDistance = 1,
          EnterAction = (EnterSquare) =>
          {
            AboveEntranceSquare = EnterSquare;
            Generator.PlacePassage(EnterSquare, Codex.Portals.stone_staircase_down, null); // placeholder.
          },
          ExitAction = null
        };

        BuildHall(AboveMap, HallRegion, AboveVariant);

        // might need a surrounding dirt veranda to reconnect the areas.
        Maker.RepairVeranda(AboveMap, Section.Region, Codex.Grounds.dirt, IsLit: true);

        // stone path around the walls.
        foreach (var HallSquare in AboveMap.GetSquares(Section.Region))
        {
          if (HallSquare.Floor?.Ground == Codex.Grounds.dirt && HallSquare.GetAdjacentSquares().Any(S => S.Wall?.Barrier == AboveVariant.MainBarrier || S.Door?.Gate == AboveVariant.MainGate))
            Generator.PlaceFloor(HallSquare, Codex.Grounds.stone_path);
        }

        // need to punch a door from the overland, into the halls.
        Maker.ConnectSquares(AboveMap.GetSquares(Section.Region), AboveEntranceSquare, PunchSquare =>
        {
          var PunchBarrier = PunchSquare.Wall.Barrier;
          Generator.PlaceFloor(PunchSquare, PunchBarrier.Underlay);

          PunchSquare.SetWall(null);

          Generator.PlaceLockedVerticalDoor(PunchSquare, AboveVariant.MainGate, SecretBarrier: PunchBarrier);
        });

        var HallsName = Generator.EscapedModuleTerm(OpusTerms.Halls);
        if (!Generator.Adventure.World.HasSite(HallsName))
        {
          var BelowSite = Generator.Adventure.World.AddSite(HallsName);
          var BelowCount = (1.d2() + 3).Roll(); // 4-5

          var BelowEntranceSquare = AboveEntranceSquare;

          for (var BelowIndex = 1; BelowIndex <= BelowCount; BelowIndex++)
          {
            var HallSize = 25;

            var BelowMap = Generator.Adventure.World.AddMap(HallsName + " " + BelowIndex, HallSize, HallSize);
            BelowMap.SetDifficulty(Section.Distance + BelowIndex);
            BelowMap.SetTerminal(BelowIndex == BelowCount);
            BelowMap.SetAtmosphere(Codex.Atmospheres.cavern);
            var BelowLevel = BelowSite.AddLevel(BelowIndex, BelowMap);

            var BelowVariant = new HallVariant()
            {
              MainBarrier = Codex.Barriers.stone_wall,
              MainGround = Codex.Grounds.stone_floor,
              MainGate = Codex.Gates.wooden_door,
              FocusGroundArray = new Ground[] { Codex.Grounds.wooden_floor, Codex.Grounds.marble_floor },
              MinimumDifficulty = Generator.MinimumDifficulty(BelowMap),
              MaximumDifficulty = Generator.MaximumDifficulty(BelowMap),
              CriticalDistance = 8,
              EnterAction = (EnterSquare) =>
              {
                Debug.Assert(EnterSquare.Wall == null);
                Debug.Assert(BelowEntranceSquare.Wall == null);

                BelowLevel.SetTransitions(EnterSquare, null);
                Generator.PlacePassage(BelowEntranceSquare, Codex.Portals.stone_staircase_down, EnterSquare);
                Generator.PlacePassage(EnterSquare, Codex.Portals.stone_staircase_up, BelowEntranceSquare);
              },
              ExitAction = (ExitSquare) =>
              {
                Debug.Assert(ExitSquare.Wall == null);

                if (BelowCount > 1)
                {
                  if (BelowIndex == BelowCount)
                  {
                    // escape portal from the Halls.
                    Generator.PlacePassage(ExitSquare, Codex.Portals.transportal, AboveEntranceSquare);

                    // boss and artifact.
                    var BossCharacter = Maker.NewEvilCharacter(ExitSquare, Maker.SelectUniqueEntity(Codex.Entities.Lord_Surtur, Codex.Entities.Thoth_Amon, Codex.Entities.Colonel_Blood, Codex.Entities.Count_Dracula));
                    Generator.AcquireUnique(ExitSquare, BossCharacter, Codex.Qualifications.specialist);
                    Generator.PlaceCharacter(ExitSquare, BossCharacter);
                  }
                  else
                  {
                    Generator.PlacePassage(ExitSquare, Codex.Portals.stone_staircase_down, null); // placeholder.
                    BelowLevel.SetTransitions(BelowLevel.UpSquare, ExitSquare);
                    BelowEntranceSquare = ExitSquare;
                  }
                }
              }
            };

            BuildHall(BelowMap, BelowMap.Region, BelowVariant);

            BelowMap.AddArea(HallsName).AddMapZones();
          }
        }

        // TODO:
        // * 

        BuildStop();
      }

      private void BuildHall(Map HallMap, Region HallRegion, HallVariant HallVariant)
      {
        var Plan = new OpusPlanner()
        {
          StartMinSize = 4,
          StartMaxSize = 9,
          SectorMinSize = 4,
          SectorMaxSize = 9,
          CriticalDistance = HallVariant.CriticalDistance
        }.Generate(HallRegion.Width, HallRegion.Height);
        Plan.PlotCriticalPaths();

        var HallIsLit = true;

        Region ShiftRegion(Region Region) => new Region(HallRegion.Left + Region.Left, HallRegion.Top + Region.Top, HallRegion.Left + Region.Right, HallRegion.Top + Region.Bottom);

        // erase anything already placed so we can build the hall room.
        foreach (var Sector in Plan.Sectors)
        {
          var SectorRegion = ShiftRegion(Sector.Region);
          foreach (var HallSquare in HallMap.GetSquares(SectorRegion))
            HallSquare.SetFloor(null);
        }

        // place the hall rooms.
        foreach (var Sector in Plan.Sectors)
        {
          var SectorRegion = ShiftRegion(Sector.Region);

          var SectorZone = HallMap.AddZone();
          SectorZone.SetDifficulty(HallVariant.MaximumDifficulty);
          SectorZone.AddRegion(SectorRegion);
          SectorZone.SetLit(HallIsLit);
          Debug.Assert(SectorZone.HasSquares());

          var FocusGround = HallVariant.FocusGroundArray.GetRandom();

          if (SectorRegion.Width >= 5 && SectorRegion.Height >= 5)
          {
            Generator.PlaceRoom(HallMap, HallVariant.MainBarrier, HallVariant.MainGround, SectorRegion);

            if (Chance.OneIn5.Hit())
            {
              Generator.PlaceFloorFill(HallMap, FocusGround, SectorRegion.Reduce(1));

              var SectorTrigger = Chance.OneIn2.Hit() ? SectorZone.InsertTrigger() : null;

              SectorTrigger?.Add(Delay.FromTurns(20), Codex.Tricks.automatic_locking);

              foreach (var HallSquare in HallMap.GetCornerSquares(SectorRegion.Reduce(2)))
              {
                Generator.PlaceBoulder(HallSquare, Codex.Blocks.statue, IsRigid: true).SetPrisoner(null);

                SectorTrigger?.Add(Delay.FromTurns(20), Codex.Tricks.animated_objects).SetTarget(HallSquare);
              }
            }
            else if (Chance.OneIn5.Hit())
            {
              Generator.PlaceFloorFrame(HallMap, FocusGround, SectorRegion.Reduce(1));
            }
            else if (Chance.OneIn5.Hit())
            {
              Generator.PlaceFloorFrame(HallMap, FocusGround, SectorRegion);
            }
            else if (Chance.OneIn5.Hit())
            {
              foreach (var HallSquare in HallMap.GetSquares(SectorRegion.Reduce(1)).Where(S => S.X % 2 == 0))
                Generator.PlaceFloor(HallSquare, FocusGround);
            }
            else if (Chance.OneIn5.Hit())
            {
              foreach (var HallSquare in HallMap.GetSquares(SectorRegion.Reduce(1)).Where(S => S.Y % 2 == 0))
                Generator.PlaceFloor(HallSquare, FocusGround);
            }
          }
          else
          {
            Generator.PlaceRoom(HallMap, HallVariant.MainBarrier, FocusGround, SectorRegion);
          }

          foreach (var Link in Sector.Links)
          {
            var LinkRegion = ShiftRegion(Link.Region);

            var LinkSquare = HallMap.GetSquares(LinkRegion).GetRandomOrNull();

            if (LinkSquare != null)
            {
              LinkSquare.SetWall(null);
              Generator.PlaceFloor(LinkSquare, HallVariant.MainGround);
              Generator.PlaceDoor(LinkSquare, HallVariant.MainGate, DoorOrientation.Vertical, SecretBarrier: HallVariant.MainBarrier);
            }
          }
        }

        // map entrance and exit.
        var EnterRegion = ShiftRegion(Plan.Sectors.Where(B => B.Critical).FirstOrDefault()?.Region ?? HallRegion);
        var EnterSquare = HallMap.GetSquares(EnterRegion).Where(S => S.Floor != null && S.Wall == null && S.Passage == null && S.Door == null).GetRandomOrNull();
        HallVariant.EnterAction(EnterSquare);

        if (HallVariant.ExitAction != null)
        {
          var ExitRegion = ShiftRegion(Plan.Sectors.Where(B => B.Critical && B.Region != EnterRegion).LastOrDefault()?.Region ?? HallRegion);
          var ExitSquare = HallMap.GetSquares(ExitRegion).Where(S => S.Floor != null && S.Passage == null && S.Wall == null && S.Door == null).GetRandomOrNull();
          HallVariant.ExitAction(ExitSquare);
        }

        var Zoo = Generator.GetZooProbability(HallVariant.MaximumDifficulty).GetRandomOrNull();
        var ZooSector = Zoo != null ? Plan.Sectors.Where(S => !S.Critical).LastOrDefault() : null;

        foreach (var Sector in Plan.Sectors)
        {
          var SectorRegion = ShiftRegion(Sector.Region);

          var FloorSquareList = HallMap.GetSquares(SectorRegion.Reduce(1)).ToDistinctList();
          Debug.Assert(FloorSquareList.All(S => S.GetDifficulty() > 0), "Generator.PlaceRoom* only work correctly when the difficulty is derivable.");

          if (Sector == ZooSector)
          {
            // place the intended zoo.
            Generator.PlaceZoo(FloorSquareList, Zoo, HallVariant.MinimumDifficulty, HallVariant.MaximumDifficulty);

            // zoo doors are locked to keep them in.
            foreach (var HallSquare in HallMap.GetFrameSquares(SectorRegion).Where(S => S.Door != null))
              HallSquare.Door.SetState(DoorState.Locked);
          }
          else
          {
            Generator.PlaceRoomFixtures(FloorSquareList);

            var DetailSquareList = FloorSquareList.Where(Generator.CanPlaceTrap).ToDistinctList();
            Generator.PlaceRoomTraps(DetailSquareList, HallVariant.MaximumDifficulty);
            Generator.PlaceRoomCoins(DetailSquareList);
            Generator.PlaceRoomAssets(DetailSquareList);
            Generator.PlaceRoomHorde(FloorSquareList);
            Generator.PlaceRoomCharacter(FloorSquareList);
            Maker.PlaceRoomBoulders(DetailSquareList);
          }
        }

        Generator.RepairMap(HallMap, HallRegion);

        // remove double walls.
        WallSegment? GetWall(Square Square, Direction Direction) => Square.Adjacent(Direction)?.Wall?.Segment;
        void RemoveDoubleWall(Square Square)
        {
          Square.SetWall(null);
          Generator.PlaceFloor(Square, HallVariant.MainGround);

          if (Chance.OneIn2.Hit())
            Generator.PlaceBoulder(Square, Codex.Blocks.statue, IsRigid: false).SetPrisoner(null);
          else
            Generator.PlaceBoulder(Square, Codex.Blocks.wooden_barrel, IsRigid: false);
        }

        foreach (var HallSquare in HallMap.GetSquares(HallRegion))
        {
          if (GetWall(HallSquare, Direction.Self) == WallSegment.Vertical)
          {
            if (GetWall(HallSquare, Direction.East) == WallSegment.Vertical)
              RemoveDoubleWall(HallSquare);
            else if (GetWall(HallSquare, Direction.West) == WallSegment.Vertical)
              RemoveDoubleWall(HallSquare);
          }
          else if (GetWall(HallSquare, Direction.Self) == WallSegment.Horizontal)
          {
            if (GetWall(HallSquare, Direction.South) == WallSegment.Horizontal)
              RemoveDoubleWall(HallSquare);
            else if (GetWall(HallSquare, Direction.North) == WallSegment.Horizontal)
              RemoveDoubleWall(HallSquare);
          }
        }

        Generator.RepairWalls(HallMap, HallRegion);

        // TODO:
        // * special rooms - shops, shrines.
        // * boss gimmick.
      }

      private sealed class HallVariant
      {
        public Barrier MainBarrier;
        public Ground MainGround;
        public Ground[] FocusGroundArray;
        public Gate MainGate;
        public int MinimumDifficulty;
        public int MaximumDifficulty;
        public int CriticalDistance;
        public Action<Square> EnterAction;
        public Action<Square> ExitAction;
      }
    }

    private sealed class HordeBuilder : Builder
    {
      public HordeBuilder(OpusMaker Maker)
        : base(Maker)
      {
      }

      public void Build(OpusClearing HordeClearing)
      {
        var Horde = Generator.RandomHorde(HordeClearing.Section.MinimumDifficulty, HordeClearing.Section.MaximumDifficulty);

        if (Horde != null)
        {
          BuildStart();

          var HordeMap = Maker.OverlandMap;
          var HordeX = HordeClearing.Circle.X;
          var HordeY = HordeClearing.Circle.Y;
          var HordeRadius = Math.Min(3, HordeClearing.Circle.Radius - 1);
          var HordeCircle = new Inv.Circle(HordeX, HordeY, HordeRadius);

          var HordeZone = HordeMap.AddZone();

          foreach (var Square in HordeMap.GetCircleInnerSquares(HordeCircle))
          {
            if (!Square.IsVoid())
              HordeZone.ForceSquare(Square);
          }

          HordeZone.SetLit(true);
          Debug.Assert(HordeZone.HasSquares());

          Generator.PlaceHorde(Horde, HordeClearing.Section.MinimumDifficulty, HordeClearing.Section.MaximumDifficulty, () => HordeZone.Squares.Where(S => S.Character == null).GetRandomOrNull());

          BuildStop();
        }
      }
    }

    private sealed class KoboldFortBuilder : Builder
    {
      public KoboldFortBuilder(OpusMaker Maker)
        : base(Maker)
      {
      }

      public void Build(OpusSection Section, Map FortMap, OpusClearing FortClearing, Item DragonScalesItem)
      {
        BuildStart();

        var FortX = FortClearing.Circle.X;
        var FortY = FortClearing.Circle.Y;
        var FortRadius = RandomSupport.NextRange(3, Math.Min(5, Math.Max(3, FortClearing.Circle.Radius - 2)));
        var FortCircle = new Inv.Circle(FortX, FortY, FortRadius);
        var FortRegion = new Region(FortCircle).Expand(1);

        var MiddleSquare = FortMap[FortX, FortY];
        Generator.PlaceFloor(MiddleSquare, Codex.Grounds.stone_floor);
        Generator.PlaceBoulder(MiddleSquare, Codex.Blocks.clay_boulder, IsRigid: true);

        // artificially circular fort.
        foreach (var FortSquare in FortMap.GetCircleInnerSquares(FortCircle))
        {
          FortSquare.SetLit(true);
          Generator.PlaceFloor(FortSquare, Codex.Grounds.stone_floor);
        }

        // eroding the circular fort body.
        foreach (var FortSquare in FortMap.GetSquares(FortRegion))
        {
          if (FortSquare.Floor?.Ground == Codex.Grounds.stone_floor)
          {
            if (FortSquare.GetNeighbourSquares().Any(S => S.Floor?.Ground == Codex.Grounds.dirt) && Chance.OneIn8.Hit())
              Generator.PlaceFloor(FortSquare, Codex.Grounds.dirt);
          }
          else if (FortSquare.Floor?.Ground == Codex.Grounds.dirt)
          {
            if (FortSquare.GetNeighbourSquares().Any(S => S.Floor?.Ground == Codex.Grounds.stone_floor) && Chance.OneIn8.Hit())
              Generator.PlaceFloor(FortSquare, Codex.Grounds.stone_floor);
          }
        }

        // fort walls.
        foreach (var FortSquare in FortMap.GetSquares(FortRegion.Expand(1)))
        {
          if (FortSquare.Floor?.Ground == Codex.Grounds.dirt && FortSquare.Boulder == null && FortSquare.GetAdjacentSquares().Any(S => S.Floor?.Ground == Codex.Grounds.stone_floor))
          {
            FortSquare.SetFloor(null);
            Generator.PlaceWall(FortSquare, Codex.Barriers.wooden_wall, WallStructure.Solid, WallSegment.Pillar);
          }
        }

        // sanctum walls.
        foreach (var FortSquare in MiddleSquare.GetAdjacentSquares())
        {
          FortSquare.SetFloor(null);
          Generator.PlaceWall(FortSquare, Codex.Barriers.wooden_wall, WallStructure.Solid, WallSegment.Pillar);
        }

        // convert stone floor back to dirt (kobolds are dirty).
        var FortZone = FortMap.AddZone();

        foreach (var FortSquare in FortMap.GetSquares(FortRegion.Expand(1)))
        {
          if (FortSquare.Floor?.Ground == Codex.Grounds.stone_floor)
          {
            Generator.PlaceFloor(FortSquare, Codex.Grounds.dirt);
            FortZone.AddSquare(FortSquare);
          }
          else if (FortSquare.Wall?.Barrier == Codex.Barriers.wooden_wall)
          {
            FortZone.AddSquare(FortSquare);

            if (FortSquare.Wall != null && FortSquare.IsFlat())
            {
              FortSquare.SetWall(null);
              Generator.PlaceFloor(FortSquare, Codex.Grounds.dirt);
              Generator.PlaceTrap(FortSquare, Codex.Devices.spiked_pit, Revealed: true);
            }
          }
        }

        Debug.Assert(FortZone.HasSquares());

        // fill with kobolds.
        var KoboldHorde = Codex.Hordes.kobold;
        foreach (var HordeIndex in (FortZone.Squares.Count / KoboldHorde.GetPotential().Maximum).NumberSeries())
          Generator.PlaceHorde(KoboldHorde, 0, Section.MaximumDifficulty, () => FortZone.Squares.Where(S => S != MiddleSquare && S.Character == null && S.Wall == null && S.Trap == null).GetRandomOrNull());

        var SectionDifficulty = Section.Distance;

        foreach (var KoboldSquare in FortZone.Squares)
        {
          var KoboldCharacter = KoboldSquare.Character;

          if (KoboldCharacter != null)
          {
            KoboldCharacter.AcquireTalent(Codex.Properties.jumping); // so they can jump into the pits without dying like idiots.

            // promote kobolds so they are not completely outmatched.
            var DifficultyGap = SectionDifficulty - KoboldCharacter.Entity.Difficulty;
            if (DifficultyGap > 5)
              Generator.PromoteCharacter(KoboldCharacter, DifficultyGap - 5);

            // make sure they have ranged weapons.
            if (!KoboldCharacter.Inventory.HasEquipment(Codex.Slots.quiver))
            {
              if (DifficultyGap <= 10 || KoboldCharacter.Inventory.HasEquipment(Codex.Slots.offhand))
              {
                Generator.EquipCharacter(KoboldCharacter, Codex.Slots.quiver, Generator.NewSpecificAsset(KoboldSquare, Codex.Items.dart));
              }
              else
              {
                var DualWielding = KoboldCharacter.Inventory.HasEquipment(Codex.Slots.main_hand);

                Generator.EquipCharacter(KoboldCharacter, DualWielding ? Codex.Slots.offhand : Codex.Slots.main_hand, Generator.NewSpecificAsset(KoboldSquare, Codex.Items.crossbow));
                Generator.EquipCharacter(KoboldCharacter, Codex.Slots.quiver, Generator.NewSpecificAsset(KoboldSquare, Codex.Items.crossbow_bolt));

                KoboldCharacter.ForceCompetency(Codex.Skills.crossbow, Codex.Qualifications.proficient);
                if (DualWielding)
                  KoboldCharacter.ForceCompetency(Codex.Skills.dual_wielding, Codex.Qualifications.proficient);
              }
            }

            if (KoboldCharacter.Entity == Codex.Entities.kobold_king)
            {
              // kobold king gets appropriate variant scales armour.
              if (DragonScalesItem != null && !KoboldCharacter.Inventory.HasEquipment(Codex.Slots.suit))
                Generator.EquipCharacter(KoboldCharacter, Codex.Slots.suit, Generator.NewSpecificAsset(KoboldSquare, DragonScalesItem));

              // kobold king is armed with a dangerous wand.
              if (DifficultyGap > 10)
              {
                // there's not enough wands to associate with each dragon variant (missing acid, poison, disintegration).
                var WandArray = new[] { Codex.Items.wand_of_cold, Codex.Items.wand_of_fire, Codex.Items.wand_of_lightning, Codex.Items.wand_of_fireball, Codex.Items.wand_of_iceball };

                KoboldCharacter.Inventory.Carried.Add(Generator.NewSpecificAsset(KoboldSquare, WandArray.GetRandom()));
              }
            }
          }
        }

        // TODO:
        // * 

        BuildStop();
      }
    }

    private sealed class LakeBuilder : Builder
    {
      public LakeBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.LakeVariance = new Variance<LakeVariant>
        (
          new LakeVariant
          {
            Name = OpusTerms.Lake,
            BodyGround = Codex.Grounds.water,
            ShoreGround = Codex.Grounds.grass,
            ShoreBarrier = Codex.Barriers.tree,
            IslandGround = Codex.Grounds.sand,
            JettyPlatform = Codex.Platforms.wooden_bridge
          },
          new LakeVariant
          {
            Name = OpusTerms.Crater,
            BodyGround = Codex.Grounds.chasm,
            ShoreGround = Codex.Grounds.moss,
            ShoreBarrier = Codex.Barriers.shroom,
            IslandGround = Codex.Grounds.moss,
            JettyPlatform = Codex.Platforms.wooden_bridge
          },
          new LakeVariant
          {
            Name = OpusTerms.Ice_Lake,
            BodyGround = Codex.Grounds.ice,
            ShoreGround = Codex.Grounds.snow,
            ShoreBarrier = null,
            IslandGround = Codex.Grounds.water,
            JettyPlatform = Codex.Platforms.wooden_bridge
          },
          new LakeVariant
          {
            Name = OpusTerms.Lava_Lake,
            BodyGround = Codex.Grounds.lava,
            ShoreGround = Codex.Grounds.obsidian_floor,
            ShoreBarrier = null,
            IslandGround = Codex.Grounds.obsidian_floor,
            JettyPlatform = Codex.Platforms.crystal_bridge
          }
        );
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        var LakeVariant = LakeVariance.NextVariant();
        Section.OverlandAreaName = LakeVariant.Name;

        var LakeMap = Maker.OverlandMap;
        var LakeClearing = Section.LargestClearing;
        var LakeX = LakeClearing.Circle.X;
        var LakeY = LakeClearing.Circle.Y;
        var LakeRadius = RandomSupport.NextRange(3, Math.Max(3, LakeClearing.Circle.Radius - 2));
        var LakeCircle = new Inv.Circle(LakeX, LakeY, LakeRadius);
        var LakeRegion = new Region(LakeCircle).Expand(1);

        // artificially circular lake.
        foreach (var LakeSquare in LakeMap.GetCircleInnerSquares(LakeCircle))
        {
          LakeSquare.SetLit(true);
          Generator.PlaceFloor(LakeSquare, LakeVariant.BodyGround);
        }

        // eroding the circular lake body.
        foreach (var LakeSquare in LakeMap.GetSquares(LakeRegion))
        {
          if (LakeSquare.Floor?.Ground == LakeVariant.BodyGround)
          {
            if (LakeSquare.GetNeighbourSquares().Any(S => S.Floor?.Ground == Codex.Grounds.dirt) && Chance.OneIn8.Hit())
              Generator.PlaceFloor(LakeSquare, Codex.Grounds.dirt);
          }
          else if (LakeSquare.Floor?.Ground == Codex.Grounds.dirt)
          {
            if (LakeSquare.GetNeighbourSquares().Any(S => S.Floor?.Ground == LakeVariant.BodyGround) && Chance.OneIn8.Hit())
              Generator.PlaceFloor(LakeSquare, LakeVariant.BodyGround);
          }
        }

        if (LakeRadius >= 5)
        {
          // room for an island?
          var IslandRadius = Math.Min(2, LakeRadius - 4);
          var IslandCircle = new Inv.Circle(LakeX, LakeY, IslandRadius);

          foreach (var LakeSquare in LakeMap.GetCircleInnerSquares(IslandCircle))
          {
            LakeSquare.SetLit(true);
            Generator.PlaceFloor(LakeSquare, LakeVariant.IslandGround);
          }

          // eroding the circular island.
          foreach (var LakeSquare in LakeMap.GetSquares(LakeRegion.Reduce(LakeRadius - IslandRadius)))
          {
            if (LakeSquare.Floor?.Ground == LakeVariant.BodyGround)
            {
              if (LakeSquare.GetNeighbourSquares().Any(S => S.Floor?.Ground == LakeVariant.IslandGround) && Chance.OneIn8.Hit())
                Generator.PlaceFloor(LakeSquare, LakeVariant.IslandGround);
            }
            else if (LakeSquare.Floor?.Ground == LakeVariant.IslandGround)
            {
              if (LakeSquare.GetNeighbourSquares().Any(S => S.Floor?.Ground == LakeVariant.BodyGround) && Chance.OneIn8.Hit())
                Generator.PlaceFloor(LakeSquare, LakeVariant.BodyGround);
            }
          }
        }

        // shoreline.
        foreach (var LakeSquare in LakeMap.GetSquares(LakeRegion.Expand(1)))
        {
          if (LakeSquare.Floor?.Ground == Codex.Grounds.dirt && LakeSquare.GetAdjacentSquares().Any(S => S.Floor?.Ground == LakeVariant.BodyGround))
          {
            if (LakeVariant.ShoreGround != null)
              Generator.PlaceFloor(LakeSquare, LakeVariant.ShoreGround);

            if (LakeVariant.ShoreBarrier != null && !LakeSquare.GetAdjacentSquares().Any(S => S.Wall != null || S.IsVoid()) && Chance.OneIn8.Hit())
              Generator.PlaceWall(LakeSquare, LakeVariant.ShoreBarrier, WallStructure.Solid, WallSegment.Pillar);
          }
        }

        // jetty.
        var MiddleSquare = LakeMap[LakeX, LakeY];
        var JettySquare = MiddleSquare.GetNeighbourSquares(LakeRadius - RandomSupport.NextRange(1, 2)).Where(S => S.Floor?.Ground == LakeVariant.BodyGround).GetRandomOrNull();
        if (JettySquare != null)
        {
          var JettyDirection = MiddleSquare.AsDirection(JettySquare);
          do
          {
            Generator.PlaceBridge(JettySquare, LakeVariant.JettyPlatform, JettyDirection.IsVertical() ? BridgeOrientation.Vertical : BridgeOrientation.Horizontal);

            JettySquare = JettySquare.Adjacent(JettyDirection);

            // erase any trees that are in the way of the jetty.
            if (JettySquare != null && LakeVariant.ShoreBarrier != null && JettySquare.Wall?.Barrier == LakeVariant.ShoreBarrier)
              JettySquare.SetWall(null);
          }
          while (JettySquare != null && JettySquare.Floor?.Ground == LakeVariant.BodyGround);
        }

        // sunken chest.
        var ChestChance = Chance.OneIn2;
        if (ChestChance.Hit())
        {
          var ChestAsset = Generator.NewSpecificAsset(MiddleSquare, Codex.Items.chest);
          ChestAsset.Container.Locked = true;

          var MercenaryEntity = Maker.RandomMercenaryEntity(Section);
          if (MercenaryEntity != null && Chance.OneIn4.Hit())
          {
            // containing a corpse of a mercenary and their gear.
            var MercenaryCharacter = Generator.NewCharacter(MiddleSquare, MercenaryEntity);

            foreach (var MercenaryAsset in MercenaryCharacter.Inventory.RemoveAllAssets())
            {
              if (MercenaryAsset.Container != null)
                MiddleSquare.PlaceAsset(MercenaryAsset);
              else
                ChestAsset.Container.Stash.Add(MercenaryAsset);
            }

            ChestAsset.Container.Stash.Add(Generator.CorpseCharacter(MercenaryCharacter));
          }
          else
          {
            // trapped because it contains loot.
            ChestAsset.Container.Trap = Maker.RandomContainerTrap(MiddleSquare, Section.Distance);
            Generator.StockContainer(MiddleSquare, ChestAsset);
          }

          // 10% chance of an artifact.
          if (Chance.OneIn10.Hit())
            ChestAsset.Container.Stash.Add(Generator.GenerateUniqueAsset(MiddleSquare));

          if (MiddleSquare.Floor?.Ground == Codex.Grounds.ice)
            Generator.PlaceFloor(MiddleSquare, Codex.Grounds.water);
          else if (MiddleSquare.Floor?.Ground == Codex.Grounds.lava || MiddleSquare.Floor?.Ground == Codex.Grounds.chasm)
            Generator.PlaceFloor(MiddleSquare, LakeVariant.IslandGround);

          MiddleSquare.PlaceAsset(ChestAsset);

          var IsSunken = MiddleSquare.IsSunken();
          var IsDangerous = LakeVariant.BodyGround == Codex.Grounds.lava || LakeVariant.BodyGround == Codex.Grounds.chasm;

          if (!IsSunken)
            Generator.PlaceTrap(MiddleSquare, Codex.Devices.pit, Revealed: true);

          // creatures spawn around the greedy player.
          var SpawnTrigger = MiddleSquare.InsertTrigger();
          SpawnTrigger.Add(Delay.Zero, IsSunken ? Codex.Tricks.watery_noodles : IsDangerous ? Codex.Tricks.arriving_bats : Codex.Tricks.pinching_crabs).SetTarget(MiddleSquare);
        }

        // water is full of marine entities.
        foreach (var LakeSquare in LakeMap.GetSquares(LakeRegion))
        {
          if (LakeSquare.Character == null)
          {
            if (LakeSquare.Floor?.Ground == Codex.Grounds.water && LakeSquare.Stash == null && Chance.OneIn8.Hit())
              Generator.PlaceRandomCharacter(LakeSquare, Section.MinimumDifficulty, Section.MaximumDifficulty);
            else if (LakeSquare.Floor?.Ground == Codex.Grounds.dirt && Chance.OneIn16.Hit())
              Generator.PlaceRandomCharacter(LakeSquare, Section.MinimumDifficulty, Section.MaximumDifficulty);
            else if (LakeSquare.Floor?.Ground == Codex.Grounds.water && LakeSquare.Stash == null && Chance.OneIn16.Hit())
              Generator.PlaceSpecificAsset(LakeSquare, Codex.Items.kelp_frond);
            else if (LakeSquare.Floor?.Ground == Codex.Grounds.water && LakeSquare.Stash == null && Chance.OneIn32.Hit())
              Generator.PlaceBoulder(LakeSquare, Codex.Blocks.stone_boulder, IsRigid: false);
          }
        }

        BuildStop();

        if (LakeVariant.BodyGround == Codex.Grounds.chasm)
          Maker.Pit.Build(Section, LakeRegion, Maker.OverlandMap, Maker.UndergroundMap, LakeVariant.ShoreGround);

        // TODO:
        // * kraken!
        // * spawn entities when on jetty?
      }

      private readonly Variance<LakeVariant> LakeVariance;

      private sealed class LakeVariant
      {
        public string Name;
        public Ground BodyGround;
        public Ground ShoreGround;
        public Barrier ShoreBarrier;
        public Ground IslandGround;
        public Platform JettyPlatform;
      }
    }

    private sealed class MazeBuilder : Builder
    {
      public MazeBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.MazeVariance = new Variance<MazeVariant>
        (
          new MazeVariant
          {
            Name = OpusTerms.Tree_Maze,
            Barrier = Codex.Barriers.tree,
            Ground = Codex.Grounds.grass,
            Feature = Codex.Features.fountain
          },
          new MazeVariant
          {
            Name = OpusTerms.Shroom_Maze,
            Barrier = Codex.Barriers.shroom,
            Ground = Codex.Grounds.moss,
            Feature = Codex.Features.grave
          },
          new MazeVariant
          {
            Name = OpusTerms.Cave_Maze,
            Barrier = Codex.Barriers.cave_wall,
            Ground = Codex.Grounds.cave_floor,
            Feature = Codex.Features.workbench
          }
        );
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        var MazeVariant = MazeVariance.NextVariant();
        Section.OverlandAreaName = MazeVariant.Name;

        var MazeMap = Maker.OverlandMap;
        var MazeRegion = Section.Region.Reduce(1);

        foreach (var MazeSquare in MazeMap.GetSquares(MazeRegion))
        {
          if (MazeSquare.Floor != null && !MazeSquare.GetAdjacentSquares().Any(S => S.IsVoid()))
          {
            Generator.PlaceFloor(MazeSquare, MazeVariant.Ground);
            Generator.PlaceSolidWall(MazeSquare, MazeVariant.Barrier, WallSegment.Pillar);
          }
        }

        foreach (var MazeClearing in Section.Clearings)
        {
          // fill with walls.
          var FillGroundChance = Chance.OneIn2;
          var IllusionWallChance = Chance.OneIn10;
          var TreantChance = MazeVariant.Barrier == Codex.Barriers.tree && Section.Distance >= (Codex.Entities.treant.Difficulty / 2) ? Chance.OneIn10 : Chance.Never;

          // punch out walls to create a maze (one of the clearings might be disconnected from the others, so we need to check them).
          var NextSquare = MazeMap[MazeClearing.Circle.X, MazeClearing.Circle.Y];
          if (NextSquare.GetAroundSquares().All(S => S.Wall != null))
          {
            var SquareStack = new Stack<Square>();
            SquareStack.Push(NextSquare);
            while (SquareStack.Count > 0)
            {
              var NeighbourArray = NextSquare.GetNeighbourSquares(2).Where(S => S.Wall != null && !S.IsEdge(MazeRegion)).ToArray();
              if (NeighbourArray.Length > 0)
              {
                var NeighbourSquare = NeighbourArray.GetRandom();
                NeighbourSquare.SetWall(null);

                var BetweenSquare = NextSquare.Adjacent(NextSquare.AsDirection(NeighbourSquare));
                if (BetweenSquare.Wall != null)
                {
                  if (IllusionWallChance.Hit())
                  {
                    BetweenSquare.Wall.SetStructure(WallStructure.Illusionary);
                  }
                  else
                  {
                    BetweenSquare.SetWall(null);
                    if (TreantChance.Hit())
                      Generator.PlaceCharacter(BetweenSquare, Codex.Entities.treant);
                  }
                }

                SquareStack.Push(NextSquare);
                NextSquare = NeighbourSquare;
              }
              else
              {
                NextSquare = SquareStack.Pop();
              }
            }
          }
        }

        Generator.RepairMap(MazeMap, MazeRegion);

        var FeatureSquareList = new Inv.DistinctList<Square>();
        foreach (var MazeSquare in MazeMap.GetSquares(MazeRegion))
        {
          if (MazeSquare.Floor?.Ground == MazeVariant.Ground && MazeSquare.Wall == null && MazeSquare.Character == null && MazeSquare.Fixture == null)
          {
            if (MazeSquare.GetAdjacentSquares().Count(S => S.Wall != null && S.Wall.IsSolid()) == 7)
            {
              // dead-end.
              FeatureSquareList.Add(MazeSquare);
            }
            else if (Chance.OneIn8.Hit())
            {
              Generator.PlaceRandomCharacter(MazeSquare, Section.MinimumDifficulty, Section.MaximumDifficulty);
            }
            else if (Chance.OneIn16.Hit())
            {
              Generator.PlaceTrap(MazeSquare, Generator.RandomDevice(MazeSquare, Section.Distance, D => !D.Descent), Revealed: false);
            }
            else if (Chance.OneIn16.Hit())
            {
              Generator.PlaceRandomAsset(MazeSquare);
            }
          }
        }

        // maximum three features in the dead-ends.
        var FeatureCount = 3;
        foreach (var FeatureIndex in FeatureCount.NumberSeries())
        {
          var FeatureSquare = FeatureSquareList.RemoveRandomOrNull();

          if (FeatureSquare == null)
            break;

          Generator.PlaceFixture(FeatureSquare, MazeVariant.Feature);
        }

        // TODO:
        // * if the maze is big enough, put a 2x2 building in the middle? entrance to what side dungeon?

        BuildStop();
      }

      private readonly Variance<MazeVariant> MazeVariance;

      private sealed class MazeVariant
      {
        public string Name;
        public Barrier Barrier;
        public Ground Ground;
        public Feature Feature;
      }
    }

    private sealed class MineBuilder : Builder
    {
      public MineBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.MineVariance = new Variance<MineVariant>
        (
          new MineVariant
          {
            Name = OpusTerms.RubyMine,
            MainGem = Codex.Items.ruby,
            OtherGem = Codex.Items.garnet,
            Golem = Codex.Entities.ruby_golem,
            //Bauble = Codex.Items.red_glass_bauble,
            //Key = Codex.Items.Ruby_Key
          },
          new MineVariant
          {
            Name = OpusTerms.SapphireMine,
            MainGem = Codex.Items.sapphire,
            OtherGem = Codex.Items.zircon,
            Golem = Codex.Entities.sapphire_golem,
            //Bauble = Codex.Items.blue_glass_bauble,
            //Key = Codex.Items.Sapphire_Key // TODO
          },
          new MineVariant
          {
            Name = OpusTerms.TopazMine,
            MainGem = Codex.Items.topaz,
            OtherGem = Codex.Items.amber,
            Golem = Codex.Entities.topaz_golem,
            //Bauble = Codex.Items.yellow_glass_bauble,
            //Key = Codex.Items.Topaz_Key // TODO
          },
          new MineVariant
          {
            Name = OpusTerms.EmeraldMine,
            MainGem = Codex.Items.emerald,
            OtherGem = Codex.Items.turquoise,
            Golem = Codex.Entities.emerald_golem,
            //Bauble = Codex.Items.green_glass_bauble,
            //Key = Codex.Items.Emerald_Key // TODO
          },
          new MineVariant
          {
            Name = OpusTerms.DiamondMine,
            MainGem = Codex.Items.diamond,
            OtherGem = Codex.Items.opal,
            Golem = Codex.Entities.diamond_golem,
            //Bauble = Codex.Items.white_glass_bauble,
            //Key = Codex.Items.Diamond_Key // TODO
          }
        );
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        var MineVariant = MineVariance.NextVariant();
        Section.OverlandAreaName = MineVariant.Name;

        var MineMap = Maker.OverlandMap;

        // blockers, tunnellers and phasers.
        var MineEntityArray = Codex.Entities.List.Where(E => E.IsEncounter &&
          (
          E.Attacks.Count == 0 ||
          E.Startup.Talents.ContainsAny(new[] { Codex.Properties.tunnelling, Codex.Properties.phasing })
          )).ToArray();

        // occupying the surface of the mine.
        var MineHorde = Codex.Hordes.orc;

        var EnterList = new Inv.DistinctList<Square>();

        foreach (var MineClearing in Section.Clearings)
        {
          var RoomSize = 3;
          if (MineClearing.Circle.Radius < 4)
            RoomSize--;

          var RoomRegion = new Region(MineClearing.Circle.X - RoomSize, MineClearing.Circle.Y - RoomSize, MineClearing.Circle.X + RoomSize, MineClearing.Circle.Y + RoomSize);

          if (MineMap.GetSquares(RoomRegion).All(S => S.Floor?.Ground == Codex.Grounds.dirt && S.Wall == null))
          {
            var RoomZone = MineMap.AddZone();
            RoomZone.AddRegion(RoomRegion);
            RoomZone.SetLit(false);
            Debug.Assert(RoomZone.HasSquares());

            Generator.PlaceRoom(MineMap, Codex.Barriers.cave_wall, Codex.Grounds.cave_floor, RoomRegion);

            var EntrancePoint = RoomRegion.Midpoint();
            var EntranceSquare = MineMap[EntrancePoint.X, EntrancePoint.Y];

            foreach (var RoomSquare in EntranceSquare.GetNeighbourSquares(RoomSize))
            {
              RoomSquare.SetWall(null);
              Generator.PlaceFloor(RoomSquare, Codex.Grounds.cave_floor);
              //Generator.PlaceClosedHorizontalDoor(RoomSquare, Codex.Gates.wooden_door, Codex.Barriers.wooden_wall);
            }

            // wooden corner struts.
            foreach (var RoomSquare in EntranceSquare.GetCornerSquares(RoomSize))
              RoomSquare.Wall.SetBarrier(Codex.Barriers.wooden_wall);

            // boulders and pits.
            foreach (var RoomSquare in EntranceSquare.GetAdjacentSquares(1))
            {
              if (RoomSquare.X != EntranceSquare.X && RoomSquare.Y != EntranceSquare.Y)
              {
                if (Chance.ThreeIn4.Hit())
                  Generator.PlaceBoulder(RoomSquare, Codex.Blocks.clay_boulder, IsRigid: true);
                else
                  Generator.PlaceTrap(RoomSquare, Codex.Devices.pit, Revealed: true);
              }
            }

            Generator.PlaceHorde(MineHorde, Section.MinimumDifficulty, Section.MaximumDifficulty, () => MineMap.GetSquares(RoomRegion.Reduce(1)).Where(S => Generator.CanPlaceCharacter(S)).GetRandomOrNull());

            EnterList.Add(EntranceSquare);
          }
          else
          {
            Generator.PlaceHorde(MineHorde, Section.MinimumDifficulty, Section.MaximumDifficulty, () => MineMap.GetCircleInnerSquares(MineClearing.Circle).Where(S => S.Floor != null && S.Character == null).GetRandomOrNull());
          }
        }

        if (EnterList.Count == 0)
        {
          Debug.Fail("Must be at least one possible entrance to the mine.");
          return;
        }

        var MineName = Generator.EscapedModuleTerm(MineVariant.Name);
        if (!Generator.Adventure.World.HasSite(MineName))
        {
          var BelowSite = Generator.Adventure.World.AddSite(MineName);
          const int BelowSize = 40;

          var BelowMap = Generator.Adventure.World.AddMap(MineName, BelowSize, BelowSize);
          BelowMap.SetDifficulty(Section.Distance + 1);
          BelowMap.SetTerminal(true);
          BelowMap.SetAtmosphere(Codex.Atmospheres.cavern);
          var BelowLevel = BelowSite.AddLevel(1, BelowMap);

          const int ShaftBracket = 2;
          const int ShaftSize = BelowSize / ShaftBracket;

          Region GenerateShaft()
          {
            var ShaftVertical = RandomSupport.NextRange(0, 1) == 0;
            var ShaftLength = RandomSupport.NextRange(5, 10);
            var ShaftX = ShaftVertical ? RandomSupport.NextRange(1, ShaftSize - 2) : RandomSupport.NextRange(1, ShaftSize - ShaftLength - 2);
            var ShaftY = ShaftVertical ? RandomSupport.NextRange(1, ShaftSize - ShaftLength - 2) : RandomSupport.NextRange(1, ShaftSize - 2);

            var ShaftRegion = new Region(
              (ShaftX * ShaftBracket) - 1,
              (ShaftY * ShaftBracket) - 1,
              ((ShaftX + (ShaftVertical ? 0 : ShaftLength)) * ShaftBracket) + 1,
              ((ShaftY + (ShaftVertical ? ShaftLength : 0)) * ShaftBracket) + 1);

            return ShaftRegion;
          }

          var ShaftCount = RandomSupport.NextNumber(15, 25);
          foreach (var Shaft in Maker.RandomConnectedRegions(ShaftCount, GenerateShaft))
          {
            foreach (var RoomSquare in BelowMap.GetFrameSquares(Shaft).Where(S => S.IsVoid()))
              Generator.PlaceSolidWall(RoomSquare, Codex.Barriers.cave_wall, WallSegment.Pillar);

            foreach (var RoomSquare in BelowMap.GetSquares(Shaft).Where(S => S.IsVoid()))
              Generator.PlaceFloor(RoomSquare, Codex.Grounds.cave_floor);
          }

          void PlaceGem(Square Square) => Generator.PlaceSpecificAsset(Square, Chance.OneIn2.Hit() ? MineVariant.MainGem : MineVariant.OtherGem);

          // start in the centermost available point in the mine.
          var EnterSquare = EnterList.GetRandom();
          var ExitSquare = Generator.ExpandingFindSquare(BelowMap.Midpoint, BelowSize / 2, S => S.Floor != null);

          BelowLevel.SetTransitions(ExitSquare, null);

          Generator.PlacePassage(EnterSquare, Codex.Portals.clay_staircase_down, ExitSquare);
          Generator.PlacePassage(ExitSquare, Codex.Portals.clay_staircase_up, EnterSquare);

          Maker.ConnectSquares(BelowMap.GetSquares(BelowMap.Region), ExitSquare, PunchSquare =>
          {
            var PunchBarrier = PunchSquare.Wall.Barrier;
            PunchSquare.SetWall(null);
            Generator.PlaceFloor(PunchSquare, PunchBarrier.Underlay);

            if (PunchSquare.Passage == null)
            {
              if (Chance.OneIn10.Hit())
              {
                Generator.PlaceSpecificAsset(PunchSquare, Codex.Items.rock);
                Generator.PlaceSpecificAsset(PunchSquare, Codex.Items.rock);
                Generator.PlaceSpecificAsset(PunchSquare, Codex.Items.rock);
              }

              if (Chance.OneIn10.Hit())
                Generator.PlaceTrap(PunchSquare, Chance.OneIn2.Hit() ? Codex.Devices.falling_rock_trap : Codex.Devices.pit, Revealed: false);
            }
          });

          // dead-ends might have a gem under a boulder.
          foreach (var EndSquare in BelowMap.GetSquares(BelowMap.Region).Where(S => S.Floor != null && S.Passage == null && S.GetAdjacentSquares().Count(S => S.Wall != null) == 7 && S.GetNeighbourSquares().Count(S => S.Floor != null && S.Door == null) == 1))
          {
            var Mining = false;

            foreach (var WallSquare in EndSquare.GetNeighbourSquares().Where(S => S.Wall != null && !S.HasAssets() && S.GetNeighbourSquares().Any(V => V.IsVoid())))
            {
              if (Chance.OneIn2.Hit())
              {
                WallSquare.SetWall(null);
                Generator.PlaceFloor(WallSquare, Codex.Grounds.cave_floor);
                Generator.PlaceBoulder(WallSquare, Codex.Blocks.clay_boulder, IsRigid: false);

                Mining = true;
              }

              if (Chance.OneIn4.Hit())
              {
                PlaceGem(WallSquare);

                Mining = true;
              }
            }

            if (Chance.OneIn4.Hit())
            {
              Generator.PlaceTrap(EndSquare, Mining ? Codex.Devices.explosive_trap : Codex.Devices.pit, Revealed: true);
            }
            else if (Chance.ThreeIn4.Hit())
            {
              Generator.PlaceCharacter(EndSquare, MineEntityArray);

              var EndCharacter = EndSquare.Character;
              if (EndCharacter != null)
              {
                Maker.SnoozeCharacter(EndCharacter);

                if (Chance.ThreeIn4.Hit())
                {
                  // TODO: check carry capacity and drop on the ground if too heavy?
                  EndCharacter.Inventory.Carried.Add(Generator.NewRandomAsset(EndSquare));
                  Generator.OutfitCharacter(EndCharacter);
                }
              }
            }
          }

          // reinforce mine corridors with wooden struts.
          bool IsStrut(Square Square)
          {
            bool IsFloor(Direction Direction)
            {
              var Result = Square.Adjacent(Direction);
              return Result != null && Result.Floor != null && Result.Boulder == null;
            }

            var Result = false;

            if (IsFloor(Direction.NorthEast) && IsFloor(Direction.North) && IsFloor(Direction.East))
            {
              if (Result)
                return false;

              Result = true;
            }

            if (IsFloor(Direction.SouthEast) && IsFloor(Direction.South) && IsFloor(Direction.East))
            {
              if (Result)
                return false;

              Result = true;
            }

            if (IsFloor(Direction.NorthWest) && IsFloor(Direction.North) && IsFloor(Direction.West))
            {
              if (Result)
                return false;

              Result = true;
            }

            if (IsFloor(Direction.SouthWest) && IsFloor(Direction.South) && IsFloor(Direction.West))
            {
              if (Result)
                return false;

              Result = true;
            }

            return Result;
          }

          // replace cave walls with wooden wall struts.
          foreach (var MineSquare in BelowMap.GetSquares().Where(S => S.Wall != null && IsStrut(S)))
            MineSquare.Wall.SetBarrier(Codex.Barriers.wooden_wall);

          // wooden floors between wooden pillars.
          foreach (var MineSquare in BelowMap.GetSquares().Where(S => S.Floor?.Ground == Codex.Grounds.cave_floor && S.GetNeighbourSquares().Count(N => N.Wall?.Barrier == Codex.Barriers.wooden_wall) >= 2))
            Generator.PlaceFloor(MineSquare, Codex.Grounds.wooden_floor);

          // place workbenches on perfect wooden nexus areas.
          foreach (var MineSquare in BelowMap.GetSquares().Where(S => S.Floor?.Ground == Codex.Grounds.cave_floor && S.GetNeighbourSquares().Count(N => N.Floor?.Ground == Codex.Grounds.wooden_floor) == 4))
          {
            Generator.PlaceFloor(MineSquare, Codex.Grounds.wooden_floor);
            Generator.PlaceFixture(MineSquare, Codex.Features.workbench);
          }

          // place gems in void space.
          foreach (var MineSquare in BelowMap.GetSquares().Where(S => S.IsVoid() && Chance.OneIn50.Hit() && S.GetAdjacentSquares().All(V => V.IsVoid())))
            PlaceGem(MineSquare);

          // place the boss in the outermost point of the mine.
          var GolemSquare = Generator.ReducingFindSquare(ExitSquare, BelowSize, S => S.Floor != null && S.Boulder == null && S.Fixture == null && S.Trap == null && S.Passage == null && S.Character == null);
          if (GolemSquare != null)
          {
            Generator.PlaceCharacter(GolemSquare, MineVariant.Golem);
            var GolemCharacter = GolemSquare.Character;
            // gimmick: raging, tunnelling golem (slowness to make it escapable).
            GolemCharacter?.AcquireTalent(Codex.Properties.aggravation, Codex.Properties.rage, Codex.Properties.tunnelling, Codex.Properties.slowness);
          }

          Generator.RepairMap(BelowMap, BelowMap.Region);

          BelowMap.AddArea(MineName).AddMapZones();
        }

        // TODO:
        // * baubles?
        // * artifact key?

        BuildStop();
      }

      private readonly Variance<MineVariant> MineVariance;

      private sealed class MineVariant
      {
        public string Name;
        public Item MainGem;
        public Item OtherGem;
        public Entity Golem;

        public override string ToString() => Name;
      }
    }

    private sealed class NestBuilder : Builder
    {
      public NestBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.NestVariance = new Variance<NestVariant>
        (
          new NestVariant
          {
            Name = OpusTerms.AntNest,
            CaveName = OpusTerms.AntCave,
            LairName = OpusTerms.AntLair,
            Ground = Codex.Grounds.dirt,
            Horde = Codex.Hordes.ant,
            EntityList = new[] { Codex.Entities.giant_ant, Codex.Entities.snow_ant, Codex.Entities.fire_ant, Codex.Entities.soldier_ant, },
            Device = Codex.Devices.ant_hole,
            Boss = Codex.Entities.Girtab
          },
          /*
          new NestVariant
          {
            Name = OpusTerms.BeetleNest,
            Ground = Codex.Grounds.dirt,
            Horde = Codex.Hordes.beetle,
            EntityList = new[] { Codex.Entities.giant_cockroach, Codex.Entities.giant_beetle, Codex.Entities.spitting_beetle, Codex.Entities.killer_beetle },
            Device = Codex.Devices.grease_trap,
            Boss = Codex.Entities.???
          }
          */
          new NestVariant
          {
            Name = OpusTerms.ScorpionNest,
            CaveName = OpusTerms.ScorpionCave,
            LairName = OpusTerms.ScorpionLair,
            Ground = Codex.Grounds.dirt,
            Horde = Codex.Hordes.scorpion,
            EntityList = new[] { Codex.Entities.giant_scorpion, Codex.Entities.scorpion },
            Device = Codex.Devices.pit,
            Boss = Codex.Entities.Scorpius
          },
          new NestVariant
          {
            Name = OpusTerms.SpiderNest,
            CaveName = OpusTerms.SpiderCave,
            LairName = OpusTerms.SpiderLair,
            Ground = Codex.Grounds.dirt,
            Horde = Codex.Hordes.spider,
            EntityList = new[] { Codex.Entities.giant_spider, Codex.Entities.cave_spider, Codex.Entities.barking_spider, Codex.Entities.phase_spider, Codex.Entities.recluse_spider },
            Device = Codex.Devices.web,
            Boss = Codex.Entities.Lolth
          }
        );

#if DEBUG
        foreach (var Variant in NestVariance.List)
        {
          if (Variant.EntityList.Any(E => E.Level >= Variant.Boss.Level))
            throw new Exception($"{Variant.Boss.Name} must be higher level than the entities.");
        }
#endif
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        var NestVariant = NestVariance.NextVariant();
        Section.OverlandAreaName = NestVariant.Name;

        var AboveMap = Maker.OverlandMap;

        var NestEnterList = new Inv.DistinctList<Square>();

        foreach (var NestClearing in Section.Clearings)
        {
          var RoomSize = 3;
          if (NestClearing.Circle.Radius < 4)
            RoomSize--;

          var RoomRegion = new Region(NestClearing.Circle.X - RoomSize, NestClearing.Circle.Y - RoomSize, NestClearing.Circle.X + RoomSize, NestClearing.Circle.Y + RoomSize);

          if (AboveMap.GetSquares(RoomRegion).All(S => S.Floor?.Ground == Codex.Grounds.dirt && S.Wall == null && S.Boulder == null))
          {
            var RoomZone = AboveMap.AddZone();
            RoomZone.AddRegion(RoomRegion);
            RoomZone.SetLit(false);
            Debug.Assert(RoomZone.HasSquares());

            Generator.PlaceRoom(AboveMap, Codex.Barriers.cave_wall, Codex.Grounds.cave_floor, RoomRegion);

            var EntrancePoint = RoomRegion.Midpoint();
            var EntranceSquare = AboveMap[EntrancePoint.X, EntrancePoint.Y];

            foreach (var RoomSquare in EntranceSquare.GetNeighbourSquares(RoomSize))
            {
              RoomSquare.SetWall(null);
              Generator.PlaceFloor(RoomSquare, Codex.Grounds.cave_floor);
            }

            // boulder corner struts.
            foreach (var RoomSquare in EntranceSquare.GetCornerSquares(RoomSize))
            {
              RoomSquare.SetWall(null);
              Generator.PlaceFloor(RoomSquare, Codex.Grounds.dirt);
              Generator.PlaceBoulder(RoomSquare, Codex.Blocks.clay_boulder, IsRigid: true);
            }

            if (RoomSize == 3)
            {
              foreach (var RoomSquare in EntranceSquare.GetCornerSquares(RoomSize - 1))
              {
                RoomSquare.SetFloor(null);
                Generator.PlaceSolidWall(RoomSquare, Codex.Barriers.cave_wall, WallSegment.Pillar);
              }
            }

            // boulders and devices.
            foreach (var RoomSquare in EntranceSquare.GetAdjacentSquares(1))
            {
              if (RoomSquare.X != EntranceSquare.X && RoomSquare.Y != EntranceSquare.Y)
              {
                if (Chance.OneIn4.Hit())
                  Generator.PlaceBoulder(RoomSquare, Codex.Blocks.clay_boulder, IsRigid: true);
                else
                  Generator.PlaceTrap(RoomSquare, NestVariant.Device, Revealed: true);
              }
            }

            Generator.PlaceHorde(NestVariant.Horde, Section.MinimumDifficulty, Section.MaximumDifficulty, () => AboveMap.GetSquares(RoomRegion.Reduce(1)).Where(S => Generator.CanPlaceCharacter(S)).GetRandomOrNull());

            NestEnterList.Add(EntranceSquare);
          }
          else
          {
            Generator.PlaceHorde(NestVariant.Horde, Section.MinimumDifficulty, Section.MaximumDifficulty, () => AboveMap.GetCircleInnerSquares(NestClearing.Circle).Where(S => S.Floor != null && S.Character == null).GetRandomOrNull());
          }
        }

        var NestName = Generator.EscapedModuleTerm(NestVariant.Name);
        if (!Generator.Adventure.World.HasSite(NestName))
        {
          var NestSite = Generator.Adventure.World.AddSite(NestName);
          const int NestSize = 32;

          var CaveName = Generator.EscapedModuleTerm(NestVariant.CaveName);

          var CaveMap = Generator.Adventure.World.AddMap(CaveName, NestSize, NestSize);
          CaveMap.SetDifficulty(Section.Distance + 1);
          CaveMap.SetTerminal(false);
          CaveMap.SetAtmosphere(Codex.Atmospheres.cavern);
          var BelowLevel = NestSite.AddLevel(1, CaveMap);

          var PointCount = RandomSupport.NextNumber(15, 20);
          var PointList = new Inv.DistinctList<Inv.Point>(PointCount);

          foreach (var PointIndex in PointCount.NumberSeries())
          {
            var Attempt = 0;
            do
            {
              var PointX = RandomSupport.NextRange(CaveMap.Region.Left + 2, CaveMap.Region.Right - 2);
              var PointY = RandomSupport.NextRange(CaveMap.Region.Top + 2, CaveMap.Region.Bottom - 2);

              var Point = new Inv.Point(PointX, PointY);

              if (!PointList.Contains(Point))
              {
                PointList.Add(Point);
                break;
              }
            }
            while (Attempt++ < 5);
          }

          var FlipHorizontal = Chance.OneIn2.Hit() ? +1 : -1;
          var FlipVertical = Chance.OneIn2.Hit() ? +1 : -1;

          PointList.Sort((A, B) =>
          {
            var Result = A.X.CompareTo(B.X) * FlipHorizontal;
            if (Result == 0)
              Result = A.Y.CompareTo(B.Y) * FlipVertical;
            return Result;
          });

          var LastPoint = (Inv.Point?)null;
          foreach (var Point in PointList)
          {
            if (LastPoint != null)
            {
              foreach (var PathSquare in Maker.DeviatingPath(CaveMap[LastPoint.Value], CaveMap[Point], CaveMap.Region))
                Generator.PlaceFloor(PathSquare, Codex.Grounds.cave_floor);
            }

            LastPoint = Point;
          }

          // nest passages.
          var EnterSquare = NestEnterList.GetRandom();
          var ExitSquare = Generator.ExpandingFindSquare(CaveMap.Midpoint, NestSize, S => S.Floor != null);

          Generator.PlacePassage(EnterSquare, Codex.Portals.clay_staircase_down, ExitSquare);
          Generator.PlacePassage(ExitSquare, Codex.Portals.clay_staircase_up, EnterSquare);

          var RoyalSquare = Generator.ReducingFindSquare(CaveMap.Midpoint, NestSize, S => S.Floor != null);
          Generator.PlacePassage(RoyalSquare, Codex.Portals.wooden_ladder_down, null);

          BelowLevel.SetTransitions(UpSquare: ExitSquare, DownSquare: RoyalSquare);

          // nest boundary.
          foreach (var BoundarySquare in CaveMap.GetSquares())
          {
            if (BoundarySquare.IsVoid() && BoundarySquare.GetAdjacentSquares().Any(S => S.Floor != null && S.Wall == null))
            {
              BoundarySquare.SetLit(true);
              Generator.PlaceWall(BoundarySquare, Codex.Barriers.cave_wall, WallStructure.Solid, WallSegment.Pillar);
            }
          }

          // traps.
          foreach (var TrapSquare in CaveMap.GetSquares().Where(S => Maker.IsCorner(S, C => C.Wall != null, O => O.Floor != null)))
          {
            if (TrapSquare.Wall == null && TrapSquare.Passage == null)
            {
              if (Chance.OneIn2.Hit())
                Generator.PlaceRandomAsset(TrapSquare);

              if (Chance.OneIn2.Hit())
                Generator.PlaceCharacter(TrapSquare, Section.MinimumDifficulty, Section.MaximumDifficulty, NestVariant.EntityList);

              Generator.PlaceTrap(TrapSquare, NestVariant.Device, Revealed: true);
            }
          }

          // nest eggs.
          var NestEggArray = Codex.Eggs.List.Where(E => NestVariant.EntityList.Contains(E.Layer)).ToArray();

          void PlaceEgg(Square Square)
          {
            var NestEgg = NestEggArray.GetRandomOrNull();
            if (NestEgg != null)
            {
              var EggAsset = Generator.NewSpecificAsset(Square: null, Codex.Items.egg);
              EggAsset.SetEgg(NestEgg);
              Square.PlaceAsset(EggAsset);
            }
          }

          foreach (var EggSquare in CaveMap.GetSquares().Where(S => S.Floor != null && S.GetAdjacentSquares().All(S => S.Floor != null)))
          {
            Generator.PlaceFloor(EggSquare, Codex.Grounds.moss);

            if (Chance.OneIn2.Hit())
              PlaceEgg(EggSquare);
          }

          Generator.RepairMap(CaveMap, CaveMap.Region);

          CaveMap.AddArea(CaveName).AddMapZones();

          // boss chambers.
          var LairName = Generator.EscapedModuleTerm(NestVariant.LairName);
          var LairMap = Generator.Adventure.World.AddMap(LairName, NestSize, NestSize);
          LairMap.SetDifficulty(Section.Distance + 2);
          LairMap.SetTerminal(true);
          LairMap.SetAtmosphere(Codex.Atmospheres.cavern);
          var BossLevel = NestSite.AddLevel(2, LairMap);

          var BossRadius = NestSize / 8;
          var BossCircle = new Inv.Circle(LairMap.Midpoint.Point, BossRadius);
          var BossSquare = LairMap[BossCircle.Origin];
          var ChamberRadius = BossRadius / 2;
          var JoinSquare = LairMap.GetCircleOuterSquares(BossCircle.Expand(ChamberRadius - 1)).GetRandomOrNull();
          var JoinCircle = new Inv.Circle(JoinSquare.Point, ChamberRadius);
          var LootSquare = JoinSquare.Rotate(BossCircle.Origin, 180);
          var LootCircle = new Inv.Circle(LootSquare.Point, ChamberRadius);

          var BossZone = LairMap.AddZone();

          var BossTrigger = BossZone.InsertTrigger();

          foreach (var ChamberSquare in LairMap.GetCircleInnerSquares(BossCircle))
          {
            Generator.PlaceFloor(ChamberSquare, Codex.Grounds.moss);

            if (ChamberSquare != BossSquare)
            {
              if (Chance.OneIn3.Hit())
              {
                Generator.PlaceTrap(ChamberSquare, NestVariant.Device, Revealed: true);
              }
              else if (Chance.OneIn3.Hit())
              {
                // TODO: don't place more eggs than the boss can have allies?
                PlaceEgg(ChamberSquare);

                BossTrigger.Add(Delay.FromTurns(20), Codex.Tricks.hatching_eggs).SetTarget(ChamberSquare);
              }
            }

            BossZone.AddSquare(ChamberSquare);

            ChamberSquare.SetLit(true);
          }
          Debug.Assert(BossZone.HasSquares());

          var JoinZone = LairMap.AddZone();
          foreach (var ChamberSquare in LairMap.GetCircleInnerSquares(JoinCircle))
          {
            Generator.PlaceFloor(ChamberSquare, Codex.Grounds.moss);

            JoinZone.AddSquare(ChamberSquare);

            ChamberSquare.SetLit(true);
          }
          Debug.Assert(JoinZone.HasSquares());

          // surround join chamber with traps.
          foreach (var ChamberSquare in LairMap.GetCircleInnerSquares(JoinCircle).Where(S => S.GetAdjacentSquares().Any(T => T.IsVoid())))
            Generator.PlaceTrap(ChamberSquare, NestVariant.Device, Revealed: true);

          var LootZone = LairMap.AddZone();
          foreach (var ChamberSquare in LairMap.GetCircleInnerSquares(LootCircle))
          {
            Generator.PlaceFloor(ChamberSquare, Codex.Grounds.moss);

            LootZone.AddSquare(ChamberSquare);

            ChamberSquare.SetLit(true);
          }
          Debug.Assert(LootZone.HasSquares());

          LootSquare.PlaceAsset(Generator.GenerateUniqueAsset(LootSquare));
          foreach (var ChamberSquare in LootSquare.GetAdjacentSquares())
          {
            Generator.DropCoins(ChamberSquare, Generator.RandomCoinQuantity(ChamberSquare));

            Generator.PlaceTrap(ChamberSquare, NestVariant.Device, Revealed: false);
          }

          foreach (var ChamberSquare in LootZone.Squares)
          {
            if (ChamberSquare.Trap == null && !ChamberSquare.HasAssets())
              Generator.PlaceRandomAsset(ChamberSquare);
          }

          Maker.RepairBoundary(LairMap, LairMap.Region, Codex.Barriers.cave_wall, Codex.Grounds.moss, IsLit: true);

          BossLevel.SetTransitions(UpSquare: JoinSquare, DownSquare: null);

          Generator.PlacePassage(JoinSquare, Codex.Portals.wooden_ladder_up, RoyalSquare);
          RoyalSquare.Passage.SetDestination(JoinSquare);

          var BossCharacter = Maker.NewEvilCharacter(BossSquare, Maker.SelectUniqueEntity(NestVariant.Boss));
          Generator.PlaceCharacter(BossSquare, BossCharacter);

          Generator.RepairMap(LairMap, LairMap.Region);

          LairMap.AddArea(LairName).AddMapZones();
        }

        // TODO:
        // *

        BuildStop();
      }

      private readonly Variance<NestVariant> NestVariance;

      private sealed class NestVariant
      {
        public string Name;
        public string CaveName;
        public string LairName;
        public Ground Ground;
        public Horde Horde;
        public IReadOnlyList<Entity> EntityList;
        public Device Device;
        public Entity Boss;

        public override string ToString() => Name;
      }
    }

    private sealed class RavineBuilder : Builder
    {
      public RavineBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.RavineVariance = new Variance<RavineVariant>
        (
          new RavineVariant
          {
            Name = OpusTerms.River,
            Ground = Codex.Grounds.water,
            Platform = Codex.Platforms.wooden_bridge,
            Barrier = Codex.Barriers.tree
          },
          new RavineVariant
          {
            Name = OpusTerms.Canyon,
            Ground = Codex.Grounds.chasm,
            Platform = Codex.Platforms.wooden_bridge,
            Barrier = Codex.Barriers.shroom
          },
          new RavineVariant
          {
            Name = OpusTerms.Glacier,
            Ground = Codex.Grounds.ice,
            Platform = Codex.Platforms.wooden_bridge,
            Barrier = Codex.Barriers.tree
          },
          new RavineVariant
          {
            Name = OpusTerms.Lava_Flow,
            Ground = Codex.Grounds.lava,
            Platform = Codex.Platforms.crystal_bridge,
            Barrier = Codex.Barriers.shroom
          }
        );
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        var RavineVariant = RavineVariance.NextVariant();
        Section.OverlandAreaName = RavineVariant.Name;

        var RavineMap = Maker.OverlandMap;
        var RavineRegion = Section.Region;

        bool IsDirt(Square Square) => Square?.Floor?.Ground == Codex.Grounds.dirt;
        bool IsRavine(Square Square) => Square?.Floor?.Ground == RavineVariant.Ground;

        var LastSquare = (Square)null;

        foreach (var RavineClearing in Section.Clearings)
        {
          var RavineCircle = RavineClearing.Circle;
          var RavineStartSquare = RavineMap.GetCircleOuterSquares(RavineCircle.Reduce(1)).GetRandomOrNull();
          var RavineEndSquare = RavineStartSquare.Rotate(RavineCircle.Origin, 180);

          // TODO: DeviatingPath needs a constraint region?
          foreach (var RavineSquare in Maker.DeviatingPath(RavineStartSquare, RavineEndSquare, RavineRegion))
          {
            Debug.Assert(RavineSquare.IsRegion(RavineRegion));

            Generator.PlaceFloor(RavineSquare, RavineVariant.Ground);

            RavineSquare.SetLit(true);
          }

          if (LastSquare != null)
          {
            var NextPoint = LastSquare.Point.ManhattanDistance(RavineStartSquare.Point) > LastSquare.Point.ManhattanDistance(RavineEndSquare.Point) ? RavineEndSquare : RavineStartSquare;

            foreach (var RavineSquare in Maker.DeviatingPath(LastSquare, NextPoint, RavineRegion))
            {
              Debug.Assert(RavineSquare.IsRegion(RavineRegion));

              Generator.PlaceFloor(RavineSquare, RavineVariant.Ground);

              RavineSquare.SetLit(true);
            }
          }

          LastSquare = RavineEndSquare;
        }

        // TODO: Section Thresholds.
        //foreach (var RavineJoin in Section.Joins)
        //{
        //  foreach (var RavineSquare in RavineMap.GetSquares(RavineJoin.Region))
        //  {
        //    if (RavineSquare.Floor?.Ground == Codex.Grounds.dirt)
        //      Generator.PlaceFloor(RavineSquare, RavineVariant.Ground);
        //  }
        //}

        // bridge across the ravines.
        var SpaceSet = RavineMap.GetSquares(RavineRegion.Expand(1)).Where(S => IsDirt(S)).ToHashSetX();
        do
        {
          var RavineSet = new HashSet<Square>();
          var PathSet = new HashSet<Square>();
          SpaceSet.GetRandomOrNull().FloodNeighbour(Square =>
          {
            if (IsDirt(Square))
            {
              if (!SpaceSet.Contains(Square))
                return false;

              return PathSet.Add(Square);
            }

            if (Square.Bridge != null)
              return PathSet.Add(Square);

            if (IsRavine(Square) && Square.Bridge == null && !Square.GetNeighbourSquares().Any(S => S.Bridge != null))
              RavineSet.Add(Square);

            return false;
          });

          if (PathSet.Count >= SpaceSet.Count)
          {
            // all floor squares are reachable.
            break;
          }
          else
          {
            bool BridgeAcross(Square F, Square S)
            {
              var NextDirection = F.AsDirection(S);

              var PreviousSquare = F.Adjacent(NextDirection.Reverse());
              if (PreviousSquare == null || !IsDirt(PreviousSquare) || !PathSet.Contains(PreviousSquare))
                return false;

              var NextSquare = S;
              while (NextSquare != null && IsRavine(NextSquare))
                NextSquare = NextSquare.Adjacent(NextDirection);

              if (NextSquare != null && IsDirt(NextSquare) && SpaceSet.Contains(NextSquare) && !PathSet.Contains(NextSquare) && NextSquare.Point.ManhattanDistance(F.Point) <= 3)
                return true;

              return false;
            }

            // choose a ravine that we can bridge across.
            var RavineSquare = RavineSet.Where(F => F.GetNeighbourSquares().Any(S => BridgeAcross(F, S))).GetRandomOrNull();
            if (RavineSquare == null)
              break;

            Generator.PlaceBridge(RavineSquare, RavineVariant.Platform, BridgeOrientation.Horizontal);

            var ContinueSquare = RavineSquare.GetNeighbourSquares().Where(S => BridgeAcross(RavineSquare, S)).GetRandomOrNull();
            while (ContinueSquare != null && IsRavine(ContinueSquare))
            {
              Generator.PlaceBridge(ContinueSquare, RavineVariant.Platform, BridgeOrientation.Horizontal);

              ContinueSquare = ContinueSquare.Adjacent(RavineSquare.AsDirection(ContinueSquare));
            }

            if (RavineSquare.Bridge != null)
              SpaceSet.Add(RavineSquare);
          }
        }
        while (true);

        // repair the map including zones.
        Generator.RepairMap(RavineMap, RavineRegion);

        SpaceSet.RemoveWhere(S => S.Bridge != null || S.Wall != null);
        var RavineCount = ((int)Math.Sqrt(SpaceSet.Count));

        // generate monsters in the water and on the ice.
        if (RavineVariant.Ground == Codex.Grounds.water || RavineVariant.Ground == Codex.Grounds.ice)
        {
          foreach (var RavineIndex in RavineCount.NumberSeries())
          {
            var RavineSquare = SpaceSet.GetRandomOrNull();
            if (RavineSquare != null)
            {
              SpaceSet.Remove(RavineSquare);

              if (RavineSquare.Character == null) // could be a worm tail.
                Generator.PlaceRandomCharacter(RavineSquare, Section.MinimumDifficulty, Section.MaximumDifficulty);
            }
          }
        }

        // generating random loot where it's harder to get to.
        SpaceSet.RemoveWhere(S => S.GetAdjacentSquares().Count(S => IsRavine(S)) < 6);

        foreach (var RavineIndex in (RavineCount / 2).NumberSeries())
        {
          var RavineSquare = SpaceSet.GetRandomOrNull();
          if (RavineSquare != null)
          {
            SpaceSet.Remove(RavineSquare);

            Generator.PlaceRandomAsset(RavineSquare);
          }
        }

        // build the underpit below the chasm.
        if (RavineVariant.Ground == Codex.Grounds.chasm)
          Maker.Pit.Build(Section, Section.Region, Maker.OverlandMap, Maker.UndergroundMap, Codex.Grounds.dirt);

        BuildStop();

        // TODO:
        // * troll settlement around bridge + goats?
        // * wooden wall fortification?
      }

      private readonly Variance<RavineVariant> RavineVariance;

      private sealed class RavineVariant
      {
        public string Name;
        public Ground Ground;
        public Platform Platform;
        public Barrier Barrier;
      }
    }

    private sealed class RoadBuilder : Builder
    {
      public RoadBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.RoadVariance = new Variance<RoadVariant>
        (
          new RoadVariant
          {
            Ground = Codex.Grounds.stone_path
          }
        );
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        Section.OverlandAreaName = OpusTerms.Respite;

        var RoadMap = Maker.OverlandMap;
        var RoadVariant = RoadVariance.NextVariant();

        foreach (var Join in Section.Joins)
        {
          var JoinSquare = RoadMap.GetSquares(Join.Region).Where(S => S.Floor != null && S.Wall == null).GetRandomOrNull();

          var NearestClearing = Section.Clearings.OrderBy(C => C.Circle.Origin.ManhattanDistance(JoinSquare.Point)).FirstOrDefault();

          foreach (var PathSquare in Maker.WalkingPath(JoinSquare, RoadMap[NearestClearing.Circle.X, NearestClearing.Circle.Y]))
            Generator.PlaceFloor(PathSquare, RoadVariant.Ground);
        }

        var ClearingList = Section.Clearings.ToDistinctList();

        foreach (var Clearing in Section.Clearings)
        {
          ClearingList.Remove(Clearing);

          var NearestClearing = ClearingList.OrderBy(C => C.Circle.Origin.ManhattanDistance(Clearing.Circle.Origin)).FirstOrDefault();

          if (NearestClearing != null)
          {
            ClearingList.Remove(NearestClearing);

            foreach (var PathSquare in Maker.WalkingPath(RoadMap[Clearing.Circle.X, Clearing.Circle.Y], RoadMap[NearestClearing.Circle.X, NearestClearing.Circle.Y]))
              Generator.PlaceFloor(PathSquare, RoadVariant.Ground);
          }
        }

        BuildStop();

        // TODO:
        // * what else is on the road?
      }

      private readonly Variance<RoadVariant> RoadVariance;

      private sealed class RoadVariant
      {
        public Ground Ground;
      }
    }

    private sealed class PitBuilder : Builder
    {
      public PitBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.PitVariance = new Variance<PitVariant>
        (
          new PitVariant
          {
            Name = OpusTerms.Moss_Pit,
            Ground = Codex.Grounds.moss,
            Kind = Codex.Kinds.fungus
          },
          new PitVariant
          {
            Name = OpusTerms.Slime_Pit,
            Ground = Codex.Grounds.cave_floor,
            Kind = Codex.Kinds.blob
          },
          new PitVariant
          {
            Name = OpusTerms.Jelly_Pit,
            Ground = Codex.Grounds.cave_floor,
            Kind = Codex.Kinds.jelly
          },
          new PitVariant
          {
            Name = OpusTerms.Pudding_Pit,
            Ground = Codex.Grounds.cave_floor,
            Kind = Codex.Kinds.pudding
          },
          new PitVariant
          {
            Name = OpusTerms.Snake_Pit,
            Ground = Codex.Grounds.dirt,
            Kind = Codex.Kinds.snake
          },
          new PitVariant
          {
            Name = OpusTerms.Rat_Pit,
            Ground = Codex.Grounds.dirt,
            Kind = Codex.Kinds.rodent
          },
          new PitVariant
          {
            Name = OpusTerms.Eye_Pit,
            Ground = Codex.Grounds.dirt,
            Kind = Codex.Kinds.eye
          },
          new PitVariant
          {
            Name = OpusTerms.Worm_Pit,
            Ground = Codex.Grounds.dirt,
            Kind = Codex.Kinds.worm
          }
        );

        // we already have a Spider/Ant/Scorpion Nest, so we don't need a pit.
      }

      public void Build(OpusSection Section, Region PitRegion, Map OverlandMap, Map UndergroundMap, Ground SurfaceGround)
      {
        BuildStart();

        var PitVariant = PitVariance.NextVariant();

        Section.UndergroundAreaName = PitVariant.Name;

        var PitIsLit = Chance.ThreeIn4.Hit();

        foreach (var OverlandSquare in OverlandMap.GetSquares(PitRegion).Where(S => S.Floor?.Ground == Codex.Grounds.chasm))
        {
          var UndergroundSquare = UndergroundMap[OverlandSquare.X, OverlandSquare.Y];

          UndergroundSquare.SetLit(PitIsLit);

          if (UndergroundSquare.Floor == null)
            Generator.PlaceFloor(UndergroundSquare, PitVariant.Ground);
        }

        var LadderSquare = UndergroundMap.GetSquares(PitRegion).Where(S => S.IsVoid() && S.GetNeighbourSquares().Where(S => S.Floor != null).Any()).GetRandomOrNull();
        LadderSquare.SetLit(PitIsLit);

        Generator.PlaceFloor(LadderSquare, PitVariant.Ground);

        var EscapeSquare = OverlandMap[LadderSquare.X, LadderSquare.Y];
        EscapeSquare.SetLit(true);
        EscapeSquare.SetWall(null);
        Generator.PlaceFloor(EscapeSquare, SurfaceGround);

        // give the escape square a chance.
        foreach (var PitSquare in EscapeSquare.GetNeighbourSquares())
        {
          if (PitSquare.Wall != null)
            PitSquare.SetWall(null);

          if (PitSquare.IsVoid())
          {
            PitSquare.SetLit(true);

            Generator.PlaceFloor(PitSquare, Codex.Grounds.dirt);
          }
        }

        Generator.PlacePassage(EscapeSquare, Codex.Portals.wooden_ladder_down, LadderSquare);
        Generator.PlacePassage(LadderSquare, Codex.Portals.wooden_ladder_up, EscapeSquare);

        Generator.RepairMap(UndergroundMap, PitRegion);

        foreach (var UndergroundSquare in UndergroundMap.GetSquares(PitRegion).Where(S => S.Floor?.Ground == PitVariant.Ground))
        {
          if (UndergroundSquare.Character == null && Chance.OneIn7.Hit())
            Generator.PlaceCharacter(UndergroundSquare, Section.MinimumDifficulty, Section.MaximumDifficulty, new[] { PitVariant.Kind });
        }

        // TODO:
        // * trick spawning?
        // * loot?

        BuildStop();
      }

      private readonly Variance<PitVariant> PitVariance;

      private sealed class PitVariant
      {
        public string Name;
        public Ground Ground;
        public Kind Kind;

        public override string ToString() => Name;
      }
    }

    private sealed class PrintBuilder : Builder
    {
      public PrintBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.PrintVariance = new Variance<PrintVariant>
(
new PrintVariant(@"
 XX XX 
XOOXXXX
XOXXXOX
 XXXOX 
  XXX 
   X
"),
new PrintVariant(@"
 XXXXX 
X  X  X
X OXO X
XXXXXXX
XXX XXX
 XXXXX 
 X X X
"),
new PrintVariant(@"
    XXX
  XX.X.XX 
 X...X...X  
 X...X...X   
X....X....X 
X...XXX...X 
X..X.X.X..X
 XX..X..XX
 X...X...X
  XX.X.XX
    XXX
")
/*
new PrintVariant(@"
H-----------H
|...........|
|.....C.....|
|...........|
|...........|
|...........|
H...........H
|.....0.....|
|....0.0....|
|...0.B.0...|
|..0.0.0.0..|
|...........|
H-----------H
"),
*/
);
      }

      public void Build(OpusClearing PrintClearing)
      {
        BuildStart();

        var PrintMap = Maker.OverlandMap;
        var PrintVariant = PrintVariance.NextVariant();
        var PrintGrid = Generator.LoadSpecialGrid(PrintVariant.Text);
        
        var RadiusX = (PrintGrid.Width / 2);
        var RadiusY = (PrintGrid.Height / 2);
        if (RadiusX <= PrintClearing.Circle.Radius && RadiusY <= PrintClearing.Circle.Radius)
        {
          var PrintY = PrintClearing.Circle.Y - RadiusY;

          for (var Y = 0; Y < PrintGrid.Height; Y++)
          {
            var PrintX = PrintClearing.Circle.X - RadiusX;

            for (var X = 0; X < PrintGrid.Width; X++)
            {
              var PrintSquare = PrintMap[PrintX, PrintY];

              switch (PrintGrid[X, Y])
              {
                case ' ':
                  // do nothing.
                  break;

                case 'O':
                  Generator.PlaceFloor(PrintSquare, Codex.Grounds.lava);
                  break;

                case 'X':
                  Generator.PlaceFloor(PrintSquare, Codex.Grounds.obsidian_floor);
                  break;

                case '.':
                  Generator.PlaceFloor(PrintSquare, Codex.Grounds.grass);
                  break;

                case '|':
                case '-':
                  Generator.PlaceFloor(PrintSquare, Codex.Grounds.wooden_floor);
                  break;

                case '0':
                  Generator.PlaceFloor(PrintSquare, Codex.Grounds.grass);
                  Generator.PlaceBoulder(PrintSquare, Codex.Blocks.clay_boulder, IsRigid: true);
                  break;

                case 'B':
                  Generator.PlaceFloor(PrintSquare, Codex.Grounds.grass);
                  Generator.PlaceBoulder(PrintSquare, Codex.Blocks.crystal_boulder, IsRigid: true);
                  break;

                case 'C':
                  Generator.PlaceFloor(PrintSquare, Codex.Grounds.grass);
                  Generator.PlaceBoulder(PrintSquare, Codex.Blocks.stone_boulder, IsRigid: true);
                  break;

                case 'W':
                  Generator.PlaceFloor(PrintSquare, Codex.Grounds.wooden_floor);
                  break;

                case 'H':
                  Generator.PlaceFloor(PrintSquare, Codex.Grounds.wooden_floor);
                  Generator.PlaceTrap(PrintSquare, Codex.Devices.pit, Revealed: false); // TODO: should be a chasm?
                  break;

                default:
                  throw new Exception($"Print symbol not handled: {PrintGrid[X, Y]}");
              }

              PrintX++;
            }

            PrintY++;
          }
        }

        // TODO:
        // * more pixel art: music cleft.
        // * billiards table with holes
        // * chessboard with statues vs. trophies.

        BuildStop();
      }

      private readonly Variance<PrintVariant> PrintVariance;

      private sealed class PrintVariant
      {
        internal PrintVariant(string Text)
        {
          this.Text = Text;
        }

        public readonly string Text;
      }
    }

    private sealed class RuinsBuilder : Builder
    {
      public RuinsBuilder(OpusMaker Maker)
        : base(Maker)
      {
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        Section.OverlandAreaName = OpusTerms.Ruins;

        var AboveMap = Maker.OverlandMap;
        var RuinRegion = Section.Region.Reduce(3);

        Square AboveEntranceSquare = null;

        var AboveVariant = new RuinVariant()
        {
          NaturalBarrier = Codex.Barriers.tree,
          NaturalGround = Codex.Grounds.dirt,
          NaturalGate = null,
          NaturalBlock = Codex.Blocks.wooden_barrel,
          NaturalIsLit = true,
          RoomBarrier = Codex.Barriers.stone_wall,
          RoomGround = Codex.Grounds.stone_floor,
          RoomBlock = Codex.Blocks.stone_boulder,
          BrokenGround = Codex.Grounds.stone_path,
          RoomGate = Codex.Gates.wooden_door,
          MinimumDifficulty = Section.MinimumDifficulty,
          MaximumDifficulty = Section.MaximumDifficulty,
          EnterAction = (EnterSquare) =>
          {
            AboveEntranceSquare = EnterSquare;
            Generator.PlacePassage(EnterSquare, Codex.Portals.stone_staircase_down, null); // placeholder.
          },
          ExitAction = null
        };

        // erase anything already placed so we can build the ruin.
        foreach (var AboveSquare in AboveMap.GetSquares(RuinRegion))
          Generator.PlaceVoid(AboveSquare);

        BuildRuin(AboveMap, RuinRegion, AboveVariant);

        // might need a surrounding dirt veranda to reconnect the areas.
        Maker.RepairVeranda(AboveMap, RuinRegion, Codex.Grounds.dirt, IsLit: true);

        // TODO: it is possible to have an inaccessible ruin, although unlikely. Player can always use a pickaxe?
        foreach (var AboveSquare in AboveMap.GetSquares(RuinRegion))
        {
          AboveSquare.SetLit(true);

          if (AboveSquare.Wall?.Barrier == AboveVariant.NaturalBarrier)
          {
            Generator.PlaceFloor(AboveSquare, AboveVariant.NaturalGround); // dirt under the tree.

            if (Chance.OneIn4.Hit())
              AboveSquare.SetWall(null);
          }
          else if (AboveSquare.Wall?.Barrier == Codex.Barriers.iron_bars)
          {
            AboveSquare.SetWall(null); // delete iron bars in the above ruin.
          }
        }

        Maker.RepairVeranda(AboveMap, Section.Region, Codex.Grounds.dirt, IsLit: true);

        var RuinsName = Generator.EscapedModuleTerm(OpusTerms.Ruins);
        if (!Generator.Adventure.World.HasSite(RuinsName))
        {
          var BelowSite = Generator.Adventure.World.AddSite(RuinsName);
          var BelowCount = (1.d2() + 3).Roll(); // 4-5

          var BelowEntranceSquare = AboveEntranceSquare;

          for (var BelowIndex = 1; BelowIndex <= BelowCount; BelowIndex++)
          {
            var RuinSize = 33; // progression? (21 + UndergroundIndex) + (5 * UndergroundIndex); // 27, 33, 39, 45, 51

            var BelowMap = Generator.Adventure.World.AddMap(RuinsName + " " + BelowIndex, RuinSize, RuinSize);
            BelowMap.SetDifficulty(Section.Distance + BelowIndex);
            BelowMap.SetTerminal(BelowIndex == BelowCount);
            BelowMap.SetAtmosphere(Codex.Atmospheres.cavern);
            var BelowLevel = BelowSite.AddLevel(BelowIndex, BelowMap);

            var BelowVariant = new RuinVariant()
            {
              NaturalBarrier = Codex.Barriers.cave_wall,
              NaturalGround = Codex.Grounds.cave_floor,
              NaturalGate = null,
              NaturalBlock = Codex.Blocks.clay_boulder,
              NaturalIsLit = Chance.ThreeIn4.Hit(),
              RoomBarrier = Codex.Barriers.stone_wall,
              RoomGround = Codex.Grounds.stone_floor,
              RoomBlock = Codex.Blocks.stone_boulder,
              BrokenGround = Codex.Grounds.stone_path,
              RoomGate = Codex.Gates.wooden_door,
              MinimumDifficulty = Generator.MinimumDifficulty(BelowMap),
              MaximumDifficulty = Generator.MaximumDifficulty(BelowMap),
              EnterAction = (EnterSquare) =>
              {
                Debug.Assert(EnterSquare.Wall == null);
                Debug.Assert(BelowEntranceSquare.Wall == null);

                BelowLevel.SetTransitions(EnterSquare, null);
                Generator.PlacePassage(BelowEntranceSquare, Codex.Portals.stone_staircase_down, EnterSquare);
                Generator.PlacePassage(EnterSquare, Codex.Portals.stone_staircase_up, BelowEntranceSquare);
              },
              ExitAction = (ExitSquare) =>
              {
                Debug.Assert(ExitSquare.Wall == null);

                if (BelowCount > 1)
                {
                  if (BelowIndex == BelowCount)
                  {
                    // escape portal from the ruins.
                    Generator.PlacePassage(ExitSquare, Codex.Portals.transportal, AboveEntranceSquare);

                    // boss and artifact.
                    var BossCharacter = Maker.NewEvilCharacter(ExitSquare, Maker.SelectUniqueEntity(Codex.Entities.Huhetotl, Codex.Entities.Maugneshaagar, Codex.Entities.Nalzok));
                    BossCharacter.Inventory.Carried.Add(Generator.GenerateUniqueAsset(ExitSquare)); // carry but don't outfit it.
                    Generator.PlaceCharacter(ExitSquare, BossCharacter);
                  }
                  else
                  {
                    Generator.PlacePassage(ExitSquare, Codex.Portals.stone_staircase_down, null); // placeholder.
                    BelowLevel.SetTransitions(BelowLevel.UpSquare, ExitSquare);
                    BelowEntranceSquare = ExitSquare;
                  }
                }
              }
            };

            BuildRuin(BelowMap, BelowMap.Region, BelowVariant);

            BelowMap.AddArea(RuinsName).AddMapZones();
          }
        }

        BuildStop();

        // TODO:
        // * 
      }

      private void BuildRuin(Map RuinMap, Region RuinRegion, RuinVariant RuinVariant)
      {
        // only works with odd sized areas due to the bracketing.
        var ConstraintWidth = RuinRegion.Width;
        if (ConstraintWidth % 2 == 0)
          ConstraintWidth--;

        var ConstraintHeight = RuinRegion.Height;
        if (ConstraintHeight % 2 == 0)
          ConstraintHeight--;
        var BoxCount = (int)Math.Sqrt(ConstraintWidth * ConstraintHeight);
        const int BracketSize = 2;

        var BracketWidth = ConstraintWidth / BracketSize;
        var BracketHeight = ConstraintHeight / BracketSize;
        var BoxMinimumSize = 4 / BracketSize;
        var BoxMaximumSize = Math.Min(Math.Min(BracketWidth, BracketHeight), 10 / BracketSize);

        Region GenerateBox()
        {
          var BoxWidth = RandomSupport.NextRange(BoxMinimumSize, BoxMaximumSize);
          var BoxHeight = RandomSupport.NextRange(BoxMinimumSize, BoxMaximumSize);
          var BoxLeft = RandomSupport.NextRange(0, BracketWidth - BoxWidth);
          var BoxRight = BoxLeft + BoxWidth;
          var BoxTop = RandomSupport.NextRange(0, BracketHeight - BoxHeight);
          var BoxBottom = BoxTop + BoxHeight;
          var BoxRegion = new Region(RuinRegion.Left + (BoxLeft * BracketSize), RuinRegion.Top + (BoxTop * BracketSize), RuinRegion.Left + (BoxRight * BracketSize), RuinRegion.Top + (BoxBottom * 2));
          return BoxRegion;
        }

        // place wall and floor on all the connected regions.
        foreach (var ConnectedRegion in Maker.RandomConnectedRegions(BoxCount, GenerateBox))
        {
          foreach (var RoomSquare in RuinMap.GetFrameSquares(ConnectedRegion).Where(S => S.IsVoid()))
          {
            RoomSquare.SetLit(RuinVariant.NaturalIsLit);

            Generator.PlaceSolidWall(RoomSquare, RuinVariant.NaturalBarrier, WallSegment.Pillar);
          }

          foreach (var RoomSquare in RuinMap.GetSquares(ConnectedRegion.Reduce(1)).Where(S => S.IsVoid()))
          {
            RoomSquare.SetLit(RuinVariant.NaturalIsLit);

            Generator.PlaceFloor(RoomSquare, RuinVariant.NaturalGround);
          }
        }

        // scan for rectangular regions that can be used or divided into rooms.
        var BuildingList = new Inv.DistinctList<OpusBuilding>();
        foreach (var BuildingRegion in Maker.ScanBoundedRegions(RuinMap, RuinRegion))
        {
          const int RoomMinimumSize = 4;
          const int RoomMaximumSize = 9;

          if (BuildingRegion.Width >= RoomMinimumSize && BuildingRegion.Height >= RoomMinimumSize)
          {
            var AdjustRegion = BuildingRegion;

            void DivideRoom()
            {
              foreach (var DividerSquare in RuinMap.GetFrameSquares(AdjustRegion).Where(S => S.Wall == null && S.Passage == null))
              {
                DividerSquare.SetFloor(null);
                Generator.PlaceSolidWall(DividerSquare, RuinVariant.NaturalBarrier, WallSegment.Pillar);
              }
            }

            while (AdjustRegion.Width >= RoomMaximumSize && AdjustRegion.Height >= RoomMaximumSize)
            {
              AdjustRegion = AdjustRegion.Reduce(2);
              DivideRoom();
            }

            while (AdjustRegion.Width >= RoomMaximumSize)
            {
              AdjustRegion = new Region(AdjustRegion.Left, AdjustRegion.Top, AdjustRegion.Right - (AdjustRegion.Width / 2), AdjustRegion.Bottom);
              DivideRoom();
            }

            while (AdjustRegion.Height >= RoomMaximumSize)
            {
              AdjustRegion = new Region(AdjustRegion.Left, AdjustRegion.Top, AdjustRegion.Right, AdjustRegion.Bottom - (AdjustRegion.Height / 2));
              DivideRoom();
            }

            var RoomZone = RuinMap.AddZone();
            RoomZone.SetDifficulty(RuinVariant.MaximumDifficulty);

            foreach (var RoomSquare in RuinMap.GetSquares(AdjustRegion))
            {
              RoomSquare.SetLit(true);
              RoomSquare.Wall?.SetBarrier(RuinVariant.RoomBarrier);

              if (RoomSquare.Floor != null)
                Generator.PlaceFloor(RoomSquare, RuinVariant.BrokenGround != null && Chance.OneIn5.Hit() ? RuinVariant.BrokenGround : RuinVariant.RoomGround);

              RoomZone.AddSquare(RoomSquare);
            }

            if (RoomZone.Squares.Count == 0)
              RuinMap.RemoveZone(RoomZone);
            else
              BuildingList.Add(new OpusBuilding(AdjustRegion));
          }
        }

        Debug.Assert(BuildingList.Count > 0, "How is it possible that the generated map has no rectangular regions?");

        // map entrance and exit.
        var EnterRegion = BuildingList.GetRandomOrNull()?.Region ?? RuinRegion;
        var EnterSquare = RuinMap.GetSquares(EnterRegion).Where(S => S.Floor != null && S.Wall == null && S.Passage == null).GetRandomOrNull();
        RuinVariant.EnterAction(EnterSquare);

        if (RuinVariant.ExitAction != null)
        {
          var ExitRegion = BuildingList.Where(B => B.Region != EnterRegion).GetRandomOrNull()?.Region ?? RuinRegion;
          var ExitSquare = RuinMap.GetSquares(ExitRegion).Where(S => S.Floor != null && S.Passage == null && S.Wall == null && S.Door == null).GetRandomOrNull();
          RuinVariant.ExitAction(ExitSquare);
        }

        // flood to punch doors through walls.
        if (EnterSquare != null)
        {
          Maker.ConnectSquares(RuinMap.GetSquares(RuinRegion), EnterSquare, PunchSquare =>
          {
            var PunchBarrier = PunchSquare.Wall.Barrier;
            Generator.PlaceFloor(PunchSquare, PunchBarrier.Underlay);

            var PunchGate = PunchSquare.Floor.Ground == RuinVariant.NaturalGround ? RuinVariant.NaturalGate : RuinVariant.RoomGate;

            if (PunchGate == null)
            {
              if (Chance.OneIn5.Hit())
                PunchSquare.Wall.SetStructure(WallStructure.Illusionary);
              else
                PunchSquare.SetWall(null);
            }
            else
            {
              PunchSquare.SetWall(null);
              Generator.PlaceDoor(PunchSquare, PunchGate, DoorOrientation.Vertical, SecretBarrier: PunchBarrier);

              if (PunchSquare.Door != null && !PunchSquare.Door.Secret && Chance.OneIn5.Hit())
              {
                PunchSquare.Door.SetState(DoorState.Broken);
                Generator.PlaceSpill(PunchSquare, Codex.Volatiles.blaze, Clock.Zero);
              }
            }
          });
        }

        // room details.
        foreach (var Building in BuildingList)
        {
          var FloorSquareList = RuinMap.GetSquares(Building.Region.Reduce(1)).ToDistinctList();
          Debug.Assert(FloorSquareList.All(S => S.GetDifficulty() > 0), "Generator.PlaceRoom* only work correctly when the difficulty is derivable.");

          Generator.PlaceRoomFixtures(FloorSquareList);

          var DetailSquareList = FloorSquareList.Where(Generator.CanPlaceTrap).ToDistinctList();
          Generator.PlaceRoomTraps(DetailSquareList, RuinVariant.MaximumDifficulty);
          Generator.PlaceRoomCoins(DetailSquareList);
          Generator.PlaceRoomAssets(DetailSquareList);
          Generator.PlaceRoomHorde(FloorSquareList);
          Generator.PlaceRoomCharacter(FloorSquareList);

          // maybe generate a broken chest because this is a looted ruin.
          if (Chance.OneIn3.Hit())
          {
            var BrokenSquare = DetailSquareList.Where(S => S.Stash == null).GetRandomOrNull();

            if (BrokenSquare != null)
            {
              Generator.PlaceSpill(BrokenSquare, Codex.Volatiles.blaze, Clock.Zero);

              var BrokenAsset = Generator.NewSpecificAsset(BrokenSquare, Generator.ContainerItems.GetRandomOrNull());
              Generator.BreakContainer(BrokenAsset);
              BrokenSquare.PlaceAsset(BrokenAsset);
            }
          }

          Maker.PlaceRoomBoulders(DetailSquareList);
        }

        // natural details.
        foreach (var NaturalSquare in RuinMap.GetSquares(RuinRegion).Where(S => S.Floor?.Ground == RuinVariant.NaturalGround && S.Passage == null))
        {
          NaturalSquare.SetLit(RuinVariant.NaturalIsLit);

          if (NaturalSquare.GetAdjacentSquares().All(S => S.Floor != null && S.Wall == null && S.Boulder == null && S.Door == null))
          {
            if (Chance.OneIn5.Hit())
            {
              NaturalSquare.SetFloor(null);
              Generator.PlaceSolidWall(NaturalSquare, RuinVariant.NaturalBarrier, WallSegment.Pillar);
            }
            else if (Chance.OneIn5.Hit())
            {
              if (Chance.OneIn5.Hit())
                Generator.PlaceFloor(NaturalSquare, Codex.Grounds.water);
              else
                Generator.PlaceBoulder(NaturalSquare, RuinVariant.NaturalBlock, IsRigid: true);

              if (Chance.OneIn5.Hit())
                Generator.PlaceRandomAsset(NaturalSquare);
            }
            else if (Chance.OneIn5.Hit())
            {
              Generator.PlaceTrap(NaturalSquare, RuinVariant.MaximumDifficulty, Revealed: false);
            }
            else if (NaturalSquare.Character == null)
            {
              if (Chance.OneIn10.Hit())
                Generator.PlaceCharacter(NaturalSquare, RuinVariant.MinimumDifficulty, RuinVariant.MaximumDifficulty);

              if (Chance.OneIn20.Hit())
                Generator.PlaceRandomAsset(NaturalSquare);
            }
          }
          else if (NaturalSquare.GetAdjacentSquares().Count(S => S.Wall != null && S.Wall.IsSolid()) == 7 && NaturalSquare.GetNeighbourSquares().Count(S => S.Floor != null) == 1)
          {
            // place containers in dead-ends.
            var BlockageSquare = NaturalSquare.GetNeighbourSquares().Where(S => S.Floor != null && S.Wall == null).GetRandomOrNull();
            if (BlockageSquare != null)
            {
              if (BlockageSquare.Door == null && BlockageSquare.Boulder == null)
              {
                if (Chance.OneIn4.Hit())
                  Generator.PlaceWall(BlockageSquare, Codex.Barriers.iron_bars, WallStructure.Solid, WallSegment.Pillar);
                else if (Chance.OneIn4.Hit())
                  Generator.PlaceWall(BlockageSquare, RuinVariant.NaturalBarrier, WallStructure.Illusionary, WallSegment.Pillar);
                else if (Chance.OneIn4.Hit())
                  Generator.PlaceBoulder(BlockageSquare, RuinVariant.NaturalBlock, IsRigid: true);
              }

              if (Chance.OneIn2.Hit())
              {
                var LootAsset = Generator.NewSpecificAsset(NaturalSquare, Codex.Items.chest);
                LootAsset.Container.Locked = true;
                LootAsset.Container.Trap = Maker.RandomContainerTrap(NaturalSquare, RuinVariant.MaximumDifficulty);
                Generator.StockContainer(NaturalSquare, LootAsset);
                NaturalSquare.PlaceAsset(LootAsset);
              }
              else
              {
                Generator.PlaceRandomAsset(NaturalSquare);
              }

              if (Generator.CanPlaceCharacter(NaturalSquare) && Chance.OneIn2.Hit())
                Generator.PlaceCharacter(NaturalSquare, RuinVariant.MinimumDifficulty, RuinVariant.MaximumDifficulty, E => !E.IsHead); // don't put multi-segment worms in dead-ends.
              else if (Generator.CanPlaceTrap(NaturalSquare) && Chance.OneIn2.Hit())
                Generator.PlaceTrap(NaturalSquare, RuinVariant.MaximumDifficulty, Revealed: false);
              else if (Generator.CanPlaceTrap(BlockageSquare))
                Generator.PlaceTrap(BlockageSquare, RuinVariant.MaximumDifficulty, Revealed: false);
              else
                Generator.PlaceSpill(BlockageSquare, Codex.Volatiles.blaze, Clock.Zero); // can't block with anything in particular, so something blew up here.

              Maker.SnoozeCharacter(NaturalSquare.Character);
            }
          }
        }

        // maybe break some walls into boulders and then maybe into rocks.
        if (RuinVariant.RoomBlock != null)
        {
          foreach (var RuinSquare in RuinMap.GetSquares(RuinRegion).Where(S => S.Wall?.Barrier == RuinVariant.RoomBarrier))
          {
            if (Chance.OneIn8.Hit())
            {
              RuinSquare.SetWall(null);

              Generator.PlaceFloor(RuinSquare, RuinVariant.BrokenGround); // floor under the boulder.
              Generator.PlaceBoulder(RuinSquare, RuinVariant.RoomBlock, IsRigid: true);

              if (Chance.OneIn8.Hit())
              {
                Generator.BreakBoulder(RuinSquare);
                Generator.PlaceSpill(RuinSquare, Codex.Volatiles.blaze, Clock.Zero);
              }
            }
          }
        }

        // get the map ready.
        Generator.RepairMap(RuinMap, RuinRegion);

        // TODO:
        // * river through ruins?
        // * more fine details such as use of tricks.
        // * packs of thieves since this is a ruins?
      }

      private sealed class RuinVariant
      {
        public Barrier NaturalBarrier;
        public Ground NaturalGround;
        public Gate NaturalGate;
        public Block NaturalBlock;
        public bool NaturalIsLit;
        public Barrier RoomBarrier;
        public Ground RoomGround;
        public Block RoomBlock;
        public Ground BrokenGround;
        public Gate RoomGate;
        public int MinimumDifficulty;
        public int MaximumDifficulty;
        public Action<Square> EnterAction;
        public Action<Square> ExitAction;
      }
    }

    private sealed class OutpostBuilder : Builder
    {
      public OutpostBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.OutpostVariance = new Variance<OutpostVariant>
        (
          new OutpostVariant
          {
            Name = OpusTerms.Outpost,
            Moat = Codex.Grounds.water,
            Drawbridge = Codex.Platforms.wooden_bridge,
            Ground = Codex.Grounds.stone_floor,
            Barrier = Codex.Barriers.stone_wall,
            Gate = Codex.Gates.wooden_door
          }
          /*,
          new OutpostVariant
          {
            Moat = Codex.Grounds.lava,
            Drawbridge = Codex.Platforms.crystal_bridge,
            Ground = Codex.Grounds.obsidian_floor,
            Barrier = Codex.Barriers.hell_brick,
            Gate = Codex.Gates.crystal_door
          }*/
        );
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        var OutpostVariant = OutpostVariance.NextVariant();
        Section.OverlandAreaName = OutpostVariant.Name;

        var OutpostMap = Maker.OverlandMap;
        var OutpostClearing = Section.LargestClearing;
        var OutpostX = OutpostClearing.Circle.X;
        var OutpostY = OutpostClearing.Circle.Y;
        var OutpostRadius = 6;

        if (OutpostRadius >= OutpostClearing.Circle.Radius)
          OutpostRadius = 4;

        if (OutpostRadius >= OutpostClearing.Circle.Radius)
          OutpostRadius = 2;

        var OutpostRegion = new Region(OutpostX - OutpostRadius - 1, OutpostY - OutpostRadius - 1, OutpostX + OutpostRadius + 1, OutpostY + OutpostRadius + 1);
        var OutpostCircle = new Inv.Circle(OutpostX, OutpostY, OutpostRadius);
        var MiddleSquare = OutpostMap[OutpostX, OutpostY];
        var EntranceSquare = MiddleSquare.GetNeighbourSquares(OutpostRadius + 1).GetRandomOrNull();

        // Outpost floor space.
        foreach (var OutpostSquare in OutpostMap.GetCircleInnerSquares(OutpostCircle))
        {
          OutpostSquare.SetLit(true);
          Generator.PlaceFloor(OutpostSquare, OutpostVariant.Ground);
        }

        // make the Outpost entrance.
        Generator.PlaceFloor(EntranceSquare, OutpostVariant.Ground);
        Generator.PlaceLockedVerticalDoor(EntranceSquare, OutpostVariant.Gate, SecretBarrier: OutpostVariant.Barrier);

        // inner Outpost ladder well.
        //foreach (var OutpostSquare in MiddleSquare.GetAdjacentSquares())
        //{
        //  OutpostSquare.SetFloor(null);
        //  Generator.PlaceSolidWall(OutpostSquare, OutpostVariant.Barrier, WallSegment.Pillar);
        //}

        // ladders.
        //Generator.PlacePassage(MiddleSquare, Codex.Portals.wooden_ladder_up, Destination: null);

        // outer Outpost walls.
        foreach (var OutpostSquare in OutpostMap.GetCircleOuterSquares(OutpostCircle))
        {
          if (OutpostSquare != EntranceSquare)
          {
            OutpostSquare.SetFloor(null);
            Generator.PlaceSolidWall(OutpostSquare, OutpostVariant.Barrier, WallSegment.Pillar);
          }
        }

        // moat, if there is enough room.
        var MoatChance = Chance.Always;
        if (OutpostRadius <= OutpostClearing.Circle.Radius - 4 && MoatChance.Hit())
        {
          // place the moat.
          foreach (var MoatSquare in OutpostMap.GetSquares(OutpostRegion.Expand(1)))
          {
            if (MoatSquare.Floor?.Ground == Codex.Grounds.dirt && MoatSquare.GetAdjacentSquares().Any(S => S.Wall?.Barrier == OutpostVariant.Barrier))
              Generator.PlaceFloor(MoatSquare, OutpostVariant.Moat);
          }

          // make a drawbridge over the moat.
          var BridgeSquare = EntranceSquare.GetCompassSquares().Where(S => S.Floor?.Ground == OutpostVariant.Moat).GetRandomOrNull();
          if (BridgeSquare != null)
            Generator.PlaceBridge(BridgeSquare, OutpostVariant.Drawbridge, BridgeOrientation.Horizontal);

          // water moat.
          foreach (var MoatSquare in OutpostMap.GetSquares(OutpostRegion.Expand(1)))
          {
            if (MoatSquare.Bridge == null && MoatSquare.Floor?.Ground == Codex.Grounds.water && Chance.OneIn8.Hit())
              Generator.PlaceRandomCharacter(MoatSquare, Section.MinimumDifficulty, Section.MaximumDifficulty);
          }
        }

        // Outpost rooms.
        var RoomRegionCount = OutpostRadius - 1;
        var RoomRegionList = new Inv.DistinctList<Region>(RoomRegionCount);

        foreach (var RoomRegionIndex in RoomRegionCount.NumberSeries())
        {
          var RoomAttempt = 0;
          do
          {
            var RoomLeft = RandomSupport.NextRange(0, OutpostRadius - 2);
            var RoomRight = RoomLeft + RandomSupport.NextRange(1, OutpostRadius - RoomLeft - 1);
            var RoomTop = RandomSupport.NextRange(0, OutpostRadius - 2);
            var RoomBottom = RoomTop + RandomSupport.NextRange(1, OutpostRadius - RoomTop - 1);
            var RoomRegion = new Region(OutpostX - OutpostRadius + 1 + (RoomLeft * 2), OutpostY - OutpostRadius + 1 + (RoomTop * 2), OutpostX - OutpostRadius + 1 + (RoomRight * 2), OutpostY - OutpostRadius + 1 + (RoomBottom * 2));

            if (!RoomRegionList.Contains(RoomRegion))
            {
              RoomRegionList.Add(RoomRegion);
              break;
            }
          }
          while (RoomAttempt++ < 5);
        }

        foreach (var RoomRegion in RoomRegionList)
        {
          foreach (var RoomSquare in OutpostMap.GetFrameSquares(RoomRegion))
          {
            if (RoomSquare.Floor?.Ground == OutpostVariant.Ground && RoomSquare.Door == null && RoomSquare.Passage == null)
            {
              RoomSquare.SetFloor(null);
              Generator.PlaceSolidWall(RoomSquare, OutpostVariant.Barrier, WallSegment.Pillar);
            }
          }
        }

        // take one step inside the entrance.
        var WalkSquare = EntranceSquare.Adjacent(EntranceSquare.AsDirection(MiddleSquare));

        if (WalkSquare.Wall != null)
        {
          Debug.Fail("Not expected to have a wall immediately at the entrance to the Outpost.");

          WalkSquare.SetWall(null);
          Generator.PlaceFloor(WalkSquare, OutpostVariant.Ground);
        }

        // ensure we have carved a dirt veranda around the Outpost.
        Maker.RepairVeranda(OutpostMap, OutpostRegion.Expand(1), Codex.Grounds.dirt, IsLit: true);

        // flood the Outpost, punching doors until we have touched all the squares.
        Maker.ConnectSquares(OutpostMap.GetSquares(OutpostRegion).Where(S => S.Floor?.Ground == OutpostVariant.Ground), WalkSquare, PunchSquare =>
        {
          Generator.PlaceDoor(PunchSquare, OutpostVariant.Gate, DoorOrientation.Vertical, SecretBarrier: PunchSquare.Wall.Barrier);
        });

        // find a dead-end to store a prisoner behind bars.
        var PrisonSquare = OutpostMap.GetSquares(OutpostRegion).Where(S => S.Floor?.Ground == OutpostVariant.Ground && S.GetAdjacentSquares().Count(S => S.Wall != null) == 7 && S.GetNeighbourSquares().Count(S => S.Floor != null && S.Door == null) == 1).GetRandomOrNull();
        if (PrisonSquare != null)
        {
          var BarsSquare = PrisonSquare.GetNeighbourSquares().Where(S => S.Floor != null && S.Door == null).GetRandomOrNull();
          if (BarsSquare != null)
          {
            Generator.PlaceWall(BarsSquare, Codex.Barriers.iron_bars, WallStructure.Solid, WallSegment.Pillar);
            Generator.PlaceCharacter(PrisonSquare, Maker.RandomMercenaryEntity(Section));
            var PrisonCharacter = PrisonSquare.Character;
            if (PrisonCharacter != null)
            {
              Maker.SnoozeCharacter(PrisonCharacter);

              static bool IsChest(Square S) => S.Floor != null && S.Character == null && S.Door == null && S.Wall == null;

              var ChestSquare = BarsSquare.Adjacent(PrisonSquare.AsDirection(BarsSquare));
              if (!IsChest(ChestSquare))
                ChestSquare = OutpostMap.GetSquares(OutpostRegion).Where(S => IsChest(S)).GetRandomOrNull() ?? PrisonSquare;

              // move the prisoner equipment to a locked chest just outside the jail.
              var ContainerAsset = Generator.NewSpecificAsset(ChestSquare, Codex.Items.large_box);
              ContainerAsset.Container.Locked = true;
              ContainerAsset.Container.Trap = Maker.RandomContainerTrap(ChestSquare, Section.Distance);
              ChestSquare.PlaceAsset(ContainerAsset);

              foreach (var Asset in PrisonCharacter.Inventory.RemoveAllAssets())
              {
                if (Asset.Container != null)
                  ChestSquare.PlaceAsset(Asset);
                else
                  ContainerAsset.Container.Stash.Add(Asset);
              }
            }
          }
        }

        var FortSoldierArray = Codex.Entities.List.Where(E => E.Startup.HasSkill(Codex.Skills.firearms) && !E.IsMercenary && E.IsEncounter).OrderBy(E => E.Level).ToArray();
        var FortSoldierMaxChallenge = FortSoldierArray.Max(E => E.Challenge);
        var FortSoldierProbability = FortSoldierArray.ToProbability(E => FortSoldierMaxChallenge - E.Challenge);

        var SoldierNumberDice = 1.d3() + 3; // 4-6
        foreach (var SoliderIndex in SoldierNumberDice.Roll().NumberSeries())
        {
          var SoldierSquare = OutpostMap.GetSquares(OutpostRegion).Where(S => S.Floor?.Ground == OutpostVariant.Ground && S.Door == null && S.Character == null).GetRandomOrNull();
          if (SoldierSquare != null)
            Generator.PlaceCharacter(SoldierSquare, FortSoldierProbability.GetRandomOrNull());
        }

        var BedNumberDice = 1.d3() + 1; // 2-4
        foreach (var Bed in BedNumberDice.Roll().NumberSeries())
        {
          var BedSquare = OutpostMap.GetSquares(OutpostRegion).Where(S => S.Floor?.Ground == OutpostVariant.Ground && Generator.CanPlaceFeature(S) && Maker.IsCorner(S, S => S.Wall != null, S => S.Floor != null && S.Boulder == null && S.Fixture == null && S.Door == null)).GetRandomOrNull();
          if (BedSquare != null)
            Generator.PlaceFixture(BedSquare, Codex.Features.bed);
        }

        // TODO:
        // * ladder down to cellar in Underground.

        BuildStop();
      }

      private readonly Variance<OutpostVariant> OutpostVariance;

      private sealed class OutpostVariant
      {
        public string Name;
        public Ground Ground;
        public Barrier Barrier;
        public Gate Gate;
        public Ground Moat;
        public Platform Drawbridge;
      }
    }

    private sealed class PrisonBuilder : Builder
    {
      public PrisonBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.PrisonVariance = new Variance<PrisonVariant>
        (
          new PrisonVariant
          {
            Name = OpusTerms.Prison
          }
        );
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        var PrisonVariant = PrisonVariance.NextVariant();
        Section.OverlandAreaName = PrisonVariant.Name;

        var AboveMap = Maker.OverlandMap;
        var PrisonRegion = Section.Region.Reduce(5);

        // odd-sized region.
        if (PrisonRegion.Width % 2 == 0)
          PrisonRegion = new Region(PrisonRegion.Left, PrisonRegion.Top, PrisonRegion.Right - 1, PrisonRegion.Bottom);

        if (PrisonRegion.Height % 2 == 0)
          PrisonRegion = new Region(PrisonRegion.Left, PrisonRegion.Top, PrisonRegion.Right, PrisonRegion.Bottom - 1);

        // square-sized region.
        if (PrisonRegion.Width > PrisonRegion.Height)
          PrisonRegion = PrisonRegion.Reduce((PrisonRegion.Width - PrisonRegion.Height) / 2, 0);
        else if (PrisonRegion.Height > PrisonRegion.Width)
          PrisonRegion = PrisonRegion.Reduce(0, (PrisonRegion.Height - PrisonRegion.Width) / 2);

        var PrisonSize = PrisonRegion.Width;
        var PrisonMoat = PrisonSize >= 11;
        if (PrisonMoat)
        {
          PrisonRegion = PrisonRegion.Reduce(1);
          PrisonSize -= 2;
        }
        Debug.Assert(PrisonSize == PrisonRegion.Height);

        // fort content.
        var FortSoldierArray = Codex.Entities.List.Where(E => E.Startup.HasSkill(Codex.Skills.firearms) && !E.IsMercenary && E.IsEncounter).OrderBy(E => E.Level).ToArray();
        var FortSoldierMaxChallenge = FortSoldierArray.Max(E => E.Challenge);
        var FortSoldierProbability = FortSoldierArray.ToProbability(E => FortSoldierMaxChallenge - E.Challenge);
        var FortHoundProbability = new[] { Codex.Entities.pit_bull, Codex.Entities.large_dog, Codex.Entities.dog }.ToProbability(I => I.Frequency);
        var FortFoodProbability = new[] { Codex.Items.cration, Codex.Items.kration }.ToProbability(I => I.Rarity);
        var FortGemProbability = Codex.Items.List.Where(I => I.Type == ItemType.Gem && I.Price > Gold.One && !I.Grade.Unique).ToProbability(I => I.Rarity);
        var FortMoatEntityArray = new[] { Codex.Entities.baby_crocodile, Codex.Entities.crocodile, Codex.Entities.jellyfish, Codex.Entities.electric_eel, Codex.Entities.giant_eel, Codex.Entities.shark };

        // middle of the facility.
        var RallySquare = AboveMap[PrisonRegion.Midpoint()];

        // guard tower areas.
        var TopLeftCircle = new Inv.Circle(PrisonRegion.Left, PrisonRegion.Top, 3);
        var TopRightCircle = new Inv.Circle(PrisonRegion.Right, PrisonRegion.Top, 3);
        var BottomLeftCircle = new Inv.Circle(PrisonRegion.Left, PrisonRegion.Bottom, 3);
        var BottomRightCircle = new Inv.Circle(PrisonRegion.Right, PrisonRegion.Bottom, 3);

        var GuardCircleArray = new[] { TopLeftCircle, TopRightCircle, BottomLeftCircle, BottomRightCircle };

        foreach (var GuardCircle in GuardCircleArray)
        {
          foreach (var PrisonSquare in AboveMap.GetCircleInnerSquares(GuardCircle))
          {
            PrisonSquare.SetLit(true);
            Generator.PlaceFloor(PrisonSquare, Codex.Grounds.stone_floor);
          }
        }

        // courtyard.
        foreach (var PrisonSquare in AboveMap.GetSquares(PrisonRegion.Reduce(1)))
          Generator.PlaceFloor(PrisonSquare, Codex.Grounds.stone_floor);

        // outer walls.
        foreach (var PrisonSquare in AboveMap.GetSquares(Section.Region))
        {
          if (PrisonSquare.Floor?.Ground == Codex.Grounds.stone_floor)
          {
            PrisonSquare.SetLit(true);

            if (PrisonSquare.GetAdjacentSquares().Any(S => S.Floor?.Ground == Codex.Grounds.dirt || S.IsVoid()))
            {
              PrisonSquare.SetFloor(null);
              Generator.PlaceSolidWall(PrisonSquare, Codex.Barriers.stone_wall, WallSegment.Pillar);
            }
          }
        }

        if (PrisonMoat)
        {
          // water around the walls.
          foreach (var PrisonSquare in AboveMap.GetSquares(Section.Region))
          {
            if ((PrisonSquare.Floor?.Ground == Codex.Grounds.dirt || PrisonSquare.IsVoid()) && PrisonSquare.GetAdjacentSquares().Any(S => S.Wall?.Barrier == Codex.Barriers.stone_wall))
            {
              PrisonSquare.SetLit(true);
              PrisonSquare.SetWall(null);
              Generator.PlaceFloor(PrisonSquare, Codex.Grounds.water);

              if (PrisonSquare.X != RallySquare.X && PrisonSquare.Y != RallySquare.Y && PrisonSquare.Floor?.Ground == Codex.Grounds.water && Chance.OneIn8.Hit())
                Generator.PlaceCharacter(PrisonSquare, Section.MinimumDifficulty, Section.MaximumDifficulty, FortMoatEntityArray);
            }
          }

          // dirt around the water.
          foreach (var PrisonSquare in AboveMap.GetSquares(Section.Region))
          {
            if (PrisonSquare.IsVoid() && PrisonSquare.GetAdjacentSquares().Any(S => S.Floor?.Ground == Codex.Grounds.water))
            {
              PrisonSquare.SetLit(true);
              Generator.PlaceFloor(PrisonSquare, Codex.Grounds.dirt);
            }
          }
        }

        // ensure accessibility around the facility.
        Maker.RepairVeranda(AboveMap, Section.Region, Codex.Grounds.dirt, IsLit: true);

        // main door.
        IEnumerable<Square> ExtendNeighbourSquares(Square InitialSquare, int RangeStart, int RangeLimit)
        {
          for (var RangeIndex = RangeStart; RangeIndex <= RangeLimit; RangeIndex++)
          {
            foreach (var RangeSquare in InitialSquare.GetNeighbourSquares(RangeIndex))
              yield return RangeSquare;
          }
        }

        var DoorSquare = ExtendNeighbourSquares(RallySquare, PrisonSize / 2 - 1, PrisonSize).Where(S => S.Wall?.Barrier == Codex.Barriers.stone_wall && S.IsFlat()).GetRandomOrNull();

        if (DoorSquare == null)
        {
          Debug.Fail("Should always be able to find an appropriate door placement.");
          DoorSquare = AboveMap.GetSquares(Section.Region).Where(S => S.Wall?.Barrier == Codex.Barriers.stone_wall && S.IsFlat()).GetRandomOrNull();
        }

        var DoorDistance = DoorSquare.AsRange(RallySquare);
        var DoorDirection = RallySquare.AsDirection(DoorSquare);

        Generator.PlaceLockedVerticalDoor(DoorSquare, Codex.Gates.wooden_door, Codex.Barriers.stone_wall);

        if (PrisonMoat)
        {
          // drawbridge.
          var MoatSquare = DoorSquare.Adjacent(DoorDirection);
          while (MoatSquare.Floor?.Ground == Codex.Grounds.water)
          {
            Generator.PlaceBridge(MoatSquare, Codex.Platforms.wooden_bridge, DoorDirection.IsVertical() ? BridgeOrientation.Vertical : BridgeOrientation.Horizontal);

            MoatSquare = MoatSquare.Adjacent(DoorDirection);
          }
        }

        // main house.
        var HouseSize = PrisonSize > 9 ? 2 : 1;
        var HouseRegion = RallySquare.SurroundingRegion(HouseSize);

        var HouseZone = AboveMap.AddZone();
        HouseZone.AddRegion(HouseRegion);
        if (HouseZone.Squares.Count == 0)
          AboveMap.RemoveZone(HouseZone);

        foreach (var PrisonSquare in AboveMap.GetFrameSquares(HouseRegion))
        {
          PrisonSquare.SetFloor(null);
          Generator.PlaceSolidWall(PrisonSquare, Codex.Barriers.stone_wall, WallSegment.Pillar);
        }

        // door to the main house.
        var HouseDirection = DoorDirection.Reverse();
        var HouseSquare = RallySquare.Adjacent(HouseDirection, HouseSize);
        Generator.PlaceLockedVerticalDoor(HouseSquare, Codex.Gates.wooden_door, Codex.Barriers.stone_wall);

        IEnumerable<Square> ExtendHouseSquares(Square CornerSquare, Direction Direction)
        {
          var ResultSquare = CornerSquare.Adjacent(Direction);

          while (ResultSquare != null)
          {
            yield return ResultSquare;

            ResultSquare = ResultSquare.Adjacent(Direction);
          }
        }

        // extend main house to separate the guard houses.
        void InternalHouseWalls(Square CornerSquare, Direction Direction)
        {
          foreach (var PrisonSquare in ExtendHouseSquares(CornerSquare, Direction))
          {
            if (PrisonSquare.Floor?.Ground != Codex.Grounds.stone_floor)
              break;

            if (PrisonSquare.IsFlat())
            {
              Generator.PlaceClosedVerticalDoor(PrisonSquare, Codex.Gates.wooden_door, Codex.Barriers.stone_wall);
              break;
            }
            else
            {
              PrisonSquare.SetFloor(null);
              Generator.PlaceSolidWall(PrisonSquare, Codex.Barriers.stone_wall, WallSegment.Pillar);
            }
          }
        }

        var HouseTopLeftSquare = RallySquare.Adjacent(new Offset(-HouseSize, -HouseSize));
        var HouseBottomLeftSquare = RallySquare.Adjacent(new Offset(-HouseSize, +HouseSize));
        var HouseTopRightSquare = RallySquare.Adjacent(new Offset(+HouseSize, -HouseSize));
        var HouseBottomRightSquare = RallySquare.Adjacent(new Offset(+HouseSize, +HouseSize));

        InternalHouseWalls(HouseTopLeftSquare, Direction.West);
        InternalHouseWalls(HouseTopLeftSquare, Direction.North);
        InternalHouseWalls(HouseTopRightSquare, Direction.East);
        InternalHouseWalls(HouseTopRightSquare, Direction.North);
        InternalHouseWalls(HouseBottomLeftSquare, Direction.West);
        InternalHouseWalls(HouseBottomLeftSquare, Direction.South);
        InternalHouseWalls(HouseBottomRightSquare, Direction.East);
        InternalHouseWalls(HouseBottomRightSquare, Direction.South);

        // guard corridors.
        var HouseDistance = DoorDistance - (HouseSize - 1);

        var HouseCorridorArray = new[]
        {
          new { Direction = Direction.North, Region = new Region(HouseTopLeftSquare.X, HouseTopLeftSquare.Y - HouseDistance + 1, HouseTopRightSquare.X, HouseTopRightSquare.Y - 1) },
          new { Direction = Direction.West, Region = new Region(HouseTopLeftSquare.X - HouseDistance + 1, HouseTopLeftSquare.Y, HouseBottomLeftSquare.X - 1, HouseBottomLeftSquare.Y) },
          new { Direction = Direction.East, Region = new Region(HouseTopRightSquare.X + 1, HouseTopRightSquare.Y, HouseBottomRightSquare.X + HouseDistance - 1, HouseBottomRightSquare.Y) },
          new { Direction = Direction.South, Region = new Region(HouseBottomLeftSquare.X, HouseBottomLeftSquare.Y + 1, HouseBottomRightSquare.X, HouseBottomRightSquare.Y + HouseDistance - 1 ) }
        };

        foreach (var HouseCorridor in HouseCorridorArray)
        {
          var CorridorZone = AboveMap.AddZone();
          CorridorZone.AddRegion(HouseCorridor.Region);
          if (CorridorZone.Squares.Count == 0)
            AboveMap.RemoveZone(CorridorZone);
        }

        // guard tower barracks.
        foreach (var GuardCircle in GuardCircleArray)
        {
          var GuardZone = AboveMap.AddZone();
          foreach (var PrisonSquare in AboveMap.GetCircleInnerSquares(GuardCircle))
            GuardZone.AddSquare(PrisonSquare);
          if (GuardZone.Squares.Count == 0)
            AboveMap.RemoveZone(GuardZone);

          var BedNumberDice = 1.d2();
          foreach (var Bed in BedNumberDice.Roll().NumberSeries())
          {
            var BedSquare = GuardZone.Squares.Where(S => S.Floor?.Ground == Codex.Grounds.stone_floor && Generator.CanPlaceFeature(S) && Maker.IsCorner(S, S => S.Wall != null, S => S.Floor != null && S.Boulder == null && S.Fixture == null && S.Door == null && S.Character == null)).GetRandomOrNull();
            if (BedSquare != null)
            {
              Generator.PlaceFixture(BedSquare, Codex.Features.bed);
              Maker.SnoozeCharacter(Generator.PlaceCharacter(BedSquare, FortSoldierProbability.GetRandomOrNull()));
            }
          }

          var LockerNumberDice = 1.d2();
          foreach (var Locker in LockerNumberDice.Roll().NumberSeries())
          {
            var LockerSquare = GuardZone.Squares.Where(S => S.Floor?.Ground == Codex.Grounds.stone_floor && Generator.CanPlaceFeature(S) && Maker.IsCorner(S, S => S.Wall != null, S => S.Floor != null && S.Boulder == null && S.Fixture == null && S.Door == null && S.Character == null)).GetRandomOrNull();
            if (LockerSquare != null)
            {
              var LargeboxAsset = Generator.NewSpecificAsset(LockerSquare, Codex.Items.large_box);
              LargeboxAsset.Container.Locked = true;
              LargeboxAsset.Container.Trap = Maker.RandomContainerTrap(LockerSquare, Section.Distance);
              LockerSquare.PlaceAsset(LargeboxAsset);

              LargeboxAsset.Container.Stash.Add(Generator.CreateCoins(LockerSquare, Generator.RandomCoinQuantity(LockerSquare) * 1.d3().Roll()));
              LargeboxAsset.Container.Stash.Add(Generator.NewSpecificAsset(LockerSquare, FortGemProbability.GetRandomOrNull()));
              if (Chance.OneIn4.Hit())
                LargeboxAsset.Container.Stash.Add(Generator.NewSpecificAsset(LockerSquare, Codex.Items.brass_bugle));
            }
          }

          var SoldierNumberDice = 1.d2() + 1;
          foreach (var SoliderIndex in SoldierNumberDice.Roll().NumberSeries())
          {
            var SoldierSquare = GuardZone.Squares.Where(S => S.Floor?.Ground == Codex.Grounds.stone_floor && S.Door == null && S.Character == null).GetRandomOrNull();
            if (SoldierSquare != null)
              Generator.PlaceCharacter(SoldierSquare, FortSoldierProbability.GetRandomOrNull());
          }

          var HoundSquare = GuardZone.Squares.Where(S => S.Floor?.Ground == Codex.Grounds.stone_floor && S.Door == null && S.Character == null).GetRandomOrNull();
          if (HoundSquare != null)
            Generator.PlaceCharacter(HoundSquare, FortHoundProbability.GetRandomOrNull());
        }

        // welcome corridor.
        var WelcomeCorridor = HouseCorridorArray.Find(H => H.Direction == DoorDirection);
        var WelcomeSquare = AboveMap[WelcomeCorridor.Region.Midpoint()];
        if (DoorDirection == Direction.East)
          WelcomeSquare = WelcomeSquare.Adjacent(Direction.West);
        else if (DoorDirection == Direction.South)
          WelcomeSquare = WelcomeSquare.Adjacent(Direction.North);
        Generator.PlaceFixture(WelcomeSquare, Codex.Features.fountain);

        // bench corridor.
        var BenchCorridor = HouseCorridorArray.Find(H => H.Direction == HouseDirection);
        var BenchSquare = AboveMap[BenchCorridor.Region.Midpoint()];
        if (HouseDirection == Direction.East)
          BenchSquare = BenchSquare.Adjacent(Direction.West);
        else if (HouseDirection == Direction.South)
          BenchSquare = BenchSquare.Adjacent(Direction.North);
        Generator.PlaceFixture(BenchSquare, Codex.Features.workbench);

        // mess corridor.
        var MessCorridor = HouseCorridorArray.Where(H => H != WelcomeCorridor && H != BenchCorridor).GetRandomOrNull();
        foreach (var PrisonSquare in AboveMap.GetSquares(MessCorridor.Region).Where(S => S.Floor?.Ground == Codex.Grounds.stone_floor && S.Door == null))
          Generator.PlaceSpecificAsset(PrisonSquare, FortFoodProbability.GetRandomOrNull());

        // hound corridor.
        var HoundCorridor = HouseCorridorArray.Where(H => H != WelcomeCorridor && H != BenchCorridor && H != MessCorridor).GetRandomOrNull();
        foreach (var PrisonSquare in AboveMap.GetSquares(HoundCorridor.Region).Where(S => S.Floor?.Ground == Codex.Grounds.stone_floor && S.Door == null && S.Character == null))
          Generator.PlaceCharacter(PrisonSquare, FortHoundProbability.GetRandomOrNull());

        // below the facility.
        var PrisonName = Generator.EscapedModuleTerm(OpusTerms.Prison);
        if (!Generator.Adventure.World.HasSite(PrisonName))
        {
          var PrisonSite = Generator.Adventure.World.AddSite(PrisonName);

          const int BelowSize = 33;

          var BelowMap = Generator.Adventure.World.AddMap(PrisonName, BelowSize, BelowSize);
          BelowMap.SetDifficulty(Section.Distance + 1);
          BelowMap.SetTerminal(true);
          BelowMap.SetSealed(true);
          BelowMap.SetAtmosphere(Codex.Atmospheres.dungeon);

          var BelowLevel = PrisonSite.AddLevel(1, BelowMap);

          // permanent stone wall around entire map.
          Generator.PlaceRoom(BelowMap, Codex.Barriers.stone_wall, Codex.Grounds.stone_path, BelowMap.Region);
          foreach (var PrisonSquare in BelowMap.GetFrameSquares(BelowMap.Region))
            PrisonSquare.Wall.SetStructure(WallStructure.Permanent);

          // building grid.
          var BuildingSize = 5;
          var BuildingGrid = new Inv.Grid<OpusBuilding>(BuildingSize, BuildingSize);
          BuildingGrid.Fill((X, Y) =>
          {
            var BuildingLeft = X * 6 + 2;
            var BuildingTop = Y * 6 + 2;
            return new OpusBuilding(new Region(BuildingLeft, BuildingTop, BuildingLeft + BuildingSize - 1, BuildingTop + BuildingSize - 1));
          });
          var BuildingList = BuildingGrid.ToDistinctList();

          var ShaftBuilding = BuildingGrid[BuildingSize / 2, BuildingSize / 2];
          BuildingList.Remove(ShaftBuilding);

          var StorageBuilding = BuildingList.RemoveRandomOrNull();

          var StorageAssetList = new Inv.DistinctList<Asset>();

          Zone PlaceZone(OpusBuilding Building)
          {
            var Zone = BelowMap.AddZone();
            Zone.AddRegion(Building.Region);
            Zone.SetAccessRestricted(true);
            Zone.SetSpawnRestricted(true);
            Zone.SetLit(true);
            Debug.Assert(Zone.HasSquares());
            return Zone;
          }
          void PlaceShaft(OpusBuilding Building)
          {
            var ShaftZone = PlaceZone(Building);

            foreach (var PrisonSquare in BelowMap.GetFrameSquares(Building.Region))
            {
              PrisonSquare.SetFloor(null);
              Generator.PlacePermanentWall(PrisonSquare, Codex.Barriers.stone_wall, WallSegment.Pillar);
            }

            foreach (var PrisonSquare in BelowMap.GetSquares(Building.Region.Reduce(1)))
              Generator.PlaceFloor(PrisonSquare, Codex.Grounds.stone_floor);

            var ShaftSquare = BelowMap[Building.Region.Midpoint()];

            BelowLevel.SetTransitions(ShaftSquare, null);

            // door to the main shaft.
            var AccessDirection = HouseDirection.Reverse();
            var AccessSquare = ShaftSquare.Adjacent(AccessDirection, 2);
            Generator.PlaceLockedVerticalDoor(AccessSquare, Codex.Gates.wooden_door, Codex.Barriers.stone_wall);
            AccessSquare.Door.SetReinforced(true);

            // stairs down/up.
            Generator.PlacePassage(RallySquare, Codex.Portals.stone_staircase_down, ShaftSquare);
            Generator.PlacePassage(ShaftSquare, Codex.Portals.stone_staircase_up, RallySquare);
          }
          void PlaceCell(OpusBuilding Building)
          {
            var CellZone = PlaceZone(Building);

            var HorizontalWide = Building.Region.Width >= 5;
            var VerticalWide = Building.Region.Height >= 5;
            var HorizontalSplit = HorizontalWide && Chance.OneIn5.Hit();
            var VerticalSplit = VerticalWide && Chance.OneIn5.Hit();
            var CellSquare = BelowMap[Building.Region.Midpoint()];

            foreach (var PrisonSquare in BelowMap.GetCornerSquares(Building.Region))
            {
              PrisonSquare.SetFloor(null);
              Generator.PlacePermanentWall(PrisonSquare, Codex.Barriers.stone_wall, WallSegment.Pillar);
            }

            foreach (var PrisonSquare in BelowMap.GetFrameSquares(Building.Region).Where(S => S.Wall == null))
            {
              if ((HorizontalSplit || VerticalSplit) && (CellSquare.X == PrisonSquare.X || CellSquare.Y == PrisonSquare.Y))
              {
                PrisonSquare.SetFloor(null);
                Generator.PlacePermanentWall(PrisonSquare, Codex.Barriers.stone_wall, WallSegment.Pillar);
              }
              else
              {
                Generator.PlaceFloor(PrisonSquare, Codex.Grounds.stone_floor);
                Generator.PlacePermanentWall(PrisonSquare, Codex.Barriers.iron_bars, WallSegment.Pillar);
              }
            }

            foreach (var PrisonSquare in BelowMap.GetSquares(Building.Region.Reduce(1)))
            {
              if ((HorizontalSplit && CellSquare.X == PrisonSquare.X) || (VerticalSplit && CellSquare.Y == PrisonSquare.Y) || (!HorizontalSplit && !VerticalSplit && HorizontalWide && VerticalWide && PrisonSquare == CellSquare))
              {
                PrisonSquare.SetFloor(null);
                Generator.PlacePermanentWall(PrisonSquare, Codex.Barriers.stone_wall, WallSegment.Pillar);
              }
              else
              {
                Generator.PlaceFloor(PrisonSquare, Codex.Grounds.stone_floor);

                if (Chance.OneIn5.Hit())
                {
                  var PrisonCharacter = Generator.NewCharacter(PrisonSquare, Maker.RandomMercenaryEntity(Section));
                  Generator.PlaceCharacter(PrisonSquare, PrisonCharacter);
                  Generator.PunishCharacter(PrisonCharacter, Codex.Punishments.ball__chain);

                  // move into storage cell.
                  StorageAssetList.AddRange(PrisonCharacter.Inventory.RemoveAllAssets());

                  if (Chance.OneIn4.Hit())
                    Generator.CorpseSquare(PrisonSquare);
                }
              }
            }

            Maker.ConnectSquares(BelowMap.GetSquares(Building.Region.Expand(1)).Where(S => S.Floor != null && S.Wall == null), BelowMap.GetFrameSquares(Building.Region.Expand(1)).GetRandomOrNull(), AccessSquare =>
            {
              AccessSquare.SetWall(null);
              Generator.PlaceFloor(AccessSquare, Codex.Grounds.stone_floor);
              Generator.PlaceLockedVerticalDoor(AccessSquare, Codex.Gates.iron_door, Codex.Barriers.iron_bars);
              AccessSquare.Door.SetReinforced(true);
            });
          }
          void PlaceYard(OpusBuilding Building)
          {
            var YardZone = PlaceZone(Building);

            foreach (var PrisonSquare in BelowMap.GetSquares(Building.Region))
            {
              Generator.PlaceFloor(PrisonSquare, Codex.Grounds.dirt);

              if (Chance.OneIn5.Hit())
                Generator.PlaceSpecificAsset(PrisonSquare, Codex.Items.rock);
              else if (Chance.OneIn5.Hit())
                Generator.PlaceBoulder(PrisonSquare, Codex.Blocks.stone_boulder, IsRigid: true);
              else if (Chance.OneIn10.Hit())
                Generator.PlaceSpecificAsset(PrisonSquare, Codex.Items.iron_chain);
              else if (Chance.OneIn10.Hit())
                Generator.PlaceSpecificAsset(PrisonSquare, Codex.Items.heavy_iron_ball);
              else if (Chance.OneIn10.Hit())
                Generator.PlaceSpecificAsset(PrisonSquare, Codex.Items.pickaxe);
            }
          }
          void PlaceOffice(OpusBuilding Building)
          {
            var OfficeZone = PlaceZone(Building);

            foreach (var PrisonSquare in BelowMap.GetFrameSquares(Building.Region))
            {
              PrisonSquare.SetFloor(null);
              Generator.PlacePermanentWall(PrisonSquare, Codex.Barriers.wooden_wall, WallSegment.Pillar);
            }

            foreach (var PrisonSquare in BelowMap.GetSquares(Building.Region.Reduce(1)))
              Generator.PlaceFloor(PrisonSquare, Codex.Grounds.wooden_floor);

            var ThroneSquare = BelowMap[Building.Region.Midpoint()];
            Generator.PlaceFixture(ThroneSquare, Codex.Features.throne);

            // door.
            var AccessSquare = BelowMap.GetFrameSquares(Building.Region).Where(S => S.IsFlat()).GetRandomOrNull();
            AccessSquare.SetWall(null);
            Generator.PlaceFloor(AccessSquare, Codex.Grounds.wooden_floor);
            Generator.PlaceLockedVerticalDoor(AccessSquare, Codex.Gates.gold_door, Codex.Barriers.wooden_wall);
            AccessSquare.Door.SetReinforced(true);
            AccessSquare.Door.SetKey(Codex.Items.Jade_Key);

            // fixtures.
            void PlaceBlessedFeature(Feature Feature) => Generator.PlaceFixture(ThroneSquare.GetCornerSquares().Where(S => S.Fixture == null).GetRandomOrNull(), Feature, Codex.Sanctities.Blessed);
            PlaceBlessedFeature(Codex.Features.bed);
            PlaceBlessedFeature(Codex.Features.workbench);

            // boss (has the Jade Key).
            var BossCharacter = Maker.NewEvilCharacter(ThroneSquare, Maker.SelectUniqueEntity(Codex.Entities.Croesus));
            Generator.PlaceCharacter(ThroneSquare, BossCharacter);
            Generator.EquipCharacter(BossCharacter, Codex.Slots.keys, Generator.NewSpecificAsset(ThroneSquare, Codex.Items.Jade_Key));
            Maker.SnoozeCharacter(BossCharacter); // asleep on the throne.

            // 5 x keystone kops.
            var BossScript = BossCharacter.InsertScript();
            var ShaftSquare = BelowMap[ShaftBuilding.Region.Midpoint()];
            BossScript.Killed.Sequence.Add(Codex.Tricks.keystone_kops).SetTarget(ShaftSquare);
            BossScript.Killed.Sequence.Add(Codex.Tricks.keystone_kops).SetTarget(ShaftSquare);
            BossScript.Killed.Sequence.Add(Codex.Tricks.keystone_kops).SetTarget(ShaftSquare);
            BossScript.Killed.Sequence.Add(Codex.Tricks.keystone_kops).SetTarget(ShaftSquare);
            BossScript.Killed.Sequence.Add(Codex.Tricks.keystone_kops).SetTarget(ShaftSquare);
          }
          void PlaceVault(OpusBuilding Building)
          {
            var VaultZone = PlaceZone(Building);

            foreach (var PrisonSquare in BelowMap.GetFrameSquares(Building.Region))
            {
              PrisonSquare.SetFloor(null);
              Generator.PlacePermanentWall(PrisonSquare, Codex.Barriers.jade_wall, WallSegment.Pillar);
            }

            // artifact.
            var VaultSquare = BelowMap[Building.Region.Midpoint()];
            VaultSquare.PlaceAsset(Generator.GenerateUniqueAsset(VaultSquare));

            foreach (var PrisonSquare in BelowMap.GetSquares(Building.Region.Reduce(1)))
            {
              Generator.PlaceFloor(PrisonSquare, Codex.Grounds.marble_floor);

              if (PrisonSquare != VaultSquare)
              {
                PrisonSquare.PlaceAsset(Generator.CreateCoins(PrisonSquare, Generator.RandomCoinQuantity(PrisonSquare) * 4.d4().Roll()));
                PrisonSquare.PlaceAsset(Generator.NewSpecificAsset(PrisonSquare, FortGemProbability.GetRandomOrNull()));
                PrisonSquare.PlaceAsset(Generator.NewSpecificAsset(PrisonSquare, FortGemProbability.GetRandomOrNull()));
              }
            }

            // golden door locked with Jade Key (office boss has the key).
            var AccessSquare = BelowMap.GetFrameSquares(Building.Region).Where(S => S.IsFlat()).GetRandomOrNull();
            AccessSquare.SetWall(null);
            Generator.PlaceFloor(AccessSquare, Codex.Grounds.marble_floor);
            Generator.PlaceLockedVerticalDoor(AccessSquare, Codex.Gates.gold_door, Codex.Barriers.wooden_wall);
            AccessSquare.Door.SetReinforced(true);
            AccessSquare.Door.SetKey(Codex.Items.Jade_Key);

            var VaultTrigger = VaultZone.InsertTrigger();
            VaultTrigger.Add(Delay.FromTurns(30), Codex.Tricks.calling_guard);
            VaultTrigger.Add(Delay.FromTurns(30), Codex.Tricks.calling_guard);
            VaultTrigger.Add(Delay.FromTurns(30), Codex.Tricks.calling_guard);
          }
          void PlaceGarrison(OpusBuilding Building)
          {
            var GarrisonZone = PlaceZone(Building);

            foreach (var PrisonSquare in BelowMap.GetFrameSquares(Building.Region))
            {
              PrisonSquare.SetFloor(null);
              Generator.PlacePermanentWall(PrisonSquare, Codex.Barriers.stone_wall, WallSegment.Pillar);
            }

            foreach (var PrisonSquare in BelowMap.GetSquares(Building.Region.Reduce(1)))
            {
              Generator.PlaceFloor(PrisonSquare, Codex.Grounds.stone_floor);
              Generator.PlaceFixture(PrisonSquare, Codex.Features.bed);

              Generator.PlaceCharacter(PrisonSquare, FortSoldierProbability.GetRandomOrNull());
              Maker.SnoozeCharacter(PrisonSquare.Character);
            }

            var AccessSquare = BelowMap.GetFrameSquares(Building.Region).Where(S => S.IsFlat()).GetRandomOrNull();
            AccessSquare.SetWall(null);
            Generator.PlaceFloor(AccessSquare, Codex.Grounds.stone_floor);
            Generator.PlaceClosedVerticalDoor(AccessSquare, Codex.Gates.wooden_door, Codex.Barriers.stone_wall);
            AccessSquare.Door.SetReinforced(true);
            AccessSquare.Door.SetKey(Codex.Items.Jade_Key);
          }
          void PlaceStorage(OpusBuilding Building)
          {
            var StorageZone = PlaceZone(Building);

            foreach (var PrisonSquare in BelowMap.GetFrameSquares(Building.Region))
            {
              PrisonSquare.SetFloor(null);
              Generator.PlacePermanentWall(PrisonSquare, Codex.Barriers.stone_wall, WallSegment.Pillar);
            }

            foreach (var PrisonSquare in BelowMap.GetSquares(Building.Region.Reduce(1)))
              Generator.PlaceFloor(PrisonSquare, Codex.Grounds.stone_floor);

            var AccessSquare = BelowMap.GetFrameSquares(Building.Region).Where(S => S.IsFlat()).GetRandomOrNull();
            AccessSquare.SetWall(null);
            Generator.PlaceFloor(AccessSquare, Codex.Grounds.stone_floor);
            Generator.PlaceLockedVerticalDoor(AccessSquare, Codex.Gates.iron_door, Codex.Barriers.stone_wall);
            AccessSquare.Door.SetReinforced(true);

            while (StorageAssetList.Count > 0)
            {
              foreach (var PrisonSquare in BelowMap.GetSquares(Building.Region.Reduce(1)))
              {
                var StorageAsset = StorageAssetList.RemoveLast();
                if (Chance.OneIn2.Hit()) // reduce the number of assets stored (maybe convert to gold instead?)
                  PrisonSquare.PlaceAsset(StorageAsset);

                if (StorageAssetList.Count == 0)
                  break;
              }
            }

            StorageAssetList.Clear();
          }
          void PlaceZoo(OpusBuilding Building)
          {
            var ZooZone = PlaceZone(Building);

            foreach (var PrisonSquare in BelowMap.GetFrameSquares(Building.Region))
            {
              PrisonSquare.SetFloor(null);
              Generator.PlacePermanentWall(PrisonSquare, Codex.Barriers.stone_wall, WallSegment.Pillar);
            }

            foreach (var PrisonSquare in BelowMap.GetSquares(Building.Region.Reduce(1)))
              Generator.PlaceFloor(PrisonSquare, Codex.Grounds.stone_floor);

            var AccessSquare = BelowMap.GetFrameSquares(Building.Region).Where(S => S.IsFlat()).GetRandomOrNull();
            AccessSquare.SetWall(null);
            Generator.PlaceFloor(AccessSquare, Codex.Grounds.stone_floor);
            Generator.PlaceLockedVerticalDoor(AccessSquare, Codex.Gates.iron_door, Codex.Barriers.stone_wall);
            AccessSquare.Door.SetReinforced(true);

            Generator.PlaceZoo(BelowMap.GetSquares(Building.Region.Reduce(1)).ToDistinctList(), Generator.GetZooProbability(Section.Distance).GetRandom(), Section.MinimumDifficulty, Section.MaximumDifficulty);
          }

          // entrance shaft.
          PlaceShaft(ShaftBuilding);

          // boss office.
          PlaceOffice(BuildingList.RemoveRandomOrNull());

          // treasure vault.
          PlaceVault(BuildingList.RemoveRandomOrNull());

          // exercise yards.
          PlaceYard(BuildingList.RemoveRandomOrNull());
          PlaceYard(BuildingList.RemoveRandomOrNull());

          // soldier garrisons.
          PlaceGarrison(BuildingList.RemoveRandomOrNull());
          PlaceGarrison(BuildingList.RemoveRandomOrNull());
          PlaceGarrison(BuildingList.RemoveRandomOrNull());
          PlaceGarrison(BuildingList.RemoveRandomOrNull());
          PlaceGarrison(BuildingList.RemoveRandomOrNull());
          PlaceGarrison(BuildingList.RemoveRandomOrNull());

          // monster zoos.
          PlaceZoo(BuildingList.RemoveRandomOrNull());
          PlaceZoo(BuildingList.RemoveRandomOrNull());
          PlaceZoo(BuildingList.RemoveRandomOrNull());
          PlaceZoo(BuildingList.RemoveRandomOrNull());

          // remainder buildings are cells contain neutral prisoners.
          foreach (var Building in BuildingList)
            PlaceCell(Building);

          // storage is last because we collect the equipment from the prisoners.
          PlaceStorage(StorageBuilding);

          // NOTE: can't have resident patrols with hostile soldiers because they will just seek out the player.

          Generator.RepairMap(BelowMap, BelowMap.Region);

          BelowMap.AddArea(PrisonName).AddMapZones();
        }

        // TODO:
        // * 

        BuildStop();
      }

      private readonly Variance<PrisonVariant> PrisonVariance;

      private sealed class PrisonVariant
      {
        public string Name;
      }
    }

    private sealed class SanatoriumBuilder : Builder
    {
      public SanatoriumBuilder(OpusMaker Maker)
        : base(Maker)
      {
        var EntityArray = new[]
        {
          // level 1 - buzzed
          Codex.Entities.migo_drone,     // 10
          Codex.Entities.migo_warrior,   // 14
          Codex.Entities.migo_queen,     // 18

          // level 2 - drowned
          Codex.Entities.deep_one,       // 18
          Codex.Entities.deeper_one,     // 26
          Codex.Entities.deepest_one,    // 33

          // level 3 - swarmed
          Codex.Entities.gug,            // 24
          Codex.Entities.nightgaunt,     // 27

          // level 4 - engulfed
          Codex.Entities.byakhee,        // 22
          Codex.Entities.shoggoth,       // 31
          Codex.Entities.giant_shoggoth, // 38

          // level 5 - drained.
          Codex.Entities.star_vampire,   // 30
          Codex.Entities.fire_vampire,   // 33

          // level 6 - boss.
        };

        var BossArray = new[]
        {
          Codex.Entities.Father_Dagon,
          Codex.Entities.Mother_Hydra,
          Codex.Entities.Cthulhu
        };

#if DEBUG
        //foreach (var EntityGroup in EntityArray.GroupBy(E => E.Difficulty).OrderBy(G => G.Key))
        //  Debug.WriteLine($"Entity {EntityGroup.Key:D2} = " + EntityGroup.Select(O => O.Name).OrderBy().AsSeparatedText(", "));
#endif

        this.SanatoriumVariance = new Variance<SanatoriumVariant>
        (
          new SanatoriumVariant
          {
          }
        );
      }

      public void Build(OpusSection Section, Map SanatoriumMap)
      {
        BuildStart();

        var SanatoriumVariant = SanatoriumVariance.NextVariant();

        // TODO:
        // * first level is pristine and then degenerates each level downwards.
        // * entrance to the sanatorium can only be found while hallucinating?

        BuildStop();
      }

      private readonly Variance<SanatoriumVariant> SanatoriumVariance;

      private sealed class SanatoriumVariant
      {
      }
    }

    private sealed class TownBuilder : Builder
    {
      public TownBuilder(OpusMaker Maker)
        : base(Maker)
      {
        this.TownVariance = new Variance<TownVariant>
        (
          new TownVariant
          {
            Name = OpusTerms.Woodtown,
            BuildingBarrier = Codex.Barriers.wooden_wall,
            OutsideBarrier = Codex.Barriers.tree,
            OutsideGround = Codex.Grounds.grass,
            BuildingGround = Codex.Grounds.wooden_floor,
            AccentGround = Codex.Grounds.wooden_floor,
            BuildingGate = Codex.Gates.wooden_door,
            MayorEntity = Codex.Entities.Guru_Quilion
          },
          new TownVariant
          {
            Name = OpusTerms.Stonetown,
            BuildingBarrier = Codex.Barriers.stone_wall,
            OutsideBarrier = Codex.Barriers.tree,
            OutsideGround = Codex.Grounds.grass,
            BuildingGround = Codex.Grounds.stone_floor,
            AccentGround = Codex.Grounds.stone_floor,
            BuildingGate = Codex.Gates.iron_door,
            MayorEntity = Codex.Entities.Sir_Lorimar
          },
          new TownVariant
          {
            Name = OpusTerms.Cavetown,
            BuildingBarrier = Codex.Barriers.cave_wall,
            OutsideBarrier = Codex.Barriers.shroom,
            OutsideGround = Codex.Grounds.moss,
            BuildingGround = Codex.Grounds.cave_floor,
            AccentGround = Codex.Grounds.moss,
            BuildingGate = Codex.Gates.wooden_door,
            MayorEntity = Codex.Entities.Shaman_Karnov
          },
          new TownVariant
          {
            Name = OpusTerms.Jadetown,
            BuildingBarrier = Codex.Barriers.jade_wall,
            OutsideBarrier = Codex.Barriers.tree,
            OutsideGround = Codex.Grounds.snow,
            BuildingGround = Codex.Grounds.marble_floor,
            AccentGround = Codex.Grounds.gold_floor,
            BuildingGate = Codex.Gates.gold_door,
            MayorEntity = Codex.Entities.Lord_Carnarvon
          },
          new TownVariant
          {
            Name = OpusTerms.Helltown,
            BuildingBarrier = Codex.Barriers.hell_brick,
            OutsideBarrier = Codex.Barriers.shroom,
            OutsideGround = Codex.Grounds.moss,
            BuildingGround = Codex.Grounds.obsidian_floor,
            AccentGround = Codex.Grounds.granite_floor,
            BuildingGate = Codex.Gates.crystal_door,
            MayorEntity = Codex.Entities.Guildmaster_Vaughn
          }
        );

#if DEBUG
        foreach (var TownVariant in TownVariance.List)
          Debug.Assert(TownVariant.MayorEntity.IsGuardian, $"Town {TownVariant.Name} mayor {TownVariant.MayorEntity.Name} must be a guardian.");
#endif
      }

      public void Build(OpusSection Section)
      {
        BuildStart();

        var TownVariant = TownVariance.NextVariant();
        Section.OverlandAreaName = TownVariant.Name;

        var TownMap = Maker.OverlandMap;
        var TownRegion = Section.Region.Reduce(1);

        var MassacreChance = Chance.OneIn25;
        var OccupyChance = Chance.OneIn2; // 50% of towns are occupied.
#if DEBUG
        //MassacreChance = Chance.Always;
#endif
        var MassacreEvent = MassacreChance.Hit(); // kill everyone in town, overrun with hordes.
        var OccupyEvent = OccupyChance.Hit();

        var BuildingCount = RandomSupport.NextRange(8, 12);
        var BuildingList = new Inv.DistinctList<OpusBuilding>(BuildingCount);

        // number of watchmen to generate.
        var WatchmanNumberDice = 1.d3() + 1;
        var WatchmanCount = OccupyEvent ? 0 : WatchmanNumberDice.Roll();

        foreach (var Building in BuildingCount.NumberSeries())
        {
          var BuildingWidth = Math.Min(TownRegion.Width, RandomSupport.NextRange(5, 7));
          var BuildingHeight = Math.Min(TownRegion.Height, RandomSupport.NextRange(5, 7));
          var BuildingLeft = RandomSupport.NextRange(TownRegion.Left, TownRegion.Right - BuildingWidth + 1);
          var BuildingTop = RandomSupport.NextRange(TownRegion.Top, TownRegion.Bottom - BuildingHeight + 1);
          var BuildingRegion = new Region(BuildingLeft, BuildingTop, BuildingLeft + BuildingWidth - 1, BuildingTop + BuildingHeight - 1);

          if (BuildingList.Any(B => B.Region == BuildingRegion))
            continue;

          foreach (var Square in TownMap.GetFrameSquares(BuildingRegion))
          {
            if (Square.Floor == null || Square.Floor.Ground == Codex.Grounds.dirt)
            {
              Square.SetFloor(null);
              Generator.PlaceSolidWall(Square, TownVariant.BuildingBarrier, WallSegment.Pillar);
            }
          }

          foreach (var Square in TownMap.GetSquares(BuildingRegion.Reduce(1)))
          {
            Square.SetWall(null);
            Generator.PlaceFloor(Square, TownVariant.BuildingGround);
          }

          var BuildingZone = TownMap.AddZone();
          BuildingZone.AddRegion(BuildingRegion);

          if (BuildingZone.Squares.Count == 0)
          {
            // zone contains no new squares, so the building region can be forgotten.
            TownMap.RemoveZone(BuildingZone);
          }
          else
          {
            BuildingZone.SetLit(true);
            BuildingZone.SetSpawnRestricted(true);
            BuildingList.Add(new OpusBuilding(BuildingRegion));
          }
        }

        if (BuildingList.Count == 0)
        {
          Debug.Fail("Unable to place a single building?");
          return;
        }

        foreach (var Building in BuildingList)
        {
          var BuildingSquare = TownMap.GetFrameSquares(Building.Region).Where(S => S.Wall != null && S.IsFlat()).GetRandomOrNull();

          if (BuildingSquare == null)
          {
            // TODO: a building in void space, completely unconnected is possible - should we instead deviate a dirt path to another building?
            if (TownMap.GetFrameSquares(Building.Region).All(S => S.Wall != null && S.Door == null))
            {
              Debug.WriteLine($"{TownVariant.Name} teardown building {Building.Region}.");

              BuildingList.Remove(Building);

              foreach (var TeardownSquare in TownMap.GetSquares(Building.Region))
              {
                if (TeardownSquare.Door?.Gate == TownVariant.BuildingGate)
                  TeardownSquare.SetDoor(null);

                if (TeardownSquare.Floor?.Ground == TownVariant.BuildingGround)
                  TeardownSquare.SetFloor(null);

                if (TeardownSquare.Wall?.Barrier == TownVariant.BuildingBarrier)
                  TeardownSquare.SetWall(null);

                var TeardownZone = TeardownSquare.Zone;
                if (TeardownZone != null && TeardownSquare.Floor == null && TeardownSquare.Wall == null)
                {
                  TeardownSquare.SetLit(false);
                  TeardownZone.RemoveSquare(TeardownSquare);

                  Debug.Assert(TeardownSquare.IsVoid());

                  if (TeardownZone.Squares.Count == 0)
                    TownMap.RemoveZone(TeardownZone);
                }
              }
            }
          }
          else
          {
            // punch a door in a flat segment of wall.
            BuildingSquare.SetWall(null);
            Generator.PlaceFloor(BuildingSquare, TownVariant.BuildingGround);
            Generator.PlaceClosedVerticalDoor(BuildingSquare, TownVariant.BuildingGate, SecretBarrier: TownVariant.BuildingBarrier);
          }
        }

        // repair weird gaps.
        Maker.RepairGaps(TownMap, TownRegion, TownVariant.BuildingBarrier, IsLit: true);

        // ensure we have carved a dirt veranda around every building, to guarantee access.
        foreach (var Building in BuildingList)
          Maker.RepairVeranda(TownMap, Building.Region.Expand(1), Codex.Grounds.dirt, IsLit: true);

        // determine any regular buildings (rectangular stone walls with at least one door).
        var RegularBuildingList = new Inv.DistinctList<OpusBuilding>();

        bool IsIrregular(OpusBuilding Building)
        {
          var Result = false;
          var FoundOneDoor = false;

          foreach (var BuildingSquare in TownMap.GetFrameSquares(Building.Region))
          {
            if (BuildingSquare.Door != null)
            {
              if (FoundOneDoor)
              {
                Result = true;
                break;
              }

              FoundOneDoor = true;
            }
            else if (BuildingSquare.Wall == null)
            {
              Result = true;
              break;
            }
          }

          return Result;
        }
        bool IsSemiregular(OpusBuilding Building) => TownMap.GetFrameSquares(Building.Region).All(R => R.Wall != null || R.Door != null);

        // regular buildings are surrounded by walls and only have one door.
        RegularBuildingList.AddRange(BuildingList.Where(B => !IsIrregular(B)));

        if (RegularBuildingList.Count == 0)
        {
          // semi-regular buildings are surrounded by walls and may have more than one door.
          RegularBuildingList.AddRange(BuildingList.Where(B => IsSemiregular(B)));

          // no regular buildings so we pick one at random.
          if (RegularBuildingList.Count == 0)
            RegularBuildingList.Add(BuildingList.GetRandom());
        }

        var OtherBuildingList = BuildingList.Except(RegularBuildingList).ToDistinctList();

        // station portal.
        var PortalSquare = TownMap.GetSquares(TownRegion).Where(S =>
          S.Floor?.Ground == Codex.Grounds.dirt &&
          Generator.CanPlacePortal(S) &&
          S.GetNeighbourSquares().Count(A => A.Wall?.Barrier == TownVariant.BuildingBarrier && A.Wall != null && A.IsFlat()) == 1 &&
          S.GetNeighbourSquares().Count(A => A.Floor?.Ground == Codex.Grounds.dirt) == 3).GetRandomOrNull();

        if (PortalSquare == null)
        {
          PortalSquare = TownMap.GetSquares(TownRegion).Where(S =>
            S.Floor?.Ground == Codex.Grounds.dirt &&
            Generator.CanPlacePortal(S) &&
            S.GetNeighbourSquares().Any(A => A.Wall?.Barrier == TownVariant.BuildingBarrier)).GetRandomOrNull();

          if (PortalSquare == null)
            Debug.Fail("Town portal square was not generated.");

          if (PortalSquare != null)
            Maker.Station.ConnectTownPortal(PortalSquare, TownVariant.BuildingBarrier, TownVariant.AccentGround);
        }
        else
        {
          Maker.Station.ConnectTownPortal(PortalSquare, TownVariant.BuildingBarrier, TownVariant.AccentGround);

          var WallSquare = PortalSquare.GetNeighbourSquares().Where(A => A.Wall?.Barrier == TownVariant.BuildingBarrier && A.Wall != null && A.IsFlat()).GetRandomOrNull();
          var OpenSquare = PortalSquare.Adjacent(WallSquare.AsDirection(PortalSquare));

          foreach (var AroundSquare in PortalSquare.GetNeighbourSquares())
          {
            if (AroundSquare != OpenSquare && AroundSquare.Wall == null && !AroundSquare.GetNeighbourSquares().Any(S => S.Door != null))
            {
              AroundSquare.SetFloor(null);
              Generator.PlaceSolidWall(AroundSquare, TownVariant.BuildingBarrier, WallSegment.Pillar);
            }
          }
        }

        // everyone in the town is allied together.
        var TownParty = Generator.NewParty(Leader: null);

        // unique mayor of the town, allied with the vendors.
        Character MayorCharacter = null;
        var MayorSquareList = new Inv.DistinctList<Square>();
        if (!MassacreEvent && !OccupyEvent)
        {
          // NPC and artifact.
          MayorCharacter = Maker.NewGoodCharacter(PortalSquare, Maker.SelectUniqueEntity(TownVariant.MayorEntity));
          Generator.PlaceCharacter(PortalSquare, MayorCharacter);
          Generator.AcquireUnique(PortalSquare, MayorCharacter, Codex.Qualifications.champion);
          MayorSquareList.Add(PortalSquare);

          TownParty.AddAlly(MayorCharacter, Clock.Zero, Delay.Zero); // leader is not the party leader, or all merchants will act as 'guardians'.
        }

        // place one shrine.
        var ShrineBuilding = RegularBuildingList.GetRandom();
        RegularBuildingList.Remove(ShrineBuilding);
        var ShrineSquare = TownMap[ShrineBuilding.Region.Left + (ShrineBuilding.Region.Width / 2), ShrineBuilding.Region.Top + (ShrineBuilding.Region.Height / 2)];
        if (!Generator.CanPlaceCharacter(ShrineSquare))
          ShrineSquare = TownMap.GetSquares(ShrineBuilding.Region.Reduce(1)).Where(S => Generator.CanPlaceCharacter(S)).GetRandomOrNull();

        if (ShrineSquare == null)
        {
          //Debug.Fail("Unable to place a shrine.");
        }
        else
        {
          MayorSquareList.Add(ShrineSquare);

          // TODO: force a better zone boundary, if necessary?

          var Shrine = Maker.NextRandomShrine();
          Generator.PlaceShrine(ShrineSquare, Shrine);
          ShrineSquare.Zone.RequireTrigger().Add(Delay.Zero, Codex.Tricks.VisitShrineArray[Shrine.Index]).SetTarget(ShrineSquare);
          ShrineSquare.Zone.SetLit(!IsSemiregular(ShrineBuilding)); // shrines are dark in regular buildings.

          var ShrineCharacter = ShrineSquare.Character;
          if (ShrineCharacter != null)
            TownParty.AddAlly(ShrineCharacter, Clock.Zero, Delay.Zero);

          if (MassacreEvent)
            Generator.CorpseSquare(ShrineSquare);
        }

        // place shops.
        var ShopNumberDice = 1.d3();
        var ShopSet = new HashSet<Shop>();
        foreach (var ShopIndex in ShopNumberDice.Roll().NumberSeries())
        {
          OpusBuilding ShopBuilding;
          if (RegularBuildingList.Count > 0)
          {
            ShopBuilding = RegularBuildingList.GetRandom();
            RegularBuildingList.Remove(ShopBuilding);
          }
          else if (OtherBuildingList.Count > 0)
          {
            ShopBuilding = OtherBuildingList.GetRandom();
            OtherBuildingList.Remove(ShopBuilding);
          }
          else
          {
            break;
          }

          var ShopSquare = TownMap[ShopBuilding.Region.Left + (ShopBuilding.Region.Width / 2), ShopBuilding.Region.Top + (ShopBuilding.Region.Height / 2)];
          if (!Generator.CanPlaceCharacter(ShopSquare) || ShopSquare.GetAdjacentSquares().Any(A => A.Character != null))
            ShopSquare = TownMap.GetSquares(ShopBuilding.Region.Reduce(1)).Where(S => S.Fixture == null && Generator.CanPlaceCharacter(S) && !S.GetAdjacentSquares().Any(A => A.Character != null)).GetRandomOrNull();

          if (ShopSquare == null)
          {
            //Debug.Fail("Unable to place a shop.");
          }
          else
          {
            MayorSquareList.Add(ShopSquare);

            // TODO: force a better zone boundary, if necessary?

            var Shop = Maker.NextRandomShop();

            if (!ShopSet.Add(Shop))
            {
              // already generated this shop for this town, put it back in the shop probability for the next town.
              Maker.ShopProbability.Add(Shop.Rarity, Shop);
            }
            else
            {
              var ItemsDice = MassacreEvent ? 1.d4() + 1 : 6.d2(); // 2-5 or 6-12

              Generator.PlaceShop(ShopSquare, Shop, Items: ItemsDice.Roll());
              ShopSquare.Zone.RequireTrigger().Add(Delay.Zero, Codex.Tricks.VisitShopArray[Shop.Index]).SetTarget(ShopSquare);

              var ShopCharacter = ShopSquare.Character;
              if (ShopCharacter != null)
              {
                TownParty.AddAlly(ShopCharacter, Clock.Zero, Delay.Zero);

                if (MassacreEvent)
                  Generator.CorpseSquare(ShopSquare);

                // only spawn mimics for towns without watchmen.
                if (OccupyEvent)
                {
                  var MimicNumberDice = 1.d3();
                  Maker.PlaceMimics(Section, TownMap.GetSquares(ShopBuilding.Region).ToArray(), MimicNumberDice.Roll());
                }
              }
            }
          }
        }

        MayorCharacter?.SetResidentRoute(MayorSquareList.ToArray(), 0);

        // make fountains.
        var FountainNumberDice = 1.d2();
        foreach (var Fountain in FountainNumberDice.Roll().NumberSeries())
        {
          var FountainSquare = TownMap.GetSquares(TownRegion).Where(S => S.Floor?.Ground == Codex.Grounds.dirt && Generator.CanPlaceFeature(S) && S.GetAdjacentSquares().Count(A => A.Floor != null && A.Door == null && A.Fixture == null && A.Passage == null) == 8).GetRandomOrNull();
          if (FountainSquare != null)
            Generator.PlaceFixture(FountainSquare, Codex.Features.fountain);
        }

        // make barrels.
        var BarrielNumberDice = 1.d2() + 1;
        foreach (var Barrel in BarrielNumberDice.Roll().NumberSeries())
        {
          var BarrelSquare = TownMap.GetSquares(TownRegion).Where(S => S.Floor?.Ground == TownVariant.BuildingGround && Generator.CanPlaceBoulder(S) && Maker.IsCorner(S, S => S.Wall != null, S => S.Floor != null && S.Boulder == null && S.Fixture == null && S.Door == null && S.Passage == null)).GetRandomOrNull();
          if (BarrelSquare != null)
            Generator.PlaceBoulder(BarrelSquare, Codex.Blocks.wooden_barrel, IsRigid: false);
        }

        var WatchBoost = Section.Distance - Codex.Entities.watchman.Level;

        void PlaceWatch(Square Square, Entity Entity)
        {
          var WatchCharacter = Generator.NewCharacter(Square, Entity);

          if (WatchBoost > 2)
            Generator.PromoteCharacter(WatchCharacter, WatchBoost / 2);

          Generator.PlaceCharacter(Square, WatchCharacter);
        }

        // make beds.
        var BedNumberDice = 1.d2() + 1;
        foreach (var Bed in BedNumberDice.Roll().NumberSeries())
        {
          if (OtherBuildingList.Count > 0)
          {
            var OtherBuilding = OtherBuildingList.GetRandom();
            OtherBuildingList.Remove(OtherBuilding);

            var BedSquare = TownMap[OtherBuilding.Region.Left + (OtherBuilding.Region.Width / 2), OtherBuilding.Region.Top + (OtherBuilding.Region.Height / 2)];
            if (Generator.CanPlaceFeature(BedSquare))
            {
              Generator.PlaceFixture(BedSquare, Codex.Features.bed);

              // never have all watchmen asleep.
              if (WatchmanCount >= 2 && Chance.OneIn2.Hit() && Generator.CanPlaceCharacter(BedSquare))
              {
                WatchmanCount--;

                PlaceWatch(BedSquare, Codex.Entities.watchman);

                var OffdutyCharacter = BedSquare.Character;
                if (OffdutyCharacter != null)
                {
                  OffdutyCharacter.SetResidentSquare(BedSquare);

                  TownParty.AddAlly(OffdutyCharacter, Clock.Zero, Delay.Zero);

                  if (MassacreEvent)
                    Generator.CorpseSquare(BedSquare);
                  else
                    Maker.SnoozeCharacter(OffdutyCharacter);
                }
              }
            }
          }
        }

        // place a standalone feature in one of the other buildings.
        if (OtherBuildingList.Count > 0)
        {
          var FeatureArray = new[] { Codex.Features.altar, Codex.Features.pentagram, Codex.Features.workbench };
          do
          {
            var FeatureBuilding = OtherBuildingList.GetRandom();
            OtherBuildingList.Remove(FeatureBuilding);

            var FeatureSquare = TownMap[FeatureBuilding.Region.Left + (FeatureBuilding.Region.Width / 2), FeatureBuilding.Region.Top + (FeatureBuilding.Region.Height / 2)];
            if (Generator.CanPlaceFeature(FeatureSquare))
            {
              Generator.PlaceFixture(FeatureSquare, FeatureArray.GetRandom());

              MayorSquareList.Add(FeatureSquare);
              break;
            }
          }
          while (OtherBuildingList.Count > 0);
        }

        if (OccupyEvent)
        {
          // instead of watchmen, the town is occupied with hostiles (which will not attack the shop/shrine keepers).
          var Occupy = Maker.NextRandomOccupy(Section.MaximumDifficulty);
          if (Occupy != null)
          {
            var OccupyProbability = Occupy.GetProbability(Section.MinimumDifficulty, Section.MaximumDifficulty);

            var OccupyParty = Generator.NewParty(Leader: null);

            foreach (var Building in BuildingList)
            {
              var OccupySquare = TownMap.GetSquares(Building.Region).Where(S => Generator.CanPlaceCharacter(S)).GetRandomOrNull();
              if (OccupySquare != null)
              {
                Generator.PlaceCharacter(OccupySquare, OccupyProbability.GetRandomOrNull());
                var OccupyCharacter = OccupySquare.Character;
                if (OccupyCharacter != null)
                  OccupyParty.AddAlly(OccupyCharacter, Clock.Zero, Delay.Zero);
              }
            }
          }
        }
        else
        {
          // collect all the dirt squares just outside a door.
          var WatchmanSquareList = new Inv.DistinctList<Square>(BuildingList.Count);
          foreach (var WatchSquare in TownMap.GetSquares(TownRegion))
          {
            if (WatchSquare.Floor?.Ground == Codex.Grounds.dirt && WatchSquare.Character == null && WatchSquare.GetNeighbourSquares().Any(S => S.Door != null))
              WatchmanSquareList.Add(WatchSquare);
          }

          // watchmen.
          var WatchmanSquareArray = WatchmanSquareList.ToArray();
          foreach (var Watchman in WatchmanCount.NumberSeries())
          {
            if (WatchmanSquareList.Count > 0)
            {
              var WatchmanSquareIndex = RandomSupport.NextNumber(WatchmanSquareList.Count);
              var WatchmanSquare = WatchmanSquareList[WatchmanSquareIndex];
              WatchmanSquareList.RemoveAt(WatchmanSquareIndex);

              PlaceWatch(WatchmanSquare, Codex.Entities.watch_captain);

              var WatchmanCharacter = WatchmanSquare.Character;
              if (WatchmanCharacter != null)
              {
                WatchmanCharacter.SetResidentRoute(WatchmanSquareArray, WatchmanSquareArray.IndexOf(WatchmanSquare));
                TownParty.AddAlly(WatchmanCharacter, Clock.Zero, Delay.Zero);
              }

              if (MassacreEvent)
                Generator.CorpseSquare(WatchmanSquare);
            }
          }

          // watch captain.
          var WatchCaptainSquareList = new Inv.DistinctList<Square>(BuildingList.Count);
          foreach (var Building in BuildingList)
          {
            var BuildingSquare = TownMap[Building.Region.Left + (Building.Region.Width / 2), Building.Region.Top + (Building.Region.Height / 2)];

            // TODO: remove this when the patrol AI no longer switches/stops when a resident is in their way.
            if (!Generator.CanPlaceCharacter(BuildingSquare))
              BuildingSquare = BuildingSquare.GetAdjacentSquares().Where(S => Generator.CanPlaceCharacter(S)).GetRandomOrNull();

            if (BuildingSquare != null && !WatchCaptainSquareList.Contains(BuildingSquare))
              WatchCaptainSquareList.Add(BuildingSquare);
          }

          var WatchCaptainSquare = WatchCaptainSquareList.Where(S => Generator.CanPlaceCharacter(S)).GetRandomOrNull() ?? TownMap.GetSquares(TownRegion).Where(S => Generator.CanPlaceCharacter(S)).GetRandomOrNull();
          if (WatchCaptainSquare != null)
          {
            PlaceWatch(WatchCaptainSquare, Codex.Entities.watch_captain);

            var WatchCaptainCharacter = WatchCaptainSquare.Character;
            if (WatchCaptainCharacter != null)
            {
              if (WatchCaptainSquareList.Count > 0)
                WatchCaptainCharacter.SetResidentRoute(WatchCaptainSquareList.ToArray(), WatchCaptainSquareList.IndexOf(WatchCaptainSquare));
              else
                WatchCaptainCharacter.SetResidentSquare(WatchCaptainSquare);

              TownParty.AddAlly(WatchCaptainCharacter, Clock.Zero, Delay.Zero);

              if (MassacreEvent)
                Generator.CorpseSquare(WatchCaptainSquare);
            }
          }

          // mercenaries are only around if the town has not been occupied by hostiles.
          var MercenaryNumberDice = 1.d3();
          var MercenaryCount = MercenaryNumberDice.Roll();

          foreach (var MercenaryIndex in MercenaryCount.NumberSeries())
          {
            var MercenarySquare = TownMap.GetSquares(TownRegion).Where(S => S.Floor?.Ground == TownVariant.BuildingGround && Generator.CanPlaceCharacter(S)).GetRandomOrNull();

            if (MercenarySquare == null)
            {
              Generator.PlaceCharacter(MercenarySquare, Maker.RandomMercenaryEntity(Section));

              if (MassacreEvent)
                Generator.CorpseSquare(MercenarySquare);
            }
          }
        }

        // render the inner floors.
        foreach (var FillSquare in TownMap.GetSquares(TownRegion))
        {
          if (FillSquare.Floor?.Ground == TownVariant.BuildingGround && FillSquare.GetAdjacentSquares().Count(S => S.Floor?.Ground == TownVariant.AccentGround || S.Floor?.Ground == TownVariant.BuildingGround) == 8)
          {
            Generator.PlaceFloor(FillSquare, TownVariant.AccentGround);
          }
          else if (FillSquare.Floor?.Ground == Codex.Grounds.dirt && FillSquare.GetAdjacentSquares().Count(S => S.Floor?.Ground == TownVariant.OutsideGround || S.Floor?.Ground == Codex.Grounds.dirt) == 8)
          {
            if (TownVariant.OutsideGround != null)
              Generator.PlaceFloor(FillSquare, TownVariant.OutsideGround);

            if (FillSquare.Fixture == null && !FillSquare.GetAdjacentSquares().Any(S => S.Wall != null || S.Passage != null) && Chance.OneIn4.Hit())
            {
              if (TownVariant.OutsideGround == null)
                FillSquare.SetFloor(null);

              Generator.PlaceSolidWall(FillSquare, TownVariant.OutsideBarrier, WallSegment.Pillar);
            }
          }
        }

        if (MassacreEvent)
        {
          foreach (var Building in BuildingList)
          {
            if (Chance.OneIn2.Hit())
              Generator.PlaceHorde(Generator.RandomHorde(Section.MinimumDifficulty, Section.MaximumDifficulty), Section.MinimumDifficulty, Section.MaximumDifficulty, () => TownMap.GetSquares(Building.Region.Reduce(1)).Where(S => Generator.CanPlaceCharacter(S)).GetRandomOrNull());
          }
        }

        Maker.RepairVeranda(TownMap, TownRegion, Codex.Grounds.dirt, IsLit: true);

        // TODO:
        // * town name signpost.
        // * sewer side dungeon - maze matching the outline of the town buildings?
        // * guarded treasure room/vault like in Dhak.

        BuildStop();
      }

      private readonly Variance<TownVariant> TownVariance;

      private sealed class TownVariant
      {
        public string Name;
        public Barrier BuildingBarrier;
        public Barrier OutsideBarrier;
        public Ground OutsideGround;
        public Ground BuildingGround;
        public Ground AccentGround;
        public Gate BuildingGate;
        public Entity MayorEntity;
      }
    }

    private sealed class VendorBuilder : Builder
    {
      public VendorBuilder(OpusMaker Maker)
        : base(Maker)
      {
      }

      public void Build(OpusClearing VendorClearing)
      {
        BuildStart();

        var VendorMap = Maker.OverlandMap;
        var VendorIsShrine = Chance.OneIn3.Hit();
        var VendorX = VendorClearing.Circle.X;
        var VendorY = VendorClearing.Circle.Y;
        var VendorRadius = Math.Min(3, VendorClearing.Circle.Radius - 2);
        var VendorSquare = VendorMap[VendorX, VendorY];
        var VendorCircle = new Inv.Circle(VendorX, VendorY, VendorRadius);
        var VendorBarrier = VendorIsShrine ? Codex.Barriers.tree : Codex.Barriers.wooden_wall;
        var VendorGround = VendorIsShrine ? Codex.Grounds.grass : Codex.Grounds.wooden_floor;

        var VendorZone = VendorMap.AddZone();

        foreach (var Square in VendorMap.GetCircleOuterSquares(VendorCircle))
        {
          VendorZone.ForceSquare(Square);

          if ((Square.X == VendorSquare.X || Square.Y == VendorSquare.Y) && !(Square.Adjacent(VendorSquare.AsDirection(Square))?.IsVoid() ?? false))
            Generator.PlaceFloor(Square, VendorGround);
          else
            Generator.PlaceSolidWall(Square, VendorBarrier, WallSegment.Pillar);
        }

        foreach (var Square in VendorMap.GetCircleInnerSquares(VendorCircle))
        {
          VendorZone.ForceSquare(Square);

          Generator.PlaceFloor(Square, VendorGround);
        }

        VendorZone.SetLit(true);

        if (VendorZone.Squares.Count == 0)
        {
          Debug.Fail("Unable to place a vendor zone.");
          VendorMap.RemoveZone(VendorZone);
        }
        else
        {
          var VendorTrigger = VendorZone.InsertTrigger();

          if (VendorIsShrine)
          {
            var VendorShrine = Maker.NextRandomShrine();

            Generator.PlaceShrine(VendorSquare, VendorShrine);

            VendorTrigger.Add(Delay.Zero, Codex.Tricks.VisitShrineArray[VendorShrine.Index]).SetTarget(VendorSquare);
          }
          else
          {
            var VendorShop = Maker.NextRandomShop();
            var VendorItemDice = 1.d6() + 4;
            Generator.PlaceShop(VendorSquare, VendorShop, Items: VendorItemDice.Roll());

            VendorTrigger.Add(Delay.Zero, Codex.Tricks.VisitShopArray[VendorShop.Index]).SetTarget(VendorSquare);

            var MimicNumberDice = 1.d3();
            Maker.PlaceMimics(VendorClearing.Section, VendorZone.Squares, MimicNumberDice.Roll());
          }

          if (VendorClearing.Circle.Radius < 4)
            Maker.RepairVeranda(VendorMap, new Region(VendorClearing.Circle), Codex.Grounds.dirt, IsLit: true);
        }

        BuildStop();

        // TODO:
        // * guards?
      }
    }

    private sealed class ZooBuilder : Builder
    {
      public ZooBuilder(OpusMaker Maker)
        : base(Maker)
      {
      }

      public void Build(OpusClearing ZooClearing)
      {
        var Zoo = Generator.GetZooProbability(ZooClearing.Section.MaximumDifficulty).GetRandomOrNull();

        if (Zoo != null)
        {
          BuildStart();

          var ZooMap = Maker.OverlandMap;
          var ZooX = ZooClearing.Circle.X;
          var ZooY = ZooClearing.Circle.Y;
          var ZooRadius = Math.Min(3, ZooClearing.Circle.Radius - 1);
          var ZooRegion = new Region(ZooX - ZooRadius - 1, ZooY - ZooRadius - 1, ZooX + ZooRadius + 1, ZooY + ZooRadius + 1);
          var ZooCircle = new Inv.Circle(ZooX, ZooY, ZooRadius);

          var ZooZone = ZooMap.AddZone();

          foreach (var Square in ZooMap.GetCircleInnerSquares(ZooCircle))
          {
            if (!Square.IsVoid())
              ZooZone.ForceSquare(Square);
          }

          ZooZone.InsertTrigger().Add(Delay.Zero, Codex.Tricks.VisitZooArray[Zoo.Index]);

          Generator.PlaceZoo(ZooZone.Squares, Zoo, ZooClearing.Section.MinimumDifficulty, ZooClearing.Section.MaximumDifficulty);

          // circular stone wall with one locked door and the rest are iron bars.
          var MiddleSquare = ZooMap[ZooX, ZooY];
          var NeighbourSquareArray = MiddleSquare.GetNeighbourSquares(ZooRadius + 1).ToArray();
          var EntranceSquare = NeighbourSquareArray.GetRandomOrNull();

          var ZooBarrier = Codex.Barriers.stone_wall;

          foreach (var ZooSquare in ZooMap.GetCircleOuterSquares(ZooCircle))
          {
            if (ZooSquare == EntranceSquare)
            {
              Generator.PlaceLockedVerticalDoor(ZooSquare, Codex.Gates.iron_door, ZooBarrier);
            }
            else if (NeighbourSquareArray.Contains(ZooSquare))
            {
              Generator.PlaceWall(ZooSquare, Codex.Barriers.iron_bars, WallStructure.Solid, WallSegment.Pillar);
            }
            else if (ZooSquare.Boulder == null)
            {
              if (ZooBarrier.Opaque)
                ZooSquare.SetFloor(null);

              Generator.PlaceWall(ZooSquare, ZooBarrier, WallStructure.Solid, WallSegment.Pillar);
            }

            ZooZone.ForceSquare(ZooSquare);
          }

          ZooZone.SetLit(true);
          Debug.Assert(ZooZone.HasSquares());

          // ensure dirt veranda.
          Maker.RepairVeranda(ZooMap, ZooRegion.Expand(1), Codex.Grounds.dirt, IsLit: true);

          BuildStop();
        }

        // TODO:
        // * what should happen when the player starts shooting spells/wands through the bars? remove iron bars on anyone's death trigger? iron bars have a chance to break when hit with fire/cold/shock/force/etc?
      }
    }

    private sealed class OpusCorner
    {
      public Direction[] Closed;
      public Direction[] Open;
    }

    private sealed class OpusOccupy
    {
      public OpusOccupy(Kind Kind)
      {
        this.Kind = Kind;
        this.EncounterEntities = Kind.Entities.Where(E => E.IsEncounter && E.Frequency > 0).OrderBy(E => E.Name).ToArray();
        this.MinimumDifficulty = EncounterEntities.Min(E => E.Difficulty);
        this.MaximumDifficulty = EncounterEntities.Max(E => E.Difficulty);
      }

      public Kind Kind { get; }
      public IReadOnlyList<Entity> EncounterEntities { get; }
      public int MinimumDifficulty { get; }
      public int MaximumDifficulty { get; }

      public Probability<Entity> GetProbability(int MinimumDifficulty, int MaximumDifficulty)
      {
        var Result = EncounterEntities.Where(E => E.Difficulty >= MinimumDifficulty && E.Difficulty <= MaximumDifficulty).ToProbability(E => E.Frequency);

        Debug.Assert(Result.HasChecks(), "Occupy probability should only be used when there is at least one viable entity.");

        return Result;
      }

      public override string ToString() => $"{Kind.Name} ({MinimumDifficulty}..{MaximumDifficulty}) = {EncounterEntities.Select(E => E.Name).AsSeparatedText(", ")}";
    }

    private sealed class OpusPlanner
    {
      public OpusPlanner()
      {
      }

      public int StartMinSize { get; set; }
      public int StartMaxSize { get; set; }
      public int SectorMinSize { get; set; }
      public int SectorMaxSize { get; set; }
      public int CriticalDistance { get; set; }

      public OpusPlan Generate(int Width, int Height)
      {
        var SpotGrid = new Inv.Grid<OpusSpot>(Width, Height);

        IEnumerable<OpusSpot> GetSpots(Region Region)
        {
          for (var X = Region.Left; X <= Region.Right; X++)
          {
            for (var Y = Region.Top; Y <= Region.Bottom; Y++)
              yield return SpotGrid[X, Y];
          }
        }
        bool IsValidRegion(Region Region) => SpotGrid.IsValid(Region.Left, Region.Top) && SpotGrid.IsValid(Region.Right, Region.Bottom);
        bool IsVoidRegion(Region Region) => GetSpots(Region).All(S => S == OpusSpot.Void);
        void FillRegion(Region Region, OpusSpot Spot)
        {
          if (!IsValidRegion(Region))
            throw new Exception("Fill out of range of the grid.");

          for (var X = Region.Left; X <= Region.Right; X++)
          {
            for (var Y = Region.Top; Y <= Region.Bottom; Y++)
              SpotGrid[X, Y] = Spot;
          }
        }

        var Result = new OpusPlan(Width, Height);

        var ShuffleArray = new int[] { 0, 1, 2, 3 }.GeneratePermutations().ToArray();

        const int GenerationLimit = 1000;

        var MaximumDistance = 0;
        var GenerationIndex = 0;
        do
        {
          void GenerateSector(OpusSector FromSector, Region SectorRegion, Region LinkRegion)
          {
            if (IsValidRegion(SectorRegion) && IsVoidRegion(SectorRegion.Reduce(1)))
            {
              var Distance = FromSector == null ? 0 : FromSector.Distance + 1;

              var ThisSector = Result.AddSector(SectorRegion, Distance);

              if (Distance > MaximumDistance)
                MaximumDistance = Distance;

              FillRegion(SectorRegion, OpusSpot.Border);
              FillRegion(SectorRegion.Reduce(1), OpusSpot.Space);

              if (FromSector != null)
              {
                FromSector.AddLink(LinkRegion, ThisSector);
                ThisSector.AddLink(LinkRegion, FromSector);
                FillRegion(LinkRegion, OpusSpot.Waypoint);
              }

              if (Distance < CriticalDistance)
              {
                foreach (var Shuffle in ShuffleArray.GetRandom())
                {
                  var NextWidth = RandomSupport.NextNumber(SectorMinSize, SectorMaxSize);
                  var NextHeight = RandomSupport.NextNumber(SectorMinSize, SectorMaxSize);

                  Region NextSectorRegion;
                  Region NextLinkRegion;

                  switch (Shuffle)
                  {
                    // go left.
                    case 0:
                      {
                        var NextX = SectorRegion.Left - NextWidth + 1;
                        var NextY = RandomSupport.NextNumber(SectorRegion.Top - NextHeight + 3, SectorRegion.Bottom - 1);
                        var LinkX = SectorRegion.Left;
                        var LinkY = Math.Max(SectorRegion.Top, NextY) + 1;

                        NextSectorRegion = new Region(NextX, NextY, NextX + NextWidth - 1, NextY + NextHeight - 1);
                        NextLinkRegion = new Region(LinkX, LinkY, LinkX, LinkY + Math.Min(NextY + NextHeight - 1, SectorRegion.Bottom) - LinkY - 1);
                      }
                      break;

                    case 1:
                      // go right.
                      {
                        var NextX = SectorRegion.Right;
                        var NextY = RandomSupport.NextNumber(SectorRegion.Top - NextHeight + 3, SectorRegion.Bottom - 1);
                        var LinkX = SectorRegion.Right;
                        var LinkY = Math.Max(SectorRegion.Top, NextY) + 1;

                        NextSectorRegion = new Region(NextX, NextY, NextX + NextWidth - 1, NextY + NextHeight - 1);
                        NextLinkRegion = new Region(LinkX, LinkY, LinkX, LinkY + Math.Min(NextY + NextHeight - 1, SectorRegion.Bottom) - LinkY - 1);
                      }
                      break;

                    // go up.
                    case 2:
                      {
                        var NextX = RandomSupport.NextNumber(SectorRegion.Left - NextWidth + 3, SectorRegion.Right - 2);
                        var NextY = SectorRegion.Top - NextHeight + 1;
                        var LinkX = Math.Max(SectorRegion.Left, NextX) + 1;
                        var LinkY = SectorRegion.Top;

                        NextSectorRegion = new Region(NextX, NextY, NextX + NextWidth - 1, NextY + NextHeight - 1);
                        NextLinkRegion = new Region(LinkX, LinkY, LinkX + Math.Min(SectorRegion.Right, NextX + NextWidth - 1) - LinkX - 1, LinkY);
                      }
                      break;

                    // go down.
                    case 3:
                      {
                        var NextX = RandomSupport.NextNumber(SectorRegion.Left - NextWidth + 3, SectorRegion.Right - 2);
                        var NextY = SectorRegion.Bottom;
                        var LinkX = Math.Max(SectorRegion.Left, NextX) + 1;
                        var LinkY = SectorRegion.Bottom;

                        NextSectorRegion = new Region(NextX, NextY, NextX + NextWidth - 1, NextY + NextHeight - 1);
                        NextLinkRegion = new Region(LinkX, LinkY, LinkX + Math.Min(SectorRegion.Right, NextX + NextWidth - 1) - LinkX - 1, LinkY);
                      }
                      break;

                    default:
                      throw new Exception("Shuffle number not handled: " + Shuffle);
                  }

                  GenerateSector(ThisSector, NextSectorRegion, NextLinkRegion);
                }
              }
            }
          }

          SpotGrid.Fill(OpusSpot.Void);
          Result.Reset();
          MaximumDistance = 0;

          // start Sector is smaller by design.
          var StartWidth = RandomSupport.NextNumber(StartMinSize, StartMaxSize);
          var StartHeight = RandomSupport.NextNumber(StartMinSize, StartMaxSize);
          var StartX = RandomSupport.NextNumber(1, Width - StartWidth - 1);
          var StartY = RandomSupport.NextNumber(1, Height - StartHeight - 1);
          GenerateSector(FromSector: null, new Region(StartX, StartY, StartX + StartWidth - 1, StartY + StartHeight - 1), Region.Zero);

          GenerationIndex++;
        }
        while (GenerationIndex < GenerationLimit && MaximumDistance < CriticalDistance);

        Debug.Assert(GenerationIndex < GenerationLimit, "Exceeded generation limit.");

        return Result;
      }

      private enum OpusSpot
      {
        Void,
        Border,
        Space,
        Waypoint
      }
    }

    private sealed class OpusPlan
    {
      internal OpusPlan(int Width, int Height)
      {
        this.Width = Width;
        this.Height = Height;
        this.SectorList = new Inv.DistinctList<OpusSector>();
      }

      public int Width { get; private set; }
      public int Height { get; private set; }
      public IReadOnlyList<OpusSector> Sectors => SectorList;

      public void Reset()
      {
        SectorList.Clear();
      }
      public OpusSector AddSector(Region Region, int Distance)
      {
        var Result = new OpusSector(Region, Distance);

        SectorList.Add(Result);

        return Result;
      }
      public void PlotCriticalPaths()
      {
        // sort sectors by distance.
        SectorList.Sort((A, B) => A.Distance.CompareTo(B.Distance));

        // markup critical path from initial to final.
        var SearchSet = new HashSet<OpusSector>();
        bool CriticalSearch(OpusSector SearchSector, OpusSector LocateSector)
        {
          if (!SearchSet.Add(SearchSector))
            return false;

          if (SearchSector == LocateSector)
            return true;

          foreach (var Link in SearchSector.Links)
          {
            if (CriticalSearch(Link.Sector, LocateSector))
            {
              Link.Sector.Critical = true;
              return true;
            }
          }

          return false;
        }

        var InitialSector = SectorList.First();
        InitialSector.Critical = true;

        var FinalSector = SectorList.Last();
        FinalSector.Critical = true;

        CriticalSearch(InitialSector, FinalSector);
      }
      public void PruneSidePaths()
      {
        foreach (var SideSector in SectorList)
        {
          if (SideSector.Critical)
          {
            foreach (var SideLink in SideSector.Links)
            {
              if (!SideLink.Sector.Critical)
                SideSector.RemoveLink(SideLink);
            }
          }
          else
          {
            SectorList.Remove(SideSector);
          }
        }
      }
      public void ClipAndResize()
      {
        var MinLeft = SectorList.Min(A => A.Region.Left);
        var MinTop = SectorList.Min(A => A.Region.Top);
        var MaxRight = SectorList.Max(A => A.Region.Right);
        var MaxBottom = SectorList.Max(A => A.Region.Bottom);
        Debug.WriteLine($"{MinLeft}, {MinTop}, {MaxRight}, {MaxBottom}");

        // NOTE: we want a 1 square empty border around the entire map.
        var ShiftX = -MinLeft + 1;
        var ShiftY = -MinTop + 1;
        if (ShiftX != 0 || ShiftY != 0)
        {
          foreach (var Sector in SectorList)
            Sector.Shift(ShiftX, ShiftY);
        }

        this.Width = (MaxRight - MinLeft) + 3;
        this.Height = (MaxBottom - MinTop) + 3;
      }

      public override string ToString() => $"{Width}x{Height} Sectors={Sectors.Count}";

      private readonly Inv.DistinctList<OpusSector> SectorList;
    }

    private sealed class OpusSector
    {
      public OpusSector(Region Region, int Distance)
      {
        this.Region = Region;
        this.Distance = Distance;
        this.LinkList = new Inv.DistinctList<OpusLink>();
      }

      public Region Region { get; private set; }
      public int Distance { get; }
      public bool Critical { get; internal set; }
      public IReadOnlyList<OpusLink> Links => LinkList;

      public OpusLink AddLink(Region Region, OpusSector Sector)
      {
        var Result = new OpusLink(Region, Sector);
        LinkList.Add(Result);
        return Result;
      }
      public void RemoveLink(OpusLink Link)
      {
        LinkList.Remove(Link);
      }
      public void Shift(int X, int Y)
      {
        this.Region = Region.Shift(X, Y);

        foreach (var Link in LinkList)
          Link.Shift(X, Y);
      }

      public override string ToString() => $"{(Critical ? "Critical " : "")}{Distance}: {Region}";

      private readonly Inv.DistinctList<OpusLink> LinkList;
    }

    private sealed class OpusLink
    {
      public OpusLink(Region Region, OpusSector Sector)
      {
        this.Region = Region;
        this.Sector = Sector;
      }

      public Region Region { get; private set; }
      public OpusSector Sector { get; }

      public void Shift(int X, int Y)
      {
        this.Region = Region.Shift(X, Y);
      }

      public override string ToString() => $"{Region}: {Sector.Distance}";
    }

    private sealed class OpusDesign
    {
      internal OpusDesign(int Width, int Height, IReadOnlyList<OpusSection> SectionList)
      {
        this.Width = Width;
        this.Height = Height;
        this.Sections = SectionList;
      }

      public int Width { get; }
      public int Height { get; }
      public IReadOnlyList<OpusSection> Sections { get; }

      public override string ToString() => $"{Width}x{Height} Sections={Sections.Count}";
    }

    private sealed class OpusSection
    {
      public OpusSection(Region Region, int Distance)
      {
        this.Region = Region;
        this.Distance = Distance;
        this.JoinList = new Inv.DistinctList<OpusJoin>();
        this.ClearingList = new Inv.DistinctList<OpusClearing>();
        this.MinimumDifficulty = Distance / 6;
        this.MaximumDifficulty = Distance;
      }

      public string OverlandAreaName { get; set; }
      public string UndergroundAreaName { get; set; }
      public Region Region { get; }
      public int Distance { get; }
      public int MinimumDifficulty { get; }
      public int MaximumDifficulty { get; }
      public IReadOnlyList<OpusClearing> Clearings => ClearingList;
      public OpusClearing SmallestClearing => ClearingList.FirstByOrder(C => C.Circle.Radius);
      public OpusClearing LargestClearing => ClearingList.LastByOrder(C => C.Circle.Radius);
      public bool Critical { get; internal set; }

      public IReadOnlyList<OpusJoin> Joins => JoinList;

      public OpusClearing AddClearing(Inv.Circle Circle)
      {
        var Result = new OpusClearing(this, Circle);
        ClearingList.Add(Result);
        return Result;
      }
      public OpusJoin AddJoin(Region Region, OpusSection Section)
      {
        var Result = new OpusJoin(Region, Section);
        JoinList.Add(Result);
        return Result;
      }

      public override string ToString() => $"{Distance}: {Region}";

      private readonly Inv.DistinctList<OpusJoin> JoinList;
      private readonly Inv.DistinctList<OpusClearing> ClearingList;
    }

    private sealed class OpusJoin
    {
      public OpusJoin(Region Region, OpusSection Section)
      {
        this.Region = Region;
        this.Section = Section;
      }

      public Region Region { get; }
      public OpusSection Section { get; }

      public override string ToString() => $"{Region}: {Section.Distance}";
    }

    private sealed class OpusClearing
    {
      internal OpusClearing(OpusSection Section, Inv.Circle Circle)
      {
        this.Section = Section;
        this.Circle = Circle;
      }

      public OpusSection Section { get; }
      public Inv.Circle Circle { get; }

      public override string ToString() => $"{Circle}";
    }

    private sealed class OpusBuilding
    {
      public OpusBuilding(Region Region)
      {
        this.Region = Region;
      }

      public Region Region { get; }

      public override string ToString() => $"{Region}";
    }
  }
}