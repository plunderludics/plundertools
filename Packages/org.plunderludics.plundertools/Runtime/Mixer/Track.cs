using System;
using System.Collections;
using System.IO;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityHawk;

namespace Plunderludics.Lib
{

// TODO: have texture reference
[RequireComponent(typeof(Emulator))]
public class Track : MonoBehaviour
{
    [Header("config")]
    [FormerlySerializedAs("m_Sample")]
    [SerializeField] string m_SamplePath;
    [SerializeField] Savestate m_Sample;
    [SerializeField] Rom m_Rom;
    [SerializeField] float m_Delay;

    [Range(0, 100)]
    [OnValueChanged(nameof(SetVolume))]
    [SerializeField] float m_Volume;

    [ShowNativeProperty]
    public bool IsPaused {
        get => m_IsPaused;
        set {
            if (m_IsPaused == value) return;
            if (value) {
                m_Emulator.Pause();
            } else {
                m_Emulator.Unpause();
            }
            m_IsPaused = value;
        }
    }

    [Header("debug")]
    [SerializeField, ReadOnly] bool m_IsLoaded;
    [SerializeField, ReadOnly] bool m_SavedSample;

    Emulator m_Emulator;
    Coroutine m_CurrentDelay;
    Action m_CurrentRunningCallback;


    bool m_IsPaused;

    // -- lifecycle --
    void Awake() {
        m_Emulator = GetComponent<Emulator>();
    }

    void Update() {
        if (m_Emulator.IsRunning && m_Sample && !m_IsLoaded) {
            LoadSample(m_Sample, m_Rom);
        }
    }

    public void TogglePause() {
        m_IsPaused = !m_IsPaused;
        OnPauseChanged();
    }

    public void OnPauseChanged() {

    }

    public void SetVolume(float volume) {
        m_Volume = volume;
        m_Emulator.SetVolume(Mathf.FloorToInt(m_Volume * 100));
    }

    // -- commands --
    public void LoadSample(Savestate sample, Rom rom) {

        // TODO: load rom reference in sample and
        if (m_Rom != rom) {
            ClearCurrent();

            m_Rom = rom;
            m_Emulator.LoadRom(rom);
            if (m_CurrentRunningCallback != null) {
                m_Emulator.OnRunning -= m_CurrentRunningCallback;
                m_CurrentRunningCallback = () => {
                    Debug.Log($"callback for {sample} on track {this} running");
                    DelayLoadOnce();
                };
            }
            m_Emulator.OnRunning += m_CurrentRunningCallback;
        } else if (m_Sample != sample) {
            ClearCurrent();

            DelayLoadOnce();
        }

        return;

        void ClearCurrent() {
            if (m_CurrentDelay != null) {
                Debug.Log($"cancelling a current delay for {m_Rom}=>{m_Sample} for: {rom}=>{sample}");
                StopCoroutine(m_CurrentDelay);
            }

        }

        void DelayLoadOnce() {
            m_Emulator.OnRunning -= DelayLoadOnce;
            m_SamplePath = sample.Location;
            m_Sample = sample;
            m_IsLoaded = true;
            m_CurrentRunningCallback = null;

            // delay the load
            m_CurrentDelay = StartCoroutine(Delay(
                m_Delay, () => {
                    m_Emulator.LoadState(sample);
                    m_CurrentDelay = null;
                    Debug.Log($"loaded state: {sample} on ready", this);
                }
            ));
        }
    }

    IEnumerator Delay(float t, Action a) {
        yield return new WaitForSeconds(t);
        a();
    }

    public void LoadState(string statePath) {
        // TODO: load rom reference in sample and
        m_Emulator.LoadState(statePath);
    }

    public void LoadRom(string romPath) {
        // TODO: load rom reference in sample and
        m_Emulator.LoadRom(romPath);
    }

    public void ReloadState() {
        m_Emulator.ReloadState();
    }

    public string SaveSample(string sampleName) {
        Debug.Log($"[track] {name} saveSample sample : {sampleName}");
        return m_Emulator.SaveState(sampleName);
    }

    // -- queries --
    public Savestate Sample {
        get => m_Sample;
        set => m_Sample = value;
    }

    public bool IsLoaded {
        get => m_IsLoaded;
    }

    public bool IsStarted {
        get => m_Emulator.IsStarted;
    }

    public bool IsRunning {
        get => m_Emulator.IsRunning;
    }

    public void Initialize() {
        m_Emulator.Initialize();
    }
}
}