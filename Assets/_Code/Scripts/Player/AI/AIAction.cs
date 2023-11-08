using UnityEngine;

public abstract class AIAction : MonoBehaviour
{
    public AIController AIController;
    public bool isPerforming = false;
    public bool isMoving = false;

    public Vector3 destination;
    public float lerpTime;

    public abstract void Perform();
    public void Stop(AIAction toAction)
    {
        Debug.Log("Action Stopping..");
        isPerforming = false;
        isMoving = false;

        AIController.animController.SetBool("Run", false);
        AIController.stepStack.StillStack();
        toAction.Perform();

    }
    public void Stop()
    {
        isPerforming = false;
        isMoving = false;

        AIController.animController.SetBool("Run", false);
        AIController.stepStack.StillStack();
    }
    public void Rotate()
    {
        // Rotate Toward Destinatino
        Vector3 moveDirection = (destination - AIController.playerTrans.position).normalized;
        moveDirection.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        lerpTime = Mathf.Clamp((lerpTime + (AIController.rotateSpeed * Time.deltaTime)), 0, 1);
        AIController.characterTrans.rotation = Quaternion.Slerp(AIController.characterTrans.rotation, targetRotation, lerpTime);
    }
}
