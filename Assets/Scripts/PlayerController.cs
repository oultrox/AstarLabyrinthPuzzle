using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    private Rigidbody2D rBody;
    private float horizontalMovement;
    private float verticalMovement;
    private Vector3 movementDir;
    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        movementDir.x = horizontalMovement;
        movementDir.y = verticalMovement;
        movementDir = movementDir.normalized * movementSpeed * Time.deltaTime;
        rBody.MovePosition(transform.position + movementDir);
        
    }
}
