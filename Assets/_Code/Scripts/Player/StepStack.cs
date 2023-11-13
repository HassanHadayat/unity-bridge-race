using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StepStack : MonoBehaviour
{
    public GameObject stackTextUI;
    public Animator stackUIAnimController;
    public TextMeshProUGUI stackCountText;

    public List<GameObject> steps = new List<GameObject>();
    public Vector3 stepTopPos = Vector3.zero;
    public Vector3 playerTopPos = Vector3.zero;
    public float stepHeight;

    [Header("Movement Variables")]
    public float backwardStartAngle;
    public float backwardEndAngle;
    public float backwardLerpSpeed;

    public float forwardStartAngle;
    public float forwardEndAngle;
    public float forwardLerpSpeed;

    public bool isNone = true;
    public bool isStill = false;
    public bool isMoving = false;
    public bool isCentering = false;
    public float normalizedTime = 0.0f;

    private void Start()
    {
        stackCountText.text = "0";
    }
    private void Update()
    {
        // Face UI toward the camera
        stackTextUI.transform.LookAt(Camera.main.transform);

        if (isNone)
        {
            return;
        }
        else if (isStill)
        {
            MoveStackSteps(1, forwardStartAngle, forwardEndAngle, forwardLerpSpeed);// Forward Direction
            if (normalizedTime == 1)
            {
                isStill = false;
                isNone = false;
                isCentering = true;
                normalizedTime = 0.0f;
            }
        }
        else if (isMoving)
        {
            MoveStackSteps(-1, backwardStartAngle, backwardEndAngle, backwardLerpSpeed);// Backward Direction
        }
        else if (isCentering)
        {
            MoveStackStepsToCenter(forwardLerpSpeed);
            if (normalizedTime == 1)
            {
                isStill = false;
                isCentering = false;
                isNone = true;
                normalizedTime = 0.0f;
            }
        }
    }
    public void UpdateStackUI()
    {
        int currCount = int.Parse(stackCountText.text);
        if (currCount == 0)
        {
            // Close
            stackUIAnimController.SetTrigger("Close");
        }
        else if (currCount > 0)
        {
            // Open
            stackUIAnimController.SetTrigger("Open");
        }
    }

    public void AddStep(GameObject step)
    {
        int currCount = int.Parse(stackCountText.text);
        if (currCount == 0)
            stackUIAnimController.SetTrigger("Open");

        // Increment Stack Count
        stackCountText.text = (currCount + 1).ToString();

        StartCoroutine(PopStackCountUI());

        step.transform.parent = this.transform;
        step.tag = "Collected";
        step.GetComponent<Step>().MoveToStack(stepTopPos, playerTopPos);

        steps.Add(step);

        stepTopPos.y = steps.Count * stepHeight;
        playerTopPos.y += stepHeight;
    }
    public void RemoveStep()
    {
        int currCount = int.Parse(stackCountText.text);
        if (currCount == 1)
            stackUIAnimController.SetTrigger("Close");

        // Decremetn Stack Count
        stackCountText.text = (int.Parse(stackCountText.text) - 1).ToString();
        StartCoroutine(PopStackCountUI());

        // Restore that step in the platform
        GameObject step = steps[steps.Count - 1];
        step.tag = step.transform.parent.tag;
        step.GetComponent<Step>().platform.AddPlatformStep(step);
        steps.RemoveAt(steps.Count - 1);


        stepTopPos.y = steps.Count * stepHeight;
        playerTopPos.y -= stepHeight;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(this.tag))
        {
            AddStep(other.gameObject);
        }

    }

    public void MoveStack()
    {
        if (steps.Count <= 1 || isMoving) return;

        normalizedTime = 0f;

        isNone = false;
        isStill = false;
        isCentering = false;
        isMoving = true;
        //MoveStackMovement();
        MoveStackSteps(-1, backwardStartAngle, backwardEndAngle, backwardLerpSpeed);// Backward Direction

    }
    public void StillStack()
    {
        if (steps.Count <= 1 || isNone) return;


        normalizedTime = 0f;

        isNone = false;
        isMoving = false;
        //isCentering = true;
        isStill = true;
        //MoveStackStepsToCenter(forwardLerpSpeed);
        //StillStackMovement();
        MoveStackSteps(1, forwardStartAngle, forwardEndAngle, forwardLerpSpeed);// Forward Direction

    }


    private void StillStackMovement()
    {
        Vector3 centerPoint = Vector3.zero; // The center of the circle
        centerPoint.z = -steps.Count;
        float radius = steps.Count;  // Radius of the circle
        int numPoints = steps.Count;   // Number of points to generate
        float startAngle = 0.0f; // Start angle in degrees
        float endAngle = 30.0f;   // End angle in degrees

        float startAngleRad = startAngle * (Mathf.PI / 180);
        float endAngleRad = endAngle * (Mathf.PI / 180);

        Vector3 tempPos = Vector3.zero;

        for (int i = 0; i < numPoints; i++)
        {
            float angle = Mathf.Lerp(startAngleRad, endAngleRad, i / (float)(numPoints - 1) * normalizedTime);
            float z = centerPoint.z + radius * Mathf.Cos(angle);

            // Position
            tempPos = steps[i].transform.localPosition;
            tempPos.z = -z;
            steps[i].transform.localPosition = tempPos;

            // Rotation
            Quaternion rotation = Quaternion.Euler(angle * Mathf.Rad2Deg, 0, 0);
            steps[i].transform.localRotation = rotation;
        }
        normalizedTime = Mathf.Clamp((normalizedTime + (Time.deltaTime * backwardLerpSpeed)), 0f, 1f);
        if (normalizedTime == 1)
        {
            tempPos = Vector3.zero;
            foreach (var step in steps)
            {
                tempPos = step.transform.localPosition;
                tempPos.z = 0f;
                step.transform.localPosition = tempPos;

                step.transform.localRotation = Quaternion.identity;
            }


            isMoving = false;
            isStill = false;
            isNone = true;
        }
    }
    public void MoveStackStepsToCenter(float lerpSpeed)
    {
        if (steps.Count <= 1) return;
        Vector3 tempPos = Vector3.zero;
        foreach (var step in steps)
        {
            tempPos = step.transform.localPosition;
            tempPos.z = 0f;

            step.transform.localPosition = Vector3.Lerp(step.transform.localPosition, tempPos, normalizedTime);
            step.transform.localRotation = Quaternion.Lerp(step.transform.localRotation, Quaternion.identity, normalizedTime);
        }
        normalizedTime = Mathf.Clamp((normalizedTime + (Time.deltaTime * lerpSpeed)), 0f, 1f);


        //isMoving = false;
        //isStill = false;
        //isNone = true;

    }
    private void MoveStackSteps(float direction, float startAngle, float endAngle, float lerpSpeed)
    {
        if (steps.Count <= 1) return;
        Vector3 centerPoint = Vector3.zero; // The center of the circle
        centerPoint.z = -steps.Count;
        float radius = steps.Count;  // Radius of the circle
        int numPoints = steps.Count;   // Number of points to generate

        float startAngleRad = startAngle * (Mathf.PI / 180);
        float endAngleRad = endAngle * (Mathf.PI / 180);

        Vector3 tempPos = Vector3.zero;

        for (int i = 0; i < numPoints; i++)
        {
            float angle = Mathf.Lerp(startAngleRad, endAngleRad, i / (float)(numPoints - 1) * normalizedTime);
            angle = (direction == -1) ? -angle : angle;
            float z = centerPoint.z + radius * Mathf.Cos(angle);

            // Position
            tempPos = steps[i].transform.localPosition;
            tempPos.z = (direction == 1) ? -z : z;
            steps[i].transform.localPosition = tempPos;

            // Rotation
            Quaternion rotation = Quaternion.Euler(angle * Mathf.Rad2Deg, 0, 0);
            steps[i].transform.localRotation = rotation;
        }
        normalizedTime = Mathf.Clamp((normalizedTime + (Time.deltaTime * lerpSpeed)), 0f, 1f);
    }



    System.Collections.IEnumerator PopStackCountUI()
    {
        stackCountText.transform.localScale = Vector3.one * 1.5f;

        yield return new WaitForSeconds(0.1f);
        stackCountText.transform.localScale = Vector3.one;
    }
}








