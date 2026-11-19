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
      Race AddRace(string Name, Glyph MaleGlyph = null, Glyph FemaleGlyph = null, Glyph NonBinaryGlyph = null)
      {
        return Register.Add(R =>
        {
          R.Name = Name;

          if (MaleGlyph != null)
            R.AddAvatar(Codex.Genders.male, MaleGlyph);

          if (FemaleGlyph != null)
            R.AddAvatar(Codex.Genders.female, FemaleGlyph);

          if (NonBinaryGlyph != null)
            R.AddAvatar(Codex.Genders.nonbinary, NonBinaryGlyph);
        });
      }

      angel = AddRace("angel");
      changeling = AddRace("changeling");
      demon = AddRace("demon");
      dracon = AddRace("dracon");
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
      minotaur = AddRace("minotaur");
      nymph = AddRace("nymph");
      ogre = AddRace("ogre");
      orc = AddRace("orc");
      plasmoid = AddRace("plasmoid");
      robot = AddRace("robot");
      satyr = AddRace("satyr", MaleGlyph: Codex.Glyphs.satyr_male, FemaleGlyph: Codex.Glyphs.satyr_female);
      tortle = AddRace("tortle");
      troll = AddRace("troll", MaleGlyph: Codex.Glyphs.troll_male, FemaleGlyph: Codex.Glyphs.troll_female);
      unicorn = AddRace("unicorn");
    }
#endif

    public Race angel { get; }
    public Race changeling { get; }
    public Race demon { get; }
    public Race dracon { get; }
    public Race dwarf { get; }
    public Race echo { get; }
    public Race elf { get; }
    public Race ettin { get; }
    public Race fairy { get; }
    public Race giant { get; }
    public Race gnoll { get; }
    public Race gnome { get; }
    public Race goblin { get; }
    public Race halfling { get; }
    public Race human { get; }
    public Race kobold { get; }
    public Race leprechaun { get; }
    public Race lizardman { get; }
    public Race minotaur { get; }
    public Race nymph { get; }
    public Race ogre { get; }
    public Race orc { get; }
    public Race plasmoid { get; }
    public Race robot { get; }
    public Race satyr { get; }
    public Race tortle { get; }
    public Race troll { get; }
    public Race unicorn { get; }
  }
}
