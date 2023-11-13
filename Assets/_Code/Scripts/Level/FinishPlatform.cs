using UnityEngine;

public class FinishPlatform : MonoBehaviour
{
    public Transform finishPos;
    public MeshRenderer[] railsMeshRenderers;

    private void OnTriggerEnter(Collider other)
    {
        // Level Finished

        if (other.tag.Contains("Player"))
        {
            foreach (MeshRenderer mesh in railsMeshRenderers)
            {
                mesh.material = other.GetComponent<PlayerController>().playerProperty.m_Material;
            }
            other.GetComponent<PlayerController>().MoveToFinishPos(finishPos);
            Camera.main.GetComponent<CameraFollow>().LevelFinished(other.gameObject.transform);
            GameManager.Instance.EndLevel((other.CompareTag("Blue Player")) ? true : false);


        }
    }
}
