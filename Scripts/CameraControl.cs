  using UnityEngine;

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

  private GameObject meshobj;
  private Mesh mesh;
  private MeshFilter mf;
  private MeshRenderer mr;
  
  
  void Start ()
  {
//    mesh = new Neoclassical(new Vector3( 4f + Random.Range(0.5f, 1.5f), 0f,  1.5f + Random.Range(0.5f, 1.5f)),
//                            new Vector3( 4f + Random.Range(0.5f, 1.5f), 0f, -1.5f - Random.Range(0.5f, 1.5f)),
//                            new Vector3(-4f - Random.Range(0.5f, 1.5f), 0f, -1.5f - Random.Range(0.5f, 1.5f)),
//                            new Vector3(-4f - Random.Range(0.5f, 1.5f), 0f,  1.5f + Random.Range(0.5f, 1.5f)));
//    mesh.ConstructFaces();
//    mesh.ConstructFaceComponents();

    CreateNeoclassical();
  
    _x_rotation = -40f;
    _y_rotation = 50f;
  
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

    if (Input.GetKeyUp(KeyCode.B))
    {
//      mesh = new Neoclassical(new Vector3( 4f + Random.Range(0.5f, 1.5f), 0f,  1.5f + Random.Range(0.5f, 1.5f)),
//      new Vector3( 4f + Random.Range(0.5f, 1.5f), 0f, -1.5f - Random.Range(0.5f, 1.5f)),
//      new Vector3(-4f - Random.Range(0.5f, 1.5f), 0f, -1.5f - Random.Range(0.5f, 1.5f)),
//      new Vector3(-4f - Random.Range(0.5f, 1.5f), 0f,  1.5f + Random.Range(0.5f, 1.5f)));
//      mesh.ConstructFaces();
//      mesh.ConstructFaceComponents();
      Destroy(meshobj);
      CreateNeoclassical();
    }
  } // end of Update()

  private void ClampCamera (float horizontal = 360f, float vertical = 80f)
  {
    if (_y_rotation < -horizontal) _y_rotation += horizontal;
    if (_y_rotation >  horizontal) _y_rotation -= horizontal;
  
    if (_x_rotation >  vertical) _x_rotation =  vertical;
    if (_x_rotation < -vertical) _x_rotation = -vertical;
  }

  void CreateNeoclassical()
  {
    mesh = new Mesh();
    
    meshobj = new GameObject();
    mf = meshobj.AddComponent<MeshFilter>();
    mr = meshobj.AddComponent<MeshRenderer>();
    mr.sharedMaterial = _material;

    Neoclassical neo = new Neoclassical(new Vector3( 1.5f + Random.Range(0.5f, 1.5f), 0,  4f + Random.Range(0.5f, 1.5f)),
                                        new Vector3( 1.5f + Random.Range(0.5f, 1.5f), 0, -4f - Random.Range(0.5f, 1.5f)),
                                        new Vector3(-1.5f - Random.Range(0.5f, 1.5f), 0, -4f - Random.Range(0.5f, 1.5f)),
                                        new Vector3(-1.5f - Random.Range(0.5f, 1.5f), 0,  4f + Random.Range(0.5f, 1.5f)));
    neo.ConstructFaces();
    neo.ConstructFaceComponents();

    mesh.Clear();
    mesh.vertices = neo.FindVertices();
    mesh.triangles = neo.FindTriangles();
    // Assign UVs to shut the editor up -_-'
    mesh.uv = new Vector2[mesh.vertices.Length];
    for (int i = 0; i < mesh.vertices.Length; ++i)
      mesh.uv[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].y);

    mesh.RecalculateNormals();
    mesh.Optimize();
    mf.sharedMesh = mesh;
  }
}
