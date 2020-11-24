using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public Transform[] waypoints;
    [SerializeField]
    private float velocity = 1f;
    [HideInInspector]
    public int waypointIndex = 0;

    public bool moveAllowed = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;   
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAllowed)
            Move();
    }

    void Move()
    {
        if(waypointIndex <= waypoints.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position,
                waypoints[waypointIndex].transform.position,
                velocity * Time.deltaTime);

            if(transform.position == waypoints[waypointIndex].transform.position)
            {
                waypointIndex += 1;
            }
        }
    }
}
