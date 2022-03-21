using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class AICharacterMovement : MonoBehaviour
{
    [SerializeField]
    float m_MoveSpeedMultiplier = 1f;
    [SerializeField]
    float m_AnimSpeedMultiplier = 1f;
    [SerializeField]
    float m_MovingTurnSpeed = 360;
    [SerializeField]
    float m_StationaryTurnSpeed = 180;
    Vector3 m_GroundNormal;
    Animator m_Animator;
    private CharacterController m_CharacterController;
    private Vector3 m_MoveDir = Vector3.zero;

    public bool m_Crouching = false;
    float m_TurnAmount;
    float m_TargetTurnAmount;
    float m_ForwardAmount;
    float m_TargetForwardAmount;
    private CollisionFlags m_CollisionFlags;

    private UnityEngine.AI.NavMeshAgent m_Agent;

    void OnEnable()
    {
        m_Animator = GetComponent<Animator>();
        if (!m_Animator)
            m_Animator = GetComponentInChildren<Animator>();

        m_CharacterController = GetComponent<CharacterController>();

        m_Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // m_OrigGroundCheckDistance = m_GroundCheckDistance;
        //  m_Jumping = false;
        //   m_IsGrounded = true;
    }
    public void SetMovementSpeed(float animSpeed, float agentSpeed)
    {
        m_AnimSpeedMultiplier = Mathf.Clamp(animSpeed, 0.5f, 1.5f);
        m_Agent.speed = agentSpeed;
    }
    private void FixedUpdate()
    {

        m_TurnAmount = Mathf.SmoothStep(m_TurnAmount, m_TargetTurnAmount, Time.deltaTime * 10f);
        m_ForwardAmount = Mathf.SmoothStep(m_ForwardAmount, m_TargetForwardAmount, Time.deltaTime * 100f);

        //  m_IsGrounded = m_CharacterController.isGrounded;
        //  m_Animator.SetBool("OnGround", m_IsGrounded);
        //  CheckGroundStatus();
        //  if (!m_IsGrounded)
        //        m_CharacterController.Move(Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime);
    }
    // Update is called once per frame

    public void Move(Vector3 move)
    {

     //   if (!m_IsGrounded)
      //      return;
       // if (move.magnitude > 1f)
        //    move.Normalize();

        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TargetTurnAmount = Mathf.Atan2(move.x, move.z);
        m_TargetForwardAmount = move.z;
        ApplyExtraTurnRotation();
        UpdateAnimator(move );
    }
    

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }
    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        m_Animator.SetBool("Crouch", m_Crouching);

        float clampedForward = Mathf.Clamp(m_ForwardAmount, -1f, 1f);
        m_Animator.SetFloat("Forward", clampedForward, 0.05f, Time.deltaTime);
        float clampedTurn = Mathf.Clamp(m_TurnAmount, -0.5f, 0.5f);
        m_Animator.SetFloat("Turn", clampedTurn, 0.15f, Time.deltaTime);



        if (move.magnitude > 0)
            m_Animator.speed = m_AnimSpeedMultiplier;
        else
            m_Animator.speed = 1;
        
    }



    // [SerializeField]
    //  float m_GroundCheckDistance = 0.2f;
    // [SerializeField]
    //  private float m_StickToGroundForce = 10f;
    //   [SerializeField]
     //   private float m_GravityMultiplier = 2f;
    //   private bool m_PreviouslyGrounded;
     //   [HideInInspector]public bool m_IsGrounded;
    //   private bool m_Jumping;
    //  float m_OrigGroundCheckDistance;
    //   

    
    public void OnAnimatorMove()
    {/*
        Vector3 newPosition = transform.position;
        newPosition += m_Animator.deltaPosition * Time.deltaTime;
        transform.position = newPosition;*/
    }
    /*
    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        #if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
        #endif
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance))
        {
            //m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
          //  m_Animator.applyRootMotion = true;
        }
        else
        {
            m_IsGrounded = false;
           // m_GroundNormal = Vector3.up;
          //  m_Animator.applyRootMotion = false;
        }
    }*/
    }


