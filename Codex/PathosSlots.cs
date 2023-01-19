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
      Slot AddSlot(string Name, Action<SlotEditor> Action)
      {
        return Add(S =>
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
        S.RequiresHands = true;
      });

      main_hand = AddSlot("main hand", S =>
      {
        S.Held = true;
        S.RequiresHands = true;
      });

      offhand = AddSlot("offhand", S =>
      {
        S.Held = true;
        S.RequiresHands = true;
      });

      quiver = AddSlot("quiver", S =>
      {
        S.Autostack = true;
        S.Held = true;
        S.RequiresLimbs = true;
        S.RequiresHands = true;
      });

      light = AddSlot("light", S =>
      {
        S.Held = true;
        S.RequiresHands = true;
      });

      gloves = AddSlot("gloves", S =>
      {
        S.Held = true;
        S.RequiresHands = true;
      });

      helmet = AddSlot("helmet", S =>
      {
        S.RequiresHead = true;
        S.RequiresHands = true;
      });

      eyewear = AddSlot("eyewear", S =>
      {
        S.RequiresHands = true;
      });

      earwear = AddSlot("earwear", S =>
      {
        S.RequiresHands = true;
      });

      amulet = AddSlot("amulet", S =>
      {
        S.RequiresHead = true;
      });

      cloak = AddSlot("cloak", S =>
      {
        S.RequiresLimbs = true;
        S.RequiresHands = true;
      });

      shirt = AddSlot("shirt", S =>
      {
        S.RequiresLimbs = true;
        S.RequiresHands = true;
      });

      suit = AddSlot("suit", S =>
      {
        S.RequiresLimbs = true;
        S.RequiresHands = true;
      });

      barding = AddSlot("barding", S =>
      {
        S.RequiresMountable = true;
      });

      keys = AddSlot("keys", S =>
      {
        S.Autostack = true;
        S.RequiresHands = true;
      });

      left_ring = AddSlot("left ring", S =>
      {
        S.RequiresHands = true;
      });

      right_ring = AddSlot("right ring", S =>
      {
        S.RequiresHands = true;
      });

      boots = AddSlot("boots", S =>
      {
        S.RequiresHands = true;
        S.RequiresFeet = true;
      });

      purse = AddSlot("purse", S =>
      {
        S.Autostack = true;
        S.RequiresHands = true;
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

      // TODO: Vanity Slot Grid declaration.

      // TODO: slots requiring further abstraction in the engine.
      Register.both_hands = both_hands;
      Register.main_hand = main_hand;
      Register.offhand = offhand;
      Register.quiver = quiver;
      Register.light = light;
      Register.helmet = helmet;
      Register.eyewear = eyewear;
      Register.earwear = earwear;
      Register.amulet = amulet;
      Register.cloak = cloak;
      Register.shirt = shirt;
      Register.suit = suit;
      Register.barding = barding;
      Register.gloves = gloves;
      Register.keys = keys;
      Register.left_ring = left_ring;
      Register.right_ring = right_ring;
      Register.boots = boots;
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