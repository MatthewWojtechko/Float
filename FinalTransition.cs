using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalTransition : MonoBehaviour
{
    public float waitTime = 2;
    public BalloonFly playerScript;
    public AudioSource normLevelCue;
    private bool isWaiting = false;
    private bool ready = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ready && !playerScript.relocating && !normLevelCue.isPlaying)
        {
            StartCoroutine(waitBroadcast());
            ready = false;
        }

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (!isWaiting)
                ready = true;
        }
    }

    IEnumerator waitBroadcast()
    {
        isWaiting = false;
        yield return new WaitForSeconds(waitTime);
        Messenger.Broadcast(GameEvent.FINAL_TRANSITION);
        Destroy(gameObject);
    }
}
