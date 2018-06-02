using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_rotate : MonoBehaviour {
//方向灵敏度  
    
    //上下最大视角（Y视角）  
    public float minmumY = -50f;  
    public float maxmunY = 50f;  
  
    float rotationY = 0f;  
  
    void Update()  
    {  
        //根据鼠标移动的快慢（增量），获得相机左右旋转的角度（处理X）  
        float rotationX =  Input.GetAxis("Mouse X");
  
        //根据鼠标移动的快慢（增量），获取相机上下移动的角度（处理Y）  
        rotationY += Input.GetAxis("Mouse Y");
        //角度限制，rotationY小于min返回min  大于max 返回max  否则返回value  
        rotationY = Clamp(rotationY,maxmunY,minmumY);  
  
        //设置摄像机角度  
        transform.localEulerAngles = new Vector3(-rotationY,rotationX,0);  
    }  
  
    public float Clamp(float value,float max,float min)  
    {  
        if (value < min) return min;  
        if (value > max) return max;  
        return value;   
    }  
  
    void Start()  
    {  
        Rigidbody rigidbody = GetComponent<Rigidbody>();  
        if (rigidbody)  
        {  
            rigidbody.freezeRotation = true;  
        }  
    }
    }
    