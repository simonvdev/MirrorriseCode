using UnityEngine;

public class Placer : MonoBehaviour
{

    [HideInInspector] public bool canPlace = true;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
    }

    private void OnTriggerStay(Collider other)
    {
        canPlace = false;
        if (other.gameObject.CompareTag("Player"))
        {
            canPlace = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canPlace = true;
    }
    
    public void SetRenderState(bool state)
    {
        _renderer.enabled = state;
    }
}
