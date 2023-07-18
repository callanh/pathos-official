﻿using System;
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
          RequiresMasterMode: true, IsPublished: true)
    {
      this.Codex = Codex;

      SetIntroduction(Codex.Sonics.introduction);
      SetConclusion(Codex.Sonics.conclusion);
      SetTrack(Codex.Tracks.nethack_title);
    }

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
      var SandboxCorridorGround = Codex.Grounds.stone_corridor;

      var SandboxSite = Adventure.World.AddSite("sandbox");
      var SandboxMap = Adventure.World.AddMap("sandbox", SandboxWidth, SandboxHeight);
      SandboxMap.SetAtmosphere(Codex.Atmospheres.dungeon);
      SandboxMap.SetTerminal(true);

      var SandboxLevel = SandboxSite.AddLevel(1, SandboxMap);

      var LightRoom = SandboxMap.AddRoom(new Region(10, 0, 19, 10), Isolated: false);

      var LightZone = SandboxMap.AddZone();
      LightZone.AddRegion(LightRoom.Region);
      LightZone.SetLit(true);

      Generator.PlaceRoom(LightRoom, SandboxBarrier, SandboxRoomGround);
      Generator.PlaceFloor(SandboxMap[16, 10], SandboxRoomGround);
      Generator.PlaceClosedHorizontalDoor(SandboxMap[16, 10], SandboxGate, SandboxBarrier);
      Generator.PlaceFloor(SandboxMap[19, 3], SandboxRoomGround);
      Generator.PlaceOpenVerticalDoor(SandboxMap[19, 3], SandboxGate, SandboxBarrier);

      var StockX = 14;
      var StockY = 1;
      foreach (var Stock in Codex.Stocks.List)
      {
        var StockSquare = SandboxMap[StockX, StockY];
        foreach (var Item in Stock.Items.OrderByDescending(I => I.Name))
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
      ContainerSquare.PlaceAsset(ContainerAsset);
      ContainerAsset.Container.Locked = true;
      ContainerAsset.Container.Trap = Generator.NewTrap(Generator.RandomContainerDevice(ContainerSquare));

      Generator.PlaceShop(SandboxMap[13, 4], Codex.Shops.List.GetRandom(), 12);
      Generator.PlaceShop(SandboxMap[14, 4], Codex.Shops.List.GetRandom(), 12);

      var FirstMerchant = SandboxMap[13, 4].Character;
      var SecondMerchant = SandboxMap[14, 4].Character;

      var MerchantParty = Generator.NewParty(Leader: null);
      MerchantParty.AddAlly(FirstMerchant, Clock.Zero, Delay.Zero);
      MerchantParty.AddAlly(SecondMerchant, Clock.Zero, Delay.Zero);

      Generator.PlaceBoulder(SandboxMap[15, 4], Codex.Blocks.stone_boulder, false);

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
        Generator.PlaceTrap(Square, Device);
        Square.Trap.SetRevealed(true);

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
      {
        var EggAsset = Generator.NewSpecificAsset(EggSquare, EggItem);
        EggAsset.SetEgg(Egg);
        EggSquare.PlaceAsset(EggAsset);
      }

      var TinSquare = SandboxMap[12, 1];
      for (var Tin = 0; Tin < 20; Tin++)
      {
        var TinAsset = Generator.NewSpecificAsset(TinSquare, Codex.Items.tin);
        TinSquare.PlaceAsset(TinAsset);
      }

      var DarkRoom = SandboxMap.AddRoom(new Region(25, 0, 34, 9), Isolated: false);

      var DarkZone = SandboxMap.AddZone();
      DarkZone.AddRegion(DarkRoom.Region);
      DarkZone.SetLit(false);

      Generator.PlaceRoom(DarkRoom, SandboxBarrier, SandboxRoomGround);
      Generator.PlaceFloor(SandboxMap[25, 3], SandboxRoomGround);
      Generator.PlaceClosedVerticalDoor(SandboxMap[25, 3], SandboxGate, SandboxBarrier);
      Generator.PlaceCharacter(SandboxMap[29, 1]);
      Generator.PlaceCorridor(SandboxMap, SandboxCorridorGround, new Region(20, 3, 24, 3));

      Generator.PlaceFloor(SandboxMap[10, 2], SandboxRoomGround);
      Generator.PlaceClosedVerticalDoor(SandboxMap[10, 2], SandboxGate, SandboxBarrier);
      SandboxMap[10, 2].Door.SetState(DoorState.Locked);
      SandboxMap[10, 2].Door.SetSecret(true);
      SandboxMap[10, 2].Door.SetTrap(Generator.NewTrap(Codex.Devices.arrow_trap));
      Generator.PlaceCorridor(SandboxMap, SandboxCorridorGround, new Region(0, 2, 9, 2));
      SandboxMap.Zones.Last().SetLit(false);

      var SpecialSquare = SandboxMap[10, 4];
      Generator.PlaceFloor(SpecialSquare, SandboxRoomGround);
      Generator.PlaceClosedHorizontalDoor(SpecialSquare, Codex.Gates.crystal_door, SandboxBarrier);
      var SpecialDoor = SpecialSquare.Door;
      SpecialDoor.SetState(DoorState.Locked);
      SpecialDoor.SetKey(Codex.Items.Ruby_Key);
      //Generator.PlaceSpecificAsset(SpecialSquare.Adjacent(Direction.East), SpecialDoor.Key);

      Adventure.World.SetStart(SandboxMap[18, 2]);

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
                  Generator.PlaceCharacter(ThankYouSquare, Entity);

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
