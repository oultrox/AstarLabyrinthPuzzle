using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D _rBody;
    private float _gravity;
    private Transform _transform;
    private float angle;

    // Start is called before the first frame update
    void Start()
    {
        _rBody = GetComponent<Rigidbody2D>();
        _transform = this.transform;
        _gravity = _rBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = _rBody.velocity;
        angle = Mathf.Atan2(velocity.y, 10) * Mathf.Rad2Deg;
        _transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        
        //_transform.Rotate(new Vector3(0, 0, (angle) * Time.deltaTime));
        if (Input.GetKey(KeyCode.Space))
        {
            _rBody.AddForce(Vector2.up * _gravity * Time.deltaTime * moveSpeed);
        }
        
    }
}
