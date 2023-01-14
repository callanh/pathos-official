using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexMotions : CodexPage<ManifestMotions, MotionEditor, Motion>
  {
    private CodexMotions() { }
#if MASTER_CODEX
    internal CodexMotions(Codex Codex)
      : base(Codex.Manifest.Motions)
    {

    }
#endif

    public Motion anoint => Register.anoint;
    public Motion capture => Register.capture;
    public Motion chant => Register.chant;
    public Motion construct => Register.construct;
    public Motion dig => Register.dig;
    public Motion dip => Register.dip;
    public Motion divine => Register.divine;
    public Motion drink => Register.drink;
    public Motion eat => Register.eat;
    public Motion empty => Register.empty;
    public Motion exchange => Register.exchange;
    public Motion flash => Register.flash;
    public Motion open => Register.open;
    public Motion pack => Register.pack;
    public Motion play => Register.play;
    public Motion pray => Register.pray;
    public Motion quaff => Register.quaff;
    public Motion read => Register.read;
    public Motion refill => Register.refill;
    public Motion release => Register.release;
    public Motion rename => Register.rename;
    public Motion inscribe => Register.inscribe;
    public Motion rub => Register.rub;
    public Motion sacrifice => Register.sacrifice;
    public Motion scry => Register.scry;
    public Motion set => Register.set;
    public Motion sit => Register.sit;
    public Motion recline => Register.recline;
    public Motion stake => Register.stake;
    public Motion study => Register.study;
    public Motion swat => Register.swat;
    public Motion write => Register.write;
    public Motion zap => Register.zap;
  }
}