using System.Collections.Generic;
using UnityEngine;

public abstract class State<T> where T : class // T에 올 수 있는 타입은 class만 가능하도록
{
    /// <summary>
    /// 해당 상태를 시작할 때 1회 호출
    /// </summary>
    public abstract void Enter(T _entity);
    /// <summary>
    /// 해당 상태를 업데이트할 때 매 프레임 호출
    /// </summary>
    public abstract void Execute(T _entity);
    /// <summary>
    /// 해당 상태를 종료할 때 1회 호출
    /// </summary>
    public abstract void Exit(T _entity);
}

public class StateMachine<T> where T : class
{
    T ownerEntity;    //StateMachine 소유주
    public Dictionary<int, State<T>> states = new Dictionary<int, State<T>>(); //Key값: 상태 enum형 | value값: 행동 함수들 구현된 상태 클래스
    State<T> previousState; public State<T> PreviousState { get { return previousState; } }
    State<T> currentState; public State<T> CurrentState { get { return currentState; } }

    public void InitState(T _owner, State<T> _entryState)   //첫 매개변수는 this
    {
        ownerEntity = _owner;
        currentState = null;
        //entryState 상태로 상태 변경
        ChangeState(_entryState);
    }
    public State<T> GetState(int _enumState)
    {
        if (states.ContainsKey(_enumState))
            return states[_enumState];
        else
        {
            Debug.LogError("no exist state");
            return null;
        }
    }

    public void AddState(int _enumState, State<T> _state)
    {
        if (states.ContainsKey(_enumState))
        {
            Debug.LogError("Already added state " + _enumState);
        }
        else
        {
            states.Add(_enumState, _state);
        }
    }

    public void Execute()   //Update: Agent의 행동을 매프레임 재생하기 위해 Agent의 Update에서 호출
    {
        if (currentState != null)
        {
            currentState.Execute(ownerEntity);
        }
    }

    public void ChangeState(State<T> _newState)
    {
        if (_newState == null)
        {
            Debug.LogError(_newState + "is null");
            return;
        }

        if (currentState != null)
        {
            previousState = currentState;
            currentState.Exit(ownerEntity);
        }
        currentState = _newState;
        currentState.Enter(ownerEntity);
    }

    public void RevertToPreviousState()
    {
        ChangeState(previousState);
    }
}