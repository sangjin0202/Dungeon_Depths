using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectData // 나중에 리소스매니저나 폴더에서 가져오기
{
    public string name;
    public GameObject prefab; // 나중에 리소스매니저나 폴더에서 가져오기
    public int count;   // 미리만들어둘 오브젝트 수
    public Transform parent;    // 모아둘 오브젝트 부모설정
}
public class PoolManager : MonoSingleton<PoolManager>
{

    void Awake()
    {
        Init();
    }

    public List<ObjectData> objList = new List<ObjectData>();
    public bool willGrow = true; // 미리 생성한 오브젝트가 모자를경우 사용할 변수

    public Dictionary<string, List<GameObject>> poolList = new Dictionary<string, List<GameObject>>();
    void Init() // 리소스매니저나 폴더에서 가져온 정보를 풀 딕셔너리에 담는 과정
    {
        GameObject _go, _obj;
        List<GameObject> _list;
        int _count;
        Transform _parent;
        for (int j = 0, jmax = objList.Count; j < jmax; j++) 
        {
            _count = objList[j].count;  // 몇개 미리 생성할지
            _obj = objList[j].prefab;   // 나중에 리소스폴더에서 가져오기.
            _parent = objList[j].parent;    // 오브젝트 풀링할 부모 빈오브젝트 설정
            _list = new List<GameObject>();
            poolList.Add(_obj.name, _list);

            for (int i = 0; i < _count; i++)
            {
                _go = Instantiate(_obj) as GameObject;
                _go.transform.SetParent(_parent);
                _go.SetActive(false);
                _list.Add(_go);
            }
        }
    }

    public GameObject Instantiate(string _name, Vector3 _pos, Quaternion _rot)
    {
        GameObject createObject = Instantiate(_name);
        createObject.transform.position = _pos;
        createObject.transform.rotation = _rot;

        return createObject;

    }

    GameObject Instantiate(string _name)
    {
        if (!poolList.ContainsKey(_name))
        {
            Debug.LogError("Not Found Pooling GameObject name" + _name);
            return null;
        }

        GameObject _rtn = null;
        bool _bFind = false;
        List<GameObject> _list = poolList[_name];
        for (int i = 0; i < _list.Count; i++)
        {
            //gameobject 비할성화 -> 지금사용중이지 않다는 의미...
            if (!_list[i].activeInHierarchy)
            {
                _rtn = _list[i];
                _bFind = true;
                _rtn.SetActive(true);
                break;
            }
        }

        //not found > create
        if (!_bFind && willGrow) // 위에 있는 카운트보다 더 필요한경우 새로생성하기 위한 조건문
        {
            ObjectData _obj = GetObject(_name);
            GameObject _go = Instantiate(_obj.prefab) as GameObject;
            _go.transform.SetParent(_obj.parent);
            _go.SetActive(true);
            _list.Add(_go);

            _rtn = _go;
        }

        return _rtn;
    }

    ObjectData GetObject(string _name) // 새로 생성할 오브젝트가 미리프리팹으로 저장되어 있는지 검사
    {
        ObjectData _obj = null;
        for (int i = 0; i < objList.Count; i++)
        {
            if (objList[i].name == _name)
            {
                _obj = objList[i];
                break;
            }
        }
        return _obj;
    }
}