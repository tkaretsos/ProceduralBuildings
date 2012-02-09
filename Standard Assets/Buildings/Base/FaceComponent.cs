using UnityEngine;
using System.Collections.Generic;

public class FaceComponent
{
	private readonly Face _parent;
	private float _height;
	private List<Vector3> _boundaries = new List<Vector3>();

	/// <summary>
	/// Initializes a new instance of the <see cref="FaceComponent"/> class.
	/// </summary>
	/// <param name='parent'>
	/// The parent face of this component.
	/// </param>
	/// <param name='width'>
	/// Width.
	/// </param>
	/// <param name='height'>
	/// Height.
	/// </param>
	/// <param name='dl'>
	/// Down-left point of the component.
	/// </param>
	/// <param name='dr'>
	/// Down-right point of the component.
	/// </param>
	public FaceComponent (Face parent, Vector3 dr, Vector3 dl, float height_modifier = 1f)
	{
		_parent = parent;
		_height = _parent.Parent.FloorHeight * height_modifier;
		
		_boundaries.Add(dr);
		_boundaries.Add(dl);
		_boundaries.Add(new Vector3(dl.x, dl.y + _height, dl.z));
		_boundaries.Add(new Vector3(dr.x, dr.y + _height, dr.z));
	}
	
	public void Draw ()
	{
		GL.PushMatrix();
		GL.Begin(GL.QUADS);
		foreach (Vector3 v in _boundaries)
			GL.Vertex(v);
		GL.End();
		GL.PopMatrix();
	}
}
