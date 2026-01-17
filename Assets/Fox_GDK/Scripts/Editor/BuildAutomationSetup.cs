using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAutomationSetup : EditorWindow
{
    private string buildMessage = "Build completed successfully";

    [MenuItem("FoxTools/Build Automation Window")]
    public static void ShowWindow()
    {
        GetWindow<BuildAutomationSetup>("Build Automation");
    }

    void OnGUI()
    {
        GUILayout.Label("Build Automation Setup", EditorStyles.boldLabel);
        GUILayout.Space(10);

        GUILayout.Label("Build Message:");
        buildMessage = EditorGUILayout.TextField(buildMessage);
        GUILayout.Space(10);

        if (GUILayout.Button("Setup Build Automation", GUILayout.Height(30)))
        {
            SetupBuildAutomation();
        }

        GUILayout.Space(10);
        GUILayout.Label("This will create/update GitHub Actions workflows with your custom build message.", EditorStyles.helpBox);
    }

    private void SetupBuildAutomation()
    {
        string projectRoot = Directory.GetParent(Application.dataPath).FullName;
        string githubFolder = Path.Combine(projectRoot, ".github");
        string workflowsFolder = Path.Combine(githubFolder, "workflows");

        if (!Directory.Exists(githubFolder))
        {
            Directory.CreateDirectory(githubFolder);
            Debug.Log("Created .github folder");
        }

        if (!Directory.Exists(workflowsFolder))
        {
            Directory.CreateDirectory(workflowsFolder);
            Debug.Log("Created .github/workflows folder");
        }

        string testBuildPath = Path.Combine(workflowsFolder, "test-build.yml");
        string submissionBuildPath = Path.Combine(workflowsFolder, "submission-build.yml");

        string testBuildContent = $@"name: Test Build - {buildMessage}

on:
  push:
    branches: [ main, develop ]
  pull_request:
    branches: [ main ]

jobs:
  test-build:
    runs-on: ubuntu-latest
    strategy:
      fail-fast: false
      matrix:
        targetPlatform:
          - Android
          - WebGL
    
    steps:
    - uses: actions/checkout@v4
      with:
        fetch-depth: 0
        lfs: true
    
    - name: Free Disk Space
      uses: jlumbroso/free-disk-space@main
      with:
        tool-cache: false
        android: true
        dotnet: true
        haskell: true
        large-packages: true
        swap-storage: true
    
    - uses: actions/cache@v3
      with:
        path: Library
        key: Library-${{{{ hashFiles('Assets/**', 'Packages/**', 'ProjectSettings/**') }}}}
        restore-keys: |
          Library-
    
    - name: Setup Android SDK
      if: matrix.targetPlatform == 'Android'
      uses: android-actions/setup-android@v3
    
    - uses: game-ci/unity-builder@v4
      env:
        UNITY_LICENSE: ${{{{ secrets.UNITY_LICENSE }}}}
        UNITY_EMAIL: ${{{{ secrets.UNITY_EMAIL }}}}
        UNITY_PASSWORD: ${{{{ secrets.UNITY_PASSWORD }}}}
      with:
        targetPlatform: ${{{{ matrix.targetPlatform }}}}
        buildMethod: UnityBuilderAction.BuildScript.Build
        unityVersion: 2022.3.10f1
    
    - uses: actions/upload-artifact@v4
      with:
        name: test-build-${{{{ matrix.targetPlatform }}}}
        path: build/${{{{ matrix.targetPlatform }}}}";

        string submissionBuildContent = $@"name: Submission Build - {buildMessage}

on:
  workflow_dispatch:
    inputs:
      version:
        description: 'Build version'
        required: true
        default: '1.0.0'

jobs:
  submission-build:
    runs-on: ubuntu-latest
    strategy:
      fail-fast: false
      matrix:
        targetPlatform:
          - Android
          - WebGL
    
    steps:
    - uses: actions/checkout@v4
      with:
        fetch-depth: 0
        lfs: true
    
    - name: Free Disk Space
      uses: jlumbroso/free-disk-space@main
      with:
        tool-cache: false
        android: true
        dotnet: true
        haskell: true
        large-packages: true
        swap-storage: true
    
    - uses: actions/cache@v3
      with:
        path: Library
        key: Library-${{{{ hashFiles('Assets/**', 'Packages/**', 'ProjectSettings/**') }}}}
        restore-keys: |
          Library-
    
    - name: Setup Android SDK
      if: matrix.targetPlatform == 'Android'
      uses: android-actions/setup-android@v3
    
    - uses: game-ci/unity-builder@v4
      env:
        UNITY_LICENSE: ${{{{ secrets.UNITY_LICENSE }}}}
        UNITY_EMAIL: ${{{{ secrets.UNITY_EMAIL }}}}
        UNITY_PASSWORD: ${{{{ secrets.UNITY_PASSWORD }}}}
      with:
        targetPlatform: ${{{{ matrix.targetPlatform }}}}
        buildMethod: UnityBuilderAction.BuildScript.Build
        versioning: Custom
        version: ${{{{ github.event.inputs.version }}}}
        unityVersion: 2022.3.10f1
    
    - uses: actions/upload-artifact@v4
      with:
        name: submission-build-${{{{ matrix.targetPlatform }}}}-${{{{ github.event.inputs.version }}}}
        path: build/${{{{ matrix.targetPlatform }}}}";

        File.WriteAllText(testBuildPath, testBuildContent);
        File.WriteAllText(submissionBuildPath, submissionBuildContent);

        Debug.Log($"Updated workflows with build message: {buildMessage}");

        EditorUtility.DisplayDialog("Build Automation Setup",
            $"GitHub Actions workflows have been created/updated successfully!\n\n" +
            $"Build Message: {buildMessage}\n\n" +
            "Files created:\n" +
            "• .github/workflows/test-build.yml\n" +
            "• .github/workflows/submission-build.yml\n\n" +
            "Don't forget to add Unity secrets to your GitHub repository:\n" +
            "• UNITY_LICENSE\n" +
            "• UNITY_EMAIL\n" +
            "• UNITY_PASSWORD", "OK");
    }
}