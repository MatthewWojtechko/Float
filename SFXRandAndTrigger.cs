using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXRandAndTrigger : MonoBehaviour
{
    public AudioSource SFX;
    public float minWait;
    public float maxWait;
    private bool playerInRange = false;
    private bool isWaiting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !isWaiting)    // plays after random periods while player in range
            StartCoroutine(waitPlay());
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
            playerInRange = true;
        if (!SFX.isPlaying)
            SFX.Play();     // plays as soon as player in range
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
            playerInRange = false;
    }

    private IEnumerator waitPlay()
    {
        isWaiting = true;
        yield return new WaitForSeconds(Random.Range(minWait, maxWait));
        if (playerInRange && !SFX.isPlaying)
            SFX.Play();
        isWaiting = false;
    }


}
