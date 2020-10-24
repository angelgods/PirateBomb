﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 游戏玩家控制器，主要包含玩家的基本移动即左右移动和跳跃攻击
public class PlayerController : MonoBehaviour
{
    // 获取刚体组件 
    private Rigidbody2D rb;
    public float speed;
    public float jumpForce; // 跳跃的力

    [Header("Ground Check")] public Transform groundCheck; // 检测的点
    public float checkRadius; // 检测范围
    public LayerMask groundLayer; // 选择对应的图层
    [Header("States Check")] public bool isGround; // 是否在地面的状态检测
    public bool isCanJump; // 是否可以跳跃
    public bool isJump; // 判断是否正在跳跃
    [Header("Jump FX")] public GameObject landFX;
    public GameObject jumpFX;
    [Header("Attack Settings")] public GameObject bombPrefab; // 获得炸弹
    public float nextAttack = 0; // 下一次攻击时间 当前时间+cd
    public float attackRate = 0; // cd时间

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckInput();
    }

    void FixedUpdate()
    {
        PhysicsCheck(); // 一开始就检测物理
        Movement();
        Jump();
    }

    // 执行人物移动的方法 需要获取按键
    void Movement()
    {
        // float horizontalInput = Input.GetAxis("Horizontal"); // 获取的值从-1～1，包括小数

        float horizontalInput = Input.GetAxisRaw("Horizontal"); // 获取的值从-1～1，不包含小数
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y); // 使用物理向量的速度固定移动速度 y保持不变，x为输入*速度

        // 修改transform的本地坐标来进行翻转
        if (horizontalInput != 0)
            transform.localScale = new Vector3(horizontalInput, 1, 1);
    }

    // 控制Player跳跃
    void Jump()
    {
        if (isCanJump)
        {
            // isJump = true;

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isCanJump = false;
        }
    }
    
    // 玩家攻击方法 
    public void Attack()
    {
        // 当前时间>下一次可攻击时间
        if (Time.time > nextAttack)
        {
            // 在场景中生成炸弹
            Instantiate()
        }

    }

    // 用来收集Player输入的操作
    void CheckInput()
    {
        // 当按下跳跃按键的时候同时也要检测是否在地面
        if (Input.GetButtonDown("Jump") && isGround)
            isCanJump = true;
    }

    // 地面检测函数使用圆形检测
    void PhysicsCheck()
    {
        // 坐标，检测范围，图层
        isGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (isGround)
        {
            rb.gravityScale = 1;
            isJump = false; // 落地为false
        }
        else
        {
            rb.gravityScale = 4;
            isJump = true;
        }
    }

    // 在人物落地的第一帧显示落地的特效
    public void LandFX()
    {
        landFX.SetActive(true);
        landFX.transform.position = transform.position + new Vector3(0, -0.72f, 0);
    }

    // 可视化检测范围 系统方法,不需要在任何Update里进行调用
    public void OnDrawGizmos()
    {
        // 中心点/检测范围
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }
}