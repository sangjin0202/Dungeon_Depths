using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    enum DataTpye { GameObj, Sound, Effect }
	
	Object LoadResources(string _path, int _type)
    {
        Object _prefab = null; 
        if (_type == (int)DataTpye.GameObj)
		{
            _prefab = Resources.Load($"{_path}");
        }
        else if (_type == (int)DataTpye.Sound)
        {
            _prefab = Resources.Load($"Sounds/{_path}");
        }
        else if (_type == (int)DataTpye.Effect)
        {
            _prefab = Resources.Load($"Effects/{_path}");
        }
        if(_prefab == null)
            return null;
        return _prefab;

    }

    void Awake()
	{

    }

}