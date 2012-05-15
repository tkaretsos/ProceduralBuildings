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

  private Dictionary<string, Color> _colors;

  private Dictionary<string, List<Color>> _collections;

  private ColorManager ()
  {
    _colors = new Dictionary<string, Color>();
    _collections = new Dictionary<string, List<Color>>();
  }

  public void Init ()
  {
    AddComponentColors();
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
}

} // namespace Thesis