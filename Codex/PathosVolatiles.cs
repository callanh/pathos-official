using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexVolatiles : CodexPage<ManifestVolatiles, VolatileEditor, Volatile>
  {
    private CodexVolatiles() { }
#if MASTER_CODEX
    internal CodexVolatiles(Codex Codex)
      : base(Codex.Manifest.Volatiles)
    {
      var Glyphs = Codex.Glyphs;
      var Sonics = Codex.Sonics;
      var Elements = Codex.Elements;
      var Properties = Codex.Properties;
      var Grounds = Codex.Grounds;

      Volatile AddVolatile(string Name, Glyph ActiveGlyph, Glyph HoldGlyph, Sonic Sonic, Action<VolatileEditor> Action)
      {
        Debug.Assert(Name != null);
        Debug.Assert(ActiveGlyph != null);
        //Debug.Assert(HoldGlyph != null);
        Debug.Assert(Sonic != null);

        return Register.Add(S =>
        {
          S.Name = Name;
          S.ActiveGlyph = ActiveGlyph;
          S.HoldGlyph = HoldGlyph;
          S.Sonic = Sonic;

          CodexRecruiter.Enrol(() => Action(S));
        });
      }

      blaze = AddVolatile("blaze", Glyphs.blaze, Glyphs.scorch, Sonics.burn, S =>
      {
        S.Apply.Harm(Elements.fire, 2.d6());

        S.Apply.WhenTargetFloor(Grounds.water, T => T.ConvertSpill(blaze, steam, Locality.Square));

        S.Apply.WhenTargetFloor(Grounds.ice, T =>
        {
          T.ConvertFloor(Grounds.ice, Grounds.water, Locality.Square);
          T.RemoveSpill(blaze);
        });

      });

      //blizzard = AddVolatile("blizzard", Glyphs.blizzard, Glyphs.frost, Sonics.freeze, S =>
      //{
      //  S.Apply.Harm(Elements.cold, 2.d4());
      //});

      steam = AddVolatile("steam", Glyphs.steam, null, Sonics.gas, S =>
      {
        // TODO: obscure vision?
        //S.Apply.Light(false, Locality.Square);
      });

      // TODO: swarm/oil/mucous/blood/smoke/pollution/blizzard? what about positive fields such as life/mana recovery?
    }
#endif

    public readonly Volatile blaze;
    //public readonly Volatile blizzard;
    public readonly Volatile steam;
  }
}