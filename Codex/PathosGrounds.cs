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

      sand = AddGround("sand", Materials.sand, Glyphs.sand, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.sand_join);
        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      snow = AddGround("snow", Materials.ice, Glyphs.snow, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.snow_join);
        G.SpeedMultiplier = 0.80F; // 80% speed when moving through snow.
        G.SetBlock(Codex.Blocks.stone_boulder);
        G.AddReaction(Chance.Always, Elements.fire, A => A.ConvertGround(FromGround: snow, ToGround: dirt, Locality.Square));
      });

      moss = AddGround("moss", Materials.vegetable, Glyphs.moss, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.moss_join);
        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      grass = AddGround("grass", Materials.vegetable, Glyphs.grass, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.grass_join);
        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      dirt = AddGround("dirt", Materials.sand, Glyphs.dirt, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.dirt_join);
        G.SetBlock(Codex.Blocks.clay_boulder);
      });

      cave_floor = AddGround("cave floor", Materials.stone, Glyphs.cave_floor, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.cave_join);
        G.SetBlock(Codex.Blocks.clay_boulder);
      });

      gold_floor = AddGround("gold floor", Materials.gold, Glyphs.gold_floor, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.gold_join);
        G.SetBlock(Codex.Blocks.gold_boulder);
        G.AddReaction(Chance.OneIn5, Elements.shock, A => A.CreateVolatile(Volatiles.electricity, Duration: 1.d100() + 100)); // gold is highly conductive.
      });

      granite_floor = AddGround("granite floor", Materials.stone, Glyphs.granite_floor, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.granite_join);
        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      hive_floor = AddGround("hive floor", Materials.stone, Glyphs.hive_floor, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.hive_join);
        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      marble_floor = AddGround("marble floor", Materials.stone, Glyphs.marble_floor, G =>
      {
        G.Description = "The cool stone surface has immaculate craftsmanship but is marked by damage and time.";

        G.SetJoin(Glyphs.marble_join);
        G.SetBlock(Codex.Blocks.crystal_boulder);
      });

      metal_floor = AddGround("metal floor", Materials.iron, Glyphs.metal_floor, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.metal_join);
        G.SetBlock(Codex.Blocks.wooden_barrel);
        G.AddReaction(Chance.OneIn10, Elements.shock, A => A.CreateVolatile(Volatiles.electricity, Duration: 1.d100() + 100));
      });

      obsidian_floor = AddGround("obsidian floor", Materials.stone, Glyphs.obsidian_floor, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.obsidian_join);
        G.SetBlock(Codex.Blocks.crystal_boulder);
      });

      sewer_floor = AddGround("sewer floor", Materials.stone, Glyphs.sewer_floor, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.sewer_join);
        G.SetBlock(Codex.Blocks.wooden_barrel);
      });

      stone_floor = AddGround("stone floor", Materials.stone, Glyphs.stone_floor, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.stone_join);
        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      stone_path = AddGround("stone path", Materials.stone, Glyphs.stone_path, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.stone_path_join);
        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      wooden_floor = AddGround("wooden floor", Materials.wood, Glyphs.wooden_floor, G =>
      {
        G.Description = null;

        G.SetJoin(Glyphs.wooden_join);
        G.AddReaction(Chance.OneIn10, Elements.fire, A => A.CreateVolatile(Volatiles.blaze, Duration: 1.d100() + 100));
        G.SetBlock(Codex.Blocks.wooden_barrel);
      });

      chasm = AddGround("chasm", Materials.air, Glyphs.chasm, G =>
      {
        G.Description = "A dark, deep fissure in the ground. Stepping into it sends you plunging into the depths below.";

        G.SetSunken(Inv.Colour.Black.Opacity(0.50F), Element: null, Sonics.thump, Skill: null,
          Enter =>
          {
            Enter.TransitionDescend(SpatialProperty: null, AdjustmentDice: Dice.One, Fixed: true);
            Enter.HarmEntity(Elements.physical, 1.d6());
          },
          Drop => Drop.TransitionDescend(SpatialProperty: null, AdjustmentDice: Dice.One, Fixed: true));
      });

      ice = AddGround("ice", Materials.ice, Glyphs.ice, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
        G.SetSlippery(Codex.Anatomies.limbs, Codex.Attributes.dexterity, Sonics.slip);
        G.AddReaction(Chance.Always, Elements.fire, A => A.ConvertGround(FromGround: ice, ToGround: water, Locality.Square));
        G.AddReaction(Chance.Always, Elements.cold, A => A.CreateVolatile(Volatiles.freeze, Duration: 1.d100() + 100));
      });

      lava = AddGround("lava", Materials.lava, Glyphs.lava, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
        G.SetSunken(Inv.Colour.Red.Opacity(0.50F), Elements.fire, Sonics.burn, Skills.swimming,
          Enter => Enter.HarmEntity(Elements.fire, 100.d10()),
          Drop => Drop.DestroyTargetItem(CountDice: null)); // TODO: fire resistant items should not be destroyed?
        G.AddReaction(Chance.Always, Elements.water, A => A.CreateVolatile(Volatiles.steam, Duration: 1.d100() + 100));

        // TODO: cold element _could_ converts lava to obsidian... but is it really cold enough to do that?
      });

      water = AddGround("water", Materials.water, Glyphs.water, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
        G.AddReaction(Chance.Always, Elements.cold, A => A.ConvertGround(FromGround: water, ToGround: ice, Locality.Square));
        G.AddReaction(Chance.Always, Elements.shock, A => A.ApplyTransient(Properties.stunned, 3.d6()));
        G.AddReaction(Chance.Always, Elements.fire, A => A.CreateVolatile(Volatiles.steam, Duration: 1.d100() + 100));
        G.SetSunken(Inv.Colour.Blue.Opacity(0.50F), Elements.water, Sonics.water_impact, Skills.swimming,
          Enter =>
          {
            Enter.HarmEntity(Elements.water, Dice.Zero);
            Enter.WhenChance(Chance.OneIn20, T => T.ConvertItem(Codex.Stocks.potion, WholeStack: true, Codex.Items.potion_of_water));
            Enter.WhenChance(Chance.OneIn20, T => T.ConvertItem(Codex.Stocks.scroll, WholeStack: true, Codex.Items.scroll_of_blank_paper));
            Enter.WhenChance(Chance.OneIn20, T => T.ConvertItem(Codex.Stocks.book, WholeStack: true, Codex.Items.book_of_blank_paper));
          },
          Drop =>
          {
            Drop.ConvertItem(Codex.Stocks.potion, WholeStack: true, Codex.Items.potion_of_water);
            Drop.ConvertItem(Codex.Stocks.scroll, WholeStack: true, Codex.Items.scroll_of_blank_paper);
            Drop.ConvertItem(Codex.Stocks.book, WholeStack: true, Codex.Items.book_of_blank_paper);
          }
        );
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
