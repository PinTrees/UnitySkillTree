using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.GraphView;
#endif

#if UNITY_EDITOR
/*
 * graph view element
 */
public class SkillNode : Node
{
    /*
     * owner
     */
    public SkillTreeGraphView m_GraphView { get; private set; }

    /*
     * data
     */
    public string m_ID { get; private set; }
    public SkillData m_Data { get; private set; }
    public SkillDataPort m_DataPort { get; private set; }

    public SkillLinkPort m_ParentSkillPort { get; private set; }
    public SkillLinkPort m_ChildSkillPort { get; private set; }

    public Image m_IconImage;

    /*
     * skill data property port
     */
    private List<Port> m_SkillPropertyPorts = new();

    public static SkillNode FromData(SkillDataContainer data)
    {
        var node = new SkillNode("");
        node.m_ID = data.uid;
        node.m_Data = data.data;
        node.SetPosition(data.nodePosition);

        return node;
    }
    public SkillNode(string nodeName)
    {
        m_ID = Guid.NewGuid().ToString();

        this.title = nodeName;
        this.capabilities |= Capabilities.Movable | Capabilities.Deletable;
    }

    public void Init(SkillTreeGraphView graphView)
    {
        m_GraphView = graphView;

        m_IconImage = new Image();
        m_IconImage.sprite = null;
        m_IconImage.style.width = 40;
        m_IconImage.style.height = 40;
        m_IconImage.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 1f);
        titleContainer.Insert(0, m_IconImage);

        // add input port
        AddParentSkillLinkerPort();
        AddSkillDataPort();

        // add output port
        AddChildSkillLinkerPort();

        RefreshExpandedState();
        RefreshPorts();
    }

    public void SetSkillPropertyPort(SkillData data)
    {
        m_Data = data;
        m_SkillPropertyPorts.ForEach(e =>
        {
            DeletePort(e);
        });
        m_SkillPropertyPorts.Clear();

        foreach (FieldInfo fieldInfo in typeof(SkillData).GetFields())
        {
            var port = CreatePortForProperty(fieldInfo, data);
            inputContainer.Add(port);
            m_SkillPropertyPorts.Add(port);
        }
    }

    private Port CreatePortForProperty(FieldInfo fieldInfo, SkillData data)
    {
        if(fieldInfo.FieldType == typeof(Sprite))
        {
            var port = new SpritePort(Direction.Input, Port.Capacity.Single);
            Sprite value = fieldInfo.GetValue(data) as Sprite;
            port.SetValue(value);

            port.onChangeValue = (value) =>
            {
                fieldInfo.SetValue(data, value);
                m_GraphView.SaveAndRefresh();
            };

            return port;
        }
        else if(fieldInfo.FieldType == typeof(string))
        {
            var port = new StringPort(Direction.Input, Port.Capacity.Single);
            string value = fieldInfo.GetValue(data) as string;
            port.SetValue(value);

            port.onChangeValue = (value) =>
            {
                fieldInfo.SetValue(data, value);
                m_GraphView.SaveAndRefresh();
            };

            return port;
        }
        else if(fieldInfo.FieldType == typeof(int))
        {
            var port = new IntPort(Direction.Input, Port.Capacity.Single);
            port.portName = fieldInfo.Name.ToCapitalizeFirst();
            int value = (int)fieldInfo.GetValue(data);
            port.SetValue(value);

            port.onChangeValue = (value) =>
            {
                fieldInfo.SetValue(data, value);
                m_GraphView.SaveAndRefresh();
            };

            return port;
        }
        else
        {
            var port = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Single, typeof(string));
            port.contentContainer.Add(new Label(fieldInfo.Name));

            return port;
        }
    }

    public void Save()
    {
        var skillDataContainer = m_GraphView.m_SkillTreeData.FindSkillDataContainer(m_ID);
        if (skillDataContainer == null) skillDataContainer = new();

        skillDataContainer.uid = m_ID;
        skillDataContainer.data = m_Data;
        skillDataContainer.nodePosition = GetPosition();

        m_GraphView.m_SkillTreeData.AddSkillDataContainer(skillDataContainer);

        m_ChildSkillPort.childSkillUids.ForEach(e =>
        {
            var skillConnectData = new SkillConnectData();
            skillConnectData.parentSkillUid = m_ID;
            skillConnectData.childSkillUid = e;
            m_GraphView.m_SkillTreeData._Editor_GetSkillConnectDatas().Add(skillConnectData);
        });

        if (m_Data != null)
        {
            EditorUtility.SetDirty(m_Data);
        }
        EditorUtility.SetDirty(m_GraphView.m_SkillTreeData);
    }
    public void Refresh()
    {
        if (m_Data == null)
        {
            title = "Empty SKill Node";
            return;
        }

        title = m_Data.skillName;
        m_IconImage.sprite = m_Data.skillIcon;
    }

    private void AddSkillDataPort()
    {
        if(m_DataPort != null)
        {
            DeletePort(m_DataPort);
        }

        m_DataPort = new SkillDataPort(Direction.Input, Port.Capacity.Single);
        m_DataPort.Init(this);
        m_DataPort.onChangeValue = (value) =>
        {
            SetSkillPropertyPort(value);
            m_GraphView.Save();
        };
        inputContainer.Add(m_DataPort);

        if (m_Data != null)
        {
            m_DataPort.SetValue(m_Data);
            SetSkillPropertyPort(m_Data);
        }
    }

    private void AddParentSkillLinkerPort()
    {
        if(m_ParentSkillPort != null)
        {
            DeletePort(m_ParentSkillPort);
        }

        m_ParentSkillPort = new SkillLinkPort(Direction.Input, Port.Capacity.Multi);
        m_ParentSkillPort.portName = "Parent Skills";
        m_ParentSkillPort.Init();
        inputContainer.Add(m_ParentSkillPort);
    }

    private void AddChildSkillLinkerPort()
    {
        if(m_ChildSkillPort != null)
        {
            DeletePort(m_ChildSkillPort);
        }

        m_ChildSkillPort = new SkillLinkPort(Direction.Output, Port.Capacity.Multi);
        m_ChildSkillPort.portName = "Child Skills";
        m_ChildSkillPort.Init();
        outputContainer.Add(m_ChildSkillPort);
    }

    public void DeletePort(Port port)
    {
        var edgesToDelete = new List<Edge>();
        edgesToDelete.AddRange(port.connections);

        foreach (var edge in edgesToDelete)
        {
            edge.input.Disconnect(edge);
            edge.output.Disconnect(edge);

            // 이제 엣지를 GraphView에서 제거합니다.
            m_GraphView.RemoveElement(edge);
        }

        (port.parent as VisualElement)?.Remove(port);

        // 노드와 GraphView의 UI를 업데이트합니다.
        this.RefreshPorts();
        this.RefreshExpandedState();
    }

    /*
     * override
     */
    #region Override Methods
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectPort(inputContainer));
        evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectPort(outputContainer));
        base.BuildContextualMenu(evt);
    }
    #endregion

    public void DisconnectAllPorts()
    {
        DisconnectPort(inputContainer);
        DisconnectPort(outputContainer);
    }

    private void DisconnectPort(VisualElement container)
    {
        foreach(Port port in container.Children())
        {
            if(!port.connected)
            {
                continue;
            }

            m_GraphView.DeleteElements(port.connections);
        }
    }
}
#endif
