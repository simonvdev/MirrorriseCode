using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour
{

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject damageImage = null;
    [SerializeField] private float damageScreenDisplayTime = 0.5f;
    private int _currentHealth = 0;


    private bool _isDead = false;

    private void Start()
    {
        _currentHealth = maxHealth;
    }

    // Update is called once per frame
    public void SubtractHealth(int amountToSubtract)
    {
        _currentHealth -= amountToSubtract;
        StartCoroutine(OnDamageTaken());
        if (_currentHealth <= 0 && !_isDead)
        {
            _isDead = true;
            OnDeath();
        }
    }

    private IEnumerator OnDamageTaken()
    {
        if (!damageImage.activeSelf)
        {
            damageImage.SetActive(true);
            yield return new WaitForSeconds(damageScreenDisplayTime);
            damageImage.SetActive(false);
        }
    }

    private void OnDeath()
    {
        GameManager.Instance.OnDeath();
    }
}
