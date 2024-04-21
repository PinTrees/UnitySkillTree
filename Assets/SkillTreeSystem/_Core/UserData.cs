using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserData
{
    public int sp = 1000;
    public UserSkllTreeData skilltree = new();
}

[System.Serializable]
public class UserSkllTreeData
{
    public List<UserSkillData> skills = new();
}

[System.Serializable]
public class UserSkillData
{
    public string uid;
}

