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
      var Entities = Codex.Entities;
      var Kinds = Codex.Kinds;
      var Barriers = Codex.Barriers;
      var Grounds = Codex.Grounds;
      var Attributes = Codex.Attributes;
      var Zoos = Codex.Zoos;
      var Shrines = Codex.Shrines;
      var Shops = Codex.Shops;

      Trick AddTrick(string Name, Action<ApplyEditor> Action)
      {
        return Register.Add(T =>
        {
          T.Name = Name;

          Action(T.Apply);
        });
      }

      awaken_slumber = AddTrick("awaken slumber", A =>
      {
        A.LoseTalent(Properties.sleeping);
      });

      living_statue = AddTrick("living statue", A =>
      {
        A.Liberate(Dice.One);
      });

      whispered_rumour = AddTrick("whispered rumour", A =>
      {
        A.Rumour(Attributes.wisdom, Skills.literacy, Truth: true, Lies: true);
      });

      arriving_bats = AddTrick("arriving bats", A =>
      {
        A.ArriveEntity(Dice.One, Sonics.chirp, Kinds.bat);
      });

      barred_way = AddTrick("barred way", A =>
      {
        A.CreateWall(WallStructure.Permanent, Barriers.iron_bars);
      });

      cleared_way = AddTrick("cleared way", A =>
      {
        A.RemoveWall(WallStructure.Permanent, Barriers.iron_bars);
      });

      connecting_portal = AddTrick("connecting portal", A =>
      {
        A.ConnectPassage(Codex.Portals.transportal);
      });

      connecting_rift = AddTrick("connecting rift", A =>
      {
        A.ConnectPassage(Codex.Portals.rift);
      });

      dismissed_illusion = AddTrick("dismissed illusion", A =>
      {
        A.RemoveWall(WallStructure.Illusionary);
      });

      emerging_blobs = AddTrick("emerging blobs", A =>
      {
        A.ArriveEntity(Dice.One, Sonics.burble, Kinds.blob);
      });

      escaping_mummies = AddTrick("escaping mummies", A =>
      {
        A.ArriveEntity(Dice.One, Sonics.moan, Kinds.mummy);
      });

      hatching_eggs = AddTrick("hatching eggs", A =>
      {
        A.Hatch();
      });

      instant_death = AddTrick("instant death", A =>
      {
        A.Death(Element: null, KindArray: null, Codex.Strikes.death, DeathSupport.deathray);
      });

      leaking_gas = AddTrick("leaking gas", A =>
      {
        A.ArriveEntity(Dice.One, Sonics.gas, new[] { Entities.gas_spore });
      });

      living_dead = AddTrick("living dead", A =>
      {
        A.AnimateRevenants(Properties.rage);
      });

      scuttling_insects = AddTrick("scuttling insects", A =>
      {
        A.ArriveEntity(Dice.One, Sonics.scuttle, Kinds.insect);
      });

      summoning_demons = AddTrick("summoning demons", A =>
      {
        A.ArriveEntity(Dice.One, Sonics.chant, Kinds.demon);
      });

      surrounding_horde = AddTrick("surrounding horde", A =>
      {
        A.CreateRandomHorde(Dice.One);
      });

      keystone_kops = AddTrick("keystone kops", A =>
      {
        A.CreateSpecificHorde(Dice.One, Codex.Hordes.keystone);
      });

      returning_undead = AddTrick("returning undead",  A =>
      {
        A.ArriveEntity(Dice.One, Sonics.tunnelling, Kinds.Undead.ToArray());
      });

      watery_noodles = AddTrick("watery noodles", A =>
      {
        A.ArriveEntity(1.d3(), Sonics.water_splash, new[] { Entities.water_moccasin, Entities.electric_eel, Entities.giant_eel, Entities.pearl_golem });
      });

      pinching_crabs = AddTrick("pinching crabs", A =>
      {
        A.ArriveEntity(1.d3(), Sonic: null, new[] { Entities.giant_crab });
      });

      calling_guard = AddTrick("calling guard", A =>
      {
        A.CreateEntity(Dice.One, Sonics.whistle, Entities.guard);
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
        A.AnimateObjects(ObjectEntity: Entities.animate_object, Corrupt: Properties.rage);
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
        A.ConvertWall(Barriers.hell_brick, Locality.Map);
        //A.ConvertFloor(Grounds.obsidian_floor, Locality.Map);
        //A.ConvertDoor(Gates.crystal_door, Locality.Map);
      });

      marble_paving = AddTrick("marble paving", A =>
      {
        A.ConvertFloor(FromGround: null, ToGround: Grounds.marble_floor, Locality.Square);
      });

      mobilise_boulder = AddTrick("mobile boulder", A =>
      {
        A.MobiliseBoulder(Rigid: false);
      });

      complete_mapping = AddTrick("complete mapping", A =>
      {
        A.Mapping(Range.Sq0, Chance.Always);
        A.DetectBoulder(Range.Sq0);
        A.DetectTrap(Range.Sq0, Reveal: false);
      });

      transport_candidate = AddTrick("transport candidate", A =>
      {
        A.Transport();
      });

      change_route = AddTrick("change route", A =>
      {
        A.Route();
      });

      warping = AddTrick("warping", A =>
      {
        A.Warping();
      });

      this.VisitZooArray = new Trick[Zoos.List.Count];
      foreach (var Zoo in Zoos.List)
        VisitZooArray[Zoo.Index] = AddTrick("visited " + Zoo.Name, A => A.VisitZoo(Zoo));

      this.visited_bazaar = AddTrick("visited bazaar", A => A.VisitBazaar(Sonics.chime));

      this.VisitShopArray = new Trick[Shops.List.Count];
      foreach (var Shop in Shops.List)
        VisitShopArray[Shop.Index] = AddTrick("visited " + Shop.Name, A => A.VisitShop(Shop));
      
      this.VisitShrineArray = new Trick[Shrines.List.Count];
      foreach (var Shrine in Shrines.List)
        VisitShrineArray[Shrine.Index] = AddTrick("visited " + Shrine.Name, A => A.VisitShrine(Shrine));

      Register.Alias(awaken_slumber, "awake character");
    }
