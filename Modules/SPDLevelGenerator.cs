﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  internal sealed class SPDLevelGenerator
  {
    public static void Run(Codex Codex, Generator Generator)
    {
      SPDDebug.generator = Generator;
      SPDDebug.adventure = Generator.Adventure;
      SPDDebug.codex = Codex;
      SPDDebug.mainSite = SPDDebug.adventure.World.AddSite("Main");

      var Gen = new SPDLevelGenerator();

      SPDDebug.maps = new Inv.DistinctList<SPDMap>();
      SPDDebug.currentmap = null;
      Gen.AssignMaps();
      foreach (var map in SPDDebug.maps)
      {
        if (!Gen.IsBossLevel(map.depth))
        {
          SPDDebug.currentmap = map;
          do
          {
            map.width = map.height = 0;
          } while (!Gen.BuildLevel());
          SPDDebug.previousmap = map;
        }
        else
        {
          SPDDebug.currentmap = map;
          Gen.BuildStatic();
          SPDDebug.previousmap = map;
        }
      }

      SPDDebug.generator.StartSquare(SPDDebug.maps[0].pathosMap[SPDDebug.maps[0].entrance.x, SPDDebug.maps[0].entrance.y]);

      Gen.ClearDebug();
    }

    private SPDLevelGenerator()
    {
      SPDGameList = new SPDGameList();
      SPDPainter = new SPDPainter();
      SPDBuilder = new SPDBuilder();
    }

    private void BuildStatic()
    {
      SPDDebug.currentmap.feeling = 0;
      Square Entrance = null;
      Square Exit = null;
      if (SPDDebug.currentmap.depth == 5)
      {
        var map = new int[925] {
                1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,
                1,2,2,2,2,2,2,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,
                1,2,2,2,5,2,2,2,3,2,2,2,2,2,3,2,2,2,2,2,2,2,3,2,2,2,2,2,3,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,3,1,1,1,1,1,3,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,2,1,0,0,0,1,2,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,
                1,1,1,1,3,1,1,1,1,0,0,0,0,0,1,2,1,1,1,1,1,2,1,0,0,0,0,0,1,1,1,1,3,1,1,1,1,
                0,0,0,0,2,0,0,0,0,0,0,0,0,0,1,3,1,2,2,2,1,3,1,0,0,0,0,0,0,0,0,0,2,0,0,0,0,
                0,0,0,0,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,0,0,0,0,
                0,0,0,0,2,0,0,0,0,0,0,0,0,0,1,2,2,2,2,2,2,2,1,0,0,0,0,0,0,0,0,0,2,0,0,0,0,
                0,0,0,0,2,0,0,0,0,0,0,0,0,0,1,2,2,2,2,2,2,2,1,0,0,0,0,0,0,0,0,0,2,0,0,0,0,
                0,0,0,0,2,0,0,0,0,0,0,0,0,0,1,2,2,2,2,2,2,2,1,0,0,0,0,0,0,0,0,0,2,0,0,0,0,
                0,0,0,0,2,2,2,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,3,2,2,2,2,2,2,2,2,2,2,0,0,0,0,
                0,0,0,0,2,0,0,0,0,0,0,0,0,0,1,3,1,2,2,2,1,3,1,0,0,0,0,0,0,0,0,0,2,0,0,0,0,
                1,1,1,1,3,1,1,1,1,0,0,0,0,0,1,2,1,1,1,1,1,2,1,0,0,0,0,0,1,1,1,1,3,1,1,1,1,
                1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,2,1,0,0,0,1,2,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,3,1,1,1,1,1,3,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,2,3,2,2,2,2,2,3,2,2,2,2,2,2,2,3,2,2,2,2,2,3,2,2,2,4,2,2,2,1,
                1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,2,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,2,2,2,2,2,2,2,1,
                1,2,2,2,2,2,2,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,2,2,2,2,2,2,2,1,
                1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1
                };
        var width = 37;
        var height = 25;
        SPDDebug.currentmap.map = new Inv.DistinctList<SPDMapPoint>();
        for (var i = 0; i < height; i++)
        {
          for (var j = 0; j < width; j++)
            SPDDebug.currentmap.map.Add(new SPDMapPoint(j, i, map[i * width + j]));
        }
        SPDDebug.currentmap.pathosMap = SPDDebug.adventure.World.AddMap(SPDDebug.CurrentLevelName(), width, height);
        SPDDebug.currentmap.entrance = new SPDPoint(32, 20);
        SPDDebug.currentmap.exit = new SPDPoint(4, 4);
        ConvertMap();
        Entrance = SPDDebug.currentmap.pathosMap[32, 20];
        Exit = SPDDebug.currentmap.pathosMap[4, 4];
        SPDDebug.currentmap.pathosMap.SetTerminal(false);
        var thisLevel = SPDDebug.mainSite.AddLevel(SPDDebug.currentmap.depth, SPDDebug.currentmap.pathosMap);
        thisLevel.SetTransitions(Entrance, Exit);
        Boss(SPDDebug.currentmap.pathosMap[18, 12], 5);
        SPDDebug.generator.PlaceSolidWall(Entrance, SPDDebug.codex.Barriers.iron_bars, WallSegment.Cross);
        SPDDebug.generator.PlaceSolidWall(Exit, SPDDebug.codex.Barriers.iron_bars, WallSegment.Cross);

        var regions = new Inv.DistinctList<Region>();
        regions.Add(new Region(0, 0, 8, 8));
        regions.Add(new Region(28, 0, 36, 8));
        regions.Add(new Region(0, 16, 8, 24));
        regions.Add(new Region(28, 16, 36, 24));
        regions.Add(new Region(8, 3, 14, 5));
        regions.Add(new Region(22, 3, 28, 5));
        regions.Add(new Region(8, 19, 14, 21));
        regions.Add(new Region(22, 19, 28, 21));
        regions.Add(new Region(14, 2, 22, 6));
        regions.Add(new Region(14, 18, 22, 22));
        regions.Add(new Region(3, 8, 14, 16));
        regions.Add(new Region(22, 8, 33, 16));
        regions.Add(new Region(14, 6, 22, 9));
        regions.Add(new Region(14, 15, 22, 18));
        regions.Add(new Region(14, 9, 22, 15));

        foreach (var region in regions)
        {
          var roomZone = SPDDebug.currentmap.pathosMap.AddZone();
          roomZone.AddRegion(region);
          roomZone.SetLit(true);
        }
      }

      if (SPDDebug.currentmap.depth == 10)
      {
        var map = new int[841]
        {
                    0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,
                    1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,
                    1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1,
                    1,2,2,2,2,2,1,2,2,2,2,2,1,1,1,1,1,2,2,2,2,2,1,2,2,2,2,2,1,
                    1,2,2,2,1,2,1,2,2,2,2,1,1,2,2,2,1,1,2,2,2,2,1,2,1,2,2,2,1,
                    1,2,2,2,2,2,1,2,2,2,2,2,3,2,4,2,3,2,2,2,2,2,1,2,2,2,2,2,1,
                    1,2,2,1,1,1,1,2,2,2,2,1,1,2,2,2,1,1,2,2,2,2,1,1,1,1,2,2,1,
                    1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,
                    1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1,
                    1,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,2,2,1,
                    1,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,1,
                    1,2,2,2,1,2,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,1,2,2,2,1,
                    1,2,2,1,1,3,1,1,2,2,2,2,1,2,2,2,1,2,2,2,2,1,1,3,1,1,2,2,1,
                    1,2,1,1,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,1,1,2,1,
                    1,2,1,1,2,2,2,1,1,2,2,2,2,2,1,2,2,2,2,2,1,1,2,2,2,1,1,2,1,
                    1,2,1,1,2,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,2,1,1,2,1,
                    1,2,2,1,1,3,1,1,2,2,2,2,1,2,2,2,1,2,2,2,2,1,1,3,1,1,2,2,1,
                    1,2,2,2,1,2,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,1,2,2,2,1,
                    1,2,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,1,1,2,2,2,2,2,2,2,2,1,
                    1,2,2,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,1,2,2,2,2,2,2,2,2,2,1,
                    1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1,
                    1,2,2,2,2,2,2,2,2,2,2,2,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,1,
                    1,2,2,1,1,1,1,2,2,2,2,1,1,2,2,2,1,1,2,2,2,2,1,1,1,1,2,2,1,
                    1,2,2,2,2,2,1,2,2,2,2,2,3,2,5,2,3,2,2,2,2,2,1,2,2,2,2,2,1,
                    1,2,2,2,1,2,1,2,2,2,2,1,1,2,2,2,1,1,2,2,2,2,1,2,1,2,2,2,1,
                    1,2,2,2,2,2,1,2,2,2,2,2,1,1,1,1,1,2,2,2,2,2,1,2,2,2,2,2,1,
                    1,2,2,2,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,1,
                    1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,
                    0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0
        };
        var width = 29;
        var height = 29;
        SPDDebug.currentmap.map = new Inv.DistinctList<SPDMapPoint>();
        for (var i = 0; i < height; i++)
        {
          for (var j = 0; j < width; j++)
            SPDDebug.currentmap.map.Add(new SPDMapPoint(j, i, map[i * width + j]));
        }
        SPDDebug.currentmap.pathosMap = SPDDebug.adventure.World.AddMap(SPDDebug.CurrentLevelName(), width, height);
        SPDDebug.currentmap.entrance = new SPDPoint(14, 5);
        SPDDebug.currentmap.exit = new SPDPoint(14, 23);
        ConvertMap();
        Entrance = SPDDebug.currentmap.pathosMap[14, 5];
        Exit = SPDDebug.currentmap.pathosMap[14, 23];
        SPDDebug.currentmap.pathosMap.SetTerminal(false);
        var thisLevel = SPDDebug.mainSite.AddLevel(SPDDebug.currentmap.depth, SPDDebug.currentmap.pathosMap);
        thisLevel.SetTransitions(Entrance, Exit);
        Boss(Exit, 10);

        var regions = new Inv.DistinctList<Region>();
        regions.Add(new Region(0, 0, 9, 9));
        regions.Add(new Region(9, 0, 19, 9));
        regions.Add(new Region(19, 0, 28, 9));
        regions.Add(new Region(0, 9, 9, 19));
        regions.Add(new Region(9, 9, 19, 19));
        regions.Add(new Region(19, 9, 28, 19));
        regions.Add(new Region(0, 19, 9, 28));
        regions.Add(new Region(9, 19, 19, 28));
        regions.Add(new Region(19, 19, 28, 28));
        foreach (var region in regions)
        {
          var roomZone = SPDDebug.currentmap.pathosMap.AddZone();
          roomZone.AddRegion(region);
          roomZone.SetLit(true);
        }
      }

      if (SPDDebug.currentmap.depth == 15)
      {
        var map = new int[529]
        {
                    1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,
                    1,2,2,2,2,2,2,2,1,1,1,1,1,1,1,2,2,2,2,2,2,2,1,
                    1,2,2,2,1,1,1,2,2,2,2,5,2,2,2,2,1,1,1,2,2,2,1,
                    1,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,1,
                    1,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,1,
                    1,2,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,1,
                    1,2,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,1,
                    1,2,2,2,2,2,2,1,2,1,1,2,1,1,2,1,2,2,2,2,2,2,1,
                    1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,
                    0,1,2,2,2,2,2,1,2,1,1,2,1,1,2,1,2,2,2,2,2,1,0,
                    0,1,2,2,2,2,2,1,2,1,2,2,2,1,2,1,2,2,2,2,2,1,0,
                    0,1,2,2,2,2,2,2,2,2,2,4,2,2,2,2,2,2,2,2,2,1,0,
                    0,1,2,2,2,2,2,1,2,1,2,2,2,1,2,1,2,2,2,2,2,1,0,
                    0,1,2,2,2,2,2,1,2,1,1,2,1,1,2,1,2,2,2,2,2,1,0,
                    1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,
                    1,2,2,2,2,2,2,1,2,1,1,2,1,1,2,1,2,2,2,2,2,2,1,
                    1,2,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,1,
                    1,2,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,2,1,
                    1,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,1,
                    1,2,2,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,1,1,2,2,1,
                    1,2,2,2,1,1,1,2,2,2,2,2,2,2,2,2,1,1,1,2,2,2,1,
                    1,2,2,2,2,2,2,2,1,1,1,1,1,1,1,2,2,2,2,2,2,2,1,
                    1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,
        };
        var width = 23;
        var height = 23;
        SPDDebug.currentmap.map = new Inv.DistinctList<SPDMapPoint>();
        for (var i = 0; i < height; i++)
        {
          for (var j = 0; j < width; j++)
            SPDDebug.currentmap.map.Add(new SPDMapPoint(j, i, map[i * width + j]));
        }
        SPDDebug.currentmap.pathosMap = SPDDebug.adventure.World.AddMap(SPDDebug.CurrentLevelName(), width, height);
        SPDDebug.currentmap.entrance = new SPDPoint(11, 11);
        SPDDebug.currentmap.exit = new SPDPoint(11, 2);
        ConvertMap();
        Entrance = SPDDebug.currentmap.pathosMap[11, 11];
        Exit = SPDDebug.currentmap.pathosMap[11, 2];
        SPDDebug.currentmap.pathosMap.SetTerminal(false);
        var thisLevel = SPDDebug.mainSite.AddLevel(SPDDebug.currentmap.depth, SPDDebug.currentmap.pathosMap);
        thisLevel.SetTransitions(Entrance, Exit);
        Boss(Exit, 15);

        var regions = new Inv.DistinctList<Region>();
        regions.Add(new Region(0, 0, 7, 7));
        regions.Add(new Region(7, 0, 15, 7));
        regions.Add(new Region(15, 0, 22, 7));
        regions.Add(new Region(0, 7, 7, 15));
        regions.Add(new Region(7, 7, 15, 15));
        regions.Add(new Region(15, 7, 22, 15));
        regions.Add(new Region(0, 15, 7, 22));
        regions.Add(new Region(7, 15, 15, 22));
        regions.Add(new Region(15, 15, 22, 22));
        foreach (var region in regions)
        {
          var roomZone = SPDDebug.currentmap.pathosMap.AddZone();
          roomZone.AddRegion(region);
          roomZone.SetLit(true);
        }
      }

      if (SPDDebug.currentmap.depth == 20)
      {
        var map = new int[338]
        {
                    0,0,0,0,1,1,1,1,1,0,0,0,0,
                    0,0,0,0,1,2,2,2,1,0,0,0,0,
                    0,0,0,0,1,2,5,2,1,0,0,0,0,
                    0,0,0,0,1,2,2,2,1,0,0,0,0,
                    0,0,0,0,1,1,3,1,1,0,0,0,0,
                    0,0,0,1,1,2,2,2,1,1,0,0,0,
                    0,0,1,1,2,2,2,2,2,1,1,0,0,
                    0,1,1,2,2,2,2,2,2,2,1,1,0,
                    1,1,2,2,2,2,2,2,2,2,2,1,1,
                    1,2,2,2,2,2,2,2,2,2,2,2,1,
                    1,2,2,2,2,2,2,2,2,2,2,2,1,
                    1,2,2,2,2,2,2,2,2,2,2,2,1,
                    1,1,2,2,2,2,2,2,2,2,2,1,1,
                    0,1,1,2,2,2,2,2,2,2,1,1,0,
                    0,0,1,1,2,2,2,2,2,1,1,0,0,
                    0,0,0,1,1,2,2,2,1,1,0,0,0,
                    0,0,0,1,1,1,3,1,1,1,0,0,0,
                    0,0,0,1,2,2,2,2,2,1,0,0,0,
                    0,0,0,1,2,2,2,2,2,1,0,0,0,
                    0,0,0,1,2,2,2,2,2,1,0,0,0,
                    0,0,0,1,2,2,2,2,2,1,0,0,0,
                    0,0,0,1,2,2,2,2,2,1,0,0,0,
                    0,0,0,1,2,2,2,2,2,1,0,0,0,
                    0,0,0,1,2,2,4,2,2,1,0,0,0,
                    0,0,0,1,2,2,2,2,2,1,0,0,0,
                    0,0,0,1,1,1,1,1,1,1,0,0,0
        };
        var width = 13;
        var height = 26;
        SPDDebug.currentmap.map = new Inv.DistinctList<SPDMapPoint>();
        for (var i = 0; i < height; i++)
        {
          for (var j = 0; j < width; j++)
            SPDDebug.currentmap.map.Add(new SPDMapPoint(j, i, map[i * width + j]));
        }
        SPDDebug.currentmap.pathosMap = SPDDebug.adventure.World.AddMap(SPDDebug.CurrentLevelName(), width, height);
        SPDDebug.currentmap.entrance = new SPDPoint(6, 23);
        SPDDebug.currentmap.exit = new SPDPoint(6, 2);
        ConvertMap();
        Entrance = SPDDebug.currentmap.pathosMap[6, 23];
        Exit = SPDDebug.currentmap.pathosMap[6, 2];
        SPDDebug.currentmap.pathosMap.SetTerminal(false);
        var thisLevel = SPDDebug.mainSite.AddLevel(SPDDebug.currentmap.depth, SPDDebug.currentmap.pathosMap);
        thisLevel.SetTransitions(Entrance, Exit);
        Boss(SPDDebug.currentmap.pathosMap[6, 10], 20);


        var regions = new Inv.DistinctList<Region>();
        regions.Add(new Region(4, 0, 8, 4));
        regions.Add(new Region(0, 4, 12, 16));
        regions.Add(new Region(0, 16, 12, 25));
        foreach (Region region in regions)
        {
          var roomZone = SPDDebug.currentmap.pathosMap.AddZone();
          roomZone.AddRegion(region);
          roomZone.SetLit(true);
        }
      }
      if (SPDDebug.currentmap.depth == 25)
      {
        var map = new int[361]
        {
                    1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
                    1,2,2,2,2,2,2,2,2,4,2,2,2,2,2,2,2,2,1,
                    1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,
                    1,2,2,1,1,2,2,1,1,2,1,1,2,2,1,1,2,2,1,
                    1,2,2,1,2,2,2,2,2,2,2,2,2,2,2,1,2,2,1,
                    1,2,2,2,2,2,1,2,1,1,1,2,1,2,2,2,2,2,1,
                    1,2,2,2,2,1,2,2,2,2,2,2,2,1,2,2,2,2,1,
                    1,2,2,1,2,2,2,1,1,1,1,1,2,2,2,1,2,2,1,
                    1,2,2,1,2,1,2,1,2,2,2,1,2,1,2,1,2,2,1,
                    1,2,2,2,2,1,2,1,2,2,2,1,2,1,2,2,2,2,1,
                    1,2,2,1,2,1,2,1,2,2,2,1,2,1,2,1,2,2,1,
                    1,2,2,1,2,2,2,1,1,3,1,1,2,2,2,1,2,2,1,
                    1,2,2,2,2,1,2,2,2,2,2,2,2,1,2,2,2,2,1,
                    1,2,2,2,2,2,1,2,1,1,1,2,1,2,2,2,2,2,1,
                    1,2,2,1,2,2,2,2,2,2,2,2,2,2,2,1,2,2,1,
                    1,2,2,1,1,2,2,1,1,2,1,1,2,2,1,1,2,2,1,
                    1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,
                    1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,1,
                    1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,
        };
        var width = 19;
        var height = 19;
        SPDDebug.currentmap.map = new Inv.DistinctList<SPDMapPoint>();
        for (var i = 0; i < height; i++)
        {
          for (var j = 0; j < width; j++)
            SPDDebug.currentmap.map.Add(new SPDMapPoint(j, i, map[i * width + j]));
        }
        SPDDebug.currentmap.pathosMap = SPDDebug.adventure.World.AddMap(SPDDebug.CurrentLevelName(), width, height);
        SPDDebug.currentmap.entrance = new SPDPoint(9, 1);
        SPDDebug.currentmap.exit = new SPDPoint(0, 0);
        ConvertMap();
        Entrance = SPDDebug.currentmap.pathosMap[9, 1];
        Exit = SPDDebug.currentmap.pathosMap[9, 9];
        SPDDebug.currentmap.pathosMap.SetTerminal(true);
        var thisLevel = SPDDebug.mainSite.AddLevel(SPDDebug.currentmap.depth, SPDDebug.currentmap.pathosMap);
        thisLevel.SetTransitions(Entrance, null);
        Boss(Exit, 25);
        SPDDebug.generator.PlacePassage(SPDDebug.currentmap.pathosMap[9, 9], SPDDebug.codex.Portals.transportal, null);

        var regions = new Inv.DistinctList<Region>();
        regions.Add(new Region(0, 0, 9, 9));
        regions.Add(new Region(9, 0, 18, 9));
        regions.Add(new Region(0, 9, 9, 18));
        regions.Add(new Region(9, 9, 18, 18));
        foreach (var region in regions)
        {
          var roomZone = SPDDebug.currentmap.pathosMap.AddZone();
          roomZone.AddRegion(region);
          roomZone.SetLit(true);
        }
      }
      SPDDebug.currentmap.pathosMap.SetAtmosphere(SPDDebug.codex.Atmospheres.dungeon);
      SPDDebug.currentmap.pathosMap.SetSealed(true);

      SPDDebug.generator.RepairWalls(SPDDebug.currentmap.pathosMap, SPDDebug.currentmap.pathosMap.Region);
      SPDDebug.generator.RepairDoors(SPDDebug.currentmap.pathosMap, SPDDebug.currentmap.pathosMap.Region);
      SPDDebug.generator.RepairVoid(SPDDebug.currentmap.pathosMap, SPDDebug.currentmap.pathosMap.Region);

      SPDDebug.generator.PlacePermanentWall(Entrance, SPDDebug.codex.Barriers.iron_bars, WallSegment.Cross);
      SPDDebug.generator.PlacePermanentWall(Exit, SPDDebug.codex.Barriers.iron_bars, WallSegment.Cross);
      SetDifficulty();
      AddTriggers();
    }
    private void Boss(Square Square, int depth)
    {
      Character boss;
      switch (depth)
      {
        case 5:
          {
            switch (SPDRandom.Int(2))
            {
              case 0:
                {
                  boss = SPDDebug.generator.NewCharacter(Square, SPDDebug.codex.Entities.shade);
                  SPDDebug.generator.PromoteCharacter(boss, 5);
                  break;
                }
              case 1:
                {
                  boss = SPDDebug.generator.NewCharacter(Square, SPDDebug.codex.Entities.white_unicorn);
                  SPDDebug.generator.AcquireTalent(boss, SPDDebug.codex.Properties.slowness);
                  break;
                }
              default: boss = null; break;
            }
            SPDDebug.generator.AcquireUnique(Square, boss, SPDDebug.codex.Qualifications.master);
            var script5 = boss.InsertScript();
            script5.Killed.Sequence.Add(SPDDebug.codex.Tricks.cleared_way).SetTarget(SPDDebug.currentmap.pathosMap[32, 20]);
            script5.Killed.Sequence.Add(SPDDebug.codex.Tricks.cleared_way).SetTarget(SPDDebug.currentmap.pathosMap[4, 4]);
            break;
          }
        case 10:
          {
            switch (SPDRandom.Int(2))
            {
              case 0:
                {
                  boss = SPDDebug.generator.NewCharacter(Square, SPDDebug.codex.Entities.air_elemental);
                  SPDDebug.generator.AcquireTalent(boss, SPDDebug.codex.Properties.confusion, SPDDebug.codex.Properties.narcolepsy);
                  break;
                }
              case 1:
                {
                  boss = SPDDebug.generator.NewCharacter(Square, SPDDebug.codex.Entities.leprechaun_wizard);
                  SPDDebug.generator.PromoteCharacter(boss, 3);
                  SPDDebug.generator.AcquireTalent(boss, SPDDebug.codex.Properties.reflection, SPDDebug.codex.Properties.displacement, SPDDebug.codex.Properties.quickness, SPDDebug.codex.Properties.mana_regeneration, SPDDebug.codex.Properties.deflection, SPDDebug.codex.Properties.free_action);
                  SPDDebug.generator.EnsureResistance(boss, SPDDebug.codex.Elements.magical, 100);
                  break;
                }
              default: boss = null; break;
            }
            SPDDebug.generator.AcquireUnique(Square, boss, SPDDebug.codex.Qualifications.master);
            var script10 = boss.InsertScript();
            script10.Killed.Sequence.Add(SPDDebug.codex.Tricks.cleared_way).SetTarget(SPDDebug.currentmap.pathosMap[14, 5]);
            script10.Killed.Sequence.Add(SPDDebug.codex.Tricks.cleared_way).SetTarget(SPDDebug.currentmap.pathosMap[14, 23]);
            break;
          }
        case 15:
          {
            switch (SPDRandom.Int(2))
            {
              case 0:
                {
                  boss = SPDDebug.generator.NewCharacter(Square, SPDDebug.codex.Entities.ettin);
                  SPDDebug.generator.AcquireTalent(boss, SPDDebug.codex.Properties.quickness);
                  SPDDebug.generator.PromoteCharacter(boss, 5);
                  break;
                }
              case 1:
                {
                  boss = SPDDebug.generator.NewCharacter(Square, SPDDebug.codex.Entities.archlich);
                  SPDDebug.generator.AcquireTalent(boss, SPDDebug.codex.Properties.reflection, SPDDebug.codex.Properties.displacement, SPDDebug.codex.Properties.quickness, SPDDebug.codex.Properties.deflection);
                  break;
                }
              default: boss = null; break;
            }
            SPDDebug.generator.AcquireUnique(Square, boss, SPDDebug.codex.Qualifications.master);
            var script15 = boss.InsertScript();
            script15.Killed.Sequence.Add(SPDDebug.codex.Tricks.cleared_way).SetTarget(SPDDebug.currentmap.pathosMap[11, 11]);
            script15.Killed.Sequence.Add(SPDDebug.codex.Tricks.cleared_way).SetTarget(SPDDebug.currentmap.pathosMap[11, 2]);
            break;
          }
        case 20:
          {
            switch (SPDRandom.Int(2))
            {
              case 0:
                {
                  boss = SPDDebug.generator.NewCharacter(Square, SPDDebug.codex.Entities.mithril_golem);
                  SPDDebug.generator.AcquireTalent(boss, SPDDebug.codex.Properties.slowness);
                  break;
                }
              case 1:
                {
                  boss = SPDDebug.generator.NewCharacter(Square, SPDDebug.codex.Entities.disintegrator);
                  SPDDebug.generator.AcquireTalent(boss, SPDDebug.codex.Properties.quickness, SPDDebug.codex.Properties.reflection);
                  SPDDebug.generator.PromoteCharacter(boss, 10);
                  break;
                }
              default: boss = null; break;
            }
            SPDDebug.generator.AcquireUnique(Square, boss, SPDDebug.codex.Qualifications.master);
            var boss20killedscript = boss.InsertScript();
            boss20killedscript.Killed.Sequence.Add(SPDDebug.codex.Tricks.cleared_way).SetTarget(SPDDebug.currentmap.pathosMap[6, 23]);
            boss20killedscript.Killed.Sequence.Add(SPDDebug.codex.Tricks.cleared_way).SetTarget(SPDDebug.currentmap.pathosMap[6, 2]);
            break;
          }
        case 25:
          {
            var bossEntity = SPDGameList.RandomUniqueBoss();
            boss = SPDDebug.generator.NewCharacter(Square, bossEntity);
            SPDDebug.generator.HostileCharacter(boss);

            if (bossEntity.Level <= 40)
            {
              // TODO: this is meant to be a static but randomish level boost (However, string.GetHashCode() is not actually deterministic across .NET versions and x86/x64!).
              var levelBoost = Math.Abs(bossEntity.Name.GetHashCode() % 10) + (bossEntity.Level / 10) + (bossEntity.Level % 10);

              if (bossEntity.Level < 10)
                levelBoost += 40;
              else if (bossEntity.Level < 20)
                levelBoost += 30;
              else if (bossEntity.Level < 30)
                levelBoost += 20;
              else if (bossEntity.Level <= 40)
                levelBoost += 10;

              SPDDebug.generator.PromoteCharacter(boss, levelBoost);
            }

            SPDDebug.generator.OutfitCharacter(boss);
            var boss25killedscript = boss.InsertScript();
            boss25killedscript.Killed.Sequence.Add(SPDDebug.codex.Tricks.cleared_way).SetTarget(SPDDebug.currentmap.pathosMap[9, 1]);
            boss25killedscript.Killed.Sequence.Add(SPDDebug.codex.Tricks.cleared_way).SetTarget(SPDDebug.currentmap.pathosMap[9, 9]);
            break;
          }
        default: boss = null; break;
      }

      if (boss != null)
        SPDDebug.generator.PlaceCharacter(Square, boss);
    }
    private bool IsBossLevel(int depth)
    {
      return depth == 5 || depth == 10 || depth == 15 || depth == 20 || depth == 25;
    }
    private void AssignMaps()
    {
      SPDDebug.maps = new Inv.DistinctList<SPDMap>();
      for (var i = 1; i < 26; i++)
      {
        var map = new SPDMap();
        map.depth = i;
        map.width = map.height = map.width = 0;
        //Feelings: 12.5% chance of having a special feeling each level.
        switch (SPDRandom.Int(8))
        {
          case 0:
            map.feeling = 1;
            break;
          case 1:
            map.feeling = 2;
            break;
          case 2:
            map.feeling = 3;
            break;
          default:
            map.feeling = 0;
            break;
        }
        SPDDebug.maps.Add(map);
      }

    }
    private bool BuildLevel()
    {
      do
      {
        ClearMap(SPDDebug.currentmap);
        InitRooms();
        Build();
      } while (!SPDBuilder.Success);
      SPDPainter.Paint();
      //Deb.WriteMapList();
      //Deb.WriteRooms();
      Final();
      return true;
    }
    private void ClearMap(SPDMap map)
    {
      map.finalRooms.Clear();
      map.initRooms.Clear();
      map.initRoomsSecret.Clear();
      map.initRoomsSpecial.Clear();
      map.initRoomsStandard.Clear();
      map.loop.Clear();
      map.mainPathRooms.Clear();
      map.map = new Inv.DistinctList<SPDMapPoint>();
      map.multiConnections.Clear();
      map.singleConnections.Clear();
      map.width = map.height = 0;
      map.facilities.Clear();
      map.monsters.Clear();
      map.items.Clear();
      map.traps.Clear();
      map.blocks.Clear();
    }
    private void InitRooms()
    {
      var roomEntrance = new SPDRoom();
      roomEntrance.type = 0;
      roomEntrance.flavor = SPDFlavour.Entrance;
      SPDDebug.currentmap.initRoomsStandard.Add(roomEntrance);

      var roomExit = new SPDRoom();
      roomExit.type = 0;
      roomExit.flavor = SPDFlavour.Exit;
      SPDDebug.currentmap.initRoomsStandard.Add(roomExit);

      var standards = StandardRooms();
      SPDDebug.currentmap.standardRooms = standards;
      for (var i = 0; i < standards; i++)
      {
        var s = SPDRoom.NewStandard();
        SPDDebug.currentmap.initRoomsStandard.Add(s);
      }

      var specials = SpecialRooms();
      SPDDebug.currentmap.specialRooms = specials;
      for (var i = 0; i < specials; i++)
      {
        SPDRoom s;
        do
        {
          s = SPDRoom.NewSpecial();
        } while (SPDDebug.currentmap.initRoomsSpecial.Exists(r => r.flavor == s.flavor));

        SPDDebug.currentmap.initRoomsSpecial.Add(s);
      }

      if (NeedShop())
      {
        var shop = SPDRoom.NewSpecial();
        shop.flavor = SPDFlavour.Shop;
        SPDDebug.currentmap.initRoomsSpecial.Add(shop);
      }

      for (var i = 0; i < SecretRooms(); i++)
        SPDDebug.currentmap.initRoomsSecret.Add(SPDRoom.NewSecret());

      foreach (var room in SPDDebug.currentmap.initRoomsStandard)
        SPDDebug.currentmap.initRooms.Add(room);

      foreach (var room in SPDDebug.currentmap.initRoomsSpecial)
        SPDDebug.currentmap.initRooms.Add(room);

      foreach (var room in SPDDebug.currentmap.initRoomsSecret)
        SPDDebug.currentmap.initRooms.Add(room);

      SPDDebug.currentmap.initRooms.Shuffle();
    }
    private int StandardRooms()
    {
      if (SPDDebug.currentmap.depth > 0 && SPDDebug.currentmap.depth < 5)
      {
        return (SPDDebug.currentmap.feeling == 1) ? 11 : 7 + SPDRandom.Chances(new float[] { 4, 2, 1 });
      }
      if (SPDDebug.currentmap.depth > 5 && SPDDebug.currentmap.depth < 10)
      {
        return (SPDDebug.currentmap.feeling == 1) ? 12 : 8 + SPDRandom.Chances(new float[] { 4, 2, 2 });
      }
      if (SPDDebug.currentmap.depth > 10 && SPDDebug.currentmap.depth < 15)
      {
        return (SPDDebug.currentmap.feeling == 1) ? 14 : 9 + SPDRandom.Chances(new float[] { 2, 3, 3, 1 });
      }
      if (SPDDebug.currentmap.depth > 15 && SPDDebug.currentmap.depth < 20)
      {
        return (SPDDebug.currentmap.feeling == 1) ? 15 : 9 + SPDRandom.Chances(new float[] { 4, 3, 2, 1 });
      }
      if (SPDDebug.currentmap.depth > 21 && SPDDebug.currentmap.depth < 25)
      {
        return (SPDDebug.currentmap.feeling == 1) ? 15 : 10 + SPDRandom.Chances(new float[] { 3, 2, 1 });
      }
      return 3;
    }
    private int SpecialRooms()
    {
      if (SPDDebug.currentmap.depth > 0 && SPDDebug.currentmap.depth < 5)
      {
        return (SPDDebug.currentmap.feeling == 1) ? 4 : 1 + SPDRandom.Chances(new float[] { 4, 4, 2 });
      }
      if (SPDDebug.currentmap.depth > 5 && SPDDebug.currentmap.depth < 10)
      {
        return (SPDDebug.currentmap.feeling == 1) ? 4 : 1 + SPDRandom.Chances(new float[] { 3, 4, 3 });
      }
      if (SPDDebug.currentmap.depth > 10 && SPDDebug.currentmap.depth < 15)
      {
        return (SPDDebug.currentmap.feeling == 1) ? 4 : 1 + SPDRandom.Chances(new float[] { 2, 4, 4 });
      }
      if (SPDDebug.currentmap.depth > 15 && SPDDebug.currentmap.depth < 20)
      {
        return (SPDDebug.currentmap.feeling == 1) ? 4 : 2 + SPDRandom.Chances(new float[] { 2, 1 });
      }
      if (SPDDebug.currentmap.depth > 21 && SPDDebug.currentmap.depth < 25)
      {
        return (SPDDebug.currentmap.feeling == 1) ? 4 : 2 + SPDRandom.Chances(new float[] { 1, 1 });
      }
      return 7;
    }
    private int SecretRooms()
    {
      if (SPDDebug.currentmap.feeling == 3) return 2;
      else if (SPDRandom.Int(3) > 0) return 1;
      else return 0;
    }
    private void Build()
    {
      if (SPDRandom.Int(2) == 0) SPDBuilder.BuildLoop();
      else SPDBuilder.BuildFigureEight();
    }
    private bool NeedShop()
    {
      if (SPDDebug.currentmap.depth == 6 || SPDDebug.currentmap.depth == 11 ||
          SPDDebug.currentmap.depth == 16 || SPDDebug.currentmap.depth == 21 || SPDDebug.currentmap.depth == 24) return true;
      else return false;
    }
    private void Final()
    {
      SPDDebug.currentmap.pathosMap = SPDDebug.adventure.World.AddMap(SPDDebug.CurrentLevelName(), Width: SPDDebug.currentmap.width, Height: SPDDebug.currentmap.height);
      ConvertMap();
      SPDDebug.currentmap.pathosMap.SetAtmosphere(SPDDebug.codex.Atmospheres.dungeon);
      if (SPDDebug.currentmap.depth < 25) SPDDebug.currentmap.pathosMap.SetTerminal(false);
      else SPDDebug.currentmap.pathosMap.SetTerminal(true);
      var thisLevel = SPDDebug.mainSite.AddLevel(SPDDebug.currentmap.depth, SPDDebug.currentmap.pathosMap);
      thisLevel.SetTransitions(SPDDebug.currentmap.pathosMap[SPDDebug.currentmap.entrance.x, SPDDebug.currentmap.entrance.y], SPDDebug.currentmap.pathosMap[SPDDebug.currentmap.exit.x, SPDDebug.currentmap.exit.y]);
      AddZones();
      SetDifficulty();
      PutBlocks();
      PutFacilities();
      PutItems();
      PutTraps();
      PutMonsters();
      AddTriggers();

      var StandardPointList = SPDPainter.AllStandardPoints();
      StandardPointList.Shuffle();

      GenerateItems(StandardPointList);
      GenerateMonsters(StandardPointList);
      GenerateTraps(StandardPointList);
      GenerateFacilities(StandardPointList);

      SPDDebug.generator.RepairWalls(SPDDebug.currentmap.pathosMap, SPDDebug.currentmap.pathosMap.Region);
      SPDDebug.generator.RepairDoors(SPDDebug.currentmap.pathosMap, SPDDebug.currentmap.pathosMap.Region);
      SPDDebug.generator.RepairVoid(SPDDebug.currentmap.pathosMap, SPDDebug.currentmap.pathosMap.Region);
    }
    private void SetDifficulty()
    {
      var Depth = SPDDebug.currentmap.depth;

      int Difficulty;
      if (SPDDebug.currentmap.depth < 3) Difficulty = Depth * 2;
      else if (SPDDebug.currentmap.depth < 11) Difficulty = Depth + 3;
      else if (SPDDebug.currentmap.depth < 21) Difficulty = Depth + 3 + Convert.ToInt32(0.5 * (Depth - 10));
      else Difficulty = 28 + Convert.ToInt32(2.5 * (Depth - 20));

      SPDDebug.currentmap.pathosMap.SetDifficulty(Depth);
      foreach (var currentZone in SPDDebug.currentmap.pathosMap.Zones)
        currentZone.SetDifficulty(Difficulty);
    }
    private void AddTriggers()
    {
      var trigger = SPDDebug.currentmap.pathosMap.InsertTrigger();

      trigger.Add(Delay.FromTurns(SPDDebug.currentmap.depth < 15 ? 10000 : 15000), SPDDebug.codex.Tricks.sudden_hellscape);

      for (var i = 0; i < 8; i++)
      {
        trigger.Add(i == 0 ? Delay.Zero : Delay.FromTurns(2000), SPDDebug.codex.Tricks.overwhelm_difficulty);

        if (IsBossLevel(SPDDebug.currentmap.depth))
        {
          for (var j = 0; j < 5; j++)
            trigger.Add(Delay.Zero, SPDDebug.codex.Tricks.random_spawning).SetTarget(RandomSquare());
        }
      }
    }
    private Square RandomSquare()
    {
      return SPDDebug.currentmap.pathosMap.Zones.GetRandom().Squares.GetRandom();
    }
    private void ConvertMap()
    {
      foreach (var point in SPDDebug.currentmap.map)
      {
        var pointSquare = SPDDebug.currentmap.pathosMap[point.x, point.y];

        switch (point.value)
        {
          case SPDMapPoint.EMPTY:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_floor);
            break;
          case SPDMapPoint.WALL:
            SPDDebug.generator.PlaceSolidWall(pointSquare, SPDDebug.codex.Barriers.stone_wall, WallSegment.Cross);
            //var wall = pointSquare.Wall;
            //if (wall != null) wall.SetStructure(WallStructure.Permanent);
            break;
          case SPDMapPoint.DOORNORM:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_floor);
            SPDDebug.generator.PlaceRandomHorizontalDoor(pointSquare, SPDDebug.codex.Gates.wooden_door, SPDDebug.codex.Barriers.stone_wall);
            SPDDebug.generator.CloseDoor(pointSquare);
            break;
          case SPDMapPoint.ENTRANCE:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_floor);
            if (SPDDebug.currentmap.depth == 1)
              SPDDebug.generator.PlacePassage(pointSquare, SPDDebug.codex.Portals.stone_staircase_up, null);
            else
              SPDDebug.generator.PlacePassage(pointSquare, SPDDebug.codex.Portals.stone_staircase_up, SPDDebug.previousmap.pathosMap[SPDDebug.previousmap.exit.x, SPDDebug.previousmap.exit.y]);
            if (SPDDebug.currentmap.depth == 25) SPDDebug.generator.PlacePassage(SPDDebug.previousmap.pathosMap[SPDDebug.previousmap.exit.x, SPDDebug.previousmap.exit.y], SPDDebug.codex.Portals.stone_staircase_down, SPDDebug.currentmap.pathosMap[SPDDebug.currentmap.entrance.x, SPDDebug.currentmap.entrance.y]);
            break;
          case SPDMapPoint.EXIT:
            {
              SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_floor);
              if (SPDDebug.currentmap.depth != 1)
              {
                var exitSquare = SPDDebug.previousmap.pathosMap[SPDDebug.previousmap.exit.x, SPDDebug.previousmap.exit.y];
                var entranceSquare = SPDDebug.currentmap.pathosMap[SPDDebug.currentmap.entrance.x, SPDDebug.currentmap.entrance.y];

                if (exitSquare.Fixture != null)
                  SPDDebug.generator.RemoveFixture(exitSquare);

                SPDDebug.generator.PlacePassage(exitSquare, SPDDebug.codex.Portals.stone_staircase_down, entranceSquare);
              }
              break;
            }
          case SPDMapPoint.WATER:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.water);
            break;
          case SPDMapPoint.WOODFLOOR:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.wooden_floor);
            break;
          case SPDMapPoint.CORRIDOR:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_path);
            break;
          case SPDMapPoint.LAVA:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.lava);
            break;
          case SPDMapPoint.TERRSTATUE:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_floor);
            SPDPainter.AddBlock(new SPDPoint(point.x, point.y), SPDMapPoint.STATUE);
            break;
          case SPDMapPoint.TERRBARREL:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_floor);
            SPDPainter.AddBlock(new SPDPoint(point.x, point.y), SPDMapPoint.BARREL);
            break;
          case SPDMapPoint.TERRTRAP:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_path);
            SPDPainter.AddTrap(new SPDPoint(point.x, point.y), SPDMapPoint.TRAP);
            break;
          case SPDMapPoint.TERRTRAPSP:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_floor);
            SPDPainter.AddTrap(new SPDPoint(point.x, point.y), SPDMapPoint.TRAPSP);
            break;
          case SPDMapPoint.GRASS:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.grass);
            break;
          case SPDMapPoint.DIRT:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.dirt);
            break;
          case SPDMapPoint.TERRSUMMTRAP:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_floor);
            SPDPainter.AddTrap(new SPDPoint(point.x, point.y), SPDMapPoint.TRAPSUMMON);
            break;
          case SPDMapPoint.HIVE:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.hive_floor);
            break;
          case SPDMapPoint.MARBLE:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.marble_floor);
            break;
          case SPDMapPoint.JADEWALL:
            SPDDebug.generator.PlaceSolidWall(pointSquare, SPDDebug.codex.Barriers.jade_wall, WallSegment.Cross);
            break;
          case SPDMapPoint.DOORLOCK:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_floor);
            SPDDebug.generator.PlaceLockedHorizontalDoor(pointSquare, SPDDebug.codex.Gates.wooden_door, SPDDebug.codex.Barriers.stone_wall, Secret: false, Trap: SPDDebug.generator.NewTrap(RandomDevice(), Revealed: SPDRandom.Int(2) == 0));
            break;
          case SPDMapPoint.DOORPLAIN:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_floor);
            SPDDebug.generator.PlaceClosedHorizontalDoor(pointSquare, SPDDebug.codex.Gates.wooden_door, SPDDebug.codex.Barriers.stone_wall, Secret: false);
            break;
          case SPDMapPoint.DOORSECH:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_floor);
            SPDDebug.generator.PlaceLockedHorizontalDoor(pointSquare, SPDDebug.codex.Gates.wooden_door, SPDDebug.codex.Barriers.stone_wall, Secret: true, Trap: SPDDebug.generator.NewTrap(RandomDevice(), Revealed: false));
            break;
          case SPDMapPoint.DOORSECL:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_floor);
            SPDDebug.generator.PlaceLockedVerticalDoor(pointSquare, SPDDebug.codex.Gates.wooden_door, SPDDebug.codex.Barriers.stone_wall, Secret: true, Trap: SPDDebug.generator.NewTrap(RandomDevice(), Revealed: false));
            break;
          case SPDMapPoint.DOORSUPER:
            SPDDebug.generator.PlaceFloor(pointSquare, SPDDebug.codex.Grounds.stone_floor);
            SPDDebug.generator.PlaceLockedVerticalDoor(pointSquare, SPDDebug.codex.Gates.crystal_door, SPDDebug.codex.Barriers.stone_wall, Reinforced: true);
            break;
        }
      }
    }
    private void AddZones()
    {
      foreach (var room in SPDDebug.currentmap.finalRooms)
      {
        var roomRegion = new Region(room.left, room.top, room.right, room.bottom);
        var roomZone = SPDDebug.currentmap.pathosMap.AddZone();
        roomZone.AddRegion(roomRegion);
        roomZone.SetLit(true);
        if (SPDDebug.currentmap.depth == 1 && room.type == 0 && room.flavor == SPDFlavour.Entrance)
        {
          SPDDebug.startRegion = roomRegion;
          SPDDebug.startZone = roomZone;
        }
      }
    }
    private void ClearDebug()
    {
      // ensure all debug state is nulled so it will be garbage collected.
      SPDDebug.generator = null;
      SPDDebug.adventure = null;
      SPDDebug.codex = null;
      SPDDebug.mainSite = null;
      SPDDebug.startRegion = Region.Zero;
      SPDDebug.startZone = null;
      SPDDebug.maps = null;
      SPDDebug.currentmap = null;
      SPDDebug.previousmap = null;
    }
    private void GenerateItems(Inv.DistinctList<SPDPoint> StandardPointList)
    {
      if (SPDPainter.PoSNeeded())
      {
        if (SPDRandom.Int(3) == 0) SPDPainter.AddItemDelayed(SPDDebug.codex.Items.potion_of_gain_level);
        else SPDPainter.AddItemDelayed(SPDDebug.codex.Items.potion_of_gain_ability);
      }
      SPDPainter.AddItemDelayed(SPDDebug.codex.Items.scroll_of_enchantment);
      if (SPDDebug.currentmap.depth > 20) SPDPainter.AddItemDelayed(SPDDebug.codex.Items.potion_of_gain_level);

      SPDPainter.AddItemDelayed(SPDDebug.codex.Items.food_ration);
      SPDPainter.AddItemDelayed(SPDDebug.codex.Items.iron_ration);

      SPDPainter.AddItemDelayed(SPDGameList.RandomTool());
      var goldcoinN = SPDRandom.IntRange(2, 3);
      for (var i = 0; i < goldcoinN; i++)
        SPDPainter.AddItemDelayed(SPDDebug.codex.Items.gold_coin, SPDGameList.GoldCoinQuantity());

      var randomItemN = SPDRandom.IntRange(3, 4);
      if (SPDDebug.currentmap.feeling == 1) randomItemN += 3;
      for (var i = 0; i < randomItemN; i++)
        SPDPainter.AddItemDelayed(null); // random item.

      var randomPoScN = SPDRandom.IntRange(2, 3);
      for (var i = 0; i < randomPoScN; i++)
      {
        if (SPDRandom.Int(3) == 0) SPDPainter.AddItemDelayed(SPDGameList.RandomPotion());
        else SPDPainter.AddItemDelayed(SPDGameList.RandomScroll());
      }

      SPDDebug.currentmap.itemsToAdd.Shuffle();
      foreach (var ItemDelayed in SPDDebug.currentmap.itemsToAdd)
      {
        var pos = StandardPointList.GetRandom();
        var square = SPDDebug.currentmap.pathosMap[pos.x, pos.y];

        if (ItemDelayed.Item == null)
        {
          if (SPDRandom.Int(40) == 0) SPDDebug.generator.PlaceSpecificAsset(square, SPDDebug.generator.RandomUniqueItem());
          else
          {
            var asset = SPDGameList.RandomAsset(square);
            if (asset != null)
              SPDDebug.generator.PlaceAsset(square, asset);
          }
        }
        else
          SPDDebug.generator.PlaceSpecificAsset(square, ItemDelayed.Item, ItemDelayed.Quantity);
      }

      if (SPDDebug.currentmap.depth == 1)
      {
        var pos = StandardPointList.GetRandom();
        var pickaxeSquare = SPDDebug.currentmap.pathosMap[pos.x, pos.y];
        var pickaxe = SPDDebug.generator.NewSpecificAsset(pickaxeSquare, SPDDebug.codex.Items.pickaxe);
        //pickaxe.SetWeight(Weight.Zero);
        SPDDebug.generator.ChangeSanctity(pickaxe, SPDDebug.codex.Sanctities.Blessed);
        SPDDebug.generator.ChangeEnchantment(pickaxe, Modifier.Zero);
        SPDDebug.generator.PlaceAsset(pickaxeSquare, pickaxe);
      }
    }
    private void GenerateMonsters(Inv.DistinctList<SPDPoint> StandardPointList)
    {
      var randomMonsterN = 5 + (SPDDebug.currentmap.depth / 5) * 2 + SPDRandom.Int(5);
      if (SPDDebug.currentmap.feeling == 1) randomMonsterN = Convert.ToInt32(1.3 * randomMonsterN);
      for (var i = 0; i < randomMonsterN; i++)
      {
        var pos = StandardPointList.GetRandom();
        var square = SPDDebug.currentmap.pathosMap[pos.x, pos.y];

        if (square.Character == null)
        {
          SPDDebug.generator.PlaceRandomCharacter(square);
          if (square.Character != null) SPDDebug.generator.AcquireTalent(SPDDebug.currentmap.pathosMap[pos.x, pos.y].Character, SPDDebug.codex.Properties.sleeping);
        }
      }
    }
    private void GenerateTraps(Inv.DistinctList<SPDPoint> StandardPointList)
    {
      var randTrapN = SPDRandom.NormalIntRange(2, 3 + SPDDebug.currentmap.depth / 5);
      if (SPDDebug.currentmap.feeling == 1) randTrapN = Convert.ToInt32(1.3 * randTrapN);
      for (var i = 0; i < randTrapN; i++)
      {
        var pos = StandardPointList.GetRandom();
        var square = SPDDebug.currentmap.pathosMap[pos.x, pos.y];

        if (square.Trap == null)
        {
          SPDDebug.generator.PlaceTrap(square, RandomDevice(), Revealed: false);
        }
      }
    }
    private void GenerateFacilities(Inv.DistinctList<SPDPoint> StandardPointList)
    {
      var randFacilN = SPDRandom.NormalIntRange(0, 2 + SPDDebug.currentmap.depth / 7);
      for (var i = 0; i < randFacilN; i++)
      {
        var pos = StandardPointList.GetRandom();
        var square = SPDDebug.currentmap.pathosMap[pos.x, pos.y];

        if (square.Fixture == null && square.Passage == null)
        {
          SPDDebug.generator.PlaceFixture(square, SPDGameList.RandomFeature());
        }
      }
    }
    private void PlacePDShop(Square Square)
    {
      var shop = SPDDebug.codex.Shops.general_store;
      SPDDebug.generator.PlaceFixture(Square, SPDDebug.codex.Features.stall, SPDDebug.codex.Sanctities.Uncursed);
      var stall = Square.Fixture.Container;
      SPDDebug.generator.PlaceSpecificCharacter(Square, SPDDebug.codex.Entities.merchant);
      var shopkeeper = Square.Character;
      SPDDebug.generator.ChangeQuantity(shopkeeper.Inventory.GetEquippedAsset(SPDDebug.codex.Slots.purse), 1); // override to start with 1 gold coin.
      SPDDebug.generator.ResidentShop(shopkeeper, Square, shop);
      SPDDebug.generator.NeutralCharacter(shopkeeper);
      SPDDebug.generator.RequireCompetency(shopkeeper, SPDDebug.codex.Skills.bartering, SPDDebug.codex.Qualifications.champion);
      SPDDebug.generator.NameCharacter(shopkeeper, SPDDebug.generator.GetCharacterName(shop.KeeperNames));

      Asset StallAdd(Item StallItem, int? Quantity = null)
      {
        var Result = SPDDebug.generator.NewSpecificAsset(Square, StallItem, Quantity);
        stall.Stash.Add(Result);
        return Result;
      }

      var Items = SPDDebug.codex.Items;

      var armour = StallAdd(SPDGameList.RandomArmour());
      SPDDebug.generator.ChangeSanctity(armour, SPDDebug.codex.Sanctities.Uncursed);

      var armour2 = StallAdd(SPDGameList.RandomArmour());
      SPDDebug.generator.ChangeSanctity(armour2, SPDDebug.codex.Sanctities.Uncursed);

      var weapon = StallAdd(SPDGameList.RandomWeapon());
      SPDDebug.generator.ChangeSanctity(weapon, SPDDebug.codex.Sanctities.Uncursed);

      var water = StallAdd(Items.potion_of_water);
      SPDDebug.generator.ChangeSanctity(water, SPDDebug.codex.Sanctities.Blessed);

      var healingPotionDice = SPDRandom.Int(10);
      if (healingPotionDice < 1) StallAdd(Items.potion_of_full_healing, 2);
      else if (healingPotionDice < 4) StallAdd(Items.potion_of_extra_healing, 2);
      else StallAdd(Items.potion_of_healing, 2);

      StallAdd(SPDGameList.RandomPotion());
      StallAdd(SPDGameList.RandomPotion());
      StallAdd(Items.scroll_of_identify);
      StallAdd(Items.scroll_of_identify);
      StallAdd(Items.scroll_of_identify);
      StallAdd(Items.scroll_of_magic_mapping);
      StallAdd(Items.potion_of_gain_level);
      StallAdd(Items.potion_of_gain_level);
      StallAdd(Items.potion_of_gain_level);
      StallAdd(SPDGameList.RandomScroll());
      StallAdd(SPDGameList.RandomScroll());
      for (var i = 0; i < 4; i++)
      {
        if (SPDRandom.Int(2) == 0)
          StallAdd(SPDGameList.RandomPotion());
        else
          StallAdd(SPDGameList.RandomScroll());
      }
      var books = SPDRandom.IntRange(1, 3);
      for (var i = 0; i < books; i++)
      {
        StallAdd(SPDGameList.RandomBook());
      }
      StallAdd(Items.lembas_wafer);
      var tools = SPDRandom.IntRange(3, 5);
      for (var i = 0; i < tools; i++)
      {
        StallAdd(SPDGameList.RandomToolOrMaybeArtifact());
      }

      var sous = SPDRandom.IntRange(0, 2);
      for (var i = 0; i < sous; i++)
      {
        StallAdd(Items.scroll_of_enchantment);
      }

      var rares = SPDRandom.IntRange(1, 3);
      for (var i = 0; i < rares; i++)
      {
        switch (SPDRandom.Int(3))
        {
          case 0: StallAdd(SPDGameList.RandomAmulet()); break;
          case 1: StallAdd(SPDGameList.RandomRing()); break;
          case 2: StallAdd(SPDGameList.RandomWand()); break;
        }
      }

      if (Inv.Assert.IsEnabled)
        Inv.Assert.CheckNotNull(Square.Zone, "Square.Zone");

      if (Square.Zone != null)
        Square.Zone.InsertTrigger().Add(Delay.Zero, SPDDebug.codex.Tricks.VisitShopArray[shop.Index]).SetTarget(Square);
    }
    private void PlacePoorShop(SPDMapPoint pos)
    {
      var shop = SPDDebug.codex.Shops.general_store;
      var square = SPDDebug.currentmap.pathosMap[pos.x, pos.y];
      SPDDebug.generator.PlaceFixture(square, SPDDebug.codex.Features.stall, SPDDebug.codex.Sanctities.Uncursed);
      var stall = square.Fixture.Container;
      SPDDebug.generator.PlaceSpecificCharacter(square, SPDDebug.codex.Entities.merchant);
      var shopkeeper = square.Character;
      SPDDebug.generator.ChangeQuantity(shopkeeper.Inventory.GetEquippedAsset(SPDDebug.codex.Slots.purse), 1);
      SPDDebug.generator.ResidentShop(shopkeeper, square, shop);
      SPDDebug.generator.NeutralCharacter(shopkeeper);
      SPDDebug.generator.RequireCompetency(shopkeeper, SPDDebug.codex.Skills.bartering, SPDDebug.codex.Qualifications.champion);
      SPDDebug.generator.NameCharacter(shopkeeper, SPDDebug.generator.GetCharacterName(shop.KeeperNames));

      void StallAdd(Item StallItem) => stall.Stash.Add(SPDDebug.generator.NewSpecificAsset(square, StallItem));

      StallAdd(SPDGameList.RandomWand());
      StallAdd(SPDGameList.RandomRing());
      StallAdd(SPDGameList.RandomAmulet());
      StallAdd(SPDGameList.RandomToolOrMaybeArtifact());
      StallAdd(SPDGameList.RandomToolOrMaybeArtifact());
      StallAdd(SPDGameList.RandomScroll());
      StallAdd(SPDGameList.RandomScroll());
      StallAdd(SPDGameList.RandomScroll());
      StallAdd(SPDGameList.RandomPotion());
      StallAdd(SPDGameList.RandomPotion());
      StallAdd(SPDGameList.RandomPotion());

      if (Inv.Assert.IsEnabled)
        Inv.Assert.CheckNotNull(square.Zone, "Square.Zone");

      if (square.Zone != null)
        square.Zone.InsertTrigger().Add(Delay.Zero, SPDDebug.codex.Tricks.VisitShopArray[shop.Index]).SetTarget(square);
    }
    private void PlaceShrine(Square Square, Shrine Shrine)
    {
      if (Inv.Assert.IsEnabled)
        Inv.Assert.CheckNotNull(Square.Zone, "Square.Zone");

      SPDDebug.generator.PlaceShrine(Square, Shrine);

      if (Square.Zone != null)
        Square.Zone.InsertTrigger().Add(Delay.Zero, SPDDebug.codex.Tricks.VisitShrineArray[Shrine.Index]).SetTarget(Square);
    }
    private void PutFacilities()
    {
      foreach (var facility in SPDDebug.currentmap.facilities)
      {
        var facilitySquare = SPDDebug.currentmap.pathosMap[facility.x, facility.y];
        switch (facility.value)
        {
          case SPDMapPoint.FOUNTAIN: SPDDebug.generator.PlaceFixture(facilitySquare, SPDDebug.codex.Features.fountain); break;
          case SPDMapPoint.ALTAR: SPDDebug.generator.PlaceFixture(facilitySquare, SPDDebug.codex.Features.altar); break;
          case SPDMapPoint.BED: SPDDebug.generator.PlaceFixture(facilitySquare, SPDDebug.codex.Features.bed); break;
          case SPDMapPoint.PDSHOP: PlacePDShop(facilitySquare); break;
          case SPDMapPoint.SACREDGROVE: PlaceShrine(facilitySquare, SPDDebug.codex.Shrines.sacred_grove); break;
          case SPDMapPoint.HOLYSHRINE: PlaceShrine(facilitySquare, SPDDebug.codex.Shrines.holy_shrine); break;
          case SPDMapPoint.DARKSHRINE: PlaceShrine(facilitySquare, SPDDebug.codex.Shrines.dark_sepulchre); break;
          case SPDMapPoint.WORKBENCH: SPDDebug.generator.PlaceFixture(facilitySquare, SPDDebug.codex.Features.workbench); break;
          case SPDMapPoint.GRAVE: SPDDebug.generator.PlaceFixture(facilitySquare, SPDDebug.codex.Features.grave); break;
          case 9: PlacePoorShop(facility); break;
        }
      }
    }
    private void PutBlocks()
    {
      foreach (var block in SPDDebug.currentmap.blocks)
      {
        var blockSquare = SPDDebug.currentmap.pathosMap[block.x, block.y];
        switch (block.value)
        {
          case SPDMapPoint.STATUE: SPDDebug.generator.PlaceBoulder(blockSquare, SPDDebug.codex.Blocks.statue, IsRigid: true); break;
          case SPDMapPoint.BARREL: SPDDebug.generator.PlaceBoulder(blockSquare, SPDDebug.codex.Blocks.wooden_barrel, IsRigid: true); break;
        }
      }
    }
    private void PutItems()
    {
      foreach (var item in SPDDebug.currentmap.items)
      {
        var itemSquare = SPDDebug.currentmap.pathosMap[item.x, item.y];
        switch (item.value)
        {
          case SPDMapPoint.RANDARMOUR: SPDDebug.generator.PlaceAsset(itemSquare, RandomGrade(itemSquare, SPDGameList.RandomArmour())); break;
          case SPDMapPoint.GOODARMOUR: SPDDebug.generator.PlaceAsset(itemSquare, RandomGrade(itemSquare, SPDGameList.GoodArmour())); break;
          case SPDMapPoint.MAGICFOOD: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDGameList.MagicFood()); break;
          case SPDMapPoint.RANDPOTION: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDGameList.RandomPotion()); break;
          case SPDMapPoint.RANDSCROLL: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDGameList.RandomScroll()); break;
          case SPDMapPoint.SOID: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDDebug.codex.Items.scroll_of_identify); break;
          case SPDMapPoint.SOU: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDDebug.codex.Items.scroll_of_enchantment); break;
          case SPDMapPoint.POS: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDDebug.codex.Items.potion_of_gain_ability); break;
          case SPDMapPoint.SORC: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDDebug.codex.Items.scroll_of_remove_curse); break;
          case SPDMapPoint.PITCHEST: SPDDebug.generator.PlaceAsset(itemSquare, SPDGameList.PitChest(itemSquare)); break;
          case SPDMapPoint.RANDRING: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDGameList.RandomRing()); break;
          case SPDMapPoint.POOLPRIZE: SPDDebug.generator.PlaceAsset(itemSquare, SPDGameList.PoolPrize(itemSquare)); break;
          case SPDMapPoint.GOODWEAPON: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDGameList.GoodWeapon()); break;
          case SPDMapPoint.GOLD: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDDebug.codex.Items.gold_coin, SPDGameList.GoldCoinQuantity()); break;
          case SPDMapPoint.GOLDHALF: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDDebug.codex.Items.gold_coin, SPDGameList.HalfGoldQuantity()); break;
          case SPDMapPoint.AMULET: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDGameList.RandomAmulet()); break;
          case SPDMapPoint.BOOK: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDGameList.RandomBook()); break;
          case SPDMapPoint.ARTIFACT: break;
          case SPDMapPoint.THROWNWEP: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDGameList.ThrownWeapon()); break;
          case SPDMapPoint.THROWNFIRE:
            var FirearmItem = SPDGameList.ThrownFirearm();
            if (FirearmItem.IsAbolitionCandidate())
              FirearmItem = SPDDebug.generator.AbolitionReplacement(FirearmItem);
            SPDDebug.generator.PlaceAsset(itemSquare, SPDDebug.generator.NewSpecificAsset(itemSquare, FirearmItem, Quantity: 1));
            break;
          case SPDMapPoint.ZOOCHEST: SPDDebug.generator.PlaceAsset(itemSquare, SPDGameList.ZooChest(itemSquare)); break;
          case SPDMapPoint.POH: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDDebug.codex.Items.potion_of_healing); break;
          case SPDMapPoint.GEMREAL: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDGameList.RealGem()); break;
          case SPDMapPoint.RANDWAND: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDGameList.RandomWand()); break;
          case SPDMapPoint.RANDTOOL: SPDDebug.generator.PlaceSpecificAsset(itemSquare, SPDGameList.RandomToolOrMaybeArtifact()); break;
        }
      }
    }
    private Asset RandomGrade(Square Square, Item item)
    {
      var asset = SPDDebug.generator.NewSpecificAsset(Square, item);
      if (!item.HasEnchantment()) return asset;

      var enchantment = Modifier.Zero;
      var sanctity = SPDDebug.codex.Sanctities.Uncursed;

      if (SPDRandom.Int(2) == 0)
      {
        if (SPDRandom.Int(2) == 0)
        {
          var dice = SPDRandom.Int(10);
          if (dice < 3)
          {
            enchantment = Modifier.Plus2;
          }
          else if (dice > 7)
          {
            enchantment = Modifier.Plus3;
          }
          else
          {
            enchantment = Modifier.Plus1;
          }
          if (SPDRandom.Int(3) == 0) sanctity = SPDDebug.codex.Sanctities.Blessed;
        }
        else
        {
          sanctity = SPDDebug.codex.Sanctities.Cursed;
          var dice = SPDRandom.Int(10);
          if (dice < 3)
          {
            enchantment = Modifier.Minus2;
          }
          else if (dice > 7)
          {
            enchantment = Modifier.Minus3;
          }
          else
          {
            enchantment = Modifier.Minus1;
          }
        }
      }

      SPDDebug.generator.ChangeSanctity(asset, item.DefaultSanctity ?? sanctity);
      SPDDebug.generator.ChangeEnchantment(asset, enchantment);

      return asset;
    }
    private void PutTraps()
    {
      Device deviceType;
      do
      {
        deviceType = SPDGameList.RandomDevice();
      } while (deviceType.Difficulty > SPDDebug.currentmap.depth + 3 || DeviceForbidden(deviceType) || deviceType == SPDDebug.codex.Devices.alarm_trap);

      foreach (var trap in SPDDebug.currentmap.traps)
      {
        var trapSquare = SPDDebug.currentmap.pathosMap[trap.x, trap.y];
        switch (trap.value)
        {
          case SPDMapPoint.TRAP: SPDDebug.generator.PlaceTrap(trapSquare, RandomDevice(), Revealed: false); break;
          case SPDMapPoint.TRAPSP:
            SPDDebug.generator.PlaceTrap(trapSquare, deviceType, Revealed: true);
            break;

          case SPDMapPoint.TRAPSUMMON:
            SPDDebug.generator.PlaceTrap(trapSquare, SPDDebug.codex.Devices.alarm_trap, Revealed: true, Triggered: 1);
            break;

        }
      }
    }
    private void PutMonsters()
    {
      foreach (var monster in SPDDebug.currentmap.monsters)
      {
        var monsterSquare = SPDDebug.currentmap.pathosMap[monster.x, monster.y];

        if (monsterSquare.Character == null)
        {
          switch (monster.value)
          {
            case SPDMapPoint.MONSTER: SPDDebug.generator.PlaceRandomCharacter(monsterSquare); break;
            case SPDMapPoint.LEPRECHAUN: SPDDebug.generator.PlaceSpecificCharacter(monsterSquare, SPDDebug.codex.Entities.leprechaun); break;
            case SPDMapPoint.LEPREWIZARD: SPDDebug.generator.PlaceSpecificCharacter(monsterSquare, SPDDebug.codex.Entities.leprechaun_wizard); break;
            case SPDMapPoint.YEOMAN: SPDDebug.generator.PlaceSpecificCharacter(monsterSquare, SPDDebug.codex.Entities.yeoman); break;
            case SPDMapPoint.YEOMANWARDER: SPDDebug.generator.PlaceSpecificCharacter(monsterSquare, SPDDebug.codex.Entities.yeoman_warder); break;
            case SPDMapPoint.YEOMANCHIEF: SPDDebug.generator.PlaceSpecificCharacter(monsterSquare, SPDDebug.codex.Entities.chief_yeoman_warder); break;
            case SPDMapPoint.BEE: SPDDebug.generator.PlaceSpecificCharacter(monsterSquare, SPDDebug.codex.Entities.killer_bee); break;
            case SPDMapPoint.BEEQUEEN: SPDDebug.generator.PlaceSpecificCharacter(monsterSquare, SPDDebug.codex.Entities.queen_bee); break;
            case SPDMapPoint.SMALMIMIC: SPDDebug.generator.PlaceSpecificCharacter(monsterSquare, SPDDebug.codex.Entities.small_mimic); break;
          }
        }
      }
    }
    private Device RandomDevice()
    {
      Device device;
      do
      {
        device = SPDGameList.RandomDevice();
      } while (device.Difficulty > SPDDebug.currentmap.depth + 3 || DeviceForbidden(device));

      return device;
    }
    private bool DeviceForbidden(Device device)
    {
      return
          device == SPDDebug.codex.Devices.hole ||
          device == SPDDebug.codex.Devices.trapdoor ||
          device == SPDDebug.codex.Devices.level_teleporter ||
          device == SPDDebug.codex.Devices.polymorph_trap ||
          device == SPDDebug.codex.Devices.squeaky_board;
    }

    private readonly SPDGameList SPDGameList;
    private readonly SPDPainter SPDPainter;
    private readonly SPDBuilder SPDBuilder;
  }
}