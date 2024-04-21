using System;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using ObjectField = UnityEditor.UIElements.ObjectField;
#endif

using Action = System.Action;

#if UNITY_EDITOR
public class SkillDataPort : Port
{
    public SkillNode m_Node;
    public SkillData skillData;
    public ObjectField m_ObjectField;
    public Action<SkillData> onChangeValue;


    public SkillDataPort(Direction portDirection, Port.Capacity capacity)
        : base(portOrientation: Orientation.Horizontal,
               portDirection,
               capacity,
               type: typeof(SkillData))
    {
    }

    public void Init(SkillNode node)
    {
        m_Node = node; 

        contentContainer.Clear();

        // input
        if (direction == Direction.Input)
        {
            contentContainer.style.flexDirection = FlexDirection.Row;
            contentContainer.style.alignItems = Align.FlexStart;
            //contentContainer.style.height = 60;

            // create select field 
            m_ObjectField = new ObjectField
            {
                objectType = typeof(SkillData),
                allowSceneObjects = false,      // scene object able
                value = null
            };
            m_ObjectField.RegisterValueChangedCallback(evt =>
            {
                if(evt.newValue == null)
                {
                    m_Node.m_IconImage.sprite = null;
                    skillData = null;
                    userData = null;
                }
                else
                {
                    var data = evt.newValue as SkillData;
                    m_Node.m_IconImage.sprite = data.skillIcon;
                    skillData = data;
                    userData = data;

                    if (onChangeValue != null)
                    {
                        onChangeValue(data);
                    }
                }
            });
            m_ObjectField.style.width = 80;

            // create scriptableObject button
            var createButton = new Button(() =>
            {
                CreateData();
            });
            createButton.text = "Create";
            createButton.style.width = 80;

            // add elements
            contentContainer.Add(m_ObjectField);
            contentContainer.Add(createButton);
        }

        //this.AddManipulator(new EdgeConnector<Edge>(new CustomEdgeConnectorListener()));
    }

    public void CreateData()
    {
        var data = new SkillData();
        string filename = TypeOf.GetLastType(data).FullName;

        var path = EditorUtility.SaveFilePanelInProject(
              "Save ScriptableObject",
              filename,
              "asset",
              "Please enter a file name to save the scriptable object to");

        // save so file
        if (!string.IsNullOrEmpty(path))
        {
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorGUIUtility.PingObject(data); // focus contents browser

            m_ObjectField.SetValueWithoutNotify(data);
            m_Node.m_IconImage.sprite = data.skillIcon;
            skillData = data;
            userData = data;
        }
    }

    public void SetValue(SkillData value)
    {
        m_ObjectField.SetValueWithoutNotify(value);
    }

    public override void OnStartEdgeDragging()
    {
        base.OnStartEdgeDragging();
    }

    public override void OnStopEdgeDragging()
    {
        base.OnStopEdgeDragging();
    }

    public override void OnSelected()
    {
        base.OnSelected();
    }
}
#endif