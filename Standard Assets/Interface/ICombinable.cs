
namespace Thesis {
namespace Interface {

public interface ICombinable
{
  UnityEngine.GameObject gameObject { get; }

  UnityEngine.MeshFilter meshFilter { get; }

  string materialName { get; }
}

} // namespace Interface
} // namespace Thesis