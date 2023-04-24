using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnumTypes;
public class OptionController : MonoBehaviour
{
    List<GameObject> optionList = new List<GameObject>();
    void Awake()
    {
        for(int i = 1; i <= 5; i++)
            optionList.Add(transform.GetChild(i).gameObject);
    }

    void Update()
    {

    }
}
