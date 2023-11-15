using UnityEngine;

public class HumanController : PlayerController
{
    public float slopeRayLength = 0.5f; // Adjust as needed
    public float groundCheckDistance = 0.5f;
    public float boundryCheckDistance = 0.25f;
    public LayerMask groundLayer;
    public LayerMask boundryLayer;

    public Rigidbody rb;

    public bool isGrounded;
    public bool canMove;

    private Touch touch; // Store the touch input
    private Vector3 startPos;


    private void Update()
    {
        if (isLevelFinished) return;

        // Check for touch input to move the player.
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startPos = new Vector3(touch.position.x, 0f, touch.position.y);
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Vector3 dir = new Vector3(touch.position.x, 0f, touch.position.y) - startPos;
                Vector3 moveDirection = dir.normalized;

                if (moveDirection == Vector3.zero)
                {
                    stepStack.StillStack();
                    return;
                }

                // Animation
                animController.SetBool("Run", true);

                // Rotation
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                characterTrans.rotation = Quaternion.Slerp(characterTrans.rotation, targetRotation, 15f * Time.deltaTime);


                // Movement
                Vector3 moveVector = playerTrans.TransformDirection(moveDirection) * walkSpeed * Time.deltaTime;
                CheckGround();
                CheckBoundry();
                if (isGrounded && canMove)
                {
                    playerTrans.position += moveVector;
                    stepStack.MoveStack();
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                animController.SetBool("Run", false);
                stepStack.StillStack();
            }
        }
    }

    private void CheckBoundry()
    {
        RaycastHit hit;
        Vector3 rayStart = characterTrans.localPosition + Vector3.up * 1.75f;
        Vector3 characterWorld = characterTrans.TransformPoint(rayStart);
        // Cast a ray downward to check for ground or slopes.
        if (Physics.Raycast(characterWorld, characterTrans.forward, out hit, boundryCheckDistance, boundryLayer))
        {
            Debug.DrawRay(characterWorld, characterTrans.forward * hit.distance, Color.green);

            if (hit.collider.tag == "Boundry")
            {
                canMove = false;
            }
            else if (hit.collider.CompareTag("Bridge Step"))
            {
                if (stepStack.steps.Count > 0)
                {
                    canMove = true;
                }
                else
                {
                    canMove = (playerProperty.m_Material.name == hit.collider.GetComponent<BridgeStep>().stepMaterial.name);
                }
            }
        }
        else
        {
            Debug.DrawRay(characterWorld, characterTrans.forward * boundryCheckDistance, Color.red);
            canMove = true;
        }
    }
    private void CheckGround()
    {
        RaycastHit hit;
        Vector3 rayStart = playerTrans.position + Vector3.up * 0.1f;

        // Cast a ray downward to check for ground or slopes.
        if (Physics.Raycast(rayStart, Vector3.down, out hit, groundCheckDistance, groundLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > 20.0f) // Adjust this angle threshold as needed
            {
                isGrounded = true;
                // Move the player up along the slope to avoid going through it.
                playerTrans.position = hit.point + Vector3.up * 0.05f; // Adjust the offset as needed
            }
            else if(slopeAngle <= 20.0f)
            {
                isGrounded = true;
            }
        }
        else
        {
            isGrounded = false;
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        rb.velocity = Vector3.zero;
    }
}
