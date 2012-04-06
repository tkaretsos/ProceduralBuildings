using UnityEngine;

using Exception = System.Exception;

public class Util
{
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
  static public int RollDice (float[] chances, int[] numbers = null, int precision = 2)
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
    var start = 0;
    var end = Mathf.FloorToInt(chances[0] * precision);
  
    for (var i = start; i < end; ++i)
      expanded[i] = numbers[0];
  
    for (var i = 1; i < chances.Length; ++i)
    {
      start = end;
      end += Mathf.FloorToInt(chances[i] * precision);
      for (var j = start; j < end; ++j)
        expanded[j] = numbers[i];
    }
  
    for (int i = 0; i < expanded.Length; ++i)
      Debug.Log("asf: " + expanded[i].ToString());


    return expanded[new System.Random().Next(precision)];
  }

	
  static public void PrintVector (string s, Vector3 v)
  {
    Debug.Log(s + " " + v.x + " " + v.y + " " + v.z);
  }
}
