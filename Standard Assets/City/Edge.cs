using UnityEngine;

namespace Thesis {
  
public class Edge
{
  private Vector3 _start;
  public Vector3 start
  {
    get { return _start; }
    set
    {
      _start = value;
      _length = Vector3.Distance(_start, _end);
      _middle = (_start + _end) / 2;
      _direction = (_end - _start).normalized;
    }
  }

  private Vector3 _end;
  public Vector3 end
  {
    get { return _end; }
    set
    {
      _end = value;
      _length = Vector3.Distance(_start, _end);
      _middle = (_start + _end) / 2;
      _direction = (_end - _start).normalized;
    }
  }

  private Vector3 _middle;
  public Vector3 middle
  {
    get { return _middle; }
  }

  private float _length;
  public float length
  {
    get { return _length; }
  }

  private Vector3 _direction;
  public Vector3 direction
  {
    get { return _direction; }
  }

  private Edge () { }

  public Edge (Vector3 from, Vector3 to)
  {
    _start = from;
    _end = to;
    _length = Vector3.Distance(from, to);
    _middle = (from + to) / 2;
    _direction = (to - from).normalized;
  }

  public bool Contains (Vector3 point)
  {
    var dist = Vector3.Distance(_start, point) + Vector3.Distance(point, _end);
    return Mathf.Abs(_length - dist) < 0.01f;
  }
}

} // namespace Thesis