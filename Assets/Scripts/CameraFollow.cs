using UnityEngine;

public class CameraFollow : MonoBehaviour {
    private const float SlowTravellingTime = 0.8f;
    private const float NormalTravellingTime = 0.2f;

    private float _timeTravelling = NormalTravellingTime;
    private bool _slowTravelling = true;
    private Vector3 _velocity;
    private bool _follow = false;
    
    public GameObject target;
    public Vector3 posOffset;
    public Animator playerAnimator;

    private void Update() {
        if (_follow) {
            if (_slowTravelling) {
                var distance = Vector3.Distance(transform.position, target.transform.position + posOffset);
                if (distance <= 0.2f) {
                    _timeTravelling = NormalTravellingTime; // Normal travelling
                    _slowTravelling = false;
                } else {
                    _timeTravelling = SlowTravellingTime; // Slow travelling
                }
            }

            // Normal follow camera
            transform.position = Vector3.SmoothDamp(transform.position,
                target.transform.position + posOffset,
                ref _velocity, _timeTravelling);
        }
    }

    public void StartSlowFollow() {
        _slowTravelling = true;
        _follow = true;
    }

    public void StartFollow() {
        _slowTravelling = false;
        _follow = true;
    }

    public void StopFollow() {
        _follow = false;
    }
}