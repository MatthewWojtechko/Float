using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Move between waypoints, waiting at them for brief periods.
 */
public class BunnyMove : MonoBehaviour
{
    public Transform[] waypoints;
    public Animator anim;
    public float speed;
    public float turnSpeed;
    public float lerpConstant;
    public float minPause;
    public float maxPause;
    public float animSpeedFactor = 1;
    [Tooltip("Should start as the waypoint the bunny starts at.")]
    public Transform lastWaypoint;
    [Tooltip("0 = idle; 1 = move; 2")]
    public int state;

    public Transform currentWaypoint;
    private bool pauseStarted = false;
    private bool waypointChosen = false;
    public int lastWaypointIndex = -1;
    public int rand;
   
    // Start is called before the first frame update
    void Start()
    {
        anim.SetFloat("speedFactor", speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0 && !pauseStarted)    // Pause
        {
            anim.SetInteger("state", 0);
            StartCoroutine(pause());
        }
        else if (state == 1)    // Move
        {
            if (!waypointChosen)
            {
                anim.SetInteger("state", 1);
                setWaypoint();
            }
            moveToWaypoint();
        }
    }

    // Moves GameObject toward currentWaypoint. If it reaches it, state is set to 0.
    private void moveToWaypoint()
    {
        float step = speed * Time.deltaTime;
        Quaternion q = Quaternion.LookRotation(currentWaypoint.transform.position - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, q, turnSpeed * Time.deltaTime);
        // animation automatically moves rabbit
        //this.transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, step);
        if (isPastWaypoint())
        {
            state = 0;
            waypointChosen = false;
            lastWaypoint = currentWaypoint;
        }
    }

    // Determines if the Game Object has reached the currentWaypoint (at it, or past it).
    private bool isPastWaypoint()
    {
        if (Mathf.Abs(Vector3.Distance(this.transform.position, lastWaypoint.position)) >= Mathf.Abs(Vector3.Distance(currentWaypoint.position, lastWaypoint.position)))
            return true;
        return false;

        //if (pos.x <= currentWaypoint.position.x + (step) && pos.x >= currentWaypoint.position.x - (step) &&
        //    pos.y <= currentWaypoint.position.y + (step) && pos.y >= currentWaypoint.position.y - (step) &&
        //    pos.z <= currentWaypoint.position.z + (step) && pos.z >= currentWaypoint.position.z - (step))

    }

    // Assigns currentWaypoint to a random one from the waypoints array.
    private void setWaypoint()
    {
        rand = Random.Range(0, waypoints.Length);
        while (rand == lastWaypointIndex)
        {
            rand = Random.Range(0, waypoints.Length);
        }
        lastWaypointIndex = rand;
        currentWaypoint = waypoints[Random.Range(0, waypoints.Length)];
        waypointChosen = true;
    }

    // Waits for a random amount of secodns between minPause and maxPause, then sets state to 1.
    private IEnumerator pause()
    {
        pauseStarted = true;
        yield return new WaitForSeconds(Random.Range(minPause, maxPause));
        state = 1;
        pauseStarted = false;
    }

}
