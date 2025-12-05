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

      this.adamantine = AddMaterial("adamantine", Elements.physical, Corporeal: true);
      this.air = AddMaterial("air", Elements.physical, Corporeal: false);
      this.animal = AddMaterial("animal", Elements.physical, Corporeal: true);
      this.bone = AddMaterial("bone", Elements.physical, Corporeal: true);
      this.clay = AddMaterial("clay", Elements.physical, Corporeal: true, Weakness: Elements.digging);
      this.cloth = AddMaterial("cloth", Elements.physical, Corporeal: true);
      this.copper = AddMaterial("copper", Elements.physical, Corporeal: true);
      this.crystal = AddMaterial("crystal", Elements.physical, Corporeal: true);
      this.electricity = AddMaterial("electricity", Elements.shock, Corporeal: false);
      this.ether = AddMaterial("ether", Elements.physical, Corporeal: false);
      this.fire = AddMaterial("fire", Elements.fire, Corporeal: false);
      this.fruit = AddMaterial("fruit", Elements.physical, Corporeal: true);
      this.gemstone = AddMaterial("gemstone", Elements.physical, Corporeal: true);
      this.glass = AddMaterial("glass", Elements.physical, Corporeal: true);
      this.gold = AddMaterial("gold", Elements.physical, Corporeal: true);
      this.hide = AddMaterial("hide", Elements.physical, Corporeal: true);
      this.ice = AddMaterial("ice", Elements.cold, Corporeal: true);
      this.iron = AddMaterial("iron", Elements.physical, Corporeal: true);
      this.lava = AddMaterial("lava", Elements.fire, Corporeal: true);
      this.leather = AddMaterial("leather", Elements.physical, Corporeal: true);
      this.mithril = AddMaterial("mithril", Elements.physical, Corporeal: true);
      this.paper = AddMaterial("paper", Elements.physical, Corporeal: true);
      this.plastic = AddMaterial("plastic", Elements.physical, Corporeal: true);
      this.platinum = AddMaterial("platinum", Elements.physical, Corporeal: true);
      this.sand = AddMaterial("sand", Elements.physical, Corporeal: true, Weakness: Elements.digging);
      this.silver = AddMaterial("silver", Elements.physical, Corporeal: true);
      this.stone = AddMaterial("stone", Elements.physical, Corporeal: true, Weakness: Elements.digging);
    //this.straw = AddMaterial("straw", Elements.physical, Solid: true);
      this.tin = AddMaterial("tin", Elements.physical, Corporeal: true);
      this.vegetable = AddMaterial("vegetable", Elements.physical, Corporeal: true);
      this.water = AddMaterial("water", Elements.water, Corporeal: true);
      this.wax = AddMaterial("wax", Elements.physical, Corporeal: true);
      this.wood = AddMaterial("wood", Elements.physical, Corporeal: true);

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

    public Material adamantine { get; }
    public Material air { get; }
    public Material animal { get; }
    public Material bone { get; }
    public Material clay { get; }
    public Material cloth { get; }
    public Material copper { get; }
    public Material crystal { get; }
    public Material electricity { get; }
    public Material ether { get; }
    public Material fire { get; }
    public Material fruit { get; }
    public Material gemstone { get; }
    public Material glass { get; }
    public Material gold { get; }
    public Material hide { get; }
    public Material ice { get; }
    public Material iron { get; }
    public Material lava { get; }
    public Material leather { get; }
    public Material mithril { get; }
    public Material paper { get; }
    public Material plastic { get; }
    public Material platinum { get; }
    public Material sand { get; }
    public Material silver { get; }
    public Material stone { get; }
  //public Material straw { get; } // TODO: need straw items to make this worthwhile.
    public Material tin { get; }
    public Material vegetable { get; }
    public Material water { get; }
    public Material wax { get; }
    public Material wood { get; }
    public IReadOnlyList<Material> RustMetals => RustMetalArray;

    private readonly Material[] RustMetalArray;
  }
}