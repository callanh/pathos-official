using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexElements : CodexPage<ManifestElements, ElementEditor, Element>
  {
    private CodexElements() { }
#if MASTER_CODEX
    internal CodexElements(Codex Codex)
      : base(Codex.Manifest.Elements)
    {
      Element AddElement(string Name, string Description, bool Destruction = false)
      {
        return Register.Add(E =>
        {
          E.Name = Name;
          E.Description = Description;
          E.Destruction = Destruction;
          E.Glyph = Codex.Glyphs.GetGlyphOrNull(E.Name + " element");
          Debug.Assert(E.Glyph != null, E.Name + " element must have a glyph.");
        });
      }

      // NOTE: only the elements that can be resisted are given a description.

      acid = AddElement("acid", "Complete resistance to acid and corrosive damage.");

      cold = AddElement("cold", "Complete resistance to cold and ice damage.");

      digging = AddElement("digging", null);

      disintegrate = AddElement("disintegrate", "Complete resistance to disintegration damage.", Destruction: true);

      drain = AddElement("drain", "Complete resistance to life and mana draining effects.");

      fire = AddElement("fire", "Complete resistance to fire and heat damage.");

      force = AddElement("force", null);

      magical = AddElement("magical", "Protects you against pure magical effects.");

      necrotic = AddElement("necrotic", null);

      petrify = AddElement("petrify", "Complete resistance to petrification effects.");

      physical = AddElement("physical", null);

      poison = AddElement("poison", "Complete resistance to poison damage.");

      shock = AddElement("shock", "Complete resistance to electric and shock damage.");

      sleep = AddElement("sleep", "You resist attempts to render you into a magical slumber.");

      water = AddElement("water", null);
    }
#endif

    public readonly Element water;
    public readonly Element sleep;
    public readonly Element shock;
    public readonly Element poison;
    public readonly Element physical;
    public readonly Element petrify;
    public readonly Element necrotic;
    public readonly Element magical;
    public readonly Element force;
    public readonly Element fire;
    public readonly Element drain;
    public readonly Element disintegrate;
    public readonly Element digging;
    public readonly Element cold;
    public readonly Element acid;
  }
}