using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [field: SerializeField] public UserData userData { get; private set; }

    public Button openSkillViewButton;
    public UIObjectBase skillTreeViewer;

    /*
     * editor
     */
    [Space]
    [Button("_Editor_UserDataClear")]
    public string __editor_userdata_clear;


    void Start()
    {
        userData = SaveSystem.Load<UserData>("userdata");

        if(userData == null)
        {
            userData = new UserData();
            SaveSystem.Save("userdata", userData);
        }

        skillTreeViewer.CloseUI();

        openSkillViewButton.onClick.RemoveAllListeners();
        openSkillViewButton.onClick.AddListener(() =>
        {
            skillTreeViewer.ShowUI();
        });
    }

    public UserData LoadUserData()
    {
        return userData = SaveSystem.Load<UserData>("userdata");
    }
    public void SaveUserData(UserData data=null)
    {
        if(data == null)
        {
            SaveSystem.Save("userdata", userData);
        }
        else
        {
            SaveSystem.Save("userdata", data);
        }
    }

#if UNITY_EDITOR
    public void _Editor_UserDataClear()
    {
        userData = new UserData();
        userData.sp = 1000;
        SaveSystem.Save("userdata", userData);
    }
#endif
}
