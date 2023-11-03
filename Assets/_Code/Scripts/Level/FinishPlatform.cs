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
        }
    }
}
