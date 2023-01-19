using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] float _speed = 6.0f;
    [SerializeField] float _gravity = -20f;
    private CharacterController _charController;
    
    void Start()
    {
        _charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        MoveCharacter();
    }

    private void MoveCharacter()
    {
        float deltaX = Input.GetAxis("Horizontal") * _speed;
        float deltaZ = Input.GetAxis("Vertical") * _speed;

        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, _speed);

        movement.y = _gravity;
        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);

        _charController.Move(movement);
    }
}
