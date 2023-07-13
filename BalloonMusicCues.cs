using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonMusicCues : MonoBehaviour
{
    public AudioSource primaryAlert;
    public AudioSource[] cues;
    public float minWait;
    public float maxWait;
    public SphereCollider collider;
    public bool isAlive = true;

    private bool isInZone;
    private bool waitPlayCalled;
    private bool timeToDie;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isInZone && !waitPlayCalled && isAlive)
            StartCoroutine(waitPlay());
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (!primaryAlert.isPlaying && isAlive)
                primaryAlert.Play();
            isInZone = true;
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            isInZone = false;
        }
    }

    public void die()
    {
        timeToDie = true;
        isAlive = false;
        //GetComponent<SphereCollider>().enabled = false;
        //StartCoroutine(waitDie());
    }

    //private IEnumerator waitDie()
    //{
    //    yield return new WaitForSeconds(3);
    //    Destroy(this);
    //}



    private IEnumerator waitPlay()
    {
        waitPlayCalled = true;
        yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        if (isInZone && !timeToDie)
            cues[Random.Range(0, cues.Length)].Play();
        waitPlayCalled = false;
    }
}
