using System;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor.Experimental.GraphView;
using ObjectField = UnityEditor.UIElements.ObjectField;
#endif

using Action = System.Action;

#if UNITY_EDITOR
/*
 * graph view custom port
 */
public class SpritePort : Port
{
    public Sprite m_Sprite;
    public ObjectField m_ObjectField;
    public Action<Sprite> onChangeValue;


    public SpritePort(Direction portDirection, Port.Capacity capacity)
        : base(portOrientation: Orientation.Horizontal,
               portDirection,
               capacity,
               type: typeof(Sprite))
    {
        // Sprite 선택을 위한 ObjectField 생성
        m_ObjectField = new ObjectField
        {
            objectType = typeof(Sprite),
            allowSceneObjects = false, 
            value = null 
        };
        m_ObjectField.RegisterValueChangedCallback(evt =>
        {
            Sprite sprite = null;

            if(evt.newValue != null)
            {
                sprite = evt.newValue as Sprite;
            }

            m_Sprite = sprite;

            if (onChangeValue != null)
            {
                onChangeValue(sprite);
            }
        });
        m_ObjectField.style.width = 80;

        this.contentContainer.Add(m_ObjectField);
    }

    public void SetValue(Sprite sprite)
    {
        m_ObjectField.SetValueWithoutNotify(sprite);
        m_Sprite = sprite;
    }
}
#endif