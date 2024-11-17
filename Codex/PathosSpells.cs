using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexSpells : CodexPage<ManifestSpells, SpellEditor, Spell>
  {
    private CodexSpells() { }
#if MASTER_CODEX
    internal CodexSpells(Codex Codex)
      : base(Codex.Manifest.Spells)
    {
      var Schools = Codex.Schools;
      var Beams = Codex.Beams;
      var Strikes = Codex.Strikes;
      var Explosions = Codex.Explosions;
      var Properties = Codex.Properties;
      var Elements = Codex.Elements;
      var Stocks = Codex.Stocks;
      var Items = Codex.Items;
      var Glyphs = Codex.Glyphs;
      var Qualifications = Codex.Qualifications;
      var Sanctities = Codex.Sanctities;
      var Kinds = Codex.Kinds;
      var Entities = Codex.Entities;
      var Devices = Codex.Devices;
      var Gates = Codex.Gates;

      Spell AddSpell(School School, string Name, int Level, Precept Precept, Glyph Glyph, Action<SpellEditor> Action)
      {
        Debug.Assert(School != null);
        Debug.Assert(Name != null);
        Debug.Assert(Glyph != null);

        return Register.Add(S =>
        {
          S.School = School;
          S.Name = Name;
          S.Level = Level;
          S.Mana = Level * 5;
          S.Precept = Precept ?? new Precept(Purpose.Unspecified);
          S.Glyph = Glyph;

          CodexRecruiter.Enrol(() => Action(S));
        });
      }

      void SetAdept(SpellEditor Spell, Action<AdeptEditor> Unskilled, Action<AdeptEditor> Proficient, Action<AdeptEditor> Specialist, Action<AdeptEditor> Expert, Action<AdeptEditor> Master, Action<AdeptEditor> Champion)
      {
        Champion?.Invoke(Spell.SetAdept(Qualifications.champion));
        Master?.Invoke(Spell.SetAdept(Qualifications.master));
        Expert?.Invoke(Spell.SetAdept(Qualifications.expert));
        Specialist?.Invoke(Spell.SetAdept(Qualifications.specialist));
        Proficient?.Invoke(Spell.SetAdept(Qualifications.proficient));
        Unskilled?.Invoke(Spell.SetAdept(Qualification: null));
      }

      acid_stream = AddSpell(Schools.evocation, "acid stream", 4, new Precept(Purpose.Blast, Elements.acid), Glyphs.acid_stream_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.acid, 1.d4() + 2);
            U.Apply.HarmEntity(Elements.acid, 4.d6());
          },
          P =>
          {
            P.SetCast().Beam(Beams.acid, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.acid, 6.d6());
            P.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.acid, R => R.ApplyTransient(Properties.rage, 2.d4() + 2)));
          },
          S =>
          {
            S.SetCast().Beam(Beams.acid, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.acid, 8.d6());
            S.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.acid, R => R.ApplyTransient(Properties.rage, 3.d4() + 3)));
          },
          E =>
          {
            E.SetCast().Beam(Beams.acid, 1.d4() + 7);
            E.Apply.HarmEntity(Elements.acid, 10.d6());
            E.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.acid, R => R.ApplyTransient(Properties.rage, 4.d4() + 4)));
          },
          M =>
          {
            M.SetCast().Beam(Beams.acid, 1.d4() + 9);
            M.Apply.HarmEntity(Elements.acid, 12.d6());
            M.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.acid, R => R.ApplyTransient(Properties.rage, 5.d4() + 5)));
          },
          C =>
          {
            C.SetCast().Beam(Beams.acid, 1.d4() + 11);
            C.Apply.HarmEntity(Elements.acid, 14.d6());
            C.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.acid, R => R.ApplyTransient(Properties.rage, 6.d4() + 6)));
          }
        );
      });

      animate_dead = AddSpell(Schools.necromancy, "animate dead", 2, new Precept(Purpose.SummonAlly, new[] { Items.animal_corpse, Items.vegetable_corpse }), Glyphs.animate_dead_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            U.Apply.AnimateRevenant(CorruptProperty: Properties.rage, CorruptDice: 6.d10());
          },
          // TODO: adept scaling of effects.
          P =>
          {
            P.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            P.Apply.AnimateRevenant(CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          S =>
          {
            S.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            S.Apply.AnimateRevenant(CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          E =>
          {
            E.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            E.Apply.AnimateRevenant(CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          M =>
          {
            M.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            M.Apply.AnimateRevenant(CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          C =>
          {
            C.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            C.Apply.AnimateRevenant(CorruptProperty: null, CorruptDice: Dice.Zero);
          }
        );
      });

      animate_object = AddSpell(Schools.enchantment, "animate object", 5, new Precept(Purpose.Blast), Glyphs.animate_object_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, 1.d4() + 1)
             .SetObjects();
            U.Apply.AnimateObject(ObjectEntity: Entities.animate_object, CorruptProperty: Properties.rage, CorruptDice: 6.d10());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, 1.d4() + 3)
             .SetObjects();
            P.Apply.AnimateObject(ObjectEntity: Entities.animate_object, CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, 1.d4() + 5)
             .SetObjects();
            S.Apply.AnimateObject(ObjectEntity: Entities.animate_object, CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, 1.d4() + 7)
             .SetObjects();
            E.Apply.AnimateObject(ObjectEntity: Entities.animate_object, CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, 1.d4() + 9)
             .SetObjects();
            M.Apply.AnimateObject(ObjectEntity: Entities.animate_object, CorruptProperty: null, CorruptDice: Dice.Zero);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, 1.d4() + 11)
             .SetObjects();
            C.Apply.AnimateObject(ObjectEntity: Entities.animate_object, CorruptProperty: null, CorruptDice: Dice.Zero);
          }
        );
      });

      cancellation = AddSpell(Schools.transmutation, "cancellation", 7, new Precept(Purpose.Blast), Glyphs.cancellation_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, 1.d4() + 1)
             .SetObjects();
            U.Apply.Cancellation(Elements.magical);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 3)
             .SetObjects();
            P.Apply.Cancellation(Elements.magical);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 5)
             .SetObjects();
            S.Apply.Cancellation(Elements.magical);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 7)
             .SetObjects();
            E.Apply.Cancellation(Elements.magical);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 9)
             .SetObjects();
            M.Apply.Cancellation(Elements.magical);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 11)
             .SetObjects();
            C.Apply.Cancellation(Elements.magical);
          }
        );
      });

      charm = AddSpell(Schools.enchantment, "charm", 3, null, Glyphs.charm_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Fixed(1))
             .SetTargetSelf(false);
            U.Apply.CharmEntity(Elements.magical, Delay.FromTurns(10000), Kinds.Living.ToArray());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.Fixed(1))
             .SetTargetSelf(false);
            P.Apply.CharmEntity(Elements.magical, Delay.FromTurns(20000), Kinds.Living.ToArray());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2))
             .SetTargetSelf(false);
            S.Apply.CharmEntity(Elements.magical, Delay.FromTurns(30000), Kinds.Living.ToArray());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3))
             .SetTargetSelf(false);
            E.Apply.CharmEntity(Elements.magical, Delay.FromTurns(40000), Kinds.Living.ToArray());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4))
             .SetTargetSelf(false);
            M.Apply.CharmEntity(Elements.magical, Delay.FromTurns(50000), Kinds.Living.ToArray());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5))
             .SetTargetSelf(false);
            C.Apply.CharmEntity(Elements.magical, Delay.FromTurns(60000), Kinds.Living.ToArray());
          }
        );
      });

      // TODO: clairvoyance is not useful to the non-prime player character.
      clairvoyance = AddSpell(Schools.divination, "clairvoyance", 3, null/*new Precept(Purpose.Buff, Properties.Clairvoyance)*/, Glyphs.clairvoyance_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Zero);
            U.Apply.ApplyTransient(Properties.clairvoyance, 1.d6());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.Zero);
            P.Apply.ApplyTransient(Properties.clairvoyance, 3.d6());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Zero);
            S.Apply.ApplyTransient(Properties.clairvoyance, 6.d6());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Zero);
            E.Apply.ApplyTransient(Properties.clairvoyance, 9.d6());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Zero);
            M.Apply.ApplyTransient(Properties.clairvoyance, 12.d6());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Zero);
            C.Apply.ApplyTransient(Properties.clairvoyance, 15.d6());
          }
        );
      });

      cone_of_cold = AddSpell(Schools.evocation, "cone of cold", 5, new Precept(Purpose.Blast, Elements.cold), Glyphs.cone_of_cold_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.cold, 1.d4() + 1);
            U.Apply.HarmEntity(Elements.cold, 4.d4() + 4);
          },
          P =>
          {
            P.SetCast().Beam(Beams.cold, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.cold, 6.d4() + 6);
            P.Apply.WhenChance(Chance.OneIn8, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.paralysis, 1.d2() + 1)));
          },
          S =>
          {
            S.SetCast().Beam(Beams.cold, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.cold, 8.d4() + 8);
            S.Apply.WhenChance(Chance.OneIn6, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.paralysis, 1.d4() + 2)));
          },
          E =>
          {
            E.SetCast().Beam(Beams.cold, 1.d4() + 7);
            E.Apply.HarmEntity(Elements.cold, 10.d4() + 10);
            E.Apply.WhenChance(Chance.OneIn4, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.paralysis, 1.d4() + 3)));
          },
          M =>
          {
            M.SetCast().Beam(Beams.cold, 1.d4() + 9);
            M.Apply.HarmEntity(Elements.cold, 12.d4() + 12);
            M.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.paralysis, 1.d4() + 4)));
          },
          C =>
          {
            C.SetCast().Beam(Beams.cold, 1.d4() + 11);
            C.Apply.HarmEntity(Elements.cold, 14.d4() + 14);
            C.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.paralysis, 1.d4() + 5)));
          }
        );
      });

      var ConfusionPrecept = new Precept(Purpose.Blast, Properties.confusion);
      var StunnedPrecept = new Precept(Purpose.Blast, Properties.stunned);

      confusion = AddSpell(Schools.enchantment, "confusion", 2, Precept: null, Glyphs.confusion_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.Precept = ConfusionPrecept;
            U.SetCast().Strike(Strikes.psychic, Dice.One);
            U.Apply.ApplyTransient(Properties.confusion, 2.d6());
          },
          P =>
          {
            P.Precept = ConfusionPrecept;
            P.SetCast().Strike(Strikes.psychic, Dice.One);
            P.Apply.ApplyTransient(Properties.confusion, 4.d6());
          },
          S =>
          {
            S.Precept = ConfusionPrecept;
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2));
            S.Apply.ApplyTransient(Properties.confusion, 6.d6());
          },
          E =>
          {
            E.Precept = StunnedPrecept;
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3));
            E.Apply.ApplyTransient(Properties.confusion, 8.d6());
            E.Apply.ApplyTransient(Properties.stunned, 1.d6());
          },
          M =>
          {
            M.Precept = StunnedPrecept;
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4));
            M.Apply.ApplyTransient(Properties.confusion, 10.d6());
            M.Apply.ApplyTransient(Properties.stunned, 2.d6());
          },
          C =>
          {
            C.Precept = StunnedPrecept;
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5));
            C.Apply.ApplyTransient(Properties.confusion, 12.d6());
            C.Apply.ApplyTransient(Properties.stunned, 3.d6());
          }
        );
      });

      bind_undead = AddSpell(Schools.necromancy, "bind undead", 5, null, Glyphs.bind_undead_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Fixed(1))
             .SetTargetSelf(false);
            U.Apply.CharmEntity(Elements.magical, Delay.FromTurns(10000), Kinds.Undead.ToArray());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.Fixed(1))
             .SetTargetSelf(false);
            P.Apply.CharmEntity(Elements.magical, Delay.FromTurns(20000), Kinds.Undead.ToArray());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Fixed(2))
             .SetTargetSelf(false);
            S.Apply.CharmEntity(Elements.magical, Delay.FromTurns(40000), Kinds.Undead.ToArray());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Fixed(3))
             .SetTargetSelf(false);
            E.Apply.CharmEntity(Elements.magical, Delay.FromTurns(60000), Kinds.Undead.ToArray());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Fixed(4))
             .SetTargetSelf(false);
            M.Apply.CharmEntity(Elements.magical, Delay.FromTurns(80000), Kinds.Undead.ToArray());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Fixed(5))
             .SetTargetSelf(false);
            C.Apply.CharmEntity(Elements.magical, Delay.FromTurns(100000), Kinds.Undead.ToArray());
          }
        );
      });

      create_familiar = AddSpell(Schools.conjuration, "create familiar", 6, new Precept(Purpose.SummonAlly), Glyphs.create_familiar_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.WhenProbability(Table =>
            {
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.giant_bat)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.chicken)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.lichen));
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.lizard));
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.giant_cockroach));
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.newt));
            });
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.WhenProbability(Table =>
            {
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.kitten)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.little_dog)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.fledgling_raven)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.black_rat)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.monkey)); // 2
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.pony)); // 3
            });
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            S.Apply.WhenProbability(Table =>
            {
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.housecat)); // 4
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.dog)); // 4
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.juvenile_raven)); // 4
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.pack_rat)); // 4
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.ape)); // 4
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.horse)); // 5
            });
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            E.Apply.WhenProbability(Table =>
            {
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.large_cat)); // 6
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.large_dog)); // 6
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.adult_raven)); // 6
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.rat_king)); // 6
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.carnivorous_ape)); // 6
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.warhorse)); // 7
            });
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            M.Apply.WhenProbability(Table =>
            {
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.tiger)); // 12
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.leocrotta)); // 13
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.wolverine)); // 13
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.wyvern)); // 16
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.bugbear)); // 17
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.mountain_centaur)); // 18
            });
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            C.Apply.WhenProbability(Table =>
            {
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.komodo_dragon)); // 21
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.sabretoothed_cat)); // 23
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.pegasus)); // 24
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.giant_scorpion)); // 25
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.king_cobra)); // 29
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.elephant)); // 35
              Table.Add(1, A => A.SummonEntity(Dice.One, Entities.adult_white_dragon, Entities.adult_black_dragon, Entities.adult_red_dragon, Entities.adult_blue_dragon, Entities.adult_green_dragon));
            });
          }
        );
      });

      curing = AddSpell(Schools.clerical, "curing", 2, null, Glyphs.curing_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.One);
            U.Apply.RemoveTransient(Properties.blindness, Properties.deafness, Properties.inebriation);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetAfflictionOverride();
            P.Apply.UnafflictEntity();
            P.Apply.RemoveTransient(Properties.blindness, Properties.deafness, Properties.inebriation, Properties.sickness);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetAfflictionOverride();
            S.Apply.UnafflictEntity();
            S.Apply.RemoveTransient(Properties.blindness, Properties.deafness, Properties.inebriation, Properties.sickness, Properties.hallucination);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetAfflictionOverride();
            E.Apply.UnafflictEntity();
            E.Apply.RemoveTransient(Properties.blindness, Properties.deafness, Properties.inebriation, Properties.sickness, Properties.hallucination, Properties.confusion, Properties.stunned);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetAfflictionOverride();
            M.Apply.UnafflictEntity();
            M.Apply.RemoveTransient(Properties.blindness, Properties.deafness, Properties.inebriation, Properties.sickness, Properties.hallucination, Properties.confusion, Properties.stunned, Properties.rage);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetAfflictionOverride();
            C.Apply.UnafflictEntity();
            C.Apply.RemoveTransient(Properties.blindness, Properties.deafness, Properties.inebriation, Properties.sickness, Properties.hallucination, Properties.confusion, Properties.stunned, Properties.rage, Properties.fear);
          }
        );
      });

      detect_food = AddSpell(Schools.divination, "detect food", 2, null, Glyphs.detect_food_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.DetectItem(Range.Sq10, Stocks.food);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            P.Apply.DetectItem(Range.Sq15, Stocks.food);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            S.Apply.DetectItem(Range.Sq20, Stocks.food);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            E.Apply.DetectItem(Range.Sq25, Stocks.food, Stocks.potion);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            M.Apply.DetectItem(Range.Sq30, Stocks.food, Stocks.potion);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            C.Apply.DetectItem(Range.Sq35, Stocks.food, Stocks.potion);
          }
        );
      });

      detect_monsters = AddSpell(Schools.divination, "detect monsters", 1, null, Glyphs.detect_monsters_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.DetectEntity(Range.Sq10);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            P.Apply.DetectEntity(Range.Sq15);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            S.Apply.DetectEntity(Range.Sq20);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            E.Apply.DetectEntity(Range.Sq25);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            M.Apply.DetectEntity(Range.Sq30);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            C.Apply.DetectEntity(Range.Sq35);
          }
        );
      });

      detect_treasure = AddSpell(Schools.divination, "detect treasure", 4, null, Glyphs.detect_treasure_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.DetectItem(Range.Sq10, Stocks.gem);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            P.Apply.DetectItem(Range.Sq15, Stocks.gem);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            S.Apply.DetectItem(Range.Sq20, Stocks.gem, Stocks.ring);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            E.Apply.DetectItem(Range.Sq25, Stocks.gem, Stocks.ring, Stocks.amulet);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            M.Apply.DetectItem(Range.Sq30, Stocks.gem, Stocks.ring, Stocks.amulet, Stocks.wand);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            C.Apply.DetectItem(Range.Sq35, Stocks.gem, Stocks.ring, Stocks.amulet, Stocks.wand, Stocks.book);
          }
        );
      });

      detect_unseen = AddSpell(Schools.divination, "detect unseen", 3, new Precept(Purpose.Buff, Properties.see_invisible), Glyphs.detect_unseen_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.ApplyTransient(Properties.see_invisible, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Fixed(1))
             .SetTerminates();
            P.Apply.ApplyTransient(Properties.see_invisible, 1.d15() + 91);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.ApplyTransient(Properties.see_invisible, 1.d15() + 121);
            S.Apply.ApplyTransient(Properties.searching, 1.d15() + 121);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.ApplyTransient(Properties.see_invisible, 1.d15() + 151);
            E.Apply.ApplyTransient(Properties.searching, 1.d15() + 151);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.Searching(Range.Sq5);
            M.Apply.ApplyTransient(Properties.see_invisible, 1.d15() + 181);
            M.Apply.ApplyTransient(Properties.searching, 1.d15() + 181);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.Searching(Range.Sq10);
            C.Apply.ApplyTransient(Properties.see_invisible, 1.d15() + 211);
            C.Apply.ApplyTransient(Properties.searching, 1.d15() + 211);
          }
        );
      });

      dig = AddSpell(Schools.transmutation, "dig", 5, null, Glyphs.dig_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.digging, 1.d4() + 1)
             .SetBounces(false)
             .SetPenetrates();
            U.Apply.Digging(Elements.digging);
          },
          P =>
          {
            P.SetCast().Beam(Beams.digging, 1.d4() + 3)
             .SetBounces(false)
             .SetPenetrates();
            P.Apply.Digging(Elements.digging);
          },
          S =>
          {
            S.SetCast().Beam(Beams.digging, 1.d4() + 5)
             .SetBounces(false)
             .SetPenetrates();
            S.Apply.Digging(Elements.digging);
          },
          E =>
          {
            E.SetCast().Beam(Beams.digging, 1.d4() + 7)
             .SetBounces(false)
             .SetPenetrates();
            E.Apply.Digging(Elements.digging);
          },
          M =>
          {
            M.SetCast().Beam(Beams.digging, 1.d4() + 9)
             .SetBounces(false)
             .SetPenetrates();
            M.Apply.Digging(Elements.digging);
          },
          C =>
          {
            C.SetCast().Beam(Beams.digging, 1.d4() + 11)
             .SetBounces(false)
             .SetPenetrates();
            C.Apply.Digging(Elements.digging);
          }
        );
      });

      disintegrate = AddSpell(Schools.transmutation, "disintegrate", 7, new Precept(Purpose.Blast, Elements.disintegrate), Glyphs.disintegrate_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.disintegration, 1.d4() + 1);
            U.Apply.HarmEntity(Elements.disintegrate, 4.d6() + 4);
          },
          P =>
          {
            P.SetCast().Beam(Beams.disintegration, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.disintegrate, 6.d6() + 6);
          },
          S =>
          {
            S.SetCast().Beam(Beams.disintegration, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.disintegrate, 8.d6() + 8);
          },
          E =>
          {
            E.SetCast().Beam(Beams.disintegration, 1.d4() + 7);
            E.Apply.HarmEntity(Elements.disintegrate, 10.d6() + 10);
          },
          M =>
          {
            M.SetCast().Beam(Beams.disintegration, 1.d4() + 9);
            M.Apply.HarmEntity(Elements.disintegrate, 12.d6() + 12);
          },
          C =>
          {
            C.SetCast().Beam(Beams.disintegration, 1.d4() + 11);
            C.Apply.HarmEntity(Elements.disintegrate, 14.d6() + 14);
          }
        );
      });

      drain_life = AddSpell(Schools.necromancy, "drain life", 3, new Precept(Purpose.Blast), Glyphs.drain_life_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            U.Apply.DrainLife(Elements.drain, 1.d6() + 1);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            P.Apply.DrainLife(Elements.drain, 2.d6() + 2);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, 1.d4() + 1) // 2-5
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            S.Apply.DrainLife(Elements.drain, 3.d6() + 3);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, 1.d6() + 2) // 3-8
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            E.Apply.DrainLife(Elements.drain, 4.d6() + 4);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, 1.d8() + 3) // 4-11
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            M.Apply.DrainLife(Elements.drain, 5.d6() + 5);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, 1.d8() + 4) // 5-12
             .SetTargetSelf(false)
             .SetPenetrates(false)
             .SetTerminates();
            C.Apply.DrainLife(Elements.drain, 6.d6() + 6);
          }
        );
      });

      healing = AddSpell(Schools.clerical, "healing", 1, new Precept(Purpose.Healing), Glyphs.healing_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero); // only self.
            U.Apply.HealEntity(5.d2(), Modifier.Zero);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.HealEntity(5.d2(), Modifier.Zero);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.HealEntity(10.d2(), Modifier.Zero);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.HealEntity(15.d2(), Modifier.Zero);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.HealEntity(20.d2(), Modifier.Zero);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.HealEntity(25.d2(), Modifier.Zero);
          }
        );
      });

      extra_healing = AddSpell(Schools.clerical, "extra healing", 3, new Precept(Purpose.Healing), Glyphs.extra_healing_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero); // only self.
            U.Apply.HealEntity(5.d4(), Modifier.Zero);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.HealEntity(5.d4(), Modifier.Zero);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.HealEntity(10.d4(), Modifier.Zero);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.HealEntity(15.d4(), Modifier.Zero);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.HealEntity(20.d4(), Modifier.Zero);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.HealEntity(25.d4(), Modifier.Zero);
          }
        );
      });

      full_healing = AddSpell(Schools.clerical, "full healing", 5, new Precept(Purpose.Healing), Glyphs.full_healing_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero); // only self.
            U.Apply.HealEntity(5.d8(), Modifier.Zero);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.HealEntity(5.d8(), Modifier.Zero);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.HealEntity(10.d8(), Modifier.Zero);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.HealEntity(15.d8(), Modifier.Zero);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.HealEntity(20.d8(), Modifier.Zero);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.HealEntity(25.d8(), Modifier.Zero);
          }
        );
      });

      fear = AddSpell(Schools.enchantment, "fear", 3, new Precept(Purpose.AreaOfEffect, Properties.fear), Glyphs.fear_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Zero);
            U.Apply.AreaTransient(Properties.fear, 2.d6(), Kinds.Living.ToArray());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.Zero);
            P.Apply.AreaTransient(Properties.fear, 3.d6(), Kinds.Living.ToArray());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.Zero);
            S.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.Living.ToArray());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.Zero);
            E.Apply.AreaTransient(Properties.fear, 5.d6(), Kinds.Living.ToArray());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.Zero);
            M.Apply.AreaTransient(Properties.fear, 6.d6(), Kinds.Living.ToArray());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Zero);
            C.Apply.AreaTransient(Properties.fear, 7.d7(), Kinds.Living.ToArray());
          }
        );
      });

      finger_of_death = AddSpell(Schools.necromancy, "finger of death", 7, new Precept(Purpose.Blast), Glyphs.finger_of_death_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.death, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates();
            U.Apply.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.death, Dice.One)
             .SetTargetSelf(false)
             .SetPenetrates();
            P.Apply.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.death, Dice.Fixed(2))
             .SetTargetSelf(false)
             .SetPenetrates();
            S.Apply.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.death, Dice.Fixed(3))
             .SetTargetSelf(false)
             .SetPenetrates();
            E.Apply.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.death, Dice.Fixed(4))
             .SetTargetSelf(false)
             .SetPenetrates();
            M.Apply.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.death, Dice.Fixed(5))
             .SetTargetSelf(false)
             .SetPenetrates();
            C.Apply.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          }
        );
      });

      fireball = AddSpell(Schools.evocation, "fireball", 4, new Precept(Purpose.Blast, Elements.fire), Glyphs.fireball_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.fiery, 1.d6());
            U.Apply.HarmEntity(Elements.fire, 6.d6());
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.fiery, 1.d6() + 3);
            P.Apply.HarmEntity(Elements.fire, 6.d6());
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.fiery, 1.d6() + 4);
            S.Apply.HarmEntity(Elements.fire, 8.d6());
            S.Apply.ApplyTransient(Properties.deafness, 1.d6() + 1);
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.fiery, 1.d6() + 5);
            E.Apply.HarmEntity(Elements.fire, 10.d6());
            E.Apply.ApplyTransient(Properties.deafness, 2.d6() + 2);
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.fiery, 1.d6() + 6);
            M.Apply.HarmEntity(Elements.fire, 12.d6());
            M.Apply.ApplyTransient(Properties.deafness, 4.d6() + 4);
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.fiery, 1.d6() + 7);
            C.Apply.HarmEntity(Elements.fire, 14.d6());
            C.Apply.ApplyTransient(Properties.deafness, 6.d6() + 6);
          }
        );
      });

      ice_storm = AddSpell(Schools.evocation, "ice storm", 4, new Precept(Purpose.Blast, Elements.cold), Glyphs.ice_storm_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Explosion(Explosions.frosty, 1.d6() + 2);
            U.Apply.HarmEntity(Elements.cold, 2.d4() + 2);
          },
          P =>
          {
            P.SetCast().Explosion(Explosions.frosty, 1.d6() + 3);
            P.Apply.HarmEntity(Elements.cold, 4.d4() + 4);
          },
          S =>
          {
            S.SetCast().Explosion(Explosions.frosty, 1.d6() + 4);
            S.Apply.HarmEntity(Elements.cold, 6.d4() + 6);
            S.Apply.UnlessTargetResistant(Elements.cold, R =>
            {
              R.ApplyTransient(Properties.fumbling, 1.d6() + 4);
            });
          },
          E =>
          {
            E.SetCast().Explosion(Explosions.frosty, 1.d6() + 5);
            E.Apply.HarmEntity(Elements.cold, 8.d4() + 8);
            E.Apply.UnlessTargetResistant(Elements.cold, R =>
            {
              R.ApplyTransient(Properties.fumbling, 1.d6() + 4);
              R.ApplyTransient(Properties.slowness, 1.d6() + 4);
            });
          },
          M =>
          {
            M.SetCast().Explosion(Explosions.frosty, 1.d6() + 6);
            M.Apply.HarmEntity(Elements.cold, 10.d4() + 10);
            M.Apply.UnlessTargetResistant(Elements.cold, R =>
            {
              R.ApplyTransient(Properties.fumbling, 1.d6() + 4);
              R.ApplyTransient(Properties.slowness, 1.d6() + 4);
              R.ApplyTransient(Properties.paralysis, 1.d6() + 4);
            });
          },
          C =>
          {
            C.SetCast().Explosion(Explosions.frosty, 1.d6() + 6);
            C.Apply.HarmEntity(Elements.cold, 12.d4() + 12);
            C.Apply.UnlessTargetResistant(Elements.cold, R =>
            {
              R.ApplyTransient(Properties.fumbling, 1.d6() + 6);
              R.ApplyTransient(Properties.slowness, 1.d6() + 6);
              R.ApplyTransient(Properties.paralysis, 1.d6() + 6);
            });
          }
        );
      });

      flaming_sphere = AddSpell(Schools.conjuration, "flaming sphere", 2, new Precept(Purpose.SummonAlly, Elements.fire), Glyphs.flaming_sphere_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.Fixed(1), Entities.flame_sphere);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.flame_sphere);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.flame_sphere);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Fixed(2), Constructed: true, Entities.flame_sphere);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Fixed(3), Constructed: true, Entities.flame_sphere);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Fixed(4), Constructed: true, Entities.flame_sphere);
          }
        );
      });

      freezing_sphere = AddSpell(Schools.conjuration, "freezing sphere", 2, new Precept(Purpose.SummonAlly, Elements.cold), Glyphs.freezing_sphere_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.Fixed(1), Entities.frost_sphere);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.frost_sphere);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.frost_sphere);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Fixed(2), Constructed: true, Entities.frost_sphere);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Fixed(3), Constructed: true, Entities.frost_sphere);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Fixed(4), Constructed: true, Entities.frost_sphere);
          }
        );
      });

      shocking_sphere = AddSpell(Schools.conjuration, "shocking sphere", 2, new Precept(Purpose.SummonAlly, Elements.shock), Glyphs.shocking_sphere_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.Fixed(1), Entities.shock_sphere);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.shock_sphere);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.shock_sphere);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Fixed(2), Constructed: true, Entities.shock_sphere);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Fixed(3), Constructed: true, Entities.shock_sphere);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Fixed(4), Constructed: true, Entities.shock_sphere);
          }
        );
      });

      soaking_sphere = AddSpell(Schools.conjuration, "soaking sphere", 2, new Precept(Purpose.SummonAlly, Elements.shock), Glyphs.soaking_sphere_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.Fixed(1), Entities.water_sphere);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.water_sphere);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.water_sphere);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Fixed(2), Constructed: true, Entities.water_sphere);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Fixed(3), Constructed: true, Entities.water_sphere);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Fixed(4), Constructed: true, Entities.water_sphere);
          }
        );
      });

      crushing_sphere = AddSpell(Schools.conjuration, "crushing sphere", 2, new Precept(Purpose.SummonAlly, Elements.shock), Glyphs.crushing_sphere_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.Fixed(1), Entities.earth_sphere);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.earth_sphere);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2))
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Fixed(1), Constructed: true, Entities.earth_sphere);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3))
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Fixed(2), Constructed: true, Entities.earth_sphere);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4))
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Fixed(3), Constructed: true, Entities.earth_sphere);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5))
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Fixed(4), Constructed: true, Entities.earth_sphere);
          }
        );
      });

      force_bolt = AddSpell(Schools.evocation, "force bolt", 1, new Precept(Purpose.Blast, Elements.force), Glyphs.force_bolt_spell, Z =>
      {
        Z.Description = "Projects a ball of energy that impacts on both monsters and objects.";
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, 2.d3() + 1)
             .SetTargetSelf(false)
             .SetObjects()
             .SetPenetrates();
            U.Apply.HarmEntity(Elements.force, 1.d6() + 1);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 2.d3() + 2)
             .SetTargetSelf(false)
             .SetObjects()
             .SetPenetrates();
            P.Apply.HarmEntity(Elements.force, 2.d6() + 2);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 2.d3() + 3)
             .SetTargetSelf(false)
             .SetObjects()
             .SetPenetrates();
            S.Apply.HarmEntity(Elements.force, 3.d6() + 3);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 2.d3() + 4)
             .SetTargetSelf(false)
             .SetObjects()
             .SetPenetrates();
            E.Apply.HarmEntity(Elements.force, 4.d6() + 4);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 2.d3() + 5)
             .SetTargetSelf(false)
             .SetObjects()
             .SetPenetrates();
            M.Apply.HarmEntity(Elements.force, 5.d6() + 5);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 2.d3() + 6)
             .SetTargetSelf(false)
             .SetObjects()
             .SetPenetrates();
            C.Apply.HarmEntity(Elements.force, 6.d6() + 6);
          }
        );
      });

      haste = AddSpell(Schools.abjuration, "haste", 3, new Precept(Purpose.Buff, Properties.quickness), Glyphs.haste_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.magic, Dice.Zero);
            U.Apply.ApplyTransient(Properties.quickness, 1.d10() + 50);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.magic, Dice.One);
            P.Apply.ApplyTransient(Properties.quickness, 1.d10() + 100);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.magic, Dice.One);
            S.Apply.ApplyTransient(Properties.quickness, 1.d10() + 200);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.magic, Dice.One);
            E.Apply.ApplyTransient(Properties.quickness, 1.d10() + 300);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.magic, Dice.One);
            M.Apply.ApplyTransient(Properties.quickness, 1.d10() + 400);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.magic, Dice.One);
            C.Apply.ApplyTransient(Properties.quickness, 1.d10() + 500);
          }
        );
      });

      identify = AddSpell(Schools.divination, "identify", 5, new Precept(Purpose.Identify), Glyphs.identify_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Plain(Dice.Zero) // identify a random item.
             .SetTerminates();
            U.Apply.IdentifyItem(All: false, Sanctity: null);
          },
          P =>
          {
            P.SetCast().FilterIdentified(false)
             .SetTerminates();
            P.Apply.IdentifyItem(All: false, Sanctity: null);
          },
          null,
          null,
          null,
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.Zero)
             .SetTerminates();
            C.Apply.IdentifyItem(All: true, Sanctity: null);
          }
        );
      });

      invisibility = AddSpell(Schools.abjuration, "invisibility", 4, new Precept(Purpose.Buff, Properties.invisibility), Glyphs.invisibility_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.invisibility, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.invisibility, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.invisibility, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.invisibility, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.invisibility, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.invisibility, 1.d15() + 151);
          }
        );
      });

      blinking = AddSpell(Schools.abjuration, "blinking", 1, new Precept(Purpose.Buff, Properties.blinking), Glyphs.blinking_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.blinking, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.blinking, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.blinking, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.blinking, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.blinking, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.blinking, 1.d15() + 151);
          }
        );
      });

      phasing = AddSpell(Schools.enchantment, "phasing", 6, new Precept(Purpose.Buff, Properties.phasing), Glyphs.phasing_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.phasing, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.phasing, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.phasing, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.phasing, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.phasing, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.phasing, 1.d15() + 151);
          }
        );
      });

      jumping = AddSpell(Schools.abjuration, "jumping", 2, new Precept(Purpose.Buff, Properties.jumping), Glyphs.jumping_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.jumping, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.jumping, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.jumping, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.jumping, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.jumping, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.jumping, 1.d15() + 151);
          }
        );
      });

      knock = AddSpell(Schools.transmutation, "knock", 1, Precept: null, Glyphs.knock_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Fixed(1))
             .SetObjects();
            U.Apply.Opening();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 1)
             .SetObjects();
            P.Apply.Opening();
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 3)
             .SetObjects();
            S.Apply.Opening();
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 5)
             .SetObjects();
            E.Apply.Opening();
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 7)
             .SetObjects();
            M.Apply.Opening();
            M.Apply.CreateNook();
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 9)
             .SetObjects();
            C.Apply.Opening();
            C.Apply.CreateNook();
          }
        );
      });

      var LevitationPrecept = new Precept(Purpose.Blast, Properties.levitation);
      var FlightPrecept = new Precept(Purpose.Buff, Properties.flight);

      levitation = AddSpell(Schools.abjuration, "levitation", 4, Precept: null, Glyphs.levitation_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.Precept = LevitationPrecept;
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.levitation, 1.d140() + 10);
          },
          P =>
          {
            P.Precept = LevitationPrecept;
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.ApplyTransient(Properties.levitation, 1.d140() + 100);
          },
          S =>
          {
            S.Precept = LevitationPrecept;
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.levitation, 1.d140() + 200);
          },
          E =>
          {
            E.Precept = FlightPrecept;
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.flight, 1.d140() + 100); // this is flight now!
          },
          M =>
          {
            M.Precept = FlightPrecept;
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.flight, 1.d140() + 200);
          },
          C =>
          {
            C.Precept = FlightPrecept;
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.flight, 1.d140() + 300);
          }
        );
      });

      lightning_bolt = AddSpell(Schools.evocation, "lightning bolt", 4, new Precept(Purpose.Blast, Elements.shock), Glyphs.lightning_bolt_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.lightning, 1.d4() + 2);
            U.Apply.HarmEntity(Elements.shock, 4.d6());
          },
          P =>
          {
            P.SetCast().Beam(Beams.lightning, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.shock, 6.d6());
            P.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.blindness, 1.d4() + 1));
          },
          S =>
          {
            S.SetCast().Beam(Beams.lightning, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.shock, 8.d6());
            S.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.blindness, 2.d4() + 2));
          },
          E =>
          {
            E.SetCast().Beam(Beams.lightning, 1.d4() + 7);
            E.Apply.HarmEntity(Elements.shock, 10.d6());
            E.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.blindness, 3.d4() + 3));
          },
          M =>
          {
            M.SetCast().Beam(Beams.lightning, 1.d4() + 9);
            M.Apply.HarmEntity(Elements.shock, 12.d6());
            M.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.blindness, 4.d4() + 4));
          },
          C =>
          {
            C.SetCast().Beam(Beams.lightning, 1.d4() + 11);
            C.Apply.HarmEntity(Elements.shock, 14.d6());
            C.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.blindness, 5.d4() + 5));
          }
        );
      });

      living_wall = AddSpell(Schools.necromancy, "living wall", 6, new Precept(Purpose.SummonAlly), Glyphs.living_wall_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.One)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.One, Entities.living_wall);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.One)
             .SetTerminates();
            P.Apply.SummonEntity(Dice.One, Constructed: true, Entities.living_wall);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.One)
             .SetTerminates();
            S.Apply.SummonEntity(Dice.Two, Constructed: true, Entities.living_wall);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.One)
             .SetTerminates();
            E.Apply.SummonEntity(Dice.Three, Constructed: true, Entities.living_wall);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.One)
             .SetTerminates();
            M.Apply.SummonEntity(Dice.Four, Constructed: true, Entities.living_wall);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.One)
             .SetTerminates();
            C.Apply.SummonEntity(Dice.Five, Constructed: true, Entities.living_wall);
          }
        );
      });

      darkness = AddSpell(Schools.necromancy, "darkness", 1, new Precept(Purpose.AreaOfEffect), Glyphs.darkness_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            U.Apply.Light(false, Locality.Area);
            U.Apply.ApplyTransient(Properties.deafness, 1.d3() + 3);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            P.Apply.Light(false, Locality.Area);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            S.Apply.Light(false, Locality.Area);
            S.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.angel, Kinds.human);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            E.Apply.Light(false, Locality.Area);
            E.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.angel, Kinds.human);
            E.Apply.AreaTransient(Properties.silence, 4.d6() + 4);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            M.Apply.Light(false, Locality.Area);
            M.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.Living.ToArray());
            M.Apply.AreaTransient(Properties.silence, 4.d6() + 4);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            C.Apply.Light(false, Locality.Area);
            C.Apply.AreaTransient(Properties.fear, 5.d6(), Kinds.Living.ToArray());
            C.Apply.AreaTransient(Properties.silence, 5.d6() + 5);
          }
        );
      });

      light = AddSpell(Schools.abjuration, "light", 1, null, Glyphs.light_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            U.Apply.Light(true, Locality.Area);
            U.Apply.ApplyTransient(Properties.blindness, 1.d3() + 3);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            P.Apply.Light(true, Locality.Area);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            S.Apply.Light(true, Locality.Area);
            S.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.demon, Kinds.vampire, Kinds.orc);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            E.Apply.Light(true, Locality.Area);
            E.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.demon, Kinds.vampire, Kinds.orc);
            E.Apply.AreaTransient(Properties.blindness, 4.d6() + 4);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            M.Apply.Light(true, Locality.Area);
            M.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.Undead.ToArray().Union(new[] { Kinds.demon, Kinds.orc }).ToArray());
            M.Apply.AreaTransient(Properties.blindness, 4.d6() + 4);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
            C.Apply.Light(true, Locality.Area);
            C.Apply.AreaTransient(Properties.fear, 5.d6(), Kinds.Undead.ToArray().Union(new[] { Kinds.demon, Kinds.orc }).ToArray());
            C.Apply.AreaTransient(Properties.blindness, 5.d6() + 5);
          }
        );
      });

      magic_mapping = AddSpell(Schools.divination, "magic mapping", 5, null, Glyphs.magic_mapping_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            U.Apply.Mapping(Range.Sq15, Chance.ThreeIn4);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            P.Apply.Mapping(Range.Sq15, Chance.Always);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            S.Apply.Mapping(Range.Sq20, Chance.Always);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            E.Apply.Mapping(Range.Sq25, Chance.Always);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            M.Apply.Mapping(Range.Sq30, Chance.Always);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
            C.Apply.Mapping(Range.Sq35, Chance.Always);
          }
        );
      });

      magic_missile = AddSpell(Schools.evocation, "magic missile", 2, new Precept(Purpose.Blast, Elements.magical), Glyphs.magic_missile_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.magic_missile, 1.d4() + 2);
            U.Apply.HarmEntity(Elements.magical, 1.d4() + 2);
          },
          P =>
          {
            P.SetCast().Beam(Beams.magic_missile, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.magical, 2.d4() + 4);
          },
          S =>
          {
            S.SetCast().Beam(Beams.magic_missile, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.magical, 3.d4() + 6);
          },
          E =>
          {
            E.SetCast().Beam(Beams.magic_missile, 1.d4() + 7);
            E.Apply.HarmEntity(Elements.magical, 4.d4() + 8);
          },
          M =>
          {
            M.SetCast().Beam(Beams.magic_missile, 1.d4() + 9);
            M.Apply.HarmEntity(Elements.magical, 5.d4() + 10);
          },
          C =>
          {
            C.SetCast().Beam(Beams.magic_missile, 1.d4() + 11);
            C.Apply.HarmEntity(Elements.magical, 6.d4() + 12);
          }
        );
      });

      poison_blast = AddSpell(Schools.evocation, "poison blast", 4, new Precept(Purpose.Blast, Elements.poison), Glyphs.poison_blast_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.poison, 1.d4() + 2);
            U.Apply.HarmEntity(Elements.poison, 4.d6());
          },
          P =>
          {
            P.SetCast().Beam(Beams.poison, 1.d4() + 3);
            P.Apply.HarmEntity(Elements.poison, 6.d6());
            P.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.poison, R => R.ApplyTransient(Properties.sickness, 1.d4() + 1)));
          },
          S =>
          {
            S.SetCast().Beam(Beams.poison, 1.d4() + 5);
            S.Apply.HarmEntity(Elements.poison, 8.d6());
            S.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.poison, R => R.ApplyTransient(Properties.sickness, 2.d4() + 2)));
          },
          E =>
          {
            E.SetCast().Beam(Beams.poison, 1.d4() + 7);
            E.Apply.HarmEntity(Elements.poison, 10.d6());
            E.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.poison, R => R.ApplyTransient(Properties.sickness, 3.d4() + 3)));
          },
          M =>
          {
            M.SetCast().Beam(Beams.poison, 1.d4() + 9);
            M.Apply.HarmEntity(Elements.poison, 12.d6());
            M.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.poison, R => R.ApplyTransient(Properties.sickness, 4.d4() + 4)));
          },
          C =>
          {
            C.SetCast().Beam(Beams.poison, 1.d4() + 11);
            C.Apply.HarmEntity(Elements.poison, 14.d6());
            C.Apply.WhenChance(Chance.OneIn2, T => T.UnlessTargetResistant(Elements.poison, R => R.ApplyTransient(Properties.sickness, 5.d4() + 5)));
          }
        );
      });

      polymorph = AddSpell(Schools.transmutation, "polymorph", 6, new Precept(Purpose.Buff), Glyphs.polymorph_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero) // can only target self.
             .SetObjects(false);
            U.Apply.PolymorphEntity();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetObjects(false);
            P.Apply.PolymorphEntity();
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, 1.d4() + 3)
             .SetObjects();
            S.Apply.PolymorphEntityAndTrap();
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, 1.d4() + 5)
             .SetObjects();
            E.Apply.PolymorphEntityAndTrap();
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, 1.d4() + 7)
             .SetObjects();
            M.Apply.PolymorphItemAndEntityAndTrap();
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, 1.d4() + 9)
             .SetObjects();
            C.Apply.PolymorphItemAndEntityAndTrap();
          }
        );
      });

      deflection = AddSpell(Schools.abjuration, "deflection", 1, new Precept(Purpose.Buff, Properties.deflection), Glyphs.deflection_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.shield, Dice.Zero);
            U.Apply.ApplyTransient(Properties.deflection, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.shield, Dice.One);
            P.Apply.ApplyTransient(Properties.deflection, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.shield, Dice.One);
            S.Apply.ApplyTransient(Properties.deflection, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.shield, Dice.One);
            E.Apply.ApplyTransient(Properties.deflection, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.shield, Dice.One);
            M.Apply.ApplyTransient(Properties.deflection, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.shield, Dice.One);
            C.Apply.ApplyTransient(Properties.deflection, 1.d15() + 151);
          }
        );
      });

      telekinesis = AddSpell(Schools.abjuration, "telekinesis", 3, new Precept(Purpose.Buff, Properties.telekinesis), Glyphs.telekinesis_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.psychic, Dice.Zero);
            U.Apply.ApplyTransient(Properties.telekinesis, 1.d15() + 16);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.psychic, Dice.One);
            P.Apply.ApplyTransient(Properties.telekinesis, 1.d15() + 31);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.psychic, Dice.One);
            S.Apply.ApplyTransient(Properties.telekinesis, 1.d15() + 61);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.psychic, Dice.One);
            E.Apply.ApplyTransient(Properties.telekinesis, 1.d15() + 91);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.psychic, Dice.One);
            M.Apply.ApplyTransient(Properties.telekinesis, 1.d15() + 121);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.psychic, Dice.One);
            C.Apply.ApplyTransient(Properties.telekinesis, 1.d15() + 151);
          }
        );
      });

      raise_dead = AddSpell(Schools.necromancy, "raise dead", 7, new Precept(Purpose.SummonEnemy, new[] { Items.animal_corpse, Items.vegetable_corpse }), Glyphs.raise_dead_spell, Z =>  // TODO: this is ENEMY, because raise dead does not have any charming effects.
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            U.Apply.RaiseDeadEntity(Percent: 50, CorruptProperty: Properties.rage, CorruptDice: 6.d10(), LoyalOnly: false);
          },
          P =>
          {
            P.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            P.Apply.RaiseDeadEntity(Percent: 20, CorruptProperty: null, CorruptDice: Dice.Zero, LoyalOnly: false);
          },
          S =>
          {
            S.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            S.Apply.RaiseDeadEntity(Percent: 40, CorruptProperty: null, CorruptDice: Dice.Zero, LoyalOnly: false);
          },
          E =>
          {
            E.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            E.Apply.RaiseDeadEntity(Percent: 60, CorruptProperty: null, CorruptDice: Dice.Zero, LoyalOnly: false);
          },
          M =>
          {
            M.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            M.Apply.RaiseDeadEntity(Percent: 80, CorruptProperty: null, CorruptDice: Dice.Zero, LoyalOnly: false);
          },
          C =>
          {
            C.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
            C.Apply.RaiseDeadEntity(Percent: 100, CorruptProperty: null, CorruptDice: Dice.Zero, LoyalOnly: false);
          }
        );
      });

      remove_curse = AddSpell(Schools.clerical, "remove curse", 5, null, Glyphs.remove_curse_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Plain(Dice.Zero);
            U.Apply.RemoveCurse(Dice.One, Sanctities.Uncursed);
          },
          P =>
          {
            P.SetCast().FilterSanctity(Sanctities.Cursed);
            P.Apply.RemoveCurse(Dice.One, Sanctities.Uncursed);
          },
          S =>
          {
            S.SetCast().FilterSanctity(Sanctities.Cursed)
             .SetPunishmentOverride();
            S.Apply.UnpunishEntity();
            S.Apply.RemoveCurse(Dice.Fixed(1), Sanctities.Uncursed);
          },
          E =>
          {
            E.SetCast().FilterSanctity(Sanctities.Cursed)
             .SetPunishmentOverride();
            E.Apply.UnpunishEntity();
            E.Apply.RemoveCurse(Dice.Fixed(2), Sanctities.Uncursed);
          },
          M =>
          {
            M.SetCast().FilterSanctity(Sanctities.Cursed)
             .SetPunishmentOverride();
            M.Apply.UnpunishEntity();
            M.Apply.RemoveCurse(Dice.Fixed(3), Sanctities.Uncursed);
          },
          C =>
          {
            C.SetCast().FilterSanctity(Sanctities.Cursed)
             .SetPunishmentOverride();
            C.Apply.UnpunishEntity();
            C.Apply.RemoveCurse(Dice.Fixed(4), Sanctities.Uncursed);
          }
        );
      });

      regenerate = AddSpell(Schools.clerical, "regenerate", 4, new Precept(Purpose.Buff, Properties.life_regeneration), Glyphs.regenerate_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.ApplyTransient(Properties.life_regeneration, 1.d60());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.Zero);
            P.Apply.ApplyTransient(Properties.life_regeneration, 3.d60());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.spirit, Dice.One);
            S.Apply.ApplyTransient(Properties.life_regeneration, 6.d60());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.spirit, Dice.One);
            E.Apply.ApplyTransient(Properties.life_regeneration, 9.d60());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.spirit, Dice.One);
            M.Apply.ApplyTransient(Properties.life_regeneration, 12.d60());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.spirit, Dice.One);
            C.Apply.ApplyTransient(Properties.life_regeneration, 15.d60());
          }
        );
      });

      restoration = AddSpell(Schools.clerical, "restoration", 4, null, Glyphs.restoration_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.spirit, Dice.Zero);
            U.Apply.RestoreAbility();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.spirit, Dice.One);
            P.Apply.RestoreAbility();
          },
          null, // TODO: adept effects?
          null,
          null,
          null
        );
      });

      walling = AddSpell(Schools.transmutation, "walling", 6, null, Glyphs.walling_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.tunnel, Dice.One);
            U.Apply.CreateBarrier(WallStructure.Illusionary, Barrier: null);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.tunnel, Dice.One);
            P.Apply.ConvertBlockToBarrier();
            P.Apply.CreateBarrier(WallStructure.Illusionary, Barrier: null);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.tunnel, Dice.One);
            S.Apply.ConvertBlockToBarrier();
            S.Apply.CreateBarrier(WallStructure.Solid, Barrier: null);
          },
          null,
          null,
          null
        );
      });

      sleep = AddSpell(Schools.enchantment, "sleep", 1, new Precept(Purpose.Blast, Properties.sleeping, Elements.sleep), Glyphs.sleep_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Beam(Beams.sleep, 1.d4() + 2)
             .SetAudibility(0);
            U.Apply.ApplyTransient(Properties.sleeping, 10.d4());
          },
          P =>
          {
            P.SetCast().Beam(Beams.sleep, 1.d4() + 3)
             .SetAudibility(0);
            P.Apply.ApplyTransient(Properties.sleeping, 10.d6());
          },
          S =>
          {
            S.SetCast().Beam(Beams.sleep, 1.d4() + 5)
             .SetAudibility(0);
            S.Apply.ApplyTransient(Properties.sleeping, 10.d8());
          },
          E =>
          {
            E.SetCast().Beam(Beams.sleep, 1.d4() + 7)
             .SetAudibility(0);
            E.Apply.ApplyTransient(Properties.sleeping, 10.d10());
          },
          M =>
          {
            M.SetCast().Beam(Beams.sleep, 1.d4() + 9)
             .SetAudibility(0);
            M.Apply.ApplyTransient(Properties.sleeping, 10.d12());
          },
          C =>
          {
            C.SetCast().Beam(Beams.sleep, 1.d4() + 11)
             .SetAudibility(0);
            C.Apply.ApplyTransient(Properties.sleeping, 10.d14());
          }
        );
      });

      slow = AddSpell(Schools.enchantment, "slow", 2, new Precept(Purpose.Blast, Properties.slowness), Glyphs.slow_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, 1.d4() + 2);
            U.Apply.ApplyTransient(Properties.slowness, 5.d6() + 5);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 3);
            P.Apply.ApplyTransient(Properties.slowness, 10.d6() + 10);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 5);
            S.Apply.ApplyTransient(Properties.slowness, 20.d6() + 20);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 7);
            E.Apply.ApplyTransient(Properties.slowness, 30.d6() + 30);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 9);
            M.Apply.ApplyTransient(Properties.slowness, 40.d6() + 40);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 11);
            C.Apply.ApplyTransient(Properties.slowness, 50.d6() + 50);
          }
        );
      });

      summoning = AddSpell(Schools.conjuration, "summoning", 2, new Precept(Purpose.SummonEnemy), Glyphs.summoning_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            U.Apply.CreateEntity(Dice.One);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            P.Apply.CreateEntity(Dice.One);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            S.Apply.CreateEntity(Dice.Fixed(2));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            E.Apply.CreateEntity(Dice.Fixed(3));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            M.Apply.CreateEntity(Dice.Fixed(4));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
            C.Apply.CreateEntity(Dice.Fixed(5));
          }
        );
      });

      teleport_away = AddSpell(Schools.abjuration, "teleport away", 6, new Precept(Purpose.Teleport), Glyphs.teleport_away_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Zero);
            U.Apply.TeleportEntity(Properties.teleportation);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, Dice.One);
            P.Apply.TeleportEntity(Properties.teleportation);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 3);
            S.Apply.TeleportEntity(Properties.teleportation);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 5)
             .SetObjects();
            E.Apply.TeleportFloorItem();
            E.Apply.TeleportEntity(Properties.teleportation);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 7)
             .SetObjects();
            M.Apply.TeleportFloorItem();
            M.Apply.TeleportEntity(Properties.teleportation);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 9)
             .SetObjects();
            C.Apply.TeleportFloorItem();
            C.Apply.TeleportEntity(Properties.teleportation);
          }
        );
      });

      toxic_spray = AddSpell(Schools.transmutation, "toxic spray", 3, new Precept(Purpose.Blast), Glyphs.toxic_spray_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.acid, 2.d3() + 1)
             .SetPenetrates(false);
            U.Apply.HarmEntity(Elements.acid, 1.d6() + 1);
          },
          P =>
          {
            P.SetCast().Strike(Strikes.acid, 2.d3() + 2)
             .SetPenetrates(false);
            P.Apply.HarmEntity(Elements.acid, 2.d6() + 2);
            P.Apply.CreateDevice(Devices.noxious_pool, Destruction: false);
          },
          S =>
          {
            S.SetCast().Strike(Strikes.acid, 2.d3() + 3)
             .SetPenetrates();
            S.Apply.HarmEntity(Elements.acid, 3.d6() + 3);
            S.Apply.ApplyTransient(Properties.hallucination, 2.d6());
            S.Apply.CreateDevice(Devices.noxious_pool, Destruction: false);
          },
          E =>
          {
            E.SetCast().Strike(Strikes.acid, 2.d3() + 4)
             .SetPenetrates();
            E.Apply.HarmEntity(Elements.acid, 4.d6() + 4);
            E.Apply.ApplyTransient(Properties.hallucination, 3.d6());
            E.Apply.ApplyTransient(Properties.confusion, 3.d6());
            E.Apply.CreateDevice(Devices.acid_trap, Destruction: false);
          },
          M =>
          {
            M.SetCast().Strike(Strikes.acid, 2.d3() + 6)
             .SetPenetrates();
            M.Apply.HarmEntity(Elements.acid, 5.d6() + 5);
            M.Apply.ApplyTransient(Properties.hallucination, 4.d6());
            M.Apply.ApplyTransient(Properties.stunned, 4.d6());
            M.Apply.CreateDevice(Devices.toxic_trap, Destruction: false);
          },
          C =>
          {
            C.SetCast().Strike(Strikes.acid, 2.d3() + 8)
             .SetPenetrates();
            C.Apply.HarmEntity(Elements.acid, 6.d6() + 6);
            C.Apply.ApplyTransient(Properties.hallucination, 5.d6());
            C.Apply.ApplyTransient(Properties.stunned, 5.d6());
            C.Apply.CreateDevice(Devices.toxic_trap, Destruction: false);
          }
        );
      });

      turn_undead = AddSpell(Schools.clerical, "turn undead", 3, null, Glyphs.turn_undead_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.holy, Dice.One);
            U.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.physical, 2.d6() + 2));
            U.Apply.AreaTransient(Properties.fear, 2.d6(), Kinds.Undead.ToArray());
          },
          P =>
          {
            P.SetCast().Strike(Strikes.holy, Dice.One);
            P.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.physical, 4.d6() + 4));
            P.Apply.AreaTransient(Properties.fear, 3.d6(), Kinds.Undead.ToArray());
          },
          S =>
          {
            S.SetCast().Strike(Strikes.holy, Dice.Fixed(2));
            S.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.physical, 6.d6() + 6));
            S.Apply.AreaTransient(Properties.fear, 4.d6(), Kinds.Undead.ToArray());
          },
          E =>
          {
            E.SetCast().Strike(Strikes.holy, Dice.Fixed(3));
            E.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.physical, 8.d6() + 8));
            E.Apply.AreaTransient(Properties.fear, 5.d6(), Kinds.Undead.ToArray());
          },
          M =>
          {
            M.SetCast().Strike(Strikes.holy, Dice.Fixed(4));
            M.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.physical, 10.d6() + 10));
            M.Apply.AreaTransient(Properties.fear, 6.d6(), Kinds.Undead.ToArray());
          },
          C =>
          {
            C.SetCast().Strike(Strikes.holy, Dice.Fixed(5));
            C.Apply.WhenTargetKind(Kinds.Undead, T => T.HarmEntity(Elements.physical, 12.d6() + 12));
            C.Apply.AreaTransient(Properties.fear, 7.d6(), Kinds.Undead.ToArray());
          }
        );
      });

      wizard_lock = AddSpell(Schools.transmutation, "wizard lock", 2, null, Glyphs.wizard_lock_spell, Z =>
      {
        Z.Description = null;
        SetAdept
        (
          Z,
          U =>
          {
            U.SetCast().Strike(Strikes.force, Dice.Fixed(1))
             .SetObjects();
            U.Apply.Locking();
          },
          P =>
          {
            P.SetCast().Strike(Strikes.force, 1.d4() + 1)
             .SetObjects();
            P.Apply.Locking();
          },
          S =>
          {
            S.SetCast().Strike(Strikes.force, 1.d4() + 3)
             .SetObjects()
             .SetPenetrates();
            S.Apply.Locking();
            S.Apply.WhenTargetKind(new[] { Kinds.golem }, T => T.ApplyTransient(Properties.paralysis, Dice.One));
          },
          E =>
          {
            E.SetCast().Strike(Strikes.force, 1.d4() + 5)
             .SetObjects()
             .SetPenetrates();
            E.Apply.Locking();
            E.Apply.WhenTargetKind(new[] { Kinds.golem }, T => T.ApplyTransient(Properties.paralysis, 1.d2()));
          },
          M =>
          {
            M.SetCast().Strike(Strikes.force, 1.d4() + 7)
             .SetObjects()
             .SetPenetrates();
            M.Apply.Locking();
            M.Apply.WhenTargetKind(new[] { Kinds.golem }, T => T.ApplyTransient(Properties.paralysis, 1.d4()));
          },
          C =>
          {
            C.SetCast().Strike(Strikes.force, 1.d4() + 9)
             .SetObjects()
             .SetPenetrates();
            C.Apply.Locking();
            C.Apply.WhenTargetKind(new[] { Kinds.golem }, T => T.ApplyTransient(Properties.paralysis, 2.d4()));
          }
        );
      });

      Register.Alias(restoration, "restore ability");
      Register.Alias(deflection, "protection");
    }
