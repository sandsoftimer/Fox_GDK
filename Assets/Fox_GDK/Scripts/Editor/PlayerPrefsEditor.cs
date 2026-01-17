#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

public class PlayerPrefsEditor : EditorWindow
{
    private static List<string> playerPrefKeys = new List<string>();
    private Dictionary<string, string> updateValues = new Dictionary<string, string>();
    private Vector2 scrollPosition;
    private string newKey = "";
    private string newValue = "";
    private string searchFilter = "";
    private int selectedType = 0;
    private string[] typeOptions = { 
        "String", "Int", "Float", "Bool", "Byte", "SByte", "Short", "UShort", 
        "UInt", "Long", "ULong", "Double", "Decimal", "Char", "DateTime" 
    };
    private string validationMessage = "";

    // Column widths
    private float serialWidth = 30f;
    private float keyWidth = 120f;
    private float typeWidth = 70f;
    private float currentValueWidth = 120f;
    private float updateValueWidth = 120f;
    private float actionsWidth = 120f;

    // Resizing state
    private bool isResizing = false;
    private int resizingColumn = -1;
    private float resizeStartX = 0f;
    private float resizeStartWidth = 0f;

    [MenuItem("FoxTools/PlayerPrefs Editor")]
    public static void ShowWindow()
    {
        GetWindow<PlayerPrefsEditor>("PlayerPrefs CRUD");
    }

