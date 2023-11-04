using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexBarriers : CodexPage<ManifestBarriers, BarrierEditor, Barrier>
  {
    private CodexBarriers() { }
#if MASTER_CODEX
    internal CodexBarriers(Codex Codex)
      : base(Codex.Manifest.Barriers)
    {
      var Glyphs = Codex.Glyphs; 
      var Grounds = Codex.Grounds;
      var Materials = Codex.Materials;

      Barrier AddBarrier(string Name, Material Material, Ground Underlay, Glyph Glyph, bool Opaque, Action<BarrierEditor> EditorAction)
      {
        return Register.Add(B =>
        {
          B.Name = Name;
          B.Material = Material;
          B.Underlay = Underlay;
          B.Opaque = Opaque;

          if (Glyph == null)
          {
            foreach (var Segment in Inv.Support.EnumHelper.GetEnumerable<WallSegment>())
              B.SetSegment(Segment, Codex.Glyphs.GetGlyph(Name + " " + Segment.ToString().PascalCaseToTitleCase().ToLowerInvariant()));
          }
          else
          {
            B.SetUniform(Glyph);
          }

          EditorAction(B);
        });
      }

      iron_bars = AddBarrier("iron bars", Materials.iron, Grounds.stone_floor, Glyphs.iron_bars, Opaque: false, B =>
      {
        B.Description = null;
      });

      shroom = AddBarrier("shroom", Materials.vegetable, Grounds.dirt, Glyphs.shroom, Opaque: true, B =>
      {
        B.Description = null;
      });

      tree = AddBarrier("tree", Materials.wood, Grounds.dirt, Glyphs.tree, Opaque: true, B =>
      {
        B.Description = null;
      });

      cave_wall = AddBarrier("cave wall", Materials.clay, Grounds.cave_floor, Glyph: null, Opaque: true, B =>
      {
        B.Description = null;
      });

      jade_wall = AddBarrier("jade wall", Materials.gemstone, Grounds.marble_floor, Glyph: null, Opaque: true, B =>
      {
        B.Description = null;
      });

      stone_wall = AddBarrier("stone wall", Materials.stone, Grounds.stone_floor, Glyph: null, Opaque: true, B =>
      {
        B.Description = "Heavy grey stones are cobbled together to make this wall.";
      });

      hell_brick = AddBarrier("hell brick", Materials.stone, Grounds.obsidian_floor, Glyph: null, Opaque: true, B =>
      {
        B.Description = null;
      });

      wooden_wall = AddBarrier("wooden wall", Materials.wood, Grounds.wooden_floor, Glyph: null, Opaque: true, B =>
      {
        B.Description = null;
      });
    }
#endif

    public readonly Barrier iron_bars;
    public readonly Barrier shroom;
    public readonly Barrier tree;
    public readonly Barrier cave_wall;
    public readonly Barrier jade_wall;
    public readonly Barrier hell_brick;
    public readonly Barrier stone_wall;
    public readonly Barrier wooden_wall;
  }
}