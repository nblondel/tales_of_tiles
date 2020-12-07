using UnityEngine;

public class PlayerSounds : MonoBehaviour {
    /* Sounds */
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip jumpLandingAudioClip;
    [SerializeField] public AudioClip[] walkingAudioClips;
    [SerializeField] public AudioClip stoneToPlayerAudioClip;

    private int _walkingAudioClipIndex = 0;

    private bool _canMove;
    private bool _grounded;
    private bool _landing;
    private float _speed;
    private bool _climbing;

    public void ProcessMovements(bool canMove, bool grounded, bool landing, float speed, bool climbing) {
        _canMove = canMove;
        _grounded = grounded;
        _landing = landing;
        _speed = speed;
        _climbing = climbing;
        UpdateSounds();
    }

    public void StartMoveStoneSound() {
        audioSource.clip = stoneToPlayerAudioClip;
        audioSource.Play();
    }

    private void UpdateSounds() {
        if (_canMove) // Movement sounds
        {
            if (_landing) // Jump initiated
            {
                if (_grounded) // Jump ended
                {
                    audioSource.PlayOneShot(jumpLandingAudioClip);
                }
            } else {
                if (_grounded) // On the floor
                {
                    if (_speed >= 0.3f) {
                        if (!audioSource.isPlaying) {
                            audioSource.clip = walkingAudioClips[_walkingAudioClipIndex++];
                            audioSource.volume = 0.6f;
                            audioSource.PlayDelayed(0.08f);
                            _walkingAudioClipIndex = _walkingAudioClipIndex % walkingAudioClips.Length;
                        }
                    } else {
                        if (audioSource.isPlaying) {
                            audioSource.Stop();
                            _walkingAudioClipIndex = 0;
                        }
                    }
                }
            }
        }
    }
}