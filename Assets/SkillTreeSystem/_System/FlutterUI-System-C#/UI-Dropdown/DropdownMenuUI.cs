using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DropdownMenuUI : UIObjectBase
{
    public Image backgroundImage;
    public float width;

    List<DropdownMenuButtonUI> dropdownMenuButtons = new();
    DropdownMenuStyle style;


    public static DropdownMenuUI Create(DropdownMenuStyle style)
    {
        if(style == null)
        {
            style = UIStyleHub.Instance.GetStyle<DropdownMenuStyle>(UIStyle.DropdownMenuStyle1);
        }

        var go = new GameObject("DropdownMenuUI");
        go.transform.SetParent(GameObject.FindAnyObjectByType<Canvas>().transform, true);
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = Vector3.zero;  
        go.transform.SetAsLastSibling();

        var dropdownMenuUI = go.AddComponent<DropdownMenuUI>();
        dropdownMenuUI.style = style;
        dropdownMenuUI.backgroundImage = UICreator.CreateImageComponent(go);

        dropdownMenuUI.InitializeUI();
        dropdownMenuUI.SetWidth(style.width);
        dropdownMenuUI.backgroundImage.sprite = style.backgroundImage;
        dropdownMenuUI.backgroundImage.color = style.backgroundColor;

        dropdownMenuUI.rectTransform.SetAncherLeftTop();

        dropdownmenu_manager.Instance.Clear();
        dropdownmenu_manager.Instance.AddDropdownMenu(dropdownMenuUI);

        return dropdownMenuUI;
    }


    public override void InitializeUI()
    {
        base.InitializeUI();
    }
   
    public void SetPosition(Vector2 position)
    {
        rectTransform.position = position;
    }
    public void SetWidth(float width)
    {
        this.width = width;
        rectTransform.sizeDelta = new Vector2(width, 100);
    }

    public void SetDropdownMenuButton(List<DropdownMenuButtonParam> param)
    {
        param.ForEach(e =>
        {
            var ui = DropdownMenuButtonUI.Create(this, style.dropdownMenuButtonStyle, e);
            dropdownMenuButtons.Add(ui);
        });

        var height = (style.dropdownMenuButtonStyle.height + style.heightSpacing) * param.Count - style.heightSpacing;
        rectTransform.sizeDelta = new Vector2(width, height);
    }
}
