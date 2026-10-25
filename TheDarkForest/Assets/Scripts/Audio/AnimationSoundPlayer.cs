using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AnimationSoundPlayer : MonoBehaviour
{
    [Serializable]
    public class SoundEntry
    {
        public string id;
        public AudioClip[] clips;
        [Range(0f, 1f)]   public float volume = 1f;
        [Range(0f, 0.5f)] public float pitchVariation = 0.05f;
    }

    [SerializeField] private AudioSource _source;
    [SerializeField] private SoundEntry[] _sounds;

    private Dictionary<string, SoundEntry> _lookup;

    private void Awake()
    {
        if (_source == null) _source = GetComponent<AudioSource>();
        
        _source.playOnAwake = false;
        _source.loop = false;
        _source.clip = null;

        _lookup = new Dictionary<string, SoundEntry>();
        foreach (var entry in _sounds)
            if (!string.IsNullOrEmpty(entry.id))
                _lookup[entry.id] = entry;
    }
    
    public void PlaySound(string id)
    {
        if (_lookup == null || !_lookup.TryGetValue(id, out var entry)) return;
        if (entry.clips == null || entry.clips.Length == 0) return;

        _source.pitch = 1f + Random.Range(-entry.pitchVariation, entry.pitchVariation);
        var clip = entry.clips[Random.Range(0, entry.clips.Length)];
        _source.PlayOneShot(clip, entry.volume);
    }
    
    //public void PlaySound() => PlaySound("default");
}
