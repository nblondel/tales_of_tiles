using UnityEngine;

public class Steps : MonoBehaviour {
    public Animator animator;

    private void OnEnable() {
        animator.SetTrigger("StartSteps");
    }
}
