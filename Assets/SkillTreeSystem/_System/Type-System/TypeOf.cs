using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public static class TypeOf
{ 
    public static Type GetLastType(object obj)
    {
        Type currentType = obj.GetType();
        Type bottomMostType = currentType;

        foreach (Type subType in currentType.Assembly.GetTypes())
        {
            if (currentType.IsAssignableFrom(subType) && subType != currentType)
            {
                bottomMostType = subType;
            }
        }

        return bottomMostType;
    }
}
