using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexSlots : CodexPage<ManifestSlots, SlotEditor, Slot>
  {
    private CodexSlots() { }
#if MASTER_CODEX
    internal CodexSlots(Codex Codex)
      : base(Codex.Manifest.Slots)
    {
      var Anatomies = Codex.Anatomies;

      Slot AddSlot(string Name, Action<SlotEditor> Action)
      {
        return Register.Add(S =>
        {
          S.Name = Name;
          S.Glyph = Codex.Glyphs.GetGlyphOrNull("equip " + S.Name);
          Debug.Assert(S.Glyph != null, $"Slot not found: {S.Name}");

          Action(S);
        });
      }

      both_hands = AddSlot("both hands", S =>
      {
        S.Held = true;
        S.RequiresAnatomy(Anatomies.hands);
      });

      main_hand = AddSlot("main hand", S =>
      {
        S.Held = true;
        S.RequiresAnatomy(Anatomies.hands);
      });

      offhand = AddSlot("offhand", S =>
      {
        S.Held = true;
        S.RequiresAnatomy(Anatomies.hands);
      });

      quiver = AddSlot("quiver", S =>
      {
        S.Autostack = true;
        S.Held = true;
        S.RequiresAnatomy(Anatomies.limbs, Anatomies.hands);
      });

      light = AddSlot("light", S =>
      {
        S.Held = true;
        S.RequiresAnatomy(Anatomies.hands);
      });

      gloves = AddSlot("gloves", S =>
      {
        S.Held = true;
        S.RequiresAnatomy(Anatomies.hands);
      });

      helmet = AddSlot("helmet", S =>
      {
        S.RequiresAnatomy(Anatomies.head, Anatomies.hands); // hands means they are likely to be humanoid.
        S.PreventsAnatomy(Anatomies.horns);
      });

      eyewear = AddSlot("eyewear", S =>
      {
        S.RequiresAnatomy(Anatomies.eyes);
      });

      earwear = AddSlot("earwear", S =>
      {
        S.RequiresAnatomy(Anatomies.ears);
      });

      amulet = AddSlot("amulet", S =>
      {
        S.RequiresAnatomy(Anatomies.head);
      });

      cloak = AddSlot("cloak", S =>
      {
        S.RequiresAnatomy(Anatomies.limbs, Anatomies.hands);
      });

      shirt = AddSlot("shirt", S =>
      {
        S.RequiresAnatomy(Anatomies.limbs, Anatomies.hands);
      });

      suit = AddSlot("suit", S =>
      {
        S.RequiresAnatomy(Anatomies.limbs, Anatomies.hands);
      });

      barding = AddSlot("barding", S =>
      {
        S.RequiresAnatomy(Anatomies.mounted);
      });

      keys = AddSlot("keys", S =>
      {
        S.Autostack = true;
        S.RequiresAnatomy(Anatomies.hands);
      });

      left_ring = AddSlot("left ring", S =>
      {
        S.RequiresAnatomy(Anatomies.hands);
      });

      right_ring = AddSlot("right ring", S =>
      {
        S.RequiresAnatomy(Anatomies.hands);
      });

      boots = AddSlot("boots", S =>
      {
        S.RequiresAnatomy(Anatomies.feet, Anatomies.hands); // hands means they are likely to be humanoid.
      });

      purse = AddSlot("purse", S =>
      {
        S.Autostack = true;
        S.RequiresAnatomy(Anatomies.hands);
      });

      Register.ItemTypeSlotArray = new Inv.EnumArray<ItemType, Slot>()
      {
        { ItemType.Amulet, amulet },
        { ItemType.Barding, barding },
        { ItemType.Coin, purse },
        { ItemType.Eyewear, eyewear },
        { ItemType.Earwear, earwear },
        { ItemType.Boots, boots },
        { ItemType.Cloak, cloak },
        { ItemType.Helmet, helmet },
        { ItemType.Gloves, gloves },
        { ItemType.Shirt, shirt },
        { ItemType.Suit, suit },
        { ItemType.Lockpick, keys },
        { ItemType.SkeletonKey, keys },
        { ItemType.SpecificKey, keys },
        { ItemType.Light, light },
        { ItemType.Shield, offhand },
        { ItemType.RangedMissile, quiver },
        { ItemType.ThrownWeapon, quiver },
        { ItemType.Gem, quiver },
        { ItemType.Rock, quiver },
      };

      Register.VanitySlotRowArray =
      [
        new SlotRow(eyewear, helmet, earwear),
        new SlotRow(cloak, shirt, amulet),
        new SlotRow(quiver, suit, light),
        new SlotRow(main_hand, barding, left_ring),
        new SlotRow(offhand, gloves, right_ring),
        new SlotRow(purse, boots, keys),
      ];

      // TODO: slots requiring further abstraction in the engine.
      Register.both_hands = both_hands;
      Register.main_hand = main_hand;
      Register.offhand = offhand;
      Register.quiver = quiver;
      Register.gloves = gloves;
      Register.keys = keys;
      Register.left_ring = left_ring;
      Register.right_ring = right_ring;
      Register.purse = purse;
    }
#endif

    public readonly Slot both_hands;
    public readonly Slot main_hand;
    public readonly Slot offhand;
    public readonly Slot quiver;
    public readonly Slot light;
    public readonly Slot helmet;
    public readonly Slot eyewear;
    public readonly Slot earwear;
    public readonly Slot amulet;
    public readonly Slot cloak;
    public readonly Slot shirt;
    public readonly Slot suit;
    public readonly Slot barding;
    public readonly Slot gloves;
    public readonly Slot keys;
    public readonly Slot left_ring;
    public readonly Slot right_ring;
    public readonly Slot boots;
    public readonly Slot purse;
  }
}