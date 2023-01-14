using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexPlatforms : CodexPage<ManifestPlatforms, PlatformEditor, Platform>
  {
    private CodexPlatforms() { }
#if MASTER_CODEX
    internal CodexPlatforms(Codex Codex)
      : base(Codex.Manifest.Platforms)
    {
      Platform AddPlatform(string Name, Material Material, Action<PlatformEditor> PlatformAction)
      {
        return Register.Add(P =>
        {
          P.Name = Name;
          P.Material = Material;

          CodexRecruiter.Enrol(() => PlatformAction(P));
        });
      }

      crystal_bridge = AddPlatform("crystal bridge", Codex.Materials.gemstone, P =>
      {
        P.Description = null;
        P.HorizontalGlyph = Codex.Glyphs.crystal_bridge_horizontal;
        P.VerticalGlyph = Codex.Glyphs.crystal_bridge_vertical;
      });

      wooden_bridge = AddPlatform("wooden bridge", Codex.Materials.wood, P =>
      {
        P.Description = "This hardwood bridge has a solid integrity despite many years of use.";
        P.HorizontalGlyph = Codex.Glyphs.wooden_bridge_horizontal;
        P.VerticalGlyph = Codex.Glyphs.wooden_bridge_vertical;
      });
    }
#endif

    public readonly Platform crystal_bridge;
    public readonly Platform wooden_bridge;
  }
}