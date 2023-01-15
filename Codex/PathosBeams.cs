using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexBeams : CodexPage<ManifestBeams, BeamEditor, Beam>
  {
    private CodexBeams() { }
#if MASTER_CODEX
    internal CodexBeams(Codex Codex)
      : base(Codex.Manifest.Beams)
    {
      var Glyphs = Codex.Glyphs;
      var Sonics = Codex.Sonics;

      Beam AddBeam(string Name, Sonic Sonic, Glyph Horizontal, Glyph Vertical, Glyph ForwardSlant, Glyph BackwardSlant)
      {
        return Register.Add(B =>
        {
          B.Name = Name;
          B.Sonic = Sonic;
          B.HorizontalGlyph = Horizontal;
          B.VerticalGlyph = Vertical;
          B.ForwardSlantGlyph = ForwardSlant;
          B.BackwardSlantGlyph = BackwardSlant;
        });
      }

      acid = AddBeam("acid", Sonics.sizzle, Glyphs.acid_beam_horizontal, Glyphs.acid_beam_vertical, Glyphs.acid_beam_forward_slant, Glyphs.acid_beam_backward_slant);
      cold = AddBeam("cold", Sonics.freeze, Glyphs.cold_beam_horizontal, Glyphs.cold_beam_vertical, Glyphs.cold_beam_forward_slant, Glyphs.cold_beam_backward_slant);
      death = AddBeam("death", Sonics.magic, Glyphs.death_beam_horizontal, Glyphs.death_beam_vertical, Glyphs.death_beam_forward_slant, Glyphs.death_beam_backward_slant);
      digging = AddBeam("digging", Sonics.magic, Glyphs.digging_beam, Glyphs.digging_beam, Glyphs.digging_beam, Glyphs.digging_beam);
      disintegration = AddBeam("disintegration", Sonics.magic, Glyphs.disintegration_beam_horizontal, Glyphs.disintegration_beam_vertical, Glyphs.disintegration_beam_forward_slant, Glyphs.disintegration_beam_backward_slant);
      fire = AddBeam("fire", Sonics.burn, Glyphs.fire_beam_horizontal, Glyphs.fire_beam_vertical, Glyphs.fire_beam_forward_slant, Glyphs.fire_beam_backward_slant);
      lightning = AddBeam("lightning", Sonics.electricity, Glyphs.lightning_beam_horizontal, Glyphs.lightning_beam_vertical, Glyphs.lightning_beam_forward_slant, Glyphs.lightning_beam_backward_slant);
      magic_missile = AddBeam("magic missile", Sonics.magic, Glyphs.magic_missile_beam_horizontal, Glyphs.magic_missile_beam_vertical, Glyphs.magic_missile_beam_forward_slant, Glyphs.magic_missile_beam_backward_slant);
      poison = AddBeam("poison", Sonics.gas, Glyphs.poison_beam_horizontal, Glyphs.poison_beam_vertical, Glyphs.poison_beam_forward_slant, Glyphs.poison_beam_backward_slant);
      sleep = AddBeam("sleep", Sonics.magic, Glyphs.sleep_beam_horizontal, Glyphs.sleep_beam_vertical, Glyphs.sleep_beam_forward_slant, Glyphs.sleep_beam_backward_slant);
    }
#endif

    public readonly Beam acid;
    public readonly Beam cold;
    public readonly Beam death;
    public readonly Beam digging;
    public readonly Beam disintegration;
    public readonly Beam fire;
    public readonly Beam lightning;
    public readonly Beam magic_missile;
    public readonly Beam poison;
    public readonly Beam sleep;
  }
}
