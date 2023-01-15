using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexEvolutions : CodexPage<ManifestEvolutions, EvolutionEditor, Evolution>
  {
    private CodexEvolutions() { }
#if MASTER_CODEX
    internal CodexEvolutions(Codex Codex)
      : base(Codex.Manifest.Evolutions)
    {
      var Entities = Codex.Entities;

      Evolution AddEvolution(params Entity[] EntityArray)
      {
        Debug.Assert(EntityArray.Length > 0 && EntityArray.IsDistinct() && EntityArray.All(E => E.Evolution == null));

        CodexRecruiter.Enrol(() =>
        {
          var Previous = (Entity)null;
          foreach (var Entity in EntityArray)
          {
            if (Previous != null)
            {
              Debug.Assert(Previous.Level < Entity.Level, Previous.Name);
              Debug.Assert(Previous.Weight <= Entity.Weight, $"{Previous.Name} weighs {Previous.Weight.GetUnits()} and {Entity.Name} weighs {Entity.Weight.GetUnits()}");
            }

            Previous = Entity;
          }
        });

        return Register.Add(E =>
        {
          E.SetEntities(EntityArray);
        });
      }

      var BabyDragonList = new Inv.DistinctList<Entity>(11);
      var AdultDragonList = new Inv.DistinctList<Entity>(11);

      void CheckDragon(Entity Baby, Entity Young, Entity Adult, Entity Ancient)
      {
        Debug.Assert(Baby.Name.StartsWith("baby ") && Baby.Name.EndsWith(" dragon"), Baby.Name);
        Debug.Assert(Young.Name.StartsWith("young ") && Young.Name.EndsWith(" dragon"), Young.Name);
        Debug.Assert(Adult.Name.StartsWith("adult ") && Adult.Name.EndsWith(" dragon"), Adult.Name);
        Debug.Assert(Ancient.Name.StartsWith("ancient ") && Ancient.Name.EndsWith(" dragon"), Ancient.Name);
        Debug.Assert(new[] { Baby, Young, Adult, Ancient }.Select(E => E.Name.Split(' ')[1]).Distinct().ToArray().Length == 1, Baby.Name);

        CodexRecruiter.Enrol(() =>
        {
          // defence.
          Debug.Assert(Young.Defence.Baseline > Baby.Defence.Baseline, Young.Name);
          Debug.Assert(Adult.Defence.Baseline > Young.Defence.Baseline, Adult.Name);
          Debug.Assert(Ancient.Defence.Baseline > Adult.Defence.Baseline, Ancient.Name);

          // bias.
          Debug.Assert(Baby.Defence.Bias.Pierce == Modifier.Plus1, Baby.Name);
          Debug.Assert(Young.Defence.Bias.Pierce == Modifier.Plus1, Young.Name);
          Debug.Assert(Adult.Defence.Bias.Pierce == Modifier.Plus1, Adult.Name);
          Debug.Assert(Ancient.Defence.Bias.Pierce == Modifier.Plus1, Ancient.Name);

          // levels.
          Debug.Assert(Young.Level > Baby.Level, Young.Name);
          Debug.Assert(Adult.Level > Young.Level, Adult.Name);
          Debug.Assert(Ancient.Level > Adult.Level, Ancient.Name);

          // difficulty.
          Debug.Assert(Young.Difficulty > Baby.Difficulty, Young.Name);
          Debug.Assert(Adult.Difficulty > Young.Difficulty, Adult.Name);
          Debug.Assert(Ancient.Difficulty > Adult.Difficulty, Ancient.Name);

          // Challenge.
          Debug.Assert(Young.Challenge > Baby.Challenge, Young.Name);
          Debug.Assert(Adult.Challenge > Young.Challenge, Adult.Name);
          Debug.Assert(Ancient.Challenge > Adult.Challenge, Ancient.Name);

          // young needs a breath attack.
          Debug.Assert(Young.Attacks.Any(A => A.Type == AttackType.Breath), Young.Name);

          // ancient can no longer fly but can jump.
          Debug.Assert(!Ancient.Startup.Talents.Contains(Codex.Properties.flight), Ancient.Name);
          Debug.Assert(Ancient.Startup.Talents.Contains(Codex.Properties.jumping), Ancient.Name);

          foreach (var Attribute in Codex.Attributes.List)
          {
            Debug.Assert(Young.DefaultForm[Attribute].Score > Baby.DefaultForm[Attribute].Score, Young.Name + " " + Attribute.Name);
            Debug.Assert(Ancient.DefaultForm[Attribute].Score > Adult.DefaultForm[Attribute].Score, Ancient.Name + " " + Attribute.Name);
          }
        });

        // assign evolutions.
        AddEvolution(Baby, Young, Adult, Ancient);

        BabyDragonList.Add(Baby);
        AdultDragonList.Add(Adult);
      }

      CheckDragon(Entities.baby_black_dragon, Entities.young_black_dragon, Entities.adult_black_dragon, Entities.ancient_black_dragon);
      CheckDragon(Entities.baby_blue_dragon, Entities.young_blue_dragon, Entities.adult_blue_dragon, Entities.ancient_blue_dragon);
      CheckDragon(Entities.baby_deep_dragon, Entities.young_deep_dragon, Entities.adult_deep_dragon, Entities.ancient_deep_dragon);
      CheckDragon(Entities.baby_green_dragon, Entities.young_green_dragon, Entities.adult_green_dragon, Entities.ancient_green_dragon);
      CheckDragon(Entities.baby_gold_dragon, Entities.young_gold_dragon, Entities.adult_gold_dragon, Entities.ancient_gold_dragon);
      CheckDragon(Entities.baby_orange_dragon, Entities.young_orange_dragon, Entities.adult_orange_dragon, Entities.ancient_orange_dragon);
      CheckDragon(Entities.baby_red_dragon, Entities.young_red_dragon, Entities.adult_red_dragon, Entities.ancient_red_dragon);
      CheckDragon(Entities.baby_shimmering_dragon, Entities.young_shimmering_dragon, Entities.adult_shimmering_dragon, Entities.ancient_shimmering_dragon);
      CheckDragon(Entities.baby_silver_dragon, Entities.young_silver_dragon, Entities.adult_silver_dragon, Entities.ancient_silver_dragon);
      CheckDragon(Entities.baby_white_dragon, Entities.young_white_dragon, Entities.adult_white_dragon, Entities.ancient_white_dragon);
      CheckDragon(Entities.baby_yellow_dragon, Entities.young_yellow_dragon, Entities.adult_yellow_dragon, Entities.ancient_yellow_dragon);

      this.AdultDragonArray = AdultDragonList.ToArray();
      this.BabyDragonArray = BabyDragonList.ToArray();

      AddEvolution(Entities.giant_tick, Entities.giant_flea, Entities.giant_louse);

      AddEvolution(Entities.migo_drone, Entities.migo_warrior);

      AddEvolution(Entities.bat, Entities.giant_bat);

      AddEvolution(Entities.fledgling_raven, Entities.juvenile_raven, Entities.adult_raven);

      AddEvolution(/*Entities.chicken, */Entities.chickatrice, Entities.cockatrice);

      AddEvolution(Entities.dingo_puppy, Entities.dingo, Entities.large_dingo);

      AddEvolution(Entities.little_dog, Entities.dog, Entities.large_dog);

      AddEvolution(Entities.hell_hound_pup, Entities.hell_hound);

      AddEvolution(Entities.winter_wolf_cub, Entities.winter_wolf);

      AddEvolution(Entities.elf_lord, Entities.elf_king);

      AddEvolution(Entities.kitten, Entities.housecat, Entities.large_cat);

      AddEvolution(Entities.shadow, Entities.shade);

      AddEvolution(Entities.gnoll, Entities.gnoll_warrior, Entities.gnoll_chieftain);

      AddEvolution(Entities.gnome_thief, Entities.gnome_lord, Entities.gnome_warrior, Entities.gnome_king);

      AddEvolution(Entities.keystone_kop, Entities.keystone_sergeant, Entities.keystone_lieutenant, Entities.keystone_captain);

      AddEvolution(Entities.soldier, Entities.lieutenant, Entities.captain);

      AddEvolution(Entities.watchman, Entities.watch_captain);

      AddEvolution(Entities.deep_one, Entities.deeper_one, Entities.deepest_one);

      AddEvolution(Entities.dwarf_thief, Entities.dwarf_warrior, Entities.dwarf_lord, Entities.dwarf_king);

      AddEvolution(Entities.mind_flayer, Entities.master_mind_flayer);

      AddEvolution(Entities.manes, Entities.lemure);

      AddEvolution(Entities.imp, Entities.blood_imp);

      AddEvolution(Entities.kobold, Entities.large_kobold, Entities.kobold_lord, Entities.kobold_king);

      AddEvolution(Entities.lich, Entities.demilich, Entities.master_lich, Entities.archlich);

      AddEvolution(Entities.lizardman_warrior, Entities.lizardman_berserker, Entities.lizardman_chieftain);

      AddEvolution(Entities.baby_crocodile, Entities.crocodile);

      AddEvolution(Entities.pile_of_killer_coins, Entities.large_pile_of_killer_coins, Entities.huge_pile_of_killer_coins);

      AddEvolution(Entities.small_mimic, Entities.large_mimic, Entities.giant_mimic);

      AddEvolution(Entities.black_naga_hatchling, Entities.black_naga);
      AddEvolution(Entities.golden_naga_hatchling, Entities.golden_naga);
      AddEvolution(Entities.guardian_naga_hatchling, Entities.guardian_naga);
      AddEvolution(Entities.red_naga_hatchling, Entities.red_naga);

      AddEvolution(Entities.ogre, Entities.ogre_lord, Entities.ogre_king);

      AddEvolution(Entities.orc_grunt, Entities.orc_warrior, Entities.orc_captain);

      AddEvolution(Entities.jabberwock, Entities.vorpal_jabberwock);

      AddEvolution(Entities.shoggoth, Entities.giant_shoggoth);

      AddEvolution(Entities.lamb, Entities.sheep);

      AddEvolution(Entities.sewer_rat, Entities.giant_rat, Entities.black_rat, Entities.pack_rat, Entities.rat_king);

      AddEvolution(Entities.cave_spider, Entities.giant_spider);

      AddEvolution(Entities.pony, Entities.horse, Entities.warhorse);

      AddEvolution(Entities.vampire, Entities.vampire_lord, Entities.vampire_king);

      AddEvolution(Entities.baby_long_worm, Entities.long_worm);

      AddEvolution(Entities.baby_purple_worm, Entities.purple_worm);

      AddEvolution(Entities.larva, Entities.maggot, Entities.dung_worm);

      AddEvolution(Entities.monkey, Entities.ape, Entities.carnivorous_ape);

      AddEvolution(Entities.ghast, Entities.ghoul);

      AddEvolution(Entities.spark_bug, Entities.arc_bug, Entities.lightning_bug);

      AddEvolution(Entities.yeoman_warder, Entities.yeoman, Entities.chief_yeoman_warder);

      AddEvolution(Entities.earth_seeker, Entities.earth_binder, Entities.earth_maker);
      AddEvolution(Entities.flame_seeker, Entities.flame_binder, Entities.flame_maker);
      AddEvolution(Entities.frost_seeker, Entities.frost_binder, Entities.frost_maker);
      AddEvolution(Entities.shock_seeker, Entities.shock_binder, Entities.shock_maker);
      AddEvolution(Entities.water_seeker, Entities.water_binder, Entities.water_maker);

      // sphere -> elemental.
      AddEvolution(Entities.earth_sphere, Entities.earth_elemental);
      AddEvolution(Entities.flame_sphere, Entities.fire_elemental);
      AddEvolution(Entities.frost_sphere, Entities.ice_elemental);
      AddEvolution(Entities.shock_sphere, Entities.air_elemental);
      AddEvolution(Entities.water_sphere, Entities.water_elemental);
    }
#endif

    public IReadOnlyList<Entity> BabyDragons => BabyDragonArray;
    public IReadOnlyList<Entity> AdultDragons => AdultDragonArray;

    private readonly Entity[] BabyDragonArray;
    private readonly Entity[] AdultDragonArray;
  }
}