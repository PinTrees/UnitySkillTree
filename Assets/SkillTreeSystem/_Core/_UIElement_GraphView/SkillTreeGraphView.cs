using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;

public class SkillTreeGraphView : GraphView
{
    /*
     * data
     */
    public SkillTreeData m_SkillTreeData { get; private set; }
    private List<SkillNode> m_Nodes = new();


    #region Initialize Methods
    public SkillTreeGraphView(SkillTreeData skillTreeData)
    {
        m_SkillTreeData = skillTreeData;

        AddManipulator();
        SetGridBackground();

        SetElementsDeleted();
        SetGraphViewChanged();


        this.StretchToParentSize();
    }

    public void AddManipulator()
    {
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger()); 
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
    }

    public void SetGridBackground()
    {
        GridBackground gridBackground = new GridBackground();
        gridBackground.StretchToParentSize();
        Insert(0, gridBackground);
    }

    /*
    * init delete action
    */
    private void SetElementsDeleted()
    {
    }

    /*
     * init refresh action
     */
    private void SetGraphViewChanged()
    {
        // set
        graphViewChanged = (changes) =>
        {
            if (changes.edgesToCreate != null)
            {
                foreach (var edge in changes.edgesToCreate)
                {
                    AddElement(edge);
                }
            }

            if (changes.elementsToRemove != null)
            {
                changes.elementsToRemove.ForEach(elem =>
                {
                    if (elem is Edge edge)
                    {
                        edge.input.Disconnect(edge);
                        edge.output.Disconnect(edge);
                    }
                });
            }

            Save();
            return changes;
        };
    }
    #endregion

    /*
     * create new skill node element
     */
    public Node AddNode(string nodeName)
    {
        var node = new SkillNode(nodeName);
        node.Init(this);

        // 노드의 초기 위치를 현재 뷰포트의 중앙으로 설정
        Vector2 center = this.layout.center - (this.contentViewContainer.transform.scale * node.layout.size / 2);
        Vector2 scrollViewCenter = this.contentViewContainer.WorldToLocal(this.WorldToLocal(center));
        node.SetPosition(new Rect(scrollViewCenter, Vector2.zero));

        node.RefreshExpandedState();
        node.RefreshPorts();

        this.AddElement(node); // 노드를 GraphView에 추가
        m_Nodes.Add(node);

        return node;
    }

    public Node AddNode(SkillDataContainer container)
    {
        var node = SkillNode.FromData(container);
        node.Init(this);

        node.RefreshExpandedState();
        node.RefreshPorts();

        AddElement(node); // 노드를 GraphView에 추가
        m_Nodes.Add(node);

        return node;
    }

    public void AddEdge(SkillConnectData data)
    {
        var parentNode = m_Nodes.FirstOrDefault((e) => e.m_ID == data.parentSkillUid);
        var childNode = m_Nodes.FirstOrDefault((e) => e.m_ID == data.childSkillUid);

        if (parentNode == null || childNode == null)
        {
            Debug.LogWarning("Nodes could not be found.");
            return;
        }

        var edge = new Edge
        {
            output = parentNode.m_ChildSkillPort,
            input = childNode.m_ParentSkillPort
        };

        edge.output.Connect(edge);
        edge.input.Connect(edge);

        AddElement(edge);
    }


    /*
     * save graph data
     * refresh node
     */
    public void SaveAndRefresh()
    {
        Save();
        Refresh();
    }
    public void Save()
    {
        // 구조 변경이 필요함
        m_SkillTreeData._Editor_Clear();

        // refresh node data
        for (int i = 0; i < m_Nodes.Count; ++i)
        {
            if (m_Nodes[i] == null)
            {
                m_Nodes.RemoveAt(i--);
                continue;
            }

            m_Nodes[i].Save();
        }
    }
    public void Refresh()
    {
        // refresh node data
        for (int i = 0; i < m_Nodes.Count; ++i)
        {
            if (m_Nodes[i] == null)
            {
                m_Nodes.RemoveAt(i--);
                continue;
            }

            m_Nodes[i].Refresh();
        }
    }

    #region Override Methods
    /*
     * 필수 오버라이드 메서드 
     * 해당 메서드를 반드시 오버라이드 해야 노드 연결이 가능해 집니다.
     * 어째서 해당 메서드를 기본 동작 함수로 설계하지 않았는지 의문
     */
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        /* C#의 널에이블은 확정 존재 ! 오퍼레이터 사용이 안되는걸로 알고 있는데 어째서 가능한지 의문 */
        return ports.ToList()!.Where(endPort =>
                      endPort.direction != startPort.direction &&
                      endPort.node != startPort.node &&
                      endPort.portType == startPort.portType).ToList();
    }
    #endregion
}
#endif