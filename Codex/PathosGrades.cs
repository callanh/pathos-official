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
        G.Indestructable = false;
        G.MinimumEnchantment = Modifier.Minus5;
        G.MaximumEnchantment = Modifier.Plus5;
      });


      artifact = Register.Add(G =>
      {
        G.Name = "artifact";
        G.Unique = true;
        G.Indestructable = true;
        G.MinimumEnchantment = Modifier.FromRank(-10);
        G.MaximumEnchantment = Modifier.FromRank(+10);
      });
    }
#endif

    public readonly Grade standard;
    public readonly Grade artifact;
  }
}