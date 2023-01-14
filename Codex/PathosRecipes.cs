using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexRecipes : CodexPage<ManifestRecipes, RecipeEditor, Recipe>
  {
    private CodexRecipes() { }
#if MASTER_CODEX
    internal CodexRecipes(Codex Codex)
      : base(Codex.Manifest.Recipes)
    {
      var Entities = Codex.Entities;
      var Items = Codex.Items;
      var Materials = Codex.Materials;

      Recipe AddRecipe(Entity Entity, Material KeyMaterial, params Item[] ItemArray)
      {
        Debug.Assert(Entity != null);
        Debug.Assert(ItemArray.IsDistinct(), $"{KeyMaterial} must have distinct items.");

        return Register.Add(R =>
        {
          R.Entity = Entity;
          R.KeyMaterial = KeyMaterial;
          R.SetItems(ItemArray);

          //CodexRecruiter.Enrol(() =>
          //{
          //  Debug.Assert(ItemArray.All(I => I.Material == KeyMaterial), $"{KeyMaterial} items must all be of the key material.");
          //});
        });
      }

      AddRecipe(Entities.clay_golem, Materials.clay, Items.rock);
      
      AddRecipe(Entities.crystal_golem, Materials.crystal, Items.crystal_plate_mail, Items.crystal_ball, Items.dilithium_crystal);
      
      AddRecipe(Entities.diamond_golem, Materials.gemstone, Items.diamond);
      
      AddRecipe(Entities.flesh_golem, Materials.animal, Items.huge_chunk_of_meat);
      
      AddRecipe(Entities.glass_golem, Materials.glass, Items.black_glass_bauble, Items.blue_glass_bauble, Items.green_glass_bauble, Items.orange_glass_bauble, Items.red_glass_bauble, Items.violet_glass_bauble, Items.white_glass_bauble, Items.yellow_glass_bauble, Items.yellowish_brown_glass_bauble);
      
      AddRecipe(Entities.gold_golem, Materials.gold, Items.gold_coin);
      
      AddRecipe(Entities.iron_golem, Materials.iron, Items.iron_chain, Items.plate_mail, Items.iron_shoes, Items.long_sword, Items.twohanded_sword);
      
      AddRecipe(Entities.leather_golem, Materials.leather, Items.leather_armour, Items.leather_cloak, Items.leather_jacket, Items.leather_gloves);
      
      AddRecipe(Entities.mithril_golem, Materials.mithril, 
        Items.elven_mithrilcoat, Items.dwarvish_mithrilcoat, Items.dark_elven_mithrilcoat, Items.mithril_shield, Items.mithril_barding,
        Items.mithril_long_sword, Items.mithril_twohanded_sword, Items.mithril_battleaxe,
        Items.mithril_spear, Items.mithril_lance, Items.mithril_sabre, Items.mithril_katar);

      AddRecipe(Entities.paper_golem, Materials.paper, Items.scroll_of_blank_paper, Items.book_of_blank_paper);
      
      AddRecipe(Entities.plastic_golem, Materials.plastic, Items.expensive_camera, Items.fly_swatter, Items.rubber_hose, Items.ice_box);
      
      AddRecipe(Entities.rope_golem, Materials.cloth, Items.sack);
      
      AddRecipe(Entities.ruby_golem, Materials.gemstone, Items.ruby);
      
      AddRecipe(Entities.sapphire_golem, Materials.gemstone, Items.sapphire);
      
      AddRecipe(Entities.snow_golem, Materials.ice, Items.carrot);
      
      AddRecipe(Entities.stone_golem, Materials.stone, Items.amulet_versus_stone, Items.flint, Items.stone_club);
      
      AddRecipe(Entities.straw_golem, Materials.vegetable, Items.pancake, Items.holy_wafer, Items.food_ration, Items.fortune_cookie, Items.lembas_wafer, Items.sandwich, Items.tortilla);
      
      AddRecipe(Entities.wax_golem, Materials.wax, Items.wax_candle, Items.magic_candle);
      
      AddRecipe(Entities.wood_golem, Materials.wood, Items.quarterstaff, Items.chest);
    }
#endif
  }
}
