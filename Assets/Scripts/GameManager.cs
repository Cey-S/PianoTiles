using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[SerializeField]
	private float cubeSpeed;

	private Color[] colors = new Color[] { Color.blue, Color.yellow, Color.red, Color.magenta };
	private GameObject[] keys;
	private int keyPosX;

	private GameObject[] scalingCubes; // Cubes that are expanding on the Y scale

	private void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}

	private void Start()
	{
		keys = new GameObject[colors.Length];
		keyPosX = -1; // First key position on the X axis
		SetScene();

		scalingCubes = new GameObject[keys.Length];
	}

	private void SetScene()
	{
		for (int i = 0; i < colors.Length; i++)
		{
			// Create cube
			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.name = $"Key_{i}";
			// Set color
			cube.GetComponent<MeshRenderer>().material.color = colors[i];
			// Set position
			cube.transform.position = new Vector3(keyPosX, 0, 0);
			// Move one right for the next cube
			keyPosX++;

			keys[i] = cube;
		}
	}

	public void CreateCube(int key)
	{
		// Create an empty GO to set the pivot point for cube's Y scale
		GameObject cubeObject = new GameObject("CubeObject");

		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.GetComponent<MeshRenderer>().material.color = colors[key];
		cube.AddComponent<Rigidbody>().isKinematic = true; // For line trigger
		cube.transform.localScale = new Vector3(0.9f, 1.0f, 0.9f);

		cube.transform.parent = cubeObject.transform;
		cube.transform.localPosition = new Vector3(0, 0.5f); // Make the bottom of the cube center of the parent

		cubeObject.transform.position = keys[key].transform.position + new Vector3(0, 0.5f); // Position the parent at the top of the key
		cubeObject.transform.localScale = new Vector3(1, 0, 1); // Set the Y scale to 0 to initially hide the cube
		scalingCubes[key] = cubeObject; // The cube can be scaled in one direction using the parent
	}

	public void ScaleCube(int key)
	{
		scalingCubes[key].transform.localScale += Vector3.up * Time.deltaTime * cubeSpeed;
	}

	public void StopScalingCube(int key)
	{
		scalingCubes[key].AddComponent<MovingCube>();
	}

	public float GetCubeSpeed()
	{
		return cubeSpeed;
	}
}
