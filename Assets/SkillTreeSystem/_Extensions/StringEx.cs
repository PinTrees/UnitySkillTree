using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public static class StringEx
{
    public static string ToCapitalizeFirst(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return str;
        }

        return char.ToUpper(str[0]) + str.Substring(1).ToLower();
    }
}
