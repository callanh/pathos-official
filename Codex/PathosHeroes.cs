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
      var Entities = Codex.Entities;

      Hero AddHero(string Name, Gender Gender, Entity Entity, Class Class, Action<HeroEditor> EditorAction)
      {
        return Register.Add(H =>
        {
          H.Name = Name;
          H.Gender = Gender;
          H.Entity = Entity;
          H.Class = Class;
          H.Special = null;
          H.CustomGlyph = null;

          EditorAction(H);
        });
      }

      OrcBarbarian = AddHero("Grytt", Genders.male, Entities.orc, Classes.barbarian, H =>
      {
        H.Pet = H.NewPet("Razz", Genders.male, Entities.little_dog);
      });

      HumanCaveman = AddHero("Bok", Genders.male, Entities.human, Classes.caveman, H =>
      {
        H.Pet = H.NewPet("Dug", Genders.male, Entities.little_dog);
      });

      DwarfExplorer = AddHero("Quandon", Genders.female, Entities.dwarf, Classes.explorer, H =>
      {
        H.Pet = H.NewPet("Pinkerton", Genders.female, Entities.kitten);
      });

      GnomeHealer = AddHero("Malasuth", Genders.female, Entities.gnome, Classes.healer, H =>
      {
        H.Pet = H.NewPet("Capo", Genders.male, Entities.kitten);
      });

      HumanKnight = AddHero("Valorn", Genders.male, Entities.human, Classes.knight, H =>
      {
        H.Pet = H.NewPet("Perceval", Genders.male, Entities.pony);
      });

      OrcMonk = AddHero("Eybrinde", Genders.male, Entities.orc, Classes.monk, H =>
      {
        H.Pet = H.NewPet("Talon", Genders.male, Entities.kitten);
      });

      DwarfPriest = AddHero("Chemron", Genders.male, Entities.dwarf, Classes.priest, H =>
      {
        H.Pet = H.NewPet("Gasberg", Genders.female, Entities.kitten);
      });

      HumanRanger = AddHero("Amaya", Genders.female, Entities.human, Classes.ranger, H =>
      {
        H.Pet = H.NewPet("Cheyne", Genders.male, Entities.little_dog);
      });

      ElfRogue = AddHero("Xantari", Genders.female, Entities.elf, Classes.rogue, H =>
      {
        H.Pet = H.NewPet("Adebesi", Genders.female, Entities.little_dog);
      });

      HumanSamurai = AddHero("Rokiju", Genders.male, Entities.human, Classes.samurai, H =>
      {
        H.Pet = H.NewPet("Kira", Genders.female, Entities.little_dog);
      });

      GnomeTourist = AddHero("Pnerfa", Genders.female, Entities.gnome, Classes.tourist, H =>
      {
        H.Pet = H.NewPet("Ninny", Genders.male, Entities.little_dog);
      });

      HumanValkyrie = AddHero("Lagratha", Genders.female, Entities.human, Classes.valkyrie, H =>
      {
        H.Pet = H.NewPet("Floki", Genders.male, Entities.fledgling_raven);
      });

      ElfWizard = AddHero("Shinaas", Genders.male, Entities.elf, Classes.wizard, H =>
      {
        H.Pet = H.NewPet("Poe", Genders.female, Entities.fledgling_raven);
      });
    }
#endif

    public readonly Hero OrcBarbarian;
    public readonly Hero OrcMonk;
    public readonly Hero DwarfExplorer;
    public readonly Hero DwarfPriest;
    public readonly Hero GnomeHealer;
    public readonly Hero GnomeTourist;
    public readonly Hero HumanCaveman;
    public readonly Hero HumanRanger;
    public readonly Hero HumanKnight;
    public readonly Hero HumanSamurai;
    public readonly Hero HumanValkyrie;
    public readonly Hero ElfRogue;
    public readonly Hero ElfWizard;
  }
}
