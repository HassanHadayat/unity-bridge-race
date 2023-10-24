using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject stepPrefab;
    public Vector3 platformStepLocalScale;
    public Transform stepPositionsTrans;
    public Transform[] stepPositions;

    public List<GameObject> platformSteps = new List<GameObject>();
    public List<Vector3> availablePositions = new List<Vector3>();


    private void Start()
    {
        foreach (var stepPos in stepPositions)
        {
            GameObject step = Instantiate(stepPrefab, stepPos.position, Quaternion.identity, stepPositionsTrans);
            step.GetComponent<Step>().platform = this;
            platformSteps.Add(step);
        }
    }
    public void AddAvailableStepPosition(Vector3 stepPos)
    {
        if (!availablePositions.Contains(stepPos))
        {
            availablePositions.Add(stepPos);
        }
    }
    public void RemoveAvailableStepPosition(Vector3 stepPos)
    {
        availablePositions.Remove(stepPos);
    }

    public void AddPlatformStep(GameObject step)
    {
        int randValue = Random.Range(0, availablePositions.Count);


        step.transform.parent = stepPositionsTrans;
        step.transform.position = availablePositions[randValue];
        step.transform.localScale = platformStepLocalScale;
        step.transform.localRotation = Quaternion.identity;

        RemoveAvailableStepPosition(availablePositions[randValue]);
    }

}
