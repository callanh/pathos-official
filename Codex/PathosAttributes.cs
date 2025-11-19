using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexAttributes : CodexPage<ManifestAttributes, AttributeEditor, Attribute>
  {
    private CodexAttributes() { }
#if MASTER_CODEX
    internal CodexAttributes(Codex Codex)
      : base(Codex.Manifest.Attributes)
    {
      Attribute AddAttribute(string Code, string Name, string Description, string Good, string Bad)
      {
        return Register.Add(A =>
        {
          A.Code = Code;
          A.Name = Name;
          A.Description = Description;
          A.Good = Good;
          A.Bad = Bad;
          A.GainSonic = Codex.Sonics.gain_ability;
          A.LoseSonic = Codex.Sonics.lose_ability;
        });
      }

      strength = AddAttribute("STR", "strength", "Bulk muscle size and condition affects the damage you inflict as well as carrying capacity.", "strong", "weak");
      dexterity = AddAttribute("DEX", "dexterity", "Agility and hand-eye coordination affects your defence as well as skill with ranged weapons.", "agile", "clumsy");
      constitution = AddAttribute("CON", "constitution", "Core fitness and endurance affects your life potential and recovery rate.", "tough", "fragile");
      intelligence = AddAttribute("INT", "intelligence", "Capacity for logic and self-awareness affects how much you learn from new experiences.", "smart", "stupid");
      wisdom = AddAttribute("WIS", "wisdom", "Common sense and understanding of the world affects your mana potential and recovery rate.", "wise", "foolish");
      charisma = AddAttribute("CHA", "charisma", "Personality, charm and communication skills affects your commercial exchanges.", "attractive", "repulsive");

      // TODO: attributes requiring further abstraction in the engine.
      Register.strength = strength;
      Register.dexterity = dexterity;
      Register.constitution = constitution;
      Register.intelligence = intelligence;
      Register.wisdom = wisdom;
      Register.charisma = charisma;
    }
#endif

    public readonly Attribute strength;
    public readonly Attribute dexterity;
    public readonly Attribute constitution;
    public readonly Attribute intelligence;
    public readonly Attribute wisdom;
    public readonly Attribute charisma;
  }
}
