using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexEngulfments : CodexPage<ManifestEngulfments, EngulfmentEditor, Engulfment>
  {
    private CodexEngulfments() { }
#if MASTER_CODEX
    internal CodexEngulfments(Codex Codex)
      : base(Codex.Manifest.Engulfments)
    {
      Engulfment AddEngulfment(string Name, Sonic Sonic)
      {
        return Register.Add(E =>
        {
          E.Name = Name;
          E.Sonic = Sonic;

          foreach (var Area in Inv.Support.EnumHelper.GetEnumerable<EngulfmentArea>())
            E.SetGlyph(Area, Codex.Glyphs.GetGlyph("swallowed " + Area.ToString().PascalCaseToTitleCase().ToLowerInvariant()));
        });
      }

      engulfed = AddEngulfment("engulfed", Codex.Sonics.swallow);
      swallowed = AddEngulfment("swallowed", Codex.Sonics.swallow);
    }
#endif

    public readonly Engulfment engulfed;
    public readonly Engulfment swallowed;
  }
}