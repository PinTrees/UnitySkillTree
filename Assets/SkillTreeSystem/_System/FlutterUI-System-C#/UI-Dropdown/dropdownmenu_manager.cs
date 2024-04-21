using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class dropdownmenu_manager : Singleton<dropdownmenu_manager>
{
    [Header("runtime debug value")]
    [SerializeField] List<DropdownMenuUI> dropdownMenus = new();

    //BackgroundUI background;


    public void AddDropdownMenu(DropdownMenuUI dropdownMenu)
    {
        //background = BackgroundUI.Create(UIStyleHub.Instance.GetStyle<BackgroundStyle>(UIStyle.DropdownSystemBackgroundStyle));
        dropdownMenus.Add(dropdownMenu);
    }

    public void RemoveDropdownMenu(DropdownMenuUI dropdownMenu) 
    {
        dropdownMenus.Remove(dropdownMenu);
    }
    
    public void Clear()
    {
        dropdownMenus.ForEach(e =>
        {
            e.CloseUI();
            Destroy(e.gameObject);
        });

        dropdownMenus.Clear();
    }

    public bool IsMouseOverlapInDropdownMenuUI()
    {
        if (dropdownMenus.Count > 0)
        {
            foreach (DropdownMenuUI dropdownMenu in dropdownMenus)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(dropdownMenu.rectTransform, Input.mousePosition, null))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (IsMouseOverlapInDropdownMenuUI())
                return;

            Clear();
        }
    }
}
