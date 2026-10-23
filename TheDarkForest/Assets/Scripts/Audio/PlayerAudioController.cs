using Player;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour
{
    [SerializeField] private AudioManager _audio;
    [SerializeField] private PlayerMovementComponent _movement;

    private AudioHandle _shotHandle;

    private void Awake()
    {
        if (_movement == null) _movement = GetComponent<PlayerMovementComponent>();
    }

    private void OnEnable()  => _movement.OnStateChanged += HandleStateChanged;
    private void OnDisable() => _movement.OnStateChanged -= HandleStateChanged;

    private void HandleStateChanged(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Idle:
                _audio.StopSFX("PlayerFootstep");
                _audio.StopSFX("BowShot");
                break;
            case PlayerState.Run:
                _shotHandle = _audio.PlaySFX("PlayerFootstep", transform.position);
                break;
            case PlayerState.Attack:
                _shotHandle = _audio.PlaySFX("BowShot", transform.position);
                
                break;

            case PlayerState.Dead:
                _shotHandle.Stop();
                _audio.StopSFX("PlayerFootstep");
                _audio.PlaySFX("PlayerDeath", transform.position);
                break;
        }
    }

    public void OnFootstep() => _audio.PlaySFX("PlayerFootstep", transform.position);

    private void OnDestroy()
    {
        _shotHandle.Stop();
        _audio?.StopSFX("PlayerFootstep");
    }
}
