using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int maxMissileNum;
    [SerializeField] private Missile missilePrefab;

    [SerializeField] private Plane plane;

    public List<Missile> missileList = new List<Missile>();

    private void Awake() {
        Instance = this;
    }

    private void Start()
    {

      
        for (int i = 0; i < maxMissileNum; i++)
        {
            SpawnMissile();
        }

    }

    private void FixedUpdate()
    {
        if (missileList.Count < maxMissileNum)
        {
            SpawnMissile();
        }
    }

    private void SpawnMissile()
    {
          Vector2 planePos = plane.transform.position;
        Vector2 pos = Vector2.zero;
        pos.x = Random.Range(planePos.x - 10f, planePos.x + 10f);
        pos.y = Random.Range(planePos.y - 10f, planePos.y + 10f);
        Missile instance = Instantiate(missilePrefab, pos, Quaternion.identity);
        missileList.Add(instance);
    }

    public Plane Plane => plane;
}
