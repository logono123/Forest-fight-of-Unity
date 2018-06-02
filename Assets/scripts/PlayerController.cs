using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class PlayerController : NetworkBehaviour
{
    

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensetivity = 3;

    private PlayerMotor motor;

    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public int killcount=0;
    public int speedupcold = 0;
    public int life = 5;
    
    void Start()
    {
        if (isLocalPlayer == false)
        {
            return;
        }
        motor = GetComponent<PlayerMotor>();
    }
    void Update()
    {
       
      if (isLocalPlayer == false)
        {
            return;
        }
      killcount = 0;
      float _xMov = Input.GetAxisRaw("Horizontal");
      float _zMov = Input.GetAxisRaw("Vertical");

      Vector3 _movHorizontal = transform.right * _xMov;
      Vector3 _movVertical = transform.forward * _zMov;

      Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

      motor.Move(_velocity);

      float _yRot = Input.GetAxisRaw("Mouse X");
      Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensetivity;

      motor.Rotate(_rotation);

      float _xRot = Input.GetAxisRaw("Mouse Y");
      Vector3 _cameraRotation = new Vector3(_xRot, 0f, 0f) * lookSensetivity;

      motor.RotateCamera(_cameraRotation);
        /*
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up * h * 120 * Time.deltaTime);
        transform.Translate(Vector3.forward * v * 6 * Time.deltaTime);*/

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CmdFire();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))//按下leftshift加速3秒，冷却时间6秒
        {
            Speedup();
         
        }

        GameObject.Find("lifetext").GetComponent<Text>().text = "生命:" + life + " 条";



    }
   
    public override void OnStartLocalPlayer()
    {
        //这个方法只会在本地角色那里调用  当角色被创建的时候
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
    [Command]// called in client, run in server
    void CmdFire()//这个方法需要在server里面调用
    {
        // 子弹的生成 需要server完成，然后把子弹同步到各个client
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation) as GameObject;
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 50;
        Destroy(bullet, 2);

        NetworkServer.Spawn(bullet);
    }

    void Speedup()//加速函数，设置计时器作为冷却时间和加速时间  
    {

        if (speedupcold == 0) { 

        speed = 20f;
       
        System.Timers.Timer t = new System.Timers.Timer(3000);//实例化Timer类，设置间隔时间为3000毫秒；

        t.Elapsed += new System.Timers.ElapsedEventHandler(Speedback);//到达时间的时候执行事件；

        t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；

        t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        speedupcold = 1;
        System.Timers.Timer t2 = new System.Timers.Timer(10000);//实例化Timer类，设置间隔时间为10000毫秒；

        t2.Elapsed += new System.Timers.ElapsedEventHandler(coldback);//到达时间的时候执行事件；

        t2.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；

        t2.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        
    }
    }

    public void Speedback(object source, System.Timers.ElapsedEventArgs e) //设置速度回归正常
    {
        speed = 5f;
    }

   /* public void cold()
    {
        
       System.Timers.Timer t2 = new System.Timers.Timer(10000);//实例化Timer类，设置间隔时间为7000毫秒；

        t2.Elapsed += new System.Timers.ElapsedEventHandler(coldback);//到达时间的时候执行事件；

        t2.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；

        t2.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
    } */
    public void coldback(object source, System.Timers.ElapsedEventArgs e) //设置速度回归正常
    {
        speedupcold = 0;
    }
    public void Killcountchanged(int num)
    {
        killcount += num;
    }
}
