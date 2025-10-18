/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    private Inventory inventory;
    public GameObject image;
    public GameObject inv_slot;
    // Start is called before the first frame update
    void Start()
    {
        inventory=GameObject.FindGameObjectWithTag("Character").GetComponent<Inventory>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D item)
    {
        if(item.CompareTag("Character"))
            {
            for(int i = 0;i<inventory.slot.Length;i++)
            {
                if (inventory.isFull[i] == false)
                {
                    inventory.isFull[i] = true;
                    inventory.inventory_ui.SetActive(true);
                    inventory.slot[i].SetActive(true);
                    inv_slot.SetActive(true);
                    Instantiate(image, inventory.slot[i].transform, false);
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }
}
*/