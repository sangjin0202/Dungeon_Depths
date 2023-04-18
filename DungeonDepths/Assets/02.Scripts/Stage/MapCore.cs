using UnityEngine;

public class MapCore : MonoBehaviour
{
	public Transform Position { get; private set; }   
	public bool IsDestroyed { get; private set; }
	//TODO 주은. 파괴 이벤트 테스트 중
	public delegate void EventHandler();
	public event EventHandler OnEvent;
	// 포탈 생성 여기서 하도록

	private void OnEnable()	// 활성화될 때 초기화되도록
    {
		IsDestroyed = false;
		Position = transform;
    }
    private void Start()
    {
		OnEvent += DestroyedCore;
	}
	private void DestroyedCore()
	{
		//TODO 주은
		IsDestroyed = true;
		Debug.Log("MapCore에서 이벤트 발생 : " + IsDestroyed);
		//gameObject.SetActive(false);
	}
    private void OnDisable()
    {
		//임시 테스트
		OnEvent();
	}
}
