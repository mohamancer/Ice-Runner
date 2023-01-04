using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [HideInInspector] public Vector3 moveVector;
    [HideInInspector] public float verticalVeclocity;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public int currentLane;

    public float distanceInBetweenLanes = 3.0f;
    public float baseRunSpeed = 5.0f;



    public float baseSidewaySpeed = 10.0f;
    public float gravity = 14.0f;
    public float terminalVelocity = 20.0f;
    public AudioClip hitSound;

    public CharacterController controller;
    public Animator anim;

    private BaseState state;
    private bool isPaused;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        state = GetComponent<RunningState>();
        state.Construct();

        isPaused = true;
    }


    private void Update()
    {
        if (!isPaused)
            UpdateMotor();
    }
    private void UpdateMotor()
    {
        //check if grounded
        isGrounded = controller.isGrounded;

        //how should we move
        moveVector = state.ProcessMotion();

        //are we trying  to change states
        state.Transition();

        // feed our animator some values
        anim?.SetBool("IsGrounded", isGrounded);
        anim?.SetFloat("Speed", Mathf.Abs(moveVector.z));

        // Move the player
        controller.Move(moveVector * Time.deltaTime);

    }
    public float SnapToLane()
    {
        float r = 0.0f;

        // If we're not directely on top of a lane
        if (transform.position.x != (currentLane * distanceInBetweenLanes)) 
        {
            float deltaToDesiredPosition = (currentLane * distanceInBetweenLanes) - transform.position.x;
            r = (deltaToDesiredPosition > 0) ? 1 : -1;
            r *= baseSidewaySpeed;

            float actualDistance = r * Time.deltaTime;
            if (Mathf.Abs(actualDistance) > Mathf.Abs(deltaToDesiredPosition))
                r = deltaToDesiredPosition * (1 / Time.deltaTime);
        }
        else
        {
            r = 0;
        }

        return r;
    }
    public void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }
    public void ChangeState(BaseState s)
    {
        state.Destruct();
        state = s;
        state.Construct();
    }
    public void ApplyGravity()
    {
       verticalVeclocity -= gravity * Time.deltaTime;
        if (verticalVeclocity < -terminalVelocity)
            verticalVeclocity = -terminalVelocity;
    }
    public void PausePlayer()
    {
        isPaused = true;
    }
    public void ResumePlayer()
    {
        isPaused = false;
    }
    public void RespawnPlayer()
    {
        ChangeState(GetComponent<RespawnState>());
        GameManager.Instance.ChangeCamera(GameCamera.Respawn);
    }
    public void ResetPlayer()
    {
        currentLane = 0;
        transform.position = Vector3.zero;
        anim?.SetTrigger("Idle");
        PausePlayer();
        ChangeState(GetComponent<RunningState>());
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer);

        if (hitLayerName == "Death") 
        {
            AudioManager.Instance.PlaySFX(hitSound, 0.7f);
            ChangeState(GetComponent<DeathState>());
        }
    }
}
