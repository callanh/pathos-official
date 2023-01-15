using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexStocks : CodexPage<ManifestStocks, StockEditor, Stock>
  {
    private CodexStocks() { }
#if MASTER_CODEX
    internal CodexStocks(Codex Codex)
      : base(Codex.Manifest.Stocks)
    {
      var Glyphs = Codex.Glyphs;

      Stock AddStock(string Name, Glyph Glyph, int Rarity)
      {
        return Register.Add(S =>
        {
          S.Name = Name;
          S.Glyph = Glyph;
          S.Rarity = Rarity;
        });
      }

      amulet = AddStock("amulet", Glyphs.amulet_stock, 1);
      armour = AddStock("armour", Glyphs.armour_stock, 10);
      book = AddStock("book", Glyphs.book_stock, 4);
      food = AddStock("food", Glyphs.food_stock, 20);
      gem = AddStock("gem", Glyphs.gem_stock, 8);
      potion = AddStock("potion", Glyphs.potion_stock, 16);
      ring = AddStock("ring", Glyphs.ring_stock, 3);
      scroll = AddStock("scroll", Glyphs.scroll_stock, 16);
      tool = AddStock("tool", Glyphs.tool_stock, 8);
      wand = AddStock("wand", Glyphs.wand_stock, 4);
      weapon = AddStock("weapon", Glyphs.weapon_stock, 10);

      Debug.Assert(Register.List.Sum(S => S.Rarity) == 100, "Stock rarity is expected to equal 100.");
      //foreach (var Stock in Register.List) Debug.WriteLine($"{Stock.Name}={Stock.Rarity}%");
    }
#endif

    public readonly Stock amulet;
    public readonly Stock armour;
    public readonly Stock book;
    public readonly Stock food;
    public readonly Stock gem;
    public readonly Stock potion;
    public readonly Stock ring;
    public readonly Stock scroll;
    public readonly Stock tool;
    public readonly Stock wand;
    public readonly Stock weapon;
  }
}
