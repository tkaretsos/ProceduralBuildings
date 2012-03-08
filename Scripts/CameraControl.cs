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
  
  private float _x_rotation;
  private float _y_rotation;
  
  private float _last_mouse_x;
  private float _last_mouse_y;
  private float _mouse_x;
  private float _mouse_y;
  private bool _follow_mouse = true;
  
  
  void Start ()
  {
    Screen.showCursor = false;

    _x_rotation = -40f;
    _y_rotation = 45f;
  
    _last_mouse_x = Input.GetAxis("Mouse X");
    _last_mouse_y = Input.GetAxis("Mouse Y");
    _mouse_x = _last_mouse_x;
    _mouse_y = _last_mouse_y;
  }

  void Update ()
  {
    if (_follow_mouse)
    {
      _mouse_x = Input.GetAxis("Mouse X");
      _mouse_y = Input.GetAxis("Mouse Y");
      _y_rotation += Mathf.Lerp(_last_mouse_x * _rotationSpeed, _mouse_x * _rotationSpeed, Time.smoothDeltaTime);
      _x_rotation += Mathf.Lerp(_last_mouse_y * _rotationSpeed, _mouse_y * _rotationSpeed, Time.smoothDeltaTime);
      _last_mouse_x = _mouse_x;
      _last_mouse_y = _mouse_y;
    }

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

    if (Input.GetMouseButtonUp(1))
      if (_follow_mouse)
        _follow_mouse = false;
      else
        _follow_mouse = true;
  }

  public void OnGUI ()
  {
    GUILayout.Label("Camera controls:");
    GUILayout.Label("W - Forward");
    GUILayout.Label("S - Backward");
    GUILayout.Label("D - Right");
    GUILayout.Label("A - Left");
    GUILayout.Label("E - Up");
    GUILayout.Label("C - Down");
    GUILayout.Label("M - Change camera mode");
  }

  private void ClampCamera (float horizontal = 360f, float vertical = 80f)
  {
    if (_y_rotation < -horizontal) _y_rotation += horizontal;
    if (_y_rotation >  horizontal) _y_rotation -= horizontal;
  
    if (_x_rotation >  vertical) _x_rotation =  vertical;
    if (_x_rotation < -vertical) _x_rotation = -vertical;
  }
}
