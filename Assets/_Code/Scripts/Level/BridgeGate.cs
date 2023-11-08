using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeGate : MonoBehaviour
{
    [Header("Gate Settings")]
    public MeshRenderer gateMeshRenderer;
    public Transform gateTrans;
    public float openingSpeed = 2f;
    public bool isOpened = false;

    [Header("Bridge Meshes")]
    public List<MeshRenderer> bridgeMeshes;

    private void OnTriggerEnter(Collider other)
    {
        if (isOpened) return;

        if (other.tag.Contains("Player"))
        {
            Material playerMat = other.GetComponent<PlayerController>().playerProperty.m_Material;
            gateMeshRenderer.material = playerMat;
            foreach (var mesh in bridgeMeshes)
            {
                mesh.material = playerMat;
            }
            isOpened = true;
            StartCoroutine(OpenGate());
        }
    }

    IEnumerator OpenGate()
    {
        while (gateTrans.localScale.x > 0f)
        {
            Vector3 temp = gateTrans.localScale;
            temp.x -= Time.deltaTime * openingSpeed;
            temp.x = Mathf.Clamp(temp.x, 0, 1);
            gateTrans.localScale = temp;
            yield return null;
        }

        Vector3 temp1 = gateTrans.localScale;
        temp1.x = 0;
        gateTrans.localScale = temp1;
    }
}
