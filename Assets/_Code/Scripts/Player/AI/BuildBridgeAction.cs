using UnityEngine;

public class BuildBridgeAction : AIAction
{
    private bool isMovingTowardStart = false;
    private bool isMovingTowardEnd = false;
    private bool isReturningTowardStart = false;

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

        isMoving = true;
        isPerforming = true;
    }

    private void Update()
    {
        if (!isPerforming) return;
        if (isMoving)
        {
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
            // Rotate Toward Destinatino
            Rotate();
            if (isMovingTowardStart)
            {
                if (Vector3.Distance(AIController.playerTrans.position, destination) <= 0.1f)
                {
                    destination = AIController.currBridge.endPos.position;

                    isMovingTowardStart = false;
                    isReturningTowardStart = false;
                    isMovingTowardEnd = true;
                    AIController.navMeshAgent.destination = destination;
                }

            }
            else if (isMovingTowardEnd)
            {
                if (Vector3.Distance(AIController.playerTrans.position, destination) <= 0.1f || AIController.stepStack.steps.Count <= 0)
                {
                    destination = AIController.currBridge.startPos.position;

                    isMovingTowardStart = false;
                    isMovingTowardEnd = false;
                    isReturningTowardStart = true;
                    AIController.navMeshAgent.destination = destination;
                }
            }
            else if (isReturningTowardStart)
            {
                if (Vector3.Distance(AIController.playerTrans.position, destination) <= 0.1f)
                {
                    // STOP ACTION -> COLLECT STEPS
                    Stop(AIController.collectStepAction);

                    AIController.navMeshAgent.destination = destination;
                    return;
                }
            }


            //if (Vector3.Distance(AIController.playerTrans.position, destination) <= 0.1f || AIController.stepStack.steps.Count <= 0)
            //{
            //    if (isMovingTowardStart)
            //    {
            //        destination = AIController.currBridge.endPos.position;

            //        isMovingTowardStart = false;
            //        isReturningTowardStart = false;
            //        isMovingTowardEnd = true;
            //    }
            //    else if (isMovingTowardEnd)
            //    {
            //        destination = AIController.currBridge.startPos.position;

            //        isMovingTowardStart = false;
            //        isMovingTowardEnd = false;
            //        isReturningTowardStart = true;

            //    }
            //    else if (isReturningTowardStart)
            //    {
            //        // STOP ACTION -> COLLECT STEPS
            //        Stop(AIController.collectStepAction);
            //        return;
            //    }
            //    AIController.navMeshAgent.destination = destination;
            //}
        }
    }
}