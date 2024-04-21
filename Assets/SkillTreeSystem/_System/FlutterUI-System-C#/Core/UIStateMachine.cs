using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIStateMachine : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private List<UIStateWidget> states = new();

    private readonly Stack<UIStateWidget> stateStack = new();
    private Dictionary<string, UIStateWidget> stateDictionary = new();    // 모든 UI 상태를 저장하는 딕셔너리


    protected virtual void Start()
    {
        InitializeStates();
    }

    private void InitializeStates()
    {
        foreach (var state in states)
        {
            stateDictionary[state.gameObject.name] = state;
            state.ExitState();  // 초기에는 모든 상태를 비활성화
        }
    }


    #region Navigation Page
    /// <summary>
    /// 새로운 상태로 전환하는 메서드
    /// </summary>
    /// <param name="stateType"></param>
    public void NavigateTo(UIStateType stateType)
    {
        if (stateDictionary.TryGetValue(stateType.Name, out var nextState))
        {
            if (stateStack.Count > 0)
            {
                stateStack.Peek().ExitState();  // 현재 상태 종료
            }

            stateStack.Push(nextState);
            nextState.EnterState();             // 새로운 상태 시작
        }
        else
        {
            Debug.LogError($"State {stateType.Name} not found in the dictionary.");
        }
    }

    /// <summary>
    /// 이전 상태로 돌아가는 메서드
    /// </summary>
    public void GoBack()
    {
        if (stateStack.Count <= 1) return;      // 스택에 하나만 있으면 돌아갈 곳이 없음

        var currentState = stateStack.Pop();
        currentState.ExitState();               // 현재 상태 종료

        var previousState = stateStack.Peek();
        previousState.EnterState();             // 이전 상태 시작
    }
    #endregion



    protected virtual void Update()
    {
        
    }
}
