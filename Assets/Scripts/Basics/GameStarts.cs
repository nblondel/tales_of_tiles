using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStarts : MonoBehaviour {
    public bool startOfGame;
    private bool _gameStarted;

    private CameraFollow _cameraFollow;
    private Animator _playerAnimator;

    // Start is called before the first frame update
    private void Awake() {
        _cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        _playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    private void Start() {
        if (!startOfGame) {
            _cameraFollow.StartFollow();
            _playerAnimator.SetTrigger("gameStartsNormalTrigger");
        }
    }

    private void Update() {
        if (startOfGame) {
            if (!_gameStarted) {
                if (Input.anyKey) {
                    _gameStarted = true;
                    _cameraFollow.StartSlowFollow();
                    _playerAnimator.SetTrigger("gameStartsStoneAnimationTrigger");
                }
            }
        }
    }
}