using UnityEngine;

public class ColliderDialogTrigger : MonoBehaviour {
    private DialogTrigger _dialogTrigger;
    
    private void Awake() {
        _dialogTrigger = GetComponent<DialogTrigger>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (_dialogTrigger) {
                _dialogTrigger.TriggerDialog();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            if (_dialogTrigger) {
                _dialogTrigger.EndDialog();
            }
        }
    }
}
