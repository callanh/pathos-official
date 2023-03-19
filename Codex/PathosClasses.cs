using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexClasses : CodexPage<ManifestClasses, ClassEditor, Class>
  {
    private CodexClasses() { }
#if MASTER_CODEX
    internal CodexClasses(Codex Codex)
      : base(Codex.Manifest.Classes)
    {
      var Genders = Codex.Genders;
      var Stocks = Codex.Stocks;
      var Items = Codex.Items;
      var Spells = Codex.Spells;
      var Glyphs = Codex.Glyphs;
      var Races = Codex.Races;
      var Properties = Codex.Properties;
      var Elements = Codex.Elements;
      var Skills = Codex.Skills;
      var Attributes = Codex.Attributes;
      var Qualifications = Codex.Qualifications;
      var Sanctities = Codex.Sanctities;

      var UndesireableItemArray = new[]
      {
        Items.amulet_of_nada,
        Items.amulet_of_change,
        Items.amulet_of_restful_sleep,
        Items.amulet_of_unchanging,
        Items.animal_corpse,    // 
        Items.vegetable_corpse, // can't have a meaningful type at start.
        Items.egg,              // 
        Items.tin,              // 
        Items.potion_of_acid,
        Items.potion_of_hallucination,
        Items.ring_of_aggravation,
        Items.ring_of_berserking,
        Items.ring_of_hunger,
        Items.ring_of_levitation,
        Items.ring_of_sleeping,
        Items.ring_of_naught,
        Items.costume_earrings,
        Items.mute_earrings,
        Items.scroll_of_amnesia,
        Items.scroll_of_blank_paper,
        Items.scroll_of_earth,
        Items.scroll_of_fire,
        Items.wand_of_nothing,
        //Items.wand_of_wishing,
      };

      var OrientalItemArray = new[]
      {
        Items.kanabo,
        Items.wakizashi,
        Items.nunchaku,
        Items.sai,
        Items.tsurugi,
        Items.katana,
        Items.yumi,
        Items.ya,
        Items.shuriken
      };

      Item[] DesirableItemArray(ClassEditor Class, Stock Stock)
      {
        return Stock.Items.Except(UndesireableItemArray).Where(I => !I.Grade.Unique && I.Rarity > 0).ToArray();
      }
      Spell[] DesireableSpellArray(ClassEditor Class)
      {
        var AlreadySpellArray = Class.Startup.Grimoires.SelectMany(G => G.Spells).ToArray();

        // spells level 1, 2, 3.
        return Spells.List.Where(S => S.Level <= 3 && Class.Startup.Graduations.Any(G => G.Skill == S.School.Skill) && !AlreadySpellArray.Contains(S)).ToArray();
      }

      Class AddClass(Action<ClassEditor> CompileAction) => Register.Add(C => CodexRecruiter.Enrol(() => CompileAction(C)));

      barbarian = AddClass(C =>
      {
        C.Name = "barbarian";
        C.Description = "Noble savages, hardened to battle by generations of survival in the unforgiving wilderness. They are not strangers to death's call; legends tell of their incredible strength and battle prowess with heavy weapons.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(14, 1.d10());
        C.ManaAdvancement.Set(1, Dice.Fixed(1));
        C.SetDistribution(Attributes.strength, Attributes.constitution, Attributes.dexterity, Attributes.wisdom, Attributes.charisma, Attributes.intelligence);
        C.AddAvatar(Genders.male, Glyphs.male_barbarian);
        C.AddAvatar(Genders.female, Glyphs.female_barbarian);
        C.AddAvatar(Races.orc, Genders.male, Glyphs.orc_male_barbarian);
        C.AddAvatar(Races.orc, Genders.female, Glyphs.orc_female_barbarian);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_barbarian);
        C.AddAvatar(Races.lizardman, Genders.female, Glyphs.lizardman_female_barbarian);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_barbarian);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_barbarian);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_barbarian);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_barbarian);
        C.AddAvatar(Races.troll, Genders.male, Glyphs.troll_male_barbarian);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_barbarian);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_barbarian);
        C.AddFeat(2, Elements.poison);
        C.AddFeat(8, Properties.quickness);
        C.AddFeat(14, Properties.stealth);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.dual_wielding, Skills.literacy, Skills.riding, Skills.swimming,
          Skills.evocation,
          Skills.light_armour, Skills.medium_armour, Skills.heavy_armour,
          Skills.light_blade, Skills.medium_blade, Skills.heavy_blade,
          Skills.axe, Skills.club, Skills.hammer, Skills.mace, Skills.pick, Skills.polearm, Skills.spear);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.twohanded_sword);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.axe);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.ring_mail);
        C.Startup.Loot.AddKit(Chance.Always, Items.food_ration);
        C.Startup.Loot.AddKit(Chance.OneIn6, Items.oil_lamp);
      });

      gladiator = AddClass(C =>
      {
        C.Name = "gladiator";
        C.Description = "Warrior slaves compelled to battle for the entertainment of raucous crowds, often to the death. Those that can survive this cauldron are trained into the most brutal of pit fighters.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(14, 1.d9());
        C.ManaAdvancement.Set(1, Dice.Fixed(1));
        C.SetDistribution(Attributes.dexterity, Attributes.strength, Attributes.constitution, Attributes.wisdom, Attributes.intelligence, Attributes.charisma);
        C.AddAvatar(Genders.male, Glyphs.male_gladiator);
        C.AddAvatar(Genders.female, Glyphs.female_gladiator);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_gladiator);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_gladiator);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_gladiator);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_gladiator);
        C.AddFeat(2, Properties.jumping);
        C.AddFeat(8, Properties.deflection);
        C.AddFeat(14, Properties.vitality);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.dual_wielding, Skills.riding, Skills.swimming,
          Skills.light_armour, Skills.medium_armour,
          Skills.light_blade, Skills.medium_blade, Skills.heavy_blade,
          Skills.axe, Skills.club, Skills.hammer, Skills.mace, Skills.polearm, Skills.spear, Skills.unarmed_combat, Skills.whip);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, new[] { Items.trident, Items.spear, Items.voulge, Items.halberd, Items.guisarme, Items.billguisarme, Items.bec_de_corbin, Items.bardiche, Items.ranseur, Items.partisan, Items.spetum, Items.fauchard, Items.glaive, Items.lucern_hammer });
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.brass_knuckles);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.brass_knuckles);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.studded_leather_armour);
      });

      bard = AddClass(C =>
      {
        C.Name = "bard";
        C.Description = "Charming minstrels, making their way through the world with their songs and stories. They are jacks-of-all-trades; alongside their troubadour talents, they dabble in both martial skills and spellcraft.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(10, 1.d7());
        C.ManaAdvancement.Set(4, 1.d2());
        C.SetDistribution(Attributes.charisma, Attributes.dexterity, Attributes.constitution, Attributes.wisdom, Attributes.intelligence, Attributes.strength);
        C.AddAvatar(Genders.male, Glyphs.male_bard);
        C.AddAvatar(Genders.female, Glyphs.female_bard);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_bard);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_bard);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_bard);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_bard);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_bard);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_bard);
        C.AddAvatar(Races.troll, Genders.male, Glyphs.troll_male_bard);
        C.AddAvatar(Races.elf, Genders.male, Glyphs.elf_male_bard);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_bard);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_bard);
        C.AddFeat(2, Properties.free_action);
        C.AddFeat(8, Properties.appraisal);
        C.AddFeat(14, Properties.clarity);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.crafting, Skills.dual_wielding, Skills.literacy, Skills.locks, Skills.music, Skills.riding, Skills.swimming,
          Skills.abjuration, Skills.clerical, Skills.divination, Skills.enchantment,
          Skills.light_armour, Skills.medium_armour,
          Skills.light_blade, Skills.medium_blade,
          Skills.crossbow, Skills.whip
          );
        C.Startup.AddGrimoire(Dice.One, DesireableSpellArray(C));
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.rapier);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.crossbow);
        C.Startup.Loot.AddKit(Chance.Always, 8.d6(), Modifier.Plus1, Items.crossbow_bolt);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.leather_cloak);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.high_boots);
        C.Startup.Loot.AddKit(Chance.Always, new[] { Items.fire_horn, Items.frost_horn, Items.magic_flute, Items.magic_harp, Items.drum_of_earthquake });
        C.Startup.Loot.AddKit(Chance.Always, Items.horn_of_plenty);
        C.Startup.Loot.AddKit(Chance.Always, 1.d250() + 250, Items.gold_coin);
      });

      caveman = AddClass(C =>
      {
        C.Name = "caveman";
        C.Description = "Primitive but formidable survivalists who act solely on instinct, free from societal burdens and civilised morality. They are regarded as vicious predators, and this fervour is matched in their loyalty to their kin.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(14, 1.d8());
        C.ManaAdvancement.Set(1, Dice.Fixed(1));
        C.SetDistribution(Attributes.strength, Attributes.constitution, Attributes.dexterity, Attributes.wisdom, Attributes.charisma, Attributes.intelligence);
        C.AddAvatar(Genders.male, Glyphs.male_caveman);
        C.AddAvatar(Genders.female, Glyphs.female_caveman);
        C.AddAvatar(Races.dwarf, Genders.male, Glyphs.dwarf_male_caveman);
        C.AddAvatar(Races.dwarf, Genders.female, Glyphs.dwarf_female_caveman);
        C.AddAvatar(Races.gnome, Genders.male, Glyphs.gnome_male_caveman);
        C.AddAvatar(Races.gnome, Genders.female, Glyphs.gnome_female_caveman);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_caveman);
        C.AddAvatar(Races.lizardman, Genders.female, Glyphs.lizardman_female_caveman);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_caveman);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_caveman);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_caveman);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_caveman);
        C.AddAvatar(Races.troll, Genders.male, Glyphs.troll_male_caveman);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_caveman);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_caveman);
        C.AddFeat(1, Properties.cannibalism);
        C.AddFeat(8, Properties.quickness);
        C.AddFeat(14, Properties.warning);
        C.AddFeat(20, Properties.slow_digestion);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.transmutation,
          Skills.light_armour, Skills.medium_armour,
          Skills.light_blade, Skills.medium_blade,
          Skills.axe, Skills.disc, Skills.bow, Skills.club, Skills.flail, Skills.hammer, Skills.mace, Skills.polearm, Skills.sling, Skills.spear, Skills.staff);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.club);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus2, Items.sling);
        C.Startup.Loot.AddKit(Chance.Always, 1.d10() + 10, Modifier.Plus0, Items.flint); // 11-20
        C.Startup.Loot.AddKit(Chance.Always, 1.d16() + 17, Modifier.Plus0, Items.rock); // 18-33
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.leather_armour);
      });

      convict = AddClass(C =>
      {
        C.Name = "convict";
        C.Description = "Escaped criminals who have spent so much of their life incarcerated that any trace of who or what they once were has all but vanished.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(12, 1.d6());
        C.ManaAdvancement.Set(1, Dice.Fixed(1));
        C.SetDistribution(Attributes.constitution, Attributes.charisma, Attributes.strength, Attributes.wisdom, Attributes.dexterity, Attributes.intelligence);
        C.AddAvatar(Genders.male, Glyphs.male_convict);
        C.AddAvatar(Genders.female, Glyphs.female_convict);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_convict);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_convict);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_convict);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_convict);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_convict);
        C.AddAvatar(Races.satyr, Genders.female, Glyphs.satyr_female_convict);
        C.AddFeat(10, Properties.hunger);
        C.AddFeat(20, Properties.aggravation);
        //C.Startup.SetSkill(Qualifications.Proficient, ); // none, an entire life in a prison cell.
        C.Startup.SetPunishment(Codex.Punishments.ball__chain);
      });

      explorer = AddClass(C =>
      {
        C.Name = "explorer";
        C.Description = "Quintessential scientific researchers and dauntless historical investigators. Both brash and debonair, they seek out knowledge and accolades alike. They are curiously fond of whips, and share a perplexing hatred towards serpents.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(11, 1.d7());
        C.ManaAdvancement.Set(1, Dice.Fixed(1));
        C.SetDistribution(Attributes.strength, Attributes.intelligence, Attributes.wisdom, Attributes.constitution, Attributes.dexterity, Attributes.charisma);
        C.AddAvatar(Genders.male, Glyphs.male_explorer);
        C.AddAvatar(Genders.female, Glyphs.female_explorer);
        C.AddAvatar(Races.dwarf, Genders.male, Glyphs.dwarf_male_explorer);
        C.AddAvatar(Races.dwarf, Genders.female, Glyphs.dwarf_female_explorer);
        C.AddAvatar(Races.gnome, Genders.male, Glyphs.gnome_male_explorer);
        C.AddAvatar(Races.gnome, Genders.female, Glyphs.gnome_female_explorer);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_explorer);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_explorer);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_explorer);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_explorer);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_explorer);
        C.AddFeat(1, Properties.appraisal);
        C.AddFeat(2, Properties.stealth);
        C.AddFeat(5, Properties.quickness);
        C.AddFeat(11, Properties.searching);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.crafting, Skills.literacy, Skills.riding, Skills.swimming,
          Skills.divination,
          Skills.light_armour, Skills.medium_armour,
          Skills.light_blade, Skills.medium_blade,
          Skills.disc, Skills.club, Skills.pick, Skills.sling, Skills.spear, Skills.staff, Skills.whip);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus3, Items.bullwhip);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.leather_jacket);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.fedora);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(3), Items.food_ration);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.pickaxe);
        C.Startup.Loot.AddKit(Chance.Always, Items.sack);
        C.Startup.Loot.AddKit(Chance.Always, Items.tinning_kit);
        C.Startup.Loot.AddKit(Chance.OneIn4, Items.oil_lamp);
        C.Startup.Loot.AddKit(Chance.OneIn10, Items.magic_marker);
      });

      miner = AddClass(C =>
      {
        C.Name = "miner";
        C.Description = "Strongly built, this tough and industrious worker is more than capable in dangerous situations. Traditional practices and heavy tools are used to overcome obstacles in their pursuit of valuable gemstones.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(11, 1.d8());
        C.ManaAdvancement.Set(1, Dice.Fixed(1));
        C.SetDistribution(Attributes.strength, Attributes.constitution, Attributes.wisdom, Attributes.intelligence, Attributes.charisma, Attributes.dexterity);
        C.AddAvatar(Genders.male, Glyphs.male_miner);
        C.AddAvatar(Genders.female, Glyphs.female_miner);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_miner);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_miner);
        C.AddFeat(1, Properties.searching);
        C.AddFeat(5, Properties.vitality);
        C.AddFeat(10, Properties.appraisal);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.crafting, Skills.literacy, Skills.traps,
          Skills.abjuration, Skills.transmutation,
          Skills.light_armour, Skills.medium_armour, Skills.heavy_armour,
          Skills.club, Skills.hammer, Skills.pick, Skills.sling, Skills.unarmed_combat, Skills.firearms);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.heavy_hammer);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus2, Items.pickaxe);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.tshirt);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.helmet);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.oilskin_cloak);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(1), Items.sandwich);
        C.Startup.Loot.AddKit(Chance.Always, Items.lantern);
        C.Startup.Loot.AddKit(Chance.Always, 2.d3(), Items.wax_candle); // backup lightsource.
        C.Startup.Loot.AddKit(Chance.Always, new[] { Items.amethyst, Items.diamond, Items.ruby, Items.emerald, Items.sapphire, Items.topaz });
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Items.scroll_of_gold_detection);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Items.stick_of_dynamite);
        C.Startup.Loot.AddKit(Chance.OneIn4, Items.wand_of_digging);
        C.Startup.Loot.AddKit(Chance.OneIn4, Items.scroll_of_earth);
        C.Startup.Loot.AddKit(Chance.OneIn4, Items.scroll_of_teleportation);
      });

      gunslinger = AddClass(C =>
      {
        C.Name = "gunslinger";
        C.Description = "Restless desperados, driven by wanderlust and troubled by the haunting memories of those they could not protect. They are sly masters of lead and steel, hellbent on redemption and retribution.";
        C.Backpack = Items.Backpack;
        C.AbolitionCandidate = true;
        C.LifeAdvancement.Set(11, 1.d7());
        C.ManaAdvancement.Set(1, Dice.One);
        C.SetDistribution(Attributes.dexterity, Attributes.constitution, Attributes.charisma, Attributes.strength, Attributes.intelligence, Attributes.wisdom);
        C.AddAvatar(Genders.male, Glyphs.male_gunslinger);
        C.AddAvatar(Genders.female, Glyphs.female_gunslinger);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_gunslinger);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_gunslinger);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_gunslinger);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_gunslinger);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_gunslinger);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_gunslinger);
        C.AddAvatar(Races.orc, Genders.male, Glyphs.orc_male_gunslinger);
        C.AddAvatar(Races.orc, Genders.female, Glyphs.orc_female_gunslinger);
        C.AddFeat(5, Properties.warning);
        C.AddFeat(10, Elements.sleep);
        C.AddFeat(15, Properties.appraisal);
        C.AddFeat(20, Properties.quickness);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.crafting, Skills.dual_wielding, Skills.riding,
          Skills.light_armour,
          Skills.light_blade,
          Skills.firearms, Skills.whip);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.pistol);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.pistol);
        C.Startup.Loot.AddKit(Chance.Always, 1.d80() + 86, Modifier.Plus0, Items.bullet);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.fedora);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.leather_jacket);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.low_boots);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.shotgun);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.knife);
        C.Startup.Loot.AddKit(Chance.Always, 1.d20() + 24, Modifier.Plus0, Items.shotgun_shell);
        C.Startup.Loot.AddKit(Chance.Always, 1.d3(), Modifier.Plus0, Items.stick_of_dynamite);
      });

      healer = AddClass(C =>
      {
        C.Name = "healer";
        C.Description = "Benevolent caretakers, always willing to lend a helping hand to the weak. Accustomed to dealing with illness, they are paragons of hygiene and cleanliness, purposed for aid. As such, they must rely on constant companionship to survive.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(11, 1.d8());
        C.ManaAdvancement.Set(5, 1.d2());
        C.SetDistribution(Attributes.constitution, Attributes.intelligence, Attributes.wisdom, Attributes.strength, Attributes.dexterity, Attributes.charisma);
        C.AddAvatar(Genders.male, Glyphs.male_healer);
        C.AddAvatar(Genders.female, Glyphs.female_healer);
        C.AddAvatar(Races.gnome, Genders.male, Glyphs.gnome_male_healer);
        C.AddAvatar(Races.gnome, Genders.female, Glyphs.gnome_female_healer);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_healer);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_healer);
        C.AddAvatar(Races.troll, Genders.male, Glyphs.troll_male_healer);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_healer);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_healer);
        C.AddFeat(2, Elements.poison);
        C.AddFeat(8, Properties.vitality);
        C.AddFeat(14, Properties.warning);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.crafting, Skills.literacy,
          Skills.abjuration, Skills.enchantment, Skills.clerical, Skills.necromancy,
          Skills.light_armour, Skills.medium_armour, Skills.heavy_armour,
          Skills.light_blade, Skills.medium_blade,
          Skills.club, Skills.dart, Skills.disc, Skills.sling, Skills.spear, Skills.staff);
        C.Startup.AddGrimoire(Dice.One, Spells.healing);
        C.Startup.AddGrimoire(Dice.One, Spells.extra_healing);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus3, Items.scalpel);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.leather_gloves);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(4), Items.potion_of_healing);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(4), Items.potion_of_extra_healing);
        C.Startup.Loot.AddKit(Chance.Always, Items.wand_of_sleep);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(5), Sanctities.Blessed, Items.apple);
        C.Startup.Loot.AddKit(Chance.OneIn25, Items.oil_lamp);
        C.Startup.Loot.AddKit(Chance.Always, 1.d1000() + 1001, Items.gold_coin);
      });

      hunter = AddClass(C =>
      {
        C.Name = "hunter";
        C.Description = "Professional marksmen who are obsessed with hunting the biggest game. Heavily equipped with firearms and the desire to build the most fantastical trophy collection.";
        C.AbolitionCandidate = true;
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(11, 1.d7());
        C.ManaAdvancement.Set(1, Dice.One);
        C.SetDistribution(Attributes.wisdom, Attributes.constitution, Attributes.dexterity, Attributes.strength, Attributes.charisma, Attributes.intelligence);
        C.AddAvatar(Genders.male, Glyphs.male_hunter);
        C.AddAvatar(Genders.female, Glyphs.female_hunter);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_hunter);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_hunter);
        C.AddFeat(5, Properties.searching);
        C.AddFeat(10, Elements.cold);
        C.AddFeat(15, Properties.clarity);
        C.AddFeat(20, Properties.free_action);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.crafting, Skills.dual_wielding, Skills.music, Skills.riding, Skills.swimming, Skills.traps, Skills.literacy,
          Skills.light_armour,
          Skills.firearms);
        C.Startup.Loot.AddKit(Chance.Always, 1.d120() + 120, Modifier.Plus0, Items.bullet);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.hunting_rifle);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.assault_rifle);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus2, Items.oilskin_cloak);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus2, Items.low_boots);
        C.Startup.Loot.AddKit(Chance.Always, Items.food_ration);
        C.Startup.Loot.AddKit(Chance.Always, Items.tinning_kit);
        C.Startup.Loot.AddKit(Chance.Always, Items.magic_whistle);
        C.Startup.Loot.AddKit(Chance.Always, Items.beartrap);
        C.Startup.Loot.AddKit(Chance.Always, Items.brass_bugle);
        C.Startup.Loot.AddKit(Chance.Always, 1.d3() + 1, Modifier.Plus0, Items.frag_grenade);
      });

      jester = AddClass(C =>
      {
        C.Name = "jester";
        C.Description = "Daft harlequins, always carrying a trick up their sleeves. Eclectic and eccentric; they are unassuming masters of chaos, adept in many bewildering talents.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(11, 1.d6());
        C.ManaAdvancement.Set(3, 1.d2());
        C.SetDistribution(Attributes.charisma, Attributes.intelligence, Attributes.dexterity, Attributes.constitution, Attributes.strength, Attributes.wisdom);
        C.AddAvatar(Genders.male, Glyphs.male_jester);
        C.AddAvatar(Genders.female, Glyphs.female_jester);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_jester);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_jester);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_jester);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_jester);
        C.AddAvatar(Races.troll, Genders.male, Glyphs.troll_male_jester);
        C.AddAvatar(Races.orc, Genders.male, Glyphs.orc_male_jester);
        C.AddAvatar(Races.orc, Genders.female, Glyphs.orc_female_jester);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_jester);
        C.AddFeat(2, Properties.see_invisible);
        C.AddFeat(4, Properties.quickness);
        C.AddFeat(8, Properties.free_action);
        C.AddFeat(12, Elements.shock);
        C.AddFeat(16, Elements.drain);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.literacy, Skills.music, Skills.swimming,
          Skills.abjuration, Skills.enchantment, Skills.divination,
          Skills.light_armour,
          Skills.light_blade,
          Skills.disc, Skills.mace, Skills.whip);
        C.Startup.AddGrimoire(Dice.One, Spells.confusion);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.morning_star);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.boomerang);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.leather_cloak);
        C.Startup.Loot.AddKit(Chance.Always, Items.bag_of_tricks);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(1), Items.potion_of_polymorph);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(1), Items.potion_of_invisibility);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(1), Items.potion_of_sleeping);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(5), Items.fortune_cookie);
        C.Startup.Loot.AddKit(Chance.Always, Items.brass_bugle);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(13), Items.cream_pie);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(1), Items.banana);
        C.Startup.Loot.AddKit(Chance.Always, 1.d50() + 51, Items.gold_coin);
      });

      knight = AddClass(C =>
      {
        C.Name = "knight";
        C.Description = "Venerable men-at-arms clad in gleaming armour, renowned for their unyielding chivalry and conviction. They are high-born and are educated to be strategists and capable leaders on any battlefront.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(14, 1.d8());
        C.ManaAdvancement.Set(3, Dice.One);
        C.SetDistribution(Attributes.strength, Attributes.constitution, Attributes.intelligence, Attributes.wisdom, Attributes.dexterity, Attributes.charisma);
        C.AddAvatar(Genders.male, Glyphs.male_knight);
        C.AddAvatar(Genders.female, Glyphs.female_knight);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_knight);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_knight);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_knight);
        C.AddAvatar(Races.lizardman, Genders.female, Glyphs.lizardman_female_knight);
        C.AddAvatar(Races.orc, Genders.male, Glyphs.orc_male_knight);
        C.AddAvatar(Races.orc, Genders.female, Glyphs.orc_female_knight);
        C.AddAvatar(Races.dwarf, Genders.male, Glyphs.dwarf_male_knight);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_knight);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_knight);
        C.AddAvatar(Races.troll, Genders.male, Glyphs.troll_male_knight);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_knight);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_knight);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_knight);
        C.AddFeat(8, Properties.quickness);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.dual_wielding, Skills.literacy, Skills.riding,
          Skills.evocation,
          Skills.light_blade, Skills.medium_blade, Skills.heavy_blade,
          Skills.light_armour, Skills.medium_armour, Skills.heavy_armour,
          Skills.axe, Skills.crossbow, Skills.lance, Skills.mace, Skills.polearm, Skills.spear, Skills.hammer, Skills.flail);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.long_sword);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.ring_mail);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.helmet);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.small_shield);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.leather_gloves);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.lance);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(10), Items.apple);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(10), Items.carrot);
      });

      monk = AddClass(C =>
      {
        C.Name = "monk";
        C.Description = "Pious scholars, oath-bound to their scripture. Although altruistic and good-natured, they are trained in martial arts so as to protect themselves and the canon they live by.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(12, 1.d8());
        C.ManaAdvancement.Set(4, 1.d2());
        C.SetDistribution(Attributes.strength, Attributes.wisdom, Attributes.dexterity, Attributes.constitution, Attributes.intelligence, Attributes.charisma);
        C.AddAvatar(Genders.male, Glyphs.male_monk);
        C.AddAvatar(Genders.female, Glyphs.female_monk);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_monk);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_monk);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_monk);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_monk);
        C.AddFeat(2, Properties.quickness);
        C.AddFeat(4, Properties.see_invisible);
        C.AddFeat(6, Elements.poison);
        C.AddFeat(8, Elements.sleep);
        C.AddFeat(10, Properties.stealth);
        C.AddFeat(12, Properties.warning);
        C.AddFeat(14, Elements.fire);
        C.AddFeat(16, Elements.cold);
        C.AddFeat(18, Elements.shock);
        C.AddFeat(20, Properties.teleport_control);
        C.AddFeat(22, Properties.telekinesis);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.crafting, Skills.literacy,
          Skills.clerical, Skills.conjuration,
          Skills.light_armour,
          Skills.spear, Skills.unarmed_combat);
        C.Startup.AddGrimoire(Dice.One, DesireableSpellArray(C));
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus2, Items.leather_gloves);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.robe);
        C.Startup.Loot.AddKit(Chance.Always, DesirableItemArray(C, Stocks.scroll));
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(3), Items.potion_of_healing);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(3), Items.food_ration);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(5), Items.apple);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(5), Items.orange);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(3), Items.fortune_cookie);
        C.Startup.Loot.AddKit(Chance.OneIn5, Items.magic_marker);
        C.Startup.Loot.AddKit(Chance.OneIn10, Items.oil_lamp);
        C.Startup.SetRecognition(OrientalItemArray);
      });

      mystic = AddClass(C =>
      {
        C.Name = "mystic";
        C.Description = "After many years of intense contemplation these soothsayers are ready to venture into the world. Masters of their own mind, they are capable of acts of formidable willpower.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(10, 1.d6());
        C.ManaAdvancement.Set(6, 1.d2());
        C.SetDistribution(Attributes.wisdom, Attributes.charisma, Attributes.intelligence, Attributes.dexterity, Attributes.constitution, Attributes.strength);
        C.AddAvatar(Genders.male, Glyphs.male_mystic);
        C.AddAvatar(Genders.female, Glyphs.female_mystic);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_mystic);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_mystic);
        C.AddFeat(1, Properties.telepathy);
        C.AddFeat(2, Properties.telekinesis);
        C.AddFeat(4, Elements.fire);
        C.AddFeat(6, Properties.warning);
        C.AddFeat(8, Elements.cold);
        C.AddFeat(10, Properties.clairvoyance);
        C.AddFeat(12, Elements.sleep);
        C.AddFeat(14, Properties.clarity);
        C.AddFeat(16, Elements.petrify);
        C.AddFeat(18, Properties.teleport_control);
        C.AddFeat(20, Elements.drain);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.crafting, Skills.dual_wielding, Skills.literacy, Skills.music, Skills.riding, Skills.swimming,
          Skills.divination, Skills.enchantment, Skills.evocation, Skills.transmutation,
          Skills.light_armour,
          Skills.light_blade,
          Skills.dart, Skills.sling, Skills.staff);
        C.Startup.AddGrimoire(Dice.One, Spells.identify);
        C.Startup.AddGrimoire(Dice.One, DesireableSpellArray(C));
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Modifier.Plus1, Items.stiletto);
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Modifier.Plus1, Items.stiletto);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.cloak_of_deflection);
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Items.potion_of_hallucination);
        C.Startup.Loot.AddKit(Chance.Always, Items.leather_drum);
        C.Startup.Loot.AddKit(Chance.Always, Items.blindfold);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Items.food_ration);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Items.fortune_cookie);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Items.cheese);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Items.pear);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Items.mushroom);
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Items.crystal_ball);
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, new[] { Items.bell_of_secrets, Items.bell_of_resources });
        C.Startup.Loot.AddKit(Chance.OneIn4, Items.magic_whistle);
        C.Startup.Loot.AddKit(Chance.OneIn20, Items.porter);
      });

      ninja = AddClass(C =>
      {
        C.Name = "ninja";
        C.Description = "Hidden agents, fleet-footed and schooled in many ancient arts of assassination. When all else fails, they are indubitably prepared to make swift escapes.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(11, 1.d7());
        C.ManaAdvancement.Set(1, Dice.One);
        C.SetDistribution(Attributes.dexterity, Attributes.strength, Attributes.constitution, Attributes.intelligence, Attributes.wisdom, Attributes.charisma);
        C.AddAvatar(Genders.male, Glyphs.male_ninja);
        C.AddAvatar(Genders.female, Glyphs.female_ninja);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_ninja);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_ninja);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_ninja);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_ninja);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_ninja);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_ninja);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_ninja);
        C.AddAvatar(Races.lizardman, Genders.female, Glyphs.lizardman_female_ninja);
        C.AddFeat(1, Properties.stealth);
        C.AddFeat(5, Properties.jumping);
        C.AddFeat(10, Properties.quickness);
        C.AddFeat(15, Properties.see_invisible);
        C.AddFeat(20, Properties.warning);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.dual_wielding, Skills.locks, Skills.literacy, Skills.traps,
          Skills.enchantment,
          Skills.light_armour, Skills.medium_blade,
          Skills.light_blade,
          Skills.polearm, Skills.flail, Skills.disc, Skills.unarmed_combat);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, new[] { Items.nunchaku, Items.sai });
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, new[] { Items.nunchaku, Items.sai });
        C.Startup.Loot.AddKit(Dice.One, Chance.Always, 1.d20() + 26, Sanctities.Blessed, new[] { Modifier.Plus1 }, new[] { Items.shuriken }); // blessed 21-47
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus3, Items.studded_leather_armour);
        C.Startup.Loot.AddKit(Chance.Always, 2.d3(), Items.caltrops);
        C.Startup.Loot.AddKit(Chance.Always, 1.d3(), Items.lock_pick);
        C.Startup.SetRecognition(OrientalItemArray);
      });

      paladin = AddClass(C =>
      {
        C.Name = "paladin";
        C.Description = "Sworn to uphold justice, these righteous warriors are dogmatic in pursuit of their sacred work. Classical martial skills are complemented by magical powers to protect the innocent and smite the wicked.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(13, 1.d8());
        C.ManaAdvancement.Set(6, Dice.One);
        C.SetDistribution(Attributes.strength, Attributes.charisma, Attributes.constitution, Attributes.intelligence, Attributes.wisdom, Attributes.dexterity);
        C.AddAvatar(Genders.male, Glyphs.male_paladin);
        C.AddAvatar(Genders.female, Glyphs.female_paladin);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_paladin);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_paladin);
        C.AddAvatar(Races.elf, Genders.male, Glyphs.elf_male_paladin);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_paladin);
        C.AddFeat(1, Properties.clarity);
        C.AddFeat(5, Properties.beatitude);
        C.AddFeat(10, Properties.quickness);
        C.AddFeat(15, Properties.warning);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.riding, Skills.literacy,
          Skills.clerical,
          Skills.medium_armour, Skills.heavy_armour,
          Skills.medium_blade, Skills.heavy_blade);
        C.Startup.AddGrimoire(Dice.One, Spells.healing);
        C.Startup.AddGrimoire(Dice.One, Spells.turn_undead);
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Modifier.Plus0, Items.long_sword);
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Modifier.Plus0, Items.bronze_plate_mail);
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Modifier.Plus0, Items.large_shield);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(1), Sanctities.Blessed, Items.holy_wafer);
      });

      pirate = AddClass(C =>
      {
        C.Name = "pirate";
        C.Description = "Freebooting buccaneers, sailing the high seas and plundering many a merchant's vessel in their endless pursuit of riches and infamy. They are murderous cutthroats with little regard for anything but gold.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(11, 1.d8());
        C.ManaAdvancement.Set(1, Dice.One);
        C.SetDistribution(Attributes.constitution, Attributes.dexterity, Attributes.strength, Attributes.intelligence, Attributes.wisdom, Attributes.charisma);
        C.AddAvatar(Genders.male, Glyphs.male_pirate);
        C.AddAvatar(Genders.female, Glyphs.female_pirate);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_pirate);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_pirate);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_pirate);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_pirate);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_pirate);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_pirate);
        C.AddFeat(1, Properties.searching);
        C.AddFeat(5, Properties.stealth);
        C.AddFeat(10, Properties.vitality);
        C.AddFeat(15, Properties.appraisal);
        C.AddFeat(20, Properties.cannibalism);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.crafting, Skills.dual_wielding, Skills.locks, Skills.music, Skills.swimming,
          Skills.light_armour, Skills.medium_armour,
          Skills.light_blade, Skills.medium_blade,
          Skills.axe, Skills.spear, Skills.unarmed_combat, Skills.firearms);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.silver_sabre);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.oilskin_cloak);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.low_boots);
        C.Startup.Loot.AddKit(Chance.Always, Items.sack);
        C.Startup.Loot.AddKit(Chance.Always, Items.lantern);
        C.Startup.Loot.AddKit(Chance.Always, Items.fish);
        C.Startup.Loot.AddKit(Chance.Always, Items.potion_of_booze);
        C.Startup.Loot.AddKit(Chance.Always, Items.scroll_of_magic_mapping);
        C.Startup.Loot.AddKit(Chance.Always, 1.d2() + 1, Items.skeleton_key);
        C.Startup.Loot.AddKit(Chance.Always, 1.d50() + 50, Items.gold_coin);
      });

      priest = AddClass(C =>
      {
        C.Name = "priest";
        C.Description = "Righteous clergymen, dedicated to a life of sacrifice and spreading the word of their almighty deities. They are well-versed in divine magic and the judicial delivery of violence to those they deem unworthy of salvation.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(12, 1.d8());
        C.ManaAdvancement.Set(6, 1.d2());
        C.SetDistribution(Attributes.wisdom, Attributes.constitution, Attributes.strength, Attributes.dexterity, Attributes.intelligence, Attributes.charisma);
        C.AddAvatar(Genders.male, Glyphs.male_priest);
        C.AddAvatar(Genders.female, Glyphs.female_priest);
        C.AddAvatar(Races.elf, Genders.male, Glyphs.elf_male_priest);
        C.AddAvatar(Races.elf, Genders.female, Glyphs.elf_female_priest);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_priest);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_priest);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_priest);
        C.AddAvatar(Races.lizardman, Genders.female, Glyphs.lizardman_female_priest);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_priest);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_priest);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_priest);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_priest);
        C.AddFeat(1, Properties.beatitude);
        C.AddFeat(8, Properties.warning);
        C.AddFeat(14, Elements.fire);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.crafting, Skills.literacy,
          Skills.clerical, Skills.conjuration, Skills.divination, Skills.necromancy,
          Skills.light_armour, Skills.medium_armour,
          Skills.club, Skills.mace, Skills.flail, Skills.hammer, Skills.staff);
        C.Startup.AddGrimoire(Dice.One, DesireableSpellArray(C));
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Modifier.Plus1, Items.mace);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.robe);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.small_shield);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(4), Sanctities.Blessed, Items.potion_of_water);
        C.Startup.Loot.AddKit(Chance.Always, Items.clove_of_garlic);
        C.Startup.Loot.AddKit(Chance.Always, Items.sprig_of_wolfsbane);
        C.Startup.Loot.AddKit(Chance.OneIn10, Items.magic_marker);
        C.Startup.Loot.AddKit(Chance.OneIn10, Items.oil_lamp);
      });

      templar = AddClass(C =>
      {
        C.Name = "templar";
        C.Description = "Questionably pious individuals from an ancient religious and military order. They abuse this power and influence to crusade against practitioners of the occult and supernatural beings.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(12, 1.d9());
        C.ManaAdvancement.Set(1, Dice.One);
        C.SetDistribution(Attributes.strength, Attributes.dexterity, Attributes.wisdom, Attributes.constitution, Attributes.charisma, Attributes.intelligence);
        C.AddAvatar(Genders.male, Glyphs.male_templar);
        C.AddAvatar(Genders.female, Glyphs.female_templar);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_templar);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_templar);
        C.AddAvatar(Races.elf, Genders.male, Glyphs.elf_male_templar);
        C.AddFeat(4, Properties.see_invisible);
        C.AddFeat(8, Elements.sleep);
        C.AddFeat(12, Properties.free_action);
        C.AddFeat(16, Elements.drain); // frees up amulet slot.
        C.AddFeat(20, Properties.beatitude);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.dual_wielding, Skills.crafting, Skills.literacy, Skills.riding, Skills.swimming,
          Skills.clerical, 
          Skills.light_blade, Skills.medium_blade, Skills.heavy_blade,
          Skills.light_armour, Skills.medium_armour, Skills.heavy_armour,
          Skills.club, Skills.mace, Skills.flail, Skills.hammer, Skills.spear, Skills.staff);
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Modifier.Plus1, new[] { Items.silver_mace, Items.silver_long_sword, Items.silver_spear } );
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.shield_of_reflection);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.helmet);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.banded_mail);
        C.Startup.Loot.AddKit(Chance.Always, Items.amulet_of_drain_resistance);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Sanctities.Blessed, Items.potion_of_water);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Items.food_ration);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Items.clove_of_garlic);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Items.sprig_of_wolfsbane);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Items.torch);
        C.Startup.Loot.AddKit(Chance.Always, Items.wooden_stake);
        C.Startup.Loot.AddKit(Chance.Always, 1.d100() + 150, Items.gold_coin);
        C.Startup.Loot.AddKit(Chance.OneIn4, Items.fly_swatter);
        C.Startup.Loot.AddKit(Chance.OneIn4, Items.wand_of_undead_turning);
      });

      reaver = AddClass(C =>
      {
        C.Name = "reaver";
        C.Description = "This corrupt and hateful soul is motivated only by carnage and chaos. Subject to uncontrollable bouts of rage, they bring doom and destruction to friend and foe alike.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(18, 1.d12());
        C.ManaAdvancement.Set(1, Dice.Zero);
        C.SetDistribution(Attributes.constitution, Attributes.strength, Attributes.dexterity, Attributes.wisdom, Attributes.charisma, Attributes.intelligence);
        C.AddAvatar(Genders.male, Glyphs.male_reaver);
        C.AddAvatar(Genders.female, Glyphs.female_reaver);
        C.AddAvatar(Races.angel, Genders.male, Glyphs.angel_male_reaver);
        C.AddAvatar(Races.angel, Genders.female, Glyphs.angel_female_reaver);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_reaver);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_reaver);
        C.AddAvatar(Races.dwarf, Genders.male, Glyphs.dwarf_male_reaver);
        C.AddAvatar(Races.dwarf, Genders.female, Glyphs.dwarf_female_reaver);
        C.AddAvatar(Races.elf, Genders.male, Glyphs.elf_male_reaver);
        C.AddAvatar(Races.elf, Genders.female, Glyphs.elf_female_reaver);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_reaver);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_reaver);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_reaver);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_reaver);
        C.AddAvatar(Races.halfling, Genders.male, Glyphs.halfling_male_reaver);
        C.AddAvatar(Races.halfling, Genders.female, Glyphs.halfling_female_reaver);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_reaver);
        C.AddAvatar(Races.lizardman, Genders.female, Glyphs.lizardman_female_reaver);
        C.AddAvatar(Races.gnome, Genders.male, Glyphs.gnome_male_reaver);
        C.AddAvatar(Races.gnome, Genders.female, Glyphs.gnome_female_reaver);
        C.AddAvatar(Races.orc, Genders.male, Glyphs.orc_male_reaver);
        C.AddAvatar(Races.orc, Genders.female, Glyphs.orc_female_reaver);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_reaver);
        C.AddAvatar(Races.satyr, Genders.female, Glyphs.satyr_female_reaver);
        C.AddAvatar(Races.troll, Genders.male, Glyphs.troll_male_reaver);
        C.AddAvatar(Races.troll, Genders.female, Glyphs.troll_female_reaver);
        C.AddFeat(1, Properties.berserking);
        C.AddFeat(4, Elements.poison);
        C.AddFeat(8, Properties.quickness);
        C.AddFeat(12, Elements.petrify);
        C.AddFeat(16, Properties.cannibalism);
        C.AddFeat(20, Elements.drain);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.dual_wielding, Skills.literacy,
          Skills.necromancy,
          Skills.light_armour, Skills.medium_armour, Skills.heavy_armour,
          Skills.light_blade, Skills.medium_blade, Skills.heavy_blade,
          Skills.unarmed_combat, Skills.axe, Skills.hammer, Skills.mace);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus3, Items.battleaxe);
        C.Startup.Loot.AddKit(Chance.Always, 1.d3() + 2, Modifier.Plus0, Items.hatchet); // 3-5
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus2, Items.scale_mail);
      });

      ranger = AddClass(C =>
      {
        C.Name = "ranger";
        C.Description = "Wilderness sentinels who understand the true balance of the universe. With bow in hand, they have devoted their lives to protecting the forces of nature from the corrupting touch of civilisation.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(13, 1.d7());
        C.ManaAdvancement.Set(1, Dice.One);
        C.SetDistribution(Attributes.strength, Attributes.dexterity, Attributes.constitution, Attributes.intelligence, Attributes.wisdom, Attributes.charisma);
        C.AddAvatar(Genders.male, Glyphs.male_ranger);
        C.AddAvatar(Genders.female, Glyphs.female_ranger);
        C.AddAvatar(Races.elf, Genders.male, Glyphs.elf_male_ranger);
        C.AddAvatar(Races.elf, Genders.female, Glyphs.elf_female_ranger);
        C.AddAvatar(Races.gnome, Genders.male, Glyphs.gnome_male_ranger);
        C.AddAvatar(Races.gnome, Genders.female, Glyphs.gnome_female_ranger);
        C.AddAvatar(Races.orc, Genders.male, Glyphs.orc_male_ranger);
        C.AddAvatar(Races.orc, Genders.female, Glyphs.orc_female_ranger);
        C.AddAvatar(Races.dwarf, Genders.male, Glyphs.dwarf_male_ranger);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_ranger);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_ranger);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_ranger);
        C.AddAvatar(Races.lizardman, Genders.female, Glyphs.lizardman_female_ranger);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_ranger);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_ranger);
        C.AddAvatar(Races.troll, Genders.male, Glyphs.troll_male_ranger);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_ranger);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_ranger);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_ranger);
        C.AddFeat(2, Properties.searching);
        C.AddFeat(8, Properties.stealth);
        C.AddFeat(14, Properties.see_invisible);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.crafting, Skills.dual_wielding, Skills.literacy, Skills.riding, Skills.swimming, Skills.traps,
          Skills.divination,
          Skills.light_armour, Skills.medium_armour,
          Skills.light_blade, Skills.medium_blade,
          Skills.axe, Skills.disc, Skills.bow, Skills.crossbow, Skills.dart, Skills.flail, Skills.polearm, Skills.sling, Skills.spear);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.bow);
        C.Startup.Loot.AddKit(Chance.Always, 1.d10() + 50, Modifier.Plus2, Items.arrow); // 51-60
        C.Startup.Loot.AddKit(Chance.Always, 1.d10() + 30, Modifier.Plus0, Items.arrow); // 31-40
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.short_sword);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus2, Items.cloak_of_displacement);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(4), Items.cram_ration);
      });

      rogue = AddClass(C =>
      {
        C.Name = "rogue";
        C.Description = "Deft miscreants, conniving and quick with their wits. Through trial and error, they have learned that their best chance in avoiding the clutches of death is to stick to the shadows and outmanoeuvre their pursuers.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(10, 1.d7());
        C.ManaAdvancement.Set(1, Dice.One);
        C.SetDistribution(Attributes.dexterity, Attributes.strength, Attributes.constitution, Attributes.intelligence, Attributes.wisdom, Attributes.charisma);
        C.AddAvatar(Genders.male, Glyphs.male_rogue);
        C.AddAvatar(Genders.female, Glyphs.female_rogue);
        C.AddAvatar(Races.orc, Genders.male, Glyphs.orc_male_rogue);
        C.AddAvatar(Races.orc, Genders.female, Glyphs.orc_female_rogue);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_rogue);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_rogue);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_rogue);
        C.AddAvatar(Races.lizardman, Genders.female, Glyphs.lizardman_female_rogue);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_rogue);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_rogue);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_rogue);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_rogue);
        C.AddFeat(2, Properties.stealth);
        C.AddFeat(6, Properties.searching);
        C.AddFeat(10, Properties.appraisal);
        C.AddFeat(14, Properties.see_invisible);
        C.AddFeat(18, Properties.deflection);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.dual_wielding, Skills.literacy, Skills.locks, Skills.swimming, Skills.traps,
          Skills.abjuration, Skills.divination, Skills.transmutation,
          Skills.light_armour,
          Skills.light_blade, Skills.medium_blade,
          Skills.club, Skills.crossbow, Skills.dart, Skills.mace, Skills.disc);
        C.Startup.Loot.AddKit(Chance.Always, 1.d250() + 250, Items.gold_coin);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.short_sword);
        C.Startup.Loot.AddKit(Chance.Always, 1.d10() + 6, Modifier.Plus0, Items.dagger); // 7-16
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.leather_armour);
        C.Startup.Loot.AddKit(Chance.Always, Items.potion_of_sickness);
        C.Startup.Loot.AddKit(Chance.Always, 1.d9(), Items.lock_pick);
        C.Startup.Loot.AddKit(Chance.Always, Items.sack);
        C.Startup.Loot.AddKit(Chance.Always, Items.wand_of_theft);
        C.Startup.Loot.AddKit(Chance.OneIn5, Items.blindfold);
      });

      samurai = AddClass(C =>
      {
        C.Name = "samurai";
        C.Description = "Honourable warriors, who have dedicated their lives to an ancient code. Skilled in art of dual-wielding blades, they make fast work of their enemies.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(13, 1.d8());
        C.ManaAdvancement.Set(1, Dice.One);
        C.SetDistribution(Attributes.strength, Attributes.dexterity, Attributes.constitution, Attributes.intelligence, Attributes.wisdom, Attributes.charisma);
        C.AddAvatar(Genders.male, Glyphs.male_samurai);
        C.AddAvatar(Genders.female, Glyphs.female_samurai);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_samurai);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_samurai);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_samurai);
        C.AddAvatar(Races.lizardman, Genders.female, Glyphs.lizardman_female_samurai);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_samurai);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_samurai);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_samurai);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_samurai);
        C.AddAvatar(Races.orc, Genders.male, Glyphs.orc_male_samurai);
        C.AddAvatar(Races.orc, Genders.female, Glyphs.orc_female_samurai);
        C.AddAvatar(Races.troll, Genders.male, Glyphs.troll_male_samurai);
        C.AddAvatar(Races.troll, Genders.female, Glyphs.troll_female_samurai);
        C.AddFeat(2, Properties.quickness);
        C.AddFeat(16, Properties.stealth);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.dual_wielding, Skills.literacy, Skills.riding,
          Skills.clerical, Skills.evocation,
          Skills.light_armour, Skills.medium_armour, Skills.heavy_armour,
          Skills.light_blade, Skills.medium_blade, Skills.heavy_blade,
          Skills.bow, Skills.flail, Skills.lance, Skills.polearm, Skills.disc);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.katana);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.wakizashi);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.yumi);
        C.Startup.Loot.AddKit(Chance.Always, 1.d20() + 26, Modifier.Plus0, Items.ya); // 27-46
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.splint_mail);
        C.Startup.Loot.AddKit(Chance.OneIn5, Items.blindfold);
        C.Startup.SetRecognition(OrientalItemArray);
      });

      shaman = AddClass(C =>
      {
        C.Name = "shaman";
        C.Description = "Keepers of cosmic secrets, who derive their sacred powers from the spirit realms hidden from mortal eyes. They are compelled by dark forces and are adept at bending the material world to their will.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(10, 1.d6());
        C.ManaAdvancement.Set(5, 1.d2());
        C.SetDistribution(Attributes.wisdom, Attributes.intelligence, Attributes.strength, Attributes.constitution, Attributes.dexterity, Attributes.charisma);
        C.AddAvatar(Genders.male, Glyphs.male_shaman);
        C.AddAvatar(Genders.female, Glyphs.female_shaman);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_shaman);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_shaman);
        //C.AddAvatar(Races.angel, Genders.male, Glyphs.angel_male_shaman);
        C.AddAvatar(Races.angel, Genders.female, Glyphs.angel_female_shaman);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_shaman);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_shaman);
        C.AddAvatar(Races.orc, Genders.male, Glyphs.orc_male_shaman);
        C.AddAvatar(Races.orc, Genders.female, Glyphs.orc_female_shaman);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_shaman);
        C.AddAvatar(Races.satyr, Genders.female, Glyphs.satyr_female_shaman);
        C.AddFeat(4, Elements.poison);
        C.AddFeat(10, Properties.polymorph_control);
        C.AddFeat(14, Properties.telekinesis);
        C.AddFeat(18, Properties.clairvoyance);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.crafting, Skills.literacy,
          Skills.abjuration, Skills.divination, Skills.enchantment, Skills.evocation, Skills.necromancy,
          Skills.light_armour,
          Skills.light_blade,
          Skills.axe, Skills.club, Skills.dart, Skills.sling, Skills.staff);
        C.Startup.AddGrimoire(Dice.One, Spells.magic_missile);
        C.Startup.AddGrimoire(Dice.One, DesireableSpellArray(C));
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Modifier.Plus1, new[] { Items.dread_staff, Items.flash_staff, Items.thunder_staff });
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Modifier.Plus1, Items.blowgun);
        C.Startup.Loot.AddKit(Chance.Always, 1.d10() + 11, Modifier.Plus1, Items.dart); // 12-21
        C.Startup.Loot.AddKit(Chance.Always, 1.d5() + 5, Modifier.Plus0, Items.poison_dart); // 6-10
        C.Startup.Loot.AddKit(Dice.Fixed(1), Chance.Always, DesirableItemArray(C, Stocks.food));
        C.Startup.Loot.AddKit(Dice.Fixed(1), Chance.Always, DesirableItemArray(C, Stocks.amulet));
        C.Startup.Loot.AddKit(Dice.Fixed(5), Chance.Always, DesirableItemArray(C, Stocks.potion));
      });

      druid = AddClass(C =>
      {
        C.Name = "druid";
        C.Description = "Conductors of natural energies, these uniquely attuned spell casters derive their powers from the living world. Peaceful until threatened, they are unafraid to respond with animalistic savagery.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(10, 1.d6());
        C.ManaAdvancement.Set(5, 1.d2());
        C.SetDistribution(Attributes.wisdom, Attributes.charisma, Attributes.dexterity, Attributes.constitution, Attributes.intelligence, Attributes.strength);
        C.AddAvatar(Genders.male, Glyphs.male_druid);
        C.AddAvatar(Genders.female, Glyphs.female_druid);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_druid);
        C.AddAvatar(Races.satyr, Genders.female, Glyphs.satyr_female_druid);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_druid);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_druid);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_druid);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_druid);
        C.AddAvatar(Races.troll, Genders.male, Glyphs.troll_male_druid);
        C.AddFeat(4, Properties.warning);
        C.AddFeat(8, Properties.teleport_control);
        C.AddFeat(16, Properties.polymorph_control);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.crafting, Skills.literacy, Skills.riding, Skills.swimming, Skills.traps,
          Skills.abjuration, Skills.divination, Skills.enchantment, Skills.transmutation, Skills.conjuration, 
          Skills.light_armour,
          Skills.light_blade,
          Skills.disc, Skills.club, Skills.polearm, Skills.staff);
        C.Startup.AddGrimoire(Dice.One, Spells.summoning);
        C.Startup.AddGrimoire(Dice.One, DesireableSpellArray(C));
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Modifier.Plus1, Items.scythe);
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Modifier.Plus1, Items.sickle);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.boomerang);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, new[] { Items.battle_robe, Items.elemental_robe, Items.fleet_robe });
        C.Startup.Loot.AddKit(Dice.Fixed(3), Chance.Always, Dice.Fixed(2), Sanctity: null, new Modifier[] { } , new[] { Items.banana, Items.apple, Items.orange, Items.melon, Items.carrot, Items.pear, Items.cheese, Items.slime_mould, Items.fish });
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Items.eucalyptus_leaf);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(2), Items.mushroom);
        C.Startup.Loot.AddKit(Dice.Fixed(1), Chance.Always, DesirableItemArray(C, Stocks.ring));
        C.Startup.Loot.AddKit(Dice.Fixed(1), Chance.Always, DesirableItemArray(C, Stocks.wand));
      });

      tinker = AddClass(C =>
      {
        C.Name = "tinker";
        C.Description = "Devoted to understanding the essence of magic and the marvel of engineering, these adept craftsman crave both knowledge and resources. Whether directed by madness or genius, they are a capable adversary and a tenacious survivor.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(12, 1.d6());
        C.ManaAdvancement.Set(1, 1.d2());
        C.SetDistribution(Attributes.dexterity, Attributes.intelligence, Attributes.constitution, Attributes.wisdom, Attributes.strength, Attributes.charisma);
        C.AddAvatar(Genders.male, Glyphs.male_tinker);
        C.AddAvatar(Genders.female, Glyphs.female_tinker);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_tinker);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_tinker);
        C.AddFeat(2, Properties.searching);
        C.AddFeat(10, Properties.appraisal);
        C.AddFeat(20, Properties.vitality);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.literacy, Skills.locks, Skills.traps, Skills.crafting,
          Skills.abjuration, Skills.divination, Skills.transmutation, Skills.enchantment, Skills.evocation,
          Skills.light_armour, Skills.medium_armour,
          Skills.light_blade, 
          Skills.club, Skills.crossbow, Skills.hammer, Skills.mace, Skills.pick);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus2, Items.war_hammer);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus3, Items.alchemy_smock);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.dented_pot);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.leather_gloves);
        C.Startup.Loot.AddKit(Chance.Always, Dice.One, Items.potion_of_acid);
        C.Startup.Loot.AddKit(Chance.Always, Dice.One, Items.potion_of_oil);
        C.Startup.Loot.AddKit(Chance.Always, Dice.One, Items.potion_of_ink);
        C.Startup.Loot.AddKit(Chance.Always, Dice.One, Items.potion_of_water);
        C.Startup.Loot.AddKit(Chance.Always, Dice.One, Items.can_of_grease);
        C.Startup.Loot.AddKit(Chance.Always, Dice.One, Items.earmuffs);
        C.Startup.Loot.AddKit(Chance.Always, 1.d9(), Items.lock_pick);
        C.Startup.Loot.AddKit(Dice.Fixed(2), Chance.Always, DesirableItemArray(C, Stocks.scroll));
        C.Startup.Loot.AddKit(Chance.Always, Dice.Two, Items.sandwich);
        C.Startup.Loot.AddKit(Chance.OneIn5, Items.pickaxe);
        C.Startup.Loot.AddKit(Chance.OneIn10, Items.magic_marker);
        C.Startup.Loot.AddKit(Chance.OneIn10, Items.magic_figurine);
      });
      
      tourist = AddClass(C =>
      {
        C.Name = "tourist";
        C.Description = "Happy-go-lucky travellers, committed to only the most authentic experiences. Quixotic and brave, they meticulously prepare with a wide variety of paraphernalia - yet remain ill-equipped for the perils they will soon face.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(8, 1.d6());
        C.ManaAdvancement.Set(1, Dice.One);
        C.SetDistribution(Attributes.constitution, Attributes.charisma, Attributes.strength, Attributes.dexterity, Attributes.intelligence, Attributes.wisdom);
        C.AddAvatar(Genders.male, Glyphs.male_tourist);
        C.AddAvatar(Genders.female, Glyphs.female_tourist);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_tourist);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_tourist);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_tourist);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_tourist);
        C.AddFeat(8, Properties.searching);
        C.AddFeat(14, Elements.poison);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.literacy, Skills.swimming,
          Skills.abjuration,
          Skills.light_armour,
          Skills.light_blade,
          Skills.dart);
        C.Startup.Loot.AddKit(Chance.Always, 1.d20() + 21, Modifier.Plus2, Items.dart); // 27-46
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.hawaiian_shirt);
        C.Startup.Loot.AddKit(Chance.Always, Items.expensive_camera);
        C.Startup.Loot.AddKit(Chance.Always, Items.sack);
        C.Startup.Loot.AddKit(Dice.Fixed(2), Chance.Always, DesirableItemArray(C, Stocks.food));
        C.Startup.Loot.AddKit(Dice.Fixed(2), Chance.Always, DesirableItemArray(C, Stocks.food));
        C.Startup.Loot.AddKit(Dice.Fixed(2), Chance.Always, DesirableItemArray(C, Stocks.food));
        C.Startup.Loot.AddKit(Dice.Fixed(2), Chance.Always, DesirableItemArray(C, Stocks.food));
        C.Startup.Loot.AddKit(Dice.Fixed(2), Chance.Always, DesirableItemArray(C, Stocks.food));
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(4), Items.potion_of_extra_healing);
        C.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(4), Items.scroll_of_magic_mapping);
        C.Startup.Loot.AddKit(Chance.OneIn25, Items.magic_marker);
        C.Startup.Loot.AddKit(Chance.Always, 1.d500() + 500, Items.gold_coin);
      });

      valkyrie = AddClass(C =>
      {
        C.Name = "valkyrie";
        C.Description = "Champions of the winter realms, fierce and proud. Hardy and strong, they will boldly face any challenger until they meet a glorious death.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(14, 1.d8());
        C.ManaAdvancement.Set(1, Dice.One);
        C.SetDistribution(Attributes.strength, Attributes.constitution, Attributes.dexterity, Attributes.wisdom, Attributes.charisma, Attributes.intelligence);
        C.AddAvatar(Genders.female, Glyphs.female_valkyrie); // female glyph first for help screen.
        C.AddAvatar(Genders.male, Glyphs.male_valkyrie); // gender equality!
        C.AddAvatar(Races.dwarf, Genders.female, Glyphs.dwarf_female_valkyrie);
        C.AddAvatar(Races.dwarf, Genders.male, Glyphs.dwarf_male_valkyrie);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_valkyrie);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_valkyrie);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_valkyrie);
        C.AddAvatar(Races.lizardman, Genders.female, Glyphs.lizardman_female_valkyrie);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_valkyrie);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_valkyrie);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_valkyrie);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_valkyrie);
        C.AddFeat(2, Elements.cold);
        C.AddFeat(4, Properties.stealth);
        C.AddFeat(8, Properties.quickness);
        C.AddFeat(12, Elements.shock);
        C.AddFeat(16, Properties.vitality);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.dual_wielding, Skills.literacy, Skills.riding, Skills.swimming,
          Skills.light_armour, Skills.medium_armour, Skills.heavy_armour,
          Skills.light_blade, Skills.medium_blade, Skills.heavy_blade,
          Skills.axe, Skills.hammer, Skills.lance, Skills.pick, Skills.polearm, Skills.spear);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.spear);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.dagger);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus3, Items.small_shield);
        C.Startup.Loot.AddKit(Chance.Always, Items.food_ration);
        C.Startup.Loot.AddKit(Chance.OneIn6, Items.oil_lamp);
      });

      wizard = AddClass(C =>
      {
        C.Name = "wizard";
        C.Description = "Powerful sorcerers, lords of arcane knowledge, and vanguards of psychic warfare. They are unrivalled in their mastery of nearly all schools of magic, augmented by a wide array of mystical devices.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(10, 1.d6());
        C.ManaAdvancement.Set(6, 1.d2());
        C.SetDistribution(Attributes.intelligence, Attributes.dexterity, Attributes.charisma, Attributes.wisdom, Attributes.constitution, Attributes.strength);
        C.AddAvatar(Genders.male, Glyphs.male_wizard);
        C.AddAvatar(Genders.female, Glyphs.female_wizard);
        C.AddAvatar(Races.elf, Genders.male, Glyphs.elf_male_wizard);
        C.AddAvatar(Races.elf, Genders.female, Glyphs.elf_female_wizard);
        C.AddAvatar(Races.gnome, Genders.male, Glyphs.gnome_male_wizard);
        C.AddAvatar(Races.gnome, Genders.female, Glyphs.gnome_female_wizard);
        C.AddAvatar(Races.orc, Genders.male, Glyphs.orc_male_wizard);
        C.AddAvatar(Races.orc, Genders.female, Glyphs.orc_female_wizard);
        C.AddAvatar(Races.fairy, Genders.male, Glyphs.fairy_male_wizard);
        C.AddAvatar(Races.fairy, Genders.female, Glyphs.fairy_female_wizard);
        C.AddAvatar(Races.lizardman, Genders.male, Glyphs.lizardman_male_wizard);
        C.AddAvatar(Races.lizardman, Genders.female, Glyphs.lizardman_female_wizard);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_wizard);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_wizard);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_wizard);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_wizard);
        C.AddAvatar(Races.troll, Genders.male, Glyphs.troll_male_wizard);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_wizard);
        C.AddAvatar(Races.satyr, Genders.female, Glyphs.satyr_female_wizard);
        C.AddFeat(8, Properties.warning);
        C.AddFeat(14, Properties.teleport_control);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.bartering, Skills.crafting, Skills.literacy,
          Skills.abjuration, Skills.divination, Skills.enchantment, Skills.evocation, Skills.necromancy, Skills.transmutation,
          Skills.light_armour,
          Skills.light_blade,
          Skills.club, Skills.dart, Skills.sling, Skills.staff);
        C.Startup.AddGrimoire(Dice.One, Spells.force_bolt);
        C.Startup.AddGrimoire(Dice.One, DesireableSpellArray(C));
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Modifier.Plus1, Items.quarterstaff);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus0, Items.cloak_of_magic_resistance);
        C.Startup.Loot.AddKit(Chance.Always, DesirableItemArray(C, Stocks.wand));
        C.Startup.Loot.AddKit(Dice.Fixed(2), Chance.Always, DesirableItemArray(C, Stocks.ring));
        C.Startup.Loot.AddKit(Dice.Fixed(3), Chance.Always, DesirableItemArray(C, Stocks.potion));
        C.Startup.Loot.AddKit(Dice.Fixed(3), Chance.Always, DesirableItemArray(C, Stocks.scroll));
        C.Startup.Loot.AddKit(Chance.OneIn5, Items.magic_marker);
        C.Startup.Loot.AddKit(Chance.OneIn5, Items.blindfold);
      });
      
      necromancer = AddClass(C =>
      {
        C.Name = "necromancer";
        C.Description = "Obsessed with blurring the line between life and death, these meddlers of dark forces prefer to associate with cadavers over the living. To some, the rituals are grotesque, but they see pure majesty in the manipulation of the natural order.";
        C.Backpack = Items.Backpack;
        C.LifeAdvancement.Set(10, 1.d6());
        C.ManaAdvancement.Set(6, 1.d2());
        C.AddAvatar(Genders.male, Glyphs.male_necromancer);
        C.AddAvatar(Genders.female, Glyphs.female_necromancer);
        C.AddAvatar(Races.giant, Genders.male, Glyphs.giant_male_necromancer);
        C.AddAvatar(Races.giant, Genders.female, Glyphs.giant_female_necromancer);
        C.AddAvatar(Races.troll, Genders.male, Glyphs.troll_male_necromancer);
        C.AddAvatar(Races.orc, Genders.male, Glyphs.orc_male_necromancer);
        C.AddAvatar(Races.orc, Genders.female, Glyphs.orc_female_necromancer);
        C.AddAvatar(Races.demon, Genders.male, Glyphs.demon_male_necromancer);
        C.AddAvatar(Races.demon, Genders.female, Glyphs.demon_female_necromancer);
        C.AddAvatar(Races.satyr, Genders.male, Glyphs.satyr_male_necromancer);
        C.SetDistribution(Attributes.intelligence, Attributes.constitution, Attributes.dexterity, Attributes.wisdom, Attributes.strength, Attributes.charisma);
        C.AddFeat(8, Properties.vitality);
        C.AddFeat(14, Properties.clairvoyance);
        C.Startup.SetSkill(Qualifications.proficient,
          Skills.crafting, Skills.literacy,
          Skills.abjuration, Skills.conjuration, Skills.divination, Skills.enchantment, Skills.necromancy, Skills.transmutation,
          Skills.light_armour,
          Skills.light_blade,
          Skills.whip, Skills.mace, Skills.flail, Skills.staff);
        C.Startup.AddGrimoire(Dice.One, Spells.animate_dead);
        C.Startup.AddGrimoire(Dice.One, DesireableSpellArray(C));
        C.Startup.Loot.AddKit(Chance.Always, Sanctities.Blessed, Modifier.Plus1, Items.athame);
        C.Startup.Loot.AddKit(Chance.Always, Modifier.Plus1, Items.robe);
        C.Startup.Loot.AddKit(Chance.Always, DesirableItemArray(C, Stocks.wand));
        C.Startup.Loot.AddKit(Dice.Fixed(1), Chance.Always, DesirableItemArray(C, Stocks.ring));
        C.Startup.Loot.AddKit(Dice.Fixed(2), Chance.Always, DesirableItemArray(C, Stocks.potion));
        C.Startup.Loot.AddKit(Dice.Fixed(2), Chance.Always, DesirableItemArray(C, Stocks.scroll));
        C.Startup.Loot.AddKit(Chance.OneIn5, Items.magic_marker);
        C.Startup.Loot.AddKit(Chance.OneIn10, Items.magic_figurine);
        C.Startup.Loot.AddKit(Chance.Always, 1.d100() + 101, Items.gold_coin);
      });
    }
#endif

    public readonly Class barbarian;
    public readonly Class bard;
    public readonly Class caveman;
    public readonly Class convict;
    public readonly Class druid;
    public readonly Class explorer;
    public readonly Class gladiator;
    public readonly Class gunslinger;
    public readonly Class healer;
    public readonly Class hunter;
    public readonly Class jester;
    public readonly Class knight;
    public readonly Class miner;
    public readonly Class monk;
    public readonly Class mystic;
    public readonly Class necromancer;
    public readonly Class ninja;
    public readonly Class paladin;
    public readonly Class pirate;
    public readonly Class priest;
    public readonly Class ranger;
    public readonly Class reaver;
    public readonly Class rogue;
    public readonly Class samurai;
    public readonly Class shaman;
    public readonly Class templar;
    public readonly Class tinker;
    public readonly Class tourist;
    public readonly Class valkyrie;
    public readonly Class wizard;
  }
}