using TMPro;
using UnityEngine;

public class Step : MonoBehaviour
{
    [HideInInspector] public Platform platform;
    public GameObject m_Trail;
    public Vector3 moveToPos;
    public Vector3 stackLocalScale;

    public float moveSpeed;
    public float rotateSpeed;
    public bool isCollected = false;

    private Vector3 a;
    private Vector3 b;
    private Vector3 c;


    public float interpolateAmount;
    Vector3 pointAB;
    Vector3 pointBC;
    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c)
    {
        interpolateAmount = (interpolateAmount + (Time.deltaTime * moveSpeed));

        pointAB = Vector3.Lerp(a, b, interpolateAmount);
        pointBC = Vector3.Lerp(b, c, interpolateAmount);

        return Vector3.Lerp(pointAB, pointBC, interpolateAmount);
    }

    private void Start()
    {
        m_Trail.SetActive(false);
    }
    private void Update()
    {
        if (isCollected)
        {
            transform.localPosition = QuadraticLerp(a, b, c);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, rotateSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.localPosition, moveToPos) <= 0.5f)
            {
                transform.localPosition = moveToPos;
                transform.localRotation = Quaternion.identity;
                m_Trail.SetActive(false);
                isCollected = false;
            }
        }
    }

    public void MoveToStack(Vector3 stackTopPos, Vector3 playerTopPos)
    {
        platform.AddAvailableStepPosition(transform.position);
        a = transform.localPosition;
        b = playerTopPos;
        c = stackTopPos;
        transform.localScale = stackLocalScale;
        m_Trail.SetActive(true);
        moveToPos = stackTopPos;
        interpolateAmount = 0;

        isCollected = true;
    }
}
