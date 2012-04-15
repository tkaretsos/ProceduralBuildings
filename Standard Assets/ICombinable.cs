using UnityEngine;

public interface ICombinable
{
  GameObject gameObject { get; }

  MeshFilter meshFilter { get; }
}
