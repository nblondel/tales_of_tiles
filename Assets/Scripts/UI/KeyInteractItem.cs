using UnityEngine;

public class KeyInteractItem : MonoBehaviour {
    public Renderer keyInteractRenderer;
    [HideInInspector] public bool enableTriggers; // Can be triggered on and off

    private void Start() {
        keyInteractRenderer.enabled = false;
        enableTriggers = true;
    }

    public void TriggerOn() {
        if (enableTriggers) {
            keyInteractRenderer.enabled = true;
        }
    }

    public void TriggerOff() {
        if (enableTriggers) {
            keyInteractRenderer.enabled = false;
        }
    }

    public void TriggerOffForever() {
        keyInteractRenderer.enabled = false;
        enableTriggers = false; // Disable the trigger on and off forever
    }
}