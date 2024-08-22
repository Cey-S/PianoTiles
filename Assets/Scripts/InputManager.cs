using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static InputManager Instance { get; private set; }

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}

	private void Update()
	{
		HandleInput(KeyCode.A, 0);		
		HandleInput(KeyCode.S, 1);		
		HandleInput(KeyCode.D, 2);		
		HandleInput(KeyCode.F, 3);		
	}

	private void HandleInput(KeyCode code, int key)
	{
		if (Input.GetKey(code))
		{
			if (Input.GetKeyDown(code))
			{
				GameManager.Instance.CreateCube(key);
			}

			GameManager.Instance.ScaleCube(key);
		}
		else if (Input.GetKeyUp(code))
		{
			GameManager.Instance.StopScalingCube(key);
		}
	}
}
