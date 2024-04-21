using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IUIStateBase
{
    void Show();
    void Close();

    void EnterState();
    void ExitState();
}


/// <summary>
/// UI의 상태를 관리하는 클래스입니다.
/// 이 클래스는 FSM 패턴의 한 상태(State)로서 동작하며, 특정 UI 화면을 나타내는 기능을 가집니다.
/// 이 클래스를 상속받아 각각의 UI 화면(예: 메인 메뉴, 설정, 게임 오버 화면 등)에 맞는 구체적인 UI 요소들을 구현할 수 있습니다.
/// 
/// Android UI 시스템과, Flutter의 Scaffold와 비슷한 형태로 구축합니다.
/// </summary>
public class UIStateWidget : MonoBehaviour, IUIStateBase
{
    [SerializeField] public GameObject baseObject;

    public UIStateMachine context { get; private set; }
    public UIStateType stateType;


    /// <summary>
    /// 컨텍스트 설정을 위한 메서드입니다.
    /// </summary>
    /// <param name="context"></param>
    public void SetContext(UIStateMachine context)
    {
        this.context = context;
    }


    /// <summary>
    /// 현재 상태와 주어진 상태가 동일한지 확인합니다.
    /// </summary>
    /// <param name="otherState"></param>
    /// <returns></returns>
    public bool IsEquals(UIStateType otherState) => stateType.Equals(otherState);

    /// <summary>
    /// 현재 상태와 주어진 상태가 다른지 확인합니다.
    /// </summary>
    /// <param name="otherState"></param>
    /// <returns></returns>
    public bool IsNotEquals(UIStateType otherState) => !stateType.Equals(otherState);


    #region Override Methode
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void Show() => baseObject.SetActive(true);
    public virtual void Close() => baseObject.SetActive(false);
    #endregion
}
