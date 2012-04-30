using System;
using System.Reflection;
using System.Collections.Generic;
using Thesis.Base;

namespace Thesis {

public sealed class TextureManager
{
  private static readonly TextureManager _instance = new TextureManager();
  public static TextureManager Instance
  {
    get { return _instance; }
  }
  
  private Dictionary<string, ProceduralTexture> _textures;

  private TextureManager ()
  {
    _textures = new Dictionary<string, ProceduralTexture>();
  }

  public void Add (string name, ProceduralTexture texture)
  {
    if (!_textures.ContainsKey(name))
      _textures.Add(name, texture);
  }

  public void Set (string name, ProceduralTexture texture)
  {
    if (_textures.ContainsKey(name))
      _textures[name] = texture;
  }

  public ProceduralTexture Get (string name)
  {
    return _textures.ContainsKey(name) ? _textures[name] : null;
  }
}

} // namespace Thesis