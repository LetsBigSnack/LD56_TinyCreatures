using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum State
{
    MainMenu,
    Game
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private State _currentState = State.MainMenu;

    public State CurrentState
    {
        get => _currentState;
        set => _currentState = value;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(string sceneName)
    {
        switch (sceneName)
        {
            case "DekisScene":
                _currentState = State.Game;
                break;
            default:
                _currentState = State.MainMenu;
                BattleManager.Instance.StopBattle();
                BattleManager.Instance.BattleRunning = true;
                BattleManager.Instance.HasBattleStarted = false;
                break;
        }
    }
}
