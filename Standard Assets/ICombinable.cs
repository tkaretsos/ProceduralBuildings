using UnityEngine;

namespace Thesis {

public interface ICombinable
{
  GameObject gameObject { get; }

  MeshFilter meshFilter { get; }
}

} // namespace Thesis