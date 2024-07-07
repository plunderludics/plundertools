// Capture texture from emulator and send it to a RawImage or a RenderTexture
// Optionally blitting with a material (e.g. chroma key) 

using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using UnityHawk;

namespace Plundertools {

[ExecuteInEditMode]
public class CaptureTexture : MonoBehaviour
{
    public Emulator emulator;
    public Material blitMaterial;

    public enum TargetType {
        RawImage,
        RenderTexture
    }
    public TargetType targetType;

    [ShowIf("targetType",TargetType.RawImage)]
    public RawImage rawImage;
    
    [ShowIf("targetType",TargetType.RenderTexture)]
    public RenderTexture renderTexture;

    [ReadOnly, SerializeField] private RenderTexture _auxTexture;

    void OnEnable()
    {
        if (rawImage == null) rawImage = GetComponent<RawImage>();
        _auxTexture = null;
    }

    void Update()
    {
        if (emulator.Texture == null) return;
        if (blitMaterial) {
            // If a blit material is set, create a new render texture and blit to that
            // then set that on the RawImage
            if (_auxTexture == null) {
                // TODO: do this also if the size of the emulator texture has changed
                if (targetType == TargetType.RenderTexture) {
                    _auxTexture = renderTexture;
                } else {
                    // targetType is RawImage
                    _auxTexture = new RenderTexture(emulator.Texture);
                }
            }
            
            Graphics.Blit(emulator.Texture, _auxTexture, blitMaterial, pass: 0);
            if (targetType == TargetType.RawImage) {
                rawImage.texture = _auxTexture;
            }
        } else {
            // Otherwise just set texture property directly to original image
            if (targetType == TargetType.RawImage) {
                rawImage.texture = emulator.Texture;
            } else {
                Debug.LogWarning("Using target type RenderTexture with no blit material makes no sense, just use the source texture");
                Graphics.CopyTexture(emulator.Texture, renderTexture); 
            }
        }
    }
}

}