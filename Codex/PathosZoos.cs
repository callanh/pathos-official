using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexZoos : CodexPage<ManifestZoos, ZooEditor, Zoo>
  {
    private CodexZoos() { }
#if MASTER_CODEX
    internal CodexZoos(Codex Codex)
      : base(Codex.Manifest.Zoos)
    {
      var Hordes = Codex.Hordes;
      var Features = Codex.Features;
      var Items = Codex.Items;
      var Entities = Codex.Entities;
      var Devices = Codex.Devices;
      var Materials = Codex.Materials;
      var Grounds = Codex.Grounds;
      var Sonics = Codex.Sonics;

      Zoo AddZoo(string Name, Sonic Sonic, Action<ZooEditor> Action)
      {
        return Register.Add(Z =>
        {
          Z.Name = Name;
          Z.Sonic = Sonic;

          CodexRecruiter.Enrol(() => Action(Z));
        });
      }

      ant_hole = AddZoo("ant hole", Sonics.scuttle, Z =>
      {
        Z.Difficulty = Entities.giant_ant.Difficulty + 1;
        Z.Rarity = 2;
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.sandwich);
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.cheese);
        Z.Ground = Grounds.dirt_floor;
        Z.Device = Devices.ant_hole;
        Z.AddSpawn(Chance.Always, 1.d4(), new[] { Entities.giant_ant });
      });

      bee_hive = AddZoo("bee hive", Sonics.buzz, Z =>
      {
        Z.Difficulty = Entities.killer_bee.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.hive_floor;
        Z.Loot.AddKit(Chance.OneIn5, Dice.One, Items.lump_of_royal_jelly);
        Z.AddSpawn(Chance.Always, Dice.One, new[] { Entities.queen_bee });
        Z.AddSpawn(Chance.Always, Count: null, new[] { Entities.killer_bee });
      });

      barracks = AddZoo("barracks", Sonics.bugle, Z =>
      {
        Z.Difficulty = Entities.captain.Difficulty + 1;
        Z.Rarity = 2;
        Z.Feature = Features.bed;
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.brass_bugle);
        Z.Loot.AddKit(Chance.OneIn10, 4.d4(), Items.bullet);
        Z.Loot.AddKit(Chance.OneIn10, 2.d2(), Items.shotgun_shell);
        Z.AddSpawn(Chance.Always, Dice.One, new[] { Entities.captain });
        Z.AddSpawn(Chance.ThreeIn4, 1.d2(), new[] { Entities.lieutenant });
        Z.AddSpawn(Chance.ThreeIn4, 1.d3(), new[] { Entities.sergeant });
        Z.AddSpawn(Chance.OneIn3, Count: null, new[] { Entities.soldier });
      });

      cocknest = AddZoo("cocknest", Sonics.cluck, Z =>
      {
        Z.Difficulty = Entities.cockatrice.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.dirt_floor;
        Z.Loot.AddKit(Chance.OneIn5, Dice.One, Items.egg);
        Z.AddSpawn(Chance.OneIn2, Count: null, new[] { Entities.cockatrice, Entities.pyrolisk, Entities.chickatrice, Entities.chicken, Entities.cockatoo });
      });

      college_of_wizardry = AddZoo("college of wizardry", Sonics.craft, Z =>
      {
        // These are too high level for college:
        // - Entities.occultist, Entities.transmuter, Entities.shifter 
        // These are hostiles:
        // - Entities.gnomish_wizard, Entities.leprechaun_wizard
        var CollegeEntityArray = new[]
        {
          Entities.earth_seeker, Entities.frost_seeker, Entities.flame_seeker, Entities.shock_seeker, Entities.water_seeker,
          Entities.earth_binder, Entities.frost_binder, Entities.flame_binder, Entities.shock_binder, Entities.water_binder,
          Entities.student, Entities.apprentice, Entities.embalmer
        };
        Debug.Assert(CollegeEntityArray.All(E => E.IsMercenary), "All college entities are intended to be mercenaries.");
        Debug.Assert(CollegeEntityArray.All(E => E.Level <= 21), "All college entities are expected to be less than level 20.");

        Z.Difficulty = CollegeEntityArray.Max(E => E.Difficulty) + 1;
        Z.Rarity = 2;
        Z.Feature = Features.workbench;
        Z.Ground = Grounds.obsidian_floor;
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.book_of_blank_paper);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.scroll_of_blank_paper);
        Z.Loot.AddKit(Chance.OneIn50, Dice.One, Items.magic_marker);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.wand_of_nothing);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.ring_of_naught);
        Z.AddSpawn(Chance.OneIn3, Count: null, CollegeEntityArray);
      });

      dragon_nest = AddZoo("dragon nest", Sonics.roar, Z =>
      {
        Z.Difficulty = Entities.adult_red_dragon.Difficulty + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.obsidian_floor;
        foreach (var DragonScaleItem in Items.DragonScales)
          Z.Loot.AddKit(Chance.OneIn(10 * Items.DragonScales.Count), Dice.One, DragonScaleItem);
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.egg); // TODO: these ought to be dragon eggs, but right now, will be a random egg.
        Z.AddSpawn(Chance.Always, Dice.Fixed(2), Codex.Evolutions.AdultDragons);
        Z.AddSpawn(Chance.OneIn3, Count: null, Codex.Evolutions.BabyDragons);
      });

      graveyard = AddZoo("graveyard", Sonics.moan, Z =>
      {
        Z.Difficulty = 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.dirt_floor;
        Z.Feature = Features.grave;
        Z.AddSpawn(Chance.Always, Dice.One, new[] { Entities.ghost });
      });

      gremlin_pit = AddZoo("gremlin pit", Sonics.cackle, Z =>
      {
        Z.Difficulty = Entities.gremlin.Difficulty + 1;
        Z.Rarity = 2;
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.tripe_ration);
        Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.fortune_cookie);
        Z.Ground = Grounds.obsidian_floor;
        Z.Device = Devices.water_trap;
        Z.AddSpawn(Chance.Always, Dice.One, new[] { Entities.gremlin });
      });

      leprechaun_hall = AddZoo("leprechaun hall", Sonics.giggle, Z =>
      {
        Z.Difficulty = Entities.leprechaun.Difficulty + 1;
        Z.Rarity = 2;
        Z.Loot.AddKit(Chance.Always, Dice.Zero, Items.gold_coin);
        Z.AddSpawn(Chance.Always, Dice.One, new[] { Entities.leprechaun_wizard });
        Z.AddSpawn(Chance.Always, Count: null, new[] { Entities.leprechaun });
      });

      salty_pool = AddZoo("salty pool", Sonics.water_splash, Z =>
      {
        var MarineArray = Entities.List.Where(E => E.HasOnlyTerrain(Materials.water)).ToArray();

        Z.Difficulty = MarineArray.Min(E => E.Level) + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.water;
        Z.AddSpawn(Chance.OneIn3, Count: null, MarineArray);
        Z.Loot.AddKit(Chance.OneIn10, 1.d5(), Items.kelp_frond);
      });

      science_lab = AddZoo("science lab", Sonics.potion, Z =>
      {
        Z.Difficulty = Math.Max(Math.Max(Entities.flesh_golem.Difficulty, Entities.quantum_mechanic.Difficulty), Entities.genetic_engineer.Difficulty) + 1;
        Z.Rarity = 2;
        Z.Ground = Grounds.marble_floor;
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.potion_of_acid);
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.potion_of_hallucination);
        Z.Loot.AddKit(Chance.OneIn10, Dice.One, Items.potion_of_speed);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.alchemy_smock);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.lab_coat);

        var EyewearArray = Items.List.Where(I => I.Type == ItemType.Eyewear && !I.Grade.Unique).ToArray();
        foreach (var Item in EyewearArray)
          Z.Loot.AddKit(Chance.OneIn(20 * EyewearArray.Length), Dice.One, Item);

        Z.AddSpawn(Chance.Always, Dice.One, new[] { Entities.flesh_golem });
        Z.AddSpawn(Chance.OneIn3, Count: null, new[] { Entities.quantum_mechanic, Entities.genetic_engineer });
      });

      spider_nest = AddZoo("spider nest", Sonics.scuttle, Z =>
      {
        Z.Difficulty = Entities.spider_queen.Difficulty + 1;
        Z.Rarity = 2;
        //Z.Loot.AddKit(Chance.OneIn8, Dice.One, Items.egg); // TODO: spider egg?
        Z.Ground = Grounds.dirt_floor;
        Z.Device = Devices.web;
        Z.AddSpawn(Chance.Always, 1.d4(), new[] { Entities.recluse_spider });
        Z.AddSpawn(Chance.Always, Dice.One, new[] { Entities.spider_queen });
      });

      slumber_party = AddZoo("slumber party", Sonics.sigh, Z =>
      {
        Z.Difficulty = Entities.mountain_nymph.Difficulty + 1;
        Z.Rarity = 2;
        Z.Loot.AddKit(Chance.ThreeIn7, Dice.Zero, Items.gold_coin);
        Z.Loot.AddKit(Chance.OneIn60, Dice.One, Items.crystal_ball);
        Z.Loot.AddKit(Chance.OneIn60, Dice.One, Items.magic_marker);
        Z.Loot.AddKit(Chance.OneIn60, Dice.One, Items.blindfold);
        Z.Loot.AddKit(Chance.OneIn60, Dice.One, Items.expensive_camera);
        Z.AddSpawn(Chance.Always, Count: null, new[] { Entities.mountain_nymph });
      });

      tavern = AddZoo("tavern", Sonics.quaff, Z =>
      {
        var TavernEntityArray = Codex.Kinds.mercenary.Entities.Where(E => E.Frequency > 0 && E.IsEncounter && E.Difficulty <= 20).ToArray();
        Debug.Assert(TavernEntityArray.All(E => E.IsMercenary), "All tavern entities are intended to be mercenaries.");

        Z.Difficulty = 15;
        Z.Rarity = 2;
        Z.Feature = null;
        Z.Ground = Grounds.stone_floor;
        Z.Loot.AddKit(Chance.OneIn5, Dice.One, Items.potion_of_booze);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.cheese);
        Z.Loot.AddKit(Chance.OneIn50, Dice.One, Items.apple);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.meat_stick);
        Z.Loot.AddKit(Chance.OneIn20, Dice.One, Items.fortune_cookie);
        Z.AddSpawn(Chance.OneIn3, Count: null, TavernEntityArray);
      });

      treasure_zoo = AddZoo("treasure zoo", Sonics.coins, Z =>
      {
        Z.Difficulty = 1;
        Z.Rarity = 8;
        Z.Loot.AddKit(Chance.ThreeIn4, Dice.Zero, Items.gold_coin);
        Z.AddSpawn(Chance.Always, Count: null, new Entity[] { });
      });

      // TODO: swamp.

      // TODO: royal chamber - throne & king and servants.
    }
#endif

    public readonly Zoo ant_hole;
    public readonly Zoo barracks;
    public readonly Zoo bee_hive;
    public readonly Zoo cocknest;
    public readonly Zoo college_of_wizardry;
    public readonly Zoo dragon_nest;
    public readonly Zoo graveyard;
    public readonly Zoo gremlin_pit;
    public readonly Zoo leprechaun_hall;
    public readonly Zoo salty_pool;
    public readonly Zoo science_lab;
    public readonly Zoo slumber_party;
    public readonly Zoo spider_nest;
    public readonly Zoo tavern;
    public readonly Zoo treasure_zoo;
  }
}