using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSpecificScene : MonoBehaviour {
    public int nextSceneIndex;
    private Animator _fadeAnimator;

    private void Awake() {
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
        SceneManager.LoadScene(nextSceneIndex);
    }
}