// [Work in progress]
// Simulate holding down a key or thumbstick

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityHawk;

namespace Plundertools {

public class AutomaticInputProvider : InputProvider
{
    [Serializable]
    public class AutoInput {
        public string inputName;
        public int value;
    }
    [SerializeField] List<AutoInput> autoInputs;

    [SerializeField] List<string> initialKeyPresses;

    public override Dictionary<string, int> AxisValuesForFrame()
    {
        return autoInputs.ToDictionary(
            m => m.inputName,
            m => m.value
        );
    }


    public override List<InputEvent> InputForFrame()
    {
        if (initialKeyPresses != null) {
            var pressed = initialKeyPresses.Select(keyName => new InputEvent {
                keyName = keyName,
                isPressed = true
            }).ToList();
            // initialKeyPresses = null;
            return pressed;
        } else {
            return new();
        }
    }
}

}