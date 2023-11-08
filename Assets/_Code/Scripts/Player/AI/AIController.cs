using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIController : PlayerController
{
    public LayerMask stepsLayer;
    public LayerMask dumbStepsLayer;
    [Range(1, 3)] public int competitiveness = 1;
    

    public CollectStepsAction collectStepAction;
    public BuildBridgeAction buildBridgeAction;

    public override void Start()
    {
        base.Start();

        // Setup AI Actions
        collectStepAction = this.AddComponent<CollectStepsAction>();
        buildBridgeAction = this.AddComponent<BuildBridgeAction>();
        collectStepAction.AIController = this;
        buildBridgeAction.AIController = this;

        Invoke("DelayStart", 1f);
    }
    private void DelayStart()
    {
        collectStepAction?.Perform();
    }

    public override void MoveToFinishPos(Transform finishPos)
    {
        // Stop All Actions
        collectStepAction?.Stop();
        buildBridgeAction?.Stop();

        base.MoveToFinishPos(finishPos);
    }
}
