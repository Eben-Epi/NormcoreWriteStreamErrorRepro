using Normal.Realtime;
using UnityEngine;

public class Player : MonoBehaviour {
    
    // Physics
    private Vector3   _targetMovement;
    private Vector3   _movement;

    private Rigidbody _rigidbody;
    private RealtimeTransform _rt;

    public static Player instance;
    
    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }

        // Set physics timestep to 60hz
        Time.fixedDeltaTime = 1.0f/60.0f;

        // Store a reference to the rigidbody for easy access
        _rigidbody = GetComponent<Rigidbody>();
        _rt = GetComponent<RealtimeTransform>();
    }

    public void RequestPlayerOwnership()
    {
        _rt.RequestOwnership();
    }

    private void Update() {
        // Use WASD input and the camera look direction to calculate the movement target
        CalculateTargetMovement();
    }

    private void FixedUpdate() {
        // Move the player based on the input
        MovePlayer();
    }

    private void CalculateTargetMovement() {
        // Get input movement. Multiple by 6.0 to increase speed.
        Vector3 inputMovement = new Vector3();
        inputMovement.z = 1 * 3.0f;
        _targetMovement = inputMovement;
    }

    private void MovePlayer() {
        // Start with the current velocity
        Vector3 velocity = _rigidbody.velocity;

        // Smoothly animate towards the target movement velocity
        _movement = Vector3.Lerp(_movement, _targetMovement, Time.fixedDeltaTime * 5.0f);
        velocity.x = _movement.x;
        velocity.z = _movement.z;
        
        // Set the velocity on the rigidbody
        _rigidbody.velocity = velocity;
    }
}
 