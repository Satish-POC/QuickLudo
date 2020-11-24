using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework;
public class Player : MonoBehaviour
{
    private GameObject[] tokenObjects;
    private Token[] tokens;
    public Dice dice;
    private int _playerNumber = 0;

    public int PlayerNumber
    {
        get
        {
            return _playerNumber;
        }
        set
        {
            _playerNumber = value;
        }
    }

    public event EventHandler<NumberEventArgs> TurnEndEvent;
    //private bool _disabled;
    //public bool Disabled
    //{
    //    get { return _disabled; }
    //    set 
    //    {
    //        dice.CanRoll = !value;
    //        _disabled = value; 
    //    }
    //}

    private Enums.PlayerResult _result;
    public Enums.PlayerResult Result
    {
        get 
        { 
            return _result; 
        }
        set
        {
            _result = value;
        }
    }

    private Enums.PlayerStatus _status;
    public Enums.PlayerStatus Status
    {
        get
        {
            return _status;
        }
        set
        {
            _status = value;
        }
    }

    Player()
    {
        tokenObjects = new GameObject[Constants.MAX_TOKENS_PER_PLAYER];
        tokens = new Token[Constants.MAX_TOKENS_PER_PLAYER];
        _status = Enums.PlayerStatus.Playing;
        _result = Enums.PlayerResult.InProgress;


    }

    private void Awake()
    {
        dice = (Dice)(transform.Find("Dice").gameObject).GetComponent(typeof(Dice));
    }
    // Start is called before the first frame update
    void Start()
    {

        FindAllTokens();
        dice.RaiseDiceRollEvent += OnDiceRollEvent;

        GoToBaseAllTokens();
    }

    public void SetPlayerReady()
    {
        //dice.ConsecutiveSixes = 0;
        dice.CanRoll = true;
    }

    public void SetPlayerSteady()
    {
        LockTokens(-1);
    }
    private void FindAllTokens()
    {
        for (int i = 0; i < Constants.MAX_TOKENS_PER_PLAYER; i++)
        {
            tokenObjects[i] = transform.Find(i.ToString("Token" + (i + 1).ToString())).gameObject;
            tokens[i] = (Token)tokenObjects[i].GetComponent(typeof(Token));
            tokens[i].TokenNumber = i;
            tokens[i].RaiseTokenSelectionEvent += OnTokenSelectionEvent;
            tokens[i].RaiseTokenMoveCompletedEvent += OnTokenMoveCompletedEvent;
        }
    }

    private void GoToBaseAllTokens()
    {
        for(int i=0; i< Constants.MAX_TOKENS_PER_PLAYER; i++)
        {
                tokens[i].GoToBase();
        }
    }

    private void OnDiceRollEvent(object sender, NumberEventArgs args)
    {
        UnlockTokens(args.Number);
    }

    private void UnlockTokens(int diceResult)
    {
        bool atLeastOneTokenIsUnlocked = false;
        for (int i = 0; i < Constants.MAX_TOKENS_PER_PLAYER; i++)
        {
            if ((tokens[i].Status == Enums.ToeknStatus.Base && Config.OPENING_NUMBERS.Contains(diceResult))
                || (tokens[i].Status != Enums.ToeknStatus.Base && tokens[i].waypointIndex < (57 - diceResult)))
            {
                tokens[i].Locked = false;
                //tokenObjects[i].layer = 10;
                atLeastOneTokenIsUnlocked = true;
            }
        }
        if (!atLeastOneTokenIsUnlocked && !Config.OPENING_NUMBERS.Contains(diceResult))
        {
            OnTurnEnd(new NumberEventArgs(_playerNumber));
        }
    }

    private void OnTokenSelectionEvent(object sender, NumberEventArgs args) 
    {
        LockTokens(args.Number);
        tokens[args.Number].MoveSteps(dice.Value);
    }

    private void OnTokenMoveCompletedEvent(object sender, EventArgs args)
    {
        OnTurnEnd(new NumberEventArgs(_playerNumber));      
    }

    private void LockTokens(int unlockedToken)
    {
        foreach(Token t in tokens)
        {
            t.Locked = (t.TokenNumber == unlockedToken) ? false : true;
            //tokenObjects[t.TokenNumber].layer = 5;
        }
    }

    protected virtual void OnTurnEnd(NumberEventArgs args)
    {
        TurnEndEvent?.Invoke(this, args);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
