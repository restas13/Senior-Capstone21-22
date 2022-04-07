using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawn : MonoBehaviour
{
	public GameObject mapObject;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(mapObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Map")
		{
			Instantiate(mapObject);
		}
	}
}
