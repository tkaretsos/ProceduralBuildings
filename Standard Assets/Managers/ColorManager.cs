using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thesis {
  
public sealed class ColorManager
{
  private static readonly ColorManager _instance = new ColorManager();
  public static ColorManager Instance
  {
    get { return _instance; }
  }

  private static bool _isInitialized;

  private Dictionary<string, Color> _colors;

  private Dictionary<string, List<Color>> _collections;

  private ColorManager ()
  {
    _colors = new Dictionary<string, Color>();
    _collections = new Dictionary<string, List<Color>>();
    _isInitialized = false;
  }

  public void Init ()
  {
    if (!_isInitialized)
    {
      AddComponentColors();
      AddWallColors();

      _isInitialized = true;
    }
  }

  public void Unload ()
  {
    _colors.Clear();
    foreach (List<Color> l in _collections.Values)
      l.Clear();
    _collections.Clear();
    _isInitialized = false;
  }

  public void Add (string name, Color color)
  {
    if (!_colors.ContainsKey(name))
      _colors.Add(name, color);
  }

  public void AddToCollection (string name, Color color)
  {
    if (_collections.ContainsKey(name))
    {
      if (!_collections[name].Contains(color))
        _collections[name].Add(color);
    }
    else
    {
      var list = new List<Color>();
      list.Add(color);
      _collections.Add(name, list);
    }
  }

  public Color Get (string name)
  {
    return _colors.ContainsKey(name) ? _colors[name] : Color.clear;
  }

  public List<Color> GetCollection (string name)
  {
    return _collections.ContainsKey(name) ? _collections[name] : null;
  }

  private void AddComponentColors ()
  {
    var name = "col_components";
    AddToCollection(name, new Color32( 245, 245, 220, 255));
    AddToCollection(name, new Color32( 210, 180, 140, 255));
    AddToCollection(name, new Color32( 151, 105,  79, 255));
    AddToCollection(name, new Color32( 139, 105, 105, 255));
    AddToCollection(name, new Color32( 139,  90,  51, 255));
    AddToCollection(name, new Color32( 139,  69,  19, 255));
    AddToCollection(name, new Color32( 133,  99,  99, 255));
    AddToCollection(name, new Color32( 107,  66,  38, 255));
    AddToCollection(name, new Color32(  92,  64,  51, 255));
    AddToCollection(name, new Color32(  92,  51,  23, 255));
  }

  private void AddWallColors ()
  {
    var name = "col_walls";
    AddToCollection(name, new Color32( 166, 159, 141, 255));
    AddToCollection(name, new Color32( 231, 206, 165, 255));
    AddToCollection(name, new Color32( 173, 155, 155, 255));
    AddToCollection(name, new Color32( 135, 125, 115, 255));
    AddToCollection(name, new Color32( 155, 157, 143, 255));
    AddToCollection(name, new Color32( 225, 168, 151, 255));
    AddToCollection(name, new Color32( 224, 174, 125, 255));
    AddToCollection(name, new Color32( 216, 191, 150, 255));
    AddToCollection(name, new Color32( 205, 171, 110, 255));
    AddToCollection(name, new Color32( 165, 133, 118, 255));
    AddToCollection(name, new Color32( 190, 169, 164, 255));
    AddToCollection(name, new Color32( 194, 183, 181, 255));
  }
}

} // namespace Thesis