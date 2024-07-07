// Use a unity UI button to send input to an Emulator instance

using UnityEngine;
using UnityEngine.EventSystems;
using UnityHawk;

namespace Plundertools {

public class EmulatorInputButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public InputProvider inputProvider;
    public string keyName;

    public void OnPointerDown(PointerEventData eventData)
    {
        inputProvider.AddInputEvent(new UnityHawk.InputEvent {
            keyName = keyName,
            isPressed = true
        });
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputProvider.AddInputEvent(new UnityHawk.InputEvent {
            keyName = keyName,
            isPressed = false
        });
    }
}

}