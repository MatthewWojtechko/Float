using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Activates the gameObjects in the array in order they are placed.
public class PhaseIn : MonoBehaviour
{
    public GameObject[] items;
    public float[] timeBetween;

    private bool waitOver = true;
    private int index;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (waitOver && index < items.Length)
        {
            StartCoroutine(waitAppear(items[index], timeBetween[index]));
            index++;
        }
        if (index >= items.Length)
        {
            Destroy(this);
        }
    }

    private IEnumerator waitAppear(GameObject G, float t)
    {
        waitOver = false;
        G.SetActive(true);
        yield return new WaitForSeconds(t);
        waitOver = true;
    }
}
