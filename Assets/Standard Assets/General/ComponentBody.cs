using UnityEngine;

namespace Thesis {

public class ComponentBody : DrawableObject
{
  /*************** FIELDS ***************/

  public readonly FaceComponent parentComponent;

  public Face parentFace
  {
    get { return parentComponent.parentFace; }
  }

  public BuildingMesh parentBuilding
  {
    get { return parentComponent.parentBuilding; }
  }

  /*************** CONSTRUCTORS ***************/

  public ComponentBody (FaceComponent parent)
  {
    parentComponent = parent;

    boundaries = new Vector3[4];
    for (var i = 0; i < parentComponent.boundaries.Length; ++i)
      boundaries[i] = parentComponent.boundaries[i] -
                      parentComponent.depth * parentComponent.normal;

    FindMeshOrigin(boundaries[0],
                   boundaries[2],
                   boundaries[1],
                   boundaries[3]);

    for (var i = 0; i < boundaries.Length; ++i)
      boundaries[i] -= meshOrigin;
  }

  /*************** METHODS ***************/

  public override void FindVertices ()
  {
    vertices = boundaries;
  }

  public override void FindTriangles ()
  {
    triangles = new int[] {
      0, 1, 3,
      1, 2, 3
    };
  }

  public override void Draw ()
  {
    base.Draw();

    var uvs = new Vector2[mesh.vertices.Length];
    uvs[1] = new Vector2(0f, 0f);
    uvs[0] = new Vector2(1f, 0f);
    uvs[2] = new Vector2(0f, 1f);
    uvs[3] = new Vector2(1f, 1f);

    mesh.uv = uvs;

    gameObject.transform.position = meshOrigin + parentBuilding.meshOrigin;
    gameObject.transform.parent = parentBuilding.parent.gameObject.transform;
  }
}

} // namespace Thesis