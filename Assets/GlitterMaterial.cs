using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterMaterial : MonoBehaviour
{
    [SerializeField] private float timeToDie;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeToDie);
    }
}
