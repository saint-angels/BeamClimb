using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private StatefulEventInt<PlayerState> state = StatefulEventInt.CreateEnum(PlayerState.ATTACHED);

    private Beam currentBeam = null;
    
    // Start is called before the first frame update
    void Start()
    {
        state.OnValueChanged += OnStateChanged;
    }

    private void OnStateChanged(PlayerState newState)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state.Value)
        {
            case PlayerState.ATTACHED:
                //Rotate af
                
                break;
            case PlayerState.DETACHED:
                Beam[] allBeams = FindObjectsOfType<Beam>();
                for (int i = 0; i < allBeams.Length; i++)
                {
                    Beam beam = allBeams[i];
                
                }
                break;
        }
        
        
    }

    private void AttachToBeam(Beam newBeam)
    {
        state.Set(PlayerState.ATTACHED);
    }
}
