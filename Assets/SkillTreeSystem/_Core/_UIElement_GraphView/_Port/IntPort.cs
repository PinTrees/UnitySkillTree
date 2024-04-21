using System;
using UnityEngine.UIElements;

using Action = System.Action;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

#if UNITY_EDITOR
public class IntPort : Port
{
    public IntegerField m_IntField;
    public Action<int> onChangeValue;


    public IntPort(Direction portDirection, Port.Capacity capacity)
        : base(portOrientation: Orientation.Horizontal,
               portDirection,
               capacity,
               type: typeof(int))
    {
        m_IntField = new IntegerField();
        m_IntField.RegisterValueChangedCallback(evt =>
        {
            int val = evt.newValue; 
            onChangeValue?.Invoke(val);
        });
        m_IntField.style.width = 80;

        this.contentContainer.Add(m_IntField);
    }

    public void SetValue(int value)
    {
        m_IntField.SetValueWithoutNotify(value);
    }
}
#endif