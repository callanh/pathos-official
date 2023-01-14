using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexWarnings : CodexPage<ManifestWarnings, WarningEditor, Warning>
  {
    private CodexWarnings() { }
#if MASTER_CODEX
    internal CodexWarnings(Codex Codex)
      : base(Codex.Manifest.Warnings)
    {
      Warning AddWarning(string Name, Inv.Colour Colour)
      {
        return Register.Add(W =>
        {
          W.Name = Name;
          W.Colour = Colour;
          W.Glyph = Codex.Glyphs.GetGlyphOrNull(W.Name + " warning");
          Debug.Assert(W.Glyph != null, W.Name + " warning must have a glyph.");
        });
      }

      White = AddWarning("white", Inv.Colour.White);
      Yellow = AddWarning("yellow", Inv.Colour.Yellow);
      Orange = AddWarning("orange", Inv.Colour.Orange);
      Pink = AddWarning("pink", Inv.Colour.Pink);
      Purple = AddWarning("purple", Inv.Colour.Purple);
      Black = AddWarning("black", Inv.Colour.Black);
    }
#endif

    /// <summary>
    /// Levels = 0..4
    /// </summary>
    public readonly Warning White;
    /// <summary>
    /// Levels = 5..9
    /// </summary>
    public readonly Warning Yellow;
    /// <summary>
    /// Levels = 10..14
    /// </summary>
    public readonly Warning Orange;
    /// <summary>
    /// Levels = 15..19
    /// </summary>
    public readonly Warning Pink;
    /// <summary>
    /// Levels = 20..24
    /// </summary>
    public readonly Warning Purple;
    /// <summary>
    /// Levels = 25+ 
    /// </summary>
    public readonly Warning Black;
  }
}
