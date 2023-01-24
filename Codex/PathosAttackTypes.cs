using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexAttackTypes : CodexPage<ManifestAttackTypes, AttackTypeEditor, AttackType>
  {
    private CodexAttackTypes() { }
#if MASTER_CODEX
    internal CodexAttackTypes(Codex Codex)
      : base(Codex.Manifest.AttackTypes)
    {
      AttackType Add(string Name, DamageType? DamageType, Action<AttackTypeEditor> EditorAction)
      {
        return Register.Add(A =>
        {
          A.Name = Name;
          A.DamageType = DamageType;

          EditorAction(A);
        });
      }

      bite = Add("bite", DamageType.Pierce, E =>
      {
      });

      blast = Add("blast", DamageType: null, E =>
      {
        E.IsIndirect = true;
        E.IsCasting = true;
        E.IsExploding = true;
      });

      breath = Add("breath", DamageType: null, E =>
      {
        E.IsIndirect = true;
        E.IsCasting = true;
      });

      butt = Add("butt", DamageType.Bludgeon, E =>
      {
      });

      claw = Add("claw", DamageType.Slash, E =>
      {
        E.IsOpenWielding = true;
      });

      engulf = Add("engulf", DamageType: null, E =>
      {
      });

      gaze = Add("gaze", DamageType: null, E =>
      {
        E.IsVision = true;
        E.IsStrengthIgnored = true;
        E.IsIndirect = true;
        E.IsCasting = true;
      });

      grapple = Add("grapple", DamageType: null, E =>
      {
      });

      horn = Add("horn", DamageType.Pierce, E =>
      {
      });

      kick = Add("kick", DamageType.Bludgeon, E =>
      {
      });

      punch = Add("punch", DamageType.Bludgeon, E =>
      {
        E.IsOpenWielding = true;
      });

      slap = Add("slap", DamageType.Bludgeon, E =>
      {
        E.IsOpenWielding = true;
      });

      shriek = Add("shriek", DamageType: null, E =>
      {
        E.IsVoice = true;
        E.IsIndirect = true;
        E.IsStrengthIgnored = true;
        E.IsCasting = true;
      });

      spell = Add("spell", DamageType: null, E =>
      {
      });

      spit = Add("spit", DamageType: null, E =>
      {
        E.IsIndirect = true;
        E.IsCasting = true;
      }); 

      splash = Add("splash", DamageType: null, E =>
      {
        E.IsIndirect = true;
        E.IsStrengthIgnored = true;
      });

      spore = Add("spore", DamageType: null, E =>
      {
        E.IsIndirect = true;
        E.IsStrengthIgnored = true;
      });

      sting = Add("sting", DamageType.Pierce, E =>
      {
      });

      summon = Add("summon", DamageType: null, E =>
      {
        E.IsSummoning = true;
      });

      tentacle = Add("tentacle", DamageType: null, E =>
      {
      });

      touch = Add("touch", DamageType: null, E =>
      {
        E.IsStrengthIgnored = true;
        E.IsOpenWielding = true;
      });

      weapon = Add("weapon", DamageType.Slash, E =>
      {
        E.IsOpenWielding = true;
      });
    }
#endif

    public readonly AttackType bite;
    public readonly AttackType blast;
    public readonly AttackType breath;
    public readonly AttackType butt;
    public readonly AttackType claw;
    public readonly AttackType engulf;
    public readonly AttackType gaze;
    public readonly AttackType grapple;
    public readonly AttackType horn;
    public readonly AttackType kick;
    public readonly AttackType punch;
    public readonly AttackType shriek;
    public readonly AttackType slap;
    public readonly AttackType spell;
    public readonly AttackType spit;
    public readonly AttackType splash;
    public readonly AttackType spore;
    public readonly AttackType sting;
    public readonly AttackType summon;
    public readonly AttackType tentacle;
    public readonly AttackType touch;
    public readonly AttackType weapon;
  }
}
