using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    AreaEffector2D windEffector;
    BoxCollider2D boxCollider2D;

    private float initialForce;

    // Start is called before the first frame update
    private void Awake() {
        windEffector = GetComponent<AreaEffector2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        initialForce = Random.Range(-1, 3.5f);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        windEffector.forceMagnitude = initialForce + Random.Range(-0.5f, 0.5f);
    }
}
