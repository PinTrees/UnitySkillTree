// Designed by YM, 2024

using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class ButtonAttribute : PropertyAttribute
{
    public string MethodName { get; private set; }

    public ButtonAttribute(string methodName)
    {
        MethodName = methodName;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ButtonAttribute))]
public class ButtonAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        /* float buttonWidth = 80;

        // �⺻ �ʵ� ǥ�ÿ� �Բ� ��ư ������ ����
        position.width -= buttonWidth + 8;      // ��ư �ʺ�ŭ �ʵ� �ʺ� ����
        EditorGUI.PropertyField(position, property, label, true);

        position.x += position.width + 5;       // ��ư ��ġ ����
        position.width = buttonWidth;           // ��ư �ʺ� ���� */

        ButtonAttribute buttonAttribute = attribute as ButtonAttribute;

        if (GUI.Button(position, buttonAttribute.MethodName))
        {
            MonoBehaviour targetObject = property.serializedObject.targetObject as MonoBehaviour;
            System.Reflection.MethodInfo methodInfo = targetObject.GetType().GetMethod(buttonAttribute.MethodName,
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);

            if (methodInfo != null)
            {
                methodInfo.Invoke(targetObject, null);
            }
            else
            {
                Debug.LogWarning("Method not found: " + buttonAttribute.MethodName);
            }
        }
    }
}
#endif