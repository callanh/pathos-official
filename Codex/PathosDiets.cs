using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexDiets : CodexPage<ManifestDiets, DietEditor, Diet>
  {
    private CodexDiets() { }
#if MASTER_CODEX
    internal CodexDiets(Codex Codex)
      : base(Codex.Manifest.Diets)
    {
      var Materials = Codex.Materials;

      Diet AddDiet(string Name, params Material[] MaterialArray)
      {
        return Register.Add(D =>
        {
          D.Name = Name;
          D.SetMaterials(MaterialArray);
        });
      }

      inediate = AddDiet("inediate");
      omnivore = AddDiet("omnivore", Materials.fruit, Materials.vegetable, Materials.animal);
      carnivore = AddDiet("carnivore", Materials.animal);
      fabrivore = AddDiet("fabrivore", Materials.fruit, Materials.vegetable, Materials.wax, Materials.leather, Materials.hide, Materials.cloth, /*Materials.straw,*/ Materials.bone);
      geophagy = AddDiet("geophagy", Materials.stone, Materials.animal, Materials.hide, Materials.leather, Materials.gemstone, Materials.glass, Materials.bone);
      hematophagy = AddDiet("hematophagy");
      Register.Edit(hematophagy).Blood = true;
      herbivore = AddDiet("herbivore", Materials.fruit, Materials.vegetable, /*Materials.straw,*/ Materials.paper);
      lithivore = AddDiet("lithivore", Materials.stone, Materials.sand, Materials.glass, Materials.clay, Materials.crystal, Materials.gemstone);
      metalivore = AddDiet("metalivore", Materials.tin, Materials.iron, Materials.copper, Materials.silver, Materials.gold, Materials.platinum, Materials.mithril);
      organivore = AddDiet("organivore", Materials.fruit, Materials.vegetable, Materials.wax, Materials.leather, Materials.hide, Materials.cloth, /*Materials.straw,*/ Materials.wood, Materials.paper, Materials.animal);
      //plastivore = AddDiet("plastivore", Materials.plastic);
      xylophagy = AddDiet("xylophagy", Materials.wood, Materials.paper);
    }
#endif

    public readonly Diet inediate;
    public readonly Diet omnivore;
    public readonly Diet carnivore;
    public readonly Diet fabrivore;
    public readonly Diet geophagy;
    public readonly Diet hematophagy;
    public readonly Diet herbivore;
    public readonly Diet lithivore;
    public readonly Diet metalivore;
    public readonly Diet organivore;
    //public readonly Diet plastivore;
    public readonly Diet xylophagy;
  }
}