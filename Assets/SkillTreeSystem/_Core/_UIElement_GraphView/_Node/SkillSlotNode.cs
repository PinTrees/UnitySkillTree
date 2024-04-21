using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

#if UNITY_EDITOR
/*
 * graph view element
 */
public class SkillSlotNode : Node
{
    /*
     * owner
     */
    public SkillTreeUIGraphView m_GraphView { get; private set; }

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

    public static SkillSlotNode FromData(SkillDataContainer data, SkillUIPositionData positionData)
    {
        var node = new SkillSlotNode();

        node.title = "";
        node.capabilities |= Capabilities.Movable | Capabilities.Deletable;
        node.capabilities &= ~Capabilities.Collapsible;

        node.m_ID = data.uid;
        node.m_Data = data.data;

        if (positionData != null)
            node.SetPosition(positionData.nodePosition);
        else
            node.SetPosition(data.nodePosition);

        return node;
    }

    public void Init(SkillTreeUIGraphView graphView)
    {
        m_GraphView = graphView;

        titleContainer.Clear();
        titleContainer.style.width = 68;
        titleContainer.style.height = 68;
        titleContainer.Insert(0, m_IconImage);

        AddParentSkillLinkerPort();

        RefreshExpandedState();
        RefreshPorts();
    }

    private void AddParentSkillLinkerPort()
    {
        if (m_ParentSkillPort != null)
        {
            DeletePort(m_ParentSkillPort);
        }

        m_ParentSkillPort = new SkillLinkPort(Direction.Input, Port.Capacity.Multi);
        m_ParentSkillPort.Fixed = true;
        m_ParentSkillPort.portName = "";
        m_ParentSkillPort.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.3f); 

        m_ChildSkillPort = new SkillLinkPort(Direction.Output, Port.Capacity.Multi);
        m_ChildSkillPort.Fixed = true;
        m_ChildSkillPort.portName = "";
        m_ChildSkillPort.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 0.3f);

        m_IconImage = new Image();
        m_IconImage.sprite = null;
        m_IconImage.style.width = 68;
        m_IconImage.style.height = 68;
        m_IconImage.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f, 1f);

        // 중앙 정렬을 위한 컨테이너 생성
        var centeringContainer = new VisualElement();
        centeringContainer.style.position = Position.Absolute;

        var Top = new VisualElement();
        Top.style.position = Position.Absolute;
        Top.style.flexDirection = FlexDirection.Row;

        Top.Add(m_ParentSkillPort);
        Top.Add(m_ChildSkillPort);

        centeringContainer.Add(Top);
        centeringContainer.Add(m_IconImage);

        titleContainer.Add(centeringContainer);
    }

    public void Save()
    {
        var skillPostionData = new SkillUIPositionData();
        skillPostionData.uid = m_ID;
        skillPostionData.nodePosition = GetPosition();

        m_GraphView.m_SkillTreeData._Editor_GetSkillUIPositionData().Add(skillPostionData);
    }
    public void Refresh()
    {
        if(m_Data == null)
        {
            return;
        }

        m_IconImage.sprite = m_Data.skillIcon;
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
        foreach (Port port in container.Children())
        {
            if (!port.connected)
            {
                continue;
            }

            m_GraphView.DeleteElements(port.connections);
        }
    }
}
#endif