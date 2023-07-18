using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexDevices : CodexPage<ManifestDevices, DeviceEditor, Device>
  {
    private CodexDevices() { }
#if MASTER_CODEX
    internal CodexDevices(Codex Codex)
      : base(Codex.Manifest.Devices)
    {
      var Items = Codex.Items;
      var Entities = Codex.Entities;
      var Blocks = Codex.Blocks;
      var Explosions = Codex.Explosions;
      var Strikes = Codex.Strikes;
      var Properties = Codex.Properties;
      var Elements = Codex.Elements;
      var Glyphs = Codex.Glyphs;
      var Sonics = Codex.Sonics;
      var Attributes = Codex.Attributes;
      var Materials = Codex.Materials;
      var Sanctities = Codex.Sanctities;
      var Skills = Codex.Skills;
      var Volatiles = Codex.Volatiles;

      Device AddDevice(string Name, int Difficulty, Dice? RepeatDice, Glyph Glyph, Sonic TriggerSonic, Action<DeviceEditor> CompileAction)
      {
        return Register.Add(D =>
        {
          D.Name = Name;
          D.Difficulty = Difficulty;
          D.RepeatDice = RepeatDice;
          D.Glyph = Glyph;
          D.TriggerSonic = TriggerSonic;
          D.Size = Size.Small; // TODO: select size.
          D.Weight = Weight.FromUnits(2000); // TODO: select weight.
          D.SetRevealElement(Elements.force);

          CodexRecruiter.Enrol(() =>
          {
            CompileAction(D);

            Debug.Assert(D.Material != null, $"{D.Name} must select a material.");
          });
        });
      }

      acid_trap = AddDevice("acid trap", Difficulty: 12, RepeatDice: 4.d6(), Glyphs.acid_trap, Sonics.sizzle, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.acid;
        D.Audibility = 30;
        D.Material = Materials.iron;
        D.TriggerApply.Harm(Elements.acid, 5.d6());
      });

      alarm_trap = AddDevice("alarm trap", Difficulty: 5, RepeatDice: 1.d3() + 3, Glyphs.alarm_trap, Sonics.shriek, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.shriek;
        D.Material = Materials.iron;
        D.TriggerApply.Alert(Dice.Fixed(20));
        D.TriggerApply.CreateHorde(Dice.One);
      });

      amnesia_trap = AddDevice("amnesia trap", Difficulty: 13, RepeatDice: Dice.One, Glyphs.amnesia_trap, Sonics.magic, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.psychic;
        D.Audibility = 1;
        D.Material = Materials.iron;
        D.TriggerApply.Amnesia(Range.Sq15);
      });

      animation_trap = AddDevice("animation trap", Difficulty: 6, RepeatDice: Dice.One, Glyphs.animation_trap, Sonics.magic, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.magic;
        D.Material = Materials.iron;
        D.TriggerApply.Animate(ObjectEntity: Entities.animate_object, Corrupt: Properties.rage);
      });

      antimagic_field = AddDevice("anti-magic field", Difficulty: 12, RepeatDice: null, Glyphs.antimagic_field, Sonics.magic, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.magic;
        D.Material = Materials.iron;
        D.TriggerApply.Diminish(1.d10(), Modifier.Zero);
        D.TriggerApply.Cancellation(Elements.magical);
      });

      ant_hole = AddDevice("ant hole", Difficulty: 8, RepeatDice: 3.d6(), Glyphs.ant_hole, Sonics.thump, D =>
      {
        D.Frequency = 10;
        D.CrushedByBoulder = true;
        D.Material = Materials.clay;
        D.SetEscapeProperty(Properties.flight, Properties.levitation);
        D.TriggerApply.CreateEntity(Dice.One, Entities.giant_ant);
      });

      arrow_trap = AddDevice("arrow trap", Difficulty: 2, RepeatDice: 3.d6(), Glyphs.arrow_trap, Sonics.bow_fire, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Material = Materials.iron;
        D.AddMissile(Items.arrow);
        D.UntrapLoot.AddKit(2.d6(), Items.arrow);
      });

      bear_trap = AddDevice("bear trap", Difficulty: 1, RepeatDice: null, Glyphs.bear_trap, Sonics.clank, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.CrushedByBoulder = true;
        D.Material = Materials.iron;
        D.SetEscapeProperty(Properties.flight, Properties.levitation);
        D.UntrapLoot.AddKit(Items.beartrap);
        D.TriggerApply.Harm(Elements.physical, 1.d3());
        D.StuckDice = 1.d4() + 4;
      });

      bolt_trap = AddDevice("bolt trap", Difficulty: 3, RepeatDice: 3.d6(), Glyphs.bolt_trap, Sonics.bow_fire, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Material = Materials.iron;
        D.AddMissile(Items.crossbow_bolt);
        D.UntrapLoot.AddKit(2.d6(), Items.crossbow_bolt);
      });

      caltrops = AddDevice("caltrops", Difficulty: 1, RepeatDice: null, Glyphs.caltrop_trap, Sonics.thump, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Material = Materials.iron;
        D.SetEscapeProperty(Properties.flight, Properties.levitation);
        D.UntrapLoot.AddKit(Items.caltrops);
        D.TriggerApply.ApplyTransient(Properties.slowness, 4.d6());
        D.TriggerApply.Harm(Elements.physical, 1.d6());
      });

      dart_trap = AddDevice("dart trap", Difficulty: 1, RepeatDice: 4.d4(), Glyphs.dart_trap, Sonics.throw_object, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Material = Materials.iron;
        D.AddMissile(Items.dart, 90); // 90% normal dart
        D.AddMissile(Items.poison_dart, 10); // 10% poison dart.
        D.UntrapLoot.AddKit(Chance.Always, 2.d4(), Items.dart);
        D.UntrapLoot.AddKit(Chance.OneIn10, 1.d4(), Items.poison_dart);
      });

      entropy_trap = AddDevice("entropy trap", Difficulty: 10, RepeatDice: 1.d10() + 10, Glyphs.entropy_trap, Sonics.magic, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.magic;
        D.Material = Materials.iron;
        D.TriggerApply.WhenProbability(Table =>
        {
          Table.Add(5, A =>
          {
            A.ApplyTransient(Properties.blindness, 1.d5() + 10);
            A.CreateEntity(1.d4());
          });
          Table.Add(4, A =>
          {
            A.Harm(Elements.magical, 1.d20());
          });
          Table.Add(2, A =>
          {
            A.Rumour(Skills.literacy, Truth: true, Lies: true);
          });
          Table.Add(1, A =>
          {
            A.ApplyTransient(Properties.conflict, 3.d20());
          });
          Table.Add(1, A =>
          {
            A.ApplyTransient(Properties.rage, 3.d20());
          });
          Table.Add(1, A =>
          {
            A.ApplyTransient(Properties.warning, 3.d100());
          });
          Table.Add(1, A =>
          {
            A.ApplyTransient(Properties.aggravation, 3.d100());
          });
          Table.Add(1, A =>
          {
            A.IncreaseOneAbility(Dice.One);
          });
          Table.Add(1, A =>
          {
            A.DecreaseOneAbility(Dice.One);
          });
          Table.Add(1, A =>
          {
            A.TeleportInventoryAsset();
          });
          Table.Add(1, A =>
          {
            A.Amnesia(Range.Sq15);
          });
          Table.Add(1, A =>
          {
            A.CreateHorde(Dice.One);
          });
          Table.Add(1, A =>
          {
            A.Charm(Elements.magical, Delay.FromTurns(30000));
          });
          Table.Add(1, A =>
          {
            A.PlaceCurse(Dice.One, Sanctities.Cursed);
          });
          Table.Add(1, A =>
          {
            A.Unpunish();
            A.RemoveCurse(Dice.One);
          });
          Table.Add(1, A =>
          {
            A.AnimateObjects(ObjectEntity: Entities.animate_object, Corrupt: Properties.rage);
          });
          Table.Add(1, A =>
          {
            A.Recall();
          });
        });
      });

      explosive_trap = AddDevice("explosive trap", Difficulty: 6, RepeatDice: Dice.One, Glyphs.explosive_trap, Sonics.explosion, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Explosion = Explosions.fiery;
        D.Material = Materials.iron;
        D.TriggerApply.Harm(Elements.force, 3.d6());
        D.TriggerApply.ApplyTransient(Properties.stunned, 1.d6() + 2);
        D.TriggerApply.ApplyTransient(Properties.deafness, 4.d6() + 4);
        D.TriggerApply.WhenChance(Chance.ThreeIn4, T => T.CreateTrap(pit, Destruction: true));
        D.SetEscapeProperty(Properties.flight, Properties.levitation);
        D.UntrapLoot.AddKit(Items.land_mine);
      });

      falling_boulder_trap = AddDevice("falling boulder trap", Difficulty: 20, RepeatDice: Dice.One, Glyphs.falling_boulder_trap, Sonics.scrape, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Material = Materials.iron;
        D.SetMissileBlock(Blocks.stone_boulder);
      });

      falling_rock_trap = AddDevice("falling rock trap", Difficulty: 1, RepeatDice: 1.d4() + 2, Glyphs.falling_rock_trap, Sonics.scrape, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Material = Materials.iron;
        D.AddMissile(Items.rock);
        D.UntrapLoot.AddKit(1.d4() + 2, Items.rock);
      });

      fire_trap = AddDevice("fire trap", Difficulty: 5, RepeatDice: 4.d6(), Glyphs.fire_trap, Sonics.burn, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Explosion = Explosions.fiery;
        D.Audibility = 30;
        D.Material = Materials.iron;
        D.TriggerApply.Harm(Elements.fire, 2.d6() + 2);
        D.TriggerApply.WhenChance(Chance.OneIn3, T => T.CreateSpill(Volatiles.blaze, 1.d100() + 100));
      });

      grease_trap = AddDevice("grease trap", Difficulty: 1, RepeatDice: 1.d4() + 2, Glyphs.grease_trap, Sonics.splat, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Explosion = Explosions.muddy;
        D.Material = Materials.iron;
        D.TriggerApply.ApplyTransient(Properties.slippery, 10.d10());
        D.TriggerApply.ApplyTransient(Properties.fumbling, 10.d10());
        D.UntrapLoot.AddKit(Chance.OneIn4, Dice.One, Items.can_of_grease);
      });

      hallucination_trap = AddDevice("hallucination trap", Difficulty: 3, RepeatDice: Dice.One, Glyphs.hallucination_trap, Sonics.magic, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Explosion = Explosions.muddy;
        D.Material = Materials.iron;
        D.TriggerApply.ApplyTransient(Properties.hallucination, 10.d10());
      });

      hole = AddDevice("hole", Difficulty: 4, RepeatDice: null, Glyphs.hole, Sonics.thump, D =>
      {
        D.Frequency = 5; // less frequent than other traps.
        D.CrushedByBoulder = true;
        D.BoulderRemoval = true;
        D.Descent = true;
        D.Material = Materials.stone;
        D.TriggerApply.TransitionDescend(Teleport: null, Dice.One);
        D.TriggerApply.Harm(Elements.physical, 1.d6());
        D.SetEscapeProperty(Properties.flight, Properties.levitation);
      });

      hunger_trap = AddDevice("hunger trap", Difficulty: 11, RepeatDice: 1.d4() + 1, Glyphs.hunger_trap, Sonics.eat, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.spirit;
        D.Audibility = 30;
        D.Material = Materials.iron;
        D.TriggerApply.Malnutrition(4.d50() + 200);
      });

      ice_trap = AddDevice("ice trap", Difficulty: 7, RepeatDice: 4.d6(), Glyphs.ice_trap, Sonics.magic, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Explosion = Explosions.frosty;
        D.Audibility = 30;
        D.Material = Materials.iron;
        D.TriggerApply.Harm(Elements.cold, 4.d4());
      });

      level_teleporter = AddDevice("level teleporter", Difficulty: 5, RepeatDice: Dice.One, Glyphs.level_teleporter, Sonics.magic, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.magic;
        D.Material = Materials.iron;
        D.TriggerApply.TransitionRandom(Properties.teleportation, 1.d4()); // -4..+4
      });

      lightning_trap = AddDevice("lightning trap", Difficulty: 8, RepeatDice: 4.d6(), Glyphs.lightning_trap, Sonics.electricity, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Explosion = Explosions.electric;
        D.Audibility = 30;
        D.Material = Materials.iron;
        D.TriggerApply.Harm(Elements.shock, 4.d6());
        D.TriggerApply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.blindness, 1.d4() + 2));
      });

      noxious_pool = AddDevice("noxious pool", Difficulty: 7, RepeatDice: 1.d6(), Glyphs.noxious_pool, Sonics.sizzle, D =>
      {
        D.Frequency = 10;
        D.CrushedByBoulder = true;
        D.BoulderRemoval = true;
        D.Strike = Strikes.acid;
        D.Audibility = 20;
        D.Material = Materials.iron;
        D.TriggerApply.Harm(Elements.acid, 2.d6() + 2);
        D.SetEscapeProperty(Properties.flight, Properties.levitation);
      });

      pit = AddDevice("pit", Difficulty: 1, RepeatDice: null, Glyphs.pit, Sonics.thump, D =>
      {
        D.Description = "This square-cut hole is deep enough to hurt and hard to escape.";
        D.Frequency = 10;
        D.CrushedByBoulder = true;
        D.BoulderRemoval = true;
        D.StuckDice = 1.d6() + 2;
        D.StuckUnderground = true;
        D.Material = Materials.stone;
        D.TriggerApply.Harm(Elements.physical, 1.d6());
        D.SetEscapeProperty(Properties.flight, Properties.levitation);
      });

      polymorph_trap = AddDevice("polymorph trap", Difficulty: 8, RepeatDice: Dice.One, Glyphs.polymorph_trap, Sonics.magic, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.spirit;
        D.Material = Materials.iron;
        //D.SetEscapeProperty(Properties.MagicResistance); // it's not clear why the player escaped the polymorph trap.
        D.TriggerApply.Polymorph();
      });

      poison_gas_trap = AddDevice("poison gas trap", Difficulty: 9, RepeatDice: 1.d20(), Glyphs.poison_gas_trap, Sonics.gas, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.gas;
        D.Material = Materials.iron;
        D.TriggerApply.Harm(Elements.poison, 3.d4());
        D.TriggerApply.UnlessTargetResistant(Elements.poison, B => B.DecreaseAbility(Attributes.strength, Dice.One));
      });

      scything_blade_trap = AddDevice("scything blade trap", Difficulty: 19, 2.d6(), Glyphs.scything_blade_trap, Sonics.weapon, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Audibility = 30;
        D.Material = Materials.iron;
        D.TriggerApply.Harm(Elements.physical, 2.d20() + 2);
        // TODO: chance of a decapitation?
        D.UntrapLoot.AddKit(1.d2() + 1, Items.scimitar); // 2..3
      });

      shock_trap = AddDevice("shock trap", Difficulty: 18, RepeatDice: 1.d6(), Glyphs.shock_trap, Sonics.electricity, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.flash;
        D.Audibility = 30;
        D.Material = Materials.iron;
        D.TriggerApply.Harm(Elements.shock, 8.d6());
        D.TriggerApply.ApplyTransient(Properties.blindness, 1.d4() + 2);
        D.TriggerApply.ApplyTransient(Properties.stunned, 1.d4() + 2);
      });

      silence_trap = AddDevice("silence trap", Difficulty: 2, RepeatDice: 1.d20(), Glyphs.silence_trap, Sonics.gas, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.gas;
        D.Material = Materials.iron;
        D.TriggerApply.ApplyTransient(Properties.silence, 10.d10());
      });

      sleeping_gas_trap = AddDevice("sleeping gas trap", Difficulty: 2, RepeatDice: 1.d20(), Glyphs.sleeping_gas_trap, Sonics.gas, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.gas;
        D.Material = Materials.iron;
        D.TriggerApply.ApplyTransient(Properties.sleeping, 5.d20());
      });

      spear_trap = AddDevice("spear trap", Difficulty: 2, RepeatDice: 1.d6(), Glyphs.spear_trap, Sonics.throw_object, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.AddMissile(Items.javelin);
        D.Material = Materials.iron;
        D.UntrapLoot.AddKit(1.d5() + 2, Items.javelin); // 3..7
      });

      spiked_pit = AddDevice("spiked pit", Difficulty: 5, null, Glyphs.spiked_pit, Sonics.thump, D =>
      {
        D.Frequency = 10;
        D.CrushedByBoulder = true;
        D.BoulderRemoval = true;
        D.StuckDice = 1.d6() + 2;
        D.StuckUnderground = true;
        D.Material = Materials.iron;
        D.TriggerApply.Harm(Elements.physical, 2.d6());
        D.TriggerApply.WhenChance(Chance.OneIn4, A => A.UnlessTargetResistant(Elements.poison, B => B.DecreaseAbility(Attributes.strength, Dice.One)));
        D.SetEscapeProperty(Properties.flight, Properties.levitation);
      });

      //StatueTrap = AddDevice("statue trap", Dice.One, 8, Glyphs.statue_trap, Sonics.Magic);
      //StatueTrap.Frequency = 10;
      // TODO: statue turns into monster?

      squeaky_board = AddDevice("squeaky board", Difficulty: 1, RepeatDice: 1.d4() + 1, Glyphs.squeaky_board, Sonics.creak, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.CrushedByBoulder = true; // squeaky board covers a hole.
        D.BoulderRemoval = true;
        D.Material = Materials.wood;
        D.TriggerApply.Alert(1.d6() + 5); // wake up near by
        D.ExhaustApply.CreateTrap(hole, Destruction: false); // worn out board reveals a hole.
        D.SetEscapeProperty(Properties.flight, Properties.levitation);
      });

      trapdoor = AddDevice("trapdoor", Difficulty: 1, RepeatDice: null, Glyphs.trapdoor, Sonics.thump, D =>
      {
        D.Frequency = 5;
        D.CrushedByBoulder = true;
        D.BoulderRemoval = true;
        D.Descent = true;
        D.Material = Materials.wood;
        D.TriggerApply.TransitionDescend(Teleport: null, 1.d3());
        D.SetEscapeProperty(Properties.flight, Properties.levitation);
      });

      teleporter = AddDevice("teleporter", Difficulty: 1, RepeatDice: null, Glyphs.teleporter, Sonics.magic, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Strike = Strikes.magic;
        D.Material = Materials.iron;
        D.TriggerApply.TeleportFloorAsset();
        D.TriggerApply.TeleportCharacter(Properties.teleportation);
      });

      toxic_trap = AddDevice("toxic trap", Difficulty: 15, RepeatDice: Dice.One, Glyphs.toxic_trap, Sonics.sizzle, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Explosion = Explosions.acid;
        D.Audibility = 30;
        D.Material = Materials.iron;
        D.TriggerApply.Harm(Elements.acid, 6.d6());
        D.TriggerApply.CreateTrap(acid_trap, Destruction: false);
      });

      water_trap = AddDevice("water trap", Difficulty: 1, RepeatDice: 2.d6(), Glyphs.water_trap, Sonics.water_crash, D =>
      {
        D.Frequency = 10;
        D.UntrapSkill = Skills.traps;
        D.UntrapAttribute = Attributes.dexterity;
        D.Explosion = Explosions.watery;
        D.Audibility = 30;
        D.Material = Materials.iron;
        D.TriggerApply.Harm(Elements.water, Dice.Zero);
        D.TriggerApply.WhenChance(Chance.OneIn20, T => T.ConvertAsset(Codex.Stocks.potion, WholeStack: true, Items.potion_of_water));
        D.TriggerApply.WhenChance(Chance.OneIn20, T => T.ConvertAsset(Codex.Stocks.scroll, WholeStack: true, Items.scroll_of_blank_paper));
        D.TriggerApply.WhenChance(Chance.OneIn20, T => T.ConvertAsset(Codex.Stocks.book, WholeStack: true, Items.book_of_blank_paper));
      });

      web = AddDevice("web", Difficulty: 7, RepeatDice: 1.d4() + 1, Glyphs.web, Sonics.thump, D =>
      {
        D.Frequency = 10;
        D.CrushedByBoulder = true;
        D.Material = Materials.vegetable;
        D.SetDestroyWithElement(Elements.fire);
        D.SetEscapeProperty(Properties.free_action, Properties.slippery);
        D.StuckDice = 1.d4() + 4; // TODO: reduced by strength.
        D.TriggerApply.CreateEntity(Dice.One, Entities.giant_spider);
      });

      Register.Alias(entropy_trap, "magic trap");
    }
