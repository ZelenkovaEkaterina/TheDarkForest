using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public struct AudioHandle
{
    private readonly AudioManager _manager;
    private readonly AudioSource _source;

    internal AudioHandle(AudioManager manager, AudioSource source)
    {
        _manager = manager;
        _source = source;
    }

    public bool IsValid => _manager != null && _source != null && _source.isPlaying;

    public void Stop() => _manager?.Stop(_source);
}

public class AudioManager : MonoBehaviour
{
    [Header("Mixer")]
    [SerializeField] private AudioMixerGroup _musicGroup;
    [SerializeField] private AudioMixerGroup _sfxGroup;
    [SerializeField] private AudioMixerGroup _uiGroup;

    [Header("Banks")]
    [SerializeField] private SoundBank[] _soundBanks;

    [Header("Pool")]
    [SerializeField] private int _poolSize = 24;

    private class ActiveSound
    {
        public AudioSource Source;
        public string BankId;
    }

    private Dictionary<string, SoundBank> _bankLookup;
    private Queue<AudioSource> _availableSources;
    private List<ActiveSound> _activeSounds;
    private AudioSource _musicSource;
    private AudioSource _uiSource;

    private void Awake()
    {
        _bankLookup = new Dictionary<string, SoundBank>();
        foreach (var bank in _soundBanks)
            if (!string.IsNullOrEmpty(bank.Id))
                _bankLookup[bank.Id] = bank;

        _musicSource = CreateSource("Music", _musicGroup, spatial: 0f, loop: true);
        _uiSource = CreateSource("UI", _uiGroup, spatial: 0f, loop: false);

        _availableSources = new Queue<AudioSource>();
        _activeSounds = new List<ActiveSound>();
        for (int i = 0; i < _poolSize; i++)
            _availableSources.Enqueue(CreateSource($"SFX_{i}", _sfxGroup, spatial: 1f, loop: false));
    }

    private AudioSource CreateSource(string name, AudioMixerGroup group, float spatial, bool loop)
    {
        var go = new GameObject(name);
        go.transform.SetParent(transform);
        var src = go.AddComponent<AudioSource>();
        src.outputAudioMixerGroup = group;
        src.playOnAwake = false;
        src.loop = loop;
        src.spatialBlend = spatial;
        src.rolloffMode = AudioRolloffMode.Logarithmic;
        src.minDistance = 2f;
        src.maxDistance = 25f;
        return src;
    }

    // ---------- Play ----------

    public AudioHandle PlaySFX(string bankId, Vector3 position, float volumeScale = 1f)
    {
        if (!_bankLookup.TryGetValue(bankId, out var bank)) return default;
        var clip = bank.GetRandomClip();
        if (clip == null) return default;

        var source = RentSource();
        source.transform.position = position;
        source.clip = clip;
        source.volume = bank.Volume * volumeScale;
        source.pitch = 1f + Random.Range(-bank.PitchVariation, bank.PitchVariation);
        source.Play();

        var active = new ActiveSound { Source = source, BankId = bankId };
        _activeSounds.Add(active);

        StartCoroutine(ReturnAfter(active, clip.length / source.pitch));
        return new AudioHandle(this, source);
    }

    private AudioSource RentSource()
    {
        if (_availableSources.Count > 0)
            return _availableSources.Dequeue();

        // Пул исчерпан — забираем самый старый активный звук
        var oldest = _activeSounds[0];
        _activeSounds.RemoveAt(0);
        oldest.Source.Stop();
        oldest.Source.clip = null;
        return oldest.Source;
    }

    // ---------- Stop ----------

    public void Stop(AudioSource source)
    {
        if (source == null) return;
        int index = _activeSounds.FindIndex(s => s.Source == source);
        if (index < 0) return; // уже не играет

        _activeSounds.RemoveAt(index);
        source.Stop();
        source.clip = null;
        _availableSources.Enqueue(source);
    }

    public void StopSFX(string bankId)
    {
        for (int i = _activeSounds.Count - 1; i >= 0; i--)
        {
            if (_activeSounds[i].BankId != bankId) continue;
            var source = _activeSounds[i].Source;
            _activeSounds.RemoveAt(i);
            source.Stop();
            source.clip = null;
            _availableSources.Enqueue(source);
        }
    }

    public void StopAllSFX()
    {
        for (int i = _activeSounds.Count - 1; i >= 0; i--)
        {
            var source = _activeSounds[i].Source;
            source.Stop();
            source.clip = null;
            _availableSources.Enqueue(source);
        }
        _activeSounds.Clear();
    }

    private IEnumerator ReturnAfter(ActiveSound active, float delay)
    {
        // Realtime — чтобы таймер не замирал на паузе (Time.timeScale = 0)
        yield return new WaitForSecondsRealtime(delay);

        // Звук мог быть остановлен вручную через handle.Stop() — тогда уже вернули в пул
        if (!_activeSounds.Remove(active)) yield break;

        active.Source.Stop();
        active.Source.clip = null;
        _availableSources.Enqueue(active.Source);
    }
}
