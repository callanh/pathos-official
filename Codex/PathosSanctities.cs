using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexSanctities : CodexPage<ManifestSanctities, SanctityEditor, Sanctity>
  {
    private CodexSanctities() { }
#if MASTER_CODEX
    internal CodexSanctities(Codex Codex)
      : base(Codex.Manifest.Sanctities)
    {
      Sanctity AddSancity(int Factor, string Code, string Name, int Frequency, Inv.Colour SolidColour)
      {
        return Register.Add(S =>
        {
          S.Factor = Factor;
          S.Code = Code;
          S.Name = Name;
          S.Frequency = Frequency;
          S.TintColour = SolidColour.Opacity(0.25F);
          S.SolidColour = SolidColour;
        });
      }

      Cursed = AddSancity(-1, "C", "cursed", Frequency: 5, Inv.Colour.Red);
      Uncursed = AddSancity(0, "U", "uncursed", Frequency: 90, Inv.Colour.WhiteSmoke);
      Blessed = AddSancity(+1, "B", "blessed", Frequency: 5, Inv.Colour.Blue);

      // TODO: ⒸⓊⒷ◯ ? They looks small and need a colour.
      // ⛨⛧⛉ ⒞⒝⒰
      // ᛔ ⁇

      Register.Mundane = Uncursed;
    }
#endif

    public readonly Sanctity Cursed;
    public readonly Sanctity Uncursed;
    public readonly Sanctity Blessed;
  }
}