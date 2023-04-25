using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class WayPoints : MonoBehaviour
{
    //local Point -> world Point
    public List<Vector3> localList = new List<Vector3>(); // 인스펙터창에서 설정한 좌표
    List<Vector3> worldList = new List<Vector3>();        // 인스펙터 창에서 설정한 좌표를 월드로 고정시키기 위한 리스트
    int index = -1;
    public Vector3 destinationPoint; // 좌표 도달값

    public void SetWayPoints()
    {
        worldList.Clear();

        if (Random.Range(0f, 1f) <= 0.5f) // 50프로확률로 방향설정   
        {
            for (int i = 0; i < localList.Count; i++) //0 번쨰 리스트부터 이동
            {
                worldList.Add(transform.TransformPoint(localList[i]));
                //로컬 좌표로 설장한 리스트를 월드좌표로 변경해서 저장
            }
        }
        else
        {
            for (int i = localList.Count - 1; i >= 0; i--) //마지막 리스트부터 거꾸로 이동
            {
                worldList.Add(transform.TransformPoint(localList[i]));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if (Application.isPlaying)
        {
            for (int i = 0; i < worldList.Count - 1; i++)
            {
                Gizmos.DrawLine(worldList[i], worldList[i + 1]);
            }
        }
        else
        {
            //editor
            for (int i = 0; i < localList.Count - 1; i++)
            {
                Gizmos.DrawLine(
                    transform.TransformPoint(localList[i]),
                    transform.TransformPoint(localList[i + 1]));
            }
            for (int i = 0; i < localList.Count; i++)
            {
                Gizmos.DrawSphere(transform.TransformPoint(localList[i]), 0.5f);
            }
        }
    }
    public void MoveNextPoint()
    {
        index = (index + 1) % worldList.Count;
        destinationPoint = worldList[index];
    }

    public bool CheckDestination(float _stopDistance, Vector3 _targetPos) // 도달지점검사
	{
        float _stopDistanceDouble = _stopDistance * _stopDistance;
        Vector3 _dir = destinationPoint - _targetPos;
		if (_dir.sqrMagnitude < _stopDistanceDouble)
		{
            return true;
		}
        return false;
	}

}