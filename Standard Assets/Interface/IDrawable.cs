
namespace Thesis {
namespace Interface {

public interface IDrawable : ICombinable
{
  UnityEngine.Vector3[] vertices { get; }

  int[] triangles { get; }

  void FindVertices ();

  void FindTriangles ();

  void Draw ();
}

} // namespace Interface
} // namespace Thesis