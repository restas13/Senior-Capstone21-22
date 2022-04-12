using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawn : MonoBehaviour
{
	public GameObject mapObject;
	public List<GameObject> spawnedMap = new List<GameObject>();
	public LayerMask mapMask;
	public float spawnDistance = 0.5f;
	public bool dead = false;

	void LateUpdate()
	{
		if(Physics.OverlapSphere(transform.position, 1f, mapMask).Length == 0 && !dead)
		{
			spawnedMap.Add(Instantiate(mapObject, transform.position, Quaternion.identity) as GameObject);
		}
	}

	public void Die()
	{
		dead = true;
		for(int i = 0; i < spawnedMap.Count; i++)
		{
			Destroy(spawnedMap[i]);
		}
	}
}
