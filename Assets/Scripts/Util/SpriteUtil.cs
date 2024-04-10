using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Izumik
{
    public class SpriteUtil : MonoBehaviour
    {
        SpriteRenderer spriteRenderer;
        Rigidbody rb;
        Enemy enemy;

        [SerializeField] bool useFlip;
        void Start()
        {
            if (useFlip)
            {
                spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
                enemy = GetComponent<Enemy>();
            }
            transform.rotation = Camera.main.transform.rotation;
        }
        void Update ()
        {
            if(useFlip && !enemy.isWaiting)
            {
                spriteRenderer.flipX = enemy.moveLeft;
            }
        }
    }
}
