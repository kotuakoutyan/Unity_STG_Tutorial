using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inputs;
using FlyCharacter;

namespace Camerawork
{
    public abstract class BaseCamerawork : MonoBehaviour
    {
        protected IInputEventProvider InputEventProvider;

        virtual protected void Start()
        {
            InputEventProvider = transform.parent.GetComponentInChildren<IInputEventProvider>();
        }
    }
}
