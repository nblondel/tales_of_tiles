using System;
using System.Collections;
using UnityEngine;

public class DeathZone : MonoBehaviour {
    private Transform _respawnPlayer;
    private Animator _fadeAnimator;

    private void Awake() {
        _respawnPlayer = GameObject.FindGameObjectWithTag("Respawn").transform;
        _fadeAnimator = GameObject.FindGameObjectWithTag("FadeSystem").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            StartCoroutine(RespawnPlayer(collision));
        }
    }

    private IEnumerator RespawnPlayer(Collider2D collision) {
        _fadeAnimator.SetTrigger("FadeInTrigger");
        yield return new WaitForSeconds(1f);
        collision.transform.position = _respawnPlayer.position;
    }
}