using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using TMPro;
public class Player : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject penguin;
    [SerializeField] ParticleSystem dieParticels;
    [SerializeField, Range(0.01f, 1f)] float moveDuration = 0.2f;
    [SerializeField, Range(0.01f, 1f)] float jumpHeight = 0.5f;
    private int minZPos;
    private int extent;
    private float backBoundary;
    private float leftBoundary;
    private float rightBoundary;
    [SerializeField] private int maxTravel;
    public int MaxTravel { get => maxTravel; }
    [SerializeField] private int currentTravel;
    public int CurrentTravel { get => currentTravel; }
    public bool IsDie { get => this.enabled == false; }


    public void SetUp(int minZPos, int extent)
    {
        backBoundary = minZPos - 1;
        leftBoundary = -(extent + 1);
        rightBoundary = extent + 1;
    }

    private void Update()
    {
        var moveDirection = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            moveDirection += new Vector3(0, 0, 1);
            JumpSFX();
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            moveDirection += new Vector3(0, 0, -1);
            JumpSFX();
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            moveDirection += new Vector3(1, 0, 0);
            JumpSFX();
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            moveDirection += new Vector3(-1, 0, 0);
            JumpSFX();
        }

        if (moveDirection == Vector3.zero)
            return;

        if (IsJumping() == false)
            Jump(moveDirection);
    }

    public void JumpSFX()
    {
        AudioSource sound = penguin.GetComponent<AudioSource>();
        sound.Play();
    }

    private void Jump(Vector3 TargetDirection)
    {
        var TargetPosition = transform.position + TargetDirection;

        transform.LookAt(TargetPosition);

        var moveSeq = DOTween.Sequence(transform);
        moveSeq.Append(transform.DOMoveY(jumpHeight, moveDuration / 2));
        moveSeq.Append(transform.DOMoveY(0, moveDuration / 2));

        if (
            TargetPosition.z <= backBoundary ||
            TargetPosition.x <= leftBoundary ||
            TargetPosition.x >= rightBoundary
            )
            return;

        if (Tree.AllPositions.Contains(TargetPosition))
            return;

        transform.DOMoveX(TargetPosition.x, moveDuration);
        transform
            .DOMoveZ(TargetPosition.z, moveDuration)
            .OnComplete(UpdateTravel);
    }

    private void UpdateTravel()
    {
        currentTravel = (int)this.transform.position.z;

        if (currentTravel > maxTravel)
            maxTravel = currentTravel;

        scoreText.text = maxTravel.ToString();
    }
    public bool IsJumping()
    {
        return DOTween.IsTweening(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.enabled == false)
            return;

        if (other.tag == "Car")
        {
            AnimateCrash();
        }
    }

    private void AnimateCrash()
    {
        transform.DOScaleY(0.1f, 0.2f);
        transform.DOScaleX(2, 0.2f);
        transform.DOScaleZ(2, 0.2f);

        this.enabled = false;
        dieParticels.Play();
    }
}
