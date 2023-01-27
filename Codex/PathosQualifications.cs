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

      // TODO: unskilled = AddQualification("u", "unskilled", -1000);
      proficient = AddQualification("p", "proficient", 0);   // +1000
      specialist = AddQualification("s", "specialist", 200); // +200
      expert = AddQualification("e", "expert", 500);         // +300
      master = AddQualification("m", "master", 1000);        // +500
      champion = AddQualification("c", "champion", 2000);    // +1000
    }
#endif

    public readonly Qualification proficient;
    public readonly Qualification specialist;
    public readonly Qualification expert;
    public readonly Qualification master;
    public readonly Qualification champion;
  }
}
