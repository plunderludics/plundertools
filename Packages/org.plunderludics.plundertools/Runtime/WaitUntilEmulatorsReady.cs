using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Plundertools {

public class WaitUntilEmulatorsReady : MonoBehaviour
{
    public UnityEvent onReady;
    public List<UnityHawk.Emulator> emulators;
    public float extraDelay = 0.2f;

    [ReadOnly, SerializeField] bool _invoked;

    void OnEnable()
    {
        _invoked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_invoked && emulators.All(e => e.IsRunning)) {
            _invoked = true;
            this.StartCoroutine(InvokeAfterTime());
        }
    }

    IEnumerator InvokeAfterTime() {
        yield return new WaitForSeconds(extraDelay);
        onReady.Invoke();
    }
}

}