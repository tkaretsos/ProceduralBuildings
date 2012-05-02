
namespace Thesis {
namespace Interface {

public interface ICombinable
{
  UnityEngine.GameObject gameObject { get; }

  UnityEngine.MeshFilter meshFilter { get; }

  UnityEngine.Material   material   { get; }
}

} // namespace Interface
} // namespace Thesis