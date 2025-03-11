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


}
