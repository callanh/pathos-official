using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexRaces : CodexPage<ManifestRaces, RaceEditor, Race>
  {
    private CodexRaces() { }
#if MASTER_CODEX
    internal CodexRaces(Codex Codex)
      : base(Codex.Manifest.Races)
    {
      Race AddRace(string Name)
      {
        return Register.Add(R =>
        {
          R.Name = Name;
        });
      }

      angel = AddRace("angel");
      changeling = AddRace("changeling");
      demon = AddRace("demon");
      dwarf = AddRace("dwarf");
      echo = AddRace("echo");
      elf = AddRace("elf");
      ettin = AddRace("ettin");
      fairy = AddRace("fairy");
      giant = AddRace("giant");
      gnoll = AddRace("gnoll");
      gnome = AddRace("gnome");
      goblin = AddRace("goblin");
      halfling = AddRace("halfling");
      human = AddRace("human");
      kobold = AddRace("kobold");
      leprechaun = AddRace("leprechaun");
      lizardman = AddRace("lizardman");
      nymph = AddRace("nymph");
      ogre = AddRace("ogre");
      orc = AddRace("orc");
      satyr = AddRace("satyr");
      troll = AddRace("troll");
      unicorn = AddRace("unicorn");
    }
#endif

    public readonly Race angel;
    public readonly Race changeling;
    public readonly Race demon;
    public readonly Race dwarf;
    public readonly Race echo;
    public readonly Race elf;
    public readonly Race ettin;
    public readonly Race fairy;
    public readonly Race giant;
    public readonly Race gnoll;
    public readonly Race gnome;
    public readonly Race goblin;
    public readonly Race halfling;
    public readonly Race human;
    public readonly Race kobold;
    public readonly Race leprechaun;
    public readonly Race lizardman;
    public readonly Race nymph;
    public readonly Race ogre;
    public readonly Race orc;
    public readonly Race satyr;
    public readonly Race troll;
    public readonly Race unicorn;
  }
}