using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexProperties : CodexPage<ManifestProperties, PropertyEditor, Property>
  {
    private CodexProperties() { }
#if MASTER_CODEX
    internal CodexProperties(Codex Codex)
      : base(Codex.Manifest.Properties)
    {
      var Glyphs = Codex.Glyphs;
      var Elements = Codex.Elements;
      var Anatomies = Codex.Anatomies;

      Property AddProperty(string Name, Action<PropertyEditor> EditorAction)
      {
        return Register.Add(P =>
        {
          P.Name = Name;
          P.Glyph = Codex.Glyphs.GetGlyphOrNull(P.Name + " property");
          Debug.Assert(P.Glyph != null, P.Name + " property must have a glyph.");

          CodexRecruiter.Enrol(() => EditorAction(P));
        });
      }

      aggravation = AddProperty("aggravation", P =>
      {
        P.Description = "Irritating disposition that provokes hostility and attracts unwanted attention.";
        P.Unwanted = true;
        P.InspireAggression = true;
      });

      appraisal = AddProperty("appraisal", P =>
      {
        P.Description = "Assess the number of charges in an item when you pick it up.";
        P.HandleAppraising = true;
      });

      beatitude = AddProperty("beatitude", P =>
      {
        P.Description = "Sense the divine status of an item simply by picking it up.";
        P.HandleDivining = true;
      });

      berserking = AddProperty("berserking", P =>
      {
        P.Description = "Unstable temperament leaves you perpetually on the brink of uncontrollable rage.";
        P.Unwanted = true;
        P.SetSymptom(Chance.OneIn85, S =>
        {
          S.UnlessTargetHasProperty(rage, U =>
          {
            U.ApplyTransient(rage, 1.d20() + 5);
          });
        });
      });

      blindness = AddProperty("blindness", P =>
      {
        P.Description = "Impairment of vision means you can only see things in a very close proximity.";
        P.Unwanted = true;
        P.VisionPrevented = true;
        P.VisionImpairing = true;
        P.PickupInterfering = true;
        P.ComplicateUntrapping = true;
      });

      blinking = AddProperty("blinking", P =>
      {
        P.Description = "Vanish and reappear a short distance away in the blink of an eye.";
      });

      cannibalism = AddProperty("cannibalism", P =>
      {
        P.Description = "Guilt-free consumption of flesh and organs from beings of your own kind.";
        P.IgnoreAffinity = true;
      });

      clairvoyance = AddProperty("clairvoyance", P =>
      {
        P.Description = "Uncanny awareness of other entities beyond your immediate vicinity.";
        P.AlertingVision = true;
      });

      clarity = AddProperty("clarity", P =>
      {
        P.Description = "Protection against deception, trickery and falsehoods that cloud the mind.";
        P.SetImmunityProperty(hallucination, confusion, fear);
        P.AmnesiaPrevented = true;
        P.IllusionNegation = true;
      });

      conflict = AddProperty("conflict", P =>
      {
        P.Description = "Incites aggression and violence in those around you, driving even normally peaceful entities to attack one another. Useful but dangerous, it demands caution and comes at the cost of increased nutritional needs.";
        P.IncreasedMetabolism = true;
        P.InspireHostility = true;
      });

      confusion = AddProperty("confusion", P =>
      {
        P.Description = "Serious impairment of judgement and concentration. You may move or attack in unintended directions and cannot cast spells. Reading scrolls risks mispronouncing their words and producing unexpected effects, while studying books becomes impossible.";
        P.Unwanted = true;
        P.PickupInterfering = true;
        P.PreventCasting = true;
        P.ComplicateUntrapping = true;
        P.ConcentrationImpairing = true;
        P.StaggerChance = Chance.OneIn5;
      });

      dark_vision = AddProperty("dark vision", P =>
      {
        P.Description = "Special vision that allows you to see warm-bodied entities in the darkness.";
        P.NightVision = new Vision(Anatomies.thermal, Inv.Colour.Red.Opacity(0.50F));
      });

      deafness = AddProperty("deafness", P =>
      {
        P.Description = "Profound hearing impairment that cuts you off from the audible world.";
        P.Unwanted = true;
        P.ComplicateUntrapping = true;
        P.HearingPrevented = true;
      });

      deflection = AddProperty("deflection", P =>
      {
        P.Description = "Shimmering magical shield surrounds your body, bolstering your defence and sometimes deflecting incoming missiles.";
        P.DefenceModifier = Modifier.Plus2;
        P.DodgeChance = Chance.OneIn2;
      });

      displacement = AddProperty("displacement", P =>
      {
        P.Description = "Visual distortion obscures your true position, making it harder for opponents to land physical and magical attacks.";
        P.Incompatible = true;
        P.IllusionBeamDodgeChance = Chance.OneIn2;
        P.IllusionAttackModifier = Rules.DefenderDisplacementPenalty;
      });

      fainting = AddProperty("fainting", P =>
      {
        P.Description = "Loss of consciousness usually caused by lack of food.";
        P.Unwanted = true;
        P.MotionlessIndicationGlyph = Glyphs.fainting_property;
        P.UnconsciousIndicationText = "oOo";
        P.VisionPrevented = true;
        P.VisionImpairing = true;
      });

      fear = AddProperty("fear", P =>
      {
        P.Description = "Gripped by terror, you cannot attack and are overcome by the urge to flee.";
        P.Unwanted = true;
        P.PickupInterfering = true;
        P.AttackPrevented = true;
      });

      flight = AddProperty("flight", P =>
      {
        P.Description = "Fly above the ground, avoiding many types of traps.";
        P.Incompatible = true;
        P.AirborneSteering = true;
      });

      free_action = AddProperty("free action", P =>
      {
        P.Description = "Protects you from effects that slow, paralyse or otherwise restrain your movement.";
        P.SetImmunityProperty(paralysis, stunned, slowness);
        P.IncarcerationPrevented = true;
      });

      fumbling = AddProperty("fumbling", P =>
      {
        P.Description = "Unfortunate tendency to drop your weapon during combat and stumble on the stairs.";
        P.Unwanted = true;
        P.PickupInterfering = true;
        P.ComplicateUntrapping = true;
        P.AttackFailChance = Chance.OneIn3;
        P.MountFailChance = Chance.ThreeIn4;
        P.TraverseFailChance = Chance.OneIn4;
      });

      hallucination = AddProperty("hallucination", P =>
      {
        P.Description = "Distorted senses transform ordinary sights and sounds into absurd and nightmarish experiences.";
        P.Unwanted = true;
        P.PickupInterfering = true;
        P.ComplicateUntrapping = true;
        P.DeathPrevented = true;
        P.VisionImpairing = true; // seeing imaginary things.
        P.DistortPerception = true;
      });
      
      hunger = AddProperty("hunger", P =>
      {
        P.Description = "Consumed by the urge to eat, yet never leaves you fully satisfied.";
        P.Unwanted = true;
        P.IncreasedMetabolism = true;
        P.CompromisedMetabolism = true;
        P.TradeMultiplier = 2; // double buy price and half sell price.
        P.AtWillNutritionCostMultiplier = 2; // double nutrition cost for at-will actions.
      });

      inebriation = AddProperty("inebriation", P => // or intoxication?
      {
        P.Description = "State of intoxication that brings wildly increased confidence, offset by badly impaired judgement.";
        P.SetImmunityProperty(fear);
        P.Unwanted = true;
        P.ComplicateUntrapping = true;
        P.ConcentrationImpairing = true;
        P.StaggerChance = Chance.OneIn10;
      });

      invisibility = AddProperty("invisibility", P =>
      {
        P.Description = "Unseen by most, with only a faint distortion of light betraying your presence at close range.";
        P.Incompatible = true;
        P.IncreasedMetabolism = true;
        P.UnseenNegationProperty = see_invisible;
      });

      jumping = AddProperty("jumping", P =>
      {
        P.Description = "Powerful, spring-like legs allow you to leap great distances.";
        P.Incompatible = true;
      });

      levitation = AddProperty("levitation", P =>
      {
        P.Description = "Floating above the ground allows you avoid many types of traps. If only you could reach the floor at the same time.";
        P.Incompatible = true;
        P.AirborneLifting = true;
      });

      life_regeneration = AddProperty("life regeneration", P =>
      {
        P.Description = "Rapid healing and regrowth renew your body but a greatly increased metabolism leaves you hungrier than normal.";
        P.IncreasedMetabolism = true;
        P.LifeRecoveryChance = Chance.Always;
      });

      lifesaving = AddProperty("lifesaving", P =>
      {
        P.Description = "Revives you to half-health when you die at the cost of one point of constitution.";
        P.Revival = true;
      });

      mana_regeneration = AddProperty("mana regeneration", P =>
      {
        P.Description = "Rapid mana recovery restores your magical reserves at an increased rate but a greatly increased metabolism leaves you hungrier than normal.";
        P.IncreasedMetabolism = true;
        P.ManaRecoveryChance = Chance.Always;
      });

      narcolepsy = AddProperty("narcolepsy", P =>
      {
        P.Description = "Sudden bouts of sleep that leave you unconscious until you wake naturally or are roused.";
        P.Unwanted = true;
        P.SetSymptom(Chance.OneIn85, S =>
        {
          S.UnlessTargetHasProperty(sleeping, S =>
          {
            // slowness will be applied instead if sleep is resisted.

            S.WhenTargetResistant(Elements.sleep, T =>
            {
              T.IntentionalTransient(slowness, 1.d20() + 5);
            },
            E =>
            {
              E.IntentionalTransient(sleeping, 1.d20() + 5);
            });            
          });
        });
      });

      paralysis = AddProperty("paralysis", P =>
      {
        P.Description = "Muscles seized an unresponsive, leaving you motionless and defenceless.";
        P.Unwanted = true;
        P.MotionlessIndicationGlyph = Glyphs.paralysis_property;
      });

      petrifying = AddProperty("petrifying", P =>
      {
        P.Description = "Growing stiff and rigid, almost like turning to stone. Movement becomes difficult and you feel brittle, as though the next blow could shatter you into a thousand pieces.";
        P.ParityElement = Elements.petrify;
        P.Unwanted = true;
        P.SpeedMultiplier = 0.25F;
        P.DamageMultiplier = 2.0F;
        P.DefenceModifier = Modifier.FromRank(-8);
      });

      phasing = AddProperty("phasing", P =>
      {
        P.Description = "Partially shifted between planes, allowing you to pass effortlessly through walls, doors and boulders.";
        P.Incompatible = true;
        P.IncreasedMetabolism = true;
        P.Passthrough = true;
      });

      polymorph = AddProperty("polymorph", P =>
      {
        P.Description = "Transform into another entity, gaining its advantages and disadvantages. If you perish while polymorphed, you will revert to your original form.";
        P.PolymorphChance = Chance.OneIn85;
      });

      polymorph_control = AddProperty("polymorph control", P =>
      {
        P.Description = "Transform into an entity of your choice, limited to those you have already killed. Forcing a transformation comes at the cost of increased nutritional needs.";
        P.CorporealShifting = true;
      });

      quickness = AddProperty("quickness", P =>
      {
        P.Description = "Increased speed allows you to move and attack more frequently.";
        P.SpeedMultiplier = 1.5F;
      });

      rage = AddProperty("rage", P =>
      {
        P.Description = "Deranged with anger, you lash out at friend and foe alike without thought or reason.";
        P.SetImmunityProperty(fear);
        P.Unwanted = true;
        P.VisionImpairing = true; // blinded by rage.
        P.UncontrolledFury = true;
      });

      reflection = AddProperty("reflection", P =>
      {
        P.Description = "Gaze and beam attacks are reflected back at the caster.";
        P.Reflective = true;
      });

      searching = AddProperty("searching", P =>
      {
        P.Description = "Heightened perception and awareness reveal secret doors and traps with ease.";
        P.SearchChance = Chance.Always;
      });

      see_invisible = AddProperty("see invisible", P =>
      {
        P.Description = "Enhances your vision so you can see otherwise invisible creatures.";
      });

      sickness = AddProperty("sickness", P =>
      {
        P.Description = "Affected by a terrible illness that brings strange symptoms and an overwhelming urge to vomit.";
        P.Unwanted = true;
        P.SetSymptom(Chance.OneIn10, S =>
        {
          S.WhenSourceNotHasProperty(fainting,
            T => T.WhenProbability(Table =>
            {
              Table.Add(15, P => P.Vomit(5.d10()));
              Table.Add(15, P => P.ApplyTransient(fainting, 2.d5()));
              Table.Add(15, P => P.ApplyTransient(confusion, 3.d5()));
              Table.Add(15, P => P.ApplyTransient(fumbling, 3.d10()));
              Table.Add(15, P => P.ApplyTransient(blindness, 3.d5()));
              Table.Add(15, P => P.ApplyTransient(deafness, 3.d10()));
              Table.Add(10, P => P.ApplyTransient(hallucination, 3.d5()));
            })
          );
        });
      });

      silence = AddProperty("silence", P =>
      {
        P.Description = "Moving in eerie silence, unable to utter even a single syllable out loud.";
        P.Unwanted = true;
        P.VoiceImpairing = true;
        P.PreventCasting = true;
      });

      sleeping = AddProperty("sleeping", P =>
      {
        P.Description = "Asleep, resting peacefully. Possibly surrounded by monsters.";
        P.ParityElement = Elements.sleep;
        P.Unwanted = true;
        P.MotionlessIndicationGlyph = Glyphs.sleeping_property;
        P.UnconsciousIndicationText = "zZz";
        P.VisionPrevented = true;
        P.VisionImpairing = true;
        P.DecreasedMetabolism = true;
        P.LifeRecoveryDoubling = true;
        P.ManaRecoveryDoubling = true;
      });

      slippery = AddProperty("slippery", P =>
      {
        P.Description = "Coated in a slick, oily resin that lets you slip effortlessly from grasping hands and snapping jaws.";
        P.AvoidGrappling = true;
        P.AvoidCapturing = true;
        P.AvoidEngulfing = true;
      });

      slow_digestion = AddProperty("slow digestion", P =>
      {
        P.Description = "Slowed metabolism reduces your need for food and sustenance.";
        P.DecreasedMetabolism = true;
      });

      slowness = AddProperty("slowness", P =>
      {
        P.Description = "Reduced speed causes you to move and attack less frequently.";
        P.Unwanted = true;
        P.SpeedMultiplier = 0.5F;
      });

      stealth = AddProperty("stealth", P =>
      {
        P.Description = "Moving quietly, you can sneak up on sleeping monsters.";
        P.Incompatible = true;
        P.MovementQuieting = true;
      });

      stunned = AddProperty("stunned", P =>
      {
        P.Description = "Severe disorientation leaves you staggering unpredictably and unable to perform even simple tasks.";
        P.Unwanted = true;
        P.PickupInterfering = true;
        P.PreventCasting = true;
        P.ComplicateUntrapping = true;
        P.VisionImpairing = true; // can't see straight.
        P.ConcentrationImpairing = true;
        P.StaggerChance = Chance.Always;
      });

      sustain_ability = AddProperty("sustain ability", P =>
      {
        P.Description = "Ability scores are locked in place, preventing both deterioration and improvement.";
        P.FreezeAbilities = true;
      });

      telekinesis = AddProperty("telekinesis", P =>
      {
        P.Description = "Move objects without physical contact through the power of your mind.";
      });

      telepathy = AddProperty("telepathy", P =>
      {
        P.Description = "Sense the presence and location of other minds but only when they are beyond your sight.";
        P.BlindVision = new Vision(Anatomies.mind, Inv.Colour.Blue.Opacity(0.50F));
      });

      teleportation = AddProperty("teleportation", P =>
      {
        P.Description = "Transports you to another location on the current level. Uncontrolled teleportation takes you to a random destination, while controlled teleportation lets you choose your destination on the map.";
        P.TeleportChance = Chance.OneIn85;
      });

      teleport_control = AddProperty("teleport control", P =>
      {
        P.Description = "Choose when and where you teleport but only when teleportation occurs.";
        P.SpatialAnchoring = true;
      });

      tunnelling = AddProperty("tunnelling", P =>
      {
        P.Description = "Well equipped to dig tunnels and carve your own path.";
      });

      unchanging = AddProperty("unchanging", P =>
      {
        P.Description = "Prevents polymorphing or reverting to your natural form, locking you in your current state.";
        P.SetImmunityProperty(polymorph);
        P.CorporealStabilising = true;
      });

      vitality = AddProperty("vitality", P =>
      {
        P.Description = "Resilient to all forms of natural and magical illness.";
        P.SetImmunityProperty(sickness);
      });

      warning = AddProperty("warning", P =>
      {
        P.Description = "Alerts you to nearby monsters beyond your field of vision and indicates their threat level.";
        P.WarningVision = true;
      });

#if DEBUG
      CodexRecruiter.Enrol(() =>
      {
        foreach (var Item in Register.List)
        {
          if (string.IsNullOrWhiteSpace(Item.Description) || !Item.Description.EndsWith(".", StringComparison.InvariantCultureIgnoreCase))
            throw new Exception(Item.Name + " must have a description that ends with a full stop.");
        }
      });
#endif

      Register.Alias(vitality, "sickness resistance");
      Register.Alias(deflection, "protection");
    }
#endif

    // NOTE: 64 is the max without changing data structures in PropertySet.
    public readonly Property aggravation;
    public readonly Property appraisal;
    public readonly Property beatitude;
    public readonly Property berserking;
    public readonly Property blindness;
    public readonly Property blinking;
    public readonly Property cannibalism;
    public readonly Property clairvoyance;
    public readonly Property clarity;
    public readonly Property conflict;
    public readonly Property confusion;
    public readonly Property dark_vision;
    public readonly Property deafness;
    public readonly Property deflection;
    public readonly Property displacement;
    public readonly Property fainting;
    public readonly Property fear;
    public readonly Property flight;
    public readonly Property free_action;
    public readonly Property fumbling;
    public readonly Property hallucination;
    public readonly Property hunger;
    public readonly Property inebriation;
    public readonly Property invisibility;
    public readonly Property jumping;
    public readonly Property levitation;
    public readonly Property life_regeneration;
    public readonly Property lifesaving;
    public readonly Property mana_regeneration;
    public readonly Property narcolepsy;
    public readonly Property paralysis;
    public readonly Property petrifying;
    public readonly Property phasing;
    public readonly Property polymorph;
    public readonly Property polymorph_control;
    public readonly Property quickness;
    public readonly Property rage;
    public readonly Property reflection;
    public readonly Property searching;
    public readonly Property see_invisible;
    public readonly Property sickness;
    public readonly Property vitality;
    public readonly Property silence;
    public readonly Property slippery;
    public readonly Property sleeping;
    public readonly Property slowness;
    public readonly Property slow_digestion;
    public readonly Property stealth;
    public readonly Property stunned;
    public readonly Property sustain_ability;
    public readonly Property telekinesis;
    public readonly Property telepathy;
    public readonly Property teleportation;
    public readonly Property teleport_control;
    public readonly Property tunnelling;
    public readonly Property unchanging;
    public readonly Property warning;
    // NOTE: total 56/64 properties.
  }
}
