using UnityEngine;

public class FinishPlatform : MonoBehaviour
{
    public Transform finishPos;

    private void OnTriggerEnter(Collider other)
    {
        // Level Finished

        if (other.tag.Contains("Player"))
        {
            other.GetComponent<PlayerController>().MoveToFinishPos(finishPos);
            Camera.main.GetComponent<CameraFollow>().LevelFinished(other.gameObject.transform);
            GameManager.Instance.EndLevel((other.CompareTag("Blue Player")) ? true : false);
        }
    }
}
