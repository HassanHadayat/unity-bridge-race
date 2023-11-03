using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float rotateSpeed = 15f;

    public Transform playerTrans;
    public Transform characterTrans;

    public PlayerProperty playerProperty;
    public StepStack stepStack;
    public Animator animController;
    public Platform currPlatform;
    public Bridge currBridge;

    public NavMeshAgent navMeshAgent;
    public bool isLevelFinished = false;

    public virtual void Start()
    {
        playerTrans = transform;
        isLevelFinished = false;
    }

    public virtual void MoveToFinishPosition(Transform finishPos) { }
    public virtual void MoveToFinishPos(Transform finishPos)
    {
        isLevelFinished = true;

        Debug.Log("Moving To Finish");

        stepStack.gameObject.SetActive(false);

        // Set the Destionation toward the Finish Position
        navMeshAgent.enabled = true;
        navMeshAgent.destination = finishPos.position;

        // Activate Animation
        animController.SetBool("Won", true);

        Invoke("LookTowardCamera", 0.5f);
    }
    private void LookTowardCamera()
    {
        // Calculate the direction vector from the object to the camera, without considering the Y axis
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0f; // Zero out the Y component

        // Rotate the object to face the calculated direction
        playerTrans.rotation = Quaternion.LookRotation(direction);

        // Make the animation moveable
        animController.applyRootMotion = true;
    }
}
