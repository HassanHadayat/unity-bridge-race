using System.Collections;
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
        if (other.tag.Contains("Player"))
        {
            Debug.Log(other.gameObject.name);

            if (other.CompareTag("Blue Player"))
            {
                if (other.GetComponent<PlayerController>().stepStack.steps.Count > 0 && other.GetComponent<PlayerProperty>().m_Material.name != stepMaterial.name)
                {

                    // Blue Player
                    //triggerCol.enabled = false;
                    //boundryCol.enabled = false;

                    other.GetComponent<PlayerController>().stepStack.RemoveStep();
                    Material newMaterial = other.GetComponent<PlayerProperty>().m_Material;
                    StartCoroutine(ChangeMaterial(newMaterial));
                }
                else if (other.GetComponent<PlayerProperty>().m_Material.name == stepMaterial.name)
                {
                    other.GetComponent<PlayerController>().canMove = true;
                }
                else
                {
                    other.GetComponent<PlayerController>().canMove = false;
                }
            }
            else if (other.tag.Contains("Player") && other.GetComponent<AIController>().stepStack.steps.Count > 0)
            {
                // AI Colored Player
                //triggerCol.enabled = false;
                //boundryCol.enabled = false;

                //other.GetComponent<AIController>().stepStack.RemoveStep();
                //Material newMaterial = other.GetComponent<PlayerProperty>().m_Material;
                //StartCoroutine(ChangeMaterial(newMaterial));


                if (other.GetComponent<AIController>().stepStack.steps.Count > 0 && other.GetComponent<PlayerProperty>().m_Material.name != stepMaterial.name)
                {

                    // Blue Player
                    //triggerCol.enabled = false;
                    //boundryCol.enabled = false;

                    other.GetComponent<AIController>().stepStack.RemoveStep();
                    Material newMaterial = other.GetComponent<PlayerProperty>().m_Material;
                    StartCoroutine(ChangeMaterial(newMaterial));
                }
                else if (other.GetComponent<PlayerProperty>().m_Material.name == stepMaterial.name)
                {
                    other.GetComponent<AIController>().canMove = true;
                }
                else
                {
                    other.GetComponent<AIController>().canMove = false;
                }
            }
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
    IEnumerator ChangeMaterial(Material newMaterial)
    {
        meshRenderer.enabled = true;
        meshRenderer.material = whiteMaterial;
        yield return new WaitForSeconds(0.1f);
        meshRenderer.material = newMaterial;
        stepMaterial = newMaterial;
    }

}
