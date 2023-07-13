using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningBalloons : MonoBehaviour
{
    public Cable_Procedural_Simple cableScript;
    public Transform cableEnd;

    Hover hoverScript;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.OPENING, FlyAway);
    }

    private void Start()
    {
        hoverScript = GetComponent<Hover>();
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.OPENING, FlyAway);
    }

    void FlyAway()
    {
        StartCoroutine(FlyInRandomDirection());
    }

    IEnumerator FlyInRandomDirection()
    {
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), Random.Range(-1f, 1f)).normalized;
        float speed = 0;
        int counter = 0;
        cableScript.endPointTransform = cableEnd;
        Destroy(hoverScript);
        while (counter < 50000)
        {
            speed++;
            transform.Translate(direction * speed * Time.deltaTime);
            counter++;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);
    }
}
