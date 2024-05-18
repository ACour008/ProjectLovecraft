using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter();
    void Exit();
    void Update();
}

public class EnemyBehaviourState
{
    IState _currentState;
    public IState currentState
    {
        get => _currentState;
        private set
        {
            _currentState?.Exit();
            _currentState = value;
            _currentState?.Enter();
        }
    }

    public void ChangeState(IState newState)
    {
        Debug.Log($"Changing state from {currentState} to {newState}");
        currentState = newState;
    }

    public void Update()
    {
        currentState?.Update();
    }
}
