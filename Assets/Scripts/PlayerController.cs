﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float maxSpeed = 100f;
    [SerializeField] float jumpForce = 100f;
    [SerializeField] LayerMask Ground_Layer;
    [SerializeField] string groundCheckName = "GroundCheck";
    [SerializeField] string ceilingCheckName = "CeilingCheck";
    [SerializeField] Transform centerOfMass;
    
    Transform groundCheck;                      //地面檢查
    const float k_GroundRadius = 0.2f;          
    bool isGrounded;
    Transform ceilingCheck;                     //天花板檢查
    const float k_CeilingRadius = 0.1f;
    Animator characterAnim;
    Rigidbody2D characterRigidBody;
    bool bFacingRight = true;
    bool bJump = false;
    float speedFactor = 0f;

    private void Awake()
    {
        groundCheck = transform.Find(groundCheckName);
        ceilingCheck = transform.Find(ceilingCheckName);
        characterAnim = GetComponent<Animator>();
        characterRigidBody = GetComponent<Rigidbody2D>();
        characterRigidBody.centerOfMass = centerOfMass.position;
    }

    private void Update()
    {
        speedFactor = Mathf.Lerp(0f, maxSpeed, 0.01f) * Time.fixedDeltaTime;
        if (Input.GetButtonDown("Jump"))
            bJump = true;
    }

    private void FixedUpdate()
    {
        Set_isGrounded();
        characterAnim.SetFloat("vSpeed", characterRigidBody.velocity.y);
        Character_Action();
    }

    void Character_Action()
    {
        // 左右移動  
        Move(Input.GetAxisRaw("Horizontal") * speedFactor, false, bJump);
        bJump = false;
    }

    void Move(float moveSpeed, bool crouch, bool jump)
    {
        //Crouch, , 接動畫(蹲下)
        //..

        //Moving, 接動畫(左右移動)
        if (isGrounded)
        {

            characterAnim.SetFloat("Speed", Mathf.Abs(moveSpeed));
            characterRigidBody.velocity = new Vector2(moveSpeed * maxSpeed, characterRigidBody.velocity.y);

            if (moveSpeed > 0 && !bFacingRight)
                Flip();
            else if (moveSpeed < 0 && bFacingRight)
                Flip();
        }

        //Jump, 接動畫(跳躍)
        if (isGrounded && jump)
        {
            isGrounded = false;
            characterAnim.SetBool("Ground", isGrounded);
            characterRigidBody.AddForce(new Vector2(0f, jumpForce));
        }
    }

    void Set_isGrounded()
    {
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, k_GroundRadius, Ground_Layer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                isGrounded = true;
        }
        characterAnim.SetBool("Ground", isGrounded);
    }

    void Flip()
    {
        bFacingRight = !bFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
