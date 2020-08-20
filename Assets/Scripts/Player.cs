using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers.StatefulEvent;
using UnityEngine;

public class Player : MonoBehaviour
{

    public enum PlayerState
    {
        ATTACHED,
        DETACHED
    }

    public IStatefulEvent<PlayerState> State => state;

    [SerializeField] private Collider2D trigger;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform rotationPoint;
    [SerializeField] private Transform legsPoint = null;
    [SerializeField] private Vector3 roationAxis;

    [SerializeField] private float detachForce = 10f;
    
    private StatefulEventInt<PlayerState> state = StatefulEventInt.CreateEnum(PlayerState.ATTACHED);

    private Beam currentBeam = null;

    [SerializeField] private float roationValue;
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        state.OnValueChanged += OnStateChanged;
        
        state.Set(PlayerState.DETACHED);
    }

    private void OnStateChanged(PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.ATTACHED:
                transform.rotation = Quaternion.identity;
                transform.position = currentBeam.transform.position + Vector3.up * .5f;
                trigger.enabled = false;
                rb.velocity = Vector3.zero;
                rb.simulated = false;
                break;
            case PlayerState.DETACHED:
                if (currentBeam != null)
                {
                    Destroy(currentBeam.gameObject);    
                }
                trigger.enabled = true;
                rb.simulated = true;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    { 
        switch (state.Value)
        {
            case PlayerState.ATTACHED:
                //Rotate af

                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    Detach();
                }
                else
                {
                    transform.RotateAround(rotationPoint.position, roationAxis, roationValue * Time.deltaTime);
                }
                
                break;
            case PlayerState.DETACHED:
                
                break;
        }
    }

    private void Detach()
    {
        transform.DOPunchScale(new Vector3(0, -.5f, 0), .2f, 0, .1f).SetEase(Ease.InOutQuint);
        state.Set(PlayerState.DETACHED);
        Vector2 flyDirection = (legsPoint.position - rotationPoint.position).normalized;
        rb.AddForce(flyDirection * detachForce, ForceMode2D.Impulse);

    }

    private void FixedUpdate()
    {
        switch (state.Value)
        {
            case PlayerState.ATTACHED:
                
                break;
            case PlayerState.DETACHED:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        currentBeam = other.GetComponent<Beam>();
        state.Set(PlayerState.ATTACHED);
    }
}
