using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace MackySoft.PackageTools.Editor
{
    public static class UnityPackageExporter
    {
        // The name of the unitypackage to output.
        private const string k_PackageName = "SerializeReference-Extensions";

        // The path to the package under the `Assets/` folder.
        private const string k_PackagePath = "MackySoft";

        // Path to export to.
        private const string k_ExportPath = "Build";

        private const string k_SearchPattern = "*";
        private const string k_PackageToolsFolderName = "PackageTools";
        private const string k_ResourcesFolderName = "Resources";

        [MenuItem("Tools/SerializeReference Extensions/Export Package")]
        public static void Export ()
        {
            ExportPackage($"{k_ExportPath}/{k_PackageName}.unitypackage");
        }


        public static string ExportPackage (string exportPath)
        {
            // Ensure export path.
            DirectoryInfo dir = new FileInfo(exportPath).Directory;
            if (dir != null && !dir.Exists)
            {
                dir.Create();
            }

            // Export
            AssetDatabase.ExportPackage(
                GetAssetPaths(),
                exportPath,
                ExportPackageOptions.Default
            );

            return Path.GetFullPath(exportPath);
        }

        public static string[] GetAssetPaths ()
        {
            string path = Path.Combine(Application.dataPath, k_PackagePath);
            string[] assets = Directory.EnumerateFiles(path, k_SearchPattern, SearchOption.AllDirectories)
                .Where(x => !x.Contains(k_PackageToolsFolderName) && !x.Contains(k_ResourcesFolderName))
                .Select(x => "Assets" + x.Replace(Application.dataPath, "").Replace(@"\", "/"))
                .ToArray();
            return assets;
        }
    }
}