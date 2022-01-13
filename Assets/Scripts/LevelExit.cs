using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            GameManager.Instance.LevelExit();
        }   
    }
}
