using System;
using System.Collections.Generic;
using System.Text;
using Inv.Support;

namespace Pathos
{
  internal sealed class ModuleResources
  {
    static ModuleResources()
    {
      ResourceAssembly = System.Reflection.Assembly.GetExecutingAssembly();
    }

    public static Inv.Binary LoadQuest(string Name) => ResourceAssembly.ExtractResourceBinary($"PathosOfficial.Resources.Quests.{Name}.Quest");
    public static string LoadSpecial(string Name) => ResourceAssembly.ExtractResourceString($"PathosOfficial.Resources.Specials.{Name}.txt");

    private static readonly System.Reflection.Assembly ResourceAssembly;
  }
}
