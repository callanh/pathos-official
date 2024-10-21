using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  internal sealed class SandboxModule : Module
  {
    internal SandboxModule(Codex Codex)
      : base(
          Handle: "sandbox play",
          Name: "Sandbox Play",
          Description: "Sandbox for experimenting with the mechanics of Pathos. Play with every item, trap and dungeon fixture in a simple two room map.",
          Colour: Inv.Colour.DarkGoldenrod,
          Author: "Callan Hodgskin", Email: "hodgskin.callan@gmail.com",
          RequiresMasterMode: true)
    {
      this.Codex = Codex;

      SetIntroduction(Codex.Sonics.introduction);
      SetConclusion(Codex.Sonics.conclusion);
      SetTrack(Codex.Tracks.nethack_title);

      AddTerm(Dungeon);
    }

    public const string Dungeon = "Dungeon";

    public override void Execute(Generator Generator)
    {
      var Adventure = Generator.Adventure;

#if DEBUG
      var SandboxWidth = 70; // for 'thank you' message.
      var SandboxHeight = 70;
#else
      var SandboxWidth = 35;
      var SandboxHeight = 35;
#endif

      var SandboxGate = Codex.Gates.wooden_door;
      var SandboxBarrier = Codex.Barriers.stone_wall;
      var SandboxRoomGround = Codex.Grounds.stone_floor;
      var SandboxCorridorGround = Codex.Grounds.stone_path;

      var SandboxSite = Adventure.World.AddSite(Generator.EscapedModuleTerm(Dungeon));
      var SandboxMap = Adventure.World.AddMap(Generator.EscapedModuleTerm(Dungeon), SandboxWidth, SandboxHeight);
      SandboxMap.SetDifficulty(1);
      SandboxMap.SetAtmosphere(Codex.Atmospheres.dungeon);
      SandboxMap.SetTerminal(true);

      var SandboxLevel = SandboxSite.AddLevel(1, SandboxMap);

      var LightRegion = new Region(10, 0, 19, 10);

      var LightZone = SandboxMap.AddZone();
      LightZone.AddRegion(LightRegion);
      LightZone.SetLit(true);

      Generator.PlaceRoom(SandboxMap, SandboxBarrier, SandboxRoomGround, LightRegion);
      Generator.PlaceFloor(SandboxMap[16, 10], SandboxRoomGround);
      Generator.PlaceClosedHorizontalDoor(SandboxMap[16, 10], SandboxGate, SandboxBarrier);
      Generator.PlaceFloor(SandboxMap[19, 3], SandboxRoomGround);
      Generator.PlaceOpenVerticalDoor(SandboxMap[19, 3], SandboxGate, SandboxBarrier);

      var StockX = 14;
      var StockY = 1;
      foreach (var Stock in Codex.Stocks.List)
      {
        var StockSquare = SandboxMap[StockX, StockY];
        foreach (var Item in Stock.Items.OrderByDescending(I => I.Name).Where(I => !Generator.Adventure.Abolition || !I.IsAbolitionCandidate()))
          Generator.PlaceSpecificAsset(StockSquare, Item);

        StockX++;

        if (StockX >= 19)
        {
          StockX = 14;
          StockY++;
        }
      }

      var ContainerSquare = SandboxMap[13, 3];
      var ContainerAsset = Generator.NewSpecificAsset(ContainerSquare, Generator.ContainerItems.GetRandomOrNull());
      Generator.PlaceAsset(ContainerSquare, ContainerAsset);
      ContainerAsset.Container.Locked = true;
      ContainerAsset.Container.Trap = Generator.NewTrap(Generator.RandomContainerDevice(ContainerSquare), Revealed: false);

      Generator.PlaceShop(SandboxMap[13, 4], Codex.Shops.List.GetRandom(), 12);
      Generator.PlaceShop(SandboxMap[14, 4], Codex.Shops.List.GetRandom(), 12);

      var FirstMerchant = SandboxMap[13, 4].Character;
      var SecondMerchant = SandboxMap[14, 4].Character;

      var MerchantParty = Generator.NewParty(Leader: null);
      MerchantParty.AddAlly(FirstMerchant, Clock.Zero, Delay.Zero);
      MerchantParty.AddAlly(SecondMerchant, Clock.Zero, Delay.Zero);

      var MerchantInsultedJuncture = Generator.Adventure.World.AddJuncture("M-Insulted");

      const string MerchantAnotherQuestion = "Ask another question";

      var MerchantDialogue = Generator.Adventure.World.AddDialogue("M");
      MerchantDialogue.Root.Document.Fragment("Hail {Character}, welcome to my humble shop. I offer a variety of self-explanatory goods and services and if necessary, will stoop to answer your inane questions.").Before(MerchantInsultedJuncture);
      MerchantDialogue.Root.Document.Fragment("I'll take your coin but you will get no more advice from the merchant guild. GOOD LUCK IDIOT!").After(MerchantInsultedJuncture);
      MerchantDialogue.Root.Branch(D => D.Fragment("Can you tell me about your services?").Before(MerchantInsultedJuncture), N =>
      {
        N.Document.Fragment("I offer a wide variety of essential services for a small and reasonable fee. If you do not have enough coin to pay, never fear! I will wait patiently while you go find more.");
        N.Branch(MerchantAnotherQuestion, MerchantDialogue.Root);
      });
      MerchantDialogue.Root.Branch(D => D.Fragment("Do you accept replica coins?").Before(MerchantInsultedJuncture), N =>
      {
        N.Document.Fragment("Absolutely not! Law abiding citizens like me can't even distinguish replica coins from real coins...");

        N.Branch("Are you sure? I have plenty of replica coins to spend.", O =>
        {
          O.Document.Fragment("This is entrapment! I'm calling the kops to deal with you!");
          O.Sequence.Add(MerchantInsultedJuncture);
          O.Sequence.Add(Codex.Tricks.keystone_kops);
        });

        N.Branch(MerchantAnotherQuestion, MerchantDialogue.Root);
      });
      MerchantDialogue.Root.Branch(D => D.Fragment("Can you help me fight the monsters?").Before(MerchantInsultedJuncture), N =>
      {
        N.Document.Fragment("Of course, you may purchase my wares and services to aid you in battle. Philosophically though, it's important to understand that they are only 'monsters' from your point of view. To them, you are the 'monster'. To me, they are an important part of our economy, responsible for the liberation of expensive items, by force if necessary.");

        N.Branch("That's a load of garbage and you know it.", O =>
        {
          O.Document.Fragment("That was a rude and ignorant thing to say. Good riddance pathetic adventurer, you'll get no more help from the merchant guild.");
          O.Sequence.Add(MerchantInsultedJuncture);
        });

        N.Branch(MerchantAnotherQuestion, MerchantDialogue.Root);
      });
      MerchantDialogue.Root.Branch(D => D.Fragment("How did you know my name is {Character}?").Before(MerchantInsultedJuncture), N =>
      {
        N.Document.Fragment("We actually get a full briefing and information pack about every new adventurer before they enter the dungeon. No surprises that way.");
        N.Branch(MerchantAnotherQuestion, MerchantDialogue.Root);
      });
      MerchantDialogue.Root.Branch("No further questions");

      Generator.AssignDialogue(FirstMerchant, MerchantDialogue);
      Generator.AssignDialogue(SecondMerchant, MerchantDialogue);

      Generator.PlaceBoulder(SandboxMap[15, 4], Codex.Blocks.stone_boulder, IsRigid: false);

      var FeatureX = 11;
      var FeatureY = 2;
      foreach (var Feature in Codex.Features.List)
      {
        var Square = SandboxMap[FeatureX, FeatureY];
        Generator.PlaceFixture(Square, Feature);

        FeatureX++;

        if (FeatureX >= 13)
        {
          FeatureX = 11;
          FeatureY++;
        }
      }

      var DeviceX = 11;
      var DeviceY = 5;
      foreach (var Device in Codex.Devices.List.Where(D => !D.Descent)) // TODO: create a basement level.
      {
        var Square = SandboxMap[DeviceX, DeviceY];
        Generator.PlaceTrap(Square, Device, Revealed: true);

        DeviceX++;

        if (DeviceX >= 19)
        {
          DeviceX = 11;
          DeviceY++;
        }
      }

      var GroundIndex = 16;
      foreach (var Ground in Codex.Grounds.List.Except(SandboxRoomGround))
        Generator.PlaceFloor(SandboxMap[GroundIndex++, 4], Ground);

      var EggSquare = SandboxMap[13, 1];
      var EggItem = Codex.Items.List.Where(I => I.Type == ItemType.Egg && !I.Grade.Unique).Single();

      foreach (var Egg in Codex.Eggs.List.OrderBy(E => E.Layer.Name))
        Generator.PlaceEggAsset(EggSquare, EggItem, Egg);

      var TinSquare = SandboxMap[12, 1];
      for (var Tin = 0; Tin < 20; Tin++)
      {
        var TinAsset = Generator.NewSpecificAsset(TinSquare, Codex.Items.tin);
        Generator.PlaceAsset(TinSquare, TinAsset);
      }

      var DarkRegion = new Region(25, 0, 34, 9);

      var DarkZone = SandboxMap.AddZone();
      DarkZone.AddRegion(DarkRegion);
      DarkZone.SetLit(false);

      Generator.PlaceRoom(SandboxMap, SandboxBarrier, SandboxRoomGround, DarkRegion);
      Generator.PlaceFloor(SandboxMap[25, 3], SandboxRoomGround);
      Generator.PlaceClosedVerticalDoor(SandboxMap[25, 3], SandboxGate, SandboxBarrier);
      Generator.PlaceRandomCharacter(SandboxMap[29, 1]);
      Generator.PlaceCorridor(SandboxMap, SandboxCorridorGround, new Region(20, 3, 24, 3));

      Generator.PlaceFloor(SandboxMap[10, 2], SandboxRoomGround);
      Generator.PlaceLockedVerticalDoor(SandboxMap[10, 2], SandboxGate, SandboxBarrier, Secret: true, Trap: Generator.NewTrap(Codex.Devices.arrow_trap, Revealed: false));
      Generator.PlaceCorridor(SandboxMap, SandboxCorridorGround, new Region(0, 2, 9, 2));
      SandboxMap.Zones.Last().SetLit(false);

      var SpecialSquare = SandboxMap[10, 4];
      Generator.PlaceFloor(SpecialSquare, SandboxRoomGround);
      Generator.PlaceLockedHorizontalDoor(SpecialSquare, Codex.Gates.crystal_door, SandboxBarrier, Key: Codex.Items.Ruby_Key);
      //Generator.PlaceSpecificAsset(SpecialSquare.Adjacent(Direction.East), SpecialDoor.Key);

      Generator.StartSquare(SandboxMap[15, 3]);

      //var KoboldEntity = Generator.Codex.Entities.kobold_shaman;
      //var KoboldSquare = SandboxMap[18, 4];
      //Generator.PlaceCharacter(KoboldSquare, KoboldEntity);

#if DEBUG
      var ThankYouText =
@"                                                
 TTTTT   H   H     A     N   N   K   K   B   B    OOO    U   U   ! 
   T     H   H    A A    NN  N   K KK     B B    O   O   U   U   ! 
   T     HHHHH   AAAAA   N N N   K@        B     O   O   U   U   ! 
   T     H   H   A   A   N  NN   K KK      B     O   O   U   U     
   T     H   H   A   A   N   N   K   k     B      OOO     UUU    ! 
                                                    ";

      var ThankYouX = 2;
      var ThankYouY = 28;

      var ThankYouGrid = Generator.LoadSpecialGrid(ThankYouText);

      var PotionArray = Codex.Items.List.Where(I => I.Type == ItemType.Potion && !I.Grade.Unique).ToArray();
      var EntityDictionary = Codex.Entities.List.GroupBy(E => char.ToLower(E.Kind.Name[0])).ToDictionary(K => K.Key, V => V.Where(A => A.IsEncounter && !A.IsHiding()).ToDistinctList());

      for (var Column = 0; Column < ThankYouGrid.Width; Column++)
      {
        for (var Row = 0; Row < ThankYouGrid.Height; Row++)
        {
          var ThankYouSymbol = ThankYouGrid[Column, Row];
          var ThankYouSquare = SandboxMap[ThankYouX + Column, ThankYouY + Row];

          switch (ThankYouSymbol)
          {
            case ' ':
              Generator.PlaceSolidWall(ThankYouSquare, SandboxBarrier, WallSegment.Cross);
              ThankYouSquare.SetLit(true);
              break;

            default:
              Generator.PlaceFloor(ThankYouSquare, SandboxRoomGround);
              ThankYouSquare.SetLit(true);

              if (ThankYouSymbol == '@')
              {
                SandboxMap.AddZone().AddSquare(ThankYouSquare);
              }
              else if (ThankYouSymbol == '!')
              {
                Generator.PlaceSpecificAsset(ThankYouSquare, PotionArray.GetRandomOrNull());
                Generator.PlaceSpecificAsset(ThankYouSquare, PotionArray.GetRandomOrNull());
                Generator.PlaceSpecificAsset(ThankYouSquare, PotionArray.GetRandomOrNull());
                Generator.PlaceSpecificAsset(ThankYouSquare, PotionArray.GetRandomOrNull());
              }
              else
              {
                var EntityList = EntityDictionary[char.ToLower(ThankYouSymbol)];

                var Entity = EntityList.GetRandomOrNull();

                if (Entity != null)
                {
                  Generator.PlaceSpecificCharacter(ThankYouSquare, Entity);

                  if (EntityList.Count > 1)
                    EntityList.Remove(Entity);
                }
              }
              break;
          }
        }
      }

      SandboxMap.AddZone().AddRegion(new Region(ThankYouX, ThankYouY, ThankYouX + ThankYouGrid.Width - 1, ThankYouY + ThankYouGrid.Height - 1));

      Generator.RepairVoid(SandboxMap, SandboxMap.Region);
      Generator.RepairWalls(SandboxMap, SandboxMap.Region);
#endif
    }

    private readonly Codex Codex;
  }
}