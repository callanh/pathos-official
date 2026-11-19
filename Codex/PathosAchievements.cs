using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexAchievements : CodexPage<ManifestAchievements, AchievementEditor, Achievement>
  {
    private CodexAchievements() { }
#if MASTER_CODEX
    internal CodexAchievements(Codex Codex)
      : base(Codex.Manifest.Achievements)
    {
      var Glyphs = Codex.Glyphs;
      var Sonics = Codex.Sonics;
      var Elements = Codex.Elements;
      var Properties = Codex.Properties;
      var Grounds = Codex.Grounds;

      Achievement AddAchievement(string SteamID, string Name, string Description, bool Victory = false)
      {
        Debug.Assert(SteamID != null);
        Debug.Assert(Name != null);
        Debug.Assert(Description != null);

        return Register.Add(S =>
        {
          S.Name = Name;
          S.Description = Description;
          S.SteamID = SteamID;
          S.Victory = Victory;
        });
      }

      this.NetHackCodex = AddAchievement("NETHACK_CODEX", "NetHack Codex", "Complete the module with a deathless victory.", Victory: true);
      this.KaloiOpus = AddAchievement("KALOI_OPUS", "Kaloi Opus", "Complete the module with a deathless victory.", Victory: true);
      this.PixelSojourn = AddAchievement("PIXEL_SOJOURN", "Pixel Sojourn", "Complete the module with a deathless victory.", Victory: true);
      this.DhakLegacy = AddAchievement("DHAK_LEGACY", "Dhak Legacy", "Complete the module with a deathless victory.", Victory: true);
      this.ShortDungeon = AddAchievement("SHORT_DUNGEON", "Short Dungeon", "Complete the module with a deathless victory.", Victory: true);

      Register.Bouldering = AddAchievement("BOULDERING", "bouldering", "push a boulder.");
      Register.Tunnelling = AddAchievement("TUNNELLING", "tunnelling", "dig through a wall.");
      Register.Teleportitis = AddAchievement("TELEPORTITIS", "teleportitis", "acquire uncontrolled teleportation.");
      //Register.Shoplifter = AddAchievement("SHOPLIFTER", "shoplifter", "steal an item from a shop.");
      Register.Survivor = AddAchievement("SURVIVOR", "survivor", "recover from 1 life point.");
      Register.Vore = AddAchievement("VORE", "vore", "eat a non-food item.");
      Register.Polymorphism = AddAchievement("POLYMORPHISM", "polymorphism", "transform into another form.");
      Register.Polypiling = AddAchievement("POLYPILING", "polypiling", "transform an item on the ground.");
      Register.PeakForm = AddAchievement("PEAK_FORM", "peak form", "maximise all ability scores.");
      Register.Immunity = AddAchievement("IMMUNITY", "immunity", "gain complete natural resistances.");
      Register.Exceptional = AddAchievement("EXCEPTIONAL", "exceptional", "acquire all positive talents.");
      Register.Spellbound = AddAchievement("SPELLBOUND", "spellbound", "learn to cast every spell.");
      Register.GrandChampion = AddAchievement("GRAND_CHAMPION", "grand champion", "train every skill to the limit.");
      Register.GreatExperience = AddAchievement("GREAT_EXPERIENCE", "great experience", $"reach experience level {Codex.Manifest.Levelling.LevelCap}.");
      Register.AllCuration = AddAchievement("ALL_CURATION", "all curation", "discover every item.");
      Register.WellMet = AddAchievement("WELL_MET", "well met", "encounter every entity.");
      Register.Deathless = AddAchievement("DEATHLESS", "deathless", "win without resurrecting.");

      // TODO: Friendship = max party at max charisma?
    }
#endif

    public Achievement NetHackCodex { get; }
    public Achievement KaloiOpus { get; }
    public Achievement PixelSojourn { get; }
    public Achievement DhakLegacy { get; }
    public Achievement ShortDungeon { get; }
  }
}
