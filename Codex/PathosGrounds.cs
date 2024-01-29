using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexGrounds : CodexPage<ManifestGrounds, GroundEditor, Ground>
  {
    private CodexGrounds() { }
#if MASTER_CODEX
    internal CodexGrounds(Codex Codex)
      : base(Codex.Manifest.Grounds)
    {
      var Materials = Codex.Materials;
      var Properties = Codex.Properties;
      var Elements = Codex.Elements;
      var Glyphs = Codex.Glyphs;
      var Sonics = Codex.Sonics;
      var Skills = Codex.Skills;

      CodexVolatiles Volatiles = null;
      CodexRecruiter.Enrol(() =>
      {
        Volatiles = Codex.Volatiles;
      });

      Ground AddGround(string Name, Material Substance, Glyph Glyph, Action<GroundEditor> EditorAction)
      {
        return Register.Add(G =>
        {
          G.Name = Name;
          G.Substance = Substance;
          G.Terrain = Materials.air;
          G.Glyph = Glyph;

          CodexRecruiter.Enrol(() => EditorAction(G));
        });
      }

      cave_floor = AddGround("cave floor", Materials.stone, Glyphs.cave_floor, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.clay_boulder);
      });

      chasm = AddGround("chasm", Materials.air, Glyphs.chasm, G =>
      {
        G.Description = null;

        G.SetSunken(Inv.Colour.Black.Opacity(0.50F), Element: null, Sonics.thump, Skill: null,
          Enter =>
          {
            Enter.TransitionDescend(Teleport: null, Fixed: true, AdjustmentDice: Dice.One);
            Enter.Harm(Elements.physical, 1.d6());
          },
          Drop => Drop.TransitionDescend(Teleport: null, Fixed: true, AdjustmentDice: Dice.One));
      });

      dirt = AddGround("dirt", Materials.sand, Glyphs.dirt, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.clay_boulder);
      });

      gold_floor = AddGround("gold floor", Materials.gold, Glyphs.gold_floor, G =>
      {
        G.Description = null;

        G.AddReaction(Chance.OneIn5, Elements.shock, A => A.CreateSpill(Volatiles.electricity, 1.d100() + 100)); // gold is highly conductive.

        G.SetBlock(Codex.Blocks.gold_boulder);
      });

      granite_floor = AddGround("granite floor", Materials.stone, Glyphs.granite_floor, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      grass = AddGround("grass", Materials.vegetable, Glyphs.grass, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      hive_floor = AddGround("hive floor", Materials.stone, Glyphs.hive_floor, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      ice = AddGround("ice", Materials.ice, Glyphs.ice, G =>
      {
        G.SetSlippery(Codex.Anatomies.limbs, Codex.Attributes.dexterity, Sonics.slip);
        G.SetBlock(Codex.Blocks.stone_boulder);

        G.AddReaction(Chance.Always, Elements.fire, A => A.ConvertFloor(FromGround: ice, ToGround: water, Locality.Square));
        G.AddReaction(Chance.Always, Elements.cold, A => A.CreateSpill(Volatiles.freeze, 1.d100() + 100));
      });

      lava = AddGround("lava", Materials.lava, Glyphs.lava, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);

        G.AddReaction(Chance.Always, Elements.water, A => A.CreateSpill(Volatiles.steam, 1.d100() + 100));

        G.SetSunken(Inv.Colour.Red.Opacity(0.50F), Elements.fire, Sonics.burn, Skills.swimming,
          Enter => Enter.Harm(Elements.fire, 100.d10()),
          Drop => Drop.DestroyTargetAsset(CountDice: null)); // TODO: fire resistant items should not be destroyed?

        // TODO: cold element _could_ converts lava to obsidian... but is it really cold enough to do that?
      });

      marble_floor = AddGround("marble floor", Materials.stone, Glyphs.marble_floor, G =>
      {
        G.Description = "The cool stone surface has immaculate craftsmanship but is marked by damage and time.";

        G.SetBlock(Codex.Blocks.crystal_boulder);
      });

      metal_floor = AddGround("metal floor", Materials.iron, Glyphs.metal_floor, G =>
      {
        G.Description = null;

        G.AddReaction(Chance.OneIn10, Elements.shock, A => A.CreateSpill(Volatiles.electricity, 1.d100() + 100));

        G.SetBlock(Codex.Blocks.wooden_barrel);
      });

      moss = AddGround("moss", Materials.vegetable, Glyphs.moss, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      obsidian_floor = AddGround("obsidian floor", Materials.stone, Glyphs.obsidian_floor, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.crystal_boulder);
      });

      sand = AddGround("sand", Materials.sand, Glyphs.sand, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      sewer_floor = AddGround("sewer floor", Materials.stone, Glyphs.sewer_floor, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.wooden_barrel);
      });

      snow = AddGround("snow", Materials.ice, Glyphs.snow, G =>
      {
        G.Description = null;

        G.SpeedMultiplier = 0.80F; // 80% speed when moving through snow.

        G.SetBlock(Codex.Blocks.stone_boulder);

        G.AddReaction(Chance.Always, Elements.fire, A => A.ConvertFloor(FromGround: snow, ToGround: dirt, Locality.Square));
      });

      stone_floor = AddGround("stone floor", Materials.stone, Glyphs.stone_floor, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      stone_path = AddGround("stone path", Materials.stone, Glyphs.stone_path, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      water = AddGround("water", Materials.water, Glyphs.water, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);

        G.AddReaction(Chance.Always, Elements.cold, A => A.ConvertFloor(FromGround: water, ToGround: ice, Locality.Square));
        G.AddReaction(Chance.Always, Elements.shock, A => A.ApplyTransient(Properties.stunned, 3.d6()));
        G.AddReaction(Chance.Always, Elements.fire, A => A.CreateSpill(Volatiles.steam, 1.d100() + 100));

        G.SetSunken(Inv.Colour.Blue.Opacity(0.50F), Elements.water, Sonics.water_impact, Skills.swimming,
          Enter =>
          {
            Enter.Harm(Elements.water, Dice.Zero);
            Enter.WhenChance(Chance.OneIn20, T => T.ConvertAsset(Codex.Stocks.potion, WholeStack: true, Codex.Items.potion_of_water));
            Enter.WhenChance(Chance.OneIn20, T => T.ConvertAsset(Codex.Stocks.scroll, WholeStack: true, Codex.Items.scroll_of_blank_paper));
            Enter.WhenChance(Chance.OneIn20, T => T.ConvertAsset(Codex.Stocks.book, WholeStack: true, Codex.Items.book_of_blank_paper));
          },
          Drop =>
          {
            Drop.ConvertAsset(Codex.Stocks.potion, WholeStack: true, Codex.Items.potion_of_water);
            Drop.ConvertAsset(Codex.Stocks.scroll, WholeStack: true, Codex.Items.scroll_of_blank_paper);
            Drop.ConvertAsset(Codex.Stocks.book, WholeStack: true, Codex.Items.book_of_blank_paper);
          }
        );
      });

      wooden_floor = AddGround("wooden floor", Materials.wood, Glyphs.wooden_floor, G =>
      {
        G.Description = null;

        G.AddReaction(Chance.OneIn10, Elements.fire, A => A.CreateSpill(Volatiles.blaze, 1.d100() + 100));

        G.SetBlock(Codex.Blocks.wooden_barrel);
      });

      // renames.
      Register.Alias(dirt, "dirt floor");
      Register.Alias(stone_path, "stone corridor");
    }
#endif

    public readonly Ground cave_floor;
    public readonly Ground chasm;
    public readonly Ground dirt;
    public readonly Ground gold_floor;
    public readonly Ground granite_floor;
    public readonly Ground grass;
    public readonly Ground hive_floor;
    public readonly Ground ice;
    public readonly Ground lava;
    public readonly Ground marble_floor;
    public readonly Ground metal_floor;
    public readonly Ground moss;
    public readonly Ground obsidian_floor;
    public readonly Ground sand;
    public readonly Ground sewer_floor;
    public readonly Ground snow;
    public readonly Ground stone_floor;
    public readonly Ground stone_path;
    public readonly Ground water;
    public readonly Ground wooden_floor;
  }
}
