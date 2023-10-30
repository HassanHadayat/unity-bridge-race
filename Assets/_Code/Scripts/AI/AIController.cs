using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float rotateSpeed = 15f;
    public float slopeRayLength = 0.5f; // Adjust as needed
    public float groundCheckDist = 0.5f;
    public float boundryCheckDist = 0.25f;
    public LayerMask boundryLayer;
    public LayerMask stepsLayer;

    public Transform parentTrans;
    public Transform characterTrans;

    public PlayerProperty playerProperty;
    public StepStack stepStack;
    public Animator animController;
    public Rigidbody rb;
    public Platform currPlatform;
    public Bridge currBridge;

    public bool isGrounded;
    public bool canMove;

    private Touch touch; // Store the touch input
    private Vector3 startPos;

    public NavMeshAgent navMeshAgent;

    private void Start()
    {
        parentTrans = transform;
        Invoke("Delay", 1f);
    }
    private void Delay()
    {
        StartCoroutine(CollectSteps());

    }

    private void OnCollisionExit(Collision collision)
    {
        rb.velocity = Vector3.zero;
    }

    //private void Update()
    //{
    //     Check for touch input to move the player.
    //    if (Input.touchCount > 0)
    //    {
    //        touch = Input.GetTouch(0);

    //        if (touch.phase == TouchPhase.Began)
    //        {
    //            startPos = new Vector3(touch.position.x, 0f, touch.position.y);
    //        }
    //        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
    //        {
    //            Vector3 dir = new Vector3(touch.position.x, 0f, touch.position.y) - startPos;
    //            Vector3 moveDirection = dir.normalized;

    //            if (moveDirection == Vector3.zero)
    //            {
    //                stepStack.StillStack();
    //                return;
    //            }

    //             Animation
    //            animController.SetBool("Run", true);

    //             Rotation
    //            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
    //            characterTrans.rotation = Quaternion.Slerp(characterTrans.rotation, targetRotation, 15f * Time.deltaTime);


    //             Movement
    //            Vector3 moveVector = parentTrans.TransformDirection(moveDirection) * walkSpeed * Time.deltaTime;
    //            CheckGround();
    //            CheckBoundry();
    //            if (isGrounded && canMove)
    //            {
    //                parentTrans.position += moveVector;
    //                stepStack.MoveStack();
    //            }
    //        }
    //        else if (touch.phase == TouchPhase.Ended)
    //        {
    //            animController.SetBool("Run", false);
    //            stepStack.StillStack();
    //        }
    //    }
    //}
    public Vector3 dest;
    private void Update()
    {

        //dest = CheckStepsNear();
        //if (dest != Vector3.zero)
        //{
        //    Debug.Log("Dest = " + dest);
        //    navMeshAgent.SetDestination(dest);
        //}





        ////++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //// Check for touch input to move the player.
        //if (Input.touchCount > 0)
        //{
        //    touch = Input.GetTouch(0);

        //    if (touch.phase == TouchPhase.Began)
        //    {
        //        startPos = new Vector3(touch.position.x, 0f, touch.position.y);
        //    }
        //    else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        //    {
        //        //Vector3 dir = new Vector3(touch.position.x, 0f, touch.position.y) - startPos;
        //        //Vector3 moveDirection = dir.normalized;

        //        //if (moveDirection == Vector3.zero)
        //        //{
        //        //    stepStack.StillStack();
        //        //    return;
        //        //}

        //        //// Animation
        //        //animController.SetBool("Run", true);

        //        //// Rotation
        //        //Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        //        //characterTrans.rotation = Quaternion.Slerp(characterTrans.rotation, targetRotation, 15f * Time.deltaTime);


        //        //// Movement
        //        //Vector3 moveVector = playerTransform.TransformDirection(moveDirection) * walkSpeed * Time.deltaTime;
        //        //CheckGround();
        //        //CheckBoundry();
        //        //if (isGrounded && canMove)
        //        //{
        //        //    playerTransform.position += moveVector;
        //        //    stepStack.MoveStack();
        //        //}
        //    }
        //    else if (touch.phase == TouchPhase.Ended)
        //    {
        //        animController.SetBool("Run", false);
        //        stepStack.StillStack();
        //    }
        //}


    }


    private Vector3 CheckStepsNear()
    {
        // Perform a SphereCast in the given direction
        RaycastHit[] hits = Physics.SphereCastAll(parentTrans.position, 15f, Vector3.up, 15f, stepsLayer);

        float minDist = float.MaxValue;
        Vector3 dest = Vector3.zero;
        // Check for hits and handle them
        foreach (var hit in hits)
        {
            if (!hit.collider.CompareTag("Collected") && Vector3.Distance(hit.collider.transform.position, parentTrans.position) < minDist)
            {
                minDist = Vector3.Distance(hit.collider.transform.position, parentTrans.position);
                dest = hit.collider.transform.position;
            }
        }
        return dest;
    }
    private void CheckBoundry()
    {
        RaycastHit hit;
        Vector3 rayStart = characterTrans.localPosition + Vector3.up * 2f;
        Vector3 characterWorld = characterTrans.TransformPoint(rayStart);
        // Cast a ray downward to check for ground or slopes.
        if (Physics.Raycast(characterWorld, characterTrans.forward, out hit, boundryCheckDist, boundryLayer))
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }
    private void CheckGround()
    {
        RaycastHit hit;
        Vector3 rayStart = parentTrans.position + Vector3.up * 0.1f;

        // Cast a ray downward to check for ground or slopes.
        if (Physics.Raycast(rayStart, Vector3.down, out hit, groundCheckDist))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle < 90.0f) // Adjust this angle threshold as needed
            {
                isGrounded = true;
                // Move the player up along the slope to avoid going through it.
                parentTrans.position = hit.point + Vector3.up * 0.05f; // Adjust the offset as needed
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            isGrounded = false;
        }
    }




    //======================================== AI Behaviours ===========================================
    IEnumerator CollectSteps()
    {
        int stepsCount = Random.Range(5, 8);
        //int stepsCount = 4;
        while (stepStack.steps.Count < stepsCount)
        {
            Debug.Log("1");
            dest = CheckStepsNear();
            if (dest != Vector3.zero)
            {
                Debug.Log("Dest = " + dest);
                navMeshAgent.destination = (dest);

                StartCoroutine(Move(dest));
            }
            else
            {
                Debug.Log("Steps Ended on Platform");

                // Move to Build Bridge Behaviour
                StartCoroutine(BuildBridge());
            }
            yield return null;
        }



        // Move to Build Bridge Behaviour
        StartCoroutine(BuildBridge());
    }

    IEnumerator BuildBridge()
    {
        if (currBridge == null)
        {
            // Set Bridge
            currBridge = currPlatform.GetBridge();
            if (currBridge == null) { StopCoroutine("BuildBridge"); }
        }

        dest = currBridge.startPos.position;
        navMeshAgent.destination = (dest);
        StartCoroutine(Move(dest));
        yield return new WaitUntil(() => (Vector3.Distance(parentTrans.position, dest) <= 0.1f));

        dest = currBridge.endPos.position;
        navMeshAgent.destination = (dest);
        StartCoroutine(Move(dest));
        yield return new WaitUntil(() => (Vector3.Distance(parentTrans.position, dest) <= 0.1f || stepStack.steps.Count <= 0));

        if (stepStack.steps.Count <= 0)
        {
            dest = currBridge.startPos.position;
            navMeshAgent.destination = (dest);
            StartCoroutine(Move(dest, false));
            yield return new WaitUntil(() => (Vector3.Distance(parentTrans.position, dest) <= 0.1f));

            StartCoroutine(CollectSteps());

        }
        else
        {
            // Move to Next Platform
            Debug.Log("Player Moved to Next Platform!");
        }

        yield return null;
    }



    //===================================================================================================
    IEnumerator Move(Vector3 dest, bool isLerp = true)
    {
        while (Vector3.Distance(dest, parentTrans.position) > 0.5f)
        {
            Vector3 dir = dest - parentTrans.position;
            Vector3 moveDirection = dir.normalized;

            if (moveDirection == Vector3.zero)
            {
                stepStack.StillStack();
                yield return null;
            }

            // Animation
            animController.SetBool("Run", true);

            // Rotation
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            characterTrans.rotation = Quaternion.Slerp(characterTrans.rotation, targetRotation, (isLerp) ? 15f * Time.deltaTime : 1f);

            stepStack.MoveStack();

            yield return null;
        }
        //yield return new WaitUntil(() => /*Vector3.Distance(dest, parentTrans.position) <= 0.5f*/);

        animController.SetBool("Run", false);
        stepStack.StillStack();


    }
}
