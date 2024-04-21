using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class UIStyleContainer
{
    public string tag;
    public UIStyle style;
}


public class UIStyleHub : Singleton<UIStyleHub>
{
    public Font font;
    public List<UIStyleContainer> uiStyles = new();


    public T GetStyle<T>(string tag) where T : class
    {
        var data = uiStyles.FirstOrDefault(e => e.tag == tag);

        if (data == null)
            return null;

        return data.style as T;
    }
}
