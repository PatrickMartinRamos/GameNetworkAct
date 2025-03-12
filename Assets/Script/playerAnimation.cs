using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class playerAnimation : NetworkBehaviour
{
    private Animator _animator;
    private playerInput _inputHandler;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _inputHandler = GetComponent<playerInput>();
    }

    void Update()
    {
        onWalkAnimation();
        onRunAnimation(); 
        onSprintAnimation();
        onJumpAnimation();
        onCrouchAnimation();
        onCrouchWalk();
        onSprintSlide();

        //Debug.Log("is crouch " + _inputHandler.Crouch);
        //Debug.Log("is walking " + _inputHandler.isWalking);
        //Debug.Log("is running " + _inputHandler.runInput);
        //Debug.Log("is sprinting " + _inputHandler.isSprinting);
    }
    public void onRunAnimation()
    {
        if(_inputHandler.isWalking || _inputHandler.isSprinting) return;
        
        _animator.SetFloat("Vertical", _inputHandler.runInput.y, 0.05f, Time.deltaTime);
        _animator.SetFloat("Horizontal", _inputHandler.runInput.x, 0.05f, Time.deltaTime);
    }
    public void onWalkAnimation()
    {
        if(_inputHandler.isWalking)
        {
            _animator.SetFloat("Vertical", _inputHandler.runInput.y / 2, 0.05f, Time.deltaTime);
            _animator.SetFloat("Horizontal", _inputHandler.runInput.x / 2, 0.05f, Time.deltaTime);
        }
    }
    public void onSprintAnimation()
    {
        if(_inputHandler.isSprinting)
        {
            _animator.SetFloat("Vertical", _inputHandler.runInput.y * 1.5f, 0.05f, Time.deltaTime);
            _animator.SetFloat("Horizontal", _inputHandler.runInput.x * 1.5f, 0.05f, Time.deltaTime);
        }
    }
    public void onJumpAnimation()
    {
        if(_inputHandler.Jump)
        {
            _animator.SetTrigger("Jump");
        }
        else{
            _animator.ResetTrigger("Jump");
        }
    }

    public void onCrouchAnimation()
    {
        if(!_inputHandler.isWalking && !_inputHandler.isSprinting && !_inputHandler.Jump)
        {
            if (_inputHandler.Crouch)
            {
                //standing to crouch idle
                _animator.SetBool("StandingToCrouch", true);
            }
            else
            {
                _animator.SetBool("StandingToCrouch", false);
            }
        }
    }
    public void onCrouchWalk()
    {
        //crouch idle to crouch walk
        if (!_inputHandler.isWalking && !_inputHandler.isSprinting && !_inputHandler.Jump)
        {
            if (_inputHandler.Crouch)
            {
                //standing to crouch idle
                _animator.SetFloat("C_Horizontal", _inputHandler.runInput.x * 1f, 0.15f, Time.deltaTime);
                _animator.SetFloat("C_Vertical", _inputHandler.runInput.y * 1f, 0.15f, Time.deltaTime);
            }
            //else
            //{
            //    _animator.SetBool("StandingToCrouch", false);
            //}
        }
    }

    public void onSprintSlide()
    {
        if (!_inputHandler.isWalking && !_inputHandler.Jump)
        {
            if (_inputHandler.isSprinting && _inputHandler.Crouch)
            {
                //sprint to slide
                _animator.SetFloat("Vertical", _inputHandler.runInput.y * 3f, 0.05f, Time.deltaTime);
                _animator.SetFloat("Horizontal", _inputHandler.runInput.x * 3f, 0.05f, Time.deltaTime);
            }
        }
    }
}
