using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float rotateSpeed = 15f;

    public Transform playerTrans;
    public Transform characterTrans;

    public PlayerProperty playerProperty;
    public StepStack stepStack;
    public Animator animController;
    public Platform currPlatform;
    public Bridge currBridge;

    public virtual void Start()
    {
        playerTrans = transform;
    }

}
