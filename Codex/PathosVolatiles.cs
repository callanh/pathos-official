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

      Volatile AddVolatile(string Name, Glyph ActiveGlyph, Glyph[] HoldGlyphArray, Sonic Sonic, Action<VolatileEditor> Action)
      {
        Debug.Assert(Name != null);
        //Debug.Assert(ActiveGlyph != null);
        //Debug.Assert(HoldGlyph != null);
        Debug.Assert(Sonic != null);

        return Register.Add(S =>
        {
          S.Name = Name;
          S.ActiveGlyph = ActiveGlyph;
          S.HoldGlyphs = HoldGlyphArray ?? Array.Empty<Glyph>();
          S.Sonic = Sonic;

          Debug.Assert(S.HoldGlyphs.IsDistinct(), "HoldGlyphs must be distinct.");

          CodexRecruiter.Enrol(() => Action(S));
        });
      }

      blaze = AddVolatile("blaze", Glyphs.blaze, new[] { Glyphs.scorch }, Sonics.burn, S =>
      {
        S.Apply.Harm(Elements.fire, 2.d6());

        S.Apply.WhenTargetFloor(Grounds.water, T => T.ConvertSpill(blaze, steam, Locality.Square));

        // NOTE: ice ground already reacts to fire and turns to water.
        //S.Apply.WhenTargetFloor(Grounds.ice, T =>
        //{
        //  T.ConvertFloor(Grounds.ice, Grounds.water, Locality.Square);
        //  T.RemoveSpill(blaze);
        //});

        S.AddReaction(Chance.Always, Elements.water, T => T.ExtinguishSpill(blaze));
        S.AddReaction(Chance.Always, Elements.cold, T => T.ExtinguishSpill(blaze));
      });

      blood = AddVolatile("blood", Glyphs.blood, new[] 
      {
        Glyphs.splatter_A,
        Glyphs.splatter_B,
        Glyphs.splatter_C,
        Glyphs.splatter_D,
        Glyphs.splatter_E,
        Glyphs.splatter_F,
        Glyphs.splatter_G,
        Glyphs.splatter_H,
        Glyphs.splatter_I,
        Glyphs.splatter_J,
        Glyphs.splatter_K,
        Glyphs.splatter_L,
        Glyphs.splatter_M,
        Glyphs.splatter_N,
        Glyphs.splatter_O,
        Glyphs.splatter_P,
        Glyphs.splatter_Q,
        Glyphs.splatter_R,
        Glyphs.splatter_S,
        Glyphs.splatter_T,
        Glyphs.splatter_U,
        Glyphs.splatter_V,
        Glyphs.splatter_W,
        Glyphs.splatter_X,
        Glyphs.splatter_Y,
        Glyphs.splatter_Z,
      }, Sonics.splat, S =>
      {
        S.Apply.ApplyTransient(Properties.fumbling, 1.d4() + 10); // TODO: for an active blood spray, needs an animated tile.

        // blood can't exist on these grounds.        
        S.Apply.WhenTargetFloor(Grounds.lava, T => T.RemoveSpill(blood));
        S.Apply.WhenTargetFloor(Grounds.water, T => T.RemoveSpill(blood));
        S.Apply.WhenTargetFloor(Grounds.chasm, T => T.RemoveSpill(blood));

        // water washes away blood.
        S.AddReaction(Chance.Always, Elements.water, T => T.RemoveSpill(blood));
      });

      electricity = AddVolatile("electricity", Glyphs.electricity, null, Sonics.electricity, S =>
      {
        S.Apply.Harm(Elements.shock, 4.d4());
        S.Apply.WhenChance(Chance.OneIn4, T => T.UnlessTargetResistant(Elements.shock, R => R.ApplyTransient(Properties.confusion, 1.d4() + 1)));
      });

      freeze = AddVolatile("freeze", Glyphs.freeze, null, Sonics.freeze, S =>
      {
        S.Apply.Harm(Elements.cold, 3.d4());
        S.Apply.WhenChance(Chance.OneIn4, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.slowness, 1.d4() + 1)));

        // NOTE: water ground already reacts to cold and turns to ice.
        //S.Apply.WhenTargetFloor(Grounds.water, T =>
        //{
        //  T.ConvertFloor(Grounds.water, Grounds.ice, Locality.Square);
        //  T.RemoveSpill(freeze);
        //});

        S.Apply.WhenTargetFloor(Grounds.lava, T => T.RemoveSpill(freeze));

        S.AddReaction(Chance.Always, Elements.fire, T => T.ExtinguishSpill(freeze));
      });

      // TODO: 'puddle' of water?

      steam = AddVolatile("steam", Glyphs.steam, null, Sonics.gas, S =>
      {
        S.Apply.Harm(Elements.water, Dice.Zero);
        S.Apply.ApplyTransient(Properties.blindness, 1.d4() + 2);
        S.Apply.WhenChance(Chance.OneIn20, V => V.ConvertAsset(Codex.Stocks.scroll, WholeStack: true, Codex.Items.scroll_of_blank_paper)); // potions & books are 'closed' so won't be directly affected by steam.

        S.AddReaction(Chance.Always, Elements.cold, T => T.ExtinguishSpill(steam));
      });

      // TODO: brambles/swarm/oil/mucous/blood/smoke/pollution? what about positive fields such as life/mana recovery?
    }
#endif

    public readonly Volatile blaze;
    public readonly Volatile blood;
    public readonly Volatile electricity;
    public readonly Volatile freeze;
    public readonly Volatile steam;
  }
}