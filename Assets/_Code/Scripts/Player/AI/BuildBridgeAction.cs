using UnityEngine;

public class BuildBridgeAction : AIAction
{
    private bool isMovingTowardStart = false;
    private bool isMovingTowardEnd = false;
    private bool isReturningTowardStart = false;
    private bool isBridgeCompleted = false;

    public BuildBridgeAction(AIController aiController)
    {
        AIController = aiController;
    }
    public override void Perform()
    {
        Debug.Log("Building Bridge Start..");
        lerpTime = 0;

        if (AIController.currBridge == null)
        {
            // Set Bridge
            AIController.currBridge = AIController.currPlatform.GetBridge();
            if (AIController.currBridge == null)
            {
                // STOP ACTION
                Stop();
                return;
            }
        }

        AIController.animController.SetBool("Run", true);

        destination = AIController.currBridge.startPos.position;
        AIController.navMeshAgent.destination = destination;

        isMovingTowardStart = true;
        isMovingTowardEnd = false;
        isReturningTowardStart = false;
        isBridgeCompleted = false;

        isMoving = true;
        isPerforming = true;
    }

    private void Update()
    {
        if (!isPerforming) return;
        if (isMoving)
        {
            if (AIController.currBridge == null && !isBridgeCompleted)
            {
                // Set Bridge
                AIController.currBridge = AIController.currPlatform.GetBridge();
                if (AIController.currBridge == null)
                {
                    // STOP ACTION
                    Stop();
                    return;
                }
            }

            // Rotate Toward Destination
            Rotate();
            AIController.stepStack.MoveStack();
            if (isMovingTowardStart)
            {
                if (Vector3.Distance(AIController.playerTrans.position, destination) <= 0.1f)
                {
                    destination = AIController.currBridge.endPos.position;

                    isMovingTowardStart = false;
                    isMovingTowardEnd = true;
                    isReturningTowardStart = false;

                    // Move to Bridge End
                    AIController.navMeshAgent.destination = destination;
                }

            }
            else if (isMovingTowardEnd && !isBridgeCompleted)
            {
                if (
                    (AIController.stepStack.steps.Count <= 0 && !AIController.currBridge.isBridgeCompleted(AIController.playerProperty.m_Material))
                    )
                {
                    Debug.Log("Moving Toward Start Pos");
                    destination = AIController.currBridge.startPos.position;

                    isMovingTowardStart = false;
                    isMovingTowardEnd = false;
                    isReturningTowardStart = true;
                    AIController.navMeshAgent.destination = destination;
                }
                else if (AIController.currBridge.isBridgeCompleted(AIController.playerProperty.m_Material))
                {
                    Debug.Log("BRIDGE COMPLETED");
                    AIController.currPlatform = null;
                    AIController.currBridge = null;

                    isMovingTowardStart = false;
                    isMovingTowardEnd = false;
                    isReturningTowardStart = false;

                    isBridgeCompleted = true;

                    return;
                }
            }
            else if (isReturningTowardStart)
            {
                if (Vector3.Distance(AIController.playerTrans.position, destination) <= 0.1f)
                {
                    // STOP ACTION -> COLLECT STEPS
                    Stop(AIController.collectStepAction);

                    //AIController.navMeshAgent.destination = destination;
                    return;
                }
            }
            else if (isBridgeCompleted)
            {
                if (Vector3.Distance(AIController.playerTrans.position, destination) <= 0.2f && AIController.currPlatform)
                {
                    Debug.Log("NEW PLATFORM (COLLECT STEPS)");
                    Stop(AIController.collectStepAction);
                    return;
                }
            }
        }
    }
}