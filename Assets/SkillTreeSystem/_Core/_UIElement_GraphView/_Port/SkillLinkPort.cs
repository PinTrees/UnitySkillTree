using System.Collections.Generic;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

#if UNITY_EDITOR
public class SkillLinkPort : Port
{
    private Label m_SkillCountLabel;
    private CustomEdgeConnectorListener m_EdgeConnectorListener;
  
    public bool Fixed = false;
    public List<string> parentSkillUids = new();
    public List<string> childSkillUids = new();


    public SkillLinkPort(Direction portDirection, Port.Capacity capacity)
        : base(portOrientation: Orientation.Horizontal,
               portDirection,
               capacity,
               type: typeof(string))
    {
        m_EdgeConnectorListener = new CustomEdgeConnectorListener();
        this.AddManipulator(new EdgeConnector<Edge>(m_EdgeConnectorListener));
    }

    public void Init()
    {
        m_SkillCountLabel = new Label();
        m_SkillCountLabel.text = "0";
        contentContainer.Add(m_SkillCountLabel);
    }

    public void Refresh()
    {
        if(direction == Direction.Input)
        {
            m_SkillCountLabel.text = parentSkillUids.Count.ToString();
        }
        else if(direction == Direction.Output)
        {
            m_SkillCountLabel.text = childSkillUids.Count.ToString();
        }
    }

    public override void Connect(Edge edge)
    {
        base.Connect(edge);

        if(Fixed)
        {
            return;
        }

        if(edge.input is not SkillLinkPort || edge.output is not SkillLinkPort)
        {
            return;
        }

        if(direction == Direction.Input)
        {
            var inputPort = edge.input as SkillLinkPort;
            inputPort.parentSkillUids.Add((edge.output.node as SkillNode).m_ID);
            inputPort.Refresh();
        }
        else
        {
            var outputPort = edge.output as SkillLinkPort;
            outputPort.childSkillUids.Add((edge.input.node as SkillNode).m_ID);
            outputPort.Refresh();
        }
    }

    public override void Disconnect(Edge edge)
    {
        base.Disconnect(edge);

        if (Fixed)
        {
            return;
        }

        if (edge.input is not SkillLinkPort || edge.output is not SkillLinkPort)
        {
            return;
        }

        if (direction == Direction.Input)
        {
            var inputPort = edge.input as SkillLinkPort;
            inputPort.parentSkillUids.Remove((edge.output.node as SkillNode).m_ID);
            inputPort.Refresh();
        }
        else
        {
            var outputPort = edge.output as SkillLinkPort;
            outputPort.childSkillUids.Remove((edge.input.node as SkillNode).m_ID);
            outputPort.Refresh();
        }
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