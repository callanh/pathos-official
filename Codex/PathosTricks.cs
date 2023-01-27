using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexTricks : CodexPage<ManifestTricks, TrickEditor, Trick>
  {
    private CodexTricks() { }
#if MASTER_CODEX
    internal CodexTricks(Codex Codex)
      : base(Codex.Manifest.Tricks)
    {
      var Strikes = Codex.Strikes;
      var Skills = Codex.Skills;
      var Sonics = Codex.Sonics;
      var Properties = Codex.Properties;

      Trick AddTrick(string Name, Action<ApplyEditor> Action)
      {
        return Register.Add(T =>
        {
          T.Name = Name;

          Action(T.Apply);
        });
      }

      living_statue = AddTrick("living statue", A =>
      {
        A.Liberate(Dice.One);
      });

      whispered_rumour = AddTrick("whispered rumour", A =>
      {
        A.Rumour(Skills.literacy, Truth: true, Lies: true);
      });

      arriving_bats = AddTrick("arriving bats", A =>
      {
        A.ArriveEntity(Dice.One, Codex.Sonics.chirp, Codex.Kinds.bat);
      });

      barred_way = AddTrick("barred way", A =>
      {
        A.CreateWall(WallStructure.Permanent, Codex.Barriers.iron_bars);
      });

      cleared_way = AddTrick("cleared way", A =>
      {
        A.DestroyWall(WallStructure.Permanent, Codex.Barriers.iron_bars);
      });

      emerging_blobs = AddTrick("emerging blobs", A =>
      {
        A.ArriveEntity(Dice.One, Codex.Sonics.burble, Codex.Kinds.blob);
      });

      scuttling_insects = AddTrick("scuttling insects", A =>
      {
        A.ArriveEntity(Dice.One, Codex.Sonics.scuttle, Codex.Kinds.insect);
      });

      summoning_demons = AddTrick("summoning demons", A =>
      {
        A.ArriveEntity(Dice.One, Codex.Sonics.chant, Codex.Kinds.demon);
      });

      escaping_mummies = AddTrick("escaping mummies", A =>
      {
        A.ArriveEntity(Dice.One, Codex.Sonics.moan, Codex.Kinds.mummy);
      });

      leaking_gas = AddTrick("leaking gas", A =>
      {
        A.ArriveEntity(Dice.One, Codex.Sonics.gas, new[] { Codex.Entities.gas_spore });
      });

      surrounding_horde = AddTrick("surrounding horde", A =>
      {
        A.CreateHorde(Dice.Fixed(1));
      });

      returning_undead = AddTrick("returning undead",  A =>
      {
        A.ArriveEntity(Dice.One, Codex.Sonics.tunnelling, Codex.Kinds.Undead.ToArray());
      });

      calling_guard = AddTrick("calling guard", A =>
      {
        A.CreateEntity(Dice.One, Codex.Sonics.whistle, Codex.Entities.guard);
      });

      random_spawning = AddTrick("random spawning", A =>
      {
        A.CreateEntity(Dice.One);
      });

      automatic_locking = AddTrick("automatic locking", A =>
      {
        A.Locking(Range.Sq6);
      });

      animated_objects = AddTrick("animated objects", A =>
      {
        A.Strike(Strikes.magic);
        A.AnimateObjects(Corrupt: Properties.rage);
      });

      increase_difficulty = AddTrick("increase difficulty", A =>
      {
        A.AdjustDifficulty(+1, Sonics.foreboding);
      });

      overwhelm_difficulty = AddTrick("overwhelm difficulty", A =>
      {
        A.AdjustDifficulty(+5, Sonics.foreboding);
      });

      sudden_hellscape = AddTrick("sudden hellscape", A =>
      {
        A.ConvertWall(Codex.Barriers.hell_brick, Locality.Map);
        //A.ConvertFloor(Codex.Grounds.obsidian_floor, Locality.Map);
        //A.ConvertDoor(Codex.Gates.crystal_door, Locality.Map);
      });

      marble_paving = AddTrick("marble paving", A =>
      {
        A.ConvertFloor(FromGround: null, ToGround: Codex.Grounds.marble_floor, Locality.Square);
      });

      complete_mapping = AddTrick("complete mapping", A =>
      {
        A.Mapping(Range.Sq0, Chance.Always);
        A.DetectBoulder(Range.Sq0);
        A.DetectTrap(Range.Sq0, Reveal: false);
      });

      warping = AddTrick("warping", A =>
      {
        A.Warping();
      });

      this.VisitZooArray = new Trick[Codex.Zoos.List.Count];
      foreach (var Zoo in Codex.Zoos.List)
        VisitZooArray[Zoo.Index] = AddTrick("visited " + Zoo.Name, A => A.VisitZoo(Zoo));

      this.visited_bazaar = AddTrick("visited bazaar", A => A.VisitShop(null));

      this.VisitShopArray = new Trick[Codex.Shops.List.Count];
      foreach (var Shop in Codex.Shops.List)
        VisitShopArray[Shop.Index] = AddTrick("visited " + Shop.Name, A => A.VisitShop(Shop));
      
      this.VisitShrineArray = new Trick[Codex.Shrines.List.Count];
      foreach (var Shrine in Codex.Shrines.List)
        VisitShrineArray[Shrine.Index] = AddTrick("visited " + Shrine.Name, A => A.VisitShrine(Shrine));
    }
#endif

    public readonly Trick barred_way;
    public readonly Trick cleared_way;
    public readonly Trick increase_difficulty;
    public readonly Trick overwhelm_difficulty;
    public readonly Trick animated_objects;
    public readonly Trick arriving_bats;
    public readonly Trick calling_guard;
    public readonly Trick leaking_gas;
    public readonly Trick escaping_mummies;
    public readonly Trick emerging_blobs;
    public readonly Trick scuttling_insects;
    public readonly Trick marble_paving;
    public readonly Trick surrounding_horde;
    public readonly Trick random_spawning;
    public readonly Trick returning_undead;
    public readonly Trick summoning_demons;
    public readonly Trick living_statue;
    public readonly Trick automatic_locking;
    public readonly Trick sudden_hellscape;
    public readonly Trick complete_mapping;
    public readonly Trick warping;
    public readonly Trick whispered_rumour;
    public readonly Trick[] VisitZooArray;
    public readonly Trick visited_bazaar;
    public readonly Trick[] VisitShopArray;
    public readonly Trick[] VisitShrineArray;
  }
}
