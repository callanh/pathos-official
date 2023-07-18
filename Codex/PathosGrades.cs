using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexGrades : CodexPage<ManifestGrades, GradeEditor, Grade>
  {
    private CodexGrades() { }
#if MASTER_CODEX
    internal CodexGrades(Codex Codex)
      : base(Codex.Manifest.Grades)
    {
      standard = Register.Add(G =>
      {
        G.Name = "standard";
        G.Unique = false;
        G.Indestructible = false;
        G.MinimumEnchantment = Modifier.Minus5;
        G.MaximumEnchantment = Modifier.Plus5;
      });

      exotic = Register.Add(G =>
      {
        G.Name = "exotic";
        G.Unique = false;
        G.Indestructible = false;
        G.MinimumEnchantment = Modifier.Minus3;
        G.MaximumEnchantment = Modifier.Plus7;
      });

      artifact = Register.Add(G =>
      {
        G.Name = "artifact";
        G.Unique = true;
        G.Indestructible = true;
        G.MinimumEnchantment = Modifier.Minus10;
        G.MaximumEnchantment = Modifier.Plus10;
      });
    }
#endif

    public readonly Grade standard;
    public readonly Grade exotic;
    public readonly Grade artifact;
  }
}