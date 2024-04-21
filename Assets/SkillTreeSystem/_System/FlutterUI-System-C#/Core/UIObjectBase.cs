using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class UIObjectBase : MonoBehaviour
{
    public RectTransform rectTransform;
    [HideInInspector] public GameObject uiObject;
    [HideInInspector] public Canvas rootCanvas;

    public List<UIObjectBase> children = new();
    public UIObjectBase child;
    public UIObjectBase parent;

    bool init = false;


    protected virtual void Start()
    {
        InitializeUI();
    }

    public virtual void InitializeUI()
    {
        if (init)
            return;

        if(uiObject == null)
        {
            uiObject = this.gameObject;
        }

        if(rectTransform == null)
        {
            rectTransform = gameObject.GetComponent<RectTransform>();   
        }

        if(rootCanvas == null)
        {
            rootCanvas = gameObject.GetComponentInParent<Canvas>();
        }

        init = true;
    }

    /* 삭제 예정 */
    public virtual void ShowUI()
    {
        InitializeUI();

        if (uiObject == null)
            return;

        uiObject.SetActive(true);

        children.ForEach(e => e.ShowUI());
    }

    /* 삭제 예정 */
    public virtual void CloseUI()
    {
        InitializeUI();

        if (uiObject == null)
            return;

        uiObject.SetActive(false);
    }

    public virtual void AddChildren(List<UIObjectBase> uis)
    {
        foreach(var ui in uis)
        {
            ui.transform.SetParent(transform, true);
            ui.transform.localScale = Vector3.one;
            ui.transform.localPosition = Vector3.zero;
            ui.parent = this;

            children.Add(ui);  
        }
    }
    public virtual void AddChild(UIObjectBase ui)
    {
        ui.transform.SetParent(transform, true);
        ui.transform.localScale = Vector3.one;
        ui.transform.localPosition = Vector3.zero;
        ui.parent = this;

        children.Add(ui);
        child = ui;
    }

    /*
     * create object, add component in here
     */
    protected virtual void SetUp()
    {
        rectTransform = gameObject.GetOrAddComponent<RectTransform>();
    }

#if UNITY_EDITOR
    public virtual void _Editor_SelectedUpdate()
    {

    }
#endif
}


#if UNITY_EDITOR
[CustomEditor(typeof(UIObjectBase))]
public class UIObjectBaseEditor : Editor
{
    UIObjectBase owner;

    public void OnEnable()
    {
        owner = target as UIObjectBase; 
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        base.OnInspectorGUI(); 

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Show"))
        {
            owner.ShowUI();
            Debug.Log("Show"); 
        }
        if (GUILayout.Button("Close"))
        {
            owner.CloseUI();
            Debug.Log("Close");
        }

        GUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck()) 
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endif