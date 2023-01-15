using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
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
}
