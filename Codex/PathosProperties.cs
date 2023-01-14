using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexProperties : CodexPage<ManifestProperties, PropertyEditor, Property>
  {
    private CodexProperties() { }
#if MASTER_CODEX
    internal CodexProperties(Codex Codex)
      : base(Codex.Manifest.Properties)
    {
      var Elements = Codex.Elements;

      foreach (var Property in List)
      {
        var Editor = Register.Edit(Property);

        Editor.Glyph = Codex.Glyphs.GetGlyphOrNull(Property.Name + " property");
        Debug.Assert(Editor.Glyph != null, Property.Name + " property must have a glyph.");
      }

      Register.Edit(petrifying).ParityElement = Elements.petrify;
      Register.Edit(sleeping).ParityElement = Elements.sleep;

      Register.Alias(vitality, "sickness resistance");
      Register.Alias(deflection, "protection");
    }
#endif

    public Property aggravation => Register.aggravation;
    public Property appraisal => Register.appraisal;
    public Property beatitude => Register.beatitude;
    public Property berserking => Register.berserking;
    public Property blindness => Register.blindness;
    public Property blinking => Register.blinking;
    public Property cannibalism => Register.cannibalism;
    public Property clairvoyance => Register.clairvoyance;
    public Property clarity => Register.clarity;
    public Property conflict => Register.conflict;
    public Property confusion => Register.confusion;
    public Property dark_vision => Register.dark_vision;
    public Property deafness => Register.deafness;
    public Property deflection => Register.deflection;
    public Property displacement => Register.displacement;
    public Property fainting => Register.fainting;
    public Property fear => Register.fear;
    public Property flight => Register.flight;
    public Property free_action => Register.free_action;
    public Property fumbling => Register.fumbling;
    public Property hallucination => Register.hallucination;
    public Property hunger => Register.hunger;
    public Property invisibility => Register.invisibility;
    public Property jumping => Register.jumping;
    public Property levitation => Register.levitation;
    public Property life_regeneration => Register.life_regeneration;
    public Property lifesaving => Register.lifesaving;
    public Property mana_regeneration => Register.mana_regeneration;
    public Property narcolepsy => Register.narcolepsy;
    public Property paralysis => Register.paralysis;
    public Property petrifying => Register.petrifying;
    public Property phasing => Register.phasing;
    public Property polymorph => Register.polymorph;
    public Property polymorph_control => Register.polymorph_control;
    public Property quickness => Register.quickness;
    public Property rage => Register.rage;
    public Property reflection => Register.reflection;
    public Property searching => Register.searching;
    public Property see_invisible => Register.see_invisible;
    public Property sickness => Register.sickness;
    public Property vitality => Register.vitality;
    public Property silence => Register.silence;
    public Property slippery => Register.slippery;
    public Property sleeping => Register.sleeping;
    public Property slowness => Register.slowness;
    public Property slow_digestion => Register.slow_digestion;
    public Property stealth => Register.stealth;
    public Property stunned => Register.stunned;
    public Property sustain_ability => Register.sustain_ability;
    public Property telekinesis => Register.telekinesis;
    public Property telepathy => Register.telepathy;
    public Property teleportation => Register.teleportation;
    public Property teleport_control => Register.teleport_control;
    public Property tunnelling => Register.tunnelling;
    public Property unchanging => Register.unchanging;
    public Property warning => Register.warning;
  }
}