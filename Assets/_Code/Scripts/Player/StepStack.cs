using System.Collections.Generic;
using UnityEngine;

public class StepStack : MonoBehaviour
{

    public Stack<GameObject> steps = new Stack<GameObject>();
    public Vector3 stepTopPos = Vector3.zero;
    public Vector3 playerTopPos = Vector3.zero;
    public float stepHeight;

    public void AddStep(GameObject step)
    {
        step.transform.parent = this.transform;

        //step.transform.localPosition = stepTopPos.localPosition;
        //step.transform.localRotation = Quaternion.identity;
        //step.transform.localScale = Vector3.one * 0.5f;
        step.GetComponent<Step>().MoveToStack(stepTopPos, playerTopPos);

        steps.Push(step);

        stepTopPos.y = steps.Count * stepHeight;
        playerTopPos.y += stepHeight;
    }
    public void RemoveStep()
    {
        // Restore that step in the platform
        GameObject step = steps.Pop();
        step.GetComponent<Step>().platform.AddPlatformStep(step);


        stepTopPos.y = steps.Count * stepHeight;
        playerTopPos.y -= stepHeight;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Step"))
        {
            AddStep(other.gameObject);
        }

    }

}
