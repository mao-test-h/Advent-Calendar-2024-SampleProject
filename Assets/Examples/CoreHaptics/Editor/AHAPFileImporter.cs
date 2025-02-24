using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Examples.CoreHaptics.Editor
{
    /// <summary>
    /// AHAP ファイルを TextAsset としてインポートするためのクラス
    /// </summary>
    [ScriptedImporter(1, "ahap")]
    internal sealed class AhapFileImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var data = File.ReadAllText(ctx.assetPath);
            var ahapTextAsset = new TextAsset(data);
            ctx.AddObjectToAsset("main", ahapTextAsset);
            ctx.SetMainObject(ahapTextAsset);
        }
    }
}
