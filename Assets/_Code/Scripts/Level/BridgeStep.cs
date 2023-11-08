using System.Collections;
using UnityEngine;

public class BridgeStep : MonoBehaviour
{
    public MeshRenderer meshRenderer;

    public Material whiteMaterial;
    public Material stepMaterial;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            //Debug.Log(other.gameObject.name);
            PlayerController PC = other.GetComponent<PlayerController>();
            int tempStackCount = PC.stepStack.steps.Count;
            if (other.CompareTag("Blue Player"))
            {
                if (tempStackCount > 0 && PC.playerProperty.m_Material.name != stepMaterial.name)
                {
                    Material newMaterial = PC.playerProperty.m_Material;
                    StartCoroutine(ChangeMaterial(newMaterial));
                    PC.stepStack.RemoveStep();
                }
                else
                {
                    // Human Can Move If Same Color, Else Stop
                    other.GetComponent<HumanController>().canMove = (PC.playerProperty.m_Material.name == stepMaterial.name);
                }
            }
            else if (other.tag.Contains("Player") && PC.stepStack.steps.Count > 0 && PC.playerProperty.m_Material.name != stepMaterial.name)
            {
                PC.stepStack.RemoveStep();
                Material newMaterial = PC.playerProperty.m_Material;
                StartCoroutine(ChangeMaterial(newMaterial));
            }
        }
    }

    IEnumerator ChangeMaterial(Material newMaterial)
    {
        meshRenderer.enabled = true;
        meshRenderer.material = whiteMaterial;
        yield return new WaitForSeconds(0.1f);
        meshRenderer.material = newMaterial;
        stepMaterial = newMaterial;
    }

}
