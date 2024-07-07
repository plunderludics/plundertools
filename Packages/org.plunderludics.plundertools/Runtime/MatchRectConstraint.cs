using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Force a RectTransform to match the (world) size and position of another RectTransform.
// Not really sure if this works in all cases but it's good enough for now.
[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class MatchRectConstraint : MonoBehaviour
{
    private RectTransform _rectTransform;
    public RectTransform target;

    void OnEnable()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        _rectTransform.anchoredPosition3D = _rectTransform.parent.InverseTransformPoint(
            target.parent.TransformPoint(target.anchoredPosition3D)
        ); // Match position in world space
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, target.rect.width);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, target.rect.height);
        _rectTransform.localScale = target.localScale;
    }
}
