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

      Motion AddMotion(string PresentName, string PastName, Attribute InfluencingAttribute, bool UseVision, bool UseHands, bool UseConcentration, bool UseVoice, Skill PractisingSkill, Action<MotionEditor> EditorAction)
      {
        return Add(M =>
        {
          M.PresentName = PresentName;
          M.PastName = PastName;
          M.InfluencingAttribute = InfluencingAttribute;
          M.UseVision = UseVision;
          M.UseHands = UseHands;
          M.UseConcentration = UseConcentration;
          M.UseVoice = UseVoice;
          M.PractisingSkill = PractisingSkill;

          EditorAction(M);
        });
      }

      anoint = AddMotion("anoint", "anointed", Attributes.wisdom, UseVision: false, UseHands: true, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      capture = AddMotion("capture", "captured", Attributes.dexterity, UseVision: false, UseHands: true, UseConcentration: false, UseVoice: true, PractisingSkill: null, M =>
      {
        M.RequiresCharacter = false;
      });
      chant = AddMotion("chant", "chanted", Attributes.charisma, UseVision: false, UseHands: false, UseConcentration: false, UseVoice: true, PractisingSkill: null, M =>
      {
      });
      construct = AddMotion("construct", "constructed", Attributes.dexterity, UseVision: true, UseHands: true, UseConcentration: true, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      dig = AddMotion("dig", "dug", Attributes.strength, UseVision: false, UseHands: true, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
        M.Digging = true;
      });
      dip = AddMotion("dip", "dipped", Attributes.dexterity, UseVision: false, UseHands: true, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      divine = AddMotion("divine", "divined", Attributes.wisdom, UseVision: false, UseHands: false, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      drink = AddMotion("drink", "drank", Attributes.constitution, UseVision: false, UseHands: false, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      eat = AddMotion("eat", "ate", Attributes.constitution, UseVision: false, UseHands: false, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
        M.Ingesting = true;
      });
      empty = AddMotion("empty", "emptied", Attributes.strength, UseVision: false, UseHands: false, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      exchange = AddMotion("exchange", "exchanged", Attributes.charisma, UseVision: true, UseHands: false, UseConcentration: true, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      flash = AddMotion("flash", "flashed", Attributes.dexterity, UseVision: false, UseHands: true, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      open = AddMotion("open", "opened", Attributes.strength, UseVision: false, UseHands: true, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      pack = AddMotion("pack", "packed", Attributes.dexterity, UseVision: true, UseHands: true, UseConcentration: true, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      play = AddMotion("play", "played", Attributes.charisma, UseVision: false, UseHands: true, UseConcentration: false, UseVoice: true, PractisingSkill: Skills.music, M =>
      {
      });
      pray = AddMotion("pray", "prayed", Attributes.charisma, UseVision: false, UseHands: false, UseConcentration: false, UseVoice: true, PractisingSkill: null, M =>
      {
      });
      quaff = AddMotion("quaff", "quaffed", Attributes.constitution, UseVision: false, UseHands: true, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
        M.Impacting = true;
      });
      read = AddMotion("read", "read", Attributes.intelligence, UseVision: true, UseHands: false, UseConcentration: false, UseVoice: true, PractisingSkill: Skills.literacy, M =>
      {
      });
      refill = AddMotion("refill", "refilled", Attributes.wisdom, UseVision: false, UseHands: true, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      release = AddMotion("release", "released", Attributes.dexterity, UseVision: false, UseHands: true, UseConcentration: false, UseVoice: true, PractisingSkill: null, M =>
      {
        M.RequiresCharacter = true;
      });
      rename = AddMotion("rename", "renamed", Attributes.wisdom, UseVision: true, UseHands: true, UseConcentration: true, UseVoice: false, PractisingSkill: Skills.literacy, M =>
      {
        M.Aliasing = true;
      });
      inscribe = AddMotion("inscribe", "inscribed", Attributes.wisdom, UseVision: true, UseHands: true, UseConcentration: true, UseVoice: false, PractisingSkill: Skills.literacy, M =>
      {
        M.Inscribing = true;
      });
      rub = AddMotion("rub", "rubbed", Attributes.strength, UseVision: false, UseHands: true, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      sacrifice = AddMotion("sacrifice", "sacrificed", Attributes.charisma, UseVision: false, UseHands: false, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
        M.Repeating = true;
      });
      scry = AddMotion("scry", "scried", Attributes.wisdom, UseVision: true, UseHands: false, UseConcentration: true, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      set = AddMotion("set", "set", Attributes.dexterity, UseVision: false, UseHands: true, true, UseVoice: false, PractisingSkill: Skills.traps, M =>
      {
        M.Landing = true;
      });
      sit = AddMotion("sit", "sat", Attributes.dexterity, UseVision: false, UseHands: false, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
        M.Grounding = true;
      });
      recline = AddMotion("recline", "reclined", Attributes.constitution, UseVision: false, UseHands: false, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      stake = AddMotion("stake", "staked", Attributes.strength, UseVision: true, UseHands: true, UseConcentration: true, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      study = AddMotion("study", "studied", Attributes.intelligence, UseVision: true, UseHands: false, UseConcentration: true, UseVoice: false, PractisingSkill: Skills.literacy, M =>
      {
      });
      swat = AddMotion("swat", "swatted", Attributes.dexterity, UseVision: true, UseHands: true, UseConcentration: true, UseVoice: false, PractisingSkill: null, M =>
      {
      });
      write = AddMotion("write", "wrote", Attributes.intelligence, UseVision: true, UseHands: true, UseConcentration: false, UseVoice: false, PractisingSkill: Skills.literacy, M =>
      {
      });
      zap = AddMotion("zap", "zapped", Attributes.wisdom, UseVision: false, UseHands: true, UseConcentration: false, UseVoice: false, PractisingSkill: null, M =>
      {
      });
    }
#endif

    public readonly Motion anoint;
    public readonly Motion capture;
    public readonly Motion chant;
    public readonly Motion construct;
    public readonly Motion dig;
    public readonly Motion dip;
    public readonly Motion divine;
    public readonly Motion drink;
    public readonly Motion eat;
    public readonly Motion empty;
    public readonly Motion exchange;
    public readonly Motion flash;
    public readonly Motion open;
    public readonly Motion pack;
    public readonly Motion play;
    public readonly Motion pray;
    public readonly Motion quaff;
    public readonly Motion read;
    public readonly Motion refill;
    public readonly Motion release;
    public readonly Motion rename;
    public readonly Motion inscribe;
    public readonly Motion rub;
    public readonly Motion sacrifice;
    public readonly Motion scry;
    public readonly Motion set;
    public readonly Motion sit;
    public readonly Motion recline;
    public readonly Motion stake;
    public readonly Motion study;
    public readonly Motion swat;
    public readonly Motion write;
    public readonly Motion zap;
  }
}