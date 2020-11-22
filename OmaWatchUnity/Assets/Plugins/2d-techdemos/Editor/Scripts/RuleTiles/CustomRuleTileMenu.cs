﻿using UnityEditor;

namespace Assets.Plugins.Editor.Scripts.RuleTiles
{
    static class CustomRuleTileMenu
    {
        [MenuItem("Assets/Create/Custom Rule Tile Script", false, 89)]
        static void CreateCustomRuleTile()
        {
            CreateScriptAsset("Assets/Tilemap/Rule Tiles/Scripts/ScriptTemplates/NewCustomRuleTile.cs.txt", "NewCustomRuleTile.cs");
        }

        static void CreateScriptAsset(string templatePath, string destName)
        {
            typeof(ProjectWindowUtil)
                .GetMethod("CreateScriptAsset", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic)
                .Invoke(null, new object[] { templatePath, destName });
        }
    }
}