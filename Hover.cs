using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [Header("Vertical Floating")]
    public float heightV;
    public float rangeV;
    public float speedV;

    [Header("Horizontal Floating")]
    public float heightH;
    public float rangeH;
    public float speedH;

    public bool isUpClose = true;

    [Tooltip("Lets this gameObject move uniquely (should be any small number above, near 1.")]
    public float differentSeed;

    public bool randomness = false;
    public bool useLocal = false;

    private Vector3 middlePos;
    // Start is called before the first frame update
    void Start()
    {
        if (randomness)
        {
            if (isUpClose)
            {
                rangeH = Random.Range(0.1f, 0.3f);
                speedH = Random.Range(0.1f, 0.4f);
                rangeV = Random.Range(0.01f, 0.05f);
                speedV = Random.Range(0.01f, 0.05f);
                differentSeed = Random.Range(0f, 1f);
            }
            else
            {
                rangeH = Random.Range(0.1f, 0.3f);
                speedH = Random.Range(0.4f, 0.9f);
                rangeV = Random.Range(0.1f, 0.3f);
                speedV = Random.Range(0.4f, 0.9f);
                differentSeed = Random.Range(0.1f, 0.3f);
            }
        }
        if (!useLocal)
            middlePos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        else
        middlePos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!useLocal)
        {
            transform.position = middlePos + (Vector3.up * Mathf.Cos(Time.time * (speedV + differentSeed)) * rangeV) +
                                (Vector3.right * Mathf.Sin(Time.time * (speedH + differentSeed)) * rangeH);
        }
        else
        {
            transform.localPosition = middlePos + (Vector3.up * Mathf.Cos(Time.time * (speedV + differentSeed)) * rangeV) +
                                (Vector3.right * Mathf.Sin(Time.time * (speedH + differentSeed)) * rangeH);
        }
    }
}
