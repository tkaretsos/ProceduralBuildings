using System.Collections.Generic;
using UnityEngine;

namespace Thesis {

public sealed class Road : DrawableObject
{
  public Road (Block parent)
  {
    boundaries = new Vector3[8];
    for (int i = 0; i < 4; ++i)
    {
      boundaries[i] = parent.edges[i].start;
      boundaries[i + 4] = parent.sidewalkVerts[i];
    }
  }

  public override void FindVertices()
  {
    vertices = boundaries;
  }

  public override void FindTriangles()
  {
    triangles = new int[24];
    int i = 0;

    triangles[i++] = 0;
    triangles[i++] = 1;
    triangles[i++] = 5;

    triangles[i++] = 0;
    triangles[i++] = 5;
    triangles[i++] = 4;

    triangles[i++] = 1;
    triangles[i++] = 2;
    triangles[i++] = 6;

    triangles[i++] = 1;
    triangles[i++] = 6;
    triangles[i++] = 5;

    triangles[i++] = 2;
    triangles[i++] = 3;
    triangles[i++] = 7;

    triangles[i++] = 2;
    triangles[i++] = 7;
    triangles[i++] = 6;

    triangles[i++] = 3;
    triangles[i++] = 0;
    triangles[i++] = 4;

    triangles[i++] = 3;
    triangles[i++] = 4;
    triangles[i++] = 7;
  }
}

} // namespace Thesis