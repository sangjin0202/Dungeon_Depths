using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class State<T> where T : class // T�� �� �� �ִ� Ÿ���� class�� �����ϵ���
{
    /// <summary>
    /// �ش� ���¸� ������ �� 1ȸ ȣ��
    /// </summary>
    public abstract void Enter(T _entity);
    /// <summary>
    /// �ش� ���¸� ������Ʈ�� �� �� ������ ȣ��
    /// </summary>
    public abstract void Execute(T _entity);
    /// <summary>
    /// �ش� ���¸� ������ �� 1ȸ ȣ��
    /// </summary>
    public abstract void Exit(T _entity);
}


public class StateMachine<T> where T : class
{
    T ownerEntity;          //StateMachine ������
    public Dictionary<int, State<T>> states = new Dictionary<int, State<T>>(); //Key��: ���� enum�� | value��: �ൿ �Լ��� ������ ���� Ŭ����
    State<T> previousState; 
    State<T> currentState;  
    public void InitState(T _owner, State<T> _entryState)   //ù �Ű������� this
    {
        ownerEntity = _owner;
        currentState = null;
        //entryState ���·� ���� ����
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
            State<T> state = _state;
            states.Add(_enumState, _state);
        }
    }

    public void Execute()   //Update: Agent�� �ൿ�� �������� ����ϱ� ���� Agent�� Update���� ȣ��
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