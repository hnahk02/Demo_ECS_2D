using UnityEngine;

public class HomingMissile : MonoBehaviour
{

	private GameObject TargetGO;
	private float turnSpeed = 3f;
	private float speed = 3f;

	[SerializeField] private Transform target;

	private int missileLayerMask;

	void Awake(){
		missileLayerMask = LayerMask.NameToLayer("Missile");
	}

	private void Start()
	{
		
	}

	

	private void Update()
	{
		// Move and rotate missile
		transform.position += transform.right * speed * Time.deltaTime;
		Vector3 direction = (target.position - transform.position).normalized;
		Vector3 dir = target.position - transform.position; float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * turnSpeed);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.layer == missileLayerMask){
			Debug.Log(gameObject.name);
			Destroy(other.gameObject);
			Destroy(gameObject);
		}
	}

}
