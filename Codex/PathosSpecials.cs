using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexSpecials : CodexPage<ManifestSpecials, SpecialEditor, Special>
  {
    private CodexSpecials() { }
#if MASTER_CODEX
    internal CodexSpecials(Codex Codex)
      : base(Codex.Manifest.Specials)
    {
      var Glyphs = Codex.Glyphs;
      var Diets = Codex.Diets;
      var Items = Codex.Items;
      var Entities = Codex.Entities;
      var Materials = Codex.Materials;
      var Properties = Codex.Properties;
      var Elements = Codex.Elements;
      var Skills = Codex.Skills;
      var Spells = Codex.Spells;

      // NOTE: Special.Name should be a noun, not an adjective.

      Special AddSpecial(string Name, Action<SpecialEditor> Action)
      {
        return Register.Add(S =>
        {
          S.Name = Name;

          CodexRecruiter.Enrol(() => Action(S));
        });
      }
      Special AddLycanthrope(Entity Animal, Entity Beast, Entity Humanoid)
      {
        return AddSpecial("were" + Animal.Name, S =>
        {
          S.Description = $"Forever cursed with lycanthropy, these beings can transform themselves into monstrous {Animal.Name} forms.";
          S.Glyph = Humanoid.Glyph;
          S.Chemistry.SetVulnerability(Materials.silver);
          S.Startup.SetResistance(Elements.drain);
          S.SetTransformations(Beast, Humanoid);
        });
      }
      Special AddElemental(string Name, Glyph Glyph, Element PositiveElement, Element NegativeElement, Spell SphereSpell)
      {
        return AddSpecial(Name, S =>
        {
          S.Description = $"Highly attuned servants of elemental {Name}, they can conjure exploding spheres to destroy their enemies.";
          S.Glyph = Glyph;

          S.Chemistry.SetWeakness(NegativeElement);
          S.Startup.SetResistance(PositiveElement);
          S.Startup.SetTradeoffSkill(Skills.conjuration, SkillCategory.Defensive);
          S.Startup.AddGrimoire(Dice.One, SphereSpell);
        });
      }

      this.colossus = AddSpecial("colossus", S =>
      {
        S.Description = "Being unusually tall and heavy, they move a bit slower but are more resilient.";
        S.Glyph = Glyphs.colossus;

        S.LifeAdvancement.Set(Dice.Zero + 2);
        S.SpeedRateDelta = -0.25F;
        S.WeightMultiplier = 1.50F;
        S.DefenceModifier = Modifier.Plus1;
      });

      this.drunkard = AddSpecial("drunkard", S =>
      {
        S.Description = "The habitually drunk make for brazen yet slightly unsteady adventurers; just don't ask them to recite the alphabet backwards.";
        S.Glyph = Glyphs.drunkard_special;

        S.Startup.SetTalent(Properties.inebriation);
        S.Startup.SetPunishment(Codex.Punishments.thirst);
      });

      this.glass = AddSpecial("glass", S =>
      {
        S.Description = "Sculptured from living glass that reflects energy and makes a perfect but delicate emulation of their natural counterpart.";
        S.Glyph = Glyphs.glass_special;

        S.Diet = Diets.geophagy;
        S.LifeAdvancement.Set(Dice.Zero - 2);
        S.ManaAdvancement.Set(Dice.Zero + 2);
        S.DefenceModifier = Modifier.Minus2;
        S.Startup.SetTalent(Properties.reflection);
        S.Chemistry.SetWeakness(Elements.force);
        S.SetMaskFigure().Set
        (
          Material: Materials.glass,
          Head: true,
          Mind: true,
          Voice: true,
          Eyes: true,
          Ears: true,
          Hands: true,
          Limbs: true,
          Feet: true,
          Thermal: false,
          Blood: false,
          Mounted: true,
          Amorphous: true
        );
      });

      this.midget = AddSpecial("midget", S =>
      {
        S.Description = "Being unusually short and slight, they are more nimble but not as effective in combat.";
        S.Glyph = Glyphs.midget;

        S.LifeAdvancement.Set(Dice.Zero - 1);
        S.SpeedRateDelta = +0.25F;
        S.WeightMultiplier = 0.75F;
        S.AttackModifier = Modifier.Minus1;
      });

      this.fugitive = AddSpecial("fugitive", S =>
      {
        S.Description = "Whether innocent or guilty of their accused crimes, these individuals are desperate to escape custody.";
        S.Glyph = Glyphs.fugitive;

        S.Startup.SetPunishment(Codex.Punishments.wanted);
      });

      this.noble = AddSpecial("noble", S =>
      {
        S.Description = "Aristocrats who enjoy all the privilege that comes with wealth and wonder why everyone hates them.";
        S.Glyph = Glyphs.noble;

        S.Startup.SetTalent(Properties.aggravation);
        S.Startup.Loot.AddKit(Chance.Always, 1.d1000() + 1000, Items.gold_coin);
      });

      this.protagonist = AddSpecial("protagonist", S =>
      {
        S.Description = "Main character syndrome has resulted in a reliance on plot armour but they are destined for a major reality check.";
        S.Glyph = Glyphs.protagonist;
      
        S.DefenceModifier = Modifier.Minus1;
        S.Startup.SetAcquisition(Properties.lifesaving);
      });

      this.psychic = AddSpecial("psychic", S =>
      {
        S.Description = "Gifted with extrasensory perception to identify entities otherwise hidden from the normal senses but at the cost of a gnawing hunger.";
        S.Glyph = Glyphs.psychic_special;

        S.Startup.SetTalent(Properties.telepathy, Properties.telekinesis, Properties.clairvoyance, Properties.hunger);
      });

      this.quantum = AddSpecial("quantum", S =>
      {
        S.Description = "Positionally uncertain, these individuals are accustomed to being anywhere and everywhere all at once.";
        S.Glyph = Glyphs.quantum_special;

        S.Startup.SetTalent(Properties.teleportation);
      });

      this.scholar = AddSpecial("scholar", S =>
      {
        S.Description = "Lifetime of study has focused on learning from books at the cost of other hobbies and fitness.";
        S.Glyph = Glyphs.scholar;

        S.LifeAdvancement.Set(Dice.Zero - 1); // -1 per level.
        S.Startup.SetTradeoffSkill(Skills.literacy, SkillCategory.Utility);
        S.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(3), Items.scroll_of_blank_paper);
        S.Startup.Loot.AddKit(Chance.Always, Dice.Fixed(1), Items.book_of_blank_paper);
        S.Startup.Loot.AddKit(Chance.Always, Items.magic_marker);
      });

      this.skeleton = AddSpecial("skeleton", S =>
      {
        S.Description = "Somehow still alive, albeit without the flesh required to be truly living, this peculiar existence has some advantages.";
        S.Glyph = Glyphs.skeleton_special;

        S.Diet = Diets.inediate;
        S.DefenceBias.Bludgeon = Modifier.Minus2;
        S.DefenceBias.Pierce = Modifier.Plus2;
        S.SpeedRateDelta = -0.50F;
        S.WeightMultiplier = 0.30F;
        S.SetMaskFigure().Set
        (
          Material: Materials.bone,
          Head: true,
          Mind: true,
          Voice: true,
          Eyes: true,
          Ears: false,
          Hands: true,
          Limbs: true,
          Feet: true,
          Thermal: false,
          Blood: false,
          Mounted: true,
          Amorphous: true
        );
        S.LifeAdvancement.Set(Dice.Zero - 1); // -1 per level.
        S.ManaAdvancement.Set(Dice.Zero - 1); // -1 per level.
        S.Startup.SetTalent(Properties.vitality);
        S.Startup.SetResistance(Elements.poison);
        S.Startup.Loot.AddKit(Chance.Always, Items.brass_bugle); // doot doot.
      });

      this.vampire = AddSpecial("vampire", S =>
      {
        S.Description = "Forsaken creature that subsists by feeding on the vital essence of the living.";
        S.Glyph = Glyphs.vampire_special;

        S.Diet = Diets.hematophagy;
        S.Chemistry.SetVulnerability(Materials.silver);

        S.SetMaskFigure().Set
        (
          Material: null,
          Head: true,
          Mind: true,
          Voice: true,
          Eyes: true,
          Ears: true,
          Hands: true,
          Limbs: true,
          Feet: true,
          Thermal: false,
          Blood: true,
          Mounted: true,
          Amorphous: true
        );

        S.Startup.SetTalent(Properties.dark_vision);
        S.Startup.SetResistance(Elements.sleep);

        S.SetTransformations(Entities.vampire_bat, Entities.fog_cloud);
      });

      this.frost = AddElemental("frost", Glyphs.frost_special, Elements.cold, Elements.fire, Spells.freezing_sphere);
      this.flame = AddElemental("flame", Glyphs.flame_special, Elements.fire, Elements.cold, Spells.flaming_sphere);
      this.shock = AddElemental("shock", Glyphs.shock_special, Elements.shock, Elements.drain, Spells.shocking_sphere);
      this.earth = AddElemental("earth", Glyphs.earth_special, Elements.petrify, Elements.disintegrate, Spells.crushing_sphere);
      this.water = AddElemental("water", Glyphs.water_special, Elements.acid, Elements.shock, Spells.soaking_sphere);

      // lycanthrope: wolf, jackal, rat, panther, snaker, spider, tiger, wolf.
      //this.werejackal = AddLycanthrope(Entities.jackal, Entities.jackalwere, Entities.werejackal);
      //this.wererat = AddLycanthrope(Entities.giant_rat, Entities.ratwere, Entities.wererat);
      //this.werepanther = AddLycanthrope(Entities.panther, Entities.pantherwere, Entities.werepanther);
      //this.weresnake = AddLycanthrope(Entities.snake, Entities.snakewere, Entities.weresnake);
      //this.werespider = AddLycanthrope(Entities.giant_spider, Entities.spiderwere, Entities.werespider);
      //this.weretiger = AddLycanthrope(Entities.tiger, Entities.tigerwere, Entities.weretiger);
      this.werewolf = AddLycanthrope(Entities.wolf, Entities.wolfwere, Entities.werewolf);

      // https://docs.google.com/document/d/1ZhMiTDQoG988_1QpnmQ4HicclRRYtXqny9YTREPngCQ/edit
      // purist - class-only skill progression.
      // deaf/blind/mute (massive challenge mode, but would it be fun for _anyone_?)
      // imbued (+mana, -life?)
      // mutant: pig, cat, frog, turtle, rat, bird.
      // shapeshifter (doppelganger power at will)
      // astral (too close to Echo?)
      // zealot: +karma, beatitude, -what?
    }
#endif

    public readonly Special colossus;
    public readonly Special drunkard;
    public readonly Special fugitive;
    public readonly Special glass;
    public readonly Special midget;
    public readonly Special noble;
    public readonly Special protagonist;
    public readonly Special psychic;
    public readonly Special quantum;
    public readonly Special scholar;
    public readonly Special skeleton;
    public readonly Special vampire;
    public readonly Special frost;
    public readonly Special flame;
    public readonly Special shock;
    public readonly Special earth;
    public readonly Special water;
    //public readonly Special werejackal;
    //public readonly Special werepanther;
    //public readonly Special wererat;
    //public readonly Special weresnake;
    //public readonly Special werespider;
    //public readonly Special weretiger;
    public readonly Special werewolf;
    //public readonly Special zealot;
  }
}
