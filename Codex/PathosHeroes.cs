using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexHeroes : CodexPage<ManifestHeroes, HeroEditor, Hero>
  {
    private CodexHeroes() { }
#if MASTER_CODEX
    internal CodexHeroes(Codex Codex)
      : base(Codex.Manifest.Heroes)
    {
      var Genders = Codex.Genders;
      var Classes = Codex.Classes;
      var Races = Codex.Races;
      var Companions = Codex.Companions;

      Hero AddHero(string Name, Gender Gender, Race Race, Class Class, Action<HeroEditor> EditorAction)
      {
        if (Inv.Assert.IsEnabled)
          Inv.Assert.Check(Race.BaseEntity != null, "Only races with a base entity may be selected a hero.");

        return Register.Add(H =>
        {
          H.Name = Name;
          H.Gender = Gender;
          H.Race = Race;
          H.Class = Class;
          H.Special = null;
          H.CustomGlyph = null;

          EditorAction(H);
        });
      }

      MaleOrcBarbarian = AddHero("Grytt", Genders.male, Races.orc, Classes.barbarian, H => H
        .Pet("Razz", Genders.male, Companions.dog));

      MaleGnomeBard = AddHero("Jevard", Genders.male, Races.gnome, Classes.bard, H => H
        .Pet("Liloquy", Genders.female, Companions.bird));

      MaleHumanCaveman = AddHero("Bok", Genders.male, Races.human, Classes.caveman, H => H
        .Pet("Dug", Genders.male, Companions.dog));

      MaleChangelingConvict = AddHero("Uballon", Genders.male, Races.changeling, Classes.convict, H => H
        .Pet("Lunch", Genders.male, Companions.rat));

      FemaleDwarfExplorer = AddHero("Quandon", Genders.female, Races.dwarf, Classes.explorer, H => H
        .Pet("Pinkerton", Genders.female, Companions.cat));

      GiantMaleGladiator = AddHero("Norvric", Genders.male, Races.giant, Classes.gladiator, H => H
        .Pet("Busko", Genders.male, Companions.dog));

      MaleHumanGunslinger = AddHero("Toland", Genders.male, Races.human, Classes.gunslinger, H => H
        .Pet("Rake", Genders.male, Companions.dog));

      FemaleTrollDruid = AddHero("Karasque", Genders.female, Races.troll, Classes.druid, H => H
        .Pet("Mili", Genders.female, Companions.cat));

      MaleDwarfJester = AddHero("Yahoa", Genders.male, Races.dwarf, Classes.jester, H => H
        .Pet("Fizzit", Genders.female, Companions.monkey));

      FemaleSatyrHealer = AddHero("Malasuth", Genders.female, Races.satyr, Classes.healer, H => H
        .Pet("Capo", Genders.male, Companions.cat));

      FemaleDraconHunter = AddHero("Farleen", Genders.female, Races.dracon, Classes.hunter, H => H
        .Pet("Denier", Genders.male, Companions.horse));

      MaleHumanKnight = AddHero("Valorn", Genders.male, Races.human, Classes.knight, H => H
        .Pet("Perceval", Genders.male, Companions.horse));

      MalePlasmoidMiner = AddHero("Waldrin", Genders.male, Races.plasmoid, Classes.miner, H => H // TODO: nonbinary
        .Pet("Rytto", Genders.female, Companions.rat));

      FemaleFairyMystic = AddHero("Ovira", Genders.female, Races.fairy, Classes.mystic, H => H
        .Pet("Hampte", Genders.female, Companions.bird));

      MaleMinotaurMonk = AddHero("Eybrinde", Genders.male, Races.minotaur, Classes.monk, H => H
        .Pet("Talon", Genders.male, Companions.cat));

      FemaleElfNinja = AddHero("Akoya", Genders.female, Races.elf, Classes.ninja, H => H
        .Pet("Zuki", Genders.male, Companions.rat));

      FemaleKoboldNecromancer = AddHero("Dastrne", Genders.female, Races.kobold, Classes.necromancer, H => H
        .Pet("Samrae", Genders.male, Companions.ghoul));

      FemaleHumanPaladin = AddHero("Hylatia", Genders.female, Races.human, Classes.paladin, H => H
        .Pet("Tempus", Genders.male, Companions.horse));

      FemaleDemonPirate = AddHero("Neeva", Genders.female, Races.demon, Classes.pirate, H => H
        .Pet("Derent", Genders.male, Companions.monkey));

      MaleDwarfPriest = AddHero("Chemron", Genders.male, Races.dwarf, Classes.priest, H => H
        .Pet("Gasberg", Genders.female, Companions.cat));

      FemaleHumanRanger = AddHero("Amaya", Genders.female, Races.human, Classes.ranger, H => H
        .Pet("Cheyne", Genders.male, Companions.dog));

      MaleLizardmanReaver = AddHero("Relinsarn", Genders.female, Races.lizardman, Classes.reaver, H => H
        .Pet("Mizra", Genders.female, Companions.imp));

      FemaleElfRogue = AddHero("Xantari", Genders.female, Races.elf, Classes.rogue, H => H
        .Pet("Adebesi", Genders.female, Companions.dog));

      MaleHumanSamurai = AddHero("Rokiju", Genders.male, Races.human, Classes.samurai, H => H
        .Pet("Kira", Genders.female, Companions.dog));

      FemaleTortleShaman = AddHero("Iralna", Genders.female, Races.tortle, Classes.shaman, H => H
        .Pet("Armax", Genders.female, Companions.dog));

      MaleAngelTemplar = AddHero("Carthyre", Genders.male, Races.angel, Classes.templar, H => H // TODO: nonbinary
        .Pet("Hal", Genders.male, Companions.bird));

      FemaleRobotTinker = AddHero("Zarja", Genders.male, Races.robot, Classes.tinker, H => H
        .Pet("Goggie", Genders.male, Companions.monkey));

      FemaleHalflingTourist = AddHero("Pnerfa", Genders.female, Races.halfling, Classes.tourist, H => H
        .Pet("Ninny", Genders.male, Companions.dog));

      FemaleEchoValkyrie = AddHero("Lagratha", Genders.female, Races.echo, Classes.valkyrie, H => H
        .Pet("Floki", Genders.male, Companions.bird));

      MaleElfWizard = AddHero("Shinaas", Genders.male, Races.elf, Classes.wizard, H => H
        .Pet("Poe", Genders.female, Companions.bird));
    }
#endif

    public Hero MaleOrcBarbarian { get; }
    public Hero MaleGnomeBard { get; }
    public Hero MaleHumanCaveman { get; }
    public Hero MaleChangelingConvict { get; }
    public Hero FemaleDwarfExplorer { get; }
    public Hero GiantMaleGladiator { get; }
    public Hero MaleHumanGunslinger { get; }
    public Hero FemaleTrollDruid { get; }
    public Hero MaleDwarfJester { get; }
    public Hero FemaleSatyrHealer { get; }
    public Hero FemaleDraconHunter { get; }
    public Hero MaleHumanKnight { get; }
    public Hero MalePlasmoidMiner { get; }
    public Hero FemaleFairyMystic { get; }
    public Hero MaleMinotaurMonk { get; }
    public Hero FemaleElfNinja { get; }
    public Hero FemaleKoboldNecromancer { get; }
    public Hero FemaleHumanPaladin { get; }
    public Hero FemaleDemonPirate { get; }
    public Hero MaleDwarfPriest { get; }
    public Hero FemaleHumanRanger { get; }
    public Hero MaleLizardmanReaver { get; }
    public Hero FemaleElfRogue { get; }
    public Hero FemaleHalflingTourist { get; }
    public Hero MaleHumanSamurai { get; }
    public Hero FemaleTortleShaman { get; }
    public Hero MaleAngelTemplar { get; }
    public Hero FemaleRobotTinker { get; }
    public Hero FemaleEchoValkyrie { get; }
    public Hero MaleElfWizard { get; }
  }
}
