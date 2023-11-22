using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexBlocks : CodexPage<ManifestBlocks, BlockEditor, Block>
  {
    private CodexBlocks() { }
#if MASTER_CODEX
    internal CodexBlocks(Codex Codex)
      : base(Codex.Manifest.Blocks)
    {
      var Materials = Codex.Materials;
      var Properties = Codex.Properties;
      var Elements = Codex.Elements;
      var Volatiles = Codex.Volatiles;

      Block AddBlock(string Name, Glyph Glyph, Sonic MoveSonic, Material Material, Size Size, Weight Weight, Action<BlockEditor> Action)
      {
        return Register.Add(B =>
        {
          B.Name = Name;
          B.Glyph = Glyph;
          B.Material = Material;
          B.Size = Size;
          B.Weight = Weight;
          B.MoveSonic = MoveSonic;

          CodexRecruiter.Enrol(() => Action(B));
        });
      }

      clay_boulder = AddBlock("clay boulder", Codex.Glyphs.clay_boulder, Codex.Sonics.scrape, Materials.clay, Size.Huge, Weight.FromUnits(200000), B =>
      {
        B.Description = "This huge block of clay can only be pushed short distances with a lot of effort.";

        B.DamageElement = Elements.physical;
        B.DamageDice = 1.d60();
        B.SetWeakness(Elements.physical, Elements.digging, Elements.force, Elements.disintegrate);

        B.BreakLoot.AddKit(Chance.Always, 3.d6(), Codex.Items.rock);
        B.BreakLoot.AddKit(Chance.OneIn100, Dice.One, Codex.Items.magic_figurine);

        B.AddBreak(1, Codex.Sonics.broken_boulder, null, K => 
        {
        });

        B.SetBarrier(Codex.Barriers.cave_wall);
      });

      crystal_boulder = AddBlock("crystal boulder", Codex.Glyphs.crystal_boulder, Codex.Sonics.scrape, Materials.crystal, Size.Huge, Weight.FromUnits(200000), B =>
      {
        B.Description = "This huge block of crystal can only be pushed short distances with a lot of effort.";

        B.DamageElement = Elements.physical;
        B.DamageDice = 1.d60();
        B.SetWeakness(Elements.physical, Elements.digging, Elements.force, Elements.disintegrate);

        B.BreakLoot.AddKit(Chance.Always, 1.d2() - 1, Codex.Items.blue_glass_bauble);
        B.BreakLoot.AddKit(Chance.Always, 1.d2() - 1, Codex.Items.white_glass_bauble);
        B.BreakLoot.AddKit(Chance.Always, 1.d2() - 1, Codex.Items.green_glass_bauble);
        B.BreakLoot.AddKit(Chance.Always, 1.d2() - 1, Codex.Items.black_glass_bauble);
        B.BreakLoot.AddKit(Chance.Always, 1.d2() - 1, Codex.Items.yellowish_brown_glass_bauble);
        B.BreakLoot.AddKit(Chance.Always, 1.d2() - 1, Codex.Items.yellow_glass_bauble);
        B.BreakLoot.AddKit(Chance.Always, 1.d2() - 1, Codex.Items.orange_glass_bauble);
        B.BreakLoot.AddKit(Chance.Always, 1.d2() - 1, Codex.Items.red_glass_bauble);
        B.BreakLoot.AddKit(Chance.Always, 1.d2() - 1, Codex.Items.violet_glass_bauble);

        B.AddBreak(1, Codex.Sonics.broken_boulder, null, K =>
        {
        });

        B.SetBarrier(Codex.Barriers.hell_brick);
      });

      gold_boulder = AddBlock("gold boulder", Codex.Glyphs.gold_boulder, Codex.Sonics.scrape, Materials.gold, Size.Huge, Weight.FromUnits(400000), B =>
      {
        B.Description = "This huge block of gold ore can only be pushed short distances with a lot of effort.";

        B.DamageElement = Elements.physical;
        B.DamageDice = 1.d120();
        B.SetWeakness(Elements.physical, Elements.digging, Elements.force, Elements.disintegrate);

        B.BreakLoot.AddKit(Chance.Always, 3.d50(), Codex.Items.gold_coin);

        B.AddBreak(1, Codex.Sonics.broken_boulder, null, K =>
        {
        });

        B.SetBarrier(Codex.Barriers.jade_wall);
      });

      statue = AddBlock("statue", Codex.Glyphs.statue, Codex.Sonics.scrape, Materials.stone, Size.Large, Weight.FromUnits(200000), B =>
      {
        B.Description = null;

        B.SetPrison(Codex.Glyphs.statue_base, Tint: null);

        B.DamageElement = Elements.physical;
        B.DamageDice = 1.d30();
        B.SetWeakness(Elements.physical, Elements.digging, Elements.force, Elements.disintegrate);

        B.BreakLoot.AddKit(Chance.Always, 3.d6(), Codex.Items.rock);

        B.AddBreak(1, Codex.Sonics.broken_boulder, null, K =>
        {
        });
      });

      trophy = AddBlock("trophy", Codex.Glyphs.trophy, Codex.Sonics.scrape, Materials.gold, Size.Large, Weight.FromUnits(400000), B =>
      {
        B.Description = null;

        B.SetPrison(Codex.Glyphs.trophy_base, Inv.Colour.DarkGoldenrod);

        B.DamageElement = Elements.physical;
        B.DamageDice = 1.d60();
        B.SetWeakness(Elements.physical, Elements.digging, Elements.force, Elements.disintegrate);

        B.BreakLoot.AddKit(Chance.Always, 5.d50(), Codex.Items.gold_coin);

        B.AddBreak(1, Codex.Sonics.broken_boulder, null, K =>
        {
        });
      });

      stone_boulder = AddBlock("stone boulder", Codex.Glyphs.stone_boulder, Codex.Sonics.scrape, Materials.stone, Size.Huge, Weight.FromUnits(200000), B =>
      {
        B.Description = "This huge block of stone can only be pushed short distances with a lot of effort.";

        B.DamageElement = Elements.physical;
        B.DamageDice = 1.d60();
        B.SetWeakness(Elements.physical, Elements.digging, Elements.force, Elements.disintegrate);
        B.BreakLoot.AddKit(Chance.Always, 3.d6(), Codex.Items.rock);

        B.AddBreak(1, Codex.Sonics.broken_boulder, null, K =>
        {
        });

        B.SetBarrier(Codex.Barriers.stone_wall);
      });

      wooden_barrel = AddBlock("wooden barrel", Codex.Glyphs.wooden_barrel, Codex.Sonics.scrape, Materials.wood, Size.Large, Weight.FromUnits(50000), B =>
      {
        B.Description = "This hefty barrel is made from hardwood and can only be pushed short distances with a lot of effort.";

        B.DamageElement = Elements.physical;
        B.DamageDice = 1.d20();
        B.SetWeakness(Elements.physical, Elements.digging, Elements.force, Elements.disintegrate, Elements.fire, Elements.acid, Elements.shock, Elements.cold);
        B.BreakLoot.AddKit(1.d3(), Chance.OneIn3); // any item.

        B.AddBreak(55, Codex.Sonics.broken_barrel, null, K =>
        {
        });
        B.AddBreak(15, Codex.Sonics.broken_barrel, Codex.Explosions.watery, K =>
        {
          K.Harm(Elements.water, Dice.Zero);
          K.WhenChance(Chance.OneIn20, T => T.ConvertAsset(Codex.Stocks.potion, WholeStack: true, Codex.Items.potion_of_water));
          K.WhenChance(Chance.OneIn20, T => T.ConvertAsset(Codex.Stocks.scroll, WholeStack: true, Codex.Items.scroll_of_blank_paper));
          K.WhenChance(Chance.OneIn20, T => T.ConvertAsset(Codex.Stocks.book, WholeStack: true, Codex.Items.book_of_blank_paper));
        });
        B.AddBreak(5, Codex.Sonics.broken_barrel, Codex.Explosions.muddy, K =>
        {
          K.ApplyTransient(Properties.fumbling, 2.d60());
          K.ApplyTransient(Properties.blindness, 2.d60());
        });
        B.AddBreak(5, Codex.Sonics.broken_barrel, Codex.Explosions.fiery, K =>
        {
          K.Harm(Elements.fire, 3.d6() + 3);
          K.WhenChance(Chance.OneIn3, T => T.CreateSpill(Volatiles.blaze, 1.d100() + 100));
        });
        B.AddBreak(5, Codex.Sonics.broken_barrel, Codex.Explosions.frosty, K =>
        {
          K.Harm(Elements.cold, 3.d6() + 3);
          K.WhenChance(Chance.OneIn3, T => T.CreateSpill(Volatiles.freeze, 1.d100() + 100));
        });
        B.AddBreak(5, Codex.Sonics.broken_barrel, Codex.Explosions.electric, K =>
        {
          K.Harm(Elements.shock, 3.d6() + 3);
          K.WhenChance(Chance.OneIn3, T => T.CreateSpill(Volatiles.electricity, 1.d100() + 100));
        });
        B.AddBreak(5, Codex.Sonics.broken_barrel, Codex.Explosions.acid, K =>
        {
          K.Harm(Elements.acid, 3.d6() + 3);
          K.WhenChance(Chance.OneIn3, T => T.CreateSpill(Volatiles.steam, 1.d100() + 100)); // TODO: acid cloud?
        });
        B.AddBreak(5, Codex.Sonics.broken_barrel, null, K =>
        {
          K.CreateEntity(1.d4() + 1, Codex.Entities.monkey.Sonic, Codex.Entities.monkey); // Barrel of Monkeys!
        });
      });

      Register.Alias(stone_boulder, "boulder");
    }
#endif

    public readonly Block clay_boulder;
    public readonly Block crystal_boulder;
    public readonly Block gold_boulder;
    public readonly Block statue;
    public readonly Block trophy;
    public readonly Block stone_boulder;
    public readonly Block wooden_barrel;
  }
}
