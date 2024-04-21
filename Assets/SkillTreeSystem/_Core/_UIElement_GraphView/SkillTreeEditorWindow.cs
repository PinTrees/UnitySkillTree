using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
#endif

#if UNITY_EDITOR
/*
 * skill tree editor window
 */
public class SkillTreeEditorWindow : EditorWindow
{
    /*
     * data
     */
    private SkillTreeData m_CurrentSkillTreeData;
    private SkillTreeGraphView m_GraphView;
    private SkillTreeUIGraphView m_UIGrahpView;


    public static void Open(SkillTreeData data)
    {
        var window = GetWindow<SkillTreeEditorWindow>();
        window.titleContent = new GUIContent("SkillTree Editor");
        window.m_CurrentSkillTreeData = data;
        window.Init();
        window.Repaint();
    }

    private void Init()
    {
        if (m_CurrentSkillTreeData != null)
        {
            SetupGraphView();
            LoadData();

            SwitchView("Nodes");
        }
    }

    private void LoadData()
    {
        foreach(var skillContainer in m_CurrentSkillTreeData._Editor_GetSKillDatas())
        {
            if (skillContainer == null) return;

            m_GraphView.AddNode(skillContainer);
            m_UIGrahpView.AddNode(skillContainer);
        }

        foreach(var skillConnectData in m_CurrentSkillTreeData._Editor_GetSkillConnectDatas())
        {
            if (skillConnectData == null) return;

            m_GraphView.AddEdge(skillConnectData);
            m_UIGrahpView.AddEdge(skillConnectData);
        }

        m_GraphView.Refresh();
        m_UIGrahpView.Refresh();
    }

    private void SaveData()
    {
        m_GraphView.SaveAndRefresh();
        m_UIGrahpView.SaveAndRefresh();
    }

    public void CreateGUI()
    {
    }

    private void OnEnable()
    {
    }

    /*
     * lazy initialization
     */
    private void SetupGraphView()
    {
        m_GraphView = new SkillTreeGraphView(m_CurrentSkillTreeData);
        m_GraphView.name = "SkillTree Graph Editor";
        m_GraphView.StretchToParentSize();

        m_UIGrahpView = new SkillTreeUIGraphView(m_CurrentSkillTreeData);
        m_UIGrahpView.name = "SkillTree UI Graph Editor";
        m_UIGrahpView.StretchToParentSize();

        // add left panel
        var leftPanelScrollView = new ScrollView();
        leftPanelScrollView.name = "LeftPanelScrollView";
        leftPanelScrollView.verticalScrollerVisibility = ScrollerVisibility.Auto;
        leftPanelScrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;

        // set style
        leftPanelScrollView.style.width = 180; // 패널의 너비 설정
        leftPanelScrollView.style.paddingTop = 8;
        leftPanelScrollView.style.paddingRight = 8;
        leftPanelScrollView.style.paddingLeft= 8;
        leftPanelScrollView.style.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f); 

        leftPanelScrollView.style.borderRightColor = new Color(0.1f, 0.1f, 0.1f, 1f);
        leftPanelScrollView.style.borderRightWidth = 1.4f;
        
        // add create skill node button
        var addButton = new Button(() => { m_GraphView.AddNode("SKill Node"); }) { text = "Add Node" };
        leftPanelScrollView.Add(addButton);

        // add save button
        var saveButton = new Button(() => { SaveData(); }) { text = "Save" };
        leftPanelScrollView.Add(saveButton);

        // ScrollView와 GraphView를 rootVisualElement에 추가
        var mainContainer = new VisualElement();
        mainContainer.style.flexDirection = FlexDirection.Row;
        mainContainer.style.flexGrow = 1; 
        mainContainer.Add(m_GraphView);
        mainContainer.Add(m_UIGrahpView);
        mainContainer.Add(leftPanelScrollView);

        var toolbar = new Toolbar();
        toolbar.style.paddingBottom = 2;
        toolbar.style.paddingLeft = 2;
        toolbar.style.paddingRight = 2;
        toolbar.style.paddingTop = 2;
        toolbar.style.height = 32;

        var nodesButton = new Button(() => SwitchView("Nodes"));
        nodesButton.text = "Nodes";
        nodesButton.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
        toolbar.Add(nodesButton);

        var settingsButton = new Button(() => SwitchView("Positioning"));
        settingsButton.text = "Positioning";
        settingsButton.style.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
        toolbar.Add(settingsButton);

        rootVisualElement.Add(toolbar);


        rootVisualElement.Add(mainContainer);
        rootVisualElement.style.flexDirection = FlexDirection.Column;
        rootVisualElement.style.flexGrow = 1; 
    }



    private void SwitchView(string viewName)
    {
        // 모든 화면 컨텐츠를 숨깁니다
        m_GraphView.visible = false;
        m_UIGrahpView.visible = false;

        m_GraphView.Refresh();
        m_UIGrahpView.Refresh();

        switch (viewName)
        {
            case "Nodes":
                m_GraphView.visible = true;
                break;
            case "Positioning":
                m_UIGrahpView.visible = true;
                break;
            default:
                break;
        }
    }
}
#endif
