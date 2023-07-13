using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public Boundaries level01Boundary;
    public Boundaries level02Boundary;
    public Boundaries level03Boundary;
    public Boundaries finalBoundary;

    public static Constants instance;

    private void Awake()
    {
        instance = this;
        level01Boundary = new Boundaries(5, 15);
        level02Boundary = new Boundaries(25, 40);
        level03Boundary = new Boundaries(50, 120);
        finalBoundary = new Boundaries(900, 1000);
    }
}

public class Boundaries
{
    public float upperBoundary;
    public float lowerBoundary;

    public Boundaries(float lower, float upper)
    {
        upperBoundary = upper;
        lowerBoundary = lower;
    }
}
