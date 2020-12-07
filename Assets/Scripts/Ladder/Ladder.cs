using System.Runtime.Serialization;
using UnityEngine;

public class Ladder : MonoBehaviour {
    private bool _isInsideTop = false;
    private bool _isInside = false;
    private bool _isInsideBottom = false;

    public BoxCollider2D topGround;
    [OptionalField] public GameObject topSpawnPoint;
    [OptionalField] public GameObject bottomSpawnPoint;

    private PlayerScript _playerScript;

    private void OnEnable() {
        InputsEventManager.OnInteractPressed += Interact;
    }

    private void OnDisable() {
        InputsEventManager.OnInteractPressed -= Interact;
    }

    private void Awake() {
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
    }

    private void Interact() {
        if (_playerScript.isClimbing == true) // Exit ladder?
        {
            if (_isInside) // Exit ladder in the middle
            {
                _playerScript.isClimbing = false;
            } else if (_isInsideBottom && Input.GetKey(KeyCode.S)) // Exit ladder on bottom
            {
                _playerScript.isClimbing = false;
            }
        } else // Enter ladder?
        {
            if (_isInsideTop) // Enter ladder on top
            {
                _playerScript.isClimbing = true;
                _playerScript.Stop();
                if (topSpawnPoint.scene.IsValid()) {
                    _playerScript.transform.position = topSpawnPoint.transform.position;
                }
            } else if (_isInsideBottom) // Enter ladder on bottom
            {
                _playerScript.isClimbing = true;
                _playerScript.Stop();
                if (bottomSpawnPoint.scene.IsValid()) {
                    _playerScript.transform.position = bottomSpawnPoint.transform.position;
                }
            } else if (_isInside) // Enter ladder in the middle 
            {
                _playerScript.isClimbing = true;
                _playerScript.Stop();
            }
        }

        topGround.isTrigger = _playerScript.isClimbing;
    }

    public void EnterTop() {
        _isInsideTop = true;
    }

    public void ExitTop() {
        _isInsideTop = false;
    }

    public void EnterBottom() {
        _isInsideBottom = true;
    }

    public void ExitBottom() {
        _isInsideBottom = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            _isInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            _isInside = false;
            _playerScript.isClimbing = false;
            topGround.isTrigger = false;
        }
    }
}