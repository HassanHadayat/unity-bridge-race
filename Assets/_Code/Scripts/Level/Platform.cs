using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Header("Level")]
    public Level level;

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

    public GameObject AIDumbSteps;

    private void Start()
    {

        for (int i = 0; i < stepSpawnPositions.Length; i++)
        {
            int randIndex = Random.Range(i, stepSpawnPositions.Length);
            Transform temp = stepSpawnPositions[i];
            stepSpawnPositions[i] = stepSpawnPositions[randIndex];
            stepSpawnPositions[randIndex] = temp;
            availablePositions.Add(stepSpawnPositions[i].position);
        }
        Debug.Log("Avail Pos Count = " + availablePositions.Count);

        for (int i = 0; i < bridges.Length; i++)
        {
            int randIndex = Random.Range(i, bridges.Length);
            Bridge temp = bridges[i];
            bridges[i] = bridges[randIndex];
            bridges[randIndex] = temp;
            availableBridgeIndexes.Add(i);
        }
    }
    public void SetupPlatform(Level _level, bool initialize = false)
    {
        level = _level;
        if (initialize) SetPlayerSteps(level.PlayerColors);
    }

    public void SetPlayerSteps(string[] playerColors)
    {
        //int extras = (stepSpawnPositions.Length % level.NoOfPlayers);

        foreach (var color in playerColors)
        {
            if (color == "Blue")
            {
                SetPlayerSteps(stepBluePrefab);
            }
            else if (color == "Red")
            {
                SetPlayerSteps(stepRedPrefab);
                AIDumbSteps.SetActive(true);
            }
            else if (color == "Green")
            {
                SetPlayerSteps(stepGreenPrefab);
                AIDumbSteps.SetActive(true);
            }
            else if (color == "Pink")
            {
                SetPlayerSteps(stepPinkPrefab);
                AIDumbSteps.SetActive(true);
            }
        }
    }

    public void SetPlayerSteps(GameObject stepPrefab)
    {
        Debug.Log("Setting Player Steps");
        int count = (stepSpawnPositions.Length / level.NoOfPlayers);

        for (int i = 0; i < count; i++)
        {
            Debug.Log("Setting Player Steps -> Test 1 = " + availablePositions.Count);
            if (availablePositions.Count <= 0) return;
            GameObject step = Instantiate(stepPrefab, availablePositions[0], Quaternion.identity, stepSpawnPositionsTrans);
            Debug.Log("Setting Player Steps -> Test 2");
            availablePositions.RemoveAt(0);
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
        if (availablePositions.Count <= 0) return;

        int randValue = Random.Range(0, availablePositions.Count);

        step.transform.parent = stepSpawnPositionsTrans;
        step.transform.position = availablePositions[randValue];
        step.transform.localScale = platformStepLocalScale;
        step.transform.localRotation = Quaternion.identity;

        RemoveAvailableStepPosition(availablePositions[randValue]);
    }

    public int GetStepsColorCount(string playerTag)
    {
        int count = 0;
        string stepTag = "Undefined";
        if (playerTag.Contains("Blue"))
        {
            stepTag = "Blue Step";
        }
        else if (playerTag.Contains("Red"))
        {
            stepTag = "Red Step";
        }
        else if (playerTag.Contains("Green"))
        {
            stepTag = "Green Step";
        }
        else if (playerTag.Contains("Pink"))
        {
            stepTag = "Pink Step";
        }

        foreach (GameObject step in platformSteps)
        {
            if (step.CompareTag(stepTag))
            {
                count++;
            }
        }
        return count;
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
            if (bridges != null && bridges.Length > 0)
                return bridges[Random.Range(0, bridges.Length)];
            else return null;
        }
    }
}