#endif

    // abjuration = 8.
    public readonly Spell blinking;
    public readonly Spell haste;
    public readonly Spell invisibility;
    public readonly Spell jumping;
    public readonly Spell levitation;
    public readonly Spell light;
    public readonly Spell teleport_away;
    public readonly Spell deflection;
    public readonly Spell telekinesis;

    // conjuration = 7
    public readonly Spell create_familiar;
    public readonly Spell summoning;
    public readonly Spell flaming_sphere;
    public readonly Spell freezing_sphere;
    public readonly Spell shocking_sphere;
    public readonly Spell crushing_sphere;
    public readonly Spell soaking_sphere;

    // clerical = 8.
    public readonly Spell curing; // 2
    public readonly Spell healing; // 1
    public readonly Spell extra_healing; // 3
    public readonly Spell regenerate; // 4
    public readonly Spell restoration; // 4
    public readonly Spell full_healing; // 5
    public readonly Spell remove_curse;
    public readonly Spell turn_undead;
    // public readonly Spell mass_heal; // area of effect healing for allies (everyone nearby when unskilled).
    // public readonly Spell aid; // temporary above life maximum boost?
    // public readonly Spell sustenance; // nutrition up to zero only.

    // divination = 7.
    public readonly Spell clairvoyance;
    public readonly Spell detect_food;
    public readonly Spell detect_monsters;
    public readonly Spell detect_treasure;
    public readonly Spell detect_unseen;
    public readonly Spell identify;
    public readonly Spell magic_mapping;

    // evocation = 8.
    public readonly Spell acid_stream;
    public readonly Spell cone_of_cold;
    public readonly Spell fireball;
    public readonly Spell ice_storm;
    public readonly Spell force_bolt;
    public readonly Spell lightning_bolt;
    public readonly Spell magic_missile;
    public readonly Spell poison_blast;

    // enchantment = 7.
    public readonly Spell animate_object;
    public readonly Spell charm;
    public readonly Spell confusion;
    public readonly Spell fear;
    public readonly Spell phasing;
    public readonly Spell sleep;
    public readonly Spell slow;

    // necromancy = 7.
    public readonly Spell animate_dead;
    public readonly Spell bind_undead;
    public readonly Spell darkness;
    public readonly Spell drain_life;
    public readonly Spell finger_of_death;
    public readonly Spell living_wall;
    public readonly Spell raise_dead;

    // transmutation = 7.
    public readonly Spell cancellation;
    public readonly Spell dig;
    public readonly Spell disintegrate;
    public readonly Spell knock;
    public readonly Spell polymorph;
    public readonly Spell toxic_spray;
    public readonly Spell walling;
    public readonly Spell wizard_lock;
  }
}