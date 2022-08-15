using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour, ISpawn
{
    public ItemPickUp_SO[] itmeDefinations;

    private int whichToSpawn = 0;
    private int totalSpawnWeight = 0;
    private int chosen = 0;

    public Rigidbody itemSpawned { get; set; }
    public Renderer itemMaterial { get; set; }
    public ItemPickUp itmeType { get; set; }

    void Start()
    {
        foreach(ItemPickUp_SO ip in itmeDefinations)
        {
            totalSpawnWeight += ip.spawnChanceWeight;
        }

      //  CreateSpawn();
    }

    public void CreateSpawn()
    {
        foreach(ItemPickUp_SO ip in itmeDefinations)
        {
            whichToSpawn += ip.spawnChanceWeight;
            if(whichToSpawn >= chosen)
            {
                itemSpawned = Instantiate(ip.itemSpawnObject,transform.position,Quaternion.identity);

                itemMaterial = itemSpawned.GetComponent<Renderer>();

                if(itemMaterial != null)
                    itemMaterial.material = ip.itemMaterial;

                itmeType = itemSpawned.GetComponent<ItemPickUp>();
                itmeType.itemDefination = ip;
                break;
            }
        }
    }

    
}
