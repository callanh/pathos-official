#if DEBUG
//#define VERBOSE
#endif
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  internal sealed class NethackModule : Module
  {
    internal NethackModule(Codex Codex)
      : base(
          Handle: "nethack codex",
          Name: "Nethack Codex",
          Description: "Descend through procedurally generated dungeons and caverns. Infinite re-playability and recommended for players new to Pathos.",
          Colour: Inv.Colour.MediumVioletRed,
          Author: "Callan Hodgskin", Email: "hodgskin.callan@gmail.com",
          RequiresMasterMode: false)
    {
      this.Codex = Codex;

      SetIntroduction(Codex.Sonics.introduction);
      SetConclusion(Codex.Sonics.conclusion);
      SetTrack(Codex.Tracks.nethack_title);

      foreach (var TermText in CollectStaticPublicConstantStrings(typeof(NethackTerms)))
        AddTerm(TermText);

      foreach (var DialogueText in CollectStaticPublicConstantStrings(typeof(NethackDialogues)))
        AddDialogue(DialogueText);

      foreach (var WatchmanText in NethackDialogues.WatchmanList)
        AddDialogue(WatchmanText);
    }

    public override void Execute(Generator Generator)
    {
      new NethackMaker(Codex, Generator).Create();
    }

    private readonly Codex Codex;
  }

  internal static class NethackTerms
  {
    public const string Dungeon = "Dungeon";
    public const string Minetown = "Minetown";
    public const string Mines = "Mines";
    public const string Fort_Ludios = "Fort Ludios";
    public const string Fort_Ludios_Cache = "Ludios Cache";
    public const string Sokoban = "Sokoban";
    public const string Lost_Chambers = "Lost Chambers";
    public const string Labyrinth = "Labyrinth";
    public const string Lich_Tower = "Lich Tower";
    public const string Medusa_Lair = "Medusa Lair";
    public const string Elf_Kingdom = "Elf Kingdom";
    public const string Elf_Forest = "Elf Forest";
    public const string Elf_Town = "Elf Town";
    public const string Elf_Town_Trove = "Elf Town Trove";
    public const string Elf_Palace_1 = "Elf Palace 1";
    public const string Elf_Palace_2 = "Elf Palace 2";
    public const string Elf_Palace_Vault = "Elf Palace Vault";
    public const string Black_Market = "Black Market";
    public const string Abyss = "Abyss";
    public const string Infernal_Fields = "Infernal Fields";
    public const string Infernal_Keep = "Infernal Keep";
    public const string Infernal_Spire = "Infernal Spire";
    public const string Infernal_Cage = "Infernal Cage";
    public const string Underpeaks = "Underpeaks";
    public const string Underlimits = "Underlimits";
    public const string Undertown = "Undertown";
    public const string Underbarracks = "Underbarracks";
    public const string Deepwilds = "Deepwilds";
    public const string Deepruins = "Deepruins";
    public const string Deephall = "Deephall";
    public const string Bigfoot = "Bigfoot";

    public const string attic = "attic";
    public const string caves = "caves";
    public const string cellar = "cellar";
    public const string trove = "trove";
    public const string level = "level//depth";

    public const string Congratulations_on_making_it_to_the_final_level_and_defeating_your_nemesis = "Congratulations on making it to the final level and defeating your nemesis.";
    public const string To_complete_this_game_you_need_to_escape_the_dungeon_on_the_first_level = "To complete this game you need to escape the dungeon on the first level.";
  }

  internal static class NethackDialogues
  {
    public const string Goodbye = "Goodbye.";

    public static readonly IReadOnlyList<string> WatchmanList = new[]
    {
      "Will you please stop playing in the fountains. I'm not allowed to get this uniform wet so if you splash me I swear: *I will end you.*",
      "There's a staircase to the mines in the caves outside of town. Maybe you should go down and never come back?",
      "The food is so lousy here. Why do you think we kill monsters and have that workbench in the middle of town?",
      "Everyone is talking about an orcish invasion but I don't believe it will ever happen.",
      "Have you noticed that there's too many gnomes around here?",
      "I'm tired and my feet hurt! I can't wait for this patrol to be over so I can go to bed.",
      "Why don't you go play in the caves outside of town? There's lots of free stuff to collect but be aware that we don't patrol outside of town so make sure you can run back if you get into any trouble.",
      "Stay out of my way! I'm on an important patrol and you adventurers are always blocking my path.",
      "The Watch and the Merchant Guild have a long-standing agreement, so don't get any funny ideas.",
      "I swear you adventurers are harbingers of misfortune! Every time one of you degenerates turns up, we lose a dozen men.",
      "Any news from the surface? I've been down here so long that I miss sunshine and weather.",
      "Did you know that half of what the merchants sell is sourced from the corpses of adventurers like you?",
      "This is my last patrol before my retirement from the Watch. I sure hope nothing untoward happens!",
      "Make sure you trade with the town merchants and support the local economy! I wouldn't recommend doing it on an empty stomach though.",
      "I can't believe I have to explain this but make sure you have a pick-axe before going into the mines. You can even craft one at the town workbench with plenty of scrap iron.",
      "I've been told that there is a dwarven city underneath the gnomish mines. That's not a journey I will ever want to take - too many smelly, short people down there for my liking.",
      "Watch your step when you first enter the mines, those sneaky gnomish bastards love to ambush an unprepared adventurer.",
      "This town is a wonderful place to spend time and prepare yourself for your journey ahead. Don't be in a rush to leave here, there's plenty of things to see and do before you delve further into the dungeon.",
      "Keep an eye out for illusionary cave walls because pesky gnomish wizards have been at work recently. Absolute pests, the lot of them.",
      "There's a gem-rush going on in the mines below town. Expensive gemstones just lying around, not the usual worthless pieces of glass!",
      "I don't know why adventurers think they can shoot, shoot, shoot around the town with their arrows and darts. We are fed up with it, so even an accidental strike on our townsfolk will be met with deadly force.",
      "You know, I once was an adventurer like you... maybe you should get a job as well? After all, *adventurer* is just a fancy way of saying homeless and unemployed.",
      "Sometimes, powerful and famous heroes from across the lands grace us with their presence... I'll let you know if I see one today.",
      "The mines are a dangerous place to travel alone so you should consider hiring some of the local mercenaries to help you out. Be aware though that they are only reliable up until your coin runs out.",
      "The town shrine offers many boons to the virtuous adventurer, make sure you visit today and donate any spare coins. Bless you adventurer on your travels!"
    };
  }

  internal sealed class NethackMaker
  {
    internal NethackMaker(Codex Codex, Generator Generator)
    {
      this.Codex = Codex;
      this.Generator = Generator;

      this.ShopProbability = Generator.GetShops().ToProbability(M => M.Rarity);
      this.ShrineProbability = Generator.GetShrines().ToProbability(M => M.Rarity);

      this.AttractionProbability = new Probability<Attraction>();
      AttractionProbability.Add(1, new Attraction(AttractionType.Prison));
      AttractionProbability.Add(2, new Attraction(AttractionType.Vault));
      AttractionProbability.Add(8, new Attraction(AttractionType.Shop));
      AttractionProbability.Add(5, new Attraction(AttractionType.Shrine));
      AttractionProbability.Add(4, new Attraction(AttractionType.Attic));
      AttractionProbability.Add(3, new Attraction(AttractionType.Maze));
      AttractionProbability.Add(2, new Attraction(AttractionType.Zoo));
    }

    public void Create()
    {
      const int StartLevel = 1;
      //const int CastleLevels = 1;  // 19 castle levels.
      const int CavernLevels = 20; // 10 cavern levels.
      const int NetherLevels = 30; // 10 nether levels.
      const int FinaleLevel = 40;  //  1 finale level.

      const int CastleWidth = 30;
      const int CastleHeight = 30;
      const int CavernWidth = 30;
      const int CavernHeight = 30;
      const int NetherWidth = 42;
      const int NetherHeight = 42;
      const int FinaleWidth = 60;
      const int FinaleHeight = 60;

      var Site = Generator.Adventure.World.AddSite(Generator.EscapedModuleTerm(NethackTerms.Dungeon));

      var Portals = Codex.Portals;

      var CastlePlan = new DungeonPlan(Portals.stone_staircase_up, Portals.stone_staircase_down);
      var CavernPlan = new DungeonPlan(Portals.clay_staircase_up, Portals.clay_staircase_down);
      var NetherPlan = new DungeonPlan(Portals.stone_staircase_up, Portals.stone_staircase_down);

      var LevelSquare = (Square)null;

      // guarantee the branches.
      this.MinesIndex = 1.d4().Roll() + 1;
#if DEBUG
      //MinesIndex = 2;
#endif
      // 1-19.
      this.SokobanIndex = MinesIndex + 1.d4().Roll();
      this.FortIndex = Math.Max(SokobanIndex + 1, 7 + 1.d6().Roll());
      this.LabyrinthIndex = Math.Max(FortIndex + 1, 10 + 1.d6().Roll());
      this.ChambersIndex = Math.Max(LabyrinthIndex + 1, 13 + 1.d6().Roll());

      // 20-40.
      this.LairIndex = CavernLevels + 1.d((NetherLevels - CavernLevels) / 2).Roll() - 1;
      this.KingdomIndex = LairIndex + 1.d(NetherLevels - LairIndex - 1).Roll();
      this.TowerIndex = NetherLevels + 1.d((FinaleLevel - NetherLevels) / 2).Roll() - 1;
      this.MarketIndex = TowerIndex + 1.d5().Roll();
      this.AbyssIndex = FinaleLevel;

#if DEBUG
      var BranchArray = new[]
      {
        new { Index = MinesIndex, Name = "Minetown", From = 2, Until = 5 },
        new { Index = SokobanIndex, Name = "Sokoban", From = 3, Until = 9 },
        new { Index = FortIndex, Name = "Fort Ludios", From = 8, Until = 13 },
        new { Index = LabyrinthIndex, Name = "Labyrinth", From = 11, Until = 16 },
        new { Index = ChambersIndex, Name = "Lost Chambers", From = 14, Until = 19 },
        new { Index = LairIndex, Name = "Medusa Lair", From = 20, Until = 24 },
        new { Index = KingdomIndex, Name = "Elf Kingdom", From = 21, Until = 29 },
        new { Index = TowerIndex, Name = "Lich Tower", From = 30, Until = 34 },
        new { Index = MarketIndex, Name = "Black Market" , From = 31, Until = 39},
        new { Index = AbyssIndex, Name = "Infernal Abyss", From = 40, Until = 40 },
      };

      foreach (var Branch in BranchArray)
        Debug.WriteLine($"{Branch.Index:00} @ ({Branch.From:00} - {Branch.Until:00}) = {Branch.Name}");

      var BranchIssueList = new Inv.DistinctList<string>();
      foreach (var Branch in BranchArray)
      {
        if (Branch.Index < Branch.From || Branch.Index > Branch.Until)
          BranchIssueList.Add($"{Branch.Name} generated at level {Branch.Index} which is not between {Branch.From} and {Branch.Until}.");
      }

      if (BranchIssueList.Count > 0)
        throw new Exception(BranchIssueList.AsSeparatedText(Environment.NewLine));

      if (BranchArray.Distinct(B => B.Index).Count() < BranchArray.Length)
        throw new Exception("Multiple branches are not allowed to start on the same level.");

      if (!BranchArray.OrderBy(B => B.Index).ToArray().ShallowEqualTo(BranchArray))
        throw new Exception("Branches must be generated in the expected order.");
#endif

      for (var LevelIndex = StartLevel; LevelIndex <= FinaleLevel; LevelIndex++)
      {
        DebugStart();
        DebugWrite("Level " + LevelIndex.ToString("00"));

        var LevelName = Generator.EscapedModuleTerm(NethackTerms.level) + " " + LevelIndex;

        Level Level;
        DungeonPlan Plan;
        DungeonStructure Structure;

        if (LevelIndex < CavernLevels)
        {
          Level = Site.AddLevel(LevelIndex, Site.World.AddMap(LevelName, CastleWidth, CastleHeight));
          Level.Map.SetDifficulty(LevelIndex);

          Structure = CreateCastleLevel(Level);
          Plan = CastlePlan;
        }
        else if (LevelIndex < NetherLevels)
        {
          Level = Site.AddLevel(LevelIndex, Site.World.AddMap(LevelName, CavernWidth, CavernHeight));
          Level.Map.SetDifficulty(LevelIndex);

          Structure = CreateCavernLevel(Level);
          Plan = CavernPlan;
        }
        else if (LevelIndex < FinaleLevel)
        {
          Level = Site.AddLevel(LevelIndex, Site.World.AddMap(LevelName, NetherWidth, NetherHeight));
          Level.Map.SetDifficulty(LevelIndex);

          Structure = CreateNetherLevel(Level);
          Plan = NetherPlan;
        }
        else
        {
          Level = Site.AddLevel(LevelIndex, Site.World.AddMap(LevelName, FinaleWidth, FinaleHeight));
          Level.Map.SetDifficulty(LevelIndex);

          Structure = CreateFinaleLevel(Level);
          Plan = NetherPlan;
        }

        var RoomList = Structure.Rooms.Where(R => !R.Isolated && R.GetFloorSquares().Any(Generator.CanPlacePortal)).ToDistinctList();

        var UpRoom = RoomList.GetRandomOrNull();

        var UpSquare = UpRoom?.GetFloorSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();
        if (UpSquare == null)
          UpSquare = Level.Map.GetSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();

        if (UpSquare == null)
          throw new Exception("UpSquare not found on: " + Level.Map.Name);

        var UpPortal = Plan.LevelUpPortal;

        if (LevelSquare == null)
        {
          // site exit.
          Generator.PlacePassage(UpSquare, UpPortal, Destination: null);

          // start player character and allies.
          Generator.StartSquare(UpSquare);
        }
        else
        {
          var DownPortal = Plan.LevelDownPortal;

          Generator.PlacePassage(UpSquare, UpPortal, LevelSquare);
          Generator.PlacePassage(LevelSquare, DownPortal, UpSquare);
        }

        var DownRoom = RoomList.Except(UpRoom).GetRandomOrNull() ?? UpRoom;

        var DownSquare = DownRoom?.GetFloorSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();
        if (DownSquare == null)
          DownSquare = Level.Map.GetSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();

        if (DownSquare == null)
          throw new Exception("DownSquare not found: " + Level.Map.Name);

        Level.SetTransitions(UpSquare, DownSquare);

        LevelSquare = DownSquare;

        DebugStop();
      }
    }

    private DungeonStructure CreateCastleLevel(Level CastleLevel)
    {
      var CastleBlock = Codex.Blocks.stone_boulder;
      var CastleBarrier = Codex.Barriers.stone_wall;
      var CastleGate = Codex.Gates.wooden_door;
      var CastleRoomGround = Codex.Grounds.stone_floor;
      var CastleCorridorGround = Codex.Grounds.stone_path;

      var CastleMap = CastleLevel.Map;
      CastleMap.SetAtmosphere(Codex.Atmospheres.dungeon);

      var CastleStructure = new DungeonStructure(CastleMap);

      const int BoxSize = 10;
      var BoxWidth = CastleMap.Region.Width / BoxSize;
      var BoxHeight = CastleMap.Region.Height / BoxSize;

      var OrdinaryRoomSize = 1.d(BoxSize - 4) + 3;

      // First level has nine castle rooms.
      var HasAttraction = CastleLevel.Index > 1;
      var Attraction = HasAttraction ? AttractionProbability.GetRandom().Type : AttractionType.Void;
      var AttractionX = HasAttraction ? 1.d(BoxWidth).Roll() - 1 : -1;
      var AttractionY = HasAttraction ? 1.d(BoxHeight).Roll() - 1 : -1;

#if DEBUG
      //Attraction = AttractionType.Zoo; AttractionX = 1; AttractionY = 1;
#endif

      // guarantee attractions.
      if (CastleLevel.Index == SokobanIndex)
        Attraction = AttractionType.Maze;
      else if (CastleLevel.Index == FortIndex)
        Attraction = AttractionType.Vault;
      else if (CastleLevel.Index == ChambersIndex)
        Attraction = AttractionType.Attic;

      Debug.WriteLine($"Castle {CastleLevel.Index} Attraction: {Attraction}");

      // Rooms.
      var RoomGrid = new Inv.Grid<DungeonRoom>(BoxWidth, BoxHeight);
      var BoxTop = 0;

      for (var BoxY = 0; BoxY < BoxHeight; BoxY++)
      {
        var BoxLeft = 0;

        for (var BoxX = 0; BoxX < BoxWidth; BoxX++)
        {
          if (BoxX == AttractionX && BoxY == AttractionY)
          {
            switch (Attraction)
            {
              case AttractionType.Void:
                // NOTE: no longer generated.
                DebugWrite("; void");

                RoomGrid[BoxX, BoxY] = null; // no corridors.
                break;

              case AttractionType.Vault:
                DebugWrite("; vault");

                const int VaultWidth = 4;
                const int VaultHeight = 4;
                var VaultLeft = BoxLeft + 1.d(BoxSize - VaultWidth).Roll();
                var VaultTop = BoxTop + 1.d(BoxSize - VaultHeight).Roll();
                var VaultRegion = new Region(VaultLeft, VaultTop, VaultLeft + VaultWidth - 1, VaultTop + VaultHeight - 1);

                var VaultZone = CastleMap.AddZone();
                VaultZone.AddRegion(VaultRegion);
                VaultZone.SetLit(Generator.RandomRoomIsLit(CastleMap));

                Generator.PlaceRoom(CastleMap, CastleBarrier, CastleRoomGround, VaultRegion);

                var VaultRoom = CastleStructure.AddRoom(VaultRegion, Isolated: true);
                if (VaultRoom.Map.Level.Index != FortIndex || !CreateFortLudiosBranch(VaultRoom))
                  CreateVaultRoom(VaultZone, VaultRoom);
                RoomGrid[BoxX, BoxY] = null; // we do not want corridors.
                break;

              case AttractionType.Maze:
                DebugWrite("; maze");

                var MazeRegion = new Region(BoxLeft + 1, BoxTop + 1, BoxLeft + BoxSize - 1, BoxTop + BoxSize - 1);

                var MazeRoom = CastleStructure.AddRoom(MazeRegion, Isolated: true);

                // fill with solid walls.
                foreach (var CastleSquare in CastleMap.GetSquares(MazeRegion))
                  Generator.PlaceSolidWall(CastleSquare, CastleBarrier, WallSegment.Cross);

                // dig out the paths.
                CreateMazePaths(CastleMap, MazeRegion);

                // NOTE: don't generate boulders in the maze as they could obstruct the corridor doors that get placed later on.
                CreateMazeDetails(CastleStructure, CastleMap, Block: null, MazeRegion, Math.Max(MazeRoom.Region.Width, MazeRoom.Region.Height) / 2);

                // repair void squares and wall segments.
                Generator.RepairVoid(CastleMap, MazeRegion);
                Generator.RepairWalls(CastleMap, MazeRegion);

                RoomGrid[BoxX, BoxY] = MazeRoom;
                MazeRoom.SetLit(false); // always dark maze.
                break;

              case AttractionType.Zoo:
                var ZooRoomSize = 1.d(BoxSize - 6) + 5;
#if DEBUG
                //ZooRoomSize = Dice.Fixed(BoxSize - 1);
#endif
                var ZooWidth = ZooRoomSize.Roll();
                var ZooHeight = ZooRoomSize.Roll();
                var ZooLeft = BoxLeft + 1.d(BoxSize - ZooWidth).Roll();
                var ZooTop = BoxTop + 1.d(BoxSize - ZooHeight).Roll();
                var ZooRegion = new Region(ZooLeft, ZooTop, ZooLeft + ZooWidth - 1, ZooTop + ZooHeight - 1);

                var ZooZone = CastleMap.AddZone();
                ZooZone.AddRegion(ZooRegion);
                ZooZone.SetLit(Generator.RandomRoomIsLit(CastleMap));

                Generator.PlaceRoom(CastleMap, CastleBarrier, CastleRoomGround, ZooRegion);

                var ZooRoom = CastleStructure.AddRoom(ZooRegion, Isolated: true);
                CreateZooRoom(ZooZone, ZooRoom, Zoo: null);
                RoomGrid[BoxX, BoxY] = ZooRoom;
                break;

              case AttractionType.Attic:
                var AtticWidth = OrdinaryRoomSize.Roll();
                var AtticHeight = OrdinaryRoomSize.Roll();
                var AtticLeft = BoxLeft + 1.d(BoxSize - AtticWidth).Roll();
                var AtticTop = BoxTop + 1.d(BoxSize - AtticHeight).Roll();
                var AtticRegion = new Region(AtticLeft, AtticTop, AtticLeft + AtticWidth - 1, AtticTop + AtticHeight - 1);

                var AtticZone = CastleMap.AddZone();
                AtticZone.AddRegion(AtticRegion);
                AtticZone.SetLit(Generator.RandomRoomIsLit(CastleMap));

                Generator.PlaceRoom(CastleMap, CastleBarrier, CastleRoomGround, AtticRegion);

                var AtticRoom = CastleStructure.AddRoom(AtticRegion, Isolated: true);
                if (AtticRoom.Map.Level.Index != ChambersIndex || !CreateLostChambersBranch(AtticRoom))
                  CreateAtticRoom(AtticRoom, CastleBarrier, CastleRoomGround);
                RoomGrid[BoxX, BoxY] = AtticRoom;
                break;

              case AttractionType.Shop:
                var ShopRoomSize = 1.d4() + 4; // 5..8 - minimum 5 is important because the merchant is always on the midpoint, and this prevents the merchant generating on the edge of a room.
                var ShopWidth = ShopRoomSize.Roll();
                var ShopHeight = ShopRoomSize.Roll();
                var ShopLeft = BoxLeft + 1.d(BoxSize - ShopWidth).Roll();
                var ShopTop = BoxTop + 1.d(BoxSize - ShopHeight).Roll();
                var ShopRegion = new Region(ShopLeft, ShopTop, ShopLeft + ShopWidth - 1, ShopTop + ShopHeight - 1);

                var ShopZone = CastleMap.AddZone();
                ShopZone.AddRegion(ShopRegion);
                ShopZone.SetLit(true); // shops are always bright.

                Generator.PlaceRoom(CastleMap, CastleBarrier, CastleRoomGround, ShopRegion);

                var ShopRoom = CastleStructure.AddRoom(ShopRegion, Isolated: true);
                CreateShopRoom(ShopZone, ShopRoom, Shop: null); // allow creating a bazaar.
                RoomGrid[BoxX, BoxY] = ShopRoom;
                break;

              case AttractionType.Shrine:
                var ShrineRoomSize = 1.d(BoxSize - 6) + 5; // 6..10
                var ShrineWidth = ShrineRoomSize.Roll();
                var ShrineHeight = ShrineRoomSize.Roll();
                var ShrineLeft = BoxLeft + 1.d(BoxSize - ShrineWidth).Roll();
                var ShrineTop = BoxTop + 1.d(BoxSize - ShrineHeight).Roll();
                var ShrineRegion = new Region(ShrineLeft, ShrineTop, ShrineLeft + ShrineWidth - 1, ShrineTop + ShrineHeight - 1);

                var ShrineZone = CastleMap.AddZone();
                ShrineZone.AddRegion(ShrineRegion);
                ShrineZone.SetLit(false); // always dark, for contemplation.

                Generator.PlaceRoom(CastleMap, CastleBarrier, CastleRoomGround, ShrineRegion);

                var ShrineRoom = CastleStructure.AddRoom(ShrineRegion, Isolated: true);
                CreateShrineRoom(ShrineZone, ShrineRoom, Shrine: null);
                RoomGrid[BoxX, BoxY] = ShrineRoom;
                break;

              case AttractionType.Prison:
                DebugWrite("; prison");

                const int PrisonWidth = 7;
                const int PrisonHeight = 7;
                var PrisonLeft = BoxLeft + 1.d(BoxSize - PrisonWidth).Roll();
                var PrisonTop = BoxTop + 1.d(BoxSize - PrisonHeight).Roll();
                var PrisonRegion = new Region(PrisonLeft, PrisonTop, PrisonLeft + PrisonWidth - 1, PrisonTop + PrisonHeight - 1);

                var PrisonZone = CastleMap.AddZone();
                PrisonZone.AddRegion(PrisonRegion);
                PrisonZone.SetLit(Generator.RandomRoomIsLit(CastleMap));

                Generator.PlaceRoom(CastleMap, CastleBarrier, CastleRoomGround, PrisonRegion);

                var PrisonRoom = CastleStructure.AddRoom(PrisonRegion, Isolated: true);
                CreatePrisonRoom(PrisonZone, PrisonRoom);

                RoomGrid[BoxX, BoxY] = null; // no corridors.
                break;

              default:
                throw new Exception("RoomType not handled: " + Attraction);
            }
          }
          else
          {
            // ordinary room.
            var OrdinaryWidth = OrdinaryRoomSize.Roll();
            var OrdinaryHeight = OrdinaryRoomSize.Roll();
            var OrdinaryLeft = BoxLeft + 1.d(BoxSize - OrdinaryWidth).Roll();
            var OrdinaryTop = BoxTop + 1.d(BoxSize - OrdinaryHeight).Roll();
            var OrdinaryRegion = new Region(OrdinaryLeft, OrdinaryTop, OrdinaryLeft + OrdinaryWidth - 1, OrdinaryTop + OrdinaryHeight - 1);

            var OrdinaryZone = CastleMap.AddZone();
            OrdinaryZone.AddRegion(OrdinaryRegion);
            OrdinaryZone.SetLit(Generator.RandomRoomIsLit(CastleMap));

            Generator.PlaceRoom(CastleMap, CastleBarrier, CastleRoomGround, OrdinaryRegion);

            var OrdinaryRoom = CastleStructure.AddRoom(OrdinaryRegion, Isolated: false);

            RoomGrid[BoxX, BoxY] = OrdinaryRoom;

            // ordinary rooms may still contain tricks.
            CreateTrickRoom(CastleLevel.Index == 1 ? Chance.Never : Chance.OneIn30, OrdinaryZone, OrdinaryRoom);
          }

          BoxLeft += BoxSize;
        }

        BoxTop += BoxSize;
      }

      // corridors & doors.
      for (var BoxY = 0; BoxY < RoomGrid.Height; BoxY++)
      {
        for (var BoxX = 0; BoxX < RoomGrid.Width; BoxX++)
        {
          var StartRoom = RoomGrid[BoxX, BoxY];
          if (StartRoom == null)
            continue;

          if (BoxX < RoomGrid.Width - 1)
          {
            var EndRoom = RoomGrid[BoxX + 1, BoxY];
            if (EndRoom != null)
            {
              var StartSquare = StartRoom.RightEdgeSquares().Where(S => S.Adjacent(Direction.West).Wall == null).GetRandomOrNull();
              var EndSquare = EndRoom.LeftEdgeSquares().Where(S => S.Adjacent(Direction.East).Wall == null).GetRandomOrNull();
              if (StartSquare != null && EndSquare != null)
              {
                Generator.PlaceFloor(StartSquare, CastleRoomGround);
                Generator.PlaceRandomVerticalDoor(StartSquare, CastleGate, CastleBarrier);

                Generator.PlaceFloor(EndSquare, CastleRoomGround);
                Generator.PlaceRandomVerticalDoor(EndSquare, CastleGate, CastleBarrier);

                var StartCorridor = new DungeonCorridor(StartRoom, CastleMap[StartSquare.X + 1, StartSquare.Y]);
                var EndCorridor = new DungeonCorridor(EndRoom, CastleMap[EndSquare.X - 1, EndSquare.Y]);
                JoinCorridor(CastleMap, CastleCorridorGround, StartCorridor, EndCorridor);

                if (Attraction == AttractionType.Zoo)
                {
                  if (StartSquare.Door != null && AttractionX == BoxX && AttractionY == BoxY)
                    Generator.LockDoor(StartSquare);
                  else if (EndSquare.Door != null && AttractionX == BoxX + 1 && AttractionY == BoxY)
                    Generator.LockDoor(EndSquare);
                }
              }
            }
          }

          if (BoxY < RoomGrid.Height - 1)
          {
            var EndRoom = RoomGrid[BoxX, BoxY + 1];
            if (EndRoom != null)
            {
              var StartSquare = StartRoom.BottomEdgeSquares().Where(S => S.Adjacent(Direction.North).Wall == null).GetRandomOrNull();
              var EndSquare = EndRoom.TopEdgeSquares().Where(S => S.Adjacent(Direction.South).Wall == null).GetRandomOrNull();
              if (StartSquare != null && EndSquare != null)
              {
                Generator.PlaceFloor(StartSquare, CastleRoomGround);
                Generator.PlaceRandomHorizontalDoor(StartSquare, CastleGate, CastleBarrier);

                Generator.PlaceFloor(EndSquare, CastleRoomGround);
                Generator.PlaceRandomHorizontalDoor(EndSquare, CastleGate, CastleBarrier);

                var StartCorridor = new DungeonCorridor(StartRoom, CastleMap[StartSquare.X, StartSquare.Y + 1]);
                var EndCorridor = new DungeonCorridor(EndRoom, CastleMap[EndSquare.X, EndSquare.Y - 1]);

                JoinCorridor(CastleMap, CastleCorridorGround, StartCorridor, EndCorridor);

                if (Attraction == AttractionType.Zoo)
                {
                  if (StartSquare.Door != null && AttractionX == BoxX && AttractionY == BoxY)
                    Generator.LockDoor(StartSquare);
                  else if (EndSquare.Door != null && AttractionX == BoxX && AttractionY == BoxY + 1)
                    Generator.LockDoor(EndSquare);
                }
              }
            }
          }
        }
      }

      // possible mines branch.
      if (CastleLevel.Index == MinesIndex)
        CreateMinesBranch(CastleLevel, CastleStructure);
      else if (CastleLevel.Index == LabyrinthIndex)
        CreateLabyrinthBranch(CastleLevel, CastleStructure);

      // castle nooks.
      foreach (var Room in CastleStructure.Rooms.Where(R => !R.Isolated))
      {
        if (Room.Region.Left > 0)
        {
          for (var Y = Room.Region.Top + 1; Y < Room.Region.Bottom; Y++)
            CreateCastleNook(CastleMap[Room.Region.Left, Y], CastleMap[Room.Region.Left - 1, Y]);
        }

        if (Room.Region.Right < CastleMap.Region.Right)
        {
          for (var Y = Room.Region.Top + 1; Y < Room.Region.Bottom; Y++)
            CreateCastleNook(CastleMap[Room.Region.Right, Y], CastleMap[Room.Region.Right + 1, Y]);
        }

        if (Room.Region.Top > 0)
        {
          for (var X = Room.Region.Left + 1; X < Room.Region.Right; X++)
            CreateCastleNook(CastleMap[X, Room.Region.Top], CastleMap[X, Room.Region.Top - 1]);
        }

        if (Room.Region.Bottom < CastleMap.Region.Bottom)
        {
          for (var X = Room.Region.Left + 1; X < Room.Region.Right; X++)
            CreateCastleNook(CastleMap[X, Room.Region.Bottom], CastleMap[X, Room.Region.Bottom + 1]);
        }
      }

      // maze boss.
      if (Attraction == AttractionType.Maze)
      {
        var MazeRoom = RoomGrid[AttractionX, AttractionY];

        Generator.RepairZones(CastleMap, MazeRoom.Region);

        var MazeSquare = CreateMazeCorner(MazeRoom, CastleBarrier, CastleGate);
        if (MazeSquare != null && (CastleLevel.Index != SokobanIndex || !CreateSokobanBranch(MazeSquare)))
          CreateMazeBoss(MazeSquare);
      }

      // Traps, gold and assets.
      foreach (var Room in CastleStructure.Rooms.Where(R => !R.Isolated))
      {
        var FloorSquareList = Room.GetFloorSquares().ToDistinctList();

        Generator.PlaceRoomFixtures(FloorSquareList);

        CreateRoomDetails(CastleStructure, CastleMap, CastleBlock, FloorSquareList);
      }

      // NOTE: only required because the maze bosses sometimes have illusionary walls.
      Generator.RepairVoid(CastleMap, CastleMap.Region);
      Generator.RepairWalls(CastleMap, CastleMap.Region);

      return CastleStructure;
    }
    private DungeonStructure CreateCavernLevel(Level CavernLevel)
    {
      var CavernBlock = Codex.Blocks.clay_boulder;
      var CavernBarrier = Codex.Barriers.cave_wall;
      var CavernGround = Codex.Grounds.cave_floor;
      var CavernGate = (Gate)null;

      var CavernMap = CavernLevel.Map;
      CavernMap.SetAtmosphere(Codex.Atmospheres.cavern);

      var LevelWidth = CavernMap.Region.Width;
      var LevelHeight = CavernMap.Region.Height;

      // TODO: randomise cave region/size relative to level index.

      var CavernWidth = (1.d(LevelWidth / CavernSize - 1).Roll() + 1) * CavernSize;
      var CavernHeight = (1.d(LevelHeight / CavernSize - 1).Roll() + 1) * CavernSize;
      var CavernLeft = 1.d(LevelWidth - CavernWidth + 1).Roll() - 1;
      var CavernTop = 1.d(LevelHeight - CavernHeight + 1).Roll() - 1;
      var CavernRegion = new Region(CavernLeft, CavernTop, CavernLeft + CavernWidth - 1, CavernTop + CavernHeight - 1);

      var CavernStructure = CreateCavern(CavernMap, CavernRegion);

      var AttractionCount = 1.d(Math.Max(1, CavernStructure.Rooms.Count / 5) - 1).Roll() + 1;

      var CavernShopProbability = ShopProbability.Clone();
      var CavernShrineProbability = ShrineProbability.Clone();
      var CavernZooProbability = GetZooProbability(CavernMap);

      for (var AttractionIndex = 0; AttractionIndex < AttractionCount; AttractionIndex++)
      {
        var CavernRoom = CavernStructure.Rooms.Where(R => !R.Isolated).GetRandomOrNull();

        if (CavernRoom != null)
        {
          if (CavernStructure.Rooms.Count > 1)
            CavernStructure.RemoveRoom(CavernRoom); // don't create a portal in this attraction room.

          if (AttractionIndex == 0 && LairIndex == CavernLevel.Index && CreateMedusaLairBranch(CavernRoom))
          {
            // medusa lair handled.
          }
          else if (AttractionIndex == 0 && KingdomIndex == CavernLevel.Index && CreateElfKingdomBranch(CavernRoom))
          {
            // elf kingdom handled.
          }
          else
          {
            var AttractionType = AttractionProbability.GetRandom().Type;
            if (AttractionIndex == 0 && TowerIndex == CavernLevel.Index)
              AttractionType = AttractionType.Attic;
#if DEBUG
            //AttractionType = AttractionType.Prison; 
#endif

            var CavernZone = CavernRoom.Midpoint.Zone;

            switch (AttractionType)
            {
              case AttractionType.Void:
                break;

              case AttractionType.Maze:
                var MazeSquare = CreateMazeCorner(CavernRoom, CavernBarrier, CavernGate);
                if (MazeSquare != null)
                  CreateMazeBoss(MazeSquare);
                break;

              case AttractionType.Attic:
                CreateAtticRoom(CavernRoom, CavernBarrier, CavernGround);
                break;

              case AttractionType.Prison:
                CreatePrisonRoom(CavernZone, CavernRoom);
                break;

              case AttractionType.Shop:
                CreateShopRoom(CavernZone, CavernRoom, CavernShopProbability.RemoveRandomOrNull()); // no bazaars in caverns anyway.
                break;

              case AttractionType.Shrine:
                CreateShrineRoom(CavernZone, CavernRoom, CavernShrineProbability.RemoveRandomOrNull());
                break;

              case AttractionType.Vault: // Trove.
                var TroveSquare = CavernRoom.GetFloorSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();
                if (TroveSquare != null)
                  CreateTroveNook(TroveSquare, CavernBarrier, CavernGround);
                break;

              case AttractionType.Zoo:
                CreateZooRoom(CavernZone, CavernRoom, CavernZooProbability.RemoveRandomOrNull());
                break;

              default:
                throw new Exception("RoomType not handled: " + AttractionType);
            }
          }
        }
      }

      // large number of rooms mean higher attraction count, therefore there will be more chances for features.
      var FeaturesChance = Chance.OneIn(AttractionCount);
      var TricksChance = Chance.OneIn(30 * AttractionCount);

      // ordinary cavern rooms need details and features.
      foreach (var CavernRoom in CavernStructure.Rooms.Where(R => !R.Isolated))
      {
        CreateTrickRoom(TricksChance, CavernRoom.Midpoint.Zone, CavernRoom);

        var FloorSquareList = CavernRoom.GetFloorSquares().ToDistinctList();

        if (FeaturesChance.Hit())
          Generator.PlaceRoomFixtures(FloorSquareList);

        CreateRoomDetails(CavernStructure, CavernMap, CavernBlock, FloorSquareList);
      }

      Generator.RepairVoid(CavernMap, CavernRegion);
      Generator.RepairWalls(CavernMap, CavernRegion);

      return CavernStructure;
    }
    private DungeonStructure CreateNetherLevel(Level NetherLevel)
    {
      var NetherBlock = Codex.Blocks.crystal_boulder;
      var NetherGate = Codex.Gates.crystal_door;
      var NetherBarrier = Codex.Barriers.hell_brick;
      var NetherGround = Codex.Grounds.obsidian_floor;

      var CavernGround = Codex.Grounds.dirt;
      var CavernBarrier = Codex.Barriers.cave_wall;

      var NetherMap = NetherLevel.Map;
      NetherMap.SetAtmosphere(Codex.Atmospheres.nether);

      var LevelRegion = NetherMap.Region;
      var LevelWidth = LevelRegion.Width;
      var LevelHeight = LevelRegion.Height;

      var NetherStructure = CreateCavern(NetherMap, LevelRegion);

      // obliterate the inside caverns.
      var NetherRegion = new Region(CavernSize, CavernSize, LevelWidth - CavernSize - 1, LevelHeight - CavernSize - 1);

      foreach (var Square in NetherMap.GetSquares(NetherRegion))
      {
        if (Square.Wall != null)
          Generator.RemoveWall(Square);

        Generator.PlaceFloor(Square, CavernGround);
      }

      // strip out all zones.
      NetherMap.RemoveZones();
      NetherStructure.RemoveRooms();

      // repair the gaps.
      foreach (var Square in NetherMap.GetSquares(LevelRegion))
      {
        if (Square.IsEmpty() && Square.GetAdjacentSquares().Any(S => S.Floor != null && S.Wall == null))
          Generator.PlaceSolidWall(Square, CavernBarrier, WallSegment.Cross);
      }

      Generator.RepairVoid(NetherMap, LevelRegion);
      Generator.RepairWalls(NetherMap, LevelRegion);

      // these rooms are where the up/down stairs should be generated.
      NetherStructure.AddRoom(new Region(1, 1, CavernSize - 2, LevelHeight - 2), Isolated: false);
      NetherStructure.AddRoom(new Region(CavernSize - 1, 1, LevelWidth - 2, CavernSize - 2), Isolated: false);
      NetherStructure.AddRoom(new Region(LevelWidth - CavernSize - 2, 1, LevelWidth - 2, LevelHeight - 2), Isolated: false);
      NetherStructure.AddRoom(new Region(CavernSize - 1, LevelHeight - CavernSize + 1, LevelWidth - CavernSize + 1, LevelHeight - 2), Isolated: false);

      var BSPEngine = new BSPEngine();
      BSPEngine.MinimumRoomSize = 5;
      BSPEngine.MinimumPartitionSize = 7;
      BSPEngine.MaximumPartitionSize = 9;

      var BSPMap = BSPEngine.Generate(NetherRegion.Width, NetherRegion.Height);

      var NetherRoomList = new Inv.DistinctList<DungeonRoom>(BSPMap.PartitionList.Count);

      var TowerGenerated = false;
      var MarketGenerated = false;

      var NetherShopProbability = ShopProbability.Clone();
      var NetherShrineProbability = ShrineProbability.Clone();
      var NetherZooProbability = GetZooProbability(NetherMap);

      foreach (var BSPPartition in BSPMap.PartitionList)
      {
        if (BSPPartition.Room != null)
        {
          var NetherRoom = NetherStructure.AddRoom(new Region(
            NetherRegion.Left + BSPPartition.Room.Value.Left,
            NetherRegion.Top + BSPPartition.Room.Value.Top,
            NetherRegion.Left + BSPPartition.Room.Value.Right,
            NetherRegion.Top + BSPPartition.Room.Value.Bottom), Isolated: true);
          NetherRoomList.Add(NetherRoom);

          var NetherZone = NetherMap.AddZone();
          NetherZone.AddRegion(NetherRoom.Region);
          NetherZone.SetLit(Generator.RandomRoomIsLit(NetherMap));

          switch (1.d10().Roll())
          {
            case 1:
            case 2:
            case 3:
            case 4:
              Generator.PlaceFloorFill(NetherMap, NetherGround, NetherRoom.Region); // replace the cavern floor under the hell brick walls with obsidian. 
              Generator.PlaceSolidWallFrame(NetherMap, NetherBarrier, NetherRoom.Region);

              switch (1.d5().Roll())
              {
                case 1:
                  Generator.PlaceRandomHorizontalDoor(NetherRoom.TopEdgeSquares().ToArray().GetRandom(), NetherGate, NetherBarrier);
                  break;

                case 2:
                  Generator.PlaceRandomHorizontalDoor(NetherRoom.TopEdgeSquares().ToArray().GetRandom(), NetherGate, NetherBarrier);
                  break;

                case 3:
                  Generator.PlaceRandomVerticalDoor(NetherRoom.LeftEdgeSquares().ToArray().GetRandom(), NetherGate, NetherBarrier);
                  break;

                case 4:
                  Generator.PlaceRandomVerticalDoor(NetherRoom.RightEdgeSquares().ToArray().GetRandom(), NetherGate, NetherBarrier);
                  break;

                case 5:
                  // no door.
                  break;

                default:
                  throw new Exception("Nether room door not handled.");
              }
              break;

            case 5:
            case 6:
            case 7:
              PlaceIllusionRoom(NetherRoom, NetherBarrier, NetherGround);
              break;

            case 8:
            case 9:
            case 10:
              PlacePillarRoom(NetherRoom, NetherBarrier, NetherGround);
              break;

            default:
              throw new Exception("Nether room case not handled.");
          }

          var AttractionType = AttractionProbability.GetRandom().Type;
#if DEBUG
          //AttractionType = AttractionType.Prison; 
#endif

          if (MarketIndex == NetherLevel.Index && !MarketGenerated && CreateBlackMarketBranch(NetherRoom))
          {
            MarketGenerated = true;
          }
          else
          {
            if (TowerIndex == NetherLevel.Index && !TowerGenerated)
            {
              TowerGenerated = true;
              AttractionType = AttractionType.Attic;
            }

            switch (AttractionType)
            {
              case AttractionType.Void:
                break;

              case AttractionType.Maze:
                var BossSquare = NetherRoom.GetFloorSquares().Where(Generator.CanPlaceCharacter).GetRandomOrNull();
                if (BossSquare != null)
                  CreateMazeBoss(BossSquare);
                else
                  Debug.Fail("Why no boss?");
                break;

              case AttractionType.Attic:
                CreateAtticRoom(NetherRoom, NetherBarrier, NetherGround);
                CreateRoomDetails(NetherStructure, NetherMap, NetherBlock, NetherRoom.GetFloorSquares().ToDistinctList());
                break;

              case AttractionType.Shop:
                CreateShopRoom(NetherZone, NetherRoom, NetherShopProbability.RemoveRandomOrNull());
                break;

              case AttractionType.Shrine:
                CreateShrineRoom(NetherZone, NetherRoom, NetherShrineProbability.RemoveRandomOrNull());
                break;

              case AttractionType.Vault: // Trove.
                var TroveSquare = NetherRoom.GetFloorSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();
                if (TroveSquare != null)
                  CreateTroveNook(TroveSquare, NetherBarrier, NetherGround);
                else
                  Debug.Fail("Why no trove?");

                CreateRoomDetails(NetherStructure, NetherMap, NetherBlock, NetherRoom.GetFloorSquares().ToDistinctList());
                break;

              case AttractionType.Prison:
                CreatePrisonRoom(NetherZone, NetherRoom);
                break;

              case AttractionType.Zoo:
                CreateZooRoom(NetherZone, NetherRoom, NetherZooProbability.RemoveRandomOrNull());
                break;

              default:
                throw new Exception("RoomType not handled: " + AttractionType);
            }
          }
        }
      }

      Generator.RepairZones(NetherMap, LevelRegion);

      // details in every zone.
      foreach (var NetherZone in NetherMap.Zones)
      {
        var Boundary = NetherZone.GetBoundary();

        if (NetherRoomList.Any(R => R.Region == Boundary))
          continue;

        var FloorSquareList = NetherZone.Squares.ToDistinctList();

        if (Chance.OneIn10.Hit())
          Generator.PlaceRoomFixtures(FloorSquareList);

        if (Chance.OneIn2.Hit())
          CreateRoomDetails(NetherStructure, NetherMap, NetherBlock, FloorSquareList);
      }

      return NetherStructure;
    }
    private DungeonStructure CreateFinaleLevel(Level FinaleLevel)
    {
      var FinaleBlock = Codex.Blocks.crystal_boulder;
      var FinaleBarrier = Codex.Barriers.hell_brick;
      var FinaleGround = Codex.Grounds.obsidian_floor;

      var FinaleMap = FinaleLevel.Map;
      FinaleMap.SetImportant(true);
      FinaleMap.SetTerminal(true);
      FinaleMap.SetAtmosphere(Codex.Atmospheres.nether);

      var FinaleStructure = new DungeonStructure(FinaleMap);

      const int BoxSize = 10;

      // fill with walls.
      foreach (var FinaleSquare in FinaleMap.GetSquares(FinaleMap.Region))
        Generator.PlaceSolidWall(FinaleSquare, FinaleBarrier, WallSegment.Cross);

      CreateMazePaths(FinaleMap, FinaleMap.Region);

      var FinalVaultsColumns = FinaleMap.Region.Width / BoxSize;
      var FinalVaultsRows = FinaleMap.Region.Height / BoxSize;

      var StartX = 1.d(FinalVaultsColumns).Roll() - 1;
      var StartY = 1.d(FinalVaultsRows).Roll() - 1;

      for (var FinalVaultX = 0; FinalVaultX < FinalVaultsColumns; FinalVaultX++)
      {
        for (var FinalVaultY = 0; FinalVaultY < FinalVaultsRows; FinalVaultY++)
        {
          var FinalVaultLeft = (FinalVaultX * BoxSize) + 3;
          var FinalVaultTop = (FinalVaultY * BoxSize) + 3;

          var FinalVaultRegion = new Region(FinalVaultLeft, FinalVaultTop, FinalVaultLeft + 3, FinalVaultTop + 3);
          foreach (var FinaleSquare in FinaleMap.GetSquares(FinalVaultRegion).Where(S => S.Wall != null))
            Generator.RemoveWall(FinaleSquare);

          var FinalVaultZone = FinaleMap.AddZone();
          FinalVaultZone.AddRegion(FinalVaultRegion);
          FinalVaultZone.SetLit(false);

          Generator.PlaceRoom(FinaleMap, FinaleBarrier, FinaleGround, FinalVaultRegion);

          foreach (var FinaleSquare in FinaleMap.GetFrameSquares(FinalVaultRegion.Expand(1)))
          {
            if (FinaleSquare.Wall != null)
              Generator.RemoveWall(FinaleSquare);

            Generator.PlaceFloor(FinaleSquare, FinaleGround);
          }

          if (StartX == FinalVaultX && StartY == FinalVaultY)
          {
            FinaleStructure.AddRoom(FinalVaultRegion, Isolated: false);
          }
          else
          {
            switch (1.d6().Roll())
            {
              case 1:
                // coins.
                foreach (var FinalSquare in FinaleMap.GetSquares(FinalVaultRegion.Reduce(1)))
                  Generator.DropCoins(FinalSquare, Generator.RandomCoinQuantity(FinalSquare) * 1.d10().Roll());
                break;

              case 2:
                // gems.
                var GemProbability = Codex.Items.List.Where(I => I.Type == ItemType.Gem && I.Price > Gold.One && !I.Grade.Unique).ToProbability(I => I.Rarity);

                foreach (var FinalSquare in FinaleMap.GetSquares(FinalVaultRegion.Reduce(1)))
                {
                  // 1..3 gems per square.
                  foreach (var Item in 1.d5().Roll().NumberSeries())
                    Generator.PlaceSpecificAsset(FinalSquare, GemProbability.GetRandom());
                }
                break;

              case 3:
                // items.
                foreach (var FinalSquare in FinaleMap.GetSquares(FinalVaultRegion.Reduce(1)))
                {
                  // 1..3 items per square.
                  foreach (var Item in 1.d3().Roll().NumberSeries())
                    Generator.PlaceRandomAsset(FinalSquare);
                }
                break;

              case 4:
                // traps.
                foreach (var FinalSquare in FinaleMap.GetSquares(FinalVaultRegion.Reduce(1)))
                  Generator.PlaceTrap(FinalSquare, Revealed: false);
                break;

              case 5:
                // boulders.
                foreach (var FinalSquare in FinaleMap.GetSquares(FinalVaultRegion.Reduce(1)))
                  Generator.PlaceBoulder(FinalSquare, FinaleBlock, IsRigid: false);
                break;

              case 6:
                // TODO: attach punishment to the prisoner (ball&chain).

                // prison.
                var PrisonSquare = FinaleMap.GetSquares(FinalVaultRegion.Reduce(1)).ToArray().GetRandom();
                Generator.PlaceRandomCharacter(PrisonSquare, FinaleMap.Difficulty, FinaleMap.Difficulty + 5);
                break;

              default:
                Debug.Fail("Final vault not handled.");
                break;
            }
          }
        }
      }

      // automatically add all the other zones.
      Generator.RepairZones(FinaleMap, FinaleMap.Region);

      // big final boss is around here somewhere!
      var UniqueEntityList = Codex.Entities.List.Where(E => E.IsUnique).ToDistinctList();

#if DEBUG
      //foreach (var Unique in UniqueEntityList.OrderBy(E => E.Difficulty))
      //  Debug.WriteLine(Unique.Difficulty.ToString("00") + " " + Unique.Name);
#endif

      var UniqueEntity = UniqueEntityList.GetRandomOrNull();
      if (UniqueEntity != null)
      {
        var StartSquareArray = FinaleStructure.Rooms.Where(R => !R.Isolated).SelectMany(R => R.GetFloorSquares()).ToArray();

        var UniqueSquare = FinaleMap.GetSquares().Where(S => Generator.CanPlaceCharacter(S) && !StartSquareArray.Contains(S)).GetRandomOrNull();
        if (UniqueSquare != null)
        {
          Debug.Assert(FinaleLevel.Index == AbyssIndex);

          if (!CreateAbyssBranch(UniqueSquare))
            Debug.WriteLine("Abyss not generated.");

          Generator.PlaceSpecificCharacter(UniqueSquare, UniqueEntity);

          var UniqueCharacter = UniqueSquare.Character;
          if (UniqueCharacter != null)
          {
            Generator.HostileCharacter(UniqueCharacter); // some of the uniques are now generated as friendly.

            if (UniqueEntity.Level <= 40)
            {
              // TODO: this is meant to be a static but randomish level boost (However, string.GetHashCode() is not actually deterministic across .NET versions and x86/x64!).
              var LevelBoost = Math.Abs(UniqueEntity.Name.GetHashCode() % 10) + (UniqueEntity.Level / 10) + (UniqueEntity.Level % 10);

              if (UniqueEntity.Level < 10)
                LevelBoost += 40;
              else if (UniqueEntity.Level < 20)
                LevelBoost += 30;
              else if (UniqueEntity.Level < 30)
                LevelBoost += 20;
              else if (UniqueEntity.Level <= 40)
                LevelBoost += 10;

              Generator.PromoteCharacter(UniqueCharacter, LevelBoost);
            }

            Debug.WriteLine($"Boss: {UniqueEntity.Name} (level +{UniqueCharacter.Level - UniqueEntity.Level})");

            var Script = UniqueCharacter.InsertScript();
            Script.Killed.Sequence.Add(Codex.Tricks.warping).SetTarget(null);

            // has the endgame letter.
            var LetterAsset = Generator.NewSpecificAsset(UniqueSquare, Codex.Items.Stamped_Letter);
            Generator.InscribeAsset(LetterAsset, 
              NethackTerms.Congratulations_on_making_it_to_the_final_level_and_defeating_your_nemesis + "|" + 
              NethackTerms.To_complete_this_game_you_need_to_escape_the_dungeon_on_the_first_level);
            UniqueCharacter.Inventory.Carried.Add(LetterAsset);
          }
          else
          {
            Debug.Fail("Unique entity was not created!");
          }
        }
      }

      CreateMazeDetails(FinaleStructure, FinaleMap, FinaleBlock, FinaleMap.Region, 8);

      Generator.RepairVoid(FinaleMap, FinaleMap.Region);
      Generator.RepairWalls(FinaleMap, FinaleMap.Region);

      return FinaleStructure;
    }
    private void CreateShrineRoom(Zone ShrineZone, DungeonRoom ShrineRoom, Shrine Shrine)
    {
      var SelectShrine = Shrine ?? ShrineProbability.GetRandomOrNull();

      if (SelectShrine == null)
      {
        Debug.Fail("Why no shrine?");
      }
      else
      {
        var ShrineSquare = ShrineRoom.Midpoint;
        if (ShrineSquare.IsObstructed())
          ShrineSquare = ShrineRoom.GetFloorSquares().Where(Generator.CanPlaceFeature).GetRandomOrNull();

        if (ShrineSquare != null)
        {
          DebugWrite("; shrine(" + SelectShrine.Name + ")");

          Generator.PlaceShrine(ShrineSquare, SelectShrine);

          ShrineZone.InsertTrigger().Add(Delay.Zero, Codex.Tricks.VisitShrineArray[SelectShrine.Index]).SetTarget(ShrineSquare);
        }
      }
    }
    private void CreateShopRoom(Zone ShopZone, DungeonRoom ShopRoom, Shop Shop)
    {
      Debug.Assert(ShopZone != null);

      var ShopWidth = ShopRoom.Region.Width;
      var ShopHeight = ShopRoom.Region.Height;

      var ShopItems = 1.d(Math.Max(ShopWidth, ShopHeight)) + 8;

      var ShopList = new List<Shop>();
      var ShopSquareList = new Inv.DistinctList<Square>();

      if (Shop == null && ShopWidth == 8 && ShopHeight == 8)
      {
        var TopLeftSquare = ShopRoom[2, 2];
        var TopRightSquare = ShopRoom[ShopWidth - 3, 2];
        var BottomLeftSquare = ShopRoom[2, ShopHeight - 3];
        var BottomRightSquare = ShopRoom[ShopWidth - 3, ShopHeight - 3];

        var BazaarProbability = ShopProbability.Clone();

        foreach (var ShopSquare in new[] { TopLeftSquare, TopRightSquare, BottomRightSquare, BottomLeftSquare })
        {
          if (Generator.CanPlaceFeature(ShopSquare))
          {
            var BazaarShop = BazaarProbability.RemoveRandomOrNull() ?? ShopProbability.GetRandomOrNull();
            if (BazaarShop != null)
            {
              ShopSquareList.Add(ShopSquare);

              ShopList.Add(BazaarShop);
              Generator.PlaceShop(ShopSquare, BazaarShop, ShopItems.Roll());
            }
          }
        }

        DebugWrite("; bazaar(" + ShopList.Select(S => S.Name).AsSeparatedText(", ") + ")");

        var ShopCharacterArray = ShopSquareList.Select(S => S.Character).ExceptNull().ToArray();

        if (ShopCharacterArray.Length > 0)
        {
          var Party = Generator.NewParty(Leader: null);
          foreach (var ShopCharacter in ShopCharacterArray)
            Party.AddAlly(ShopCharacter, Clock.Zero, Delay.Zero);
        }
      }
      else
      {
        var ShopSquare = ShopRoom.Midpoint;
        if (ShopSquare.IsObstructed())
          ShopSquare = ShopRoom.GetFloorSquares().Where(S => Generator.CanPlaceFeature(S)).ToArray().GetRandomOrNull();

        if (ShopSquare == null)
        {
          Debug.Fail("Why no shop?");
        }
        else
        {
          var SelectShop = Shop ?? ShopProbability.GetRandomOrNull();

          if (SelectShop == null)
          {
            Debug.Fail("Why no shop?");
          }
          else
          {
            ShopSquareList.Add(ShopSquare);

            ShopList.Add(SelectShop);

            DebugWrite("; shop(" + SelectShop.Name + ")");

            Generator.PlaceShop(ShopSquare, SelectShop, ShopItems.Roll());
          }
        }
      }

      if (ShopList.Count > 0)
      {
        var Trigger = ShopZone.InsertTrigger();
        if (ShopList.Count == 1 && ShopSquareList.Count == 1)
          Trigger.Add(Delay.Zero, Codex.Tricks.VisitShopArray[ShopList[0].Index]).SetTarget(ShopSquareList[0]);
        else
          Trigger.Add(Delay.Zero, Codex.Tricks.visited_bazaar);
      }

      var ServantProbability = ShopList.SelectMany(S => S.ServantEntities).Distinct().Where(E => E.Difficulty <= ShopRoom.Map.Difficulty).ToProbability(E => E.Frequency);

      if (ServantProbability.Checks.Count > 0)
      {
        var ServantCount = 1.d(Math.Max(ShopWidth, ShopHeight) / 2).Roll();

        foreach (var ServantNumber in ServantCount.NumberSeries())
        {
          var ServantSquare = ShopRoom.GetFloorSquares().Where(Generator.CanPlaceCharacter).GetRandomOrNull();
          if (ServantSquare != null)
            Generator.PlaceSpecificCharacter(ServantSquare, ServantProbability.GetRandomOrNull());
        }
      }
    }
    private void CreateTrickRoom(Chance TrickChance, Zone TrickZone, DungeonRoom TrickRoom)
    {
#if DEBUG
      //TrickChance = Chance.Always;
#endif
      if (!TrickChance.Hit())
        return;

      var Trigger = TrickZone.InsertTrigger();

      // 1. Hole of bats
      // 2. pentagram of demons
      // 3. grave of undead
      // 4. pool of blobs
      // 5. living statue 
      // 6. lock 'n horde
      // 7. gas leak
      // 8. escaping mummies
      // 9. animate objects
      // TODO: 
      // * alcoves
      // * quadrant room
      // * function room (four features)
      // * pool room
      // * illusionary walls closing in
      // * earthquake holes
      // * leprechaun ambush
      // * physical walls prison.
      // * exploding barrels.
      // * rolling boulders.
      // * convert into hell room.
      // * prison.

      var FloorSquareList = TrickRoom.GetFloorSquares().ToDistinctList();

      var SummonDelay = 6.d10() + 30; // 31..90

      var TrickRoll = 1.d10().Roll();
#if DEBUG
      //TrickRoll = 5;
#endif

      switch (TrickRoll)
      {
        case 1:
          var HoleSquare = TrickRoom.Midpoint;
          if (!Generator.CanPlaceTrap(HoleSquare))
            HoleSquare = FloorSquareList.Where(Generator.CanPlaceTrap).GetRandomOrNull();

          if (HoleSquare != null)
          {
            Generator.PlaceTrap(HoleSquare, Codex.Devices.hole, Revealed: false);
            var HoleTrap = HoleSquare.Trap;
            if (HoleTrap != null && HoleTrap.Device == Codex.Devices.hole)
            {
              Generator.RevealTrap(HoleSquare);

              foreach (var FloorSquare in FloorSquareList)
                Trigger.Add(Delay.FromTurns(SummonDelay.Roll()), Codex.Tricks.arriving_bats).SetTarget(HoleSquare);
            }
          }
          break;

        case 2:
          var PentagramSquare = TrickRoom.Midpoint;
          if (!Generator.CanPlaceFeature(PentagramSquare))
            PentagramSquare = FloorSquareList.Where(Generator.CanPlaceFeature).GetRandomOrNull();

          if (PentagramSquare != null)
          {
            Generator.PlaceFixture(PentagramSquare, Codex.Features.pentagram);

            foreach (var FloorSquare in FloorSquareList)
              Trigger.Add(Delay.FromTurns(SummonDelay.Roll()), Codex.Tricks.summoning_demons).SetTarget(PentagramSquare);
          }
          break;

        case 3:
          var GraveSquare = TrickRoom.Midpoint;
          if (!Generator.CanPlaceFeature(GraveSquare))
            GraveSquare = FloorSquareList.Where(Generator.CanPlaceFeature).GetRandomOrNull();

          if (GraveSquare != null)
          {
            Generator.PlaceFixture(GraveSquare, Codex.Features.grave);

            foreach (var FloorSquare in FloorSquareList)
              Trigger.Add(Delay.FromTurns(SummonDelay.Roll()), Codex.Tricks.returning_undead).SetTarget(GraveSquare);
          }
          break;

        case 4:
          var PoolSquare = TrickRoom.Midpoint;
          if (!Generator.CanPlaceTrap(PoolSquare))
            PoolSquare = FloorSquareList.Where(Generator.CanPlaceTrap).GetRandomOrNull();

          if (PoolSquare != null)
          {
            Generator.PlaceTrap(PoolSquare, Codex.Devices.noxious_pool, Revealed: true, Triggered: FloorSquareList.Count);

            foreach (var FloorSquare in FloorSquareList)
              Trigger.Add(Delay.FromTurns(SummonDelay.Roll()), Codex.Tricks.emerging_blobs).SetTarget(PoolSquare);
          }
          break;

        case 5:
          var StatueSquare = TrickRoom.Midpoint;
          if (!Generator.CanPlaceBoulder(StatueSquare))
            StatueSquare = FloorSquareList.Where(Generator.CanPlaceBoulder).GetRandomOrNull();

          if (StatueSquare != null)
          {
            Trigger.Add(Delay.FromTurns(SummonDelay.Roll()), Codex.Tricks.living_statue).SetTarget(StatueSquare);
            Generator.PlaceBoulder(StatueSquare, Codex.Blocks.statue, IsRigid: true);
          }
          break;

        case 6:
          var HordeSquare = TrickRoom.Midpoint;
          Trigger.Add(Delay.FromTurns(SummonDelay.Roll()), Codex.Tricks.automatic_locking);
          Trigger.Add(Delay.FromTurns(SummonDelay.Roll()), Codex.Tricks.surrounding_horde).SetTarget(HordeSquare);
          break;

        case 7:
          var LeakSquare = TrickRoom.Midpoint;
          if (!Generator.CanPlaceTrap(LeakSquare))
            LeakSquare = FloorSquareList.Where(Generator.CanPlaceTrap).GetRandomOrNull();

          if (LeakSquare != null)
          {
            Generator.PlaceTrap(LeakSquare, Codex.Devices.sleeping_gas_trap, Revealed: true, Triggered: FloorSquareList.Count);

            foreach (var FloorSquare in FloorSquareList)
              Trigger.Add(Delay.FromTurns(SummonDelay.Roll()), Codex.Tricks.leaking_gas).SetTarget(LeakSquare);
          }
          break;

        case 8:
          var SarcophagusSquare = TrickRoom.Midpoint;
          if (!Generator.CanPlaceFeature(SarcophagusSquare))
            SarcophagusSquare = FloorSquareList.Where(Generator.CanPlaceFeature).GetRandomOrNull();

          if (SarcophagusSquare != null)
          {
            Generator.PlaceFixture(SarcophagusSquare, Codex.Features.sarcophagus);

            foreach (var FloorSquare in FloorSquareList)
              Trigger.Add(Delay.FromTurns(SummonDelay.Roll()), Codex.Tricks.escaping_mummies).SetTarget(SarcophagusSquare);
          }
          break;

        case 9:
          // NOTE: this is now fair, because the animation squares are converted to marble, with a delay, before the square is animated.
          var AnimateMaximum = TrickZone.Map.Difficulty;
          var AnimateCount = 0;
          do
          {
            var AnimateSquare = FloorSquareList.GetRandomOrNull();
            if (AnimateSquare != null && !AnimateSquare.HasAssets())
            {
              FloorSquareList.Remove(AnimateSquare);

              Generator.PlaceRandomAsset(AnimateSquare);

              if (AnimateSquare.HasAssets())
              {
                foreach (var AnimateAsset in AnimateSquare.GetAssets())
                {
                  if (AnimateAsset.HasSanctity)
                    Generator.ChangeSanctity(AnimateAsset, Codex.Sanctities.Blessed); // so it becomes uncursed when killed.
                }

                Trigger.Add(Delay.Zero, Codex.Tricks.marble_paving).SetTarget(AnimateSquare);
                Trigger.Add(Delay.FromTurns(SummonDelay.Roll()), Codex.Tricks.animated_objects).SetTarget(AnimateSquare);
              }
            }

            AnimateCount++;
          }
          while (Chance.OneIn3.Hit() && AnimateCount < AnimateMaximum); // cap to difficulty of items in the room (very unlikely).
          break;

        case 10:
          var GreaseSquare = TrickRoom.Midpoint;
          if (!Generator.CanPlaceTrap(GreaseSquare))
            GreaseSquare = FloorSquareList.Where(Generator.CanPlaceTrap).GetRandomOrNull();

          if (GreaseSquare != null)
          {
            Generator.PlaceTrap(GreaseSquare, Codex.Devices.grease_trap, Revealed: true, Triggered: FloorSquareList.Count);

            foreach (var FloorSquare in FloorSquareList)
              Trigger.Add(Delay.FromTurns(SummonDelay.Roll()), Codex.Tricks.scuttling_insects).SetTarget(GreaseSquare);
          }
          break;

        default:
          Debug.Fail("Failed to create a trick room.");
          break;
      }
    }
    private void CreateZooRoom(Zone ZooZone, DungeonRoom ZooRoom, Zoo Zoo)
    {
      var SelectZoo = Zoo ?? GetZooProbability(ZooRoom.Map).GetRandomOrNull();

#if DEBUG
      //SelectZoo = Codex.Zoos.spider_nest;
#endif

      if (SelectZoo == null)
      {
        Debug.Fail("Why no zoo?");
      }
      else
      {
        DebugWrite("; zoo(" + SelectZoo.Name + ")");

        ZooZone.InsertTrigger().Add(Delay.Zero, Codex.Tricks.VisitZooArray[SelectZoo.Index]);

        Generator.PlaceZoo(ZooZone.Squares, SelectZoo, Generator.MinimumDifficulty(ZooRoom.Map), Generator.MaximumDifficulty(ZooRoom.Map));
      }
    }
    private void CreateAtticRoom(DungeonRoom SourceRoom, Barrier SourceBarrier, Ground SourceGround)
    {
      var SourceMap = SourceRoom.Map;

      var AtticMapName = SourceMap.Name + " " + Generator.EscapedModuleTerm(NethackTerms.attic);
      if (Generator.Adventure.World.HasMap(AtticMapName))
        return;

      var SourceSquare = SourceRoom.GetFloorSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();

      if (SourceSquare == null)
      {
        Debug.Fail("Why no attic?");
      }
      else
      {
        DebugWrite("; attic");

        var AtticTemplate = LoadAtticTemplateList().GetRandomOrNull();

        var AtticGrid = AtticTemplate?.Grid;

        if (AtticGrid != null)
        {
          // rotate 0-3 times (0/90/180/270 degrees).
          var RotateTimes = (1.d4() - 1).Roll();

          if (RotateTimes > 0)
          {
            AtticGrid = AtticGrid.Copy();
            for (var Index = 0; Index < RotateTimes; Index++)
              AtticGrid.Rotate90Degrees();
          }
        }

        var AtticWidth = AtticGrid?.Width ?? 10;
        var AtticHeight = AtticGrid?.Height ?? 10;

        var AtticMap = Generator.Adventure.World.AddMap(AtticMapName, AtticWidth, AtticHeight);
        AtticMap.SetLevel(SourceMap.Level);
        AtticMap.SetDifficulty(SourceMap.Difficulty); // match difficulty to source map.
        AtticMap.SetAtmosphere(Codex.Atmospheres.dungeon);

        var AtticStructure = new DungeonStructure(AtticMap);

        var StorageRoom = AtticStructure.AddRoom(AtticMap.Region, Isolated: false);

        if (AtticTemplate == null)
        {
          // NOTE: this can only happen if there is something wrong with the Attics specials.
          PlaceRoom(StorageRoom, SourceBarrier, SourceGround);
        }
        else
        {
          for (var Column = 0; Column < AtticGrid.Width; Column++)
          {
            for (var Row = 0; Row < AtticGrid.Height; Row++)
            {
              var AtticSymbol = AtticGrid[Column, Row];
              var AtticSquare = AtticMap[Column, Row];

              switch (AtticSymbol)
              {
                case '#':
                  Generator.PlaceWall(AtticSquare, SourceBarrier, WallStructure.Solid, WallSegment.Cross);
                  break;

                case '.':
                  Generator.PlaceFloor(AtticSquare, SourceGround);
                  break;

                case ' ':
                  // void space.
                  break;

                default:
                  Debug.Fail($"Attic symbol not handled: {AtticSymbol} {(int)AtticSymbol}");

                  Generator.PlaceFloor(AtticSquare, SourceGround);
                  break;
              }
            }
          }

          Generator.RepairWalls(AtticMap, AtticMap.Region);
        }

        // generate the zones.
        Generator.RepairZones(AtticMap, AtticMap.Region);

        // even lighting for all zones in the attic.
        var AtticIsLit = Generator.RandomRoomIsLit(AtticMap);
        foreach (var StorageZone in AtticMap.Zones)
          StorageZone.SetLit(AtticIsLit);

        var StorageSquare = AtticMap.GetSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();

        if (StorageSquare != null)
        {
          Generator.PlacePassage(SourceSquare, Codex.Portals.wooden_ladder_up, StorageSquare);
          Generator.PlacePassage(StorageSquare, Codex.Portals.wooden_ladder_down, SourceSquare);
        }

        if (SourceRoom.Map.Level.Index != TowerIndex || !CreateLichTowerBranch(StorageRoom)) // TODO: don't generate any entities/items on the Lich Portal attic?
        {
          // TODO: the size of the _Source_ room dictates how many entities/items (not the size of the attic).
          var AtticDice = 1.d(SourceRoom.Region.Width) + SourceRoom.Region.Height;

          foreach (var Index in AtticDice.Roll().NumberSeries())
          {
            var AssetSquare = AtticMap.GetSquares().Where(Generator.CanPlaceAsset).GetRandomOrNull();
            if (AssetSquare != null)
              Generator.PlaceRandomAsset(AssetSquare);

            var CharacterSquare = AtticMap.GetSquares().Where(Generator.CanPlaceCharacter).GetRandomOrNull();
            if (CharacterSquare != null)
              Generator.PlaceRandomCharacter(CharacterSquare);
          }
        }
      }
    }
    private void CreateVaultRoom(Zone VaultZone, DungeonRoom VaultRoom)
    {
      var VaultTrigger = VaultZone.InsertTrigger();

      var VaultDice = 3.d20() + 20;

      foreach (var VaultSquare in VaultRoom.GetFloorSquares().Where(Generator.CanPlaceAsset))
      {
        DropVaultCoins(VaultSquare);

        VaultTrigger.Add(Delay.FromTurns(VaultDice.Roll()), Codex.Tricks.calling_guard).SetTarget(VaultSquare);
      }
    }
    private void CreatePrisonRoom(Zone PrisonZone, DungeonRoom PrisonRoom)
    {
      if (PrisonRoom.Region.Width < 7 || PrisonRoom.Region.Height < 7)
      {
        // too small, create a trap room instead.
        foreach (var TrapSquare in PrisonRoom.GetFloorSquares().Where(Generator.CanPlaceTrap))
        {
          if (Chance.OneIn2.Hit())
            Generator.PlaceTrap(TrapSquare, Revealed: false);

          if (Chance.OneIn5.Hit())
            Generator.PlaceRandomAsset(TrapSquare);
          else
            Generator.DropCoins(TrapSquare, Generator.RandomCoinQuantity(TrapSquare));
        }
      }
      else
      {
        var PrisonBarrier = Codex.Barriers.hell_brick;
        var IronBars = Codex.Barriers.iron_bars;

        var MiddleX = PrisonRoom.Region.Width / 2;
        var MiddleY = PrisonRoom.Region.Height / 2;

        Generator.PlaceWall(PrisonRoom[MiddleX - 1, MiddleY - 1], PrisonBarrier, WallStructure.Permanent, WallSegment.TopLeftCorner);
        Generator.PlaceWall(PrisonRoom[MiddleX, MiddleY - 1], IronBars, WallStructure.Solid, WallSegment.Horizontal);
        Generator.PlaceWall(PrisonRoom[MiddleX + 1, MiddleY - 1], PrisonBarrier, WallStructure.Permanent, WallSegment.TopRightCorner);
        Generator.PlaceWall(PrisonRoom[MiddleX - 1, MiddleY], IronBars, WallStructure.Solid, WallSegment.Vertical);
        Generator.PlaceWall(PrisonRoom[MiddleX - 1, MiddleY + 1], PrisonBarrier, WallStructure.Permanent, WallSegment.BottomLeftCorner);
        Generator.PlaceWall(PrisonRoom[MiddleX, MiddleY + 1], IronBars, WallStructure.Solid, WallSegment.Horizontal);
        Generator.PlaceWall(PrisonRoom[MiddleX + 1, MiddleY + 1], PrisonBarrier, WallStructure.Permanent, WallSegment.BottomRightCorner);
        Generator.PlaceWall(PrisonRoom[MiddleX + 1, MiddleY], IronBars, WallStructure.Solid, WallSegment.Vertical);

        foreach (var CellSquare in PrisonRoom.GetFloorSquares().Where(S => S.Wall == null))
          Generator.PlaceFloor(CellSquare, Codex.Grounds.obsidian_floor);

        var PrisonSquare = PrisonRoom[MiddleX, MiddleY];

        var MercenaryProbability = Codex.Entities.List.Where(E => E.IsMercenary && E.IsEncounter).ToProbability(E => E.Frequency);

        var MercenaryEntity = MercenaryProbability.GetRandomOrNull();

        if (MercenaryEntity != null)
        {
          Generator.PlaceSpecificCharacter(PrisonSquare, MercenaryEntity);

          var PrisonCharacter = PrisonSquare.Character;

          if (PrisonCharacter != null)
          {
            // prisoner is asleep.
            SnoozeCharacter(PrisonCharacter);

            // prisoners have their spell mastery erased.
            if (PrisonCharacter.Knowledge.HasMasteries())
            {
              foreach (var Mastery in PrisonCharacter.Knowledge.GetMasteries())
                PrisonCharacter.Knowledge.ForgetSpell(Mastery.Spell);
            }

            var ChestSquare = PrisonRoom.GetPerimeterSquares().Where(S => !S.IsObstructed()).GetRandomOrNull() ?? PrisonSquare;

            var ContainerAsset = CreateContainer(ChestSquare, Locked: true, Trapped: true);
            Generator.PlaceAsset(ChestSquare, ContainerAsset);

            foreach (var Asset in PrisonCharacter.Inventory.RemoveAllAssets())
            {
              if (Asset.Container != null)
                Generator.PlaceAsset(ChestSquare, Asset);
              else
                ContainerAsset.Container.Stash.Add(Asset);
            }

            var PrisonDice = 3.d20() + 20;

            foreach (var AlarmSquare in PrisonRoom.GetPerimeterSquares().Where(S => !S.IsObstructed()))
              AlarmSquare.InsertTrigger().Add(Delay.FromTurns(PrisonDice.Roll()), Codex.Tricks.calling_guard).SetTarget(AlarmSquare);
          }
        }
      }
    }
    private void CreateCellarNook(Square SourceSquare, Barrier SourceBarrier, Ground SourceGround)
    {
      var SourceMap = SourceSquare.Map;

      var CellarMapName = SourceMap.Name + " " + Generator.EscapedModuleTerm(NethackTerms.cellar);
      if (Generator.Adventure.World.HasMap(CellarMapName))
        return;

      var CellarDice = 1.d4() + 4; // 5..8
#if DEBUG
      //CellarDice = Dice.Fixed(5);
#endif

      var CellarWidth = CellarDice.Roll();  
      var CellarHeight = CellarDice.Roll();

      var CellarMap = Generator.Adventure.World.AddMap(CellarMapName, CellarWidth, CellarHeight);
      CellarMap.SetLevel(SourceMap.Level);
      CellarMap.SetDifficulty(SourceMap.Difficulty);
      CellarMap.SetAtmosphere(Codex.Atmospheres.dungeon);

      var CellarStructure = new DungeonStructure(CellarMap);

      var CellarZone = CellarMap.AddZone();
      CellarZone.AddRegion(CellarMap.Region);
      CellarZone.SetLit(Generator.RandomRoomIsLit(CellarMap));

      var CellarRoom = CellarStructure.AddRoom(CellarMap.Region, Isolated: false);
      PlaceRoom(CellarRoom, SourceBarrier, SourceGround);

      var CellarSquare = CellarRoom.Midpoint; // ladder in the midpoint always for the cellar.

      Generator.PlacePassage(SourceSquare, Codex.Portals.wooden_ladder_down, CellarSquare);
      Generator.PlacePassage(CellarSquare, Codex.Portals.wooden_ladder_up, SourceSquare);

      var CellarOption = 1.d5().Roll();
#if DEBUG
      //CellarOption = 4;
#endif

      switch (CellarOption)
      {
        case 1:
          // wine cellar.
          var PotionDice = 1.d(CellarWidth) + (CellarHeight / 2);
          var PotionProbability = Codex.Items.List.Where(I => I.Type == ItemType.Potion && !I.Grade.Unique).ToProbability(P => P.Rarity);

          foreach (var Index in PotionDice.Roll().NumberSeries())
          {
            var AssetSquare = CellarRoom.GetFloorSquares().Where(Generator.CanPlaceAsset).GetRandomOrNull();
            if (AssetSquare != null)
              Generator.PlaceSpecificAsset(AssetSquare, PotionProbability.GetRandom());
          }

          var BossSquare = CellarRoom.GetFloorSquares().Where(Generator.CanPlaceCharacter).GetRandomOrNull();
          if (BossSquare != null)
          {
            Generator.PlaceRandomCharacter(BossSquare, BossSquare.Map.Difficulty + 1, BossSquare.Map.Difficulty + 3);
            if (BossSquare.Character != null)
              SnoozeCharacter(BossSquare.Character);
          }
          break;

        case 2:
          // flooded/frozen cellar.
          var IsFrozen = Chance.OneIn10.Hit();
          var FloodedGround = IsFrozen ? Codex.Grounds.ice : Codex.Grounds.water;

          foreach (var Square in CellarRoom.GetFloorSquares())
          {
            Generator.PlaceFloor(Square, FloodedGround);

            if (Generator.CanPlaceCharacter(Square) && Chance.OneIn2.Hit())
              Generator.PlaceRandomCharacter(Square); // marine/ice.

            if (!IsFrozen && Generator.CanPlaceAsset(Square) && Chance.OneIn5.Hit())
              Generator.PlaceSpecificAsset(Square, Codex.Items.kelp_frond);
          }
          break;

        case 3:
          // barrel cellar.
          foreach (var Square in CellarRoom.GetPerimeterSquares())
          {
            if (Generator.CanPlaceBoulder(Square) && Chance.ThreeIn4.Hit())
              Generator.PlaceBoulder(Square, Codex.Blocks.wooden_barrel, IsRigid: false);
            else if (Generator.CanPlaceCharacter(Square))
              Generator.PlaceRandomCharacter(Square);
          }
          break;

        case 4:
          // crypt cellar.
          foreach (var Square in CellarRoom.GetPerimeterSquares())
          {
            if (Generator.CanPlaceBoulder(Square) && Chance.ThreeIn4.Hit())
              Generator.PlaceFixture(Square, Codex.Features.sarcophagus);
          }
          break;

        default:
          CreateRoomDetails(CellarStructure, CellarRoom.Map, Codex.Blocks.stone_boulder, CellarRoom.GetFloorSquares().ToDistinctList());
          break;
      }
    }
    private void CreateCavesNook(Square SourceSquare)
    {
      var CavernBlock = Codex.Blocks.clay_boulder;

      var SourceMap = SourceSquare.Map;

      var CavesMapName = SourceMap.Name + " " + Generator.EscapedModuleTerm(NethackTerms.caves);
      if (Generator.Adventure.World.HasMap(CavesMapName))
        return;

      var CavesMap = Generator.Adventure.World.AddMap(CavesMapName, CavernSize * 1.d2().Roll(), CavernSize * 1.d2().Roll());
      CavesMap.SetLevel(SourceMap.Level);
      CavesMap.SetDifficulty(SourceMap.Difficulty);
      CavesMap.SetAtmosphere(Codex.Atmospheres.cavern);

      var CavesStructure = CreateCavern(CavesMap, CavesMap.Region);

      Generator.RepairVoid(CavesMap, CavesMap.Region);
      Generator.RepairWalls(CavesMap, CavesMap.Region);

      var AccessSquare = CavesMap.GetSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();

      if (AccessSquare != null)
      {
        Generator.PlacePassage(SourceSquare, Codex.Portals.wooden_ladder_down, AccessSquare);
        Generator.PlacePassage(AccessSquare, Codex.Portals.wooden_ladder_up, SourceSquare);
      }

      foreach (var CavernRoom in CavesStructure.Rooms.Where(R => !R.Isolated))
        CreateRoomDetails(CavesStructure, CavesMap, CavernBlock, CavernRoom.GetFloorSquares().ToDistinctList());
    }
    private void CreateTroveNook(Square SourceSquare, Barrier SourceBarrier, Ground SourceGround)
    {
      var SourceMap = SourceSquare.Map;

      var TroveMapName = SourceMap.Name + " " + Generator.EscapedModuleTerm(NethackTerms.trove);
      if (Generator.Adventure.World.HasMap(TroveMapName))
        return;

      var TroveMap = Generator.Adventure.World.AddMap(TroveMapName, 5, 5);
      TroveMap.SetLevel(SourceMap.Level);
      TroveMap.SetDifficulty(SourceMap.Difficulty);
      TroveMap.SetAtmosphere(Codex.Atmospheres.dungeon);

      var TroveStructure = new DungeonStructure(TroveMap);

      var TroveZone = TroveMap.AddZone();
      TroveZone.AddRegion(TroveMap.Region);
      TroveZone.SetLit(Generator.RandomRoomIsLit(TroveMap));

      var TroveRoom = TroveStructure.AddRoom(TroveMap.Region, Isolated: false);
      PlaceRoom(TroveRoom, SourceBarrier, SourceGround);

      var AccessSquare = TroveRoom.Midpoint;

      Generator.PlacePassage(SourceSquare, Codex.Portals.wooden_ladder_down, AccessSquare);
      Generator.PlacePassage(AccessSquare, Codex.Portals.wooden_ladder_up, SourceSquare);

      foreach (var Square in TroveRoom.GetFloorSquares())
      {
        if (Square != TroveRoom.Midpoint)
        {
          if (Chance.OneIn9.Hit())
            Generator.PlaceCharacter(Square, E => E.IsMimicking());

          if (Square.Character != null)
          {
            SnoozeCharacter(Square.Character); // sleeping mimic.
          }
          else
          {
            Generator.PlaceTrap(Square, Revealed: false);

            if (!Chance.OneIn3.Hit())
              PlaceContainer(Square, Locked: true, Trapped: true);
          }
        }
      }
    }
    private void JoinCorridor(Map Map, Ground Ground, DungeonCorridor StartCorridor, DungeonCorridor EndCorridor)
    {
      if (Inv.Assert.IsEnabled)
      {
        Inv.Assert.Check(StartCorridor.Room.Map == Map, "Corridor must start in the expected map.");
        Inv.Assert.Check(EndCorridor.Room.Map == Map, "Corridor must end in the expected map.");
      }

      var StartCorridorX = StartCorridor.Start.X;
      var StartCorridorY = StartCorridor.Start.Y;
      var EndCorridorX = EndCorridor.Start.X;
      var EndCorridorY = EndCorridor.Start.Y;

      if (StartCorridorX <= EndCorridorX)
        Generator.PlaceCorridor(Map, Ground, new Region(StartCorridorX, StartCorridorY, EndCorridorX, StartCorridorY));
      else if (StartCorridorX > EndCorridorX)
        Generator.PlaceCorridor(Map, Ground, new Region(EndCorridorX, StartCorridorY, StartCorridorX, StartCorridorY));

      if (StartCorridorY < EndCorridorY)
        Generator.PlaceCorridor(Map, Ground, new Region(EndCorridorX, StartCorridorY + 1, EndCorridorX, EndCorridorY));
      else if (StartCorridorY > EndCorridorY)
        Generator.PlaceCorridor(Map, Ground, new Region(EndCorridorX, EndCorridorY, EndCorridorX, StartCorridorY - 1));
    }
    private void DropVaultCoins(Square Square)
    {
      Generator.DropCoins(Square, Generator.RandomCoinQuantity(Square) * 1.d5().Roll());
    }
    private void CreateCastleNook(Square AccessSquare, Square NookSquare)
    {
      if (AccessSquare == null || NookSquare == null)
        return;

      Debug.Assert(AccessSquare != NookSquare);

      var AccessMap = AccessSquare.Map;

      var CastleGround = Codex.Grounds.stone_floor;
      var CastleBarrier = Codex.Barriers.stone_wall;
      var CastleGate = Codex.Gates.wooden_door;
      var CastleBed = Codex.Features.bed;

      // NOTE: this will not allow nooks right up against the edge of the map (the adjacent squares method will not return squares off the edge of the map).

      if (Chance.OneIn10.Hit() && NookSquare.GetAdjacentSquares().Where(S => S.Wall == null && S.Door == null && S.Floor == null).Count() >= 5)
      {
        var NookZone = AccessMap.AddZone();

        NookZone.AddSquare(NookSquare);
        NookZone.AddSquare(AccessSquare);

        var AccessGround = AccessSquare.Wall != null ? AccessSquare.Wall.Barrier.Underlay : CastleGround;

        Generator.PlaceFloor(NookSquare, AccessGround);

        // restricted means you cannot teleport into it.
        var NookRestricted = Chance.OneIn10.Hit();
        NookZone.SetAccessRestricted(NookRestricted);
        NookZone.SetSpawnRestricted(NookRestricted);

        if (NookRestricted)
        {
          Generator.PlaceRandomAsset(NookSquare); // occasionally no door, but a treasure is here.
        }
        else
        {
          Generator.PlaceFloor(AccessSquare, AccessGround);
          Generator.PlaceRandomDoor(AccessSquare, CastleGate, AccessSquare.AsOffset(NookSquare).X == 0 ? DoorOrientation.Horizontal : DoorOrientation.Vertical, CastleBarrier);
        }

#if DEBUG
        //if (Chance.Always.Hit()) CreateCaves(NookSquare); else
#endif
        // TODO:
        // * chess nook
        // * gem mining nook

        if (Chance.OneIn40.Hit())
          CreateTroveNook(NookSquare, CastleBarrier, CastleGround);
        else if (Chance.OneIn40.Hit())
          CreateCellarNook(NookSquare, CastleBarrier, CastleGround);
        else if (Chance.OneIn40.Hit())
          CreateCavesNook(NookSquare);
        else if (Chance.OneIn40.Hit())
          Generator.PlaceFixture(NookSquare, CastleBed);
        else if (Chance.OneIn7.Hit())
        {
          Generator.DropCoins(NookSquare, Generator.RandomCoinQuantity(NookSquare));
          if (Chance.OneIn3.Hit())
            Generator.PlaceTrap(NookSquare, Revealed: false);
        }
        else if (Chance.OneIn7.Hit())
          PlaceContainer(NookSquare);
        else if (Chance.OneIn7.Hit())
          Generator.PlaceTrap(NookSquare, Revealed: false);
        else if (Chance.OneIn7.Hit())
          Generator.PlaceRandomCharacter(NookSquare, AccessMap.Difficulty, AccessMap.Difficulty);
        else if (Chance.OneIn7.Hit())
        {
          Block NookBlock;

          if (Chance.OneIn16.Hit())
            NookBlock = Codex.Blocks.statue;
          else if (Chance.OneIn4.Hit())
            NookBlock = Codex.Blocks.wooden_barrel;
          else
            NookBlock = Codex.Blocks.stone_boulder;

          Generator.PlaceBoulder(NookSquare, NookBlock, IsRigid: false);
          Generator.PlaceRandomAsset(NookSquare);
        }

        // nooks are always secret if there is something good in them.
        if (AccessSquare.Door != null && NookSquare.HasAssets())
        {
          if (!AccessSquare.Door.IsClosed)
            Generator.CloseDoor(AccessSquare); // might already be closed/locked.

          Generator.ConcealDoor(AccessSquare);
        }

        //var RecurseDirection = AccessSquare.AsDirection(NookSquare);
        //var RecurseSquare = NookSquare.Adjacent(RecurseDirection);
        //if (RecurseSquare != null)
        //  CreateNook(RecurseSquare, RecurseSquare.Adjacent(RecurseDirection));
      }
    }
    private Square CreateMazeCorner(DungeonRoom MazeRoom, Barrier MazeBarrier, Gate MazeGate)
    {
      var MazeSquareArray = MazeRoom.GetFloorSquares().Where(M => Generator.CanPlaceCharacter(M) && M.GetAdjacentSquares().Count(S => S.Wall == null) == 1).ToArray();

      var MazeSquare = MazeSquareArray.GetRandomOrNull();

      void CollectSquare(Square EmptySquare)
      {
        // delete any traps.
        if (EmptySquare.Trap != null)
          Generator.RemoveTrap(EmptySquare);

        // delete any boulders (probably won't come up).
        if (EmptySquare.Boulder != null)
          Generator.RemoveBoulder(EmptySquare);

        // don't allow items to be placed inside the illusionary wall or locked door.
        if (EmptySquare.HasAssets())
          Generator.TransferAssets(MazeSquare, EmptySquare);
      }

      if (MazeSquare == null)
      {
        // no suitable 'prison' for the maze boss, find a square with the most adjacent walls.
        MazeSquare = MazeRoom.GetFloorSquares().Where(Generator.CanPlaceCharacter).OrderByDescending(M => M.GetAdjacentSquares().Count(S => S.Wall != null)).FirstOrDefault();

        if (MazeSquare != null)
        {
          foreach (var AdjacentSquare in MazeSquare.GetAdjacentSquares())
          {
            if (AdjacentSquare.Wall == null && AdjacentSquare.Door == null)
            {
              CollectSquare(AdjacentSquare);

              Generator.PlaceIllusionaryWall(AdjacentSquare, MazeBarrier, WallSegment.Cross);
            }
          }
        }
      }
      else
      {
        var EmptySquare = MazeSquare.GetAdjacentSquares().Find(S => S.Wall == null && S.Door == null);

        if (EmptySquare != null)
        {
          CollectSquare(EmptySquare);

          var DoorOffset = EmptySquare.AsOffset(MazeSquare);

          if (MazeGate == null)
          {
            // no doors in this plan.
            Generator.PlaceIllusionaryWall(EmptySquare, MazeBarrier, DoorOffset.Y == 0 ? WallSegment.Vertical : WallSegment.Horizontal);
          }
          else
          {
            Generator.PlaceRandomDoor(EmptySquare, MazeGate, DoorOffset.Y == 0 ? DoorOrientation.Vertical : DoorOrientation.Horizontal, MazeBarrier);

            // door might be trapped but we always want it locked and not secret.
            if (EmptySquare.Door != null)
            {
              Generator.LockDoor(EmptySquare);
              Generator.RevealDoor(EmptySquare);
            }
          }
        }
      }

      // force the corner square into different zone.
      if (MazeSquare != null)
        MazeRoom.Map.AddZone().ForceSquare(MazeSquare);

      return MazeSquare;
    }
    private void CreateMazeBoss(Square MazeSquare)
    {
      Generator.PlaceRandomAsset(MazeSquare);
      Generator.DropCoins(MazeSquare, Generator.RandomCoinQuantity(MazeSquare) * 1.d5().Roll());
      Generator.PlaceRandomCharacter(MazeSquare, MazeSquare.Map.Difficulty + 1, MazeSquare.Map.Difficulty + 3);
      if (MazeSquare.Character != null)
        SnoozeCharacter(MazeSquare.Character);
    }
    /// <summary>
    /// Mark the character as asleep.
    /// </summary>
    /// <param name="Character"></param>
    public void SnoozeCharacter(Character Character)
    {
      Generator.AcquireTalent(Character, Codex.Properties.sleeping);
    }
    private void PlaceRoom(DungeonRoom Room, Barrier Barrier, Ground Ground)
    {
      if (Inv.Assert.IsEnabled)
        Inv.Assert.CheckNotNull(Room, nameof(Room));

      Generator.PlaceRoom(Room.Map, Barrier, Ground, Room.Region);
    }
    private void PlacePillarRoom(DungeonRoom Room, Barrier Barrier, Ground Ground)
    {
      if (Inv.Assert.IsEnabled)
        Inv.Assert.CheckNotNull(Room, nameof(Room));

      var Map = Room.Map;
      var Region = Room.Region;

      Generator.PlaceSolidWall(Map[Region.Left, Region.Top], Barrier, WallSegment.Pillar);
      Generator.PlaceSolidWall(Map[Region.Right, Region.Top], Barrier, WallSegment.Pillar);
      Generator.PlaceSolidWall(Map[Region.Left, Region.Bottom], Barrier, WallSegment.Pillar);
      Generator.PlaceSolidWall(Map[Region.Right, Region.Bottom], Barrier, WallSegment.Pillar);

      for (var X = Region.Left + 1; X <= Region.Right - 1; X++)
      {
        Generator.PlaceFloor(Map[X, Region.Top], Ground);
        Generator.PlaceFloor(Map[X, Region.Bottom], Ground);
      }

      for (var Y = Region.Top + 1; Y <= Region.Bottom - 1; Y++)
      {
        Generator.PlaceFloor(Map[Region.Left, Y], Ground);
        Generator.PlaceFloor(Map[Region.Right, Y], Ground);
      }
    }
    private void PlaceIllusionRoom(DungeonRoom Room, Barrier Barrier, Ground Ground)
    {
      if (Inv.Assert.IsEnabled)
        Inv.Assert.CheckNotNull(Room, nameof(Room));

      var Map = Room.Map;
      var Region = Room.Region;

      Generator.PlaceIllusionaryWall(Map[Region.Left, Region.Top], Barrier, WallSegment.TopLeftCorner);
      Generator.PlaceIllusionaryWall(Map[Region.Right, Region.Top], Barrier, WallSegment.TopRightCorner);
      Generator.PlaceIllusionaryWall(Map[Region.Left, Region.Bottom], Barrier, WallSegment.BottomLeftCorner);
      Generator.PlaceIllusionaryWall(Map[Region.Right, Region.Bottom], Barrier, WallSegment.BottomRightCorner);

      for (var X = Region.Left + 1; X <= Region.Right - 1; X++)
      {
        Generator.PlaceIllusionaryWall(Map[X, Region.Top], Barrier, WallSegment.Horizontal);
        Generator.PlaceIllusionaryWall(Map[X, Region.Bottom], Barrier, WallSegment.Horizontal);
      }

      for (var Y = Region.Top + 1; Y <= Region.Bottom - 1; Y++)
      {
        Generator.PlaceIllusionaryWall(Map[Region.Left, Y], Barrier, WallSegment.Vertical);
        Generator.PlaceIllusionaryWall(Map[Region.Right, Y], Barrier, WallSegment.Vertical);
      }

      Generator.PlaceFloorFill(Map, Ground, Region);
    }
    private void CreateRoomDetails(DungeonStructure Structure, Map Map, Block Block, IReadOnlyList<Square> FloorSquareList)
    {
      var DetailSquareList = FloorSquareList.Where(Generator.CanPlaceTrap).ToDistinctList();

      // traps.
      Generator.PlaceRoomTraps(DetailSquareList, Map.Difficulty);

      // coins.
      Generator.PlaceRoomCoins(DetailSquareList);

      // estimated number of containers per area.
      var ContainerIndex = Structure.Rooms.Count;

      if (ContainerIndex == 0)
        ContainerIndex = Map.Zones.Count;

      if (ContainerIndex == 0)
        ContainerIndex = Map.Region.Width * Map.Region.Height / 100;

      if (ContainerIndex == 0)
        ContainerIndex = 1;

      if (Chance.OneIn(ContainerIndex * 5 / 2).Hit())
      {
        // 40% chance for at least 1 box, regardless of number of rooms; about 5 - 7.5% for 2 boxes, least likely when few rooms; chance for 3 or more is negligible.

        var Square = DetailSquareList.GetRandomOrNull();
        if (Square != null)
          PlaceContainer(Square);
      }

      Generator.PlaceRoomAssets(DetailSquareList);

      Generator.PlaceRoomHorde(FloorSquareList);

      Generator.PlaceRoomCharacter(FloorSquareList);

      if (Block != null && Chance.OneIn25.Hit())
      {
        var Square = FloorSquareList.Where(Generator.CanPlaceBoulder).GetRandomOrNull();

        // NOTE: don't place boulders in front of doors to ensure we don't generate maps that are unplayable.
        if (Square != null && !Square.GetCompassSquares().Any(S => S.Door != null))
        {
          if (Chance.OneIn12.Hit())
          {
            Generator.PlaceBoulder(Square, Codex.Blocks.statue, IsRigid: false);
          }
          else if (Chance.OneIn3.Hit())
          {
            Generator.PlaceBoulder(Square, Codex.Blocks.wooden_barrel, IsRigid: false);
          }
          else
          {
            Generator.PlaceBoulder(Square, Block, IsRigid: false);

            // any items under the boulder?
            if (!Square.HasAssets())
            {
              if (Square.GetAdjacentSquares().Count(S => S.Wall != null || S.Floor == null) >= 5)
              {
                // blocked boulders are more likely to have items under them.
                if (Chance.OneIn2.Hit())
                  Generator.PlaceRandomAsset(Square);
              }
              else if (Chance.OneIn5.Hit())
              {
                Generator.PlaceRandomAsset(Square);
              }
            }
          }
        }
      }
    }
    private Asset CreateContainer(Square Square, bool Locked, bool Trapped)
    {
      var Item = Generator.ContainerItems.GetRandom();

      var Result = Generator.NewSpecificAsset(Square, Item);
      Result.Container.Locked = Item.Storage.Locking && Locked;
      Result.Container.Trap = Item.Storage.Trapping && Trapped ? Generator.NewTrap(Generator.RandomContainerDevice(Square), Revealed: false) : null;
      return Result;
    }
    private void PlaceContainer(Square Square, bool Locked, bool Trapped)
    {
      var ContainerAsset = CreateContainer(Square, Locked, Trapped);
      Generator.PlaceAsset(Square, ContainerAsset);

      StockContainer(Square, ContainerAsset, Locked, Trapped);
    }
    private void PlaceContainer(Square Square)
    {
      var Locked = !Chance.OneIn5.Hit(); // 20% chance of being unlocked.
      var Trapped = Locked && Chance.OneIn3.Hit(); // 33% chance of being trapped.

      PlaceContainer(Square, Locked, Trapped);
    }
    private void StockContainer(Square Square, Asset Asset, bool Locked, bool Trapped)
    {
      Asset.Container.Locked = Asset.Item.Storage.Locking && Locked;
      Asset.Container.Trap = Asset.Item.Storage.Trapping && Trapped ? Generator.NewTrap(Generator.RandomContainerDevice(Square), Revealed: false) : null;
      Generator.StockContainer(Square, Asset);
    }
    private void CreateMazePaths(Map Map, Region Region)
    {
      var SquareStack = new Stack<Square>();

      var StartColumn = (1.d((Region.Width - 1) / 2).Roll() - 1) * 2 + 1;
      var StartRow = (1.d((Region.Height - 1) / 2).Roll() - 1) * 2 + 1;

      var NextSquare = Map[Region.Left + StartColumn, Region.Top + StartRow];
      SquareStack.Push(NextSquare);

      while (SquareStack.Count > 0)
      {
        var NeighbourArray = NextSquare.GetNeighbourSquares(2).Where(S => S.Wall != null && !S.IsEdge(Region)).ToArray();
        if (NeighbourArray.Length > 0)
        {
          var Neighbour = NeighbourArray.GetRandom();
          var NeighbourUnderlay = Neighbour.Wall.Barrier.Underlay;
          Generator.RemoveWall(Neighbour);
          Generator.PlaceFloor(Neighbour, NeighbourUnderlay);

          var Between = NextSquare.Adjacent(NextSquare.AsDirection(Neighbour));
          if (Between.Wall != null)
          {
            var BetweenUnderlay = Between.Wall.Barrier.Underlay;
            Generator.RemoveWall(Between);
            Generator.PlaceFloor(Between, BetweenUnderlay);
          }

          SquareStack.Push(NextSquare);
          NextSquare = Neighbour;
        }
        else
        {
          NextSquare = SquareStack.Pop();
        }
      }
    }
    private void CreateMazeDetails(DungeonStructure Structure, Map Map, Block Block, Region Region, int BoxSize)
    {
      var BoxWidth = Region.Width / BoxSize;
      var BoxHeight = Region.Height / BoxSize;

      var BoxTop = Region.Top;

      for (var BoxY = 0; BoxY <= BoxHeight; BoxY++)
      {
        if (BoxTop > Region.Bottom)
          break;

        var BoxBottom = Math.Min(Region.Bottom, BoxTop + BoxSize - 1);

        var BoxLeft = Region.Left;

        for (var BoxX = 0; BoxX <= BoxWidth; BoxX++)
        {
          if (BoxLeft > Region.Right)
            break;

          var BoxRight = Math.Min(Region.Right, BoxLeft + BoxSize - 1);

          CreateRoomDetails(Structure, Map, Block, Map.GetSquares(new Region(BoxLeft, BoxTop, BoxRight, BoxBottom)).Where(S => S.Wall == null).ToDistinctList());

          BoxLeft += BoxSize;
        }

        BoxTop += BoxSize;
      }
    }
    private DungeonStructure CreateCavern(Map Map, Region Region)
    {
      Debug.Assert(Region.Width % CavernSize == 0);
      Debug.Assert(Region.Height % CavernSize == 0);

      var Structure = new DungeonStructure(Map);

      var BoxWidth = Region.Width / CavernSize;
      var BoxHeight = Region.Height / CavernSize;

      var TemplateList = LoadCaveTemplateList();

      // basic maze algorithm to give the caves a structure.
      var MazeGrid = new MazeGrid(BoxWidth, BoxHeight);

      var MazeStack = new Stack<MazeNode>();

      var StartColumn = 1.d(MazeGrid.Width).Roll() - 1;
      var StartRow = 1.d(MazeGrid.Height).Roll() - 1;

      var NextNode = MazeGrid[StartColumn, StartRow];
      MazeStack.Push(NextNode);

      var CavernBarrier = Codex.Barriers.cave_wall;
      var CavernGround = Codex.Grounds.cave_floor;

      while (MazeStack.Count > 0)
      {
        var NeighbourArray = MazeGrid.GetNeighbourNodes(NextNode).Where(N => N.IsNone()).ToArray();

        if (NeighbourArray.Length > 0)
        {
          var NeighbourNode = NeighbourArray.GetRandom();

          var DeltaX = NextNode.X - NeighbourNode.X;
          var DeltaY = NextNode.Y - NeighbourNode.Y;

          if (DeltaX < 0)
          {
            NeighbourNode.Left = true;
            NextNode.Right = true;
          }
          else if (DeltaX > 0)
          {
            NeighbourNode.Right = true;
            NextNode.Left = true;
          }
          else if (DeltaY < 0)
          {
            NeighbourNode.Top = true;
            NextNode.Bottom = true;
          }
          else if (DeltaY > 0)
          {
            NeighbourNode.Bottom = true;
            NextNode.Top = true;
          }

          MazeStack.Push(NextNode);
          NextNode = NeighbourNode;
        }
        else
        {
          NextNode = MazeStack.Pop();
        }
      }

#if DEBUG
      // coherency check.
      for (var BoxY = 0; BoxY < BoxHeight; BoxY++)
      {
        for (var BoxX = 0; BoxX < BoxWidth; BoxX++)
        {
          var Node = MazeGrid[BoxX, BoxY];

          if (BoxX == 0)
            Debug.Assert(Node.Left == false);
          else
            Debug.Assert(Node.Left == MazeGrid[BoxX - 1, BoxY].Right);

          if (BoxX == BoxWidth - 1)
            Debug.Assert(Node.Right == false);

          if (BoxY == 0)
            Debug.Assert(Node.Top == false);
          else
            Debug.Assert(Node.Top == MazeGrid[BoxX, BoxY - 1].Bottom);

          if (BoxY == BoxHeight - 1)
            Debug.Assert(Node.Bottom == false);
        }
      }
#endif

      // place suitable template pieces into the maze.
      var TemplateGrid = new CaveTemplate[BoxWidth, BoxHeight];

      for (var BoxY = 0; BoxY < BoxHeight; BoxY++)
      {
        for (var BoxX = 0; BoxX < BoxWidth; BoxX++)
        {
          var BoxNode = MazeGrid[BoxX, BoxY];

          var BoxLeft = Region.Left + (BoxX * CavernSize);
          var BoxTop = Region.Top + (BoxY * CavernSize);
          var BoxRegion = new Region(BoxLeft, BoxTop, BoxLeft + CavernSize - 1, BoxTop + CavernSize - 1);
          var BoxRoom = Structure.AddRoom(BoxRegion, Isolated: false);
          var BoxZone = Map.AddZone();
          BoxZone.AddRegion(BoxRegion);
          BoxZone.SetLit(Generator.RandomCaveIsLit(Map));

          var TemplateQuery = (IEnumerable<CaveTemplate>)TemplateList;

          if (BoxX > 0)
            TemplateQuery = TemplateQuery.Where(T => T.LeftMask == TemplateGrid[BoxX - 1, BoxY].RightMask);

          if (BoxY > 0)
            TemplateQuery = TemplateQuery.Where(T => T.TopMask == TemplateGrid[BoxX, BoxY - 1].BottomMask);

          if (BoxX == 0)
            TemplateQuery = TemplateQuery.Where(T => T.LeftMask == 0);

          if (BoxX == BoxWidth - 1)
            TemplateQuery = TemplateQuery.Where(T => T.RightMask == 0);

          if (BoxY == 0)
            TemplateQuery = TemplateQuery.Where(T => T.TopMask == 0);

          if (BoxY == BoxHeight - 1)
            TemplateQuery = TemplateQuery.Where(T => T.BottomMask == 0);

          if (BoxNode.Left)
            TemplateQuery = TemplateQuery.Where(T => T.LeftMask != 0);

          if (BoxNode.Right)
            TemplateQuery = TemplateQuery.Where(T => T.RightMask != 0);

          if (BoxNode.Top)
            TemplateQuery = TemplateQuery.Where(T => T.TopMask != 0);

          if (BoxNode.Bottom)
            TemplateQuery = TemplateQuery.Where(T => T.BottomMask != 0);

          var Template = TemplateQuery.GetRandomOrNull();

          if (Template == null)
          {
            var WallArray = new bool?[CavernSize, CavernSize];

            for (var WallY = 1; WallY < CavernSize - 1; WallY++)
            {
              for (var WallX = 1; WallX < CavernSize - 1; WallX++)
                WallArray[WallX, WallY] = false;
            }

            if (BoxX == 0)
            {
              for (var WallY = 0; WallY < CavernSize; WallY++)
                WallArray[0, WallY] = true;
            }
            else
            {
              var LeftEdge = TemplateGrid[BoxX - 1, BoxY];
              for (var WallY = 0; WallY < CavernSize; WallY++)
                WallArray[0, WallY] = LeftEdge.WallArray[CavernSize - 1, WallY] != false;
            }

            if (BoxX == BoxWidth - 1)
            {
              for (var WallY = 0; WallY < CavernSize; WallY++)
                WallArray[CavernSize - 1, WallY] = true;
            }
            else
            {
              // corners.
              WallArray[CavernSize - 1, 0] = true;
              WallArray[CavernSize - 1, CavernSize - 1] = true;

              if (BoxNode.Right)
              {
                for (var WallY = 1; WallY < CavernSize - 1; WallY++)
                  WallArray[CavernSize - 1, WallY] = false;

                var WallCount = 1.d(CavernSize - 2).Roll();
                for (var WallIndex = 0; WallIndex < WallCount; WallIndex++)
                  WallArray[CavernSize - 1, 1.d(CavernSize - 2).Roll()] = true;
              }
              else
              {
                for (var WallY = 1; WallY < CavernSize - 1; WallY++)
                  WallArray[CavernSize - 1, WallY] = true;
              }
            }

            if (BoxY == 0)
            {
              for (var WallX = 0; WallX < CavernSize; WallX++)
                WallArray[WallX, 0] = true;
            }
            else
            {
              var TopEdge = TemplateGrid[BoxX, BoxY - 1];

              for (var WallX = 1; WallX < CavernSize - 1; WallX++)
                WallArray[WallX, 0] = TopEdge.WallArray[WallX, CavernSize - 1] != false;
            }

            if (BoxY == BoxHeight - 1)
            {
              for (var WallX = 0; WallX < CavernSize; WallX++)
                WallArray[WallX, CavernSize - 1] = true;
            }
            else
            {
              if (BoxNode.Bottom)
              {
                for (var WallX = 1; WallX < CavernSize - 1; WallX++)
                  WallArray[WallX, CavernSize - 1] = false;

                var WallCount = 1.d(CavernSize - 2).Roll();
                for (var WallIndex = 0; WallIndex < WallCount; WallIndex++)
                  WallArray[1.d(CavernSize - 2).Roll(), CavernSize - 1] = true;
              }
              else
              {
                for (var WallX = 1; WallX < CavernSize - 1; WallX++)
                  WallArray[WallX, CavernSize - 1] = true;
              }
            }

            Template = new CaveTemplate("null", WallArray);
          }

          TemplateGrid[BoxX, BoxY] = Template;

          for (var WallY = 0; WallY < CavernSize; WallY++)
          {
            for (var WallX = 0; WallX < CavernSize; WallX++)
            {
              var Wall = Template.WallArray[WallX, WallY];

              if (Wall != null)
              {
                var Square = Map[BoxLeft + WallX, BoxTop + WallY];

                if (Wall.Value)
                  Generator.PlaceSolidWall(Square, CavernBarrier, WallSegment.Cross);
                else
                  Generator.PlaceFloor(Square, CavernGround);
              }
            }
          }
        }
      }

      Generator.PlaceIllusionaryWalls(Map, CavernBarrier, Region);

      return Structure;
    }
    private Inv.DistinctList<AtticTemplate> LoadAtticTemplateList()
    {
      if (AtticTemplateList == null)
      {
        this.AtticTemplateList = [];

        using (var TemplateReader = new System.IO.StringReader(Official.Resources.Specials.Attics))
        {
          var AtticBuilder = new StringBuilder();

          var AtticLine = TemplateReader.ReadLine();
          while (AtticLine != null)
          {
            if (string.IsNullOrWhiteSpace(AtticLine))
            {
              if (AtticBuilder.Length > 0)
              {
                var AtticGrid = Generator.LoadSpecialGrid(AtticBuilder.ToString());
                AtticTemplateList.Add(new AtticTemplate(AtticGrid));

                AtticBuilder.Clear();
              }
            }
            else
            {
              AtticBuilder.AppendLine(AtticLine);
            }

            AtticLine = TemplateReader.ReadLine();
          }
        }
      }

      return AtticTemplateList;
    }
    private Inv.DistinctList<CaveTemplate> LoadCaveTemplateList()
    {
      if (CaveTemplateList == null)
      {
        this.CaveTemplateList = [];

        var IdentitySet = new HashSet<string>();

        using (var TemplateReader = new System.IO.StringReader(Official.Resources.Specials.SixBy6))
        {
          var Name = TemplateReader.ReadLine();

          while (Name != null)
          {
            var WallArray = new bool?[CavernSize, CavernSize];

            for (var Y = 0; Y < CavernSize; Y++)
            {
              var Row = TemplateReader.ReadLine();

              for (var X = 0; X < CavernSize; X++)
                WallArray[X, Y] = Row[X] == 'x' ? (bool?)null : Row[X] != '.';
            }

            TemplateReader.ReadLine(); // whitespace.

            var Template = new CaveTemplate(Name, WallArray);
            CaveTemplateList.Add(Template);

#if DEBUG
            var Identity = Template.Print();
            if (!IdentitySet.Add(Identity))
              throw new Exception("Duplicate " + Name + ":" + Environment.NewLine + Identity);
#endif

            Name = TemplateReader.ReadLine();
          }
        }

        // Rotate declared templates to increase permutations.
        var RotateList = new Inv.DistinctList<CaveTemplate>();

        foreach (var Template in CaveTemplateList)
        {
          var RotateGrid = RotateMatrix90Degrees(Template.WallArray);
          RotateList.Add(new CaveTemplate(Template.Name + "-rotate90", RotateGrid));

          RotateGrid = RotateMatrix90Degrees(RotateGrid);
          RotateList.Add(new CaveTemplate(Template.Name + "-rotate180", RotateGrid));

          RotateGrid = RotateMatrix90Degrees(RotateGrid);
          RotateList.Add(new CaveTemplate(Template.Name + "-rotate270", RotateGrid));
        }

        // TODO: eliminate duplicates templates or leave them in the mix?

        CaveTemplateList.AddRange(RotateList);
      }

      return CaveTemplateList;
    }
    private Probability<Zoo> GetZooProbability(Map Map)
    {
      return Generator.GetZooProbability(Map.Difficulty);
    }
    private bool CreateMinesBranch(Level EntranceLevel, DungeonStructure EntranceStructure)
    {
      if (Generator.Adventure.World.HasSite(Generator.EscapedModuleTerm(NethackTerms.Mines)))
        return false;

      var EntranceMap = EntranceLevel.Map;

      var EntranceRoom = EntranceStructure.Rooms.Where(R => !R.Isolated && !R.GetFloorSquares().Any(S => EntranceLevel.UpSquare == S || EntranceLevel.DownSquare == S)).GetRandomOrNull();
      if (EntranceRoom == null)
        return false;

      var EntranceSquare = EntranceRoom.GetFloorSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();
      if (EntranceSquare == null)
        return false;

      var MinesBarrier = Codex.Barriers.cave_wall;
      var MinesBlock = Codex.Blocks.clay_boulder;
      var MinesGround = Codex.Grounds.cave_floor;
      var MinesLevelDownPortal = Codex.Portals.clay_staircase_down;
      var MinesLevelUpPortal = Codex.Portals.clay_staircase_up;
      var PreciousGemProbability = Codex.Stocks.gem.Items.Where(I => I.Type == ItemType.Gem && I.Price > Gold.One).ToProbability(I => I.Rarity);

      // don't use this room for other portals (so we have to handle the details ourselves except don't create details on the minestown entrance staircase).
      EntranceStructure.RemoveRoom(EntranceRoom);
      CreateRoomDetails(EntranceStructure, EntranceMap, MinesBlock, EntranceRoom.GetFloorSquares().Except(EntranceSquare).ToDistinctList());

      // repair entrance to minetown to look like mines.
      foreach (var FixSquare in EntranceRoom.GetEdgeSquares())
      {
        if (FixSquare.Wall != null)
        {
          Generator.ReplaceWall(FixSquare, MinesBarrier);
        }
        else if (FixSquare.Door != null && FixSquare.Door.Secret)
        {
          var FixSegment = FixSquare.Door.IsHorizontal ? WallSegment.Horizontal : WallSegment.Vertical;
          Generator.RemoveDoor(FixSquare);
          Generator.PlaceIllusionaryWall(FixSquare, MinesBarrier, FixSegment);
        }
      }

      foreach (var FixSquare in EntranceRoom.GetFloorSquares().Where(S => S.Floor != null))
        Generator.PlaceFloor(FixSquare, MinesGround);

      // generate the town.
      var TownBarrier = Codex.Barriers.stone_wall;
      var TownBuildingGround = Codex.Grounds.stone_floor;
      var TownGate = Codex.Gates.wooden_door;
      var TownOutskirtGround = Codex.Grounds.dirt;

      var GridArray = new[]
      {
        new { Frequency = 12, Name = "Frontier Town", Text = Official.Resources.Specials.Town1Frontier },
        new { Frequency = 12, Name = "Townsquare", Text = Official.Resources.Specials.Town2Square },
        new { Frequency = 12, Name = "Alley Town", Text = Official.Resources.Specials.Town3Alley },
        new { Frequency = 12, Name = "College Town", Text = Official.Resources.Specials.Town4College },
        new { Frequency = 12, Name = "Bustling Town", Text = Official.Resources.Specials.Town5Bustling },
        new { Frequency = 12, Name = "The Bazaar", Text = Official.Resources.Specials.Town6Bazaar },
        new { Frequency = 4, Name = "Orc Town", Text = Official.Resources.Specials.Town7Orcish }, // massacre.
        new { Frequency = 12, Name = "The Waterway", Text = Official.Resources.Specials.Town8Waterway },
        new { Frequency = 12, Name = "The Lavaflow", Text = Official.Resources.Specials.Town9Lavaflow },
      };

      var TownArray = new[] { GridArray.ToProbability(T => T.Frequency).GetRandom() }; // generate a single town.
#if DEBUG
      //TownArray = new[] { GridArray[6] }; // test a specific town.
      //TownArray = GridArray; // test all towns.
#endif
      foreach (var Town in TownArray)
      {
        var SiteName = TownArray[0] == Town ? NethackTerms.Mines : Town.Name; // NOTE: Town Names are not translated, this is just for debugging.
        var MinesSite = Generator.Adventure.World.AddSite(Generator.EscapedModuleTerm(SiteName));

        var TownGrid = Generator.LoadSpecialGrid(Town.Text);

        // rotate 0-3 times (0/90/180/270 degrees).
        for (var Index = 0; Index < (1.d4() - 1).Roll(); Index++)
          TownGrid.Rotate90Degrees();

        var DoubleCavern = CavernSize * 2;

        var TownWidth = TownGrid.Width + (DoubleCavern * 2);
        TownWidth -= TownWidth % CavernSize;

        var TownHeight = TownGrid.Height + (DoubleCavern * 2);
        TownHeight -= TownHeight % CavernSize;

        var TownName = TownArray[0] == Town ? NethackTerms.Minetown : Town.Name;

        var TownMap = Generator.Adventure.World.AddMap(Generator.EscapedModuleTerm(TownName), TownWidth, TownHeight);
        TownMap.SetDifficulty(EntranceMap.Difficulty + 1);
        TownMap.SetAtmosphere(Codex.Atmospheres.civilisation);

        var TownLevel = MinesSite.AddLevel(1, TownMap);

        var TownStructure = CreateCavern(TownMap, TownMap.Region);

        // strip out all zones and rooms.
        TownStructure.RemoveRooms();
        TownMap.RemoveZones();

        var TownRegion = new Region(DoubleCavern, DoubleCavern, DoubleCavern + TownGrid.Width - 1, DoubleCavern + TownGrid.Height - 1);

        // obliterate the inside caverns.
        foreach (var Square in TownMap.GetSquares(TownRegion))
        {
          if (Square.Wall != null)
            Generator.RemoveWall(Square);

          Generator.PlaceFloor(Square, MinesGround); // required for the repair gaps algorithm.
        }

        foreach (var Square in TownMap.GetSquares(TownRegion))
        {
          Generator.RemoveFloor(Square);
          Debug.Assert(Square.IsEmpty());
        }

        // put in the caves zones, so we can create details.
        Generator.RepairZones(TownMap, TownMap.Region);

        // passages to the mines levels.
        var BelowSquare = TownMap.GetSquares(TownMap.Region).Where(Generator.CanPlacePortal).ToArray().GetRandom();
        Generator.PlacePassage(BelowSquare, MinesLevelDownPortal, Destination: null);

        // the outer caves.
        foreach (var TownZone in TownMap.Zones)
        {
          if (Chance.OneIn3.Hit())
          {
            if (Chance.OneIn3.Hit())
              Generator.PlaceRoomFixtures(TownZone.Squares);

            CreateRoomDetails(TownStructure, TownMap, MinesBlock, TownZone.Squares);
          }
          else if (Chance.OneIn3.Hit())
          {
            Generator.PlaceHorde(TownZone.Squares);
          }
        }

        var TownParty = Generator.NewParty(Leader: null);

        void RecruitTownParty(Square Square)
        {
          var Character = Square.Character;
          if (Character != null)
          {
            Generator.NeutralCharacter(Character);

            TownParty.AddAlly(Character, Clock.Zero, Delay.Zero);
          }
        }

        var DeferList = new Inv.DistinctList<Action>();

        var TownShopProbability = ShopProbability.Clone();

        var WatchmanDialogueList = NethackDialogues.WatchmanList.ToDistinctList();
        
        void AssignWatchCharacter(Character WatchCharacter, int WatchNumber)
        {
          var WatchDialogue = Generator.Adventure.World.AddDialogue("WATCH-" + WatchNumber);
          WatchDialogue.Root.Document.Fragment(WatchmanDialogueList.RemoveRandomOrNull() ?? NethackDialogues.WatchmanList.GetRandom());
          WatchDialogue.Root.Branch(NethackDialogues.Goodbye);
          Generator.AssignDialogue(WatchCharacter, WatchDialogue);
        }

        var WatchmanArray = new Character[9];

        for (var Column = 0; Column < TownGrid.Width; Column++)
        {
          for (var Row = 0; Row < TownGrid.Height; Row++)
          {
            var TownSymbol = TownGrid[Column, Row];
            var TownSquare = TownMap[Column + DoubleCavern, Row + DoubleCavern];

            switch (TownSymbol)
            {
              case '<':
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                Generator.PlacePassage(EntranceSquare, MinesLevelDownPortal, TownSquare);
                Generator.PlacePassage(TownSquare, MinesLevelUpPortal, EntranceSquare);

                TownLevel.SetTransitions(TownSquare, BelowSquare);
                break;

              case '.':
                // room floor.
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);
                break;

              case '#':
                // corridor floor.
                Generator.PlaceFloor(TownSquare, TownOutskirtGround);
                TownSquare.SetLit(true);
                break;

              case '-':
              case '|':
                // walls.
                Generator.PlaceSolidWall(TownSquare, TownBarrier, WallSegment.Cross);
                TownSquare.SetLit(true);
                break;

              case '+':
                // door.
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                if (TownGate == null)
                {
                  Generator.PlaceIllusionaryWall(TownSquare, TownBarrier, WallSegment.Cross);
                }
                else
                {
                  Generator.PlaceClosedHorizontalDoor(TownSquare, TownGate, TownBarrier);
                  /*
                  var Door = TownSquare.Door;
                  if (Door != null)
                  {
                    //Door.SetState(DoorState.Locked);
                    //Door.SetTrap(Generator.Engine.CreateTrap(Codex.Devices.fire_trap));
                  }
                  */
                }
                break;

              case '=':
                // workbench.
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                Generator.PlaceFixture(TownSquare, Codex.Features.workbench);
                break;

              case '{':
                // fountain.
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                Generator.PlaceFixture(TownSquare, Codex.Features.fountain);
                break;

              case '}':
                // tree.
                Generator.PlaceFloor(TownSquare, TownOutskirtGround);
                Generator.PlaceSolidWall(TownSquare, Codex.Barriers.tree, WallSegment.Pillar);
                TownSquare.SetLit(true);
                break;

              case '_':
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                var TownShrine = ShrineProbability.GetRandom();

                Generator.PlaceShrine(TownSquare, TownShrine);
                RecruitTownParty(TownSquare);

                DeferList.Add(() =>
                {
                  var ShrineZone = TownMap.AddZone();
                  ShrineZone.AddRegion(TownSquare.FindBoundary());

                  ShrineZone.InsertTrigger().Add(Delay.Zero, Codex.Tricks.VisitShrineArray[TownShrine.Index]).SetTarget(TownSquare);
                });

                break;

              case '~':
                Generator.PlaceFloor(TownSquare, Codex.Grounds.water);
                TownSquare.SetLit(true);

                if (Generator.CanPlaceAsset(TownSquare) && Chance.OneIn10.Hit())
                  Generator.PlaceSpecificAsset(TownSquare, Codex.Items.kelp_frond);
                break;

              case '[':
                Generator.PlaceFloor(TownSquare, Codex.Grounds.water);
                Generator.PlaceBridge(TownSquare, Codex.Platforms.wooden_bridge, BridgeOrientation.Horizontal);
                TownSquare.SetLit(true);
                break;

              case '`':
                Generator.PlaceFloor(TownSquare, Codex.Grounds.lava);
                TownSquare.SetLit(true);
                break;

              case ']':
                Generator.PlaceFloor(TownSquare, Codex.Grounds.lava);
                Generator.PlaceBridge(TownSquare, Codex.Platforms.crystal_bridge, BridgeOrientation.Horizontal);
                TownSquare.SetLit(true);
                break;

              case '@':
              case '%':
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                var TownShop = TownShopProbability.RemoveRandomOrNull();

                if (TownShop != null)
                {
                  if (TownSymbol == '@')
                  {
                    Generator.PlaceShop(TownSquare, TownShop, 6.d2().Roll());
                    RecruitTownParty(TownSquare);

                    DeferList.Add(() =>
                    {
                      var ShopZone = TownMap.AddZone();
                      ShopZone.AddRegion(TownSquare.FindBoundary());
                      ShopZone.InsertTrigger().Add(Delay.Zero, Codex.Tricks.VisitShopArray[TownShop.Index]).SetTarget(TownSquare);
                    });
                  }
                  else
                  {
                    Generator.PlaceShop(TownSquare, TownShop, 1.d4().Roll());
                    Generator.CorpseSquare(TownSquare);

                    if (TownSquare.Fixture != null)
                      Generator.BreakFixture(TownSquare);

                    // murdered by orcs.
                    DeferList.Add(() =>
                    {
                      var ShopParty = Generator.PlaceHorde(Codex.Hordes.orc, Generator.MinimumDifficulty(TownSquare), Generator.MaximumDifficulty(TownSquare), () => Generator.CanPlaceCharacter(TownSquare) ? TownSquare : Generator.ExpandingFindSquare(TownSquare, 3));
                      if (ShopParty == null)
                        Generator.PlaceSpecificCharacter(TownSquare, Codex.Entities.orc_grunt);
                    });
                  }
                }
                break;

              case '&':
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                Generator.PlaceShrine(TownSquare, ShrineProbability.GetRandom());
                Generator.CorpseSquare(TownSquare);

                //if (TownSquare.Fixture != null)
                //  Generator.BreakFixture(TownSquare);

                DeferList.Add(() =>
                {
                  var ShrineParty = Generator.PlaceHorde(Codex.Hordes.orc, Generator.MinimumDifficulty(TownSquare), Generator.MaximumDifficulty(TownSquare), () => Generator.CanPlaceCharacter(TownSquare) ? TownSquare : Generator.ExpandingFindSquare(TownSquare, 3));
                  if (ShrineParty == null)
                    Generator.PlaceSpecificCharacter(TownSquare, Codex.Entities.orc_grunt);
                });
                break;

              case 'f':
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                Generator.PlaceSpecificCharacter(TownSquare, Codex.Entities.housecat);
                break;

              case 'k':
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                Generator.PlaceSpecificCharacter(TownSquare, Codex.Entities.kobold_shaman);
                break;

              case 'l':
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                Generator.PlaceSpecificCharacter(TownSquare, Codex.Entities.gnome_lord);
                break;

              case 'n':
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                Generator.PlaceSpecificCharacter(TownSquare, Codex.Entities.water_nymph);
                break;

              case 'w':
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                Generator.PlaceSpecificCharacter(TownSquare, Codex.Entities.gnomish_wizard);
                break;

              case 'y':
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                Generator.PlaceSpecificCharacter(TownSquare, Codex.Entities.monkey);
                break;

              case 'G':
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                Generator.PlaceSpecificCharacter(TownSquare, Codex.Entities.watch_captain);

                var CaptainCharacter = TownSquare.Character;
                if (CaptainCharacter != null)
                  AssignWatchCharacter(CaptainCharacter, 0);

                RecruitTownParty(TownSquare);
                break;

              case '\\':
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                Generator.PlaceFixture(TownSquare, Codex.Features.bed);
                break;

              case '1':
              case '2':
              case '3':
              case '4':
              case '5':
              case '6':
              case '7':
              case '8':
              case '9':
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);

                var WatchmanNumber = (int)TownSymbol - (int)'0';
                var WatchmanCharacter = WatchmanArray[WatchmanNumber - 1];

                if (WatchmanCharacter == null)
                {
                  Generator.PlaceSpecificCharacter(TownSquare, Codex.Entities.watchman);

                  WatchmanCharacter = TownSquare.Character;
                  if (WatchmanCharacter != null)
                  {
                    AssignWatchCharacter(WatchmanCharacter, WatchmanNumber);

                    WatchmanArray[WatchmanNumber - 1] = WatchmanCharacter;
                    Generator.ResidentSquare(WatchmanCharacter, TownSquare);

                    RecruitTownParty(TownSquare);
                  }
                }
                else
                {
                  Generator.ResidentRoute(WatchmanCharacter, WatchmanCharacter.Resident.Routes.Union(TownSquare).ToArray(), 0);
                }
                break;

              case ' ':
                // void.
                Generator.RemoveFloor(TownSquare);
                TownSquare.SetLit(false);
                break;

              default:
                Generator.PlaceFloor(TownSquare, TownBuildingGround);
                TownSquare.SetLit(true);
                Debug.WriteLine(TownSymbol);
                break;
            }
          }
        }

        // repair the gaps.
        foreach (var Square in TownMap.GetSquares(TownMap.Region))
        {
          if (Square.IsEmpty() && Square.GetAdjacentSquares().Any(S => S.Floor != null && S.Wall == null))
            Generator.PlaceSolidWall(Square, MinesBarrier, WallSegment.Cross);
        }

        foreach (var Defer in DeferList)
          Defer();

        Generator.RepairVoid(TownMap, TownMap.Region);
        Generator.RepairWalls(TownMap, TownMap.Region);
        Generator.RepairDoors(TownMap, TownMap.Region);
        Generator.RepairBridges(TownMap, TownMap.Region);
        Generator.RepairZones(TownMap, TownMap.Region);

        // mines levels.
        var LevelArray = new[]
        {
          new { Number = 1, Text = Official.Resources.Specials.Mines1 },
          new { Number = 2, Text = Official.Resources.Specials.Mines2 },
          new { Number = 3, Text = Official.Resources.Specials.Mines3 },
          new { Number = 4, Text = Official.Resources.Specials.Mines4 },
          new { Number = 5, Text = Official.Resources.Specials.Mines5 },
        };

        Square NextLevelSquare = null;

        foreach (var Level in LevelArray)
        {
          var LevelMapName = Generator.EscapedModuleTerm(SiteName) + " " + Level.Number;
          if (Generator.Adventure.World.HasMap(LevelMapName))
          {
            Debug.Fail("How is there a duplicate mines level?");
            return false;
          }

          var MinesGrid = Generator.LoadSpecialGrid(Level.Text);

          // rotate 0-3 times (0/90/180/270 degrees).
          for (var Index = 0; Index < (1.d4() - 1).Roll(); Index++)
            MinesGrid.Rotate90Degrees();

          var MinesMap = Generator.Adventure.World.AddMap(LevelMapName, MinesGrid.Width, MinesGrid.Height);
          MinesMap.SetDifficulty(TownMap.Difficulty + Level.Number);
          MinesMap.SetAtmosphere(Codex.Atmospheres.cavern);

          var MinesStructure = new DungeonStructure(MinesMap);

          var MinesLevel = MinesSite.AddLevel(Level.Number + 1, MinesMap);

          Square DownLevelSquare = null;

          for (var Column = 0; Column < MinesGrid.Width; Column++)
          {
            for (var Row = 0; Row < MinesGrid.Height; Row++)
            {
              var MinesSymbol = MinesGrid[Column, Row];
              var MinesSquare = MinesMap[Column, Row];

              switch (MinesSymbol)
              {
                case ' ':
                  // void.
                  break;

                case '.':
                  // room floor.
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  MinesSquare.SetLit(true);
                  break;

                case '#':
                  // corridor floor.
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  MinesSquare.SetLit(true);
                  break;

                case '-':
                case '|':
                  // walls.
                  Generator.PlaceSolidWall(MinesSquare, MinesBarrier, WallSegment.Cross);
                  MinesSquare.SetLit(true);
                  break;

                case '`':
                  // can't dig through this wall.
                  Generator.PlacePermanentWall(MinesSquare, MinesBarrier, WallSegment.Cross);
                  MinesSquare.SetLit(true);
                  break;

                case '~':
                  // locked door to Under levels staircase.
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceLockedHorizontalDoor(MinesSquare, Codex.Gates.crystal_door, MinesBarrier);
                  MinesSquare.SetLit(true);
                  break;

                case '+':
                  // door.
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceIllusionaryWall(MinesSquare, MinesBarrier, WallSegment.Cross);
                  MinesSquare.SetLit(true);
                  break;

                case 'S':
                  // secret door.
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceIllusionaryWall(MinesSquare, MinesBarrier, WallSegment.Cross);
                  MinesSquare.SetLit(true);
                  break;

                case 'D':
                  // locked secret door.
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceIllusionaryWall(MinesSquare, MinesBarrier, WallSegment.Cross);
                  MinesSquare.SetLit(true);
                  break;

                case 'H':
                  // maybe floor, maybe wall.
                  if (Chance.OneIn2.Hit())
                    Generator.PlaceFloor(MinesSquare, MinesGround);
                  else
                    Generator.PlaceSolidWall(MinesSquare, MinesBarrier, WallSegment.Cross);
                  MinesSquare.SetLit(true);
                  break;

                case '<':
                  // up stair.
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  MinesSquare.SetLit(true);

                  var DestinationSquare = NextLevelSquare != null && NextLevelSquare.Map != MinesMap ? NextLevelSquare : BelowSquare;

                  DestinationSquare.Passage.SetDestination(MinesSquare);
                  Generator.PlacePassage(MinesSquare, MinesLevelUpPortal, DestinationSquare);

                  MinesLevel.SetTransitions(MinesSquare, MinesLevel.DownSquare);
                  break;

                case '>':
                  // down stair.
                  Generator.PlaceFloor(MinesSquare, MinesGround);

                  DownLevelSquare = MinesSquare;

                  // NOTE: the passage destination is replaced by the upstairs placement of the next level (we do it this way to avoid traps being generated on the downstairs).
                  Generator.PlacePassage(DownLevelSquare, MinesLevelDownPortal, Destination: null);

                  MinesLevel.SetTransitions(MinesLevel.UpSquare, MinesSquare);

                  MinesSquare.SetLit(true);
                  break;

                case 'g':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceSpecificCharacter(MinesSquare, Codex.Entities.gnome_warrior);

                  MinesSquare.SetLit(true);
                  break;

                case 'l':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceSpecificCharacter(MinesSquare, Codex.Entities.gnome_lord);

                  MinesSquare.SetLit(true);
                  break;

                case 't':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceSpecificCharacter(MinesSquare, Codex.Entities.gnome_thief);

                  MinesSquare.SetLit(true);
                  break;

                case 'm':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceSpecificCharacter(MinesSquare, Codex.Entities.gnome_mummy);

                  MinesSquare.SetLit(true);
                  break;

                case 'z':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceSpecificCharacter(MinesSquare, Codex.Entities.gnome_zombie);

                  MinesSquare.SetLit(true);
                  break;

                case 'w':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceSpecificCharacter(MinesSquare, Codex.Entities.gnomish_wizard);

                  MinesSquare.SetLit(true);
                  break;

                case 'k':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  MinesSquare.SetLit(true);

                  Generator.PlaceSpecificCharacter(MinesSquare, Codex.Entities.kobold_shaman);
                  break;

                case 'U':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceSpecificCharacter(MinesSquare, Codex.Entities.umber_hulk);

                  MinesSquare.SetLit(true);
                  break;

                case 'Z':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceRandomCharacter(MinesSquare); // this is a zoo.
                  Generator.DropCoins(MinesSquare, Generator.RandomCoinQuantity(MinesSquare));

                  var ZooCharacter = MinesSquare.Character;
                  if (ZooCharacter != null)
                    SnoozeCharacter(ZooCharacter);

                  MinesSquare.SetLit(true);
                  break;

                case '\\':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceFixture(MinesSquare, Codex.Features.throne);
                  Generator.PlaceSpecificCharacter(MinesSquare, Codex.Entities.gnome_king);
                  var KingCharacter = MinesSquare.Character;

                  if (KingCharacter != null)
                  {
                    var KingPromotion = MinesMap.Difficulty - KingCharacter.Level + 5;
                    if (KingPromotion > 0)
                      Generator.PromoteCharacter(KingCharacter, KingPromotion);

                    var Properties = Codex.Properties;
                    var Elements = Codex.Elements;

                    Generator.AcquireTalent(KingCharacter, Properties.free_action, Properties.polymorph_control, Properties.slippery);
                    Generator.EnsureResistance(KingCharacter, Elements.magical, 100);

                    Generator.AcquireUnique(MinesSquare, KingCharacter, Codex.Qualifications.master);

                    // gnome with a wand of death!
                    KingCharacter.Inventory.Carried.Add(Generator.NewSpecificAsset(MinesSquare, Codex.Items.wand_of_death));
                  }

                  MinesSquare.SetLit(true);
                  break;

                case '0':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceBoulder(MinesSquare, MinesBlock, IsRigid: false);

                  MinesSquare.SetLit(true);
                  break;

                case '*':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceSpecificAsset(MinesSquare, PreciousGemProbability.GetRandom());

                  MinesSquare.SetLit(true);
                  break;

                case '!':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceRandomAsset(MinesSquare, Codex.Stocks.potion);

                  MinesSquare.SetLit(true);
                  break;

                case '?':
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceRandomAsset(MinesSquare, Codex.Stocks.scroll);

                  MinesSquare.SetLit(true);
                  break;

                case '{':
                  // fountain.                
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceFixture(MinesSquare, Codex.Features.fountain);
                  MinesSquare.SetLit(true);
                  break;

                case '_':
                  // altar.
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceFixture(MinesSquare, Codex.Features.altar);
                  MinesSquare.SetLit(true);
                  break;

                case '&':
                  // grave.
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceFixture(MinesSquare, Codex.Features.grave);
                  MinesSquare.SetLit(true);
                  break;

                case '=':
                  // workbench.
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlaceFixture(MinesSquare, Codex.Features.workbench);
                  MinesSquare.SetLit(true);
                  break;

                case '^':
                  // portal back to the dungeon.
                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  Generator.PlacePassage(MinesSquare, Codex.Portals.transportal, EntranceSquare);
                  MinesSquare.SetLit(true);
                  break;

                default:
                  Debug.Fail($"Mines symbol not handled: {MinesSymbol} {(int)MinesSymbol}");

                  Generator.PlaceFloor(MinesSquare, MinesGround);
                  break;
              }
            }
          }

          Generator.RepairVoid(MinesMap, MinesMap.Region);
          Generator.RepairWalls(MinesMap, MinesMap.Region);
          Generator.RepairDoors(MinesMap, MinesMap.Region);
          Generator.RepairZones(MinesMap, MinesMap.Region);

          foreach (var MinesZone in MinesMap.Zones)
          {
            if (Chance.OneIn3.Hit())
              CreateRoomDetails(MinesStructure, MinesMap, MinesBlock, MinesZone.Squares);
          }

          NextLevelSquare = DownLevelSquare;
        }

        if (Inv.Assert.IsEnabled)
          Inv.Assert.CheckNotNull(NextLevelSquare, nameof(NextLevelSquare));

        var ResourceFile = Official.Resources.Quests.Underdeep;

        var Quest = Generator.ImportQuest(ResourceFile.Load().GetBuffer());

        var QuestSite = Quest.World.Sites.Single();
        var QuestStart = Quest.World.Start;
        var QuestDifficulty = NextLevelSquare.Map.Difficulty + 1;

        NextLevelSquare.Passage.SetDestination(QuestStart);
        QuestStart.Passage.SetDestination(NextLevelSquare);

        void GenerateMap(Map UnderMap)
        {
          var UnderParty = Generator.NewParty(Leader: null);

          UnderMap.SetName(Generator.EscapedModuleTerm(UnderMap.Name));

          Generator.BuildMap(UnderMap);

          foreach (var UnderSquare in UnderMap.GetSquares())
          {
            var UnderCharacter = UnderSquare.Character;
            if (UnderCharacter != null)
            {
              if (UnderCharacter.Entity == Codex.Entities.guard ||
                  UnderCharacter.Entity == Codex.Entities.watchman ||
                  UnderCharacter.Entity == Codex.Entities.watch_captain ||
                  UnderCharacter.Entity == Codex.Entities.merchant ||
                  UnderCharacter.Entity == Codex.Entities.holy_cleric ||
                  UnderCharacter.Entity == Codex.Entities.witch ||
                  UnderCharacter.Entity == Codex.Entities.dryad)
              {
                Debug.Assert(UnderCharacter.Neutral);

                // all guards are allied to each other.
                UnderParty.AddAlly(UnderCharacter, Clock.Zero, Delay.Zero);

                // some of the guards in Undertown are fixed (and indicated by obsidian floor).
                if (UnderCharacter.Square.Floor.Ground == Codex.Grounds.obsidian_floor)
                  Generator.ResidentSquare(UnderCharacter, UnderCharacter.Square);

                if (UnderCharacter.IsResident())
                {
                  void ResidentArea(Trick Trick)
                  {
                    var ResidentZone = UnderMap.AddZone();

                    foreach (var ResidentSquare in UnderMap.GetSquares(UnderCharacter.Square.FindBoundary()))
                    {
                      if (!ResidentSquare.IsObscuredFrom(UnderCharacter.Square))
                        ResidentZone.ForceSquare(ResidentSquare);
                    }
                    ResidentZone.InsertTrigger().Add(Delay.Zero, Trick).SetTarget(UnderCharacter.Square);
                  }

                  if (UnderCharacter.Resident.Shop != null)
                    ResidentArea(Codex.Tricks.VisitShopArray[UnderCharacter.Resident.Shop.Index]);
                  else if (UnderCharacter.Resident.Shrine != null)
                    ResidentArea(Codex.Tricks.VisitShrineArray[UnderCharacter.Resident.Shrine.Index]);
                }
              }
              else if (UnderCharacter.Entity == Codex.Entities.dwarf_king)
              {
                var UnderPromotion = UnderMap.Difficulty - UnderCharacter.Level + 5;
                if (UnderPromotion > 0)
                  Generator.PromoteCharacter(UnderCharacter, UnderPromotion);

                var Properties = Codex.Properties;
                var Elements = Codex.Elements;

                Generator.AcquireTalent(UnderCharacter, Properties.free_action, Properties.polymorph_control, Properties.slippery);
                Generator.EnsureResistance(UnderCharacter, Elements.magical, 100);

                Generator.AcquireUnique(UnderCharacter.Square, UnderCharacter, Codex.Qualifications.master);

                UnderCharacter.Inventory.Carried.Add(Generator.NewSpecificAsset(UnderCharacter.Square, Codex.Items.potion_of_full_healing));
              }
              else
              {
                //if (UnderCharacter.Neutral) Debug.WriteLine($"Underdeep Neutral: {UnderCharacter.Entity.Name}");
              }
            }

            foreach (var UnderAsset in UnderSquare.GetAssets())
            {
              if (UnderAsset.Container != null)
              {
                // random container loot.
                if (UnderAsset.Item.Type == ItemType.Chest && UnderAsset.Container.Stash.Count == 0)
                  StockContainer(UnderSquare, UnderAsset, Locked: true, Trapped: true);
              }
              else if (UnderAsset.Item.Type == ItemType.Gem || UnderAsset.Item.Type == ItemType.Book || UnderAsset.Item.Type == ItemType.Rock)
              {
                // randomise items.
                Generator.ReplaceRandomAsset(UnderSquare, UnderAsset);
              }
              else
              {
                //Debug.WriteLine(UnderAsset);
              }
            }
          }
        }

        foreach (var QuestLevel in QuestSite.Levels)
        {
          var UnderMap = QuestLevel.Map;
          UnderMap.SetDifficulty(QuestDifficulty + QuestLevel.Index);
          UnderMap.SetTerminal(QuestLevel == QuestSite.LastLevel);

          Generator.Adventure.World.AddMap(UnderMap);

          var UnderLevel = MinesSite.AddLevel(MinesSite.LastLevel.Index + 1, UnderMap);
          UnderLevel.SetTransitions(QuestLevel.UpSquare, QuestLevel.DownSquare);

          GenerateMap(UnderMap);

          // return portal to the entrance of the underdeep.
          if (UnderMap.Terminal)
            Generator.PlacePassage(UnderMap[10, 31], Codex.Portals.transportal, NextLevelSquare);

          foreach (var QuestMap in QuestLevel.GetMaps().Except(UnderMap))
          {
            QuestMap.SetDifficulty(UnderMap.Difficulty);

            Generator.Adventure.World.AddMap(QuestMap);

            QuestMap.SetLevel(UnderLevel);

            GenerateMap(QuestMap);
          }
        }
      }

      return true;
    }
    private bool CreateLostChambersBranch(DungeonRoom AtticRoom)
    {
      var EntranceSquare = AtticRoom.GetFloorSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();
      if (EntranceSquare == null)
        return false;

      if (Generator.Adventure.World.HasSite(Generator.EscapedModuleTerm(NethackTerms.Lost_Chambers)))
        return false;

      var ChambersSite = Generator.Adventure.World.AddSite(Generator.EscapedModuleTerm(NethackTerms.Lost_Chambers));

      var ResourceFile = Official.Resources.Quests.Chambers;

      var Quest = Generator.ImportQuest(ResourceFile.Load().GetBuffer());

      var QuestSite = Quest.World.Sites.Single();
      var QuestStart = Quest.World.Start;
      var QuestDifficulty = AtticRoom.Map.Difficulty + 1;

      const int QuestColumns = 5;
      const int QuestRows = 5;
      const int ChamberSize = 17;
      const int ChamberMargin = -1;
      const int TotalWidth = QuestColumns * (ChamberSize + ChamberMargin) - ChamberMargin;
      const int TotalHeight = QuestRows * (ChamberSize + ChamberMargin) - ChamberMargin;

      var PointList = new Inv.DistinctList<Inv.Point>(QuestColumns * QuestRows);

      var PatternArray = new string[QuestRows]
      {
        "#####",
        "## ##",
        "# * #",
        "## ##",
        "#####",
      };

      for (var Y = 0; Y < QuestRows; Y++)
      {
        for (var X = 0; X < QuestColumns; X++)
        {
          if (PatternArray[Y][X] == '#')
            PointList.Add(new Inv.Point(X, Y));
        }
      }

      PointList.Shuffle();
      PointList.Add(new Inv.Point(2, 2));

      Debug.Assert(QuestSite.Levels.Count == PointList.Count);

      var ChamberMap = Generator.Adventure.World.AddMap(Generator.EscapedModuleTerm(NethackTerms.Lost_Chambers), TotalWidth, TotalHeight);
      ChamberMap.SetDifficulty(QuestDifficulty);
      ChamberMap.SetAtmosphere(Codex.Atmospheres.nether);
      ChamberMap.SetTerminal(true);

      var ChamberLevel = ChambersSite.AddLevel(1, ChamberMap);

      var ConversionDictionary = new Dictionary<Square, Square>(TotalWidth * TotalHeight);

      // transfer from the quest to the actual map.
      var PointIndex = 0;
      foreach (var QuestLevel in QuestSite.Levels)
      {
        Debug.Assert(QuestLevel.Map.Region.Width == ChamberSize);
        Debug.Assert(QuestLevel.Map.Region.Height == ChamberSize);

        QuestLevel.Map.RotateRandomDegrees();

        var TargetPoint = PointList[PointIndex++];
        var TargetX = (TargetPoint.X * ChamberSize) + (ChamberMargin * TargetPoint.X);
        var TargetY = (TargetPoint.Y * ChamberSize) + (ChamberMargin * TargetPoint.Y);

        for (var SourceY = 0; SourceY < ChamberSize; SourceY++)
        {
          for (var SourceX = 0; SourceX < ChamberSize; SourceX++)
          {
            var QuestSquare = QuestLevel.Map[SourceX, SourceY];
            if (!QuestSquare.IsVoid())
            {
              var ChamberSquare = ChamberMap[TargetX + SourceX, TargetY + SourceY];
              if (ChamberSquare.Wall == null) // ChamberMargin causes an overlap.
                Generator.TransferSquare(ChamberSquare, QuestSquare);

              if (ChamberSquare.Floor != null)
                ConversionDictionary.Add(QuestSquare, ChamberSquare);
            }
          }
        }
      }

      Generator.BuildMap(ChamberMap);

      var ChamberShrineProbability = ShrineProbability.Clone();
      var ChamberShopProbability = ShopProbability.Clone();
      var ChamberShopItems = 1.d6() + 6;

      var BossCharacter = (Character)null;

      void ResidentArea(Square ResidentSquare, Trick ResidentTrick)
      {
        // make the immediate squares part of the resident zone.
        var ChamberZone = ChamberMap.AddZone();
        ChamberZone.ForceSquare(ResidentSquare);
        foreach (var AdjacentSquare in ResidentSquare.GetAdjacentSquares())
          ChamberZone.ForceSquare(AdjacentSquare);

        // all residents start with psychosis in the Lost Chambers.
        var ResidentCharacter = ResidentSquare.Character;
        if (ResidentCharacter != null)
        {
          Generator.PunishCharacter(ResidentCharacter, Codex.Punishments.psychosis);

          ChamberZone.InsertTrigger().Add(Delay.Zero, ResidentTrick).SetTarget(ResidentSquare);
        }
      }

      // convert passage destinations to the assigned square.
      foreach (var ChamberSquare in ChamberMap.GetSquares())
      {
        // resident shops and shrines.
        if (ChamberSquare.Fixture?.Feature == Codex.Features.stall)
        {
          var Shop = ChamberShopProbability.RemoveRandomOrNull();
          if (Shop != null)
          {
            Generator.RemoveFixture(ChamberSquare); // erase the current stall.

            Generator.PlaceShop(ChamberSquare, Shop, ChamberShopItems.Roll());

            ResidentArea(ChamberSquare, Codex.Tricks.VisitShopArray[Shop.Index]);
          }
        }
        else if (ChamberSquare.Fixture?.Feature == Codex.Features.altar)
        {
          var Shrine = ChamberShrineProbability.RemoveRandomOrNull();
          if (Shrine != null)
          {
            Generator.PlaceShrine(ChamberSquare, Shrine);

            ResidentArea(ChamberSquare, Codex.Tricks.VisitShrineArray[Shrine.Index]);
          }
        }

        // passage connections.
        if (ChamberSquare.Passage != null && ChamberSquare.Passage.Destination != null)
          ChamberSquare.Passage.SetDestination(ConversionDictionary[ChamberSquare.Passage.Destination]);

        // no-dig.
        if (ChamberSquare.Wall != null && ChamberSquare.Wall.IsPhysical())
          Generator.AdjustToPermanentWall(ChamberSquare);

        // find the boss.
        if (ChamberSquare.Character?.Entity == Codex.Entities.master_mind_flayer)
          BossCharacter = ChamberSquare.Character;
      }

      if (BossCharacter != null)
      {
        Debug.Assert(BossCharacter != null, "Boss could not be generated?");

        if (BossCharacter != null)
        {
          var BossPromotion = ChamberMap.Difficulty - BossCharacter.Level + 5;
          if (BossPromotion > 0)
            Generator.PromoteCharacter(BossCharacter, BossPromotion);

          var Properties = Codex.Properties;
          var Elements = Codex.Elements;

          Generator.AcquireTalent(BossCharacter, Properties.polymorph_control, Properties.slippery, Properties.clarity, Properties.free_action);
          Generator.EnsureResistance(BossCharacter, Elements.poison, 100);
          Generator.EnsureResistance(BossCharacter, Elements.shock, 100);
          Generator.EnsureResistance(BossCharacter, Elements.cold, 100);

          // master competency in all skills.
          foreach (var Competency in BossCharacter.Competencies)
            Generator.RequireCompetency(BossCharacter, Competency.Skill, Codex.Qualifications.master);

          Generator.AcquireUnique(BossCharacter.Square, BossCharacter, Codex.Qualifications.master);
        }
      }

      // no-teleport.
      foreach (var ChamberZone in ChamberMap.Zones)
      {
        ChamberZone.SetAccessRestricted(true);
        ChamberZone.SetSpawnRestricted(true);
      }

      var ChamberStart = ConversionDictionary[QuestStart];

      // corrupt the entrance room, to alert to the Lost Chambers branch, not an ordinary attic.
      foreach (var AdjacentSquare in EntranceSquare.GetAroundSquares())
      {
        if (AdjacentSquare.Floor != null)
          Generator.PlaceFloor(AdjacentSquare, Codex.Grounds.obsidian_floor);

        // TODO: this causes the wall repair algorithm to use undesirable segments, in this particular case.
        //if (AdjacentSquare.Wall != null)
        //  AdjacentSquare.Wall.SetBarrier(Codex.Barriers.hell_brick);
      }

      Generator.PlacePassage(EntranceSquare, Codex.Portals.wooden_ladder_up, ChamberStart);
      Generator.PlacePassage(ChamberStart, Codex.Portals.wooden_ladder_down, EntranceSquare);

      return true;
    }
    private bool CreateLabyrinthBranch(Level EntranceLevel, DungeonStructure EntranceStructure)
    {
      if (Generator.Adventure.World.HasSite(Generator.EscapedModuleTerm(NethackTerms.Labyrinth)))
        return false;

      var EntranceMap = EntranceLevel.Map;

      var EntranceRoom = EntranceStructure.Rooms.Where(R => !R.Isolated && !R.GetFloorSquares().Any(S => EntranceLevel.UpSquare == S || EntranceLevel.DownSquare == S)).GetRandomOrNull();
      if (EntranceRoom == null)
        return false;

      var EntranceSquare = EntranceRoom.GetFloorSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();
      if (EntranceSquare == null)
        return false;

      var LabyrinthBarrier = Codex.Barriers.hell_brick;
      var LabyrinthBlock = Codex.Blocks.crystal_boulder;
      var LabyrinthGround = Codex.Grounds.obsidian_floor;
      var LabyrinthLevelDownPortal = Codex.Portals.stone_staircase_down;
      var LabyrinthLevelUpPortal = Codex.Portals.stone_staircase_up;

      Generator.PlacePassage(EntranceSquare, LabyrinthLevelDownPortal, Destination: null);

      // don't use this room for other portals (so we have to handle the details ourselves except don't create details on the labyrinth entrance staircase).
      EntranceStructure.RemoveRoom(EntranceRoom);
      CreateRoomDetails(EntranceStructure, EntranceMap, LabyrinthBlock, EntranceRoom.GetFloorSquares().Except(EntranceSquare).ToDistinctList());

      // repair entrance to labyrinth to look like nether.
      foreach (var FixSquare in EntranceRoom.GetEdgeSquares())
      {
        if (FixSquare.Wall != null)
        {
          Generator.ReplaceWall(FixSquare, LabyrinthBarrier);
        }
        else if (FixSquare.Door != null && FixSquare.Door.Secret)
        {
          var FixSegment = FixSquare.Door.IsHorizontal ? WallSegment.Horizontal : WallSegment.Vertical;
          Generator.RemoveDoor(FixSquare);
          Generator.PlaceIllusionaryWall(FixSquare, LabyrinthBarrier, FixSegment);
        }
      }

      foreach (var FixSquare in EntranceRoom.GetFloorSquares().Where(S => S.Floor != null))
        Generator.PlaceFloor(FixSquare, LabyrinthGround);

      // three maze levels, 10x10 -> 20x20 -> 30x30.
      var LabyrinthSite = Generator.Adventure.World.AddSite(Generator.EscapedModuleTerm(NethackTerms.Labyrinth));

      var BaseSize = 11;
      var BoxSize = 5;
      var LevelCount = 3;

      for (var LevelIndex = 1; LevelIndex <= LevelCount; LevelIndex++)
      {
        var LabyrinthSize = BaseSize * LevelIndex;
        var LabyrinthMap = Generator.Adventure.World.AddMap(Generator.EscapedModuleTerm(NethackTerms.Labyrinth) + " " + LevelIndex, LabyrinthSize, LabyrinthSize);
        LabyrinthMap.SetDifficulty(EntranceMap.Difficulty + 1);
        LabyrinthMap.SetAtmosphere(Codex.Atmospheres.nether);
        LabyrinthMap.SetTerminal(LevelIndex == LevelCount);

        var LabyrinthStructure = new DungeonStructure(LabyrinthMap);

        var LabyrinthLevel = LabyrinthSite.AddLevel(LevelIndex, LabyrinthMap);
        var LabyrinthRegion = LabyrinthMap.Region;

        foreach (var LabyrinthSquare in LabyrinthMap.GetSquares(LabyrinthRegion))
          Generator.PlaceSolidWall(LabyrinthSquare, LabyrinthBarrier, WallSegment.Cross);

        CreateMazePaths(LabyrinthMap, LabyrinthRegion);
        CreateMazeDetails(LabyrinthStructure, LabyrinthMap, LabyrinthBlock, LabyrinthRegion, BoxSize);

        var ExclusionSize = BoxSize * LevelIndex;
        var ExclusionMargin = (LabyrinthSize - ExclusionSize) / 2;
        var ExclusionRegion = new Region(ExclusionMargin, ExclusionMargin, LabyrinthSize - ExclusionMargin, LabyrinthSize - ExclusionMargin);

        // upstairs.
        var LevelUpSquare = LabyrinthMap.GetSquares().Where(S => Generator.CanPlacePortal(S) && !S.IsRegion(ExclusionRegion)).GetRandomOrNull();
        if (LevelUpSquare == null)
          LevelUpSquare = LabyrinthMap.GetSquares().Where(Generator.CanPlacePortal).ToArray().GetRandom();
        Generator.PlacePassage(LevelUpSquare, LabyrinthLevelUpPortal, Destination: null);

        // downstairs.
        var LevelDownSquare = LabyrinthMap.GetSquares().Where(S => Generator.CanPlacePortal(S) && S.AsRange(LevelUpSquare) >= ExclusionSize).GetRandomOrNull();
        if (LevelDownSquare == null)
          LevelDownSquare = LabyrinthMap.GetSquares().Where(Generator.CanPlacePortal).ToArray().GetRandom();
        Generator.PlacePassage(LevelDownSquare, LabyrinthLevelDownPortal, Destination: null);

        LabyrinthLevel.SetTransitions(LevelUpSquare, LevelDownSquare);

        if (LabyrinthMap.Terminal)
        {
          // replace some walls with lava.
          foreach (var InnerSquare in LabyrinthMap.GetSquares(LabyrinthRegion.Reduce(1)))
          {
            if (InnerSquare.Wall != null && Chance.OneIn10.Hit())
            {
              Generator.RemoveWall(InnerSquare);
              Generator.PlaceFloor(InnerSquare, Codex.Grounds.lava);
            }
          }
        }

        Generator.RepairVoid(LabyrinthMap, LabyrinthRegion);
        Generator.RepairWalls(LabyrinthMap, LabyrinthRegion);
        Generator.RepairZones(LabyrinthMap, LabyrinthRegion);
      }

      var CurrentSquare = EntranceSquare;

      foreach (var LabyrinthLevel in LabyrinthSite.Levels)
      {
        CurrentSquare.Passage.SetDestination(LabyrinthLevel.UpSquare);
        LabyrinthLevel.UpSquare.Passage.SetDestination(CurrentSquare);

        CurrentSquare = LabyrinthLevel.DownSquare;
      }

      Generator.PlacePassage(CurrentSquare, Codex.Portals.transportal, EntranceSquare);
      LabyrinthSite.LastLevel.SetTransitions(LabyrinthSite.LastLevel.UpSquare, null); // no way down any further.

      // minotaur boss, obviously.
      Generator.PlaceSpecificCharacter(CurrentSquare, Codex.Entities.minotaur);

      var BossCharacter = CurrentSquare.Character;

      Debug.Assert(BossCharacter != null, "Minotaur could not be generated?");

      if (BossCharacter != null)
      {
        var BossPromotion = EntranceMap.Difficulty - BossCharacter.Level + 5;
        if (BossPromotion > 0)
          Generator.PromoteCharacter(BossCharacter, BossPromotion);

        var Properties = Codex.Properties;
        var Elements = Codex.Elements;

        Generator.AcquireTalent(BossCharacter, Properties.polymorph_control, Properties.slippery, Properties.free_action, Properties.clarity);
        Generator.EnsureResistance(BossCharacter, Elements.fire, 100);
        Generator.EnsureResistance(BossCharacter, Elements.magical, 100);
        Generator.EnsureResistance(BossCharacter, Elements.poison, 100);

        // master competency in all skills.
        foreach (var Competency in BossCharacter.Competencies)
          Generator.RequireCompetency(BossCharacter, Competency.Skill, Codex.Qualifications.master);

        var ArtifactAsset = Generator.NewSpecificAsset(BossCharacter.Square, Codex.Items.wand_of_digging);
        BossCharacter.Inventory.Carried.Add(ArtifactAsset);
      }

      // artifact is placed under a boulder, as a homage to the original Hack's location of the Amulet of Yendor.
      Square GetJammedSquare() => CurrentSquare.Map.GetSquares().Where(S => Generator.CanPlaceBoulder(S) && S.IsJammed()).GetRandomOrNull();

      var JammedSquare = GetJammedSquare();
      if (JammedSquare != null)
      {
        var ArtifactAsset = Generator.GenerateUniqueAsset(JammedSquare);
        if (ArtifactAsset != null)
          Generator.PlaceAsset(JammedSquare, ArtifactAsset);

        Generator.PlaceBoulder(JammedSquare, LabyrinthBlock, IsRigid: true); // rigid so monsters don't move it around.

        // 5-8 decoy boulders.
        var DecoyDice = 1.d4() + 4;
        for (var DecoyIndex = 0; DecoyIndex < DecoyDice.Roll(); DecoyIndex++)
        {
          var DecoySquare = GetJammedSquare();
          if (DecoySquare != null)
            Generator.PlaceBoulder(DecoySquare, LabyrinthBlock, IsRigid: true); // rigid so monsters don't move it around.
        }
      }
      else if (BossCharacter != null)
      {
        // carrying the artifact instead.
        Generator.AcquireUnique(CurrentSquare, BossCharacter, Codex.Qualifications.master);
      }

      return true;
    }
    private bool CreateSokobanBranch(Square MazeSquare)
    {
      if (!Generator.CanPlacePortal(MazeSquare))
        return false;

      if (Generator.Adventure.World.HasSite(Generator.EscapedModuleTerm(NethackTerms.Sokoban)))
        return false;

      var MazeMap = MazeSquare.Map;

      var SokobanSite = Generator.Adventure.World.AddSite(Generator.EscapedModuleTerm(NethackTerms.Sokoban));

      var Specials = Official.Resources.Specials;

      var SokobanLevelArray = new[]
      {
        new { Number = 1, OptionArray = new [] { Specials.Sokoban1a, Specials.Sokoban1b, Specials.Sokoban1c, Specials.Sokoban1d, Specials.Sokoban1e  } },
        new { Number = 2, OptionArray = new [] { Specials.Sokoban2a, Specials.Sokoban2b, Specials.Sokoban2c, Specials.Sokoban2d, Specials.Sokoban2e, Specials.Sokoban2f, Specials.Sokoban2g  } },
        new { Number = 3, OptionArray = new [] { Specials.Sokoban3a, Specials.Sokoban3b, Specials.Sokoban3c, Specials.Sokoban3d, Specials.Sokoban3e, Specials.Sokoban3f, Specials.Sokoban3g  } },
        new { Number = 4, OptionArray = new [] { Specials.Sokoban4a, Specials.Sokoban4b, Specials.Sokoban4c, Specials.Sokoban4d } },
      };

      var SokobanBarrier = Codex.Barriers.jade_wall;
      var SokobanGround = Codex.Grounds.marble_floor;
      var SokobanGate = Codex.Gates.gold_door;
      var SokobanBlock = Codex.Blocks.gold_boulder;
      var SokobanUpPortal = Codex.Portals.jade_ladder_up;
      var SokobanDownPortal = Codex.Portals.jade_ladder_down;

      Square PreviousLevelSquare = null;

      foreach (var Level in SokobanLevelArray)
      {
        var Map = Level.OptionArray.GetRandom();

        var Grid = Generator.LoadSpecialGrid(Map);

        var LevelMapName = Generator.EscapedModuleTerm(NethackTerms.Sokoban) + " " + Level.Number;
        if (Generator.Adventure.World.HasMap(LevelMapName))
          return false;

        var SokobanMap = Generator.Adventure.World.AddMap(LevelMapName, Grid.Width, Grid.Height);
        SokobanMap.SetDifficulty(MazeMap.Difficulty + Level.Number);
        SokobanMap.SetAtmosphere(Codex.Atmospheres.dungeon);

        if (Level == SokobanLevelArray.First())
          SokobanMap.SetTerminal(true);

        var SokobanLevel = SokobanSite.AddLevel(SokobanLevelArray.Length - Level.Number + 1, SokobanMap);

        Square UpLevelSquare = null;

        var TreasureSquareList = new Inv.DistinctList<Square>();

        for (var Column = 0; Column < Grid.Width; Column++)
        {
          for (var Row = 0; Row < Grid.Height; Row++)
          {
            var SokobanSymbol = Grid[Column, Row];

            var SokobanSquare = SokobanMap[Column, Row];

            switch (SokobanSymbol)
            {
              case '.':
                // floor.
                Generator.PlaceFloor(SokobanSquare, SokobanGround);
                SokobanSquare.SetLit(true);
                break;

              case ' ':
                // void.
                break;

              case '-':
              case '|':
                // walls.
                Generator.PlacePermanentWall(SokobanSquare, SokobanBarrier, WallSegment.Cross);
                SokobanSquare.SetLit(true);
                break;

              case '+':
                // door.
                Generator.PlaceFloor(SokobanSquare, SokobanGround);

                if (SokobanGate == null)
                  Generator.PlaceIllusionaryWall(SokobanSquare, SokobanBarrier, WallSegment.Cross);
                else
                  Generator.PlaceLockedHorizontalDoor(SokobanSquare, SokobanGate, SokobanBarrier, Trap: Generator.NewTrap(Codex.Devices.fire_trap, Revealed: false));

                SokobanSquare.SetLit(true);
                break;

              case '<':
                // up ladder.
                Generator.PlaceFloor(SokobanSquare, SokobanGround);

                UpLevelSquare = SokobanSquare;

                SokobanLevel.SetTransitions(SokobanSquare, SokobanLevel.DownSquare);

                SokobanSquare.SetLit(true);
                break;

              case '>':
                // down ladder.
                Generator.PlaceFloor(SokobanSquare, SokobanGround);

                var DestinationSquare = PreviousLevelSquare != null && PreviousLevelSquare.Map != SokobanMap ? PreviousLevelSquare : MazeSquare;

                Generator.PlacePassage(DestinationSquare, SokobanUpPortal, SokobanSquare);
                Generator.PlacePassage(SokobanSquare, SokobanDownPortal, DestinationSquare);

                SokobanLevel.SetTransitions(SokobanLevel.UpSquare, SokobanSquare);

                SokobanSquare.SetLit(true);

                SokobanSquare.InsertTrigger().Add(Delay.Zero, Codex.Tricks.complete_mapping);
                break;

              case '0':
                Generator.PlaceFloor(SokobanSquare, SokobanGround);
                Generator.PlaceBoulder(SokobanSquare, SokobanBlock, IsRigid: true);

                SokobanSquare.SetLit(true);
                break;

              case '^':
                Generator.PlaceFloor(SokobanSquare, SokobanGround);
                Generator.PlaceTrap(SokobanSquare, PreviousLevelSquare == null ? Codex.Devices.pit : Codex.Devices.hole, Revealed: true);

                SokobanSquare.SetLit(true);
                break;

              case '?':
                // scroll of earth.
                Generator.PlaceFloor(SokobanSquare, SokobanGround);
                Generator.PlaceSpecificAsset(SokobanSquare, Codex.Items.scroll_of_earth);

                SokobanSquare.SetLit(true);
                break;

              case '#':
                // TODO: iron bars.
                Generator.PlacePermanentWall(SokobanSquare, SokobanBarrier, WallSegment.Cross);
                SokobanSquare.SetLit(true);
                break;

              case '}':
                // water.
                Generator.PlaceFloor(SokobanSquare, Codex.Grounds.water);
                SokobanSquare.SetLit(true);

                if (Generator.CanPlaceAsset(SokobanSquare) && Chance.OneIn10.Hit())
                  Generator.PlaceSpecificAsset(SokobanSquare, Codex.Items.kelp_frond);
                break;

              case '$':
                Generator.PlaceFloor(SokobanSquare, SokobanGround);
                SokobanSquare.SetLit(true);

                TreasureSquareList.Add(SokobanSquare);
                break;

              case 'z':
                Generator.PlaceFloor(SokobanSquare, SokobanGround);
                Generator.PlaceRandomCharacter(SokobanSquare); // this is a zoo.
                Generator.DropCoins(SokobanSquare, Generator.RandomCoinQuantity(SokobanSquare));

                var ZooCharacter = SokobanSquare.Character;
                if (ZooCharacter != null)
                  SnoozeCharacter(ZooCharacter);

                SokobanSquare.SetLit(true);
                break;

              default:
                Debug.Fail($"Sokoban symbol not handled: {SokobanSymbol} {(int)SokobanSymbol}");

                Generator.PlaceFloor(SokobanSquare, SokobanGround);
                break;
            }
          }
        }

        Generator.RepairVoid(SokobanMap, SokobanMap.Region);
        Generator.RepairWalls(SokobanMap, SokobanMap.Region);
        Generator.RepairDoors(SokobanMap, SokobanMap.Region);
        Generator.RepairZones(SokobanMap, SokobanMap.Region);

        foreach (var Zone in SokobanMap.Zones)
        {
          Zone.SetAccessRestricted(true);
          Zone.SetSpawnRestricted(true);
        }

        if (TreasureSquareList.Count > 0)
        {
          var ReturnPortalSquare = TreasureSquareList.GetRandomOrNull();
          if (ReturnPortalSquare != null)
            Generator.PlacePassage(ReturnPortalSquare, Codex.Portals.transportal, MazeSquare);

          var Cursed = Codex.Sanctities.Cursed;

          var BagSquare = TreasureSquareList.GetRandomOrNull();
          var BagItem = Codex.Items.List.Where(I => I.Type == ItemType.Bag).Where(I => I.DefaultSanctity != Cursed && !I.Grade.Unique && I.DowngradeItem != null).ToProbability(I => I.Rarity).GetRandomOrNull();
          if (BagSquare != null && BagItem != null)
          {
            Generator.PlaceSpecificAsset(BagSquare, BagItem);
            TreasureSquareList.Remove(BagSquare);
          }

          var CloakSquare = TreasureSquareList.GetRandomOrNull();
          var CloakItem = Codex.Items.List.Where(I => I.Type == ItemType.Cloak).Where(I => I.DefaultSanctity != Cursed && !I.Grade.Unique && I.Equip.HasEffects()).ToProbability(I => I.Rarity).GetRandomOrNull();
          if (CloakSquare != null && CloakItem != null)
          {
            Generator.PlaceSpecificAsset(CloakSquare, CloakItem);
            TreasureSquareList.Remove(CloakSquare);
          }

          var AmuletSquare = TreasureSquareList.GetRandomOrNull();
          var AmuletItem = Codex.Items.List.Where(I => I.Type == ItemType.Amulet).Where(I => I.DefaultSanctity != Cursed && !I.Grade.Unique && I.Equip.HasEffects()).ToProbability(I => I.Rarity).GetRandomOrNull();
          if (AmuletSquare != null && AmuletItem != null)
          {
            Generator.PlaceSpecificAsset(AmuletSquare, AmuletItem);
            TreasureSquareList.Remove(AmuletSquare);
          }
        }

        var FloorSquareList = SokobanMap.GetSquares().Where(S => Generator.CanPlaceAsset(S) && S.Trap == null).ToDistinctList();

        // 1 x ring.
        var RingSquare = FloorSquareList.GetRandomOrNull();
        var RingItem = Codex.Items.List.Where(I => I.Type == ItemType.Ring && !I.Grade.Unique).ToProbability(I => I.Rarity).GetRandomOrNull();
        if (RingSquare != null && RingItem != null)
        {
          Generator.PlaceSpecificAsset(RingSquare, RingItem);
          FloorSquareList.Remove(RingSquare);
        }

        // 1 x wand.
        var WandSquare = FloorSquareList.GetRandomOrNull();
        var WandItem = Codex.Items.List.Where(I => I.Type == ItemType.Wand && !I.Grade.Unique).ToProbability(I => I.Rarity).GetRandomOrNull();
        if (WandSquare != null && WandItem != null)
        {
          Generator.PlaceSpecificAsset(WandSquare, WandItem);
          FloorSquareList.Remove(WandSquare);
        }

        // 5 x comestible + one huge chunk of meat.
        var FoodProbability = Codex.Items.List.Where(I => I.Type == ItemType.Food && !I.Grade.Unique).ToProbability(I => I.Rarity);

        for (var FoodIndex = 0; FoodIndex < 6; FoodIndex++)
        {
          var FoodSquare = FloorSquareList.GetRandomOrNull();
          var FoodItem = FoodProbability.GetRandomOrNull();

          if (FoodSquare != null && FoodItem != null)
          {
            Generator.PlaceSpecificAsset(FoodSquare, FoodItem);
            FloorSquareList.Remove(FoodSquare);
          }
        }

        var MeatSquare = FloorSquareList.GetRandomOrNull();
        if (MeatSquare != null)
        {
          Generator.PlaceSpecificAsset(MeatSquare, Codex.Items.huge_chunk_of_meat);
          FloorSquareList.Remove(MeatSquare);
        }

        PreviousLevelSquare = UpLevelSquare;
      }

      return true;
    }
    private bool CreateFortLudiosBranch(DungeonRoom VaultRoom)
    {
      var PortalSquare = VaultRoom.GetFloorSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();
      if (PortalSquare == null)
        return false;

      if (Generator.Adventure.World.HasSite(Generator.EscapedModuleTerm(NethackTerms.Fort_Ludios)))
        return false;

      var FortItems = Codex.Items;
      var FortEntities = Codex.Entities;

      var FortSoldierArray = FortEntities.List.Where(E => E.Startup.HasSkill(Codex.Skills.firearms) && !E.IsMercenary && E.IsEncounter).OrderBy(E => E.Level).ToArray();
      // possible from level 8 onwards (soldier is level 8).
      Debug.Assert(FortSoldierArray.Length > 0 && FortSoldierArray.Min(E => E.Difficulty) <= VaultRoom.Map.Difficulty);

      var FortFirearmArray = FortItems.List.Where(I => I.Weapon != null && I.IsAbolitionCandidate() && !I.Grade.Unique && I.Price > Gold.One).ToArray();

      var VaultMap = VaultRoom.Map;

      var FortSite = Generator.Adventure.World.AddSite(Generator.EscapedModuleTerm(NethackTerms.Fort_Ludios));

      var FortArray = new[]
      {
        Official.Resources.Specials.FortLudios1,
        Official.Resources.Specials.FortLudios2,
        Official.Resources.Specials.FortLudios3,
        Official.Resources.Specials.FortLudios4,
        Official.Resources.Specials.FortLudios5
      };

      var FortGrid = Generator.LoadSpecialGrid(FortArray.GetRandom());
      var FortWidth = FortGrid.Width;
      var FortHeight = FortGrid.Height;
      var FortMap = Generator.Adventure.World.AddMap(Generator.EscapedModuleTerm(NethackTerms.Fort_Ludios), FortWidth, FortHeight);
      FortMap.SetDifficulty(VaultMap.Difficulty);
      FortMap.SetAtmosphere(Codex.Atmospheres.civilisation);

      var FortLevel = FortSite.AddLevel(1, FortMap);

      var FortGate = Codex.Gates.wooden_door;
      var FortBarrier = Codex.Barriers.stone_wall;
      var FortRoomGround = Codex.Grounds.stone_floor;
      var FortPathGround = Codex.Grounds.stone_path;
      var FortWaterGround = Codex.Grounds.water;

      var FortSoldierMaxChallenge = FortSoldierArray.Max(E => E.Challenge);
      var FortFirearmMaxEssence = FortFirearmArray.Max(I => I.Essence);

      // NOTE: deliberately zero probability for the highest level soldier to be generated.
      var FortSoldierProbability = FortSoldierArray.ToProbability(E => FortSoldierMaxChallenge - E.Challenge);
      var FortGemProbability = FortItems.List.Where(I => I.Type == ItemType.Gem && I.Price > Gold.One && !I.Grade.Unique).ToProbability(I => I.Rarity);
      var FortFirearmProbability = FortFirearmArray.ToProbability(I => (FortFirearmMaxEssence - I.Essence).GetUnits() + 1);
      var FortDogProbability = new[] { FortEntities.pit_bull, FortEntities.large_dog, FortEntities.dog, FortEntities.death_dog, FortEntities.hell_hound }.ToProbability(I => I.Frequency);
      var FortFoodProbability = new[] { FortItems.cration, FortItems.kration }.ToProbability(I => I.Rarity);

      for (var Row = 0; Row < FortHeight; Row++)
      {
        for (var Column = 0; Column < FortWidth; Column++)
        {
          var FortSquare = FortMap[Column, Row];

          var FortSymbol = FortGrid[Column, Row];

          switch (FortSymbol)
          {
            case ' ':
              // void.
              break;

            case '|':
            case '-':
              // walls.
              Generator.PlacePermanentWall(FortSquare, FortBarrier, WallSegment.Cross);
              FortSquare.SetLit(true);
              break;

            case 'x':
              // inside floor.
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              FortSquare.SetLit(true);
              break;

            case '.':
              // outside floor.
              Generator.PlaceFloor(FortSquare, FortPathGround);
              FortSquare.SetLit(false);
              break;

            case '}':
              // water.
              Generator.PlaceFloor(FortSquare, FortWaterGround);
              FortSquare.SetLit(false);

              if (Generator.CanPlaceAsset(FortSquare) && Chance.OneIn10.Hit())
                Generator.PlaceSpecificAsset(FortSquare, Codex.Items.kelp_frond);
              break;

            case ';':
              // water monster.
              Generator.PlaceFloor(FortSquare, FortWaterGround);
              Generator.PlaceRandomCharacter(FortSquare);
              FortSquare.SetLit(false);
              break;

            case '*':
              // precious gems.
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              Generator.PlaceSpecificAsset(FortSquare, FortGemProbability.GetRandomOrNull());
              FortSquare.SetLit(true);
              break;

            case ')':
              // firearms.
              Generator.PlaceFloor(FortSquare, FortRoomGround);

              var FirearmItem = FortFirearmProbability.GetRandomOrNull();
              if (Generator.Adventure.Abolition && FirearmItem.IsAbolitionCandidate())
                FirearmItem = Generator.AbolitionReplacement(FirearmItem);

              Generator.PlaceSpecificAsset(FortSquare, FirearmItem);
              FortSquare.SetLit(true);
              break;

            case '%':
              // rations.
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              Generator.PlaceSpecificAsset(FortSquare, FortFoodProbability.GetRandomOrNull());
              FortSquare.SetLit(true);
              break;

            case '(':
              // chests.
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              var ChestAsset = Generator.NewSpecificAsset(FortSquare, Codex.Items.chest);
              Generator.PlaceAsset(FortSquare, ChestAsset);

              ChestAsset.Container.Stash.Add(Generator.CreateCoins(FortSquare, Generator.RandomCoinQuantity(FortSquare) * 1.d3().Roll()));
              ChestAsset.Container.Stash.Add(Generator.NewSpecificAsset(FortSquare, FortGemProbability.GetRandomOrNull()));

              FortSquare.SetLit(true);
              break;

            case '^':
              // start portal.
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              Generator.PlacePassage(PortalSquare, Codex.Portals.transportal, FortSquare);
              Generator.PlacePassage(FortSquare, Codex.Portals.transportal, PortalSquare);
              FortSquare.SetLit(true);
              break;

            case '+':
              // door.
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              Generator.PlaceLockedHorizontalDoor(FortSquare, FortGate, FortBarrier);
              FortSquare.SetLit(true);
              break;

            case 'S':
              // secret door.
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              Generator.PlaceClosedHorizontalDoor(FortSquare, FortGate, FortBarrier, Secret: true);
              FortSquare.SetLit(true);
              break;

            case '$':
              // coins and traps.
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              Generator.DropCoins(FortSquare, Generator.RandomCoinQuantity(FortSquare) * 4.d4().Roll());
              FortSquare.SetLit(true);

              if (Chance.OneIn3.Hit())
                Generator.PlaceTrap(FortSquare, Chance.OneIn3.Hit() ? Codex.Devices.spiked_pit : Codex.Devices.explosive_trap, Revealed: false);
              break;

            case '@':
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              Generator.PlaceSpecificCharacter(FortSquare, FortSoldierProbability.GetRandomOrNull());

              var BarracksCharacter = FortSquare.Character;
              if (BarracksCharacter != null)
                SnoozeCharacter(BarracksCharacter);

              if (Chance.OneIn8.Hit())
                Generator.PlaceFixture(FortSquare, Codex.Features.bed);

              FortSquare.SetLit(true);
              break;

            case 'G':
              // outside soldiers.
              Generator.PlaceFloor(FortSquare, FortPathGround);
              Generator.PlaceSpecificCharacter(FortSquare, FortSoldierArray[0]); // lowest level.
              FortSquare.SetLit(false);
              break;

            case 'D':
              // outside dragons.
              Generator.PlaceFloor(FortSquare, FortPathGround);
              Generator.PlaceSpecificCharacter(FortSquare, Codex.Kinds.dragon);
              FortSquare.SetLit(false);
              break;

            case 'd':
              // inside dogs.
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              Generator.PlaceSpecificCharacter(FortSquare, FortDogProbability.GetRandomOrNull());
              FortSquare.SetLit(true);
              break;

            case 'o':
              // inside orcs.
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              Generator.PlaceSpecificCharacter(FortSquare, Codex.Races.orc);
              FortSquare.SetLit(true);
              break;

            case 'B':
              // inside boss.
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              Generator.PlaceSpecificCharacter(FortSquare, FortSoldierArray[FortSoldierArray.Length - 1]);
              Generator.PlaceFixture(FortSquare, Codex.Features.throne);
              FortSquare.SetLit(true);

              var FortCharacter = FortSquare.Character;
              if (FortCharacter != null)
              {
                var FortPromotion = FortMap.Difficulty - FortCharacter.Level + 5;
                if (FortPromotion > 0)
                  Generator.PromoteCharacter(FortCharacter, FortPromotion);

                var Properties = Codex.Properties;
                var Elements = Codex.Elements;

                Generator.AcquireTalent(FortCharacter, Properties.clarity, Properties.free_action, Properties.slippery, Properties.polymorph_control, Properties.see_invisible, Properties.vitality);
                Generator.EnsureResistance(FortCharacter, Elements.drain, 100);
                Generator.EnsureResistance(FortCharacter, Elements.cold, 100);
                Generator.EnsureResistance(FortCharacter, Elements.fire, 100);
                Generator.EnsureResistance(FortCharacter, Elements.sleep, 100);
                Generator.EnsureResistance(FortCharacter, Elements.magical, 100);

                Generator.AcquireUnique(FortSquare, FortCharacter, Codex.Qualifications.proficient);
              }

              FortLevel.SetTransitions(null, FortSquare);
              break;

            case 't':
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              Generator.PlaceRandomCharacter(FortSquare); // this is a throne room.

              var ThroneCharacter = FortSquare.Character;
              if (ThroneCharacter != null)
                SnoozeCharacter(ThroneCharacter);

              FortSquare.SetLit(true);
              break;

            case 'z':
              Generator.PlaceFloor(FortSquare, FortRoomGround);
              Generator.PlaceRandomCharacter(FortSquare); // this is a zoo.
              Generator.DropCoins(FortSquare, Generator.RandomCoinQuantity(FortSquare));

              var ZooCharacter = FortSquare.Character;
              if (ZooCharacter != null)
                SnoozeCharacter(ZooCharacter);

              FortSquare.SetLit(true);
              break;

            default:
              Debug.Fail($"Fort symbol not handled: {FortSymbol} {(int)FortSymbol}");

              Generator.PlaceFloor(FortSquare, FortRoomGround);
              break;
          }
        }
      }

      Generator.RepairVoid(FortMap, FortMap.Region);
      Generator.RepairWalls(FortMap, FortMap.Region);
      Generator.RepairDoors(FortMap, FortMap.Region);
      Generator.RepairZones(FortMap, FortMap.Region);

      foreach (var Zone in FortMap.Zones)
      {
        Zone.SetAccessRestricted(true);
        Zone.SetSpawnRestricted(true);
      }

      var CacheGate = Codex.Gates.wooden_door;

      var CacheWidth = 12;
      var CacheHeight = 12;
      var CacheMap = Generator.Adventure.World.AddMap(Generator.EscapedModuleTerm(NethackTerms.Fort_Ludios_Cache), CacheWidth, CacheHeight);
      CacheMap.SetDifficulty(VaultMap.Difficulty);
      CacheMap.SetTerminal(true);
      CacheMap.SetAtmosphere(Codex.Atmospheres.civilisation);

      var CacheLevel = FortSite.AddLevel(2, CacheMap);
      var CacheGrid = Generator.LoadSpecialGrid(Official.Resources.Specials.FortLudiosCache);

      for (var Row = 0; Row < CacheHeight; Row++)
      {
        for (var Column = 0; Column < CacheWidth; Column++)
        {
          var CacheSquare = CacheMap[Column, Row];
          var CacheSymbol = CacheGrid[Column, Row];

          switch (CacheSymbol)
          {
            case '|':
            case '-':
              // walls.
              Generator.PlacePermanentWall(CacheSquare, FortBarrier, WallSegment.Cross);
              CacheSquare.SetLit(false);
              break;

            case '.':
              // room.
              Generator.PlaceFloor(CacheSquare, FortRoomGround);
              CacheSquare.SetLit(false);
              break;

            case '#':
              // corridor.
              Generator.PlaceFloor(CacheSquare, FortPathGround);
              CacheSquare.SetLit(false);
              break;

            case '@':
              Generator.PlaceFloor(CacheSquare, FortRoomGround);
              Generator.PlaceSpecificCharacter(CacheSquare, FortSoldierProbability.GetRandomOrNull());

              var BarracksCharacter = CacheSquare.Character;
              if (BarracksCharacter != null)
                SnoozeCharacter(BarracksCharacter);

              CacheSquare.SetLit(false);
              break;

            case '+':
              // door.
              Generator.PlaceFloor(CacheSquare, FortRoomGround);
              Generator.PlaceClosedHorizontalDoor(CacheSquare, FortGate, FortBarrier);
              CacheSquare.SetLit(false);
              break;

            case 'S':
              // secret door.
              Generator.PlaceFloor(CacheSquare, FortRoomGround);
              Generator.PlaceClosedHorizontalDoor(CacheSquare, CacheGate, FortBarrier, Secret: true);
              CacheSquare.SetLit(false);
              break;

            case '*':
              // gems.
              Generator.PlaceFloor(CacheSquare, FortRoomGround);
              Generator.PlaceSpecificAsset(CacheSquare, FortGemProbability.GetRandomOrNull());
              CacheSquare.SetLit(false);
              break;

            case '(':
              // chests.
              Generator.PlaceFloor(CacheSquare, FortRoomGround);
              PlaceContainer(CacheSquare, Locked: true, Trapped: true);
              CacheSquare.SetLit(false);
              break;

            case 'z':
              Generator.PlaceFloor(CacheSquare, FortRoomGround);
              Generator.PlaceRandomCharacter(CacheSquare); // this is a zoo.
              Generator.DropCoins(CacheSquare, Generator.RandomCoinQuantity(CacheSquare) * 4.d4().Roll());

              var ZooCharacter = CacheSquare.Character;
              if (ZooCharacter != null)
                SnoozeCharacter(ZooCharacter);

              CacheSquare.SetLit(false);
              break;

            case '^':
              // exit portal.
              Generator.PlaceFloor(CacheSquare, FortRoomGround);
              Generator.PlacePassage(CacheSquare, Codex.Portals.transportal, PortalSquare);
              CacheSquare.SetLit(true);

              CacheLevel.SetTransitions(CacheSquare, null);
              break;

            case 'D':
              // surprise dragon.
              Generator.PlaceFloor(CacheSquare, FortRoomGround);
              Generator.PlaceSpecificCharacter(CacheSquare, Codex.Kinds.dragon);
              CacheSquare.SetLit(false);
              break;

            default:
              Debug.Fail($"Cache symbol not handled: {CacheSymbol} {(int)CacheSymbol}");

              Generator.PlaceFloor(CacheSquare, FortRoomGround);
              break;
          }
        }
      }

      Generator.RepairVoid(CacheMap, CacheMap.Region);
      Generator.RepairWalls(CacheMap, CacheMap.Region);
      Generator.RepairDoors(CacheMap, CacheMap.Region);

      // entry point.
      var CacheZone = CacheMap.AddZone();
      CacheZone.AddSquare(CacheMap[8, 4]);

      Generator.RepairZones(CacheMap, CacheMap.Region);

      foreach (var Zone in CacheMap.Zones.Except(CacheZone))
      {
        Zone.SetAccessRestricted(true);
        Zone.SetSpawnRestricted(true);
      }

      return true;
    }
    private bool CreateElfKingdomBranch(DungeonRoom EntryRoom)
    {
      var PortalSquare = EntryRoom.GetFloorSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();
      if (PortalSquare == null)
        return false;

      if (!Generator.CanPlacePortal(PortalSquare))
        return false;

      if (Generator.Adventure.World.HasSite(Generator.EscapedModuleTerm(NethackTerms.Elf_Kingdom)))
        return false;

      var StandardGeneration = Chance.ThreeIn4.Hit(); // 75% normal, 25% massacre.
#if DEBUG
      //StandardGeneration = false;
#endif

      var ResourceFile = StandardGeneration ? Official.Resources.Quests.Kingdom1 : Official.Resources.Quests.Kingdom2;

      var Quest = Generator.ImportQuest(ResourceFile.Load().GetBuffer());
      var QuestSite = Quest.World.Sites.Single();
      var QuestStart = Quest.World.Start;

      var KingdomSite = Generator.Adventure.World.AddSite(Generator.EscapedModuleTerm(NethackTerms.Elf_Kingdom));

      var KingdomDifficulty = EntryRoom.Map.Difficulty + 1;

      var TownParty = Generator.NewParty(Leader: null);

      void GenerateMap(Map KingdomMap)
      {
        KingdomMap.RotateRandomDegrees();

        var IsTown = KingdomMap.Name == "Elf Town";
        var IsPalace1 = KingdomMap.Name.StartsWith("Elf Palace 1");

        KingdomMap.SetName(Generator.EscapedModuleTerm(KingdomMap.Name));

        if (IsPalace1)
        {
          var PentagramZone = KingdomMap.AddZone();
          PentagramZone.AddRegion(new Region(1, 6, 6, 11));
          Debug.Assert(PentagramZone.Squares.Count > 0);

          var Trigger = PentagramZone.InsertTrigger();

          var SummonDelay = 1.d60();

          for (var Repeat = 0; Repeat < 2.d3().Roll(); Repeat++)
          {
            foreach (var PentagramSquare in PentagramZone.Squares.Where(S => S.Fixture != null))
              Trigger.Add(Delay.FromTurns(SummonDelay.Roll()), Codex.Tricks.summoning_demons).SetTarget(PentagramSquare);
          }
        }

        Generator.BuildMap(KingdomMap);

        // randomised shops per level.
        var KingdomShopProbability = ShopProbability.Clone();

        Shop NextRandomShop()
        {
          if (KingdomShopProbability.Checks.Count == 0)
            KingdomShopProbability = ShopProbability.Clone(); 

          return KingdomShopProbability.RemoveRandomOrNull();
        }

        foreach (var KingdomSquare in KingdomMap.GetSquares())
        {
          if (KingdomSquare.Passage != null && KingdomSquare.Passage.Destination == null && KingdomSquare != QuestStart && KingdomSquare.Passage.Portal == Codex.Portals.transportal)
          {
            if (KingdomMap.Level.Index == 1)
              KingdomSquare.Passage.SetDestination(PortalSquare);
            else
              Generator.RemovePassage(KingdomSquare);
          }

          var Shrine = KingdomSquare.Character?.Resident?.Shrine;
          if (Shrine != null)
          {
            // force a zone so we can have a visit trigger.
            var ShrineZone = KingdomMap.AddZone();
            ShrineZone.ForceRegion(KingdomSquare.FindBoundary());
            Debug.Assert(ShrineZone.Squares.Count > 0);

            ShrineZone.InsertTrigger().Add(Delay.Zero, Codex.Tricks.VisitShrineArray[Shrine.Index]).SetTarget(KingdomSquare);
          }

          if (KingdomSquare.Character?.Resident?.Shop != null && KingdomSquare.Fixture?.Container != null)
          {
            // all shops are randomised.
            var Shop = NextRandomShop();
            if (Shop != null)
            {
              // replace the shop.
              Generator.ResidentShop(KingdomSquare.Character, KingdomSquare, Shop);

              // force a shop zone.
              var ShopZone = KingdomMap.AddZone();
              ShopZone.ForceRegion(KingdomSquare.FindBoundary());
              Debug.Assert(ShopZone.Squares.Count > 0);

              var ShopStall = KingdomSquare.Fixture.Container;

              ShopStall.Stash.RemoveAll(); // delete what build map produced.

              if (StandardGeneration)
              {
                // visit trigger.
                ShopZone.InsertTrigger().Add(Delay.Zero, Codex.Tricks.VisitShopArray[Shop.Index]).SetTarget(KingdomSquare);

                // the shop is fully stocked.
                Generator.StockShop(KingdomSquare, ShopStall, Shop, (2.d4() + 4).Roll());
              }
              else
              {
                // kill the merchant and drop a corpse.
                Generator.CorpseSquare(KingdomSquare);

                // the shop is ransacked.
                Generator.StockShop(KingdomSquare, ShopStall, Shop, (1.d3() + 2).Roll());
              }
            }
          }
          else if (KingdomSquare.Fixture?.Feature == Codex.Features.stall)
          {
            // abandoned shop, no keeper, but some left over stock, unrelated to the nearby shop.
            Generator.StockShop(KingdomSquare, KingdomSquare.Fixture.Container, NextRandomShop(), (1.d3() + 2).Roll());
          }

          if (!StandardGeneration && KingdomSquare.Character?.Entity == Codex.Entities.high_elf)
          {
            if (IsTown)
              Generator.CorpseSquare(KingdomSquare); // murder all high elves in the town
            else
              Generator.HostileCharacter(KingdomSquare.Character); // high elves are hostile in the palace.
          }

          if (!StandardGeneration && KingdomSquare.Character != null)
          {
            var UpgradeCharacter = KingdomSquare.Character;

            var Skills = Codex.Skills;
            var Qualifications = Codex.Qualifications;

            if (UpgradeCharacter.Entity == Codex.Entities.orc_king)
            {
              var Elements = Codex.Elements;

              Generator.EnsureResistance(UpgradeCharacter, Elements.cold, 100);
              Generator.EnsureResistance(UpgradeCharacter, Elements.fire, 100);
              Generator.EnsureResistance(UpgradeCharacter, Elements.magical, 100);
              Generator.EnsureResistance(UpgradeCharacter, Elements.poison, 100);
            }
            else if (UpgradeCharacter.Entity == Codex.Entities.orc_captain)
            {
              Generator.RequireCompetency(UpgradeCharacter, Skills.light_blade, Qualifications.master);
              Generator.RequireCompetency(UpgradeCharacter, Skills.medium_blade, Qualifications.master);
              Generator.RequireCompetency(UpgradeCharacter, Skills.heavy_blade, Qualifications.master);
              Generator.RequireCompetency(UpgradeCharacter, Skills.light_armour, Qualifications.master);
              Generator.RequireCompetency(UpgradeCharacter, Skills.medium_armour, Qualifications.master);
              Generator.RequireCompetency(UpgradeCharacter, Skills.heavy_armour, Qualifications.master);
            }
            else if (UpgradeCharacter.Entity == Codex.Entities.orc_shaman)
            {
              Generator.RequireCompetency(UpgradeCharacter, Skills.evocation, Qualifications.specialist);
            }
          }

          if (KingdomSquare.Fixture?.Feature == Codex.Features.throne && KingdomSquare.Character?.Entity == Codex.Entities.elf_king)
          {
            // the elf king is powerful.
            var KingCharacter = KingdomSquare.Character;

            var KingPromotion = KingdomMap.Difficulty - KingCharacter.Level + 5;
            if (KingPromotion > 0)
              Generator.PromoteCharacter(KingCharacter, KingPromotion);

            var Properties = Codex.Properties;
            var Elements = Codex.Elements;

            Generator.AcquireTalent(KingCharacter, Properties.free_action, Properties.invisibility, Properties.polymorph_control, Properties.slippery);
            Generator.EnsureResistance(KingCharacter, Elements.cold, 100);
            Generator.EnsureResistance(KingCharacter, Elements.fire, 100);
            Generator.EnsureResistance(KingCharacter, Elements.magical, 100);
            Generator.EnsureResistance(KingCharacter, Elements.poison, 100);

            KingCharacter.Inventory.Carried.Add(Generator.NewSpecificAsset(KingdomSquare, Codex.Items.potion_of_full_healing, 1));
            KingCharacter.Inventory.Carried.Add(Generator.NewSpecificAsset(KingdomSquare, Codex.Items.potion_of_extra_healing, 3));
            KingCharacter.Inventory.Carried.Add(Generator.NewSpecificAsset(KingdomSquare, Codex.Items.scroll_of_terror, 2));
            KingCharacter.Inventory.Carried.Add(Generator.NewSpecificAsset(KingdomSquare, Codex.Items.scroll_of_confusion, 2));
            KingCharacter.Inventory.Carried.Add(Generator.NewSpecificAsset(KingdomSquare, Codex.Items.potion_of_blindness, 1));
            KingCharacter.Inventory.Carried.Add(Generator.NewSpecificAsset(KingdomSquare, Codex.Items.potion_of_speed, 1));
            KingCharacter.Inventory.Carried.Add(Generator.NewSpecificAsset(KingdomSquare, Codex.Items.potion_of_sleeping, 1));
            KingCharacter.Inventory.Carried.Add(Generator.NewSpecificAsset(KingdomSquare, Codex.Items.potion_of_sickness, 1));

            // maximum knowledge of the known spells.
            var SpellArray = new[]
            {
              Codex.Spells.drain_life,
              Codex.Spells.cone_of_cold,
              Codex.Spells.poison_blast,
              Codex.Spells.ice_storm
            };

            foreach (var Spell in SpellArray)
              KingCharacter.Knowledge.LearnSpell(Spell, 4);

            // master competency in all skills.
            foreach (var Competency in KingCharacter.Competencies)
              Generator.RequireCompetency(KingCharacter, Competency.Skill, Codex.Qualifications.master);

            var SchoolSkillArray = SpellArray.Select(S => S.School.Skill).Distinct().ToArray();
            foreach (var SchoolSkill in SchoolSkillArray.Except(KingCharacter.Competencies.Select(C => C.Skill)))
              Generator.RequireCompetency(KingCharacter, SchoolSkill, Codex.Qualifications.master);

            if (!StandardGeneration)
              Generator.AcquireUnique(KingdomSquare, KingCharacter, Codex.Qualifications.master);
          }

          if (IsTown && KingdomSquare.Character != null && KingdomSquare.Character.Neutral)
          {
            // all neutrals in town are allied together.
            TownParty.AddAlly(KingdomSquare.Character, Clock.Zero, Delay.Zero);
          }

          if (KingdomSquare.Character?.Entity == Codex.Entities.straw_golem)
          {
            // paralysed for training students.
            Generator.AcquireTalent(KingdomSquare.Character, Codex.Properties.paralysis);
          }

          foreach (var Asset in KingdomSquare.GetAssets())
          {
            if (Asset.Item == Codex.Items.sandwich)
            {
              // sandwich is replaced by an artifact.
              Generator.RemoveAsset(KingdomSquare, Asset);

              if (StandardGeneration)
              {
                var KingAsset = Generator.GenerateUniqueAsset(KingdomSquare);
                if (KingAsset != null)
                  Generator.PlaceAsset(KingdomSquare, KingAsset);
              }
            }
            else if (Asset.Container != null)
            {
              // random container loot.
              if (Asset.Item.Type == ItemType.Chest && Asset.Container.Stash.Count == 0)
                StockContainer(KingdomSquare, Asset, Locked: true, Trapped: true);
            }
            else if (Asset.Item != Codex.Items.skeleton_key && Asset.Item.Type != ItemType.Corpse)
            {
              // randomise items.
              Generator.ReplaceRandomAsset(KingdomSquare, Asset);
            }
          }
        }
      }

      foreach (var QuestLevel in QuestSite.Levels)
      {
        var KingdomMap = QuestLevel.Map;
        KingdomMap.SetDifficulty(KingdomDifficulty + QuestLevel.Index);
        KingdomMap.SetAtmosphere(Codex.Atmospheres.forest);
        KingdomMap.SetTerminal(QuestLevel == QuestSite.LastLevel);

        Generator.Adventure.World.AddMap(KingdomMap);

        var KingdomLevel = KingdomSite.AddLevel(QuestLevel.Index, KingdomMap);
        KingdomLevel.SetTransitions(QuestLevel.UpSquare, QuestLevel.DownSquare);

        GenerateMap(KingdomMap);

        foreach (var QuestMap in QuestLevel.GetMaps().Except(KingdomMap))
        {
          QuestMap.SetDifficulty(KingdomMap.Difficulty);
          QuestMap.SetAtmosphere(Codex.Atmospheres.civilisation);

          Generator.Adventure.World.AddMap(QuestMap);

          QuestMap.SetLevel(KingdomLevel);

          GenerateMap(QuestMap);
        }
      }

      var EntryZone = EntryRoom.Map.AddZone();
      EntryZone.ForceSquare(PortalSquare); // so we can teleport directly onto the portal.
      Debug.Assert(EntryZone.Squares.Count > 0);

      Generator.PlacePassage(PortalSquare, Codex.Portals.transportal, QuestStart);
      Generator.PlacePassage(QuestStart, Codex.Portals.transportal, PortalSquare);

      foreach (var TreeSquare in PortalSquare.GetCompassSquares())
      {
        if (Generator.CanPlaceBoulder(TreeSquare))
        {
          Generator.PlaceFloor(TreeSquare, Codex.Grounds.dirt);
          Generator.PlaceSolidWall(TreeSquare, Codex.Barriers.tree, WallSegment.Cross);
        }
      }

      return true;
    }
    private bool CreateMedusaLairBranch(DungeonRoom EntryRoom)
    {
      var PortalSquare = EntryRoom.GetFloorSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();
      if (PortalSquare == null)
        return false;

      if (!Generator.CanPlacePortal(PortalSquare))
        return false;

      if (Generator.Adventure.World.HasSite(Generator.EscapedModuleTerm(NethackTerms.Medusa_Lair)))
        return false;

      var Quest = Generator.ImportQuest(Official.Resources.Quests.Lair.Load().GetBuffer());
      var QuestSite = Quest.World.Sites.Single();
      var QuestStart = Quest.World.Start;

      var LairSite = Generator.Adventure.World.AddSite(Generator.EscapedModuleTerm(NethackTerms.Medusa_Lair));

      var LairDifficulty = EntryRoom.Map.Difficulty + 1;

      var MercenaryProbability = Codex.Entities.List.Where(E => E.IsMercenary && E.IsEncounter).ToProbability(E => E.Frequency);

      var TreasureItemList = new Inv.DistinctList<Item>
      {
        Codex.Items.amulet_versus_stone,
        Codex.Items.shield_of_reflection,
        Codex.Items.blindfold,
        Codex.Items.helm_of_telepathy,
        Codex.Items.ring_of_free_action
      };

      Item GetTreasureItem()
      {
        var Result = TreasureItemList.GetRandomOrNull();

        if (Result != null)
          TreasureItemList.Remove(Result);

        return Result;
      }

      foreach (var QuestLevel in QuestSite.Levels)
      {
        var LairMap = QuestLevel.Map;
        LairMap.SetName(Generator.EscapedModuleTerm(NethackTerms.Medusa_Lair) + " " + QuestLevel.Index);
        LairMap.SetDifficulty(LairDifficulty + QuestLevel.Index);
        LairMap.SetAtmosphere(Codex.Atmospheres.forest);
        LairMap.SetTerminal(QuestLevel == QuestSite.LastLevel);

        Generator.Adventure.World.AddMap(LairMap);

        var LairLevel = LairSite.AddLevel(QuestLevel.Index, LairMap);
        LairLevel.SetTransitions(QuestLevel.UpSquare, QuestLevel.DownSquare);

        Generator.BuildMap(LairMap);

        foreach (var LairSquare in LairMap.GetSquares())
        {
          if (LairSquare.Zone != null)
          {
            LairSquare.Zone.SetAccessRestricted(true);
            LairSquare.Zone.SetSpawnRestricted(true);
          }

          if (LairSquare.Wall != null && LairSquare.Wall.IsPhysical())
            Generator.AdjustToPermanentWall(LairSquare);

          if (LairSquare.Boulder != null && LairSquare.Boulder.Block.Prison != null)
            Generator.ImprisonCharacter(LairSquare, Generator.NewCharacter(LairSquare, MercenaryProbability.GetRandom()));

          if (LairSquare.Passage != null && LairSquare.Passage.Destination == null)
            LairSquare.Passage.SetDestination(PortalSquare);

          foreach (var Asset in LairSquare.GetAssets())
          {
            if (Asset.Container != null)
            {
              // random container loot.
              if (Asset.Item.Type == ItemType.Chest && Asset.Container.Stash.Count == 0)
              {
                StockContainer(LairSquare, Asset, Locked: true, Trapped: true);

                if (QuestLevel == QuestSite.LastLevel)
                  Asset.Container.Stash.Add(Generator.NewSpecificAsset(LairSquare, GetTreasureItem()));
              }
            }
            else
            {
              // randomise items.
              Generator.ReplaceRandomAsset(LairSquare, Asset);
            }
          }

          var LairCharacter = LairSquare.Character;
          if (LairCharacter != null && LairCharacter.Entity == Codex.Entities.medusa)
          {
            var LairPromotion = LairMap.Difficulty - LairCharacter.Level + 5;
            if (LairPromotion > 0)
              Generator.PromoteCharacter(LairCharacter, LairPromotion);

            var Properties = Codex.Properties;
            var Elements = Codex.Elements;

            Generator.AcquireTalent(LairCharacter, Properties.clarity, Properties.life_regeneration, Properties.mana_regeneration, Properties.polymorph_control, Properties.see_invisible, Properties.vitality, Properties.slippery, Properties.free_action);
            Generator.EnsureResistance(LairCharacter, Elements.drain, 100);
            Generator.EnsureResistance(LairCharacter, Elements.cold, 100);
            Generator.EnsureResistance(LairCharacter, Elements.fire, 100);
            Generator.EnsureResistance(LairCharacter, Elements.sleep, 100);
            Generator.EnsureResistance(LairCharacter, Elements.magical, 100);

            // maximum knowledge of the known spells.
            var SpellArray = new[]
            {
              Codex.Spells.invisibility,
              Codex.Spells.haste,
              Codex.Spells.summoning,
              Codex.Spells.poison_blast,
              Codex.Spells.toxic_spray,
              Codex.Spells.darkness,
            };

            foreach (var Spell in SpellArray)
              LairCharacter.Knowledge.LearnSpell(Spell, 4);

            // master competency in all skills.
            foreach (var Competency in LairCharacter.Competencies)
              Generator.RequireCompetency(LairCharacter, Competency.Skill, Codex.Qualifications.master);

            var SchoolSkillArray = SpellArray.Select(S => S.School.Skill).Distinct().ToArray();
            foreach (var SchoolSkill in SchoolSkillArray.Except(LairCharacter.Competencies.Select(C => C.Skill)))
              Generator.RequireCompetency(LairCharacter, SchoolSkill, Codex.Qualifications.master);

            var ArtifactAsset = Generator.GenerateUniqueAsset(LairSquare);

            if (ArtifactAsset != null)
            {
              LairCharacter.Inventory.Carried.Add(ArtifactAsset);

              // medusa will not use the artifact because her attacks are better.
              //Generator.OutfitCharacter(LairCharacter);
            }
          }
        }
      }

      Generator.PlacePassage(PortalSquare, Codex.Portals.transportal, QuestStart);
      Generator.PlacePassage(QuestStart, Codex.Portals.transportal, PortalSquare);

      foreach (var StatueSquare in PortalSquare.GetCompassSquares())
      {
        if (Generator.CanPlaceBoulder(StatueSquare))
          Generator.PlaceBoulder(StatueSquare, Codex.Blocks.statue, IsRigid: false);
      }

      return true;
    }
    private bool CreateBlackMarketBranch(DungeonRoom EntryRoom)
    {
      var PortalSquare = EntryRoom.GetFloorSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();
      if (PortalSquare == null)
        return false;

      if (!Generator.CanPlacePortal(PortalSquare))
        return false;

      if (Generator.Adventure.World.HasSite(Generator.EscapedModuleTerm(NethackTerms.Black_Market)))
        return false;

      var Quest = Generator.ImportQuest(Official.Resources.Quests.Market.Load().GetBuffer());
      var QuestSite = Quest.World.Sites.Single();
      var QuestStart = Quest.World.Start;

      Generator.PlacePassage(PortalSquare, Codex.Portals.transportal, QuestStart);

      var MarketSite = Generator.Adventure.World.AddSite(Generator.EscapedModuleTerm(NethackTerms.Black_Market));

      var MarketDifficulty = EntryRoom.Map.Difficulty + 1;

      var MercenaryProbability = Codex.Entities.List.Where(E => E.IsMercenary && E.IsEncounter).ToProbability(E => E.Level); // higher level ones are preferred.

      // ignore probability for the black market, we want all the shops equally.
      var ShopList = Generator.GetShops().ToDistinctList();

      Shop GetShop()
      {
        if (ShopList.Count == 0)
          ShopList.AddRange(Generator.GetShops());

        var Result = ShopList.GetRandom();
        ShopList.Remove(Result);
        return Result;
      }

      var ItemDice = 2.d4() + 6;

      var QuestLevel = QuestSite.Levels.Single();

      var MarketMap = QuestLevel.Map;
      MarketMap.SetName(Generator.EscapedModuleTerm(NethackTerms.Black_Market));
      MarketMap.SetDifficulty(MarketDifficulty + QuestLevel.Index);
      MarketMap.SetAtmosphere(Codex.Atmospheres.civilisation);
      MarketMap.SetTerminal(true);

      Generator.Adventure.World.AddMap(MarketMap);

      var MarketLevel = MarketSite.AddLevel(QuestLevel.Index, MarketMap);
      MarketLevel.SetTransitions(QuestLevel.UpSquare, QuestLevel.DownSquare);

      Generator.BuildMap(MarketMap);

      var Party = Generator.NewParty(Leader: null);

      foreach (var MarketSquare in MarketMap.GetSquares().Where(S => !S.IsEmpty()))
      {
        MarketSquare.SetLit(true);

        // no teleport.
        if (MarketSquare.Zone != null)
        {
          MarketSquare.Zone.SetAccessRestricted(true);
          MarketSquare.Zone.SetSpawnRestricted(true);
        }

        // permanent walls.
        if (MarketSquare.Wall != null)
          Generator.AdjustToPermanentWall(MarketSquare);

        // return portal.
        if (MarketSquare.Passage != null)
          Generator.PlacePassage(MarketSquare, Codex.Portals.transportal, PortalSquare);

        // statues are black marketeers as well.
        if (MarketSquare.Boulder != null && MarketSquare.Boulder.Block.Prison != null)
          Generator.ImprisonCharacter(MarketSquare, Generator.NewCharacter(MarketSquare, MercenaryProbability.GetRandomOrNull()));

        // create a shop.
        if (MarketSquare.Fixture != null && MarketSquare.Fixture.Feature == Codex.Features.stall)
        {
          Generator.RemoveFixture(MarketSquare); // erase the current stall.

          Generator.PlaceShop(MarketSquare, GetShop(), ItemDice.Roll());
        }

        // all neutrals are allied.
        var Character = MarketSquare.Character;
        if (Character != null && Character.Neutral)
        {
          Party.AddAlly(Character, Clock.Zero, Delay.Zero);

          // guards are resident.
          if (Character.Entity == Codex.Entities.guard && !Character.IsResident())
            Generator.ResidentSquare(Character, MarketSquare);
        }

        // randomise items.
        foreach (var Asset in MarketSquare.GetAssets())
        {
          if (Asset.Container != null)
          {
            // random container loot.
            if (Asset.Item.Type == ItemType.Chest && Asset.Container.Stash.Count == 0)
            {
              StockContainer(MarketSquare, Asset, Locked: true, Trapped: true);

              Asset.Container.Stash.Add(Generator.CreateCoins(MarketSquare, 1000.d10().Roll())); // 1000-10,000 gold.
            }
          }
          else
          {
            // randomise items.
            Generator.ReplaceRandomAsset(MarketSquare, Asset);
          }
        }
      }

      return true;
    }
    private bool CreateLichTowerBranch(DungeonRoom AtticRoom)
    {
      var PortalSquare = AtticRoom.GetFloorSquares().Where(Generator.CanPlacePortal).GetRandomOrNull();
      if (PortalSquare == null)
        return false;

      if (!Generator.CanPlacePortal(PortalSquare))
        return false;

      if (Generator.Adventure.World.HasSite(Generator.EscapedModuleTerm(NethackTerms.Lich_Tower)))
        return false;

      var Quest = Generator.ImportQuest(Official.Resources.Quests.Tower.Load().GetBuffer());
      var QuestSite = Quest.World.Sites.Single();
      var QuestStart = Quest.World.Start;

      var TowerSite = Generator.Adventure.World.AddSite(Generator.EscapedModuleTerm(NethackTerms.Lich_Tower));

      var TowerDifficulty = AtticRoom.Map.Difficulty;

      foreach (var QuestLevel in QuestSite.Levels)
      {
        var TowerMap = QuestLevel.Map;
        TowerMap.SetName(Generator.EscapedModuleTerm(NethackTerms.Lich_Tower) + " " + QuestLevel.Index);
        TowerMap.SetDifficulty(TowerDifficulty + (QuestSite.Levels.Count - QuestLevel.Index));
        TowerMap.SetAtmosphere(Codex.Atmospheres.dungeon);

        Generator.Adventure.World.AddMap(TowerMap);

        var TowerLevel = TowerSite.AddLevel(QuestLevel.Index, TowerMap);
        TowerLevel.SetTransitions(QuestLevel.UpSquare, QuestLevel.DownSquare);

        Generator.BuildMap(TowerMap);

        foreach (var TowerSquare in TowerMap.GetSquares())
        {
          if (TowerSquare.Zone != null)
          {
            TowerSquare.Zone.SetAccessRestricted(true);
            TowerSquare.Zone.SetSpawnRestricted(true);
          }

          if (TowerSquare.Wall != null && TowerSquare.Wall.IsPhysical())
            Generator.AdjustToPermanentWall(TowerSquare);

          if (TowerSquare.Passage != null && TowerSquare.Passage.Destination == null && TowerSquare != QuestStart)
            TowerSquare.Passage.SetDestination(PortalSquare); // exit portal.

          foreach (var Asset in TowerSquare.GetAssets())
          {
            if (Asset.Container != null)
            {
              // random container loot.
              if (Asset.Item.Type == ItemType.Chest && Asset.Container.Stash.Count == 0)
              {
                StockContainer(TowerSquare, Asset, Locked: true, Trapped: true);

                // every chest contains a book - magical knowledge and power is the treasure.
                var ChestAsset = Generator.NewRandomAsset(TowerSquare, Codex.Stocks.book);
                if (ChestAsset != null)
                  Asset.Container.Stash.Add(ChestAsset);
              }
            }
            else
            {
              // randomise items.
              Generator.ReplaceRandomAsset(TowerSquare, Asset);
            }
          }

          var TowerCharacter = TowerSquare.Character;
          if (TowerCharacter != null && TowerCharacter.Entity == Codex.Entities.dracolich)
          {
            var TowerPromotion = TowerDifficulty - TowerCharacter.Level + 5;
            if (TowerPromotion > 0)
              Generator.PromoteCharacter(TowerCharacter, TowerPromotion);

            var Properties = Codex.Properties;
            var Elements = Codex.Elements;

            Generator.AcquireTalent(TowerCharacter, Properties.free_action, Properties.displacement, Properties.polymorph_control, Properties.slippery);
            Generator.EnsureResistance(TowerCharacter, Elements.fire, 100);

            // maximum knowledge of the known spells.
            var SpellArray = new[]
            {
              Codex.Spells.drain_life,
              Codex.Spells.haste,
              Codex.Spells.invisibility,
              Codex.Spells.summoning,
              Codex.Spells.lightning_bolt,
              Codex.Spells.cone_of_cold,
              Codex.Spells.fireball,
              Codex.Spells.ice_storm,
              Codex.Spells.magic_missile
            };

            foreach (var Spell in SpellArray)
              TowerCharacter.Knowledge.LearnSpell(Spell, 4);

            // master competency in all skills.
            foreach (var Competency in TowerCharacter.Competencies)
              Generator.RequireCompetency(TowerCharacter, Competency.Skill, Codex.Qualifications.master);

            var SchoolSkillArray = SpellArray.Select(S => S.School.Skill).Distinct().ToArray();
            foreach (var SchoolSkill in SchoolSkillArray.Except(TowerCharacter.Competencies.Select(C => C.Skill)))
              Generator.RequireCompetency(TowerCharacter, SchoolSkill, Codex.Qualifications.master);

            // downgrade enchantment to specialist.
            Generator.RequireCompetency(TowerCharacter, Codex.Skills.enchantment, Codex.Qualifications.specialist);

            var ArtifactAsset = Generator.GenerateUniqueAsset(TowerSquare);

            if (ArtifactAsset != null)
            {
              TowerCharacter.Inventory.Carried.Add(ArtifactAsset);

              // dracolich will not use the artifact.
              //Generator.OutfitCharacter(LairCharacter);
            }
          }
        }
      }

      Generator.PlacePassage(PortalSquare, Codex.Portals.transportal, QuestStart);
      Generator.PlacePassage(QuestStart, Codex.Portals.transportal, PortalSquare);

      return true;
    }
    private bool CreateAbyssBranch(Square PortalSquare)
    {
      if (!Generator.CanPlacePortal(PortalSquare))
        return false;

      if (Generator.Adventure.World.HasSite(Generator.EscapedModuleTerm(NethackTerms.Abyss)))
        return false;

      var Quest = Generator.ImportQuest(Official.Resources.Quests.Abyss.Load().GetBuffer());
      var QuestSite = Quest.World.Sites.Single();
      var QuestStart = Quest.World.Start;

      var AbyssSite = Generator.Adventure.World.AddSite(Generator.EscapedModuleTerm(NethackTerms.Abyss));
      AbyssSite.SetWarped(true); // prevent this site from being warped.

      var AbyssDifficulty = PortalSquare.Map.Difficulty + 1;

      Generator.PlacePassage(PortalSquare, Codex.Portals.rift, QuestStart);
      Generator.PlacePassage(QuestStart, Codex.Portals.rift, PortalSquare);

      void GenerateMap(Map AbyssMap)
      {
        AbyssMap.RotateRandomDegrees();

        Generator.BuildMap(AbyssMap);

        foreach (var AbyssSquare in AbyssMap.GetSquares())
        {
          // allow teleporting in this branch.
          //if (Square.Zone != null)
          //  Square.Zone.SetRestricted(true);

          if (AbyssSquare.Wall != null && AbyssSquare.Wall.IsPhysical())
            Generator.AdjustToPermanentWall(AbyssSquare);

          if (AbyssSquare.Passage != null && AbyssSquare.Passage.Destination == null && AbyssSquare != QuestStart)
            AbyssSquare.Passage.SetDestination(PortalSquare); // exit rift.

          foreach (var Asset in AbyssSquare.GetAssets())
          {
            if (Asset.Container != null)
            {
              var ContainerItem = Asset.Item;

              // random container loot.
              if (ContainerItem.Type == ItemType.Chest && Asset.Container.Stash.Count == 0)
              {
                var Trapped = Chance.OneIn3.Hit(); // 1 in 3 chests are trapped.
                var Locked = Trapped || !Chance.OneIn3.Hit(); // all trapped chests are locked, 2 in 3 untrapped chests are also locked.
                StockContainer(AbyssSquare, Asset, Locked, Trapped);

                // chests are considered 'rich' and are double-stocked with items.
                if (ContainerItem == Codex.Items.chest)
                  Generator.StockContainer(AbyssSquare, Asset);
              }
            }
            else
            {
              // randomise items.
              Generator.ReplaceRandomAsset(AbyssSquare, Asset);
            }
          }

          var AbyssCharacter = AbyssSquare.Character;
          if (AbyssCharacter != null)
          {
            var Properties = Codex.Properties;
            var Elements = Codex.Elements;

            // minions.
            Generator.EnsureResistance(AbyssCharacter, Elements.drain, 100);
            Generator.EnsureResistance(AbyssCharacter, Elements.fire, 100);
            Generator.EnsureResistance(AbyssCharacter, Elements.poison, 100);

            var IsBoss = AbyssCharacter.Entity == Codex.Entities.nabassu;

            if (AbyssCharacter.HasAcquiredTalent(Properties.sleeping) || IsBoss)
            {
              // flunkies.
              Generator.ReleaseTalent(AbyssCharacter, Properties.sleeping);
              Generator.AcquireTalent(AbyssCharacter, Properties.polymorph_control);
              Generator.AcquireTalent(AbyssCharacter, Properties.slippery);

              Generator.EnsureResistance(AbyssCharacter, Elements.magical, 100);
              Generator.EnsureResistance(AbyssCharacter, Elements.sleep, 100);
              Generator.EnsureResistance(AbyssCharacter, Elements.petrify, 100);

              if (IsBoss)
              {
                // the boss.

                // resistances to all main elements.
                foreach (var Element in Elements.List.Where(E => E.IsResistance))
                  Generator.EnsureResistance(AbyssCharacter, Element, 100);

                Generator.AcquireTalent(AbyssCharacter, Properties.reflection, Properties.see_invisible, Properties.teleport_control, Properties.vitality);

                Asset NewAsset(Item Item, int Quantity, Sanctity Sanctity)
                {
                  var Result = Generator.NewSpecificAsset(AbyssSquare, Item, Quantity);
                  Generator.ChangeSanctity(Result, Sanctity);
                  return Result;
                }
                AbyssCharacter.Inventory.Carried.Add(NewAsset(Codex.Items.scroll_of_terror, 1, Codex.Sanctities.Blessed));
                AbyssCharacter.Inventory.Carried.Add(NewAsset(Codex.Items.scroll_of_teleportation, 1, Codex.Sanctities.Uncursed));
                AbyssCharacter.Inventory.Carried.Add(NewAsset(Codex.Items.potion_of_full_healing, 2, Codex.Sanctities.Blessed));
                AbyssCharacter.Inventory.Carried.Add(NewAsset(Codex.Items.potion_of_sickness, 1, Codex.Sanctities.Cursed));

                Generator.AcquireUnique(AbyssSquare, AbyssCharacter, Codex.Qualifications.master);
              }
            }
          }
        }
      }

      foreach (var QuestLevel in QuestSite.Levels)
      {
        var AbyssMap = QuestLevel.Map;
        AbyssMap.SetName(Generator.EscapedModuleTerm(AbyssMap.Name));
        AbyssMap.SetDifficulty(AbyssDifficulty + (QuestSite.Levels.Count - QuestLevel.Index));
        AbyssMap.SetAtmosphere(Codex.Atmospheres.nether);
        AbyssMap.SetTerminal(QuestLevel == QuestSite.LastLevel);

        Generator.Adventure.World.AddMap(AbyssMap);

        var AbyssLevel = AbyssSite.AddLevel(QuestLevel.Index, AbyssMap);
        AbyssLevel.SetTransitions(QuestLevel.UpSquare, QuestLevel.DownSquare);

        GenerateMap(AbyssMap);

        foreach (var QuestMap in QuestLevel.GetMaps().Except(AbyssMap))
        {
          QuestMap.SetName(Generator.EscapedModuleTerm(QuestMap.Name));
          QuestMap.SetDifficulty(AbyssMap.Difficulty);
          QuestMap.SetAtmosphere(AbyssMap.Atmosphere);
          QuestMap.SetTerminal(AbyssMap.Terminal);

          Generator.Adventure.World.AddMap(QuestMap);

          QuestMap.SetLevel(AbyssLevel);

          GenerateMap(QuestMap);
        }
      }

      return true;
    }
    private bool CreateSecretLevel(Square EntrySquare)
    {
      var SecretArray = new[]
      {
        //new { Grid = Official.Resources.Specials.AncientStronghold },
        //new { Grid = Official.Resources.Specials.AsmodeusLair },
        //new { Grid = Official.Resources.Specials.BaalzebubLair },
        //new { Grid = Official.Resources.Specials.CthulhuSanctum },
        //new { Grid = Official.Resources.Specials.DemogorgonLair },
        //new { Grid = Official.Resources.Specials.DemonLair },
        //new { Grid = Official.Resources.Specials.DispaterLair },
        //new { Grid = Official.Resources.Specials.GeryonLair },
        new { Grid = Official.Resources.Specials.GnollTown }, // 33x38
        new { Grid = Official.Resources.Specials.HallOfGiants }, // 22x36
        //new { Grid = Official.Resources.Specials.KoboldTown },
        new { Grid = Official.Resources.Specials.LostTomb }, // 28x26
        new { Grid = Official.Resources.Specials.MonsterLair }, // 14x49
        //new { Grid = Official.Resources.Specials.OrcusTown },
        //new { Grid = Official.Resources.Specials.PleasantValley },
        //new { Grid = Official.Resources.Specials.RatInfestation },
        //new { Grid = Official.Resources.Specials.RottingTemple },
        //new { Grid = Official.Resources.Specials.SecretLaboratory },
        //new { Grid = Official.Resources.Specials.SpiderCaves },
        //new { Grid = Official.Resources.Specials.SunlessSea },
        //new { Grid = Official.Resources.Specials.WyrmCaves },
        //new { Grid = Official.Resources.Specials.YeenoghuLair },
      };

      var Secret = SecretArray.GetRandom();

      var Grid = Generator.LoadSpecialGrid(Secret.Grid);

      return false;
    }

    private static T[,] RotateMatrix90Degrees<T>(T[,] matrix)
    {
      var n = matrix.GetLength(0);
      Debug.Assert(matrix.GetLength(1) == n);

      var ret = new T[n, n];

      for (var i = 0; i < n; ++i)
      {
        for (var j = 0; j < n; ++j)
          ret[i, j] = matrix[n - j - 1, i];
      }

      return ret;
    }

    [Conditional("VERBOSE")]
    private void DebugStart()
    {
      this.DebugBuilder = new StringBuilder();
    }
    [Conditional("VERBOSE")]
    private void DebugWrite(string Text)
    {
      DebugBuilder.Append(Text);
    }
    [Conditional("VERBOSE")]
    private void DebugStop()
    {
      Debug.WriteLine(DebugBuilder.ToString());
      this.DebugBuilder = null;
    }

    private StringBuilder DebugBuilder;
    private readonly Generator Generator;
    private readonly Codex Codex;
    private Inv.DistinctList<AtticTemplate> AtticTemplateList;
    private Inv.DistinctList<CaveTemplate> CaveTemplateList;
    private readonly Probability<Shop> ShopProbability;
    private readonly Probability<Shrine> ShrineProbability;
    private readonly Probability<Attraction> AttractionProbability;
    private int MinesIndex;
    private int SokobanIndex;
    private int FortIndex;
    private int ChambersIndex;
    private int LabyrinthIndex;
    private int LairIndex;
    private int KingdomIndex;
    private int TowerIndex;
    private int MarketIndex;
    private int AbyssIndex;

    private const int CavernSize = 6;

    private sealed class DungeonStructure
    {
      internal DungeonStructure(Map Map)
      {
        this.Map = Map;
        this.RoomList = [];
      }

      public Map Map { get; }

      /// <summary>
      /// Register a rectangular room on the map.
      /// </summary>
      /// <param name="Region"></param>
      /// <param name="Isolated">Isolated rooms will not be considered when placing the up/down passages</param>
      /// <returns></returns>
      public DungeonRoom AddRoom(Region Region, bool Isolated)
      {
        var Result = new DungeonRoom(Map, Region, Isolated);

        RoomList.Add(Result);

        return Result;
      }
      public void RemoveRoom(DungeonRoom Room)
      {
        RoomList.Remove(Room);
      }
      public void RemoveRooms()
      {
        RoomList.Clear();
      }

      public IReadOnlyList<DungeonRoom> Rooms => RoomList;

      private readonly Inv.DistinctList<DungeonRoom> RoomList;
    }

    private sealed class DungeonRoom
    {
      internal DungeonRoom(Map Map, Region Region, bool Isolated)
      {
        this.Map = Map;
        this.Region = Region;
        this.Isolated = Isolated;
      }

      public Map Map { get; }
      public Region Region { get; }
      /// <summary>
      /// Room cannot be used to place an up/down transition passage.
      /// </summary>
      public bool Isolated { get; }
      public Square Midpoint => Map[Region.Left + ((Region.Right - Region.Left) / 2), Region.Top + ((Region.Bottom - Region.Top) / 2)];
      public Square this[int X, int Y] => Map[Region.Left + X, Region.Top + Y];

      public IEnumerable<Square> GetFloorSquares()
      {
        for (var X = 1; X < Region.Width - 1; X++)
        {
          for (var Y = 1; Y < Region.Height - 1; Y++)
            yield return this[X, Y];
        }
      }
      public IEnumerable<Square> GetPerimeterSquares()
      {
        for (var X = 1; X < Region.Width - 1; X++)
        {
          yield return this[X, 1];
          yield return this[X, Region.Height - 2];
        }

        for (var Y = 2; Y < Region.Height - 2; Y++)
        {
          yield return this[1, Y];
          yield return this[Region.Width - 2, Y];
        }
      }
      public IEnumerable<Square> GetEdgeSquares()
      {
        for (var X = 0; X < Region.Width; X++)
        {
          yield return this[X, 0];
          yield return this[X, Region.Height - 1];
        }

        for (var Y = 1; Y < Region.Height - 1; Y++)
        {
          yield return this[0, Y];
          yield return this[Region.Width - 1, Y];
        }
      }
      public IEnumerable<Square> TopEdgeSquares()
      {
        for (var X = 1; X < Region.Width - 1; X++)
          yield return this[X, 0];
      }
      public IEnumerable<Square> BottomEdgeSquares()
      {
        for (var X = 1; X < Region.Width - 1; X++)
          yield return this[X, Region.Height - 1];
      }
      public IEnumerable<Square> LeftEdgeSquares()
      {
        for (var Y = 1; Y < Region.Height - 1; Y++)
          yield return this[0, Y];
      }
      public IEnumerable<Square> RightEdgeSquares()
      {
        for (var Y = 1; Y < Region.Height - 1; Y++)
          yield return this[Region.Width - 1, Y];
      }

      public void SetLit(bool IsLit)
      {
        for (var X = Region.Left; X <= Region.Right; X++)
        {
          for (var Y = Region.Top; Y <= Region.Bottom; Y++)
            Map[X, Y].SetLit(IsLit);
        }
      }
    }

    private sealed class DungeonCorridor
    {
      internal DungeonCorridor(DungeonRoom Room, Square Start)
      {
        this.Room = Room;
        this.Start = Start;
      }

      public DungeonRoom Room { get; }
      public Square Start { get; }
    }

    private sealed class DungeonPlan
    {
      public DungeonPlan(Portal LevelUpPortal, Portal LevelDownPortal)
      {
        this.LevelUpPortal = LevelUpPortal;
        this.LevelDownPortal = LevelDownPortal;
      }

      public Portal LevelUpPortal { get; }
      public Portal LevelDownPortal { get; }
    }

    private enum AttractionType
    {
      Void, // no longer generated as it's boring for players
      Attic,
      Flood, // not yet implemented.
      Maze,
      Prison,
      Shop,
      Shrine,
      Vault,
      Zoo
    }

    private sealed class Attraction
    {
      public Attraction(AttractionType Type) => this.Type = Type;

      public AttractionType Type { get; }
    }

    private sealed class CaveTemplate
    {
      public CaveTemplate(string Name, bool?[,] WallArray)
      {
        this.Name = Name;
        this.WallArray = WallArray;

        var WallWidth = WallArray.GetLength(0);
        var WallHeight = WallArray.GetLength(1);

        for (var WallY = 0; WallY < WallHeight; WallY++)
        {
          this.LeftMask |= (byte)(((WallArray[0, WallY] == false) ? 1 : 0) << WallY);
          this.RightMask |= (byte)(((WallArray[WallWidth - 1, WallY] == false) ? 1 : 0) << WallY);
        }

        for (var WallX = 0; WallX < WallWidth; WallX++)
        {
          this.TopMask |= (byte)(((WallArray[WallX, 0] == false) ? 1 : 0) << WallX);
          this.BottomMask |= (byte)(((WallArray[WallX, WallHeight - 1] == false) ? 1 : 0) << WallX);
        }
      }

      public string Name { get; private set; }
      public bool?[,] WallArray { get; private set; }
      public byte LeftMask { get; private set; }
      public byte TopMask { get; private set; }
      public byte RightMask { get; private set; }
      public byte BottomMask { get; private set; }

      public string Print()
      {
        var n = WallArray.GetLength(0);
        Debug.Assert(WallArray.GetLength(1) == n);

        var Result = "";

        for (var y = 0; y < n; y++)
        {
          for (var x = 0; x < n; x++)
          {
            var cell = WallArray[x, y];

            if (cell == null)
              Result += "╳";
            else if (cell.Value)
              Result += "█";
            else
              Result += ".";
          }

          Result += Environment.NewLine;
        }

        return Result;
      }
    }

    private sealed class AtticTemplate
    {
      public AtticTemplate(Inv.Grid<char> Grid)
      {
        this.Grid = Grid;
      }

      public Inv.Grid<char> Grid { get; }
    }

    private sealed class BSPEngine
    {
      internal BSPEngine()
      {
        this.MinimumPartitionSize = 6;
        this.MaximumPartitionSize = 12;
        this.MinimumRoomSize = 4;
      }

      public int MinimumPartitionSize { get; set; }
      public int MaximumPartitionSize { get; set; }
      public int MinimumRoomSize { get; set; }

      public BSPMap Generate(int MapWidth, int MapHeight)
      {
        var Result = new BSPMap(MapWidth, MapHeight);

        var Continue = true;
        while (Continue)
        {
          Continue = false;

          foreach (var Partition in Result.PartitionList)
          {
            if (Partition.LeftChild == null && Partition.RightChild == null) // if this Leaf is not already split...
            {
              if (Partition.Width > MaximumPartitionSize || Partition.Height > MaximumPartitionSize || RandomSupport.NextPercentile() > 0.25)
              {
                if (Split(Partition))
                {
                  Result.PartitionList.Add(Partition.LeftChild);
                  Result.PartitionList.Add(Partition.RightChild);

                  Continue = true;
                }
              }
            }
          }
        }

        CreateRooms(Result.Main);

        return Result;
      }

      private bool Split(BSPPartition Partition)
      {
        if (Partition.LeftChild != null || Partition.RightChild != null)
          return false;

        var SplitHorizontal = RandomSupport.NextPercentile() > 0.5;
        if (Partition.Width > Partition.Height && (double)Partition.Width / (double)Partition.Height >= 1.25)
          SplitHorizontal = false;
        else if (Partition.Height > Partition.Width && (double)Partition.Height / (double)Partition.Width >= 1.25)
          SplitHorizontal = true;

        var max = (SplitHorizontal ? Partition.Height : Partition.Width) - MinimumPartitionSize; // determine the maximum height or width
        if (max <= MinimumPartitionSize)
          return false;

        var SplitSize = RandomSupport.NextNumber(MinimumPartitionSize, max); // determine where we're going to split

        if (SplitHorizontal)
        {
          Partition.LeftChild = new BSPPartition(Partition.X, Partition.Y, Partition.Width, SplitSize);
          Partition.RightChild = new BSPPartition(Partition.X, Partition.Y + SplitSize, Partition.Width, Partition.Height - SplitSize);
        }
        else
        {
          Partition.LeftChild = new BSPPartition(Partition.X, Partition.Y, SplitSize, Partition.Height);
          Partition.RightChild = new BSPPartition(Partition.X + SplitSize, Partition.Y, Partition.Width - SplitSize, Partition.Height);
        }

        return true;
      }
      private void CreateRooms(BSPPartition Partition)
      {
        if (Partition.LeftChild != null || Partition.RightChild != null)
        {
          if (Partition.LeftChild != null)
            CreateRooms(Partition.LeftChild);

          if (Partition.RightChild != null)
            CreateRooms(Partition.RightChild);
        }
        else
        {
          var RoomWidth = RandomSupport.NextNumber(MinimumRoomSize, Partition.Width - 2);
          var RoomHeight = RandomSupport.NextNumber(MinimumRoomSize, Partition.Height - 2);

          var RoomX = RandomSupport.NextNumber(1, Partition.Width - RoomWidth - 1);
          var RoomY = RandomSupport.NextNumber(1, Partition.Height - RoomHeight - 1);

          Partition.Room = new Inv.Rect(Partition.X + RoomX, Partition.Y + RoomY, RoomWidth, RoomHeight);
        }
      }
      private Inv.Rect? GetRoom(BSPPartition Partition)
      {
        if (Partition.Room != null)
          return Partition.Room;

        var lRoom = (Inv.Rect?)null;
        var rRoom = (Inv.Rect?)null;

        if (Partition.LeftChild != null)
          lRoom = GetRoom(Partition.LeftChild);

        if (Partition.RightChild != null)
          rRoom = GetRoom(Partition.RightChild);

        if (lRoom == null && rRoom == null)
          return null;
        else if (rRoom == null)
          return lRoom;
        else if (lRoom == null)
          return rRoom;
        else if (RandomSupport.NextPercentile() > 0.5)
          return lRoom;
        else
          return rRoom;
      }
    }

    private sealed class BSPMap
    {
      internal BSPMap(int MapWidth, int MapHeight)
      {
        this.PartitionList = [];

        this.Main = new BSPPartition(0, 0, MapWidth, MapHeight);
        PartitionList.Add(Main);
      }

      public BSPPartition Main { get; private set; }
      public Inv.DistinctList<BSPPartition> PartitionList { get; private set; }
    }

    private sealed class BSPPartition
    {
      internal BSPPartition(int X, int Y, int Width, int Height)
      {
        this.X = X;
        this.Y = Y;
        this.Width = Width;
        this.Height = Height;
      }

      public int Y { get; }
      public int X { get; }
      public int Width { get; }
      public int Height { get; }

      public BSPPartition LeftChild { get; set; }
      public BSPPartition RightChild { get; set; }
      public Inv.Rect? Room { get; set; }
    }

    private sealed class MazeGrid
    {
      public MazeGrid(int Width, int Height)
      {
        this.Base = new Inv.Grid<MazeNode>(Width, Height);
        Base.Fill((X, Y) => new MazeNode(X, Y));
      }

      public int Width => Base.Width;
      public int Height => Base.Height;
      public MazeNode this[int X, int Y]
      {
        get => Base[X, Y];
      }

      public IEnumerable<MazeNode> GetNeighbourNodes(MazeNode Node)
      {
        if (Base.IsValid(Node.X - 1, Node.Y))
          yield return Base[Node.X - 1, Node.Y];

        if (Base.IsValid(Node.X + 1, Node.Y))
          yield return Base[Node.X + 1, Node.Y];

        if (Base.IsValid(Node.X, Node.Y - 1))
          yield return Base[Node.X, Node.Y - 1];

        if (Base.IsValid(Node.X, Node.Y + 1))
          yield return Base[Node.X, Node.Y + 1];
      }

      private readonly Inv.Grid<MazeNode> Base;
    }

    private sealed class MazeNode
    {
      public MazeNode(int X, int Y)
      {
        this.X = X;
        this.Y = Y;
      }

      public int X { get; }
      public int Y { get; }
      public bool Left { get; set; }
      public bool Top { get; set; }
      public bool Right { get; set; }
      public bool Bottom { get; set; }

      public bool IsNone()
      {
        return !Left && !Top && !Right && !Bottom;
      }
    }
  }
}