using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  internal sealed class DhakModule : Module
  {
    internal DhakModule(Codex Codex)
      : base(
          Handle: "dhak legacy",
          Name: "Dhak Legacy",
          Description: "Hand-designed maps in an epic overland adventure. Take a break from the confines of the dungeon and embark on a challenging quest.",
          Colour: Inv.Colour.DarkCyan,
          Author: "Callan Hodgskin", Email: "hodgskin.callan@gmail.com",
          RequiresMasterMode: false)
    {
      this.Codex = Codex;

      SetIntroduction(Codex.Sonics.introduction);
      SetConclusion(Codex.Sonics.conclusion);
      SetTrack(Codex.Tracks.dhak_title);

      foreach (var TermText in CollectStaticPublicConstantStrings(typeof(DhakTerms)))
        AddTerm(TermText);
    }

    public override void Execute(Generator Generator)
    {
      Generator.BuildWorld(Generator.ImportQuest(Official.Resources.Quests.Dhak.GetBuffer()).World);

      if (Inv.Assert.IsEnabled)
      {
        var OverlandSite = Generator.Adventure.World.Sites.Single();
        var OverlandText = Generator.EscapedModuleTerm(DhakTerms.Overland);

        Inv.Assert.Check(OverlandSite.Name == OverlandText, "Site name mismatch.");
        Inv.Assert.Check(OverlandSite.GetMaps().Single().Name == OverlandText, "Map name mismatch.");
      }
    }

    private readonly Codex Codex;
  }

  internal static class DhakTerms
  {
    public const string Overland = "Overland";
  }
}