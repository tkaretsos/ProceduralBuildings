using UnityEngine;
using System.Collections.Generic;

public class FaceComponent
{
	private readonly Face _parent;
	private float _width;
	private float _height;
	private List<Vector3> _boundaries = new List<Vector3>();
	
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
