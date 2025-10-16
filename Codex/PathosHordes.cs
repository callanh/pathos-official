using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexHordes : CodexPage<ManifestHordes, HordeEditor, Horde>
  {
    private CodexHordes() { }
#if MASTER_CODEX
    internal CodexHordes(Codex Codex)
      : base(Codex.Manifest.Hordes)
    {
      var Entities = Codex.Entities;

      Horde AddHorde(string Name, Entity LeaderEntity, Action<HordeEditor> CreateAction)
      {
        return Register.Add(H =>
        {
          H.Name = Name;
          H.LeaderEntity = LeaderEntity;

          CodexRecruiter.Enrol(() => CreateAction(H));
        });
      }

      ape = AddHorde("ape", null, H =>
      {
        H.AddMinion(Entities.ape, 1.d3());
      });

      ant = AddHorde("ant", null, H =>
      {
        H.AddMinion(Entities.snow_ant, Dice.One);
        H.AddMinion(Entities.fire_ant, Dice.One);
        H.AddMinion(Entities.soldier_ant, 1.d2());
        H.AddMinion(Entities.giant_ant, 1.d3());
      });

      bat = AddHorde("bat", null, H =>
      {
        H.AddMinion(Entities.bat, 1.d3());
        H.AddMinion(Entities.giant_bat, 1.d2());
        H.AddMinion(Entities.vampire_bat, Dice.One);
      });

      bee = AddHorde("bee", null, H =>
      {
        H.AddMinion(Entities.killer_bee, 1.d10());
      });

      beetle = AddHorde("beetle", null, H =>
      {
        H.AddMinion(Entities.giant_beetle, 1.d2() + 1);
      });

      cockroach = AddHorde("cockroach", null, H =>
      {
        H.AddMinion(Entities.giant_cockroach, 1.d3());
      });

      couatl = AddHorde("couatl", null, H =>
      {
        H.AddMinion(Entities.couatl, 1.d3());
      });

      coyote = AddHorde("coyote", null, H =>
      {
        H.AddMinion(Entities.coyote, 1.d3());
      });

      demon = AddHorde("demon", null, H =>
      {
        H.AddMinion(Entities.horned_devil, 1.d2() - 1);
        H.AddMinion(Entities.barbed_devil, 1.d2() - 1);
        H.AddMinion(Entities.bone_devil, 1.d2() - 1);
        H.AddMinion(Entities.ice_devil, 1.d2() - 1); // 0..1
        H.AddMinion(Entities.water_demon, 1.d2() - 1); // 0..1
        H.AddMinion(Entities.pit_fiend, 1.d2() - 1); // 0..1
        H.AddMinion(Entities.marilith, 1.d2() - 1); // 0..1
        H.AddMinion(Entities.erinys, 1.d2() - 1); // 0..1
        H.AddMinion(Entities.vrock, 1.d2() - 1); // 0..1
        H.AddMinion(Entities.hezrou, 1.d2() - 1); // 0..1
        H.AddMinion(Entities.balrog, 1.d2() - 1); // 0..1
        H.AddMinion(Entities.nalfeshnee, 1.d2() - 1); // 0..1
      });

      drow = AddHorde("drow", null, H =>
      {
        H.AddCavalry(Entities.drow, Entities.giant_spider, 1.d3() + 1);
      });

      dwarf = AddHorde("dwarf", Entities.dwarf_king, H =>
      {
        H.AddMinion(Entities.dwarf_lord, 1.d2());
        H.AddMinion(Entities.dwarf_warrior, 1.d3());
      });

      elf = AddHorde("elf", Entities.elf_king, H =>
      {
        H.AddMinion(Entities.elf_lord, 1.d2());
        H.AddMinion(Entities.green_elf, 1.d3() - 1);
        H.AddMinion(Entities.grey_elf, 1.d3() - 1);
        H.AddMinion(Entities.woodland_elf, 1.d3() - 1);
      });

      electric_bug = AddHorde("electric bug", null, H =>
      {
        H.AddMinion(Entities.spark_bug, 1.d3());
        H.AddMinion(Entities.arc_bug, 1.d2());
        H.AddMinion(Entities.lightning_bug, Dice.One);
      });

      flea = AddHorde("flea", null, H =>
      {
        H.AddMinion(Entities.giant_flea, 2.d6());
      });

      garter_snake = AddHorde("garter snake", null, H =>
      {
        H.AddMinion(Entities.garter_snake, 1.d8() + 2);
      });

      gnome = AddHorde("gnome", Entities.gnome_king, H =>
      {
        H.AddMinion(Entities.gnome_lord, Dice.One);
        H.AddMinion(Entities.gnome_warrior, 1.d3());
        H.AddMinion(Entities.gnome_thief, Dice.One);
        H.AddMinion(Entities.gnomish_wizard, Dice.One);
      });

      goblin = AddHorde("goblin", Entities.goblin_king, H =>
      {
        H.AddMinion(Entities.hobgoblin, 1.d2());
        H.AddMinion(Entities.goblin, 1.d3());
        H.AddCavalry(Entities.goblin, Entities.giant_slug, 1.d2());
        H.AddCavalry(Entities.hobgoblin, Entities.giant_frog, Dice.One);
      });

      gibberling = AddHorde("gibberling", null, H =>
      {
        H.AddMinion(Entities.gibberling, 2.d10()); // 2-20
      });

      grimlock = AddHorde("grimlock", null, H =>
      {
        H.AddMinion(Entities.grimlock, 2.d10()); // 2-20
      });

      harpy = AddHorde("harpy", null, H =>
      {
        H.AddMinion(Entities.harpy, 1.d4());
      });

      hell_hound = AddHorde("hell hound", Entities.hell_hound, H =>
      {
        H.AddMinion(Entities.hell_hound_pup, 1.d3());
      });

      jackal = AddHorde("jackal", null, H =>
      {
        H.AddMinion(Entities.jackal, 1.d3());
      });

      keystone = AddHorde("keystone", Entities.keystone_captain, H =>
      {
        H.ArriveSonic = Codex.Sonics.whistle;
        H.AddMinion(Entities.keystone_lieutenant, 1.d2());
        H.AddMinion(Entities.keystone_sergeant, 1.d4());
        H.AddMinion(Entities.keystone_kop, 1.d6());
      });

      kobold = AddHorde("kobold", Entities.kobold_king, H =>
      {
        H.AddMinion(Entities.kobold_warrior, Dice.One);
        H.AddMinion(Entities.kobold_lord, Dice.One);
        H.AddMinion(Entities.kobold_shaman, Dice.One);
        H.AddMinion(Entities.large_kobold, Dice.One);
        H.AddMinion(Entities.kobold_scout, 1.d3());
        // NOTE: rock_kobold, swamp_kobold are outside of kobold 'society' and would have their own hordes?
      });

      lizardman = AddHorde("lizardman", Entities.lizardman_chieftain, H =>
      {
        H.AddMinion(Entities.lizardman_shaman, 1.d2() - 1);
        H.AddMinion(Entities.lizardman_mage, 1.d2() - 1);
        H.AddMinion(Entities.lizardman_berserker, 1.d2() - 1);
        H.AddMinion(Entities.lizardman_warrior, 1.d3() + 1);
      });

      lemure = AddHorde("lemure", null, H =>
      {
        H.AddMinion(Entities.lemure, 1.d10());
      });

      louse = AddHorde("louse", null, H =>
      {
        H.AddMinion(Entities.giant_louse, 2.d6());
      });

      mane = AddHorde("mane", null, H =>
      {
        H.AddMinion(Entities.manes, 1.d10());
      });

      mangler = AddHorde("mangler", null, H =>
      {
        H.AddMinion(Entities.mangler, 1.d4());
      });

      migo = AddHorde("migo", Entities.migo_queen, H =>
      {
        H.AddMinion(Entities.migo_warrior, 1.d2());
        H.AddMinion(Entities.migo_drone, 1.d4() + 2);
      });

      mould = AddHorde("mould", null, H =>
      {
        H.AddMinion(Entities.black_mould, Dice.One);
        H.AddMinion(Entities.yellow_mould, Dice.One);
        H.AddMinion(Entities.red_mould, Dice.One);
        H.AddMinion(Entities.disgusting_mould, Dice.One);
        H.AddMinion(Entities.brown_mould, Dice.One);
        H.AddMinion(Entities.green_mould, Dice.One);
      });

      orc = AddHorde("orc", Entities.orc_captain, H =>
      {
        H.AddMinion(Entities.orc_shaman, 1.d2());
        H.AddMinion(Entities.orc_grunt, 1.d3());
        H.AddMinion(Entities.orc_warrior, 2.d4());
      });

      ogre = AddHorde("ogre", Entities.ogre_king, H =>
      {
        H.AddMinion(Entities.ogre_lord, 1.d2());
        H.AddMinion(Entities.ogre, 1.d3());
      });

      // TODO: would be difficult to generate... needs to be placed on a water room.
      //piranha = AddHorde("piranha", null, H =>
      //{
      //  H.AddMinion(Entities.piranha, 2.d3());
      //});

      rat = AddHorde("rat", Entities.wererat, H =>
      {
        H.AddMinion(Entities.giant_rat, 1.d4());
        H.AddMinion(Entities.sewer_rat, 1.d3());
        H.AddMinion(Entities.rabid_rat, 1.d2());
      });

      rothe = AddHorde("rothé", null, H =>
      {
        H.AddMinion(Entities.rothe, 1.d2() + 1);
      });

      scorpion = AddHorde("scorpion", null, H =>
      {
        H.AddMinion(Entities.scorpion, 1.d2() + 1); // 2-3
        H.AddMinion(Entities.giant_scorpion, Dice.One); // 1
      });

      spider = AddHorde("spider", null, H =>
      {
        H.AddMinion(Entities.cave_spider, 1.d2() + 1);
      });

      spore = AddHorde("spore", null, H =>
      {
        H.AddMinion(Entities.gas_spore, 3.d4());
      });

      termite = AddHorde("termite", null, H =>
      {
        H.AddMinion(Entities.giant_termite, 2.d4());
      });

      tick = AddHorde("tick", null, H =>
      {
        H.AddMinion(Entities.giant_tick, 2.d6());
      });

      tsetse_fly = AddHorde("tsetse fly", null, H =>
      {
        H.AddMinion(Entities.tsetse_fly, 1.d4() + 4);
      });

      wasp = AddHorde("wasp", null, H =>
      {
        H.AddMinion(Entities.giant_wasp, Dice.One);
        H.AddMinion(Entities.yellow_jacket, 1.d3());
        H.AddMinion(Entities.black_wasp, 1.d4());
      });

      water_moccasin = AddHorde("water moccasin", null, H =>
      {
        H.AddMinion(Entities.water_moccasin, 1.d8() + 2);
      });

      warg = AddHorde("warg", null, H =>
      {
        H.AddMinion(Entities.warg, 1.d3());
        H.AddCavalry(Entities.orc_grunt, Entities.warg, 1.d3());
        H.AddCavalry(Entities.orc_warrior, Entities.warg, 1.d2());
        H.AddCavalry(Entities.orc_captain, Entities.warg, Dice.One);
      });

      winter_wolf = AddHorde("winter wolf", Entities.winter_wolf, H =>
      {
        H.AddMinion(Entities.winter_wolf_cub, 1.d2() + 1);
      });

      wolf = AddHorde("wolf", Entities.werewolf, H =>
      {
        H.AddMinion(Entities.wolf, 1.d2() + 1);
      });

      worm = AddHorde("worm", null, H =>
      {
        H.AddMinion(Entities.larva, 1.d4()); // lvl 1
        H.AddMinion(Entities.maggot, 1.d3()); // lvl 2
        H.AddMinion(Entities.dung_worm, 1.d2()); // lvl 3
      });

      yeoman = AddHorde("yeoman", Entities.chief_yeoman_warder, H =>
      {
        H.AddMinion(Entities.yeoman_warder, 1.d2() + 1);
        H.AddMinion(Entities.yeoman, 2.d3());
      });

      zombie = AddHorde("zombie", null, H =>
      {
        H.AddMinion(Entities.kobold_zombie, 1.d2());
        H.AddMinion(Entities.gnome_zombie, 1.d2());
        H.AddMinion(Entities.orc_zombie, 1.d2());
        H.AddMinion(Entities.dwarf_zombie, 1.d2());
        H.AddMinion(Entities.elf_zombie, 1.d2());
        H.AddMinion(Entities.human_zombie, 1.d2());
        H.AddMinion(Entities.ettin_zombie, Dice.One);
        H.AddMinion(Entities.giant_zombie, Dice.One);
      });
    }
#endif

    public readonly Horde ant;
    public readonly Horde ape;
    public readonly Horde bat;
    public readonly Horde bee;
    public readonly Horde beetle;
    public readonly Horde cockroach;
    public readonly Horde couatl;
    public readonly Horde coyote;
    public readonly Horde demon;
    public readonly Horde drow;
    public readonly Horde dwarf;
    public readonly Horde elf;
    public readonly Horde electric_bug;
    public readonly Horde flea;
    public readonly Horde garter_snake;
    public readonly Horde gnome;
    public readonly Horde goblin;
    public readonly Horde gibberling;
    public readonly Horde grimlock;
    public readonly Horde harpy;
    public readonly Horde hell_hound;
    public readonly Horde jackal;
    public readonly Horde keystone;
    public readonly Horde kobold;
    public readonly Horde lizardman;
    public readonly Horde lemure;
    public readonly Horde louse;
    public readonly Horde mane;
    public readonly Horde mangler;
    public readonly Horde migo;
    public readonly Horde mould;
    public readonly Horde orc;
    public readonly Horde ogre;
    //public readonly Horde piranha;
    public readonly Horde rat;
    public readonly Horde rothe;
    public readonly Horde scorpion;
    public readonly Horde spider;
    public readonly Horde spore;
    public readonly Horde termite;
    public readonly Horde tick;
    public readonly Horde tsetse_fly;
    public readonly Horde warg;
    public readonly Horde wasp;
    public readonly Horde water_moccasin;
    public readonly Horde winter_wolf;
    public readonly Horde wolf;
    public readonly Horde worm;
    public readonly Horde yeoman;
    public readonly Horde zombie;
  }
}
