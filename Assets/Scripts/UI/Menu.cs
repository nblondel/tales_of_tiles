using System;
using UnityEngine;

public class Menu : MonoBehaviour {
    private PlayerScript _playerScript;
    private GameObject _menu;
    private bool _menuDisplayed;
    public AudioManager audioManager;

    // Start is called before the first frame update
    private void Awake() {
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        _menu = GameObject.FindGameObjectWithTag("MenuUI");
        _menuDisplayed = false;
        _menu.SetActive(_menuDisplayed);
    }

    public void ContinueButton() {
        TriggerMenu();
    }

    public void ExitGameButton() {
        Application.Quit();
    }

    public void TriggerMenu() {
        _menuDisplayed = !_menuDisplayed;
        _menu.SetActive(_menuDisplayed);
        _playerScript.enabled = (_menuDisplayed ? false : true);
        Time.timeScale = (_menuDisplayed ? 0 : 1);
    }

    public void SetSoundVolume(float volume) {
        audioManager.SetSoundVolume(volume);
    }

    public void SetMusicVolume(float volume) {
        audioManager.SetMusicVolume(volume);
    }
}