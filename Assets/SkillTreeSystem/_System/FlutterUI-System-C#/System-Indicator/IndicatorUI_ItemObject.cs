using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class IndicatorUI_ItemObject : IndicatorUIBase
{
    ContainerUI ui;


    /*public static IndicatorUI_ItemObject Create(Interactable interactor)
    {
        var go = new GameObject("IndicatorUI-Row");
        go.AddComponent<RectTransform>();

        var ui = go.AddComponent<IndicatorUI_ItemObject>();

        List<UIObjectBase> interactDataChildren = new();
        foreach (var interactData in interactor.GetInteractableData())
        {
            var rowStyle = new RowStyle();
            rowStyle.fitWidth = true;
            rowStyle.fitHeight = true;
            rowStyle.spacing = 16;

            var textButtonStyle = new TextButtonStyle();
            textButtonStyle.textStyle = new TextStyle();
            textButtonStyle.elevationStyle = new();
            textButtonStyle.height = 32;
            textButtonStyle.width = 32;
            textButtonStyle.elevationStyle.color = Color.black;
            textButtonStyle.elevationStyle.elevation = 18;

            interactDataChildren.Add(
                RowUI.Create(
                    style: rowStyle,
                    children: new List<UIObjectBase>() {
                        TextButtonUI.Create(
                            textButtonStyle,
                            interactData.InteractKey.ToString()
                        ),
                        TextUI.Create(
                            new xTextStyle(overflow: true, bold: true),
                            interactData.InteractionName
                        ),
                    }
                )
            ); ;
        }

        var containerStyle = new ContainerStyle();
        containerStyle.backgroundImageColor = new Color(0.3f, 0.3f, 0.3f, 1f);
        containerStyle.fitWidth = true;
        containerStyle.fitHeight = true;

        var columnStyle = new ColumnStyle();
        columnStyle.fitHeight = true;
        columnStyle.fitWidth = true;
        columnStyle.spacing = 4;

        ui.ui = ContainerUI.Create(
            containerStyle,
            parent: go,
            padding: new Padding(12, 12, 12, 12),
            child: ColumnUI.Create(
                style: columnStyle,
                children: interactDataChildren
            )
        );

        return ui;
    }*/


    public override void InitializeUI()
    {
        base.InitializeUI();
    }

    public override void CloseUI()
    {
        base.CloseUI();
    }
}
