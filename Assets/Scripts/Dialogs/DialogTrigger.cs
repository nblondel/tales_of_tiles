using System;
using UnityEngine;

public class DialogTrigger : MonoBehaviour {
    private DialogManager _dialogManager;
    
    [Header("Dialog Text")] 
    [SerializeField] private DialogXMLObject dialogObject;
    [SerializeField] private string dialogId;
    private DialogXMLObject.Dialog _dialog;
    
    [Header("Dialog aspect")]
    [SerializeField] private Sprite dialogSprite;
    [SerializeField] private DialogColorAndFont dialogColorAndFont;
    
    [Header("Interaction")]
    [SerializeField] private bool triggerOnlyOnce;
    [SerializeField] private bool playerCanMove;
    [SerializeField] private bool canInteract = true;

    private bool _triggered;

    private void Awake() {
        _dialogManager = FindObjectOfType<DialogManager>();
        _dialog = dialogObject.Load(); // Load XML file
    }

    public void TriggerDialog() {
        if (!_triggered) {
            _dialogManager.StartDialog(_dialog, dialogId, dialogSprite, dialogColorAndFont, playerCanMove, canInteract);

            if (triggerOnlyOnce) {
                _triggered = true;
            }
        }
    }

    public void EndDialog() {
        _dialogManager.EndDialog();
    }
}