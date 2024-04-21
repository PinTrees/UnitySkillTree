using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIStateType
{
    public string Name { get; private set; }
    public int Value { get; private set; }


    public UIStateType()
    {
    }
    public UIStateType(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public override string ToString() => Name;
}
