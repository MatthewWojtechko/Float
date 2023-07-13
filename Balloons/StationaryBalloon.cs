using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryBalloon : MonoBehaviour
{
    public BalloonLevel balloonLevel;

    public float lerpSmoothing = 0.1f;
    public Vector3 collectedScaleBalloon;
    public float collectedScaleString = 0.01f;
    public BalloonMusicCues musicCuer;
    public Transform target;
    public Transform endPoint;
    public LineRenderer line;

    MeshRenderer meshRenderer;
    Hover hoverScript;
    Cable_Procedural_Simple cableScript;
    Constants constants;
    Vector3 vectorFromTarget;
    bool hasFoundPlayer = false;

    public delegate void OnStart(BalloonLevel level);
    public static event OnStart onStart;
    public delegate void OnBalloonGet(BalloonLevel level);
    public static event OnBalloonGet onBalloonGet;

    void Start()
    {
        lerpSmoothing += Random.Range(0.03f, -0.09f);

        constants = Constants.instance;
        cableScript = GetComponentInChildren<Cable_Procedural_Simple>();
        hoverScript = GetComponent<Hover>();
        meshRenderer = GetComponent<MeshRenderer>();

        //Messenger.AddListener(GameEvent.TRANSITION_TO_TWO, EnableMeshRendererOnLevel02);
        //Messenger.AddListener(GameEvent.TRANSITION_TO_THREE, EnableMeshRendererOnLevel03);

        if (transform.position.y < constants.level01Boundary.upperBoundary + 7)
        {
            balloonLevel = BalloonLevel.L1;
        }
        else if (transform.position.y < constants.level02Boundary.upperBoundary + 7)
        {
            //meshRenderer.enabled = false;
            balloonLevel = BalloonLevel.L2;
        }
        else
        {
            //meshRenderer.enabled = false;
            balloonLevel = BalloonLevel.L3;
        }
        onStart?.Invoke(balloonLevel);
    }

    private void OnDestroy()
    {
        //Messenger.RemoveListener(GameEvent.TRANSITION_TO_TWO, EnableMeshRendererOnLevel02);
       // Messenger.RemoveListener(GameEvent.TRANSITION_TO_THREE, EnableMeshRendererOnLevel03);
    }

    //void EnableMeshRendererOnLevel02()
    //{
    //    if (balloonLevel == BalloonLevel.L2)
    //    {
    //        meshRenderer.enabled = true;
    //    }
    //}

    //void EnableMeshRendererOnLevel03()
    //{
    //    if (balloonLevel == BalloonLevel.L3)
    //    {
    //        meshRenderer.enabled = true;
    //    }
    //}

    //private void LateUpdate()
    //{
    //    if (target != null)
    //    {
    //        Debug.Log(Time.time + " s");
    //        Vector3 targetPosition = target.position + vectorFromTarget;
    //        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSmoothing);
    //    }
    //}

    IEnumerator UpdatePos()
    {
        while (true)
        {
            if (target != null)
            {
                Vector3 targetPosition = target.position + vectorFromTarget;
                transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSmoothing);
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !hasFoundPlayer)
        {
            hoverScript.enabled = false;
            hasFoundPlayer = true;
            StartCoroutine(UpdatePos());
            //target = other.transform.GetChild(1);
            cableScript.endPointTransform = endPoint;
            vectorFromTarget = new Vector3(Random.Range(-1f, 1f), Random.Range(0.3f, -0.3f), Random.Range(1f, -1f));
            this.transform.localScale = collectedScaleBalloon;
            line.startWidth = collectedScaleString;

            StartCoroutine(FollowPlayer());
            StartCoroutine(UpdatePos());

            musicCuer.die();
            onBalloonGet?.Invoke(balloonLevel);
            Messenger.Broadcast(GameEvent.BALLOON_GET);
        }
    }

    IEnumerator FollowPlayer()
    {
        while (true)
        {
            Vector3 targetPosition = target.position + vectorFromTarget;
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSmoothing);
            yield return new WaitForEndOfFrame();
        }
    }
}
