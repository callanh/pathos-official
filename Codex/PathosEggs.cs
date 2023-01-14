using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexEggs : CodexPage<ManifestEggs, EggEditor, Egg>
  {
    private CodexEggs() { }
#if MASTER_CODEX
    internal CodexEggs(Codex Codex)
      : base(Codex.Manifest.Eggs)
    {
      Egg AddEgg(string Name, Entity Layer, Entity Hatchling, Element Element)
      {
        Debug.Assert(Layer != null);
        Debug.Assert(Hatchling != null);

        return Register.Add(E =>
        {
          E.Name = Name;
          E.Layer = Layer;
          E.Hatchling = Hatchling;
          E.Element = Element;
        });
      }

      CodexRecruiter.Enrol(() =>
      {
        var Entities = Codex.Entities;
        var Items = Codex.Items;
        var Elements = Codex.Elements;

        // dragon.
        AddEgg("black dragon", Entities.adult_black_dragon, Entities.baby_black_dragon, Elements.acid);
        AddEgg("blue dragon", Entities.adult_blue_dragon, Entities.baby_blue_dragon, Elements.shock);
        AddEgg("deep dragon", Entities.adult_deep_dragon, Entities.baby_deep_dragon, Elements.poison);
        AddEgg("green dragon", Entities.adult_green_dragon, Entities.baby_green_dragon, Elements.poison);
        AddEgg("gold dragon", Entities.adult_gold_dragon, Entities.baby_gold_dragon, Elements.magical);
        AddEgg("orange dragon", Entities.adult_orange_dragon, Entities.baby_orange_dragon, Elements.sleep);
        AddEgg("red dragon", Entities.adult_red_dragon, Entities.baby_red_dragon, Elements.fire);
        AddEgg("shimmering dragon", Entities.adult_shimmering_dragon, Entities.baby_shimmering_dragon, Elements.magical);
        AddEgg("silver dragon", Entities.adult_silver_dragon, Entities.baby_silver_dragon, Elements.cold);
        AddEgg("white dragon", Entities.adult_white_dragon, Entities.baby_white_dragon, Elements.cold);
        AddEgg("yellow dragon", Entities.adult_yellow_dragon, Entities.baby_yellow_dragon, Elements.disintegrate);
        // wyvern/hydra?

        // TODO: explicit Egg Names.

        // worm.
        AddEgg(null, Entities.long_worm, Entities.baby_long_worm, Elements.digging);
        AddEgg(null, Entities.purple_worm, Entities.baby_purple_worm, Elements.acid);

        // insect.
        AddEgg(null, Entities.killer_bee, Entities.killer_bee, Elements.force);
        AddEgg(null, Entities.centipede, Entities.centipede, Elements.force);
        AddEgg(null, Entities.nickelpede, Entities.nickelpede, Elements.force);
        AddEgg(null, Entities.scorpion, Entities.scorpion, Elements.force);
        AddEgg(null, Entities.giant_scorpion, Entities.giant_scorpion, Elements.force);
        AddEgg(null, Entities.giant_tick, Entities.giant_tick, Elements.force);
        AddEgg(null, Entities.giant_flea, Entities.giant_flea, Elements.force);
        AddEgg(null, Entities.carrion_crawler, Entities.carrion_crawler, Elements.acid);
        AddEgg(null, Entities.giant_louse, Entities.giant_louse, Elements.force);
        AddEgg(null, Entities.giant_cockroach, Entities.giant_cockroach, Elements.force);

        // bird.
        AddEgg(null, Entities.cockatrice, Entities.chickatrice, Elements.force);
        AddEgg(null, Entities.pyrolisk, Entities.pyrolisk, Elements.fire);
        AddEgg(null, Entities.chicken, Entities.chicken, Elements.force);
        AddEgg(null, Entities.parrot, Entities.parrot, Elements.force);
        AddEgg(null, Entities.asphynx, Entities.asphynx, Elements.force);

        // snake.
        AddEgg(null, Entities.snake, Entities.snake, Elements.force);
        AddEgg(null, Entities.garter_snake, Entities.garter_snake, Elements.force);
        AddEgg(null, Entities.water_moccasin, Entities.water_moccasin, Elements.water);
        AddEgg(null, Entities.cobra, Entities.cobra, Elements.force);
        AddEgg(null, Entities.king_cobra, Entities.king_cobra, Elements.force);
        AddEgg(null, Entities.python, Entities.python, Elements.force);

        // spider.
        AddEgg(null, Entities.giant_spider, Entities.giant_spider, Elements.force);
        AddEgg(null, Entities.cave_spider, Entities.cave_spider, Elements.force);
        AddEgg(null, Entities.recluse_spider, Entities.recluse_spider, Elements.force);
        AddEgg(null, Entities.barking_spider, Entities.barking_spider, Elements.force);
        AddEgg(null, Entities.phase_spider, Entities.phase_spider, Elements.force);
        
        // ant.
        AddEgg(null, Entities.giant_ant, Entities.giant_ant, Elements.force);
        AddEgg(null, Entities.soldier_ant, Entities.soldier_ant, Elements.force);
        AddEgg(null, Entities.fire_ant, Entities.fire_ant, Elements.fire);
        AddEgg(null, Entities.snow_ant, Entities.snow_ant, Elements.cold);
        
        // gremlin.
        AddEgg(null, Entities.gargoyle, Entities.gargoyle, Elements.force);
        AddEgg(null, Entities.winged_gargoyle, Entities.winged_gargoyle, Elements.force);

        // aquatic.
        AddEgg(null, Entities.crocodile, Entities.baby_crocodile, Elements.water);
        AddEgg(null, Entities.giant_crab, Entities.giant_crab, Elements.water);
        AddEgg(null, Entities.giant_eel, Entities.giant_eel, Elements.water);
        AddEgg(null, Entities.electric_eel, Entities.electric_eel, Elements.water);
        AddEgg(null, Entities.piranha, Entities.piranha, Elements.water);
        // TODO: shark, some sharks lay eggs, some don't.

        // lizard.
        AddEgg(null, Entities.rhaumbusun, Entities.rhaumbusun, Elements.force);
        AddEgg(null, Entities.basilisk, Entities.basilisk, Elements.acid);
        AddEgg(null, Entities.komodo_dragon, Entities.komodo_dragon, Elements.force);
        AddEgg(null, Entities.lizard, Entities.lizard, Elements.force);
        AddEgg(null, Entities.newt, Entities.newt, Elements.water);
        AddEgg(null, Entities.iguana, Entities.iguana, Elements.water);
        AddEgg(null, Entities.chameleon, Entities.chameleon, Elements.magical);
        AddEgg(null, Entities.gila_monster, Entities.gila_monster, Elements.poison);
        AddEgg(null, Entities.salamander, Entities.salamander, Elements.fire);

        // naga.
        AddEgg(null, Entities.red_naga, Entities.red_naga_hatchling, Elements.fire);
        AddEgg(null, Entities.black_naga, Entities.black_naga_hatchling, Elements.acid);
        AddEgg(null, Entities.golden_naga, Entities.golden_naga_hatchling, Elements.poison);
        AddEgg(null, Entities.guardian_naga, Entities.guardian_naga_hatchling, Elements.magical);
      });
    }
#endif
  }
}
