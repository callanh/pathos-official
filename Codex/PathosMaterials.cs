using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexMaterials : CodexPage<ManifestMaterials, MaterialEditor, Material>
  {
    private CodexMaterials() { }
#if MASTER_CODEX
    internal CodexMaterials(Codex Codex)
      : base(Codex.Manifest.Materials)
    {
      var Elements = Codex.Elements;

      Material AddMaterial(string Name, Element Element, bool Corporeal, params Element[] Weakness)
      {
        return Register.Add(M =>
        {
          M.Name = Name;
          M.Element = Element;
          M.Corporeal = Corporeal;

          if (Weakness.Length > 0)
            M.SetWeakness(Weakness);
        });
      }

      adamantine = AddMaterial("adamantine", Elements.physical, Corporeal: true);
      air = AddMaterial("air", Elements.physical, Corporeal: false);
      animal = AddMaterial("animal", Elements.physical, Corporeal: true);
      bone = AddMaterial("bone", Elements.physical, Corporeal: true);
      clay = AddMaterial("clay", Elements.physical, Corporeal: true, Weakness: Elements.digging);
      cloth = AddMaterial("cloth", Elements.physical, Corporeal: true);
      copper = AddMaterial("copper", Elements.physical, Corporeal: true);
      crystal = AddMaterial("crystal", Elements.physical, Corporeal: true);
      electricity = AddMaterial("electricity", Elements.shock, Corporeal: false);
      ether = AddMaterial("ether", Elements.physical, Corporeal: false);
      fire = AddMaterial("fire", Elements.fire, Corporeal: false);
      fruit = AddMaterial("fruit", Elements.physical, Corporeal: true);
      gemstone = AddMaterial("gemstone", Elements.physical, Corporeal: true);
      glass = AddMaterial("glass", Elements.physical, Corporeal: true);
      gold = AddMaterial("gold", Elements.physical, Corporeal: true);
      hide = AddMaterial("hide", Elements.physical, Corporeal: true);
      ice = AddMaterial("ice", Elements.cold, Corporeal: true);
      iron = AddMaterial("iron", Elements.physical, Corporeal: true);
      lava = AddMaterial("lava", Elements.fire, Corporeal: true);
      leather = AddMaterial("leather", Elements.physical, Corporeal: true);
      mithril = AddMaterial("mithril", Elements.physical, Corporeal: true);
      paper = AddMaterial("paper", Elements.physical, Corporeal: true);
      plastic = AddMaterial("plastic", Elements.physical, Corporeal: true);
      platinum = AddMaterial("platinum", Elements.physical, Corporeal: true);
      sand = AddMaterial("sand", Elements.physical, Corporeal: true, Weakness: Elements.digging);
      silver = AddMaterial("silver", Elements.physical, Corporeal: true);
      stone = AddMaterial("stone", Elements.physical, Corporeal: true, Weakness: Elements.digging);
      //straw = AddMaterial("straw", Elements.physical, Solid: true);
      tin = AddMaterial("tin", Elements.physical, Corporeal: true);
      vegetable = AddMaterial("vegetable", Elements.physical, Corporeal: true);
      water = AddMaterial("water", Elements.water, Corporeal: true);
      wax = AddMaterial("wax", Elements.physical, Corporeal: true);
      wood = AddMaterial("wood", Elements.physical, Corporeal: true);

      this.RustMetalArray =
      [
        iron, // rust is an iron oxide, usually red oxide formed by the redox reaction of iron and oxygen in the presence of water or air moisture
        copper, // copper tarnishes but which comes to the same effect as rusting.
        silver, // silver can rust or tarnish over time because of exposure to moisture or sulfur in the air
        //gold, // gold does not rust.
        //bronze, // bronze does not rust, it is an alloy of tin and copper.
        //tin // tin does not rust, it is an elemental metal and therefore does not contain iron.
      ];

      foreach (var Material in List)
        Register.Alias(Material, Material.Name.ToSentenceCase()); // backwards compatibility fallback for when materials started with a capital letter. eg. "Wood".
    }
#endif

    public readonly Material adamantine;
    public readonly Material air;
    public readonly Material animal;
    public readonly Material bone;
    public readonly Material clay;
    public readonly Material cloth;
    public readonly Material copper;
    public readonly Material crystal;
    public readonly Material electricity;
    public readonly Material ether;
    public readonly Material fire;
    public readonly Material fruit;
    public readonly Material gemstone;
    public readonly Material glass;
    public readonly Material gold;
    public readonly Material hemp;
    public readonly Material hide;
    public readonly Material ice;
    public readonly Material iron;
    public readonly Material lava;
    public readonly Material leather;
    public readonly Material mithril;
    public readonly Material paper;
    public readonly Material plastic;
    public readonly Material platinum;
    public readonly Material sand;
    public readonly Material silver;
    public readonly Material stone;
    //public readonly Material straw; // TODO: need straw items to make this worthwhile.
    public readonly Material tin;
    public readonly Material vegetable;
    public readonly Material water;
    public readonly Material wax;
    public readonly Material wood;
    public IReadOnlyList<Material> RustMetals => RustMetalArray;

    private readonly Material[] RustMetalArray;
  }
}
