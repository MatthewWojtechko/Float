using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the growing / shrinking animation for when the balloon nears a target balloon. 
public class Stretcher : MonoBehaviour
{
    public Animator anim;
    public ParticleSystem enterEffect;
    public ParticleSystem nearingEffect;
    public float nearingFactor = 1;
    public GameObject currentClosest;
    public float distance;

    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentClosest != null)
        {
            distance = Vector3.Distance(this.transform.position, currentClosest.transform.position);
            speed = getSpeed(distance);
            anim.SetFloat("speed", speed);
            var nearEmission = nearingEffect.emission;
            nearEmission.rateOverTime = speed * nearingFactor;
        }
        else
            anim.SetFloat("speed", 0);
    }

    public float getSpeed(float distance)
    {
        float result = 25.0f / distance;
        if (result > 1.5f)
            return 1.5f;
        return result;
    }

    public void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "TargetBalloonZone")
        {
            if (!col.gameObject.GetComponent<BalloonMusicCues>().isAlive)  // if this one has been killed...
            {
                if (currentClosest == col.gameObject)
                {
                    currentClosest = null;
                    nearingEffect.Stop();
                }
            }
            // if not in range of anything currently or if this one is closer than the current one...
            else if ((currentClosest == null) || (Vector3.Distance(this.transform.position, col.gameObject.transform.position) < Vector3.Distance(this.transform.position, currentClosest.transform.position)))
            {
                if (currentClosest != col.gameObject)   // if entering 
                {
                    enterEffect.Play();
                    nearingEffect.Play();
                }
                currentClosest = col.gameObject;
            }
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "TargetBalloonZone")
        {
            if (col.gameObject == currentClosest)
            {
                currentClosest = null;
                nearingEffect.Stop();
            }
        }
    }
}
