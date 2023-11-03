using UnityEngine;

public class PlatformConnector : MonoBehaviour
{
    public Platform toPlatform;


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().currPlatform = toPlatform;
            other.gameObject.GetComponent<PlayerController>().currBridge = null;

            toPlatform.SetPlayerSteps(new string[] { other.gameObject.GetComponent<PlayerController>().characterTrans.tag });
        }
    }
}
