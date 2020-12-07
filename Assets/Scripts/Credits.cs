using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {
    private void OnEnable() {
        InputsEventManager.OnEscapePressed += ReloadGame;
    }

    private void OnDisable() {
        InputsEventManager.OnEscapePressed -= ReloadGame;
    }

    public void ReloadGame() {
        SceneManager.LoadScene(0);
    }
}