using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	private Dictionary<Type, IPlayerComponent> _components;

	private void Awake()
	{
		_components = new Dictionary<Type, IPlayerComponent>();

		IPlayerComponent[] comArr = GetComponentsInChildren<IPlayerComponent>();
		foreach (var component in comArr)
		{
			_components.Add(component.GetType(), component);
		}

		foreach (IPlayerComponent compo in _components.Values)
		{
			compo.Initialize(this);
		}
	}

	public T GetCompo<T>() where T : class
	{
		if (_components.TryGetValue(typeof(T), out IPlayerComponent compo))
			return compo as T;
		return default(T);
	}
}
