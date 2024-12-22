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
      var Attributes = Codex.Attributes;
      var Skills = Codex.Skills;
      var Anatomies = Codex.Anatomies;

      Motion AddMotion(string PresentName, string PastName, Attribute InfluencingAttribute, Action<MotionEditor> EditorAction)
      {
        return Register.Add(M =>
        {
          M.PresentName = PresentName;
          M.PastName = PastName;
          M.InfluencingAttribute = InfluencingAttribute;

          EditorAction(M);
        });
      }

      anoint = AddMotion("anoint", "anointed", Attributes.wisdom, M =>
      {
        M.UseAnatomy(Anatomies.hands);
      });

      capture = AddMotion("capture", "captured", Attributes.dexterity, M =>
      {
        M.RequiresCharacter = false;
        M.UseAnatomy(Anatomies.hands);
      });

      chant = AddMotion("chant", "chanted", Attributes.charisma, M =>
      {
        M.UseVoice = true;
        M.UseAnatomy(Anatomies.voice);
      });

      construct = AddMotion("construct", "constructed", Attributes.dexterity, M =>
      {
        M.UseVision = true;
        M.UseConcentration = true;
        M.UseAnatomy(Anatomies.hands, Anatomies.eyes, Anatomies.mind);
      });

      copy = AddMotion("copy", "copied", Attributes.intelligence, M =>
      {
        M.UseAnatomy(Anatomies.hands);
      });

      dig = AddMotion("dig", "dug", Attributes.strength, M =>
      {
        M.Digging = true;
        M.UseAnatomy(Anatomies.hands);
      });

      dip = AddMotion("dip", "dipped", Attributes.dexterity, M =>
      {
        M.UseAnatomy(Anatomies.hands);
      });

      divine = AddMotion("divine", "divined", Attributes.wisdom, M =>
      {
      });

      drink = AddMotion("drink", "drank", Attributes.constitution, M =>
      {
      });

      eat = AddMotion("eat", "ate", Attributes.constitution, M =>
      {
        M.Ingesting = true;
      });

      empty = AddMotion("empty", "emptied", Attributes.strength, M =>
      {
      });

      exchange = AddMotion("exchange", "exchanged", Attributes.charisma, M =>
      {
        M.UseVision = true;
        M.UseConcentration = true;
        M.UseAnatomy(Anatomies.eyes, Anatomies.mind);
      });

      inscribe = AddMotion("inscribe", "inscribed", Attributes.wisdom, M =>
      {
        M.Inscribing = true;
        M.UseVision = true;
        M.UseConcentration = true;
        M.PractisingSkill = Skills.literacy;
        M.UseAnatomy(Anatomies.hands, Anatomies.eyes, Anatomies.mind);
      });

      flash = AddMotion("flash", "flashed", Attributes.dexterity, M =>
      {
        M.UseAnatomy(Anatomies.hands);
      });

      mount = AddMotion("mount", "mounted", Attributes.dexterity, M =>
      {
        M.Grounding = true;
      });

      open = AddMotion("open", "opened", Attributes.strength, M =>
      {
        M.UseAnatomy(Anatomies.hands);
      });

      pack = AddMotion("pack", "packed", Attributes.dexterity, M =>
      {
        M.UseVision = true;
        M.UseConcentration = true;
        M.UseAnatomy(Anatomies.hands, Anatomies.eyes, Anatomies.mind);
      });

      play = AddMotion("play", "played", Attributes.charisma, M =>
      {
        M.UseVoice = true;
        M.PractisingSkill = Skills.music;
        M.UseAnatomy(Anatomies.hands, Anatomies.voice);
      });

      pray = AddMotion("pray", "prayed", Attributes.charisma, M =>
      {
        M.UseVoice = true;
        M.UseAnatomy(Anatomies.voice);
      });

      quaff = AddMotion("quaff", "quaffed", Attributes.constitution, M =>
      {
        M.Impacting = true;
        M.UseAnatomy(Anatomies.hands);
      });

      read = AddMotion("read", "read", Attributes.intelligence, M =>
      {
        M.UseVision = true;
        M.UseVoice = true;
        M.PractisingSkill = Skills.literacy;
        M.UseAnatomy(Anatomies.eyes, Anatomies.voice);
      });

      refill = AddMotion("refill", "refilled", Attributes.wisdom, M =>
      {
        M.UseAnatomy(Anatomies.hands);
      });

      release = AddMotion("release", "released", Attributes.dexterity, M =>
      {
        M.RequiresCharacter = true;
        M.UseAnatomy(Anatomies.hands, Anatomies.voice);
      });

      rename = AddMotion("rename", "renamed", Attributes.wisdom, M =>
      {
        M.Aliasing = true;
        M.UseVision = true;
        M.UseConcentration = true;
        M.PractisingSkill = Skills.literacy;
        M.UseAnatomy(Anatomies.hands, Anatomies.eyes, Anatomies.mind);
      });

      rub = AddMotion("rub", "rubbed", Attributes.strength, M =>
      {
        M.UseAnatomy(Anatomies.hands);
      });

      sacrifice = AddMotion("sacrifice", "sacrificed", Attributes.charisma, M =>
      {
        M.Repeating = true;
      });

      scry = AddMotion("scry", "scried", Attributes.wisdom, M =>
      {
        M.UseVision = true;
        M.UseConcentration = true;
        M.UseAnatomy(Anatomies.eyes, Anatomies.mind);
      });

      set = AddMotion("set", "set", Attributes.dexterity, M =>
      {
        M.Landing = true;
        M.UseConcentration = true;
        M.PractisingSkill = Skills.traps;
        M.UseAnatomy(Anatomies.hands, Anatomies.mind);
      });

      recline = AddMotion("recline", "reclined", Attributes.constitution, M =>
      {
      });

      sit = AddMotion("sit", "sat", Attributes.dexterity, M =>
      {
        M.Grounding = true;
      });

      stake = AddMotion("stake", "staked", Attributes.strength, M =>
      {
        M.UseVision = true;
        M.UseConcentration = true;
        M.UseAnatomy(Anatomies.hands, Anatomies.eyes, Anatomies.mind);
      });

      study = AddMotion("study", "studied", Attributes.intelligence, M =>
      {
        M.UseVision = true;
        M.UseConcentration = true;
        M.PractisingSkill = Skills.literacy;
        M.UseAnatomy(Anatomies.eyes, Anatomies.mind);
      });

      swat = AddMotion("swat", "swatted", Attributes.dexterity, M =>
      {
        M.UseVision = true;
        M.UseConcentration = true;
        M.UseAnatomy(Anatomies.hands, Anatomies.eyes, Anatomies.mind);
      });

      write = AddMotion("write", "wrote", Attributes.intelligence, M =>
      {
        M.UseVision = true;
        M.PractisingSkill = Skills.literacy;
        M.UseAnatomy(Anatomies.hands, Anatomies.eyes);
      });

      zap = AddMotion("zap", "zapped", Attributes.wisdom, M =>
      {
        M.UseAnatomy(Anatomies.hands);
      });
    }
#endif

    public readonly Motion anoint;
    public readonly Motion capture;
    public readonly Motion chant;
    public readonly Motion construct;
    public readonly Motion copy;
    public readonly Motion dig;
    public readonly Motion dip;
    public readonly Motion divine;
    public readonly Motion drink;
    public readonly Motion eat;
    public readonly Motion empty;
    public readonly Motion exchange;
    public readonly Motion flash;
    public readonly Motion inscribe;
    public readonly Motion mount;
    public readonly Motion open;
    public readonly Motion pack;
    public readonly Motion play;
    public readonly Motion pray;
    public readonly Motion quaff;
    public readonly Motion read;
    public readonly Motion recline;
    public readonly Motion refill;
    public readonly Motion release;
    public readonly Motion rename;
    public readonly Motion rub;
    public readonly Motion sacrifice;
    public readonly Motion scry;
    public readonly Motion set;
    public readonly Motion sit;
    public readonly Motion stake;
    public readonly Motion study;
    public readonly Motion swat;
    public readonly Motion write;
    public readonly Motion zap;
  }
}