using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexGates : CodexPage<ManifestGates, GateEditor, Gate>
  {
    private CodexGates() { }
#if MASTER_CODEX
    internal CodexGates(Codex Codex)
      : base(Codex.Manifest.Gates)
    {
      var Glyphs = Codex.Glyphs; 
      var Sonics = Codex.Sonics;
      var Materials = Codex.Materials;
      var Elements = Codex.Elements;

      Gate AddGate(string Name, Material Material, Action<GateEditor> Action)
      {
        return Register.Add(G =>
        {
          G.Name = Name;
          G.Material = Material;

          Action(G);
        });
      }

      var GateWeakness = new[] { Elements.force, Elements.digging }; 

      crystal_door = AddGate("crystal door", Materials.crystal, G =>
      {
        G.Description = "This heavy door is made out of hard crystal formations and is not easily damaged.";
        G.OpenHorizontalGlyph = Glyphs.crystal_door_open_horizontal;
        G.OpenVerticalGlyph = Glyphs.crystal_door_open_vertical;
        G.ClosedHorizontalGlyph = Glyphs.crystal_door_closed_horizontal;
        G.ClosedVerticalGlyph = Glyphs.crystal_door_closed_vertical;
        G.BrokenGlyph = Glyphs.crystal_door_broken;
        G.LockedHorizontalGlyph = Glyphs.crystal_door_locked_horizontal;
        G.LockedVerticalGlyph = Glyphs.crystal_door_locked_vertical;
        G.TrappedHorizontalGlyph = Glyphs.crystal_door_trapped_horizontal;
        G.TrappedVerticalGlyph = Glyphs.crystal_door_trapped_vertical;
        G.OpenSonic = Sonics.open_door;
        G.CloseSonic = Sonics.close_door;
        G.BreakSonic = Sonics.broken_door;
        G.DefaultSecretBarrier = Codex.Barriers.hell_brick;
        G.SetWeakness(GateWeakness);
      });

      wooden_door = AddGate("wooden door", Materials.wood, G =>
      {
        G.OpenHorizontalGlyph = Glyphs.wooden_door_open_horizontal;
        G.OpenVerticalGlyph = Glyphs.wooden_door_open_vertical;
        G.ClosedHorizontalGlyph = Glyphs.wooden_door_closed_horizontal;
        G.ClosedVerticalGlyph = Glyphs.wooden_door_closed_vertical;
        G.BrokenGlyph = Glyphs.wooden_door_broken;
        G.LockedHorizontalGlyph = Glyphs.wooden_door_locked_horizontal;
        G.LockedVerticalGlyph = Glyphs.wooden_door_locked_vertical;
        G.TrappedHorizontalGlyph = Glyphs.wooden_door_trapped_horizontal;
        G.TrappedVerticalGlyph = Glyphs.wooden_door_trapped_vertical;
        G.OpenSonic = Sonics.open_door;
        G.CloseSonic = Sonics.close_door;
        G.BreakSonic = Sonics.broken_door;
        G.DefaultSecretBarrier = Codex.Barriers.stone_wall;
        G.SetWeakness(GateWeakness);
      });
    }
#endif

    public readonly Gate wooden_door;
    public readonly Gate crystal_door;
  }
}
