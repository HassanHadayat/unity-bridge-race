
using UnityEngine;

public class CollectStepsAction : AIAction
{
    private int stepsCount;


    public CollectStepsAction(AIController aiController)
    {
        AIController = aiController;
    }
    public override void Perform()
    {
        lerpTime = 0;
        isPerforming = true;

        stepsCount = Random.Range(5, 8);
        int maxCount = AIController.currPlatform.GetStepsColorCount(AIController.tag);
        stepsCount = Mathf.Clamp(stepsCount, (int)(maxCount / 2), maxCount);

        AIController.animController.SetBool("Run", true);

    }
    private void Update()
    {
        if (!isPerforming) return;

        if (AIController.stepStack.steps.Count < stepsCount)
        {
            if (!isMoving)
            {
                destination = GetNearestStep();
                if (destination == Vector3.zero)
                {
                    // STOP ACTION -> MOVE TO BUILD BRIDGE ACTION
                    Stop(AIController.buildBridgeAction);
                }

                AIController.navMeshAgent.destination = destination;
                isMoving = true;
            }

            if (isMoving)
            {
                // Rotate Toward Destinatino
                Rotate()
;

                // Check Player Reached Destination or not
                if (Vector3.Distance(AIController.playerTrans.position, destination) <= 0.1f)
                {
                    isMoving = false;
                }
            }
        }
        else
        {
            // STOP ACTION -> MOVE TO BUILD BRIDGE ACTION
            Stop(AIController.buildBridgeAction);
        }
    }
    private Vector3 GetNearestStep()
    {
        // Perform a SphereCast in the given direction
        RaycastHit[] hits = Physics.SphereCastAll(AIController.playerTrans.position, 15f, Vector3.up, 15f, AIController.stepsLayer);

        float minDist = float.MaxValue;
        Vector3 dest = Vector3.zero;
        // Check for hits and handle them
        foreach (var hit in hits)
        {
            if (!hit.collider.CompareTag("Collected") && Vector3.Distance(hit.collider.transform.position, AIController.playerTrans.position) < minDist)
            {
                minDist = Vector3.Distance(hit.collider.transform.position, AIController.playerTrans.position);
                dest = hit.collider.transform.position;
            }
        }
        return dest;
    }

}

