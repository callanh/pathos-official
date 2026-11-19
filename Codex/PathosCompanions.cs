using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexCompanions : CodexPage<ManifestCompanions, CompanionEditor, Companion>
  {
    private CodexCompanions() { }
#if MASTER_CODEX
    internal CodexCompanions(Codex Codex)
      : base(Codex.Manifest.Companions)
    {
      Companion AddCompanion(string Name, Entity Entity, Item Item)
      {
        return Register.Add(C =>
        {
          C.Name = Name;
          C.Entity = Entity;
          C.Item = Item;
        });
      }

      dog = AddCompanion("dog", Codex.Entities.little_dog, null);
      cat = AddCompanion("cat", Codex.Entities.kitten, null);
      horse = AddCompanion("horse", Codex.Entities.pony, null);
      bird = AddCompanion("bird", Codex.Entities.fledgling_raven, null);
      monkey = AddCompanion("monkey", Codex.Entities.monkey, null);
      rat = AddCompanion("rat", Codex.Entities.black_rat, null);
      egg = AddCompanion("egg", null, Codex.Items.egg);
      imp = AddCompanion("imp", Codex.Entities.imp, null);
      ghoul = AddCompanion("ghoul", Codex.Entities.ghast, null);

      CodexRecruiter.Enrol(() =>
      {
        Register.SetEggs(Codex.Manifest.Eggs.List.Where(E => E.Hatchling.Kind == Codex.Kinds.dragon).ToArray()); // permitted egg companions.
      });
    }
#endif

    public Companion dog { get; }
    public Companion cat { get; }
    public Companion horse { get; }
    public Companion bird { get; }
    public Companion monkey { get; }
    public Companion rat { get; }
    public Companion egg { get; }
    public Companion imp { get; }
    public Companion ghoul { get; }
  }
}
