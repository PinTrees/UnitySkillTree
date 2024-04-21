using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MdiController : Singleton<MdiController>
{
    [Header("runtime value")]
    [field: SerializeField]
    public Canvas canvas { get; private set; }

    [SerializeField] MdiWindowData testMdiData;
    [SerializeField] List<MdiWindow> windows = new();


    protected override void Awake()
    {
        if(canvas == null)
        {
            canvas = GetComponent<Canvas>();
            if(canvas == null)
            {
                canvas = gameObject.GetComponentInParent<Canvas>();
            }
        }

        windows = gameObject.GetComponentsInChildren<MdiWindow>().ToList();
        windows.ForEach(e => e.Init());
    }


    public void AddMdiWindow(MdiWindow window)
    {
        window.transform.SetParent(gameObject.transform, true);
        window.transform.localScale = Vector3.one;

        windows.Add(window);
    }
    public void RemoveMdiWindow(MdiWindow window)
    {
        window.CloseUI();
        //Destroy(window.gameObject);
        //windows.Remove(window);
    }





    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            var m = MdiWindow._(testMdiData);
        }
    }
}
