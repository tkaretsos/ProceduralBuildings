using UnityEngine;

using Random = System.Random;
using Exception = System.Exception;
using CombinablesCollection = System.Collections.Generic.IList<Thesis.ICombinable>;

namespace Thesis {

public class Util
{
  public static Random random = new Random();

  /// <summary>
  /// Rolls a weighted dice.
  /// </summary>
  /// <returns>
  /// The result of the dice (random).
  /// </returns>
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
    var sum = 0f;
    foreach (var chance in chances)
    {
      if (chance == 0f) throw new Exception("RollDice: a chance cannot be zero.");
      sum += chance;
    }
    if (sum != 1f) throw new Exception("RollDice: the sum of chances is not equal to 1.");
  
    if (numbers == null)
    {
      numbers = new int[chances.Length];
      for (var i = 0; i < chances.Length; ++i)
      numbers[i] = i + 1;
    }
    else if (chances.Length != numbers.Length)
      throw new Exception("RollDice: number of chances not equal to number of possible values");

    precision = (int) Mathf.Pow(10, precision);

    int[] expanded = new int[precision];
    int start = 0;
    int end = Mathf.FloorToInt(chances[0] * precision);
  
    for (var i = start; i < end; ++i)
      expanded[i] = numbers[0];
  
    for (var i = 1; i < chances.Length; ++i)
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

	
  public static void PrintVector (string s, Vector3 v)
  {
    Debug.Log(s + " " + v.x + " " + v.y + " " + v.z);
  }
}

} // namespace Thesis