using UnityEngine;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour 
{	
  private enum CameraMode
  {
    Horizontal,
    Free
  }

  private enum BuildingMode
  {
    Many,
    Big,
    Small
  }
  
  [SerializeField]
  private CameraMode _cameraMode = CameraMode.Horizontal;
  
  [SerializeField]
  private float _movementSpeed = 10f;
  
  [SerializeField]
  private float _rotationSpeed = 5f;
  
  [SerializeField]
  private Material _material;
  
  private float _x_rotation;
  private float _y_rotation;
  
  private float _last_mouse_x;
  private float _last_mouse_y;
  private float _mouse_x;
  private float _mouse_y;

  private List<Neoclassical> neo = new List<Neoclassical>();

//  private GameObject meshobj;
//  private Mesh mesh;
//  private MeshFilter mf;
//  private MeshRenderer mr;
  
  
  void Start ()
  {
    CreateNeoclassical();
  
    _x_rotation = -40f;
    _y_rotation = 45f;
  
    _last_mouse_x = Input.GetAxis("Mouse X");
    _last_mouse_y = Input.GetAxis("Mouse Y");
    _mouse_x = _last_mouse_x;
    _mouse_y = _last_mouse_y;
  }
  
  
//  void OnPostRender ()
//  {
//    GL.Color(Color.gray);
//  
//    mesh.Draw(_primitiveMaterial);
//  }
  
  
  void Update ()
  {
    _mouse_x = Input.GetAxis("Mouse X");
    _mouse_y = Input.GetAxis("Mouse Y");
    _y_rotation += Mathf.Lerp(_last_mouse_x * _rotationSpeed, _mouse_x * _rotationSpeed, Time.smoothDeltaTime);
    _x_rotation += Mathf.Lerp(_last_mouse_y * _rotationSpeed, _mouse_y * _rotationSpeed, Time.smoothDeltaTime);
    _last_mouse_x = _mouse_x;
    _last_mouse_y = _mouse_y;

    ClampCamera();
    camera.transform.eulerAngles = new Vector3(-_x_rotation, _y_rotation, 0f);

    if (Input.GetAxis("Vertical") != 0f)
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

    if (Input.GetAxis("Horizontal") != 0f)
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

    if (Input.GetKeyUp(KeyCode.Alpha1))
    {
      DestroyBuildings();
      CreateNeoclassical(BuildingMode.Many);
    }

    if (Input.GetKeyUp(KeyCode.Alpha2))
    {
      DestroyBuildings();
      CreateNeoclassical(BuildingMode.Big);
    }

    if (Input.GetKeyUp(KeyCode.Alpha3))
    {
      DestroyBuildings();
      CreateNeoclassical(BuildingMode.Small);
    }
  } // end of Update()

  public void OnGUI ()
  {
    GUILayout.Label("Controls:");
    GUILayout.Label("W - Forward");
    GUILayout.Label("S - Backward");
    GUILayout.Label("D - Right");
    GUILayout.Label("A - Left");
    GUILayout.Label("E - Up");
    GUILayout.Label("C - Down");
    GUILayout.Label("B - New buildings");
    GUILayout.Label("M - Change camera mode");
  }

  private void ClampCamera (float horizontal = 360f, float vertical = 80f)
  {
    if (_y_rotation < -horizontal) _y_rotation += horizontal;
    if (_y_rotation >  horizontal) _y_rotation -= horizontal;
  
    if (_x_rotation >  vertical) _x_rotation =  vertical;
    if (_x_rotation < -vertical) _x_rotation = -vertical;
  }

  private void CreateNeoclassical(BuildingMode mode = BuildingMode.Small)
  {
    switch (mode)
    {
      case BuildingMode.Many:
        for (int i = 0; i < 5; ++i)
          for (int j = 0; j < 5; ++j)
          {
            float x_mod = i * 15f;
            float z_mod = j * 9f;
            neo.Add(new Neoclassical(
              new Vector3(x_mod + 9f + Random.Range(0.5f, 1.5f), 0f, z_mod + 3.5f + Random.Range(0.5f, 1.5f)),
              new Vector3(x_mod + 9f + Random.Range(0.5f, 1.5f), 0f, z_mod - Random.Range(0.5f, 1.5f)),
              new Vector3(x_mod - Random.Range(0.5f, 1.5f), 0f, z_mod - Random.Range(0.5f, 1.5f)),
              new Vector3(x_mod - Random.Range(0.5f, 1.5f), 0f, z_mod + 3.5f + Random.Range(0.5f, 1.5f)),
              _material
            ));
          }
        break;

      case BuildingMode.Big:
        neo.Add(new Neoclassical(
          new Vector3(20f + Random.Range(0.5f, 1.5f), 0f, 8f + Random.Range(0.5f, 1.5f)),
          new Vector3(20f + Random.Range(0.5f, 1.5f), 0f, Random.Range(0.5f, 1.5f)),
          new Vector3(Random.Range(0.5f, 1.5f), 0f, -Random.Range(0.5f, 1.5f)),
          new Vector3(Random.Range(0.5f, 1.5f), 0f, 8f + Random.Range(0.5f, 1.5f)),
          _material
        ));
        break;

      case BuildingMode.Small:
        neo.Add(new Neoclassical(
          new Vector3(9f + Random.Range(0.25f, 0.75f), 0f, 3.5f + Random.Range(0.25f, 0.75f)),
          new Vector3(9f + Random.Range(0.25f, 0.75f), 0f, Random.Range(0.25f, 0.75f)),
          new Vector3(Random.Range(0.25f, 0.75f), 0f, -Random.Range(0.25f, 0.75f)),
          new Vector3(Random.Range(0.25f, 0.75f), 0f, 3.5f + Random.Range(0.25f, 0.75f)),
          _material
        ));
        break;
    }
  }

  public void DestroyBuildings ()
  {
    foreach (Neoclassical n in neo)
      Destroy(n.gameObject);
    neo.Clear();
  }
}