    private void OnEnable()
    {
        LoadExistingKeys();
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();

        EditorGUILayout.LabelField("PlayerPrefs CRUD System", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Controls
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Refresh", GUILayout.Width(80)))
        {
            LoadExistingKeys();
        }
        if (GUILayout.Button("Delete All", GUILayout.Width(80)))
        {
            if (EditorUtility.DisplayDialog("Delete All", "Delete all PlayerPrefs?", "Yes", "No"))
            {
                PlayerPrefs.DeleteAll();
                playerPrefKeys.Clear();
                updateValues.Clear();
            }
        }
        GUILayout.FlexibleSpace();
        searchFilter = EditorGUILayout.TextField("Search:", searchFilter);
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // CREATE with Real-time Validation
        EditorGUILayout.LabelField("CREATE New PlayerPref", EditorStyles.boldLabel);
        
        GUILayout.BeginVertical("box");
        newKey = EditorGUILayout.TextField("Key:", newKey);
        
        // Value field with immediate validation feedback
        string previousValue = newValue;
        newValue = EditorGUILayout.TextField("Value:", newValue);
        
        // Check if value changed and validate immediately
        if (newValue != previousValue)
        {
            ValidateValueRealTime();
        }
        
        selectedType = EditorGUILayout.Popup("Type:", selectedType, typeOptions);
        
        // Show validation message immediately
        if (!string.IsNullOrEmpty(validationMessage))
        {
            EditorGUILayout.HelpBox(validationMessage, MessageType.Error);
        }
        else if (!string.IsNullOrEmpty(newValue) && !IsValidForSelectedType(newValue, selectedType))
        {
            EditorGUILayout.HelpBox(GetValidationErrorForType(selectedType), MessageType.Error);
        }
        else if (!string.IsNullOrEmpty(newValue))
        {
            EditorGUILayout.HelpBox("✓ Valid input", MessageType.Info);
        }
        
        if (GUILayout.Button("CREATE"))
        {
            // Final validation before creation
            if (string.IsNullOrEmpty(newKey))
            {
                validationMessage = "Key cannot be empty!";
                return;
            }
            
            if (playerPrefKeys.Contains(newKey))
            {
                validationMessage = "Key already exists!";
                return;
            }
            
            if (string.IsNullOrEmpty(newValue))
            {
                validationMessage = "Value cannot be empty!";
                return;
            }
            
            if (!IsValidForSelectedType(newValue, selectedType))
            {
                validationMessage = GetValidationErrorForType(selectedType);
                return;
            }
            
            CreatePlayerPref();
        }
        GUILayout.EndVertical();

        EditorGUILayout.Space();

        // RESIZABLE TABLE
        EditorGUILayout.LabelField($"PlayerPrefs Table ({playerPrefKeys.Count})", EditorStyles.boldLabel);
        
        GUILayout.BeginVertical("box");
        
        // Table Header with resizable columns
        DrawResizableHeader();
        
        // Table Content
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        int serialNumber = 1;
        for (int i = 0; i < playerPrefKeys.Count; i++)
        {
            string key = playerPrefKeys[i];
            
            if (!string.IsNullOrEmpty(searchFilter) && !key.ToLower().Contains(searchFilter.ToLower()))
                continue;

            if (!PlayerPrefs.HasKey(key))
            {
                playerPrefKeys.RemoveAt(i);
                i--;
                continue;
            }

            DrawTableRow(key, serialNumber);
            serialNumber++;
        }

        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();
        GUILayout.EndVertical();

        HandleResizing();
    }

    private void DrawResizableHeader()
    {
        GUILayout.BeginHorizontal("toolbar");
        
        // Serial Number Column
        GUILayout.Label("#", EditorStyles.boldLabel, GUILayout.Width(serialWidth));
        DrawResizeHandle(0);
        
        // Key Column
        GUILayout.Label("Key", EditorStyles.boldLabel, GUILayout.Width(keyWidth));
        DrawResizeHandle(1);
        
        // Type Column
        GUILayout.Label("Type", EditorStyles.boldLabel, GUILayout.Width(typeWidth));
        DrawResizeHandle(2);
        
        // Current Value Column
        GUILayout.Label("Current Value", EditorStyles.boldLabel, GUILayout.Width(currentValueWidth));
        DrawResizeHandle(3);
        
        // Update Value Column
        GUILayout.Label("Update Value", EditorStyles.boldLabel, GUILayout.Width(updateValueWidth));
        DrawResizeHandle(4);
        
        // Actions Column
        GUILayout.Label("Actions", EditorStyles.boldLabel, GUILayout.Width(actionsWidth));
        
        GUILayout.EndHorizontal();
    }

    private void DrawResizeHandle(int columnIndex)
    {
        Rect handleRect = GUILayoutUtility.GetRect(4, 18, GUILayout.ExpandHeight(true));
        EditorGUIUtility.AddCursorRect(handleRect, MouseCursor.ResizeHorizontal);
        
        if (Event.current.type == EventType.MouseDown && handleRect.Contains(Event.current.mousePosition))
        {
            isResizing = true;
            resizingColumn = columnIndex;
            resizeStartX = Event.current.mousePosition.x;
            resizeStartWidth = GetColumnWidth(columnIndex);
            Event.current.Use();
        }
        
        GUI.Box(handleRect, "", "box");
    }

    private void DrawTableRow(string key, int serialNumber)
    {
        Color originalColor = GUI.backgroundColor;
        GUI.backgroundColor = serialNumber % 2 == 0 ? Color.white : new Color(0.9f, 0.9f, 0.9f);
        
        GUILayout.BeginHorizontal("box");
        GUI.backgroundColor = originalColor;

        // Serial Number
        GUILayout.Label(serialNumber.ToString(), GUILayout.Width(serialWidth));

        // Key
        GUILayout.Label(key, GUILayout.Width(keyWidth));

        // Type (Read-only)
        string valueType = GetValueType(key);
        GUI.enabled = false;
        EditorGUILayout.TextField(valueType, GUILayout.Width(typeWidth));
        GUI.enabled = true;

        // Current Value (Read-only)
        string currentValue = GetCurrentValue(key);
        GUI.enabled = false;
        EditorGUILayout.TextField(currentValue, GUILayout.Width(currentValueWidth));
        GUI.enabled = true;

        // Update Value Field with real-time validation
        if (!updateValues.ContainsKey(key))
            updateValues[key] = currentValue;
        
        string previousUpdateValue = updateValues[key];
        updateValues[key] = GUILayout.TextField(updateValues[key], GUILayout.Width(updateValueWidth));
        
        // Show validation feedback for update field
        if (updateValues[key] != previousUpdateValue)
        {
            int detectedType = GetDetectedTypeIndex(key);
            if (!string.IsNullOrEmpty(updateValues[key]) && !IsValidForSelectedType(updateValues[key], detectedType))
            {
                // Show red background for invalid input
                GUI.backgroundColor = new Color(1f, 0.8f, 0.8f);
            }
        }

        // Action Buttons
        GUILayout.BeginHorizontal(GUILayout.Width(actionsWidth));
        GUI.backgroundColor = new Color(0.7f, 1f, 0.7f);
        if (GUILayout.Button("Modify", GUILayout.Width(55)))
        {
            ModifyPlayerPref(key, updateValues[key]);
        }
        GUI.backgroundColor = new Color(1f, 0.7f, 0.7f);
        if (GUILayout.Button("Delete", GUILayout.Width(55)))
        {
            if (EditorUtility.DisplayDialog("Delete", $"Delete '{key}'?", "Yes", "No"))
            {
                DeletePlayerPref(key);
            }
        }
        GUI.backgroundColor = originalColor;
        GUILayout.EndHorizontal();

        GUILayout.EndHorizontal();
    }

    private void HandleResizing()
    {
        if (isResizing)
        {
            if (Event.current.type == EventType.MouseDrag)
            {
                float deltaX = Event.current.mousePosition.x - resizeStartX;
                float newWidth = Mathf.Max(30f, resizeStartWidth + deltaX);
                SetColumnWidth(resizingColumn, newWidth);
                Repaint();
                Event.current.Use();
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                isResizing = false;
                resizingColumn = -1;
                Event.current.Use();
            }
        }
    }

    private float GetColumnWidth(int columnIndex)
    {
        switch (columnIndex)
        {
            case 0: return serialWidth;
            case 1: return keyWidth;
            case 2: return typeWidth;
            case 3: return currentValueWidth;
            case 4: return updateValueWidth;
            default: return actionsWidth;
        }
    }

    private void SetColumnWidth(int columnIndex, float width)
    {
        switch (columnIndex)
        {
            case 0: serialWidth = width; break;
            case 1: keyWidth = width; break;
            case 2: typeWidth = width; break;
            case 3: currentValueWidth = width; break;
            case 4: updateValueWidth = width; break;
            case 5: actionsWidth = width; break;
        }
    }

    private void ValidateValueRealTime()
    {
        validationMessage = "";
        
        if (string.IsNullOrEmpty(newKey))
        {
            validationMessage = "Key cannot be empty!";
            return;
        }
        
        if (playerPrefKeys.Contains(newKey))
        {
            validationMessage = "Key already exists!";
            return;
        }
    }
    
    private bool IsValidForSelectedType(string value, int type)
    {
        if (string.IsNullOrEmpty(value)) return false;
        
        switch (type)
        {
            case 1: return int.TryParse(value, out _);
            case 2: return float.TryParse(value, out _);
            case 3: return bool.TryParse(value, out _);
            case 4: return byte.TryParse(value, out _);
            case 5: return sbyte.TryParse(value, out _);
            case 6: return short.TryParse(value, out _);
            case 7: return ushort.TryParse(value, out _);
            case 8: return uint.TryParse(value, out _);
            case 9: return long.TryParse(value, out _);
            case 10: return ulong.TryParse(value, out _);
            case 11: return double.TryParse(value, out _);
            case 12: return decimal.TryParse(value, out _);
            case 13: return value.Length == 1;
            case 14: return System.DateTime.TryParse(value, out _);
            default: return true; // String
        }
    }
    
    private string GetValidationErrorForType(int type)
    {
        switch (type)
        {
            case 1: return "❌ Invalid integer! Use numbers like: 123, -456";
            case 2: return "❌ Invalid float! Use numbers like: 12.5, -3.14";
            case 3: return "❌ Invalid boolean! Use: true or false";
            case 4: return "❌ Invalid byte! Use numbers 0-255";
            case 5: return "❌ Invalid sbyte! Use numbers -128 to 127";
            case 6: return "❌ Invalid short! Use numbers -32768 to 32767";
            case 7: return "❌ Invalid ushort! Use numbers 0-65535";
            case 8: return "❌ Invalid uint! Use positive numbers";
            case 9: return "❌ Invalid long! Use large numbers";
            case 10: return "❌ Invalid ulong! Use large positive numbers";
            case 11: return "❌ Invalid double! Use decimal numbers";
            case 12: return "❌ Invalid decimal! Use precise decimal numbers";
            case 13: return "❌ Invalid char! Use single character only";
            case 14: return "❌ Invalid datetime! Use format: MM/dd/yyyy";
            default: return "";
        }
    }

    private bool IsValidInput(string input, int type)
    {
        if (string.IsNullOrEmpty(input)) return true;
        
        switch (type)
        {
            case 1: case 4: case 5: case 6: case 7: case 8: case 9: case 10:
                return int.TryParse(input, out _) || long.TryParse(input, out _) || input == "-";
            case 2: case 11: case 12:
                return float.TryParse(input, out _) || double.TryParse(input, out _) || input == "-" || input.EndsWith(".");
            case 3:
                return bool.TryParse(input, out _) || input.ToLower() == "t" || input.ToLower() == "f";
            case 13:
                return input.Length <= 1;
            case 14:
                return DateTime.TryParse(input, out _) || input.Length < 10;
            default:
                return true;
        }
    }

    private int GetDetectedTypeIndex(string key)
    {
        string type = GetValueType(key);
        for (int i = 0; i < typeOptions.Length; i++)
        {
            if (typeOptions[i] == type) return i;
        }
        return 0;
    }

    private bool ValidateValue(string value, int type)
    {
        validationMessage = "";
        
        if (string.IsNullOrEmpty(newKey))
        {
            validationMessage = "Key cannot be empty!";
            return false;
        }

        if (string.IsNullOrEmpty(value))
        {
            validationMessage = "Value cannot be empty!";
            return false;
        }

        switch (type)
        {
            case 1: if (!int.TryParse(value, out _)) { validationMessage = "Invalid integer!"; return false; } break;
            case 2: if (!float.TryParse(value, out _)) { validationMessage = "Invalid float!"; return false; } break;
            case 3: if (!bool.TryParse(value, out _)) { validationMessage = "Invalid boolean!"; return false; } break;
            case 4: if (!byte.TryParse(value, out _)) { validationMessage = "Invalid byte!"; return false; } break;
            case 5: if (!sbyte.TryParse(value, out _)) { validationMessage = "Invalid sbyte!"; return false; } break;
            case 6: if (!short.TryParse(value, out _)) { validationMessage = "Invalid short!"; return false; } break;
            case 7: if (!ushort.TryParse(value, out _)) { validationMessage = "Invalid ushort!"; return false; } break;
            case 8: if (!uint.TryParse(value, out _)) { validationMessage = "Invalid uint!"; return false; } break;
            case 9: if (!long.TryParse(value, out _)) { validationMessage = "Invalid long!"; return false; } break;
            case 10: if (!ulong.TryParse(value, out _)) { validationMessage = "Invalid ulong!"; return false; } break;
            case 11: if (!double.TryParse(value, out _)) { validationMessage = "Invalid double!"; return false; } break;
            case 12: if (!decimal.TryParse(value, out _)) { validationMessage = "Invalid decimal!"; return false; } break;
            case 13: if (value.Length != 1) { validationMessage = "Char must be single character!"; return false; } break;
            case 14: if (!DateTime.TryParse(value, out _)) { validationMessage = "Invalid datetime!"; return false; } break;
        }
        return true;
    }

    private string GetValueType(string key)
    {
        string stringVal = PlayerPrefs.GetString(key, "");
        
        if (bool.TryParse(stringVal, out _)) return "Bool";
        if (byte.TryParse(stringVal, out _)) return "Byte";
        if (sbyte.TryParse(stringVal, out _)) return "SByte";
        if (short.TryParse(stringVal, out _)) return "Short";
        if (ushort.TryParse(stringVal, out _)) return "UShort";
        if (uint.TryParse(stringVal, out _)) return "UInt";
        if (ulong.TryParse(stringVal, out _)) return "ULong";
        if (decimal.TryParse(stringVal, out _)) return "Decimal";
        if (stringVal.Length == 1 && char.IsLetterOrDigit(stringVal[0])) return "Char";
        if (DateTime.TryParse(stringVal, out _)) return "DateTime";
        if (IsIntValue(key)) return "Int";
        if (IsFloatValue(key)) return "Float";
        if (long.TryParse(stringVal, out _)) return "Long";
        if (double.TryParse(stringVal, out _)) return "Double";
        
        return "String";
    }

    private string GetCurrentValue(string key)
    {
        if (IsIntValue(key)) return PlayerPrefs.GetInt(key).ToString();
        if (IsFloatValue(key)) return PlayerPrefs.GetFloat(key).ToString();
        return PlayerPrefs.GetString(key);
    }

    private void ModifyPlayerPref(string key, string newValue)
    {
        if (IsIntValue(key) && int.TryParse(newValue, out int intVal))
            PlayerPrefs.SetInt(key, intVal);
        else if (IsFloatValue(key) && float.TryParse(newValue, out float floatVal))
            PlayerPrefs.SetFloat(key, floatVal);
        else
            PlayerPrefs.SetString(key, newValue);
        
        PlayerPrefs.Save();
        updateValues[key] = GetCurrentValue(key);
    }

    private void DeletePlayerPref(string key)
    {
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.Save();
        playerPrefKeys.Remove(key);
        updateValues.Remove(key);
    }

    private void CreatePlayerPref()
    {
        if (!ValidateValue(newValue, selectedType)) return;
        if (playerPrefKeys.Contains(newKey)) { validationMessage = "Key already exists!"; return; }

        switch (selectedType)
        {
            case 0: PlayerPrefs.SetString(newKey, newValue); break;
            case 1: PlayerPrefs.SetInt(newKey, int.Parse(newValue)); break;
            case 2: PlayerPrefs.SetFloat(newKey, float.Parse(newValue)); break;
            default: PlayerPrefs.SetString(newKey, newValue); break;
        }

        PlayerPrefs.Save();
        playerPrefKeys.Add(newKey);
        validationMessage = "";
        newKey = "";
        newValue = "";
    }

    private bool IsIntValue(string key)
    {
        int intVal = PlayerPrefs.GetInt(key, int.MinValue);
        string stringVal = PlayerPrefs.GetString(key, "");
        return intVal != int.MinValue && (intVal.ToString() == stringVal || stringVal == "");
    }

    private bool IsFloatValue(string key)
    {
        float floatVal = PlayerPrefs.GetFloat(key, float.MinValue);
        string stringVal = PlayerPrefs.GetString(key, "");
        return floatVal != float.MinValue && (floatVal.ToString() == stringVal || stringVal == "");
    }

    private void LoadExistingKeys()
    {
        // Don't clear existing keys - only add new ones
        string[] testKeys = { "volume", "quality", "resolution", "fullscreen", "score", "level", "coins", "lives", "highscore", "music", "sfx", "difficulty" };
        
        foreach (string key in testKeys)
        {
            if (PlayerPrefs.HasKey(key) && !playerPrefKeys.Contains(key))
                playerPrefKeys.Add(key);
        }
        
        // Also scan for any PlayerPrefs that were created through this editor
        for (int i = playerPrefKeys.Count - 1; i >= 0; i--)
        {
            if (!PlayerPrefs.HasKey(playerPrefKeys[i]))
            {
                playerPrefKeys.RemoveAt(i);
                if (updateValues.ContainsKey(playerPrefKeys[i]))
                    updateValues.Remove(playerPrefKeys[i]);
            }
        }
        
        Repaint();
    }
}
#endif