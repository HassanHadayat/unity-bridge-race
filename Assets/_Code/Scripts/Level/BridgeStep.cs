using UnityEngine;

public class BridgeStep : MonoBehaviour
{
    public Collider triggerCol;
    public Collider boundryCol;
    public MeshRenderer meshRenderer;

    public Material whiteMaterial;
    public Material stepMaterial;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PlayerController>().stepStack.steps.Count > 0)
        {
            meshRenderer.material = whiteMaterial;
            meshRenderer.enabled = true;
            triggerCol.enabled = false;
            boundryCol.enabled = false;

            other.GetComponent<PlayerController>().stepStack.RemoveStep();

            Invoke("ChangeMaterial", 0.1f);
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Player") && collision.collider.GetComponent<PlayerController>().stepStack.steps.Count > 0)
    //    {
    //        meshRenderer.material = whiteMaterial;
    //        meshRenderer.enabled = true;
    //        triggerCol.enabled = false;

    //        collision.collider.GetComponent<PlayerController>().stepStack.RemoveStep();

    //        Invoke("ChangeMaterial", 0.1f);
    //    }
    //}
    private void ChangeMaterial()
    {
        meshRenderer.material = stepMaterial;
    }

}
