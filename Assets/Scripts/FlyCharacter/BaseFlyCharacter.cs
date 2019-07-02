using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs;
using FlyCharacter;

public abstract class BaseFlyCharacter : MonoBehaviour
{
    protected IInputEventProvider InputEventProvider;

    virtual protected void Start()
    {
        InputEventProvider = GetComponent<IInputEventProvider>();
    }
}
