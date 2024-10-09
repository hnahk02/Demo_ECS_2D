using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float steer = 30f;
    [SerializeField] private int missileLayerMask;
    // Start is called before the first frame update
    float input;
    private Rigidbody2D rb;
     private void Awake() {
        missileLayerMask = LayerMask.NameToLayer("Missile");
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        input = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        rb.velocity = transform.up * speed * Time.fixedDeltaTime * 10f;
        rb.angularVelocity = -input * steer * 10f;

    }

    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log(other.gameObject.name);
        if(other.gameObject.layer == missileLayerMask){
        Debug.Log(other.gameObject.name);
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
