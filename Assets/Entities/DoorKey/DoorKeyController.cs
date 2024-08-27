
using Cinemachine;
using UnityEngine;

public class DoorKeyController : MonoBehaviour
{
    public Transform key;
    public Transform keyTargetPosition;
    public float keySpeed = 2f;
    public AudioSource keyAudio;
    public Transform door;
    public Transform doorTargetPosition;
    public float doorSpeed = 3f;
    public AudioSource doorAudio;

    private bool _isGoingDown;

    enum State
    {
        Open,
        PushingKey,
        ClosingDoor,
        Close
    }

    private CinemachineImpulseSource _cinemachineImpulse;
    private State _currentState = State.Open;

    private void Awake()
    {
        TryGetComponent(out _cinemachineImpulse);
    }

    private void Update()
    {
        switch (_currentState)
        {
            case State.PushingKey:
                if ((_isGoingDown && key.position.y < keyTargetPosition.position.y) || (!_isGoingDown && key.position.y > keyTargetPosition.position.y))
                {
                    _currentState = State.ClosingDoor;
                    _isGoingDown = door.position.y > doorTargetPosition.position.y;
                    Shake(0.2f);
                    doorAudio.Play();
                    break;
                }
                key.position += (_isGoingDown ? Vector3.down : Vector3.up) * (Time.deltaTime * keySpeed);
                break;
            case State.ClosingDoor:
                if ((_isGoingDown && door.position.y < doorTargetPosition.position.y) || (!_isGoingDown && door.position.y > doorTargetPosition.position.y))
                {
                    _currentState = State.Close;
                    doorAudio.Stop();
                    keyAudio.Play();
                    Shake(0.5f);
                    break;
                }
                door.position += (_isGoingDown ? Vector3.down : Vector3.up) * (Time.deltaTime * doorSpeed);
                break;
        }
    }

    public void OpenDoor()
    {
        if (!_currentState.Equals(State.Open)) return;
        _currentState = State.PushingKey;
        _isGoingDown = key.position.y > keyTargetPosition.position.y;
        keyAudio.Play();
    }

    private void Shake(float power)
    {
        _cinemachineImpulse.GenerateImpulseWithForce(power);
    }
}
