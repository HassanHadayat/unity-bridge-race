using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Bridges")]
    public Bridge[] bridges;
    public List<int> availableBridgeIndexes = new List<int>();

    [Header("Steps Prefabs")]
    public GameObject stepPrefab;
    public GameObject stepBluePrefab;
    public GameObject stepRedPrefab;
    public GameObject stepGreenPrefab;
    public GameObject stepPinkPrefab;


    [Header("Steps Properties")]
    public Vector3 platformStepLocalScale;

    [Header("Spawn Step Position")]
    public Transform stepSpawnPositionsTrans;
    public Transform[] stepSpawnPositions;

    public List<GameObject> platformSteps = new List<GameObject>();
    public List<Vector3> availablePositions = new List<Vector3>();


    private void Start()
    {
        for (int i = 0; i < stepSpawnPositions.Length; i++)
        {
            int randIndex = Random.Range(i, stepSpawnPositions.Length);
            Transform temp = stepSpawnPositions[i];
            stepSpawnPositions[i] = stepSpawnPositions[randIndex];
            stepSpawnPositions[randIndex] = temp;
        }


        for (int i = 0; i < bridges.Length; i++)
        {
            availableBridgeIndexes.Add(i);
        }
    }
    public void SetupPlatform(string[] playerColors)
    {
        int stepsPerColor = (stepSpawnPositions.Length / playerColors.Length);
        int extras = (stepSpawnPositions.Length % playerColors.Length);
        //Debug.Log("Count = " + stepsPerColor);
        //Debug.Log("Extras = " + extras);
        int i = 0;
        foreach (var color in playerColors)
        {
            if (color == "Blue")
            {
                for (int j = 0; j < (stepsPerColor + extras); j++, i++)
                {
                    GameObject step = Instantiate(stepBluePrefab, stepSpawnPositions[i].position, Quaternion.identity, stepSpawnPositionsTrans);
                    step.GetComponent<Step>().platform = this;
                    platformSteps.Add(step);
                }
            }
            else if (color == "Red")
            {
                for (int j = 0; j < stepsPerColor; j++, i++)
                {
                    GameObject step = Instantiate(stepRedPrefab, stepSpawnPositions[i].position, Quaternion.identity, stepSpawnPositionsTrans);
                    step.GetComponent<Step>().platform = this;
                    platformSteps.Add(step);
                }
            }
            else if (color == "Green")
            {
                for (int j = 0; j < stepsPerColor; j++, i++)
                {
                    GameObject step = Instantiate(stepGreenPrefab, stepSpawnPositions[i].position, Quaternion.identity, stepSpawnPositionsTrans);
                    step.GetComponent<Step>().platform = this;
                    platformSteps.Add(step);
                }
            }
            else if (color == "Pink")
            {
                for (int j = 0; j < stepsPerColor; j++, i++)
                {
                    GameObject step = Instantiate(stepPinkPrefab, stepSpawnPositions[i].position, Quaternion.identity, stepSpawnPositionsTrans);
                    step.GetComponent<Step>().platform = this;
                    platformSteps.Add(step);
                }
            }
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


        step.transform.parent = stepSpawnPositionsTrans;
        step.transform.position = availablePositions[randValue];
        step.transform.localScale = platformStepLocalScale;
        step.transform.localRotation = Quaternion.identity;

        RemoveAvailableStepPosition(availablePositions[randValue]);
    }





    public Bridge GetBridge()
    {
        if (availableBridgeIndexes.Count > 0)
        {
            Bridge bridge = bridges[availableBridgeIndexes[0]];
            bridge.isAssigned = true;
            availableBridgeIndexes.RemoveAt(0);
            return bridge;
        }
        else
        {
            return null;
        }

        //List<int> indexes = new List<int>();
        //for (int i = 0; i < bridges.Length; i++) {  indexes.Add(i); }

        //foreach(int index in indexes)
        //{

        //}
        //bool flag = true;
        //foreach (Bridge bridge in bridges)
        //{
        //    if (!bridge.isAssigned)
        //    {
        //        flag = false;
        //        break;
        //    }
        //}
        //// If al bridges are already assigned
        //if (flag) return null;

        //int index = 0;
        //do
        //{
        //    index = Random.Range(0, bridges.Length);
        //    flag = bridges[index].isAssigned;
        //}
        //while (flag);
        //bridges[index].isAssigned = true;

        //return bridges[index];
    }
}
