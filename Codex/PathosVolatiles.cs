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

      blaze = AddVolatile("blaze", Glyphs.blaze, Glyphs.scorch, Sonics.burn, S => // TODO: rename 'flames'?
      {
        S.Apply.Harm(Elements.fire, 2.d6());
        S.Apply.WhenTargetFloor(Grounds.water, T => T.ConvertSpill(blaze, steam, Locality.Square));
        S.Apply.WhenTargetFloor(Grounds.ice, T =>
        {
          T.ConvertFloor(Grounds.ice, Grounds.water, Locality.Square);
          T.RemoveSpill(blaze);
        });

        S.AddReaction(Chance.Always, Elements.water, T =>
        {
          T.ExtinguishSpill(blaze);
        });
        S.AddReaction(Chance.Always, Elements.cold, T =>
        {
          T.ExtinguishSpill(blaze);
        });
      });

      electricity = AddVolatile("electricity", Glyphs.electricity, null, Sonics.electricity, S =>
      {
        S.Apply.Harm(Elements.shock, 4.d4());
      });

      freeze = AddVolatile("freeze", Glyphs.freeze, null, Sonics.freeze, S =>
      {
        S.Apply.Harm(Elements.cold, 3.d4());

        // NOTE: water ground already reacts to cold and turns to ice.
        //S.Apply.WhenTargetFloor(Grounds.water, T =>
        //{
        //  T.ConvertFloor(Grounds.water, Grounds.ice, Locality.Square);
        //  T.RemoveSpill(freeze);
        //});

        S.Apply.WhenTargetFloor(Grounds.lava, T =>
        {
          T.RemoveSpill(freeze);
        });

        S.AddReaction(Chance.Always, Elements.fire, T =>
        {
          T.ExtinguishSpill(freeze);
        });
      });

      steam = AddVolatile("steam", Glyphs.steam, null, Sonics.gas, S =>
      {
        S.Apply.Harm(Elements.water, Dice.Zero);
        S.Apply.ApplyTransient(Properties.blindness, 1.d4() + 2);
        S.Apply.WhenChance(Chance.OneIn20, V => V.ConvertAsset(Codex.Stocks.scroll, WholeStack: true, Codex.Items.scroll_of_blank_paper)); // potions & books are 'closed' so won't be directly affected by steam.

        S.AddReaction(Chance.Always, Elements.cold, T =>
        {
          T.ExtinguishSpill(steam);
        });
      });

      // TODO: brambles/swarm/oil/mucous/blood/smoke/pollution? what about positive fields such as life/mana recovery?
    }
#endif

    public readonly Volatile blaze;
    public readonly Volatile electricity;
    public readonly Volatile freeze;
    public readonly Volatile steam;
  }
}