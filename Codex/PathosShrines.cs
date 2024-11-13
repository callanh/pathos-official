using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexShrines : CodexPage<ManifestShrines, ShrineEditor, Shrine>
  {
    private CodexShrines() { }
#if MASTER_CODEX
    internal CodexShrines(Codex Codex)
      : base(Codex.Manifest.Shrines)
    {
      var Genders = Codex.Genders;
      var Entities = Codex.Entities;
      var Features = Codex.Features;
      var Items = Codex.Items;
      var Strikes = Codex.Strikes;
      var Sanctities = Codex.Sanctities;
      var Elements = Codex.Elements;
      var Glyphs = Codex.Glyphs;
      var Sonics = Codex.Sonics;
      var Skills = Codex.Skills;
      var Attributes = Codex.Attributes;

      var MaleNameArray = new string[]
      {
        "Atral",
        "Bolan",
        "Cazedda",
        "Damano",
        "Ellent",
        "Faraze",
        "Gomack",
        "Haquian",
        "Ishgood",
        "Jeem",
        "Karbri",
        "Lant",
        "Melchai",
        "Norbant",
        "Ollac",
        "Paltrick",
        "Quibb",
        "Rambrant",
        "Sully",
        "Trank",
        "Uffle",
        "Vorzian",
        "Waltis",
        "Xamarin",
        "Yassage",
        "Zimn"
      };

      var FemaleNameArray = new string[]
      {
        "Atrya",
        "Belietha",
        "Cailie",
        "D'rey",
        "Emina",
        "Franzia",
        "Gmima",
        "Haique",
        "Illitasha",
        "Jessera",
        "Kierantha",
        "Lesha",
        "Meloty",
        "Ninerma",
        "Oscae",
        "Penevieve",
        "Quarah",
        "Rebenna",
        "Sarlia",
        "Tanyacka",
        "Umazi",
        "Viophie",
        "Wendry",
        "Xiolet",
        "Yorque",
        "Zultry"
      };

      Shrine AddShrine(string Name, Entity ShrineEntity, Glyph Glyph, Sonic Sonic, int Rarity, Action<ShrineEditor> Action)
      {
        return Register.Add(S =>
        {
          S.Name = Name;
          S.Glyph = Glyph;
          S.Sonic = Sonic;
          S.Rarity = Rarity;
          S.KeeperEntity = ShrineEntity;

          CodexRecruiter.Enrol(() =>
          {
            S.SetKeeperNames(ShrineEntity.Genders.Count == 1 && ShrineEntity.Genders[0] == Genders.female ? FemaleNameArray : MaleNameArray);

            Action(S);
          });
        });
      }

      holy_shrine = AddShrine("holy shrine", Entities.holy_cleric, Glyphs.holy_shrine, Sonics.bell, 30, S =>
      {
        S.KeeperFeature = Features.altar;

        S.AddBoon("divine", B =>
        {
          B.Description = "Learn the divine status of all carried items.";
          B.Cost = 50;
          B.SetCast().Strike(Strikes.magic, Dice.Zero);
          B.Apply.Divine();
        });

        S.AddBoon("rejuvenate", B =>
        {
          B.Description = "Heal damage and recover mana for yourself and your steed if mounted.";
          B.Cost = 100;
          B.SetCast().Strike(Strikes.spirit, Dice.One);
          B.Apply.Heal(Dice.Fixed(100), Modifier.Zero);
          B.Apply.Energise(Dice.Fixed(100), Modifier.Zero);
        });

        S.AddBoon("remove curse", B =>
        {
          B.Description = "Remove the curse on one equipped or carried item.";
          B.Cost = 200;
          B.SetCast().FilterSanctity(Sanctities.Cursed);
          B.Apply.RemoveCurse(Dice.One);
        });

        S.AddBoon("bless", B =>
        {
          B.Description = "Bless one item in your inventory or on the ground.";
          B.Cost = 250;
          B.SetCast().FilterSanctity(Sanctities.Uncursed);
          B.Apply.Sanctify(Item: null, Sanctities.Blessed);
        });

        S.AddBoon("purify", B =>
        {
          B.Description = "Restore any lost ability and remove any negative transient conditions.";
          B.Cost = 150;
          B.SetCast().Strike(Strikes.spirit, Dice.One);
          B.Apply.Unafflict();
          B.Apply.Unpunish();
          B.Apply.Unpolymorph();
          B.Apply.RestoreAbility();
          B.Apply.RemoveTransient(Codex.Properties.List.Where(P => P.Unwanted).ToArray());
        });

        S.AddBoon("raise dead", B =>
        {
          B.Description = "Return a corpse back to life.";
          B.Cost = 750;
          B.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
          B.Apply.RaiseDead(Percent: 50, Corrupt: null, LoyalOnly: false);
        });
      });

      dark_sepulchre = AddShrine("dark sepulchre", Entities.dark_cleric, Glyphs.dark_sepulchre, Sonics.bell, 10, S =>
      {
        S.KeeperFeature = Features.grave; // or sarcophagus?

        //S.AddBoon("body disposal", B =>
        //{
        //  B.Description = "Discrete removal of your dearly departed and other unwanted corpses.";
        //  B.Cost = 50;
        //  B.Apply.CreateAsset(Dice.One, new[] { Items.animal_corpse, Items.vegetable_corpse });
        //});

        S.AddBoon("corpse wish", B =>
        {
          B.Description = "Request a fresh corpse with no questions asked.";
          B.Cost = 100;
          B.Apply.CreateAsset(Dice.One, QuantityDice: null, new[] { Items.animal_corpse, Items.vegetable_corpse });
        });

        S.AddBoon("tinned meat", B =>
        {
          B.Description = "Butcher a corpse into an easy to carry container.";
          B.Cost = 150;
          B.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
          B.Apply.Tinning(Codex.Items.tin);
        });

        S.AddBoon("animate dead", B =>
        {
          B.Description = "Reanimate a corpse as a loyal revenant.";
          B.Cost = 500;
          B.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
          B.Apply.AnimateRevenants(Corrupt: null);
        });

        S.AddBoon("undead army", B =>
        {
          B.Description = "Call upon the undead to fight for you.";
          B.Cost = 750;
          B.Apply.SummonEntity(1.d3(), Entities.human_zombie, Entities.elf_zombie, Entities.dwarf_zombie, Entities.orc_zombie, Entities.gnome_zombie);
          B.Apply.SummonEntity(1.d2(), Entities.ghoul);
          B.Apply.SummonEntity(Dice.One, Entities.human_mummy, Entities.elf_mummy, Entities.dwarf_mummy, Entities.orc_mummy, Entities.gnome_mummy);
        });

        S.AddBoon("nightmare steed", B =>
        {
          B.Description = "Summon a powerful steed from the underworld.";
          B.Cost = 1000;
          B.Apply.SummonEntity(Dice.Fixed(1), Entities.nightmare);
        });
      });

      sacred_grove = AddShrine("sacred grove", Entities.dryad, Glyphs.sacred_grove, Sonics.bell, 20, S =>
      {
        S.KeeperFeature = Features.fountain;

        S.AddBoon("erase", B =>
        {
          B.Description = "Erase the magic from a potion, scroll or book.";
          B.Cost = 50;
          B.SetCast().FilterStock(Codex.Stocks.potion, Codex.Stocks.scroll, Codex.Stocks.book);
          B.Apply.ConvertAsset(Codex.Stocks.potion, WholeStack: true, Items.potion_of_water);
          B.Apply.ConvertAsset(Codex.Stocks.scroll, WholeStack: true, Items.scroll_of_blank_paper);
          B.Apply.ConvertAsset(Codex.Stocks.book, WholeStack: true, Items.book_of_blank_paper);
        });

        S.AddBoon("hatch", B =>
        {
          B.Description = "Hatch an egg and become a parent.";
          B.Cost = 100;
          B.SetCast().FilterItem(Codex.Items.egg);
          B.Apply.Hatch();
        });

        S.AddBoon("grow", B =>
        {
          B.Description = "Use nature magic to grow the power of your ally.";
          B.Cost = 200;
          B.SetCast().Strike(Strikes.spirit, Dice.One);
          B.Apply.Growth();
        });

        S.AddBoon("bless", B =>
        {
          B.Description = "Bless one item in your inventory or on the ground.";
          B.Cost = 250;
          B.SetCast().FilterSanctity(Sanctities.Uncursed);
          B.Apply.Sanctify(Item: null, Sanctities.Blessed);
        });

        S.AddBoon("unicorn friend", B =>
        {
          B.Description = "Ask for a unicorn to join your party.";
          B.Cost = 500;
          B.Apply.SummonEntity(Dice.Fixed(1), Entities.white_unicorn, Entities.grey_unicorn, Entities.black_unicorn);
        });

        S.AddBoon("fantastical beast", B =>
        {
          B.Description = "Summon a tame forest beast as your ally.";
          B.Cost = 750;
          B.Apply.SummonEntity(Dice.Fixed(1),
            Entities.kirin,
            Entities.wyvern,
            Entities.wolverine,
            Entities.sasquatch,
            Entities.forest_centaur,
            Entities.displacer_beast,
            Entities.owlbear,
            Entities.minotaur,
            Entities.guardian_naga,
            Entities.ettin,
            Entities.basilisk);
        });
      });

      mystic_coven = AddShrine("mystic coven", Entities.witch, Glyphs.mystic_coven, Sonics.bell, 20, S =>
      {
        S.KeeperFeature = Features.pentagram;

        S.AddBoon("place curse", B =>
        {
          B.Description = "Place a curse on one equipped or carried item.";
          B.Cost = 50;
          B.SetCast().FilterSanctity(Sanctities.Uncursed, Sanctities.Blessed);
          B.Apply.PlaceCurse(Dice.One, Sanctities.Cursed);
        });

        S.AddBoon("scribe", B =>
        {
          B.Description = "Write a random scroll onto your blank paper.";
          B.Cost = 250;
          B.SetCast().FilterItem(Items.scroll_of_blank_paper)
           .SetAssetIndividualised();
          B.Apply.ConvertAsset(Codex.Stocks.scroll, WholeStack: false, Codex.Stocks.scroll.Items.Where(I => I != Items.scroll_of_blank_paper && !I.Grade.Unique && I.Rarity > 0).ToArray());
        });

        S.AddBoon("brew", B =>
        {
          B.Description = "Brew a random potion into your bottle of water.";
          B.Cost = 250;
          B.SetCast().FilterItem(Items.potion_of_water)
           .SetAssetIndividualised();
          B.Apply.ConvertAsset(Codex.Stocks.potion, WholeStack: false, Codex.Stocks.potion.Items.Where(I => I != Items.potion_of_water && !I.Grade.Unique && I.Rarity > 0).ToArray());
        });

        S.AddBoon("polymorph", B =>
        {
          B.Description = "Request one cast of the polymorph spell.";
          B.Cost = 500;
          B.SetCast().Plain(Dice.One)
           .SetObjects(false);
          B.Apply.PolymorphEntity();
        });

        S.AddBoon("teach spell", B =>
        {
          B.Description = "Learn how to cast a random spell.";
          B.Cost = 750;
          B.SetCast().Plain(Dice.One);
          B.Apply.LearnSpell(Attributes.intelligence, Skills.literacy, Spell: null);
        });

        //S.AddBoon("life insurance", B =>
        //{
        //  B.Description = "Apply a policy that will save your life.";
        //  B.Cost = 1000;
        //  B.SetCast().Plain(Dice.One);
        //  B.Apply.ApplyTransient(Property.Lifesaving, Dice.Fixed(2000));
        //});
      });

      craft_station = AddShrine("craft station", Entities.artisan, Glyphs.craft_station, Sonics.bell, 20, S =>
      {
        S.KeeperFeature = Features.workbench;

        S.AddBoon("assess", B =>
        {
          B.Description = "Determine the enchantment and charges of all carried items.";
          B.Cost = 50;
          B.SetCast().Strike(Strikes.psychic, Dice.Zero);
          B.Apply.Assess();
        });

        S.AddBoon("rename", B =>
        {
          B.Description = "Rename an item for vanity and as a small protection against cancellation.";
          B.Cost = 100;
          B.SetCast().FilterAnyItem()
           .SetAssetIndividualised(false)
           .FilterCoins(false);
          B.AssetMotion = Codex.Motions.rename;
          B.Apply.Nothing();
        });

        // not useful until inscribing is mainstream, also a bit too similar in name with 'scribe' boon.
        //S.AddBoon("inscribe", B =>
        //{
        //  B.Description = "Write, engrave or carve something onto your item.";
        //  B.Cost = 100;
        //  B.Cast = Cast.AnyItem();
        //  B.AssetMotion = Motion.Inscribe;
        //  B.Apply.Nothing();
        //});

        S.AddBoon("enlighten", B =>
        {
          B.Description = "Learn the name of one random unknown item.";
          B.Cost = 100;
          B.Apply.Enlightenment(null);
        });

        S.AddBoon("cancel", B =>
        {
          B.Description = "Remove all magic from an item.";
          B.Cost = 250;
          B.SetCast().FilterAnyItem();
          B.Apply.Cancellation(Elements.magical);
        });

        S.AddBoon("recharge", B =>
        {
          B.Description = "Partially recharge a spent item.";
          B.Cost = 250;
          B.SetCast().FilterCharged();
          B.Apply.Charging(Dice.One, Dice.Fixed(75)); // 75%
        });

        S.AddBoon("reforge", B =>
        {
          B.Description = "Convert one item into a random alternative of the same type.";
          B.Cost = 500;
          B.SetCast().FilterAnyItem()
           .FilterEquipped(false) // otherwise can reforge an equipped one-handed weapon into a two-handed weapon.
           .FilterUniques(false)
           .FilterCoins(false);
          B.Apply.PolymorphItem();
        });

        S.AddBoon("enchant", B =>
        {
          B.Description = "Upgrade an item or increase its power.";
          B.Cost = 500;
          B.SetCast().FilterEnchanted()
           .SetAssetIndividualised();
          B.Apply.EnchantUp(Dice.Fixed(+1));
        });

        S.AddBoon("replicate", B =>
        {
          B.Description = "Make an imitation copy of an item.";
          B.Cost = 1000;
          B.SetCast().FilterAnyItem();
          B.Apply.ReplicateAsset();
        });
      });

      Register.Alias(holy_shrine, "altar");
    }
#endif

    public readonly Shrine craft_station;
    public readonly Shrine holy_shrine;
    public readonly Shrine dark_sepulchre;
    public readonly Shrine sacred_grove;
    public readonly Shrine mystic_coven;
  }
}