#endif

    public readonly Device acid_trap;
    public readonly Device alarm_trap;
    public readonly Device amnesia_trap;
    public readonly Device ant_hole;
    public readonly Device animation_trap;
    public readonly Device antimagic_field;
    public readonly Device arrow_trap;
    public readonly Device bear_trap;
    public readonly Device bolt_trap;
    public readonly Device caltrops;
    public readonly Device dart_trap;
    public readonly Device explosive_trap;
    public readonly Device falling_boulder_trap;
    public readonly Device falling_rock_trap;
    public readonly Device fire_trap;
    public readonly Device grease_trap;
    public readonly Device hallucination_trap;
    public readonly Device hunger_trap;
    public readonly Device hole;
    public readonly Device ice_trap;
    public readonly Device level_teleporter;
    public readonly Device lightning_trap;
    public readonly Device entropy_trap;
    public readonly Device noxious_pool;
    public readonly Device pit;
    public readonly Device polymorph_trap;
    public readonly Device poison_gas_trap;
    public readonly Device scything_blade_trap;
    public readonly Device shock_trap;
    public readonly Device silence_trap;
    public readonly Device sleeping_gas_trap;
    public readonly Device spear_trap;
    public readonly Device spiked_pit;
    public readonly Device squeaky_board;
    public readonly Device trapdoor;
    public readonly Device teleporter;
    public readonly Device toxic_trap;
    public readonly Device water_trap;
    public readonly Device web;
  }
}