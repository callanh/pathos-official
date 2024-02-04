using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexAfflictions : CodexPage<ManifestAfflictions, AfflictionEditor, Affliction>
  {
    private CodexAfflictions() { }
#if MASTER_CODEX
    internal CodexAfflictions(Codex Codex)
      : base(Codex.Manifest.Afflictions)
    {
      var Properties = Codex.Properties;
      var Elements = Codex.Elements;
      var Glyphs = Codex.Glyphs;
      var Sonics = Codex.Sonics;
      var Attributes = Codex.Attributes;

      Affliction AddAffliction(string Name, string Description, Glyph Glyph, Sonic Sonic, Action<AfflictionEditor> Action)
      {
        return Register.Add(A =>
        {
          A.Name = Name;
          A.Description = Description;
          A.Glyph = Glyph;
          A.Sonic = Sonic;

          Action(A);
        });
      }

      crabs = AddAffliction("crabs", "Something deeply unsettling is happening... down there.", Glyphs.crabs_affliction, Sonics.polymorph, A =>
      {
        A.Severe = true;
        A.Taint = Inv.Colour.DarkSalmon.Opacity(0.50F);
        A.SetSymptom(Chance.OneIn100, P => P.CreateEntity(Dice.One, Codex.Entities.giant_crab));
        //A.RemoveLoot.AddKit(Dice.One, Codex.Items.egg); // TODO: drop crab egg.
        //A.RequiresAnatomy(Codex.Anatomies.crotch);
      });
      /*
      insomnia = AddAffliction("insomnia", ".", Glyphs.insomnia_affliction, Sonics.polymorph, A =>
      {
        A.Severe = false;
        A.Taint = Inv.Colour.HotPink.Opacity(0.50F);
        // TODO: can't rest.
      });
      */
      /*
      mutation = AddAffliction("mutation", "Groteseque changes are happening to your body.", Glyphs.mutation_affliction, Sonics.polymorph, A =>
      {
        A.Taint = Inv.Colour.LightPink.Opacity(0.50F);

        A.SetSymptom(Chance.OneIn100, P =>
        {
          P.WhenChance(Chance.OneIn2, T => T.DecreaseAbility(Attributes.Charisma, Dice.One));
          P.WhenProbability(R =>
          {
            R.Add(10, S => S.IncreaseAbility(Attributes.Strength, Dice.One));
            R.Add(10, S => S.IncreaseAbility(Attributes.Dexterity, Dice.One));
            R.Add(10, S => S.IncreaseAbility(Attributes.Constitution, Dice.One));
            R.Add(10, S => S.IncreaseAbility(Attributes.Intelligence, Dice.One));
            R.Add(10, S => S.IncreaseAbility(Attributes.Wisdom, Dice.One));
            R.Add(10, S => S.DecreaseAbility(Attributes.Strength, Dice.One));
            R.Add(10, S => S.DecreaseAbility(Attributes.Dexterity, Dice.One));
            R.Add(10, S => S.DecreaseAbility(Attributes.Constitution, Dice.One));
            R.Add(10, S => S.DecreaseAbility(Attributes.Intelligence, Dice.One));
            R.Add(10, S => S.DecreaseAbility(Attributes.Wisdom, Dice.One));
          });
        });
      });
      */
      /*
      myopia = AddAffliction("myopia", ".", Glyphs.myopia_affliction, Sonics.polymorph, A =>
      {
        A.Severe = false;
        A.Taint = Inv.Colour.DodgerBlue.Opacity(0.50F);
        // TODO: cause nyctalopia - night blindness: can't see into unlit squares, effectively blind in dark areas; restrict vision range from 5 to 3 squares, zones turned off?
      });
      */
      nits = AddAffliction("nits", "You feel unclean and everything itches.", Glyphs.nits_affliction, Sonics.polymorph, A =>
      {
        A.Taint = Inv.Colour.Brown.Opacity(0.50F);
        A.SetSymptom(Chance.OneIn100, P => P.CreateEntity(1.d3(), Codex.Entities.giant_louse));
        //A.RemoveLoot.AddKit(Dice.One, Codex.Items.egg); // TODO: drop louse egg.
        A.RequiresAnatomy(Codex.Anatomies.blood);
      });

      poisoning = AddAffliction("poisoning", "Deadly venom courses through your veins.", Glyphs.poisoning_affliction, Sonics.polymorph, A =>
      {
        A.Severe = true;
        A.Taint = Inv.Colour.Red.Opacity(0.50F);
        A.SetResistance(Elements.poison);
        A.SetSymptom(Chance.OneIn2, P => P.Harm(Elements.poison, 1.d3()));
        A.RequiresAnatomy(Codex.Anatomies.blood);
      });

      rabies = AddAffliction("rabies", "This viral disease causes inflammation of the brain and is spread by infected animals.", Glyphs.rabies_affliction, Sonics.polymorph, A =>
      {
        A.Taint = Inv.Colour.HotPink.Opacity(0.50F);
        A.SetImmunity(Properties.vitality);
        A.SetSymptom(Chance.OneIn100, P =>
        {
          P.WhenProbability(R =>
          {
            R.Add(10, S => S.ApplyTransient(Properties.sickness, 4.d4()));
            R.Add(10, S => S.ApplyTransient(Properties.confusion, 2.d4()));
            R.Add(10, S => S.ApplyTransient(Properties.fear, 2.d4()));
            R.Add(10, S => S.ApplyTransient(Properties.paralysis, 2.d4()));
            R.Add(10, S => S.ApplyTransient(Properties.blindness, 2.d4()));
            R.Add(10, S => S.ApplyTransient(Properties.aggravation, 2.d8()));
            R.Add(10, S => S.ApplyTransient(Properties.rage, 2.d4()));
            R.Add(10, S => S.ApplyTransient(Properties.fainting, 1.d4()));
            R.Add(10, S => S.ApplyTransient(Properties.silence, 2.d8()));
            R.Add(9, S => S.ApplyTransient(Properties.hunger, 2.d20()));
            R.Add(1, S => S.DecreaseAbility(Attributes.constitution, Dice.One));
          });
        });
        A.RequiresAnatomy(Codex.Anatomies.blood);
      });

      reflux = AddAffliction("reflux", "You have a dreadful case of stomach acid.", Glyphs.reflux_affliction, Sonics.polymorph, A =>
      {
        A.Taint = Inv.Colour.LightPink.Opacity(0.50F);
        A.SetResistance(Elements.acid);
        A.SetSymptom(Chance.OneIn100, P => P.CreateEntity(1.d3(), Codex.Entities.acid_blob));
        //A.RequiresAnatomy(Codex.Anatomies.stomach);
      });

      sliming = AddAffliction("sliming", "Your skin is sickly green and something is very wrong inside your stomach.", Glyphs.sliming_affliction, Sonics.polymorph, A =>
      {
        A.Severe = true;
        A.Taint = Inv.Colour.LightGreen.Opacity(0.50F);
        A.SetImmunity(Properties.vitality);
        A.SetSymptom(Chance.OneIn100, P => P.CreateEntity(Dice.One, Codex.Entities.green_slime));
        //A.RequiresAnatomy(Codex.Anatomies.stomach);
      });

      worms = AddAffliction("worms", "There is a gnawing sensation from deep inside your stomach.", Glyphs.worms_affliction, Sonics.polymorph, A =>
      {
        A.Severe = true;
        A.Taint = Inv.Colour.Tan.Opacity(0.50F);
        A.SetSymptom(Chance.OneIn100, P => P.ApplyTransient(Properties.hunger, 5.d10()));
        A.SetSymptom(Chance.OneIn100, P => P.CreateEntity(1.d3(), Codex.Entities.baby_long_worm));
        //A.RequiresAnatomy(Codex.Anatomies.stomach);
      });
    }
#endif

    public readonly Affliction crabs;
    //public readonly Affliction insomnia;
    //public readonly Affliction mutation;
    //public readonly Affliction myopia;
    public readonly Affliction nits;
    public readonly Affliction poisoning;
    public readonly Affliction rabies;
    public readonly Affliction reflux;
    public readonly Affliction sliming;
    public readonly Affliction worms;
  }
}