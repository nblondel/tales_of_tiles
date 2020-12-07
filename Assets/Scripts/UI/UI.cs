using UnityEngine;

public class UI : MonoBehaviour {
    public GameObject menu;
    private Menu _menuScript;

    private void OnEnable() {
        InputsEventManager.OnEscapePressed += _menuScript.TriggerMenu;
    }

    private void OnDisable() {
        InputsEventManager.OnEscapePressed -= _menuScript.TriggerMenu;
    }

    // Start is called before the first frame update
    private void Awake() {
        _menuScript = menu.GetComponent<Menu>();
    }
}