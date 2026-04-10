﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Inv.Support;

namespace Pathos
{
  internal static class Resources
  {
    static Resources()
    {
      ResourceAssembly = System.Reflection.Assembly.GetExecutingAssembly();
    }

    public static Inv.Binary LoadQuest(string Name) => ResourceAssembly.ExtractResourceBinary($"PathosOfficial.Resources.Quests.{Name}.Quest");
    public static string LoadSpecial(string Name) => ResourceAssembly.ExtractResourceString($"PathosOfficial.Resources.Specials.{Name}.txt");

    private static readonly System.Reflection.Assembly ResourceAssembly;
  }
}