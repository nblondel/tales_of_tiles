using System;
using System.Runtime.Serialization;
using UnityEngine;

public class Chest : MonoBehaviour {
    public bool keyInteractEnabled;
    public BoxCollider2D physicalCollider;

    [OptionalField] public GameObject keyInteract;
    private KeyInteractItem _keyInteractScript;

    private bool _isInside = false;
    private bool _isOpened = false;
    private Animator _animator;

    public AudioSource audioSource;
    public AudioClip audioClip;

    private void OnEnable() {
        InputsEventManager.OnInteractPressed += Open;
    }

    private void OnDisable() {
        InputsEventManager.OnInteractPressed -= Open;
    }

    private void Awake() {
        _animator = this.GetComponent<Animator>();
        if (keyInteractEnabled && keyInteract.scene.IsValid()) {
            _keyInteractScript = keyInteract.GetComponent<KeyInteractItem>();
        }
    }

    private void Open() {
        if (_isInside && !_isOpened) {
            _isOpened = true;
            audioSource.PlayOneShot(audioClip);
            _animator.SetTrigger("OpenChest");
            physicalCollider.isTrigger = true;
            if (keyInteractEnabled) {
                _keyInteractScript.TriggerOffForever();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (keyInteractEnabled) {
                _keyInteractScript.TriggerOn();
            }
            _isInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (keyInteractEnabled) {
                _keyInteractScript.TriggerOff();
            }
            _isInside = false;
        }
    }
}