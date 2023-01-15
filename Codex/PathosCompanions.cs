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

      AddCompanion("dog", Codex.Entities.little_dog, null);
      AddCompanion("cat", Codex.Entities.kitten, null);
      AddCompanion("horse", Codex.Entities.pony, null);
      AddCompanion("bird", Codex.Entities.fledgling_raven, null);
      AddCompanion("monkey", Codex.Entities.monkey, null);
      AddCompanion("rat", Codex.Entities.black_rat, null);
      AddCompanion("egg", null, Codex.Items.egg);
      AddCompanion("imp", Codex.Entities.imp, null);
      AddCompanion("ghoul", Codex.Entities.ghast, null);

      CodexRecruiter.Enrol(() =>
      {
        Register.SetEggs(Codex.Manifest.Eggs.List.Where(E => E.Hatchling.Kind == Codex.Kinds.dragon).ToArray()); // permitted egg companions.
      });
    }
#endif
  }
}