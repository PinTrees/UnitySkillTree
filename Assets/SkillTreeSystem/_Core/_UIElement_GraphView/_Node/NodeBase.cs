using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

#if UNITY_EDITOR
/*
 * graph view element
 */
public class NodeBase : Node
{
    /*
     * owner
     */
    public SkillTreeGraphView m_OwnerGraphView { get; private set; }

    /*
     * data
     */
    public string m_ID { get; private set; }


    public NodeBase(string nodeName)
    {
        m_ID = Guid.NewGuid().ToString();

        this.title = nodeName;
        this.capabilities |= Capabilities.Movable | Capabilities.Deletable;
    }

    public void Init(SkillTreeGraphView graphView)
    {
        m_OwnerGraphView = graphView;

        RefreshExpandedState();
        RefreshPorts();
    }

    private Port CreatePortFromScriptableObject(FieldInfo fieldInfo, SkillData data)
    {
        if (fieldInfo.FieldType == typeof(Sprite))
        {
            var port = new SpritePort(Direction.Input, Port.Capacity.Single);
            Sprite value = fieldInfo.GetValue(data) as Sprite;
            port.SetValue(value);

            port.onChangeValue = (value) =>
            {
                fieldInfo.SetValue(data, value);
                m_OwnerGraphView.SaveAndRefresh();
            };

            return port;
        }
        else if (fieldInfo.FieldType == typeof(string))
        {
            var port = new StringPort(Direction.Input, Port.Capacity.Single);
            string value = fieldInfo.GetValue(data) as string;
            port.SetValue(value);

            port.onChangeValue = (value) =>
            {
                fieldInfo.SetValue(data, value);
                m_OwnerGraphView.SaveAndRefresh();
            };

            return port;
        }
        else if (fieldInfo.FieldType == typeof(int))
        {
            var port = new IntPort(Direction.Input, Port.Capacity.Single);
            port.portName = fieldInfo.Name.ToCapitalizeFirst();
            int value = (int)fieldInfo.GetValue(data);
            port.SetValue(value);

            port.onChangeValue = (value) =>
            {
                fieldInfo.SetValue(data, value);
                m_OwnerGraphView.SaveAndRefresh();
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

    public virtual void Save()
    {
    }
    public virtual void Refresh()
    {
    }

    public void DeletePort(Port port)
    {
        var edgesToDelete = new List<Edge>();
        edgesToDelete.AddRange(port.connections);

        foreach (var edge in edgesToDelete)
        {
            edge.input.Disconnect(edge);
            edge.output.Disconnect(edge);
            
            // remove element
            m_OwnerGraphView.RemoveElement(edge);
        }

        (port.parent as VisualElement)?.Remove(port);

        // update view
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

            m_OwnerGraphView.DeleteElements(port.connections);
        }
    }
}
#endif