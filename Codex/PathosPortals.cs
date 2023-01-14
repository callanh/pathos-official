using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexPortals : CodexPage<ManifestPortals, PortalEditor, Portal>
  {
    private CodexPortals() { }
#if MASTER_CODEX
    internal CodexPortals(Codex Codex)
      : base(Codex.Manifest.Portals)
    {
      var Glyphs = Codex.Glyphs;
      var Sonics = Codex.Sonics;
      var Materials = Codex.Materials;

      Portal AddPortal(string Name, Material Material, Traversal Traversal, Glyph Glyph, Sonic Sonic, Action<PortalEditor> EditorAction)
      {
        return Register.Add(P =>
        {
          P.Name = Name;
          P.Material = Material;  
          P.Traversal = Traversal;
          P.Glyph = Glyph;
          P.Sonic = Sonic;

          EditorAction(P);
        });
      }

      clay_staircase_up = AddPortal("clay staircase up", Materials.clay, Traversal.Ascend, Glyphs.clay_staircase_up, Sonics.footsteps, P =>
      {
        P.Description = null;
      });
      clay_staircase_down = AddPortal("clay staircase down", Materials.clay, Traversal.Descend, Glyphs.clay_staircase_down, Sonics.footsteps, P =>
      {
        P.Description = null;
      });

      jade_ladder_up = AddPortal("jade ladder up", Materials.gemstone, Traversal.Ascend, Glyphs.jade_ladder_up, Sonics.footsteps, P =>
      {
        P.Description = null;
      });
      jade_ladder_down = AddPortal("jade ladder down", Materials.gemstone, Traversal.Descend, Glyphs.jade_ladder_down, Sonics.footsteps, P =>
      {
        P.Description = null;
      });

      rift = AddPortal("rift", Materials.ether, Traversal.Travel, Glyphs.rift, Sonics.teleport, P =>
      {
        P.Description = "This twisting and howling gateway of damned souls connects you to the netherworld.";
      });

      stone_staircase_up = AddPortal("stone staircase up", Materials.stone, Traversal.Ascend, Glyphs.stone_staircase_up, Sonics.footsteps, P =>
      {
        P.Description = null;
      });
      stone_staircase_down = AddPortal("stone staircase down", Materials.stone, Traversal.Descend, Glyphs.stone_staircase_down, Sonics.footsteps, P =>
      {
        P.Description = null;
      });

      transportal = AddPortal("transportal", Materials.stone, Traversal.Travel, Glyphs.transportal, Sonics.magic, P =>
      {
        P.Description = "Magical energy swirls and contracts revealing glimpses of what might be waiting for you on other side.";
      });

      wooden_ladder_up = AddPortal("wooden ladder up", Materials.wood, Traversal.Ascend, Glyphs.wooden_ladder_up, Sonics.footsteps, P =>
      {
        P.Description = null;
      });
      wooden_ladder_down = AddPortal("wooden ladder down", Materials.wood, Traversal.Descend, Glyphs.wooden_ladder_down, Sonics.footsteps, P =>
      {
        P.Description = null;
      });
    }
#endif

    public readonly Portal clay_staircase_up;
    public readonly Portal clay_staircase_down;
    public readonly Portal jade_ladder_up;
    public readonly Portal jade_ladder_down;
    public readonly Portal rift;
    public readonly Portal stone_staircase_up;
    public readonly Portal stone_staircase_down;
    public readonly Portal transportal;
    public readonly Portal wooden_ladder_up;
    public readonly Portal wooden_ladder_down;
  }
}