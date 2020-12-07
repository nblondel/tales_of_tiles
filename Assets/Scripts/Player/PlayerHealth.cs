using UnityEngine;

public class PlayerHealth : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip heartbeatAudioClip;

    private bool _hearthBeat = false;
    private float _timeBetweenBeats = 3.0f;
    private float _timer;

    private void Awake() {
        audioSource.clip = heartbeatAudioClip;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.H)) {
            TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            Heal(1);
        }

        if (_hearthBeat) {
            if (_timer >= _timeBetweenBeats) {
                audioSource.PlayOneShot(heartbeatAudioClip);
                _timer = 0f;
            }

            _timer += Time.deltaTime;
        }
    }

    public void StartHeartBeat() {
        _timer = _timeBetweenBeats; // Trigger first heartbeat immediatly
        _hearthBeat = true;
    }

    public void StopHeartBeat() {
        _hearthBeat = false;
    }

    public void TakeDamage(int amount) {
        // TODO heartbeat increase
    }

    public void Heal(int amount) {
        // TODO heartbeat decrease
    }
}