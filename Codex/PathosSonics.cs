using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexSonics : CodexPage<ManifestSonics, SonicEditor, Sonic>
  {
    private CodexSonics() { }
#if MASTER_CODEX
    internal CodexSonics(Codex Codex)
      : base(Codex.Manifest.Sonics)
    {
      foreach (var SonicField in typeof(CodexSonics).GetReflectionInfo().GetReflectionFields().Where(T => T.FieldType == typeof(Sonic)).OrderBy(T => T.Name, StringComparer.Ordinal))
        SonicField.SetValue(this, Register.Add(SonicField.Name.Replace('_', ' ')));

      Register.ShortTap.Set(short_tap);
      Register.LongTap.Set(long_tap);
      Register.Introduction.Set(introduction);
      Register.Conclusion.Set(conclusion);
      Register.Switch.Set(@switch);
      Register.Realtime.Set(realtime);
      Register.Turnbased.Set(turnbased);
      Register.Reroll.Set(dice);
      Register.Jump.Set(jump);
      Register.Teleport.Set(teleport);
      Register.Polymorph.Set(polymorph);
      Register.Fizzle.Set(fizzle);
      Register.Foreboding.Set(foreboding);
      Register.Death.Set(death);
      Register.LowHealth.Set(low_health);
      Register.Scrap.Set(scrap);
      Register.Craft.Set(craft);
      Register.Kick.Set(kick);
      Register.Blink.Set(blink);
      Register.Warp.Set(warping);
      Register.Slime.Set(slime);
      Register.Slip.Set(slip);
      Register.Hit.Set(hit);
      Register.Miss.Set(miss);
      Register.GainLevel.Set(gain_level);
      Register.LoseLevel.Set(lose_level);
      Register.GainKarma.Set(gain_karma);
      Register.LoseKarma.Set(lose_karma);
    }

    internal Sonic GetSonicOrNull(string Name)
    {
      return List.Find(G => G.Name.Equals(Name, StringComparison.OrdinalIgnoreCase));
    }
#endif

    public readonly Sonic hit;
    public readonly Sonic miss;
    public readonly Sonic kick;
    public readonly Sonic polymorph;
    public readonly Sonic teleport;
    public readonly Sonic bleat;
    public readonly Sonic blink;
    public readonly Sonic bow_fire;
    public readonly Sonic burble;
    public readonly Sonic buzz;
    public readonly Sonic chime;
    public readonly Sonic clank;
    public readonly Sonic sling_shot;
    public readonly Sonic broken_boulder;
    public readonly Sonic broken_barrel;
    public readonly Sonic broken_door;
    public readonly Sonic broken_glass;
    public readonly Sonic broken_lock;
    public readonly Sonic close_door;
    public readonly Sonic quaff;
    public readonly Sonic ammo;
    public readonly Sonic armour;
    public readonly Sonic amulet;
    public readonly Sonic baa;
    public readonly Sonic book;
    public readonly Sonic chant;
    public readonly Sonic cluck;
    public readonly Sonic coins;
    public readonly Sonic craft;
    public readonly Sonic crumble;
    public readonly Sonic food;
    public readonly Sonic freeze;
    public readonly Sonic gain_ability;
    public readonly Sonic gain_karma;
    public readonly Sonic gain_level;
    public readonly Sonic gain_skill;
    public readonly Sonic gem;
    public readonly Sonic grumble;
    public readonly Sonic hiss;
    public readonly Sonic jump;
    public readonly Sonic leather;
    public readonly Sonic lose_ability;
    public readonly Sonic lose_karma;
    public readonly Sonic lose_level;
    public readonly Sonic lose_skill;
    public readonly Sonic low_health;
    public readonly Sonic potion;
    public readonly Sonic ring;
    public readonly Sonic scroll;
    public readonly Sonic tool;
    public readonly Sonic wand;
    public readonly Sonic weapon;
    public readonly Sonic eat;
    public readonly Sonic electricity;
    public readonly Sonic explosion;
    public readonly Sonic burn;
    public readonly Sonic cackle;
    public readonly Sonic chirp;
    public readonly Sonic creak;
    public readonly Sonic fizzle;
    public readonly Sonic flash;
    public readonly Sonic force;
    public readonly Sonic footsteps;
    public readonly Sonic foreboding;
    public readonly Sonic gas;
    public readonly Sonic gibber;
    public readonly Sonic giggle;
    public readonly Sonic groan;
    public readonly Sonic growl;
    public readonly Sonic grunt;
    public readonly Sonic howl;
    public readonly Sonic launch_grenade;
    public readonly Sonic launch_rocket;
    public readonly Sonic lit_fuse;
    public readonly Sonic locked;
    public readonly Sonic magic;
    public readonly Sonic meow;
    public readonly Sonic neigh;
    public readonly Sonic open_door;
    public readonly Sonic pick_axe;
    public readonly Sonic pistol_fire;
    public readonly Sonic flute;
    public readonly Sonic harp;
    public readonly Sonic horn;
    public readonly Sonic drum;
    public readonly Sonic moan;
    public readonly Sonic whistle;
    public readonly Sonic bugle;
    public readonly Sonic bell;
    public readonly Sonic moo;
    public readonly Sonic oink;
    public readonly Sonic prayer;
    public readonly Sonic read;
    //public readonly Sonic resting;
    public readonly Sonic rifle_burst;
    public readonly Sonic rifle_shot;
    public readonly Sonic roar;
    public readonly Sonic scrap;
    public readonly Sonic scrape;
    public readonly Sonic scuttle;
    public readonly Sonic sever;
    //public readonly Sonic searching;
    public readonly Sonic shackle;
    public readonly Sonic shriek;
    public readonly Sonic shotgun_blast;
    public readonly Sonic shotgun_burst;
    public readonly Sonic sigh;
    public readonly Sonic sizzle;
    public readonly Sonic slime;
    public readonly Sonic slip;
    public readonly Sonic squall;
    public readonly Sonic snarl;
    public readonly Sonic splat;
    public readonly Sonic squawk;
    public readonly Sonic squeak;
    public readonly Sonic squeal;
    public readonly Sonic swallow;
    public readonly Sonic throw_grenade;
    public readonly Sonic throw_object;
    public readonly Sonic throne;
    public readonly Sonic thump;
    public readonly Sonic trumpet;
    public readonly Sonic tunnelling;
    public readonly Sonic wailing;
    //public readonly Sonic waiting;
    public readonly Sonic warping;
    public readonly Sonic water_crash;
    public readonly Sonic water_fountain;
    public readonly Sonic water_impact;
    public readonly Sonic water_splash;   // TODO: rename to just splash?
    public readonly Sonic woof;
    public readonly Sonic write;

    public readonly Sonic introduction;
    public readonly Sonic conclusion;
    public readonly Sonic @switch;
    public readonly Sonic death;
    public readonly Sonic realtime;
    public readonly Sonic turnbased;
    public readonly Sonic short_tap;
    public readonly Sonic long_tap;

    public readonly Sonic dice;
    public readonly Sonic birds;
    public readonly Sonic chains;
    public readonly Sonic drips;
    public readonly Sonic leaves;
    public readonly Sonic roars;
    public readonly Sonic scrapes;
    public readonly Sonic squeaks;
    public readonly Sonic taps;

    public Sonic Add(string Name) => Register.Add(Name);
  }
}