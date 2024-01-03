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
      var Sonics = Codex.Sonics;
      var Grounds = Codex.Grounds;
      var Materials = Codex.Materials;
      var Elements = Codex.Elements;

      CodexVolatiles Volatiles = null;
      CodexRecruiter.Enrol(() =>
      {
        Volatiles = Codex.Volatiles;
      });

      Barrier AddBarrier(string Name, Material Material, Ground Underlay, Glyph Glyph, bool Opaque, bool Rebound, Action<BarrierEditor> EditorAction)
      {
        return Register.Add(B =>
        {
          B.Name = Name;
          B.Material = Material;
          B.Underlay = Underlay;
          B.Opaque = Opaque;
          B.Rebound = Rebound;

          if (Glyph == null)
          {
            foreach (var Segment in Inv.Support.EnumHelper.GetEnumerable<WallSegment>())
              B.SetSegment(Segment, Codex.Glyphs.GetGlyph(Name + " " + Segment.ToString().PascalCaseToTitleCase().ToLowerInvariant()));
          }
          else
          {
            B.SetUniform(Glyph);
          }

          CodexRecruiter.Enrol(() => EditorAction(B));
        });
      }

      iron_bars = AddBarrier("iron bars", Materials.iron, Grounds.stone_floor, Glyphs.iron_bars, Opaque: false, Rebound: false, B =>
      {
        B.Description = null;
        B.CreateSonic = Sonics.bars_slamming;
        B.DestroySonic = Sonics.bars_raising;
        B.AddReaction(Chance.Always, Elements.fire, A => A.RemoveWall(WallStructure.Solid, iron_bars));
        B.AddReaction(Chance.Always, Elements.cold, A => A.RemoveWall(WallStructure.Solid, iron_bars));
        B.AddReaction(Chance.Always, Elements.shock, A => A.RemoveWall(WallStructure.Solid, iron_bars));
        B.AddReaction(Chance.Always, Elements.acid, A => A.RemoveWall(WallStructure.Solid, iron_bars));
        B.AddReaction(Chance.Always, Elements.force, A => A.RemoveWall(WallStructure.Solid, iron_bars));
        B.AddReaction(Chance.Always, Elements.magical, A => A.RemoveWall(WallStructure.Solid, iron_bars));
        B.AddReaction(Chance.Always, Elements.disintegrate, A => A.RemoveWall(WallStructure.Solid, iron_bars));
      });

      shroom = AddBarrier("shroom", Materials.vegetable, Grounds.dirt, Glyphs.shroom, Opaque: true, Rebound: false, B =>
      {
        B.Description = null;
        B.AddReaction(Chance.Always, Elements.cold, A => A.CreateSpill(Volatiles.freeze, 1.d100() + 100));
      });

      tree = AddBarrier("tree", Materials.wood, Grounds.dirt, Glyphs.tree, Opaque: true, Rebound: false, B =>
      {
        B.Description = null;
        B.AddReaction(Chance.Always, Elements.fire, A => A.CreateSpill(Volatiles.blaze, 1.d100() + 100));
      });

      cave_wall = AddBarrier("cave wall", Materials.clay, Grounds.cave_floor, Glyph: null, Opaque: true, Rebound: true, B =>
      {
        B.Description = null;
      });

      jade_wall = AddBarrier("jade wall", Materials.gemstone, Grounds.marble_floor, Glyph: null, Opaque: true, Rebound: true, B =>
      {
        B.Description = null;
      });

      stone_wall = AddBarrier("stone wall", Materials.stone, Grounds.stone_floor, Glyph: null, Opaque: true, Rebound: true, B =>
      {
        B.Description = "Heavy grey stones are cobbled together to make this wall.";
      });

      hell_brick = AddBarrier("hell brick", Materials.stone, Grounds.obsidian_floor, Glyph: null, Opaque: true, Rebound: true, B =>
      {
        B.Description = null;
      });

      wooden_wall = AddBarrier("wooden wall", Materials.wood, Grounds.wooden_floor, Glyph: null, Opaque: true, Rebound: true, B =>
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