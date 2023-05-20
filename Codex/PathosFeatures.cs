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

      Feature AddFeature(string Name, Material Material, Chance RoomChance, Glyph Glyph, Action<FeatureEditor> Action)
      {
        return Register.Add(F =>
        {
          F.Name = Name;
          F.Material = Material;
          F.RoomChance = RoomChance;
          F.Glyph = Glyph;

          CodexRecruiter.Enrol(() => Action(F));
        });
      }

      altar = AddFeature("altar", Materials.stone, Chance.OneIn60, Glyphs.altar, F =>
      {
        F.Sonic = Sonics.prayer;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(200000);

        // TODO: can't use the altar at all, when shunned.

        F.DropApply.Karma(ChangeType.Decrease, Dice.Fixed(5));
        F.DropApply.Divine();

        F.DestroyStrike = Strikes.holy;
        F.DestroyApply.Harm(Elements.shock, 6.d6() + 6);
        F.DestroyApply.ConvertFixture(altar, Devices.spiked_pit);
        F.DestroyApply.CreateHorde(Dice.One, Hordes.jackal);
        F.DestroyApply.CreateHorde(Dice.One, Hordes.hell_hound);
        F.DestroyApply.CreateHorde(Dice.One, Hordes.demon);

        var DivineUse = F.AddUse(Codex.Motions.divine, null, Delay.FromTurns(40), Sonics.prayer, Audibility: 5);
        DivineUse.SetCast().FilterDivined(false);
        DivineUse.Apply.Karma(ChangeType.Decrease, Dice.Fixed(5));
        DivineUse.Apply.Divine();

        var PrayUse = F.AddUse(Codex.Motions.pray, null, Delay.FromTurns(40), Sonics.prayer, Audibility: 5);
        PrayUse.Apply.Karma(ChangeType.Decrease, Dice.Fixed(200));
        PrayUse.Apply.WithSourceSanctity
        (
          B =>
          {
            B.Unpunish();
            B.RemoveCurse(Dice.One);
            B.Divine();
            B.Sanctify(Items.potion_of_water, Sanctities.Blessed);
          },
          U =>
          {
            U.Divine();
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
        SacrificeUse.Apply.Sacrifice();
      });

      bed = AddFeature("bed", Materials.wood, Chance.OneIn120, Glyphs.bed, F =>
      {
        F.Sonic = Sonics.scrape; // TODO: Sonics.snore?
        F.Mountable = true;
        F.Weight = Weight.FromUnits(18000);

        F.DestroyApply.ConvertFixture(bed, Devices.squeaky_board);
        F.DestroyApply.Karma(ChangeType.Decrease, Dice.Fixed(50));

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
              Table.Add(5, A => A.Afflict(Codex.Afflictions.nits));
            });
            C.WhenChance(Chance.OneIn4, T => T.TeleportCharacter(Properties.teleportation));
          }
        );
      });

      fountain = AddFeature("fountain", Materials.stone, Chance.OneIn10, Glyphs.fountain, F =>
      {
        F.Sonic = Sonics.water_splash;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(200000);

        F.DestroyExplosion = Codex.Explosions.watery;
        F.DestroyApply.Harm(Elements.water, Dice.Zero);
        F.DestroyApply.ConvertFixture(fountain, Devices.water_trap);

        F.DropApply.Harm(Elements.water, Dice.Zero);
        F.DropApply.ConvertAsset(Stocks.potion, WholeStack: true, Items.potion_of_water);
        F.DropApply.ConvertAsset(Stocks.scroll, WholeStack: true, Items.scroll_of_blank_paper);
        F.DropApply.ConvertAsset(Stocks.book, WholeStack: true, Items.book_of_blank_paper);
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
          B => B.Heal(1.d4(), Modifier.Zero),
          U => U.Nutrition(Dice.Fixed(10)),
          C => C.Malnutrition(Dice.Fixed(10))
        );
        DrinkUse.Apply.WhenProbability(Table =>
        {
          Table.Add(50, A => { });
          Table.Add(10, A => A.CreateAsset(Dice.One, Stocks.gem));
          Table.Add(10, A => A.CreateHorde(Dice.One, Hordes.water_moccasin));
          Table.Add(10, A => A.ConvertFixture(fountain, Devices.water_trap)); // destroy 10%
          Table.Add(5, A => A.CreateEntity(Dice.One, Entities.water_demon));
          Table.Add(5, A => A.CreateEntity(Dice.One, Entities.water_nymph));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.water_elemental));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.water_hulk));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.water_troll));
          Table.Add(1, A => A.PlaceCurse(Dice.One, Sanctities.Cursed));
          Table.Add(1, A => A.GainTalent(Properties.see_invisible));
          Table.Add(1, A => A.DetectCharacter(Range.Sq15));
          Table.Add(1, A => A.AreaTransient(Properties.fear, 4.d6(), Kinds.Living.ToArray()));
          Table.Add(1, A => A.DestroyCarriedAsset(4.d10() + 10, new[] { Stocks.gem }, null, null));
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
        DipUse.Apply.ConvertAsset(Stocks.potion, WholeStack: true, Items.potion_of_water);
        DipUse.Apply.ConvertAsset(Stocks.scroll, WholeStack: true, Items.scroll_of_blank_paper);
        DipUse.Apply.ConvertAsset(Stocks.book, WholeStack: true, Items.book_of_blank_paper);
        DipUse.Apply.WithSourceSanctity
        (
          B => B.Sanctify(Item: null, Sanctities.Blessed),
          U => U.Nothing(),
          C => C.Sanctify(Item: null, Sanctities.Cursed)
        );
        Abuse(DipUse.Apply);

        var AnointUse = F.AddUse(Codex.Motions.anoint, null, Delay.FromTurns(20), Sonics.water_splash, Audibility: 2);
        AnointUse.SetCast().FilterAnyItem().FilterSanctity(Sanctities.List.ToArray()); // include all BUC, but don't allow items that do not have sanctity like coins.
        AnointUse.Apply.Harm(Elements.water, Dice.Zero);
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
            Table.Add(10, A => A.ConvertFixture(fountain, Devices.water_trap)); // destroy 10%
            Table.Add(10, A => A.CreateHorde(Dice.One, Hordes.water_moccasin));
            Table.Add(1, A => A.CreateEntity(Dice.One, Entities.water_demon));
            Table.Add(1, A => A.CreateEntity(Dice.One, Entities.water_nymph));
          });
        }
      });

      grave = AddFeature("grave", Materials.stone, Chance.OneIn60, Glyphs.grave, F =>
      {
        F.Sonic = Sonics.groan;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(200000);

        F.DestroyApply.ConvertFixture(grave, Devices.pit);
        F.DestroyApply.Karma(ChangeType.Decrease, Dice.Fixed(50));

        var DigUse = F.AddUse(Codex.Motions.dig, null, Delay.FromTurns(50), Sonics.pick_axe, Audibility: 10);
        DigUse.SetCast().FilterItem(Items.pickaxe, Items.dwarvish_mattock, Items.Colossal_Excavator, Items.wand_of_digging).SetAssetIndividualised(); // use a charge for the wand of digging.
        DigUse.Apply.ConvertFixture(grave, Devices.pit);
        DigUse.Apply.Karma(ChangeType.Decrease, Dice.Fixed(50));

        DigUse.Apply.WhenChance(Chance.OneIn2, T => T.CreateEntity(1.d3(), Kinds.Undead.ToArray()));
        DigUse.Apply.WhenChance(Chance.OneIn2, T => T.CreateAsset(1.d2()));
      });

      sarcophagus = AddFeature("sarcophagus", Materials.stone, Chance.OneIn120, Glyphs.sarcophagus, F =>
      {
        F.Sonic = Sonics.scrape;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(200000);

        F.DestroyApply.ConvertFixture(sarcophagus, Devices.hole);
        F.DestroyApply.Karma(ChangeType.Decrease, Dice.Fixed(50));

        var OpenUse = F.AddUse(Codex.Motions.open, null, Delay.FromTurns(20), Sonics.scrape, Audibility: 10);
        OpenUse.Apply.ConvertFixture(sarcophagus, Device: null);

        // BUC doesn't really matter because it's removed the first time you use it?

        OpenUse.Apply.WhenProbability(Table =>
        {
          Table.Add(30, A => A.CreateEntity(Dice.One, Kinds.mummy));
          Table.Add(20, A => A.CreateAsset(Dice.One, QuantityDice: null, new[] { Items.animal_corpse, Items.vegetable_corpse }));
          Table.Add(15, A => A.CreateAsset(Dice.One, 10.d100(), Items.gold_coin));
          Table.Add(10, A => A.CreateAsset(2.d3() + 1, new[] { Stocks.ring, Stocks.amulet, Stocks.gem, Stocks.wand, Stocks.scroll, Stocks.potion, Stocks.book }));
          Table.Add(5, A => A.PlaceCurse(1.d4() + 1, Sanctities.Cursed));
          Table.Add(5, A => A.ApplyTransient(Properties.hunger, 10.d100() + 100));
          Table.Add(5, A => A.CreateEntity(Dice.One, Entities.stone_golem));
          Table.Add(5, A => A.Punish(Codex.Punishments.malignant_aura));
          Table.Add(5, A => A.CreateEntity(Dice.Fixed(8), Kinds.mummy));
        });
      });

      pentagram = AddFeature("pentagram", Materials.wax, Chance.OneIn90, Glyphs.pentagram, F =>
      {
        F.Sonic = Sonics.chant;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(10000);

        F.DestroyExplosion = Codex.Explosions.fiery;
        F.DestroyApply.Harm(Elements.fire, Dice.Zero);
        F.DestroyApply.ConvertFixture(pentagram, Devices.fire_trap);

        var ChantUse = F.AddUse(Codex.Motions.chant, null, Delay.FromTurns(20), Sonics.chant, Audibility: 5);
        ChantUse.Apply.Light(IsLit: false);
        //ChantUse.Apply.WhenTargetKind(new[] { Kinds.echo }, T => T.Energise(Dice.Fixed(50), Modifier.Zero)); // TODO: good or bad idea?
        ChantUse.Apply.WithSourceSanctity
        (
          B =>
          {
            B.Energise(Dice.Fixed(10), Modifier.Zero);
          },
          U =>
          {
            U.Energise(Dice.Fixed(5), Modifier.Zero);
          },
          C =>
          {
            C.Diminish(Dice.Fixed(10), Modifier.Zero);
          }
        );
        ChantUse.Apply.WhenProbability(Table =>
        {
          Table.Add(50, A => { });
          Table.Add(10, A => A.CreateEntity(Dice.One, Kinds.Undead.ToArray()));
          Table.Add(10, A => A.CreateHorde(Dice.One, Hordes.spider));
          Table.Add(10, A => A.ConvertFixture(pentagram, Devices.fire_trap));
          Table.Add(5, A => A.CreateEntity(Dice.One, Entities.efreeti));
          Table.Add(5, A => A.CreateEntity(Dice.One, Entities.succubus));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.fire_elemental));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.fire_vortex));
          Table.Add(1, A => A.CreateEntity(Dice.One, Entities.fire_giant));
          Table.Add(1, A => A.PlaceCurse(Dice.One, Sanctities.Cursed));
          Table.Add(1, A => A.GainTalent(Properties.cannibalism));
          Table.Add(1, A => A.DetectAsset(Range.Sq15));
          Table.Add(1, A => A.AreaTransient(Properties.rage, 4.d6(), Kinds.Living.ToArray()));
          Table.Add(1, A => A.DestroyCarriedAsset(1.d4() + 1, new[] { Stocks.food }, null, null));
          //Table.Add(1, A => A.Afflict(Codex.Afflictions.Array.Where(Z => !Z.Severe).ToArray()));
          Table.Add(1, A => A.UnlessTargetResistant(Elements.poison, T => T.DecreaseAbility(Attributes.strength, Dice.One)));
          Table.Add(1, A =>
          {
            A.RestoreAbility();
            A.IncreaseOneAbility(Dice.One);
          });
        });
      });

      stall = AddFeature("stall", Materials.wood, Chance.Never, Glyphs.stall, F =>
      {
        F.Sonic = Sonics.creak; // NOTE: stalls don't make a chime SFX when they are unoccupied (rely on Shop.Sonic).
        F.Mountable = true;
        F.Weight = Weight.FromUnits(25000);

        F.DestroyApply.ConvertFixture(stall, Devices.entropy_trap);

        var Storage = F.SetStorage();
        Storage.Locking = true;
        Storage.Preservation = true;
        Storage.ContainedDice = Dice.Zero;
        Storage.LockSonic = Codex.Sonics.locked;
        Storage.BreakSonic = Codex.Sonics.broken_lock;
      });

      throne = AddFeature("throne", Materials.stone, Chance.OneIn120, Glyphs.throne, F =>
      {
        F.Sonic = Sonics.throne;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(35000);

        F.DestroyApply.ConvertFixture(throne, Devices.trapdoor);

        var SitUse = F.AddUse(Codex.Motions.sit, null, Delay.FromTurns(20), Sonics.throne, Audibility: 1);
        SitUse.Apply.WhenProbability(Table =>
        {
          Table.Add(2, A => A.Nothing());
          Table.Add(1, A => A.DecreaseOneAbility(1.d4()));
          Table.Add(1, A => A.IncreaseOneAbility(Dice.One));
          Table.Add(1, A => A.Harm(Elements.shock, 1.d30()));
          Table.Add(1, A =>
          {
            A.Heal(8.d8(), Modifier.FromRank(4));
            A.RemoveTransient(Properties.sickness, Properties.blindness);
          });
          Table.Add(1, A => A.DestroyOwnedAsset(5.d1000(), StockArray: null, SanctityArray: null, new Material[] { Materials.gold }));
          //Table.Add(1, A => { }); // TODO: wish!
          //Table.Add(1, A => { }); // TODO: genocide!
          Table.Add(1, A =>
          {
            A.ApplyTransient(Properties.blindness, 1.d100() + 250);
            A.PlaceCurse(Dice.One, Sanctities.Cursed);
          });
          Table.Add(1, A => A.Mapping(Range.Sq30, Chance.Always));
          Table.Add(1, A => A.GainTalent(Properties.see_invisible));
          Table.Add(1, A => A.TeleportCharacter(Properties.teleportation));
          Table.Add(1, A => A.TeleportInventoryAsset());
          Table.Add(1, A => A.Polymorph());
          Table.Add(1, A => A.Identify(All: true, Sanctity: null)); // identify all items in inventory.
          Table.Add(1, A => A.ApplyTransient(Properties.stunned, 1.d7() + 16));
          Table.Add(1, A => A.GainSkill(RandomPoints: false, Codex.Skills.heavy_armour));
          Table.Add(1, A => A.AnimateObjects(ObjectEntity: Entities.animate_object, Corrupt: null));
          Table.Add(1, A => A.EnchantUp(Dice.One)); // enchant a random item.
          Table.Add(1, A => A.Energise(Dice.Zero, Modifier.FromRank(4))); // increase maximum mana.
          Table.Add(1, A => A.Heal(Dice.Zero, Modifier.FromRank(4))); // increase maximum life.
          //Table.Add(1, A => A.CloneSourceCharacter(Dice.One)); // TODO: hostile and replica equipment.
        });
        SitUse.Apply.WhenChance(Chance.OneIn2, A => A.ConvertFixture(throne, Devices.trapdoor));
      });

      workbench = AddFeature("workbench", Materials.wood, Chance.OneIn80, Glyphs.workbench, F =>
      {
        F.Description = "This well-worn table is imbued with the ancient magics of creation and destruction.";
        F.Sonic = Sonics.craft;
        F.Mountable = true;
        F.Weight = Weight.FromUnits(50000);

        var Workbench = F.SetWorkbench();
        Workbench.CraftSkill = Skills.crafting;
        Workbench.CraftSonic = Sonics.craft;
        Workbench.ScrapSkill = Skills.crafting;
        Workbench.ScrapSonic = Sonics.scrap;

        /*01*/
        Workbench.AddAccident(Codex.Explosions.dark, A => A.Light(false));
        /*02*/Workbench.AddAccident(Codex.Explosions.light, A =>
        {
          A.Light(true);
          A.ApplyTransient(Properties.blindness, 3.d100());
        });
        /*03*/Workbench.AddAccident(Codex.Explosions.watery, A =>
        {
          A.Harm(Elements.water, Dice.One);
          A.WhenChance(Chance.OneIn20, T => T.ConvertAsset(Codex.Stocks.potion, WholeStack: true, Items.potion_of_water));
          A.WhenChance(Chance.OneIn20, T => T.ConvertAsset(Codex.Stocks.scroll, WholeStack: true, Items.scroll_of_blank_paper));
          A.WhenChance(Chance.OneIn20, T => T.ConvertAsset(Codex.Stocks.book, WholeStack: true, Items.book_of_blank_paper));
        });
        /*04*/Workbench.AddAccident(Codex.Explosions.magical, A => A.Harm(Elements.magical, 2.d6() + 2));
        /*05*/Workbench.AddAccident(Codex.Explosions.fiery, A => A.Harm(Elements.fire, 3.d6() + 3));
        /*06*/Workbench.AddAccident(Codex.Explosions.frosty, A => A.ApplyTransient(Properties.slowness, 3.d100()));
        /*07*/Workbench.AddAccident(Codex.Explosions.electric, A => A.Harm(Elements.shock, 4.d6() + 4));
        /*08*/Workbench.AddAccident(Codex.Explosions.muddy, A => A.ApplyTransient(Properties.hallucination, 3.d100()));
        /*09*/Workbench.AddAccident(Codex.Explosions.acid, A => A.Harm(Elements.acid, 6.d6() + 6));
        /*10*/Workbench.AddAccident(Codex.Explosions.death, A => A.ApplyTransient(Properties.fear, 5.d50()));

        /*11*/Workbench.AddAccident(Codex.Explosions.dark, A => 
        {
          A.Light(false);
          A.CreateHorde(Dice.One, Targeted: true);
        });
        /*12*/Workbench.AddAccident(Codex.Explosions.light, A => 
        {
          A.Light(true);
          A.ApplyTransient(Properties.blindness, 4.d100());
          A.CreateHorde(Dice.One, Targeted: true);
        });
        /*13*/Workbench.AddAccident(Codex.Explosions.watery, A => 
        {
          A.Harm(Elements.water, Dice.One);
          A.ApplyTransient(Properties.sleeping, 4.d50());
        });
        /*14*/Workbench.AddAccident(Codex.Explosions.magical, A =>
        {
          A.Harm(Elements.magical, 4.d6() + 4);
          A.TeleportInventoryAsset();
        });
        /*15*/Workbench.AddAccident(Codex.Explosions.fiery, A => 
        {
          A.Harm(Elements.fire, 5.d6() + 5);
          A.DestroyCarriedAsset(2.d3(), null, null, new Material[] { Materials.paper });
        });
        /*16*/Workbench.AddAccident(Codex.Explosions.frosty, A =>
        {
          A.Harm(Elements.cold, 6.d6() + 6);
          A.DestroyCarriedAsset(2.d3(), null, null, new Material[] { Materials.glass });
        });
        /*17*/Workbench.AddAccident(Codex.Explosions.electric, A => 
        {
          A.Harm(Elements.shock, 7.d6() + 7);
          A.TeleportAway();
        });
        /*18*/Workbench.AddAccident(Codex.Explosions.muddy, A =>
        {
          A.ApplyTransient(Properties.hallucination, 4.d100());
          A.Polymorph();
        });
        /*19*/Workbench.AddAccident(Codex.Explosions.acid, A => 
        {
          A.Harm(Elements.acid, 8.d6() + 8);
          A.DestroyEquippedAsset(Dice.One, null, null, null);
        });
        /*20*/Workbench.AddAccident(Codex.Explosions.death, A =>
        {
          A.Death(Elements.magical, Kinds.Living.ToArray(), Strikes.death, Cause: null);
          A.Punish(Codex.Punishments.List.ToArray());
          A.Afflict(Codex.Afflictions.List.ToArray());
          //A.ConvertFixture(F, Devices.entropy_trap); // NOTE: this will convert any nearby fixtures as well.
        });

        F.DestroyApply.ConvertFixture(workbench, Devices.entropy_trap);
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
