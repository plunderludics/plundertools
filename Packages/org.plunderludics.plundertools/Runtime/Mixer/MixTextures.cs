using System;
using System.Collections.Generic;
using Soil;
using UnityEngine;

public class MixTextures : MonoBehaviour {
    public Material mat;

    [System.Serializable]
    public class TrackMix {
        [UnityEngine.Range(0, 1)]
        public float Mix;

        public Rect Rect = new Rect(0, 0, 1, 1);
    }

    public TrackMix mix0;
    public TrackMix mix1;
    public TrackMix mix2;
    public TrackMix mix3;
    public TrackMix mix4;
    public TrackMix mix5;
    public TrackMix mix6;
    public TrackMix mix7;

    [Header("remapping input")]
    public MapInCurve Curve;

    public float tex0mix {
        get => mix0.Mix;
        set => mix0.Mix = value;
    }

    public float tex1mix {
        get => mix1.Mix;
        set => mix1.Mix = value;
    }

    public float tex2mix {
        get => mix2.Mix;
        set => mix2.Mix = value;
    }

    public float tex3mix {
        get => mix3.Mix;
        set => mix3.Mix = value;
    }

    public float tex4mix {
        get => mix4.Mix;
        set => mix4.Mix = value;
    }

    public float tex5mix {
        get => mix5.Mix;
        set => mix5.Mix = value;
    }

    public float tex6mix {
        get => mix6.Mix;
        set => mix6.Mix = value;
    }

    public float tex7mix {
        get => mix7.Mix;
        set => mix7.Mix = value;
    }

    MaterialPropertyBlock _mpb;

    List<float> _mixList;

    static readonly int s_TEX0_Mix = Shader.PropertyToID("_mix0");
    static readonly int s_TEX0_ST = Shader.PropertyToID("_tex0_ST");
    static readonly int s_TEX1_Mix = Shader.PropertyToID("_mix1");
    static readonly int s_TEX1_ST = Shader.PropertyToID("_tex1_ST");
    static readonly int s_TEX2_Mix = Shader.PropertyToID("_mix2");
    static readonly int s_TEX2_ST = Shader.PropertyToID("_tex2_ST");
    static readonly int s_TEX3_Mix = Shader.PropertyToID("_mix3");
    static readonly int s_TEX3_ST = Shader.PropertyToID("_tex3_ST");
    static readonly int s_TEX4_Mix = Shader.PropertyToID("_mix4");
    static readonly int s_TEX4_ST = Shader.PropertyToID("_tex4_ST");
    static readonly int s_TEX5_Mix = Shader.PropertyToID("_mix5");
    static readonly int s_TEX5_ST = Shader.PropertyToID("_tex5_ST");
    static readonly int s_TEX6_Mix = Shader.PropertyToID("_mix6");
    static readonly int s_TEX6_ST = Shader.PropertyToID("_tex6_ST");
    static readonly int s_TEX7_Mix = Shader.PropertyToID("_mix7");
    static readonly int s_TEX7_ST = Shader.PropertyToID("_tex7_ST");

    public void SetTex0mix(float value) => SetTexMix(0, value);
    public void SetTex1mix(float value) => SetTexMix(1, value);
    public void SetTex2mix(float value) => SetTexMix(2, value);
    public void SetTex3mix(float value) => SetTexMix(3, value);
    public void SetTex4mix(float value) => SetTexMix(4, value);
    public void SetTex5mix(float value) => SetTexMix(5, value);
    public void SetTex6mix(float value) => SetTexMix(6, value);
    public void SetTex7mix(float value) => SetTexMix(7, value);

    public void SetTrackMix(int index, float value) => SetTexMix(index, value);

    void SetTexMix(int index, float value) {
        var mix = index switch {
            0 => mix0,
            1 => mix1,
            2 => mix2,
            3 => mix3,
            4 => mix4,
            5 => mix5,
            6 => mix6,
            7 => mix7,
            _ => throw new IndexOutOfRangeException($"ïndex {index} was out of range")
        };

        mix.Mix = value;
    }

    // Start is called before the first frame update
    void Start() {
        _mixList = new List<float>();
        _mixList.Add(tex0mix);
        _mixList.Add(tex1mix);
        _mixList.Add(tex2mix);
        _mixList.Add(tex3mix);
        _mixList.Add(tex4mix);
        _mixList.Add(tex5mix);
        _mixList.Add(tex6mix);
        _mixList.Add(tex7mix);

        SetMaterialProperties();
    }

    // Update is called once per frame
    void Update() {
        SetMaterialProperties();
    }

    // -- commands --
    void SetMaterialProperties() {
        SetMaterialTexture(s_TEX0_Mix, s_TEX0_ST, mix0);
        SetMaterialTexture(s_TEX1_Mix, s_TEX1_ST, mix1);
        SetMaterialTexture(s_TEX2_Mix, s_TEX2_ST, mix2);
        SetMaterialTexture(s_TEX3_Mix, s_TEX3_ST, mix3);
        SetMaterialTexture(s_TEX4_Mix, s_TEX4_ST, mix4);
        SetMaterialTexture(s_TEX5_Mix, s_TEX5_ST, mix5);
        SetMaterialTexture(s_TEX6_Mix, s_TEX6_ST, mix6);
        SetMaterialTexture(s_TEX7_Mix, s_TEX7_ST, mix7);
    }

    void SetMaterialTexture(int smix, int sst, TrackMix m) {
        mat.SetFloat(smix, Curve.Evaluate(m.Mix));
        var st = new Vector4(
            1f / m.Rect.width, 1f / m.Rect.height,
            -m.Rect.x / m.Rect.width, -m.Rect.y / m.Rect.height
        );

        mat.SetVector(sst, st);
    }

}