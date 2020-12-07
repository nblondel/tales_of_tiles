using UnityEngine;

public class InputsEventManager : MonoBehaviour {
    public delegate void BasicInput();

    public delegate void MovementInput(float horizontalAxeValue, float verticalAxeValue);

    public static event BasicInput OnSpacePressed;
    public static event BasicInput OnEnterPressed;
    public static event BasicInput OnEscapePressed;
    public static event BasicInput OnSpaceReleased;
    public static event BasicInput OnInteractPressed;

    public static event MovementInput OnMovementKeyPressed;

    // Update is called once per frame
    private void Update() {
        if (Input.GetButtonDown("Jump")) {
            if (OnSpacePressed != null) {
                OnSpacePressed();
            }
        }
        if (Input.GetButtonUp("Jump")) {
            if (OnSpaceReleased != null) {
                OnSpaceReleased();
            }
        }

        if (Input.GetKeyDown(KeyCode.Return)) {
            if (OnEnterPressed != null) {
                OnEnterPressed();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (OnEscapePressed != null) {
                OnEscapePressed();
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (OnInteractPressed != null) {
                OnInteractPressed();
            }
        }

        var horizontalMovement = Input.GetAxis("Horizontal");
        var verticalMovement = Input.GetAxis("Vertical");
        if (OnMovementKeyPressed != null) {
            OnMovementKeyPressed(horizontalMovement, verticalMovement);
        }
    }

    public static bool IsJumpButtonHeld() {
        return Input.GetButton("Jump");
    }
}