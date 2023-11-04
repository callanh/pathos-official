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
        P.Description = "Your mere presence aggravates everyone in the dungeon.";
        P.Unwanted = true;
        P.InspireAggression = true;
      });

      appraisal = AddProperty("appraisal", P =>
      {
        P.Description = "You assess the number of charges in an item simply by picking it up.";
        P.HandleAppraising = true;
      });

      beatitude = AddProperty("beatitude", P =>
      {
        P.Description = "You learn the divine status of an item simply by picking it up.";
        P.HandleDivining = true;
      });

      berserking = AddProperty("berserking", P =>
      {
        P.Description = "You feel unstable like you could be consumed with rage at any time.";
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
        P.Description = "This impairment of vision means you can only see things in a very close proximity.";
        P.Unwanted = true;
        P.VisionPrevented = true;
        P.VisionImpairing = true;
        P.PickupInterfering = true;
        P.ComplicateUntrapping = true;
      });

      blinking = AddProperty("blinking", P =>
      {
        P.Description = "Your position is uncertain and with a little effort you can vanish and then reappear.";
      });

      cannibalism = AddProperty("cannibalism", P =>
      {
        P.Description = "The guilt-free practice of eating the flesh and organs of beings from your own race.";
        P.IgnoreAffinity = true;
      });

      clairvoyance = AddProperty("clairvoyance", P =>
      {
        P.Description = "You have an awareness of other entities elsewhere in the dungeon.";
        P.AlertingVision = true;
      });

      clarity = AddProperty("clarity", P =>
      {
        P.Description = "Your mind is protected against deception, trickery and falsehoods.";
        P.SetImmunityProperty(hallucination, confusion, fear);
        P.AmnesiaPrevented = true;
        P.IllusionNegation = true;
      });

      conflict = AddProperty("conflict", P =>
      {
        P.Description = "You cause all around you to devolve into aggression and violence. Even normally peaceful entities will attack each other. Both useful and dangerous this property should be used with caution. You also feel that your nutrition levels are being sapped by this property.";
        P.IncreasedMetabolism = true;
        P.InspireHostility = true;
      });

      confusion = AddProperty("confusion", P =>
      {
        P.Description = "When confused you will struggle to make decisions. You may move or attack in an unintended direction and you cannot cast spells. When reading scrolls you may mispronounce the words and cause different effects than expected. It is impossible to focus for long enough to study a book.";
        P.Unwanted = true;
        P.PickupInterfering = true;
        P.PreventCasting = true;
        P.ComplicateUntrapping = true;
        P.ConcentrationImpairing = true;
        P.StaggerChance = Chance.OneIn5;
      });

      dark_vision = AddProperty("dark vision", P =>
      {
        P.Description = "This special vision allows you to see warm-bodied entities in the dark areas of the dungeon.";
        P.NightVision = new Vision(Anatomies.thermal, Inv.Colour.Red.Opacity(0.50F));
      });

      deafness = AddProperty("deafness", P =>
      {
        P.Description = "This profound hearing impairment has cut you off from the audible world.";
        P.Unwanted = true;
        P.ComplicateUntrapping = true;
        P.HearingPrevented = true;
      });

      deflection = AddProperty("deflection", P =>
      {
        P.Description = "A shimmering magical shield surrounds your body which aids in your defence and has a chance to deflect incoming missiles.";
        P.DefenceModifier = Modifier.Plus2;
        P.DodgeChance = Chance.OneIn2;
      });

      displacement = AddProperty("displacement", P =>
      {
        P.Description = "This causes your image to appear not quite where you are located. For sentient opponents this makes it more difficult for them to strike at you. Magical effects such as beams and strikes have a reduced chance to hit you.";
        P.Incompatible = true;
        P.IllusionBeamDodgeChance = Chance.OneIn2;
        P.IllusionAttackModifier = Rules.DefenderDisplacementPenalty;
      });

      fainting = AddProperty("fainting", P =>
      {
        P.Description = "Fainting from lack of food.";
        P.Unwanted = true;
        P.MotionlessIndicationGlyph = Glyphs.fainting_property;
        P.UnconsciousIndicationText = "oOo";
        P.VisionPrevented = true;
        P.VisionImpairing = true;
      });

      fear = AddProperty("fear", P =>
      {
        P.Description = "Gripped by terror you cannot attack and should flee.";
        P.Unwanted = true;
        P.PickupInterfering = true;
        P.AttackPrevented = true;
      });

      flight = AddProperty("flight", P =>
      {
        P.Description = "Fly above the ground to avoid many types of traps.";
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
        P.Description = "Fumbling causes you to drop your weapon during combat and fall up and down the stairs.";
        P.Unwanted = true;
        P.PickupInterfering = true;
        P.ComplicateUntrapping = true;
        P.AttackFailChance = Chance.OneIn3;
        P.MountFailChance = Chance.ThreeIn4;
        P.TraverseFailChance = Chance.OneIn4;
      });

      hallucination = AddProperty("hallucination", P =>
      {
        P.Description = "Visual perception is distorted as your mind twists ordinary items and entities into absurd and nightmarish forms.";
        P.Unwanted = true;
        P.PickupInterfering = true;
        P.ComplicateUntrapping = true;
        P.DeathPrevented = true;
        P.VisionImpairing = true; // seeing imaginary things.
        P.DistortPerception = true;
      });
      
      hunger = AddProperty("hunger", P =>
      {
        P.Description = "You are consumed by the urge to eat and know that you will never be fully satisfied.";
        P.Unwanted = true;
        P.IncreasedMetabolism = true;
        P.CompromisedMetabolism = true;
        P.TradeMultiplier = 2; // double buy price and half sell price.
        P.AtWillNutritionCostMultiplier = 2; // double nutrition cost for at-will actions.
      });

      inebriation = AddProperty("inebriation", P => // or intoxication?
      {
        P.Description = "In this heady state of intoxication, your wildly increased confidence is only offset by your badly impaired judgement.";
        P.SetImmunityProperty(fear);
        P.Unwanted = true;
        P.ComplicateUntrapping = true;
        P.ConcentrationImpairing = true;
        P.StaggerChance = Chance.OneIn10;
      });

      invisibility = AddProperty("invisibility", P =>
      {
        P.Description = "This is the state of being unseen. You move with a slight distortion of light that is only noticeable when you are nearby.";
        P.Incompatible = true;
        P.IncreasedMetabolism = true;
        P.UnseenNegationProperty = see_invisible;
      });

      jumping = AddProperty("jumping", P =>
      {
        P.Description = "Your legs feel like strong coils ready to spring into action and over great distances.";
        P.Incompatible = true;
      });

      levitation = AddProperty("levitation", P =>
      {
        P.Description = "This is in defiance to gravity and is useful to avoid many types of traps. If only you could reach the floor at the same time.";
        P.Incompatible = true;
        P.AirborneLifting = true;
      });

      life_regeneration = AddProperty("life regeneration", P =>
      {
        P.Description = "This is the renewal of your body through rapid healing and regrowth. Your metabolism is greatly increased and you require more food than normal.";
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
        P.Description = "Recover mana at an increased rate.";
        P.IncreasedMetabolism = true;
        P.ManaRecoveryChance = Chance.Always;
      });

      narcolepsy = AddProperty("narcolepsy", P =>
      {
        P.Description = "Suddenly you will fall asleep until you wake or are woken.";
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
        P.Description = "Your muscles are seized in position leaving you motionless and defenceless.";
        P.Unwanted = true;
        P.MotionlessIndicationGlyph = Glyphs.paralysis_property;
      });

      petrifying = AddProperty("petrifying", P =>
      {
        P.Description = "Your body becomes stiff and rigid almost like it is turning to stone. All movement is difficult and you feel brittle like the next blow will shatter you into a thousand pieces.";
        P.ParityElement = Elements.petrify;
        P.Unwanted = true;
        P.SpeedMultiplier = 0.25F;
        P.DamageMultiplier = 2.0F;
        P.DefenceModifier = Modifier.FromRank(-8);
      });

      phasing = AddProperty("phasing", P =>
      {
        P.Description = "Your body is halfway between planes and this means you can pass effortlessly through walls, doors and boulders.";
        P.Incompatible = true;
        P.IncreasedMetabolism = true;
        P.Passthrough = true;
      });

      polymorph = AddProperty("polymorph", P =>
      {
        P.Description =
          "Transform into another entity with all the advantages or disadvantages that brings. " +
          "For example, entities may not have hands but can fly and breathe deadly beams of energy. " +
          "When you perish in your polymorphed form you will often revert to your original form.";
        P.PolymorphChance = Chance.OneIn85;
      });

      polymorph_control = AddProperty("polymorph control", P =>
      {
        P.Description =
        "You can transform into the entity of your choice when you have polymorph control. " +
        "This control is limited to entities that you have already killed. " +
        "Forcing a transformation comes at a cost to your nutrition level.";
        P.CorporealShifting = true;
      });

      quickness = AddProperty("quickness", P =>
      {
        P.Description = "This enhancement to your speed allows you to move and attack more often.";
        P.SpeedMultiplier = 1.5F;
      });

      rage = AddProperty("rage", P =>
      {
        P.Description = "Deranged with anger you lash out at friend and foe alike without thought or reason.";
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
        P.Description = "Your perception and awareness are increased so you can find secret doors and traps without any effort.";
        P.SearchChance = Chance.Always;
      });

      see_invisible = AddProperty("see invisible", P =>
      {
        P.Description = "Enhances your vision so you can see otherwise invisible creatures.";
      });

      sickness = AddProperty("sickness", P =>
      {
        P.Description = "You are afflicted with a terrible sickness with strange symptoms and have to quell the urge to vomit.";
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
        P.Description = "You move in eerie silence and cannot utter even a single syllable out loud.";
        P.Unwanted = true;
        P.VoiceImpairing = true;
        P.PreventCasting = true;
      });

      sleeping = AddProperty("sleeping", P =>
      {
        P.Description = "You are asleep, resting peacefully. In a dungeon full of monsters.";
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
        P.Description = "Coated in a slick and oily resin, you simply slide out of grasp and maw.";
        P.AvoidGrappling = true;
        P.AvoidCapturing = true;
        P.AvoidEngulfing = true;
      });

      slow_digestion = AddProperty("slow digestion", P =>
      {
        P.Description = "Your metabolism is slowed and you require a lot less nutrition.";
        P.DecreasedMetabolism = true;
      });

      slowness = AddProperty("slowness", P =>
      {
        P.Description = "This reduction to your speed causes you to move and attack less often.";
        P.Unwanted = true;
        P.SpeedMultiplier = 0.5F;
      });

      stealth = AddProperty("stealth", P =>
      {
        P.Description = "Moving quietly through the dungeon you can sneak up on sleeping monsters.";
        P.Incompatible = true;
        P.MovementQuieting = true;
      });

      stunned = AddProperty("stunned", P =>
      {
        P.Description = "When stunned you are unable to walk in a straight line or perform any higher level tasks, much like when confused.";
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
        P.Description = "Your ability scores are fixed so they cannot degrade but also cannot improve.";
        P.FreezeAbilities = true;
      });

      telekinesis = AddProperty("telekinesis", P =>
      {
        P.Description = "Using the power of your mind to move objects without physical interaction.";
      });

      telepathy = AddProperty("telepathy", P =>
      {
        P.Description = "This allows you to detect the presence and location of minds but only when you cannot see.";
        P.BlindVision = new Vision(Anatomies.mind, Inv.Colour.Blue.Opacity(0.50F));
      });

      teleportation = AddProperty("teleportation", P =>
      {
        P.Description = "Teleportation transports your character around the immediate dungeon level. When uncontrolled, this is a random event that adds a degree of unpredictability to your journey. Controlled teleportation allows you to select on the map your destination location.";
        P.TeleportChance = Chance.OneIn85;
      });

      teleport_control = AddProperty("teleport control", P =>
      {
        P.Description = "Control when and where you teleport.";
        P.SpatialAnchoring = true;
      });

      tunnelling = AddProperty("tunnelling", P =>
      {
        P.Description = "You are well equipped to dig tunnels and make your own path through the dungeon.";
      });

      unchanging = AddProperty("unchanging", P =>
      {
        P.Description = "Prevents you being polymorphed but also prevents you turning back into your natural form.";
        P.SetImmunityProperty(polymorph);
        P.CorporealStabilising = true;
      });

      vitality = AddProperty("vitality", P =>
      {
        P.Description = "You are resilient and unaffected by any type of natural or magical sickness.";
        P.SetImmunityProperty(sickness);
      });

      warning = AddProperty("warning", P =>
      {
        P.Description = "Gives you advance warning of nearby monsters and their approximate threat level.";
        P.WarningVision = true;
      });

#if DEBUG
      CodexRecruiter.Enrol(() =>
      {
        foreach (var Item in Register.List)
        {
          if (string.IsNullOrWhiteSpace(Item.Description) || !Item.Description.EndsWith("."))
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