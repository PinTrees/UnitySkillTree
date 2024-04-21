using System;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
#endif

using Action = System.Action;

#if UNITY_EDITOR
public class StringPort : Port
{
    public TextField m_TextField;
    public Action<string> onChangeValue;


    public StringPort(Direction portDirection, Port.Capacity capacity)
        : base(portOrientation: Orientation.Horizontal,
               portDirection,
               capacity,
               type: typeof(string))
    {
        m_TextField = new TextField("");
        m_TextField.RegisterValueChangedCallback(evt =>
        {
            string val = evt.newValue; // Directly assign, no need for 'as string'
            onChangeValue?.Invoke(val);
        });
        m_TextField.style.width = 80;

        this.contentContainer.Add(m_TextField);
    }

    public void SetValue(string value)
    {
        m_TextField.SetValueWithoutNotify(value);
    }
}
#endif