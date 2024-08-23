using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	[SerializeField]
	private float cubeSpeed;

	[Header("Cube Object Pool")]
	[SerializeField]
	private int capacity;
	[SerializeField]
	private int maxSize;

	private Color[] colors = new Color[] { Color.blue, Color.yellow, Color.red, Color.magenta };
	private GameObject[] keys;
	private int keyPosX;

	private GameObject[] scalingCubes; // Cubes that are expanding on the Y scale
	private ObjectPool<GameObject> cubePool;

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
		SetScene();
	}

	private void SetScene()
	{
		keys = new GameObject[colors.Length];
		keyPosX = -1; // First key position on the X axis

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

		scalingCubes = new GameObject[keys.Length];
		CreateCubePool(capacity, maxSize);
	}

	private void CreateCubePool(int capacity, int maxSize)
	{
		cubePool = new ObjectPool<GameObject>(
		createFunc: () =>
		{
			// Create an empty parent GO to set the pivot point for cube's Y scale
			GameObject cubeObject = new GameObject("CubeObject");
			cubeObject.AddComponent<MovingCube>().IsMoving = false;

			GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
			cube.AddComponent<Rigidbody>().isKinematic = true; // For line trigger
			cube.transform.localScale = new Vector3(0.9f, 1.0f, 0.9f); // Make the cube smaller than the key

			cube.transform.parent = cubeObject.transform;
			cube.transform.localPosition = new Vector3(0, 0.5f); // Make the bottom of the cube center of the parent

			return cubeObject; // The cube can be scaled in one direction using the parent
		},
		actionOnGet: cube =>
		{
			cube.transform.localScale = new Vector3(1, 0, 1); // Set the Y scale to 0 to initially hide the cube
			cube.gameObject.SetActive(true);
		},
		actionOnRelease: cube =>
		{
			cube.GetComponent<MovingCube>().IsMoving = false;
			cube.gameObject.SetActive(false);
		},
		actionOnDestroy: cube =>
		{
			Destroy(cube.gameObject);
		},
		false, capacity, maxSize);
	}

	public void DestroyCube(GameObject cube)
	{
		cubePool.Release(cube);
	}

	public void CreateCube(int key)
	{
		var cube = cubePool.Get();
		cube.GetComponentInChildren<MeshRenderer>().material.color = colors[key];
		cube.transform.position = keys[key].transform.position + new Vector3(0, 0.5f); // Position the cube at the top of the key
		scalingCubes[key] = cube;
	}

	public void ScaleCube(int key)
	{
		scalingCubes[key].transform.localScale += Vector3.up * Time.deltaTime * cubeSpeed;
	}

	public void StopScalingCube(int key)
	{
		scalingCubes[key].GetComponent<MovingCube>().IsMoving = true; // Start moving up
	}

	public float GetCubeSpeed()
	{
		return cubeSpeed;
	}
}
