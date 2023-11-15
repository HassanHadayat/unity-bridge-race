using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;
    public float smoothLerp = 0;
    public float levelFinishedSmoothSpeed = 25f;
    private float lerpTime;
    public float rotationSpeed = 0.5f;
    public bool isRotate = false;
    public bool isLevelFinished = false;

    private void Start()
    {
        //transform.position = target.position + offset;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            if (isLevelFinished && isRotate)
            {
                // Level is finished And Camera rotating around the winner
                transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);

                transform.LookAt(target.position);
                return;
            }
            else if (isLevelFinished && !isRotate)
            {
                // Level is finished And Camera moving toward the winner

                Vector3 desiredPosition = target.position + (offset / 2);
                lerpTime += Time.deltaTime * levelFinishedSmoothSpeed;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, lerpTime);

                // Set the camera's position to the smoothed position
                transform.position = smoothedPosition;

                _camera.fieldOfView += levelFinishedSmoothSpeed * Time.deltaTime;
                _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, 60f, 70f);

                if (Vector3.Distance(transform.position, desiredPosition) <= 0.1f)
                {
                    isRotate = true;
                    _camera.fieldOfView = 60f;
                }
            }
            else
            {
                Vector3 desiredPosition = target.position + offset;

                smoothLerp += Time.deltaTime * smoothSpeed;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothLerp);

                if (transform.position == smoothedPosition)
                    smoothLerp = 0f;
                // Set the camera's position to the smoothed position
                transform.position = desiredPosition;

            }

        }

    }
    public void SetTarget(Transform _target)
    {
        target = _target;
    }
    public void LevelFinished(Transform _winnerTarget)
    {
        isLevelFinished = true;
        target = _winnerTarget;

        lerpTime = 0;
    }
}
