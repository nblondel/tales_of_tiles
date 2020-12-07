using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderTop : MonoBehaviour {
    public GameObject ladder;
    private Ladder _ladderScript;

    private void Start() {
        _ladderScript = ladder.GetComponent<Ladder>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            _ladderScript.EnterTop();
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            _ladderScript.ExitTop();
        }
    }
}