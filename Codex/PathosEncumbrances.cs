using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexEncumbrances : CodexPage<ManifestEncumbrances, EncumbranceEditor, Encumbrance>
  {
    private CodexEncumbrances() { }
#if MASTER_CODEX
    internal CodexEncumbrances(Codex Codex)
      : base(Codex.Manifest.Encumbrances)
    {
      Encumbrance AddEncumbrance(string Name, string Description, Inv.Colour Colour, bool NutritionConsumption, bool Offbalanced)
      {
        return Register.Add(E =>
        {
          E.Name = Name;
          E.Description = Description;
          E.Colour = Colour;
          E.NutritionConsumption = NutritionConsumption;
          E.Offbalanced = Offbalanced;
        });
      }

      Unencumbered = AddEncumbrance("unencumbered", "Your movements are unencumbered.", Inv.Colour.DarkGreen, NutritionConsumption: false, Offbalanced: false);
      Burdened = AddEncumbrance("burdened", "Your movements are slightly slowed because of your load.", Inv.Colour.DarkYellow, NutritionConsumption: true, Offbalanced: false);
      Stressed = AddEncumbrance("stressed", "You rebalance your load but movement is difficult.", Inv.Colour.DarkOrange, NutritionConsumption: true, Offbalanced: true);
      Strained = AddEncumbrance("strained", "You stagger under your load.", Inv.Colour.DarkRed, NutritionConsumption: true, Offbalanced: true);
      Overtaxed = AddEncumbrance("overtaxed", "You can barely move with this load.", Inv.Colour.DarkRed, NutritionConsumption: true, Offbalanced: true);
      Overloaded = AddEncumbrance("overloaded", "You collapse under your load.", Inv.Colour.DarkRed, NutritionConsumption: true, Offbalanced: true);
    }
#endif

    public readonly Encumbrance Unencumbered;
    public readonly Encumbrance Burdened;
    public readonly Encumbrance Stressed;
    public readonly Encumbrance Strained;
    public readonly Encumbrance Overtaxed;
    public readonly Encumbrance Overloaded;
  }
}