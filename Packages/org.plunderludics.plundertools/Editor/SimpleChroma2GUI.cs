using UnityEngine;
using UnityEditor;
using System;

public class SimpleChroma2GUI : ShaderGUI
{
    const int MaxKeyCount = 8;
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        // render the default gui
        // base.OnGUI(materialEditor, properties);
        materialEditor.RegisterPropertyChangeUndo("SimpleChroma2");

        Material targetMat = materialEditor.target as Material;
        
        // Assuming _Keys is stored as a Vector4 array
        // Retrieve the existing keys array or initialize a new one
        Vector4[] keys = new Vector4[MaxKeyCount];
        Vector4[] hslTolerance = new Vector4[MaxKeyCount];
        float[] rgbTolerance = new float[MaxKeyCount];
        bool[] enable = new bool[MaxKeyCount];
        int keyCount = targetMat.GetInt("_KeyCount");
        for (int i = 0; i < keyCount; i++)
        {
            keys[i] = targetMat.GetVector($"_Key{i}");
            hslTolerance[i] = targetMat.GetVector($"_HSLTolerance{i}");
            rgbTolerance[i] = targetMat.GetFloat($"_RGBTolerance{i}");
            enable[i] = targetMat.GetFloat($"_Enable{i}") > 0;
        }

        // Add a toggle for the '_Invert' parameter
        bool invert = targetMat.GetInt("_Invert") == 1;
        invert = EditorGUILayout.Toggle("Invert", invert);
        targetMat.SetInt("_Invert", invert ? 1 : 0);
        
        // Add toggle for '_Enable' param
        bool topEnable = targetMat.GetInt("_Enable") == 1;
        topEnable = EditorGUILayout.Toggle("Enable", topEnable);
        targetMat.SetInt("_Enable", topEnable ? 1 : 0);

        EditorGUILayout.LabelField($"Keys ({keyCount})", EditorStyles.boldLabel);

        // Display the current keys with options to modify
        for (int i = 0; i < keyCount; i++)
        {
            EditorGUILayout.BeginHorizontal();
            
            // Add a button to remove the key
            if (GUILayout.Button("Remove", GUILayout.Width(70)))
            {
                keyCount--;
                for (int j = i; j < keyCount; j++)
                {
                    keys[j] = keys[j + 1];
                }
                for (int j = i; j < keyCount; j++)
                {
                    hslTolerance[j] = hslTolerance[j + 1];
                }
                break; // Exit loop to avoid modifying the array while iterating
            }

            Vector4 key = keys[i];
            Color keyColor = new Color(key.x, key.y, key.z, key.w); // Convert Vector4 to Color

            // Use a horizontal layout to ensure the color picker fits properly
            EditorGUILayout.LabelField($"Key {i}", GUILayout.Width(50));
            keyColor = EditorGUILayout.ColorField(keyColor);
            keys[i] = new Vector4(keyColor.r, keyColor.g, keyColor.b, keyColor.a); // Convert back to Vector4

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            enable[i] = EditorGUILayout.Toggle(enable[i]);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();

            EditorGUIUtility.labelWidth = 30;
            Vector4 tolerance = hslTolerance[i];
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("HSL Tolerance", GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            tolerance.x = EditorGUILayout.FloatField("H", tolerance.x, GUILayout.MinWidth(50));
            tolerance.y = EditorGUILayout.FloatField("S", tolerance.y, GUILayout.MinWidth(50));
            tolerance.z = EditorGUILayout.FloatField("L", tolerance.z, GUILayout.MinWidth(50));
            hslTolerance[i] = tolerance;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 30;
            EditorGUILayout.LabelField("RGB Tolerance", GUILayout.Width(100));
            GUILayout.FlexibleSpace();
            rgbTolerance[i] = EditorGUILayout.FloatField("", rgbTolerance[i], GUILayout.MinWidth(50));
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel--;
        }

        // Add a button to add a new key or show max message
        if (keyCount < MaxKeyCount)
        {
            if (GUILayout.Button("Add Key"))
            {
                keys[keyCount] = Color.green; // Initialize new key with default value
                hslTolerance[keyCount] = new Vector4(0.1f, 0.1f, 0.1f, 0); // Initialize new HSL tolerance with default value
                enable[keyCount] = true;
                rgbTolerance[keyCount] = 0.1f; // Initialize new RGB tolerance with default value
                keyCount++;
            }
        }
        else
        {
            EditorGUILayout.HelpBox($"Maximum {MaxKeyCount} keys", MessageType.Info);
        }

        // Apply all the key changes
        targetMat.SetInt("_KeyCount", keyCount); // Set the key count
        for (int i = 0; i < keyCount; i++)
        {
            targetMat.SetFloat($"_Enable{i}", enable[i] ? 1 : 0);
            targetMat.SetVector($"_Key{i}", keys[i]);
            targetMat.SetVector($"_HSLTolerance{i}", hslTolerance[i]);
            targetMat.SetFloat($"_RGBTolerance{i}", rgbTolerance[i]);
        }

        // Render the default inspector below the custom GUI
        EditorGUILayout.Space();
        base.OnGUI(materialEditor, properties);
    }
}