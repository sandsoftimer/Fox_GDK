#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildAnalyzerWindow : EditorWindow, IPostprocessBuildWithReport
{
    public int callbackOrder => 0;
    private Vector2 scrollPosition;
    private BuildData buildData;
    private List<AssetInfo> usedAssets = new List<AssetInfo>();
    private List<AssetInfo> unusedAssets = new List<AssetInfo>();
    private string searchFilter = "";
    private int selectedTab;
    private SortType sortType = SortType.SizeOnDiskDesc;
    private bool sortAscending;
    private static string SavePath => Path.Combine(Application.dataPath, "../Library/BuildAnalyzerData.json");

    private enum SortType { Name, SizeInBuildDesc, SizeOnDiskDesc, Path, Type }

    [Serializable]
    private class AssetInfo
    {
        public string path, name, type;
        public ulong sizeInBuild;
        public long sizeOnDisk;
        public bool isUsed;
    }

    [Serializable]
    private class BuildData
    {
        public string platform, buildResult;
        public ulong totalSize;
        public double totalTime;
        public List<AssetInfo> buildFiles = new List<AssetInfo>();
        public List<AssetInfo> allAssets = new List<AssetInfo>();
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        SaveBuildReportStatic(report);
        EditorApplication.delayCall += ShowWindow;
    }

    [MenuItem("Window/Build Analyzer")]
    public static void ShowWindow()
    {
        var window = GetWindow<BuildAnalyzerWindow>("Build Analyzer");
        window.minSize = new Vector2(800, 600);
        window.LoadBuildData();
        window.Show();
    }

    private static void SaveBuildReportStatic(BuildReport report)
    {
        var data = new BuildData
        {
            platform = report.summary.platform.ToString(),
            totalSize = report.summary.totalSize,
            totalTime = report.summary.totalTime.TotalSeconds,
            buildResult = report.summary.result.ToString()
        };

        foreach (var file in report.GetFiles().Where(f => !string.IsNullOrEmpty(f.path)))
        {
            var fileInfo = new FileInfo(file.path);
            data.buildFiles.Add(new AssetInfo
            {
                path = file.path,
                name = Path.GetFileName(file.path),
                sizeInBuild = file.size,
                sizeOnDisk = fileInfo.Exists ? fileInfo.Length : 0,
                type = Path.GetExtension(file.path),
                isUsed = true
            });
        }

        var assetSizes = ParseEditorLogForAssetSizes();
        var usedPaths = new HashSet<string>();
        
        foreach (var scene in EditorBuildSettings.scenes.Where(s => s.enabled))
        {
            usedPaths.Add(scene.path);
            foreach (var dep in AssetDatabase.GetDependencies(scene.path, true))
                usedPaths.Add(dep);
        }

        foreach (var assetPath in AssetDatabase.GetAllAssetPaths().Where(p => p.StartsWith("Assets/") && !AssetDatabase.IsValidFolder(p)))
        {
            var fileInfo = new FileInfo(assetPath);
            if (fileInfo.Exists)
            {
                data.allAssets.Add(new AssetInfo
                {
                    path = assetPath,
                    name = Path.GetFileName(assetPath),
                    sizeInBuild = assetSizes.TryGetValue(assetPath, out var size) ? size : 0,
                    sizeOnDisk = fileInfo.Length,
                    type = Path.GetExtension(assetPath),
                    isUsed = usedPaths.Contains(assetPath)
                });
            }
        }

        File.WriteAllText(SavePath, JsonUtility.ToJson(data));
    }

    private static Dictionary<string, ulong> ParseEditorLogForAssetSizes()
    {
        var sizes = new Dictionary<string, ulong>();
        try
        {
            var logPath = Application.platform == RuntimePlatform.WindowsEditor
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Unity/Editor/Editor.log")
                : Application.platform == RuntimePlatform.OSXEditor
                ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Library/Logs/Unity/Editor.log")
                : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".config/unity3d/Editor.log");

            if (!File.Exists(logPath)) return sizes;

            var lines = File.ReadAllLines(logPath);
            bool inBuildReport = false;
            
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                var line = lines[i];
                if (line.Contains("Used Assets and files from the Resources folder") || line.Contains("Used Assets, sorted by uncompressed size"))
                {
                    inBuildReport = true;
                    continue;
                }
                
                if (inBuildReport)
                {
                    if (line.Contains("---") || line.Contains("Mono dependencies")) break;
                    
                    var parts = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2)
                    {
                        var pathPart = parts[^1].Trim();
                        if (pathPart.Contains("%"))
                            pathPart = pathPart.Split(' ', StringSplitOptions.RemoveEmptyEntries)[^1];
                        
                        if (pathPart.StartsWith("Assets/"))
                        {
                            var size = ParseSize(parts[0].Trim());
                            if (size > 0) sizes[pathPart] = size;
                        }
                    }
                }
            }
        }
        catch { }
        return sizes;
    }

    private static ulong ParseSize(string sizeStr)
    {
        try
        {
            sizeStr = sizeStr.ToLower().Trim();
            var multiplier = sizeStr.Contains("gb") ? 1073741824UL : sizeStr.Contains("mb") ? 1048576UL : sizeStr.Contains("kb") ? 1024UL : 1UL;
            var value = double.Parse(sizeStr.Replace("gb", "").Replace("mb", "").Replace("kb", "").Replace("b", "").Trim());
            return (ulong)(value * multiplier);
        }
        catch { return 0; }
    }

    private void LoadBuildData()
    {
        if (!File.Exists(SavePath)) return;
        try
        {
            buildData = JsonUtility.FromJson<BuildData>(File.ReadAllText(SavePath));
            usedAssets = buildData.allAssets.Where(a => a.isUsed).ToList();
            unusedAssets = buildData.allAssets.Where(a => !a.isUsed).ToList();
        }
        catch { buildData = null; }
    }

    private void SortAssets(List<AssetInfo> list)
    {
        if (list == null) return;
        list.Sort((a, b) => sortType switch
        {
            SortType.Name => sortAscending ? a.name.CompareTo(b.name) : b.name.CompareTo(a.name),
            SortType.SizeInBuildDesc => sortAscending ? a.sizeInBuild.CompareTo(b.sizeInBuild) : b.sizeInBuild.CompareTo(a.sizeInBuild),
            SortType.SizeOnDiskDesc => sortAscending ? a.sizeOnDisk.CompareTo(b.sizeOnDisk) : b.sizeOnDisk.CompareTo(a.sizeOnDisk),
            SortType.Path => sortAscending ? a.path.CompareTo(b.path) : b.path.CompareTo(a.path),
            SortType.Type => sortAscending ? a.type.CompareTo(b.type) : b.type.CompareTo(a.type),
            _ => 0
        });
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Build Analyzer", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Reload", GUILayout.Width(60))) LoadBuildData();
        if (GUILayout.Button("Clear", GUILayout.Width(60)))
        {
            if (EditorUtility.DisplayDialog("Clear Build Data", "Clear all build data?", "Yes", "No"))
            {
                buildData = null;
                usedAssets.Clear();
                unusedAssets.Clear();
                Repaint();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(5);

        if (buildData == null)
        {
            EditorGUILayout.HelpBox("No build report available. Build the project first.", MessageType.Info);
            return;
        }

        selectedTab = GUILayout.Toolbar(selectedTab, new[] { 
            "Summary", $"Used ({usedAssets.Count})", $"Unused ({unusedAssets.Count})", $"Build ({buildData.buildFiles.Count})" 
        });
        EditorGUILayout.Space(10);

        switch (selectedTab)
        {
            case 0: DrawSummary(usedAssets, unusedAssets); break;
            case 1: DrawAssetList(usedAssets, true); break;
            case 2: DrawAssetList(unusedAssets, true); break;
            case 3: DrawAssetList(buildData.buildFiles, false); break;
        }
    }

    private void DrawSummary(List<AssetInfo> usedAssets, List<AssetInfo> unusedAssets)
    {
        EditorGUILayout.LabelField("Build Summary", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField($"Platform: {buildData.platform}");
        EditorGUILayout.LabelField($"Build Result: {buildData.buildResult}");
        EditorGUILayout.LabelField($"Total Size: {FormatBytes(buildData.totalSize)}");
        EditorGUILayout.LabelField($"Build Time: {buildData.totalTime:F2}s");
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Optimization Insights", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        var unusedSize = (ulong)unusedAssets.Sum(a => a.sizeOnDisk);
        EditorGUILayout.LabelField($"Unused Assets: {unusedAssets.Count} files ({FormatBytes(unusedSize)})");
        EditorGUILayout.LabelField($"Used Assets: {usedAssets.Count} files");
        
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Top 5 Largest Assets in Build:", EditorStyles.boldLabel);
        foreach (var asset in usedAssets.OrderByDescending(a => a.sizeInBuild).Take(5).Where(a => a.sizeInBuild > 0))
            EditorGUILayout.LabelField($"  • {asset.name}: {FormatBytes(asset.sizeInBuild)}");
        
        EditorGUILayout.Space(5);
        var typeGroups = usedAssets.Where(a => a.sizeInBuild > 0).GroupBy(a => a.type).OrderByDescending(g => g.Sum(a => (long)a.sizeInBuild)).Take(5);
        EditorGUILayout.LabelField("Size by Type:", EditorStyles.boldLabel);
        foreach (var group in typeGroups)
        {
            var groupSize = (ulong)group.Sum(a => (long)a.sizeInBuild);
            EditorGUILayout.LabelField($"  • {group.Key}: {FormatBytes(groupSize)} ({group.Count()} files)");
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawAssetList(List<AssetInfo> assets, bool showPing)
    {
        EditorGUILayout.BeginHorizontal();
        searchFilter = EditorGUILayout.TextField("Search:", searchFilter);
        if (GUILayout.Button("Clear", GUILayout.Width(60))) searchFilter = "";
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal("box");
        if (showPing) GUILayout.Label("", GUILayout.Width(30));
        
        if (GUILayout.Button($"Name {GetSortIcon(SortType.Name)}", EditorStyles.toolbarButton, GUILayout.Width(200)))
            ToggleSort(SortType.Name, assets);
        if (GUILayout.Button($"Type {GetSortIcon(SortType.Type)}", EditorStyles.toolbarButton, GUILayout.Width(80)))
            ToggleSort(SortType.Type, assets);
        if (GUILayout.Button($"Disk {GetSortIcon(SortType.SizeOnDiskDesc)}", EditorStyles.toolbarButton, GUILayout.Width(100)))
            ToggleSort(SortType.SizeOnDiskDesc, assets);
        if (GUILayout.Button($"Build {GetSortIcon(SortType.SizeInBuildDesc)}", EditorStyles.toolbarButton, GUILayout.Width(100)))
            ToggleSort(SortType.SizeInBuildDesc, assets);
        if (GUILayout.Button($"Path {GetSortIcon(SortType.Path)}", EditorStyles.toolbarButton))
            ToggleSort(SortType.Path, assets);
        EditorGUILayout.EndHorizontal();

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        var filtered = string.IsNullOrEmpty(searchFilter) ? assets : 
            assets.Where(a => a.name.IndexOf(searchFilter, StringComparison.OrdinalIgnoreCase) >= 0 || 
                             a.path.IndexOf(searchFilter, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

        foreach (var asset in filtered)
        {
            EditorGUILayout.BeginHorizontal();
            if (showPing && GUILayout.Button("→", GUILayout.Width(30)))
            {
                var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(asset.path);
                if (obj != null) { Selection.activeObject = obj; EditorGUIUtility.PingObject(obj); }
            }
            EditorGUILayout.LabelField(asset.name, GUILayout.Width(200));
            EditorGUILayout.LabelField(asset.type, GUILayout.Width(80));
            EditorGUILayout.LabelField(FormatBytes((ulong)asset.sizeOnDisk), GUILayout.Width(100));
            EditorGUILayout.LabelField(FormatBytes(asset.sizeInBuild), GUILayout.Width(100));
            EditorGUILayout.LabelField(asset.path, EditorStyles.miniLabel);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }

    private void ToggleSort(SortType type, List<AssetInfo> list)
    {
        sortAscending = sortType == type && !sortAscending;
        sortType = type;
        SortAssets(list);
    }

    private string GetSortIcon(SortType type) => sortType == type ? (sortAscending ? "▲" : "▼") : "";
    private string FormatBytes(ulong bytes)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = bytes;
        int order = 0;
        while (len >= 1024 && order < sizes.Length - 1) { order++; len /= 1024; }
        return $"{len:0.##} {sizes[order]}";
    }
}
#endif