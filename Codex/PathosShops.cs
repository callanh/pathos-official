using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexShops : CodexPage<ManifestShops, ShopEditor, Shop>
  {
    private CodexShops() { }
#if MASTER_CODEX
    internal CodexShops(Codex Codex)
      : base(Codex.Manifest.Shops)
    {
      var Features = Codex.Features;
      var Stocks = Codex.Stocks;
      var Items = Codex.Items;
      var Entities = Codex.Entities;
      var Services = Codex.Services;
      var Glyphs = Codex.Glyphs;

      var MerchantNameArray = new string[]
      {
        "Alista", "Arnoken", "Arrom",
        "Bartist", "Brouwer", "Bason",
        "Carthyre", "Crepsal", "Choung",
        "Davicohr", "Donneller", "Daracy",
        "Elishmeli", "Ericendrick", "Enlan",
        "Fenraymond", "Fenlas", "Forro",
        "Gleichman", "Galday", "Gohu",
        "Hough", "Hendrick", "Haelom",
        "Isylav", "Izamiller", "Istophe",
        "Javarl", "Jayenason", "Jonat",
        "Kennold", "Kevugo", "Kneller",
        "Lanetwalz", "Luickean", "Lzay",
        "Marisa", "Misteph", "Miller",
        "Nichaeltoy", "Ninhart", "Nodin",
        "Okennwood", "Orankin", "Omdarro",
        "Plegg", "Pauwinner", "Pleyson",
        "Quohnrupley", "Quizchak", "Quamma",
        "Raugarr", "Renorber", "Rupley",
        "Stevere", "Stewite", "Smither",
        "Thakuline", "Timonen", "Threepo",
        "Ujopayne", "Urenval", "Udfoot",
        "Vikethome", "Vekincarc", "Vidar",
        "Wollet", "Wallison", "Walzen",
        "Xandries", "Xarrung", "Xank",
        "Yarromdee", "Yuvaloren", "Yral",
        "Zonagan", "Zerbarry", "Zean"
      };

      Shop AddShop(string Name, Glyph Glyph, int Rarity, Func<IEnumerable<Stock>> BuyStockFunc, Func<IEnumerable<Item>> SellItemFunc)
      {
        return Register.Add(S =>
        {
          S.Name = Name;
          S.Glyph = Glyph;
          S.Sonic = Codex.Sonics.chime;
          S.Rarity = Rarity;
          S.KeeperEntity = Entities.merchant;
          S.SetServantEntities(new[] { Entities.small_mimic, Entities.large_mimic, Entities.giant_mimic });
          S.KeeperFeature = Features.stall;
          S.SetKeeperNames(MerchantNameArray);
          S.SetServices(Services.List.ToArray());

          CodexRecruiter.Enrol(() =>
          {
            S.SetBuyStocks(BuyStockFunc().ToArray());
            S.SetSellItems(SellItemFunc().Where(I => !I.Artifact).ToArray());
          });
        });
      }

      // all shops see the value in gems, and will buy/trade them from you.

      //local_butcher = AddShop("local butcher", Glyphs.food_stock, 5,
      //  () => new[] { Stocks.food, Stocks.gem },
      //  () => Stocks.food.ItemList.Where(I => I.Rarity > 0 && I.Material == Material.Animal).Union(new Item[] { Items.animal_corpse }));

      general_store = AddShop("general store", Glyphs.general_stock, Rarity: 44,
        () => Stocks.List,
        () => Items.List.Where(I => I.Rarity > 0 || (!I.Artifact && I.IsAbolitionCandidate())));

      gun_mart = AddShop("gun mart", Glyphs.firearm_stock, Rarity: 2,
        () => new[] { Stocks.weapon, Stocks.gem },
        () => Stocks.weapon.Items.Where(I => I.IsAbolitionCandidate()));

      used_armour_dealership = AddShop("used armour dealership", Glyphs.armour_stock, Rarity: 14,
        () => new[] { Stocks.armour, Stocks.gem },
        () => Stocks.armour.Items.Where(I => I.Rarity > 0));

      secondhand_stationery_store = AddShop("second-hand stationery store", Glyphs.scroll_stock, Rarity: 10,
        () => new[] { Stocks.scroll, Stocks.book, Stocks.gem },
        () => Stocks.scroll.Items.Where(I => I.Rarity > 0)/*.Union(new Item[] { Items.potion_of_ink })*/);

      liquor_emporium = AddShop("liquor emporium", Glyphs.potion_stock, Rarity: 10,
        () => new[] { Stocks.potion, Stocks.gem },
        () => Stocks.potion.Items.Where(I => I.Rarity > 0));

      antique_weapons_outlet = AddShop("antique weapons outlet", Glyphs.weapon_stock, Rarity: 5,
        () => new[] { Stocks.weapon, Stocks.gem },
        () => Stocks.weapon.Items.Where(I => I.Rarity > 0 && !I.IsAbolitionCandidate()));

      delicatessen = AddShop("delicatessen", Glyphs.food_stock, Rarity: 5,
        () => new[] { Stocks.food, Stocks.gem },
        () => Stocks.food.Items.Where(I => I.Rarity > 0).Union(new Item[] { Items.potion_of_water, Items.potion_of_booze, Items.potion_of_fruit_juice, Items.ice_box, Items.horn_of_plenty }));

      jewellers = AddShop("jewellers", Glyphs.ring_stock, Rarity: 3,
        () => new[] { Stocks.ring, Stocks.amulet, Stocks.gem },
        () => Stocks.ring.Items.Union(Stocks.amulet.Items).Union(Stocks.gem.Items).Where(I => I.Rarity > 0));

      quality_apparel_and_accessories = AddShop("quality apparel and accessories", Glyphs.wand_stock, Rarity: 3,
        () => new[] { Stocks.wand, Stocks.gem },
        () => Stocks.wand.Items.Where(I => I.Rarity > 0).Union(new[] { Items.leather_gloves, Items.elven_cloak }));

      hardware_store = AddShop("hardware store", Glyphs.tool_stock, Rarity: 3,
        () => new[] { Stocks.tool, Stocks.gem },
        () => Stocks.tool.Items.Where(I => I.Rarity > 0));

      rare_books = AddShop("rare books", Glyphs.book_stock, Rarity: 3,
        () => new[] { Stocks.book, Stocks.gem },
        () => Stocks.book.Items.Where(I => I.Rarity > 0));

      lighting_store = AddShop("lighting store", Glyphs.light_stock, Rarity: 3,
        () => new[] { Stocks.tool, Stocks.gem },
        () => new[] { Items.oil_lamp, Items.magic_lamp, Items.lantern, Items.torch, Items.magic_candle, Items.wax_candle, Items.potion_of_oil, Items.wand_of_light });
    }
#endif

    public readonly Shop general_store;
    //public readonly Shop local_butcher;
    public readonly Shop gun_mart;
    public readonly Shop used_armour_dealership;
    public readonly Shop secondhand_stationery_store;
    public readonly Shop liquor_emporium;
    public readonly Shop antique_weapons_outlet;
    public readonly Shop delicatessen;
    public readonly Shop jewellers;
    public readonly Shop quality_apparel_and_accessories;
    public readonly Shop hardware_store;
    public readonly Shop rare_books;
    public readonly Shop lighting_store;
  }
}