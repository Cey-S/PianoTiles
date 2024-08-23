using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
	public bool IsMoving { get; set; }

	void Update()
	{
		if (IsMoving)
			transform.Translate(Vector3.up * Time.deltaTime * GameManager.Instance.GetCubeSpeed());
	}
}
