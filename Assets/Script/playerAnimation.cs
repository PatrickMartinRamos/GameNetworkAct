using System;
using System.Collections;
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
        if (IsOwner)
        {
            onWalkAnimation();
            onRunAnimation(); 
            onSprintAnimation();
            onJumpAnimation();
            onCrouchAnimation();
            onSprintSlide();
        }
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
            _animator.SetBool("Jump", true);
        }
        else
        {
            _animator.SetBool("Jump", false);
        }
    }

    public void onCrouchAnimation()
    {
        if(!_inputHandler.isWalking && !_inputHandler.isSprinting && !_inputHandler.Jump)
        {
            if (_inputHandler.Crouch)
            {
                _animator.SetBool("StandingToCrouch", true);
                _animator.SetFloat("C_Horizontal", _inputHandler.runInput.x, 0.05f, Time.deltaTime);
                _animator.SetFloat("C_Vertical" , _inputHandler.runInput.y, 0.05f, Time.deltaTime);
            }
            else
            {
                _animator.SetBool("StandingToCrouch", false);
            }
        }
    }

    public void onSprintSlide()
    {
        if (!_inputHandler.isWalking && !_inputHandler.Jump)
        {
            if (_inputHandler.runningSlide)
            {
                _animator.SetBool("SprintSlide", true);
            }
            else
            {
                _animator.SetBool("SprintSlide", false);
            }
        }
    }
}
