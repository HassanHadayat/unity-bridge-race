using UnityEngine;

public class Bridge : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public BridgeStep lastStep;

    public bool isAssigned = false;

    public bool isBridgeCompleted(Material playerMat)
    {
        bool flag = (playerMat.name == lastStep.meshRenderer.sharedMaterial.name || (lastStep.meshRenderer.sharedMaterial.name == lastStep.whiteMaterial.name));
        Debug.Log(lastStep.meshRenderer.sharedMaterial.name + " => " + flag);
        return flag;
    }

}
