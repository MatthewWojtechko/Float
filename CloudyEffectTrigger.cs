using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudyEffectTrigger : MonoBehaviour
{
    public ParticleSystem effect;
    public GameObject groundWorld;
    public GameObject SkyWorld;

    public float worldWait;
    public float cloudEffectGoneWait;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            effect.Play();
            StartCoroutine(swapWorlds());
            StartCoroutine(removeCloudy());
        }
    }

    IEnumerator swapWorlds()
    {
        yield return new WaitForSeconds(worldWait);
        groundWorld.SetActive(false);
        SkyWorld.SetActive(true);
    }

    IEnumerator removeCloudy()
    {
        yield return new WaitForSeconds(cloudEffectGoneWait);
        effect.Stop();
        yield return new WaitForSeconds(2);
    }
}
