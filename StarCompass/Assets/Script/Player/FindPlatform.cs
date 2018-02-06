using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPlatform : MonoBehaviour
{ //吸力
    float speed =0.3f;
    // 目標playform
    public GameObject TargetPlatform;
    // 場上所有platform
    public GameObject[] Platform;
    // 萬有引力場
    public bool targetDir = false;
    //地上
    static public bool isGround = false;
    static public bool onPlatform = false;
    private void FixedUpdate()
    {
        
        if (targetDir)
        {
            MagnetFunction();
        }
        if (isGround)
        {
          //  PlayerController();
        }
        FindClosestObj();
    }
   public GameObject FindClosestObj()
    {
        Platform = GameObject.FindGameObjectsWithTag("platform");
        TargetPlatform = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in Platform)
        {
            Vector3 diff = go.transform.position - position;
            float currDistance = diff.sqrMagnitude;
            if (currDistance < distance)
            {
                TargetPlatform = go;
                distance = currDistance;
            }
        }
        return TargetPlatform;
    }
    void MagnetFunction()
    {

        Vector3 dir = transform.forward;
        float dis = Vector3.Distance(transform.position, TargetPlatform.transform.position);
        print(dis);
       if (dis > 3.5)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPlatform.transform.position, Time.deltaTime * speed);
        }
    }
  /*  void PlayerController()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate((Vector3.left) * 5f * Time.deltaTime);
            print("Hit");
        }

        // 禁空白鍵跳既時侯 target Dir = false 再比力佢望住既方向跳 
        // target dir = TRUE先可以移動
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == TargetPlatform)
        {
            targetDir = true;            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == TargetPlatform)
        {
            targetDir = false;
        }
    }
    
}

