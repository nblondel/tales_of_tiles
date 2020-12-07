using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour {
    // Dialog box objects
    public RectTransform dialogBox;
    public Image dialogImage;
    public Text dialogText;
    public SpriteRenderer dialogSpriteRenderer;
    public Animator animator;
    
    // Audio
    public AudioSource audioSource;
    public AudioClip typingAudioClip;

    // Dialog properties
    private Coroutine _sentenceTypingCoroutine;
    private Queue<string> _sentencesQueue;
    private float _typingSpeed = 0.015f;
    private bool _canInteract = true;
    private bool _letterByletter = true;
    
    // Dialog box open/close animation
    private float _timeOfTravel = 0.120f;
    private Vector3 _dialogBoxStartPosition;
    
    // Delegate to stop player if needed
    public delegate void DialogEvent(bool playerCanMove);
    public static event DialogEvent OnPlayerDialog;

    private void Start() {
        _sentencesQueue = new Queue<string>();
        _dialogBoxStartPosition = dialogBox.anchoredPosition;
    }

    public void StartDialog(DialogXMLObject.Dialog dialog, string lineId, Sprite dialogSprite, DialogColorAndFont dialogColorAndFont, bool playerCanMove, bool canInteract) {
        // Set the player movement allowed or disabled
        SetPlayerDialogChanged(true, playerCanMove);
        // Move the dialog box in the UI
        StartCoroutine(OpenDialog());
        _sentencesQueue.Clear();

        // Create dialog with each sentences of a line
        foreach (var lines in dialog.LinesList) {
            if (lines.id == lineId) {
                foreach (var text in lines.text)
                    _sentencesQueue.Enqueue(text);
                break;
            }
        }

        _letterByletter = dialogColorAndFont.GetLetterByLetter();
        _canInteract = canInteract;
        if (_canInteract) { // Press ENTER to go to next sentence
            if (_letterByletter) { // Display dialog letter by letter or the entire sentence directly
                InputsEventManager.OnEnterPressed += DisplayNextSentenceLetterByLetter;
            } else {
                InputsEventManager.OnEnterPressed += DisplayNextSentence;
            }
        }

        StartCoroutine(StartTextDisplay(dialogSprite, dialogColorAndFont));
    }

    private IEnumerator StartTextDisplay(Sprite dialogSprite, DialogColorAndFont dialogColorAndFont) {
        // Configure the text and image
        dialogImage.color = dialogColorAndFont.GetColor();
        dialogText.alignment = dialogColorAndFont.GetTextAnchor();
        dialogText.font = dialogColorAndFont.GetFont();
        dialogText.fontStyle = dialogColorAndFont.GetFontStyle();
        dialogText.fontSize = dialogColorAndFont.GetFontSize();
        dialogSpriteRenderer.sprite = dialogSprite;
        
        if (_letterByletter) {
            DisplayNextSentenceLetterByLetter();
        } else {
            DisplayNextSentence();
        }

        yield return null;
    }

    // Display an entire sentence directly
    private void DisplayNextSentence() {
        if (_sentencesQueue.Count <= 0) {
            EndDialog();
            return;
        }

        dialogText.text = _sentencesQueue.Dequeue();
    }

    // Display a sentence letter by letter
    private void DisplayNextSentenceLetterByLetter() {
        if (_sentencesQueue.Count <= 0) {
            if (_sentenceTypingCoroutine != null) StopCoroutine(_sentenceTypingCoroutine);
            EndDialog();
            return;
        }

        var sentence = _sentencesQueue.Dequeue();
        if (_sentenceTypingCoroutine != null) StopCoroutine(_sentenceTypingCoroutine);
        _sentenceTypingCoroutine = StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence) {
        audioSource.PlayOneShot(typingAudioClip);
        for (var index = 0; index < sentence.Length; index++) {
            dialogText.text = sentence.Substring(0, index + 1);
            yield return new WaitForSeconds(_typingSpeed);
        }
    }

    public void EndDialog() {
        if (_canInteract) {
            if (_letterByletter) {
                InputsEventManager.OnEnterPressed -= DisplayNextSentenceLetterByLetter;
            } else {
                InputsEventManager.OnEnterPressed -= DisplayNextSentence;
            }
            _canInteract = false;
        }

        // Reset the player movement allowed
        SetPlayerDialogChanged(false, true);
        // Move the dialog box in the UI
        StartCoroutine(CloseDialog());
    }

    private void SetPlayerDialogChanged(bool started, bool playerCanMove) {
        // started is not used here but it could be used to know if dialog started or ended for animations etc
        if (OnPlayerDialog != null) {
            OnPlayerDialog(playerCanMove);
        }
    }

    // Move the dialog box in the UI canvas (up)
    private IEnumerator OpenDialog() {
        var endPosition = new Vector2(_dialogBoxStartPosition.x, _dialogBoxStartPosition.y + 88f);
        var currentTime = 0f;
        while (currentTime <= 1.0f) {
            currentTime += Time.deltaTime / _timeOfTravel;
            dialogBox.anchoredPosition = Vector2.Lerp(_dialogBoxStartPosition, endPosition, currentTime);
            yield return new WaitForEndOfFrame();
        }
    }
        
    // Move the dialog box out of the UI canvas (down)
    private IEnumerator CloseDialog() {
        var currentPosition = dialogBox.anchoredPosition;
        var currentTime = 0f;
        while (currentTime <= 1.0f) {
            currentTime += Time.deltaTime / _timeOfTravel;
            dialogBox.anchoredPosition = Vector2.Lerp(currentPosition, _dialogBoxStartPosition, currentTime);
            yield return new WaitForEndOfFrame();
        }
    }
}