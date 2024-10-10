using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
	private Dictionary<Type, IPlayerComponent> _components;

	private void Awake()
	{
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _components = new Dictionary<Type, IPlayerComponent>();

		GetComponentsInChildren<IPlayerComponent>().ToList()
			.ForEach(compo => _components.Add(compo.GetType(), compo));

		_components.Values.ToList().ForEach(compo => compo.Initialize(this)); //������Ʈ �ʱ�ȭ ����

		_components.Values.ToList().ForEach(compo => compo.AfterInitialize()); //�ʱ�ȭ ���� ����
	}

	public T GetCompo<T>() where T : class
	{
		if (_components.TryGetValue(typeof(T), out IPlayerComponent compo))
			return compo as T;
		return default(T);
	}
}
