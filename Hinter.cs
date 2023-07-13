using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hinter : MonoBehaviour
{
    public GameObject hintItem;
    public Animator hintAnimator;
    [Tooltip("1 = move hint, 2 = sprint hint")]
    public int animState;
    public float speed;
    [Tooltip("How long action has to be held for hint to go away")]
    public float holdTimeRequirement;
    [Tooltip("How many seconds to check whether mouse button 0 has been pressed enough at the start.")]
    public float hint1AppearAfter;
    [Tooltip("How many seconds the player must hold mouse button 0 to avoid hint 1 at start.")]
    public float startMovmentRequired;
    [Tooltip("How many seconds the player must hold mouse button 1 to avoid hint 2 at trigger.")]
    public float sprintRequired;

    public Transform hintStart;
    public Transform hintMiddle;
    public Transform hintEnd;

    private int nextPosition; // 1 = bottom, 2 = middle, 3 = top
    private bool goHigher;  // whether need to keep going
    private bool hint2Needed = true;
    private float mouseTime = 0;
    private float mainMouseTime = 0;
    private float secondMouseTime = 0;
    private bool isActive = false;


    void Awake()
    {
        Messenger.AddListener(GameEvent.START_LEVEL_ONE, becomeActive);
    }
    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.START_LEVEL_ONE, becomeActive);
    }


    // Start is called before the first frame update
    void Start()
    {
        hintItem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (mainMouseTime < hint1AppearAfter && Input.GetKey(KeyCode.Mouse0))
            {
                mainMouseTime += Time.deltaTime;
            }
            if (secondMouseTime < sprintRequired && Input.GetKey(KeyCode.Mouse1))
            {
                secondMouseTime += Time.deltaTime;
            }

            moveHint();
        }
    }

    void startHint(int animS)
    {
        hintItem.SetActive(true);
        hintItem.transform.position = new Vector3(hintStart.position.x, hintStart.position.y, hintStart.position.z);
        animState = animS;
        nextPosition = 2;
        mouseTime = 0;
        goHigher = true;
    }


    void moveHint()
    {
        if (goHigher)
        {
            if (nextPosition == 2)
            {
                hintItem.transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
                if (hintItem.transform.localPosition.y >= hintMiddle.localPosition.y)
                {
                    goHigher = false;
                    nextPosition = 3;
                    hintAnimator.SetInteger("state", animState);
                }
            }
            else
            {
                hintItem.transform.localPosition += new Vector3(0, speed * Time.deltaTime, 0);
                if (hintItem.transform.localPosition.y >= hintEnd.localPosition.y)
                {
                    goHigher = false;
                    nextPosition = 4;
                    hintItem.SetActive(false);
                    hintAnimator.SetInteger("state", 0);
                }
            }
        }
        else   // wait for action
        {
            if (animState == 1 && Input.GetKey(KeyCode.Mouse0))
                mouseTime += Time.deltaTime;
            else if (animState == 2 && Input.GetKey(KeyCode.Mouse1))
                mouseTime += Time.deltaTime;
            if (mouseTime >= holdTimeRequirement)
                goHigher = true;
        }
    }

    private IEnumerator waitHint1()
    {
        yield return new WaitForSeconds(hint1AppearAfter);
        if (mainMouseTime < startMovmentRequired)
            startHint(1);
    }

    // When player hits certain trigger, if they haven't sprinted enough, start hint 2.
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (secondMouseTime < sprintRequired)
            {
                secondMouseTime = sprintRequired;
                startHint(2);
            }
        }
    }

    void becomeActive()
    {
        StartCoroutine(waitHint1());
        isActive = true;        
    }


}
