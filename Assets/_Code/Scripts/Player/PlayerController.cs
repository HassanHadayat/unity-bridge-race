using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float slopeRayLength = 0.5f; // Adjust as needed

    public Transform playerTransform;
    public float groundCheckDistance = 0.12f;
    public bool isGrounded;

    private Touch touch; // Store the touch input
    private Vector3 startPos;

    public Transform characterTrans;
    public Animator animController;

    private void Start()
    {
        playerTransform = transform;
    }

    private void Update()
    {
        // Check for touch input to move the player.
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0); // Get the first touch (you can modify this for multi-touch)



            if(touch.phase == TouchPhase.Began)
            {
                startPos = new Vector3(touch.position.x, 0f, touch.position.y);
            }
            // Check if the touch is a move (drag) type, and move the player accordingly.
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                animController.SetBool("Run", true);
                Vector3 dir = new Vector3(touch.position.x, 0f, touch.position.y) - startPos;
                Vector3 moveDirection = dir.normalized;


                // Calculate the angle between the current forward direction and the desired direction
                float angle = Vector3.Angle(characterTrans.forward, moveDirection);

                // Calculate the rotation axis as the cross product of the current forward direction and the desired direction
                Vector3 rotationAxis = Vector3.Cross(characterTrans.forward, moveDirection);

                // Apply the rotation around the local up axis
                characterTrans.Rotate(rotationAxis, angle, Space.Self);


                Debug.Log(moveDirection);

                // Calculate the movement vector based on touch input and desired speed.
                Vector3 moveVector = playerTransform.TransformDirection(moveDirection) * walkSpeed * Time.deltaTime;

                // Check if the player is grounded and adjust movement accordingly.
                CheckGround();
                if (isGrounded)
                {
                    playerTransform.position += moveVector;
                }
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                animController.SetBool("Run", false);
            }
        }
    }

    private void CheckGround()
    {
        RaycastHit hit;
        Vector3 rayStart = playerTransform.position + Vector3.up * 0.1f; // Slightly above the player's position

        // Cast a ray downward to check for ground or slopes.
        if (Physics.Raycast(rayStart, Vector3.down, out hit, groundCheckDistance))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            Debug.Log("Slope Angle = " + slopeAngle);
            if (slopeAngle < 45.0f) // Adjust this angle threshold as needed
            {
                isGrounded = true;
                // Move the player up along the slope to avoid going through it.
                playerTransform.position = hit.point + Vector3.up * 0.05f; // Adjust the offset as needed
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
}
