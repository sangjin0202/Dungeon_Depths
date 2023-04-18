using UnityEngine;

public class TreasureChest : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    void Update()
    {
    }
}
