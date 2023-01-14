using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexAttributes : CodexPage<ManifestAttributes, AttributeEditor, Attribute>
  {
    private CodexAttributes() { }
#if MASTER_CODEX
    internal CodexAttributes(Codex Codex)
      : base(Codex.Manifest.Attributes)
    {
      foreach (var Attribute in Register.List)
      {
        var Editor = Register.Edit(Attribute);
        Editor.GainSonic = Codex.Sonics.gain_ability;
        Editor.LoseSonic = Codex.Sonics.lose_ability;
      }
    }
#endif

    public Attribute strength => Register.strength;
    public Attribute dexterity => Register.dexterity;
    public Attribute constitution => Register.constitution;
    public Attribute intelligence => Register.intelligence;
    public Attribute wisdom => Register.wisdom;
    public Attribute charisma => Register.charisma;
  }
}
