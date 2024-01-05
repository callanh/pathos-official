using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Inv.Support;
using System.Collections;

namespace Pathos
{
  public sealed class Codex
  {
    internal Codex() { }
#if MASTER_CODEX
    public Codex(Manifest Manifest)
    {
      this.Manifest = Manifest;

      CodexRecruiter.Codex = this;
      CodexRecruiter.Start();
      try
      {
        this.Glyphs = new CodexGlyphs(this);
        this.Sonics = new CodexSonics(this);
        this.Tracks = new CodexTracks(this);
        this.Atmospheres = new CodexAtmospheres(this);
        this.Attributes = new CodexAttributes(this);
        this.Sanctities = new CodexSanctities(this);
        this.Encumbrances = new CodexEncumbrances(this);
        this.Qualifications = new CodexQualifications(this);
        this.AttackTypes = new CodexAttackTypes(this);
        this.Genders = new CodexGenders(this);
        this.Skills = new CodexSkills(this);
        this.Anatomies = new CodexAnatomies(this);
        this.Slots = new CodexSlots(this);
        this.Elements = new CodexElements(this);
        this.Properties = new CodexProperties(this);
        this.Appetites = new CodexAppetites(this);
        this.Standings = new CodexStandings(this);
        this.Warnings = new CodexWarnings(this);
        this.Materials = new CodexMaterials(this);
        this.Diets = new CodexDiets(this);
        this.Motions = new CodexMotions(this);
        this.Rumours = new CodexRumours(this);
        this.Recruitments = new CodexRecruitments(this);
        this.Kinds = new CodexKinds(this);
        this.Races = new CodexRaces(this);
        this.Grounds = new CodexGrounds(this);
        this.Barriers = new CodexBarriers(this);
        this.Engulfments = new CodexEngulfments(this);
        this.Beams = new CodexBeams(this);
        this.Strikes = new CodexStrikes(this);
        this.Explosions = new CodexExplosions(this);
        this.Stocks = new CodexStocks(this);
        this.Grades = new CodexGrades(this);
        this.Items = new CodexItems(this);
        this.Entities = new CodexEntities(this);
        this.Evolutions = new CodexEvolutions(this);
        this.Volatiles = new CodexVolatiles(this);
        this.Blocks = new CodexBlocks(this);
        this.Devices = new CodexDevices(this);
        this.Gates = new CodexGates(this);
        this.Schools = new CodexSchools(this);
        this.Spells = new CodexSpells(this);
        this.Specials = new CodexSpecials(this);
        this.Classes = new CodexClasses(this);
        this.Hordes = new CodexHordes(this);
        this.Features = new CodexFeatures(this);
        this.Zoos = new CodexZoos(this);
        this.Heroes = new CodexHeroes(this);
        this.Services = new CodexServices(this);
        this.Shops = new CodexShops(this);
        this.Shrines = new CodexShrines(this);
        this.Portals = new CodexPortals(this);
        this.Platforms = new CodexPlatforms(this);
        this.Punishments = new CodexPunishments(this);
        this.Afflictions = new CodexAfflictions(this);
        this.Eggs = new CodexEggs(this);
        this.Recipes = new CodexRecipes(this);
        this.Tricks = new CodexTricks(this);
        this.Companions = new CodexCompanions(this);

        CodexRecruiter.Commit();
      }
      finally
      {
        CodexRecruiter.Stop();
        CodexRecruiter.Codex = null;
      }

      Manifest.Levelling.Set(Sonics.gain_level, Sonics.lose_level, new int[40]
      {
        /* 01 */ 0,
        /* 02 */ 25,
        /* 03 */ 50,
        /* 04 */ 100,
        /* 05 */ 200,
        /* 06 */ 500,
        /* 07 */ 1000,
        /* 08 */ 5000,
        /* 09 */ 10000,
        /* 10 */ 25000,
        /* 11 */ 50000,
        /* 12 */ 100000,
        /* 13 */ 150000,
        /* 14 */ 200000,
        /* 15 */ 250000,
        /* 16 */ 300000,
        /* 17 */ 350000,
        /* 18 */ 400000,
        /* 19 */ 450000,
        /* 20 */ 500000,
        /* 21 */ 550000,
        /* 22 */ 600000,
        /* 23 */ 650000,
        /* 24 */ 700000,
        /* 25 */ 750000,
        /* 26 */ 800000,
        /* 27 */ 850000,
        /* 28 */ 900000,
        /* 29 */ 950000,
        /* 30 */ 1000000,
        /* 31 */ 1100000,
        /* 32 */ 1200000,
        /* 33 */ 1300000,
        /* 34 */ 1400000,
        /* 35 */ 1500000,
        /* 36 */ 1600000,
        /* 37 */ 1700000,
        /* 38 */ 1800000,
        /* 39 */ 1900000,
        /* 40 */ 2000000
      });

      Manifest.Blinking.Set(Properties.blinking, new Requirement(Attributes.intelligence, 3), Sonics.blink, NutritionCost: 5);

      Manifest.Casting.Set(Attributes.intelligence, new[] { Anatomies.mind, Anatomies.voice });

      Manifest.Jumping.Set(Properties.jumping, new Requirement(Attributes.strength, 3), Sonics.jump, NutritionCost: 5);

      Manifest.Sliding.Set(Anatomies.amorphous, new Requirement(Attributes.constitution, 3), Sonics.slime, NutritionCost: 5);

      Manifest.Kicking.Set(new Requirement(Attributes.strength, 3), Elements.physical, Sonics.kick, NutritionCost: 1);

      Manifest.Praying.Set(Motions.pray, Sonics.gain_karma, Sonics.lose_karma, PrayKarmaCost: 250, new[] { Anatomies.mind, Anatomies.voice });
      Manifest.Praying.AddPrayer(Standings.hopeful, A =>
      {
        A.WhenSourceBelowAppetite(Appetites.hungry, T => T.Nutrition(Dice.Fixed(Rules.PrayNutrition)));
        A.Unstuck();
        A.Unpunish();
        A.Unafflict();
        A.WhenSourceNotHasProperty(Properties.polymorph_control, T => T.Unpolymorph());
      });
      Manifest.Praying.AddPrayer(Standings.good, A =>
      {
        A.WhenSourceBelowAppetite(Appetites.hungry, T => T.Nutrition(Dice.Fixed(Rules.PrayNutrition)));
        A.Unstuck();
        A.Unpunish();
        A.Unafflict();
        A.WhenSourceNotHasProperty(Properties.polymorph_control, T => T.Unpolymorph());
        A.Replenish(LifeThreshold: 50, ManaThreshold: 50);
      });
      Manifest.Praying.AddPrayer(Standings.glorious, A =>
      {
        A.WhenSourceBelowAppetite(Appetites.hungry, T => T.Nutrition(Dice.Fixed(Rules.PrayNutrition)));
        A.Unstuck();
        A.Unpunish();
        A.Unafflict();
        A.WhenSourceNotHasProperty(Properties.polymorph_control, T => T.Unpolymorph());
        A.Replenish(LifeThreshold: 50, ManaThreshold: 50);
        A.RemoveTransient(Properties.blindness, Properties.deafness, Properties.hallucination, Properties.rage, Properties.sickness);
      });
      Manifest.Praying.AddPrayer(Standings.exalted, A =>
      {
        A.WhenSourceBelowAppetite(Appetites.hungry, T => T.Nutrition(Dice.Fixed(Rules.PrayNutrition)));
        A.Unstuck();
        A.Unpunish();
        A.Unafflict();
        A.WhenSourceNotHasProperty(Properties.polymorph_control, T => T.Unpolymorph());
        A.Replenish(LifeThreshold: 50, ManaThreshold: 50);
        A.RemoveTransient(Properties.blindness, Properties.deafness, Properties.hallucination, Properties.rage, Properties.sickness);
        A.RemoveCurse(Dice.One); // remove one curse.
        A.RaiseDead(100, Corrupt: null, LoyalOnly: true); // raise one loyal companion from the dead.
      });

      Manifest.Searching.Set(Attributes.wisdom, Skills.traps);

      Manifest.Telekinesis.Set(Properties.telekinesis, new Requirement(Attributes.intelligence, 3), NutritionCost: 5);

      Manifest.Trading.Set(Attributes.charisma, Skills.bartering);

      Manifest.Tunnelling.Set(Properties.tunnelling, Strikes.tunnel, Elements.digging);
    }
#endif

