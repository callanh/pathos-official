using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexAnatomies : CodexPage<ManifestAnatomies, AnatomyEditor, Anatomy>
  {
    private CodexAnatomies() { }
#if MASTER_CODEX
    internal CodexAnatomies(Codex Codex)
      : base(Codex.Manifest.Anatomies)
    {
      Anatomy AddAnatomy(string Name, string AbsenceText)
      {
        return Register.Add(A =>
        {
          A.Name = Name;
          A.AbsenceText = AbsenceText;
        });
      }

      head = AddAnatomy("head", "NO HEAD");
      mind = AddAnatomy("mind", "MINDLESS");
      eyes = AddAnatomy("eyes", "NO EYES");
      ears = AddAnatomy("ears", "NO EARS");
      voice = AddAnatomy("voice", "VOICELESS");
      hands = AddAnatomy("hands", "NO HANDS");
      limbs = AddAnatomy("limbs", "NO LIMBS");
      feet = AddAnatomy("feet", "NO FEET");
      blood = AddAnatomy("blood", "NO BLOOD");
      thermal = AddAnatomy("thermal", "NOT WARM");
      amorphous = AddAnatomy("amorphous", "NOT AMORPHOUS");
      mounted = AddAnatomy("mounted", "NOT MOUNTED");

      Register.head = head;
      Register.mind = mind;
      Register.eyes = eyes;
      Register.ears = ears;
      Register.voice = voice;
      Register.hands = hands;
      Register.limbs = limbs;
      Register.feet = feet;
      Register.thermal = thermal;
      Register.blood = blood;
      Register.mounted = mounted;
      Register.amorphous = amorphous;
    }
#endif

    public readonly Anatomy head;
    public readonly Anatomy mind;
    public readonly Anatomy eyes;
    public readonly Anatomy ears;
    public readonly Anatomy hands;
    public readonly Anatomy limbs;
    public readonly Anatomy feet;
    public readonly Anatomy thermal;
    public readonly Anatomy blood;
    public readonly Anatomy mounted;
    public readonly Anatomy amorphous;
    public readonly Anatomy voice;
  }
}