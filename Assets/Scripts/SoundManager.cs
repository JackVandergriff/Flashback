using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour {
    static SoundManager soundManager;
    public static int intensity = 0;

    public List<AudioSource> music;
    public List<float> trackVolumeTargets = new List<float>();
    public AudioClip[] sfx;
    public AudioSource player;
    int local_intensity;

    void Start() {
        soundManager = this;
        DontDestroyOnLoad(gameObject);
        GetComponentsInChildren(true, music);
        foreach (var _ in music) {
            trackVolumeTargets.Add(0);
        }
        trackVolumeTargets[0] = 1;
    }

    void Awake() {
        intensity = 0;
    }

    void Update() {
        for (int i = 0; i < music.Count; i++) {
            if (music[i].volume != trackVolumeTargets[i])
                music[i].volume += (trackVolumeTargets[i] - music[i].volume) / 500f;
        }

        if (intensity != local_intensity) {
            local_intensity = intensity;
            for (int i = 0; i < music.Count; i++) {
                if (i <= intensity) {
                    SetTarget(i, 1);
                } else {
                    SetTarget(i, 0);
                }
            }
        }
    }

    public static void SetTarget(int track, float volume) {
        soundManager.trackVolumeTargets[track] = volume;
    } 

    public static void Play(string name) {
        foreach (var fx in soundManager.sfx) {
            if (fx.name == name) {
                Camera.main.GetComponent<AudioSource>().PlayOneShot(fx);
            }
        }
    }
}