//private void MoveStackMovement()
//{
//    Vector3 centerPoint = Vector3.zero; // The center of the circle
//    centerPoint.z = -steps.Count;
//    float radius = steps.Count;  // Radius of the circle
//    int numPoints = steps.Count;   // Number of points to generate
//    float startAngle = 0.0f; // Start angle in degrees
//    float endAngle = 25.0f;   // End angle in degrees

//    float startAngleRad = startAngle * (Mathf.PI / 180);
//    float endAngleRad = endAngle * (Mathf.PI / 180);


//    for (int i = 0; i < numPoints; i++)
//    {
//        float angle = Mathf.Lerp(startAngleRad, endAngleRad, (i / (float)(numPoints - 1)) * normalizedTime);
//        float z = centerPoint.z + radius * Mathf.Cos(angle);

//        // Position
//        Vector3 tempPos = steps[i].transform.localPosition;
//        tempPos.z = z;
//        steps[i].transform.localPosition = tempPos;

//        // Rotation
//        Quaternion rotation = Quaternion.Euler(-angle * Mathf.Rad2Deg, 0, 0);
//        steps[i].transform.localRotation = rotation;
//    }

//    normalizedTime = Mathf.Clamp(normalizedTime + (Time.deltaTime * forwardLerpSpeed), 0f, 1f);
//}