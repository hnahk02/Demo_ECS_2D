using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
     [SerializeField] private float speed = 20f;
    [SerializeField] private float steer = 30f;
    // Start is called before the first frame update
    [SerializeField] private Transform target;

    private Rigidbody2D rb;

    private int missileLayerMask;

	void Awake(){
		missileLayerMask = LayerMask.NameToLayer("Missile");
	}

    void Start()
    {
        target = GameManager.Instance.Plane.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //input = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        rb.velocity = transform.up * speed * Time.fixedDeltaTime * 10f;
        Vector2 direction = (target.position - transform.position).normalized;
        float rotationSteer = Vector3.Cross(transform.up, direction).z;
        rb.angularVelocity = rotationSteer * steer * 10f;

    }
    
	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.layer == missileLayerMask){
            GameManager.Instance.missileList.Remove(other.gameObject.GetComponent<Missile>());
            GameManager.Instance.missileList.Remove(gameObject.GetComponent<Missile>());
			Debug.Log(gameObject.name);
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}
}
