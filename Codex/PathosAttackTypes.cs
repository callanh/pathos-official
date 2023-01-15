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
      AttackType Add(string Name, DamageType? DamageType, Action<AttackTypeEditor> EditorAction = null)
      {
        return Register.Add(A =>
        {
          A.Name = Name;
          A.DamageType = DamageType;

          EditorAction?.Invoke(A);
        });
      }

      bite = Add("bite", DamageType.Pierce);

      blast = Add("blast", DamageType: null, E => E.Indirect().Casting().Exploding());

      breath = Add("breath", DamageType: null, E => E.Indirect().Casting());

      butt = Add("butt", DamageType.Bludgeon);

      claw = Add("claw", DamageType.Slash, E => E.OpenWielding());

      engulf = Add("engulf", DamageType: null);

      gaze = Add("gaze", DamageType: null, E => E.Vision().Indirect().StrengthIgnored().Casting());

      grapple = Add("grapple", DamageType: null);

      horn = Add("horn", DamageType.Pierce);

      kick = Add("kick", DamageType.Bludgeon);

      punch = Add("punch", DamageType.Bludgeon, E => E.OpenWielding());

      shriek = Add("shriek", DamageType: null, E => E.Voice().Indirect().StrengthIgnored().Casting());

      spell = Add("spell", DamageType: null);

      spit = Add("spit", DamageType: null, E => E.Indirect().Casting());

      splash = Add("splash", DamageType: null, E => E.Indirect().StrengthIgnored());

      spore = Add("spore", DamageType: null, E => E.Indirect().StrengthIgnored());

      sting = Add("sting", DamageType.Pierce);

      summon = Add("summon", DamageType: null, E => E.Summoning());

      tentacle = Add("tentacle", DamageType: null);

      touch = Add("touch", DamageType: null, E => E.StrengthIgnored().OpenWielding());

      weapon = Add("weapon", DamageType.Slash, E => E.OpenWielding());
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
