using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexItems : CodexPage<ManifestItems, ItemEditor, Item>
  {
    private CodexItems() { }
#if MASTER_CODEX
    internal CodexItems(Codex Codex)
      : base(Codex.Manifest.Items)
    {
      this.DragonScalesArmourList = new Inv.DistinctList<Item>();

      var Stocks = Codex.Stocks;
      var Kinds = Codex.Kinds;
      var Motions = Codex.Motions;
      var Beams = Codex.Beams;
      var Strikes = Codex.Strikes;
      var Engulfments = Codex.Engulfments;
      var Explosions = Codex.Explosions;
      var Materials = Codex.Materials;
      var Properties = Codex.Properties;
      var Elements = Codex.Elements;
      var Attributes = Codex.Attributes;
      var Sanctities = Codex.Sanctities;
      var Skills = Codex.Skills;
      var Glyphs = Codex.Glyphs;
      var Sonics = Codex.Sonics;

      CodexEntities Entities = null;
      CodexRaces Races = null;

      CodexRecruiter.Enrol(() =>
      {
        Races = Codex.Races;
        Entities = Codex.Entities;
      });

      Item AddItem(Stock Stock, ItemType ItemType, string Name, Action<ItemEditor> DeclareAction)
      {
        return Register.Add(I =>
        {
          I.Stock = Stock;
          I.Type = ItemType;
          I.Name = Name;

          if (ItemType == ItemType.Instrument)
            I.UtilitySkill = Skills.music;

          if (ItemType == ItemType.Lockpick || ItemType == ItemType.SkeletonKey)
            I.UtilitySkill = Skills.locks;

          if (ItemType == ItemType.Device)
            I.UtilitySkill = Skills.traps;

          CodexRecruiter.Enrol(() =>
          {
            DeclareAction(I);

            Debug.Assert(I.Artifact || I.Impact != null || I.Material != Materials.glass);
          });
        });
      }
      Item AddPotion(string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.potion, ItemType.Potion, Name, I =>
        {
          DeclareAction(I);

          Debug.Assert(I.Material == Materials.glass);

          I.SetImpact(Sonics.broken_glass);
        });
      }
      Item AddScroll(string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.scroll, ItemType.Scroll, Name, I =>
        {
          DeclareAction(I);
        });
      }
      Item AddWand(string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.wand, ItemType.Wand, Name, I =>
        {
          DeclareAction(I);
        });
      }
      Item AddRangedWeapon(Ammunition Ammunition, string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.weapon, ItemType.RangedWeapon, Name, I =>
        {
          DeclareAction(I);

          I.SetWeapon().Ammunition = Ammunition;
        });
      }
      Item AddRangedMissile(Ammunition Ammunition, string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.weapon, ItemType.RangedMissile, Name, I =>
        {
          DeclareAction(I);

          I.SetWeapon().Ammunition = Ammunition;
        });
      }
      Item AddThrownMissile(Ammunition Ammunition, string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.weapon, ItemType.ThrownWeapon, Name, I =>
        {
          DeclareAction(I);

          I.SetWeapon().Ammunition = Ammunition;
        });
      }
      Item AddThrownWeapon(string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.weapon, ItemType.ThrownWeapon, Name, I =>
        {
          DeclareAction(I);
        });
      }
      Item AddMeleeWeapon(string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.weapon, ItemType.MeleeWeapon, Name, I =>
        {
          DeclareAction(I);
        });
      }
      Item AddReachWeapon(string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.weapon, ItemType.ReachWeapon, Name, I =>
        {
          DeclareAction(I);

          //I.Weapon.FixedRange = 2; // NOTE: only needed for ranged weapons (and if you want fixed range text in the item help).
        });
      }
      Item AddArmour(ItemType ItemType, string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.armour, ItemType, Name, I =>
        {
          DeclareAction(I);
        });
      }
      Item AddLight(string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.tool, ItemType.Light, Name, I =>
        {
          DeclareAction(I);
        });
      }
      Item AddInstrument(string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.tool, ItemType.Instrument, Name, I => // TODO: ItemType.Instrument?
        {
          DeclareAction(I);
        });
      }
      Item AddGem(string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.gem, ItemType.Gem, Name, I =>
        {
          Debug.Assert(I.BundleDice == null);

          I.BundleDice = Dice.One;

          DeclareAction(I);
        });
      }
      Item AddAmulet(string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.amulet, ItemType.Amulet, Name, I =>
        {
          DeclareAction(I);
        });
      }
      Item AddRing(string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.ring, ItemType.Ring, Name, I =>
        {
          DeclareAction(I);
        });
      }
      Item AddBook(string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.book, ItemType.Book, Name, I =>
        {
          DeclareAction(I);

          Debug.Assert(I.Rarity > 0, $"{I.Name} must have a specified rarity.");
        });
      }
      Item AddFood(string Name, Action<ItemEditor> DeclareAction)
      {
        return AddItem(Stocks.food, ItemType.Food, Name, I =>
        {
          DeclareAction(I);
        });
      }
      void EquipPellet(ItemEditor I, Dice Damage)
      {
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.gem);
        var W = I.SetOneHandedWeapon(Skills.sling, null, Elements.physical, DamageType.Bludgeon, Damage);
        W.Ammunition = Ammunition.Pellet;
      }
      void SugarRush(ItemEditor I, ApplyEditor A, int? Duration = null)
      {
        // fairies benefit from sweet things and an increased metabolism.
        A.WhenTargetKind(new[] { Kinds.fairy }, T =>
        {
          T.ApplyTransient(Properties.life_regeneration, 10.d(Duration ?? (I.Nutrition / 10)));
          T.ApplyTransient(Properties.mana_regeneration, 10.d(Duration ?? (I.Nutrition / 10)));
        });
      }
      void KnockoutPoison(ApplyEditor Apply)
      {
        Apply.WhenChance(Chance.OneIn4, T => T.ApplyTransient(Properties.sleeping, 3.d20()));
      }
      void SetUpgradeDowngradePair(Item LowerItem, Item UpperItem)
      {
        Debug.Assert(LowerItem != UpperItem && LowerItem.UpgradeItem == null && UpperItem.DowngradeItem == null);

        Register.Edit(UpperItem).SetDowngradeItem(LowerItem);
        Register.Edit(LowerItem).SetUpgradeItem(UpperItem);
      }
      void SetUpgradeDowngradeChain(params Item[] ItemArray)
      {
        Debug.Assert(ItemArray.Length >= 3);

        Register.Edit(ItemArray[0]).SetUpgradeItem(ItemArray[1]);

        for (var ItemIndex = 1; ItemIndex < ItemArray.Length - 1; ItemIndex++)
        {
          var ItemEditor = Register.Edit(ItemArray[ItemIndex]);
          ItemEditor.SetDowngradeItem(ItemArray[ItemIndex - 1]);
          ItemEditor.SetUpgradeItem(ItemArray[ItemIndex + 1]);
        }

        Register.Edit(ItemArray[ItemArray.Length - 1]).SetDowngradeItem(ItemArray[ItemArray.Length - 2]);
      }
      void SetDowngradeItem(Item LowerItem, params Item[] UpperItemArray)
      {
        Debug.Assert(UpperItemArray.Length >= 1);

        foreach (var UpperItem in UpperItemArray)
          Register.Edit(UpperItem).SetDowngradeItem(LowerItem);
      }

      #region artifact.
      var ArtifactEssence = Essence.FromUnits(0); // artifacts cannot be scrapped into essence.

      Backpack = AddItem(Stocks.tool, ItemType.Bag, "Backpack", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Backpack;
        I.Sonic = Sonics.tool;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(150);
        I.Material = Materials.cloth;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(25);

        var Storage = I.SetStorage();
        Storage.Locking = false;
        Storage.Preservation = false;
        Storage.Compression = 1.0F;
        Storage.ContainedDice = Dice.Zero;
        Storage.LockSonic = null;
        Storage.BreakSonic = null;
        Storage.TrappedExplosion = null;
      });

      Stamped_Letter = AddItem(Stocks.scroll, ItemType.Letter, "Stamped Letter", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Stamped_Letter;
        I.Sonic = Sonics.scroll;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.paper;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(5000);
      });

      Gold_Key = AddItem(Stocks.tool, ItemType.SpecificKey, "Gold Key", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Gold_Key;
        I.Sonic = Sonics.tool;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.gold;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1000);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.tool);
      });

      Ruby_Key = AddItem(Stocks.tool, ItemType.SpecificKey, "Ruby Key", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Ruby_Key;
        I.Sonic = Sonics.tool;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.gemstone;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1000);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.tool);
      });

      Shadow_Key = AddItem(Stocks.tool, ItemType.SpecificKey, "Shadow Key", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Shadow_Key;
        I.Sonic = Sonics.tool;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.ether;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1000);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.tool);
      });

      Silver_Key = AddItem(Stocks.tool, ItemType.SpecificKey, "Silver Key", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Silver_Key;
        I.Sonic = Sonics.tool;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.silver;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1000);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.tool);
      });

      Jade_Key = AddItem(Stocks.tool, ItemType.SpecificKey, "Jade Key", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Jade_Key;
        I.Sonic = Sonics.tool;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.stone;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1000);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.tool);
      });

      Master_Key = AddItem(Stocks.tool, ItemType.SkeletonKey, "Master Key", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Master_Key;
        I.Sonic = Sonics.tool;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Tiny;
        I.BundleDice = Dice.One;
        I.Weight = Weight.FromUnits(40);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1100);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.tool)
         .SetTalent(Properties.stealth, Properties.quickness, Properties.dark_vision);
      });

      Colossal_Excavator = AddMeleeWeapon("Colossal Excavator", I =>
      {
        I.Description = "This ancient artifact is a marvel to behold with exquisite craftsmanship and unbridled digging power.";
        I.Glyph = Glyphs.Colossal_Excavator;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1200);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1400);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon)
         .SetTalent(Properties.tunnelling);
        var W = I.SetTwoHandedWeapon(Skills.pick, null, Elements.physical, DamageType.Pierce, 1.d12() + 6);
        W.AddVersus(new[] { Materials.stone }, Elements.physical, 2.d12() + 12);
        I.AddObviousUse(Motions.dig, Delay.FromTurns(10), Sonics.pick_axe, Use =>
        {
          Use.SetCast().Beam(Beams.digging, 1.d4())
             .SetAudibility(10)
             .SetPenetrates()
             .SetBounces(false);
          Use.Apply.Digging(Elements.digging);
        });
      });

      Mirrorbright = AddArmour(ItemType.Shield, "Mirrorbright", I =>
      {
        I.Description = null;// "Magical rays and beams bounce right off this gleaming shield. The surface shines images of the real world, heedless of the trickery enemies might employ.";
        I.Glyph = Glyphs.Mirrorbright;
        I.Sonic = Sonics.armour;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.silver;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1500);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.reflection, Properties.clarity);
        I.SetArmour(Skills.heavy_armour, 2);
      });

      Escapist = AddArmour(ItemType.Cloak, "Escapist", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Escapist;
        I.Sonic = Sonics.armour;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.cloth;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(700);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.displacement, Properties.stealth, Properties.invisibility, Properties.phasing, Properties.free_action, Properties.conflict, Properties.fear, Properties.silence, Properties.hunger)
         .SetBoostAttribute(Attributes.dexterity)  
         .SetSpeedBoost();
        I.SetArmour(Skills.light_armour, 1);
      });

      Philosophers_Stone = AddAmulet("Philosophers Stone", I =>
      {
        I.Description = "Gazing into its crimson depths you can see the concentrated life essence of countless victims swirling inside of it. The artifact was completed in the moment of the creators' death, as his soul was claimed by his creation. Now residing among the souls of his former victims, his essence as an alchemist provides the stone with ultimate transmutative powers.";
        I.Glyph = Glyphs.Philosophers_Stone;
        I.Sonic = Sonics.amulet;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.gemstone;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(5000);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet).SetTalent(Properties.life_regeneration, Properties.vitality, Properties.slow_digestion);
        I.AddObviousUse(Motions.rub, Delay.FromTurns(20), Sonics.magic, Use =>
        {
          Use.Apply.Harm(Elements.physical, 2.d5());
          Use.Apply.SummonEntity(Dice.One, Entities.homunculus);
        });
        I.AddObviousUse(Motions.zap, Delay.FromTurns(30), Sonics.magic, Use =>
        {
          Use.SetCast().FilterAnyItem();
          Use.Apply.Harm(Elements.physical, Dice.One, Modifier.Plus1);
          Use.Apply.TransmuteAsset(Materials.gold);
        });
      });

      Eye_of_Aethiopica = AddAmulet("Eye of Aethiopica", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Eye_of_Aethiopica;
        I.Sonic = Sonics.amulet;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(2000);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet)
         .SetTalent(Properties.telepathy, Properties.mana_regeneration, Properties.teleport_control);
      });

      Eyes_of_Ra = AddItem(Stocks.tool, ItemType.Eyewear, "Eyes of Ra", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Eyes_of_Ra;
        I.Sonic = Sonics.tool;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.glass;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1500);
        I.SetIllumination(4);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.tool)
         .SetTalent(Properties.dark_vision, Properties.clarity, Properties.clairvoyance);
      });

      Rosenthral = AddWand("Rosenthral", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Rosenthral;
        I.Sonic = Sonics.wand;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.vegetable;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(800);
        I.AddObviousUse(Motions.zap, Delay.FromTurns(30), Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.magic, Dice.One)
             .SetAudibility(1);
          Use.Apply.WithSourceSanctity
          (
            B => B.Charm(Elements.magical, Delay.FromTurns(2000)),
            U => U.Charm(Elements.magical, Delay.FromTurns(1000), Kinds.Living.ToArray()),
            C => C.ApplyTransient(Properties.rage, 4.d6() + 4)
          );
        });
      });

      Witherloch = AddWand("Witherloch", I =>
      {
        I.Description = "This withered hand is the last remnant of an ancient and powerful lich.";
        I.Glyph = Glyphs.Witherloch;
        I.Sonic = Sonics.wand;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.DefaultSanctity = Sanctities.Cursed;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.animal;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(900);
        I.AddObviousUse(Motions.zap, Delay.FromTurns(30), Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.magic, Dice.One)
             .SetAudibility(1);
          Use.Apply.Karma(ChangeType.Decrease, 1.d50() + 100); // costs 101-150 karma
          Use.Apply.WithSourceSanctity
          (
            B => B.AnimateRevenants(Corrupt: false),
            U => U.AnimateRevenants(Corrupt: true),
            C => C.Backfire(F => F.Death(Elements.magical, new Kind[] { }, Strikes.death, Cause: null))
          );
          Use.Apply.Backfire(F => F.PlaceCurse(Dice.One, Sanctities.Cursed)); // curse the wand.
        });
        I.AddObviousUse(Motions.swat, Delay.FromTurns(10), Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.death, Dice.One)
             .SetAudibility(5);
          Use.Apply.Harm(Elements.physical, Dice.Zero);
          Use.Apply.Karma(ChangeType.Decrease, 1.d50() + 200); // costs 201-250 karma
          Use.Apply.WhenTargetKind(Kinds.Living, T =>
          {
            T.DrainLife(Elements.drain, 6.d6() + 6);
          }, F =>
          {
            F.Charm(Elements.magical, Delay.FromTurns(10000), Kinds.Undead.ToArray()); // bind undead.
          });
        });
      });

      Illuminare = AddLight("Illuminare", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Illuminare;
        I.Sonic = Sonics.tool;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1200);
        I.SetIllumination(3);
        I.SetEquip(EquipAction.Employ, Delay.FromTurns(10), Sonics.tool) 
         .SetTalent(Properties.see_invisible);
        I.AddObviousUse(Motions.flash, Delay.FromTurns(10), Sonics.flash, Use =>
        {
          Use.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetAudibility(5);
          Use.Apply.Malnutrition(Dice.Fixed(100));
          Use.Apply.WithSourceSanctity
          (
            B => B.AreaTransient(Properties.blindness, 4.d6() + 4),
            U => U.AreaTransient(Properties.blindness, 3.d6() + 3),
            C => C.Backfire(B => B.ApplyTransient(Properties.blindness, 2.d6() + 2))
          );
        });
        I.AddPropertyAreaUse(Motions.zap, Properties.fear, Delay.FromTurns(20), Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetAudibility(5);
          Use.Apply.Malnutrition(Dice.Fixed(100));
          Use.Apply.Light(true);
          Use.Apply.WithSourceSanctity
          (
            B => B.AreaTransient(Properties.fear, 6.d6(), Kinds.Undead.ToArray()),
            U => U.AreaTransient(Properties.fear, 4.d6(), Kinds.Undead.ToArray()),
            C => C.AreaTransient(Properties.rage, 4.d6(), Kinds.Undead.ToArray())
          );
        });
        I.AddObviousUse(Motions.scry, Delay.FromTurns(30), Sonics.magic, Use =>
        {
          Use.Apply.Malnutrition(Dice.Fixed(100));
          Use.Apply.WithSourceSanctity
          (
            B => B.Mapping(Range.Sq20, Chance.Always),
            U => U.Mapping(Range.Sq15, Chance.Always),
            C => C.Mapping(Range.Sq10, Chance.OneIn2)
          );
        });
      });

      Pandoras_Box = AddItem(Stocks.tool, ItemType.Tool, "Pandora's Box", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Pandoras_Box;
        I.Sonic = Sonics.tool;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(2000);
        I.Material = Materials.wood;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(2000);
        I.DefaultSanctity = Sanctities.Cursed;
        I.AddObviousUse(Motions.open, Delay.FromTurns(30), Sonics.magic, Use =>
        {
          Use.Apply.Karma(ChangeType.Decrease, 1.d100() + 50); // costs 51-150 karma
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.Backfire(F => F.PlaceCurse(Dice.One, Sanctities.Cursed)); // curse the box.
              B.Rumour(true, false);
              B.WhenProbability(Table =>
              {
                Table.Add(10, A =>
                {
                  A.Light(true);
                  A.Mapping(Range.Sq20, Chance.Always);
                  A.Searching(Range.Sq20);
                  A.DetectTrap(Range.Sq20);
                });
                Table.Add(10, A =>
                {
                  A.Pacify(Elements.magical, Kinds.Living.ToArray());
                  A.AreaTransient(Properties.fear, 4.d10(), Kinds.Undead.ToArray());
                });
                Table.Add(10, A =>
                {
                  A.Heal(5.d20(), Modifier.Zero);
                  A.Energise(5.d20(), Modifier.Zero);
                  A.RestoreAbility();
                });
                Table.Add(10, A =>
                {
                  A.Nutrition(5.d100());
                  A.ApplyTransient(Properties.slow_digestion, 5.d100());
                  A.ApplyTransient(Properties.life_regeneration, 5.d100());
                  A.ApplyTransient(Properties.mana_regeneration, 5.d100());
                });
                Table.Add(10, A =>
                {
                  A.Unafflict();
                  A.Unpunish();
                  A.Unpolymorph();
                  A.RemoveCurse(Dice.One);
                  A.Charging(Dice.One, Dice.Fixed(100)); // 100%
                });
                Table.Add(10, A => A.CreateAsset(Dice.One));
                Table.Add(10, A => A.SummonEntity(Dice.One, Entities.List.Where(E => E.IsEncounter && E.IsDomestic).ToArray()));
                Table.Add(5, A => A.CreateFixture(Codex.Features.fountain));
              });
            },
            U =>
            {
              U.Backfire(F => F.PlaceCurse(Dice.One, Sanctities.Cursed));  // curse the box.
              U.Rumour(true, true);
              U.WhenProbability(Table =>
              {
                Table.Add(10, A =>
                {
                  A.Light(true);
                  A.DetectTrap(Range.Sq20);
                });
                Table.Add(10, A =>
                {
                  A.Light(false);
                  A.Concealing(Range.Sq20);
                });
                Table.Add(10, A =>
                {
                  A.CreateHorde(Dice.One);
                  A.AreaTransient(Properties.slowness, 4.d10());
                });
                Table.Add(10, A =>
                {
                  A.CreateHorde(Dice.One);
                  A.AreaTransient(Properties.quickness, 4.d10());
                });
                Table.Add(10, A => A.TeleportInventoryAsset());
                Table.Add(10, A => A.TransitionRandom(Teleport: null, 1.d2()));
                Table.Add(5, A => A.CreateFixture(Codex.Features.altar));
              });
            },
            C =>
            {
              C.Rumour(false, true);
              C.WhenProbability(Table =>
              {
                Table.Add(10, A =>
                {
                  A.Light(false);
                  A.Amnesia(Range.Sq30);
                  A.Concealing(Range.Sq30);
                });
                Table.Add(10, A =>
                {
                  A.ApplyTransient(Properties.paralysis, 3.d6());
                  A.Murder(MurderType.Every, Strikes.death, Kinds.Living.ToArray());
                });
                Table.Add(10, A =>
                {
                  A.ApplyTransient(Properties.petrifying, 3.d6());
                  A.CreateHorde(Dice.One);
                  A.AreaTransient(Properties.rage, 10.d10());
                });
                Table.Add(10, A =>
                {
                  A.Afflict(Codex.Afflictions.List.ToArray());
                  A.Punish(Codex.Punishments.List.ToArray());
                });
                Table.Add(10, A =>
                {
                  A.TeleportInventoryAsset();
                  A.CreateEntity(1.d3(), Kinds.golem.Entities.Where(E => E.IsEncounter).ToArray());
                });
                Table.Add(10, A =>
                {
                  A.CreateBoulder(2.d3());
                  A.CreateHorde(Dice.One);
                });
                Table.Add(10, A =>
                {
                  A.CreateFixture(Codex.Features.pentagram);
                  A.RaiseDead(Percent: 100, Corrupt: true);
                  A.CreateEntity(1.d3(), Entities.ghost);
                });
                Table.Add(10, A => A.TransitionDescend(Teleport: null, 4.d3())); // 4-12 levels down.
                Table.Add(10, A => A.Polymorph(Kinds.worm.Entities.Where(E => E.IsEncounter).ToArray(), Items: false));
                Table.Add(5, A => A.Death(Elements.magical, new Kind[] { }, Strikes.death, Cause: null));
              });
            }
          );
        });
        I.AddObviousUse(Motions.sacrifice, Delay.FromTurns(60), Sonics.prayer, Use =>
        {
          Use.SetCast().FilterItem(Codex.Items.animal_corpse, Codex.Items.vegetable_corpse);
          Use.Apply.Backfire(F => F.Sanctify(null, Sanctities.Blessed));
          Use.Apply.Sacrifice();
        });
      });

      Prudentia = AddLight("Prudentia", I =>
      {
        I.Description = "This glowing looking glass reveals glimpses of the future and corrupts your soul.";
        I.Glyph = Glyphs.Prudentia;
        I.Sonic = Sonics.tool;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1000);
        I.SetIllumination(2);
        I.SetEquip(EquipAction.Employ, Delay.FromTurns(10), Sonics.tool)
         .SetTalent(Properties.reflection, Properties.warning);
        I.AddObviousUse(Motions.scry, Delay.FromTurns(30), Sonics.magic, Use =>
        {
          Use.Apply.Karma(ChangeType.Decrease, Dice.Fixed(100));
          Use.Apply.DetectTrap(Range.Sq10);
          Use.Apply.Searching(Range.Sq10);
          Use.Apply.WithSourceSanctity
          (
            B => B.DetectAsset(Range.Sq20),
            U => U.DetectAsset(Range.Sq15),
            C => C.DetectAsset(Range.Sq10)
          );
        });
      });

      Verimurus = AddMeleeWeapon("Verimurus", I =>
      {
        I.Description = "A surviving portion of the legendary wall of Verimurus, an ancient elven city said to have lasted since time immemorial. However, not even the mightiest walls could withstand the endless onslaught of the dragons. Now the enchanted walls stand alone over the land that once held them.";
        I.Glyph = Glyphs.Verimurus;
        I.Sonic = Sonics.tool;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(5000);
        I.Material = Materials.stone;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1350);
        I.SetArmour(Skills.heavy_armour, 8);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(20), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.hammer, null, Elements.physical, DamageType.Bludgeon, 2.d4()).AttackModifier = Modifier.Minus2;
        I.AddObviousUse(Motions.exchange, Delay.FromTurns(20), Sonics.scrape, Use =>
        {
          Use.SetCast().Strike(Strikes.magic, Dice.Fixed(10))
             .SetAudibility(1);
          Use.Apply.Exchange();
          Use.Apply.Repel(Range.Sq3, Items: true, Characters: true, Boulders: true);
          Use.Apply.WithSourceSanctity
          (
            B => B.AreaTransient(Properties.slowness, 1.d6() + 5),
            U => U.AreaTransient(Properties.slowness, 1.d6() + 3),
            C => C.Backfire(F => F.ApplyTransient(Properties.stunned, 1.d3() + 1))
          );
        });
      });

      Talaria = AddArmour(ItemType.Boots, "Talaria", I =>
      {
        I.Description = "Winged sandals made of imperishable gold which fly the wearer swiftly and silently.";
        I.Appearance = null;
        I.Glyph = Glyphs.Talaria;
        I.Sonic = Sonics.armour;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.gold;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(900);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.armour)
         .SetTalent(Properties.quickness, Properties.flight, Properties.stealth);
        I.SetArmour(Skills.light_armour, 1);
      });

      Dragonbane = AddMeleeWeapon("Dragonbane", I =>
      {
        I.Description = "Forged deep underground by dwarven hands, this blade was made to slay the scaled beasts who sought to steal their treasures. Throughout the long years this blade has bathed in the blood of its winged prey. Now almost forgotten, it yearns again for the taste of dragon flesh.";
        I.Glyph = Glyphs.Dragonbane;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(700);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1000);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        var W = I.SetOneHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 2.d5(), D =>
        {
          D.WhenTargetKind(new[] { Kinds.dragon }, T => T.WhenChance(Chance.OneIn4, A =>
          {
            A.WithSourceSanctity
            (
              B => B.ApplyTransient(Properties.fear, 2.d4()),
              U => U.ApplyTransient(Properties.fear, 2.d3()),
              C => C.Backfire(F => F.ApplyTransient(Properties.fear, 1.d3()))
            );
          }));
        });
        W.AddVersus(new[] { Kinds.dragon }, Elements.physical, 2.d4());
      });

      Lashing_Tongue = AddReachWeapon("Lashing Tongue", I =>
      {
        I.Description = "This curious artifact somehow chastens those it strikes and submits them to your will.";
        I.Glyph = Glyphs.Lashing_Tongue;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.leather;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1000);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.leather);
        I.SetOneHandedWeapon(Skills.whip, null, Elements.physical, DamageType.Slash, 1.d6(), D =>
        {
          D.Disarm();
          D.WhenChance(Chance.OneIn10, T =>
          {
            T.WithSourceSanctity
            (
              B => B.Charm(Elements.magical, Delay.FromTurns(30000), Kinds.Living.ToArray()),
              U => U.Charm(Elements.magical, Delay.FromTurns(20000), Kinds.Living.ToArray()),
              C => C.Charm(Elements.magical, Delay.FromTurns(10000), Kinds.Living.ToArray())
            );
          });
        });
      });

      Cleaver = AddMeleeWeapon("Cleaver", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Cleaver;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1200);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1400);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon)
         .SetAttackBoost();
        I.SetTwoHandedWeapon(Skills.axe, null, Elements.physical, DamageType.Slash, 3.d6());
      });

      Magistrator = AddMeleeWeapon("Magistrator", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Magistrator;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(600);
        I.Material = Materials.gold;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1500);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon)
         .SetResistance(Elements.magical);
        I.SetOneHandedWeapon(Skills.mace, null, Elements.physical, DamageType.Bludgeon, 2.d6() + 2, D => D.WhenChance(Chance.OneIn4, T =>
        {
          T.WithSourceSanctity
          (
            B => B.Punish(Codex.Punishments.List.ToArray()),
            U => U.Punish(Codex.Punishments.List.ToArray()),
            C => C.Backfire(F => F.Punish(Codex.Punishments.List.ToArray()))
          );
        }));
      });

      Sunsword = AddMeleeWeapon("Sunsword", I =>
      {
        I.Description = "This powerful artifact blade glows with a holy fury that lights the area.";
        I.Glyph = Glyphs.Sunsword;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1500);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetIllumination(2);
        var W = I.SetOneHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 1.d8(), D => D.WhenChance(Chance.OneIn4, T =>
        {
          T.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.blindness, 1.d6() + 5),
            U => U.ApplyTransient(Properties.blindness, 1.d6() + 3),
            C => C.Backfire(F => F.ApplyTransient(Properties.blindness, 1.d6() + 1))
          );
        }));
        W.AddVersus(Kinds.Undead.ToArray(), Elements.fire, 1.d8());
      });

      Vorpal_Blade = AddMeleeWeapon("Vorpal Blade", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Vorpal_Blade;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.mithril;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1800);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 1.d8(), D =>
        {
          D.WhenTargetEntity(new[] { Entities.jabberwock, Entities.vorpal_jabberwock }, T => T.Decapitate(Strikes.sever));
          D.WithSourceSanctity
          (
            B => B.WhenChance(Chance.OneIn10, T => T.Decapitate(Strikes.sever)),
            U => U.WhenChance(Chance.OneIn20, T => T.Decapitate(Strikes.sever)),
            C => C.Backfire(F => F.WhenChance(Chance.OneIn10, T => T.Decapitate(Strikes.sever)))
          );
        });
      });

      Grimtooth = AddMeleeWeapon("Grimtooth", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Grimtooth;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1200);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon)
         .SetResistance(Elements.poison);
        I.SetOneHandedWeapon(Skills.light_blade, null, Elements.physical, DamageType.Pierce, 1.d6(), D =>
        {
          D.Harm(Elements.poison, 1.d8());
          D.WhenChance(Chance.OneIn4, T =>
          {
            T.WithSourceSanctity
            (
              B =>
              {
                B.UnlessTargetResistant(Elements.poison, A => A.DecreaseAbility(Attributes.strength, Dice.One));
                B.Afflict(Codex.Afflictions.poisoning);
              },
              U =>
              {
                U.UnlessTargetResistant(Elements.poison, A => A.DecreaseAbility(Attributes.strength, Dice.One));
              },
              C =>
              {
                C.Backfire(F => F.DecreaseAbility(Attributes.strength, Dice.One)); // NOTE: can't use MinorPoison because we need to bypass poison resistance.
              }
            );
          });
        });
      });

      Giantslayer = AddRangedWeapon(Ammunition.Pellet, "Giantslayer", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Giantslayer;
        I.Sonic = Sonics.leather;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Weight = Weight.FromUnits(30);
        I.Size = Size.Small;
        I.Material = Materials.wood;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1000);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.leather)
         .SetBoostAttribute(Attributes.strength);
        var Weapon = I.SetOneHandedWeapon(Skills.sling, Sonics.sling_shot, Elements.physical, DamageType.Bludgeon, Dice.One);
        Weapon.AddVersus(new[] { Kinds.giant }, Elements.physical, 2.d4());
        Weapon.AttackModifier = Modifier.Plus2;
        Weapon.BurstRate = 2;
      });

      Carapace = AddArmour(ItemType.Suit, "Carapace", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Carapace;
        I.Sonic = Sonics.armour;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(6000);
        I.Material = Materials.animal;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1800);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(90), Sonics.armour) 
         .SetBoostAttribute(Attributes.constitution)
         .SetTalent(Properties.vitality)
         .SetResistance(Elements.petrify, Elements.poison);
        I.SetArmour(Skills.heavy_armour, D: 10, P: +0, S: +0, B: +0);
      });

      Chaoshammer = AddThrownWeapon("Chaoshammer", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Chaoshammer;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1000);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1200);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        var W = I.SetOneHandedWeapon(Skills.hammer, null, Elements.physical, DamageType.Bludgeon, 2.d4());
        W.AddDetonation(Explosions.fiery, A =>
        {
          A.Harm(Elements.fire, 1.d12());
          A.UnlessTargetResistant(Elements.fire, R => R.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.blindness, 1.d4() + 1)));
        });
        W.AddDetonation(Explosions.frosty, A =>
        {
          A.Harm(Elements.cold, 1.d14());
          A.UnlessTargetResistant(Elements.cold, R => R.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.paralysis, 1.d4() + 1)));
        });
        W.AddDetonation(Explosions.electric, A =>
        {
          A.Harm(Elements.shock, 1.d16());
          A.UnlessTargetResistant(Elements.shock, R => R.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.stunned, 1.d4() + 1)));
        });
        W.AddDetonation(Explosions.acid, A =>
        {
          A.Harm(Elements.acid, 1.d18());
          A.UnlessTargetResistant(Elements.acid, R => R.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.confusion, 1.d4() + 1)));
        });
        W.AddDetonation(Explosions.magical, A =>
        {
          A.Harm(Elements.magical, 1.d20());
          A.UnlessTargetResistant(Elements.magical, R => R.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.sleeping, 1.d4() + 1)));
        });
      });

      Aurigage = AddArmour(ItemType.Gloves, "Aurigage", I =>
      {
        I.Description = null;
        I.Appearance = null;
        I.Glyph = Glyphs.Aurigage;
        I.Sonic = Sonics.armour;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(1000);
        I.Material = Materials.gold;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1500);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.warning, Properties.deflection);
        I.SetImpact(Codex.Sonics.coins, A =>
        {
          A.Repel(Range.Sq1, Items: false, Characters: true, Boulders: false);
          A.WhenChance(Chance.OneIn4, T =>
          {
            T.WithSourceSanctity
            (
              B => B.ApplyTransient(Properties.stunned, 1.d6() + 5),
              U => U.ApplyTransient(Properties.stunned, 1.d6() + 3),
              C => C.Backfire(F => F.ApplyTransient(Properties.stunned, 1.d6() + 1))
            );
          });
        });
        I.SetArmour(Skills.heavy_armour, 2);
      });

      Blinderag = AddArmour(ItemType.Helmet, "Blinderäg", I =>
      {
        I.Description = null;
        I.Appearance = null;
        I.Glyph = Glyphs.Blinderag;
        I.Sonic = Sonics.armour;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.DefaultSanctity = Sanctities.Cursed;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(800);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1250);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour) 
         .SetAttackBoost()
         .SetDamageBoost()
         .SetCrippleAttribute(Attributes.intelligence, Attributes.wisdom, Attributes.charisma)
         .SetTalent(Properties.rage);
        I.SetArmour(Skills.heavy_armour, 2);
      });

      Maulfrost = AddMeleeWeapon("Maulfrost", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Maulfrost;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1200);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1400);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.hammer, null, Elements.physical, DamageType.Bludgeon, 2.d8() + 2, A => A.WhenChance(Chance.OneIn4, T =>
        {
          T.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.slowness, 2.d6() + 5),
            U => U.ApplyTransient(Properties.slowness, 2.d6() + 3),
            C => C.Backfire(F => F.ApplyTransient(Properties.slowness, 2.d6() + 1))
          );
        }));
      });

      Voltaic = AddReachWeapon("Voltaic", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Voltaic;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(600);
        I.Material = Materials.platinum;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1200);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.spear, null, Elements.physical, DamageType.Pierce, 2.d6() + 2, A => A.WhenChance(Chance.OneIn4, T =>
        {
          T.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.stunned, 1.d6() + 5),
            U => U.ApplyTransient(Properties.stunned, 1.d6() + 3),
            C => C.Backfire(F => F.ApplyTransient(Properties.stunned, 1.d6() + 1))
          );
        }));
      });

      Deadwood = AddReachWeapon("Deadwood", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Deadwood;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(750);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(950);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon)
         .SetResistance(Elements.drain);
        var W = I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Slash, 2.d6(), D => D.WhenChance(Chance.OneIn4, T =>
        {
          T.WithSourceSanctity
          (
            B => B.DrainMana(Elements.drain, 2.d4()),
            U => U.DrainMana(Elements.drain, 1.d6()),
            C => C.Backfire(F => F.DrainMana(Elements.drain, 1.d4()))
          );
        }));
        W.AddVersus(new[] { Materials.vegetable }, Elements.physical, 2.d4());
      });

      Dire_Needle = AddMeleeWeapon("Dire Needle", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Dire_Needle;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(250);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1100);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Pierce, 1.d10(), D => D.WhenChance(Chance.OneIn4, T =>
        {
          T.WithSourceSanctity
          (
            B => B.DrainLife(Elements.drain, 2.d4()),
            U => U.DrainLife(Elements.drain, 1.d6()),
            C => C.Backfire(F => F.DrainLife(Elements.drain, 1.d4()))
          );
        }));
      });

      Chasm_Edge = AddMeleeWeapon("Chasm Edge", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Chasm_Edge;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1500);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1500);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.AddBlastUse(Motions.zap, Delay.FromTurns(20), Sonics.explosion, A =>
        {
          A.SetCast().Strike(Strikes.force, Dice.One);
          A.Apply.Karma(ChangeType.Decrease, Dice.Fixed(Codex.Devices.spiked_pit.KarmaCost));
          A.Apply.WithSourceSanctity
          (
            B => B.CreateTrap(Codex.Devices.spiked_pit, Destruction: false),
            U => U.CreateTrap(Codex.Devices.pit, Destruction: false),
            C => C.Backfire(F => F.CreateTrap(Codex.Devices.pit, Destruction: false))
          );
        });
        I.SetTwoHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 4.d4(), A => A.WhenChance(Chance.OneIn4, T =>
        {
          T.WithSourceSanctity
          (
            B => B.Harm(Elements.force, 1.d8()),
            U => U.Harm(Elements.force, 1.d6()),
            C => C.Backfire(F => F.Harm(Elements.force, 1.d4()))
          );
        }));
      });

      Deagle = AddRangedWeapon(Ammunition.Bullet, "Deagle", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Deagle;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.gold;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1500);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon)
         .SetTalent(Properties.free_action)
         .SetDamageBoost();
        var W = I.SetTwoHandedWeapon(Skills.firearms, Sonics.pistol_fire, Elements.physical, DamageType.Bludgeon, 1.d6());
        W.AttackModifier = Modifier.Plus5;
        W.FixedRange = 20;
      });

      Drilanze = AddReachWeapon("Drilanze", I =>
      {
        I.Description = "A wonder of dwarven ingenuity and craftsmanship, stolen and repurposed by orcs. Mounted on a long shaft, this tool is a formidable weapon on horseback or warg-back. The electrical current that powers its drill will occasionally discharge on whomever is willing to stand in the way of this monstrosity.";
        I.Glyph = Glyphs.Drilanze;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(3600);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1100);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon)
         .SetResistance(Elements.shock);
        I.AddObviousUse(Motions.dig, Delay.FromTurns(20), Sonics.electricity, Use =>
        {
          Use.SetCast().Strike(Strikes.tunnel, Dice.One);
          Use.Apply.Karma(ChangeType.Decrease, Dice.Fixed(Codex.Devices.hole.KarmaCost));
          Use.Apply.WithSourceSanctity
          (
            B => B.CreateTrap(Codex.Devices.hole, Destruction: true),
            U => U.CreateTrap(Codex.Devices.hole, Destruction: false),
            C => C.Backfire(F => F.CreateTrap(Codex.Devices.hole, Destruction: false))
          );
        });
        I.SetTwoHandedMomentumWeapon(Skills.lance, null, Elements.physical, DamageType.Pierce, 2.d6(), A => A.WhenChance(Chance.OneIn4, T =>
        {
          T.WithSourceSanctity
          (
            B => B.Harm(Elements.shock, 1.d8()),
            U => U.Harm(Elements.shock, 1.d6()),
            C => C.Backfire(F => F.Harm(Elements.physical, 1.d4()))
          );
        }));
      });

      Runesword = AddMeleeWeapon("Runesword", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.Runesword;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Artifact = true;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(950);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon)
         .SetDamageBoost();
        var W = I.SetTwoHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 1.d6()); // max: 1d6 + 20 damage.
        W.AddVersus(new[] { Kinds.orc }, Elements.drain, 2.d6());
      });

      Ancient_Katana = AddMeleeWeapon("Ancient Katana", I =>
      {
        I.Description = "This old katana has been battered and broken through a million battles. Hardly more than a scrap of metal and wood and yet it flickers with a strange power.";
        I.Glyph = Glyphs.Ancient_Katana;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(50);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Slash, 1.d5());
      });

      Masamune = AddMeleeWeapon("Masamune", I =>
      {
        I.Description = "A righteous sword forged by a revered blacksmith many years ago who set out to hone his craft and to create the ultimate blade. The noble intent of its creator lingers in this weapon to cut down the wicked and protect the innocent.";
        I.Glyph = Glyphs.Masamune;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(320);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.DefaultSanctity = Sanctities.Blessed;
        I.Price = Gold.FromCoins(1500);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon) 
         .SetTalent(Properties.beatitude, Properties.clarity);
        var W = I.SetTwoHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 4.d4());
        W.AddVersus(Kinds.Undead.ToArray(), Elements.physical, 1.d8());
        I.SetDowngradeItem(Ancient_Katana);
      });

      Muramasa = AddMeleeWeapon("Muramasa", I =>
      {
        I.Description = "The final sword forged by a once respected blacksmith who lost himself in the depths of the dungeon. His warped soul now inhabits this blade and lusts for blood. It is said that the one who wields this sword is coerced into cutting down friend and foe alike.";
        I.Glyph = Glyphs.Muramasa;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(240);
        I.Material = Materials.iron;
        I.Essence = ArtifactEssence;
        I.DefaultSanctity = Sanctities.Cursed;
        I.Price = Gold.FromCoins(1000);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon)
         .SetTalent(Properties.berserking);
        var W = I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Slash, 2.d4(), D =>
        {
          D.Harm(Elements.disintegrate, 1.d8());
        });
        W.AddVersus(Kinds.Living.ToArray(), Elements.physical, 1.d8());
        I.SetDowngradeItem(Ancient_Katana);
      });

      #endregion

      #region amulet.
      var AmuletEssence0 = Essence.FromUnits(10); // no amulet of nothing.
      var AmuletEssence1 = Essence.FromUnits(25);
      var AmuletEssence2 = Essence.FromUnits(50);
      var AmuletEssence3 = Essence.FromUnits(100);
      var AmuletEssence4 = Essence.FromUnits(150);
      var AmuletSize = Size.Tiny;
      var AmuletWeight = Weight.FromUnits(100);
      var AmuletSeries = new Series("amulet");
      var AmuletWeakness = new[] { Elements.shock };

      amulet_of_nada = AddAmulet("amulet of nada", I =>
      {
        I.Description = null;
        I.SetAppearance("medallion amulet", null, Price: Gold.FromCoins(150));
        I.Glyph = Glyphs.medallion_amulet;
        I.Sonic = Sonics.amulet;
        I.Series = AmuletSeries;
        I.Rarity = 100;
        I.Size = AmuletSize;
        I.Weight = AmuletWeight;
        I.Material = Materials.gold;
        I.Essence = AmuletEssence0;
        I.Price = Gold.FromCoins(25);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.amulet, A =>
        {
          A.Nothing();
        });
        I.SetWeakness(AmuletWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet);
        // NOTE: does nothing.
      });

      amulet_of_change = AddAmulet("amulet of change", I =>
      {
        I.Description = null;
        I.SetAppearance("square amulet", null);
        I.Glyph = Glyphs.square_amulet;
        I.Sonic = Sonics.amulet;
        I.Series = AmuletSeries;
        I.Rarity = 110;
        I.Size = AmuletSize;
        I.Weight = AmuletWeight;
        I.Material = Materials.iron;
        I.Essence = AmuletEssence3;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.amulet, A =>
        {
          A.MajorProperty(Properties.polymorph);
          A.Polymorph();
        });
        I.SetWeakness(AmuletWeakness);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet)
         .SetTalent(Properties.polymorph);
      });

      amulet_of_drain_resistance = AddAmulet("amulet of drain resistance", I =>
      {
        I.Description = null;
        I.SetAppearance("warped amulet", null);
        I.Glyph = Glyphs.warped_amulet;
        I.Sonic = Sonics.amulet;
        I.Series = AmuletSeries;
        I.Rarity = 60;
        I.Size = AmuletSize;
        I.Weight = AmuletWeight;
        I.Material = Materials.iron;
        I.Essence = AmuletEssence3;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.amulet, A =>
        {
          A.ConsumeResistance(Elements.drain);
        });
        I.SetWeakness(AmuletWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet)
         .SetResistance(Elements.drain);
      });

      amulet_of_ESP = AddAmulet("amulet of ESP", I =>
      {
        I.Description = null;
        I.SetAppearance("circular amulet", null);
        I.Glyph = Glyphs.circular_amulet;
        I.Sonic = Sonics.amulet;
        I.Series = AmuletSeries;
        I.Rarity = 140;
        I.Size = AmuletSize;
        I.Weight = AmuletWeight;
        I.Material = Materials.iron;
        I.Essence = AmuletEssence3;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.amulet, A =>
        {
          A.MajorProperty(Properties.telepathy);
        });
        I.SetWeakness(AmuletWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet)
         .SetTalent(Properties.telepathy);
      });

      amulet_of_flying = AddAmulet("amulet of flying", I =>
      {
        I.Description = null;
        I.SetAppearance("convex amulet", null);
        I.Glyph = Glyphs.convex_amulet;
        I.Sonic = Sonics.amulet;
        I.Series = AmuletSeries;
        I.Rarity = 50;
        I.Size = AmuletSize;
        I.Weight = AmuletWeight;
        I.Material = Materials.iron;
        I.Essence = AmuletEssence4;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.amulet, A =>
        {
          A.MajorProperty(Properties.flight);
        });
        I.SetWeakness(AmuletWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet)
         .SetTalent(Properties.flight);
      });

      amulet_of_life_saving = AddAmulet("amulet of life saving", I =>
      {
        I.Description = null;
        I.SetAppearance("spherical amulet", null);
        I.Glyph = Glyphs.spherical_amulet;
        I.Sonic = Sonics.amulet;
        I.Series = AmuletSeries;
        I.Rarity = 60;
        I.Size = AmuletSize;
        I.Weight = AmuletWeight;
        I.Material = Materials.iron;
        I.Essence = AmuletEssence4;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.amulet, A =>
        {
          A.MajorProperty(Properties.lifesaving);
        });
        I.SetWeakness(AmuletWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet)
         .SetTalent(Properties.lifesaving);
      });

      amulet_of_reflection = AddAmulet("amulet of reflection", I =>
      {
        I.Description = null;
        I.SetAppearance("hexagonal amulet", null);
        I.Glyph = Glyphs.hexagonal_amulet;
        I.Sonic = Sonics.amulet;
        I.Series = AmuletSeries;
        I.Rarity = 60;
        I.Size = AmuletSize;
        I.Weight = AmuletWeight;
        I.Material = Materials.iron;
        I.Essence = AmuletEssence4;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.amulet, A =>
        {
          A.MajorProperty(Properties.reflection);
        });
        I.SetWeakness(AmuletWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet)
         .SetTalent(Properties.reflection);
      });

      amulet_of_restful_sleep = AddAmulet("amulet of restful sleep", I =>
      {
        I.Description = null;
        I.SetAppearance("triangular amulet", null);
        I.Glyph = Glyphs.triangular_amulet;
        I.Sonic = Sonics.amulet;
        I.Series = AmuletSeries;
        I.Rarity = 110;
        I.Size = AmuletSize;
        I.Weight = AmuletWeight;
        I.Material = Materials.iron;
        I.Essence = AmuletEssence1;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.amulet, A =>
        {
          A.MajorProperty(Properties.narcolepsy);
          A.ApplyTransient(Properties.sleeping, 1.d25() + 25);
        });
        I.SetWeakness(AmuletWeakness);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet)
         .SetTalent(Properties.narcolepsy);
      });

      amulet_of_unchanging = AddAmulet("amulet of unchanging", I =>
      {
        I.Description = null;
        I.SetAppearance("concave amulet", null);
        I.Glyph = Glyphs.concave_amulet;
        I.Sonic = Sonics.amulet;
        I.Series = AmuletSeries;
        I.Rarity = 50;
        I.Size = AmuletSize;
        I.Weight = AmuletWeight;
        I.Material = Materials.iron;
        I.Essence = AmuletEssence1;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.amulet, A =>
        {
          A.MajorProperty(Properties.unchanging);
        });
        I.SetWeakness(AmuletWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet)
         .SetTalent(Properties.unchanging);
      });

      amulet_of_vitality = AddAmulet("amulet of vitality", I =>
      {
        I.Description = null;
        I.SetAppearance("octagonal amulet", null);
        I.Glyph = Glyphs.octagonal_amulet;
        I.Sonic = Sonics.amulet;
        I.Series = AmuletSeries;
        I.Rarity = 50;
        I.Size = AmuletSize;
        I.Weight = AmuletWeight;
        I.Material = Materials.iron;
        I.Essence = AmuletEssence3;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.amulet, A =>
        {
          A.MajorProperty(Properties.vitality);
        });
        I.SetWeakness(AmuletWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet)
         .SetTalent(Properties.vitality);
      });

      amulet_versus_poison = AddAmulet("amulet versus poison", I =>
      {
        I.Description = null;
        I.SetAppearance("pyramidal amulet", null);
        I.Glyph = Glyphs.pyramidal_amulet;
        I.Sonic = Sonics.amulet;
        I.Series = AmuletSeries;
        I.Rarity = 140;
        I.Size = AmuletSize;
        I.Weight = AmuletWeight;
        I.Material = Materials.iron;
        I.Essence = AmuletEssence2;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.amulet, A =>
        {
          A.ConsumeResistance(Elements.poison);
        });
        I.SetWeakness(AmuletWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet)
         .SetResistance(Elements.poison);
      });

      amulet_versus_stone = AddAmulet("amulet versus stone", I =>
      {
        I.Description = null;
        I.SetAppearance("oval amulet", null);
        I.Glyph = Glyphs.oval_amulet;
        I.Sonic = Sonics.amulet;
        I.Series = AmuletSeries;
        I.Rarity = 60;
        I.Size = AmuletSize;
        I.Weight = AmuletWeight;
        I.Material = Materials.iron;
        I.Essence = AmuletEssence2;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.amulet, A =>
        {
          A.ConsumeResistance(Elements.petrify);
        });
        I.SetWeakness(AmuletWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.amulet)
         .SetResistance(Elements.petrify);
      });

      CodexRecruiter.Enrol(() =>
      {
        foreach (var Amulet in Stocks.amulet.Items.Where(R => R != amulet_of_nada && !R.Artifact))
          Register.Edit(Amulet).SetDowngradeItem(amulet_of_nada);
      });
      #endregion

      #region armour.
      var ArmourEssence1 = Essence.FromUnits(5);
      var ArmourEssence2 = Essence.FromUnits(10);
      var ArmourEssence3 = Essence.FromUnits(20);
      var ArmourEssence4 = Essence.FromUnits(50);
      var ArmourEssence5 = Essence.FromUnits(100);
      var ArmourEssence6 = Essence.FromUnits(150);
      var HelmSeries = new Series("helm");
      var CloakSeries = new Series("cloak");
      var BootsSeries = new Series("boots");
      var GlovesSeries = new Series("gloves");
      var RobesSeries = new Series("robes");
      var RobePrice = Gold.FromCoins(100);

      alchemy_smock = AddArmour(ItemType.Cloak, "alchemy smock", I =>
      {
        I.Description = null;
        I.SetAppearance("leather apron", null);
        I.Glyph = Glyphs.alchemy_smock;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(60);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour, A =>
        {
          A.ConsumeResistance(Elements.acid);
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetResistance(Elements.acid);
        I.SetArmour(Skills.light_armour, 1);
      });

      banded_mail = AddArmour(ItemType.Suit, "banded mail", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.banded_mail;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 45;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(3500);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(90);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour);
        I.SetArmour(Skills.heavy_armour, D: 6, P: -1, S: +0, B: +1);
        I.AddObviousIngestUse(Motions.eat, 350, Delay.FromTurns(50), Sonics.armour);
      });

      chain_mail = AddArmour(ItemType.Suit, "chain mail", I =>
      {
        I.Description = "Chain mail consists of a mesh of metal rings interlinked to form a flexible armour.";
        I.Glyph = Glyphs.chain_mail;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 40;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(3000);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(75);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour);
        I.SetArmour(Skills.medium_armour, D: 5, P: +0, S: +1, B: +0);
        I.AddObviousIngestUse(Motions.eat, 300, Delay.FromTurns(50), Sonics.armour);
      });

      cloak_of_blinking = AddArmour(ItemType.Cloak, "cloak of blinking", I =>
      {
        I.Description = null;
        I.SetAppearance("dirty rag", null);
        I.Glyph = Glyphs.dirty_rag;
        I.Sonic = Sonics.armour;
        I.Series = CloakSeries;
        I.Rarity = 10;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence6;
        I.Price = Gold.FromCoins(180);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.blinking);
        I.SetArmour(Skills.light_armour, 1);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour, A =>
        {
          A.MajorProperty(Properties.blinking);
        });
      });

      cloak_of_displacement = AddArmour(ItemType.Cloak, "cloak of displacement", I =>
      {
        I.Description = null;
        I.SetAppearance("piece of cloth", null);
        I.Glyph = Glyphs.piece_of_cloth;
        I.Sonic = Sonics.armour;
        I.Series = CloakSeries;
        I.Rarity = 10;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence6;
        I.Price = Gold.FromCoins(140);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.displacement);
        I.SetArmour(Skills.light_armour, 1);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour, A =>
        {
          A.MajorProperty(Properties.displacement);
        });
      });

      cloak_of_invisibility = AddArmour(ItemType.Cloak, "cloak of invisibility", I =>
      {
        I.Description = null;
        I.SetAppearance("opera cloak", null);
        I.Glyph = Glyphs.opera_cloak;
        I.Sonic = Sonics.armour;
        I.Series = CloakSeries;
        I.Rarity = 10;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence6;
        I.Price = Gold.FromCoins(170);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.invisibility);
        I.SetArmour(Skills.light_armour, 1);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour, A =>
        {
          A.MajorProperty(Properties.invisibility);
        });
      });

      cloak_of_magic_resistance = AddArmour(ItemType.Cloak, "cloak of magic resistance", I =>
      {
        I.Description = null;
        I.SetAppearance("ornamental cope", "A long cloak which is open in the front and fastened at the breast with a clasp.");
        I.Glyph = Glyphs.ornamental_cope;
        I.Sonic = Sonics.armour;
        I.Series = CloakSeries;
        I.Rarity = 2;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence6;
        I.Price = Gold.FromCoins(160);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetResistance(Elements.magical);
        I.SetArmour(Skills.light_armour, 1);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour, A =>
        {
          A.ConsumeResistance(Elements.magical);
        });
      });

      cloak_of_deflection = AddArmour(ItemType.Cloak, "cloak of deflection", I =>
      {
        I.Description = null;
        I.SetAppearance("tattered cape", null);
        I.Glyph = Glyphs.tattered_cape;
        I.Sonic = Sonics.armour;
        I.Series = CloakSeries;
        I.Rarity = 9;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence6;
        I.Price = Gold.FromCoins(150);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.deflection);
        I.SetArmour(Skills.light_armour, 1);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour, A =>
        {
          A.MajorProperty(Properties.deflection);
        });
      });

      Item AddConicalHatArmour(string Name, Action<ItemEditor> EditorAction)
      {
        return AddArmour(ItemType.Helmet, Name, I =>
        {
          I.SetAppearance("conical hat", "This classic wizard accessory is a pointed hat decorated with moons and stars.", Price: Gold.FromCoins(300));

          EditorAction(I);
        });
      }
      dunce_cap = AddConicalHatArmour("dunce cap", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.conical_hat;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(40);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(80);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour, A =>
        {
          A.DecreaseAbility(Attributes.intelligence, Dice.One);
          A.DecreaseAbility(Attributes.wisdom, Dice.One);
        });
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetCrippleAttribute(Attributes.intelligence, Attributes.wisdom);
        I.SetArmour(Skills.light_armour, 0);
      });

      cornuthaum = AddConicalHatArmour("cornuthaum", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.conical_hat;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(40);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence6;
        I.Price = Gold.FromCoins(400);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour, A =>
        {
          A.MajorProperty(Properties.clairvoyance);
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.clairvoyance);
        I.SetArmour(Skills.light_armour, 0);
      });

      bronze_plate_mail = AddArmour(ItemType.Suit, "bronze plate mail", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.bronze_plate_mail;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 25;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(4500);
        I.Material = Materials.copper;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(400);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour);
        I.SetArmour(Skills.heavy_armour, D: 6, P: +0, S: +1, B: -1);
        I.AddObviousIngestUse(Motions.eat, 450, Delay.FromTurns(50), Sonics.armour);
      });

      plate_mail = AddArmour(ItemType.Suit, "plate mail", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.plate_mail;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 30;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(4500);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(600);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour);
        I.SetArmour(Skills.heavy_armour, D: 7, P: +0, S: +1, B: +0);
        I.AddObviousIngestUse(Motions.eat, 450, Delay.FromTurns(50), Sonics.armour);
      });

      crystal_plate_mail = AddArmour(ItemType.Suit, "crystal plate mail", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.crystal_plate_mail;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(5000);
        I.Material = Materials.crystal;
        I.Essence = ArmourEssence3;
        I.Price = Gold.FromCoins(820);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour);
        I.SetArmour(Skills.heavy_armour, D: 8, P: +1, S: +0, B: -1);
        I.AddObviousIngestUse(Motions.eat, 450, Delay.FromTurns(50), Sonics.armour);
      });

      mithril_plate_mail = AddArmour(ItemType.Suit, "mithril plate mail", I =>
      {
        I.Description = null;
        //I.SetAppearance("ornate plate mail", "An intricately designed suit of lightweight, nearly impenetrable armour");
        I.Glyph = Glyphs.mithril_plate_mail;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(3500);
        I.Material = Materials.mithril;
        I.Essence = ArmourEssence3;
        I.Price = Gold.FromCoins(920);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour);
        I.SetArmour(Skills.medium_armour, D: 7, P: +0, S: +1, B: +0);
        I.AddObviousIngestUse(Motions.eat, 250, Delay.FromTurns(25), Sonics.armour);
      });

      elven_mithrilcoat = AddArmour(ItemType.Suit, "elven mithril-coat", I =>
      {
        I.Description = null;
        I.SetAppearance("fine mithril-coat", "A slender and delicate looking coat of mithril armour.");
        I.Glyph = Glyphs.elven_mithrilcoat;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.elf;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(1500);
        I.Material = Materials.mithril;
        I.Essence = ArmourEssence3;
        I.Price = Gold.FromCoins(240);
        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(20), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.light_armour, D: 5, P: +0, S: +1, B: +0);
      });

      dark_elven_mithrilcoat = AddArmour(ItemType.Suit, "dark elven mithril-coat", I =>
      {
        I.Description = null;
        I.SetAppearance("glossy mithril-coat", "A slender and glossy looking coat of mithril armour.");
        I.Glyph = Glyphs.dark_elven_mithrilcoat;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.elf;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(1500);
        I.Material = Materials.mithril;
        I.Essence = ArmourEssence3;
        I.Price = Gold.FromCoins(240);
        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(20), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.light_armour, D: 5, P: +0, S: +0, B: +1);
      });

      dwarvish_mithrilcoat = AddArmour(ItemType.Suit, "dwarvish mithril-coat", I =>
      {
        I.Description = null;
        I.SetAppearance("stout mithril-coat", "A short but sturdy looking coat of mithril armour.");
        I.Glyph = Glyphs.dwarvish_mithrilcoat;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.dwarf;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(2500);
        I.Material = Materials.mithril;
        I.Essence = ArmourEssence3;
        I.Price = Gold.FromCoins(240);
        I.AddObviousIngestUse(Motions.eat, 250, Delay.FromTurns(25), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.medium_armour, D: 6, P: +1, S: +0, B: +0);
      });

      dented_pot = AddArmour(ItemType.Helmet, "dented pot", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.dented_pot;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(8);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.medium_armour, 1);
      });

      dwarvish_cloak = AddArmour(ItemType.Cloak, "dwarvish cloak", I =>
      {
        I.Description = null;
        I.SetAppearance("hooded cloak", null);
        I.Glyph = Glyphs.dwarvish_cloak;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.dwarf;
        I.Series = null;
        I.Rarity = 8;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.light_armour, 1);
      });

      dwarvish_iron_helm = AddArmour(ItemType.Helmet, "dwarvish iron helm", I =>
      {
        I.Description = null;
        I.SetAppearance("hard hat", null);
        I.Glyph = Glyphs.dwarvish_iron_helm;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.dwarf;
        I.Series = null;
        I.Rarity = 6;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(20);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(40), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.heavy_armour, 2);
      });

      dwarvish_roundshield = AddArmour(ItemType.Shield, "dwarvish roundshield", I =>
      {
        I.Description = null;
        I.SetAppearance("large round shield", null);
        I.Glyph = Glyphs.dwarvish_roundshield;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.dwarf;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(1000);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(10), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.heavy_armour, 2);
      });

      elven_boots = AddArmour(ItemType.Boots, "elven boots", I =>
      {
        I.Description = null;
        I.SetAppearance("mud boots", null);
        I.Glyph = Glyphs.mud_boots;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.elf;
        I.Series = BootsSeries;
        I.Rarity = 12;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(150);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(8);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(20), Sonics.armour, A =>
        {
          A.ApplyTransient(Properties.stealth, 10.d100());
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.armour)
         .SetTalent(Properties.stealth);
        I.SetArmour(Skills.light_armour, 1);
      });

      elven_cloak = AddArmour(ItemType.Cloak, "elven cloak", I =>
      {
        I.Description = null;
        I.SetAppearance("faded pall", null);
        I.Glyph = Glyphs.elven_cloak;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.elf;
        I.Series = null;
        I.Rarity = 8;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(60);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour, A =>
        {
          A.ApplyTransient(Properties.stealth, 10.d100());
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.stealth);
        I.SetArmour(Skills.light_armour, 1);
      });

      elven_leather_helm = AddArmour(ItemType.Helmet, "elven leather helm", I =>
      {
        I.Description = null;
        I.SetAppearance("leather hat", null);
        I.Glyph = Glyphs.elven_leather_helm;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.elf;
        I.Series = null;
        I.Rarity = 6;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(8);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(30), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.light_armour, 1);
      });

      elven_shield = AddArmour(ItemType.Shield, "elven shield", I =>
      {
        I.Description = null;
        I.SetAppearance("blue and green shield", null);
        I.Glyph = Glyphs.elven_shield;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.elf;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.wood;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(7);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(10), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.medium_armour, 2);
      });

      fedora = AddArmour(ItemType.Helmet, "fedora", I =>
      {
        I.Description = "A soft-brimmed hat, somewhat out of fashion.";
        I.Glyph = Glyphs.fedora;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence5;
        I.Price = Gold.FromCoins(1);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetBoostAttribute(Attributes.charisma);
        I.SetArmour(Skills.light_armour, 0);
      });

      fumble_boots = AddArmour(ItemType.Boots, "fumble boots", I =>
      {
        I.Description = null;
        I.SetAppearance("riding boots", null);
        I.Glyph = Glyphs.riding_boots;
        I.Sonic = Sonics.armour;
        I.Series = BootsSeries;
        I.Rarity = 12;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(30);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour, A =>
        {
          A.DecreaseAbility(Attributes.dexterity, Dice.One);
        });
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.armour)
         .SetTalent(Properties.fumbling);
        I.SetArmour(Skills.light_armour, 1);
      });

      gauntlets_of_dexterity = AddArmour(ItemType.Gloves, "gauntlets of dexterity", I =>
      {
        I.Description = null;
        I.SetAppearance("fencing gloves", null);
        I.Glyph = Glyphs.fencing_gloves;
        I.Sonic = Sonics.armour;
        I.Series = GlovesSeries;
        I.Rarity = 8;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence5;
        I.Price = Gold.FromCoins(50);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetBoostAttribute(Attributes.dexterity);
        I.SetArmour(Skills.light_armour, 1);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour, A =>
        {
          A.IncreaseAbility(Attributes.dexterity, Dice.One);
        });
      });

      gauntlets_of_fumbling = AddArmour(ItemType.Gloves, "gauntlets of fumbling", I =>
      {
        I.Description = null;
        I.SetAppearance("padded gloves", null);
        I.Glyph = Glyphs.padded_gloves;
        I.Sonic = Sonics.armour;
        I.Series = GlovesSeries;
        I.Rarity = 8;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(50);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.fumbling);
        I.SetArmour(Skills.light_armour, 1);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour, A =>
        {
          A.DecreaseAbility(Attributes.dexterity, Dice.One);
        });
      });

      gauntlets_of_phasing = AddArmour(ItemType.Gloves, "gauntlets of phasing", I =>
      {
        I.Description = null;
        I.SetAppearance("black gloves", null);
        I.Glyph = Glyphs.black_gloves;
        I.Sonic = Sonics.armour;
        I.Series = GlovesSeries;
        I.Rarity = 6;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence5;
        I.Price = Gold.FromCoins(50);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.phasing);
        I.SetArmour(Skills.light_armour, 1);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour, A =>
        {
          A.Polymorph(Entities.ghost);
          A.SummonEntity(4.d4(), Entities.phase_spider);
        });
      });

      gauntlets_of_power = AddArmour(ItemType.Gloves, "gauntlets of power", I =>
      {
        I.Description = null;
        I.SetAppearance("riding gloves", null);
        I.Glyph = Glyphs.riding_gloves;
        I.Sonic = Sonics.armour;
        I.Series = GlovesSeries;
        I.Rarity = 8;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence5;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour, A =>
        {
          A.IncreaseAbility(Attributes.strength, Dice.One);
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetBoostAttribute(Attributes.strength);
        I.SetArmour(Skills.heavy_armour, 1);
      });

      hawaiian_shirt = AddArmour(ItemType.Shirt, "hawaiian shirt", I =>
      {
        I.Description = "Bold, or just tacky?";
        I.Glyph = Glyphs.hawaiian_shirt;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 8;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(3);
        I.AddObviousIngestUse(Motions.eat, 5, Delay.FromTurns(10), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.light_armour, 0);
      });

      helm_of_brilliance = AddArmour(ItemType.Helmet, "helm of brilliance", I =>
      {
        I.Description = null;
        I.SetAppearance("etched helmet", null);
        I.Glyph = Glyphs.etched_helmet;
        I.Sonic = Sonics.armour;
        I.Series = HelmSeries;
        I.Rarity = 6;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence5;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(40), Sonics.armour, A =>
        {
          A.IncreaseAbility(Attributes.intelligence, Dice.One);
          A.IncreaseAbility(Attributes.wisdom, Dice.One);
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetBoostAttribute(Attributes.intelligence, Attributes.wisdom);
        I.SetArmour(Skills.heavy_armour, 1);
      });

      helm_of_weakness = AddArmour(ItemType.Helmet, "helm of weakness", I =>
      {
        I.Description = null;
        I.SetAppearance("crested helmet", null);
        I.Glyph = Glyphs.crested_helmet;
        I.Sonic = Sonics.armour;
        I.Series = HelmSeries;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.DefaultSanctity = Sanctities.Cursed;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(40), Sonics.armour, A =>
        {
          A.DecreaseAbility(Attributes.strength, Dice.One);
          A.DecreaseAbility(Attributes.constitution, Dice.One);
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetCrippleAttribute(Attributes.strength, Attributes.constitution);
        I.SetArmour(Skills.heavy_armour, 1);
      });

      helm_of_telepathy = AddArmour(ItemType.Helmet, "helm of telepathy", I =>
      {
        I.Description = null;
        I.SetAppearance("visored helmet", null);
        I.Glyph = Glyphs.visored_helmet;
        I.Sonic = Sonics.armour;
        I.Series = HelmSeries;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(40), Sonics.armour, A =>
        {
          A.MajorProperty(Properties.telepathy);
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.telepathy);
        I.SetArmour(Skills.heavy_armour, 1);
      });

      mithril_helmet = AddArmour(ItemType.Helmet, "mithril helmet", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.mithril_helmet;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.mithril;
        I.Essence = ArmourEssence3;
        I.Price = Gold.FromCoins(100);
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(40), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.medium_armour, 2);
      });

      helmet = AddArmour(ItemType.Helmet, "helmet", I =>
      {
        I.Description = "Standard issue protective metal headgear.";
        I.SetAppearance("plumed helmet", null);
        I.Glyph = Glyphs.plumed_helmet;
        I.Sonic = Sonics.armour;
        I.Series = HelmSeries;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(50), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.heavy_armour, 1);
      });

      high_boots = AddArmour(ItemType.Boots, "high boots", I =>
      {
        I.Description = "A pair of sturdy leather boots which extend upward until just below the knee.";
        I.SetAppearance("jackboots", null);
        I.Glyph = Glyphs.high_boots;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 15;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(12);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.armour);
        I.SetArmour(Skills.medium_armour, 2);
      });

      iron_shoes = AddArmour(ItemType.Boots, "iron shoes", I =>
      {
        I.Description = null;
        I.SetAppearance("hard shoes", null);
        I.Glyph = Glyphs.iron_shoes;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 7;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(16);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(50), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.armour);
        I.SetArmour(Skills.heavy_armour, 2);
      });

      jumping_boots = AddArmour(ItemType.Boots, "jumping boots", I =>
      {
        I.Description = null;
        I.SetAppearance("hiking boots", null);
        I.Glyph = Glyphs.hiking_boots;
        I.Sonic = Sonics.armour;
        I.Series = BootsSeries;
        I.Rarity = 12;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour, A =>
        {
          A.MajorProperty(Properties.jumping);
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.armour)
         .SetTalent(Properties.jumping);
        I.SetArmour(Skills.medium_armour, 1);
      });

      panic_boots = AddArmour(ItemType.Boots, "panic boots", I =>
      {
        I.Description = null;
        I.SetAppearance("steel boots", null);
        I.Glyph = Glyphs.steel_boots;
        I.Sonic = Sonics.armour;
        I.Series = BootsSeries;
        I.Rarity = 6;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(30);
        I.DefaultSanctity = Sanctities.Cursed;
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour, A =>
        {
          A.DecreaseAbility(Attributes.wisdom, Dice.One);
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.armour)
         .SetTalent(Properties.fear);
        I.SetArmour(Skills.medium_armour, 1);
      });

      winged_boots = AddArmour(ItemType.Boots, "winged boots", I =>
      {
        I.Description = null;
        I.SetAppearance("jungle boots", null);
        I.Glyph = Glyphs.jungle_boots;
        I.Sonic = Sonics.armour;
        I.Series = BootsSeries;
        I.Rarity = 4;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence5;
        I.Price = Gold.FromCoins(60);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour, A =>
        {
          A.MajorProperty(Properties.flight);
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.armour)
         .SetTalent(Properties.flight);
        I.SetArmour(Skills.medium_armour, 1);
      });

      lab_coat = AddArmour(ItemType.Cloak, "lab coat", I =>
      {
        I.Description = null;
        I.SetAppearance("white coat", null);
        I.Glyph = Glyphs.lab_coat;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(60);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour, A =>
        {
          A.ConsumeResistance(Elements.poison);
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetResistance(Elements.poison);
        I.SetArmour(Skills.light_armour, 1);
      });

      large_shield = AddArmour(ItemType.Shield, "large shield", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.large_shield;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 7;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1000);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(50), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.heavy_armour, 2);
      });

      mithril_shield = AddArmour(ItemType.Shield, "mithril shield", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.mithril_shield;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.mithril;
        I.Essence = ArmourEssence3;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(50), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.medium_armour, 2);
      });

      leather_armour = AddArmour(ItemType.Suit, "leather armour", I =>
      {
        I.Description = "Boiled leather, moulded into a bodily shape to be donned as light armour.";
        I.Glyph = Glyphs.leather_armour;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 45;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(1500);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(5);
        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(30), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(30), Sonics.armour);
        I.SetArmour(Skills.light_armour, D: 2, P: +0, S: +0, B: +0);
      });

      leather_cloak = AddArmour(ItemType.Cloak, "leather cloak", I =>
      {
        I.Description = "An article of leather clothing covering the torso that hangs around the neck and on the shoulders.";
        I.Glyph = Glyphs.leather_cloak;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 8;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(150);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(40);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(15), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.light_armour, 1);
      });

      leather_gloves = AddArmour(ItemType.Gloves, "leather gloves", I =>
      {
        I.Description = "Sturdy leather garments fitted firm onto the hands, sheathing each finger individually.";
        I.SetAppearance("old gloves", null);
        I.Glyph = Glyphs.leather_gloves;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 16;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(8);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.light_armour, 1);
      });

      leather_jacket = AddArmour(ItemType.Suit, "leather jacket", I =>
      {
        I.Description = "A thick leather coat, constructed to withstand weather and wear.";
        I.Glyph = Glyphs.leather_jacket;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 11;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(30), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.light_armour, D: 1, P: +0, S: +0, B: +0);
      });

      levitation_boots = AddArmour(ItemType.Boots, "levitation boots", I =>
      {
        I.Description = null;
        I.SetAppearance("snow boots", null);
        I.Glyph = Glyphs.snow_boots;
        I.Sonic = Sonics.armour;
        I.Series = BootsSeries;
        I.Rarity = 12;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(150);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(30);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(15), Sonics.armour);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.armour)
         .SetTalent(Properties.levitation);
        I.SetArmour(Skills.light_armour, 1);
      });

      low_boots = AddArmour(ItemType.Boots, "low boots", I =>
      {
        I.Description = "A pair of sturdy leather boots which extend upwards to just above the ankle.";
        I.SetAppearance("walking shoes", null);
        I.Glyph = Glyphs.low_boots;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 25;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(8);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.armour);
        I.SetArmour(Skills.light_armour, 1);
      });

      mummy_wrapping = AddArmour(ItemType.Cloak, "mummy wrapping", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.mummy_wrapping;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(2);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.light_armour, 0);
      });

      oilskin_cloak = AddArmour(ItemType.Cloak, "oilskin cloak", I =>
      {
        I.Description = "An article of waterproof cotton clothing covering the torso that hangs around the neck and on the shoulders.";
        I.SetAppearance("slippery cloak", null);
        I.Glyph = Glyphs.oilskin_cloak;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 8;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.slippery);
        I.SetArmour(Skills.light_armour, 1);

        // TODO: needs an actual mechanic.
      });

      orcish_chain_mail = AddArmour(ItemType.Suit, "orcish chain mail", I =>
      {
        I.Description = null;
        I.SetAppearance("crude chain mail", null);
        I.Glyph = Glyphs.orcish_chain_mail;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.orc;
        I.Series = null;
        I.Rarity = 20;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(3000);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(75);
        I.AddObviousIngestUse(Motions.eat, 300, Delay.FromTurns(50), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour);
        I.SetArmour(Skills.medium_armour, D: 4, P: +0, S: +1, B: +0);
      });

      orcish_cloak = AddArmour(ItemType.Cloak, "orcish cloak", I =>
      {
        I.Description = null;
        I.SetAppearance("coarse mantelet", null);
        I.Glyph = Glyphs.orcish_cloak;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.orc;
        I.Series = null;
        I.Rarity = 8;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(40);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.light_armour, 1);
      });

      orcish_helm = AddArmour(ItemType.Helmet, "orcish helm", I =>
      {
        I.Description = null;
        I.SetAppearance("iron skull cap", null);
        I.Glyph = Glyphs.orcish_helm;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.orc;
        I.Series = null;
        I.Rarity = 6;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(30), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.medium_armour, 1);
      });

      orcish_ring_mail = AddArmour(ItemType.Suit, "orcish ring mail", I =>
      {
        I.Description = null;
        I.SetAppearance("crude ring mail", null);
        I.Glyph = Glyphs.orcish_ring_mail;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.orc;
        I.Series = null;
        I.Rarity = 20;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(2500);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 250, Delay.FromTurns(50), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour);
        I.SetArmour(Skills.medium_armour, D: 2, P: +0, S: +0, B: +0);
      });

      orcish_shield = AddArmour(ItemType.Shield, "orcish shield", I =>
      {
        I.Description = null;
        I.SetAppearance("red-eyed shield", null);
        I.Glyph = Glyphs.orcish_shield;
        I.Sonic = Sonics.armour;
        I.OriginRace = Races.orc;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(7);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(20), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.medium_armour, 1);
      });

      ring_mail = AddArmour(ItemType.Suit, "ring mail", I =>
      {
        I.Description = "A suit of armour made from metal rings sewn onto a foundation of leather or cloth.";
        I.Glyph = Glyphs.ring_mail;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 40;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(2500);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(60);
        I.AddObviousIngestUse(Motions.eat, 250, Delay.FromTurns(50), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour);
        I.SetArmour(Skills.medium_armour, D: 3, P: -1, S: +1, B: +0);
      });

      robe = AddArmour(ItemType.Suit, "robe", I =>
      {
        I.Description = "A long, flowing garment, with long sleeves and a hood.";
        I.SetAppearance("plain robe", null, Price: RobePrice);
        I.Glyph = Glyphs.plain_robe;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(25);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(40), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.light_armour, D: 1, P: +0, S: +0, B: +0);
      });

      battle_robe = AddArmour(ItemType.Suit, "battle robe", I =>
      {
        I.Description = "A long, flowing garment, with long sleeves and a hood.";
        I.SetAppearance("red robe", null, Price: RobePrice);
        I.Glyph = Glyphs.red_robe;
        I.Sonic = Sonics.armour;
        I.Series = RobesSeries;
        I.Rarity = 1;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(250);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetAttackBoost()
         .SetDamageBoost();
        I.SetArmour(Skills.light_armour, D: 1, P: +0, S: +0, B: +0);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(40), Sonics.armour, T =>
        {
          T.WhenProbability(Table =>
          {
            Table.Add(1, A => A.MajorProperty(Properties.conflict));
            Table.Add(1, A => A.MajorProperty(Properties.aggravation));
            Table.Add(1, A => A.MajorProperty(Properties.berserking));
          });
        });
      });

      fleet_robe = AddArmour(ItemType.Suit, "fleet robe", I =>
      {
        I.Description = "A long, flowing garment, with long sleeves and a hood.";
        I.SetAppearance("blue robe", null, Price: RobePrice);
        I.Glyph = Glyphs.blue_robe;
        I.Sonic = Sonics.armour;
        I.Series = RobesSeries;
        I.Rarity = 1;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(250);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.quickness)
         .SetDefenceBoost();
        I.SetArmour(Skills.light_armour, D: 1, P: +0, S: +0, B: +0);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(40), Sonics.armour, T =>
        {
          T.WhenProbability(Table =>
          {
            Table.Add(1, A => A.MajorProperty(Properties.quickness));
            Table.Add(1, A => A.MajorProperty(Properties.deflection));
          });
        });
      });

      elemental_robe = AddArmour(ItemType.Suit, "elemental robe", I =>
      {
        I.Description = "A long, flowing garment, with long sleeves and a hood.";
        I.SetAppearance("green robe", null, Price: RobePrice);
        I.Glyph = Glyphs.green_robe;
        I.Sonic = Sonics.armour;
        I.Series = RobesSeries;
        I.Rarity = 1;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(250);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(40), Sonics.armour, T =>
        {
          T.WhenProbability(Table =>
          {
            Table.Add(1, A => A.ConsumeResistance(Elements.fire));
            Table.Add(1, A => A.ConsumeResistance(Elements.cold));
            Table.Add(1, A => A.ConsumeResistance(Elements.shock));
            Table.Add(1, A => A.ConsumeResistance(Elements.poison));
          });
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetResistance(Elements.fire, Elements.cold, Elements.shock, Elements.poison);
        I.SetArmour(Skills.light_armour, D: 1, P: +0, S: +0, B: +0);
      });

      hermit_robe = AddArmour(ItemType.Suit, "hermit robe", I =>
      {
        I.Description = "A long, flowing garment, with long sleeves and a hood.";
        I.SetAppearance("orange robe", null, Price: RobePrice);
        I.Glyph = Glyphs.orange_robe;
        I.Sonic = Sonics.armour;
        I.Series = RobesSeries;
        I.Rarity = 1;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence4;
        I.Price = Gold.FromCoins(50);
        I.DefaultSanctity = Sanctities.Cursed;
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(40), Sonics.armour, A =>
        {
          A.DecreaseAbility(Attributes.charisma, Dice.One);
          A.DecreaseAbility(Attributes.dexterity, Dice.One);
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetCrippleAttribute(Attributes.charisma, Attributes.dexterity);
        I.SetArmour(Skills.light_armour, D: 1, P: +0, S: +0, B: +0);
      });

      scale_mail = AddArmour(ItemType.Suit, "scale mail", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.scale_mail;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 40;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(2500);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(65);
        I.AddObviousIngestUse(Motions.eat, 250, Delay.FromTurns(50), Sonics.armour);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour);
        I.SetArmour(Skills.medium_armour, D: 4, P: +1, S: -1, B: +0);
      });

      shield_of_reflection = AddArmour(ItemType.Shield, "shield of reflection", I =>
      {
        I.Description = null;
        I.SetAppearance("polished silver shield", null);
        I.Glyph = Glyphs.shield_of_reflection;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.silver;
        I.Essence = ArmourEssence6;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(30), Sonics.armour, A =>
        {
          A.MajorProperty(Properties.reflection);
        });
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour)
         .SetTalent(Properties.reflection);
        I.SetArmour(Skills.heavy_armour, 2);
      });

      small_shield = AddArmour(ItemType.Shield, "small shield", I =>
      {
        I.Description = "A lightweight protective panel strapped to the forearm, made from wood with metal reinforcement.";
        I.Glyph = Glyphs.small_shield;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 6;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.wood;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(3);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.light_armour, 1);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(30), Sonics.armour);
      });

      disencumbrance_boots = AddArmour(ItemType.Boots, "disencumbrance boots", I =>
      {
        I.Description = null;
        I.SetAppearance("fur boots", null);
        I.Glyph = Glyphs.fur_boots;
        I.Sonic = Sonics.armour;
        I.Series = BootsSeries;
        I.Rarity = 15;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence5;
        I.Price = Gold.FromCoins(50);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.armour)
         .SetEncumbranceBoost();
        I.SetArmour(Skills.medium_armour, 1);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour, A =>
        {
          A.IncreaseAbility(Attributes.strength, Dice.One);
          A.Polymorph(Entities.elephant);
        });
      });

      speed_boots = AddArmour(ItemType.Boots, "speed boots", I =>
      {
        I.Description = null;
        I.SetAppearance("combat boots", null);
        I.Glyph = Glyphs.combat_boots;
        I.Sonic = Sonics.armour;
        I.Series = BootsSeries;
        I.Rarity = 12;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence6;
        I.Price = Gold.FromCoins(50);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.armour) 
         .SetSpeedBoost();
        I.SetArmour(Skills.medium_armour, 1);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.armour, A =>
        {
          A.MajorProperty(Properties.quickness);
        });
      });

      splint_mail = AddArmour(ItemType.Suit, "splint mail", I =>
      {
        I.Description = "A heavy suit of armour made up of many strips of metal.";
        I.Glyph = Glyphs.splint_mail;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 35;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(4000);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(80);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour);
        I.SetArmour(Skills.heavy_armour, D: 6, P: -1, S: +1, B: +0);
        I.AddObviousIngestUse(Motions.eat, 400, Delay.FromTurns(40), Sonics.armour);
      });

      studded_leather_armour = AddArmour(ItemType.Suit, "studded leather armour", I =>
      {
        I.Description = "Leather armour with close-set studs so as to offer more protection.";
        I.Glyph = Glyphs.studded_leather_armour;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 40;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(2000);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(15);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(30), Sonics.armour);
        I.SetArmour(Skills.light_armour, D: 3, P: +1, S: +0, B: +0);
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(20), Sonics.armour);
      });

      tshirt = AddArmour(ItemType.Shirt, "t-shirt", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.tshirt;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.cloth;
        I.Essence = ArmourEssence1;
        I.Price = Gold.FromCoins(2);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.armour);
        I.SetArmour(Skills.light_armour, 0);
        I.AddObviousIngestUse(Motions.eat, 5, Delay.FromTurns(10), Sonics.armour);
      });

      // barding.
      saddle = AddArmour(ItemType.Barding, "saddle", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.saddle;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 20;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(1500);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(50);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.armour);
        I.SetArmour(Skills.light_armour, D: 1, P: +0, S: +0, B: +0);
        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(20), Sonics.armour);
      });

      leather_barding = AddArmour(ItemType.Barding, "leather barding", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.leather_barding;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 15;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(2500);
        I.Material = Materials.leather;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(150);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(30), Sonics.armour);
        I.SetArmour(Skills.light_armour, D: 3, P: +0, S: +0, B: -1);
        I.AddObviousIngestUse(Motions.eat, 250, Delay.FromTurns(30), Sonics.armour);
      });

      chain_barding = AddArmour(ItemType.Barding, "chain barding", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.chain_barding;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(6000);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(300);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(40), Sonics.armour);
        I.SetArmour(Skills.medium_armour, D: 5, P: -1, S: +1, B: +0);
        I.AddObviousIngestUse(Motions.eat, 600, Delay.FromTurns(40), Sonics.armour);
      });

      mithril_barding = AddArmour(ItemType.Barding, "mithril barding", I =>
      {
        I.Description = null;
        //I.SetAppearance("", null);
        I.Glyph = Glyphs.mithril_barding;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(3500);
        I.Material = Materials.mithril;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(500);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(40), Sonics.armour);
        I.SetArmour(Skills.light_armour, D: 5, P: +0, S: +0, B: +0);
        I.AddObviousIngestUse(Motions.eat, 700, Delay.FromTurns(40), Sonics.armour);
      });

      plate_barding = AddArmour(ItemType.Barding, "plate barding", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.plate_barding;
        I.Sonic = Sonics.armour;
        I.Series = null;
        I.Rarity = 5;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(9000);
        I.Material = Materials.iron;
        I.Essence = ArmourEssence2;
        I.Price = Gold.FromCoins(700);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour);
        I.SetArmour(Skills.heavy_armour, D: 7, P: +0, S: +1, B: -1);
        I.AddObviousIngestUse(Motions.eat, 900, Delay.FromTurns(50), Sonics.armour);
      });

      // dragon scales/mail.
      var DragonArmourEatDelay = Delay.FromTurns(40);
      var DragonArmourNutrition = 400;

      Item AddDragonScaleMailArmour(string Name, Element Element, Action<ItemEditor> EditorAction)
      {
        return AddArmour(ItemType.Suit, Name, I =>
        {
          I.Sonic = Sonics.armour;
          I.Series = null;
          I.Rarity = 0;
          I.Size = Size.Large;
          I.Weight = Weight.FromUnits(600);
          I.Material = Materials.hide;
          I.Essence = Essence.FromUnits(100);
          I.SetArmour(Skills.light_armour, D: 6, P: +1, S: +0, B: +0);

          if (Element != null)
          {
            I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour)
             .SetResistance(Element);
            I.AddObviousIngestUse(Motions.eat, DragonArmourNutrition, DragonArmourEatDelay, Sonics.armour, A =>
            {
              A.MinorResistance(Element);
            });
          }

          EditorAction(I);
        });
      }
      Item AddDragonScalesArmour(string Name, Element Element, Action<ItemEditor> EditorAction)
      {
        var Result = AddArmour(ItemType.Suit, Name, I =>
        {
          I.Sonic = Sonics.armour;
          I.Series = null;
          I.Rarity = 0;
          I.Size = Size.Medium;
          I.Weight = Weight.FromUnits(400);
          I.Material = Materials.hide;
          I.Essence = Essence.FromUnits(50);
          I.SetArmour(Skills.light_armour, D: 3, P: +1, S: +0, B: +0);

          if (Element != null)
          {
            I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour)
             .SetResistance(Element);
            I.AddObviousIngestUse(Motions.eat, DragonArmourNutrition, DragonArmourEatDelay, Sonics.armour, A =>
            {
              A.MinorResistance(Element);
            });
          }

          EditorAction(I);
        });
        
        DragonScalesArmourList.Add(Result);

        return Result;
      }

      black_dragon_scale_mail = AddDragonScaleMailArmour("black dragon scale mail", Elements.acid, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.black_dragon_scale_mail;
        I.Price = Gold.FromCoins(1200);
        I.SetDerivative(Entities.adult_black_dragon);
      });

      black_dragon_scales = AddDragonScalesArmour("black dragon scales", Elements.acid, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.black_dragon_scales;
        I.Price = Gold.FromCoins(700);
        I.SetDerivative(Entities.adult_black_dragon);
      });

      blue_dragon_scale_mail = AddDragonScaleMailArmour("blue dragon scale mail", Elements.shock, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.blue_dragon_scale_mail;
        I.Price = Gold.FromCoins(900);
        I.SetDerivative(Entities.adult_blue_dragon);
      });

      blue_dragon_scales = AddDragonScalesArmour("blue dragon scales", Elements.shock, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.blue_dragon_scales;
        I.Price = Gold.FromCoins(500);
        I.SetDerivative(Entities.adult_blue_dragon);
      });

      deep_dragon_scale_mail = AddDragonScaleMailArmour("deep dragon scale mail", Elements.drain, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.deep_dragon_scale_mail;
        I.Price = Gold.FromCoins(1200);
        I.SetDerivative(Entities.adult_deep_dragon);
      });

      deep_dragon_scales = AddDragonScalesArmour("deep dragon scales", Elements.drain, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.deep_dragon_scales;
        I.Price = Gold.FromCoins(500);
        I.SetDerivative(Entities.adult_deep_dragon);
      });

      gold_dragon_scale_mail = AddDragonScaleMailArmour("gold dragon scale mail", Elements.magical, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.gold_dragon_scale_mail;
        I.Price = Gold.FromCoins(1200);
        I.SetDerivative(Entities.adult_gold_dragon);
      });

      gold_dragon_scales = AddDragonScalesArmour("gold dragon scales", Elements.magical, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.gold_dragon_scales;
        I.Price = Gold.FromCoins(700);
        I.SetDerivative(Entities.adult_gold_dragon);
      });

      green_dragon_scale_mail = AddDragonScaleMailArmour("green dragon scale mail", Elements.poison, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.green_dragon_scale_mail;
        I.Price = Gold.FromCoins(900);
        I.SetDerivative(Entities.adult_green_dragon);
      });

      green_dragon_scales = AddDragonScalesArmour("green dragon scales", Elements.poison, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.green_dragon_scales;
        I.Price = Gold.FromCoins(500);
        I.SetDerivative(Entities.adult_green_dragon);
      });

      orange_dragon_scale_mail = AddDragonScaleMailArmour("orange dragon scale mail", Elements.sleep, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.orange_dragon_scale_mail;
        I.Price = Gold.FromCoins(900);
        I.SetDerivative(Entities.adult_orange_dragon);
      });

      orange_dragon_scales = AddDragonScalesArmour("orange dragon scales", Elements.sleep, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.orange_dragon_scales;
        I.Price = Gold.FromCoins(500);
        I.SetDerivative(Entities.adult_orange_dragon);
      });

      red_dragon_scale_mail = AddDragonScaleMailArmour("red dragon scale mail", Elements.fire, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.red_dragon_scale_mail;
        I.Price = Gold.FromCoins(900);
        I.SetDerivative(Entities.adult_red_dragon);
      });

      red_dragon_scales = AddDragonScalesArmour("red dragon scales", Elements.fire, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.red_dragon_scales;
        I.Price = Gold.FromCoins(500);
        I.SetDerivative(Entities.adult_red_dragon);
      });

      shimmering_dragon_scale_mail = AddDragonScaleMailArmour("shimmering dragon scale mail", Element: null, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.shimmering_dragon_scale_mail;
        I.Price = Gold.FromCoins(1200);
        I.SetDerivative(Entities.adult_shimmering_dragon);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour)
         .SetTalent(Properties.displacement);
        I.AddObviousIngestUse(Motions.eat, DragonArmourNutrition, DragonArmourEatDelay, Sonics.armour, A =>
        {
          A.ApplyTransient(Properties.displacement, 10.d100());
        });
      });

      shimmering_dragon_scales = AddDragonScalesArmour("shimmering dragon scales", Element: null, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.shimmering_dragon_scales;
        I.Price = Gold.FromCoins(700);
        I.SetDerivative(Entities.adult_shimmering_dragon);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour)
         .SetTalent(Properties.displacement);
        I.AddObviousIngestUse(Motions.eat, DragonArmourNutrition, DragonArmourEatDelay, Sonics.armour, A =>
        {
          A.ApplyTransient(Properties.displacement, 10.d100());
        });
      });

      silver_dragon_scale_mail = AddDragonScaleMailArmour("silver dragon scale mail", Element: null, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.silver_dragon_scale_mail;
        I.Price = Gold.FromCoins(1200);
        I.SetDerivative(Entities.adult_silver_dragon);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour)
         .SetTalent(Properties.reflection);
        I.AddObviousIngestUse(Motions.eat, DragonArmourNutrition, DragonArmourEatDelay, Sonics.armour, A =>
        {
          A.ApplyTransient(Properties.reflection, 10.d100());
        });
      });

      silver_dragon_scales = AddDragonScalesArmour("silver dragon scales", Element: null, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.silver_dragon_scales;
        I.Price = Gold.FromCoins(700);
        I.SetDerivative(Entities.adult_silver_dragon);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(50), Sonics.armour)
         .SetTalent(Properties.reflection);
        I.AddObviousIngestUse(Motions.eat, DragonArmourNutrition, DragonArmourEatDelay, Sonics.armour, A =>
        {
          A.ApplyTransient(Properties.reflection, 10.d100());
        });
      });

      white_dragon_scale_mail = AddDragonScaleMailArmour("white dragon scale mail", Elements.cold, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.white_dragon_scale_mail;
        I.Price = Gold.FromCoins(900);
        I.SetDerivative(Entities.adult_white_dragon);
      });

      white_dragon_scales = AddDragonScalesArmour("white dragon scales", Elements.cold, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.white_dragon_scales;
        I.Price = Gold.FromCoins(500);
        I.SetDerivative(Entities.adult_white_dragon);
      });

      yellow_dragon_scale_mail = AddDragonScaleMailArmour("yellow dragon scale mail", Elements.disintegrate, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.yellow_dragon_scale_mail;
        I.Price = Gold.FromCoins(900);
        I.SetDerivative(Entities.adult_yellow_dragon);
      });

      yellow_dragon_scales = AddDragonScalesArmour("yellow dragon scales", Elements.disintegrate, I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.yellow_dragon_scales;
        I.Price = Gold.FromCoins(500);
        I.SetDerivative(Entities.adult_yellow_dragon);
      });
            
      SetUpgradeDowngradePair(dunce_cap, cornuthaum);

      SetUpgradeDowngradePair(black_dragon_scales, black_dragon_scale_mail);
      SetUpgradeDowngradePair(blue_dragon_scales, blue_dragon_scale_mail);
      SetUpgradeDowngradePair(deep_dragon_scales, deep_dragon_scale_mail);
      SetUpgradeDowngradePair(gold_dragon_scales, gold_dragon_scale_mail);
      SetUpgradeDowngradePair(green_dragon_scales, green_dragon_scale_mail);
      SetUpgradeDowngradePair(orange_dragon_scales, orange_dragon_scale_mail);
      SetUpgradeDowngradePair(red_dragon_scales, red_dragon_scale_mail);
      SetUpgradeDowngradePair(shimmering_dragon_scales, shimmering_dragon_scale_mail);
      SetUpgradeDowngradePair(silver_dragon_scales, silver_dragon_scale_mail);
      SetUpgradeDowngradePair(white_dragon_scales, white_dragon_scale_mail);
      SetUpgradeDowngradePair(yellow_dragon_scales, yellow_dragon_scale_mail);
      #endregion

      #region book.
      var BookSize = Size.Small;
      var BookWeight = Weight.FromUnits(500);
      var BookEssence0 = Essence.FromUnits(4);
      var BookSeries = new Series("book");
      var BookWeakness = new[] { Elements.fire };

      void AssignBookSpell(ItemEditor Item, Spell Spell, bool Fireproof = false)
      {
        Item.Sonic = Sonics.book;
        Item.Series = BookSeries;
        Item.Size = BookSize;
        Item.Weight = BookWeight;
        Item.Material = Materials.paper;
        Item.Essence = Essence.FromUnits((Spell.Level + 1) * 16);
        Item.Price = Gold.FromCoins(Spell.Level * 100);
        if (Fireproof)
          Item.SetWeakness();
        else
          Item.SetWeakness(BookWeakness);
        Item.AddLearnSpellUse(Motions.study, Delay.FromTurns(Spell.Level * 10), Sonics.magic, Spell);
      }

      book_of_blank_paper = AddBook("book of blank paper", I =>
      {
        I.Description = null;
        I.SetAppearance("plain book", null);
        I.Glyph = Glyphs.plain_book;
        I.Sonic = Sonics.book;
        I.Series = null;
        I.Rarity = 20;
        I.Size = BookSize;
        I.Weight = BookWeight;
        I.Material = Materials.paper;
        I.Essence = BookEssence0;
        I.Price = Gold.FromCoins(50);
        I.SetWeakness(BookWeakness);
        I.AddPretendUse(Motions.study, Delay.FromTurns(10), Sonics.magic, Use =>
        {
          Use.Apply.Nothing();
        });
        I.AddDiscoverIngestUse(Motions.eat, 400, Delay.FromTurns(40), Sonics.book);
      });

      book_of_acid_stream = AddBook("book of acid stream", I =>
      {
        I.Description = null;
        I.SetAppearance("colourful book", null);
        I.Glyph = Glyphs.colourful_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.acid_stream);
      });

      book_of_animate_dead = AddBook("book of animate dead", I =>
      {
        I.Description = null;
        I.SetAppearance("thick book", null);
        I.Glyph = Glyphs.thick_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.animate_dead);
      });

      book_of_animate_object = AddBook("book of animate object", I =>
      {
        I.Description = null;
        I.SetAppearance("spotted book", null);
        I.Glyph = Glyphs.spotted_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.animate_object);
      });

      book_of_poison_blast = AddBook("book of poison blast", I =>
      {
        I.Description = null;
        I.SetAppearance("tattered book", null);
        I.Glyph = Glyphs.tattered_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.poison_blast);
      });

      book_of_cancellation = AddBook("book of cancellation", I =>
      {
        I.Description = null;
        I.SetAppearance("shining book", null);
        I.Glyph = Glyphs.shining_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.cancellation);
      });

      book_of_fear = AddBook("book of fear", I =>
      {
        I.Description = null;
        I.SetAppearance("light blue book", null);
        I.Glyph = Glyphs.light_blue_book;
        I.Rarity = 20;
        AssignBookSpell(I, Codex.Spells.fear);
      });

      book_of_bind_undead = AddBook("book of bind undead", I =>
      {
        I.Description = null;
        I.SetAppearance("black book", null);
        I.Glyph = Glyphs.black_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.bind_undead);
      });

      book_of_charm = AddBook("book of charm", I =>
      {
        I.Description = null;
        I.SetAppearance("magenta book", null);
        I.Glyph = Glyphs.magenta_book;
        I.Rarity = 20;
        AssignBookSpell(I, Codex.Spells.charm);
      });

      book_of_clairvoyance = AddBook("book of clairvoyance", I =>
      {
        I.Description = null;
        I.SetAppearance("dark blue book", null);
        I.Glyph = Glyphs.dark_blue_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.clairvoyance);
      });

      book_of_cone_of_cold = AddBook("book of cone of cold", I =>
      {
        I.Description = null;
        I.SetAppearance("dog eared book", null);
        I.Glyph = Glyphs.dog_eared_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.cone_of_cold);
      });

      book_of_confusion = AddBook("book of confusion", I =>
      {
        I.Description = null;
        I.SetAppearance("orange book", null);
        I.Glyph = Glyphs.orange_book;
        I.Rarity = 25;
        AssignBookSpell(I, Codex.Spells.confusion);
      });

      book_of_create_familiar = AddBook("book of create familiar", I =>
      {
        I.Description = null;
        I.SetAppearance("glittering book", null);
        I.Glyph = Glyphs.glittering_book;
        I.Rarity = 10;
        AssignBookSpell(I, Codex.Spells.create_familiar);
      });

      book_of_living_wall = AddBook("book of living wall", I =>
      {
        I.Description = null;
        I.SetAppearance("deep book", null);
        I.Glyph = Glyphs.deep_book;
        I.Rarity = 10;
        AssignBookSpell(I, Codex.Spells.living_wall);
      });

      book_of_summoning = AddBook("book of summoning", I =>
      {
        I.Description = null;
        I.SetAppearance("turquoise book", null);
        I.Glyph = Glyphs.turquoise_book;
        I.Rarity = 25;
        AssignBookSpell(I, Codex.Spells.summoning);
      });

      book_of_curing = AddBook("book of curing", I =>
      {
        I.Description = null;
        I.SetAppearance("indigo book", null);
        I.Glyph = Glyphs.indigo_book;
        I.Rarity = 20;
        AssignBookSpell(I, Codex.Spells.curing);
      });

      book_of_detect_food = AddBook("book of detect food", I =>
      {
        I.Description = null;
        I.SetAppearance("cyan book", null);
        I.Glyph = Glyphs.cyan_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.detect_food);
      });

      book_of_detect_monsters = AddBook("book of detect monsters", I =>
      {
        I.Description = null;
        I.SetAppearance("leather book", null);
        I.Glyph = Glyphs.leather_book;
        I.Rarity = 20;
        AssignBookSpell(I, Codex.Spells.detect_monsters);
      });

      book_of_detect_treasure = AddBook("book of detect treasure", I =>
      {
        I.Description = null;
        I.SetAppearance("grey book", null);
        I.Glyph = Glyphs.grey_book;
        I.Rarity = 25;
        AssignBookSpell(I, Codex.Spells.detect_treasure);
      });

      book_of_detect_unseen = AddBook("book of detect unseen", I =>
      {
        I.Description = null;
        I.SetAppearance("violet book", null);
        I.Glyph = Glyphs.violet_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.detect_unseen);
      });

      book_of_dig = AddBook("book of dig", I =>
      {
        I.Description = null;
        I.SetAppearance("parchment book", null);
        I.Glyph = Glyphs.parchment_book;
        I.Rarity = 20;
        AssignBookSpell(I, Codex.Spells.dig);
      });

      book_of_disintegrate = AddBook("book of disintegrate", I =>
      {
        I.Description = null;
        I.SetAppearance("yellow book", null);
        I.Glyph = Glyphs.yellow_book;
        I.Rarity = 5;
        AssignBookSpell(I, Codex.Spells.disintegrate);
      });

      book_of_flaming_sphere = AddBook("book of flaming sphere", I =>
      {
        I.Description = null;
        I.SetAppearance("canvas book", null);
        I.Glyph = Glyphs.canvas_book;
        I.Rarity = 20;
        AssignBookSpell(I, Codex.Spells.flaming_sphere);
      });

      book_of_freezing_sphere = AddBook("book of freezing sphere", I =>
      {
        I.Description = null;
        I.SetAppearance("hardcover book", null);
        I.Glyph = Glyphs.hardcover_book;
        I.Rarity = 20;
        AssignBookSpell(I, Codex.Spells.freezing_sphere);
      });

      book_of_shocking_sphere = AddBook("book of shocking sphere", I =>
      {
        I.Description = null;
        I.SetAppearance("stylish book", null);
        I.Glyph = Glyphs.stylish_book;
        I.Rarity = 20;
        AssignBookSpell(I, Codex.Spells.shocking_sphere);
      });

      book_of_soaking_sphere = AddBook("book of soaking sphere", I =>
      {
        I.Description = null;
        I.SetAppearance("tiny book", null);
        I.Glyph = Glyphs.tiny_book;
        I.Rarity = 20;
        AssignBookSpell(I, Codex.Spells.soaking_sphere);
      });

      book_of_crushing_sphere = AddBook("book of crushing sphere", I =>
      {
        I.Description = null;
        I.SetAppearance("humongous book", null);
        I.Glyph = Glyphs.humongous_book;
        I.Rarity = 20;
        AssignBookSpell(I, Codex.Spells.crushing_sphere);
      });

      book_of_drain_life = AddBook("book of drain life", I =>
      {
        I.Description = null;
        I.SetAppearance("velvet book", null);
        I.Glyph = Glyphs.velvet_book;
        I.Rarity = 10;
        AssignBookSpell(I, Codex.Spells.drain_life);
      });

      book_of_extra_healing = AddBook("book of extra healing", I =>
      {
        I.Description = null;
        I.SetAppearance("plaid book", null);
        I.Glyph = Glyphs.plaid_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.extra_healing);
      });

      book_of_full_healing = AddBook("book of full healing", I =>
      {
        I.Description = null;
        I.SetAppearance("long book", null);
        I.Glyph = Glyphs.long_book;
        I.Rarity = 5;
        AssignBookSpell(I, Codex.Spells.full_healing);
      });

      book_of_regenerate = AddBook("book of regenerate", I =>
      {
        I.Description = null;
        I.SetAppearance("dull book", null);
        I.Glyph = Glyphs.dull_book;
        I.Rarity = 10;
        AssignBookSpell(I, Codex.Spells.regenerate);
      });

      book_of_finger_of_death = AddBook("book of finger of death", I =>
      {
        I.Description = null;
        I.SetAppearance("stained book", null);
        I.Glyph = Glyphs.stained_book;
        I.Rarity = 5;
        AssignBookSpell(I, Codex.Spells.finger_of_death);
      });

      book_of_fireball = AddBook("book of fireball", I =>
      {
        I.Description = null;
        I.SetAppearance("ragged book", null);
        I.Glyph = Glyphs.ragged_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.fireball, Fireproof: true);
      });

      book_of_ice_storm = AddBook("book of ice storm", I =>
      {
        I.Description = null;
        I.SetAppearance("thin book", null);
        I.Glyph = Glyphs.thin_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.ice_storm, Fireproof: true);
      });

      book_of_force_bolt = AddBook("book of force bolt", I =>
      {
        I.Description = null;
        I.SetAppearance("red book", null);
        I.Glyph = Glyphs.red_book;
        I.Rarity = 25;
        AssignBookSpell(I, Codex.Spells.force_bolt);
      });

      book_of_haste = AddBook("book of haste", I =>
      {
        I.Description = null;
        I.SetAppearance("purple book", null);
        I.Glyph = Glyphs.purple_book;
        I.Rarity = 20;
        AssignBookSpell(I, Codex.Spells.haste);
      });

      book_of_healing = AddBook("book of healing", I =>
      {
        I.Description = null;
        I.SetAppearance("white book", null);
        I.Glyph = Glyphs.white_book;
        I.Rarity = 30;
        AssignBookSpell(I, Codex.Spells.healing);
      });

      book_of_identify = AddBook("book of identify", I =>
      {
        I.Description = null;
        I.SetAppearance("bronze book", null);
        I.Glyph = Glyphs.bronze_book;
        I.Rarity = 30;
        AssignBookSpell(I, Codex.Spells.identify);
      });

      book_of_invisibility = AddBook("book of invisibility", I =>
      {
        I.Description = null;
        I.SetAppearance("dark brown book", null);
        I.Glyph = Glyphs.dark_brown_book;
        I.Rarity = 20;
        AssignBookSpell(I, Codex.Spells.invisibility);
      });

      book_of_jumping = AddBook("book of jumping", I =>
      {
        I.Description = null;
        I.SetAppearance("torn book", null);
        I.Glyph = Glyphs.torn_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.jumping);
      });

      book_of_blinking = AddBook("book of blinking", I =>
      {
        I.Description = null;
        I.SetAppearance("fuzzy book", null);
        I.Glyph = Glyphs.fuzzy_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.blinking);
      });

      book_of_phasing = AddBook("book of phasing", I =>
      {
        I.Description = null;
        I.SetAppearance("shadow book", null);
        I.Glyph = Glyphs.shadow_book;
        I.Rarity = 10;
        AssignBookSpell(I, Codex.Spells.phasing);
      });

      book_of_knock = AddBook("book of knock", I =>
      {
        I.Description = null;
        I.SetAppearance("pink book", null);
        I.Glyph = Glyphs.pink_book;
        I.Rarity = 25;
        AssignBookSpell(I, Codex.Spells.knock);
      });

      book_of_levitation = AddBook("book of levitation", I =>
      {
        I.Description = null;
        I.SetAppearance("tan book", null);
        I.Glyph = Glyphs.tan_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.levitation);
      });

      book_of_darkness = AddBook("book of darkness", I =>
      {
        I.Description = null;
        I.SetAppearance("faded book", null);
        I.Glyph = Glyphs.faded_book;
        I.Rarity = 30;
        AssignBookSpell(I, Codex.Spells.darkness);
      });

      book_of_light = AddBook("book of light", I =>
      {
        I.Description = null;
        I.SetAppearance("cloth book", null);
        I.Glyph = Glyphs.cloth_book;
        I.Rarity = 30;
        AssignBookSpell(I, Codex.Spells.light);
      });

      book_of_lightning_bolt = AddBook("book of lightning bolt", I =>
      {
        I.Description = null;
        I.SetAppearance("rainbow book", null);
        I.Glyph = Glyphs.rainbow_book;
        I.Rarity = 10;
        AssignBookSpell(I, Codex.Spells.lightning_bolt);
      });

      book_of_magic_mapping = AddBook("book of magic mapping", I =>
      {
        I.Description = null;
        I.SetAppearance("dusty book", null);
        I.Glyph = Glyphs.dusty_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.magic_mapping);
      });

      book_of_magic_missile = AddBook("book of magic missile", I =>
      {
        I.Description = null;
        I.SetAppearance("vellum book", null);
        I.Glyph = Glyphs.vellum_book;
        I.Rarity = 40;
        AssignBookSpell(I, Codex.Spells.magic_missile);
      });

      book_of_polymorph = AddBook("book of polymorph", I =>
      {
        I.Description = null;
        I.SetAppearance("silver book", null);
        I.Glyph = Glyphs.silver_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.polymorph);
      });

      book_of_deflection = AddBook("book of deflection", I =>
      {
        I.Description = null;
        I.SetAppearance("wide book", null);
        I.Glyph = Glyphs.wide_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.deflection);
      });

      book_of_telekinesis = AddBook("book of telekinesis", I =>
      {
        I.Description = null;
        I.SetAppearance("backwards book", null);
        I.Glyph = Glyphs.backwards_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.telekinesis);
      });

      book_of_raise_dead = AddBook("book of raise dead", I =>
      {
        I.Description = null;
        I.SetAppearance("papyrus book", null);
        I.Glyph = Glyphs.papyrus_book;
        I.Rarity = 10;
        AssignBookSpell(I, Codex.Spells.raise_dead);
      });

      book_of_remove_curse = AddBook("book of remove curse", I =>
      {
        I.Description = null;
        I.SetAppearance("wrinkled book", null);
        I.Glyph = Glyphs.wrinkled_book;
        I.Rarity = 25;
        AssignBookSpell(I, Codex.Spells.remove_curse);
      });

      book_of_restoration = AddBook("book of restoration", I =>
      {
        I.Description = null;
        I.SetAppearance("light brown book", null);
        I.Glyph = Glyphs.light_brown_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.restoration);
      });

      book_of_sleep = AddBook("book of sleep", I =>
      {
        I.Description = null;
        I.SetAppearance("mottled book", null);
        I.Glyph = Glyphs.mottled_book;
        I.Rarity = 35;
        AssignBookSpell(I, Codex.Spells.sleep);
      });

      book_of_slow = AddBook("book of slow", I =>
      {
        I.Description = null;
        I.SetAppearance("light green book", null);
        I.Glyph = Glyphs.light_green_book;
        I.Rarity = 25;
        AssignBookSpell(I, Codex.Spells.slow);
      });

      book_of_teleport_away = AddBook("book of teleport away", I =>
      {
        I.Description = null;
        I.SetAppearance("gold book", null);
        I.Glyph = Glyphs.gold_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.teleport_away);
      });

      book_of_toxic_spray = AddBook("book of toxic spray", I =>
      {
        I.Description = null;
        I.SetAppearance("ochre book", null);
        I.Glyph = Glyphs.ochre_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.toxic_spray);
      });

      book_of_turn_undead = AddBook("book of turn undead", I =>
      {
        I.Description = null;
        I.SetAppearance("copper book", null);
        I.Glyph = Glyphs.copper_book;
        I.Rarity = 15;
        AssignBookSpell(I, Codex.Spells.turn_undead);
      });

      book_of_walling = AddBook("book of walling", I =>
      {
        I.Description = null;
        I.SetAppearance("big book", null);
        I.Glyph = Glyphs.big_book;
        I.Rarity = 10;
        AssignBookSpell(I, Codex.Spells.walling);
      });

      book_of_wizard_lock = AddBook("book of wizard lock", I =>
      {
        I.Description = null;
        I.SetAppearance("dark green book", null);
        I.Glyph = Glyphs.dark_green_book;
        I.Rarity = 30;
        AssignBookSpell(I, Codex.Spells.wizard_lock);
      });

      CodexRecruiter.Enrol(() =>
      {
        foreach (var Book in Stocks.book.Items.Where(B => B != book_of_blank_paper && !B.Artifact))
          Register.Edit(Book).SetDowngradeItem(book_of_blank_paper);
      });
      #endregion

      #region coin.
      gold_coin = AddItem(Stocks.gem, ItemType.Coin, "gold coin", I =>
      {
        I.Description = "The only coin that can be found and spent in the dungeon.";
        I.Glyph = Glyphs.gold_coin;
        I.Sonic = Sonics.coins;
        I.Series = null;
        I.Rarity = 0;
        I.BundleDice = Dice.One;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(0);
        I.Material = Materials.gold;
        I.Essence = Essence.FromUnits(1);
        I.Price = Gold.FromCoins(1);
        I.SetEquip(EquipAction.Purse, Delay.FromTurns(10), Sonics.coins);
      });
      #endregion

      #region food.
      var FoodEssence0 = Essence.FromUnits(5);
      var FoodEssence1 = Essence.FromUnits(10);
      var FoodEssence2 = Essence.FromUnits(25);
      var FoodEssence3 = Essence.FromUnits(50);

      apple = AddFood("apple", I =>
      {
        I.Description = "A sweet, round fruit, of the red variety. Reportedly a great supplement to your physical health.";
        I.Glyph = Glyphs.apple;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 13;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(20);
        I.Material = Materials.fruit;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(7);
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.RemoveTransient(Properties.sickness), // keeps the doctor away.
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sleeping, 10.d6() + 6) // classic sleeping beauty scenario.
          );
          SugarRush(I, A);
        });
      });

      banana = AddFood("banana", I =>
      {
        I.Description = "A curved yellow fruit, and a great source of fibre.";
        I.Glyph = Glyphs.banana;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(20);
        I.Material = Materials.fruit;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(9);
        I.AddObviousIngestUse(Motions.eat, 160, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.slow_digestion, 6.d60()),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
          SugarRush(I, A);
        });
      });

      candy_bar = AddFood("candy bar", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.candy_bar;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 13;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(20);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B =>
            {
              B.RemoveTransient(Properties.fear);
              B.ApplyTransient(Properties.quickness, 1.d6() + 4);
            },
            U => U.RemoveTransient(Properties.fear),
            C =>
            {
              C.WhenChance(Chance.OneIn20, T => T.Punish(Codex.Punishments.gluttony), E => E.ApplyTransient(Properties.sickness, 1.d6() + 4));
            }
          );
          A.WhenTargetKind(new[] { Kinds.dog, Kinds.cat }, T =>
          {
            // dogs and cats cannot eat chocolate without serious illness.
            T.ApplyTransient(Properties.sickness, 4.d40());
          });
          SugarRush(I, A);
        });
      });

      carrot = AddFood("carrot", I =>
      {
        I.Description = "An orange root-vegetable, commonly believed to improve eyesight.";
        I.Glyph = Glyphs.carrot;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 15;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(20);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(7);
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B =>
            {
              B.RemoveTransient(Properties.blindness);
              B.ApplyTransient(Properties.dark_vision, 80.d4());
            },
            U => U.RemoveTransient(Properties.blindness),
            C => C.ApplyTransient(Properties.blindness, 2.d4() + 2)
          );
        });
      });

      cheese = AddFood("cheese", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.cheese;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(20);
        I.Material = Materials.animal;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(17);
        I.AddObviousIngestUse(Motions.eat, 250, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.RemoveTransient(Properties.teleportation),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
        });
      });

      clove_of_garlic = AddFood("clove of garlic", I =>
      {
        I.Description = "One segment of a particularly pungent and bulbous herb.";
        I.Glyph = Glyphs.clove_of_garlic;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 7;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence1;
        I.Price = Gold.FromCoins(7);
        I.AddObviousIngestUse(Motions.eat, 80, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(), // TODO: remove Vampirism affliction.
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
          A.WhenTargetKind(Kinds.Undead.ToArray(), T =>
          {
            T.Harm(Elements.physical, 4.d6() + 4);
            T.ApplyTransient(Properties.stunned, 4.d6());
          });
        });
      });

      animal_corpse = AddItem(Stocks.food, ItemType.Corpse, "animal corpse", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.animal_corpse;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 0;
        I.Size = null;
        I.Weight = Weight.FromUnits(0);
        I.Material = Materials.animal;
        I.Essence = Essence.FromUnits(1);
        I.Price = Gold.FromCoins(1);
        I.AddObviousIngestUse(Motions.eat, 0, Delay.FromTurns(10), Sonics.eat);
      });

      vegetable_corpse = AddItem(Stocks.food, ItemType.Corpse, "vegetable corpse", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.vegetable_corpse;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 0;
        I.Size = null;
        I.Weight = Weight.FromUnits(0);
        I.Material = Materials.vegetable;
        I.Essence = Essence.FromUnits(1);
        I.Price = Gold.FromCoins(1);
        I.AddObviousIngestUse(Motions.eat, 0, Delay.FromTurns(10), Sonics.eat);
      });

      cram_ration = AddFood("cram ration", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.cram_ration;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 20;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(150);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(35);
        I.AddObviousIngestUse(Motions.eat, 600, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
        });
      });

      cration = AddFood("c-ration", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.cration;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 11;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(20);
        I.AddObviousIngestUse(Motions.eat, 300, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
        });
      });

      cream_pie = AddFood("cream pie", I =>
      {
        I.Description = "A sweet custard-filled pie with a thin crust, topped with whipped cream.";
        I.Glyph = Glyphs.cream_pie;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(10);
        I.SetWeakness(Elements.physical); // break on impact.
        I.AddIngestUse(Mode.Obvious, Motions.eat, 200, new Utility(Purpose.Toss, Property: Properties.blindness, Level: 0), Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.WhenChance(Chance.OneIn20, T => T.Punish(Codex.Punishments.gluttony), E => E.ApplyTransient(Properties.sickness, 1.d6() + 4))
          );
          SugarRush(I, A);
        });
        I.SetImpact(Sonics.splat, A =>
        {
          A.ApplyTransient(Properties.blindness, 2.d6() + 6);
          A.ApplyTransient(Properties.fumbling, 2.d6() + 6);
        });
      });

      egg = AddItem(Stocks.food, ItemType.Egg, "egg", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.egg;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 80;
        I.Size = null;
        I.Weight = Weight.Zero;
        I.Material = Materials.animal;
        I.Essence = FoodEssence1;
        I.Price = Gold.FromCoins(9);
        //I.Impact = new Impact(Sonics.splat);
        I.AddObviousIngestUse(Motions.eat, 0, Delay.FromTurns(10), Sonics.eat, A =>
        {
          // depends on the hatchling entity.
        });
      });

      eucalyptus_leaf = AddFood("eucalyptus leaf", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.eucalyptus_leaf;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 7;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence1;
        I.Price = Gold.FromCoins(6);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.RemoveTransient(Properties.silence),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.silence, 1.d6() + 40)
          );
        });
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.whistle, Use =>
        {
          // single use magic whistle.
          Use.Consume();
          Use.Apply.Alert(1.d6() + 1);
          Use.Apply.Recall();
        });
      });
      /*
      eyeball = AddFood("eyeball", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.eyeball;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.animal;
        I.Essence = FoodEssence1;
        I.Price = Gold.FromCoins(5);
        I.AddEat(20, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.Sickness, 1.d6() + 4)
          );
        });
      });
      */

      tin = AddItem(Stocks.food, ItemType.Tin, "tin", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.tin;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 75;
        I.Size = null;
        I.Weight = Weight.Zero;
        I.Material = Materials.tin;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 0, Delay.FromTurns(50), Sonics.eat, A =>
        {
          // depends on the tinned entity.
        });
      });

      food_ration = AddFood("food ration", I =>
      {
        I.Description = "A hearty, multi-portioned and nutritious vegetarian meal.";
        I.Glyph = Glyphs.food_ration;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 380;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(45);
        I.AddObviousIngestUse(Motions.eat, 800, Delay.FromTurns(50), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
        });
      });

      fortune_cookie = AddFood("fortune cookie", I =>
      {
        I.Description = "An inconspicuous snack that tells deep secrets.";
        I.Glyph = Glyphs.fortune_cookie;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 55;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence1;
        I.Price = Gold.FromCoins(7);
        I.AddObviousIngestUse(Motions.eat, 80, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Rumour(Truth: true, Lies: false),
            U => U.Rumour(Truth: true, Lies: true),
            C => C.Rumour(Truth: false, Lies: true)
          );
          SugarRush(I, A);
        });
        I.AddObviousUse(Motions.open, Delay.FromTurns(10), Sonics.crumble, A =>
        {
          A.Consume();
          A.Apply.WithSourceSanctity
          (
            B => B.Rumour(Truth: true, Lies: false),
            U => U.Rumour(Truth: true, Lies: true),
            C => C.Rumour(Truth: false, Lies: true)
          );
        });
      });

      holy_wafer = AddFood("holy wafer", I =>
      {
        I.Description = "A sanctimonious biscuit. A miracle to the meek, a scourge to the undead.";
        I.Glyph = Glyphs.holy_wafer;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 7;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence1;
        I.Price = Gold.FromCoins(12);
        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WhenTargetKind(Kinds.Undead.ToArray().Union(Kinds.demon).ToArray(),
          T =>
          {
            T.Harm(Elements.physical, 4.d6() + 4);
            T.ApplyTransient(Properties.stunned, 4.d6());
          },
          E => E.WithSourceSanctity
          (
            B =>
            {
              B.Heal(1.d10() + 20, Modifier.Zero);
              B.RemoveTransient(Properties.sickness);
            },
            U =>
            {
              U.Heal(1.d10() + 2, Modifier.Zero);
              U.RemoveTransient(Properties.sickness);
            },
            C =>
            {
              C.Harm(Elements.necrotic, 1.d10() + 2);
              C.ApplyTransient(Properties.sickness, 1.d10() + 10);
            }
          ));
        });
      });

      huge_chunk_of_meat = AddFood("huge chunk of meat", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.huge_chunk_of_meat;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(4000);
        I.Material = Materials.animal;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(105);
        I.AddObviousIngestUse(Motions.eat, 2000, Delay.FromTurns(200), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
        });
      });

      kelp_frond = AddFood("kelp frond", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.kelp_frond;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(6);
        I.AddObviousIngestUse(Motions.eat, 60, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
        });
      });

      kration = AddFood("k-ration", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.kration;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 13;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(25);
        I.AddObviousIngestUse(Motions.eat, 400, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
        });
      });

      lembas_wafer = AddFood("lembas wafer", I =>
      {
        I.Description = "An elven travel food. It is incredibly satiating.";
        I.SetAppearance("thin cake", null);
        I.OriginRace = Races.elf;
        I.Glyph = Glyphs.lembas_wafer;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 20;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence2;
        I.Price = Gold.FromCoins(45);
        I.AddObviousIngestUse(Motions.eat, 800, Delay.FromTurns(20), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
          SugarRush(I, A);
        });
      });

      lump_of_royal_jelly = AddFood("lump of royal jelly", I =>
      {
        I.Description = null;
        I.SetAppearance("sticky glob", null);
        I.Glyph = Glyphs.lump_of_royal_jelly;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(20);
        I.Material = Materials.animal;
        I.Essence = FoodEssence3;
        I.Price = Gold.FromCoins(15);
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.WhenChance(Chance.OneIn3, T => T.IncreaseAbility(Attributes.strength, Dice.One)),
            U => U.WhenChance(Chance.OneIn6, T => T.IncreaseAbility(Attributes.strength, Dice.One)),
            C => C.WhenChance(Chance.OneIn3, T => T.DecreaseAbility(Attributes.strength, Dice.One))
          );
          SugarRush(I, A);
        });
      });

      meat_ring = AddFood("meat ring", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.meat_ring;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.animal;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(1);
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
        });
      });

      meat_stick = AddFood("meat stick", I =>
      {
        I.Description = "A stick of cured meat.";
        I.Glyph = Glyphs.meat_stick;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.animal;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(5);
        I.AddObviousIngestUse(Motions.eat, 300, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
        });
      });

      meatball = AddFood("meatball", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.meatball;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(25);
        I.Material = Materials.animal;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(5);
        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
        });
      });

      melon = AddFood("melon", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.melon;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 9;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.fruit;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.RemoveTransient(Properties.rage),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
          SugarRush(I, A);
        });
      });

      mushroom = AddFood("mushroom", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.mushroom;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 6;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence1;
        I.Price = Gold.FromCoins(9);
        I.AddObviousIngestUse(Motions.eat, 180, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WhenChance(Chance.OneIn10,
            T => T.UnlessTargetResistant(Elements.poison, R =>
            {
              R.Harm(Elements.poison, 1.d15());
              R.DecreaseAbility(Attributes.strength, 1.d4());
            }),
            E => E.WhenChance(Chance.OneIn10, S =>
            {
              S.IncreaseAbility(Attributes.strength, Dice.One);
            }));

          A.WhenChance(Chance.OneIn10, T => T.ApplyTransient(Properties.stunned, 1.d10() + 30));
          A.WhenChance(Chance.OneIn10, T => T.ApplyTransient(Properties.hallucination, 1.d10() + 150));
        });
      });

      orange = AddFood("orange", I =>
      {
        I.Description = "A sweet citrus fruit, coloured eponymously.";
        I.Glyph = Glyphs.orange;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(20);
        I.Material = Materials.fruit;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(9);
        I.AddObviousIngestUse(Motions.eat, 160, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.RemoveTransient(Properties.sickness),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
          SugarRush(I, A);
        });
      });

      pancake = AddFood("pancake", I =>
      {
        I.Description = "A flat, round cake. Cooked in a pan, as you might presume.";
        I.Glyph = Glyphs.pancake;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 14;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(20);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(15);
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(20), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.WhenChance(Chance.OneIn20, T => T.Punish(Codex.Punishments.gluttony), E => E.ApplyTransient(Properties.sickness, 1.d6() + 4))
          );
          SugarRush(I, A);
        });
      });

      pear = AddFood("pear", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.pear;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 9;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(20);
        I.Material = Materials.fruit;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(8);
        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.RemoveTransient(Properties.hunger),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.hunger, 1.d6() + 40)
          );
          SugarRush(I, A);
        });
      });

      sandwich = AddFood("sandwich", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.sandwich;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.animal;
        I.Essence = FoodEssence1; // made with love ;-)
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 500, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
        });
      });
      /*
      severed_hand = AddFood("severed hand", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.severed_hand;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.animal;
        I.Essence = FoodEssence1;
        I.Price = Gold.FromCoins(7);
        I.AddEat(80, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.Sickness, 1.d6() + 4)
          );
        });
      });
      */
      slime_mould = AddFood("slime mould", I =>
      {
        I.Description = "An ambiguous curiosity. It seems edible.";
        I.Glyph = Glyphs.slime_mould;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 75;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence1;
        I.Price = Gold.FromCoins(17);
        I.AddObviousIngestUse(Motions.eat, 250, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
        });
      });

      sprig_of_wolfsbane = AddFood("sprig of wolfsbane", I =>
      {
        I.Description = "The snipped segment of a plant known for its antilycanthropic properties.";
        I.Glyph = Glyphs.sprig_of_wolfsbane;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 7;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence1;
        I.Price = Gold.FromCoins(7);
        I.AddObviousIngestUse(Motions.eat, 80, Delay.FromTurns(10), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(), // TODO: remove lycanthropy affliction.
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.paralysis, 1.d6() + 4)
          );
          A.WhenTargetKind(new[] { Kinds.lycanthrope }, T =>
          {
            T.Harm(Elements.physical, 4.d6() + 4);
            T.ApplyTransient(Properties.stunned, 4.d6());
          });
        });
      });

      tortilla = AddFood("tortilla", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.tortilla;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(20);
        I.Material = Materials.vegetable;
        I.Essence = FoodEssence1;
        I.Price = Gold.FromCoins(9);
        I.AddObviousIngestUse(Motions.eat, 400, Delay.FromTurns(20), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 1.d6() + 4)
          );
        });
      });

      tripe_ration = AddFood("tripe ration", I =>
      {
        I.Description = "A meal made from the stomach lining of an animal.";
        I.Glyph = Glyphs.tripe_ration;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 142;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.animal;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(15);
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(20), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.WhenChance(Chance.OneIn20, T => T.Afflict(Codex.Afflictions.worms), E => E.ApplyTransient(Properties.sickness, 1.d6() + 4))
          );
        });
      });

      iron_ration = AddFood("iron ration", I =>
      {
        I.Description = "A package of preserved meats, all dried and chewy.";
        I.Glyph = Glyphs.iron_ration;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 71;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.animal;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(35);
        I.AddObviousIngestUse(Motions.eat, 600, Delay.FromTurns(60), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Nothing(),
            U => U.Nothing(),
            C => C.WhenChance(Chance.OneIn20, T => T.Afflict(Codex.Afflictions.worms), E => E.ApplyTransient(Properties.sickness, 1.d6() + 4))
          );
        });
      });

      fish = AddFood("fish", I =>
      {
        I.Description = "A slimy, scaly sea creature, in its entirety. Good for a meal.";
        I.Glyph = Glyphs.fish;
        I.Sonic = Sonics.food;
        I.Series = null;
        I.Rarity = 5;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.animal;
        I.Essence = FoodEssence0;
        I.Price = Gold.FromCoins(15);
        I.AddObviousIngestUse(Motions.eat, 400, Delay.FromTurns(30), Sonics.eat, A =>
        {
          A.WithSourceSanctity
          (
            B => B.RestoreAbility(),
            U => U.Nothing(),
            C => C.ApplyTransient(Properties.sickness, 2.d6() + 8)
          );
        });
      });

      The_Hero = AddFood("The Hero", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.The_Hero;
        I.Sonic = Sonics.food;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.animal;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1000);
        I.SetNutrition(1000);
        I.AddObviousUse(Motions.eat, Delay.FromTurns(60), Sonics.eat, Use =>
        {
          // TODO: replica heros should be destroyed instead of teleporting away.
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.Heal(Dice.Fixed(1000), Modifier.Zero);
              B.Energise(Dice.Fixed(1000), Modifier.Zero);
            },
            U =>
            {
              U.Heal(Dice.Fixed(1000), Modifier.Zero);
            },
            C =>
            {
              C.ApplyTransient(Properties.hunger, 4.d100());
            }
          );
          Use.Apply.AreaTransient(Properties.rage, 6.d6(), Kinds.Living.ToArray());
          Use.Apply.ApplyTransient(Properties.aggravation, 6.d100() + 400);
          Use.Apply.WhenSourceReplica
          (
            T => T.DestroySourceAsset(Dice.One),
            F => F.TeleportAway()
          );
        });
      });
      #endregion

      var AmmoEssence0 = Essence.FromUnits(1);
      var AmmoEssence1 = Essence.FromUnits(2);
      var AmmoEssence2 = Essence.FromUnits(3);
      var AmmoEssence3 = Essence.FromUnits(4);
      var AmmoWeakness = new[] { Elements.physical };

      #region gem.
      var GemSize = Size.Tiny;
      var GemWeight = Weight.FromUnits(10);
      var GemEssence0 = Essence.FromUnits(10);
      var BaubleDamage = 1.d5();
      var Gem1Damage = 2.d3();
      var Gem2Damage = 2.d4();
      var BaubleImpactSonic = Sonics.broken_glass;
      
      Item AddOrangeGem(string Name, Action<ItemEditor> EditorAction) => AddGem(Name, I => { I.SetAppearance("orange gem", null, Price: Gold.FromCoins(325)).Indiscriminate(); EditorAction(I); }); // 3250, 200, 1
      Item AddWhiteGem(string Name, Action<ItemEditor> EditorAction) => AddGem(Name, I => { I.SetAppearance("white gem", null, Price: Gold.FromCoins(450)).Indiscriminate(); EditorAction(I); }); // 4500, 4000, 800, 1
      Item AddYellowBrownGem(string Name, Action<ItemEditor> EditorAction) => AddGem(Name, I => { I.SetAppearance("yellowish brown gem", null, Price: Gold.FromCoins(100)).Indiscriminate(); EditorAction(I); }); // 1000, 900, 1
      Item AddVioletGem(string Name, Action<ItemEditor> EditorAction) => AddGem(Name, I => { I.SetAppearance("violet gem", null, Price: Gold.FromCoins(60)).Indiscriminate(); EditorAction(I); }); // 600, 400, 1
      Item AddBlackGem(string Name, Action<ItemEditor> EditorAction) => AddGem(Name, I => { I.SetAppearance("black gem", null, Price: Gold.FromCoins(250)).Indiscriminate(); EditorAction(I); }); // 2500, 850, 200, 1.
      Item AddGreenGem(string Name, Action<ItemEditor> EditorAction) => AddGem(Name, I => { I.SetAppearance("green gem", null, Price: Gold.FromCoins(250)).Indiscriminate(); EditorAction(I); }); // 2500, 2000, 1500, 300, 1
      Item AddBlueGem(string Name, Action<ItemEditor> EditorAction) => AddGem(Name, I => { I.SetAppearance("blue gem", null, Price: Gold.FromCoins(300)).Indiscriminate(); EditorAction(I); }); // 3000, 1
      Item AddRedGem(string Name, Action<ItemEditor> EditorAction) => AddGem(Name, I => { I.SetAppearance("red gem", null, Price: Gold.FromCoins(350)).Indiscriminate(); EditorAction(I); }); // 3500, 700, 500, 1
      Item AddYellowGem(string Name, Action<ItemEditor> EditorAction) => AddGem(Name, I => { I.SetAppearance("yellow gem", null, Price: Gold.FromCoins(150)).Indiscriminate(); EditorAction(I); }); // 1500, 700, 1

      agate = AddOrangeGem("agate", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.orange_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 12;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(14);
        I.Price = Gold.FromCoins(200);
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      amber = AddYellowBrownGem("amber", I =>
      {
        I.Description = "This gemstone is made from hardened tree sap.";
        I.Glyph = Glyphs.yellowish_brown_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 8;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(32);
        I.Price = Gold.FromCoins(1000);
        I.AddObviousIngestUse(Motions.eat, 1000, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      amethyst = AddVioletGem("amethyst", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.violet_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 14;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(24);
        I.Price = Gold.FromCoins(600);
        I.AddObviousIngestUse(Motions.eat, 600, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      aquamarine = AddGreenGem("aquamarine", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.green_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 6;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(39);
        I.Price = Gold.FromCoins(1500);
        I.AddObviousIngestUse(Motions.eat, 1500, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      var BaubleWeakness = new[] { Elements.physical, Elements.force };

      black_glass_bauble = AddBlackGem("black glass bauble", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.black_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 76;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.glass;
        I.Essence = GemEssence0;
        I.Price = Gold.FromCoins(1);
        I.SetImpact(BaubleImpactSonic);
        I.SetWeakness(BaubleWeakness);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, BaubleDamage);
      });

      black_opal = AddBlackGem("black opal", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.black_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 3;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(50);
        I.Price = Gold.FromCoins(2500);
        I.AddObviousIngestUse(Motions.eat, 2500, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      blue_glass_bauble = AddBlueGem("blue glass bauble", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.blue_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 76;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.glass;
        I.Essence = GemEssence0;
        I.Price = Gold.FromCoins(1);
        I.SetImpact(BaubleImpactSonic);
        I.SetWeakness(BaubleWeakness);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, BaubleDamage);
      });

      chrysoberyl = AddYellowGem("chrysoberyl", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.yellow_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 8;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(26);
        I.Price = Gold.FromCoins(700);
        I.AddObviousIngestUse(Motions.eat, 700, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      citrine = AddYellowGem("citrine", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.yellow_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 4;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(39);
        I.Price = Gold.FromCoins(1500);
        I.AddObviousIngestUse(Motions.eat, 1500, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      diamond = AddWhiteGem("diamond", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.white_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 3;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(63);
        I.Price = Gold.FromCoins(4000);
        I.AddObviousIngestUse(Motions.eat, 4000, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem2Damage);
      });

      dilithium_crystal = AddWhiteGem("dilithium crystal", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.white_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 2;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(100);
        I.Price = Gold.FromCoins(4500);
        I.AddObviousIngestUse(Motions.eat, 4500, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem2Damage);
      });

      emerald = AddGreenGem("emerald", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.green_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 5;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(50);
        I.Price = Gold.FromCoins(2500);
        I.AddObviousIngestUse(Motions.eat, 2500, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      fluorite = AddVioletGem("fluorite", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.violet_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 15;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(20);
        I.Price = Gold.FromCoins(400);
        I.AddObviousIngestUse(Motions.eat, 400, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      garnet = AddRedGem("garnet", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.red_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 12;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(26);
        I.Price = Gold.FromCoins(700);
        I.AddObviousIngestUse(Motions.eat, 700, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      green_glass_bauble = AddGreenGem("green glass bauble", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.green_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 76;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.glass;
        I.Essence = GemEssence0;
        I.Price = Gold.FromCoins(1);
        I.SetImpact(BaubleImpactSonic);
        I.SetWeakness(BaubleWeakness);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, BaubleDamage);
      });

      jacinth = AddOrangeGem("jacinth", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.orange_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 3;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(57);
        I.Price = Gold.FromCoins(3250);
        I.AddObviousIngestUse(Motions.eat, 3250, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      jade = AddGreenGem("jade", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.green_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 10;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(17);
        I.Price = Gold.FromCoins(300);
        I.AddObviousIngestUse(Motions.eat, 300, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      jasper = AddRedGem("jasper", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.red_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 15;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(22);
        I.Price = Gold.FromCoins(500);
        I.AddObviousIngestUse(Motions.eat, 500, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      jet = AddBlackGem("jet", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.black_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 6;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(29);
        I.Price = Gold.FromCoins(850);
        I.AddObviousIngestUse(Motions.eat, 850, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      obsidian = AddBlackGem("obsidian", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.black_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 9;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(14);
        I.Price = Gold.FromCoins(200);
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      opal = AddWhiteGem("opal", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.white_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 12;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(28);
        I.Price = Gold.FromCoins(800);
        I.AddObviousIngestUse(Motions.eat, 800, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, Gem1Damage);
      });

      orange_glass_bauble = AddOrangeGem("orange glass bauble", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.orange_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 76;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.glass;
        I.Essence = GemEssence0;
        I.Price = Gold.FromCoins(1);
        I.SetImpact(BaubleImpactSonic);
        I.SetWeakness(BaubleWeakness);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, BaubleDamage);
      });

      red_glass_bauble = AddRedGem("red glass bauble", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.red_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 76;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.glass;
        I.Essence = GemEssence0;
        I.Price = Gold.FromCoins(1);
        I.SetImpact(BaubleImpactSonic);
        I.SetWeakness(BaubleWeakness);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, BaubleDamage);
      });

      rock = AddItem(Stocks.gem, ItemType.Rock, "rock", I =>
      {
        I.Description = "A sturdy and solid mineral.";
        I.Glyph = Glyphs.rock;
        I.Sonic = Sonics.gem;
        I.BundleDice = 1.d6() + 6;
        I.Series = null;
        I.Rarity = 100;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.stone; // TODO: rock turns into clay golem, but it's not clay material?
        I.Price = Gold.FromCoins(1);
        I.Essence = AmmoEssence0;
        I.SetWeakness(AmmoWeakness);
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, 1.d3());
      });

      flint = AddItem(Stocks.gem, ItemType.Rock, "flint", I =>
      {
        I.Description = "A hard rock, smooth and therefore aerodynamic.";
        I.SetAppearance("grey stone", null, Price: Gold.FromCoins(1));
        I.Glyph = Glyphs.flint;
        I.Sonic = Sonics.gem;
        I.BundleDice = 1.d4() + 6;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.stone;
        I.Essence = AmmoEssence1;
        I.Price = Gold.FromCoins(1);
        I.SetWeakness(AmmoWeakness);
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, 1.d4());
      });

      ruby = AddRedGem("ruby", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.red_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 4;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(59);
        I.Price = Gold.FromCoins(3500);
        I.AddObviousIngestUse(Motions.eat, 3500, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, 1.d6());
      });

      sapphire = AddBlueGem("sapphire", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.blue_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 4;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(55);
        I.Price = Gold.FromCoins(3000);
        I.AddObviousIngestUse(Motions.eat, 3000, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, 1.d6());
      });

      topaz = AddYellowBrownGem("topaz", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.yellowish_brown_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 10;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(30);
        I.Price = Gold.FromCoins(900);
        I.AddObviousIngestUse(Motions.eat, 900, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, 1.d6());
      });

      turquoise = AddGreenGem("turquoise", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.green_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 6;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.gemstone;
        I.Essence = Essence.FromUnits(45);
        I.Price = Gold.FromCoins(2000);
        I.AddObviousIngestUse(Motions.eat, 2000, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, 1.d6());
      });

      violet_glass_bauble = AddVioletGem("violet glass bauble", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.violet_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 76;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.glass;
        I.Essence = GemEssence0;
        I.Price = Gold.FromCoins(1);
        I.SetImpact(BaubleImpactSonic);
        I.SetWeakness(BaubleWeakness);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, BaubleDamage);
      });

      white_glass_bauble = AddWhiteGem("white glass bauble", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.white_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 76;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.glass;
        I.Essence = GemEssence0;
        I.Price = Gold.FromCoins(1);
        I.SetImpact(BaubleImpactSonic);
        I.SetWeakness(BaubleWeakness);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, BaubleDamage);
      });

      yellow_glass_bauble = AddYellowGem("yellow glass bauble", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.yellow_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 76;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.glass;
        I.Essence = GemEssence0;
        I.Price = Gold.FromCoins(1);
        I.SetImpact(BaubleImpactSonic);
        I.SetWeakness(BaubleWeakness);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, BaubleDamage);
      });

      yellowish_brown_glass_bauble = AddYellowBrownGem("yellowish brown glass bauble", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.yellowish_brown_gem;
        I.Sonic = Sonics.gem;
        I.Series = null;
        I.Rarity = 76;
        I.Size = GemSize;
        I.Weight = GemWeight;
        I.Material = Materials.glass;
        I.Essence = GemEssence0;
        I.Price = Gold.FromCoins(1);
        I.SetImpact(BaubleImpactSonic);
        I.SetWeakness(BaubleWeakness);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.gem);

        EquipPellet(I, BaubleDamage);
      });
      #endregion

      #region potion.
      var PotionSize = Size.Tiny;
      var PotionWeight = Weight.FromUnits(100); // 1lb.
      var PotionWeakness = new[] { Elements.physical, Elements.force, Elements.fire, Elements.cold, Elements.shock };
      var PotionEssence0 = Essence.FromUnits(5);
      var PotionEssence1 = Essence.FromUnits(10);
      var PotionEssence2 = Essence.FromUnits(20);
      var PotionEssence3 = Essence.FromUnits(30);
      var PotionEssence4 = Essence.FromUnits(50);
      var PotionEssence5 = Essence.FromUnits(100);
      var PotionEssence6 = Essence.FromUnits(150);
      var PotionSeries = new Series("potion");

      potion_of_acid = AddPotion("potion of acid", I =>
      {
        I.Description = null;
        I.SetAppearance("white potion", null);
        I.Glyph = Glyphs.white_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 20;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence0;
        I.Price = Gold.FromCoins(250);
        I.SetWeakness(PotionWeakness);
        I.SetImpact(Sonics.broken_glass, A =>
        {
          A.Harm(Elements.acid, 1.d6());
          A.CreateTrap(Codex.Devices.noxious_pool, Destruction: false);
        });
        I.AddPropertyTossUse(Motions.quaff, Property: null, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.RemoveTransient(Properties.petrifying);
          Use.Apply.WithSourceSanctity
          (
            B => B.Harm(Elements.acid, 1.d4()),
            U => U.Harm(Elements.acid, 1.d8()),
            C => C.Harm(Elements.acid, 2.d8())
          );
        });
      });

      potion_of_affliction = AddPotion("potion of affliction", I =>
      {
        I.Description = null;
        I.SetAppearance("icy potion", null);
        I.Glyph = Glyphs.icy_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 4;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence5;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyTossUse(Motions.quaff, Property: null, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.Afflict(Codex.Afflictions.List.Where(F => !F.Severe).ToArray()),
            U => U.Afflict(Codex.Afflictions.List.ToArray()),
            C => C.Afflict(Codex.Afflictions.List.Where(F => F.Severe).ToArray())
          );
        });
      });

      potion_of_amnesia = AddPotion("potion of amnesia", I =>
      {
        I.Description = null;
        I.SetAppearance("sparkling potion", null);
        I.Glyph = Glyphs.sparkling_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 16;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence1;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyTossUse(Motions.quaff, Property: null, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.Amnesia(Range.Sq10),
            U => U.Amnesia(Range.Sq15),
            C => C.Amnesia(Range.Sq20)
          );
        });
      });

      potion_of_blindness = AddPotion("potion of blindness", I =>
      {
        I.Description = null;
        I.SetAppearance("yellow potion", null);
        I.Glyph = Glyphs.yellow_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 38;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence1;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyTossUse(Motions.quaff, Properties.blindness, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.blindness, 1.d200() + 125),
            U => U.ApplyTransient(Properties.blindness, 1.d200() + 250),
            C => C.ApplyTransient(Properties.blindness, 1.d200() + 375)
          );
        });
      });

      potion_of_booze = AddPotion("potion of booze", I =>
      {
        I.Description = null;
        I.SetAppearance("brown potion", null);
        I.Glyph = Glyphs.brown_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 40;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence0;
        I.Price = Gold.FromCoins(50);
        I.SetWeakness(PotionWeakness);
        I.AddObviousUse(Motions.quaff, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.Heal(Dice.One, Modifier.Zero);
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.Nutrition(Dice.Fixed(30));
              B.RemoveTransient(Properties.fear);
            },
            U =>
            {
              U.Nutrition(Dice.Fixed(20));
              U.RemoveTransient(Properties.fear);
              U.ApplyTransient(Properties.confusion, 5.d10()); // a bit drunk.
            },
            C =>
            {
              C.Nutrition(Dice.Fixed(10));
              C.ApplyTransient(Properties.confusion, 5.d10());
              C.ApplyTransient(Properties.fainting, 3.d6()); // pass out.
            }
          );
          Use.Apply.WhenTargetKind(new[] { Kinds.dwarf }, T =>
          {
            T.Energise(2.d4(), Modifier.Zero);
          });
        });
      });

      potion_of_clairvoyance = AddPotion("potion of clairvoyance", I =>
      {
        I.Description = null;
        I.SetAppearance("luminescent potion", null);
        I.Glyph = Glyphs.luminescent_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 20;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence3;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyBuffUse(Motions.quaff, Properties.clairvoyance, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.GainTalent(Properties.clairvoyance);
              //B.RemoveTransient(Properties.Blindness);
            },
            U =>
            {
              U.ApplyTransient(Properties.clairvoyance, 1.d50() + 100);
              //U.RemoveTransient(Properties.Blindness);
            },
            C =>
            {
              C.ApplyTransient(Properties.clairvoyance, 1.d50() + 50);
              C.ApplyTransient(Properties.aggravation, 1.d50() + 50);
            }
          );
        });
      });

      potion_of_confusion = AddPotion("potion of confusion", I =>
      {
        I.Description = null;
        I.SetAppearance("orange potion", null);
        I.Glyph = Glyphs.orange_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 40;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence2;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyTossUse(Motions.quaff, Properties.confusion, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.confusion, 1.d50() + 50), // TODO: anything useful for blessed confusion?
            U => U.ApplyTransient(Properties.confusion, 1.d100() + 100),
            C =>
            {
              C.ApplyTransient(Properties.confusion, 1.d100() + 100);
              C.ApplyTransient(Properties.stunned, 1.d10() + 10);
            }
          );
        });
      });

      potion_of_divinity = AddPotion("potion of divinity", I =>
      {
        I.Description = null;
        I.SetAppearance("bloody potion", null);
        I.Glyph = Glyphs.bloody_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 20;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence3;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyBuffUse(Motions.quaff, Properties.beatitude, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.Unpunish();
              B.Divine();
              B.ApplyTransient(Properties.beatitude, 1.d500() + 250);
            },
            U =>
            {
              U.Divine();
              U.ApplyTransient(Properties.beatitude, 1.d50() + 50);
            },
            C =>
            {
              C.ApplyTransient(Properties.beatitude, 1.d10() + 10);
            }
          );
        });
      });

      potion_of_ESP = AddPotion("potion of ESP", I =>
      {
        I.Description = null;
        I.SetAppearance("muddy potion", null);
        I.Glyph = Glyphs.muddy_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 20;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence3;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyBuffUse(Motions.quaff, Properties.telepathy, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.GainTalent(Properties.telepathy);
              B.ApplyTransient(Properties.blindness, 1.d10() + 10);
            },
            U =>
            {
              U.ApplyTransient(Properties.telepathy, 1.d50() + 100);
              //U.RemoveTransient(Properties.Blindness);
            },
            C =>
            {
              C.ApplyTransient(Properties.telepathy, 1.d50() + 50);
              C.ApplyTransient(Properties.blindness, 1.d50() + 50);
              C.ApplyTransient(Properties.deafness, 1.d50() + 50);
            }
          );
        });
      });

      potion_of_extra_healing = AddPotion("potion of extra healing", I =>
      {
        I.Description = null;
        I.SetAppearance("puce potion", null);
        I.Glyph = Glyphs.puce_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 45;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Price = Gold.FromCoins(150);
        I.Essence = PotionEssence4;
        I.SetWeakness(PotionWeakness);
        I.AddHealingUse(Motions.quaff, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.RemoveTransient(Properties.hallucination, Properties.blindness);
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.Heal(8.d8(), Modifier.FromRank(5));
              B.RemoveTransient(Properties.sickness);
            },
            U =>
            {
              U.Heal(6.d8(), Modifier.Zero);
              U.RemoveTransient(Properties.sickness);
            },
            C =>
            {
              C.Heal(4.d8(), Modifier.Zero);
            }
          );
        });
      });

      potion_of_fruit_juice = AddPotion("potion of fruit juice", I =>
      {
        I.Description = null;
        I.SetAppearance("dark potion", null);
        I.Glyph = Glyphs.dark_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 40;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence1;
        I.Price = Gold.FromCoins(50);
        I.SetWeakness(PotionWeakness);
        I.AddObviousUse(Motions.quaff, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.Nutrition(Dice.Fixed(200)),
            U => U.Nutrition(Dice.Fixed(100)),
            C =>
            {
              C.Malnutrition(Dice.Fixed(50));
              C.ApplyTransient(Properties.sickness, 4.d6()); // minor sickness
            }
          );
          SugarRush(I, Use.Apply, Duration: 5);
        });
      });

      potion_of_full_healing = AddPotion("potion of full healing", I =>
      {
        I.Description = null;
        I.SetAppearance("black potion", null);
        I.Glyph = Glyphs.black_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 20;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Price = Gold.FromCoins(200);
        I.Essence = PotionEssence5;
        I.SetWeakness(PotionWeakness);
        I.AddHealingUse(Motions.quaff, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.Heal(Dice.Fixed(1000), Modifier.FromRank(+8)),
            U => U.Heal(Dice.Fixed(1000), Modifier.Zero),
            C => C.Heal(Dice.Fixed(1000), Modifier.Zero)
          );
        });
      });

      potion_of_gain_ability = AddPotion("potion of gain ability", I =>
      {
        I.Description = null;
        I.SetAppearance("ruby potion", null);
        I.Glyph = Glyphs.ruby_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 38;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Price = Gold.FromCoins(300);
        I.Essence = PotionEssence5;
        I.SetWeakness(PotionWeakness);
        I.AddBuffUse(Motions.quaff, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.IncreaseAbilities(Attributes.List, Dice.One),
            U => U.IncreaseOneAbility(Dice.One),
            C => C.DecreaseOneAbility(Dice.One)
          );
        });
      });

      potion_of_gain_energy = AddPotion("potion of gain energy", I =>
      {
        I.Description = "This is a potion of pure magical energy which will restore and increase your mana.";
        I.SetAppearance("cloudy potion", null);
        I.Glyph = Glyphs.cloudy_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 40;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence5;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(PotionWeakness);
        I.AddBuffUse(Motions.quaff, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.Energise(1.d25() + 14, Modifier.FromRank(6)),
            U => U.Energise(1.d25() + 9, Modifier.Zero),
            C => C.Diminish(1.d25() + 9, Modifier.FromRank(3))
          );
        });
      });

      potion_of_gain_level = AddPotion("potion of gain level", I =>
      {
        I.Description = null;
        I.SetAppearance("milky potion", null);
        I.Glyph = Glyphs.milky_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 20;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence5;
        I.Price = Gold.FromCoins(300);
        I.SetWeakness(PotionWeakness);
        I.AddBuffUse(Motions.quaff, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.GainLevel(Dice.Fixed(+1), RandomExperience: true), // gain level plus random xp.
            U => U.GainLevel(Dice.Fixed(+1), RandomExperience: false), // gain level to base xp.
            C => C.TransitionAscend(Properties.teleportation, Dice.One) // rise up through ceiling.
          );
        });
      });

      potion_of_hallucination = AddPotion("potion of hallucination", I =>
      {
        I.Description = null;
        I.SetAppearance("sky blue potion", null);
        I.Glyph = Glyphs.sky_blue_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 40;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence1;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyTossUse(Motions.quaff, Properties.hallucination, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.ApplyTransient(Properties.hallucination, 1.d10() + 40);
              B.Enlightenment(null);
            },
            U =>
            {
              U.ApplyTransient(Properties.hallucination, 1.d10() + 100);
            },
            C =>
            {
              C.ApplyTransient(Properties.hallucination, 1.d10() + 160);
            }
          );
        });
      });

      potion_of_healing = AddPotion("potion of healing", I =>
      {
        I.Description = null;
        I.SetAppearance("purple-red potion", null);
        I.Glyph = Glyphs.purplered_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 55;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence3;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(PotionWeakness);
        I.AddHealingUse(Motions.quaff, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.Heal(8.d4(), Modifier.FromRank(1)),
            U => U.Heal(6.d4(), Modifier.Zero),
            C => C.Heal(4.d4(), Modifier.Zero)
          );
        });
      });

      potion_of_invisibility = AddPotion("potion of invisibility", I =>
      {
        I.Description = null;
        I.SetAppearance("brilliant blue potion", null);
        I.Glyph = Glyphs.brilliant_blue_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 40;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence3;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyBuffUse(Motions.quaff, Properties.invisibility, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.GainTalent(Properties.invisibility),
            U => U.ApplyTransient(Properties.invisibility, 1.d15() + 31),
            C =>
            {
              C.ApplyTransient(Properties.invisibility, 1.d15() + 31);
              C.ApplyTransient(Properties.aggravation, 1.d15() + 31);
            }
          );
        });
      });

      potion_of_levitation = AddPotion("potion of levitation", I =>
      {
        I.Description = null;
        I.SetAppearance("cyan potion", null);
        I.Glyph = Glyphs.cyan_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 38;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence3;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyTossUse(Motions.quaff, Properties.levitation, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.flight, 1.d50() + 250),
            U => U.ApplyTransient(Properties.levitation, 1.d140() + 10),
            C => C.ApplyTransient(Properties.levitation, 1.d300() + 300)
          );
        });
      });

      potion_of_monster_detection = AddPotion("potion of monster detection", I =>
      {
        I.Description = null;
        I.SetAppearance("bubbly potion", null);
        I.Glyph = Glyphs.bubbly_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 38;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence2;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(PotionWeakness);
        I.AddObviousUse(Motions.quaff, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.DetectCharacter(Range.Sq20);
              B.ApplyTransient(Properties.see_invisible, 1.d100() + 50);
            },
            U =>
            {
              U.DetectCharacter(Range.Sq15);
            },
            C =>
            {
              C.DetectCharacter(Range.Sq10);
              C.ApplyTransient(Properties.aggravation, 1.d20() + 10);
            }
          );
        });
      });

      /*
            potion_of_mutation = AddPotion("potion of mutation", I =>
            {
              I.Description = null;
              I.SetAppearance("bloody potion", null);
              I.Glyph = Glyphs.bloody_potion;
              I.Sonic = Sonics.potion;
                    I.Series = PotionSeries;
              I.Rarity = 10;
              I.Size = PotionSize;
              I.Weight = PotionWeight;
              I.Material = Materials.glass;
              I.Price = Gold.FromCoins(300);
              I.Essence = PotionEssence6;
              I.SetWeakness(PotionWeakness);
              I.AddQuaffAdvancement(Delay.FromTurns(15), Sonics.quaff, A =>
              {
                // increase/decrease ability
                // absorb equipped item ('eat')
                // transient life/mana regeneration
                // gain talent cannibalism, decrease charisma.
                // grow up! grow down!
                // gain dark vision, transient blindness
                // increase/decrease life/mana potential
                // change base race or polymorph
                // 

                A.WithSourceSanctity
                (
                  B =>
                  {
                  },
                  U =>
                  {
                  },
                  C =>
                  {
                  }
                );
              });
            });
      */

      potion_of_object_detection = AddPotion("potion of object detection", I =>
      {
        I.Description = null;
        I.SetAppearance("smoky potion", null);
        I.Glyph = Glyphs.smoky_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 38;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Price = Gold.FromCoins(150);
        I.Essence = PotionEssence2;
        I.SetWeakness(PotionWeakness);
        I.AddObviousUse(Motions.quaff, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.DetectAsset(Range.Sq20),
            U => U.DetectAsset(Range.Sq15),
            C => C.DetectAsset(Range.Sq10)
          );
        });
      });

      potion_of_ink = AddPotion("potion of ink", I =>
      {
        I.Description = "This convenient vial of ink can be used to replenish your markers.";
        I.SetAppearance("gloomy potion", null);
        I.Glyph = Glyphs.gloomy_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 10;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence0;
        I.Price = Gold.FromCoins(250);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyTossUse(Motions.quaff, Properties.blindness, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.Nutrition(Dice.Fixed(+50)),
            U => U.Nutrition(Dice.Fixed(+10)),
            C => C.Malnutrition(Dice.Fixed(+50))
          );
        });
        I.AddDiscoverUse(Motions.refill, Delay.FromTurns(30), Sonics.quaff, Use =>
        {
          Use.SetCast().FilterItem(magic_marker)
             .SetAssetIndividualised()
             .FilterCharged();
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.Charging(Dice.One, Dice.Fixed(100)), // 100%
            U => U.Charging(Dice.One, 1.d20() + 70),    // 71..90%
            C => C.Charging(Dice.One, 1.d20() + 30)     // 31..50%
          );
        });
        I.SetImpact(Sonics.broken_glass, A => A.ApplyTransient(Properties.blindness, 2.d6() + 6));
      });

      potion_of_oil = AddPotion("potion of oil", I =>
      {
        I.Description = "This handy canister of oil can be used to refuel your lamps and lanterns.";
        I.SetAppearance("murky potion", null);
        I.Glyph = Glyphs.murky_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 30;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence0;
        I.Price = Gold.FromCoins(250);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyTossUse(Motions.quaff, Properties.fumbling, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.Nutrition(Dice.Fixed(+50)),
            U => U.Nutrition(Dice.Fixed(+10)),
            C => C.Malnutrition(Dice.Fixed(+50))
          );
        });
        I.AddDiscoverUse(Motions.refill, Delay.FromTurns(30), Sonics.quaff, Use =>
        {
          Use.SetCast().FilterItem(oil_lamp, lantern)
             .SetAssetIndividualised()
             .FilterCharged();
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.Charging(Dice.One, Dice.Fixed(100)), // 100%
            U => U.Charging(Dice.One, 1.d20() + 70),    // 71..90%
            C => C.Charging(Dice.One, 1.d20() + 30)     // 31..50%
          );
        });
        I.SetImpact(Sonics.broken_glass, A => A.WithSourceSanctity
        (
          B =>
          {
            B.ApplyTransient(Properties.fumbling, 2.d6() + 6);
            B.ApplyTransient(Properties.slippery, 4.d6() + 6);
          },
          U =>
          {
            U.ApplyTransient(Properties.fumbling, 3.d6() + 6);
            U.ApplyTransient(Properties.slippery, 3.d6() + 6);
          },
          C =>
          {
            C.ApplyTransient(Properties.fumbling, 4.d6() + 6);
            C.ApplyTransient(Properties.slippery, 2.d6() + 6);
          }
        ));
      });

      potion_of_paralysis = AddPotion("potion of paralysis", I =>
      {
        I.Description = null;
        I.SetAppearance("emerald potion", null);
        I.Glyph = Glyphs.emerald_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 38;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence2;
        I.Price = Gold.FromCoins(300);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyTossUse(Motions.quaff, Properties.paralysis, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.paralysis, 1.d10() + 40),
            U => U.ApplyTransient(Properties.paralysis, 1.d10() + 100),
            C => C.ApplyTransient(Properties.paralysis, 1.d10() + 160)
          );
        });
      });

      potion_of_polymorph = AddPotion("potion of polymorph", I =>
      {
        I.Description = null;
        I.SetAppearance("golden potion", null);
        I.Glyph = Glyphs.golden_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 10;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence4;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyBuffUse(Motions.quaff, Properties.polymorph, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.ApplyTransient(Properties.polymorph_control, 10.d100());
              B.ApplyTransient(Properties.polymorph, 10.d100());
            },
            U => U.Polymorph(),
            C =>
            {
              C.Polymorph();
              C.ApplyTransient(Properties.paralysis, 10.d10());
            }
          );
        });
      });

      potion_of_rage = AddPotion("potion of rage", I =>
      {
        I.Description = null;
        I.SetAppearance("swirly potion", null);
        I.Glyph = Glyphs.swirly_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 20;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence4;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyTossUse(Motions.quaff, Properties.rage, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.rage, 2.d6() + 2),
            U => U.ApplyTransient(Properties.rage, 4.d6() + 4),
            C =>
            {
              C.ApplyTransient(Properties.berserking, 1.d100() + 750);
              C.ApplyTransient(Properties.rage, 4.d6() + 4);
            }
          );
        });
      });

      potion_of_recovery = AddPotion("potion of recovery", I =>
      {
        I.Description = null;
        I.SetAppearance("pink potion", null);
        I.Glyph = Glyphs.pink_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 40;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence4;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(PotionWeakness);
        I.AddObviousUse(Motions.quaff, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.RestoreAbility();
              B.Unafflict();
              B.Unpolymorph();
              B.RemoveTransient(Properties.blindness, Properties.deafness, Properties.hallucination, Properties.sickness, Properties.confusion, Properties.stunned, Properties.petrifying);
            },
            U =>
            {
              U.RestoreAbility();
              U.Unafflict();
              U.Unpolymorph();
            },
            C =>
            {
              C.ApplyTransient(Properties.sustain_ability, 1.d100() + 750);
            }
          );
        });
      });

      potion_of_see_invisible = AddPotion("potion of see invisible", I =>
      {
        I.Description = null;
        I.SetAppearance("magenta potion", null);
        I.Glyph = Glyphs.magenta_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 38;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence1;
        I.Price = Gold.FromCoins(50);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyBuffUse(Motions.quaff, Properties.see_invisible, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.GainTalent(Properties.see_invisible);
              B.RemoveTransient(Properties.blindness);
            },
            U =>
            {
              U.ApplyTransient(Properties.see_invisible, 1.d100() + 750);
              U.RemoveTransient(Properties.blindness);
            },
            C =>
            {
              C.ApplyTransient(Properties.see_invisible, 1.d100() + 50);
            }
          );
        });
      });

      potion_of_sickness = AddPotion("potion of sickness", I =>
      {
        I.Description = null;
        I.SetAppearance("fizzy potion", null);
        I.Glyph = Glyphs.fizzy_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 40;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence1;
        I.Price = Gold.FromCoins(50);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyTossUse(Motions.quaff, Properties.sickness, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.Harm(Elements.physical, Dice.One);
              B.ApplyTransient(Properties.sickness, 2.d40());
            },
            U =>
            {
              U.Harm(Elements.physical, 1.d10());
              U.DecreaseOneAbility(Dice.One);
              U.ApplyTransient(Properties.sickness, 6.d40());
            },
            C =>
            {
              C.Harm(Elements.physical, 1.d10() + 5);
              C.DecreaseOneAbility((1.d4() + 2)); // 3..6
              C.ApplyTransient(Properties.sickness, 10.d40());
            }
          );
        });
      });

      potion_of_sleeping = AddPotion("potion of sleeping", I =>
      {
        I.Description = null;
        I.SetAppearance("effervescent potion", null);
        I.Glyph = Glyphs.effervescent_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 40;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence2;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyTossUse(Motions.quaff, Properties.sleeping, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.sleeping, 1.d10() + 40),
            U => U.ApplyTransient(Properties.sleeping, 1.d10() + 100),
            C =>
            {
              C.ApplyTransient(Properties.narcolepsy, 1.d100() + 320);
              C.ApplyTransient(Properties.sleeping, 1.d10() + 160);
            }
          );
        });
      });

      potion_of_speed = AddPotion("potion of speed", I =>
      {
        I.Description = null;
        I.SetAppearance("dark green potion", null);
        I.Glyph = Glyphs.dark_green_potion;
        I.Sonic = Sonics.potion;
        I.Series = PotionSeries;
        I.Rarity = 38;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence4;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(PotionWeakness);
        I.AddPropertyBuffUse(Motions.quaff, Properties.quickness, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.quickness, 1.d10() + 160),
            U => U.ApplyTransient(Properties.quickness, 1.d10() + 100),
            C => C.ApplyTransient(Properties.quickness, 1.d10() + 40)
          );
          // TODO: heal legs.
        });
      });

      potion_of_water = AddPotion("potion of water", I =>
      {
        I.Description = null;
        I.SetAppearance("clear potion", null);
        I.Glyph = Glyphs.clear_potion;
        I.Sonic = Sonics.potion;
        I.Series = null;
        I.Rarity = 55;
        I.Size = PotionSize;
        I.Weight = PotionWeight;
        I.Material = Materials.glass;
        I.Essence = PotionEssence0;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(PotionWeakness);
        I.AddObviousUse(Motions.quaff, Delay.FromTurns(15), Sonics.quaff, Use =>
        {
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.Nutrition(Dice.Fixed(20)),
            U => U.Nutrition(Dice.Fixed(10)),
            C => C.Malnutrition(Dice.Fixed(10))
          );
        });
        I.AddDiscoverUse(Motions.anoint, Delay.FromTurns(10), Sonics.magic, Use =>
        {
          Use.SetCast().FilterAnyItem();
          Use.Consume();
          Use.Apply.WithSourceSanctity
          (
            B => B.Sanctify(null, Sanctities.Blessed),
            U => U.Nothing(),
            C => C.Sanctify(null, Sanctities.Cursed)
          );
        });
      });

      SetUpgradeDowngradeChain(potion_of_healing, potion_of_extra_healing, potion_of_full_healing);

      CodexRecruiter.Enrol(() =>
      {
        foreach (var Potion in Stocks.potion.Items.Where(P => P != potion_of_water && !P.Artifact && P.DowngradeItem == null))
          Register.Edit(Potion).SetDowngradeItem(potion_of_water);
      });
      #endregion

      #region ring.
      var RingSize = Size.Tiny;
      var RingWeight = Weight.FromUnits(30);
      var RingEssence0 = Essence.FromUnits(10);
      var RingEssence1 = Essence.FromUnits(20);
      var RingEssence2 = Essence.FromUnits(30);
      var RingEssence3 = Essence.FromUnits(40);
      var RingEssence4 = Essence.FromUnits(50);
      var RingEssence5 = Essence.FromUnits(60);
      var RingEssence6 = Essence.FromUnits(100);
      var RingEssence7 = Essence.FromUnits(200);
      var RingSeries = new Series("ring");
      var RingWeakness = new[] { Elements.shock };

      ring_of_adornment = AddRing("ring of adornment", I =>
      {
        I.Description = null;
        I.SetAppearance("wooden ring", null);
        I.Glyph = Glyphs.wooden_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.wood;
        I.Essence = RingEssence2;
        I.Price = Gold.FromCoins(100);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.WithSourceSanctity
          (
            B => B.IncreaseAbility(Attributes.charisma, 1.d3()),
            U => U.IncreaseAbility(Attributes.charisma, Dice.One),
            C => C.DecreaseAbility(Attributes.charisma, Dice.One)
          );
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetBoostAttribute(Attributes.charisma);
      });

      ring_of_aggravation = AddRing("ring of aggravation", I =>
      {
        I.Description = null;
        I.SetAppearance("sapphire ring", null);
        I.Glyph = Glyphs.sapphire_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gemstone;
        I.Essence = RingEssence1;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.aggravation);
        });
        I.SetWeakness(RingWeakness);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.aggravation);
      });

      ring_of_berserking = AddRing("ring of berserking", I =>
      {
        I.Description = "The wearer will occasionally fly into uncontrollable rage lashing out at both friend and foe.";
        I.SetAppearance("shiny ring", null);
        I.Glyph = Glyphs.shiny_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.iron;
        I.Essence = RingEssence5;
        I.Price = Gold.FromCoins(100);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.berserking);
          A.ApplyTransient(Properties.rage, 2.d20());
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.berserking);
      });

      ring_of_cold_resistance = AddRing("ring of cold resistance", I =>
      {
        I.Description = null;
        I.SetAppearance("brass ring", null);
        I.Glyph = Glyphs.brass_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.copper;
        I.Essence = RingEssence4;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.ConsumeResistance(Elements.cold);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetResistance(Elements.cold);
      });

      ring_of_conflict = AddRing("ring of conflict", I =>
      {
        I.Description = null;
        I.SetAppearance("ruby ring", null);
        I.Glyph = Glyphs.ruby_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gemstone;
        I.Essence = RingEssence6;
        I.Price = Gold.FromCoins(300);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.conflict);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.conflict);
      });

      ring_of_fire_resistance = AddRing("ring of fire resistance", I =>
      {
        I.Description = null;
        I.SetAppearance("iron ring", null);
        I.Glyph = Glyphs.iron_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.iron;
        I.Essence = RingEssence4;
        I.Price = Gold.FromCoins(200);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.ConsumeResistance(Elements.fire);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetResistance(Elements.fire);
      });

      ring_of_free_action = AddRing("ring of free action", I =>
      {
        I.Description = null;
        I.SetAppearance("twisted ring", null);
        I.Glyph = Glyphs.twisted_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.iron;
        I.Essence = RingEssence6;
        I.Price = Gold.FromCoins(200);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.free_action);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.free_action);
      });

      ring_of_constitution = AddRing("ring of constitution", I =>
      {
        I.Description = null;
        I.SetAppearance("opal ring", null);
        I.Glyph = Glyphs.opal_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.stone;
        I.Essence = RingEssence2;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.WithSourceSanctity
          (
            B => B.IncreaseAbility(Attributes.constitution, 1.d3()),
            U => U.IncreaseAbility(Attributes.constitution, Dice.One),
            C => C.DecreaseAbility(Attributes.constitution, Dice.One)
          );
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetBoostAttribute(Attributes.constitution);
      });

      ring_of_dexterity = AddRing("ring of dexterity", I =>
      {
        I.Description = null;
        I.SetAppearance("obsidian ring", null);
        I.Glyph = Glyphs.obsidian_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gemstone;
        I.Essence = RingEssence2;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.WithSourceSanctity
          (
            B => B.IncreaseAbility(Attributes.dexterity, 1.d3()),
            U => U.IncreaseAbility(Attributes.dexterity, Dice.One),
            C => C.DecreaseAbility(Attributes.dexterity, Dice.One)
          );
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetBoostAttribute(Attributes.dexterity);
      });

      ring_of_intelligence = AddRing("ring of intelligence", I =>
      {
        I.Description = null;
        I.SetAppearance("plain ring", null);
        I.Glyph = Glyphs.plain_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.stone;
        I.Essence = RingEssence2;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.WithSourceSanctity
          (
            B => B.IncreaseAbility(Attributes.intelligence, 1.d3()),
            U => U.IncreaseAbility(Attributes.intelligence, Dice.One),
            C => C.DecreaseAbility(Attributes.intelligence, Dice.One)
          );
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetBoostAttribute(Attributes.intelligence);
      });

      ring_of_strength = AddRing("ring of strength", I =>
      {
        I.Description = null;
        I.SetAppearance("granite ring", null);
        I.Glyph = Glyphs.granite_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.stone;
        I.Essence = RingEssence2;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.WithSourceSanctity
          (
            B => B.IncreaseAbility(Attributes.strength, 1.d3()),
            U => U.IncreaseAbility(Attributes.strength, Dice.One),
            C => C.DecreaseAbility(Attributes.strength, Dice.One)
          );
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetBoostAttribute(Attributes.strength);
      });

      ring_of_wisdom = AddRing("ring of wisdom", I =>
      {
        I.Description = null;
        I.SetAppearance("glass ring", null);
        I.Glyph = Glyphs.glass_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.glass;
        I.Essence = RingEssence2;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.WithSourceSanctity
          (
            B => B.IncreaseAbility(Attributes.wisdom, 1.d3()),
            U => U.IncreaseAbility(Attributes.wisdom, Dice.One),
            C => C.DecreaseAbility(Attributes.wisdom, Dice.One)
          );
        });
        I.SetImpact(Sonics.broken_glass);
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetBoostAttribute(Attributes.wisdom);
      });

      ring_of_hunger = AddRing("ring of hunger", I =>
      {
        I.Description = null;
        I.SetAppearance("topaz ring", null);
        I.Glyph = Glyphs.topaz_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gemstone;
        I.Essence = RingEssence1;
        I.Price = Gold.FromCoins(100);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.hunger);
          A.Malnutrition(Dice.Fixed(500));
        });
        I.SetWeakness(RingWeakness);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.hunger);
      });

      ring_of_accuracy = AddRing("ring of accuracy", I =>
      {
        I.Description = null;
        I.SetAppearance("clay ring", null);
        I.Glyph = Glyphs.clay_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.clay;
        I.Essence = RingEssence3;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.WhenChance(Chance.OneIn2, T => T.GainSkill(RandomPoints: false, Skills.bow, Skills.crossbow, Skills.firearms, Skills.disc, Skills.dart, Skills.sling), E => E.Nothing());
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetAttackBoost();
      });

      ring_of_impact = AddRing("ring of impact", I =>
      {
        I.Description = "This ring augments the damage caused by your strikes";
        I.SetAppearance("coral ring", null);
        I.Glyph = Glyphs.coral_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.stone;
        I.Essence = RingEssence3;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.WhenChance(Chance.OneIn2, T => T.GainSkill(RandomPoints: false, Skills.heavy_blade, Skills.hammer, Skills.axe, Skills.club, Skills.mace, Skills.pick), E => E.Nothing());
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetDamageBoost();
      });

      ring_of_invisibility = AddRing("ring of invisibility", I =>
      {
        I.Description = null;
        I.SetAppearance("wire ring", null);
        I.Glyph = Glyphs.wire_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.iron;
        I.Essence = RingEssence6;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.invisibility);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.invisibility);
      });

      ring_of_levitation = AddRing("ring of levitation", I =>
      {
        I.Description = null;
        I.SetAppearance("agate ring", null);
        I.Glyph = Glyphs.agate_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gemstone;
        I.Essence = RingEssence5;
        I.Price = Gold.FromCoins(200);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.levitation);
        });
        I.SetWeakness(RingWeakness);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.levitation);
      });

      ring_of_naught = AddRing("ring of naught", I =>
      {
        I.Description = null;
        I.SetAppearance("mood ring", null, Price: Gold.FromCoins(150));
        I.Glyph = Glyphs.mood_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gemstone;
        I.Essence = RingEssence0;
        I.Price = Gold.FromCoins(25);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.Nothing();
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring);
        // NOTE: does nothing.
      });

      ring_of_poison_resistance = AddRing("ring of poison resistance", I =>
      {
        I.Description = null;
        I.SetAppearance("pearl ring", null);
        I.Glyph = Glyphs.pearl_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gemstone;
        I.Essence = RingEssence4;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.ConsumeResistance(Elements.poison);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetResistance(Elements.poison);
      });

      ring_of_polymorph = AddRing("ring of polymorph", I =>
      {
        I.Description = null;
        I.SetAppearance("ivory ring", null);
        I.Glyph = Glyphs.ivory_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.bone;
        I.Essence = RingEssence6;
        I.Price = Gold.FromCoins(300);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.polymorph);
          A.Polymorph();
        });
        I.SetWeakness(RingWeakness);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.polymorph);
      });

      ring_of_polymorph_control = AddRing("ring of polymorph control", I =>
      {
        I.Description = null;
        I.SetAppearance("emerald ring", null);
        I.Glyph = Glyphs.emerald_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gemstone;
        I.Essence = RingEssence7;
        I.Price = Gold.FromCoins(300);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.polymorph_control);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.polymorph_control);
      });

      ring_of_protection = AddRing("ring of protection", I =>
      {
        I.Description = null;
        I.SetAppearance("black onyx ring", null);
        I.Glyph = Glyphs.black_onyx_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.stone;
        I.Essence = RingEssence3;
        I.Price = Gold.FromCoins(100);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.WhenChance(Chance.OneIn2, T => T.GainSkill(RandomPoints: false, Skills.heavy_armour, Skills.medium_armour, Skills.light_armour), E => E.Nothing());
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetDefenceBoost();
      });

      ring_of_regeneration = AddRing("ring of regeneration", I =>
      {
        I.Description = null;
        I.SetAppearance("moonstone ring", null);
        I.Glyph = Glyphs.moonstone_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.stone;
        I.Essence = RingEssence6;
        I.Price = Gold.FromCoins(200);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.life_regeneration);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.life_regeneration);
      });

      ring_of_searching = AddRing("ring of searching", I =>
      {
        I.Description = null;
        I.SetAppearance("tiger eye ring", null);
        I.Glyph = Glyphs.tiger_eye_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gemstone;
        I.Essence = RingEssence5;
        I.Price = Gold.FromCoins(200);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.searching);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.searching);
      });

      ring_of_see_invisible = AddRing("ring of see invisible", I =>
      {
        I.Description = null;
        I.SetAppearance("engagement ring", null);
        I.Glyph = Glyphs.engagement_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gemstone;
        I.Essence = RingEssence1;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.see_invisible);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.see_invisible);
      });

      ring_of_shock_resistance = AddRing("ring of shock resistance", I =>
      {
        I.Description = null;
        I.SetAppearance("copper ring", null);
        I.Glyph = Glyphs.copper_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.copper;
        I.Essence = RingEssence4;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.ConsumeResistance(Elements.shock);
        });
        //I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetResistance(Elements.shock);
      });

      ring_of_sleeping = AddRing("ring of sleeping", I =>
      {
        I.Description = null;
        I.SetAppearance("wedding ring", null);
        I.Glyph = Glyphs.wedding_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gold;
        I.Essence = RingEssence1;
        I.Price = Gold.FromCoins(100);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.narcolepsy);
          A.ApplyTransient(Properties.sleeping, 1.d25() + 25);
        });
        I.SetWeakness(RingWeakness);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.narcolepsy);
      });

      ring_of_slow_digestion = AddRing("ring of slow digestion", I =>
      {
        I.Description = null;
        I.SetAppearance("steel ring", null);
        I.Glyph = Glyphs.steel_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.iron;
        I.Essence = RingEssence6;
        I.Price = Gold.FromCoins(200);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.slow_digestion);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.slow_digestion);
      });

      ring_of_stealth = AddRing("ring of stealth", I =>
      {
        I.Description = null;
        I.SetAppearance("jade ring", null);
        I.Glyph = Glyphs.jade_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gemstone;
        I.Essence = RingEssence5;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.stealth);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.stealth);
        });
      });

      ring_of_sustain_ability = AddRing("ring of sustain ability", I =>
      {
        I.Description = null;
        I.SetAppearance("bronze ring", null);
        I.Glyph = Glyphs.bronze_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.copper;
        I.Essence = RingEssence1;
        I.Price = Gold.FromCoins(100);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.sustain_ability);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.sustain_ability);
      });

      ring_of_telekinesis = AddRing("ring of telekinesis", I =>
      {
        I.Description = null;
        I.SetAppearance("mithril ring", null);
        I.Glyph = Glyphs.mithril_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.mithril;
        I.Essence = RingEssence5;
        I.Price = Gold.FromCoins(250);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.telekinesis);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.telekinesis);
      });

      ring_of_teleport_control = AddRing("ring of teleport control", I =>
      {
        I.Description = null;
        I.SetAppearance("gold ring", null);
        I.Glyph = Glyphs.gold_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gold;
        I.Essence = RingEssence7;
        I.Price = Gold.FromCoins(300);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.teleport_control);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.teleport_control);
      });

      ring_of_teleportation = AddRing("ring of teleportation", I =>
      {
        I.Description = null;
        I.SetAppearance("silver ring", null);
        I.Glyph = Glyphs.silver_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.silver;
        I.Essence = RingEssence6;
        I.Price = Gold.FromCoins(200);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.teleportation);
        });
        I.SetWeakness(RingWeakness);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.teleportation);
      });

      ring_of_warning = AddRing("ring of warning", I =>
      {
        I.Description = null;
        I.SetAppearance("diamond ring", null);
        I.Glyph = Glyphs.diamond_ring;
        I.Sonic = Sonics.ring;
        I.Series = RingSeries;
        I.Rarity = 100;
        I.Size = RingSize;
        I.Weight = RingWeight;
        I.Material = Materials.gemstone;
        I.Essence = RingEssence6;
        I.Price = Gold.FromCoins(100);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.warning);
        });
        I.SetWeakness(RingWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.ring)
         .SetTalent(Properties.warning);
      });

      CodexRecruiter.Enrol(() =>
      {
        foreach (var Ring in Stocks.ring.Items.Where(R => R.Type == ItemType.Ring && R != ring_of_naught && !R.Artifact))
          Register.Edit(Ring).SetDowngradeItem(ring_of_naught);
      });
      #endregion

      #region earrings.
      var EarringsSeries = new Series("earrings");
      var EarringsWeakness = new[] { Elements.shock };

      costume_earrings = AddItem(Stocks.ring, ItemType.Earwear, "costume earrings", I =>
      {
        I.Description = null;
        I.SetAppearance("gold earrings", null);
        I.Glyph = Glyphs.gold_earrings;
        I.Sonic = Sonics.ring;
        I.Series = EarringsSeries;
        I.Rarity = 10;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.gold;
        I.Essence = RingEssence0;
        I.Price = Gold.FromCoins(120);
        I.SetWeakness(EarringsWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.ring);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.ring);
      });

      mute_earrings = AddItem(Stocks.ring, ItemType.Earwear, "mute earrings", I =>
      {
        I.Description = null;
        I.SetAppearance("bone earrings", null);
        I.Glyph = Glyphs.bone_earrings;
        I.Sonic = Sonics.ring;
        I.Series = EarringsSeries;
        I.Rarity = 10;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.bone;
        I.Essence = RingEssence4;
        I.Price = Gold.FromCoins(120);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetWeakness(EarringsWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.ring)
         .SetTalent(Properties.silence);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.silence);
        });
      });

      proof_earrings = AddItem(Stocks.ring, ItemType.Earwear, "proof earrings", I =>
      {
        I.Description = null;
        I.SetAppearance("pearl earrings", null);
        I.Glyph = Glyphs.pearl_earrings;
        I.Sonic = Sonics.ring;
        I.Series = EarringsSeries;
        I.Rarity = 10;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.gemstone;
        I.Essence = RingEssence4;
        I.Price = Gold.FromCoins(120);
        I.SetWeakness(EarringsWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(20), Sonics.ring)
         .SetTalent(Properties.appraisal);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.ring, A =>
        {
          A.MajorProperty(Properties.appraisal);
        });
      });

      CodexRecruiter.Enrol(() =>
      {
        foreach (var Earring in Stocks.ring.Items.Where(R => R.Type == ItemType.Earwear && R != costume_earrings && !R.Artifact))
          Register.Edit(Earring).SetDowngradeItem(costume_earrings);
      });
      #endregion

      #region scroll.
      var ScrollSize = Size.Small;
      var ScrollWeight = Weight.FromUnits(50);
      var ScrollEssence0 = Essence.FromUnits(5);
      var ScrollEssence1 = Essence.FromUnits(10);
      var ScrollEssence2 = Essence.FromUnits(25);
      var ScrollEssence3 = Essence.FromUnits(50);
      var ScrollEssence4 = Essence.FromUnits(100);
      var ScrollWeakness = new[] { Elements.fire };
      var ScrollSeries = new Series("scroll");

      scroll_of_amnesia = AddScroll("scroll of amnesia", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled DUAM XNAHT", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 35;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence1;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.psychic, Dice.Zero)
             .SetTerminates(false); // NOTE: also affects your steed so that they will 'forget' that you are allied.
          Use.Apply.WhenConfused
          (
            T => T.LearnSpell(Codex.Spells.confusion),
            F => F.WithSourceSanctity
            (
              B => B.Amnesia(Range.Sq10),
              U => U.Amnesia(Range.Sq15),
              C => C.Amnesia(Range.Sq20)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.Amnesia(Range.Sq60);
        });
      });

      scroll_of_blank_paper = AddScroll("scroll of blank paper", I =>
      {
        I.Description = null;
        I.SetAppearance("unlabelled scroll", null);
        I.Glyph = Glyphs.scroll_of_blank_paper;
        I.Sonic = Sonics.scroll;
        I.Series = null;
        I.Rarity = 28;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence0;
        I.Price = Gold.FromCoins(10);
        I.SetWeakness(ScrollWeakness);
        I.AddPretendUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Apply.Nothing();
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll);
      });

      scroll_of_charging = AddScroll("scroll of charging", I =>
      {
        I.Description = "This utility scroll restores some of the charges to your spent wands and tools.";
        I.SetAppearance("scroll labelled HACKEM MUCHE", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 15;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence3;
        I.Price = Gold.FromCoins(300);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().FilterCharged();
          Use.Apply.WhenConfused
          (
            T => T.WithSourceSanctity
            (
              B => B.Energise(Dice.Fixed(+300), Modifier.Plus5),
              U => U.Energise(Dice.Fixed(+100), Modifier.Zero),
              C => C.Diminish(Dice.Fixed(+300), Modifier.Plus5)
            ),
            E => E.WithSourceSanctity
            (
              B => B.Charging(Dice.One, Dice.Fixed(100)), // 100%
              U => U.Charging(Dice.One, 1.d20() + 70),    // 71..90%
              C => C.Charging(Dice.One, 1.d20() + 30)     // 31..50%
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.ConsumeResistance(Elements.shock);
        });
      });

      scroll_of_confusion = AddScroll("scroll of confusion", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled NR 9", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 43;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence1;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(ScrollWeakness);
        I.AddBlastUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.psychic, Dice.One)
             .SetAudibility(10);
          Use.Apply.WhenConfused
          (
            T => T.RemoveTransient(Properties.confusion),
            E => E.WithSourceSanctity
            (
              B => B.ApplyTransient(Properties.confusion, 3.d6() + 3),
              U => U.ApplyTransient(Properties.confusion, 2.d6() + 2),
              C => C.ApplyTransient(Properties.confusion, 1.d6() + 1)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.stunned, 5.d6()), E => E.ApplyTransient(Properties.confusion, 5.d20()));
        });
      });

      scroll_of_destruction = AddScroll("scroll of destruction", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled JUYED AWK YACC", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 45;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence2;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          var DestroyArray = new[] { Stocks.armour, Stocks.weapon, Stocks.ring, Stocks.tool, Stocks.amulet };

          Use.Consume();
          Use.SetCast().FilterStock(DestroyArray);
          Use.Apply.WhenConfused
          (
            T => T.CreateAsset(Dice.Fixed(1), DestroyArray),
            E => E.WithSourceSanctity
            (
              B => B.DestroyEquippedAsset(Dice.One, DestroyArray, new[] { Sanctities.Cursed }, null),
              U => U.DestroyEquippedAsset(Dice.One, DestroyArray, null, null),
              C =>
              {
                C.DestroyEquippedAsset(Dice.One, DestroyArray, null, null);
                C.ApplyTransient(Properties.stunned, 1.d10() + 1);
              }
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.Harm(Elements.physical, 4.d6() + 4);
        });
      });

      scroll_of_devouring = AddScroll("scroll of devouring", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled OMNO MNOM", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 15;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence2;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().FilterEdibleItem(false);
          Use.Apply.WhenConfused
          (
            T => T.Polymorph(Entities.killer_food_ration),
            E => E.WithSourceSanctity
            (
              B => B.DevourAsset(Sanctities.Cursed), // devour only cursed items.
              U => U.DevourAsset(Sanctities.Uncursed), // devour only uncursed or cursed items.
              C =>
              {
                C.DevourAsset(Sanctities.Blessed); // devour blessed, uncursed or cursed items.
                C.ApplyTransient(Properties.hunger, 1.d10() + 10);
              }
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.Punish(Codex.Punishments.gluttony);
        });
      });

      scroll_of_air = AddScroll("scroll of air", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled AZZGAZ", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 20;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence3;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(ScrollWeakness);
        I.AddPropertyAreaUse(Motions.read, Properties.silence, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates()
             .SetAudibility(5);
          Use.Apply.WhenConfused
          (
            T => T.CreateEntity(Dice.Fixed(8), Entities.gas_spore),
            E => E.WithSourceSanctity
            (
              B =>
              {
                B.AreaTransient(Properties.silence, 4.d50());
                B.AreaTransient(Properties.sickness, 4.d10());
              },
              U =>
              {
                U.AreaTransient(Properties.silence, 2.d50());
                U.AreaTransient(Properties.sickness, 2.d10());
              },
              C =>
              {
                C.ApplyTransient(Properties.silence, 4.d50());
                C.ApplyTransient(Properties.sickness, 4.d10());
              }
            )
          );

        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          // TODO: eating air does what?
          A.WhenChance(Chance.OneIn2, T => T.Polymorph(Entities.air_elemental), E => E.Polymorph(Entities.energy_vortex));
        });
      });

      scroll_of_earth = AddScroll("scroll of earth", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled KIRJE", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 20;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence3;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(ScrollWeakness);
        I.AddBlockUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.boost, Dice.One)
           .SetTerminates()
           .SetAudibility(10);
          Use.Apply.WhenConfused
          (
            T => T.CreateAsset(5.d6(), rock), // 5d6 x (1d6 + 6) can be a lot of rocks.
            E => E.WithSourceSanctity
            (
              B => B.CreateBoulder(Dice.One),
              U => U.CreateBoulder(1.d3()),
              C => C.CreateBoulder(Dice.Fixed(9))
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.CreateAsset(1.d3(), rock); // poop rocks (quantity is more than this)!
          A.CreateAsset(1.d2() - 1, diamond); // maybe something valuable comes out.
          A.WhenChance(Chance.OneIn2, T => T.Polymorph(Entities.earth_elemental), E => E.Polymorph(Entities.dust_vortex));
        });
      });

      scroll_of_enchantment = AddScroll("scroll of enchantment", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled DAIYEN FOOELS", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 143;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence4;
        I.Price = Gold.FromCoins(140);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().FilterEnchanted()
             .SetAssetIndividualised();

          Use.Apply.WhenConfused
          (
            T => T.Cancellation(Elements.magical),
            E => E.WithSourceSanctity
            (
              B => B.EnchantUp(1.d3()),
              U => U.EnchantUp(Dice.One),
              C => C.EnchantDown(Dice.One)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.WhenChance(Chance.OneIn2,
            T => T.IncreaseAbilities(Attributes.List, Dice.One),
            E => E.IncreaseOneAbility(Dice.One));
        });
      });

      scroll_of_enlightenment = AddScroll("scroll of enlightenment", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled VELOX NEB", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 99;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence2;
        I.Price = Gold.FromCoins(20);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.psychic, Dice.Zero)
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T =>
            {
              T.Amnesia(Range.Sq30);
              T.Mapping(Range.Sq30, Chance.Always);
              T.DetectAsset(Range.Sq30);
            },
            E => E.WithSourceSanctity
            (
              B =>
              {
                B.Enlightenment(scroll_of_amnesia);
                B.Enlightenment(potion_of_amnesia);
                B.Enlightenment(null);
              },
              U =>
              {
                U.Enlightenment(null);
              },
              C =>
              {
                C.Enlightenment(null);
                C.ApplyTransient(Properties.hallucination, 10.d10());
              }
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.WhenChance(Chance.OneIn2, T => T.IncreaseAbility(Attributes.wisdom, Dice.One), E => E.ApplyTransient(Properties.hallucination, 10.d100()));
        });
      });

      scroll_of_entrapment = AddScroll("scroll of entrapment", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled GABZPLOIT", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 20;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence2;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.boost, Dice.One)
             .SetAudibility(2)
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T => T.CreateEntity(4.d2(), Entities.trapper), // 4-8 trappers.
            E => E.WithSourceSanctity
            (
              B =>
              {
                B.CreateTrap(null, Destruction: false);
                B.ApplyTransient(Properties.paralysis, 4.d6());
              },
              U => U.CreateTrap(null, Destruction: false),
              C => C.Backfire(Z => Z.CreateTrap(null, Destruction: false))
            )
          );

        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.CreateAsset(Dice.One, Codex.Items.beartrap);
          A.CreateAsset(Dice.One, Codex.Items.caltrops);
          A.CreateAsset(Dice.One, Codex.Items.land_mine);
        });
      });

      scroll_of_fire = AddScroll("scroll of fire", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled ANDOVA BEGARIN", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 33;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence1;
        I.Price = Gold.FromCoins(100);
        //I.SetWeakness(ScrollWeakness); // immune to fire.
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Explosion(Explosions.fiery, Dice.Zero);
          Use.Apply.WhenConfused
          (
            T => T.CreateTrap(Codex.Devices.fire_trap, Destruction: false),
            E => E.WithSourceSanctity
            (
              B => B.Harm(Elements.fire, 3.d3()),
              U => U.Harm(Elements.fire, 4.d3()),
              C => C.Harm(Elements.fire, 5.d3())
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          //A.Harm(Elements.fire, 5.d6() + 5); // NOTE: this effectively means you can't turn a bird pet into a fire elemental/vortex (because they die first).
          A.ConsumeResistance(Elements.fire);
          A.WhenChance(Chance.OneIn2, T => T.Polymorph(Entities.fire_elemental), E => E.Polymorph(Entities.fire_vortex));
        });
      });

      scroll_of_ice = AddScroll("scroll of ice", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled ECI ALLINAV", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 33;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence1;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Explosion(Explosions.frosty, Dice.Zero);
          Use.Apply.WhenConfused
          (
            T => T.CreateTrap(Codex.Devices.ice_trap, Destruction: false),
            E => E.WithSourceSanctity
            (
              B => B.Harm(Elements.cold, 3.d3()),
              U => U.Harm(Elements.cold, 4.d3()),
              C => C.Harm(Elements.cold, 5.d3())
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          //A.Harm(Elements.cold, 5.d6() + 5); // NOTE: this effectively means you can't turn a bird pet into an ice elemental/vortex (because they die first).
          A.ConsumeResistance(Elements.cold);
          A.WhenChance(Chance.OneIn2, T => T.Polymorph(Entities.ice_elemental), E => E.Polymorph(Entities.ice_vortex));
        });
      });

      scroll_of_water = AddScroll("scroll of water", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled FOOBIE BLETCH", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 33;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence1;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Explosion(Explosions.watery, Dice.Zero);
          Use.Apply.WhenConfused
          (
            T => T.CreateTrap(Codex.Devices.water_trap, Destruction: false),
            E =>
            {
              E.WhenChance(Chance.OneIn20, V => V.ConvertAsset(Codex.Stocks.potion, WholeStack: true, Codex.Items.potion_of_water));
              E.WhenChance(Chance.OneIn20, V => V.ConvertAsset(Codex.Stocks.scroll, WholeStack: true, Codex.Items.scroll_of_blank_paper));
              E.WhenChance(Chance.OneIn20, V => V.ConvertAsset(Codex.Stocks.book, WholeStack: true, Codex.Items.book_of_blank_paper));
              E.WithSourceSanctity
              (
                B =>
                {
                  B.Harm(Elements.water, Dice.Zero); // TODO: BCU
                  B.Unpunish();
                  B.RemoveCurse(Dice.One);
                },
                U =>
                {
                  U.Harm(Elements.water, Dice.Zero);
                },
                C =>
                {
                  C.Harm(Elements.water, Dice.Zero);
                  C.PlaceCurse(Dice.One, Sanctities.Cursed);
                }
              );
            }
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.Nutrition(Dice.Fixed(100));
          A.WhenChance(Chance.OneIn2, T => T.Polymorph(Entities.water_elemental), E => E.Polymorph(Entities.steam_vortex));
        });
      });

      scroll_of_food_detection = AddScroll("scroll of food detection", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled YUM YUM", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 25;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence1;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T => T.DetectAsset(Range.Sq15, Stocks.potion),
            E => E.WithSourceSanctity
            (
              B => B.DetectAsset(Range.Sq20, Stocks.food, Stocks.potion),
              U => U.DetectAsset(Range.Sq15, Stocks.food),
              C => C.DestroyCarriedAsset(Dice.One, new[] { Stocks.food }, null, null)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.Nutrition(Dice.Fixed(+1000));
        });
      });

      scroll_of_gathering = AddScroll("scroll of gathering", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled HEREYEET", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 20;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence2;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T => T.Gather(Range.Sq30, Items: false, Characters: false, Boulders: true),
            E => E.WithSourceSanctity
            (
              B => B.Gather(Range.Sq15, Items: true, Characters: false, Boulders: false),
              U => U.Gather(Range.Sq10, Items: true, Characters: false, Boulders: false),
              C => C.Gather(Range.Sq5, Items: true, Characters: false, Boulders: false)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.Gather(Range.Sq15, Items: false, Characters: true, Boulders: false);
        });
      });

      scroll_of_genocide = AddScroll("scroll of genocide", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled ELBIB YLOH", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 15;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence4;
        I.Price = Gold.FromCoins(300);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetAudibility(10)
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T => T.CloneSourceCharacter(1.d3()), // clones self in confusion.
            E => E.WithSourceSanctity
            (
              B => B.Genocide(true),
              U => U.Genocide(false),
              C => C.SpawnCharacter(1.d3() + 4)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.Death(Elements.magical, new Kind[] { }, Strikes.death, Cause: DeathSupport.genocide);
        });
      });

      scroll_of_gold_detection = AddScroll("scroll of gold detection", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled THARR", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 33;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence1;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T => T.DetectTrap(Range.Sq15),
            E => E.WithSourceSanctity
            (
              B => B.DetectAsset(Range.Sq20, Materials.gold),
              U => U.DetectAsset(Range.Sq15, Materials.gold),
              C => C.DestroyOwnedAsset(4.d10() + 10, null, null, new[] { Materials.gold })
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.CreateAsset(10.d100(), gold_coin);
        });
      });

      scroll_of_identify = AddScroll("scroll of identify", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled KERNOD WEL", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 185;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence2;
        I.Price = Gold.FromCoins(20);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, new Utility(Purpose.Identify), Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().FilterIdentified(false)
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T => T.ApplyTransient(Properties.beatitude, 10.d100()),
            F => F.WithSourceSanctity
            (
              B => B.Identify(All: false, Sanctities.Blessed),
              U => U.Identify(All: false, Sanctity: null),
              C => C.Identify(All: false, Sanctities.Cursed)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.Identify(All: true, Sanctity: null);
        });
      });

      scroll_of_light = AddScroll("scroll of light", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled VERR YED HORRE", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 90;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence1;
        I.Price = Gold.FromCoins(50);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T =>
            {
              T.CreateEntity(Dice.Fixed(1), Entities.black_light);
            },
            E => E.WithSourceSanctity
            (
              B =>
              {
                B.Light(true);
                B.Heal(1.d4() + 1, Modifier.Zero);
              },
              U => U.Light(true),
              C => C.Light(false)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.Polymorph(Entities.yellow_light);
        });
      });

      scroll_of_magic_mapping = AddScroll("scroll of magic mapping", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled ELAM EBOW", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 45;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence3;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.boost, Dice.Zero)
           .SetTerminates();
          Use.Apply.WhenConfused
          (
            T => T.Mapping(Range.Sq30, Chance.OneIn3),
            F => F.WithSourceSanctity
            (
              B =>
              {
                B.DetectTrap(Range.Sq10);
                B.Searching(Range.Sq5);
                B.Mapping(Range.Sq20, Chance.Always);
              },
              U =>
              {
                U.Mapping(Range.Sq15, Chance.Always);
              },
              C =>
              {
                C.Concealing(Range.Sq30);
                C.Mapping(Range.Sq10, Chance.OneIn2);
              }
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.Mapping(Range.Sq30, Chance.Always, ThisLevel: false);
        });
      });

      scroll_of_murder = AddScroll("scroll of murder", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled GARVEN DEH", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 15;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence4;
        I.Price = Gold.FromCoins(300);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.death, Dice.Zero)
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T => T.AreaTransient(Properties.lifesaving, 10.d100(), Kinds.Living.ToArray()),
            F =>
            {
              F.Karma(ChangeType.Decrease, Dice.Fixed(250));
              F.WithSourceSanctity
              (
                B => B.Murder(MurderType.Hostile, Strikes.death, Kinds.Living.ToArray()),
                U => U.Murder(MurderType.Every, Strikes.death, Kinds.Living.ToArray()),
                C => C.Murder(MurderType.Allied, Strikes.death, Kinds.Living.ToArray())
              );
            }
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: DeathSupport.murder);
        });
      });

      scroll_of_punishment = AddScroll("scroll of punishment", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled VE FORBRYDERNE", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 25;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence3;
        I.Price = Gold.FromCoins(80);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, /*new Utility(Purpose.Punish),*/ Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Plain(Dice.Zero) // can't be range 1, because then the player would be prompted to target.
             .SetAudibility(10);
          Use.Apply.WhenConfused
          (
            T =>
            {
              T.WithSourceSanctity
              (
                B => B.Karma(ChangeType.Increase, 6.d50()),
                U => U.Karma(ChangeType.Increase, 3.d50()),
                C => C.Karma(ChangeType.Increase, 1.d50())
              );
            },
            F =>
            {
              F.WithSourceSanctity
              (
                B => B.Punish(Codex.Punishments.List.ToArray()),
                U => U.Punish(Codex.Punishments.List.ToArray()),
                C => C.Punish(Codex.Punishments.List.ToArray())
              );
            }
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.Punish(Codex.Punishments.gluttony);
        });
      });

      scroll_of_raise_dead = AddScroll("scroll of raise dead", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled TEMOV", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 20;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence4;
        I.Price = Gold.FromCoins(300);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, new Utility(Purpose.SummonEnemy, ItemArray: new[] { animal_corpse, vegetable_corpse }), Delay.FromTurns(15), Sonics.read, Use => // TODO: this is ENEMY, because raise dead does not have any charming effects.
        {
          Use.Consume();
          Use.SetCast().FilterItem(animal_corpse, vegetable_corpse)
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T => T.CreateEntity(1.d4(), Kinds.Undead.ToArray()),
            F => F.WithSourceSanctity
            (
              B => B.RaiseDead(Percent: 100, Corrupt: false),
              U => U.RaiseDead(Percent: 50, Corrupt: false),
              C => C.RaiseDead(Percent: 100, Corrupt: true)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.ApplyTransient(Properties.lifesaving, 5.d100());
        });
      });

      scroll_of_remove_curse = AddScroll("scroll of remove curse", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled PRATYAVAYAH", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 65;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence3;
        I.Price = Gold.FromCoins(80);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().FilterSanctity(Sanctities.Cursed)
             .SetPunishmentOverride()
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T => T.WithSourceSanctity
            (
              B => B.PlaceCurse(Dice.Zero, Sanctities.Cursed),
              U => U.PlaceCurse(Dice.One, Sanctities.Cursed),
              C => C.PlaceCurse(3.d6(), Sanctities.Cursed)
            ),
            F =>
            {
              F.Unpunish();
              F.WithSourceSanctity
              (
                B => B.RemoveCurse(3.d6()),
                U => U.RemoveCurse(Dice.One),
                C => C.RemoveCurse(Dice.Zero)
              );
            }
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.Unpunish();
          A.RemoveCurse(2.d3() - 2);
        });
      });

      scroll_of_replication = AddScroll("scroll of replication", I =>
      {
        I.Description = "This powerful magic can be used to create copies of any item.";
        I.SetAppearance("scroll labelled ZELGO MER", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Irreplicable = true;
        I.Series = ScrollSeries;
        I.Rarity = 5;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence4;
        I.Price = Gold.FromCoins(300);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().FilterAnyItem()
             .SetTerminates();
          Use.Apply.ExceptSourceNonReplica // prevent replica scrolls of replications from having any effect.
          (
            R => R.WhenConfused
            (
              T => T.CreateEntity(6.d6(), Entities.rabbit, Entities.rabid_rabbit),
              F => F.ReplicateAsset() // TODO: BUC effects?
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.CloneTargetCharacter(Dice.One);
        });
      });

      scroll_of_summoning = AddScroll("scroll of summoning", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled LEP GEX VEN ZEA", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 45;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence2;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(ScrollWeakness);
        I.AddSummonEnemyUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T => T.CreateEntity(Dice.Fixed(8), Entities.acid_blob),
            F => F.WithSourceSanctity
            (
              B => B.CreateEntity(Dice.One),
              U => U.CreateEntity(1.d4() + 1),
              C => C.CreateEntity(1.d4() + 12)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.CreateAsset(Dice.One, bag_of_tricks);
        });
      });

      scroll_of_terror = AddScroll("scroll of terror", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled XIXAXA XOXAXA XUXAXA", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 35;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence2;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(ScrollWeakness);
        I.AddPropertyAreaUse(Motions.read, Properties.fear, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.psychic, Dice.Zero)
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T => T.AreaTransient(Properties.rage, 4.d6(), Kinds.Living.ToArray()),
            F => F.WithSourceSanctity
            (
              B => B.AreaTransient(Properties.fear, 6.d6()),
              U => U.AreaTransient(Properties.fear, 4.d6(), Kinds.Living.ToArray()),
              C => C.ApplyTransient(Properties.fear, 4.d6())
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.ApplyTransient(Properties.fear, 5.d6());
        });
      });

      scroll_of_taming = AddScroll("scroll of taming", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled PRIRUTSENIE", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 15;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence3;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.psychic, Dice.One)
             .SetAudibility(10);
          Use.Apply.WhenConfused
          (
            T => T.ApplyTransient(Properties.paralysis, 5.d100()),
            F => F.WithSourceSanctity
            (
              B => B.Charm(Elements.magical, Delay.FromTurns(40000)),
              U => U.Charm(Elements.magical, Delay.FromTurns(30000), Kinds.Living.ToArray()),
              C => C.Charm(Elements.magical, Delay.FromTurns(20000), Kinds.Undead.ToArray())
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.SummonEntity(Dice.One, Entities.gelatinous_cube);
        });
      });

      scroll_of_teleportation = AddScroll("scroll of teleportation", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled VENZAR BORGAVVE", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 55;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence2;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(ScrollWeakness);
        I.AddTeleportUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.magic, Dice.Zero);
          Use.Apply.WhenConfused
          (
            T =>
            {
              T.TransitionRandom(Properties.teleportation, Dice.One); // -1, +1
              T.TeleportCharacter(Properties.teleportation);
            },
            E => E.WithSourceSanctity
            (
              B =>
              {
                B.ApplyTransient(Properties.teleportation, 10.d100());
                B.ApplyTransient(Properties.teleport_control, 10.d100());
              },
              U => U.TeleportCharacter(Properties.teleportation),
              C =>
              {
                C.TransitionRandom(Properties.teleportation, Dice.One);
                C.TeleportInventoryAsset(); // good bye equipment!
              }
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.MajorProperty(Properties.teleportation);
        });
      });

      scroll_of_training = AddScroll("scroll of training", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled READ ME", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 10;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence4;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use =>
        {
          Use.Consume();
          Use.Apply.WhenConfused
          (
            T => T.ApplyTransient(Properties.fumbling, 10.d100()),
            F => F.WithSourceSanctity
            (
              B => B.GainSkill(RandomPoints: true),
              U => U.GainSkill(RandomPoints: false),
              C => C.LoseSkill(RandomPoints: false)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.CreateAsset(Dice.One, bag_of_tricks);
        });
      });

      scroll_of_tranquillity = AddScroll("scroll of tranquillity", I =>
      {
        I.Description = null;
        I.SetAppearance("scroll labelled XALICH UDED", null);
        I.Glyph = Glyphs.scroll_of_labelled_paper;
        I.Sonic = Sonics.scroll;
        I.Series = ScrollSeries;
        I.Rarity = 15;
        I.Size = ScrollSize;
        I.Weight = ScrollWeight;
        I.Material = Materials.paper;
        I.Essence = ScrollEssence2;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(ScrollWeakness);
        I.AddObviousUse(Motions.read, Delay.FromTurns(15), Sonics.read, Use => // TODO: pacify purpose.
        {
          Use.Consume();
          Use.SetCast().Strike(Strikes.psychic, Dice.Zero)
             .SetTerminates();
          Use.Apply.WhenConfused
          (
            T => T.ApplyTransient(Properties.conflict, 4.d200()),
            F => F.WithSourceSanctity
            (
              B =>
              {
                B.Pacify(Elements.magical);
                B.ApplyTransient(Properties.deflection, 4.d60());
                B.RemoveTransient(Properties.fear, Properties.aggravation, Properties.rage);
              },
              U =>
              {
                U.Pacify(Elements.magical);
                U.RemoveTransient(Properties.fear, Properties.aggravation, Properties.rage);
              },
              C =>
              {
                C.ApplyTransient(Properties.aggravation, 10.d6());
                C.ApplyTransient(Properties.fear, 10.d6());
                C.Light(false);
              }
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.scroll, A =>
        {
          A.RemoveTransient(Properties.fear, Properties.rage, Properties.slowness, Properties.quickness, Properties.confusion, Properties.stunned);
        });
      });

      CodexRecruiter.Enrol(() =>
      {
        foreach (var Scroll in Stocks.scroll.Items.Where(B => B != scroll_of_blank_paper && !B.Artifact))
          Register.Edit(Scroll).SetDowngradeItem(scroll_of_blank_paper);
      });
      #endregion

      #region tool.
      var ToolEssence0 = Essence.FromUnits(5);
      var ToolEssence1 = Essence.FromUnits(10);
      var ToolEssence2 = Essence.FromUnits(25);
      var ToolEssence3 = Essence.FromUnits(40);
      var ToolEssence4 = Essence.FromUnits(50);
      var ToolEssence5 = Essence.FromUnits(100);
      var ContainerWeakness = new[] { Elements.force };
      var LightWeakness = new[] { Elements.water };

      Item AddBagItem(string Name, Action<ItemEditor> EditorAction)
      {
        return AddItem(Stocks.tool, ItemType.Bag, Name, I =>
        {
          I.SetAppearance("bag", null, Glyphs.bag, Gold.FromCoins(50));

          EditorAction(I);
        });
      }

      sack = AddBagItem("sack", I =>
      {
        I.Description = "A spacious cloth bag.";
        I.Glyph = Glyphs.sack;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 40;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(150);
        I.Material = Materials.cloth;
        I.Essence = ToolEssence0;
        I.Price = Gold.FromCoins(2);

        var Storage = I.SetStorage();
        Storage.Locking = false;
        Storage.Preservation = false;
        Storage.Compression = 1.0F;
        Storage.ContainedDice = Dice.Zero;
        Storage.LockSonic = null;
        Storage.BreakSonic = null;
        Storage.TrappedExplosion = null;

        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(20), Sonics.tool);
      });

      porter = AddBagItem("porter", I =>
      {
        I.Description = "A spacious bag made of incredibly stretchy cotton.";
        I.Glyph = Glyphs.porter;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 20;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(150);
        I.Material = Materials.cloth;
        I.Essence = ToolEssence5;
        I.Price = Gold.FromCoins(200);
        I.Confinement = true;

        void Capture(ApplyEditor Apply)
        {
          Apply.WithSourceSanctity
          (
            B => B.Capture(Polite: true, Expansive: true),
            U => U.Capture(Polite: false, Expansive: true),
            C => C.Capture(Polite: false, Expansive: false)
          );
        }

        I.AddDiscoverUse(Motions.capture, Delay.FromTurns(20), Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.magic, Dice.One)
             .SetTargetSelf(false);
          Capture(Use.Apply);
        });
        I.AddDiscoverUse(Motions.release, Delay.FromTurns(20), Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.magic, Dice.One)
             .SetTargetSelf(false);
          Use.Apply.Release();
        });
        I.AddPretendUse(Motions.open, Delay.FromTurns(10), Sonics.magic, Use =>
        {
          Capture(Use.Apply);
        });
        I.AddPretendUse(Motions.empty, Delay.FromTurns(10), Sonics.magic, Use =>
        {
          Use.Apply.Release();
        });
        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(20), Sonics.tool, A =>
        {
          A.MajorProperty(Properties.slippery);
        });
      });

      bag_of_holding = AddBagItem("bag of holding", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.bag_of_holding;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 20;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(150);
        I.Material = Materials.cloth;
        I.Essence = Essence.FromUnits(50);
        I.Essence = ToolEssence5;
        I.Price = Gold.FromCoins(100);
        I.AddObviousIngestUse(Motions.eat, 1500, Delay.FromTurns(50), Sonics.tool); // TODO: what?

        var Storage = I.SetStorage();
        Storage.Locking = false;
        Storage.Preservation = false;
        Storage.ContainedDice = Dice.Zero;
        Storage.Compression = 0.5F;
        Storage.TrappedExplosion = null;
        Storage.LockSonic = null;
        Storage.BreakSonic = null;
      });

      bag_of_tricks = AddBagItem("bag of tricks", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.bag_of_tricks;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 20;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(150);
        I.Material = Materials.cloth;
        I.Essence = ToolEssence5;
        I.Price = Gold.FromCoins(100);
        I.ChargesDice = Dice.Fixed(+20);
        I.AddObviousUse(Motions.open, Delay.FromTurns(10), Sonics.magic, Use =>
        {
          Use.Apply.TeleportCharacter(Properties.teleportation);
          Use.Apply.CreateEntity(1.d3() + 3);
        });
        I.AddObviousUse(Motions.empty, Delay.FromTurns(10), Sonics.magic, Use =>
        {
          Use.Consume();
          Use.Apply.TeleportCharacter(Properties.teleportation);
          Use.Apply.CreateHorde(1.d3() + 3);
        });
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(20), Sonics.tool);
      });

      beartrap = AddItem(Stocks.tool, ItemType.Device, "beartrap", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.beartrap;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(2000);
        I.Material = Materials.iron;
        I.Essence = ToolEssence2;
        I.Price = Gold.FromCoins(60);
        I.AddObviousUse(Motions.set, Delay.FromTurns(10), Sonics.tool, Use =>
        {
          Use.SetCast().Plain(Dice.One);
          Use.Consume();
          Use.Apply.WhenConfused
          (
            T => T.Backfire(B => B.CreateTrap(Codex.Devices.bear_trap, Destruction: false)),
            F => F.CreateTrap(Codex.Devices.bear_trap, Destruction: false)
          );
        });
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(40), Sonics.tool);
      });

      caltrops = AddItem(Stocks.tool, ItemType.Device, "caltrops", I =>
      {
        I.Description = "Tiny metal spikes thrown to the ground to impede mobility.";
        I.Glyph = Glyphs.caltrops;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.iron;
        I.Essence = ToolEssence2;
        I.Price = Gold.FromCoins(30);
        I.AddObviousUse(Motions.set, Delay.FromTurns(10), Sonics.tool, Use =>
        {
          Use.SetCast().Plain(Dice.One);
          Use.Consume();
          Use.Apply.WhenConfused
          (
            T => T.Backfire(B => B.CreateTrap(Codex.Devices.caltrops, Destruction: false)),
            F => F.CreateTrap(Codex.Devices.caltrops, Destruction: false)
          );
        });
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(20), Sonics.tool);
      });

      can_of_grease = AddItem(Stocks.tool, ItemType.Tool, "can of grease", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.can_of_grease;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 5;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.iron;
        I.Essence = ToolEssence2;
        I.ChargesDice = 5.d5();
        I.Price = Gold.FromCoins(20);
        I.AddObviousUse(Motions.rub, Delay.FromTurns(15), Sonics.tool, Use =>
        {
          Use.SetCast().Plain(Dice.One);
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.slippery, 5.d100()),
            U => U.ApplyTransient(Properties.slippery, 5.d50()),
            C => C.ApplyTransient(Properties.fumbling, 5.d100())
          );
        });
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(20), Sonics.tool, A =>
        {
          A.WithSourceSanctity
          (
            B => B.GainTalent(Properties.slippery),
            U => U.MajorProperty(Properties.slippery),
            C => C.MajorProperty(Properties.fumbling)
          );
        });
      });

      Item AddBellItem(string Name, Action<ItemEditor> EditorAction)
      {
        return AddInstrument(Name, I =>
        {
          I.Weight = Weight.FromUnits(300);
          I.SetAppearance("bell", null, Glyphs.bell, Gold.FromCoins(50));

          EditorAction(I);
        });
      }

      bronze_bell = AddBellItem("bronze bell", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.bronze_bell;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Material = Materials.copper;
        I.Essence = ToolEssence0;
        I.Price = Gold.FromCoins(10);
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.bell, Use =>
        {
          Use.Apply.Alert(Dice.Fixed(10));
        });
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(10), Sonics.bell);
      });

      bell_of_secrets = AddBellItem("bell of secrets", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.bell_of_secrets;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Material = Materials.silver;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(200);
        I.ChargesDice = Dice.Fixed(+3);
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.bell, Use =>
        {
          Use.Apply.Alert(Dice.Fixed(10));
          Use.Apply.WhenConfused
          (
            T => T.Concealing(Range.Sq10),
            F => F.WithSourceSanctity
            (
              B => B.Searching(Range.Sq15),
              U => U.Searching(Range.Sq10),
              C => C.Searching(Range.Sq5)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(10), Sonics.bell, A =>
        {
          A.MajorProperty(Properties.searching);
        });
      });

      bell_of_resources = AddBellItem("bell of resources", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.bell_of_resources;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Material = Materials.silver;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(300);
        I.ChargesDice = Dice.Fixed(+3);
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.bell, Use =>
        {
          Use.Apply.Alert(Dice.Fixed(10));
          Use.Apply.WhenConfused
          (
            T => T.Repel(Range.Sq10, Items: true, Characters: true, Boulders: true), // opposite.
            F => F.WithSourceSanctity
            (
              B => B.Gather(Range.Sq15, Items: true, Characters: false, Boulders: false),
              U => U.Gather(Range.Sq10, Items: true, Characters: false, Boulders: true),
              C => C.Gather(Range.Sq15, Items: false, Characters: true, Boulders: false)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(10), Sonics.bell, A =>
        {
          A.MajorProperty(Properties.appraisal);
        });
      });

      bell_of_harmony = AddBellItem("bell of harmony", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.bell_of_harmony;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Material = Materials.silver;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(250);
        I.ChargesDice = Dice.Fixed(+3);
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.bell, Use =>
        {
          Use.Apply.Alert(Dice.Fixed(10));
          Use.Apply.WhenConfused
          (
            T => T.AreaTransient(Properties.fear, 4.d60(), Kinds.Living.ToArray()),
            F => F.WithSourceSanctity
            (
              B =>
              {
                B.Pacify(Elements.magical);
                B.ApplyTransient(Properties.deflection, 4.d60());
                B.RemoveTransient(Properties.fear, Properties.aggravation, Properties.rage);
              },
              U =>
              {
                U.Pacify(Elements.magical);
                U.RemoveTransient(Properties.fear, Properties.aggravation, Properties.rage);
              },
              C =>
              {
                C.ApplyTransient(Properties.aggravation, 10.d6());
                C.ApplyTransient(Properties.fear, 10.d6());
                C.Light(false);
              }
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(10), Sonics.bell, A =>
        {
          A.GainSkill(RandomPoints: false, Skills.music);
        });
      });

      bell_of_strife = AddBellItem("bell of strife", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.bell_of_strife;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Material = Materials.silver;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(150);
        I.ChargesDice = Dice.Fixed(+3);
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.bell, Use =>
        {
          Use.Apply.Alert(Dice.Fixed(10));
          Use.Apply.WhenConfused
          (
            T => T.AreaTransient(Properties.quickness, 4.d60(), Kinds.Living.ToArray()),
            F => F.WithSourceSanctity
            (
              B =>
              {
                B.Murder(MurderType.Hostile, Strikes.death, Kinds.Living.ToArray());
              },
              U =>
              {
                U.Murder(MurderType.Every, Strikes.death, Kinds.Living.ToArray());
              },
              C =>
              {
                C.Murder(MurderType.Allied, Strikes.death, Kinds.Living.ToArray());
                C.AnimateRevenants(Corrupt: true);
              }
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(10), Sonics.bell, A =>
        {
          A.MajorProperty(Properties.rage);
        });
      });

      blindfold = AddItem(Stocks.tool, ItemType.Eyewear, "blindfold", I =>
      {
        I.Description = "A cloth mask that completely obstructs the wearer's vision.";
        I.Glyph = Glyphs.blindfold;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 50;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(20);
        I.Material = Materials.cloth;
        I.Essence = ToolEssence0;
        I.Price = Gold.FromCoins(20);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.tool)
         .SetTalent(Properties.blindness);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(10), Sonics.tool);
      });

      earmuffs = AddItem(Stocks.tool, ItemType.Earwear, "earmuffs", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.earmuffs;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 5;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(40);
        I.Material = Materials.cloth;
        I.Essence = ToolEssence0;
        I.Price = Gold.FromCoins(40);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.tool)
         .SetTalent(Properties.deafness);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(10), Sonics.tool);
      });

      brass_bugle = AddInstrument("brass bugle", I =>
      {
        I.Description = "A hand-held brass horn.";
        I.SetAppearance("bugle", null);
        I.Glyph = Glyphs.brass_bugle;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.copper;
        I.Essence = ToolEssence0;
        I.Price = Gold.FromCoins(15);
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.bugle, Use =>
        {
          Use.Apply.Alert(Dice.Fixed(10));
        });
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(20), Sonics.tool);
      });

      chest = AddItem(Stocks.tool, ItemType.Chest, "chest", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.chest_closed;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(3000);
        I.Material = Materials.wood;
        I.Essence = ToolEssence1;
        I.Price = Gold.FromCoins(16);
        I.SetWeakness(ContainerWeakness);
        
        var Storage = I.SetStorage();
        Storage.Locking = true;
        Storage.Preservation = false;
        Storage.Compression = 1.0F;
        Storage.ContainedDice = 1.d5();
        Storage.TrappedGlyph = Glyphs.chest_trapped;
        Storage.LockedGlyph = Glyphs.chest_locked;
        Storage.LockSonic = Sonics.locked;
        Storage.BrokenGlyph = Glyphs.chest_broken;
        Storage.BreakSonic = Sonics.broken_lock;
        Storage.EmptyGlyph = Glyphs.chest_empty;
        Storage.TrappedExplosion = Explosions.fiery;
        Storage.TrappedElement = Elements.physical;
        Storage.TrappedProperty = Properties.stunned;

        I.AddObviousIngestUse(Motions.eat, 600, Delay.FromTurns(60), Sonics.tool);
      });

      crystal_ball = AddItem(Stocks.tool, ItemType.Tool, "crystal ball", I =>
      {
        I.Description = null;
        I.SetAppearance("glass orb", null);
        I.Glyph = Glyphs.crystal_ball;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(1500);
        I.Material = Materials.crystal;
        I.Essence = ToolEssence5;
        I.Price = Gold.FromCoins(60);
        I.ChargesDice = 1.d5();
        I.AddObviousUse(Motions.scry, Delay.FromTurns(10), Sonics.magic, Use =>
        {
          Use.Apply.WithSourceSanctity
          (
            B => B.DetectAsset(Range.Sq15),
            U => U.DetectAsset(Range.Sq10),
            C => C.DetectAsset(Range.Sq5)
          );
        });
        I.AddObviousIngestUse(Motions.eat, 500, Delay.FromTurns(30), Sonics.tool, A =>
        {
          A.GainTalent(Properties.searching);
        });
      });

      drum_of_earthquake = AddInstrument("drum of earthquake", I =>
      {
        I.Description = null;
        I.SetAppearance("drum", null, Glyphs.leather_drum);
        I.Glyph = Glyphs.drum_of_earthquake;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(250);
        I.Material = Materials.leather;
        I.Essence = ToolEssence5;
        I.Price = Gold.FromCoins(25);
        I.ChargesDice = 1.d5() + 4;
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.drum, Use =>
        {
          Use.SetCast().Explosion(Explosions.light, Dice.Zero);
          Use.Apply.Alert(Dice.Fixed(10));
          Use.Apply.WhenConfused
          (
            T => T.Harm(Elements.force, 4.d6()),
            F =>
            {
              F.CreateTrap(Codex.Devices.pit, Destruction: true);
              F.WithSourceSanctity
              (
                B => B.Shout(A => A.ApplyTransient(Properties.stunned, 1.d5() + 5)),
                U => U.Shout(A => A.ApplyTransient(Properties.deafness, 1.d10() + 10)),
                C => C.ApplyTransient(Properties.deafness, 2.d10() + 10) // deafen yourself
              );
            }
          );
        });
        I.AddObviousIngestUse(Motions.eat, 250, Delay.FromTurns(20), Sonics.tool);
      });

      expensive_camera = AddItem(Stocks.tool, ItemType.Tool, "expensive camera", I =>
      {
        I.Description = "A foreign device which creates perfect visual depictions of reality.";
        I.SetAppearance("mysterious gadget", null);
        I.Glyph = Glyphs.expensive_camera;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(120);
        I.Material = Materials.plastic;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(200);
        I.AddObviousUse(Motions.flash, new Utility(Purpose.Blast, Property: Properties.blindness), Delay.FromTurns(10), Sonics.flash, Use =>
        {
          Use.SetCast().Strike(Strikes.flash, Dice.Fixed(+6))
             .SetAudibility(5);
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.blindness, 4.d6() + 4),
            U => U.ApplyTransient(Properties.blindness, 3.d6() + 3),
            C => C.Backfire(B => B.ApplyTransient(Properties.blindness, 2.d6() + 2))
          );
          Use.Apply.WhenTargetKind(new[] { Kinds.gremlin }, T => T.Harm(Elements.physical, 8.d6() + 8));
        });
        //I.AddEat(200, Delay.FromTurns(20), Sonics.tool); // NOTE: no diet can eat plastic yet.
      });

      fly_swatter = AddItem(Stocks.tool, ItemType.Tool, "fly swatter", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.fly_swatter;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(120);
        I.Material = Materials.plastic;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(2);
        I.AddObviousUse(Motions.swat, Delay.FromTurns(10), Sonics.force, Use =>
        {
          Use.SetCast().Strike(Strikes.force, Dice.One)
             .SetAudibility(5);
          Use.Apply.Harm(Elements.physical, Dice.Zero);
          Use.Apply.WhenTargetKind(new[] { Kinds.insect, Kinds.spider, Kinds.xan }, T =>
          {
            T.ApplyTransient(Properties.stunned, 3.d6() + 3);
            T.Harm(Elements.physical, 8.d6() + 8);
          });
        });
        //I.AddEat(100, Delay.FromTurns(20), Sonics.tool); // NOTE: no diet can eat plastic yet.
      });

      wooden_stake = AddItem(Stocks.tool, ItemType.Tool, "wooden stake", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.wooden_stake;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(120);
        I.Material = Materials.wood;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(2);
        I.AddObviousUse(Motions.stake, Delay.FromTurns(10), Sonics.force, Use =>
        {
          Use.SetCast().Strike(Strikes.force, Dice.One)
             .SetAudibility(1);
          Use.Apply.Harm(Elements.physical, 1.d4());
          Use.Apply.WhenTargetKind(new[] { Kinds.vampire }, T =>
          {
            T.ApplyTransient(Properties.slowness, 3.d6() + 3);
            T.Harm(Elements.physical, 8.d6() + 8);
          });
        });
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(20), Sonics.tool);
      });

      magic_figurine = AddItem(Stocks.tool, ItemType.Tool, "magic figurine", I =>
      {
        I.Description = null;
        I.SetAppearance("figurine", null);
        I.Glyph = Glyphs.figurine;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 5;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(120);
        I.Material = Materials.clay;
        I.Essence = ToolEssence5;
        I.Price = Gold.FromCoins(300);
        I.AddDiscoverUse(Motions.construct, Delay.FromTurns(10), Sonics.tool, Use =>
        {
          Use.Consume();
          Use.SetCast().FilterItem(Codex.Recipes.List.SelectMany(R => R.Items).ToArray())
              .SetAssetIndividualised() 
              .SetAudibility(5);
          Use.Apply.CreateGolem();
        });
        I.AddObviousIngestUse(Motions.eat, 120, Delay.FromTurns(20), Sonics.tool, A =>
        {
          A.WhenProbability(Table =>
          {
            Table.Add(5, T => T.Harm(Elements.magical, 1.d50()));
            Table.Add(5, T => T.ApplyTransient(Properties.petrifying, 3.d100()));
            Table.Add(2, T => T.ConsumeResistance(Elements.petrify));
            Table.Add(1, T => T.Rumour(true, true));
            Table.Add(1, T => T.ApplyTransient(Properties.conflict, 3.d20()));
            Table.Add(1, T => T.ApplyTransient(Properties.slow_digestion, 6.d100()));
            Table.Add(1, T => T.ApplyTransient(Properties.warning, 3.d100()));
            Table.Add(1, T => T.ApplyTransient(Properties.aggravation, 3.d100()));
            Table.Add(1, T => T.ApplyTransient(Properties.fear, 3.d20()));
            Table.Add(1, T => T.IncreaseOneAbility(Dice.One));
            Table.Add(1, T => T.DecreaseOneAbility(Dice.One));
          });
        });
      });

      fire_horn = AddInstrument("fire horn", I =>
      {
        I.Description = null;
        I.SetAppearance("horn", null, Glyphs.tooled_horn);
        I.Glyph = Glyphs.fire_horn;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(180);
        I.Material = Materials.bone;
        I.Essence = ToolEssence4;
        I.Price = Gold.FromCoins(50);
        I.ChargesDice = 1.d5() + 4;
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.horn, Use =>
        {
          Use.SetCast().Beam(Beams.fire, 1.d4() + 4);
          Use.Apply.WhenConfused
          (
            T => T.ConvertFloor(FromGround: null, ToGround: Codex.Grounds.lava, Locality.Square),
            F => F.WithSourceSanctity
            (
              B => B.Harm(Elements.fire, 7.d6()),
              U => U.Harm(Elements.fire, 6.d6()),
              C => C.Harm(Elements.fire, 5.d6())
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 180, Delay.FromTurns(20), Sonics.tool, A =>
        {
          A.Harm(Elements.fire, 7.d6() + 7);
          A.ConsumeResistance(Elements.fire);
        });
      });

      frost_horn = AddInstrument("frost horn", I =>
      {
        I.Description = null;
        I.SetAppearance("horn", null, Glyphs.tooled_horn);
        I.Glyph = Glyphs.frost_horn;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(180);
        I.Material = Materials.bone;
        I.Essence = ToolEssence4;
        I.Price = Gold.FromCoins(50);
        I.ChargesDice = 1.d5() + 4;
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.horn, Use =>
        {
          Use.SetCast().Beam(Beams.cold, 1.d4() + 4);
          Use.Apply.WhenConfused
          (
            T => T.ConvertFloor(FromGround: null, ToGround: Codex.Grounds.ice, Locality.Square),
            F => F.WithSourceSanctity
            (
              B => B.Harm(Elements.cold, 5.d4() + 5),
              U => U.Harm(Elements.cold, 4.d4() + 4),
              C => C.Harm(Elements.cold, 3.d4() + 3)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 180, Delay.FromTurns(20), Sonics.tool, A =>
        {
          A.Harm(Elements.cold, 7.d6() + 7);
          A.ConsumeResistance(Elements.cold);
        });
      });

      heavy_iron_ball = AddItem(Stocks.tool, ItemType.Tool, "heavy iron ball", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.heavy_iron_ball;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(4800);
        I.Material = Materials.iron;
        I.Essence = ToolEssence2;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 800, Delay.FromTurns(20), Sonics.tool);
      });

      horn_of_plenty = AddInstrument("horn of plenty", I =>
      {
        I.Description = null;
        I.SetAppearance("horn", null, Glyphs.tooled_horn);
        I.Glyph = Glyphs.horn_of_plenty;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(180);
        I.Material = Materials.bone;
        I.Essence = ToolEssence5;
        I.Price = Gold.FromCoins(50);
        I.ChargesDice = Dice.Fixed(+20);
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.horn, Use =>
        {
          Use.Apply.WhenConfused
          (
            T => T.DestroyCarriedAsset(1.d2(), new[] { Stocks.food }, null, null),
            F => F.WithSourceSanctity
            (
              B =>
              {
                B.CreateAsset(1.d2(), Stocks.food);
                B.WhenChance(Chance.OneIn9, A => A.CreateAsset(Dice.One, Stocks.potion));
              },
              U =>
              {
                U.CreateAsset(Dice.One, Stocks.food);
                U.WhenChance(Chance.OneIn13, A => A.CreateAsset(Dice.One, Stocks.potion));
              },
              C =>
              {
                C.CreateAsset(1.d2() - 1, Stocks.food);
                C.WhenChance(Chance.OneIn17, A => A.CreateAsset(Dice.One, Stocks.potion));
              }
            )
          ); ;
        });
        I.AddObviousIngestUse(Motions.eat, 1000, Delay.FromTurns(20), Sonics.tool);
      });

      ice_box = AddItem(Stocks.tool, ItemType.Chest, "ice box", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.ice_box;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(2000);
        I.Material = Materials.plastic;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(42);
        I.SetWeakness(ContainerWeakness);

        var Storage = I.SetStorage();
        Storage.Locking = false;
        Storage.Preservation = true;
        Storage.Compression = 1.0F;
        Storage.ContainedDice = 1.d4();
        Storage.LockSonic = null;
        Storage.BreakSonic = null;
        Storage.TrappedExplosion = null;

        //I.AddEat(900, Delay.FromTurns(50), Sonics.tool); // nothing can eat plastic.
      });

      land_mine = AddItem(Stocks.tool, ItemType.Device, "land mine", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.land_mine;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(3000);
        I.Material = Materials.iron;
        I.Essence = ToolEssence2;
        I.Price = Gold.FromCoins(180);
        I.AddObviousUse(Motions.set, Delay.FromTurns(10), Sonics.tool, Use =>
        {
          Use.SetCast().Plain(Dice.One);
          Use.Consume();
          Use.Apply.WhenConfused
          (
            T => T.Backfire(B => B.CreateTrap(Codex.Devices.explosive_trap, Destruction: false)),
            F => F.CreateTrap(Codex.Devices.explosive_trap, Destruction: false)
          );
        });
        I.AddObviousIngestUse(Motions.eat, 300, Delay.FromTurns(30), Sonics.tool);
      });

      lantern = AddLight("lantern", I =>
      {
        I.Description = null; // "A portable copper enclosure with a candle inside.";
        I.Glyph = Glyphs.lantern;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 15;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.copper;
        I.Essence = ToolEssence1;
        I.Price = Gold.FromCoins(12);
        I.SetWeakness(LightWeakness);
        I.ChargesDice = 1.d500() + 1000;
        I.SetEquip(EquipAction.Employ, Delay.FromTurns(10), Sonics.tool);
        I.SetIllumination(3);
        I.AddObviousIngestUse(Motions.eat, 300, Delay.FromTurns(20), Sonics.tool);
      });

      large_box = AddItem(Stocks.tool, ItemType.Chest, "large box", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.large_box_closed;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(3500);
        I.Material = Materials.wood;
        I.Essence = ToolEssence1;
        I.Price = Gold.FromCoins(8);
        I.SetWeakness(ContainerWeakness);

        var Storage = I.SetStorage();
        Storage.Locking = true;
        Storage.Preservation = false;
        Storage.Compression = 1.0F;
        Storage.ContainedDice = 1.d3();
        Storage.TrappedGlyph = Glyphs.large_box_trapped;
        Storage.LockedGlyph = Glyphs.large_box_locked;
        Storage.LockSonic = Sonics.locked;
        Storage.BrokenGlyph = Glyphs.large_box_broken;
        Storage.BreakSonic = Sonics.broken_lock;
        Storage.EmptyGlyph = Glyphs.large_box_empty;
        Storage.TrappedExplosion = Explosions.fiery;
        Storage.TrappedElement = Elements.physical;
        Storage.TrappedProperty = Properties.stunned;

        I.AddObviousIngestUse(Motions.eat, 350, Delay.FromTurns(30), Sonics.tool);
      });

      leather_drum = AddInstrument("leather drum", I =>
      {
        I.Description = null;
        I.SetAppearance("drum", null);
        I.Glyph = Glyphs.leather_drum;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(250);
        I.Material = Materials.leather;
        I.Essence = ToolEssence0;
        I.Price = Gold.FromCoins(25);
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.drum, Use =>
        {
          Use.Apply.Alert(Dice.Fixed(20));
        });
        I.AddObviousIngestUse(Motions.eat, 250, Delay.FromTurns(20), Sonics.tool);
      });

      var EyeglassImpactSonic = Sonics.broken_glass;
      var EyeglassWeakness = new[] { Elements.physical, Elements.force };

      lenses = AddItem(Stocks.tool, ItemType.Eyewear, "lenses", I =>
      {
        I.Description = null;
        I.SetAppearance("glasses", null, Price: Gold.FromCoins(80));
        I.Glyph = Glyphs.lenses;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 5;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.glass;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(160);
        I.SetImpact(EyeglassImpactSonic);
        I.SetWeakness(EyeglassWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.tool)
         .SetTalent(Properties.searching);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(10), Sonics.tool, A =>
        {
          A.MajorProperty(Properties.searching);
        });
      });

      shades = AddItem(Stocks.tool, ItemType.Eyewear, "shades", I =>
      {
        I.Description = null;
        I.SetAppearance("glasses", null, Price: Gold.FromCoins(80));
        I.Glyph = Glyphs.lenses;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 5;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.glass;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(160);
        I.SetImpact(EyeglassImpactSonic);
        I.SetWeakness(EyeglassWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.tool)
         .SetBoostAttribute(Attributes.charisma);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(10), Sonics.tool, A =>
        {
          A.WhenChance(Chance.OneIn2, T => T.IncreaseAbility(Attributes.charisma, Dice.Fixed(2)), E => E.IncreaseAbility(Attributes.charisma, Dice.Fixed(1)));
        });
      });

      kaleidoscope_glasses = AddItem(Stocks.tool, ItemType.Eyewear, "kaleidoscope glasses", I =>
      {
        I.Description = null;
        I.SetAppearance("glasses", null, Price: Gold.FromCoins(80));
        I.Glyph = Glyphs.lenses;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 5;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.glass;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(20);
        I.SetImpact(EyeglassImpactSonic);
        I.SetWeakness(EyeglassWeakness);
        I.DefaultSanctity = Sanctities.Cursed;
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.tool)
         .SetTalent(Properties.hallucination);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(10), Sonics.tool, A =>
        {
          A.WhenChance(Chance.OneIn2, T => T.Polymorph(Entities.black_light), E => E.ApplyTransient(Properties.hallucination, 10.d100()));
        });
      });

      spectacles = AddItem(Stocks.tool, ItemType.Eyewear, "spectacles", I =>
      {
        I.Description = null;
        I.SetAppearance("glasses", null, Price: Gold.FromCoins(80));
        I.Glyph = Glyphs.lenses;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 5;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.glass;
        I.Essence = ToolEssence5;
        I.Price = Gold.FromCoins(350);
        I.SetImpact(EyeglassImpactSonic);
        I.SetWeakness(EyeglassWeakness);
        I.SetEquip(EquipAction.Wear, Delay.FromTurns(10), Sonics.tool)
         .SetTalent(Properties.clarity);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(10), Sonics.tool, A =>
        {
          A.MajorProperty(Properties.clarity);
        });
      });

      lock_pick = AddItem(Stocks.tool, ItemType.Lockpick, "lock pick", I =>
      {
        I.Description = "A small iron tool designed for opening all sorts of locks.";
        I.Glyph = Glyphs.lock_pick;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 60;
        I.Size = Size.Tiny;
        I.BundleDice = 1.d3();
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.iron;
        I.Essence = ToolEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 5, Delay.FromTurns(10), Sonics.tool);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.tool);
      });

      magic_bugle = AddInstrument("magic bugle", I =>
      {
        I.Description = null;
        I.SetAppearance("bugle", null, Glyphs.brass_bugle, Gold.FromCoins(15));
        I.Glyph = Glyphs.magic_bugle;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.copper;
        I.Essence = ToolEssence4;
        I.Price = Gold.FromCoins(35);
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.bugle, Use =>
        {
          Use.Apply.Alert(Dice.Fixed(10));
          Use.Apply.WhenConfused
          (
            T => T.Gather(Range.Sq6, Items: true, Characters: true, Boulders: true), // opposite blast
            F => F.WithSourceSanctity
            (
              B => B.Repel(Range.Sq6, Items: false, Characters: true, Boulders: false),
              U => U.Repel(Range.Sq4, Items: true, Characters: true, Boulders: false),
              C => C.Repel(Range.Sq2, Items: true, Characters: true, Boulders: true)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(20), Sonics.tool);
      });

      magic_flute = AddInstrument("magic flute", I =>
      {
        I.Description = null;
        I.SetAppearance("flute", null, Glyphs.wooden_flute, Price: Gold.FromCoins(12));
        I.Glyph = Glyphs.magic_flute;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.wood;
        I.Essence = ToolEssence4;
        I.Price = Gold.FromCoins(36);
        I.ChargesDice = 1.d5() + 4;
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.flute, Use =>
        {
          Use.Apply.WhenConfused
          (
            T => T.Polymorph(Entities.rat_king), // opposite blast
            F => F.WithSourceSanctity
            (
              B =>
              {
                B.CreateEntity(1.d2(), Kinds.rodent);
                B.AreaTransient(Properties.sleeping, 2.d20(), Kinds.rodent);
              },
              U => U.CreateEntity(1.d4(), Kinds.rodent),
              C => C.CreateEntity(1.d8(), Kinds.rodent)
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(10), Sonics.tool, A =>
        {
          A.Polymorph(Entities.wererat);
        });
      });

      var WoodenHarpPrice = Gold.FromCoins(50);

      wooden_harp = AddInstrument("wooden harp", I =>
      {
        I.Description = null;
        I.SetAppearance("harp", null);
        I.Glyph = Glyphs.wooden_harp;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.wood;
        I.Essence = ToolEssence0;
        I.Price = WoodenHarpPrice;
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.harp, Use =>
        {
          Use.Apply.WhenConfused
          (
            T => T.Alert(Dice.Fixed(10)),
            F =>
            {
              F.Alert(Dice.Fixed(5));
              F.Pacify(Elements.magical, Kinds.fairy);
            }
          );
        });
        I.AddObviousIngestUse(Motions.eat, 300, Delay.FromTurns(20), Sonics.tool);
      });

      magic_harp = AddInstrument("magic harp", I =>
      {
        I.Description = null;
        I.SetAppearance("harp", null, Glyphs.wooden_harp, WoodenHarpPrice);
        I.Glyph = Glyphs.magic_harp;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.wood;
        I.Essence = ToolEssence4;
        I.Price = Gold.FromCoins(150);
        I.ChargesDice = 1.d5() + 4;
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.harp, Use =>
        {
          Use.Apply.WhenConfused
          (
            T => T.AreaTransient(Properties.rage, 10.d10()), // opposite blast
            F => F.WithSourceSanctity
            (
              B => B.Charm(Elements.magical, Delay.FromTurns(30000), Kinds.Living.ToArray()),
              U => U.Charm(Elements.magical, Delay.FromTurns(20000), Kinds.Living.ToArray()),
              C => C.ApplyTransient(Properties.conflict, 10.d10())
            )
          );
        });
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(20), Sonics.tool, A =>
        {
          A.IncreaseAbility(Attributes.charisma, Dice.One);
        });
      });

      magic_lamp = AddLight("magic lamp", I =>
      {
        I.Description = null;
        I.SetAppearance("lamp", null, Glyphs.oil_lamp, Gold.FromCoins(10));
        I.Glyph = Glyphs.magic_lamp;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.copper;
        I.Essence = ToolEssence4;
        I.Price = Gold.FromCoins(1000);
        I.SetEquip(EquipAction.Employ, Delay.FromTurns(10), Sonics.tool);
        I.SetIllumination(2);
        I.ChargesDice = null;
        I.AddObviousUse(Motions.rub, Delay.FromTurns(10), Sonics.magic, Use =>
        {
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.ConvertAsset(magic_lamp.Stock, WholeStack: false, oil_lamp);
              B.SummonEntity(Dice.One, Entities.djinni);
            },
            U =>
            {
              U.Nothing();
            },
            C =>
            {
              C.ConvertAsset(magic_lamp.Stock, WholeStack: false, oil_lamp);
              C.CreateEntity(Dice.One, Entities.djinni);
            }
          );
        });
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(20), Sonics.tool, A =>
        {
          A.Polymorph(Entities.djinni);
        });
      });

      magic_marker = AddItem(Stocks.tool, ItemType.Tool, "magic marker", I =>
      {
        I.Description = "This magical pen has only a limited amount of ink. It seems to vibrate and move on its own accord when you hold it in your hand. You wonder what it would do if you pressed it to some paper.";
        I.SetAppearance("marker", null);
        I.Glyph = Glyphs.magic_marker;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 15;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(20);
        I.Material = Materials.plastic;
        I.Essence = ToolEssence5;
        I.Essence = Essence.FromUnits(100);
        I.Price = Gold.FromCoins(50);
        I.ChargesDice = 1.d3() + 2;
        I.AddDiscoverUse(Motions.write, Delay.FromTurns(50), Sonics.write, Use =>
        {
          Use.SetCast().FilterItem(scroll_of_blank_paper, book_of_blank_paper)
             .SetAssetIndividualised();
          Use.Apply.ConvertAsset(Stocks.scroll, WholeStack: false, Stocks.scroll.Items.Where(S => S != scroll_of_blank_paper && !S.Artifact && S.Rarity > 0).ToArray());
          Use.Apply.ConvertAsset(Stocks.book, WholeStack: false, Stocks.book.Items.Where(S => S != book_of_blank_paper && !S.Artifact && S.Rarity > 0).ToArray());
        });
        I.AddDiscoverUse(Motions.rename, Delay.FromTurns(50), Sonics.write, Use =>
        {
          Use.SetCast().FilterAnyItem()
             .SetAssetIndividualised(false)
             .FilterCoins(false);
          Use.Apply.WithSourceSanctity
          (
            B => B.Sanctify(null, Sanctities.Blessed),
            U => U.Nothing(),
            C => C.Sanctify(null, Sanctities.Cursed)
          );
        });
        // NOTE: inscribe is not particularly useful, except for Studio quests.
        /*
        I.AddDiscoverUse(Motions.Inscribe, Delay.FromTurns(50), Sonics.write, Use =>
        {
          Use.SetCast().AnyItem();
          Use.Cast.AssetIndividualised = false;
          Use.Apply.Exercise(Attributes.Wisdom);
          Use.Apply.WithSourceSanctity
          (
            B => B.Bless(null),
            U => U.Nothing(),
            C => C.Curse(null)
          );
        });
        */
        //I.AddEat(25, Delay.FromTurns(10), Sonics.tool, A =>
        //{
        //  A.GainSkill(Skills.Literacy, RandomPoints: false);
        //});
      });

      magic_whistle = AddInstrument("magic whistle", I =>
      {
        I.Description = null;
        I.SetAppearance("whistle", null, Glyphs.tin_whistle, Gold.FromCoins(10));
        I.Glyph = Glyphs.magic_whistle;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 25;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.tin;
        I.Essence = ToolEssence4;
        I.Price = Gold.FromCoins(100);
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.whistle, Use =>
        {
          Use.Apply.Alert(1.d6() + 1);
          Use.Apply.WhenConfused
          (
            T => T.Alert(Dice.Fixed(10)),
            F =>
            {
              F.Recall();
              /*
              F.WithSourceSanctity
              (
                B => B.Recall(), // TODO: mild area healing?
                U => U.Recall(),
                C => C.Gather(Range.Sq15, Items: false, Characters: true, Boulders: false)
              );
              */
            }
          );
        });
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(10), Sonics.tool, A =>
        {
          A.MajorProperty(Properties.aggravation);
        });
      });

      oil_lamp = AddLight("oil lamp", I =>
      {
        I.Description = "A portable source of light fashioned out of copper and fuelled by oil.";
        I.SetAppearance("lamp", null);
        I.Glyph = Glyphs.oil_lamp;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 25;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.copper;
        I.Essence = ToolEssence1;
        I.Price = Gold.FromCoins(10);
        I.ChargesDice = 1.d500() + 1000;
        I.SetWeakness(LightWeakness);
        I.SetEquip(EquipAction.Employ, Delay.FromTurns(10), Sonics.tool);
        I.SetIllumination(2);
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(20), Sonics.tool);
      });

      pickaxe = AddItem(Stocks.tool, ItemType.MeleeWeapon, "pick-axe", I =>
      {
        I.Description = "A pointed iron bar fashioned at a right-angle to a sturdy wooden handle, used to pierce through hard surfaces.";
        I.Glyph = Glyphs.pickaxe;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 20;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(800);
        I.Material = Materials.iron;
        I.Essence = ToolEssence2;
        I.Price = Gold.FromCoins(50);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon)
         .SetTalent(Properties.tunnelling);
        var W = I.SetOneHandedWeapon(Skills.pick, null, Elements.physical, DamageType.Pierce, 1.d6());
        W.AddVersus(new[] { Materials.stone }, Elements.physical, 2.d6());
        I.AddObviousUse(Motions.dig, Delay.FromTurns(30), Sonics.pick_axe, Use =>
        {
          Use.SetCast().Strike(Strikes.tunnel, Dice.One);
          Use.Apply.Digging(Elements.digging);
        });
        I.AddObviousIngestUse(Motions.eat, 300, Delay.FromTurns(20), Sonics.weapon);
      });

      dwarvish_mattock = AddMeleeWeapon("dwarvish mattock", I =>
      {
        I.Description = "This tool is used for digging and chopping, similar to the pick-axe. It has a long handle, and a stout head, which combines an axe blade and an adze. It can also be used as an effective melee weapon.";
        I.SetAppearance("broad pick", null);
        I.Glyph = Glyphs.dwarvish_mattock;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.dwarf;
        I.Series = null;
        I.Rarity = 13;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1200);
        I.Material = Materials.iron;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(50);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon)
         .SetTalent(Properties.tunnelling);
        var W = I.SetTwoHandedWeapon(Skills.pick, null, Elements.physical, DamageType.Pierce, 1.d12());
        W.AddVersus(new[] { Materials.stone }, Elements.physical, 2.d12());
        I.AddObviousUse(Motions.dig, Delay.FromTurns(30), Sonics.pick_axe, Use =>
        {
          Use.SetCast().Strike(Strikes.tunnel, Dice.One);
          Use.Apply.Digging(Elements.digging);
        });
        I.AddObviousIngestUse(Motions.eat, 600, Delay.FromTurns(40), Sonics.weapon);
      });

      Item AddSkeletonKeyItem(string Name, Action<ItemEditor> EditorAction)
      {
        return AddItem(Stocks.tool, ItemType.SkeletonKey, Name, I =>
        {
          I.Weight = Weight.FromUnits(40);
          I.BundleDice = Dice.One;
          I.Size = Size.Tiny;
          I.SetAppearance("key", null, Glyphs.key, Gold.FromCoins(250));

          EditorAction(I);
        });
      }

      skeleton_key = AddSkeletonKeyItem("skeleton key", I =>
      {
        I.Description = "A key designed to open all mundane locks.";
        I.Glyph = Glyphs.skeleton_key;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 80;
        I.Material = Materials.iron;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(20);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.tool);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.tool, A =>
        {
          A.Polymorph(Entities.skeleton);
        });
      });

      detonation_key = AddSkeletonKeyItem("detonation key", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.detonation_key;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 20;
        I.Material = Materials.iron;
        I.Essence = ToolEssence4;
        I.DefaultSanctity = Sanctities.Cursed;
        I.Price = Gold.FromCoins(30);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.tool);
        I.AddDiscoverUse(Motions.zap, Delay.FromTurns(10), Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.force, Dice.One);
          Use.Apply.Harm(Elements.force, 3.d6());
          Use.Consume();
        });
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.tool, A =>
        {
          A.ApplyTransient(Properties.rage, 5.d10());
        });
      });

      dimension_key = AddSkeletonKeyItem("dimension key", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.dimension_key;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 20;
        I.Material = Materials.iron;
        I.Essence = ToolEssence4;
        I.Price = Gold.FromCoins(20);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.tool);
        I.AddDiscoverUse(Motions.zap, Delay.FromTurns(10), Sonics.magic, Use =>
        {
          Use.SetCast().Plain(Dice.One) // don't need a strike, blink has it's own SFX.
             .SetTargetSelf(false);
          Use.Apply.Blinking();
          Use.Consume();
        });
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.tool, A =>
        {
          A.ApplyTransient(Properties.beatitude, 5.d10());
        });
      });

      phantom_key = AddSkeletonKeyItem("phantom key", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.phantom_key;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 20;
        I.Material = Materials.iron;
        I.Essence = ToolEssence4;
        I.Price = Gold.FromCoins(50);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.tool);
        I.AddDiscoverUse(Motions.zap, Delay.FromTurns(10), Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.boost, Dice.Zero);
          Use.Apply.ApplyTransient(Properties.phasing, Dice.Three); // one turn to cast, one turn to phase, and then can exit the door.
          Use.Consume();
        });
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.tool, A =>
        {
          A.ApplyTransient(Properties.phasing, 5.d10());
        });
      });

      tinning_kit = AddItem(Stocks.tool, ItemType.Tool, "tinning kit", I =>
      {
        I.Description = "A collection of utensils, including empty tins and butchering tools, for packing corpses so as to condense them.";
        I.Glyph = Glyphs.tinning_kit;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.ChargesDice = Dice.Fixed(+20);
        I.Rarity = 75;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(750);
        I.Material = Materials.tin;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(30);
        I.AddObviousUse(Motions.pack, Delay.FromTurns(60), Sonics.scrap, Use =>
        {
          Use.SetCast().FilterItem(animal_corpse, vegetable_corpse);
          Use.Apply.Tinning(tin);
        });
        I.AddObviousIngestUse(Motions.eat, 750, Delay.FromTurns(35), Sonics.eat);
      });

      tin_whistle = AddInstrument("tin whistle", I =>
      {
        I.Description = null;
        I.SetAppearance("whistle", null);
        I.Glyph = Glyphs.tin_whistle;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 63;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.tin;
        I.Essence = ToolEssence0;
        I.Price = Gold.FromCoins(10);
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.whistle, Use =>
        {
          Use.Apply.Alert(2.d6() + 6);
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.tool);
      });

      torch = AddLight("torch", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.torch;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 25;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.wood;
        I.Essence = ToolEssence0;
        I.Price = Gold.FromCoins(5);
        I.ChargesDice = 1.d500() + 500;
        I.SetWeakness(LightWeakness);
        I.SetEquip(EquipAction.Employ, Delay.FromTurns(10), Sonics.tool);
        I.SetIllumination(2);
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(20), Sonics.tool);
      });

      tooled_horn = AddInstrument("tooled horn", I =>
      {
        I.Description = null;
        I.SetAppearance("horn", null);
        I.Glyph = Glyphs.tooled_horn;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(180);
        I.Material = Materials.bone;
        I.Essence = ToolEssence0;
        I.Price = Gold.FromCoins(15);
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.horn, Use =>
        {
          Use.Apply.Alert(Dice.Fixed(10));
        });
        I.AddObviousIngestUse(Motions.eat, 180, Delay.FromTurns(20), Sonics.tool);
      });

      unicorn_horn = AddItem(Stocks.tool, ItemType.MeleeWeapon, "unicorn horn", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.unicorn_horn;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.bone;
        I.Essence = ToolEssence3;
        I.Price = Gold.FromCoins(100);
        I.SetDerivative(Entities.black_unicorn, Entities.grey_unicorn, Entities.white_unicorn);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.tool);
        I.SetTwoHandedWeapon(Skills.spear, null, Elements.physical, DamageType.Pierce, 1.d12());
        I.AddObviousUse(Motions.zap, Delay.FromTurns(10), Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.boost, Dice.One);
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.RemoveTransient(Properties.blindness, Properties.deafness, Properties.hallucination, Properties.sickness, Properties.confusion, Properties.stunned, Properties.petrifying);
            },
            U =>
            {
              U.RemoveTransient(Properties.blindness, Properties.deafness, Properties.hallucination, Properties.confusion);
            },
            C =>
            {
              C.ApplyTransient(Properties.blindness, 1.d6() + 1);
              C.ApplyTransient(Properties.deafness, 1.d6() + 1);
              C.ApplyTransient(Properties.hallucination, 2.d6() + 1);
              C.ApplyTransient(Properties.sickness, 2.d6() + 1);
              C.ApplyTransient(Properties.stunned, 1.d6() + 1);
            }
          );
          Use.Apply.WhenAfflicted(T =>
          {
            T.Unafflict();
            T.DestroySourceAsset(Dice.One);
          });
        });
        I.AddObviousIngestUse(Motions.eat, 250, Delay.FromTurns(20), Sonics.tool, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Polymorph(Entities.white_unicorn),
            U => U.Polymorph(Entities.grey_unicorn),
            C => C.Polymorph(Entities.black_unicorn)
          );
        });
      });

      magic_candle = AddLight("magic candle", I =>
      {
        I.Description = null;
        I.SetAppearance("candle", null, Glyphs.wax_candle, Gold.FromCoins(10));
        I.Glyph = Glyphs.magic_candle;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.wax;
        I.Essence = ToolEssence4;
        I.Price = Gold.FromCoins(100);
        I.ChargesDice = null;
        I.SetIllumination(2);
        I.SetEquip(EquipAction.Employ, Delay.FromTurns(10), Sonics.tool);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(10), Sonics.tool, A =>
        {
          A.MajorProperty(Properties.dark_vision);
        });
      });

      wax_candle = AddLight("wax candle", I =>
      {
        I.Description = null;
        I.SetAppearance("candle", null);
        I.Glyph = Glyphs.wax_candle;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 25;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.wax;
        I.Essence = ToolEssence0;
        I.Price = Gold.FromCoins(10);
        I.ChargesDice = 1.d100() + 100;
        I.SetWeakness(LightWeakness);
        I.SetIllumination(2);
        I.SetEquip(EquipAction.Employ, Delay.FromTurns(10), Sonics.tool);

        // TODO: do we want to refill lanterns with candles?
        //I.AddDiscoverUse(Motions.Refill, Delay.FromTurns(30), Sonics.lit_fuse, Use =>
        //{
        //  Use.SetCast().SelectItem(lantern);
        //  Use.Cast.AssetIndividualised = true;
        //  Use.Consume();
        //  Use.Apply.WithSourceSanctity
        //  (
        //    B => B.Charging(Dice.One, Dice.Fixed(100)), // 100%
        //    U => U.Charging(Dice.One, 1.d20() + 70),    // 71..90%
        //    C => C.Charging(Dice.One, 1.d20() + 30)     // 31..50%
        //  );
        //});
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.tool, A =>
        {
          A.ApplyTransient(Properties.sickness, 4.d6() + 4);
        });
      });

      wooden_flute = AddInstrument("wooden flute", I =>
      {
        I.Description = null;
        I.SetAppearance("flute", null);
        I.Glyph = Glyphs.wooden_flute;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.wood;
        I.Essence = ToolEssence0;
        I.Price = Gold.FromCoins(12);
        I.AddObviousUse(Motions.play, Delay.FromTurns(10), Sonics.flute, Use =>
        {
          Use.Apply.WhenConfused
          (
            T => T.Alert(Dice.Fixed(10)),
            F =>
            {
              F.Alert(Dice.Fixed(5));
              F.Charm(Elements.magical, Delay.FromTurns(30000), Kinds.snake);
            }
          );

          // NOTE: wooden flute is not meant to be strongly magical, so the below powers are probably too much:
          /*
          Use.Apply.WhenConfused
          (
            T => T.Polymorph(Entities.king_cobra),
            F => F.WithSourceSanctity
            (
              B => B.Charm(Elements.magical, Delay.FromTurns(40000), Kinds.snake, Kinds.naga),
              U => U.Charm(Elements.magical, Delay.FromTurns(30000), Kinds.snake),
              C => C.CreateEntity(1.d3(), Kinds.snake)
            )
          );*/
        });
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(10), Sonics.tool);
      });

      SetDowngradeItem(bronze_bell, bell_of_resources, bell_of_secrets, bell_of_harmony, bell_of_strife);
      SetDowngradeItem(leather_drum, drum_of_earthquake);
      SetDowngradeItem(brass_bugle, magic_bugle);
      SetDowngradeItem(wooden_flute, magic_flute);
      SetDowngradeItem(tin_whistle, magic_whistle);
      SetDowngradeItem(wooden_harp, magic_harp);
      SetDowngradeItem(tooled_horn, frost_horn, fire_horn, horn_of_plenty, unicorn_horn);
      SetDowngradeItem(sack, porter, bag_of_holding, bag_of_tricks);
      SetDowngradeItem(oil_lamp, magic_lamp);
      SetDowngradeItem(wax_candle, magic_candle);
      SetUpgradeDowngradePair(lock_pick, skeleton_key);
      SetDowngradeItem(skeleton_key, dimension_key, phantom_key, detonation_key);

      // NOTE: this doesn't really make sense - pickaxe and dwarvish mattock are not naturally related and it prevents enchanting and dual wielding 2 x +5 pickaxe.
      //SetUpgradeDowngradePair(pickaxe, dwarvish_mattock);
      #endregion

      #region wand.
      var WandEssence0 = Essence.FromUnits(10);
      var WandEssence1 = Essence.FromUnits(20);
      var WandEssence2 = Essence.FromUnits(30);
      var WandEssence3 = Essence.FromUnits(40);
      var WandEssence4 = Essence.FromUnits(50);
      var WandEssence5 = Essence.FromUnits(100);
      var WandSize = Size.Small;
      var WandZapDelay = Delay.FromTurns(15);
      var WandSeries = new Series("wand");
      var WandWeakness = new[] { Elements.shock };

      wand_of_animation = AddWand("wand of animation", I =>
      {
        I.Description = null;
        I.SetAppearance("uranium wand", null);
        I.Glyph = Glyphs.uranium_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 20;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.iron;
        I.Essence = WandEssence4;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddBlastUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.magic, 1.d6() + 2)
             .SetObjects()
          .SetAudibility(1);
          Use.Apply.WithSourceSanctity
          (
            B => B.Animate(Corrupt: false),
            U => U.Animate(Corrupt: false),
            C => C.Animate(Corrupt: true)
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.AnimateObjects(Corrupt: false);
          A.AnimateObjects(Corrupt: false);
          A.AnimateObjects(Corrupt: false);
          A.AnimateObjects(Corrupt: false);
          A.AnimateObjects(Corrupt: false);
        });
      });

      wand_of_cancellation = AddWand("wand of cancellation", I =>
      {
        I.Description = null;
        I.SetAppearance("platinum wand", null);
        I.Glyph = Glyphs.platinum_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 15;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.platinum;
        I.Essence = WandEssence4;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddBlastUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.force, 1.d6() + 2)
             .SetObjects()
             .SetAudibility(1);
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.Cancellation(Elements.magical);
              B.Death(Elements.magical, new[] { Kinds.golem, Kinds.elemental }, Strikes.death, DeathSupport.cancellation);
            },
            U => U.Cancellation(Elements.magical),
            C => C.Backfire(F => F.Cancellation(Elements.magical))
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.Cancellation(Elements.magical);
        });
      });

      wand_of_cold = AddWand("wand of cold", I =>
      {
        I.Description = null;
        I.SetAppearance("short wand", null);
        I.Glyph = Glyphs.short_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 30;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.iron;
        I.Essence = WandEssence3;
        I.Price = Gold.FromCoins(175);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddElementBlastUse(Motions.zap, Elements.cold, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Beam(Beams.cold, 1.d4() + 4)
             .SetAudibility(10);
          Use.Apply.WithSourceSanctity
          (
            B => B.Harm(Elements.cold, 7.d6()),
            U => U.Harm(Elements.cold, 6.d6()),
            C => C.Harm(Elements.cold, 5.d6())
          );
          Use.Apply.UnlessTargetResistant(Elements.cold, R => R.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.paralysis, 1.d4() + 1)));
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.Harm(Elements.cold, 14.d6() + 14);
          A.ConsumeResistance(Elements.cold);
        });
      });

      wand_of_create_horde = AddWand("wand of create horde", I =>
      {
        I.Description = null;
        I.SetAppearance("black wand", null);
        I.Glyph = Glyphs.black_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 5;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.wood;
        I.Essence = WandEssence5;
        I.Price = Gold.FromCoins(300);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 11;
        I.AddSummonEnemyUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.holy, Dice.Zero)
             .SetTerminates();
          Use.Apply.WithSourceSanctity
          (
            B => B.CreateHorde(Dice.One),
            U => U.CreateHorde(1.d2()),
            C => C.CreateHorde(1.d2() + 1)
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.MajorProperty(Properties.conflict);
        });
      });

      wand_of_summoning = AddWand("wand of summoning", I =>
      {
        I.Description = null;
        I.SetAppearance("maple wand", null);
        I.Glyph = Glyphs.maple_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 35;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.wood;
        I.Essence = WandEssence1;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 11;
        I.AddSummonEnemyUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.holy, Dice.Zero)
           .SetTerminates();
          Use.Apply.WithSourceSanctity
          (
            B => B.CreateEntity(Dice.One),
            U => U.CreateEntity(1.d2()),
            C => C.CreateEntity(1.d7() + 2)
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.MajorProperty(Properties.aggravation);
        });
      });

      wand_of_death = AddWand("wand of death", I =>
      {
        I.Description = null;
        I.SetAppearance("long wand", null);
        I.Glyph = Glyphs.long_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 5;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.iron;
        I.Essence = WandEssence5;
        I.Price = Gold.FromCoins(500);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddBlastUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Beam(Beams.death, 1.d4() + 4)
             .SetAudibility(10);
          Use.Apply.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
        });
      });

      wand_of_digging = AddWand("wand of digging", I =>
      {
        I.Description = "Zapping it at a wall creates a big tunnel.";
        I.SetAppearance("iron wand", null);
        I.Glyph = Glyphs.iron_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 50;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.iron;
        I.Essence = WandEssence4;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddObviousUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Beam(Beams.digging, 1.d4() + 4)
             .SetAudibility(10)
             .SetPenetrates()
             .SetBounces(false);
          Use.Apply.Digging(Elements.digging);
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.Digging(Elements.digging);
        });
      });

      wand_of_draining = AddWand("wand of draining", I =>
      {
        I.Description = null;
        I.SetAppearance("ceramic wand", null);
        I.Glyph = Glyphs.ceramic_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 25;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.clay;
        I.Essence = WandEssence3;
        I.Price = Gold.FromCoins(175);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddBlastUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.spirit, 2.d6() + 2)
             .SetAudibility(5) 
             .SetPenetrates()
             .SetTerminates();
          Use.Apply.WithSourceSanctity
          (
            B => B.DrainLife(Elements.drain, 3.d8() + 3),
            U => U.DrainLife(Elements.drain, 2.d8() + 2),
            C => C.DrainLife(Elements.drain, 1.d8() + 1)
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Heal(1.d6(), Modifier.Plus1),
            U => U.Heal(1.d6(), Modifier.Zero),
            C => C.Harm(Elements.necrotic, 1.d6())
          );
        });
      });

      wand_of_extra_healing = AddWand("wand of extra healing", I =>
      {
        I.Description = null;
        I.SetAppearance("bronze wand", null);
        I.Glyph = Glyphs.bronze_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 30;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.copper;
        I.Essence = WandEssence4;
        I.Price = Gold.FromCoins(300);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddHealingUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetAudibility(2);
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.Heal(5.d4() + 10, Modifier.Zero);
              B.RemoveTransient(Properties.blindness, Properties.deafness);
            },
            U =>
            {
              U.Heal(5.d4() + 5, Modifier.Zero);
              U.RemoveTransient(Properties.blindness, Properties.deafness);
            },
            C =>
            {
              C.Heal(Dice.Fixed(10), Modifier.Zero);
            }
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.RemoveTransient(Properties.blindness, Properties.deafness);
          A.WithSourceSanctity
          (
            B => B.Heal(5.d6() + 50, Modifier.Plus5),
            U => U.Heal(5.d6() + 25, Modifier.Plus3),
            C => C.Heal(5.d6(), Modifier.Zero)
          );
        });
      });

      wand_of_fear = AddWand("wand of fear", I =>
      {
        I.Description = null;
        I.SetAppearance("rusty wand", null);
        I.Glyph = Glyphs.rusty_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 25;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.iron;
        I.Essence = WandEssence2;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddPropertyAreaUse(Motions.zap, Properties.fear, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.psychic, Dice.Zero);
          Use.Apply.WithSourceSanctity
          (
            B => B.AreaTransient(Properties.fear, 4.d6(), Kinds.List.ToArray()),
            U => U.AreaTransient(Properties.fear, 3.d6(), Kinds.Living.ToArray()),
            C => C.AreaTransient(Properties.fear, 1.d6(), Kinds.Living.ToArray())
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.AreaTransient(Properties.fear, 10.d6(), Kinds.Living.ToArray());
        });
      });

      wand_of_fire = AddWand("wand of fire", I =>
      {
        I.Description = null;
        I.SetAppearance("hexagonal wand", null);
        I.Glyph = Glyphs.hexagonal_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 25;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.iron;
        I.Essence = WandEssence3;
        I.Price = Gold.FromCoins(175);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddElementBlastUse(Motions.zap, Elements.fire, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Beam(Beams.fire, 1.d4() + 4)
             .SetAudibility(10);
          Use.Apply.WithSourceSanctity
          (
            B => B.Harm(Elements.fire, 7.d6()),
            U => U.Harm(Elements.fire, 6.d6()),
            C => C.Harm(Elements.fire, 5.d6())
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.Harm(Elements.fire, 14.d6() + 14);
          A.ConsumeResistance(Elements.fire);
        });
      });

      wand_of_fireball = AddWand("wand of fireball", I =>
      {
        I.Description = null;
        I.SetAppearance("octagonal wand", null);
        I.Glyph = Glyphs.octagonal_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 5;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.iron;
        I.Essence = WandEssence5;
        I.Price = Gold.FromCoins(300);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddElementBlastUse(Motions.zap, Elements.fire, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Explosion(Explosions.fiery, 2.d6() + 2);
          Use.Apply.WithSourceSanctity
          (
            B => B.Harm(Elements.fire, 8.d6()),
            U => U.Harm(Elements.fire, 6.d6()),
            C => C.Harm(Elements.fire, 4.d6())
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.Harm(Elements.fire, 14.d6() + 14);
          A.ConsumeResistance(Elements.fire);
        });
      });

      wand_of_iceball = AddWand("wand of iceball", I =>
      {
        I.Description = null;
        I.SetAppearance("jewelled wand", null);
        I.Glyph = Glyphs.jewelled_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 5;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.iron;
        I.Essence = WandEssence5;
        I.Price = Gold.FromCoins(300);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddElementBlastUse(Motions.zap, Elements.cold, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Explosion(Explosions.frosty, 2.d6() + 2);
          Use.Apply.WithSourceSanctity
          (
            B => B.Harm(Elements.cold, 8.d6()),
            U => U.Harm(Elements.cold, 6.d6()),
            C => C.Harm(Elements.cold, 4.d6())
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.Harm(Elements.cold, 14.d6() + 14);
          A.ConsumeResistance(Elements.cold);
        });
      });

      wand_of_healing = AddWand("wand of healing", I =>
      {
        I.Description = null;
        I.SetAppearance("bamboo wand", null);
        I.Glyph = Glyphs.bamboo_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 60;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.wood;
        I.Essence = WandEssence1;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddHealingUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.spirit, Dice.One)
             .SetAudibility(2);
          Use.Apply.WithSourceSanctity
          (
            B =>
            {
              B.Heal(5.d2() + 5, Modifier.Zero);
              B.RemoveTransient(Properties.blindness, Properties.deafness);
            },
            U =>
            {
              U.Heal(5.d2(), Modifier.Zero);
              U.RemoveTransient(Properties.blindness, Properties.deafness);
            },
            C =>
            {
              C.Heal(Dice.Five, Modifier.Zero);
            }
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.WithSourceSanctity
          (
            B => B.Heal(5.d4() + 20, Modifier.Plus2),
            U => U.Heal(5.d4() + 10, Modifier.Plus1),
            C => C.Heal(5.d4(), Modifier.Zero)
          );
        });
      });

      wand_of_light = AddWand("wand of light", I =>
      {
        I.Description = null;
        I.SetAppearance("glass wand", null);
        I.Glyph = Glyphs.glass_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 50;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.glass;
        I.Essence = WandEssence1;
        I.Price = Gold.FromCoins(100);
        I.SetImpact(Sonics.broken_glass);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 11;
        I.AddObviousUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.flash, Dice.Zero)
             .SetTerminates();
          Use.Apply.WithSourceSanctity
          (
            B => B.Light(true),
            U => U.Light(true),
            C => C.Light(false)
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.Light(true);
          A.MajorProperty(Properties.flight);
        });
      });

      wand_of_lightning = AddWand("wand of lightning", I =>
      {
        I.Description = null;
        I.SetAppearance("curved wand", null);
        I.Glyph = Glyphs.curved_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 20;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.iron;
        I.Essence = WandEssence3;
        I.Price = Gold.FromCoins(175);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddElementBlastUse(Motions.zap, Elements.shock, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Beam(Beams.lightning, 1.d4() + 4)
             .SetAudibility(10);
          Use.Apply.WithSourceSanctity
          (
            B => B.Harm(Elements.shock, 7.d6()),
            U => U.Harm(Elements.shock, 6.d6()),
            C => C.Harm(Elements.shock, 5.d6())
          );
          Use.Apply.WhenChance(Chance.OneIn2, T => T.ApplyTransient(Properties.blindness, 1.d4() + 1));
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.Harm(Elements.shock, 14.d6() + 14);
          A.ConsumeResistance(Elements.shock);
        });
      });

      wand_of_locking = AddWand("wand of locking", I =>
      {
        I.Description = null;
        I.SetAppearance("aluminium wand", null);
        I.Glyph = Glyphs.aluminium_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 25;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.tin;
        I.Essence = WandEssence1;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddObviousUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.force, 2.d6() + 2)
             .SetObjects()
             .SetAudibility(1);
          Use.Apply.Locking();
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.WhenChance(Chance.OneIn2, T => T.GainSkill(RandomPoints: false, Skills.locks), E => E.Nothing());
        });
      });

      wand_of_magic_missile = AddWand("wand of magic missile", I =>
      {
        I.Description = null;
        I.SetAppearance("steel wand", null);
        I.Glyph = Glyphs.steel_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 50;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.iron;
        I.Essence = WandEssence2;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddElementBlastUse(Motions.zap, Elements.magical, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Beam(Beams.magic_missile, 1.d4() + 4)
             .SetAudibility(10);
          Use.Apply.WithSourceSanctity
          (
            B => B.Harm(Elements.magical, 2.d6() + 1),
            U => U.Harm(Elements.magical, 2.d6()),
            C => C.Harm(Elements.magical, 2.d6() - 1)
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.Harm(Elements.magical, 14.d6() + 14);
          A.ConsumeResistance(Elements.magical);
        });
      });

      wand_of_make_invisible = AddWand("wand of make invisible", I =>
      {
        I.Description = null;
        I.SetAppearance("marble wand", null);
        I.Glyph = Glyphs.marble_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 45;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.stone;
        I.Essence = WandEssence2;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddPropertyBuffUse(Motions.zap, Properties.invisibility, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.magic, 2.d6() + 2)
             .SetAudibility(1);
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.invisibility, 20.d6()),
            U => U.ApplyTransient(Properties.invisibility, 10.d6()),
            C => C.ApplyTransient(Properties.invisibility, 5.d6())
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.MajorProperty(Properties.invisibility);
        });
      });

      wand_of_nothing = AddWand("wand of nothing", I =>
      {
        I.Description = null;
        I.SetAppearance("oak wand", null);
        I.Glyph = Glyphs.oak_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 20;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.wood;
        I.Essence = WandEssence0;
        I.Price = Gold.FromCoins(100);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddObviousUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Plain(Dice.Zero);
          Use.Apply.Nothing();
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand);
      });

      wand_of_opening = AddWand("wand of opening", I =>
      {
        I.Description = null;
        I.SetAppearance("zinc wand", null);
        I.Glyph = Glyphs.zinc_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 25;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.iron;
        I.Essence = WandEssence1;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddObviousUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.force, 2.d6() + 2)
             .SetObjects()
             .SetAudibility(1);
          Use.Apply.Opening();
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.WhenChance(Chance.OneIn2, T => T.GainSkill(RandomPoints: false, Skills.locks), E => E.Nothing());
        });
      });

      wand_of_polymorph = AddWand("wand of polymorph", I =>
      {
        I.Description = null;
        I.SetAppearance("silver wand", null);
        I.Glyph = Glyphs.silver_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 45;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.silver;
        I.Essence = WandEssence5;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddPropertyBuffUse(Motions.zap, Properties.polymorph, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.spirit, 2.d6() + 2)
             .SetObjects()
             .SetAudibility(5);
          Use.Apply.Polymorph();
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.MajorProperty(Properties.polymorph);
          A.Polymorph();
        });
      });

      wand_of_punishment = AddWand("wand of punishment", I =>
      {
        I.Description = null;
        I.SetAppearance("spiked wand", null);
        I.Glyph = Glyphs.spiked_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 45;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.silver;
        I.Essence = WandEssence4;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddObviousUse(Motions.zap, new Utility(Purpose.Punish), WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.psychic, 2.d6() + 2)
             .SetAudibility(5);
          Use.Apply.Punish(Codex.Punishments.List.ToArray());
          // TODO: BUC differences?
          //Use.Apply.WithSourceSanctity
          //(
          //  B => B.Punish(Codex.Punishments.List.ToArray()),
          //  U => U.Punish(Codex.Punishments.List.ToArray()),
          //  C => C.Punish(Codex.Punishments.List.ToArray())
          //);
        });
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.Punish(Codex.Punishments.gluttony);
        });
      });

      wand_of_secret_detection = AddWand("wand of secret detection", I =>
      {
        I.Description = null;
        I.SetAppearance("balsa wand", null);
        I.Glyph = Glyphs.balsa_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 30;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.wood;
        I.Essence = WandEssence1;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 11;
        I.AddObviousUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.boost, Dice.Zero)
             .SetTerminates();
          Use.Apply.WithSourceSanctity
          (
            B => B.Searching(Range.Sq6),
            U => U.Searching(Range.Sq4),
            C => C.Searching(Range.Sq2)
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.MajorProperty(Properties.searching);
        });
      });

      wand_of_sleep = AddWand("wand of sleep", I =>
      {
        I.Description = null;
        I.SetAppearance("runed wand", null);
        I.Glyph = Glyphs.runed_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 50;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.iron;
        I.Essence = WandEssence2;
        I.Price = Gold.FromCoins(175);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddPropertyBlastUse(Motions.zap, Properties.sleeping, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Beam(Beams.sleep, 1.d4() + 4)
             .SetAudibility(0);
          Use.Apply.Harm(Elements.sleep, Dice.Zero); // hatching eggs.
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.sleeping, 20.d6()),
            U => U.ApplyTransient(Properties.sleeping, 10.d6()),
            C => C.ApplyTransient(Properties.sleeping, 5.d6())
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.ApplyTransient(Properties.narcolepsy, 1.d250() + 250);
          A.ApplyTransient(Properties.sleeping, 1.d25() + 25);
        });
      });

      wand_of_slow = AddWand("wand of slow", I =>
      {
        I.Description = null;
        I.SetAppearance("tin wand", null);
        I.Glyph = Glyphs.tin_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 45;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.tin;
        I.Essence = WandEssence2;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddPropertyBlastUse(Motions.zap, Properties.slowness, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.spirit, 2.d6() + 2)
             .SetAudibility(5);
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.slowness, 20.d6()),
            U => U.ApplyTransient(Properties.slowness, 10.d6()),
            C => C.ApplyTransient(Properties.slowness, 5.d6())
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.ApplyTransient(Properties.slowness, 1.d250() + 250);
        });
      });

      wand_of_phase = AddWand("wand of phase", I =>
      {
        I.Description = null;
        I.SetAppearance("crystal wand", null);
        I.Glyph = Glyphs.crystal_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 10;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.crystal;
        I.Essence = WandEssence4;
        I.Price = Gold.FromCoins(350);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddPropertyBuffUse(Motions.zap, Properties.phasing, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.spirit, 2.d6() + 2)
             .SetAudibility(5);
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.phasing, 20.d6()),
            U => U.ApplyTransient(Properties.phasing, 10.d6()),
            C => C.ApplyTransient(Properties.phasing, 5.d6())
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.ApplyTransient(Properties.phasing, 1.d250() + 250);
        });
      });

      wand_of_haste = AddWand("wand of haste", I =>
      {
        I.Description = null;
        I.SetAppearance("brass wand", null);
        I.Glyph = Glyphs.brass_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 45;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.copper;
        I.Essence = WandEssence2;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddPropertyBuffUse(Motions.zap, Properties.quickness, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.spirit, 2.d6() + 2)
             .SetAudibility(5);
          Use.Apply.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.quickness, 20.d6()),
            U => U.ApplyTransient(Properties.quickness, 10.d6()),
            C => C.ApplyTransient(Properties.quickness, 5.d6())
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.ApplyTransient(Properties.quickness, 1.d250() + 250);
        });
      });

      wand_of_striking = AddWand("wand of striking", I =>
      {
        I.Description = null;
        I.SetAppearance("ebony wand", null);
        I.Glyph = Glyphs.ebony_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 65;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.wood;
        I.Essence = WandEssence2;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddElementBlastUse(Motions.zap, Elements.force, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.force, 2.d4() + 2)
             .SetObjects()
             .SetAudibility(10)
             .SetPenetrates();
          Use.Apply.WithSourceSanctity
          (
            B => B.Harm(Elements.force, 3.d6() + 3),
            U => U.Harm(Elements.force, 2.d6() + 2),
            C => C.Harm(Elements.force, 1.d6() + 1)
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.Harm(Elements.force, 6.d6() + 6);
        });
      });

      wand_of_teleportation = AddWand("wand of teleportation", I =>
      {
        I.Description = null;
        I.SetAppearance("iridium wand", null);
        I.Glyph = Glyphs.iridium_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 45;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.platinum;
        I.Essence = WandEssence4;
        I.Price = Gold.FromCoins(200);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddTeleportUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.magic, 2.d6() + 2)
             .SetObjects()
             .SetAudibility(1);
          Use.Apply.TeleportFloorAsset();
          Use.Apply.TeleportCharacter(Properties.teleportation);
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.MajorProperty(Properties.teleportation);
        });
      });

      wand_of_theft = AddWand("wand of theft", I =>
      {
        I.Description = null;
        I.SetAppearance("gold wand", null);
        I.Glyph = Glyphs.gold_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 45;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.gold;
        I.Essence = WandEssence4;
        I.Price = Gold.FromCoins(175);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddBlastUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.force, 1.d6() + 2)
             .SetObjects()
             .SetAudibility(1);
          Use.Apply.StealAsset();
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.StealAsset();
        });
      });

      wand_of_undead_turning = AddWand("wand of undead turning", I =>
      {
        I.Description = null;
        I.SetAppearance("copper wand", null);
        I.Glyph = Glyphs.copper_wand;
        I.Sonic = Sonics.wand;
        I.Series = WandSeries;
        I.Rarity = 45;
        I.Size = WandSize;
        I.Weight = Weight.FromUnits(70);
        I.Material = Materials.copper;
        I.Essence = WandEssence3;
        I.Price = Gold.FromCoins(150);
        I.SetWeakness(WandWeakness);
        I.ChargesDice = 1.d5() + 4;
        I.AddObviousUse(Motions.zap, WandZapDelay, Sonics.magic, Use =>
        {
          Use.SetCast().Strike(Strikes.holy, Dice.Zero);
          Use.Apply.WithSourceSanctity
          (
            B => B.AreaTransient(Properties.fear, 6.d6(), Kinds.Undead.ToArray()),
            U => U.AreaTransient(Properties.fear, 4.d6(), Kinds.Undead.ToArray()),
            C => C.AreaTransient(Properties.rage, 4.d6(), Kinds.Undead.ToArray())
          );
        });
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(10), Sonics.wand, A =>
        {
          A.Polymorph(Entities.vampire);
        });
      });

      SetUpgradeDowngradePair(wand_of_healing, wand_of_extra_healing);

      CodexRecruiter.Enrol(() =>
      {
        foreach (var Wand in Stocks.wand.Items.Where(W => W != wand_of_nothing && W.DowngradeItem == null && !W.Artifact))
          Register.Edit(Wand).SetDowngradeItem(wand_of_nothing);
      });
      #endregion

      #region weapon.
      var WeaponEssence0 = Essence.FromUnits(5);
      var WeaponEssence1 = Essence.FromUnits(10);
      var WeaponEssence2 = Essence.FromUnits(15);
      var WeaponEssence3 = Essence.FromUnits(25);
      var WeaponEssence4 = Essence.FromUnits(50);
      var StaffSeries = new Series("staff");

      rubber_hose = AddReachWeapon("rubber hose", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.rubber_hose;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.plastic;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(4);
        //I.AddEat(20, Delay.FromTurns(20), Sonics.tool); // Plastic can't be eaten by any diet.
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.tool);
        I.SetOneHandedWeapon(Skills.whip, null, Elements.physical, DamageType.Bludgeon, 1.d2(), A =>
        {
          A.Disarm();
        });
      });

      iron_chain = AddReachWeapon("iron chain", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.iron_chain;
        I.Sonic = Sonics.tool;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(1200);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 600, Delay.FromTurns(20), Sonics.tool);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.tool);
        I.SetOneHandedWeapon(Skills.whip, null, Elements.physical, DamageType.Bludgeon, 1.d3(), A =>
        {
          A.Disarm();
        });
      });

      aklys = AddThrownWeapon("aklys", I =>
      {
        I.Description = null;
        I.SetAppearance("thonged club", null);
        I.Glyph = Glyphs.aklys;
        I.Sonic = Sonics.weapon;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.Rarity = 16;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(150);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(4);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(15), Sonics.weapon);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.club, null, Elements.physical, DamageType.Bludgeon, 1.d6());
      });

      arrow = AddRangedMissile(Ammunition.Arrow, "arrow", I =>
      {
        I.Description = "A projectile consisting of a wooden shaft with a sharp metal tip at its front and stabilising fins at its rear.";
        I.Glyph = Glyphs.arrow;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 50;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.iron;
        I.Essence = AmmoEssence0;
        I.Price = Gold.FromCoins(2);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.ammo);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.bow, null, Elements.physical, DamageType.Pierce, 1.d6());
      });

      athame = AddMeleeWeapon("athame", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.athame;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(4);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, null, Elements.physical, DamageType.Slash, 1.d4());
      });

      axe = AddMeleeWeapon("axe", I =>
      {
        I.Description = "A sharp iron wedge fashioned to a sturdy handle.";
        I.Glyph = Glyphs.axe;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 35;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(600);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(8);
        I.AddObviousIngestUse(Motions.eat, 60, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.axe, null, Elements.physical, DamageType.Slash, 1.d7());
      });

      silver_axe = AddMeleeWeapon("silver axe", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.silver_axe;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(720);
        I.Material = Materials.silver;
        I.Essence = WeaponEssence2;
        I.Price = Gold.FromCoins(40);
        I.AddObviousIngestUse(Motions.eat, 70, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.axe, null, Elements.physical, DamageType.Slash, 1.d7());
      });

      hatchet = AddThrownWeapon("hatchet", I =>
      {
        I.Description = null;
        I.SetAppearance("throwing axe", null);
        I.Glyph = Glyphs.hatchet;
        I.Sonic = Sonics.weapon;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.Rarity = 15;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(350);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(6);
        I.AddObviousIngestUse(Motions.eat, 35, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.axe, null, Elements.physical, DamageType.Slash, 1.d6());
      });

      battleaxe = AddMeleeWeapon("battle-axe", I =>
      {
        I.Description = "This axe is specifically designed to facilitate deep, grievous wounds in combat. This large weapon is wielded in both hands.";
        I.SetAppearance("double-headed axe", null); // TODO: mithril battle-axe doesn't have an appearance, so this shouldn't either?
        I.Glyph = Glyphs.battleaxe;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1200);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(40);
        I.AddObviousIngestUse(Motions.eat, 120, Delay.FromTurns(40), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.axe, null, Elements.physical, DamageType.Slash, 2.d6());
      });

      bardiche = AddReachWeapon("bardiche", I =>
      {
        I.Description = "This is a pole weapon with a long, cleaver type blade.";
        I.SetAppearance("long poleaxe", null);
        I.Glyph = Glyphs.bardiche;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1200);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(7);
        I.AddObviousIngestUse(Motions.eat, 120, Delay.FromTurns(40), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Slash, 2.d4());
      });

      bec_de_corbin = AddReachWeapon("bec de corbin", I =>
      {
        I.Description = "This is a pole weapon consisting of a modified hammer head and spike mounted atop a long pole.";
        I.SetAppearance("beaked polearm", null);
        I.Glyph = Glyphs.bec_de_corbin;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1000);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(8);
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(25), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Bludgeon, 1.d8());
      });

      billguisarme = AddReachWeapon("bill-guisarme", I =>
      {
        I.Description = "This weapon consists of a hooked chopping blade with several pointed projections mounted on a staff. The end of the cutting blade curves forward to form a hook. In addition, the blade has one pronounced spike straight off the top like a spear head, and also a hook or spike mounted on the reverse side of the blade.";
        I.SetAppearance("hooked polearm", null);
        I.Glyph = Glyphs.billguisarme;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1200);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(7);
        I.AddObviousIngestUse(Motions.eat, 120, Delay.FromTurns(40), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Slash, 2.d4());
      });

      var HorshoePrice = Gold.FromCoins(5);

      horseshoe = AddThrownWeapon("horseshoe", I =>
      {
        I.Description = null;
        I.Appearance = null;
        I.Glyph = Glyphs.horseshoe;
        I.Sonic = Sonics.weapon;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.Rarity = 15;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(250);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = HorshoePrice;
        I.AddObviousIngestUse(Motions.eat, 25, Delay.FromTurns(10), Sonics.weapon);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.disc, Sonics.throw_object, Elements.physical, DamageType.Bludgeon, 1.d5());
      });

      magic_horseshoe = AddThrownWeapon("magic horseshoe", I =>
      {
        I.Description = null;
        I.SetAppearance("horseshoe", null, Glyphs.horseshoe, HorshoePrice);
        I.Glyph = Glyphs.magic_horseshoe;
        I.Sonic = Sonics.weapon;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(250);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence4;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 250, Delay.FromTurns(10), Sonics.weapon);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.disc, Sonics.throw_object, Elements.physical, DamageType.Bludgeon, 1.d10(), D => D.ApplyTransient(Properties.confusion, 3.d6()));
      });

      boomerang = AddThrownWeapon("boomerang", I =>
      {
        I.Description = "A flat throwing weapon made of wood, often used for hunting.";
        I.SetAppearance("throwing stick", null);
        I.Glyph = Glyphs.boomerang;
        I.Sonic = Sonics.weapon;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.Rarity = 15;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(20);
        I.AddObviousIngestUse(Motions.eat, 5, Delay.FromTurns(10), Sonics.weapon);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.disc, Sonics.throw_object, Elements.physical, DamageType.Bludgeon, 1.d7());
      });

      bow = AddRangedWeapon(Ammunition.Arrow, "bow", I =>
      {
        I.Description = "A semi-rigid weapon made up of a notched wooden shaft with a drawstring attached to both ends, used to launch arrows.";
        I.Glyph = Glyphs.bow;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 24;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(60);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.bow, Sonics.bow_fire, Elements.physical, DamageType.Bludgeon, Dice.One);
      });

      brass_knuckles = AddMeleeWeapon("brass knuckles", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.brass_knuckles;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 12;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(150);
        I.Material = Materials.copper;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(4);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(15), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.unarmed_combat, null, Elements.physical, DamageType.Bludgeon, 1.d4());
      });

      broadsword = AddMeleeWeapon("broadsword", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.broadsword;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 20;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(700);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 70, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 2.d4());
      });

      bullwhip = AddReachWeapon("bullwhip", I =>
      {
        I.Description = "A long, flexible leather thong. Unexpectedly sharp.";
        I.Glyph = Glyphs.bullwhip;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.leather;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(4);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.leather);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.leather);
        I.SetOneHandedWeapon(Skills.whip, null, Elements.physical, DamageType.Slash, 1.d4(), A =>
        {
          A.Disarm();
        });
      });

      mithril_whip = AddReachWeapon("mithril whip", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.mithril_whip;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.mithril;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(40);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(20), Sonics.leather);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.leather);
        I.SetOneHandedWeapon(Skills.whip, null, Elements.physical, DamageType.Slash, 1.d4() + 1, A =>
        {
          A.Disarm();
        });
      });

      club = AddMeleeWeapon("club", I =>
      {
        I.Description = "A thick wooden bludgeon. What it lacks in style, it makes up for in function.";
        I.Glyph = Glyphs.club;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 22;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(3);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.club, null, Elements.physical, DamageType.Bludgeon, 1.d6());
      });

      stone_club = AddMeleeWeapon("stone club", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.stone_club;
        I.Sonic = Sonics.scrape;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(6000);
        I.Material = Materials.stone;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(30);
        I.AddObviousIngestUse(Motions.eat, 600, Delay.FromTurns(60), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        var W = I.SetOneHandedWeapon(Skills.club, null, Elements.physical, DamageType.Bludgeon, 2.d10());
        W.AttackModifier = Modifier.Minus5;
      });

      war_club = AddMeleeWeapon("war club", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.war_club;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 11;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(600);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(60);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(50), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.club, null, Elements.physical, DamageType.Bludgeon, 2.d5());
      });

      crossbow = AddRangedWeapon(Ammunition.Bolt, "crossbow", I =>
      {
        I.Description = "A horizontal, mechanically-aided bow and arrow with a shoulder stock.";
        I.Glyph = Glyphs.crossbow;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 45;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(40);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.crossbow, Sonics.bow_fire, Elements.physical, DamageType.Bludgeon, Dice.One);
      });

      crossbow_bolt = AddRangedMissile(Ammunition.Bolt, "crossbow bolt", I =>
      {
        I.Description = "An iron projectile made up of a shaft with a sharpened square-pyramid tip.";
        I.Glyph = Glyphs.crossbow_bolt;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 45;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.iron;
        I.Essence = AmmoEssence0;
        I.Price = Gold.FromCoins(2);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.ammo);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.crossbow, null, Elements.physical, DamageType.Pierce, 1.d4() + 1);
      });

      mithril_crossbow_bolt = AddRangedMissile(Ammunition.Bolt, "mithril crossbow bolt", I =>
      {
        I.Description = null;
        //I.SetAppearance("mithril bolt", null);
        I.Glyph = Glyphs.mithril_crossbow_bolt;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(5);
        I.Material = Materials.mithril;
        I.Essence = AmmoEssence2;
        I.Price = Gold.FromCoins(20);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.ammo);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.crossbow, null, Elements.physical, DamageType.Pierce, 1.d4() + 2);
      });

      silver_crossbow_bolt = AddRangedMissile(Ammunition.Bolt, "silver crossbow bolt", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.silver_crossbow_bolt;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(12);
        I.Material = Materials.silver;
        I.Essence = AmmoEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.ammo);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.crossbow, null, Elements.physical, DamageType.Pierce, 1.d4() + 1);
      });

      crysknife = AddMeleeWeapon("crysknife", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.crysknife;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.stone;
        I.Essence = WeaponEssence4;
        I.Price = Gold.FromCoins(100);
        I.SetDerivative(Entities.long_worm);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, null, Elements.physical, DamageType.Pierce, 1.d10(), A =>
        {
          A.WithSourceSanctity
          (
            B => B.WhenChance(Chance.OneIn100, T => T.Degrade()),
            U => U.WhenChance(Chance.OneIn50, T => T.Degrade()),
            C => C.WhenChance(Chance.OneIn10, T => T.Degrade())
          );
          ;
        });

        // TODO: 1% chance of degrading.
      });

      dagger = AddThrownWeapon("dagger", I =>
      {
        I.Description = "A long iron knife with a sharp tip, weighted for throwing.";
        I.Glyph = Glyphs.dagger;
        I.Sonic = Sonics.weapon;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.Rarity = 25;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(4);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.weapon);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, Sonics.throw_object, Elements.physical, DamageType.Pierce, 1.d4());
      });

      dark_elven_arrow = AddRangedMissile(Ammunition.Arrow, "dark elven arrow", I =>
      {
        I.Description = null;
        I.SetAppearance("black runed arrow", null);
        I.Glyph = Glyphs.dark_elven_arrow;
        I.Sonic = Sonics.ammo;
        I.OriginRace = Races.elf;
        I.Series = null;
        I.Rarity = 13;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.wood;
        I.Essence = AmmoEssence1;
        I.Price = Gold.FromCoins(2);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.ammo);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.bow, null, Elements.physical, DamageType.Pierce, 1.d7(), A =>
        {
          A.Macro(KnockoutPoison);
        });
      });

      dark_elven_bow = AddRangedWeapon(Ammunition.Arrow, "dark elven bow", I =>
      {
        I.Description = null;
        I.SetAppearance("black runed bow", null);
        I.Glyph = Glyphs.dark_elven_bow;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.elf;
        I.Series = null;
        I.Rarity = 6;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(60);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        var W = I.SetTwoHandedWeapon(Skills.bow, Sonics.bow_fire, Elements.physical, DamageType.Bludgeon, Dice.One, A =>
        {
          A.Macro(KnockoutPoison);
        });
      });

      dark_elven_dagger = AddThrownWeapon("dark elven dagger", I =>
      {
        I.Description = null;
        I.SetAppearance("black runed dagger", null);
        I.Glyph = Glyphs.dark_elven_dagger;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.elf;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(4);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.weapon);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, Sonics.throw_object, Elements.physical, DamageType.Pierce, 1.d5(), A =>
        {
          A.Macro(KnockoutPoison);
        });
      });

      dark_elven_short_sword = AddMeleeWeapon("dark elven short sword", I =>
      {
        I.Description = null;
        I.SetAppearance("black runed short sword", null);
        I.Glyph = Glyphs.dark_elven_short_sword;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.elf;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Pierce, 1.d8(), A =>
        {
          A.Macro(KnockoutPoison);
        });
      });

      dart = AddThrownMissile(Ammunition.Dart, "dart", I =>
      {
        I.Description = "A small missile with a pointed tip meant for throwing.";
        I.Glyph = Glyphs.dart;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 55;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.iron;
        I.Essence = AmmoEssence0;
        I.Price = Gold.FromCoins(2);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.weapon);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.dart, Sonics.throw_object, Elements.physical, DamageType.Pierce, 1.d3())/*.FixedRange = 2*/; // NOTE: for testing short ranged AI.
      });

      mithril_dart = AddThrownMissile(Ammunition.Dart, "mithril dart", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.mithril_dart;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 5;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(5);
        I.Material = Materials.mithril;
        I.Essence = AmmoEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.weapon);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d4() + 4;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.dart, Sonics.throw_object, Elements.physical, DamageType.Pierce, 1.d3() + 1);
      });

      poison_dart = AddThrownMissile(Ammunition.Dart, "poison dart", I =>
      {
        I.Description = "A small missile with a reservoir of liquid delivered through a hollow pointed tip.";
        I.Glyph = Glyphs.poison_dart;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 15;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(15);
        I.Material = Materials.iron;
        I.Essence = AmmoEssence1;
        I.Price = Gold.FromCoins(4);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.weapon);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d3() + 3;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.dart, Sonics.throw_object, Elements.physical, DamageType.Pierce, 1.d2(), D =>
        {
          D.Harm(Elements.poison, 1.d3());
          D.WhenChance(Chance.OneIn10, T =>
          {
            T.Afflict(Codex.Afflictions.poisoning);
          });
        });
      });

      dwarvish_short_sword = AddMeleeWeapon("dwarvish short sword", I =>
      {
        I.Description = "This dwarvish manufactured sword is sturdy and suited to being wielded in the offhand.";
        I.SetAppearance("broad short sword", null);
        I.Glyph = Glyphs.dwarvish_short_sword;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.dwarf;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Pierce, 1.d7());
      });

      dwarvish_spear = AddReachWeapon("dwarvish spear", I =>
      {
        I.Description = "This compact, dwarvish manufactured spear is ideal for close quarters combat.";
        I.SetAppearance("stout spear", null);
        I.Glyph = Glyphs.dwarvish_spear;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.dwarf;
        I.Series = null;
        I.Rarity = 12;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(350);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(3);
        I.AddObviousIngestUse(Motions.eat, 35, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.spear, null, Elements.physical, DamageType.Pierce, 1.d8());
      });

      elven_arrow = AddRangedMissile(Ammunition.Arrow, "elven arrow", I =>
      {
        I.Description = null;
        I.SetAppearance("runed arrow", null);
        I.Glyph = Glyphs.elven_arrow;
        I.Sonic = Sonics.ammo;
        I.OriginRace = Races.elf;
        I.Series = null;
        I.Rarity = 25;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.wood;
        I.Essence = AmmoEssence1;
        I.Price = Gold.FromCoins(2);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.ammo);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.bow, null, Elements.physical, DamageType.Pierce, 1.d7());
      });

      elven_bow = AddRangedWeapon(Ammunition.Arrow, "elven bow", I =>
      {
        I.Description = null;
        I.SetAppearance("runed bow", null);
        I.Glyph = Glyphs.elven_bow;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.elf;
        I.Series = null;
        I.Rarity = 12;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(60);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.bow, Sonics.bow_fire, Elements.physical, DamageType.Bludgeon, Dice.One);
        // TODO: difference between bows.
      });

      elven_broadsword = AddMeleeWeapon("elven broadsword", I =>
      {
        I.Description = null;
        I.SetAppearance("runed broadsword", null);
        I.Glyph = Glyphs.elven_broadsword;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.elf;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(700);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 70, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 2.d5());
      });

      elven_dagger = AddThrownWeapon("elven dagger", I =>
      {
        I.Description = "This high-quality elven manufactured dagger is perfectly weighted for throwing.";
        I.SetAppearance("runed dagger", null);
        I.Glyph = Glyphs.elven_dagger;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.elf;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.Rarity = 8;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(4);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.weapon);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, Sonics.throw_object, Elements.physical, DamageType.Pierce, 1.d5());
      });

      elven_short_sword = AddMeleeWeapon("elven short sword", I =>
      {
        I.Description = "This light blade of elvish manufacture is particularly suited as an offhand weapon.";
        I.SetAppearance("runed short sword", null);
        I.Glyph = Glyphs.elven_short_sword;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.elf;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Pierce, 1.d8());
      });

      elven_spear = AddReachWeapon("elven spear", I =>
      {
        I.Description = "Its high-quality elvish manufacture makes for an effective close combat weapon.";
        I.SetAppearance("runed spear", null);
        I.Glyph = Glyphs.elven_spear;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.elf;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(350);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(3);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.spear, null, Elements.physical, DamageType.Pierce, 1.d7());
      });

      fauchard = AddReachWeapon("fauchard", I =>
      {
        I.Description = "This is a curved blade put atop a long pole. The blade is moderately curved along its length with the cutting edge only on the concave side.";
        I.SetAppearance("pole sickle", null);
        I.Glyph = Glyphs.fauchard;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(600);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(5);
        I.AddObviousIngestUse(Motions.eat, 60, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Slash, 1.d6());
      });

      flail = AddMeleeWeapon("flail", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.flail;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 30;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(450);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(4);
        I.AddObviousIngestUse(Motions.eat, 45, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.flail, null, Elements.physical, DamageType.Bludgeon, 1.d6() + 1);
      });

      glaive = AddReachWeapon("glaive", I =>
      {
        I.Description = "This weapon consists of a large blade on the end of a long pole. The blade is affixed in a socket-shaft configuration similar to an axe head.";
        I.SetAppearance("single-edged polearm", null);
        I.Glyph = Glyphs.glaive;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(750);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(6);
        I.AddObviousIngestUse(Motions.eat, 75, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Slash, 1.d6());
      });

      great_dagger = AddMeleeWeapon("great dagger", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.great_dagger;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(20);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, null, Elements.physical, DamageType.Slash, 1.d6());
      });

      guisarme = AddReachWeapon("guisarme", I =>
      {
        I.Description = "This weapon consists of a single-edged blade on the end of a pole.";
        I.SetAppearance("pruning hook", null);
        I.Glyph = Glyphs.guisarme;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(800);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(5);
        I.AddObviousIngestUse(Motions.eat, 80, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Slash, 2.d4());
      });

      halberd = AddReachWeapon("halberd", I =>
      {
        I.Description = "This is a two-handed pole weapon consisting of an axe blade with a spike mounted on a long shaft.";
        I.SetAppearance("angled poleaxe", null);
        I.Glyph = Glyphs.halberd;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1500);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(40), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Slash, 1.d8());
      });

      heavy_hammer = AddMeleeWeapon("heavy hammer", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.heavy_hammer;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(600);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 60, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.hammer, null, Elements.physical, DamageType.Bludgeon, 2.d4() + 2);
      });

      silver_heavy_hammer = AddMeleeWeapon("silver heavy hammer", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.silver_heavy_hammer;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(720);
        I.Material = Materials.silver;
        I.Essence = WeaponEssence2;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 70, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.hammer, null, Elements.physical, DamageType.Bludgeon, 2.d4() + 1);
      });

      javelin = AddThrownWeapon("javelin", I =>
      {
        I.Description = "This is a light spear designed to be thrown by hand.";
        I.SetAppearance("throwing spear", null);
        I.Glyph = Glyphs.javelin;
        I.Sonic = Sonics.weapon;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(3);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.spear, Sonics.throw_object, Elements.physical, DamageType.Pierce, 1.d6());
      });

      katar = AddMeleeWeapon("katar", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.katar;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 6;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(250);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(8);
        I.AddObviousIngestUse(Motions.eat, 25, Delay.FromTurns(25), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.unarmed_combat, null, Elements.physical, DamageType.Pierce, 1.d5());
      });

      mithril_katar = AddMeleeWeapon("mithril katar", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.mithril_katar;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(125);
        I.Material = Materials.mithril;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(80);
        I.AddObviousIngestUse(Motions.eat, 25, Delay.FromTurns(25), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.unarmed_combat, null, Elements.physical, DamageType.Pierce, 1.d5() + 1);
      });

      kanabo = AddMeleeWeapon("kanabo", I =>
      {
        I.Description = null;
        I.SetAppearance("studded samurai stick", null);
        I.Glyph = Glyphs.kanabo;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(60);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.mace, null, Elements.physical, DamageType.Bludgeon, 2.d5());
      });

      nunchaku = AddMeleeWeapon("nunchaku", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.nunchaku;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(150);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(20);
        I.AddObviousIngestUse(Motions.eat, 15, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        var W = I.SetOneHandedWeapon(Skills.flail, null, Elements.physical, DamageType.Bludgeon, 1.d3() + 1, A =>
        {
          A.Disarm();
        });
        W.AttackModifier = Modifier.Plus1;
      });

      sai = AddMeleeWeapon("sai", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.sai;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(60);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(15);
        I.SetArmour(Skills.light_armour, 1);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, null, Elements.physical, DamageType.Pierce, 1.d3());
        I.AddObviousIngestUse(Motions.eat, 6, Delay.FromTurns(10), Sonics.weapon);
      });

      wakizashi = AddMeleeWeapon("wakizashi", I =>
      {
        I.Description = "Like a smaller variant of the katana, often used as a side-arm.";
        I.SetAppearance("short samurai sword", null);
        I.Glyph = Glyphs.wakizashi;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(40);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Slash, 1.d7());
      });

      katana = AddMeleeWeapon("katana", I =>
      {
        I.Description = "This is a sharp and curved single-edged blade with a circular guard and long grip.";
        I.SetAppearance("samurai sword", null);
        I.Glyph = Glyphs.katana;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 4;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(80);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 1.d10());
      });

      tsurugi = AddMeleeWeapon("tsurugi", I =>
      {
        I.Description = null;
        I.SetAppearance("long samurai sword", null);
        I.Glyph = Glyphs.tsurugi;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(600);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(100);
        I.AddObviousIngestUse(Motions.eat, 60, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 1.d16());
      });

      knife = AddMeleeWeapon("knife", I =>
      {
        I.Description = "A small cutting utensil made of metal with a wooden handle.";
        I.Glyph = Glyphs.knife;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 15;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(4);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, null, Elements.physical, DamageType.Pierce, 1.d3());
        I.AddObviousIngestUse(Motions.eat, 5, Delay.FromTurns(10), Sonics.weapon);
      });

      main_gauche = AddMeleeWeapon("main gauche", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.main_gauche;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(4);
        I.SetArmour(Skills.light_armour, 1);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, null, Elements.physical, DamageType.Pierce, 1.d2());
        I.AddObviousIngestUse(Motions.eat, 5, Delay.FromTurns(10), Sonics.weapon);
      });

      lance = AddReachWeapon("lance", I =>
      {
        I.Description = "A long thrusting polearm best suited to mounted combat.";
        I.Glyph = Glyphs.lance;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1800);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 180, Delay.FromTurns(40), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedMomentumWeapon(Skills.lance, null, Elements.physical, DamageType.Pierce, 1.d6());
      });

      long_sword = AddMeleeWeapon("long sword", I =>
      {
        I.Description = "A straight, double-edged, and often cruciform blade.";
        I.Glyph = Glyphs.long_sword;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 50;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(15);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 1.d8());
      });

      lucern_hammer = AddReachWeapon("lucern hammer", I =>
      {
        I.Description = "This hammer is a four-pronged head mounted atop a long polearm stick. It bears a long spike on its reverse and an even longer spike extending from the very top. It proves effective at puncturing or smashing armour as well as dismounting riders.";
        I.SetAppearance("pronged polearm", null);
        I.Glyph = Glyphs.lucern_hammer;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1500);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(7);
        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Bludgeon, 2.d4());
      });

      mace = AddMeleeWeapon("mace", I =>
      {
        I.Description = "A sophisticated bludgeon with a heavy iron head.";
        I.Glyph = Glyphs.mace;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 40;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(5);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.mace, null, Elements.physical, DamageType.Bludgeon, 1.d6() + 1);
      });

      morning_star = AddMeleeWeapon("morning star", I =>
      {
        I.Description = "A mace that is spiked on its head.";
        I.Glyph = Glyphs.morning_star;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 12;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1200);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 120, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.mace, null, Elements.physical, DamageType.Bludgeon, 2.d4());
      });

      orcish_arrow = AddRangedMissile(Ammunition.Arrow, "orcish arrow", I =>
      {
        I.Description = null;
        I.SetAppearance("crude arrow", null);
        I.Glyph = Glyphs.orcish_arrow;
        I.Sonic = Sonics.ammo;
        I.OriginRace = Races.orc;
        I.Series = null;
        I.Rarity = 15;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.iron;
        I.Essence = AmmoEssence0;
        I.Price = Gold.FromCoins(2);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.ammo);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.bow, null, Elements.physical, DamageType.Pierce, 1.d5());
      });

      orcish_bow = AddRangedWeapon(Ammunition.Arrow, "orcish bow", I =>
      {
        I.Description = null;
        I.SetAppearance("crude bow", null);
        I.Glyph = Glyphs.orcish_bow;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.orc;
        I.Series = null;
        I.Rarity = 12;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(60);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.bow, Sonics.bow_fire, Elements.physical, DamageType.Bludgeon, 1.d4());
      });

      orcish_dagger = AddThrownWeapon("orcish dagger", I =>
      {
        I.Description = "This crudely made blade is still an effective throwing weapon.";
        I.SetAppearance("crude dagger", null);
        I.Glyph = Glyphs.orcish_dagger;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.orc;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(4);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.weapon);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, Sonics.throw_object, Elements.physical, DamageType.Pierce, 1.d3());
      });

      orcish_short_sword = AddMeleeWeapon("orcish short sword", I =>
      {
        I.Description = null;
        I.SetAppearance("crude short sword", null);
        I.Glyph = Glyphs.orcish_short_sword;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.orc;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Pierce, 1.d5());
      });

      orcish_spear = AddReachWeapon("orcish spear", I =>
      {
        I.Description = null;
        I.SetAppearance("crude spear", null);
        I.Glyph = Glyphs.orcish_spear;
        I.Sonic = Sonics.weapon;
        I.OriginRace = Races.orc;
        I.Series = null;
        I.Rarity = 13;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(350);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(3);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.spear, null, Elements.physical, DamageType.Pierce, 1.d5());
      });

      partisan = AddReachWeapon("partisan", I =>
      {
        I.Description = "This weapon consists of a spearhead mounted on a long wooden shaft with protrusions on the sides which aided in parrying sword thrusts.";
        I.SetAppearance("vulgar polearm", null);
        I.Glyph = Glyphs.partisan;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(800);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 80, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Pierce, 1.d6());
      });

      quarterstaff = AddMeleeWeapon("quarterstaff", I =>
      {
        I.Description = "A hardwood pole. It is an unassuming but formidable weapon in trained hands.";
        I.SetAppearance("staff", null);
        I.Glyph = Glyphs.quarterstaff;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 11;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(5);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.staff, null, Elements.physical, DamageType.Bludgeon, 1.d6());
      });

      dread_staff = AddMeleeWeapon("dread staff", I =>
      {
        I.Description = null;
        I.SetAppearance("bone staff", null);
        I.Glyph = Glyphs.bone_staff;
        I.Sonic = Sonics.weapon;
        I.Series = StaffSeries;
        I.Rarity = 4;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.bone;
        I.Essence = WeaponEssence4;
        I.Price = Gold.FromCoins(500);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.staff, null, Elements.physical, DamageType.Bludgeon, 1.d5(), A =>
        {
          A.WhenChance(Chance.OneIn10, T => T.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.fear, 4.d4() + 4),
            U => U.ApplyTransient(Properties.fear, 4.d4()),
            C => C.Backfire(TT => TT.ApplyTransient(Properties.fear, 4.d4()))
          ));
        });
      });

      flash_staff = AddMeleeWeapon("flash staff", I =>
      {
        I.Description = null;
        I.SetAppearance("mithril staff", null);
        I.Glyph = Glyphs.mithril_staff;
        I.Sonic = Sonics.weapon;
        I.Series = StaffSeries;
        I.Rarity = 2;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.mithril;
        I.Essence = WeaponEssence4;
        I.Price = Gold.FromCoins(500);
        I.AddObviousIngestUse(Motions.eat, 160, Delay.FromTurns(40), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.staff, null, Elements.physical, DamageType.Bludgeon, 1.d6() + 1, A =>
        {
          A.WhenChance(Chance.OneIn10, T => T.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.blindness, 4.d4() + 4),
            U => U.ApplyTransient(Properties.blindness, 4.d4()),
            C => C.Backfire(TT => TT.ApplyTransient(Properties.blindness, 4.d4()))
          ));
        });
      });

      thunder_staff = AddMeleeWeapon("thunder staff", I =>
      {
        I.Description = null;
        I.SetAppearance("crystal staff", null);
        I.Glyph = Glyphs.crystal_staff;
        I.Sonic = Sonics.weapon;
        I.Series = StaffSeries;
        I.Rarity = 4;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(800);
        I.Material = Materials.crystal;
        I.Essence = WeaponEssence4;
        I.Price = Gold.FromCoins(500);
        I.AddObviousIngestUse(Motions.eat, 80, Delay.FromTurns(40), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.staff, null, Elements.physical, DamageType.Bludgeon, 1.d7(), A =>
        {
          A.WhenChance(Chance.OneIn10, T => T.WithSourceSanctity
          (
            B => B.ApplyTransient(Properties.stunned, 4.d4() + 4),
            U => U.ApplyTransient(Properties.stunned, 4.d4()),
            C => C.Backfire(TT => TT.ApplyTransient(Properties.stunned, 4.d4()))
          ));
        });
      });

      ranseur = AddReachWeapon("ranseur", I =>
      {
        I.Description = "The head of a ranseur consists of a spear-tip affixed with a cross hilt at its base.";
        I.SetAppearance("hilted polearm", null);
        I.Glyph = Glyphs.ranseur;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(6);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Pierce, 2.d4());
      });

      rapier = AddMeleeWeapon("rapier", I =>
      {
        I.Description = "A thin and agile sword meant almost exclusively for thrusting.";
        I.Glyph = Glyphs.rapier;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(40);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Pierce, 1.d6());
      });

      scalpel = AddMeleeWeapon("scalpel", I =>
      {
        I.Description = "An extremely sharp little blade used for precise incisions.";
        I.Glyph = Glyphs.scalpel;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(4);
        I.AddObviousIngestUse(Motions.eat, 5, Delay.FromTurns(10), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, null, Elements.physical, DamageType.Slash, 1.d3());
      });

      scimitar = AddMeleeWeapon("scimitar", I =>
      {
        I.Description = "This is a sword weapon with a long curved design.";
        I.SetAppearance("curved sword", null);
        I.Glyph = Glyphs.scimitar;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 15;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(15);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 1.d8());
      });

      scythe = AddReachWeapon("scythe", I =>
      {
        I.Description = "Agricultural implement consisting of a long curving blade and fastened to an angled handle.";
        I.SetAppearance("curved polearm", null);
        I.Glyph = Glyphs.scythe;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(750);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(6);
        I.AddObviousIngestUse(Motions.eat, 75, Delay.FromTurns(25), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        var W = I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Slash, 2.d4());
        W.AttackModifier = Modifier.Minus2;
        W.AddVersus(new[] { Materials.vegetable }, Elements.physical, 2.d4());
      });

      sickle = AddMeleeWeapon("sickle", I =>
      {
        I.Description = null;
        I.Appearance = null;
        I.Glyph = Glyphs.sickle;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(250);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(3);
        I.AddObviousIngestUse(Motions.eat, 25, Delay.FromTurns(15), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        var W = I.SetOneHandedWeapon(Skills.light_blade, null, Elements.physical, DamageType.Slash, 1.d4() + 1);
        W.AttackModifier = Modifier.Minus1;
        W.AddVersus(new[] { Materials.vegetable }, Elements.physical, 1.d4());
      });

      short_sword = AddMeleeWeapon("short sword", I =>
      {
        I.Description = "A short and light double-edged blade.";
        I.Glyph = Glyphs.short_sword;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 8;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Pierce, 1.d6());
      });

      shuriken = AddThrownWeapon("shuriken", I =>
      {
        I.Description = "A sharp metal throwing weapon, smaller than a hand and shaped like a star.";
        I.SetAppearance("throwing star", null);
        I.Glyph = Glyphs.shuriken;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 35;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(5);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.weapon);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.disc, Sonics.throw_object, Elements.physical, DamageType.Pierce, 1.d6());
      });

      chakram = AddThrownWeapon("chakram", I =>
      {
        I.Description = null;
        I.SetAppearance("throwing disc", null);
        I.Glyph = Glyphs.chakram;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 15;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.weapon);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d3() + 3;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.disc, Sonics.throw_object, Elements.physical, DamageType.Slash, 1.d8());
      });

      mithril_arrow = AddRangedMissile(Ammunition.Arrow, "mithril arrow", I =>
      {
        I.Description = null;
        //I.SetAppearance("mithril shaft", null);
        I.Glyph = Glyphs.mithril_arrow;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(5);
        I.Material = Materials.mithril;
        I.Essence = AmmoEssence2;
        I.Price = Gold.FromCoins(20);
        I.AddObviousIngestUse(Motions.eat, 2, Delay.FromTurns(10), Sonics.ammo);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.bow, null, Elements.physical, DamageType.Pierce, 1.d6() + 1);
      });

      mithril_dagger = AddThrownWeapon("mithril dagger", I =>
      {
        I.Description = null;
        //I.SetAppearance("small mithril blade", null);
        I.Glyph = Glyphs.mithril_dagger;
        I.Sonic = Sonics.weapon;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.mithril;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(40);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(10), Sonics.weapon);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, Sonics.throw_object, Elements.physical, DamageType.Pierce, 1.d4() + 1);
      });

      mithril_long_sword = AddMeleeWeapon("mithril long sword", I =>
      {
        I.Description = null;
        //I.SetAppearance("long mithril blade", null);
        I.Glyph = Glyphs.mithril_long_sword;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.mithril;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 100, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 1.d8() + 1);
      });

      mithril_short_sword = AddMeleeWeapon("mithril short sword", I =>
      {
        I.Description = null;
        //I.SetAppearance("short mithril blade", null);
        I.Glyph = Glyphs.mithril_short_sword;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(150);
        I.Material = Materials.mithril;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(100);
        I.AddObviousIngestUse(Motions.eat, 60, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Pierce, 1.d6() + 1);
      });

      mithril_battleaxe = AddMeleeWeapon("mithril battle-axe", I =>
      {
        I.Description = null;
        //I.SetAppearance("large mithril axe", null);
        I.Glyph = Glyphs.mithril_battleaxe;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(600);
        I.Material = Materials.mithril;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(400);
        I.AddObviousIngestUse(Motions.eat, 240, Delay.FromTurns(40), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.axe, null, Elements.physical, DamageType.Slash, 2.d6() + 1);
      });

      mithril_lance = AddReachWeapon("mithril lance", I =>
      {
        I.Description = null;
        //I.SetAppearance("mithril pole-arm", null);
        I.Glyph = Glyphs.mithril_lance;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(900);
        I.Material = Materials.mithril;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(100);
        I.AddObviousIngestUse(Motions.eat, 360, Delay.FromTurns(40), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedMomentumWeapon(Skills.lance, null, Elements.physical, DamageType.Pierce, 1.d6() + 1);
      });

      mithril_sabre = AddMeleeWeapon("mithril sabre", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.mithril_sabre;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.mithril;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Slash, 1.d6() + 2);
      });

      silver_arrow = AddRangedMissile(Ammunition.Arrow, "silver arrow", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.silver_arrow;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(12);
        I.Material = Materials.silver;
        I.Essence = AmmoEssence1;
        I.Price = Gold.FromCoins(10);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.ammo);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.bow, null, Elements.physical, DamageType.Pierce, 1.d6());
      });

      silver_dagger = AddThrownWeapon("silver dagger", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.silver_dagger;
        I.Sonic = Sonics.weapon;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(120);
        I.Material = Materials.silver;
        I.Essence = WeaponEssence2;
        I.Price = Gold.FromCoins(20);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(10), Sonics.weapon);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, Sonics.throw_object, Elements.physical, DamageType.Pierce, 1.d4());
      });

      silver_long_sword = AddMeleeWeapon("silver long sword", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.silver_long_sword;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(480);
        I.Material = Materials.silver;
        I.Essence = WeaponEssence2;
        I.Price = Gold.FromCoins(75);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 1.d8());
      });

      silver_mace = AddMeleeWeapon("silver mace", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.silver_mace;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(360);
        I.Material = Materials.silver;
        I.Essence = WeaponEssence2;
        I.Price = Gold.FromCoins(25);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.mace, null, Elements.physical, DamageType.Bludgeon, 1.d6());
      });

      silver_sabre = AddMeleeWeapon("silver sabre", I =>
      {
        I.Description = "A curved single-edged blade often used by cavalry, especially effective against creatures of the netherworld.";
        I.Glyph = Glyphs.silver_sabre;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 27;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.silver;
        I.Essence = WeaponEssence2;
        I.Price = Gold.FromCoins(75);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Slash, 1.d6() + 1);
      });

      silver_short_sword = AddMeleeWeapon("silver short sword", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.silver_short_sword;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(360);
        I.Material = Materials.silver;
        I.Essence = WeaponEssence2;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.medium_blade, null, Elements.physical, DamageType.Pierce, 1.d6());
      });

      silver_spear = AddReachWeapon("silver spear", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.silver_spear;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(420);
        I.Material = Materials.silver;
        I.Essence = WeaponEssence2;
        I.Price = Gold.FromCoins(15);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.spear, null, Elements.physical, DamageType.Pierce, 1.d6());
      });

      silver_lance = AddReachWeapon("silver lance", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.silver_lance;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(2160);
        I.Material = Materials.silver;
        I.Essence = WeaponEssence2;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 180, Delay.FromTurns(40), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedMomentumWeapon(Skills.lance, null, Elements.physical, DamageType.Pierce, 1.d6());
      });

      blowgun = AddRangedWeapon(Ammunition.Dart, "blowgun", I =>
      {
        I.Description = "A long pipe meant for shooting darts through force of breath.";
        I.Glyph = Glyphs.blowgun;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 20;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(20);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.dart, Sonics.throw_object, Elements.physical, DamageType.Bludgeon, Dice.One).AttackModifier = Modifier.Plus2;
        I.AddObviousIngestUse(Motions.eat, 3, Delay.FromTurns(10), Sonics.weapon);
      });

      sling = AddRangedWeapon(Ammunition.Pellet, "sling", I =>
      {
        I.Description = "A leather pouch set between two cords, meant for launching small projectiles.";
        I.Glyph = Glyphs.sling;
        I.Sonic = Sonics.leather;
        I.Series = null;
        I.Rarity = 40;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(30);
        I.Material = Materials.leather;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(20);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.leather);
        I.SetOneHandedWeapon(Skills.sling, Sonics.sling_shot, Elements.physical, DamageType.Bludgeon, Dice.One);
        I.AddObviousIngestUse(Motions.eat, 3, Delay.FromTurns(10), Sonics.leather);
      });

      spear = AddReachWeapon("spear", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.spear;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 50;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(350);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(3);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.spear, null, Elements.physical, DamageType.Pierce, 1.d6());
      });

      mithril_spear = AddReachWeapon("mithril spear", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.mithril_spear;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(175);
        I.Material = Materials.mithril;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(30);
        I.AddObviousIngestUse(Motions.eat, 60, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.spear, null, Elements.physical, DamageType.Pierce, 1.d6() + 1);
      });

      spetum = AddReachWeapon("spetum", I =>
      {
        I.Description = "This weapon consists of a long pole with a mounted a spear head and two projections at its base.";
        I.SetAppearance("forked polearm", null);
        I.Glyph = Glyphs.spetum;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 3;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(5);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Pierce, 1.d6() + 1);
      });

      stiletto = AddMeleeWeapon("stiletto", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.stiletto;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 5;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(4);
        I.AddObviousIngestUse(Motions.eat, 5, Delay.FromTurns(10), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, null, Elements.physical, DamageType.Pierce, 1.d3());
      });

      // NOTE: the origins are well explained here https://en.wikipedia.org/wiki/Trident
      trident = AddReachWeapon("trident", I =>
      {
        I.Description = null;
        I.SetAppearance("three-pointed polearm", null);
        I.Glyph = Glyphs.trident;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(250);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(5);
        I.AddObviousIngestUse(Motions.eat, 25, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Pierce, 1.d6() + 1);
      });

      twohanded_sword = AddMeleeWeapon("two-handed sword", I =>
      {
        I.Description = "A heavy and long double-edged blade. Similar to a long sword, scaled-up specifically for two-handed usage.";
        I.Glyph = Glyphs.twohanded_sword;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 25;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(800);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(50);
        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 1.d12());
      });

      silver_twohanded_sword = AddMeleeWeapon("silver two-handed sword", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.silver_twohanded_sword;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(960);
        I.Material = Materials.silver;
        I.Essence = WeaponEssence2;
        I.Price = Gold.FromCoins(250);
        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 1.d12());
      });

      mithril_twohanded_sword = AddMeleeWeapon("mithril two-handed sword", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.mithril_twohanded_sword;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.mithril;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(500);
        I.AddObviousIngestUse(Motions.eat, 150, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.heavy_blade, null, Elements.physical, DamageType.Slash, 1.d12() + 1);
      });

      voulge = AddReachWeapon("voulge", I =>
      {
        I.Description = "This weapon is a curved blade attached to a pole by binding the lower two-thirds of the blade to the side of the pole, to form a sort of axe.";
        I.SetAppearance("pole cleaver", null);
        I.Glyph = Glyphs.voulge;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 2;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1250);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(5);
        I.AddObviousIngestUse(Motions.eat, 125, Delay.FromTurns(30), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedMomentumWeapon(Skills.polearm, null, Elements.physical, DamageType.Slash, 2.d4());
      });

      war_hammer = AddMeleeWeapon("war hammer", I =>
      {
        I.Description = "A heavy iron hammer designed specifically for use in melee combat.";
        I.Glyph = Glyphs.war_hammer;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 25;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(5);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.hammer, null, Elements.physical, DamageType.Bludgeon, 1.d4() + 1);
      });

      worm_tooth = AddMeleeWeapon("worm tooth", I =>
      {
        I.Description = null;
        I.Glyph = Glyphs.worm_tooth;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(200);
        I.Material = Materials.bone;
        I.Essence = WeaponEssence1;
        I.Price = Gold.FromCoins(2);
        I.SetDerivative(Entities.long_worm);
        I.AddObviousIngestUse(Motions.eat, 20, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetOneHandedWeapon(Skills.light_blade, null, Elements.physical, DamageType.Pierce, 1.d2());
      });

      ya = AddRangedMissile(Ammunition.Arrow, "ya", I =>
      {
        I.Description = "A long-shafted arrow with a distinctive tip.";
        I.SetAppearance("bamboo arrow", null);
        I.Glyph = Glyphs.ya;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 10;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.iron;
        I.Essence = AmmoEssence0;
        I.Price = Gold.FromCoins(4);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.ammo);
        I.SetWeakness(AmmoWeakness);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.bow, null, Elements.physical, DamageType.Pierce, 1.d7());
      });

      yumi = AddRangedWeapon(Ammunition.Arrow, "yumi", I =>
      {
        I.Description = "A tall, asymmetric bow.";
        I.SetAppearance("long bow", null);
        I.Glyph = Glyphs.yumi;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.wood;
        I.Essence = WeaponEssence0;
        I.Price = Gold.FromCoins(60);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);
        I.SetTwoHandedWeapon(Skills.bow, Sonics.bow_fire, Elements.physical, DamageType.Bludgeon, Dice.One);
        // TODO: difference between bows.
      });

      Harbalest = AddRangedWeapon(Ammunition.Bolt, "Harbalest", I =>
      {
        I.Description = null;
        I.Appearance = null;
        I.Glyph = Glyphs.Harbalest;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(350);
        I.Material = Materials.mithril;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1350);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon)
         .SetTalent(Properties.invisibility, Properties.stealth)
         .SetResistance(Elements.magical);
        var W = I.SetTwoHandedWeapon(Skills.crossbow, Sonics.bow_fire, Elements.physical, DamageType.Bludgeon, 1.d6());
        W.BurstRate = 2;
        W.AttackModifier = Modifier.Plus2;
      });

      Ravenbow = AddRangedWeapon(Ammunition.Arrow, "Ravenbow", I =>
      {
        I.Description = null;
        I.Appearance = null;
        I.Glyph = Glyphs.Ravenbow;
        I.Sonic = Sonics.weapon;
        I.Artifact = true;
        I.Series = null;
        I.Rarity = 1;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.wood;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(1200);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon)
         .SetTalent(Properties.telepathy, Properties.see_invisible, Properties.searching);
        var W = I.SetTwoHandedWeapon(Skills.bow, Sonics.bow_fire, Elements.physical, DamageType.Bludgeon, 1.d6());
        W.BurstRate = 2;
      });

      Creeping_Sprout = AddRangedMissile(Ammunition.Arrow, "Creeping Sprout", I =>
      {
        I.Description = null;
        I.Appearance = null;
        I.Glyph = Glyphs.Creeping_Sprout;
        I.Sonic = Sonics.ammo;
        I.Artifact = true;
        I.BundleDice = Dice.One;
        I.Series = null;
        I.InfiniteQuantity = true; // infinite ammo.
        I.Rarity = 1;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(15);
        I.Material = Materials.wood;
        I.Essence = ArtifactEssence;
        I.Price = Gold.FromCoins(600);
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo)
         .SetResistance(Elements.sleep)
         .SetTalent(Properties.warning);
        I.SetOneHandedWeapon(Skills.bow, null, Elements.physical, DamageType.Pierce, 1.d6()); // artifacts can go to +10.
      });

      SetUpgradeDowngradePair(rubber_hose, bullwhip);
      
      SetUpgradeDowngradePair(worm_tooth, crysknife);
      #endregion

      #region guns.
      rocket_launcher = AddRangedWeapon(Ammunition.Rocket, "rocket launcher", I =>
      {
        I.Description = null;
        I.SetAppearance("shoulder gun", null);
        I.Glyph = Glyphs.rocket_launcher;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(1500);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(600);
        I.AddObviousIngestUse(Motions.eat, 750, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);

        var W = I.SetTwoHandedWeapon(Skills.firearms, Sonics.launch_rocket, Elements.physical, DamageType.Bludgeon, 1.d4());
        W.FixedRange = 20;
        W.AttackModifier = Modifier.Minus4;
        W.AttackDelay = Delay.FromTurns(+5);
      });

      rocket = AddRangedMissile(Ammunition.Rocket, "rocket", I =>
      {
        I.Description = null;
        I.SetAppearance("winged missile", null);
        I.Glyph = Glyphs.rocket;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence2;
        I.Price = Gold.FromCoins(200);
        I.AddObviousIngestUse(Motions.eat, 200, Delay.FromTurns(10), Sonics.ammo);
        I.BundleDice = 1.d3();
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        var W = I.SetOneHandedWeapon(Skills.firearms, null, Elements.physical, DamageType.Bludgeon, Dice.Zero);
        W.AddDetonation(Explosions.fiery, A =>
        {
          A.Harm(Elements.force, 1.d60());
          A.ApplyTransient(Properties.stunned, 1.d12() + 4);
        });
      });

      grenade_launcher = AddRangedWeapon(Ammunition.Grenade, "grenade launcher", I =>
      {
        I.Description = null;
        I.SetAppearance("mortar gun", null);
        I.Glyph = Glyphs.grenade_launcher;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(600);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(400);
        I.AddObviousIngestUse(Motions.eat, 300, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);

        var W = I.SetTwoHandedWeapon(Skills.firearms, Sonics.launch_grenade, Elements.physical, DamageType.Bludgeon, 1.d4());
        W.FixedRange = 6;
        W.AttackModifier = Modifier.Minus3;
        W.AttackDelay = Delay.FromTurns(+3);
      });

      frag_grenade = AddThrownMissile(Ammunition.Grenade, "frag grenade", I =>
      {
        I.Description = null;
        I.SetAppearance("metal pineapple", null);
        I.Glyph = Glyphs.frag_grenade;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence2;
        I.Price = Gold.FromCoins(100);
        I.AddObviousIngestUse(Motions.eat, 140, Delay.FromTurns(10), Sonics.ammo);
        I.BundleDice = 1.d3();
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);

        var W = I.SetOneHandedWeapon(Skills.firearms, Sonics.throw_grenade, Elements.physical, DamageType.Pierce, Dice.Zero);
        W.AddDetonation(Explosions.fiery, A =>
        {
          A.Harm(Elements.physical, 1.d45());
          A.ApplyTransient(Properties.stunned, 5.d6() + 10);
        });
      });

      gas_grenade = AddThrownMissile(Ammunition.Grenade, "gas grenade", I =>
      {
        I.Description = null;
        I.SetAppearance("metal canister", null);
        I.Glyph = Glyphs.gas_grenade;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(50);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence2;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 140, Delay.FromTurns(10), Sonics.ammo);
        I.BundleDice = 1.d3();
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        var W = I.SetOneHandedWeapon(Skills.firearms, Sonics.throw_grenade, Elements.acid, DamageType.Bludgeon, Dice.Zero);
        W.AddDetonation(Explosions.acid, A =>
        {
          A.ApplyTransient(Properties.paralysis, 10.d12() + 20);
          A.Harm(Elements.acid, 1.d20());
        });
      });

      stick_of_dynamite = AddThrownWeapon("stick of dynamite", I =>
      {
        I.Description = "A stick of highly explosive material, with a short fuse on one end.";
        I.SetAppearance("plastic stick", null);
        I.Glyph = Glyphs.stick_of_dynamite;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.plastic;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(300);
        I.BundleDice = 1.d3();
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        var W = I.SetOneHandedWeapon(Skills.firearms, Sonics.lit_fuse, Elements.physical, DamageType.Bludgeon, Dice.Zero);
        W.AddDetonation(Explosions.fiery, A =>
        {
          A.ApplyTransient(Properties.stunned, 1.d6() + 2);
          A.Harm(Elements.force, 1.d100());
          A.CreateTrap(Codex.Devices.pit, Destruction: true);
        });
        //I.AddEat(10, Delay.FromTurns(10), Sonics.ammo); // plastic, no one can eat it.
      });

      hunting_rifle = AddRangedWeapon(Ammunition.Bullet, "hunting rifle", I =>
      {
        I.Description = null;
        I.SetAppearance("long gun", null);
        I.Glyph = Glyphs.hunting_rifle;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);

        var W = I.SetTwoHandedWeapon(Skills.firearms, Sonics.rifle_shot, Elements.physical, DamageType.Bludgeon, 1.d4());
        W.FixedRange = 22;
        W.BurstRate = 0;
        W.AttackDelay = Delay.FromTurns(+1);
        W.AttackModifier = Modifier.Plus1;
      });

      sniper_rifle = AddRangedWeapon(Ammunition.Bullet, "sniper rifle", I =>
      {
        I.Description = null;
        I.SetAppearance("scoped long gun", null);
        I.Glyph = Glyphs.sniper_rifle;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(500);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(800);
        I.AddObviousIngestUse(Motions.eat, 50, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);

        var W = I.SetTwoHandedWeapon(Skills.firearms, Sonics.rifle_shot, Elements.physical, DamageType.Bludgeon, 1.d4());
        W.FixedRange = 25;
        W.BurstRate = 0;
        W.AttackModifier = Modifier.Plus4;
        W.AttackDelay = Delay.FromTurns(+3);
      });

      assault_rifle = AddRangedWeapon(Ammunition.Bullet, "assault rifle", I =>
      {
        I.Description = null;
        I.SetAppearance("large gun", null);
        I.Glyph = Glyphs.assault_rifle;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(400);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(350);
        I.AddObviousIngestUse(Motions.eat, 40, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);

        var W = I.SetTwoHandedWeapon(Skills.firearms, Sonics.rifle_burst, Elements.physical, DamageType.Bludgeon, 1.d4());
        W.FixedRange = 20;
        W.BurstRate = 5;
        W.AttackModifier = Modifier.Minus2;
        W.AttackDelay = Delay.FromTurns(-3);
      });

      submachine_gun = AddRangedWeapon(Ammunition.Bullet, "submachine gun", I =>
      {
        I.Description = null;
        I.SetAppearance("compact gun", null);
        I.Glyph = Glyphs.submachine_gun;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(250);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(200);
        I.AddObviousIngestUse(Motions.eat, 25, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);

        var W = I.SetOneHandedWeapon(Skills.firearms, Sonics.rifle_burst, Elements.physical, DamageType.Bludgeon, 1.d4());
        W.FixedRange = 10;
        W.BurstRate = 3;
        W.AttackModifier = Modifier.Minus1;
        W.AttackDelay = Delay.FromTurns(+3);
      });

      heavy_machine_gun = AddRangedWeapon(Ammunition.Bullet, "heavy machine gun", I =>
      {
        I.Description = null;
        I.SetAppearance("heavy gun", null);
        I.Glyph = Glyphs.heavy_machine_gun;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(5000);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(500);
        I.AddObviousIngestUse(Motions.eat, 500, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);

        var W = I.SetTwoHandedWeapon(Skills.firearms, Sonics.rifle_burst, Elements.physical, DamageType.Bludgeon, 1.d4());
        W.FixedRange = 20;
        W.BurstRate = 8;
        W.AttackModifier = Modifier.Minus4;
      });

      shotgun = AddRangedWeapon(Ammunition.Shell, "shotgun", I =>
      {
        I.Description = "A two-handed firearm, steadied at the shoulder, often used in small game hunting. It packs quite a punch.";
        I.SetAppearance("long barrelled gun", null);
        I.Glyph = Glyphs.shotgun;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(350);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(200);
        I.AddObviousIngestUse(Motions.eat, 35, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);

        var W = I.SetTwoHandedWeapon(Skills.firearms, Sonics.shotgun_blast, Elements.physical, DamageType.Bludgeon, 1.d4());
        W.FixedRange = 6;
        W.AttackDelay = Delay.FromTurns(+1);
        W.AttackModifier = Modifier.Plus3;
      });

      auto_shotgun = AddRangedWeapon(Ammunition.Shell, "auto shotgun", I =>
      {
        I.Description = null;
        I.SetAppearance("heavy barrelled gun", null);
        I.Glyph = Glyphs.auto_shotgun;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Large;
        I.Weight = Weight.FromUnits(350);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(350);
        I.AddObviousIngestUse(Motions.eat, 35, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);

        var W = I.SetTwoHandedWeapon(Skills.firearms, Sonics.shotgun_burst, Elements.physical, DamageType.Bludgeon, 1.d4());
        W.FixedRange = 6;
        W.BurstRate = 2;
      });

      sawnoff_shotgun = AddRangedWeapon(Ammunition.Shell, "sawn-off shotgun", I =>
      {
        I.Description = null;
        I.SetAppearance("short barrelled gun", null);
        I.Glyph = Glyphs.sawnoff_shotgun;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Medium;
        I.Weight = Weight.FromUnits(300);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 30, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);

        var W = I.SetOneHandedWeapon(Skills.firearms, Sonics.shotgun_blast, Elements.physical, DamageType.Bludgeon, 1.d3());
        W.FixedRange = 5;
        W.AttackDelay = Delay.FromTurns(+1);
        W.AttackModifier = Modifier.Plus2;
      });

      shotgun_shell = AddRangedMissile(Ammunition.Shell, "shotgun shell", I =>
      {
        I.Description = null;
        I.SetAppearance("plastic slug", null);
        I.Glyph = Glyphs.shotgun_shell;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(10);
        I.Material = Materials.plastic;
        I.Essence = AmmoEssence1;
        I.Price = Gold.FromCoins(5);
        //I.AddEat(1, Delay.FromTurns(10), Sonics.ammo); // no diet can eat plastic.
        I.BundleDice = 1.d4() + 4;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.firearms, null, Elements.physical, DamageType.Pierce, 1.d30());
      });

      pistol = AddRangedWeapon(Ammunition.Bullet, "pistol", I =>
      {
        I.Description = "A compact hand-held firearm.";
        I.SetAppearance("small gun", null);
        I.Glyph = Glyphs.pistol;
        I.Sonic = Sonics.weapon;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Small;
        I.Weight = Weight.FromUnits(100);
        I.Material = Materials.iron;
        I.Essence = WeaponEssence3;
        I.Price = Gold.FromCoins(150);
        I.AddObviousIngestUse(Motions.eat, 10, Delay.FromTurns(20), Sonics.weapon);
        I.SetEquip(EquipAction.Wield, Delay.FromTurns(10), Sonics.weapon);

        var W = I.SetOneHandedWeapon(Skills.firearms, Sonics.pistol_fire, Elements.physical, DamageType.Bludgeon, 1.d2());
        W.FixedRange = 15;
      });

      bullet = AddRangedMissile(Ammunition.Bullet, "bullet", I =>
      {
        I.Description = "A pointed metal cylinder meant to be discharged from a firearm.";
        I.SetAppearance("metal slug", null);
        I.Glyph = Glyphs.bullet;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(5);
        I.Material = Materials.iron;
        I.Essence = AmmoEssence1;
        I.Price = Gold.FromCoins(5);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.ammo);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.firearms, null, Elements.physical, DamageType.Pierce, 1.d20());
      });

      silver_bullet = AddRangedMissile(Ammunition.Bullet, "silver bullet", I =>
      {
        I.Description = null;
        I.SetAppearance("silver slug", null);
        I.Glyph = Glyphs.silver_bullet;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(5);
        I.Material = Materials.silver;
        I.Essence = AmmoEssence2;
        I.Price = Gold.FromCoins(15);
        I.AddObviousIngestUse(Motions.eat, 1, Delay.FromTurns(10), Sonics.ammo);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.firearms, null, Elements.physical, DamageType.Pierce, 1.d20());
      });

      mithril_bullet = AddRangedMissile(Ammunition.Bullet, "mithril bullet", I =>
      {
        I.Description = null;
        I.SetAppearance("mithril slug", null);
        I.Glyph = Glyphs.mithril_bullet;
        I.Sonic = Sonics.ammo;
        I.Series = null;
        I.Rarity = 0;
        I.Size = Size.Tiny;
        I.Weight = Weight.FromUnits(3);
        I.Material = Materials.mithril;
        I.Essence = AmmoEssence3;
        I.Price = Gold.FromCoins(45);
        I.AddObviousIngestUse(Motions.eat, 2, Delay.FromTurns(10), Sonics.ammo);
        I.BundleDice = 1.d6() + 6;
        I.SetEquip(EquipAction.Ready, Delay.FromTurns(10), Sonics.ammo);
        I.SetOneHandedWeapon(Skills.firearms, null, Elements.physical, DamageType.Pierce, 1.d25());
      });

      // TODO: we need to choose between enchantment modifiers and upgrades somehow?
      // - firearms can upgrade/downgrade with enchantment: sawn off -> shotgun -> autoshotgun; grenade launcher -> rocket launcher; hunting rifle -> sniper rifle; submachine gun -> assault rifle -> heavy machine gun.

      //hunting_rifle.SetUpgradeItem(sniper_rifle;
      //sniper_rifle.SetDowngradeItem(hunting_rifle;
      //
      //submachine_gun.SetUpgradeItem(assault_rifle;
      //assault_rifle.SetDowngradeItem(submachine_gun;
      //assault_rifle.SetUpgradeItem(heavy_machine_gun;
      //heavy_machine_gun.SetDowngradeItem(assault_rifle;
      //
      //sawn_off_shotgun.SetUpgradeItem(shotgun;
      //shotgun.SetDowngradeItem(sawn_off_shotgun;
      //shotgun.SetUpgradeItem(auto_shotgun;
      //auto_shotgun.SetDowngradeItem(shotgun;
      //
      //grenade_launcher.SetUpgradeItem(rocket_launcher;
      //rocket_launcher.SetDowngradeItem(grenade_launcher;
      #endregion

      CodexRecruiter.Enrol(() =>
      {
        void CheckWeaponVariant(Item Iron, Item Silver, Item Mithril)
        {
          Debug.Assert(Iron != null);

          Debug.Assert(Iron.Material == Materials.iron, Iron.Name);

          if (Iron.IsRangedMissile())
            Debug.Assert(Iron.Essence == AmmoEssence0, Iron.Name);
          else
            Debug.Assert(Iron.Essence == WeaponEssence0, Iron.Name);

          if (Silver != null)
          {
            Debug.Assert(Silver.Name == "silver " + Iron.Name, Silver.Name);
            Debug.Assert(Silver.Material == Materials.silver, Silver.Name);
            Debug.Assert(Silver.Rarity == 2, Silver.Name);

            if (Silver.IsRangedMissile())
              Debug.Assert(Silver.Essence == AmmoEssence1, Silver.Name);
            else
              Debug.Assert(Silver.Essence == WeaponEssence2, Silver.Name);

            Debug.Assert((int)(Iron.Weight.GetUnits() * 1.2F) == Silver.Weight.GetUnits(), Silver.Name);
            Debug.Assert(Iron.Price.GetCoins() * 5 == Silver.Price.GetCoins(), Silver.Name);
          }

          if (Mithril != null)
          {
            Debug.Assert(Mithril.Name == "mithril " + Iron.Name, Mithril.Name);
            Debug.Assert(Mithril.Material == Materials.mithril, Mithril.Name);
            Debug.Assert(Mithril.Rarity == 1, Mithril.Name);
            Debug.Assert(Mithril.Weight < Iron.Weight, Mithril.Name);

            if (Mithril.IsRangedMissile())
              Debug.Assert(Mithril.Essence == AmmoEssence2, Mithril.Name);
            else
              Debug.Assert(Mithril.Essence == WeaponEssence3, Mithril.Name);

            Debug.Assert((int)(Iron.Weight.GetUnits() * 0.5F) == Mithril.Weight.GetUnits(), Mithril.Name);
            Debug.Assert(Iron.Price.GetCoins() * 10 == Mithril.Price.GetCoins(), Mithril.Name);
          }
        }

        CheckWeaponVariant(long_sword, silver_long_sword, mithril_long_sword);
        CheckWeaponVariant(twohanded_sword, silver_twohanded_sword, mithril_twohanded_sword);
        CheckWeaponVariant(dagger, silver_dagger, mithril_dagger);
        CheckWeaponVariant(spear, silver_spear, mithril_spear);
        CheckWeaponVariant(lance, silver_lance, mithril_lance);
        CheckWeaponVariant(short_sword, silver_short_sword, mithril_short_sword);
        CheckWeaponVariant(arrow, silver_arrow, mithril_arrow);
        CheckWeaponVariant(crossbow_bolt, silver_crossbow_bolt, mithril_crossbow_bolt);
        //CheckWeaponVariant(bullet, silver_bullet, mithril_bullet);
        CheckWeaponVariant(mace, silver_mace, Mithril: null);
        CheckWeaponVariant(heavy_hammer, silver_heavy_hammer, Mithril: null);
        CheckWeaponVariant(axe, Silver: silver_axe, Mithril: null);
        CheckWeaponVariant(battleaxe, Silver: null, mithril_battleaxe);
        CheckWeaponVariant(katar, Silver: null, mithril_katar);
        //CheckWeaponVariant(Iron: null, silver_sabre, mithril_sabre);
      });

      CodexRecruiter.Enrol(() =>
      {
        Register.AddAbolitionReplacement(rocket_launcher, crossbow);
        Register.AddAbolitionReplacement(rocket, crossbow_bolt);
        Register.AddAbolitionReplacement(grenade_launcher, crossbow);
        Register.AddAbolitionReplacement(frag_grenade, crossbow_bolt);
        Register.AddAbolitionReplacement(gas_grenade, crossbow_bolt);
        Register.AddAbolitionReplacement(stick_of_dynamite, potion_of_paralysis);
        Register.AddAbolitionReplacement(hunting_rifle, bow);
        Register.AddAbolitionReplacement(assault_rifle, bow);
        Register.AddAbolitionReplacement(sniper_rifle, bow);
        Register.AddAbolitionReplacement(submachine_gun, bow);
        Register.AddAbolitionReplacement(heavy_machine_gun, bow);
        Register.AddAbolitionReplacement(shotgun, heavy_hammer);
        Register.AddAbolitionReplacement(auto_shotgun, lucern_hammer);
        Register.AddAbolitionReplacement(sawnoff_shotgun, war_hammer);
        Register.AddAbolitionReplacement(shotgun_shell, dagger);
        Register.AddAbolitionReplacement(pistol, bow);
        Register.AddAbolitionReplacement(bullet, arrow);
        Register.AddAbolitionReplacement(silver_bullet, silver_arrow);
        Register.AddAbolitionReplacement(mithril_bullet, mithril_arrow);

        var MissingAbolitionReplacementArray = List.Where(I => I.IsAbolitionCandidate() && !I.Artifact).Except(Register.AbolitionReplacements.Select(R => R.AbolitionItem)).ToArray();
        if (MissingAbolitionReplacementArray.Length > 0)
          throw new Exception("Abolition replacement not implemented:" + Environment.NewLine + MissingAbolitionReplacementArray.Select(I => I.Name).AsSeparatedText(Environment.NewLine));
      });

      #region Compatibility
      // replaced grey dragon with gold dragon patch.
      Register.Alias(gold_dragon_scales, "grey dragon scales");
      Register.Alias(gold_dragon_scale_mail, "grey dragon scale mail");

      // all artifacts need capitalised naming.
      Register.Alias(Stamped_Letter, "stamped letter");
      Register.Alias(Backpack, "backpack");
      Register.Alias(Silver_Key, "silver key");
      Register.Alias(Gold_Key, "gold key");
      Register.Alias(Jade_Key, "jade key");
      Register.Alias(Ruby_Key, "ruby key");
      Register.Alias(Shadow_Key, "shadow key");

      // animal/vegetable corpse patch.
      Register.Alias(animal_corpse, "corpse");

      // 'Runesword' is now an artifact, so any ordinary versions of it are translated into an elven broadsword.
      Register.Alias(elven_broadsword, "runesword");

      // greatsword was a mistake, two-handed sword for naming consistency.
      Register.Alias(mithril_twohanded_sword, "mithril greatsword");

      // simplicity of naming.
      Register.Alias(book_of_restoration, "book of restore ability");
      Register.Alias(potion_of_recovery, "potion of restore ability");

      // to allow 'magic bugle'.
      Register.Alias(brass_bugle, "bugle");

      // British English.
      Register.Alias(silver_sabre, "silver saber");
      Register.Alias(scroll_of_tranquillity, "scroll of tranquility");

      // one type of pear is plenty.
      Register.Alias(pear, "asian pear");

      // for consistency with other 'magic' tools.
      Register.Alias(magic_figurine, "figurine");

      // protection -> deflection patch.
      Register.Alias(cloak_of_deflection, "cloak of protection");
      Register.Alias(book_of_deflection, "book of protection");

      // mold -> mould spelling patch.
      Register.Alias(slime_mould, "slime mold");

      // oilskin sack had no distinct mechanics.
      Register.Alias(sack, "oilskin sack");

      // naming consistency.
      Register.Alias(book_of_lightning_bolt, "book of lightning");
      #endregion
    }
#endif

    public IReadOnlyList<Item> DragonScales => DragonScalesArmourList;

    // suits.
    public readonly Item banded_mail;
    public readonly Item bronze_plate_mail;
    public readonly Item chain_mail;
    public readonly Item crystal_plate_mail;
    public readonly Item mithril_plate_mail;
    public readonly Item leather_armour;
    public readonly Item leather_jacket;
    public readonly Item plate_mail;
    public readonly Item ring_mail;
    public readonly Item scale_mail;
    public readonly Item splint_mail;
    public readonly Item studded_leather_armour;

    // cloaks.
    public readonly Item leather_cloak;
    public readonly Item alchemy_smock;
    public readonly Item cloak_of_blinking;
    public readonly Item cloak_of_displacement;
    public readonly Item cloak_of_invisibility;
    public readonly Item cloak_of_magic_resistance;
    public readonly Item cloak_of_deflection;
    public readonly Item lab_coat;
    public readonly Item mummy_wrapping;
    public readonly Item oilskin_cloak;

    // robes.
    public readonly Item robe;
    public readonly Item battle_robe;
    public readonly Item fleet_robe;
    public readonly Item elemental_robe;
    public readonly Item hermit_robe;

    // shirts.
    public readonly Item tshirt;
    public readonly Item hawaiian_shirt;

    // helmets.
    public readonly Item cornuthaum;
    public readonly Item dunce_cap;
    public readonly Item dented_pot;
    public readonly Item fedora;
    public readonly Item helmet;
    public readonly Item helm_of_weakness;
    public readonly Item helm_of_brilliance;
    public readonly Item helm_of_telepathy;
    public readonly Item mithril_helmet;

    // boots.
    public readonly Item low_boots;
    public readonly Item high_boots;
    public readonly Item iron_shoes;
    public readonly Item disencumbrance_boots;
    public readonly Item fumble_boots;
    public readonly Item jumping_boots;
    public readonly Item levitation_boots;
    public readonly Item panic_boots;
    public readonly Item speed_boots;
    public readonly Item winged_boots;

    // gloves.
    public readonly Item gauntlets_of_dexterity;
    public readonly Item gauntlets_of_fumbling;
    public readonly Item gauntlets_of_phasing;
    public readonly Item gauntlets_of_power;
    public readonly Item leather_gloves;

    // shields.
    public readonly Item small_shield;
    public readonly Item large_shield;
    public readonly Item mithril_shield;
    public readonly Item shield_of_reflection;

    // glasses.
    public readonly Item lenses;
    public readonly Item shades;
    public readonly Item kaleidoscope_glasses;
    public readonly Item spectacles;

    // weapon.
    public readonly Item aklys;
    public readonly Item arrow;
    public readonly Item athame;
    public readonly Item axe;
    public readonly Item silver_axe;
    public readonly Item bardiche;
    public readonly Item battleaxe;
    public readonly Item bec_de_corbin;
    public readonly Item billguisarme;
    public readonly Item boomerang;
    public readonly Item horseshoe;
    public readonly Item blowgun;
    public readonly Item bow;
    public readonly Item brass_knuckles;
    public readonly Item broadsword;
    public readonly Item bullwhip;
    public readonly Item club;
    public readonly Item stone_club;
    public readonly Item war_club;
    public readonly Item crossbow;
    public readonly Item crossbow_bolt;
    public readonly Item crysknife;
    public readonly Item dagger;
    public readonly Item dart;
    public readonly Item mithril_dart;
    public readonly Item poison_dart;
    public readonly Item fauchard;
    public readonly Item flail;
    public readonly Item glaive;
    public readonly Item great_dagger;
    public readonly Item guisarme;
    public readonly Item halberd;
    public readonly Item hatchet;
    public readonly Item heavy_hammer;
    public readonly Item javelin;
    public readonly Item katar;
    public readonly Item knife;
    public readonly Item long_sword;
    public readonly Item lucern_hammer;
    public readonly Item magic_horseshoe;
    public readonly Item mace;
    public readonly Item main_gauche;
    public readonly Item morning_star;
    public readonly Item lance;
    public readonly Item partisan;
    public readonly Item ranseur;
    public readonly Item rapier;
    public readonly Item rubber_hose;
    public readonly Item scalpel;
    public readonly Item scimitar;
    public readonly Item scythe;
    public readonly Item sickle;
    public readonly Item short_sword;
    public readonly Item sling;
    public readonly Item spear;
    public readonly Item spetum;
    public readonly Item stiletto;
    public readonly Item trident;
    public readonly Item twohanded_sword;
    public readonly Item voulge;
    public readonly Item war_hammer;
    public readonly Item worm_tooth;
    public readonly Item chakram;

    // japanese.
    public readonly Item kanabo;
    public readonly Item nunchaku;
    public readonly Item sai;
    public readonly Item katana;
    public readonly Item wakizashi;
    public readonly Item tsurugi;
    public readonly Item shuriken;
    public readonly Item ya;
    public readonly Item yumi;

    // staves.
    public readonly Item quarterstaff;
    public readonly Item dread_staff;
    public readonly Item flash_staff;
    public readonly Item thunder_staff;

    // dark elven.
    public readonly Item dark_elven_arrow;
    public readonly Item dark_elven_bow;
    public readonly Item dark_elven_dagger;
    public readonly Item dark_elven_mithrilcoat;
    public readonly Item dark_elven_short_sword;

    // dwarvish.
    public readonly Item dwarvish_cloak;
    public readonly Item dwarvish_iron_helm;
    public readonly Item dwarvish_mattock;
    public readonly Item dwarvish_mithrilcoat;
    public readonly Item dwarvish_roundshield;
    public readonly Item dwarvish_short_sword;
    public readonly Item dwarvish_spear;

    // elvish.
    public readonly Item elven_arrow;
    public readonly Item elven_boots;
    public readonly Item elven_bow;
    public readonly Item elven_broadsword;
    public readonly Item elven_cloak;
    public readonly Item elven_dagger;
    public readonly Item elven_leather_helm;
    public readonly Item elven_mithrilcoat;
    public readonly Item elven_shield;
    public readonly Item elven_short_sword;
    public readonly Item elven_spear;

    // orcish.
    public readonly Item orcish_arrow;
    public readonly Item orcish_bow;
    public readonly Item orcish_chain_mail;
    public readonly Item orcish_cloak;
    public readonly Item orcish_dagger;
    public readonly Item orcish_helm;
    public readonly Item orcish_ring_mail;
    public readonly Item orcish_shield;
    public readonly Item orcish_short_sword;
    public readonly Item orcish_spear;

    // silver.
    public readonly Item silver_arrow;
    public readonly Item silver_crossbow_bolt;
    public readonly Item silver_dagger;
    public readonly Item silver_dragon_scale_mail;
    public readonly Item silver_dragon_scales;
    public readonly Item silver_heavy_hammer;
    public readonly Item silver_long_sword;
    public readonly Item silver_mace;
    public readonly Item silver_sabre;
    public readonly Item silver_short_sword;
    public readonly Item silver_spear;
    public readonly Item silver_lance;
    public readonly Item silver_twohanded_sword;

    // mithril.
    public readonly Item mithril_arrow;
    public readonly Item mithril_crossbow_bolt;
    public readonly Item mithril_dagger;
    public readonly Item mithril_long_sword;
    public readonly Item mithril_short_sword;
    public readonly Item mithril_battleaxe;
    public readonly Item mithril_katar;
    public readonly Item mithril_lance;
    public readonly Item mithril_sabre;
    public readonly Item mithril_spear;
    public readonly Item mithril_twohanded_sword;
    public readonly Item mithril_whip;

    // dragon scales.
    public readonly Item black_dragon_scale_mail;
    public readonly Item black_dragon_scales;
    public readonly Item blue_dragon_scale_mail;
    public readonly Item blue_dragon_scales;
    public readonly Item deep_dragon_scale_mail;
    public readonly Item deep_dragon_scales;
    public readonly Item gold_dragon_scale_mail;
    public readonly Item gold_dragon_scales;
    public readonly Item green_dragon_scale_mail;
    public readonly Item green_dragon_scales;
    public readonly Item orange_dragon_scale_mail;
    public readonly Item orange_dragon_scales;
    public readonly Item red_dragon_scale_mail;
    public readonly Item red_dragon_scales;
    public readonly Item shimmering_dragon_scale_mail;
    public readonly Item shimmering_dragon_scales;
    public readonly Item white_dragon_scale_mail;
    public readonly Item white_dragon_scales;
    public readonly Item yellow_dragon_scale_mail;
    public readonly Item yellow_dragon_scales;

    // food.
    public readonly Item apple;
    public readonly Item banana;
    public readonly Item candy_bar;
    public readonly Item carrot;
    public readonly Item cheese;
    public readonly Item clove_of_garlic;
    public readonly Item animal_corpse;
    public readonly Item vegetable_corpse;
    public readonly Item cram_ration;
    public readonly Item cration;
    public readonly Item cream_pie;
    public readonly Item egg;
    public readonly Item eucalyptus_leaf;
    //public readonly Item eyeball;
    public readonly Item fish;
    public readonly Item food_ration;
    public readonly Item fortune_cookie;
    public readonly Item huge_chunk_of_meat;
    public readonly Item kelp_frond;
    public readonly Item kration;
    public readonly Item lembas_wafer;
    public readonly Item lump_of_royal_jelly;
    public readonly Item meat_ring;
    public readonly Item meat_stick;
    public readonly Item meatball;
    public readonly Item melon;
    public readonly Item mushroom;
    public readonly Item orange;
    public readonly Item pancake;
    public readonly Item pear;
    public readonly Item sandwich;
    //public readonly Item severed_hand;
    public readonly Item slime_mould;
    public readonly Item sprig_of_wolfsbane;
    public readonly Item tin;
    public readonly Item tortilla;
    public readonly Item tripe_ration;
    public readonly Item iron_ration;

    // coins.
    public readonly Item gold_coin;

    // rocks.
    public readonly Item rock;
    public readonly Item flint;

    // gems.
    public readonly Item agate;
    public readonly Item amber;
    public readonly Item amethyst;
    public readonly Item aquamarine;
    public readonly Item black_opal;
    public readonly Item chrysoberyl;
    public readonly Item citrine;
    public readonly Item diamond;
    public readonly Item dilithium_crystal;
    public readonly Item emerald;
    public readonly Item fluorite;
    public readonly Item jacinth;
    public readonly Item jade;
    public readonly Item jasper;
    public readonly Item jet;
    public readonly Item garnet;
    public readonly Item obsidian;
    public readonly Item opal;
    //public readonly Item pearl;
    public readonly Item ruby;
    public readonly Item sapphire;
    public readonly Item topaz;
    public readonly Item turquoise;

    // baubles.
    public readonly Item black_glass_bauble;
    public readonly Item blue_glass_bauble;
    public readonly Item green_glass_bauble;
    public readonly Item orange_glass_bauble;
    public readonly Item violet_glass_bauble;
    public readonly Item red_glass_bauble;
    public readonly Item white_glass_bauble;
    public readonly Item yellow_glass_bauble;
    public readonly Item yellowish_brown_glass_bauble;

    // tools.
    public readonly Item bag_of_holding;
    public readonly Item bag_of_tricks;
    public readonly Item beartrap;
    public readonly Item blindfold;
    public readonly Item brass_bugle;
    public readonly Item lock_pick;
    public readonly Item bronze_bell;
    public readonly Item bell_of_resources;
    public readonly Item bell_of_secrets;
    public readonly Item bell_of_harmony;
    public readonly Item bell_of_strife;
    public readonly Item caltrops;
    public readonly Item can_of_grease;
    public readonly Item chest;
    public readonly Item crystal_ball;
    public readonly Item drum_of_earthquake;
    public readonly Item earmuffs;
    public readonly Item costume_earrings;
    public readonly Item mute_earrings;
    public readonly Item proof_earrings;
    public readonly Item expensive_camera;
    public readonly Item fire_horn;
    public readonly Item fly_swatter;
    public readonly Item frost_horn;
    public readonly Item heavy_iron_ball;
    public readonly Item holy_wafer;
    public readonly Item horn_of_plenty;
    public readonly Item ice_box;
    public readonly Item iron_chain;
    public readonly Item land_mine;
    public readonly Item lantern;
    public readonly Item large_box;
    public readonly Item leather_drum;
    public readonly Item magic_bugle;
    public readonly Item magic_candle;
    public readonly Item magic_figurine;
    public readonly Item magic_flute;
    public readonly Item magic_harp;
    public readonly Item magic_lamp;
    public readonly Item magic_marker;
    public readonly Item magic_whistle;
    public readonly Item oil_lamp;
    public readonly Item pickaxe;
    public readonly Item porter;
    public readonly Item sack;
    public readonly Item tinning_kit;
    public readonly Item tin_whistle;
    public readonly Item tooled_horn;
    public readonly Item torch;
    public readonly Item unicorn_horn;
    public readonly Item wooden_flute;
    public readonly Item wooden_harp;
    public readonly Item wax_candle;
    public readonly Item wooden_stake;

    // firearms.
    public readonly Item rocket_launcher;
    public readonly Item rocket;
    public readonly Item grenade_launcher;
    public readonly Item frag_grenade;
    public readonly Item gas_grenade;
    public readonly Item stick_of_dynamite;
    public readonly Item hunting_rifle;
    public readonly Item assault_rifle;
    public readonly Item sniper_rifle;
    public readonly Item submachine_gun;
    public readonly Item heavy_machine_gun;
    public readonly Item shotgun;
    public readonly Item auto_shotgun;
    public readonly Item sawnoff_shotgun;
    public readonly Item shotgun_shell;
    public readonly Item pistol;
    public readonly Item bullet;
    public readonly Item silver_bullet;
    public readonly Item mithril_bullet;

    // amulets.
    public readonly Item amulet_of_change;
    public readonly Item amulet_of_drain_resistance;
    public readonly Item amulet_of_ESP;
    public readonly Item amulet_of_flying;
    public readonly Item amulet_of_life_saving;
    public readonly Item amulet_of_nada;
    public readonly Item amulet_of_reflection;
    public readonly Item amulet_of_restful_sleep;
    public readonly Item amulet_of_unchanging;
    public readonly Item amulet_of_vitality;
    public readonly Item amulet_versus_poison;
    public readonly Item amulet_versus_stone;

    // books.
    public readonly Item book_of_acid_stream;
    public readonly Item book_of_animate_dead;
    public readonly Item book_of_animate_object;
    public readonly Item book_of_poison_blast;
    public readonly Item book_of_bind_undead;
    public readonly Item book_of_blank_paper;
    public readonly Item book_of_cancellation;
    public readonly Item book_of_charm;
    public readonly Item book_of_clairvoyance;
    public readonly Item book_of_cone_of_cold;
    public readonly Item book_of_confusion;
    public readonly Item book_of_create_familiar;
    public readonly Item book_of_fear;
    public readonly Item book_of_living_wall;
    public readonly Item book_of_summoning;
    public readonly Item book_of_curing;
    public readonly Item book_of_detect_food;
    public readonly Item book_of_detect_monsters;
    public readonly Item book_of_detect_treasure;
    public readonly Item book_of_detect_unseen;
    public readonly Item book_of_dig;
    public readonly Item book_of_disintegrate;
    public readonly Item book_of_drain_life;
    public readonly Item book_of_extra_healing;
    public readonly Item book_of_full_healing;
    public readonly Item book_of_regenerate;
    public readonly Item book_of_flaming_sphere;
    public readonly Item book_of_freezing_sphere;
    public readonly Item book_of_shocking_sphere;
    public readonly Item book_of_soaking_sphere;
    public readonly Item book_of_crushing_sphere;
    public readonly Item book_of_finger_of_death;
    public readonly Item book_of_fireball;
    public readonly Item book_of_ice_storm;
    public readonly Item book_of_force_bolt;
    public readonly Item book_of_haste;
    public readonly Item book_of_healing;
    public readonly Item book_of_identify;
    public readonly Item book_of_invisibility;
    public readonly Item book_of_jumping;
    public readonly Item book_of_blinking;
    public readonly Item book_of_phasing;
    public readonly Item book_of_knock;
    public readonly Item book_of_levitation;
    public readonly Item book_of_darkness;
    public readonly Item book_of_light;
    public readonly Item book_of_lightning_bolt;
    public readonly Item book_of_magic_mapping;
    public readonly Item book_of_magic_missile;
    public readonly Item book_of_polymorph;
    public readonly Item book_of_deflection;
    public readonly Item book_of_raise_dead;
    public readonly Item book_of_telekinesis;
    public readonly Item book_of_remove_curse;
    public readonly Item book_of_restoration;
    public readonly Item book_of_sleep;
    public readonly Item book_of_slow;
    public readonly Item book_of_teleport_away;
    public readonly Item book_of_toxic_spray;
    public readonly Item book_of_turn_undead;
    public readonly Item book_of_walling;
    public readonly Item book_of_wizard_lock;

    // potions.
    public readonly Item potion_of_acid;
    public readonly Item potion_of_affliction;
    public readonly Item potion_of_amnesia;
    public readonly Item potion_of_blindness;
    public readonly Item potion_of_booze;
    public readonly Item potion_of_clairvoyance;
    public readonly Item potion_of_confusion;
    public readonly Item potion_of_divinity;
    public readonly Item potion_of_ESP;
    public readonly Item potion_of_extra_healing;
    public readonly Item potion_of_fruit_juice;
    public readonly Item potion_of_full_healing;
    public readonly Item potion_of_gain_ability;
    public readonly Item potion_of_gain_energy;
    public readonly Item potion_of_gain_level;
    public readonly Item potion_of_hallucination;
    public readonly Item potion_of_healing;
    public readonly Item potion_of_invisibility;
    public readonly Item potion_of_levitation;
    public readonly Item potion_of_monster_detection;
    //public readonly Item potion_of_mutation;
    public readonly Item potion_of_object_detection;
    public readonly Item potion_of_ink;
    public readonly Item potion_of_oil;
    public readonly Item potion_of_paralysis;
    public readonly Item potion_of_polymorph;
    public readonly Item potion_of_rage;
    public readonly Item potion_of_recovery;
    public readonly Item potion_of_see_invisible;
    public readonly Item potion_of_sickness;
    public readonly Item potion_of_sleeping;
    public readonly Item potion_of_speed;
    public readonly Item potion_of_water;

    // rings.
    public readonly Item ring_of_adornment;
    public readonly Item ring_of_aggravation;
    public readonly Item ring_of_berserking;
    public readonly Item ring_of_cold_resistance;
    public readonly Item ring_of_conflict;
    public readonly Item ring_of_fire_resistance;
    public readonly Item ring_of_free_action;
    public readonly Item ring_of_constitution;
    public readonly Item ring_of_dexterity;
    public readonly Item ring_of_intelligence;
    public readonly Item ring_of_strength;
    public readonly Item ring_of_wisdom;
    public readonly Item ring_of_hunger;
    public readonly Item ring_of_accuracy;
    public readonly Item ring_of_impact;
    public readonly Item ring_of_invisibility;
    public readonly Item ring_of_levitation;
    public readonly Item ring_of_naught;
    public readonly Item ring_of_poison_resistance;
    public readonly Item ring_of_polymorph;
    public readonly Item ring_of_polymorph_control;
    public readonly Item ring_of_protection;
    public readonly Item ring_of_regeneration;
    public readonly Item ring_of_searching;
    public readonly Item ring_of_see_invisible;
    public readonly Item ring_of_shock_resistance;
    public readonly Item ring_of_sleeping;
    public readonly Item ring_of_slow_digestion;
    public readonly Item ring_of_stealth;
    public readonly Item ring_of_sustain_ability;
    public readonly Item ring_of_telekinesis;
    public readonly Item ring_of_teleport_control;
    public readonly Item ring_of_teleportation;
    public readonly Item ring_of_warning;

    // scrolls.
    public readonly Item scroll_of_amnesia;
    public readonly Item scroll_of_blank_paper;
    public readonly Item scroll_of_charging;
    public readonly Item scroll_of_confusion;
    public readonly Item scroll_of_destruction;
    public readonly Item scroll_of_devouring;
    public readonly Item scroll_of_air;
    public readonly Item scroll_of_earth;
    public readonly Item scroll_of_enchantment;
    public readonly Item scroll_of_enlightenment;
    public readonly Item scroll_of_entrapment;
    public readonly Item scroll_of_ice;
    public readonly Item scroll_of_fire;
    public readonly Item scroll_of_water;
    public readonly Item scroll_of_food_detection;
    public readonly Item scroll_of_gathering;
    public readonly Item scroll_of_genocide;
    public readonly Item scroll_of_gold_detection;
    public readonly Item scroll_of_identify;
    public readonly Item scroll_of_light;
    public readonly Item scroll_of_magic_mapping;
    public readonly Item scroll_of_murder;
    public readonly Item scroll_of_punishment;
    public readonly Item scroll_of_raise_dead;
    public readonly Item scroll_of_remove_curse;
    public readonly Item scroll_of_replication;
    public readonly Item scroll_of_summoning;
    public readonly Item scroll_of_terror;
    public readonly Item scroll_of_taming;
    public readonly Item scroll_of_teleportation;
    public readonly Item scroll_of_training;
    public readonly Item scroll_of_tranquillity;

    // wands.
    public readonly Item wand_of_animation;
    public readonly Item wand_of_cancellation;
    public readonly Item wand_of_cold;
    public readonly Item wand_of_create_horde;
    public readonly Item wand_of_summoning;
    public readonly Item wand_of_death;
    public readonly Item wand_of_digging;
    public readonly Item wand_of_draining;
    public readonly Item wand_of_extra_healing;
    public readonly Item wand_of_fear;
    public readonly Item wand_of_fire;
    public readonly Item wand_of_fireball;
    public readonly Item wand_of_iceball;
    public readonly Item wand_of_healing;
    public readonly Item wand_of_light;
    public readonly Item wand_of_lightning;
    public readonly Item wand_of_locking;
    public readonly Item wand_of_magic_missile;
    public readonly Item wand_of_make_invisible;
    public readonly Item wand_of_nothing;
    public readonly Item wand_of_opening;
    public readonly Item wand_of_polymorph;
    public readonly Item wand_of_punishment;
    public readonly Item wand_of_secret_detection;
    public readonly Item wand_of_sleep;
    public readonly Item wand_of_slow;
    public readonly Item wand_of_haste;
    public readonly Item wand_of_phase;
    public readonly Item wand_of_striking;
    public readonly Item wand_of_teleportation;
    public readonly Item wand_of_theft;
    public readonly Item wand_of_undead_turning;

    // barding.
    public readonly Item saddle;
    public readonly Item leather_barding;
    public readonly Item chain_barding;
    public readonly Item mithril_barding;
    public readonly Item plate_barding;

    // keys.
    public readonly Item skeleton_key;
    public readonly Item detonation_key;
    public readonly Item dimension_key;
    public readonly Item phantom_key;

    // artifacts.
    public readonly Item Stamped_Letter;
    public readonly Item Backpack;
    public readonly Item Gold_Key;
    public readonly Item Ruby_Key;
    public readonly Item Shadow_Key;
    public readonly Item Silver_Key;
    public readonly Item Jade_Key;
    public readonly Item Master_Key;
    public readonly Item Creeping_Sprout;
    public readonly Item Eyes_of_Ra;
    public readonly Item Talaria;
    public readonly Item Verimurus;
    public readonly Item Dragonbane;
    public readonly Item Illuminare;
    public readonly Item Escapist;
    public readonly Item Mirrorbright;
    public readonly Item Rosenthral;
    public readonly Item Witherloch;
    public readonly Item Pandoras_Box;
    public readonly Item Prudentia;
    public readonly Item Lashing_Tongue;
    public readonly Item Cleaver;
    public readonly Item Magistrator;
    public readonly Item Sunsword;
    public readonly Item Carapace;
    public readonly Item Chaoshammer;
    public readonly Item The_Hero;
    public readonly Item Colossal_Excavator;
    public readonly Item Grimtooth;
    public readonly Item Giantslayer;
    public readonly Item Harbalest;
    public readonly Item Aurigage;
    public readonly Item Blinderag;
    public readonly Item Maulfrost;
    public readonly Item Voltaic;
    public readonly Item Deadwood;
    public readonly Item Dire_Needle;
    public readonly Item Chasm_Edge;
    public readonly Item Deagle;
    public readonly Item Drilanze;
    public readonly Item Ancient_Katana;
    public readonly Item Masamune;
    public readonly Item Muramasa;
    public readonly Item Ravenbow;
    public readonly Item Eye_of_Aethiopica;
    public readonly Item Philosophers_Stone;
    public readonly Item Runesword;
    public readonly Item Vorpal_Blade;

    private readonly Inv.DistinctList<Item> DragonScalesArmourList;
  }
}