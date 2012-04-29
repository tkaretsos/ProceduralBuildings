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

  public void Add (string name, Type type)
  {
    if (!_textures.ContainsKey(name))
    {
      ConstructorInfo[] ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
      var texture = (ProceduralTexture) ctors[0].Invoke(new object[] { });
      texture.Draw();
      _textures.Add(name, texture);
    }
  }

  public ProceduralTexture Get (string name)
  {
    return _textures.ContainsKey(name) ? _textures[name] : null;
  }
}

} // namespace Thesis