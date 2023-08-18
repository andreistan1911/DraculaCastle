using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactibles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartAll()
    {
        foreach (Transform child in transform)
            if (child.gameObject.tag == "door")
            {
                Door door = child.GetComponent<Door>() as Door;

                if (door.Open)
                {
                    Debug.Log("Old:" + child.position + " " + child.rotation);
                    child.position = door.StartingTransform.position;
                    child.rotation = door.StartingTransform.rotation;
                    Debug.Log("New:" + child.position + " " + child.rotation);

                    door.Open = false;
                }
            }
    }
}
