using MazeFinder.Scripts.Events;
using SimpleBus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Gamaga.Scripts.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] float _speed = 6;
        [SerializeField] float _gravity = -20;
        [SerializeField] private float _smoothInputSpeed = 0.2f;
        private CharacterController _characterController;
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private Vector3 _moveDirection;
        private Vector3 _movement;
        private bool _isTiltActivated = false;
        private float _tiltSpeed = 7;
        private Vector2 _currentInput;
        private Vector2 _smoothInputVelocity;
        

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _playerInput = GetComponent<PlayerInput>();
            _moveAction = _playerInput.actions["Movement"];
            #if UNITY_ANDROID || UNITY_IOS
            ToggleTiltMovement();
            #endif
        }
        
        private void Update()
        {
            MoveCharacter();

            AttemptApplyTiltMovement();
        }
        
        public void ToggleTiltMovement()
        {
            if (SystemInfo.supportsGyroscope)
            {
                _isTiltActivated = !_isTiltActivated;
                Input.gyro.enabled = _isTiltActivated;
            }
        }
        
        private void AttemptApplyTiltMovement()
        {
            //Just for fun.
            if (_isTiltActivated && _moveAction.ReadValue<Vector2>() == Vector2.zero)
            {
                TiltMovement();
            }
        }


        private void TiltMovement()
        {
            _moveDirection = new Vector3(Input.acceleration.x, 0, Input.acceleration.y);
            _moveDirection.y = _gravity;
            _characterController.Move(_tiltSpeed * Time.deltaTime * _moveDirection);
        }


        private void MoveCharacter()
        {
            _moveDirection = _moveAction.ReadValue<Vector2>();
            _currentInput = Vector2.SmoothDamp(_currentInput, _moveDirection, ref _smoothInputVelocity, _smoothInputSpeed);

            _movement = new Vector3(_currentInput.x, 0, _currentInput.y);
            _movement.y = _gravity;
            _characterController.Move(_speed * Time.deltaTime * _movement);
        }
    }

}
