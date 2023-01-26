using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexSkills : CodexPage<ManifestSkills, SkillEditor, Skill>
  {
    private CodexSkills() { }
#if MASTER_CODEX
    internal CodexSkills(Codex Codex)
      : base(Codex.Manifest.Skills)
    {
      Skill AddSkill(SkillCategory Category, string Name, Action<SkillEditor> EditorAction)
      {
        return Register.Add(S =>
        {
          S.Category = Category;
          S.Name = Name;
          S.GainSonic = Codex.Sonics.gain_skill;
          S.LoseSonic = Codex.Sonics.lose_skill;
          S.Glyph = Codex.Glyphs.GetGlyphOrNull("skill " + S.Name);
          Debug.Assert(S.Glyph != null, $"Skill not found: {S.Name}");

          EditorAction(S);
        });
      }

      bartering = AddSkill(SkillCategory.Utility, "bartering", S =>
      {
        S.Description = null;
      });

      crafting = AddSkill(SkillCategory.Utility, "crafting", S =>
      {
        S.Description = null;
      });

      dual_wielding = AddSkill(SkillCategory.Utility, "dual wielding", S =>
      {
        S.Description = null;
      });

      literacy = AddSkill(SkillCategory.Utility, "literacy", S =>
      {
        S.Description = null;
        S.IlliteracyBlocked = true;
      });

      locks = AddSkill(SkillCategory.Utility, "locks", S =>
      {
        S.Description = "The art of unlocking by manipulating the components of a locked device with the aid of lock picks.";
      });

      music = AddSkill(SkillCategory.Utility, "music", S =>
      {
        S.Description = null;
      });

      riding = AddSkill(SkillCategory.Utility, "riding", S =>
      {
        S.Description = null;
      });

      swimming = AddSkill(SkillCategory.Utility, "swimming", S =>
      {
        S.Description = null;
      });

      traps = AddSkill(SkillCategory.Utility, "traps", S =>
      {
        S.Description = null;
      });

      // defensive.
      heavy_armour = AddSkill(SkillCategory.Defensive, "heavy armour", S =>
      {
        S.Description = null;
        S.ArmourCapDexterityModifier = 2;
        S.ArmourSkilledSpeedPenalty = Rules.HeavyArmourSkilledSpeedPenalty;
        S.ArmourUnskilledSpeedPenalty = Rules.HeavyArmourUnskilledSpeedPenalty;
      });

      medium_armour = AddSkill(SkillCategory.Defensive, "medium armour", S =>
      {
        S.Description = null;
        S.ArmourCapDexterityModifier = 4;
        S.ArmourSkilledSpeedPenalty = Rules.MediumArmourSkilledSpeedPenalty;
        S.ArmourUnskilledSpeedPenalty = Rules.MediumArmourUnskilledSpeedPenalty;
      });

      light_armour = AddSkill(SkillCategory.Defensive, "light armour", S =>
      {
        S.Description = null;
        S.ArmourCapDexterityModifier = int.MaxValue;
        S.ArmourSkilledSpeedPenalty = 0.0F;
        S.ArmourUnskilledSpeedPenalty = 0.0F;
      });

      // offensive.
      axe = AddSkill(SkillCategory.Offensive, "axe", S =>
      {
        S.Description = null;
        S.WeaponRotation = true;
      });

      disc = AddSkill(SkillCategory.Offensive, "disc", S =>
      {
        S.Description = null;
        S.WeaponRotation = true;
      });

      bow = AddSkill(SkillCategory.Offensive, "bow", S => { });

      club = AddSkill(SkillCategory.Offensive, "club", S => { });

      crossbow = AddSkill(SkillCategory.Offensive, "crossbow", S => { });

      dart = AddSkill(SkillCategory.Offensive, "dart", S => { });

      firearms = AddSkill(SkillCategory.Offensive, "firearms", S =>
      {
        S.Description = null;
        S.AbolitionCandidate = true;
      });

      flail = AddSkill(SkillCategory.Offensive, "flail", S => { });

      hammer = AddSkill(SkillCategory.Offensive, "hammer", S => { });

      heavy_blade = AddSkill(SkillCategory.Offensive, "heavy blade", S => { });

      lance = AddSkill(SkillCategory.Offensive, "lance", S => { });

      light_blade = AddSkill(SkillCategory.Offensive, "light blade", S => { });

      mace = AddSkill(SkillCategory.Offensive, "mace", S => { });

      medium_blade = AddSkill(SkillCategory.Offensive, "medium blade", S => { });

      pick = AddSkill(SkillCategory.Offensive, "pick", S => { });

      polearm = AddSkill(SkillCategory.Offensive, "polearm", S => { });

      sling = AddSkill(SkillCategory.Offensive, "sling", S => { });

      spear = AddSkill(SkillCategory.Offensive, "spear", S => { });

      staff = AddSkill(SkillCategory.Offensive, "staff", S => { });

      unarmed_combat = AddSkill(SkillCategory.Offensive, "unarmed combat", S => { });

      whip = AddSkill(SkillCategory.Offensive, "whip", S => { });

      // mysical.
      abjuration = AddSkill(SkillCategory.Mystical, "abjuration", S => { });

      clerical = AddSkill(SkillCategory.Mystical, "clerical", S => { });

      conjuration = AddSkill(SkillCategory.Mystical, "conjuration", S => { });

      divination = AddSkill(SkillCategory.Mystical, "divination", S => { });

      enchantment = AddSkill(SkillCategory.Mystical, "enchantment", S => { });

      evocation = AddSkill(SkillCategory.Mystical, "evocation", S => { });

      necromancy = AddSkill(SkillCategory.Mystical, "necromancy", S => { });

      transmutation = AddSkill(SkillCategory.Mystical, "transmutation", S => { });

      Register.Alias(clerical, "healing");
      Register.Alias(disc, "boomerang");

      // TODO: skills requiring further abstraction in the engine.
      Register.dual_wielding = dual_wielding;
    }
#endif

    public readonly Skill bartering;
    public readonly Skill crafting;
    public readonly Skill dual_wielding;
    public readonly Skill literacy;
    public readonly Skill locks;
    public readonly Skill music;
    public readonly Skill riding;
    public readonly Skill swimming;
    //public readonly Skill theft;
    public readonly Skill traps;

    public readonly Skill light_armour;
    public readonly Skill medium_armour;
    public readonly Skill heavy_armour;

    public readonly Skill light_blade;
    public readonly Skill medium_blade;
    public readonly Skill heavy_blade;

    public readonly Skill axe;
    public readonly Skill bow;
    public readonly Skill club;
    public readonly Skill crossbow;
    public readonly Skill dart;
    public readonly Skill disc;
    public readonly Skill firearms;
    public readonly Skill flail;
    public readonly Skill hammer;
    public readonly Skill lance;
    public readonly Skill mace;
    public readonly Skill pick;
    public readonly Skill polearm;
    public readonly Skill sling;
    public readonly Skill spear;
    public readonly Skill staff;
    public readonly Skill unarmed_combat;
    public readonly Skill whip;

    public readonly Skill abjuration;
    public readonly Skill clerical;
    public readonly Skill conjuration;
    public readonly Skill divination;
    public readonly Skill enchantment;
    public readonly Skill evocation;
    //public readonly Skill illusion;
    public readonly Skill necromancy;
    public readonly Skill transmutation;
  }
}