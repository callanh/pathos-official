using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexExplosions : CodexPage<ManifestExplosions, ExplosionEditor, Explosion>
  {
    private CodexExplosions() { }
#if MASTER_CODEX
    internal CodexExplosions(Codex Codex)
      : base(Codex.Manifest.Explosions)
    {
      var Sonics = Codex.Sonics;

      Explosion AddExplosion(string Name, Sonic Sonic)
      {
        return Register.Add(E =>
        {
          E.Name = Name;
          E.Sonic = Sonic;

          foreach (var Area in Inv.Support.EnumHelper.GetEnumerable<ExplosionArea>())
            E.SetGlyph(Area, Codex.Glyphs.GetGlyph(Name + " explosion " + Area.ToString().PascalCaseToTitleCase().ToLowerInvariant()));
        });
      }

      acid = AddExplosion("acid", Sonics.water_splash);
      dark = AddExplosion("dark", Sonics.magic); // sfx
      death = AddExplosion("death", Sonics.magic); // sfx
      electric = AddExplosion("electric", Sonics.electricity);
      fiery = AddExplosion("fiery", Sonics.burn);
      frosty = AddExplosion("frosty", Sonics.freeze);
      light = AddExplosion("light", Sonics.magic); // sfx
      magical = AddExplosion("magical", Sonics.magic);
      muddy = AddExplosion("muddy", Sonics.water_crash);
      watery = AddExplosion("watery", Sonics.water_crash);
    }
#endif

    public readonly Explosion acid;
    public readonly Explosion dark;
    public readonly Explosion death;
    public readonly Explosion electric;
    public readonly Explosion fiery;
    public readonly Explosion frosty;
    public readonly Explosion light;
    public readonly Explosion magical;
    public readonly Explosion muddy;
    public readonly Explosion watery;
  }

  public sealed class CodexStrikes : CodexPage<ManifestStrikes, StrikeEditor, Strike>
  {
    private CodexStrikes() { }
#if MASTER_CODEX
    internal CodexStrikes(Codex Codex)
      : base(Codex.Manifest.Strikes)
    {
      var Glyphs = Codex.Glyphs;
      var Sonics = Codex.Sonics;

      Strike AddStrike(string Name, Sonic Sonic, Glyph Glyph)
      {
        return Register.Add(S =>
        {
          S.Name = Name;
          S.Sonic = Sonic;
          S.Glyph = Glyph;
        });
      }

      acid = AddStrike("acid", Sonics.sizzle, Glyphs.acid_strike);
      boost = AddStrike("boost", Sonics.magic, Glyphs.boost_strike);
      death = AddStrike("death", Sonics.magic, Glyphs.death_strike);
      energy = AddStrike("energy", Sonics.electricity, Glyphs.energy_strike);
      flame = AddStrike("flame", Sonics.burn, Glyphs.flame_strike);
      flash = AddStrike("flash", Sonics.flash, Glyphs.flash_strike);
      force = AddStrike("force", Sonics.force, Glyphs.force_strike);
      frost = AddStrike("frost", Sonics.freeze, Glyphs.frost_strike);
      gas = AddStrike("gas", Sonics.gas, Glyphs.gas_strike);
      holy = AddStrike("holy", Sonics.magic, Glyphs.holy_strike);
      magic = AddStrike("magic", Sonics.force, Glyphs.magic_strike);
      psychic = AddStrike("psychic", Sonics.force, Glyphs.psychic_strike);
      sever = AddStrike("sever", Sonics.sever, Glyphs.sever_strike);
      shield = AddStrike("shield", Sonics.magic, Glyphs.shield_strike);
      shriek = AddStrike("shriek", Sonics.shriek, Glyphs.force_strike);
      spirit = AddStrike("spirit", Sonics.force, Glyphs.spirit_strike);
      tunnel = AddStrike("tunnel", Sonics.tunnelling, Glyphs.tunnel_strike);
      venom = AddStrike("venom", Sonics.water_splash, Glyphs.venom_strike);
      wail = AddStrike("wail", Sonics.wailing, Glyphs.force_strike);
    }
#endif

    public readonly Strike acid;
    public readonly Strike boost;
    public readonly Strike death;
    public readonly Strike energy;
    public readonly Strike flame;
    public readonly Strike flash;
    public readonly Strike force;
    public readonly Strike frost;
    public readonly Strike gas;
    public readonly Strike holy;
    public readonly Strike magic;
    public readonly Strike psychic;
    public readonly Strike sever;
    public readonly Strike shield;
    public readonly Strike shriek;
    public readonly Strike spirit;
    public readonly Strike tunnel;
    public readonly Strike venom;
    public readonly Strike wail;
  }

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

  public sealed class CodexEngulfments : CodexPage<ManifestEngulfments, EngulfmentEditor, Engulfment>
  {
    private CodexEngulfments() { }
#if MASTER_CODEX
    internal CodexEngulfments(Codex Codex)
      : base(Codex.Manifest.Engulfments)
    {
      Engulfment AddEngulfment(string Name, Sonic Sonic)
      {
        return Register.Add(E =>
        {
          E.Name = Name;  
          E.Sonic = Sonic;

          foreach (var Area in Inv.Support.EnumHelper.GetEnumerable<EngulfmentArea>())
            E.SetGlyph(Area, Codex.Glyphs.GetGlyph("swallowed " + Area.ToString().PascalCaseToTitleCase().ToLowerInvariant()));
        });
      }

      engulfed = AddEngulfment("engulfed", Codex.Sonics.swallow);
      swallowed = AddEngulfment("swallowed", Codex.Sonics.swallow);
    }
#endif

    public readonly Engulfment engulfed;
    public readonly Engulfment swallowed;
  }
}
