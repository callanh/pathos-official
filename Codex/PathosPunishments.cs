using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexPunishments : CodexPage<ManifestPunishments, PunishmentEditor, Punishment>
  {
    private CodexPunishments() { }
#if MASTER_CODEX
    internal CodexPunishments(Codex Codex)
      : base(Codex.Manifest.Punishments)
    {
      var Items = Codex.Items;
      var Properties = Codex.Properties;
      var Glyphs = Codex.Glyphs;
      var Sonics = Codex.Sonics;
      var Sanctities = Codex.Sanctities;

      Punishment AddPunishment(string Name, string Description, Glyph Glyph, Sonic Sonic, Action<PunishmentEditor> Action)
      {
        return Register.Add(P =>
        {
          P.Name = Name;
          P.Description = Description;
          P.Glyph = Glyph;
          P.Sonic = Sonic;

          Action(P);
        });
      }

      ball__chain = AddPunishment("ball & chain", "A heavy iron ball is attached to you by a chain.", Glyphs.ball_and_chain_punishment, Sonics.shackle, P =>
      {
        P.SetImpediment(SpeedMultiplier: 0.8F, AttackModifier: Modifier.Minus2, DefenceModifier: Modifier.Minus2);
        P.ReleaseLoot.AddKit(Chance.Always, Dice.Fixed(1), Items.heavy_iron_ball);
        P.ReleaseLoot.AddKit(Chance.Always, Dice.Fixed(1), Items.iron_chain);
      });

      gluttony = AddPunishment("gluttony", "You are overwhelmed by the compulsion to eat and consume anything that you find.", Glyphs.gluttony_punishment, Sonics.shackle, P =>
      {
        P.SetCompulsions(new[] { Codex.Motions.eat });
      });

      illiteracy = AddPunishment("illiteracy", "This malaise has rendered you completely unable to read or write.", Glyphs.illiteracy_punishment, Sonics.shackle, P =>
      {
        P.Illiteracy = true;
      });

      ignoramus = AddPunishment("ignoramus", "All your knowledge has disappeared and retaining new information has become impossible.", Glyphs.ignoramus_punishment, Sonics.shackle, P =>
      {
        P.Ignorance = true;
      });

      malignant_aura = AddPunishment("malignant aura", "Malevolence and ghostly blackness seeps from your fingertips, corrupting all that you touch.", Glyphs.malignant_aura_punishment, Sonics.shackle, P =>
      {
        P.ConversionSanctity = Sanctities.Cursed;
      });

      midas_touch = AddPunishment("midas touch", "Everything you touch will quite literally turn into gold.", Glyphs.midas_touch_punishment, Sonics.shackle, P =>
      {
        P.ConversionMaterial = Codex.Materials.gold;
      });
      
      psychosis = AddPunishment("psychosis", "This derangement makes you prone to fits of rage, hallucination, fear, agitation and the occasional act of cannibalism.", Glyphs.psychosis_punishment, Sonics.shackle, P =>
      {
        P.SetBouts(new[] { Properties.rage, Properties.fear, Properties.hallucination, Properties.cannibalism, Properties.aggravation });
      });
      
      shunning = AddPunishment("shunning", "This formal rejection means your actions will not be rewarded and your prayers will be unanswered.", Glyphs.shunning_punishment, Sonics.shackle, P =>
      {
        P.Shunned = true;
      });
      
      thirst = AddPunishment("thirst", "You are overwhelmed by the compulsion to quaff everything that you can find.", Glyphs.thirst_punishment, Sonics.shackle, P =>
      {
        P.SetCompulsions(new[] { Codex.Motions.quaff, Codex.Motions.drink });
      });

      wanted = AddPunishment("wanted", "You are marked as a criminal and wanted by authorities.", Glyphs.wanted_punishment, Sonics.shackle, P =>
      {
        P.Wanted = true;
        P.PursuitHorde = Codex.Hordes.keystone;
      });

      // TODO: insomnia - can't rest or sleep.
      //       contrary - blessed in cursed and cursed is blessed.
    }
#endif

    public readonly Punishment ball__chain;
    public readonly Punishment gluttony;
    public readonly Punishment ignoramus;
    public readonly Punishment illiteracy;
    public readonly Punishment malignant_aura;
    public readonly Punishment midas_touch;
    public readonly Punishment psychosis;
    public readonly Punishment shunning;
    public readonly Punishment thirst;
    public readonly Punishment wanted;
  }
}
