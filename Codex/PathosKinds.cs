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
      Kind AddLivingKind(string Name)
      {
        return Register.Add(K =>
        {
          K.Name = Name;
          K.Living = true;
        });
      }
      Kind AddUndeadKind(string Name)
      {
        return Register.Add(K =>
        {
          K.Name = Name;
          K.Undead = true;
        });
      }
      Kind AddOtherKind(string Name)
      {
        return Register.Add(K =>
        {
          K.Name = Name;
        });
      }

      angel = AddLivingKind("angel");
      insect = AddLivingKind("insect");
      bat = AddLivingKind("bat");
      beast = AddLivingKind("beast");
      bird = AddLivingKind("bird");
      blob = AddLivingKind("blob");
      cat = AddLivingKind("cat");
      changeling = AddLivingKind("changeling");
      centaur = AddLivingKind("centaur");
      demon = AddOtherKind("demon");
      devourer = AddLivingKind("devourer");
      dog = AddLivingKind("dog");
      dragon = AddLivingKind("dragon");
      dwarf = AddLivingKind("dwarf");
      echo = AddOtherKind("echo");
      elemental = AddOtherKind("elemental");
      elf = AddLivingKind("elf");
      eye = AddLivingKind("eye");
      fairy = AddLivingKind("fairy");
      flayer = AddLivingKind("flayer");
      fungus = AddLivingKind("fungus");
      ghost = AddUndeadKind("ghost");
      giant = AddLivingKind("giant");
      gnoll = AddLivingKind("gnoll");
      gnome = AddLivingKind("gnome");
      golem = AddOtherKind("golem");
      gremlin = AddLivingKind("gremlin");
      halfling = AddLivingKind("halfling");
      horse = AddLivingKind("horse");
      human = AddLivingKind("human");
      imp = AddLivingKind("imp");
      jelly = AddLivingKind("jelly");
      kobold = AddLivingKind("kobold");
      lich = AddLivingKind("lich");
      light = AddLivingKind("light");
      lizard = AddLivingKind("lizard");
      lycanthrope = AddLivingKind("lycanthrope");
      marine = AddLivingKind("marine");
      mercenary = AddLivingKind("mercenary");
      mimic = AddLivingKind("mimic");
      mummy = AddUndeadKind("mummy");
      naga = AddLivingKind("naga");
      ogre = AddLivingKind("ogre");
      orc = AddLivingKind("orc");
      plasmoid = AddLivingKind("plasmoid");
      pudding = AddLivingKind("pudding");
      quadruped = AddLivingKind("quadruped");
      rodent = AddLivingKind("rodent");
      snake = AddLivingKind("snake");
      military = AddLivingKind("military");
      spider = AddLivingKind("spider");
      tortle = AddLivingKind("tortle");
      trapper = AddLivingKind("trapper");
      troll = AddLivingKind("troll");
      umber = AddLivingKind("umber");
      vampire = AddUndeadKind("vampire");
      vortex = AddOtherKind("vortex");
      worm = AddLivingKind("worm");
      wraith = AddUndeadKind("wraith");
      xan = AddLivingKind("xan");
      zombie = AddUndeadKind("zombie");

      this.UndeadArray = List.Where(K => K.Undead).ToArray();
      this.LivingArray = List.Where(K => K.Living).ToArray();
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
    public readonly Kind plasmoid;
    public readonly Kind pudding;
    public readonly Kind quadruped;
    public readonly Kind rodent;
    public readonly Kind snake;
    public readonly Kind spider;
    public readonly Kind tortle;
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