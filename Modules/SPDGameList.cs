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
            var Items = SPDDebug.codex.Items;
            var Entities = SPDDebug.codex.Entities;
            var Features = SPDDebug.codex.Features;

            this.forbidden = Items.List.Where(item => item.Type == ItemType.Gem || item.Type == ItemType.Food || item.Type == ItemType.Coin || item.Type == ItemType.Tin || item.Type == ItemType.Egg).Union(new []
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
        
            this.uniques = new Inv.DistinctList<Entity>()
            {
              Entities.Archmage_Dirachi,
              Entities.Archmage_Flaynn,
              Entities.Dark_Lord,
              Entities.Master_Kaen,
              Entities.Assassin_Mortimer,
              Entities.Ashikaga_Takauji,
              Entities.Girtab,
              Entities.Lolth,
              Entities.Nalzok,
              Entities.Archpriest_Avvakrum,
              Entities.Twoflower,
              Entities.Vecna,
              Entities.Dark_One,
              Entities.Guru_Quilion,
              Entities.Yeenoghu,
            };

            this.devices = SPDDebug.codex.Devices.List.ToDistinctList();

            this.realGems = SPDDebug.codex.Items.List.Where(i => i.Type == ItemType.Gem).Where(i => !i.Artifact && i.Material == SPDDebug.codex.Materials.gemstone).ToDistinctList();

            this.tools = SPDDebug.codex.Items.List
              .Where(i => i.Type == ItemType.Device || i.Type == ItemType.Eyewear || i.Type == ItemType.Instrument || i.Type == ItemType.Light || i.Type == ItemType.SkeletonKey || i.Type == ItemType.Tool)
              .Where(i => !i.Artifact && i.Rarity > 0 && i != SPDDebug.codex.Items.porter).ToDistinctList();

            this.thrownWeapons = SPDDebug.codex.Items.List.Where(i => i.Type == ItemType.ThrownWeapon).Where(i => !i.Artifact && !i.IsAbolitionCandidate()).ToDistinctList();

            this.thrownFirearms = new Inv.DistinctList<Item>()
            {
                Items.stick_of_dynamite,
                Items.rocket,
                Items.frag_grenade,
                Items.gas_grenade
            };

            this.goodWeapon = new Inv.DistinctList<Item>()
            {
                Items.mithril_battleaxe,
                Items.mithril_katar,
                Items.mithril_long_sword,
                Items.mithril_sabre,
                Items.mithril_short_sword,
                Items.mithril_twohanded_sword,
                Items.mithril_arrow,
                Items.mithril_crossbow_bolt,
                Items.yumi,
                Items.mithril_lance,
                Items.magic_horseshoe,
                Items.mithril_dagger,
                Items.flash_staff,
                Items.silver_heavy_hammer,
                Items.silver_long_sword,
                Items.silver_mace,
                Items.silver_short_sword,
                Items.silver_twohanded_sword,
                Items.tsurugi,
                Items.silver_arrow,
                Items.silver_crossbow_bolt,
                Items.mithril_whip,
                Items.silver_lance,
                Items.silver_spear,
                Items.trident,
                Items.silver_dagger,
                Items.dread_staff,
                Items.katana,
                Items.thunder_staff,
                Items.wakizashi
            };

            this.randomArmour = SPDDebug.codex.Items.List.Where(armour =>
                (armour.Type == ItemType.Boots || armour.Type == ItemType.Cloak ||
                armour.Type == ItemType.Gloves || armour.Type == ItemType.Helmet || armour.Type == ItemType.Shield ||
                armour.Type == ItemType.Shirt || armour.Type == ItemType.Suit) && !armour.Artifact && armour.Rarity > 0).ToDistinctList();

            this.amulets = SPDDebug.codex.Items.List.Where(
                amulet => amulet.Type == ItemType.Amulet && !amulet.Artifact && amulet.Rarity > 0 && amulet != SPDDebug.codex.Items.amulet_of_change).ToDistinctList();

            this.randomWeapon = SPDDebug.codex.Items.List.Where(
                weapon => (weapon.Type == ItemType.MeleeWeapon || weapon.Type == ItemType.ReachWeapon || weapon.Type == ItemType.RangedWeapon) && !weapon.Artifact && weapon.Rarity > 0).ToDistinctList();

            this.books = SPDDebug.codex.Items.List.Where(book => book.Type == ItemType.Book && !ItemForbidden(book)).ToDistinctList();

            this.potions = SPDDebug.codex.Stocks.potion.Items.Where(I => !I.Artifact && !ItemForbidden(I) && I.Rarity > 0).ToProbability(I => I.Rarity);
            
            this.scrolls = SPDDebug.codex.Stocks.scroll.Items.Where(I => !I.Artifact && !ItemForbidden(I) && I.Rarity > 0).ToProbability(I => I.Rarity);

            this.unleveledRing = new Inv.DistinctList<Item>()
            {
                Items.ring_of_aggravation,
                Items.ring_of_berserking,
                Items.ring_of_cold_resistance,
                Items.ring_of_conflict,
                Items.ring_of_fire_resistance,
                Items.ring_of_free_action,
                Items.ring_of_hunger,
                Items.ring_of_levitation,
                Items.ring_of_naught,
                Items.ring_of_poison_resistance,
                Items.ring_of_regeneration,
                Items.ring_of_searching,
                Items.ring_of_see_invisible,
                Items.ring_of_shock_resistance,
                Items.ring_of_sleeping,
                Items.ring_of_slow_digestion,
                Items.ring_of_stealth,
                Items.ring_of_sustain_ability,
                Items.ring_of_teleport_control,
                Items.ring_of_teleportation,
                Items.ring_of_warning
            };

            this.wands = SPDDebug.codex.Items.List.Where(
              wand => wand.Type == ItemType.Wand && !wand.Artifact && wand.Rarity > 0 && !ItemForbidden(wand)).ToDistinctList();

            this.randRing = SPDDebug.codex.Items.List.Where(
              ring => ring.Type == ItemType.Ring && !ring.Artifact && ring.Rarity > 0 && ring != SPDDebug.codex.Items.ring_of_polymorph && ring != SPDDebug.codex.Items.ring_of_polymorph_control).ToDistinctList();

            this.leveledRing = new Inv.DistinctList<Item>()
            {
                Items.ring_of_accuracy,
                Items.ring_of_adornment,
                Items.ring_of_constitution,
                Items.ring_of_dexterity,
                Items.ring_of_impact,
                Items.ring_of_intelligence,
                Items.ring_of_protection,
                Items.ring_of_strength,
                Items.ring_of_wisdom,
            };

            this.goodArmour = new Inv.DistinctList<Item>()
            { 
                Items.mithril_barding,
                Items.mithril_shield,
                Items.dark_elven_mithrilcoat,
                Items.dwarvish_mithrilcoat,
                Items.elven_mithrilcoat,
                Items.hawaiian_shirt,
                Items.shield_of_reflection,
                Items.gauntlets_of_phasing,
                Items.battle_robe,
                Items.elemental_robe,
                Items.fleet_robe,
            };
            goodArmour.AddRange(Items.List.Where(
                    Armour => Armour.Type == ItemType.Cloak).Where(Armour => !Armour.Artifact && Armour.Equip.HasEffects() && Armour.DefaultSanctity != SPDDebug.codex.Sanctities.Cursed));
            goodArmour.AddRange(Items.List.Where(
                    Armour => Armour.Type == ItemType.Boots).Where(Armour => !Armour.Artifact && Armour.Equip.HasEffects() && Armour.DefaultSanctity != SPDDebug.codex.Sanctities.Cursed));
            goodArmour.AddRange(Items.List.Where(
                    Armour => Armour.Type == ItemType.Gloves).Where(Armour => !Armour.Artifact && Armour.Equip.HasEffects() && Armour.DefaultSanctity != SPDDebug.codex.Sanctities.Cursed));
            goodArmour.AddRange(Items.List.Where(
                    Armour => Armour.Type == ItemType.Helmet).Where(Armour => !Armour.Artifact && Armour.Equip.HasEffects() && Armour.DefaultSanctity != SPDDebug.codex.Sanctities.Cursed));

            this.magicFood = new Inv.DistinctList<Item>()
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

            this.features = new Inv.DistinctList<Feature>()
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
            } while (asset == null || ItemForbidden(asset.Item));
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
                case 2: StoreAsset(RandomTool()); break;
                case 3: StoreAsset(RandomBook()); break;
                default: StoreAsset(RandomTool()); break;
            }

            return chest;
        }
        public Item RandomTool()
        {
            return MaybeArtifact(ArtifactChance: 30) ?? tools.GetRandom();
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
            } else
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
                Prize.SetEnchantment(Modifier.Zero);
                var dice = SPDRandom.Int(10);
                if (dice < 3) Prize.SetEnchantment(Modifier.Plus1);
                else if (dice > 6) Prize.SetEnchantment(Modifier.Plus3);
                else Prize.SetEnchantment(Modifier.Plus2);
            }
            Prize.SetSanctity(SPDDebug.codex.Sanctities.Blessed);
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
            return SPDRandom.Int(ArtifactChance) == 0 ? SPDDebug.generator.GetArtifactItem(I => !ItemForbidden(I)) : null;
        }
        
        private readonly Inv.DistinctList<Item> randomArmour;
        private readonly Inv.DistinctList<Item> goodArmour;
        private readonly Inv.DistinctList<Item> magicFood;
        private readonly Inv.DistinctList<Item> leveledRing;
        private readonly Inv.DistinctList<Item> unleveledRing;
        private readonly Inv.DistinctList<Item> wands;
        private readonly Inv.DistinctList<Item> amulets;
        private readonly Inv.DistinctList<Item> randRing;
        private readonly Inv.DistinctList<Item> randomWeapon;
        private readonly Inv.DistinctList<Item> books;
        private readonly Inv.DistinctList<Item> goodWeapon;
        private readonly Inv.DistinctList<Item> thrownFirearms;
        private readonly Inv.DistinctList<Item> thrownWeapons;
        private readonly Inv.DistinctList<Item> tools;
        private readonly Inv.DistinctList<Item> realGems;
        private readonly Inv.DistinctList<Device> devices;
        private readonly Inv.DistinctList<Entity> uniques;
        private readonly Inv.DistinctList<Feature> features;
        private readonly Probability<Item> potions;
        private readonly Probability<Item> scrolls;
        private readonly HashSet<Item> forbidden;
    }
}