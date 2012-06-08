using UnityEngine;

using Random = System.Random;
using Exception = System.Exception;
using CombinablesCollection = System.Collections.Generic.IList<Thesis.Interface.ICombinable>;

namespace Thesis {

public class Util
{
  public static Random random = new Random();

  /// <summary>
  /// Rolls a weighted dice.
  /// </summary>
  /// <param name='chances'>
  /// The chances for each possible result.
  /// </param>
  /// <param name='numbers'>
  /// A set of the possible result values.
  /// </param>
  /// <param name='precision'>
  /// The maximum of decimal digits that a number has.
  /// </param>
  public static int RollDice (float[] chances, int[] numbers = null, int precision = 2)
  {
    if (numbers == null)
    {
      numbers = new int[chances.Length];
      for (var i = 0; i < chances.Length; ++i)
        numbers[i] = i + 1;
    }

    precision = (int) Mathf.Pow(10, precision);

    int[] expanded = new int[precision];
    int start = 0;
    int end = 0;
    for (var i = 0; i < chances.Length; ++i)
    {
      start = end;
      end += Mathf.FloorToInt(chances[i] * precision);
      for (var j = start; j < end; ++j)
        expanded[j] = numbers[i];
    }

    return expanded[random.Next(precision)];
  }

  public static GameObject CombineMeshes (string name,
                                          string materialName,
                                          CombinablesCollection objects,
                                          GameObject parent = null)
  {
    var gameObject = new GameObject(name);
    gameObject.active = false;
    if (parent != null)
      gameObject.transform.parent = parent.transform;
    var meshFilter = gameObject.AddComponent<MeshFilter>();
    var meshRenderer = gameObject.AddComponent<MeshRenderer>();
    meshRenderer.sharedMaterial = Resources.Load("Materials/" + materialName,
                                                 typeof(Material)) as Material;

    MeshFilter[] meshFilters = new MeshFilter[objects.Count];
    for (var i = 0; i < objects.Count; ++i)
    {
      meshFilters[i] = objects[i].meshFilter;
      GameObject.Destroy(objects[i].gameObject);
    }

    CombineInstance[] combine = new CombineInstance[meshFilters.Length];
    for (var i = 0; i < meshFilters.Length; ++i)
    {
      combine[i].mesh = meshFilters[i].sharedMesh;
      combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
    }

    meshFilter.mesh = new Mesh();
    meshFilter.mesh.CombineMeshes(combine);

    return gameObject;
  }

  public static Vector3 IntersectionPoint (Vector3 p1, Vector3 dir1,
                                           Vector3 p2, Vector3 dir2)
  {
    var dir_cross = Vector3.Cross(dir1, dir2);
    var tmp_cross = Vector3.Cross(p2 - p1, dir2);

    float c1;
    if (dir_cross.x != 0f)
      c1 = tmp_cross.x / dir_cross.x;
    else if (dir_cross.y != 0f)
      c1 = tmp_cross.y / dir_cross.y;
    else if (dir_cross.z != 0f)
      c1 = tmp_cross.z / dir_cross.z;
    else
      throw new System.DivideByZeroException("Result of cross product is Vector3(0,0,0)!");

    return p1 + c1 * dir1;
  }
	
  public static void PrintVector (string s, Vector3 v)
  {
    Debug.Log(s + " " + v.x + " " + v.y + " " + v.z);
  }
}

} // namespace Thesis