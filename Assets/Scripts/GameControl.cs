using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    private static GameObject whoWinsTextShadow, player1MoveText, player2MoveText;

    private static GameObject player1, player2;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    public static bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        whoWinsTextShadow = GameObject.Find("WhoWinsText");
        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");

        player1 = GameObject.Find("Red1");
        player2 = GameObject.Find("Yellow1");

        player1.GetComponent<PathManager>().moveAllowed = false;
        player2.GetComponent<PathManager>().moveAllowed = false;

        whoWinsTextShadow.gameObject.SetActive(false);
        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (player1.GetComponent<PathManager>().waypointIndex > player1StartWaypoint + diceSideThrown)
        {
            player1.GetComponent<PathManager>().moveAllowed = false;
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(true);
            player1StartWaypoint = player1.GetComponent<PathManager>().waypointIndex - 1;
        }
        if (player2.GetComponent<PathManager>().waypointIndex > player2StartWaypoint + diceSideThrown)
        {
            player2.GetComponent<PathManager>().moveAllowed = false;
            player2MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(true);
            player2StartWaypoint = player2.GetComponent<PathManager>().waypointIndex - 1;
        }

        if (player1.GetComponent<PathManager>().waypointIndex == player1.GetComponent<PathManager>().waypoints.Length)
        {
            whoWinsTextShadow.gameObject.SetActive(true);
            whoWinsTextShadow.GetComponent<Text>().text = "Player1 wins";
            gameOver = true;
        }

        if (player2.GetComponent<PathManager>().waypointIndex == player2.GetComponent<PathManager>().waypoints.Length)
        {
            whoWinsTextShadow.gameObject.SetActive(true);
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(false);
            whoWinsTextShadow.GetComponent<Text>().text = "Player2 wins";
            gameOver = true;
        }
    }

    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove)
        {
            case 1:
                player1.GetComponent<PathManager>().moveAllowed = true;
                break;
            case 2:
                player2.GetComponent<PathManager>().moveAllowed = true;
                break;
        }
    }
}