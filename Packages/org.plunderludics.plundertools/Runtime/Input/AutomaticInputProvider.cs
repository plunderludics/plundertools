// [Work in progress]
// Simulate holding down a key or thumbstick

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityHawk;

namespace Plundertools {

public class AutomaticInputProvider : InputProvider {
    [Serializable]
    public class AutoInput {
        public string Name;
        public int Value;
    }

    [SerializeField] List<AutoInput> autoInputs;

    [SerializeField] List<string> initialKeyPresses;

    public override List<InputEvent> InputForFrame() {
        if (initialKeyPresses != null) {
            var pressed = initialKeyPresses.Select(keyName => new InputEvent {
                name = keyName,
                value = 1
            }).ToList();
            // initialKeyPresses = null;
            return pressed;
        } else {
            return new();
        }
    }
}

}