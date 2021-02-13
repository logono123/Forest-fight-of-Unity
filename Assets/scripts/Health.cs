using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Networking;
public class Health : NetworkBehaviour { 

    public const int maxHealth = 100;
    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;
    public Slider healthSlider;
    public bool destroyOnDeath = false;
    private NetworkStartPosition[] spawnPoints;

    void Start()
    {
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isServer == false) return;// 血量的处理只在服务器端执行
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            if (destroyOnDeath)
            {
                
                Destroy(this.gameObject); return;
            }
            currentHealth = maxHealth;
            Debug.Log("Dead");
            RpcRespawn();
        }
       
    }
    void OnChangeHealth(int health)
    {

        healthSlider.value = health / (float)maxHealth;
        if (isLocalPlayer)
        {
            GameObject.Find("life2333").GetComponent<Scrollbar>().size = healthSlider.value;
        }
    }
    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer == false) return;

        Vector3 spawnPosition = Vector3.zero;

        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
        }

        this.GetComponent<PlayerController>().life = this.GetComponent<PlayerController>().life - 1;



        transform.position = spawnPosition;
    }
}