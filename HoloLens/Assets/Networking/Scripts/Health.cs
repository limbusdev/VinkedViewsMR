/*
Copyright 2019 Georg Eckert (MIT License)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/
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
