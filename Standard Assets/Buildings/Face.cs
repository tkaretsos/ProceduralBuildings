#pragma once

using UnityEngine;
using System.Collections.Generic;

public class Face
{
	// fields
#pragma warning disable 0414
	private readonly BuildingMesh _parent;
	private Vector3 _normal;
	private float _width;
	private bool _is_free;
	private List<Vector3> _boundaries = new List<Vector3>();
	private List<FaceComponent> _face_components = new List<FaceComponent>();
#pragma warning restore 0414
	
	
	// properties
	public BuildingMesh Parent
	{
		get { return _parent; }
	}
	
	
	public Vector3 Normal
	{
		get { return _normal; }
	}
	
	
	public float Width
	{
		get { return _width; }
	}
	
	
	// constructors
	public Face(BuildingMesh parent, Vector3 dl, Vector3 dr)
	{
		_parent = parent;
		
		_boundaries.Add(dl);
		_boundaries.Add(dr);
		_boundaries.Add(new Vector3(dr.x, dr.y + _parent.Height, dr.z));
		_boundaries.Add(new Vector3(dl.x, dl.y + _parent.Height, dl.z));
		
		this.CalculateNormal();
		this.CalculateWidth();
	}
	
	
	// methods
	private void CalculateNormal()
	{
		Vector3 edge1 = new Vector3(_boundaries[1].x - _boundaries[0].x,
									_boundaries[1].y - _boundaries[0].y,
									_boundaries[1].z - _boundaries[0].z);
		
		Vector3 edge2 = new Vector3(_boundaries[3].x - _boundaries[0].x,
									_boundaries[3].y - _boundaries[0].y,
									_boundaries[3].z - _boundaries[0].z);
		
		_normal = Vector3.Cross(edge2, edge1).normalized;
	}
	
	
	private void CalculateWidth()
	{
		Vector3 edge = new Vector3(_boundaries[0].x - _boundaries[1].x,
								   0f,
								   _boundaries[0].z - _boundaries[1].z);
		_width = edge.magnitude;
	}
}
