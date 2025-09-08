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
    public bool multipleMaterials = false;
    [HideIf(nameof(multipleMaterials))]
    public Material blitMaterial;

    [ShowIf(nameof(multipleMaterials))]
    public Material[] blitMaterials;

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

        if ((!multipleMaterials && blitMaterial == null) || (multipleMaterials && (blitMaterials == null || blitMaterials.Length == 0))) {
            // No blit materials set, just set texture property directly to original image
            if (targetType == TargetType.RawImage) {
                rawImage.texture = emulator.Texture;
            } else {
                Debug.LogWarning("Using target type RenderTexture with no blit material makes no sense, just use the source texture");
                Graphics.CopyTexture(emulator.Texture, renderTexture); 
            }
        } else {
            // Blit material(s) set, create a new render texture and blit to that
            // then set that on the RawImage
            if (_auxTexture == null || _auxTexture.width != emulator.Texture.width || _auxTexture.height != emulator.Texture.height) {
                if (targetType == TargetType.RenderTexture) {
                    _auxTexture = renderTexture;
                } else {
                    // targetType is RawImage
                    _auxTexture = new RenderTexture(emulator.Texture.width, emulator.Texture.height, depth: 0);
                }
            }

            if (multipleMaterials) {
                Texture currentSource = emulator.Texture;
                RenderTexture tempRT;
                for (int i = 0; i < blitMaterials.Length; i++) {
                    // Create a temporary render texture if it's not the last step
                    if (i < blitMaterials.Length - 1) {
                        tempRT = RenderTexture.GetTemporary(emulator.Texture.width, emulator.Texture.height);
                    } else {
                        tempRT = _auxTexture; // For the last step, blit to the final destination
                    }
                    Graphics.Blit(currentSource, tempRT, blitMaterials[i], pass: 0);
                    if (currentSource != emulator.Texture) {
                        RenderTexture.ReleaseTemporary(currentSource as RenderTexture);
                    }
                    currentSource = tempRT; // Set the current source for the next iteration
                }
            } else {
                Graphics.Blit(emulator.Texture, _auxTexture, blitMaterial, pass: 0);
            }
            
            if (targetType == TargetType.RawImage) {
                rawImage.texture = _auxTexture;
            }
        }
    }

    [Button]
    void ClearTexture() {
        if (_auxTexture != null) {
            var activeRT = RenderTexture.active;
            RenderTexture.active = _auxTexture;
            GL.Clear(true, true, new Color(0, 0, 0, 0));
            RenderTexture.active = activeRT;
        }
    }
}

}