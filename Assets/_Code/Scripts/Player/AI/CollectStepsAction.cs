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
        isMoving = false;

        // Set Steps Count to Collect
        stepsCount = Random.Range(5, 8);
        int maxCount = AIController.currPlatform.GetStepsColorCount(AIController.tag);
        stepsCount = Mathf.Clamp(stepsCount, (int)(maxCount / 2), (maxCount - (3 - AIController.competitiveness)));
        Debug.Log("AI Steps Count = " + stepsCount);

        // Movement Visual Effects
        AIController.animController.SetBool("Run", true);
        AIController.stepStack.MoveStack();
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
                    return;
                }

                AIController.navMeshAgent.destination = destination;
                isMoving = true;
            }

            if (isMoving)
            {
                // Rotate Toward Destination
                Rotate();
                AIController.stepStack.MoveStack();

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
            return;
        }
    }
    private Vector3 GetNearestStep()
    {
        // DUMB STEPS FINDING

        int dumbProb = Random.Range(0, AIController.competitiveness + 1);
        RaycastHit[] hits = null;
        if (dumbProb == 0)
        {
            // Detecting Dumb Steps Layer
            hits = Physics.SphereCastAll(AIController.playerTrans.position, 0.1f, Vector3.up, 0.1f, AIController.dumbStepsLayer);
            if (hits != null && hits.Length > 0)
            {
                RaycastHit randHit = hits[Random.Range(0, hits.Length)];

                if (!randHit.collider.CompareTag("Collected"))
                {
                    return randHit.collider.transform.position;
                }
            }
        }


        // REAL STEP FINDING
        // Detecting Steps Layer
        hits = Physics.SphereCastAll(AIController.characterTrans.position, 15f, Vector3.up, 15f, AIController.stepsLayer);

        float minDist = float.MaxValue;
        Vector3 dest = Vector3.zero;

        // Check for hits and handle them
        foreach (var hit in hits)
        {

            if (!hit.collider.CompareTag("Collected") 
                && Vector3.Distance(hit.collider.transform.position, AIController.playerTrans.position) < minDist)
            {
                minDist = Vector3.Distance(hit.collider.transform.position, AIController.playerTrans.position);
                dest = hit.collider.transform.position;
            }
        }
        return dest;
    }

}

