using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework;
using System;

public class Token : MonoBehaviour
{
    [SerializeField]

    private List<int> _openingNumbers;

    public Transform[] waypoints;

    [SerializeField]
    private float velocity = 2f;
    [HideInInspector]
    public int waypointIndex = 0;
    public event EventHandler<NumberEventArgs> RaiseTokenSelectionEvent;
    public event EventHandler<EventArgs> RaiseTokenMoveCompletedEvent;

    private int _tokenNumber = 0;
    private int _steps = 0;
    private int _stepCounter;
    private bool _isMovable = false;
    
    public int TokenNumber
    {
        get
        {
            return _tokenNumber;
        }
        set
        {
            _tokenNumber = value;
        }
    }

    private bool _locked = false;
    public bool Locked
    {
        get
        {
            return _locked;
        }
        set
        {
            _locked = value;
        }
    }


    private Enums.ToeknStatus _status = Enums.ToeknStatus.Base;
    public Enums.ToeknStatus Status
    {
        get
        {
        
            switch (waypointIndex)
            {
                case 0:
                    _status = Enums.ToeknStatus.Base;
                    break;
                case 1:
                case 8:
                case 13:
                case 21:
                case 26:
                case 34:
                case 39:
                case 47:
                    _status = Enums.ToeknStatus.Safe;
                    break;
                case 57:
                    _status = Enums.ToeknStatus.Home;
                    break;
                default:
                    _status = Enums.ToeknStatus.Risk;
                    break;
            }

            return _status;
        }
    }

    internal void GoToBase()
    {
        transform.position = waypoints[waypointIndex].transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        _status = Enums.ToeknStatus.Base;
    }

    void OnMouseDown()
    {
        if(!_locked)
            OnTokenSelection(new NumberEventArgs(_tokenNumber));
    }
    // Update is called once per frame
    void Update()
    {
        if (_isMovable && _steps > 0)
            Move();
    }


    public void MoveSteps(int noOfSteps)
    {
        if (noOfSteps > 0)
        {
            //call MoveAStep n times
            _isMovable = true;
            _steps = noOfSteps;
            _stepCounter = 1;

            //if token is still in home and if there is 6 or any value eleigible to move the token from base, token is moved by one point.
            if (Config.OPENING_NUMBERS.Contains(noOfSteps) && _status == Enums.ToeknStatus.Base)
            {
                _steps = 1;
            }
            // if token can't be opened because dice didn't have the opening number, we disable to movement.
            else if(_status == Enums.ToeknStatus.Base && !Config.OPENING_NUMBERS.Contains(noOfSteps))
            {
                _isMovable = false;
                _steps = 0;
                _stepCounter = 0;
            }
            //IF approaching home and dice value is more than needed to go to home then diable the token.
            else if(_status != Enums.ToeknStatus.Base && waypointIndex > 57 - noOfSteps)
            {
                _isMovable = false;
                _steps = 0;
                _stepCounter = 0;
            }
        }
        
        //inform player when token has been moved completely.
    }
    void Move()
    {
        if (waypointIndex <= waypoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                waypoints[waypointIndex+_stepCounter].transform.position,
                velocity * Time.deltaTime);
            if(transform.position == waypoints[waypointIndex + _stepCounter].position)
            {
                _stepCounter++;
            }
            if (transform.position == waypoints[waypointIndex + _steps].position)
            {
                waypointIndex = waypointIndex + _steps;
                _isMovable = false;
                _stepCounter = 0;
                _steps = 0;
                _locked = true;
                OnTokenMoveCompleted();
            }
        }
    }

    protected virtual void OnTokenSelection(NumberEventArgs args)
    {
        RaiseTokenSelectionEvent?.Invoke(this, args);
    }
    protected virtual void OnTokenMoveCompleted()
    {
        RaiseTokenMoveCompletedEvent?.Invoke(this, new EventArgs());
    }
}
