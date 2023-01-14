using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexGenders : CodexPage<ManifestGenders, GenderEditor, Gender>
  {
    private CodexGenders() { }
#if MASTER_CODEX
    internal CodexGenders(Codex Codex)
      : base(Codex.Manifest.Genders)
    {
      Gender AddGender(string Name, string He, string Him, string His, string Himself, bool Recognised)
      {
        return Register.Add(G =>
        {
          G.Name = Name;
          G.He = He;  
          G.Him = Him;
          G.His = His;
          G.Himself = Himself;
          G.Recognised = Recognised;
        });
      }

      male = AddGender("male", "he", "him", "his", "himself", Recognised: true);
      female = AddGender("female", "she", "her", "her", "herself", Recognised: true);
      nonbinary = AddGender("non-binary", "they", "them", "their", "themself", Recognised: true);
      neuter = AddGender("neuter", "it", "it", "its", "itself", Recognised: false);
    }
#endif

    /// <summary>
    /// Identifies as male.
    /// </summary>
    public readonly Gender male;
    /// <summary>
    /// Identifies as female.
    /// </summary>
    public readonly Gender female;
    /// <summary>
    /// Identifies as neither male or female.
    /// </summary>
    public readonly Gender nonbinary;
    /// <summary>
    /// Identifies as having no gender.
    /// </summary>
    public readonly Gender neuter;
  }
}
