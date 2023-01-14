using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexTracks : CodexPage<ManifestTracks, TrackEditor, Track>
  {
    private CodexTracks() { }
#if MASTER_CODEX
    internal CodexTracks(Codex Codex)
      : base(Codex.Manifest.Tracks)
    {
      Track AddTrack(string Name) => Register.Add(T => T.Name = Name);

      inside = AddTrack("inside");
      outside = AddTrack("outside");
    }
#endif

    public readonly Track inside;
    public readonly Track outside;
  }
}
