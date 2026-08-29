using System.Collections.Generic;
using System.Linq;

namespace Plunderludics.Lib {

public class TrackMixer: MixTextures {
    Track[] m_Tracks;
    Dictionary<Track, int> m_TrackMap;

    void Awake() {
        m_Tracks = GetComponentsInChildren<Track>(includeInactive: false);
        m_TrackMap = m_Tracks
            .Select((t, i) => new { t, i })
            .ToDictionary(t => t.t, t => t.i);
    }

    public Track[] Tracks {
        get => m_Tracks;
    }

    public bool AllStarted {
        get => Tracks.All(t => t.gameObject.activeSelf && t.IsStarted);
    }

    public bool AllRunning {
        get => Tracks.All(t => t.gameObject.activeSelf && t.IsRunning);
    }

    public void SetTrackMix(Track track, float value) {
        SetTrackMix(m_TrackMap[track], value);
    }

    public void InitializeAll() {
        foreach (var track in Tracks) {
            if (!track.gameObject.activeSelf) {
                continue;
            }

            track.Initialize();
        }
    }
}

}