#endif

    public readonly Trick awaken_slumber;
    public readonly Trick barred_way;
    public readonly Trick cleared_way;
    public readonly Trick change_route;
    public readonly Trick connecting_portal;
    public readonly Trick connecting_rift;
    public readonly Trick dismissed_illusion;
    public readonly Trick increase_difficulty;
    public readonly Trick overwhelm_difficulty;
    public readonly Trick animated_objects;
    public readonly Trick arriving_bats;
    public readonly Trick calling_guard;
    public readonly Trick leaking_gas;
    public readonly Trick living_dead;
    public readonly Trick escaping_mummies;
    public readonly Trick emerging_blobs;
    public readonly Trick hatching_eggs;
    public readonly Trick instant_death;
    public readonly Trick scuttling_insects;
    public readonly Trick marble_paving;
    public readonly Trick mobilise_boulder;
    public readonly Trick surrounding_horde;
    public readonly Trick keystone_kops;
    public readonly Trick random_spawning;
    public readonly Trick returning_undead;
    public readonly Trick summoning_demons;
    public readonly Trick living_statue;
    public readonly Trick automatic_locking;
    public readonly Trick pinching_crabs;
    public readonly Trick watery_noodles;
    public readonly Trick sudden_hellscape;
    public readonly Trick complete_mapping;
    public readonly Trick transport_candidate;
    public readonly Trick warping;
    public readonly Trick whispered_rumour;
    public readonly Trick[] VisitZooArray;
    public readonly Trick visited_bazaar;
    public readonly Trick[] VisitShopArray;
    public readonly Trick[] VisitShrineArray;
  }
}
