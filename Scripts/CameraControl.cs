using UnityEngine;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour 
{	
	private enum CameraMode
	{
		Horizontal,
		Free
	}
	
	[SerializeField]
	private CameraMode _cameraMode = CameraMode.Horizontal;
	
	[SerializeField]
	private float _movementSpeed = 10.0f;
	
	[SerializeField]
	private float _rotationSpeed = 200.0f;
	
	[SerializeField]
	private Material _primitiveMaterial;
	
	private float _x_rotation = 0.0f;
	private float _y_rotation = 0.0f;
	
	private Neoclassical mesh;
	
	
	void Start ()
	{
		mesh = new Neoclassical(new Vector3( 4f + Random.Range(0.5f, 1.5f), 0,  1.5f + Random.Range(0.5f, 1.5f)),
								new Vector3( 4f + Random.Range(0.5f, 1.5f), 0, -1.5f - Random.Range(0.5f, 1.5f)),
								new Vector3(-4f - Random.Range(0.5f, 1.5f), 0, -1.5f - Random.Range(0.5f, 1.5f)), 
								new Vector3(-4f - Random.Range(0.5f, 1.5f), 0,  1.5f + Random.Range(0.5f, 1.5f)));
		mesh.ConstructFaces();
	}
	
	
	void OnPostRender ()
	{		
		mesh.Draw(_primitiveMaterial);
	}
	
	
	void Update ()
	{
		_y_rotation += Input.GetAxis("Mouse X") * _rotationSpeed * Time.deltaTime;
		_x_rotation += Input.GetAxis("Mouse Y") * _rotationSpeed * Time.deltaTime;
		ClampCamera();
		camera.transform.eulerAngles = new Vector3(-_x_rotation, _y_rotation, 0.0f);
		
		if (Input.GetAxis("Vertical") != 0.0f)
		{
			if (_cameraMode == CameraMode.Horizontal)
				camera.transform.position += Vector3.Cross(camera.transform.right, Vector3.up) *
											 Input.GetAxis("Vertical") *
											 _movementSpeed * Time.deltaTime;
			else
				camera.transform.position += camera.transform.forward *
											 Input.GetAxis("Vertical") *
											 _movementSpeed * Time.deltaTime;
		}
		
		if (Input.GetAxis("Horizontal") != 0.0f)
			camera.transform.position += camera.transform.right *
										 Input.GetAxis("Horizontal") *
										 _movementSpeed * Time.deltaTime;

		if (Input.GetKey(KeyCode.E))
			camera.transform.position += Vector3.up * _movementSpeed * Time.deltaTime;
			
		if (Input.GetKey(KeyCode.C))
			camera.transform.position -= Vector3.up * _movementSpeed * Time.deltaTime;

		if (Input.GetKeyUp(KeyCode.M))
			if (_cameraMode == CameraMode.Free)
				_cameraMode = CameraMode.Horizontal;
			else
				_cameraMode = CameraMode.Free;
		
		if (Input.GetKeyUp(KeyCode.R))
			Debug.Log(Util.RollDice(new float[] {0.1f, 0.2f, 0.7f}, new int[] {4, 11, 20}));
	}
	
	private void ClampCamera ()
	{
		if (_y_rotation < -360.0f) _y_rotation += 360.0f;
		if (_y_rotation >  360.0f) _y_rotation -= 360.0f;

        if (_x_rotation >  80.0f) _x_rotation = 80.0f;
        if (_x_rotation < -80.0f) _x_rotation = -80.0f;
	}
}
