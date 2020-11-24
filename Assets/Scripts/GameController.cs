using Assets.Scripts.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        private GameObject[] playerObjects;
        private Player[] players;
        private int _noOfPlayers;
        public int NoOfPlayers
        {
            get { return _noOfPlayers; }
            set { _noOfPlayers = value; }
        }

        private bool isGameOver = false;
        private GameController()
        {
            playerObjects = new GameObject[Constants.MAX_TOKENS_PER_PLAYER];
            players = new Player[Constants.MAX_TOKENS_PER_PLAYER];
        }

        private void Start()
        {
            FindAllPlayers();
            players[0].SetPlayerReady();

        }

        private void FindAllPlayers()
        {
            for (int i = 0; i < 4; i++)
            {
                playerObjects[i] = GameObject.Find(i.ToString("Player" + (i + 1).ToString()));
                players[i] = (Player)playerObjects[i].GetComponent(typeof(Player));
                players[i].PlayerNumber = i;
                players[i].TurnEndEvent += OnTurnEndEvent;
            }
        }

        private void OnTurnEndEvent(object sender, NumberEventArgs args)
        {
            players[args.Number].SetPlayerSteady();
            if (args.Number == 3)
                players[0].SetPlayerReady();

            else
                players[args.Number + 1].SetPlayerReady();
        }

    }
}
