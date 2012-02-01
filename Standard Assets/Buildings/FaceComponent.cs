using UnityEngine;
using System.Collections.Generic;

public class FaceComponent
{
#pragma warning disable 0414
	private readonly Face _parent;
	private float _width;
	private float _height;
	private List<Vector3> _boundaries = new List<Vector3>();
#pragma warning restore 0414
	
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
	public FaceComponent (Face parent, float width, float height, Vector3 dl, Vector3 dr)
	{
		_parent = parent;
		_width = width;
		_height = height;
		
		_boundaries.Add(dr);
		_boundaries.Add(dl);
		_boundaries.Add(new Vector3(dl.x, dl.y + _height, dl.z));
		_boundaries.Add(new Vector3(dr.x, dr.y + _height, dr.z));
	}
}
