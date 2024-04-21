// Designed by YM, 2024

using System;
using UnityEngine;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class CreateScriptableAttribute : PropertyAttribute
{
    public const string PATH_FILE_NAME = "filename:{}";

    public string ButtonName { get; private set; }
    public string MethodName { get; private set; }
    public string onLoadMethodName { get; private set; }
    public string SavePath { get; private set; }

    public CreateScriptableAttribute(string buttonName=null, string create_func=null, string onload_func=null, string savePath=null)
    {
        ButtonName = buttonName ?? "Create";
        MethodName = create_func;
        onLoadMethodName = onload_func;

#if UNITY_EDITOR
        if (savePath != null)
        {
            SavePath = AssetDatabase.GenerateUniqueAssetPath($"{savePath}/{PATH_FILE_NAME}.asset");
        }
        else
        {
            SavePath = null;
        }
#endif
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(CreateScriptableAttribute))]
public class CreateScriptableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float buttonWidth = 80;

        // 기본 필드 표시와 함께 버튼 영역을 설정
        position.width -= buttonWidth + 8;      // 버튼 너비만큼 필드 너비 줄임
        EditorGUI.PropertyField(position, property, label, true);

        position.x += position.width + 5;       // 버튼 위치 조정
        position.width = buttonWidth;           // 버튼 너비 설정

        var attribute = (CreateScriptableAttribute)this.attribute;
        if (GUI.Button(position, attribute.ButtonName))
        {
            var targetObject = property.serializedObject.targetObject;
            var targetType = targetObject.GetType();
            var methodInfo = targetType.GetMethod(attribute.MethodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (methodInfo != null)
            {
                var result = methodInfo.Invoke(targetObject, null);

                if (result is ScriptableObject so)
                {
                    // set file save path
                    string path = attribute.SavePath;

                    if (path == null)
                    {
                        string filename = string.IsNullOrEmpty(path) ? TypeOf.GetLastType(result).FullName : so.name;

                        // select file save path
                        path = EditorUtility.SaveFilePanelInProject(
                            "Save ScriptableObject",
                            filename,
                            "asset",
                            "Please enter a file name to save the scriptable object to");
                    }
                    else
                    {
                        path.Replace(CreateScriptableAttribute.PATH_FILE_NAME, so.name);
                    }

                    // save so file
                    if (!string.IsNullOrEmpty(path))
                    {
                        AssetDatabase.CreateAsset(so, path);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        EditorGUIUtility.PingObject(so); // focus contents browser

                        property.objectReferenceValue = so;
                        property.serializedObject.ApplyModifiedProperties(); // 변경사항 적용
                    }
                }
            }
            else 
            {
                Debug.LogError($"Method {attribute.MethodName} not found in {targetType}");
            }
        }

        EditorGUI.EndProperty();
    }
}
#endif