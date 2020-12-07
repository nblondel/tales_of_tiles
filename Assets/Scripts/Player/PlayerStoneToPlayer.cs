using UnityEngine;

public class PlayerStoneToPlayer : StateMachineBehaviour {
    private PlayerScript _playerScript;
    private PlayerHealth _playerHealth;
    private CameraFollow _cameraFollow;

    private void Awake() {
        _playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>();
        _playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        _cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // Stop the camera follow during animation
        _cameraFollow.StopFollow();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // Start the camera follow after animation
        _cameraFollow.StartFollow();
        // Able the player to move
        _playerScript.SetCanMove(true);
        // Start heartbeat
        _playerHealth.StartHeartBeat();
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}