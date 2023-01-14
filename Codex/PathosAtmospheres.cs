using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexAtmospheres : CodexPage<ManifestAtmospheres, AtmosphereEditor, Atmosphere>
  {
    private CodexAtmospheres() { }
#if MASTER_CODEX
    internal CodexAtmospheres(Codex Codex)
      : base(Codex.Manifest.Atmospheres)
    {
      Atmosphere AddAtmosphere(string Name, Track Track, params Sonic[] AmbientArray)
      {
        return Register.Add(A =>
        {
          A.Name = Name;
          A.Track = Track;
          A.SetAmbients(AmbientArray);
        });
      }

      var Tracks = Codex.Tracks;
      var Sonics = Codex.Sonics;

      // NOTE: distinct tracks would be desirable for each atmosphere.
      // NOTE: duplicate sonics is okay - it increases the probability of that sound.
      cavern = AddAtmosphere("cavern", Tracks.inside, Sonics.taps, Sonics.scrapes, Sonics.drips, Sonics.drips);
      civilisation = AddAtmosphere("civilisation", Tracks.inside, Sonics.taps, Sonics.squeaks, Sonics.squeaks, Sonics.scrapes);
      dungeon = AddAtmosphere("dungeon", Tracks.inside, Sonics.taps, Sonics.scrapes, Sonics.squeaks, Sonics.chains);
      forest = AddAtmosphere("forest", Tracks.inside, Sonics.birds, Sonics.birds, Sonics.leaves, Sonics.leaves);
      nether = AddAtmosphere("nether", Tracks.inside, Sonics.taps, Sonics.scrapes, Sonics.roars, Sonics.chains);
    }
#endif

    public readonly Atmosphere cavern;
    public readonly Atmosphere civilisation;
    public readonly Atmosphere dungeon;
    public readonly Atmosphere forest;
    public readonly Atmosphere nether;
  }
}