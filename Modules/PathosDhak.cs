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
          RequiresMasterMode: false, IsPublished: true)
    {
      this.Codex = Codex;
      SetTrack(Codex.Tracks.outside);
    }

    public override void Execute(Generator Generator)
    {
      Generator.BuildWorld(Generator.ImportQuest(Official.Resources.Quests.Dhak.GetBuffer()).World);
    }

    private readonly Codex Codex;
  }
}