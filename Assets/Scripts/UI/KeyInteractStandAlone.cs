using UnityEngine;

public class KeyInteractStandAlone : MonoBehaviour {
    public Renderer keyInteractRenderer;

    private void Start() {
        keyInteractRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            keyInteractRenderer.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            keyInteractRenderer.enabled = false;
        }
    }
}