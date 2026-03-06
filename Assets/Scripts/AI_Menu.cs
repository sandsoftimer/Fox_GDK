#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class AI_Menu : EditorWindow
{
    private string eventName = "";
    private string eventParameters = "";
    private List<EventInfo> existingEvents = new List<EventInfo>();
    private Vector2 scrollPosition;
    private EventInfo editingEvent = null;
    private string newEventName = "";
    private string newEventParameters = "";

    [System.Serializable]
    public class EventInfo
    {
        public string name;
        public string parameters;
        public string fullDeclaration;

        public EventInfo(string name, string parameters, string fullDeclaration)
        {
            this.name = name;
            this.parameters = parameters;
            this.fullDeclaration = fullDeclaration;
        }
    }

    [MenuItem("Tools/AI Event Generator")]
    public static void ShowWindow()
    {
        GetWindow<AI_Menu>("AI Event Generator");
    }

    void OnEnable()
    {
        RefreshEventList();
    }

    void OnGUI()
    {
        GUILayout.Label("Event Generator", EditorStyles.boldLabel);

        GUILayout.Space(10);
        GUILayout.Label("Create New Event:", EditorStyles.boldLabel);
        eventName = EditorGUILayout.TextField("Event Name:", eventName);
        eventParameters = EditorGUILayout.TextField("Parameters (e.g., int, bool):", eventParameters);
        GUILayout.Label("Leave parameters empty for Action, or use types like: int, bool, string, GameObject", EditorStyles.helpBox);

        if (GUILayout.Button("Create Event"))
        {
            if (!string.IsNullOrEmpty(eventName))
            {
                CreateEvent(eventName, eventParameters);
                eventName = "";
                eventParameters = "";
            }
        }

        GUILayout.Space(20);
        GUILayout.Label("Existing Events:", EditorStyles.boldLabel);

        if (GUILayout.Button("Refresh List"))
        {
            RefreshEventList();
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = 0; i < existingEvents.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();

            if (editingEvent == existingEvents[i])
            {
                EditorGUILayout.BeginVertical();
                newEventName = EditorGUILayout.TextField("Name:", newEventName);
                newEventParameters = EditorGUILayout.TextField("Parameters:", newEventParameters);
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUILayout.Width(120));
                if (GUILayout.Button("Save"))
                {
                    EditEvent(existingEvents[i], newEventName, newEventParameters);
                    editingEvent = null;
                }
                if (GUILayout.Button("Cancel"))
                {
                    editingEvent = null;
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Name:", existingEvents[i].name, EditorStyles.boldLabel);
                if (!string.IsNullOrEmpty(existingEvents[i].parameters))
                    EditorGUILayout.LabelField("Parameters:", existingEvents[i].parameters);
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUILayout.Width(120));
                if (GUILayout.Button("Edit"))
                {
                    editingEvent = existingEvents[i];
                    newEventName = existingEvents[i].name.Replace("On", "");
                    newEventParameters = existingEvents[i].parameters;
                }
                if (GUILayout.Button("Delete"))
                {
                    if (EditorUtility.DisplayDialog("Delete Event", $"Delete {existingEvents[i].name}?", "Yes", "No"))
                    {
                        DeleteEvent(existingEvents[i]);
                    }
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
    }

    void RefreshEventList()
    {
        existingEvents.Clear();
        string gameManagerPath = "Assets/Scripts/GameManager.cs";
        if (File.Exists(gameManagerPath))
        {
            string content = File.ReadAllText(gameManagerPath);
            var matches = Regex.Matches(content, @"public static Action(?:<([^>]+)>)? (On\w+);?");
            foreach (Match match in matches)
            {
                string name = match.Groups[2].Value;
                string parameters = match.Groups[1].Value;
                string fullDeclaration = match.Value;
                existingEvents.Add(new EventInfo(name, parameters, fullDeclaration));
            }
        }
    }

    void CreateEvent(string name, string parameters)
    {
        string gameManagerPath = "Assets/Scripts/GameManager.cs";
        string content = File.ReadAllText(gameManagerPath);

        string actionParameters = parameters;
        if (!string.IsNullOrEmpty(parameters))
        {
            CreateUnknownParameterTypes(parameters);
        }

        string eventDeclaration;
        if (string.IsNullOrEmpty(actionParameters))
        {
            eventDeclaration = $"    public static Action On{name};";
        }
        else
        {
            eventDeclaration = $"    public static Action<{actionParameters}> On{name};";
        }

        if (content.Contains($"On{name}"))
        {
            Debug.LogWarning($"Event On{name} already exists!");
            return;
        }

        int insertIndex = content.IndexOf("public static Action<RewardBackType> OnNotEnoughBooster;");
        if (insertIndex != -1)
        {
            insertIndex = content.IndexOf("\n", insertIndex) + 1;
            content = content.Insert(insertIndex, eventDeclaration + "\n");

            string triggerFunction = CreateTriggerFunction($"On{name}", actionParameters);
            int functionInsertIndex = content.IndexOf("    public bool CAN_I_SHOOT");
            if (functionInsertIndex != -1)
            {
                content = content.Insert(functionInsertIndex, triggerFunction + "\n\n");
            }

            File.WriteAllText(gameManagerPath, content);
            AssetDatabase.Refresh();

            AddToBaseGameBehaviour($"On{name}", actionParameters);
            RefreshEventList();
            Debug.Log($"Event On{name} created successfully!");
        }
    }

    void EditEvent(EventInfo oldEvent, string newName, string newParameters)
    {
        string gameManagerPath = "Assets/Scripts/GameManager.cs";
        string content = File.ReadAllText(gameManagerPath);

        string newEventName = newName.StartsWith("On") ? newName : $"On{newName}";

        if (newEventName != oldEvent.name && existingEvents.Any(e => e.name == newEventName))
        {
            EditorUtility.DisplayDialog("Not Possible", $"Event {newEventName} already exists!", "OK");
            return;
        }

        if (!string.IsNullOrEmpty(newParameters))
        {
            CreateUnknownParameterTypes(newParameters);
        }

        string newDeclaration;
        if (string.IsNullOrEmpty(newParameters))
        {
            newDeclaration = $"public static Action {newEventName};";
        }
        else
        {
            newDeclaration = $"public static Action<{newParameters}> {newEventName};";
        }

        content = content.Replace(oldEvent.fullDeclaration, newDeclaration);

        string oldFunctionName = oldEvent.name.Replace("On", "");
        string newFunctionName = newEventName.Replace("On", "");

        string oldFunctionPattern = $@"    public void {Regex.Escape(oldFunctionName)}\([^\)]*\)\s*\{{[^\}}]*\}}";
        string newTriggerFunction = CreateTriggerFunction(newEventName, newParameters);

        content = Regex.Replace(content, oldFunctionPattern, newTriggerFunction, RegexOptions.Singleline);

        File.WriteAllText(gameManagerPath, content);
        AssetDatabase.Refresh();

        RemoveFromBaseGameBehaviour(oldEvent.name);
        AddToBaseGameBehaviour(newEventName, newParameters);

        RefreshEventList();
        Debug.Log($"Event updated to {newEventName}");
    }

    void DeleteEvent(EventInfo eventInfo)
    {
        string gameManagerPath = "Assets/Scripts/GameManager.cs";
        string content = File.ReadAllText(gameManagerPath);

        string eventPattern = Regex.Escape(eventInfo.fullDeclaration) + @"(\r?\n)";
        content = Regex.Replace(content, eventPattern, "");

        string functionName = eventInfo.name.Replace("On", "");
        string functionPattern = $@"    public void {Regex.Escape(functionName)}\([^\)]*\)[^{{]*\{{[^}}]*\}}(\r?\n)";
        content = Regex.Replace(content, functionPattern, "", RegexOptions.Singleline);

        File.WriteAllText(gameManagerPath, content);
        AssetDatabase.Refresh();

        RemoveFromBaseGameBehaviour(eventInfo.name);
        RefreshEventList();
        Debug.Log($"Event {eventInfo.name} deleted successfully! (Parameter types preserved)");
    }

    void CreateUnknownParameterTypes(string parameters)
    {
        string[] knownTypes = { "int", "float", "bool", "string", "Vector2", "Vector3", "GameObject", "Transform", "Rigidbody", "Collider" };
        string[] paramTypes = parameters.Split(',').Select(p => p.Trim()).ToArray();

        foreach (string paramType in paramTypes)
        {
            if (!knownTypes.Contains(paramType) && !paramType.Contains("<") && !paramType.Contains("[") && !TypeExists(paramType))
            {
                CreateParameterScript(paramType);
            }
        }
    }

    bool TypeExists(string typeName)
    {
        return System.AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Any(type => type.Name == typeName);
    }

    string ConvertToInterfaceNames(string parameters)
    {
        string[] paramTypes = parameters.Split(',').Select(p => p.Trim()).ToArray();
        string[] knownTypes = { "int", "float", "bool", "string", "Vector2", "Vector3", "GameObject", "Transform", "Rigidbody", "Collider" };
        
        for (int i = 0; i < paramTypes.Length; i++)
        {
            if (!knownTypes.Contains(paramTypes[i]) && !paramTypes[i].Contains("<") && !paramTypes[i].Contains("[") && !paramTypes[i].StartsWith("I"))
            {
                paramTypes[i] = $"I{paramTypes[i]}";
            }
        }
        
        return string.Join(", ", paramTypes);
    }

    void CreateParameterScript(string typeName)
    {
        string scriptPath = $"Assets/Scripts/{typeName}.cs";
        if (!File.Exists(scriptPath))
        {
            string scriptContent = $@"using UnityEngine;

public class {typeName} : MonoBehaviour
{{
    // Add your methods/properties here
}}";

            File.WriteAllText(scriptPath, scriptContent);
            AssetDatabase.Refresh();
            Debug.Log($"Created class: {typeName}");
        }
    }

    string CreateTriggerFunction(string eventName, string parameters)
    {
        string functionName = eventName.Replace("On", "");

        if (string.IsNullOrEmpty(parameters))
        {
            return $@"    public void {functionName}()
    {{
        {eventName}?.Invoke();
    }}";
        }
        else
        {
            string[] paramTypes = parameters.Split(',').Select(p => p.Trim()).ToArray();
            string paramList = string.Join(", ", paramTypes.Select((type, index) => $"{type} {ToCamelCase(type)}"));
            string invokeParams = string.Join(", ", paramTypes.Select((type, index) => ToCamelCase(type)));

            return $@"    public void {functionName}({paramList})
    {{
        {eventName}?.Invoke({invokeParams});
    }}";
        }
    }

    string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || str.Length < 1) return str;
        return char.ToLower(str[0]) + str.Substring(1);
    }

    void AddToBaseGameBehaviour(string eventName, string parameters)
    {
        string basePath = "Assets/Scripts/BaseGameBehaviour.cs";
        if (!File.Exists(basePath)) return;

        string content = File.ReadAllText(basePath);

        string[] paramTypes = string.IsNullOrEmpty(parameters) ? new string[0] : parameters.Split(',').Select(p => p.Trim()).ToArray();
        string paramList = paramTypes.Length > 0 ? string.Join(", ", paramTypes.Select(type => $"{type} {ToCamelCase(type)}")) : "";

        string virtualFunction = paramTypes.Length > 0
            ? $"    public virtual void {eventName}({paramList}) {{ }}\n"
            : $"    public virtual void {eventName}() {{ }}\n";

        string registerLine = $"        GameManager.{eventName} += {eventName};\n";
        string deregisterLine = $"        GameManager.{eventName} -= {eventName};\n";

        int funcIndex = content.IndexOf("    public virtual void OnActiveStepBack()");
        if (funcIndex != -1)
        {
            content = content.Insert(funcIndex, virtualFunction + "\n");
        }

        int enableIndex = content.IndexOf("        GameManager.OnActiveStepBack += OnActiveStepBack;");
        if (enableIndex != -1)
        {
            enableIndex = content.IndexOf("\n", enableIndex) + 1;
            content = content.Insert(enableIndex, registerLine);
        }

        int disableIndex = content.IndexOf("        GameManager.OnActiveStepBack -= OnActiveStepBack;");
        if (disableIndex != -1)
        {
            disableIndex = content.IndexOf("\n", disableIndex) + 1;
            content = content.Insert(disableIndex, deregisterLine);
        }

        File.WriteAllText(basePath, content);
        AssetDatabase.Refresh();
    }

    void RemoveFromBaseGameBehaviour(string eventName)
    {
        string basePath = "Assets/Scripts/BaseGameBehaviour.cs";
        if (!File.Exists(basePath)) return;

        string content = File.ReadAllText(basePath);

        string funcPattern = $@"    public virtual void {Regex.Escape(eventName)}\([^\)]*\) \{{ \}}(\r?\n)";
        content = Regex.Replace(content, funcPattern, "");

        string registerPattern = $@"        GameManager\.{Regex.Escape(eventName)} \+= {Regex.Escape(eventName)};(\r?\n)";
        content = Regex.Replace(content, registerPattern, "");

        string deregisterPattern = $@"        GameManager\.{Regex.Escape(eventName)} -= {Regex.Escape(eventName)};(\r?\n)";
        content = Regex.Replace(content, deregisterPattern, "");

        File.WriteAllText(basePath, content);
        AssetDatabase.Refresh();
    }
}
#endif