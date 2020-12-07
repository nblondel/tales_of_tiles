using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderBottom : MonoBehaviour {
    public GameObject ladder;
    private Ladder _ladderScript;

    private void Start() {
        _ladderScript = ladder.GetComponent<Ladder>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            _ladderScript.EnterBottom();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            _ladderScript.ExitBottom();
        }
    }
}