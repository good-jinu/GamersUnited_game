using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    DW.DroppedWeaponGenerator gene;
    // Start is called before the first frame update
    void Start()
    {
        gene = new DW.DroppedWeaponGenerator();
        gene.SetPos(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gene.GenDW();
        }
    }
}
