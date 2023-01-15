using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexKinds : CodexPage<ManifestKinds, KindEditor, Kind>
  {
    private CodexKinds() { }
#if MASTER_CODEX
    internal CodexKinds(Codex Codex)
      : base(Codex.Manifest.Kinds)
    {
      Kind AddKind(string Name)
      {
        return Register.Add(K =>
        {
          K.Name = Name;
        });
      }

      angel = AddKind("angel");
      insect = AddKind("insect");
      bat = AddKind("bat");
      beast = AddKind("beast");
      bird = AddKind("bird");
      blob = AddKind("blob");
      cat = AddKind("cat");
      changeling = AddKind("changeling");
      centaur = AddKind("centaur");
      demon = AddKind("demon");
      devourer = AddKind("devourer");
      dog = AddKind("dog");
      dragon = AddKind("dragon");
      dwarf = AddKind("dwarf");
      echo = AddKind("echo");
      elemental = AddKind("elemental");
      elf = AddKind("elf");
      eye = AddKind("eye");
      fairy = AddKind("fairy");
      flayer = AddKind("flayer");
      fungus = AddKind("fungus");
      ghost = AddKind("ghost");
      giant = AddKind("giant");
      gnoll = AddKind("gnoll");
      gnome = AddKind("gnome");
      golem = AddKind("golem");
      gremlin = AddKind("gremlin");
      halfling = AddKind("halfling");
      horse = AddKind("horse");
      human = AddKind("human");
      imp = AddKind("imp");
      jelly = AddKind("jelly");
      kobold = AddKind("kobold");
      lich = AddKind("lich");
      light = AddKind("light");
      lizard = AddKind("lizard");
      lycanthrope = AddKind("lycanthrope");
      marine = AddKind("marine");
      mercenary = AddKind("mercenary");
      mimic = AddKind("mimic");
      mummy = AddKind("mummy");
      naga = AddKind("naga");
      ogre = AddKind("ogre");
      orc = AddKind("orc");
      pudding = AddKind("pudding");
      quadruped = AddKind("quadruped");
      rodent = AddKind("rodent");
      snake = AddKind("snake");
      military = AddKind("military");
      spider = AddKind("spider");
      trapper = AddKind("trapper");
      troll = AddKind("troll");
      umber = AddKind("umber");
      vampire = AddKind("vampire");
      vortex = AddKind("vortex");
      worm = AddKind("worm");
      wraith = AddKind("wraith");
      xan = AddKind("xan");
      zombie = AddKind("zombie");

      this.UndeadArray = new[] { ghost, lich, mummy, vampire, wraith, zombie };
      this.LivingArray = Register.List.Except(UndeadArray).Except(new[] { demon, elemental, golem, vortex }).ToArray();

      var Kinds = Codex.Manifest.Kinds;
      Kinds.Undead.Set(UndeadArray);
      Kinds.Living.Set(LivingArray);
    }
#endif

    public IReadOnlyList<Kind> Undead => UndeadArray;
    public IReadOnlyList<Kind> Living => LivingArray;
    public readonly Kind angel;
    public readonly Kind insect;
    public readonly Kind bat;
    public readonly Kind beast;
    public readonly Kind bird;
    public readonly Kind blob;
    public readonly Kind cat;
    public readonly Kind centaur;
    public readonly Kind changeling;
    public readonly Kind demon;
    public readonly Kind devourer;
    public readonly Kind dog;
    public readonly Kind dragon;
    public readonly Kind dwarf;
    public readonly Kind echo;
    public readonly Kind elemental;
    public readonly Kind elf;
    public readonly Kind eye;
    public readonly Kind fairy;
    public readonly Kind flayer;
    public readonly Kind fungus;
    public readonly Kind ghost;
    public readonly Kind giant;
    public readonly Kind gnoll;
    public readonly Kind gnome;
    public readonly Kind golem;
    public readonly Kind gremlin;
    public readonly Kind halfling;
    public readonly Kind horse;
    public readonly Kind human;
    public readonly Kind imp;
    public readonly Kind jelly;
    public readonly Kind kobold;
    public readonly Kind lich;
    public readonly Kind light;
    public readonly Kind lizard;
    public readonly Kind lycanthrope;
    public readonly Kind marine;
    public readonly Kind mercenary;
    public readonly Kind military;
    public readonly Kind mimic;
    public readonly Kind mummy;
    public readonly Kind naga;
    public readonly Kind ogre;
    public readonly Kind orc;
    public readonly Kind pudding;
    public readonly Kind quadruped;
    public readonly Kind rodent;
    public readonly Kind snake;
    public readonly Kind spider;
    public readonly Kind trapper;
    public readonly Kind troll;
    public readonly Kind umber;
    public readonly Kind vampire;
    public readonly Kind vortex;
    public readonly Kind worm;
    public readonly Kind wraith;
    public readonly Kind xan;
    public readonly Kind zombie;

    private readonly Kind[] UndeadArray;
    private readonly Kind[] LivingArray;
  }
}