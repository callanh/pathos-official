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
      foreach (var Skill in List)
      {
        var Editor = Register.Edit(Skill);

        Editor.Glyph = Codex.Glyphs.GetGlyphOrNull("skill " + Skill.Name);
        Debug.Assert(Editor.Glyph != null, $"Skill not found: {Skill.Name}");

        Editor.GainSonic = Codex.Sonics.gain_skill;
        Editor.LoseSonic = Codex.Sonics.lose_skill;
      }

      Register.Alias(clerical, "healing");
      Register.Alias(disc, "boomerang");
    }
#endif

    public Skill bartering => Register.bartering;
    public Skill crafting => Register.crafting;
    public Skill dual_wielding => Register.dual_wielding;
    public Skill literacy => Register.literacy;
    public Skill locks => Register.locks;
    public Skill music => Register.music;
    public Skill riding => Register.riding;
    public Skill swimming => Register.swimming;
    public Skill traps => Register.traps;
    public Skill light_armour => Register.light_armour;
    public Skill medium_armour => Register.medium_armour;
    public Skill heavy_armour => Register.heavy_armour;
    public Skill light_blade => Register.light_blade;
    public Skill medium_blade => Register.medium_blade;
    public Skill heavy_blade => Register.heavy_blade;
    public Skill axe => Register.axe;
    public Skill bow => Register.bow;
    public Skill club => Register.club;
    public Skill crossbow => Register.crossbow;
    public Skill dart => Register.dart;
    public Skill disc => Register.disc;
    public Skill firearms => Register.firearms;
    public Skill flail => Register.flail;
    public Skill hammer => Register.hammer;
    public Skill lance => Register.lance;
    public Skill mace => Register.mace;
    public Skill pick => Register.pick;
    public Skill polearm => Register.polearm;
    public Skill sling => Register.sling;
    public Skill spear => Register.spear;
    public Skill staff => Register.staff;
    public Skill unarmed_combat => Register.unarmed_combat;
    public Skill whip => Register.whip;
    public Skill abjuration => Register.abjuration;
    public Skill clerical => Register.clerical;
    public Skill conjuration => Register.conjuration;
    public Skill divination => Register.divination;
    public Skill enchantment => Register.enchantment;
    public Skill evocation => Register.evocation;
    public Skill necromancy => Register.necromancy;
    public Skill transmutation => Register.transmutation;
  }
}