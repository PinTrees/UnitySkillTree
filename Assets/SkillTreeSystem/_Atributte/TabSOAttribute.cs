// Designed by YM, 2024

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class TabSOAttribute : PropertyAttribute
{
    public readonly string tabName;

    public TabSOAttribute(string tabName)
    {
        this.tabName = tabName;
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(ScriptableObject), true)]
public class TabSOAttributeEditor : Editor
{
    private int selectedTab;
    private List<string> tabHeaders;
    private Dictionary<string, List<SerializedProperty>> tabProperties;


    private void OnEnable()
    {
        CollectTabProperties();
    }

    private void CollectTabProperties()
    {
        tabHeaders = new List<string>();
        tabProperties = new Dictionary<string, List<SerializedProperty>>();

        SerializedProperty property = serializedObject.GetIterator();
        string currentHeader = null;

        do
        {
            if (property.depth > 0)
            {
                continue;
            }

            // Check if the property has a Header attribute
            var attributes = GetPropertyAttributes<TabAttribute>(property);
            if (attributes.Length > 0)
            {
                currentHeader = attributes[0].tabName;
                tabHeaders.Add(currentHeader);
                tabProperties[currentHeader] = new List<SerializedProperty>();
            }

            if (currentHeader != null)
            {
                tabProperties[currentHeader].Add(property.Copy());
            }

        } while (property.NextVisible(true));
    }


    public override void OnInspectorGUI()
    {
        if (tabHeaders.Count > 0)
        {
            selectedTab = GUILayout.Toolbar(selectedTab, tabHeaders.ToArray());

            if (selectedTab >= 0 && selectedTab < tabHeaders.Count)
            {
                string header = tabHeaders[selectedTab];
                foreach (SerializedProperty property in tabProperties[header])
                {
                    serializedObject.Update(); // Add this line
                    EditorGUILayout.PropertyField(property, true);
                    serializedObject.ApplyModifiedProperties(); // Add this line
                }
            }
        }
        else
        {
            serializedObject.Update(); // Add this line
            base.OnInspectorGUI();
            serializedObject.ApplyModifiedProperties(); // Add this line
        }
    }


    /// <summary>
    /// Retrieves the specified attribute from a serialized property, if it exists.
    /// </summary>
    /// <typeparam name="T">The type of attribute to retrieve.</typeparam>
    /// <param name="property">The property to search for the attribute.</param>
    /// <returns>The attribute if found; otherwise null.</returns>
    private T[] GetPropertyAttributes<T>(SerializedProperty property) where T : System.Attribute
    {
        // get field [ instance, public, non-public ] 
        FieldInfo fieldInfo = serializedObject.targetObject.GetType().GetField(property.propertyPath,
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        if (fieldInfo != null)
        {
            // get info attributes -                      /* abstruct option */
            return (T[])fieldInfo.GetCustomAttributes(typeof(T), true);
        }

        // not found
        return new T[0];
    }
}
#endif