using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  internal sealed class SPDGameList
  {
    public SPDGameList()
    {
      var Stocks = SPDDebug.codex.Stocks;
      var Items = SPDDebug.codex.Items;
      var Entities = SPDDebug.codex.Entities;
      var Features = SPDDebug.codex.Features;
      var Devices = SPDDebug.codex.Devices;
      var Materials = SPDDebug.codex.Materials;
      var Sanctities = SPDDebug.codex.Sanctities;

      this.forbidden = Items.List.Where(item => item.Type == ItemType.Gem || item.Type == ItemType.Food || item.Type == ItemType.Coin || item.Type == ItemType.Tin || item.Type == ItemType.Egg).Union(new[]
      {
        Items.wand_of_polymorph,
        Items.ring_of_polymorph,
        Items.ring_of_polymorph_control,
        Items.book_of_polymorph,
        Items.amulet_of_change,
        Items.potion_of_polymorph,
        Items.porter,
        Items.wand_of_summoning,
        Items.wand_of_create_horde,
        Items.book_of_summoning,
        Items.scroll_of_punishment,
        Items.scroll_of_light,
        Items.scroll_of_ice,
        Items.scroll_of_destruction,
        Items.scroll_of_fire,
        Items.scroll_of_gold_detection,
        Items.scroll_of_enlightenment,
        Items.scroll_of_amnesia,
        Items.scroll_of_food_detection,
        Items.scroll_of_water,
        Items.scroll_of_summoning,
        Items.potion_of_rage,
        Items.potion_of_ESP,
        Items.potion_of_affliction,
        Items.potion_of_fruit_juice,
        Items.potion_of_hallucination,
        Items.potion_of_gain_ability,
        Items.potion_of_amnesia
      }).ToHashSetX();

      this.uniques = new[]
      {
        Entities.Archmage_Dirachi,
        Entities.Archmage_Flaynn,
        Entities.Master_Kaen,
        Entities.Assassin_Mortimer,
        Entities.Ashikaga_Takauji,
        Entities.Girtab,
        Entities.Lolth,
        Entities.Nalzok,
        Entities.Archpriest_Avvakrum,
        Entities.Twoflower,
        Entities.Vecna,
        Entities.Deliarne,
        Entities.Guru_Quilion,
        Entities.Yeenoghu,
      };

      this.devices = Devices.List.Where(D => D.Frequency > 0).ToArray();

      this.realGems = Items.List.Where(i => i.Type == ItemType.Gem).Where(i => !i.Grade.Unique && i.Material == Materials.gemstone).ToArray();

      this.tools = Items.List
        .Where(i => i.Type == ItemType.Device || i.Type == ItemType.Eyewear || i.Type == ItemType.Instrument || i.Type == ItemType.Light || i.Type == ItemType.SkeletonKey || i.Type == ItemType.Tool)
        .Where(i => !i.Grade.Unique && i.Rarity > 0 && i != Items.porter).ToArray();

      this.thrownWeapons = Items.List.Where(i => i.Type == ItemType.ThrownWeapon).Where(i => !i.Grade.Unique && !i.IsAbolitionCandidate()).ToArray();

      this.thrownFirearms = new[]
      {
        Items.stick_of_dynamite,
        Items.rocket,
        Items.frag_grenade,
        Items.gas_grenade
      };

      this.goodWeapon = Items.List.Where(I => I.IsWeapon() && !I.Grade.Unique && I.Rarity > 0 && (I.Material == Materials.adamantine || I.Material == Materials.mithril || I.Material == Materials.silver || I.Material == Materials.bone || I.Material == Materials.crystal))
        .Union(new[]
      {
        Items.magic_horseshoe,
        Items.trident,
        Items.tsurugi,
        Items.katana,
        Items.wakizashi,
        Items.yumi
      }).ToArray();

      this.randomArmour = Items.List.Where(I =>
          (I.Type == ItemType.Boots || I.Type == ItemType.Cloak ||
          I.Type == ItemType.Gloves || I.Type == ItemType.Helmet || I.Type == ItemType.Shield ||
          I.Type == ItemType.Shirt || I.Type == ItemType.Suit) && !I.Grade.Unique && I.Rarity > 0).ToArray();

      this.amulets = Items.List.Where(
          I => I.Type == ItemType.Amulet && !I.Grade.Unique && I.Rarity > 0 && I != Items.amulet_of_change).ToArray();

      this.randomWeapon = Items.List.Where(
          I => (I.Type == ItemType.MeleeWeapon || I.Type == ItemType.ReachWeapon || I.Type == ItemType.RangedWeapon) && !I.Grade.Unique && I.Rarity > 0).ToArray();

      this.books = Items.List.Where(I => I.Type == ItemType.Book && !I.Grade.Unique && I.Rarity > 0 && !ItemForbidden(I)).ToArray();

      this.potions = Stocks.potion.Items.Where(I => !I.Grade.Unique && I.Rarity > 0 && !ItemForbidden(I)).ToProbability(I => I.Rarity);

      this.scrolls = Stocks.scroll.Items.Where(I => !I.Grade.Unique && I.Rarity > 0 && !ItemForbidden(I)).ToProbability(I => I.Rarity);

      this.leveledRing = Items.List.Where(I => I.Type == ItemType.Ring && !I.Grade.Unique && I.Rarity > 0 && I.HasEnchantment() && !ItemForbidden(I)).ToArray();

      this.unleveledRing = Items.List.Where(I => I.Type == ItemType.Ring && !I.Grade.Unique && I.Rarity > 0 && !I.HasEnchantment() && !ItemForbidden(I)).ToArray();
      
      this.wands = Items.List.Where(I => I.Type == ItemType.Wand && !I.Grade.Unique && I.Rarity > 0 && !ItemForbidden(I)).ToArray();

      this.randRing = Items.List.Where(I => I.Type == ItemType.Ring && !I.Grade.Unique && I.Rarity > 0 && !ItemForbidden(I)).ToArray();

      this.goodArmour = new[]
      {
        Items.adamantine_plate_mail,
        Items.mithril_barding,
        Items.mithril_shield,
        Items.mithril_plate_mail,
        Items.mithril_helmet,
        Items.dark_elven_mithrilcoat,
        Items.dwarvish_mithrilcoat,
        Items.elven_mithrilcoat,
        Items.hawaiian_shirt,
        Items.shield_of_reflection,
        Items.gauntlets_of_phasing,
        Items.battle_robe,
        Items.elemental_robe,
        Items.fleet_robe,
      }
        .Union(Items.List.Where(I => I.Type == ItemType.Cloak && !I.Grade.Unique && I.Equip.HasEffects() && I.DefaultSanctity != Sanctities.Cursed))
        .Union(Items.List.Where(I => I.Type == ItemType.Boots && !I.Grade.Unique && I.Equip.HasEffects() && I.DefaultSanctity != Sanctities.Cursed))
        .Union(Items.List.Where(I => I.Type == ItemType.Gloves && !I.Grade.Unique && I.Equip.HasEffects() && I.DefaultSanctity != Sanctities.Cursed))
        .Union(Items.List.Where(I => I.Type == ItemType.Helmet && !I.Grade.Unique && I.Equip.HasEffects() && I.DefaultSanctity != Sanctities.Cursed)).ToArray();

      this.magicFood = new[]
      {
        Items.apple,
        Items.banana,
        Items.candy_bar,
        Items.carrot,
        Items.cheese,
        Items.clove_of_garlic,
        Items.cream_pie,
        Items.eucalyptus_leaf,
        Items.fish,
        Items.fortune_cookie,
        Items.holy_wafer,
        Items.lump_of_royal_jelly,
        Items.melon,
        Items.mushroom,
        Items.orange,
        Items.pear,
        Items.sprig_of_wolfsbane
      };

      this.features = new[]
      {
        Features.altar,
        Features.bed,
        Features.fountain,
        Features.grave,
        Features.pentagram,
        Features.sarcophagus
      };
    }

    public Entity RandomUniqueBoss()
    {
      return uniques.GetRandom();
    }
    public Asset RandomAsset(Square Square)
    {
      Asset asset;
      do
      {
        asset = SPDDebug.generator.NewRandomAsset(Square, Stock: null);
      } while (asset != null && ItemForbidden(asset.Item));
      return asset;
    }
    public Device RandomDevice()
    {
      return devices.GetRandom();
    }
    public Item RealGem()
    {
      return realGems.GetRandom();
    }
    public Asset ZooChest(Square Square)
    {
      var chest = SPDDebug.generator.NewSpecificAsset(Square, SPDDebug.codex.Items.chest);

      void StoreAsset(Item chestItem) => chest.Container.Stash.Add(SPDDebug.generator.NewSpecificAsset(Square, chestItem));

      switch (SPDRandom.Int(4))
      {
        case 0: StoreAsset(RandomWand()); break;
        case 1:
          for (var i = 0; i < 2; i++)
          {
            if (SPDRandom.Int(2) == 0)
              StoreAsset(RandomPotion());
            else
              StoreAsset(RandomScroll());
          }
          break;
        case 2: StoreAsset(RandomToolOrMaybeArtifact()); break;
        case 3: StoreAsset(RandomBook()); break;
        default: StoreAsset(RandomToolOrMaybeArtifact()); break;
      }

      return chest;
    }
    public Item RandomToolOrMaybeArtifact()
    {
      return MaybeArtifact(ArtifactChance: 30) ?? tools.GetRandom();
    }
    public Item RandomTool()
    {
      return tools.GetRandom();
    }
    public Item ThrownWeapon()
    {
      return thrownWeapons.GetRandom();
    }
    public Item ThrownFirearm()
    {
      return thrownFirearms.GetRandom();
    }
    public int GoldCoinQuantity()
    {
      return SPDRandom.Int(30 + SPDDebug.currentmap.depth * 7, 60 + SPDDebug.currentmap.depth * 15);
    }
    public int HalfGoldQuantity()
    {
      return SPDRandom.Int(15 + SPDDebug.currentmap.depth * 4, 30 + SPDDebug.currentmap.depth * 8);
    }
    public Item GoodWeapon()
    {
      return MaybeArtifact(ArtifactChance: 30) ?? goodWeapon.GetRandom();
    }
    public Item RandomBook()
    {
      return books.GetRandom();
    }
    public Item RandomWeapon()
    {
      return MaybeArtifact(ArtifactChance: 50) ?? randomWeapon.GetRandom();
    }
    public Asset PitChest(Square Square)
    {
      var Chest = SPDDebug.generator.NewSpecificAsset(Square, SPDDebug.codex.Items.chest);
      Item MainPrize;
      var n = SPDRandom.IntRange(2, 3);
      for (var i = 0; i < n; i++)
      {
        switch (SPDRandom.Int(6))
        {
          case 0: MainPrize = LeveledRing(); break;
          case 1: MainPrize = RandomWand(); break;
          case 2: MainPrize = UnleveledRing(); break;
          case 3: MainPrize = GoodArmour(); break;
          case 4: MainPrize = RandomAmulet(); break;
          case 5: MainPrize = RandomBook(); break;
          default: MainPrize = RandomArmour(); break;
        }
        Chest.Container.Stash.Add(SPDDebug.generator.NewSpecificAsset(Square, MainPrize));
      }
      return Chest;
    }
    public Asset PoolPrize(Square Square)
    {
      Item prize;
      if (SPDRandom.Int(2) == 0)
      {
        do
        {
          if (SPDRandom.Int(3) == 0) prize = GoodWeapon();
          else prize = RandomWeapon();
        } while (prize.DefaultSanctity == SPDDebug.codex.Sanctities.Cursed);
      }
      else
      {
        do
        {
          if (SPDRandom.Int(3) == 0) prize = GoodArmour();
          else prize = RandomArmour();
        } while (prize.DefaultSanctity == SPDDebug.codex.Sanctities.Cursed);
      }
      var Prize = SPDDebug.generator.NewSpecificAsset(Square, prize);
      if (Prize.CanUpgrade())
      {
        var dice = SPDRandom.Int(10);
        if (dice < 3) SPDDebug.generator.ChangeEnchantment(Prize, Modifier.Plus1);
        else if (dice > 6) SPDDebug.generator.ChangeEnchantment(Prize, Modifier.Plus3);
        else SPDDebug.generator.ChangeEnchantment(Prize, Modifier.Plus2);
      }
      SPDDebug.generator.ChangeSanctity(Prize, SPDDebug.codex.Sanctities.Blessed);
      return Prize;
    }
    public Item RandomAmulet()
    {
      return MaybeArtifact(ArtifactChance: 50) ?? amulets.GetRandom();
    }
    public Item UnleveledRing()
    {
      return unleveledRing.GetRandom();
    }
    public Item RandomWand()
    {
      return MaybeArtifact(ArtifactChance: 30) ?? wands.GetRandom();
    }
    public Item LeveledRing()
    {
      return leveledRing.GetRandom();
    }
    public Item RandomRing()
    {
      return randRing.GetRandom();
    }
    public Item MagicFood()
    {
      return magicFood.GetRandom();
    }
    public Item RandomPotion()
    {
      return potions.GetRandom();
    }
    public Item RandomScroll()
    {
      return scrolls.GetRandom();
    }
    public Item RandomArmour()
    {
      return MaybeArtifact(ArtifactChance: 50) ?? randomArmour.GetRandom();
    }
    public Item GoodArmour()
    {
      return MaybeArtifact(ArtifactChance: 50) ?? goodArmour.GetRandom();
    }
    public Feature RandomFeature()
    {
      return features.GetRandom();
    }

    private bool ItemForbidden(Item item)
    {
      return forbidden.Contains(item);
    }
    private Item MaybeArtifact(int ArtifactChance)
    {
      return SPDRandom.Int(ArtifactChance) == 0 ? SPDDebug.generator.RandomUniqueItem(I => !ItemForbidden(I)) : null;
    }

    private readonly IReadOnlyList<Item> randomArmour;
    private readonly IReadOnlyList<Item> goodArmour;
    private readonly IReadOnlyList<Item> magicFood;
    private readonly IReadOnlyList<Item> leveledRing;
    private readonly IReadOnlyList<Item> unleveledRing;
    private readonly IReadOnlyList<Item> wands;
    private readonly IReadOnlyList<Item> amulets;
    private readonly IReadOnlyList<Item> randRing;
    private readonly IReadOnlyList<Item> randomWeapon;
    private readonly IReadOnlyList<Item> books;
    private readonly IReadOnlyList<Item> goodWeapon;
    private readonly IReadOnlyList<Item> thrownFirearms;
    private readonly IReadOnlyList<Item> thrownWeapons;
    private readonly IReadOnlyList<Item> tools;
    private readonly IReadOnlyList<Item> realGems;
    private readonly IReadOnlyList<Device> devices;
    private readonly IReadOnlyList<Entity> uniques;
    private readonly IReadOnlyList<Feature> features;
    private readonly Probability<Item> potions;
    private readonly Probability<Item> scrolls;
    private readonly HashSet<Item> forbidden;
  }
}