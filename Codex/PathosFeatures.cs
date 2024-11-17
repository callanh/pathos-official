using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexFeatures : CodexPage<ManifestFeatures, FeatureEditor, Feature>
  {
    private CodexFeatures() { }
#if MASTER_CODEX
    internal CodexFeatures(Codex Codex)
      : base(Codex.Manifest.Features)
    {
      var Stocks = Codex.Stocks;
      var Items = Codex.Items;
      var Hordes = Codex.Hordes;
      var Devices = Codex.Devices;
      var Kinds = Codex.Kinds;
      var Entities = Codex.Entities;
      var Materials = Codex.Materials;
      var Properties = Codex.Properties;
      var Elements = Codex.Elements;
      var Attributes = Codex.Attributes;
      var Sanctities = Codex.Sanctities;
      var Strikes = Codex.Strikes;
      var Glyphs = Codex.Glyphs; 
      var Sonics = Codex.Sonics;
      var Skills = Codex.Skills;

      Feature AddFeature(string Name, Material Material, Chance RoomChance, Glyph RegularGlyph, Glyph BrokenGlyph, Action<FeatureEditor> Action)
      {
        return Register.Add(F =>
        {
          F.Name = Name;
          F.Material = Material;
          F.RoomChance = RoomChance;
          F.RegularGlyph = RegularGlyph;
          F.BrokenGlyph = BrokenGlyph;

          CodexRecruiter.Enrol(() => Action(F));
        });
      }

      altar = AddFeature("altar", Materials.stone, Chance.OneIn60, Glyphs.altar, Glyphs.altar_broken, F =>
      {
        F.Sonic = Sonics.prayer;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(200000);

        // TODO: can't use the altar at all, when shunned.

        F.DropApply.DecreaseKarma(Dice.Fixed(5));
        F.DropApply.DivineItem();

        F.DestroyStrike = Strikes.holy;
        F.DestroyApply.HarmEntity(Elements.shock, 6.d6() + 6);
        F.DestroyApply.BreakFeature(altar);
        F.DestroyApply.CreateSpecificHorde(Dice.One, Hordes.jackal);
        F.DestroyApply.CreateSpecificHorde(Dice.One, Hordes.hell_hound);
        F.DestroyApply.CreateSpecificHorde(Dice.One, Hordes.demon);

        var DivineUse = F.AddUse(Codex.Motions.divine, null, Delay.FromTurns(40), Sonics.prayer, Audibility: 5);
        DivineUse.SetCast().FilterDivined(false);
        DivineUse.Apply.DecreaseKarma(Dice.Fixed(5));
        DivineUse.Apply.DivineItem();

        var PrayUse = F.AddUse(Codex.Motions.pray, null, Delay.FromTurns(40), Sonics.prayer, Audibility: 5);
        PrayUse.Apply.DecreaseKarma(Dice.Fixed(200));
        PrayUse.Apply.WithSourceSanctity
        (
          B =>
          {
            B.UnpunishEntity();
            B.RemoveCurse(Dice.One);
            B.DivineItem();
            B.Sanctify(Items.potion_of_water, Sanctities.Blessed);
          },
          U =>
          {
            U.DivineItem();
            U.Sanctify(Items.potion_of_water, Sanctities.Blessed);
          },
          C =>
          {
            C.Sanctify(Items.potion_of_water, Sanctities.Cursed);
            C.Amnesia(Range.Sq10);
          }
        );

        var SacrificeUse = F.AddUse(Codex.Motions.sacrifice, Utility: null, Delay.FromTurns(60), Sonics.prayer, Audibility: 5);
        SacrificeUse.SetCast().FilterItem(Items.animal_corpse, Items.vegetable_corpse);
        SacrificeUse.Apply.SacrificeItem();
      });

      bed = AddFeature("bed", Materials.wood, Chance.OneIn120, Glyphs.bed, Glyphs.bed_broken, F =>
      {
        F.Sonic = Sonics.scrape; // TODO: Sonics.snore?
        F.Mountable = true;
        F.Weight = Weight.FromUnits(18000);

        F.DestroyApply.ConvertFeatureToDevice(bed, Devices.squeaky_board);
        F.DestroyApply.DecreaseKarma(Dice.Fixed(50));

        var ReclineUse = F.AddUse(Codex.Motions.recline, Utility: null, Delay.FromTurns(20), Sonics.magic, Audibility: 1); // springs creak softly.
        ReclineUse.Apply.WithSourceSanctity
        (
          B =>
          {
            B.IntentionalTransient(Properties.sleeping, 1.d50() + 50);
            B.WhenChance(Chance.OneIn4, T => T.ApplyTransient(Properties.life_regeneration, 1.d50() + 50));
            B.WhenChance(Chance.OneIn4, T => T.ApplyTransient(Properties.mana_regeneration, 1.d50() + 50));
          },
          U =>
          {
            U.IntentionalTransient(Properties.sleeping, 1.d50() + 50);
          },
          C =>
          {
            C.WhenProbability(Table =>
            {
              Table.Add(50, A => A.IntentionalTransient(Properties.sleeping, 1.d100() + 100)); // extra long sleep
              Table.Add(20, A => A.ApplyTransient(Properties.paralysis, 2.d10() + 10));
              Table.Add(20, A => A.ApplyTransient(Properties.hallucination, 2.d10() + 10));
              Table.Add(5, A => A.ApplyTransient(Properties.narcolepsy, 2.d200() + 200));
              Table.Add(5, A => A.AfflictEntity(Codex.Afflictions.nits));
            });
            C.WhenChance(Chance.OneIn4, T => T.TeleportEntity(Properties.teleportation));
          }
        );
      });

      fountain = AddFeature("fountain", Materials.stone, Chance.OneIn10, Glyphs.fountain, Glyphs.fountain_broken, F =>
      {
        F.Sonic = Sonics.water_splash;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(200000);

        F.DestroyExplosion = Codex.Explosions.watery;
        F.DestroyApply.HarmEntity(Elements.water, Dice.Zero);
        F.DestroyApply.BreakFeature(fountain);

        F.DropApply.HarmEntity(Elements.water, Dice.Zero);
        F.DropApply.ConvertItem(Stocks.potion, WholeStack: true, Items.potion_of_water);
        F.DropApply.ConvertItem(Stocks.scroll, WholeStack: true, Items.scroll_of_blank_paper);
        F.DropApply.ConvertItem(Stocks.book, WholeStack: true, Items.book_of_blank_paper);
        // TODO: the dropped item is the SourceAsset which means it's BUC is used instead of the SourceFixture in WithSourceSanctity.
        //F.DropApply.WithSourceSanctity
        //(
        //  B => B.Bless(null),
        //  U => U.Nothing(),
        //  C => C.Curse(null)
        //); 
        Abuse(F.DropApply);

        var DrinkUse = F.AddUse(Codex.Motions.drink, null, Delay.FromTurns(20), Sonics.water_fountain, Audibility: 2);
        DrinkUse.Apply.WithSourceSanctity
        (
          B => B.HealEntity(1.d4(), Modifier.Zero),
          U => U.GainNutrition(Dice.Fixed(10)),
          C => C.LoseNutrition(Dice.Fixed(10))
        );
        DrinkUse.Apply.WhenProbability(Table =>
        {
          Table.Add(50, A => { });
          Table.Add(10, A => A.CreateItem(Dice.One, Stocks.gem));
          Table.Add(10, A => A.CreateSpecificHorde(Dice.One, Hordes.water_moccasin));
          Table.Add(5, A => A.ConvertFeatureToDevice(fountain, Devices.water_trap)); // destroy 5%
          Table.Add(5, A => A.BreakFeature(fountain)); // destroy 5%
          Table.Add(5, A => A.CreateEntity(Dice.One, Entities.water_demon));
          Table.Add(5, A => A.CreateEntity(Dice.One, Entities.water_nymph));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.water_elemental));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.water_hulk));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.water_troll));
          Table.Add(1, A => A.PlaceCurse(Dice.One, Sanctities.Cursed));
          Table.Add(1, A => A.GainTalent(Properties.see_invisible));
          Table.Add(1, A => A.DetectEntity(Range.Sq15));
          Table.Add(1, A => A.AreaTransient(Properties.fear, 4.d6(), Kinds.Living.ToArray()));
          Table.Add(1, A => A.DestroyCarriedItem(4.d10() + 10, new[] { Stocks.gem }, null, null));
          Table.Add(1, A => A.UnlessTargetResistant(Elements.poison, T => T.DecreaseAbility(Attributes.strength, Dice.One)));
          Table.Add(1, A =>
          {
            A.RestoreAbility();
            A.IncreaseOneAbility(Dice.One);
          });
          //Table.Add(1, A => A.CreateAsset(Dice.One, Items.water_walking_boots);
        });

        var DipUse = F.AddUse(Codex.Motions.dip, null, Delay.FromTurns(20), Sonics.water_splash, Audibility: 2);
        DipUse.SetCast().FilterStock(Stocks.book, Stocks.scroll, Stocks.potion);
        DipUse.Apply.ConvertItem(Stocks.potion, WholeStack: true, Items.potion_of_water);
        DipUse.Apply.ConvertItem(Stocks.scroll, WholeStack: true, Items.scroll_of_blank_paper);
        DipUse.Apply.ConvertItem(Stocks.book, WholeStack: true, Items.book_of_blank_paper);
        DipUse.Apply.WithSourceSanctity
        (
          B => B.Sanctify(Item: null, Sanctities.Blessed),
          U => U.Nothing(),
          C => C.Sanctify(Item: null, Sanctities.Cursed)
        );
        Abuse(DipUse.Apply);

        var AnointUse = F.AddUse(Codex.Motions.anoint, null, Delay.FromTurns(20), Sonics.water_splash, Audibility: 2);
        AnointUse.SetCast().FilterAnyItem().FilterSanctity(Sanctities.List.ToArray()); // include all BUC, but don't allow items that do not have sanctity like coins.
        AnointUse.Apply.HarmEntity(Elements.water, Dice.Zero);
        AnointUse.Apply.WithSourceSanctity
        (
          B => B.Sanctify(Item: null, Sanctities.Blessed),
          U => U.Nothing(),
          C => C.Sanctify(Item: null, Sanctities.Cursed)
        );
        Abuse(AnointUse.Apply);

        void Abuse(ApplyEditor Apply)
        {
          Apply.WhenProbability(Table =>
          {
            Table.Add(78, A => A.Nothing());
            Table.Add(10, A => A.ConvertFeatureToDevice(fountain, Devices.water_trap)); // destroy 10%
            Table.Add(10, A => A.CreateSpecificHorde(Dice.One, Hordes.water_moccasin));
            Table.Add(1, A => A.CreateEntity(Dice.One, Entities.water_demon));
            Table.Add(1, A => A.CreateEntity(Dice.One, Entities.water_nymph));
          });
        }
      });

      grave = AddFeature("grave", Materials.stone, Chance.OneIn60, Glyphs.grave, Glyphs.grave_broken, F =>
      {
        F.Sonic = Sonics.groan;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(200000);

        F.DestroyApply.ConvertFeatureToDevice(grave, Devices.pit);
        F.DestroyApply.DecreaseKarma(Dice.Fixed(50));

        var DigUse = F.AddUse(Codex.Motions.dig, null, Delay.FromTurns(50), Sonics.pick_axe, Audibility: 10);
        DigUse.SetCast().FilterItem(Items.pickaxe, Items.dwarvish_mattock, Items.Colossal_Excavator, Items.wand_of_digging).SetAssetIndividualised(); // use a charge for the wand of digging.
        DigUse.Apply.ConvertFeatureToDevice(grave, Devices.pit);
        DigUse.Apply.DecreaseKarma(Dice.Fixed(50));

        DigUse.Apply.WhenChance(Chance.OneIn2, T => T.CreateEntity(1.d3(), Kinds.Undead.ToArray()));
        DigUse.Apply.WhenChance(Chance.OneIn2, T => T.CreateItem(1.d2()));
      });

      sarcophagus = AddFeature("sarcophagus", Materials.stone, Chance.OneIn120, Glyphs.sarcophagus, Glyphs.sarcophagus_broken, F =>
      {
        F.Sonic = Sonics.scrape;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(200000);

        F.DestroyApply.BreakFeature(sarcophagus);
        F.DestroyApply.DecreaseKarma(Dice.Fixed(50));

        var OpenUse = F.AddUse(Codex.Motions.open, null, Delay.FromTurns(20), Sonics.scrape, Audibility: 10);
        OpenUse.Apply.BreakFeature(sarcophagus);

        // BUC doesn't really matter because it's removed the first time you use it?

        OpenUse.Apply.WhenProbability(Table =>
        {
          // 75% typical.
          Table.Add(30, A => A.CreateEntity(Dice.One, Kinds.mummy));
          Table.Add(20, A => A.CreateItem(Dice.One, QuantityDice: null, new[] { Items.animal_corpse, Items.vegetable_corpse }));
          Table.Add(15, A => A.CreateItem(Dice.One, 10.d100(), Items.gold_coin));
          Table.Add(10, A => A.CreateItem(2.d3() + 1, new[] { Stocks.ring, Stocks.amulet, Stocks.gem, Stocks.wand, Stocks.scroll, Stocks.potion, Stocks.book }));

          // 20% annoying.
          Table.Add(5, A => A.PlaceCurse(1.d4() + 1, Sanctities.Cursed));
          Table.Add(5, A => A.ApplyTransient(Properties.hunger, 10.d100() + 100));
          Table.Add(5, A => A.ApplyTransient(Properties.sickness, 10.d100() + 100));
          Table.Add(5, A => A.PunishEntity(Codex.Punishments.malignant_aura));

          // 5% dangerous.
          Table.Add(1, A => A.CreateEntity(Dice.One, Kinds.vampire));
          Table.Add(1, A => A.CreateEntity(Dice.Fixed(8), Kinds.mummy));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.stone_golem));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.skeleton));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.mellified_man));
        });
      });

      pentagram = AddFeature("pentagram", Materials.wax, Chance.OneIn90, Glyphs.pentagram, Glyphs.pentagram_broken, F =>
      {
        F.Sonic = Sonics.chant;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(10000);

        F.DestroyExplosion = Codex.Explosions.fiery;
        F.DestroyApply.HarmEntity(Elements.fire, Dice.Zero);
        F.DestroyApply.ConvertFeatureToDevice(pentagram, Devices.fire_trap);

        var ChantUse = F.AddUse(Codex.Motions.chant, null, Delay.FromTurns(20), Sonics.chant, Audibility: 5);
        ChantUse.Apply.Light(IsLit: false, Locality.Area);
        //ChantUse.Apply.WhenTargetKind(new[] { Kinds.echo }, T => T.Energise(Dice.Fixed(50), Modifier.Zero)); // TODO: good or bad idea?
        ChantUse.Apply.WithSourceSanctity
        (
          B =>
          {
            B.EnergiseEntity(Dice.Fixed(10), Modifier.Zero);
          },
          U =>
          {
            U.EnergiseEntity(Dice.Fixed(5), Modifier.Zero);
          },
          C =>
          {
            C.DiminishEntity(Dice.Fixed(10), Modifier.Zero);
          }
        );
        ChantUse.Apply.WhenProbability(Table =>
        {
          Table.Add(50, A => { });
          Table.Add(10, A => A.CreateEntity(Dice.One, Kinds.Undead.ToArray()));
          Table.Add(10, A => A.CreateSpecificHorde(Dice.One, Hordes.spider));
          Table.Add(10, A => A.ConvertFeatureToDevice(pentagram, Devices.fire_trap));
          Table.Add(5, A => A.CreateEntity(Dice.One, Entities.efreeti));
          Table.Add(5, A => A.CreateEntity(Dice.One, Entities.succubus));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.fire_elemental));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.fire_vortex));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.fire_giant));
          Table.Add(1, A => A.PlaceCurse(Dice.One, Sanctities.Cursed));
          Table.Add(1, A => A.GainTalent(Properties.cannibalism));
          Table.Add(1, A => A.DetectItem(Range.Sq15));
          Table.Add(1, A => A.AreaTransient(Properties.rage, 4.d6(), Kinds.Living.ToArray()));
          Table.Add(1, A => A.DestroyCarriedItem(1.d4() + 1, new[] { Stocks.food }, null, null));
          //Table.Add(1, A => A.Afflict(Codex.Afflictions.Array.Where(Z => !Z.Severe).ToArray()));
          Table.Add(1, A => A.UnlessTargetResistant(Elements.poison, T => T.DecreaseAbility(Attributes.strength, Dice.One)));
          Table.Add(1, A =>
          {
            A.RestoreAbility();
            A.IncreaseOneAbility(Dice.One);
          });
        });
      });

      stall = AddFeature("stall", Materials.wood, Chance.Never, Glyphs.stall, Glyphs.stall_broken, F =>
      {
        F.Sonic = Sonics.creak; // NOTE: stalls don't make a chime SFX when they are unoccupied (rely on Shop.Sonic).
        F.Mountable = true;
        F.Weight = Weight.FromUnits(25000);

        F.DestroyApply.BreakFeature(stall);

        var Storage = F.SetStorage();
        Storage.Locking = true;
        Storage.Preservation = true;
        Storage.ContainedDice = Dice.Zero;
        Storage.LockSonic = Codex.Sonics.locked;
        Storage.BreakSonic = Codex.Sonics.broken_lock;
      });

      throne = AddFeature("throne", Materials.stone, Chance.OneIn120, Glyphs.throne, Glyphs.throne_broken, F =>
      {
        F.Sonic = Sonics.throne;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(35000);

        F.DestroyApply.ConvertFeatureToDevice(throne, Devices.trapdoor);

        var SitUse = F.AddUse(Codex.Motions.sit, null, Delay.FromTurns(20), Sonics.throne, Audibility: 1);
        SitUse.Apply.WhenProbability(Table =>
        {
          Table.Add(2, A => A.Nothing());
          Table.Add(1, A => A.DecreaseOneAbility(1.d4()));
          Table.Add(1, A => A.IncreaseOneAbility(Dice.One));
          Table.Add(1, A => A.HarmEntity(Elements.shock, 1.d30()));
          Table.Add(1, A =>
          {
            A.HealEntity(8.d8(), Modifier.FromRank(4));
            A.RemoveTransient(Properties.sickness, Properties.blindness);
          });
          Table.Add(1, A => A.DestroyOwnedItem(5.d1000(), StockArray: null, SanctityArray: null, new Material[] { Materials.gold }));
          //Table.Add(1, A => { }); // TODO: wish!
          //Table.Add(1, A => { }); // TODO: genocide!
          Table.Add(1, A =>
          {
            A.ApplyTransient(Properties.blindness, 1.d100() + 250);
            A.PlaceCurse(Dice.One, Sanctities.Cursed);
          });
          Table.Add(1, A => A.Mapping(Range.Sq30, Chance.Always));
          Table.Add(1, A => A.GainTalent(Properties.see_invisible));
          Table.Add(1, A => A.TeleportEntity(Properties.teleportation));
          Table.Add(1, A => A.TeleportInventoryItem());
          Table.Add(1, A => A.PolymorphEntity());
          Table.Add(1, A => A.Identify(All: true, Sanctity: null)); // identify all items in inventory.
          Table.Add(1, A => A.ApplyTransient(Properties.stunned, 1.d7() + 16));
          Table.Add(1, A => A.GainSkill(RandomPoints: false, Codex.Skills.heavy_armour));
          Table.Add(1, A => A.AnimateObject(ObjectEntity: Entities.animate_object, CorruptProperty: null, CorruptDice: Dice.Zero));
          Table.Add(1, A => A.EnchantUp(Dice.One)); // enchant a random item.
          Table.Add(1, A => A.EnergiseEntity(Dice.Zero, Modifier.FromRank(4))); // increase maximum mana.
          Table.Add(1, A => A.HealEntity(Dice.Zero, Modifier.FromRank(4))); // increase maximum life.
          //Table.Add(1, A => A.CloneSourceCharacter(Dice.One)); // TODO: hostile and replica equipment.
        });
        SitUse.Apply.WhenChance(Chance.OneIn2, A => A.ConvertFeatureToDevice(throne, Devices.trapdoor));
      });

      workbench = AddFeature("workbench", Materials.wood, Chance.OneIn80, Glyphs.workbench, Glyphs.workbench_broken, F =>
      {
        F.Description = "This well-worn table is imbued with the ancient magics of creation and destruction.";
        F.Sonic = Sonics.craft;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(50000);

        F.DestroyApply.ConvertFeatureToDevice(workbench, Devices.entropy_trap);

        var Workbench = F.SetWorkbench();
        Workbench.CraftSkill = Skills.crafting;
        Workbench.CraftSonic = Sonics.craft;
        Workbench.ScrapSkill = Skills.crafting;
        Workbench.ScrapSonic = Sonics.scrap;

        /*01*/
        Workbench.AddAccident(Codex.Explosions.dark, A => A.Light(false, Locality.Area));
        /*02*/Workbench.AddAccident(Codex.Explosions.light, A =>
        {
          A.Light(true, Locality.Area);
          A.ApplyTransient(Properties.blindness, 3.d100());
        });
        /*03*/Workbench.AddAccident(Codex.Explosions.watery, A =>
        {
          A.HarmEntity(Elements.water, Dice.One);
          A.WhenChance(Chance.OneIn20, T => T.ConvertItem(Codex.Stocks.potion, WholeStack: true, Items.potion_of_water));
          A.WhenChance(Chance.OneIn20, T => T.ConvertItem(Codex.Stocks.scroll, WholeStack: true, Items.scroll_of_blank_paper));
          A.WhenChance(Chance.OneIn20, T => T.ConvertItem(Codex.Stocks.book, WholeStack: true, Items.book_of_blank_paper));
        });
        /*04*/Workbench.AddAccident(Codex.Explosions.magical, A => A.HarmEntity(Elements.magical, 2.d6() + 2));
        /*05*/Workbench.AddAccident(Codex.Explosions.fiery, A => A.HarmEntity(Elements.fire, 3.d6() + 3));
        /*06*/Workbench.AddAccident(Codex.Explosions.frosty, A => A.ApplyTransient(Properties.slowness, 3.d100()));
        /*07*/Workbench.AddAccident(Codex.Explosions.electric, A => A.HarmEntity(Elements.shock, 4.d6() + 4));
        /*08*/Workbench.AddAccident(Codex.Explosions.muddy, A => A.ApplyTransient(Properties.hallucination, 3.d100()));
        /*09*/Workbench.AddAccident(Codex.Explosions.acid, A => A.HarmEntity(Elements.acid, 6.d6() + 6));
        /*10*/Workbench.AddAccident(Codex.Explosions.death, A => A.ApplyTransient(Properties.fear, 5.d50()));

        /*11*/Workbench.AddAccident(Codex.Explosions.dark, A => 
        {
          A.Light(false, Locality.Area);
          A.CreateRandomHorde(Dice.One, Targeted: true);
        });
        /*12*/Workbench.AddAccident(Codex.Explosions.light, A => 
        {
          A.Light(true, Locality.Area);
          A.ApplyTransient(Properties.blindness, 4.d100());
          A.CreateRandomHorde(Dice.One, Targeted: true);
        });
        /*13*/Workbench.AddAccident(Codex.Explosions.watery, A => 
        {
          A.HarmEntity(Elements.water, Dice.One);
          A.ApplyTransient(Properties.sleeping, 4.d50());
        });
        /*14*/Workbench.AddAccident(Codex.Explosions.magical, A =>
        {
          A.HarmEntity(Elements.magical, 4.d6() + 4);
          A.TeleportInventoryItem();
        });
        /*15*/Workbench.AddAccident(Codex.Explosions.fiery, A => 
        {
          A.HarmEntity(Elements.fire, 5.d6() + 5);
          A.DestroyCarriedItem(2.d3(), null, null, new Material[] { Materials.paper });
        });
        /*16*/Workbench.AddAccident(Codex.Explosions.frosty, A =>
        {
          A.HarmEntity(Elements.cold, 6.d6() + 6);
          A.DestroyCarriedItem(2.d3(), null, null, new Material[] { Materials.glass });
        });
        /*17*/Workbench.AddAccident(Codex.Explosions.electric, A => 
        {
          A.HarmEntity(Elements.shock, 7.d6() + 7);
          A.TeleportItemAway();
        });
        /*18*/Workbench.AddAccident(Codex.Explosions.muddy, A =>
        {
          A.ApplyTransient(Properties.hallucination, 4.d100());
          A.PolymorphItemAndEntityAndTrap();
        });
        /*19*/Workbench.AddAccident(Codex.Explosions.acid, A => 
        {
          A.HarmEntity(Elements.acid, 8.d6() + 8);
          A.DestroyEquippedItem(Dice.One, null, null, null);
        });
        /*20*/Workbench.AddAccident(Codex.Explosions.death, A =>
        {
          A.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          A.PunishEntity(Codex.Punishments.List.ToArray());
          A.AfflictEntity(Codex.Afflictions.List.ToArray());
          //A.ConvertFixture(F, Devices.entropy_trap); // NOTE: this will convert any nearby fixtures as well.
        });
      });
    }
#endif

    public readonly Feature altar;
    public readonly Feature bed;
    public readonly Feature fountain;
    public readonly Feature grave;
    public readonly Feature pentagram;
    public readonly Feature sarcophagus;
    public readonly Feature stall;
    public readonly Feature throne;
    public readonly Feature workbench;
  }
}
