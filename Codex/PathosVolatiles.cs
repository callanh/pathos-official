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
          S.HoldGlyphs = HoldGlyphArray ?? [];
          S.Sonic = Sonic;

          Debug.Assert(S.HoldGlyphs.IsDistinct(), "HoldGlyphs must be distinct.");

          CodexRecruiter.Enrol(() => Action(S));
        });
      }

      blaze = AddVolatile("blaze", Glyphs.blaze, [Glyphs.scorch], Sonics.burn, S =>
      {
        S.Apply.HarmEntity(Elements.fire, 2.d6());

        S.Apply.WhenTargetGround(Grounds.water, T => T.ConvertVolatile(blaze, steam, Locality.Square));

        // NOTE: ice ground already reacts to fire and turns to water.
        //S.Apply.WhenTargetFloor(Grounds.ice, T =>
        //{
        //  T.ConvertFloor(Grounds.ice, Grounds.water, Locality.Square);
        //  T.RemoveSpill(blaze);
        //});

        S.AddReaction(Chance.Always, Elements.water, T => T.ExtinguishVolatile(blaze));
        S.AddReaction(Chance.Always, Elements.cold, T => T.ExtinguishVolatile(blaze));
      });

      blood = AddVolatile("blood", Glyphs.blood,
      [
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
      ], Sonics.splat, S =>
      {
        S.Apply.ApplyTransient(Properties.fumbling, 1.d4() + 10); // TODO: for an active blood spray, needs an animated tile.

        // blood can't exist on these grounds.        
        S.Apply.WhenTargetGround(Grounds.lava, T => T.DestroyVolatile(blood));
        S.Apply.WhenTargetGround(Grounds.water, T => T.DestroyVolatile(blood));
        S.Apply.WhenTargetGround(Grounds.chasm, T => T.DestroyVolatile(blood));

        // water washes away blood.
        S.AddReaction(Chance.Always, Elements.water, T => T.DestroyVolatile(blood));
      });

      electricity = AddVolatile("electricity", Glyphs.electricity, null, Sonics.electricity, S =>
      {
        S.Apply.HarmEntity(Elements.shock, 4.d4());
        S.Apply.WhenChance(Chance.OneIn4, T => T.UnlessTargetResistant(Elements.shock, R => R.ApplyTransient(Properties.confusion, 1.d4() + 1)));
      });

      freeze = AddVolatile("freeze", Glyphs.freeze, null, Sonics.freeze, S =>
      {
        S.Apply.HarmEntity(Elements.cold, 3.d4());
        S.Apply.WhenChance(Chance.OneIn4, T => T.UnlessTargetResistant(Elements.cold, R => R.ApplyTransient(Properties.slowness, 1.d4() + 1)));

        // NOTE: water ground already reacts to cold and turns to ice.
        //S.Apply.WhenTargetFloor(Grounds.water, T =>
        //{
        //  T.ConvertFloor(Grounds.water, Grounds.ice, Locality.Square);
        //  T.RemoveSpill(freeze);
        //});

        S.Apply.WhenTargetGround(Grounds.lava, T => T.DestroyVolatile(freeze));

        S.AddReaction(Chance.Always, Elements.fire, T => T.ExtinguishVolatile(freeze));
      });
      /*
      oil = AddVolatile("oil", Glyphs.oil, new[] { Glyphs.oil } , Sonics.water_splash, S =>
      {
        // active apply.
        S.Apply.ApplyTransient(Properties.fumbling, 4.d4() + 4);
        S.Apply.ApplyTransient(Properties.slippery, 4.d4() + 4);

        // TODO: SetSlippery?

        S.AddReaction(Chance.Always, Elements.fire, T => T.CreateVolatile(blaze, 4.d60()));
      });
      */
      // TODO: 'puddle' of water?

      steam = AddVolatile("steam", Glyphs.steam, null, Sonics.gas, S =>
      {
        S.Apply.HarmEntity(Elements.water, Dice.Zero);
        S.Apply.ApplyTransient(Properties.blindness, 1.d4() + 2);
        S.Apply.WhenChance(Chance.OneIn20, V => V.ConvertItem(Codex.Stocks.scroll, WholeStack: true, Codex.Items.scroll_of_blank_paper)); // potions & books are 'closed' so won't be directly affected by steam.

        S.AddReaction(Chance.Always, Elements.cold, T => T.ExtinguishVolatile(steam));
      });

      // TODO: brambles/swarm/oil/mucous/blood/smoke/pollution? what about positive fields such as life/mana recovery?
    }
#endif

    public readonly Volatile blaze;
    public readonly Volatile blood;
    public readonly Volatile electricity;
    public readonly Volatile freeze;
    //public readonly Volatile oil;
    public readonly Volatile steam;
  }
}