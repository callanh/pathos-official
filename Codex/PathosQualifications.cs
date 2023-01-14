using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexQualifications : CodexPage<ManifestQualifications, QualificationEditor, Qualification>
  {
    private CodexQualifications() { }
#if MASTER_CODEX
    internal CodexQualifications(Codex Codex)
      : base(Codex.Manifest.Qualifications)
    {
      Qualification AddQualification(string Code, string Name, int Rating)
      {
        return Register.Add(Q =>
        {
          Q.Code = Code;
          Q.Name = Name;
          Q.Rating = Rating;
        });
      }

      Proficient = AddQualification("p", "proficient", 0);
      Specialist = AddQualification("s", "specialist", 200); // +200
      Expert = AddQualification("e", "expert", 500);         // +300
      Master = AddQualification("m", "master", 1000);        // +500
      Champion = AddQualification("c", "champion", 2000);    // +1000
    }
#endif

    public readonly Qualification Proficient;
    public readonly Qualification Specialist;
    public readonly Qualification Expert;
    public readonly Qualification Master;
    public readonly Qualification Champion;
  }
}
