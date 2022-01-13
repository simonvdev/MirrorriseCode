using System.Collections;
using UnityEngine;

public class MirrorSummon : MonoBehaviour
{
    [SerializeField] private float timeToRise = 5f;
    [SerializeField] private float risingSpeed = 2f;
    private Collider _collider;

    private Vector3 _startSize;
    private Vector3 _endSize;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;

        var localScale = transform.localScale;
        _startSize = new Vector3(localScale.x, 0, localScale.z);
        _endSize = localScale;

        StartCoroutine(LerpScale(_startSize, _endSize, timeToRise));
    }

    private IEnumerator LerpScale(Vector3 start,Vector3 end,float scaleTime)
    {
        var time = 0.0f;
        while(time < scaleTime)
        {
            time += Time.deltaTime * risingSpeed;
            transform.localScale = Vector3.Lerp(start, end, time);
            _collider.enabled = true;
            yield return null;
        }    
    }
}
