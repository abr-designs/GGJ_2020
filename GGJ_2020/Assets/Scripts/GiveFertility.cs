using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveFertility : MonoBehaviour
{
    public GameObject plane;
    public int growth = 1;
    public int fertility;
    SphereCollider CircleCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

              if(fertility <= 10)
        {

            fertility++;
        }
    }
}
