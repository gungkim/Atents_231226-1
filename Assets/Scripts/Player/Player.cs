using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour , IAlive
{
    PlayerInputActions inputActions;
    Rigidbody rigid;
    Animator animator;

    float moveDirection = 0.0f;

    public float moveSpeed = 5.0f;

    float currentMoveSpeed = 5.0f;

    float rotateDirection = 0.0f;

    public float rotateSpeed = 180.0f;

    readonly int IsMoveHash = Animator.StringToHash("IsMove");
    readonly int UseHash = Animator.StringToHash("Use");
    readonly int JumpHash = Animator.StringToHash("Jump");
    readonly int DieHash = Animator.StringToHash("Die");

    public float jumpPower = 6.0f;

    bool InAir
    {
        get => GroundCount < 1;
    }

    int GroundCount
    {
        get => groundCount;
        set
        {
            if (groundCount < 0)   
            {
                groundCount = 0;
            }
            groundCount = value;
            if (groundCount < 0)   
            {
                groundCount = 0;
            }
        }
    }

    int groundCount = 0;

    public float jumpCoolTime = 2.0f;

    float jumpCoolRemains = -1.0f;

    float JumpCoolRemains
    {
        get => jumpCoolRemains;
        set
        {
            jumpCoolRemains = value;
            onJumpCoolTimeChange?.Invoke(jumpCoolRemains / jumpCoolTime);
        }
    }

    public Action<float> onJumpCoolTimeChange;

    bool IsJumpAvailable => !InAir && (JumpCoolRemains < 0.0f) && isAlive;

    bool isAlive = true;

    public Action onDie;

    public float startLifeTime = 10.0f;

    float lifeTime = 0.0f;

    float LifeTime
    {
        get => lifeTime;
        set
        {
            if (isPlaying)  
            {
                lifeTime = value;
                if (lifeTime < 0.0f)
                {
                    lifeTime = 0.0f;   
                    Die();
                }
                onLifeTimeChange?.Invoke(lifeTime / startLifeTime);  
            }
        }
    }

    public Action<float> onLifeTimeChange;

    bool isPlaying = true;

    private void Awake()
    {
        inputActions = new();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        ItemUseChecker checker = GetComponentInChildren<ItemUseChecker>();
        checker.onItemUse += (interacable) => interacable.Use();
    }

    void Start()
    {
        currentMoveSpeed = moveSpeed;
        LifeTime = startLifeTime;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMoveInput;
        inputActions.Player.Move.canceled += OnMoveInput;
        inputActions.Player.Jump.performed += OnJumpInput;
        inputActions.Player.Use.performed += OnUseInput;
    }

    private void OnDisable()
    {
        inputActions.Player.Use.performed -= OnUseInput;
        inputActions.Player.Jump.performed -= OnJumpInput;
        inputActions.Player.Move.canceled -= OnMoveInput;
        inputActions.Player.Move.performed -= OnMoveInput;
        inputActions.Player.Disable();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        SetInput(context.ReadValue<Vector2>(), !context.canceled);
    }

    private void OnJumpInput(InputAction.CallbackContext _)
    {
        Jump();

    }

    private void OnUseInput(InputAction.CallbackContext context)
    {
        Use();
    }

    private void Update()
    {
        JumpCoolRemains -= Time.deltaTime;
        LifeTime -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        Use();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundCount++;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GroundCount--;
        }
    }

    void SetInput(Vector2 input, bool isMove)
    {
        rotateDirection = input.x;
        moveDirection = input.y;

        animator.SetBool(IsMoveHash, isMove);
    }

    void Move()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * currentMoveSpeed * moveDirection * transform.forward);
    }

    void Rotate()
    {
        Quaternion rotate = Quaternion.AngleAxis(Time.fixedDeltaTime * rotateSpeed * rotateDirection, transform.up);

        rigid.MoveRotation(rigid.rotation * rotate);
    }

    void Jump()
    {
        if (IsJumpAvailable)
        {
            animator.SetTrigger(JumpHash);
            rigid.AddForce(jumpPower * Vector3.up, ForceMode.Impulse);
            JumpCoolRemains = jumpCoolTime;       
        }
    }

    void Use()
    {
        animator.SetTrigger(UseHash);
    }

    public void Die()
    {
        if (isAlive)
        {
            Debug.Log("죽었음");

            animator.SetTrigger(DieHash);

            inputActions.Player.Disable();

            onDie?.Invoke();

            isAlive = false;
        }
    }

    public void SetSpeedModifier(float ratio = 1.0f)
    {
        currentMoveSpeed = moveSpeed * ratio;
    }

    public void RestoreMoveSpeed()
    {
        currentMoveSpeed = moveSpeed;
    }

    private void OnGameClear()
    {
        isPlaying = false;

        inputActions.Player.Disable();
    }
}
