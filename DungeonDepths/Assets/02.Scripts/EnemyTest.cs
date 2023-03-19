using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest : EnemyBase 
{

  protected override void Awake()
  {
    
	}

  void Start() 
  {

  }

  void Update()
  {
    sm.Execute();

  }

  public override void Init() 
  {
  
  }
  public override void GetHit()
  {
  
  }
}
