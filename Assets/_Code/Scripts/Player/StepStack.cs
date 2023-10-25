using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StepStack : MonoBehaviour
{
    public List<GameObject> steps = new List<GameObject>();
    public Vector3 stepTopPos = Vector3.zero;
    public Vector3 playerTopPos = Vector3.zero;
    public float stepHeight;

    public void AddStep(GameObject step)
    {
        step.transform.parent = this.transform;
        step.GetComponent<Step>().MoveToStack(stepTopPos, playerTopPos);

        steps.Add(step);

        stepTopPos.y = steps.Count * stepHeight;
        playerTopPos.y += stepHeight;
    }
    public void RemoveStep()
    {
        // Restore that step in the platform
        GameObject step = steps[steps.Count - 1];
        step.GetComponent<Step>().platform.AddPlatformStep(step);
        steps.RemoveAt(steps.Count - 1);


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

    public void MoveStack()
    {
        if (steps.Count <= 1) return;

        Vector3 centerPoint = Vector3.zero; // The center of the circle
        centerPoint.z = -steps.Count;
        float radius = steps.Count;  // Radius of the circle
        int numPoints = steps.Count;   // Number of points to generate
        float startAngle = 0.0f; // Start angle in degrees
        float endAngle = 25.0f;   // End angle in degrees

        float startAngleRad = startAngle * (Mathf.PI / 180);
        float endAngleRad = endAngle * (Mathf.PI / 180);

        for (int i = 0; i < numPoints; i++)
        {
            float angle = Mathf.Lerp(startAngleRad, endAngleRad, i / (float)(numPoints - 1));
            float z = centerPoint.z + radius * Mathf.Cos(angle);

            // Position
            Vector3 tempPos = steps[i].transform.localPosition;
            tempPos.z = z;
            steps[i].transform.localPosition = tempPos;

            // Rotation
            Quaternion rotation = Quaternion.Euler(-angle * Mathf.Rad2Deg, 0, 0);
            steps[i].transform.localRotation = rotation;
        }

    }
    public void StillStack()
    {
        if (steps.Count <= 1) return;

        Vector3 centerPoint = Vector3.zero; // The center of the circle
        centerPoint.z = -steps.Count;
        float radius = steps.Count;  // Radius of the circle
        int numPoints = steps.Count;   // Number of points to generate
        float startAngle = 0.0f; // Start angle in degrees
        float endAngle = 20.0f;   // End angle in degrees

        float startAngleRad = startAngle * (Mathf.PI / 180);
        float endAngleRad = endAngle * (Mathf.PI / 180);

        Vector3 tempPos = Vector3.zero;

        for (int i = 0; i < numPoints; i++)
        {
            float angle = Mathf.Lerp(startAngleRad, endAngleRad, i / (float)(numPoints - 1));
            float z = centerPoint.z + radius * Mathf.Cos(angle);

            // Position
            tempPos = steps[i].transform.localPosition;
            tempPos.z = -z;
            steps[i].transform.localPosition = tempPos;

            // Rotation
            Quaternion rotation = Quaternion.Euler(angle * Mathf.Rad2Deg, 0, 0);
            steps[i].transform.localRotation = rotation;
        }

        tempPos = Vector3.zero;
        foreach (var step in steps)
        {
            tempPos = step.transform.localPosition;
            tempPos.z = 0f;
            step.transform.localPosition = tempPos;

            step.transform.localRotation = Quaternion.identity;
        }

    }
}
