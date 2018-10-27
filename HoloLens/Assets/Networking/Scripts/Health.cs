using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    public const int maxHealth = 100;

    public bool destroyOnDeath;

    [SyncVar(hook = "OnChangeHealth")]
    public int currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        if(!isServer)
        {
            return;
        }
        
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            if(destroyOnDeath)
            {
                Destroy(gameObject);
            } else
            {
                // existing Respawn code 
                currentHealth = maxHealth;

                // Called on the server, run on the clients
                RpcRespawn();
            }
            
        }
    }

    void OnChangeHealth(int currentHealth)
    {
        Debug.Log("Health changed");
    }

    // [ClientRpc] methods are called on the server and run on the client
    [ClientRpc]
    void RpcRespawn()
    {
        if(isLocalPlayer)
        {
            transform.position = Vector3.zero;
        }
    }
}
