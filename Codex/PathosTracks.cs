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
      dhak_title = AddTrack("dhak title");
      kaloi_title = AddTrack("kaloi title");
      nethack_title = AddTrack("nethack title");
      pixel_title = AddTrack("pixel title");
    }
#endif

    public readonly Track inside;
    public readonly Track dhak_title;
    public readonly Track kaloi_title;
    public readonly Track nethack_title;
    public readonly Track pixel_title;
  }
}
