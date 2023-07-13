using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonEffects : MonoBehaviour
{
    public ParticleSystem collectpartFX;
    public AudioSource collectSFX;

    void Awake()
    {
        Messenger.AddListener(GameEvent.BALLOON_GET, CollectFXPlay);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.BALLOON_GET, CollectFXPlay);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CollectFXPlay()
    {
        collectpartFX.Play();
        collectSFX.Play();
    }
}
