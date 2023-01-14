using System;
using System.Collections.Generic;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexRecruitments : CodexPage<ManifestRecruitments, RecruitmentEditor, Recruitment>
  {
    private CodexRecruitments() { }
#if MASTER_CODEX
    internal CodexRecruitments(Codex Codex)
      : base(Codex.Manifest.Recruitments)
    {
      Recruitment AddRecruitment(string Name, int Cost, int Turns)
      {
        return Register.Add(R =>
        {
          R.Name = Name;
          R.Cost = Gold.FromCoins(Cost);
          R.Duration = Turns;
        });
      }

      BriefStinct = AddRecruitment("Brief stint", 300, 1000);
      ShortJourney = AddRecruitment("Short journey", 500, 3000);
      ExtendedTrip = AddRecruitment("Extended trip", 1000, 5000);
      LongCampaign = AddRecruitment("Long campaign", 2000, 10000);
      ToTheBitterEnd = AddRecruitment("To the bitter end", 5000, 100000);
    }
#endif

    public readonly Recruitment BriefStinct;
    public readonly Recruitment ShortJourney;
    public readonly Recruitment ExtendedTrip;
    public readonly Recruitment LongCampaign;
    public readonly Recruitment ToTheBitterEnd;
  }
}