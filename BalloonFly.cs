using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonFly : MonoBehaviour
{
    public float maxVelocity = 10f;
    public float maxSprintVelocity = 10f;
    public float acceleration = 5f;
    public float speedUpSmoothing = 0.1f;
    public float barrierSmoothing = 0.01f;
    public float audioFadeInSpeed = 0.01f;
    public float audioFadeOutSpeed = 0.01f;
    public Vector3 targetOffset;
    public Vector3 endPointOffset;
    public Transform mainCam;
    public Transform target;
    public Transform endPoint;
    public float riseSpeed;
    public float fastRiseSpeed;

    public bool autoStart = false;
    float upperBound = 7;
    float lowerBound = 0;
    private bool isControllable = false;
    private float lerpAmount = 0.01f;

    AudioSource[] audioSources;
    Rigidbody rb;
    CharacterController characterController;

    Vector3 moveVector;
    Vector3 vectorToTravelIn;
    Vector3 newZonePosition;

    float variableMaxVelocity;
    float actualVelocity = 0;
    bool normalSpeed = true;
    ThirdPersonOrbitCamBasic camScript;
    Constants constants;
    public bool relocating = false;

    void Awake()
    {
        Messenger.AddListener(GameEvent.OPENING, giveControl);  // Game event should be resume game for final thing
        Messenger.AddListener(GameEvent.FINAL_TRANSITION, finalTransition);
    }

    void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.OPENING, giveControl);
        Messenger.RemoveListener(GameEvent.FINAL_TRANSITION, finalTransition);
    }

    void Start()
    {
        constants = Constants.instance;
        upperBound = constants.level01Boundary.upperBoundary;
        lowerBound = constants.level01Boundary.lowerBoundary;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (autoStart || isControllable)
        {
            if (!relocating)
            {
                vectorToTravelIn = (transform.position - new Vector3(mainCam.position.x, mainCam.position.y - 2, mainCam.position.z)).normalized;

                if (Input.GetMouseButton(0))
                {
                    if (!normalSpeed)
                    {
                        StartCoroutine(FadeOutSound(audioSources[1], audioFadeOutSpeed, false));
                        camScript.ResetFOV();
                        normalSpeed = true;
                    }
                    if (transform.position.y > upperBound && vectorToTravelIn.y > 0)
                    {
                        moveVector = Vector3.Lerp(moveVector, new Vector3(vectorToTravelIn.x, 0, vectorToTravelIn.z) * maxVelocity, barrierSmoothing);
                    }
                    else if (transform.position.y < lowerBound && vectorToTravelIn.y < 0)
                    {
                        moveVector = Vector3.Lerp(moveVector, new Vector3(vectorToTravelIn.x, 0, vectorToTravelIn.z) * maxVelocity, barrierSmoothing);
                    }
                    else
                    {
                        moveVector = Vector3.Lerp(moveVector, vectorToTravelIn * maxVelocity, speedUpSmoothing);
                    }
                }
                else if (Input.GetMouseButton(1))
                {
                    if (normalSpeed)
                    {
                        audioSources[0].Play();
                        if (!audioSources[1].isPlaying)
                        {
                            StartCoroutine(FadeInSound(audioSources[1], audioFadeInSpeed, false));
                        }
                        else
                        {
                            StartCoroutine(FadeInSound(audioSources[1], audioFadeInSpeed, true));
                        }

                        camScript.SetFOV(120);
                        normalSpeed = false;
                    }
                    if (transform.position.y > upperBound && vectorToTravelIn.y > 0)
                    {
                        moveVector = Vector3.Lerp(moveVector, new Vector3(vectorToTravelIn.x, 0, vectorToTravelIn.z) * maxSprintVelocity, barrierSmoothing);
                    }
                    else if (transform.position.y < lowerBound && vectorToTravelIn.y < 0)
                    {
                        moveVector = Vector3.Lerp(moveVector, new Vector3(vectorToTravelIn.x, 0, vectorToTravelIn.z) * maxSprintVelocity, barrierSmoothing);
                    }
                    else
                    {
                        moveVector = Vector3.Lerp(moveVector, vectorToTravelIn * maxSprintVelocity, speedUpSmoothing);
                    }
                }
                else
                {
                    if (!normalSpeed)
                    {
                        StartCoroutine(FadeOutSound(audioSources[0], audioFadeOutSpeed, true));
                        StartCoroutine(FadeOutSound(audioSources[1], audioFadeOutSpeed, false));
                        camScript.ResetFOV();
                        normalSpeed = true;
                    }
                    moveVector = Vector3.Lerp(moveVector, Vector3.zero, speedUpSmoothing);
                    if (moveVector.magnitude < 0.5f)
                    {
                        moveVector = Vector3.zero;
                    }
                }

                characterController.Move(moveVector * Time.deltaTime);
                //target.position = transform.position - new Vector3(vectorToTravelIn.x * targetOffset.x, targetOffset.y, vectorToTravelIn.z * targetOffset.z);
                //endPoint.position = transform.position - new Vector3(vectorToTravelIn.x * endPointOffset.x, endPointOffset.y, vectorToTravelIn.z * endPointOffset.z);
            }
        }
    }

    private void LateUpdate()
    {
        //target.position = transform.position - new Vector3(vectorToTravelIn.x * targetOffset.x, targetOffset.y, vectorToTravelIn.z * targetOffset.z);
        //endPoint.position = transform.position - new Vector3(vectorToTravelIn.x * endPointOffset.x, endPointOffset.y, vectorToTravelIn.z * endPointOffset.z);
    }

    public void SetBounds(float lower, float upper, float maxLerp = 0.03f)
    {
        camScript.SetFOV(90);
        upperBound = upper;
        lowerBound = lower;
        relocating = true;
        newZonePosition = new Vector3(transform.position.x, (upper + lower + 8) / 2, transform.position.z);
        StartCoroutine("MoveBalloonToBounds");
        StartCoroutine("DecreaseLerp", maxLerp);
    }

    IEnumerator MoveBalloonToBounds()
    {
        audioSources[2].Play();
        characterController.enabled = false;
        rb.isKinematic = true;
        moveVector = Vector3.zero;
        while (transform.position.y < newZonePosition.y - 5)
        {
            Vector3 newPos = Vector3.Lerp(transform.position, newZonePosition, lerpAmount);
            Vector3 moveAmount = newPos - transform.position;
            transform.Translate(moveAmount);
            //float riseStep = riseSpeed * Time.deltaTime;
            //transform.Translate(new Vector3(0, riseStep, 0));
            yield return new WaitForEndOfFrame();
        }
        rb.isKinematic = false;
        characterController.enabled = true;

        camScript.ResetFOV();
        relocating = false;
    }

    IEnumerator FadeOutSound(AudioSource audioSource, float speed, bool stop)
    {
        float originalVolume = audioSource.volume;
        while (audioSource.volume > 0.1)
        {
            audioSource.volume -= speed;
            yield return null;
        }
        if (stop)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Pause();
        }
        audioSource.volume = originalVolume;
    }

    IEnumerator FadeInSound(AudioSource audioSource, float speed, bool unpause)
    {
        float originalVolume = audioSource.volume;
        audioSource.volume = 0;
        if (unpause)
        {
            audioSource.UnPause();
        }
        else
        {
            audioSource.Play();
        }
        while (audioSource.volume < originalVolume)
        {
            audioSource.volume += speed;
            yield return null;
        }
    }

    private void giveControl()
    {
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        audioSources = GetComponents<AudioSource>();
        Cursor.lockState = CursorLockMode.Locked;
        //mainCam = Camera.main.transform;
        camScript = mainCam.GetComponent<ThirdPersonOrbitCamBasic>();
        variableMaxVelocity = maxVelocity;

        isControllable = true;
    }

    void finalTransition()
    {
        riseSpeed = fastRiseSpeed;
        // effect
        lerpAmount = 0.0001f;
        StopCoroutine("MoveBalloonToBounds");
        SetBounds(constants.finalBoundary.lowerBoundary, constants.finalBoundary.upperBoundary, 0.01f);
    }

    IEnumerator DecreaseLerp(float max)
    {
        //yield return new WaitForSeconds(3);
        while(lerpAmount < max)
        {
            lerpAmount += 0.000001f;
            yield return new WaitForEndOfFrame();
        }
    }
}
