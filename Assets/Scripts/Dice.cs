using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework;
using System;

public class Dice : MonoBehaviour
{
    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private bool _canRoll = false;
    private int _consecutiveSixes = 0;
    public int ConsecutiveSixes 
    {
        get { return _consecutiveSixes; }
        set { _consecutiveSixes = value; }
    }

    public bool CanRoll 
    {
        get
        {
            return _canRoll;
        }
        set
        {
            _canRoll = value;
        }
    }

    private int _diceResult = 0;
    public event EventHandler<NumberEventArgs> RaiseDiceRollEvent;

    public int Value {
        get
        {
            return _diceResult;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSlides/");
        rend.sprite = diceSides[5];
    }

    void OnMouseDown()
    {
        if (!GameControl.gameOver && _canRoll)
        {
            Roll();
        }
    }

    void Roll()
    {
        StartCoroutine(RollTheDice());
    }

    IEnumerator RollTheDice()
    {
        _canRoll = false;
        _diceResult = 0;
        int randomDiceSide = 0;
        for(int i=0; i<= 20; i++)
        {
            randomDiceSide = UnityEngine.Random.Range(0, 6);//(_consecutiveSixes > 2)?UnityEngine.Random.Range(0, 5): UnityEngine.Random.Range(0, 6);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }
        //_canRoll = true;
        _diceResult = randomDiceSide + 1;
        if (_diceResult == 6)
            _consecutiveSixes++;
        OnDiceRolled(new NumberEventArgs(_diceResult));

    }

    protected virtual void OnDiceRolled(NumberEventArgs args)
    {
        RaiseDiceRollEvent?.Invoke(this, args);
    }
    // Update is called once per frame

}
