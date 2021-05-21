using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Camera camera;

	public float speed = 10.0f;
	public float maxHeight = 10.8f;
	public float minHeight = 5.4f;

	private float h, v;
	private float height;
	private float tempHeight;
	private bool L, R, U, D;

	public void CursorTriggerEnter(string triggerName)
	{
		switch (triggerName)
		{
			case "L":
				L = true;
				break;
			case "R":
				R = true;
				break;
			case "U":
				U = true;
				break;
			case "D":
				D = true;
				break;
		}
	}

	public void CursorTriggerExit(string triggerName)
	{
		switch (triggerName)
		{
			case "L":
				L = false;
				break;
			case "R":
				R = false;
				break;
			case "U":
				U = false;
				break;
			case "D":
				D = false;
				break;
		}
	}

	// Start is called before the first frame update
	void Start()
    {
		height = (maxHeight + minHeight) / 2;
		tempHeight = height;
		transform.position = new Vector3(transform.position.x, height, transform.position.z);
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKey("a") || L) h = -1; else if (Input.GetKey("d") || R) h = 1; else h = 0;
		if (Input.GetKey("s") || D) v = -1; else if (Input.GetKey("w") || U) v = 1; else v = 0;

		if (Input.GetAxis("Mouse ScrollWheel") < 0)
		{
			if (height < maxHeight) tempHeight += 1;
		}
		if (Input.GetAxis("Mouse ScrollWheel") > 0)
		{
			if (height > minHeight) tempHeight -= 1;
		}

		tempHeight = Mathf.Clamp(tempHeight, minHeight, maxHeight);
		height = Mathf.Lerp(height, tempHeight, 3 * Time.deltaTime);

		Vector3 direction = new Vector3(h, v, 0);
		transform.Translate(direction * speed * Time.deltaTime);
		camera.orthographicSize = height;
	}
}
