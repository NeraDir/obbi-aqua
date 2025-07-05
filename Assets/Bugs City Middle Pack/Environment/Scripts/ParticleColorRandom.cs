using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleColorRandom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ParticleSystem.MainModule psMain = GetComponent<ParticleSystem>().main;
        psMain.startColor = new ParticleSystem.MinMaxGradient(Color.yellow, Color.white);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
