using UnityEngine;

public class CameraController : MonoBehaviour
{
	public new Camera camera;

	public float speed = 10.0f;
	public float maxHeight = 5.4f;
	public float minHeight = 5.4f;

	private float h, v;
	private float height;
	private float tempHeight;

	// Start is called before the first frame update
	void Start()
    {
		height = (maxHeight + minHeight) / 2;
		tempHeight = height;
		camera.transform.position = new Vector3(camera.transform.position.x, height, camera.transform.position.z);
    }

	// Update is called once per frame
	void Update()
	{

		if (Input.GetKey("a")) h = -1; else if (Input.GetKey("d")) h = 1; else h = 0;
		if (Input.GetKey("s")) v = -1; else if (Input.GetKey("w")) v = 1; else v = 0;

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

		if (System.Math.Abs((camera.transform.position.x + h * speed * Time.deltaTime)) <= 8 && System.Math.Abs((camera.transform.position.y + v * speed * Time.deltaTime)) <= 8)
		{
			Vector3 direction = new Vector3(h, v, 0);
			camera.transform.Translate(direction * (speed * Time.deltaTime));
			camera.orthographicSize = height;
		}
	}
}
