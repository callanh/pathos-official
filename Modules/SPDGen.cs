using System;
using System.Text;
using System.Linq;
using Inv.Support;

namespace Pathos
{
    internal sealed class SPDModule : Module
    {
        internal SPDModule(Codex Codex)
        : base(Handle: "Pixel Sojourn", Name: "Pixel Sojourn", Description: 
              "Interesting rooms and secrets await but do not linger; "
              + "you must descend or die. Based on the map generation from Shattered Pixel Dungeon.", 
              Colour: Inv.Colour.SeaGreen, Author: "KW", Email: "2159687@gmail.com", 
              RequiresMasterMode: false, IsPublished: true)
        {
            this.Codex = Codex;

            SetIntroduction(Codex.Sonics.introduction);
            SetConclusion(Codex.Sonics.conclusion);
            SetTrack(Codex.Tracks.pixel_title);
            AddTerms(new[] { SPDDebug.level_depth });
        }

        public override void Execute(Generator Generator) => SPDLevelGenerator.Run(Codex, Generator);

        private readonly Codex Codex;
  }
}
