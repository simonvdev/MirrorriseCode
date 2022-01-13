using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorObject : MonoBehaviour
{
    [SerializeField] private LayerMask spawnLayer;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == spawnLayer.value)
        {
            Destroy(gameObject);
        }
    }
}
