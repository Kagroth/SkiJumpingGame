using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    ParticleSystem windParticleSystem;
    AreaEffector2D windEffector;
    BoxCollider2D boxCollider2D;

    // Start is called before the first frame update
    private void Awake() {
        windEffector = GetComponent<AreaEffector2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        ParticleSystem.ShapeModule ps = windParticleSystem.shape;
        ps.position = Vector3.zero - new Vector3(0, boxCollider2D.size.y / 2, 0);
        ps.scale = new Vector3(boxCollider2D.size.x, boxCollider2D.size.y, 1);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
