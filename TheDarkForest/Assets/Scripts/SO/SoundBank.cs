using UnityEngine;

[CreateAssetMenu(fileName = "SoundBank", menuName = "Scriptable Objects/Sound Bank")]
public class SoundBank : ScriptableObject
{
    public string Id;
    public AudioClip[] Clips;
    [Range(0f, 1f)] public float Volume = 1f;
    [Range(0f, 0.5f)] public float PitchVariation = 0.05f;

    public AudioClip GetRandomClip()
    {
        if (Clips == null || Clips.Length == 0) return null;
        return Clips[Random.Range(0, Clips.Length)];
    }
}
