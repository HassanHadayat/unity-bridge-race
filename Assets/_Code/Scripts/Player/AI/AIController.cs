using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIController : PlayerController
{
    public LayerMask stepsLayer;

    public NavMeshAgent navMeshAgent;

    //public AIAction behaviour;
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
}
