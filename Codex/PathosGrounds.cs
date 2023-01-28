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

      Ground AddGround(string Name, Material Substance, Glyph Glyph, Action<GroundEditor> Action)
      {
        return Register.Add(G =>
        {
          G.Name = Name;
          G.Substance = Substance;
          G.Terrain = Materials.air;
          G.Glyph = Glyph;

          CodexRecruiter.Enrol(() => Action(G));
        });
      }
      
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

      dirt_floor = AddGround("dirt floor", Materials.sand, Glyphs.dirt_floor, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.clay_boulder);
      });

      stone_floor = AddGround("stone floor", Materials.stone, Glyphs.stone_floor, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      wooden_floor = AddGround("wooden floor", Materials.wood, Glyphs.wooden_floor, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.wooden_barrel);
      });

      stone_corridor = AddGround("stone corridor", Materials.stone, Glyphs.stone_corridor, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      grass = AddGround("grass", Materials.vegetable, Glyphs.grass, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      moss = AddGround("moss", Materials.vegetable, Glyphs.moss, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      sand = AddGround("sand", Materials.sand, Glyphs.sand, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      hive_floor = AddGround("hive floor", Materials.stone, Glyphs.hive_floor, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);
      });

      obsidian_floor = AddGround("obsidian floor", Materials.stone, Glyphs.obsidian_floor, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.crystal_boulder);
      });

      marble_floor = AddGround("marble floor", Materials.stone, Glyphs.marble_floor, G =>
      {
        G.Description = "The cool stone surface has immaculate craftsmanship but is marked by damage and time.";

        G.SetBlock(Codex.Blocks.crystal_boulder);
      });

      metal_floor = AddGround("metal floor", Materials.iron, Glyphs.metal_floor, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.wooden_barrel);
      });

      ice = AddGround("ice", Materials.ice, Glyphs.ice, G =>
      {
        G.SetSlippery(Codex.Anatomies.limbs, Codex.Attributes.dexterity, Sonics.slip);
        G.SetBlock(Codex.Blocks.stone_boulder);

        G.AddReaction(Chance.Always, Elements.fire, A => A.ConvertFloor(FromGround: null, ToGround: water, Locality.Square));
      });

      lava = AddGround("lava", Materials.lava, Glyphs.lava, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);

        G.SetSunken(Inv.Colour.Red.Opacity(0.50F), Elements.fire, Sonics.burn, Skills.swimming,
          Enter => Enter.Harm(Elements.fire, 100.d10()),
          Drop => Drop.DestroyTargetAsset(CountDice: null)); // TODO: fire resistant items should not be destroyed?

        // TODO: cold element _could_ converts lava to obsidian... but is it really cold enough to do that?
      });

      water = AddGround("water", Materials.water, Glyphs.water, G =>
      {
        G.Description = null;

        G.SetBlock(Codex.Blocks.stone_boulder);

        G.AddReaction(Chance.Always, Elements.cold, A => A.ConvertFloor(FromGround: null, ToGround: ice, Locality.Square));
        G.AddReaction(Chance.Always, Elements.shock, A => A.ApplyTransient(Properties.stunned, 3.d6()));

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
    }
#endif

    public readonly Ground chasm;
    public readonly Ground dirt_floor;
    public readonly Ground grass;
    public readonly Ground hive_floor;
    public readonly Ground ice;
    public readonly Ground lava;
    public readonly Ground marble_floor;
    public readonly Ground metal_floor;
    public readonly Ground moss;
    public readonly Ground obsidian_floor;
    public readonly Ground stone_floor;
    public readonly Ground stone_corridor;
    public readonly Ground sand;
    public readonly Ground water;
    public readonly Ground wooden_floor;
  }
}
