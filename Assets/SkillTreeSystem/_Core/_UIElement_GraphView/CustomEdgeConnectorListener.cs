using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using static UnityEditor.Experimental.GraphView.Port;
#endif

#if UNITY_EDITOR
public class CustomEdgeConnectorListener : IEdgeConnectorListener
{
    private GraphViewChange m_GraphViewChange;

    private List<Edge> m_EdgesToCreate;

    private List<GraphElement> m_EdgesToDelete;

    public Action<Port, Port> onCreateEdge;


    public CustomEdgeConnectorListener()
    {
        m_EdgesToCreate = new List<Edge>();
        m_EdgesToDelete = new List<GraphElement>();
        m_GraphViewChange.edgesToCreate = m_EdgesToCreate;
    }

    public void OnDropOutsidePort(Edge edge, Vector2 position)
    {
    }

    public void OnDrop(GraphView graphView, Edge edge)
    {
        m_EdgesToCreate.Clear();
        m_EdgesToCreate.Add(edge);
        m_EdgesToDelete.Clear();
        if (edge.input.capacity == Capacity.Single)
        {
            foreach (Edge connection in edge.input.connections)
            {
                if (connection != edge)
                {
                    m_EdgesToDelete.Add(connection);
                }
            }
        }

        if (edge.output.capacity == Capacity.Single)
        {
            foreach (Edge connection2 in edge.output.connections)
            {
                if (connection2 != edge)
                {
                    m_EdgesToDelete.Add(connection2);
                }
            }
        }

        if (m_EdgesToDelete.Count > 0)
        {
            graphView.DeleteElements(m_EdgesToDelete);
        }

        List<Edge> edgesToCreate = m_EdgesToCreate;
        if (graphView.graphViewChanged != null)
        {
            edgesToCreate = graphView.graphViewChanged(m_GraphViewChange).edgesToCreate;
        }

        foreach (Edge item in edgesToCreate)
        {
            graphView.AddElement(item);
            edge.input.Connect(item);
            edge.output.Connect(item);
        }

        if(onCreateEdge != null)
        {
            onCreateEdge(edge.input, edge.output);
        }
    }
}
#endif