using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Inv.Support;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Pathos")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("PathosCompiler")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("PathosMaker")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("PathosStudioW")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("PathosSyntaxTest")]

namespace Pathos
{
  public sealed class OfficialCampaign : Campaign
  {
    internal OfficialCampaign(System.IO.Stream CodexStream)
      : base("official", "Official")
    {
#if MASTER_CODEX
      this.Codex = new Codex(new Manifest("Pathos")); // 1.3 seconds.
#endif
      if (this.Codex == null)
      {
        this.Codex = new Codex();

        Debug.Assert(CodexStream != null, "CodexStream must be provided.");

        if (CodexStream != null)
          new CodexGovernor().Load(Codex, CodexStream); // 0.3 seconds.
      }

      SetManifest(Codex.Manifest);

      AddModule(new NethackModule(Codex));
      AddModule(new DhakModule(Codex));
      AddModule(new SPDModule(Codex));
      AddModule(new SandboxModule(Codex));

      AddAtlasType("absurd");
      AddAtlasType("classic");
      AddAtlasType("geoduck");
      AddAtlasType("sonyvanda");

      AddAlbumType("freesound");
    }

    public Codex Codex { get; }

    public override void Make(System.IO.Stream CodexStream)
    {
      // only save the Codex when it has been declared in this execution.
#if MASTER_CODEX
      Debug.Assert(CodexStream != null, "CodexStream must be provided.");

      if (CodexStream != null)
        new CodexGovernor().Save(Codex, CodexStream);

      // TODO: Codex Reports?
#endif
    }
  }
}