using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  public sealed class CodexExplosions : CodexPage<ManifestExplosions, ExplosionEditor, Explosion>
  {
    private CodexExplosions() { }
#if MASTER_CODEX
    internal CodexExplosions(Codex Codex)
      : base(Codex.Manifest.Explosions)
    {
      var Sonics = Codex.Sonics;

      Explosion AddExplosion(string Name, Sonic Sonic)
      {
        return Register.Add(E =>
        {
          E.Name = Name;
          E.Sonic = Sonic;

          foreach (var Area in Inv.Support.EnumHelper.GetEnumerable<ExplosionArea>())
            E.SetGlyph(Area, Codex.Glyphs.GetGlyph(Name + " explosion " + Area.ToString().PascalCaseToTitleCase().ToLowerInvariant()));
        });
      }

      acid = AddExplosion("acid", Sonics.water_splash);
      dark = AddExplosion("dark", Sonics.magic); // sfx
      death = AddExplosion("death", Sonics.magic); // sfx
      electric = AddExplosion("electric", Sonics.electricity);
      fiery = AddExplosion("fiery", Sonics.burn);
      frosty = AddExplosion("frosty", Sonics.freeze);
      light = AddExplosion("light", Sonics.magic); // sfx
      magical = AddExplosion("magical", Sonics.magic);
      muddy = AddExplosion("muddy", Sonics.water_crash);
      watery = AddExplosion("watery", Sonics.water_crash);
    }
#endif

    public readonly Explosion acid;
    public readonly Explosion dark;
    public readonly Explosion death;
    public readonly Explosion electric;
    public readonly Explosion fiery;
    public readonly Explosion frosty;
    public readonly Explosion light;
    public readonly Explosion magical;
    public readonly Explosion muddy;
    public readonly Explosion watery;
  }
}
