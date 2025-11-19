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
      dracon = AddLivingKind("dracon");
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
      lich = AddUndeadKind("lich");
      light = AddLivingKind("light");
      lizard = AddLivingKind("lizard");
      lizardman = AddLivingKind("lizardman");
      lycanthrope = AddLivingKind("lycanthrope");
      marine = AddLivingKind("marine");
      mercenary = AddLivingKind("mercenary");
      mimic = AddLivingKind("mimic");
      minotaur = AddLivingKind("minotaur");
      mummy = AddUndeadKind("mummy");
      naga = AddLivingKind("naga");
      ogre = AddLivingKind("ogre");
      orc = AddLivingKind("orc");
      plasmoid = AddLivingKind("plasmoid");
      pudding = AddLivingKind("pudding");
      quadruped = AddLivingKind("quadruped");
      robot = AddOtherKind("robot");
      rodent = AddLivingKind("rodent");
      satyr = AddLivingKind("satyr");
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
    public Kind angel { get; }
    public Kind insect { get; }
    public Kind bat { get; }
    public Kind beast { get; }
    public Kind bird { get; }
    public Kind blob { get; }
    public Kind cat { get; }
    public Kind centaur { get; }
    public Kind changeling { get; }
    public Kind demon { get; }
    public Kind devourer { get; }
    public Kind dog { get; }
    public Kind dracon { get; }
    public Kind dragon { get; }
    public Kind dwarf { get; }
    public Kind echo { get; }
    public Kind elemental { get; }
    public Kind elf { get; }
    public Kind eye { get; }
    public Kind fairy { get; }
    public Kind flayer { get; }
    public Kind fungus { get; }
    public Kind ghost { get; }
    public Kind giant { get; }
    public Kind gnoll { get; }
    public Kind gnome { get; }
    public Kind golem { get; }
    public Kind gremlin { get; }
    public Kind halfling { get; }
    public Kind horse { get; }
    public Kind human { get; }
    public Kind imp { get; }
    public Kind jelly { get; }
    public Kind kobold { get; }
    public Kind lich { get; }
    public Kind light { get; }
    public Kind lizard { get; }
    public Kind lizardman { get; }
    public Kind lycanthrope { get; }
    public Kind marine { get; }
    public Kind mercenary { get; }
    public Kind military { get; }
    public Kind mimic { get; }
    public Kind minotaur { get; }
    public Kind mummy { get; }
    public Kind naga { get; }
    public Kind ogre { get; }
    public Kind orc { get; }
    public Kind plasmoid { get; }
    public Kind pudding { get; }
    public Kind quadruped { get; }
    public Kind robot { get; }
    public Kind rodent { get; }
    public Kind satyr { get; }
    public Kind snake { get; }
    public Kind spider { get; }
    public Kind tortle { get; }
    public Kind trapper { get; }
    public Kind troll { get; }
    public Kind umber { get; }
    public Kind vampire { get; }
    public Kind vortex { get; }
    public Kind worm { get; }
    public Kind wraith { get; }
    public Kind xan { get; }
    public Kind zombie { get; }

    private Kind[] UndeadArray;
    private Kind[] LivingArray;
  }
}