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
      foreach (var Slot in List)
      {
        var Editor = Register.Edit(Slot);

        Editor.Glyph = Codex.Glyphs.GetGlyphOrNull("equip " + Editor.Name);
        Debug.Assert(Editor.Glyph != null, $"Slot not found: {Editor.Name}");
      }
    }
#endif

    public Slot both_hands => Register.both_hands;
    public Slot main_hand => Register.main_hand;
    public Slot offhand => Register.offhand;
    public Slot quiver => Register.quiver;
    public Slot light => Register.light;
    public Slot helmet => Register.helmet;
    public Slot eyewear => Register.eyewear;
    public Slot earwear => Register.earwear;
    public Slot amulet => Register.amulet;
    public Slot cloak => Register.cloak;
    public Slot shirt => Register.shirt;
    public Slot suit => Register.suit;
    public Slot barding => Register.barding;
    public Slot gloves => Register.gloves;
    public Slot keys => Register.keys;
    public Slot left_ring => Register.left_ring;
    public Slot right_ring => Register.right_ring;
    public Slot boots => Register.boots;
    public Slot purse => Register.purse;
  }
}