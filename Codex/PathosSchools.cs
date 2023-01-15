using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexSchools : CodexPage<ManifestSchools, SchoolEditor, School>
  {
    private CodexSchools() { }
#if MASTER_CODEX
    internal CodexSchools(Codex Codex)
      : base(Codex.Manifest.Schools)
    {
      var Skills = Codex.Skills;

      School AddSchool(string Name, char Symbol, Inv.Colour Colour, Skill Skill)
      {
        return Register.Add(S =>
        {
          S.Name = Name;
          S.Symbol = Symbol;
          S.Colour = Colour;
          S.Skill = Skill;
        });
      }

      abjuration = AddSchool("abjuration", 'å', Inv.Colour.Gold, Skills.abjuration);
      clerical = AddSchool("clerical", 'Ͼ', Inv.Colour.DarkCyan.Lighten(0.10F), Skills.clerical);
      conjuration = AddSchool("conjuration", 'ĵ', Inv.Colour.MediumPurple.Darken(0.30F), Skills.conjuration);
      divination = AddSchool("divination", 'Đ', Inv.Colour.SandyBrown, Skills.divination);
      enchantment = AddSchool("enchantment", 'Ǝ', Inv.Colour.HotPink.Darken(0.05F), Skills.enchantment);
      evocation = AddSchool("evocation", 'Ɣ', Inv.Colour.DarkRed.Lighten(0.20F), Skills.evocation);
      //illusion = AddSchool("illusion", 'i', Inv.Colour.Yellow, Skills.Illusion);
      transmutation = AddSchool("transmutation", 'Ͳ', Inv.Colour.RoyalBlue.Darken(0.30F), Skills.transmutation);
      necromancy = AddSchool("necromancy", 'ň', Inv.Colour.Gray, Skills.necromancy);
    }
#endif

    public readonly School abjuration;
    public readonly School clerical;
    public readonly School conjuration;
    public readonly School divination;
    public readonly School enchantment;
    public readonly School evocation;
    //public readonly School illusion;
    public readonly School transmutation;
    public readonly School necromancy;
  }
}