    public Manifest Manifest { get; }
    public CodexAfflictions Afflictions { get; }
    public CodexAnatomies Anatomies { get; }
    public CodexAppetites Appetites { get; }
    public CodexAtmospheres Atmospheres { get; }
    public CodexAttackTypes AttackTypes { get; }
    public CodexAttributes Attributes { get; }
    public CodexBarriers Barriers { get; }
    public CodexBeams Beams { get; }
    public CodexBlocks Blocks { get; }
    public CodexClasses Classes { get; }
    public CodexCompanions Companions { get; }
    public CodexDevices Devices { get; }
    public CodexDiets Diets { get; }
    public CodexEggs Eggs { get; }
    public CodexElements Elements { get; }
    public CodexEncumbrances Encumbrances { get; }
    public CodexEngulfments Engulfments { get; }
    public CodexEntities Entities { get; }
    public CodexEvolutions Evolutions { get; }
    public CodexExplosions Explosions { get; }
    public CodexFeatures Features { get; }
    public CodexGates Gates { get; }
    public CodexGenders Genders { get; }
    public CodexGlyphs Glyphs { get; }
    public CodexGrades Grades { get; }
    public CodexGrounds Grounds { get; }
    public CodexHeroes Heroes { get; }
    public CodexHordes Hordes { get; }
    public CodexItems Items { get; }
    public CodexKinds Kinds { get; }
    public CodexMaterials Materials { get; }
    public CodexMotions Motions { get; }
    public CodexPlatforms Platforms { get; }
    public CodexPortals Portals { get; }
    public CodexProperties Properties { get; }
    public CodexPunishments Punishments { get; }
    public CodexQualifications Qualifications { get; }
    public CodexRaces Races { get; }
    public CodexRecipes Recipes { get; }
    public CodexRecruitments Recruitments { get; }
    public CodexRumours Rumours { get; }
    public CodexSanctities Sanctities { get; }
    public CodexSchools Schools { get; }
    public CodexServices Services { get; }
    public CodexShops Shops { get; }
    public CodexShrines Shrines { get; }
    public CodexSkills Skills { get; }
    public CodexSlots Slots { get; }
    public CodexSonics Sonics { get; }
    public CodexSpecials Specials { get; }
    public CodexSpells Spells { get; }
    public CodexStandings Standings { get; }
    public CodexStocks Stocks { get; }
    public CodexStrikes Strikes { get; }
    public CodexTracks Tracks { get; }
    public CodexTricks Tricks { get; }
    public CodexVolatiles Volatiles { get; }
    public CodexWarnings Warnings { get; }
    public CodexZoos Zoos { get; }

    public override string ToString() => Manifest.Name;
  }

  public abstract class CodexPage<TRegister, TEditor, TRecord>
    where TRegister : ManifestRegister<TEditor, TRecord>
    where TEditor : Editor<TRecord>
    where TRecord : ManifestRecord
  {
    protected CodexPage() { }
    internal CodexPage(TRegister Register)
    {
      this.RegisterField = Register;
    }

    /// <summary>
    /// Name of this register.
    /// </summary>
    public string Name => Register.Name;
    /// <summary>
    /// A list of everything registered.
    /// </summary>
    public IReadOnlyList<TRecord> List => Register.List;

    public TRecord Add(Action<TEditor> EditorAction) => Register.Add(EditorAction);
    public TEditor Edit(TRecord Record) => Register.Edit(Record);

    protected TRegister Register
    {
      get
      {
        if (Inv.Assert.IsEnabled)
          Inv.Assert.Check(RegisterField != null, "CodexPage must be constructed with a register.");

        return RegisterField;
      }
    }

    private readonly TRegister RegisterField;
  }

#if MASTER_CODEX
  internal static class CodexRecruiter
  {
    static CodexRecruiter()
    {
      RecruitList = new Inv.DistinctList<Action>();
    }

    internal static Codex Codex { get; set; }

    internal static void Start()
    {
      if (Inv.Assert.IsEnabled)
      {
        Inv.Assert.Check(!IsRecruiting, "Must not start recruiting multiple times in a row.");
        Inv.Assert.Check(RecruitList.Count == 0, "Must not start again when there are existing enrolments.");
      }

      IsRecruiting = true;
    }
    internal static void Stop()
    {
      if (Inv.Assert.IsEnabled)
        Inv.Assert.Check(IsRecruiting, "Must not stop recruiting multiple times in a row.");

      IsRecruiting = false;
      RecruitList.Clear(); // should be empty already, unless an exception occurred before or during commit.
    }
    internal static void Commit()
    {
      if (Inv.Assert.IsEnabled)
      {
        Inv.Assert.Check(IsRecruiting, "Must not commit recruiting when it isn't active.");
        Inv.Assert.Check(RecruitList.Count > 0, "Commit was called without enrolling any actions.");
      }

      foreach (var Recruit in RecruitList)
        Recruit();
      RecruitList.Clear();
    }
    internal static void Enrol(Action Action)
    {
      RecruitList.Add(Action);
    }

    // extension helper methods.
    internal static void Set(this FormEditor Form, int STR, int DEX, int CON, int INT, int WIS, int CHA)
    {
      var Attributes = Codex.Attributes;

      Form.Initialise(Attributes.List);
      Form.SetScore(Attributes.strength, STR);
      Form.SetScore(Attributes.dexterity, DEX);
      Form.SetScore(Attributes.constitution, CON);
      Form.SetScore(Attributes.intelligence, INT);
      Form.SetScore(Attributes.wisdom, WIS);
      Form.SetScore(Attributes.charisma, CHA);
    }

    internal static void Set(this FigureEditor Figure, Material Material, bool Head, bool Mind, bool Eyes, bool Ears, bool Hands, bool Limbs, bool Feet, bool Thermal, bool Blood, bool Mounted, bool Amorphous, bool Voice)
    {
      var Anatomy = Codex.Anatomies;

      Figure.Material = Material;
      if (Head) Figure.Set(Anatomy.head);
      if (Mind) Figure.Set(Anatomy.mind);
      if (Eyes) Figure.Set(Anatomy.eyes);
      if (Ears) Figure.Set(Anatomy.ears);
      if (Voice) Figure.Set(Anatomy.voice);
      if (Hands) Figure.Set(Anatomy.hands);
      if (Limbs) Figure.Set(Anatomy.limbs);
      if (Feet) Figure.Set(Anatomy.feet);
      if (Thermal) Figure.Set(Anatomy.thermal);
      if (Blood) Figure.Set(Anatomy.blood);
      if (Mounted) Figure.Set(Anatomy.mounted);
      if (Amorphous) Figure.Set(Anatomy.amorphous);
    }
    internal static void WithSourceSanctity(this ApplyEditor Apply, Action<ApplyEditor> BlessedAction, Action<ApplyEditor> UncursedAction, Action<ApplyEditor> CursedAction)
    {
      Apply.WithSourceSanctity(Effect =>
      {
        var Sanctities = Codex.Sanctities;

        Effect.Initialise(Sanctities.List);

        BlessedAction(new ApplyEditor(Codex.Manifest, Effect.GetApply(Sanctities.Blessed)));

        UncursedAction(new ApplyEditor(Codex.Manifest, Effect.GetApply(Sanctities.Uncursed)));

        CursedAction(new ApplyEditor(Codex.Manifest, Effect.GetApply(Sanctities.Cursed)));
      });
    }

    /// <summary>
    /// Special disarming has a 50% chance of stunning animate objects. Use normal disarm effect for everyone else.
    /// </summary>
    /// <param name="Apply"></param>
    internal static void SpecialDisarm(this ApplyEditor Apply)
    {
      Apply.WhenTargetAnimated
      (
        T => T.WhenChance(Chance.OneIn2, D => D.ApplyTransient(Codex.Properties.stunned, 3.d6())), // animate objects have a 50% chance of being stunned by a 'disarming' effect.
        E => E.Disarm(Codex.Attributes.dexterity)
      );
    }

    private static readonly Inv.DistinctList<Action> RecruitList;
    private static bool IsRecruiting;
  }
#endif

  internal sealed class CodexSanity
  {
    public CodexSanity()
    {
      this.Base = new Sanity();
    }

    public IReadOnlyList<string> Messages => Base.Messages;

