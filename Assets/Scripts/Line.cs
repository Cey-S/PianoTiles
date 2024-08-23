using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
	private void OnTriggerExit(Collider other)
	{
		//Destroy(other.transform.parent.gameObject);
		GameManager.Instance.DestroyCube(other.transform.parent.gameObject);
	}
}
