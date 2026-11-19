using System;
using System.Collections.Generic;
using System.Text;
using Inv.Support;

namespace Pathos
{
  internal sealed class ShortDungeonModule : Module
  {
    internal ShortDungeonModule(Codex Codex)
      : base(
          Handle: "short dungeon",
          Name: "Short Dungeon",
          Description: "Survive the dungeon to reach Minetown and then venture into the tunnels below, which connect to the perilous Underdeep.",
          Colour: Inv.Colour.FromHtmlString("#FFB55E").Darken(0.30F),
          Author: "Callan Hodgskin", Email: "hodgskin.callan@gmail.com",
          RequiresMasterMode: false)
    {
      this.Codex = Codex;

      SetDemo();
      SetIntroduction(Codex.Sonics.introduction);
      SetConclusion(Codex.Sonics.conclusion);
      SetTrack(Codex.Tracks.nethack_title);
      SetVictory(Codex.Achievements.ShortDungeon);
    }

    public override void Execute(Generator Generator)
    {
      new NetHackMaker(Codex, Generator, IsShortened: true).Create();
    }

    private readonly Codex Codex;
  }
}
