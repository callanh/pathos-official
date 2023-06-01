using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexStandings : CodexPage<ManifestStandings, StandingEditor, Standing>
  {
    private CodexStandings() { }
#if MASTER_CODEX
    internal CodexStandings(Codex Codex)
      : base(Codex.Manifest.Standings)
    {
      var Properties = Codex.Properties;
      var Glyphs = Codex.Glyphs;

      Standing AddStatus(string Name, int Threshold, Action<StandingEditor> EditAction)
      {
        return Register.Add(E =>
        {
          E.Name = Name;
          E.Threshold = Threshold;
          E.Glyph = Glyphs.standing;

          CodexRecruiter.Enrol(() => EditAction(E));
        });
      }

      damned = AddStatus("damned", -1000, S =>
      {
        S.SpawnModifier = -4;
      });
      doomed = AddStatus("doomed", -750, S =>
      {
        S.SpawnModifier = -3;
      });
      evil = AddStatus("evil", -500, S =>
      {
        S.SpawnModifier = -2;
      });
      doubtful = AddStatus("doubtful", -250, S =>
      {
        S.SpawnModifier = -1;
      });
      strayed = AddStatus("strayed", -1, S =>
      {
        S.SpawnModifier = +0;
      });

      reconciled = AddStatus("reconciled", +0, S =>
      {
        S.SpawnModifier = +0;
      });
      hopeful = AddStatus("hopeful", +250, S =>
      {
        S.SpawnModifier = +1;
      });
      good = AddStatus("good", +500, S =>
      {
        S.SpawnModifier = +2;
      });
      glorious = AddStatus("glorious", +750, S =>
      {
        S.SpawnModifier = +3;
      });
      exalted = AddStatus("exalted", +1000, S =>
      {
        S.SpawnModifier = +4;
      });
    }
#endif

    public readonly Standing damned;
    public readonly Standing doomed;
    public readonly Standing evil;
    public readonly Standing doubtful;
    public readonly Standing strayed;

    public readonly Standing reconciled;
    public readonly Standing hopeful;
    public readonly Standing good;
    public readonly Standing glorious;
    public readonly Standing exalted;
  }
}