    public void Check(Codex Codex)
    {
      var Manifest = Codex.Manifest;

      var EatMaterialArray = Manifest.Diets.List.SelectMany(D => D.Materials).ToArray();

      #region Items.
      Base.UniqueCheck("Item", Manifest.Items.List, I => I.Name);

      foreach (var Item in Manifest.Items.List)
      {
        if (Item.Weapon != null && Item.Equip == null)
          Record($"Item {Item.Name} weapon must have an equip specification.");

        if (Item.Sonic == null)
          Record($"Item {Item.Name} must have a drop sonic.");

        if (Item.Size == null && Item.Type != ItemType.Corpse && Item.Type != ItemType.Tin && Item.Type != ItemType.Egg)
          Record($"Item {Item.Name} must have a size.");

        if (Item.Weight <= Weight.Zero && Item.Type != ItemType.Coin && Item.Type != ItemType.Corpse && Item.Type != ItemType.Tin && Item.Type != ItemType.Egg)
          Record($"Item {Item.Name} must have a weight.");

        if (Item.Essence <= Essence.Zero && !Item.Grade.Unique)
          Record($"Item {Item.Name} is not unique so must have essence.");
        else if (Item.Essence != Essence.Zero && Item.Grade.Unique)
          Record($"Item {Item.Name} is unique so must have zero essence.");

        if (Item.Grade.Unique && !char.IsUpper(Item.Name[0]))
          Record($"Item {Item.Name} is unique so must have a proper name.");
        else if (!Item.Grade.Unique && char.IsUpper(Item.Name[0]))
          Record($"Item {Item.Name} is not unique so must have a common name.");

        if (Item.Rarity < 0)
          Record($"Item {Item.Name} must not have a negative rarity.");

        if (Item.Grade.Unique)
        {
          //if (Item.Description == null)
          //  Record($"Item {Item.Name} unique must have a description.");

          if (Item.Appearance != null)
            Record($"Item {Item.Name} unique must not have an appearance.");

          if (Item.DowngradeItem != null && !Item.DowngradeItem.Grade.Unique)
            Record($"Item {Item.Name} unique must not have a non-unique downgrade item.");

          if (Item.UpgradeItem != null && !Item.UpgradeItem.Grade.Unique)
            Record($"Item {Item.Name} unique must not have a non-unique upgrade item.");

          if (Item.Rarity > 1)
            Record($"Item {Item.Name} unique must not have a rarity greater than one.");

          if (Item.Weaknesses.Count > 0)
            Record($"Item {Item.Name} unique must not have any weaknesses.");
        }

        if (Item.Series != null && Item.Appearance == null)
          Record($"Item {Item.Name} must have an appearance if it is belongs to a series.");

        //if (Item.Appearance != null && Item.Glyph.Name != Item.Appearance.Name && Item.Shuffled && Item.Type != ItemType.Scroll)
        //  Record($"Item '{Item.Name}' appearance name '{Item.Appearance.Name}' and glyph name '{Item.Glyph.Name}' should match.");

        if (Item.Uses.Count == 0 && Item.Equip == null && Item.Storage == null && Item.Type != ItemType.Coin && Item.Type != ItemType.SpecificKey && !Item.Grade.Unique)
          Record($"Item {Item.Name} must have at least one use or can be equipped.");

        foreach (var Use in Item.Uses)
        {
          if (Use.Motion == Codex.Motions.eat && !EatMaterialArray.Contains(Item.Material))
            Record($"Item {Item.Name} is {Item.Material.Name} material which cannot be eaten by any diet.");

          if (Use.Apply.HasEffects())
          {
            if (Use.Motion == Codex.Motions.zap && Use.Cast == null)
              Record($"Item {Item.Name} must have a cast specified for the zap motion.");
          }
          else
          {
            if (Use.Motion != Codex.Motions.eat || (Item.Type != ItemType.Tin && Item.Type != ItemType.Egg))
              Record($"Item {Item.Name} Use {Use.Motion} must have some effect.");
          }

          if (Use.Motion != Codex.Motions.eat && Use.Motion != Codex.Motions.empty && Use.Consumed && Item.HasCharges())
            Record($"Item {Item.Name} Use {Use.Motion} should not consume an item that has charges.");

          if (!Use.Consumed && (Use.Motion == Codex.Motions.quaff/* || Use.Motion == Codex.Motions.Read || Use.Motion == Codex.Motions.Study*/))
            Record($"Item {Item.Name} Use {Use.Motion} should consume the item.");

          CheckApply(Use.Apply, () => $"Item {Item.Name} Use {Use.Motion}.");
        }

        //if (Item.UseList.Count > 1 && Item.UseList[0].Motion == Motion.Eat)
        //  Record($"Item {Item.Name} with more than one use should declare eating as the last use");

        if (Item.Appearance != null && Item.Glyph == Item.Appearance.Glyph)
          Record($"Item {Item.Name} should have a different glyph to the appearance glyph.");

        if ((Item.IsMeleeWeapon() || Item.IsReachWeapon() || Item.IsRangedWeapon() || Item.IsRangedMissile() || Item.IsThrownWeapon()) && Item.Weapon == null)
          Record($"Item {Item.Name} weapon must be specified");
        else if ((Item.IsRangedWeapon() || Item.IsRangedMissile()) && Item.Weapon.Ammunition == null)
          Record($"Item {Item.Name} ranged weapon/missile must have ammunition specified.");

        if ((Item.IsMeleeWeapon() || Item.IsRangedWeapon()) && Item.Size < Size.Small)
          Record($"Item {Item.Name} cannot be a tiny weapon.");

        if (Item.IsWeapon() && Item.Weapon.Skill == Codex.Skills.polearm && Item.Appearance == null && !Item.Grade.Unique)
          Record($"Item {Item.Name} must have an appearance because it is a polearm.");

        if (Item.Weapon != null && Item.Armour != null && Item.Weapon.Skill != Item.Armour.Skill)
          Record($"Item {Item.Name} must use the same skill for both weapon and armour.");

        if (Item.DerivativeEntities != null && Item.DerivativeEntities.Count == 0)
          Record($"Item {Item.Name} with derivative entity array must specified at least one entity.");

        var IsMissile = Item.IsCoin() || Item.IsSkeletonKey() || Item.IsLockPick() || Item.IsThrownWeapon() || Item.IsRangedMissile();
        if (IsMissile && !Item.IsBundled)
          Record($"Item {Item.Name} is a missile so must be bundled.");
        else if (!IsMissile && Item.IsBundled)
          Record($"Item {Item.Name} is not a missile so must not be bundled.");
      }

      if (!Manifest.Items.List.Any(I => I.Type == ItemType.Coin))
        Record("At least one coin item must be declared.");

      foreach (var ItemGroup in Manifest.Items.List.Where(I => I.Series != null).GroupBy(I => I.Series))
      {
        var ItemTypeArray = ItemGroup.Select(I => I.Type).Distinct().ToArray();

        if (ItemTypeArray.Length != 1)
          Record($"Series {ItemGroup.Key.Name} must contain only one type of item: {ItemTypeArray.Select(T => T.ToString()).AsSeparatedText(", ")}.");
      }

      foreach (var Item in Codex.Items.DragonScales)
      {
        if (Item.DerivativeEntities == null)
          Record($"Item {Item.Name} must have a derivative entity array.");
      }
      #endregion

      #region Entities.
      Base.UniqueCheck("Entity", Manifest.Entities.List, I => I.Name);

      foreach (var Entity in Manifest.Entities.List)
      {
        if (Entity.IsBase)
        {
          if (Entity.Frequency != 0)
            Record($"Entity {Entity.Name} is a base entity and must have a frequency of zero.");

          if (Entity.IsUnique)
            Record($"Entity {Entity.Name} is a base entity and must not be unique.");

          if (Entity.IsDomestic)
            Record($"Entity {Entity.Name} is a base entity and must not be domestic.");

          if (Entity.LesserEntity() != null || Entity.GreaterEntity() != null)
            Record($"Entity {Entity.Name} is a base entity and must not have a lesser or greater entity.");

          if (Entity.HasGreeds())
            Record($"Entity {Entity.Name} is a base entity and must not have any greeds.");

          if (Entity.Sonic != null)
            Record($"Entity {Entity.Name} is a base entity and must not have a sonic.");
        }
        else
        {
          //if (Entity.Name == Entity.Kind.Name)
          //  Record($"Entity {Entity.Name} should have a name that is distinct the owning kind name.");

          if (!Entity.IsTail)
          {
            if (Entity.DefaultForm.IsAllAssigned(10))
              Record($"Entity {Entity.Name} must have an assigned default form.");

            if (Entity.LimitForm.IsAllAssigned(30))
              Record($"Entity {Entity.Name} must have an assigned limit form.");

            if (Entity.LimitForm.Abilities.Any(A => A.Score > 30))
              Record($"Entity {Entity.Name} must have all assigned limit abilities no more than 30.");

            //if (Entity.Sonic == null)
            //  Record($"Entity {Entity.Name} must have a sonic");
          }
        }

        if (Entity.Weight < Weight.Zero)
          Record($"Entity {Entity.Name} weight base must be zero or more.");

        if (Entity.IsDomestic && Entity.CorpseChance != Chance.Always)
          Record($"Entity {Entity.Name} must always drop a corpse because it is domestic.");

        if (Entity.CorpseChance != Chance.Never && Entity.CorpseItem == null)
          Record($"Entity {Entity.Name} has a corpse so it must have a designated corpse item.");

        if (Entity.CorpseChance != Chance.Never && Entity.CorpseItem != null && Entity.Figure.Material != Entity.CorpseItem.Material)
          Record($"Entity {Entity.Name} has a corpse so the figure and item must be of the same material.");

        if (Entity.CorpseChance != Chance.Never && Entity.Figure.Material != Codex.Materials.animal && Entity.Figure.Material != Codex.Materials.vegetable)
          Record($"Entity {Entity.Name} has a corpse so must be animal or vegetable material.");

        if (Entity.Imitation && !Entity.Figure.Has(Codex.Anatomies.voice))
          Record($"Entity {Entity.Name} has imitation so it must have a voice.");

        if (Entity.IsUnique && Entity.Genders.Count != 1)
          Record($"Entity {Entity.Name} is unique so they must have exactly one gender.");

        if (Entity.IsUnique && !Entity.Startup.Resistances.Contains(Codex.Elements.magical))
          Record($"Entity {Entity.Name} must have magic resistance when it is marked as unique.");

        if (Entity.IsUnique && !Entity.Startup.Talents.Contains(Codex.Properties.polymorph_control))
          Record($"Entity {Entity.Name} must have polymorph control when it is marked as unique.");

        if (Entity.IsUnique && !char.IsUpper(Entity.Name[0]))
          Record($"Entity {Entity.Name} must have a proper name when it is marked as unique.");

        if ((Entity.LifeAdvancement.HardBase ?? 0) < 0)
          Record($"Entity {Entity.Name} must have a positive life hard base value.");
        //else if ((Entity.LifeAdvancement.HardBase ?? 0) == 0 && Entity.LifeAdvancement.LevelDice == Dice.Zero)
        //  Record($"Entity {Entity.Name} must have more than zero life.");

        if ((Entity.ManaAdvancement.HardBase ?? 0) < 0)
          Record($"Entity {Entity.Name} must have a positive mana hard base value.");
        //else if ((Entity.ManaAdvancement.HardBase ?? 0) == 0 && Entity.ManaAdvancement.LevelDice == Dice.Zero)
        //  Record($"Entity {Entity.Name} must have more than zero mana");

        if (Entity.IsMercenary && !Entity.IsGuardian)
          Record($"Entity {Entity.Name} must be a guardian if they are a mercenary.");

        if (!Entity.Figure.Has(Codex.Anatomies.limbs) && Entity.Startup.Talents.Contains(Codex.Properties.jumping))
          Record($"Entity {Entity.Name} without limbs should not be able to jump.");

        if (Entity.Figure.Has(Codex.Anatomies.mounted) && Entity.Figure.MountSkill == null)
          Record($"Entity {Entity.Name} that can be mounted is expected to have a mount skill.");
        else if (!Entity.Figure.Has(Codex.Anatomies.mounted) && Entity.Figure.MountSkill != null)
          Record($"Entity {Entity.Name} that cannot be mounted is not expected to have a mount skill.");

        if ((Entity.IsBase || Entity.IsAnimate) && Entity.Figure.CombatSkill == null)
          Record($"Entity {Entity.Name} is expected to have a combat skill.");
        else if (!(Entity.IsBase || Entity.IsAnimate) && Entity.Figure.CombatSkill != null)
          Record($"Entity {Entity.Name} is not expected to have a combat skill.");

        if (Entity.GreaterEntity() != null)
        {
          if (Entity.GreaterEntity() == Entity)
            Record($"Entity {Entity.Name} must not have a base greater entity pointing to itself.");

          if (Entity.GreaterEntity().IsBase)
            Record($"Entity {Entity.Name} must not have a base greater entity.");
        }

        if (Entity.LesserEntity() != null)
        {
          if (Entity.LesserEntity() == Entity)
            Record($"Entity {Entity.Name} must not have a base lesser entity pointing to itself.");

          if (Entity.LesserEntity().IsBase)
            Record($"Entity {Entity.Name} must not have a base lesser entity.");
        }

        if (Entity.IsHead && !Entity.Tail.Entity.IsTail)
          Record($"Entity head {Entity.Name} must have their tail {Entity.Tail.Entity.Name} declared as a tail.");

        if (Entity.IsHead && Entity.Figure.Has(Codex.Anatomies.mounted))
          Record($"Entity {Entity.Name} is mountable but heads are not yet implemented in the engine as mounts (the tail gets detached).");

        if (Entity.IsTail && Entity.Figure.Has(Codex.Anatomies.mounted))
          Record($"Entity {Entity.Name} is a tail and must not be set as mountable (riding a tail cannot be implemented).");

        if (Entity.IsTail && Entity.MayDropCorpse())
          Record($"Entity {Entity.Name} is a tail and must not drop a corpse.");

        if (Entity.Startup.Acquisitions.Contains(Codex.Properties.sleeping) && Entity.Startup.Resistances.Contains(Codex.Elements.sleep))
          Record($"Entity {Entity.Name} should not be sleeping and have sleep resistance.");

        if (Entity.Race != null && Entity.Genders.Any(G => !G.Recognised))
          Record($"Entity {Entity.Name} of race {Entity.Race.Name} should not be marked with a unrecognised gender.");

        //if (Entity.Race != null && Entity.Race.Name != Entity.Kind.Name)
        //  Record($"Entity {Entity.Name} of race {Entity.Race.Name} should not belong to a different kind {Entity.Kind.Name}.");

        foreach (var Attribute in Manifest.Attributes.List)
        {
          var DefaultAbility = Entity.DefaultForm[Attribute];
          var LimitAbility = Entity.LimitForm[Attribute];

          if (DefaultAbility.Score > LimitAbility.Score)
            Record($"Entity {Entity.Name} default {Attribute.Name} must be within the limit.");
        }

        var AttackIndex = 0;
        foreach (var Attack in Entity.Attacks)
        {
          //if (Attack.Cast != null && Attack.DamageDice != Dice.Zero)
          //  Record($"Entity {Entity.Name} attack[{AttackIndex}] cast must not have damage specified");

          if (Attack.Type.IsCasting && Attack.Cast == null)
            Record($"Entity {Entity.Name} attack[{AttackIndex}] {Attack.Type.ToString().ToLower()} must have a cast.");

          if (Attack.DamageDice == Dice.Zero && !Attack.Apply.HasEffects())
            Record($"Entity {Entity.Name} attack[{AttackIndex}] {Attack.Type.ToString().ToLower()} should have some damage or effect.");

          //if (!Entity.IsUnique && Attack.Type == AttackType.Summon && (Attack.Apply.GetEffects().First() as SummonEntityEffect).Entities.Any(E => E.Kind != Entity.Kind))
          //  Record($"Entity {Entity.Name} attack[{AttackIndex}] {Attack.Type.ToString().ToLower()} must only summon entities of the same kind");

          if (Entity.IsUnique && !char.IsUpper(Entity.Name[0]))
            Record($"Entity {Entity.Name} name must have a proper name when marked as unique.");
          else if (!Entity.IsUnique && char.IsUpper(Entity.Name[0]))
            Record($"Entity {Entity.Name} name must have a common name when marked as non-unique.");

          CheckReactions(Entity.Reactions, () => $"Entity {Entity.Name}");

          AttackIndex++;
        }

        if (Entity.Engulf == null && Entity.Attacks.Any(A => A.Type == Codex.AttackTypes.engulf))
          Record($"Entity {Entity.Name} must have an engulf specified if it has an engulfing attack.");
        else if (Entity.Engulf != null && !Entity.Attacks.Any(A => A.Type == Codex.AttackTypes.engulf))
          Record($"Entity {Entity.Name} must have an engulfing attack if it has an engulf specified.");

        foreach (var Retaliation in Entity.Retaliations)
        {
          //if (Retaliation.Cast == null)
          //  Record($"Entity {Entity.Name} retaliation {Retaliation.Type} must have a cast.");
        }

        if (Entity.Conveyance.HasEffects() && !Entity.MayDropCorpse())
          Record($"Entity {Entity.Name} must not have conveyance if it never drops a corpse.");

        CheckLoot($"Entity {Entity.Name}", Entity.Startup.Loot);

        var KitIndex = 0;
        foreach (var Kit in Entity.Startup.Loot.Kits)
        {
          if (Kit.Items == null)
          {
            Record($"Entity {Entity.Name} startup kit[{KitIndex}] must have an item array.");
          }
          else
          {
            foreach (var Item in Kit.Items)
            {
              if (Item.Weapon != null && !Entity.Startup.HasSkill(Item.Weapon.Skill))
                Record($"Entity {Entity.Name} can have kit weapon {Item.Name} but does not have skill in {Item.Weapon.Skill}.");
              else if (Item.Armour != null && !Entity.Startup.HasSkill(Item.Armour.Skill))
                Record($"Entity {Entity.Name} can have kit armour {Item.Name} but does not have skill in {Item.Armour.Skill}.");

              if (Item.Grade.Unique)
                Record($"Entity {Entity.Name} must not start with a unique {Item.Name}.");
            }
          }

          KitIndex++;
        }

        if (Entity.Terrains == null)
          Record($"Entity {Entity.Name} must have a terrain set");
        else if ((Entity.HasOnlyTerrain(Codex.Materials.water) || Entity.HasOnlyTerrain(Codex.Materials.lava)) && !Entity.Startup.HasSkill(Codex.Skills.swimming))
          Record($"Entity {Entity.Name} with water terrain must have skill in swimming.");

        if (!Entity.IsBase && Entity.Startup.Grimoires.Count > 0 && !Entity.Startup.HasSkill(Codex.Skills.literacy))
          Record($"Entity {Entity.Name} has grimoire spells but not skilled in {Codex.Skills.literacy.Name}.");

        var SpellIndex = 0;
        foreach (var Grimoire in Entity.Startup.Grimoires)
        {
          if (Grimoire.Spells.Count == 0)
            Record($"Entity {Entity.Name} startup grimoire[{SpellIndex}]must have spells.");

          foreach (var Spell in Grimoire.Spells)
          {
            if (!Entity.IsBase && !Entity.Startup.HasSkill(Spell.School.Skill))
              Record($"Entity {Entity.Name} has grimoire spell {Spell.Name} but not skill in {Spell.School.Skill}.");
          }
        }

        CheckLoot("Entity " + Entity.Name + " Drop Loot", Entity.DropLoot);
      }

      if (!Manifest.Entities.List.Any(E => E.IsAnimate))
        Record("At least one entity must be for animate objects.");
      #endregion

      #region Hordes.
      foreach (var Horde in Manifest.Hordes.List)
      {
        if (Horde.Minions.Count == 0 && Horde.Cavalries.Count == 0)
          Record($"Horde {Horde.Name} must have minions or cavalries");

        if (Horde.Minions.Any(M => M.Entity.IsBase))
          Record($"Horde {Horde.Name} minion must not have any base entities");

        foreach (var Cavalry in Horde.Cavalries)
        {
          if (Cavalry.RiderEntity.IsBase || Cavalry.SteedEntity.IsBase)
            Record($"Horde {Horde.Name} cavalry must not have any base entities");

          if (Cavalry.RiderEntity.Size >= Cavalry.SteedEntity.Size)
            Record($"Horde {Horde.Name} cavalry rider size must be less than the steed size");

          if (!Cavalry.SteedEntity.Figure.Has(Codex.Anatomies.mounted))
            Record($"Horde {Horde.Name} cavalry steed {Cavalry.SteedEntity.Name} must be mountable");
        }
      }
      #endregion

      #region Eggs.
      var EggIndex = 0;
      foreach (var Egg in Manifest.Eggs.List)
      {
        if (Egg.Hatchling == null)
          Record($"Egg {EggIndex} must have a hatchling entity.");

        if (Egg.Layer == null)
          Record($"Egg {EggIndex} must have a layer entity.");

        EggIndex++;
      }
      #endregion

      #region Features.
      Base.UniqueCheck("Feature", Manifest.Features.List, I => I.Name);

      foreach (var Feature in Manifest.Features.List)
      {
        if (Feature.RegularGlyph == null)
          Record($"Feature {Feature.Name} must have a regular glyph");

        if (Feature.BrokenGlyph == null)
          Record($"Feature {Feature.Name} must have a broken glyph");

        if (Feature.Sonic == null)
          Record($"Feature {Feature.Name} must have a sonic");

        if (Feature.Weight <= Weight.Zero)
          Record($"Feature {Feature.Name} must have a weight");

        foreach (var Use in Feature.Uses)
        {
          if (Use.Motion.PractisingSkill != null)
            Record($"Feature {Feature.Name} must not have uses {Use.Motion.PresentName} with a practising skill");
        }
      }
      #endregion

      #region Gates.
      Base.UniqueCheck("Gate", Manifest.Gates.List, I => I.Name);

      foreach (var Gate in Manifest.Gates.List)
      {
        if (Gate.CloseSonic == null)
          Record($"Gate {Gate.Name} must have a close sonic");

        if (Gate.BreakSonic == null)
          Record($"Gate {Gate.Name} must have a broken sonic");
      }
      #endregion

      #region Classes.
      Base.UniqueCheck("Class", Manifest.Classes.List, I => I.Name);

      foreach (var Class in Manifest.Classes.List)
      {
        CheckLoot("Class " + Class.Name, Class.Startup.Loot);

        foreach (var Kit in Class.Startup.Loot.Kits)
        {
          foreach (var Item in Kit.Items)
          {
            foreach (var Skill in Item.GetSkills())
            {
              if (Skill != null && !Class.Startup.HasSkill(Skill))
                Record($"Class {Class.Name} can start with an item that requires skill in {Skill.Name} (but is unskilled)");
            }
          }
        }

        foreach (var Grimoire in Class.Startup.Grimoires)
        {
          foreach (var Spell in Grimoire.Spells)
          {
            if (!Class.Startup.HasSkill(Spell.School.Skill))
              Record($"Class {Class.Name} can start with a spell that requires skill in {Spell.School.Skill.Name} (but is unskilled)");
          }
        }

        if (!Class.Distributions.IsDistinct() || Class.Distributions.Count != Manifest.Attributes.List.Count)
          Record($"Class {Class.Name} distribution must include all attributes exactly once.");
      }
      #endregion

      #region Devices.
      Base.UniqueCheck("Device.Name", Manifest.Devices.List, I => I.Name);
      Base.UniqueCheck("Device.Glyph", Manifest.Devices.List, I => I.Glyph.Name);

      foreach (var Device in Manifest.Devices.List)
      {
        CheckApply(Device.TriggerApply, () => $"Device {Device.Name} trigger apply");

        if (!Device.ManualUntrap && !Device.CrushedByBoulder)
          Record($"Device {Device.Name} must have a method for disarming/removal");

        if (Device.BoulderRemoval && !Device.CrushedByBoulder)
          Record($"Device {Device.Name} must be crushed by boulder, if it will remove the boulder");

        if (Device.MissileBlock != null && Device.Missiles.Count > 0)
          Record($"Device {Device.Name} with missile block must not have any missile items");

        if (Device.Weight <= Weight.Zero)
          Record($"Device {Device.Name} must have a weight");

        foreach (var UntrapLoot in Device.UntrapLoot.Kits)
        {
          //if (UntrapLoot.QuantityDice != Dice.One && !UntrapLoot.Item.Stacked)
          //  Record("Device {0} loot {1} must be a stacked item", Device.Name, UntrapLoot.Item.Name);
        }
      }
      #endregion

      #region Blocks.
      Base.UniqueCheck("Block", Manifest.Blocks.List, I => I.Name);

      foreach (var Block in Manifest.Blocks.List)
      {
        if (Block.Weight <= Weight.Zero)
          Record($"Block {Block.Name} must have a weight");
      }
      #endregion

      Base.UniqueCheck("Element", Manifest.Elements.List, I => I.Name);

      Base.UniqueCheck("Property", Manifest.Properties.List, I => I.Name);

      Base.UniqueCheck("Barrier", Manifest.Barriers.List, I => I.Name);

      Base.UniqueCheck("Platform", Manifest.Platforms.List, I => I.Name);

      Base.UniqueCheck("Affliction", Manifest.Afflictions.List, I => I.Name);

      Base.UniqueCheck("Punishment", Manifest.Punishments.List, I => I.Name);

      Base.UniqueCheck("Trick", Manifest.Tricks.List, I => I.Name);

      foreach (var Trick in Manifest.Tricks.List)
      {
        if (!Trick.Apply.HasEffects())
          Record($"Trick {Trick.Name} must have apply effects");
      }

      #region Grounds
      Base.UniqueCheck("Ground", Manifest.Grounds.List, I => I.Name);

      foreach (var Ground in Manifest.Grounds.List)
      {
        CheckReactions(Ground.Reactions, () => $"Ground {Ground.Name}");
      }
      #endregion

      #region Volatiles
      Base.UniqueCheck("Volatile", Manifest.Volatiles.List, I => I.Name);

      foreach (var Volatile in Manifest.Volatiles.List)
      {
        CheckReactions(Volatile.Reactions, () => $"Volatile {Volatile.Name}");
      }
      #endregion

      Base.UniqueCheck("Motion", Manifest.Motions.List, I => I.PastName);
      Base.UniqueCheck("Motion", Manifest.Motions.List, I => I.PresentName);

      var MotionSet = Manifest.Items.List.SelectMany(I => I.Uses.Select(U => U.Motion)).Union(
        Manifest.Features.List.SelectMany(F => F.Uses.Select(U => U.Motion))).ToHashSetX();
      MotionSet.Add(Codex.Motions.inscribe); // eventually used?

      foreach (var Motion in Manifest.Motions.List.Except(MotionSet))
        Record($"Motion {Motion.PresentName} is not used");

      #region Spells.
      Base.UniqueCheck("School", Manifest.Schools.List, I => I.Name);
      Base.UniqueCheck("Spell", Manifest.Spells.List, I => I.Name);

      foreach (var Spell in Manifest.Spells.List)
      {
        if (!Spell.GetAdepts().Any(A => A.Qualification == Manifest.Qualifications.First))
          Record($"Spell {Spell.Name} must have at least the {Manifest.Qualifications.First.Name} adept");

        foreach (var Adept in Spell.GetAdepts())
        {
          if (Adept.Cast == null)
            Record($"Spell {Spell.Name} {Adept.Qualification.Name} adept must have a cast semantic");

          if (!Adept.Apply.HasEffects())
            Record($"Spell {Spell.Name} {Adept.Qualification.Name} adept must have apply effects");
        }
      }
      #endregion

      #region Casts.
      Base.UniqueCheck("Strike", Manifest.Strikes.List, I => I.Name);

      Base.UniqueCheck("Beam", Manifest.Beams.List, I => I.Name);

      Base.UniqueCheck("Explosion", Manifest.Explosions.List, I => I.Name);

      Base.UniqueCheck("Engulfment", Manifest.Engulfments.List, I => I.Name);
      #endregion

      #region Shops.
      Base.UniqueCheck("Shop", Manifest.Shops.List, I => I.Name);

      foreach (var Shop in Manifest.Shops.List)
      {
        if (Shop.SellItems.Any(I => I.Grade.Unique))
          Record($"Shop {Shop.Name} should not explicitly sell any unique items");
      }
      #endregion

      #region Heroes.
      foreach (var Hero in Manifest.Heroes.List)
      {
        if (Hero.Pet != null && !Manifest.Companions.List.Any(C => C.Entity == Hero.Pet.Entity))
          Record($"Hero '{Hero.Name}' has a pet '{Hero.Pet.Entity.Name}' that is not a declared companion");
      }
      #endregion

      #region Companions
      foreach (var Companion in Manifest.Companions.List)
      {
        var CheckEntity = Companion.Entity;
        var CheckItem = Companion.Item;

        if (CheckEntity != null && CheckItem == null)
        {
          var ExpectDomestic = CheckEntity.IsDomestic;

          var CheckCount = 0;

          while (CheckEntity != null)
          {
            if (ExpectDomestic && !CheckEntity.IsDomestic)
              Record($"Companion '{CheckEntity.Name}' must be declared as domestic");

            if (CheckEntity.CorpseChance != Chance.Always)
              Record($"Companion '{CheckEntity.Name}' must always drop a corpse");

            if (CheckEntity.Genders.All(G => !G.Recognised))
              Record($"Companion '{CheckEntity.Name}' must have a recognised gender");

            //if (CheckEntity.Level != CheckLevel)
            //  Record($"Companion '{CheckEntity.Name}' was expected to be level {CheckLevel}");

            CheckEntity = CheckEntity.GreaterEntity();
            CheckCount++;
          }

          if (ExpectDomestic && CheckCount != 3)
            Record($"Companion '{Companion.Name}' requires three entities");
        }
        else if (CheckEntity == null && CheckItem != null)
        {
          // TODO: egg.
        }
        else
        {
          Record($"Companion '{Companion.Name}' requires an entity or an item");
        }
      }
      #endregion

      Base.UniqueCheck("Shrine", Manifest.Shrines.List, I => I.Name);

      Base.UniqueCheck("Zoo", Manifest.Zoos.List, I => I.Name);

      Base.UniqueCheck("Qualification.Code", Manifest.Qualifications.List, Q => Q.Code);
      Base.UniqueCheck("Qualification.Name", Manifest.Qualifications.List, Q => Q.Name);

      #region Glyphs.
      Base.UniqueCheck("Glyph", Manifest.Glyphs.List, I => I.Name);

      var UsedGlyphSet = new HashSet<Glyph>();
      UsedGlyphSet.AddRange(Manifest.Entities.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Items.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Items.List.Select(E => E.Storage).ExceptNull().SelectMany(E => new[] { E.LockedGlyph, E.TrappedGlyph, E.BrokenGlyph, E.EmptyGlyph }));
      UsedGlyphSet.AddRange(Manifest.Items.List.Select(E => E.Appearance?.Glyph).ExceptNull());
      UsedGlyphSet.AddRange(Manifest.Devices.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Features.List.SelectMany(E => new[] { E.RegularGlyph, E.BrokenGlyph }));
      UsedGlyphSet.AddRange(Manifest.Gates.List.SelectMany(E => new[] { E.OpenHorizontalGlyph, E.OpenVerticalGlyph, E.ClosedHorizontalGlyph, E.ClosedVerticalGlyph, E.LockedHorizontalGlyph, E.LockedVerticalGlyph, E.TrappedHorizontalGlyph, E.TrappedVerticalGlyph, E.BrokenGlyph }));
      UsedGlyphSet.AddRange(Manifest.Platforms.List.SelectMany(E => new[] { E.VerticalGlyph, E.HorizontalGlyph }));
      UsedGlyphSet.AddRange(Manifest.Afflictions.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Punishments.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Barriers.List.SelectMany(E => E.GetGlyphs()));
      UsedGlyphSet.AddRange(Manifest.Grounds.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Portals.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Blocks.List.SelectMany(E => new[] { E.Glyph, E.Prison?.Glyph }.ExceptNull()));
      UsedGlyphSet.AddRange(Manifest.Beams.List.SelectMany(E => new[] { E.HorizontalGlyph, E.VerticalGlyph, E.BackwardSlantGlyph, E.ForwardSlantGlyph }));
      UsedGlyphSet.AddRange(Manifest.Explosions.List.SelectMany(E => E.GetGlyphs()));
      UsedGlyphSet.AddRange(Manifest.Strikes.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Spells.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Stocks.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Shops.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Shrines.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Engulfments.List.SelectMany(E => E.GetGlyphs()));
      UsedGlyphSet.AddRange(Manifest.Specials.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Classes.List.SelectMany(E => E.Avatars.Select(A => A.Glyph)));
      UsedGlyphSet.AddRange(Manifest.Glyphs.CustomPortraitArray.SelectMany(E => new[] { E.HeroGlyph, E.CompanionGlyph }));
      UsedGlyphSet.AddRange(Manifest.Skills.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Properties.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Elements.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Glyphs.Icons.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Glyphs.Quicks.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Warnings.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Slots.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Appetites.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Standings.List.Select(E => E.Glyph));
      UsedGlyphSet.AddRange(Manifest.Volatiles.List.SelectMany(E => new[] { E.ActiveGlyph, E.HoldGlyph }));
      UsedGlyphSet.Add(Manifest.Glyphs.Interrupt);
      UsedGlyphSet.Add(Manifest.Glyphs.Shroud);

      foreach (var UnusedGlyph in Manifest.Glyphs.List.Except(UsedGlyphSet))
        Record($"Glyph {UnusedGlyph.Name} is not used");
      #endregion

      #region Sonics.
      Base.UniqueCheck("Sonics", Manifest.Sonics.List, I => I.Name);

      var UsedSonicSet = new HashSet<Sonic>();
      UsedSonicSet.AddRange(Manifest.Entities.List.Select(E => E.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Items.List.Select(E => E.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Items.List.Select(E => E.Weapon?.AttackSonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Items.List.Select(E => E.Impact?.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Items.List.SelectMany(E => new[] { E.Storage?.BreakSonic, E.Storage?.LockSonic }).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Items.List.SelectMany(E => E.Uses.Select(U => U.Sonic)).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Shops.List.Select(E => E.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Shrines.List.Select(E => E.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Zoos.List.Select(E => E.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Explosions.List.Select(E => E.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Beams.List.Select(E => E.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Strikes.List.Select(E => E.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Engulfments.List.Select(E => E.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Barriers.List.SelectMany(E => new[] { E.CreateSonic, E.DestroySonic }).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Devices.List.Select(E => E.TriggerSonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Features.List.Select(E => E.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Features.List.SelectMany(E => E.Uses.Select(U => U.Sonic)).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Portals.List.Select(E => E.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Grounds.List.SelectMany(E => new[] { E.Slippery?.Sonic, E.Sunken?.Sonic }).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Punishments.List.Select(E => E.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Afflictions.List.Select(E => E.Sonic).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Blocks.List.Select(E => E.MoveSonic).Union(Manifest.Blocks.List.SelectMany(B => B.Breaks).Select(B => B.Sonic)).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Gates.List.SelectMany(E => new[] { E.OpenSonic, E.CloseSonic, E.BreakSonic }).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Atmospheres.List.SelectMany(E => E.Ambients).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Attributes.List.SelectMany(E => new[] { E.GainSonic, E.LoseSonic }).ExceptNull());
      UsedSonicSet.AddRange(Manifest.Skills.List.SelectMany(E => new[] { E.GainSonic, E.LoseSonic }).ExceptNull());
      UsedSonicSet.Add(Manifest.Kicking.Sonic);
      UsedSonicSet.Add(Manifest.Blinking.Sonic);
      UsedSonicSet.Add(Manifest.Jumping.Sonic);
      UsedSonicSet.Add(Manifest.Sliding.Sonic);
      UsedSonicSet.Add(Manifest.Levelling.GainLevelSonic);
      UsedSonicSet.Add(Manifest.Levelling.LoseLevelSonic);
      UsedSonicSet.Add(Manifest.Praying.GainKarmaSonic);
      UsedSonicSet.Add(Manifest.Praying.LoseKarmaSonic);
      UsedSonicSet.Add(Manifest.Sonics.ShortTap);
      UsedSonicSet.Add(Manifest.Sonics.LongTap);
      UsedSonicSet.Add(Manifest.Sonics.Switch);
      UsedSonicSet.Add(Manifest.Sonics.Realtime);
      UsedSonicSet.Add(Manifest.Sonics.Turnbased);
      UsedSonicSet.Add(Manifest.Sonics.Reroll);
      UsedSonicSet.Add(Manifest.Sonics.Teleport);
      UsedSonicSet.Add(Manifest.Sonics.Polymorph);
      UsedSonicSet.Add(Manifest.Sonics.Fizzle);
      UsedSonicSet.Add(Manifest.Sonics.Death);
      UsedSonicSet.Add(Manifest.Sonics.LowHealth);
      UsedSonicSet.Add(Manifest.Sonics.Warp);
      UsedSonicSet.Add(Manifest.Sonics.Hit);
      UsedSonicSet.Add(Manifest.Sonics.Miss);

      UsedSonicSet.Add(Codex.Sonics.foreboding); // used in AdjustDifficultyEffects
      UsedSonicSet.Add(Codex.Sonics.introduction); // used in modules.
      UsedSonicSet.Add(Codex.Sonics.conclusion); // used in modules.

      foreach (var UnusedSonic in Manifest.Sonics.List.Except(UsedSonicSet))
        Record($"Sonic {UnusedSonic.Name} is not used");
      #endregion
    }

    private void CheckReactions(IReadOnlyList<Reaction> ReactionList, Func<string> Function)
    {
      var Title = Function();

      Base.UniqueCheck(Title + "Reactions", ReactionList, I => I.Element);

      foreach (var Reaction in ReactionList)
      {
        foreach (var Effect in Reaction.Apply.GetEffects())
        {
          if (Effect is HarmEffect && (Effect as HarmEffect).Element == Reaction.Element)
            Record($"{Title} reaction to {Reaction.Element} should not harm with the same element to prevent endless cycles of 'harm'.");
        }
      }
    }
    private void CheckApply(Apply Apply, Func<string> Function)
    {
      foreach (var Effect in Apply.GetEffects())
      {
        var WhenProbability = Effect as WhenProbabilityEffect;

        if (WhenProbability != null && (100 % WhenProbability.GetChecks().Sum(C => C.Factor) != 0))
          Record(Function() + " probability table must be an even division of 100");
      }
    }
    private void CheckLoot(string Title, Loot Loot)
    {
      foreach (var Kit in Loot.Kits)
      {
        if (Kit.Items.Count > 1 && Kit.Items.Any(I => I.Rarity == 0))
          Record(Title + " must not have a kit item with zero rarity");
      }
    }
    private void Record(string Message)
    {
      Base.Record(Message);
    }

    private readonly Sanity Base;
  }

  public sealed class CodexGovernor
  {
    public CodexGovernor()
    {
      this.Base = new Inv.Persist.Governor();

      void RegisterRecord<TRegister, TEditor, TRecord>()
        where TRegister : ManifestRegister<TEditor, TRecord>
        where TEditor : Editor<TRecord>
        where TRecord : ManifestRecord
      {
        Base.Register<TRegister>();
        Base.Register<ManifestAlias<TRecord>>();
      }

      Base.Register<Inv.BitmixFlag>();

      Base.Register<Codex>();
      Base.Register<Manifest>();
      Base.Register<ManifestLevelling>();
      Base.Register<ManifestBlinking>();
      Base.Register<ManifestCasting>();
      Base.Register<ManifestJumping>();
      Base.Register<ManifestKicking>();
      Base.Register<ManifestPraying>();
      Base.Register<ManifestSearching>();
      Base.Register<ManifestSliding>();
      Base.Register<ManifestTelekinesis>();
      Base.Register<ManifestTrading>();
      Base.Register<ManifestTunnelling>();
      Base.Register<ManifestAbolitionReplacement>();
      Base.Register<Icons>();
      Base.Register<Icon>();
      Base.Register<Quicks>();
      Base.Register<Quick>();

      RegisterRecord<ManifestAfflictions, AfflictionEditor, Affliction>();
      RegisterRecord<ManifestAnatomies, AnatomyEditor, Anatomy>();
      RegisterRecord<ManifestAppetites, AppetiteEditor, Appetite>();
      RegisterRecord<ManifestAtmospheres, AtmosphereEditor, Atmosphere>();
      RegisterRecord<ManifestAttackTypes, AttackTypeEditor, AttackType>();
      RegisterRecord<ManifestAttributes, AttributeEditor, Attribute>();
      RegisterRecord<ManifestBarriers, BarrierEditor, Barrier>();
      RegisterRecord<ManifestBeams, BeamEditor, Beam>();
      RegisterRecord<ManifestBlocks, BlockEditor, Block>();
      RegisterRecord<ManifestClasses, ClassEditor, Class>();
      RegisterRecord<ManifestCompanions, CompanionEditor, Companion>();
      RegisterRecord<ManifestDevices, DeviceEditor, Device>();
      RegisterRecord<ManifestDiets, DietEditor, Diet>();
      RegisterRecord<ManifestEggs, EggEditor, Egg>();
      RegisterRecord<ManifestElements, ElementEditor, Element>();
      RegisterRecord<ManifestEncumbrances, EncumbranceEditor, Encumbrance>();
      RegisterRecord<ManifestEngulfments, EngulfmentEditor, Engulfment>();
      RegisterRecord<ManifestEntities, EntityEditor, Entity>();
      RegisterRecord<ManifestEvolutions, EvolutionEditor, Evolution>();
      RegisterRecord<ManifestExplosions, ExplosionEditor, Explosion>();
      RegisterRecord<ManifestFeatures, FeatureEditor, Feature>();
      RegisterRecord<ManifestGates, GateEditor, Gate>();
      RegisterRecord<ManifestGenders, GenderEditor, Gender>();
      RegisterRecord<ManifestGlyphs, GlyphEditor, Glyph>();
      RegisterRecord<ManifestGrades, GradeEditor, Grade>();
      RegisterRecord<ManifestGrounds, GroundEditor, Ground>();
      RegisterRecord<ManifestHeroes, HeroEditor, Hero>();
      RegisterRecord<ManifestHordes, HordeEditor, Horde>();
      RegisterRecord<ManifestItems, ItemEditor, Item>();
      RegisterRecord<ManifestKinds, KindEditor, Kind>();
      RegisterRecord<ManifestMaterials, MaterialEditor, Material>();
      RegisterRecord<ManifestMotions, MotionEditor, Motion>();
      RegisterRecord<ManifestPlatforms, PlatformEditor, Platform>();
      RegisterRecord<ManifestPortals, PortalEditor, Portal>();
      RegisterRecord<ManifestProperties, PropertyEditor, Property>();
      RegisterRecord<ManifestPunishments, PunishmentEditor, Punishment>();
      RegisterRecord<ManifestQualifications, QualificationEditor, Qualification>();
      RegisterRecord<ManifestRaces, RaceEditor, Race>();
      RegisterRecord<ManifestRecipes, RecipeEditor, Recipe>();
      RegisterRecord<ManifestRecruitments, RecruitmentEditor, Recruitment>();
      RegisterRecord<ManifestRumours, RumourEditor, Rumour>();
      RegisterRecord<ManifestSanctities, SanctityEditor, Sanctity>();
      RegisterRecord<ManifestSchools, SchoolEditor, School>();
      RegisterRecord<ManifestServices, ServiceEditor, Service>();
      RegisterRecord<ManifestShops, ShopEditor, Shop>();
      RegisterRecord<ManifestShrines, ShrineEditor, Shrine>();
      RegisterRecord<ManifestSkills, SkillEditor, Skill>();
      RegisterRecord<ManifestSlots, SlotEditor, Slot>();
      RegisterRecord<ManifestSonics, SonicEditor, Sonic>();
      RegisterRecord<ManifestSpecials, SpecialEditor, Special>();
      RegisterRecord<ManifestSpells, SpellEditor, Spell>();
      RegisterRecord<ManifestVolatiles, VolatileEditor, Volatile>();
      RegisterRecord<ManifestStandings, StandingEditor, Standing>();
      RegisterRecord<ManifestStocks, StockEditor, Stock>();
      RegisterRecord<ManifestStrikes, StrikeEditor, Strike>();
      RegisterRecord<ManifestTracks, TrackEditor, Track>();
      RegisterRecord<ManifestTricks, TrickEditor, Trick>();
      RegisterRecord<ManifestWarnings, WarningEditor, Warning>();
      RegisterRecord<ManifestZoos, ZooEditor, Zoo>();

      Base.Register<CodexAfflictions>();
      Base.Register<CodexAnatomies>();
      Base.Register<CodexAppetites>();
      Base.Register<CodexAtmospheres>();
      Base.Register<CodexAttackTypes>();
      Base.Register<CodexAttributes>();
      Base.Register<CodexBarriers>();
      Base.Register<CodexBeams>();
      Base.Register<CodexBlocks>();
      Base.Register<CodexClasses>();
      Base.Register<CodexCompanions>();
      Base.Register<CodexDevices>();
      Base.Register<CodexDiets>();
      Base.Register<CodexEggs>();
      Base.Register<CodexElements>();
      Base.Register<CodexEncumbrances>();
      Base.Register<CodexEngulfments>();
      Base.Register<CodexEntities>();
      Base.Register<CodexEvolutions>();
      Base.Register<CodexExplosions>();
      Base.Register<CodexFeatures>();
      Base.Register<CodexGates>();
      Base.Register<CodexGenders>();
      Base.Register<CodexGlyphs>();
      Base.Register<CodexGrades>();
      Base.Register<CodexGrounds>();
      Base.Register<CodexHeroes>();
      Base.Register<CodexHordes>();
      Base.Register<CodexItems>();
      Base.Register<CodexKinds>();
      Base.Register<CodexMaterials>();
      Base.Register<CodexMotions>();
      Base.Register<CodexPlatforms>();
      Base.Register<CodexPortals>();
      Base.Register<CodexProperties>();
      Base.Register<CodexPunishments>();
      Base.Register<CodexQualifications>();
      Base.Register<CodexRaces>();
      Base.Register<CodexRecipes>();
      Base.Register<CodexRecruitments>();
      Base.Register<CodexRumours>();
      Base.Register<CodexSanctities>();
      Base.Register<CodexSchools>();
      Base.Register<CodexServices>();
      Base.Register<CodexShops>();
      Base.Register<CodexShrines>();
      Base.Register<CodexSkills>();
      Base.Register<CodexSlots>();
      Base.Register<CodexSonics>();
      Base.Register<CodexSpecials>();
      Base.Register<CodexSpells>();
      Base.Register<CodexVolatiles>();
      Base.Register<CodexStandings>();
      Base.Register<CodexStocks>();
      Base.Register<CodexStrikes>();
      Base.Register<CodexTracks>();
      Base.Register<CodexTricks>();
      Base.Register<CodexWarnings>();
      Base.Register<CodexZoos>();

      Base.Register<Ability>();
      Base.Register<Adept>();
      Base.Register<Advancement>();
      Base.Register<Affliction>();
      Base.Register<Anatomy>();
      Base.Register<AnatomySet>();
      Base.Register<Appearance>();
      Base.Register<Appetite>();
      Base.Register<Apply>();
      Base.Register<Armour>();
      Base.Register<AssetFilter>();
      Base.Register<Atmosphere>();
      Base.Register<Attack>();
      Base.Register<AttackType>();
      Base.Register<Attribute>();
      Base.Register<Avatar>();
      Base.Register<Barrier>();
      Base.Register<Beam>();
      Base.Register<Block>();
      Base.Register<Prison>();
      Base.Register<Boon>();
      Base.Register<Break>();
      Base.Register<Cast>();
      Base.Register<Cavalry>();
      Base.Register<Chemistry>();
      Base.Register<Class>();
      Base.Register<Companion>();
      Base.Register<Concealment>();
      Base.Register<CustomPortrait>();
      Base.Register<Defence>();
      Base.Register<DefenceBias>();
      Base.Register<Detonation>();
      Base.Register<Device>();
      Base.Register<Diet>();
      Base.Register<Egg>();
      Base.Register<Element>();
      Base.Register<Enchantment>();
      Base.Register<Encumbrance>();
      Base.Register<Engulf>();
      Base.Register<Engulfment>();
      Base.Register<Entity>();
      Base.Register<Equip>();
      Base.Register<Evolution>();
      Base.Register<Explosion>();
      Base.Register<Feat>();
      Base.Register<Feature>();
      Base.Register<Figure>();
      Base.Register<Form>();
      Base.Register<Gate>();
      Base.Register<Gender>();
      Base.Register<Glyph>().Ignore("Sprite"); // expected to be null.
      Base.Register<Grade>();
      Base.Register<Graduation>();
      Base.Register<Grimoire>();
      Base.Register<Ground>();
      Base.Register<Hero>();
      Base.Register<Horde>();
      Base.Register<Illumination>();
      Base.Register<Impact>();
      Base.Register<Impediment>();
      Base.Register<Item>();
      Base.Register<Kind>();
      Base.Register<Kit>();
      Base.Register<Loot>();
      Base.Register<Material>();
      Base.Register<Minion>();
      Base.Register<Missile>();
      Base.Register<Motion>();
      Base.Register<Pet>();
      Base.Register<Platform>();
      Base.Register<Portal>();
      Base.Register<Prayer>();
      Base.Register<Precept>();
      Base.Register<Property>();
      Base.Register<Punishment>();
      Base.Register<Qualification>();
      Base.Register<Race>();
      Base.Register<Reaction>();
      Base.Register<Recipe>();
      Base.Register<Recruitment>();
      Base.Register<Retaliation>();
      Base.Register<Requirement>();
      Base.Register<Rumour>();
      Base.Register<Sanctity>();
      Base.Register<School>();
      Base.Register<Series>();
      Base.Register<Service>();
      Base.Register<Shop>();
      Base.Register<Shrine>();
      Base.Register<Size>();
      Base.Register<Skill>();
      Base.Register<Slippery>();
      Base.Register<Slot>();
      Base.Register<SlotRow>();
      Base.Register<Sonic>().Ignore("Battery"); // expected to be null.
      Base.Register<Spawn>();
      Base.Register<Special>();
      Base.Register<Spell>();
      Base.Register<Volatile>();
      Base.Register<Standing>();
      Base.Register<Startup>();
      Base.Register<Stock>();
      Base.Register<Storage>();
      Base.Register<Strike>();
      Base.Register<Sunken>();
      Base.Register<Symptom>();
      Base.Register<Tail>();
      Base.Register<Track>().Ignore("Music"); // expected to be null.
      Base.Register<Trick>();
      Base.Register<Use>();
      Base.Register<Utility>();
      Base.Register<Versus>();
      Base.Register<Vision>();
      Base.Register<Warning>();
      Base.Register<Weapon>();
      Base.Register<Workbench>();
      Base.Register<WorkbenchAccident>();
      Base.Register<Zoo>();
      Base.Register<Effect>().AsPolymorph();
      Base.Register<WhenProbabilityCheck>();
      Base.RegisterGold();
      Base.RegisterWeight();
      Base.RegisterEssence();
      Base.RegisterModifier();
      Base.RegisterRange();
      Base.RegisterDelay();
      Base.RegisterSpeed();
      Base.RegisterDice();
      Base.RegisterChance();
    }

    public void Validate()
    {
      Base.Validate<Codex>();
    }
    public void Save(Codex Codex, System.IO.Stream Stream)
    {
      var SW = new Stopwatch();
      SW.Measure("Save Codex", () => Base.Save(Codex, Stream));
    }
    public void Load(Codex Codex, System.IO.Stream Stream)
    {
      var SW = new Stopwatch();
      SW.Measure("Load Codex", () => Base.Load(Codex, Stream));
    }

    private readonly Inv.Persist.Governor Base;
  }
}