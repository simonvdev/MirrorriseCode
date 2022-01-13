using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : ScriptableObject
{
    private float _mLaserRange = 500f;
    private int _mMaxAmountOfBounces = 5;
    private int _mAmountOfBounces = 0;
    private int _mLaserID = 0;
    private int _mDamage = 2;

    private List<Vector3> _mHitPositions;
    private LineRenderer _mLineRenderer = null;
    private LayerMask _mHitMask;


    public void Init(float laserRange,int maxBounce, int laserID, LayerMask hitMask , LineRenderer renderer , int damage)
    {
        _mMaxAmountOfBounces = maxBounce;
        _mLaserRange = laserRange;
        _mHitMask = hitMask;
        _mLineRenderer = renderer;
        _mHitPositions = new List<Vector3>();
        _mLaserID = laserID;
        _mDamage = damage;
    }

    public void Shoot(Vector3 start, Vector3 direction)
    {
        RaycastHit hit;
        _mHitPositions.Add(start);
        direction.Normalize();

        if (Physics.Raycast(start, direction, out hit, _mLaserRange, _mHitMask))
        {
            if (_mAmountOfBounces <= _mMaxAmountOfBounces )
            {
                if(hit.transform.tag == "Player")
                {
                    hit.transform.GetComponent<Health>().SubtractHealth(_mDamage);
                }

                if(hit.transform.GetComponent<Sensor>())
                {
                    Sensor sensor = hit.transform.GetComponent<Sensor>();
                    sensor.TryActivate(_mLaserID);
                }

                if(hit.transform.tag == "Mirror" || hit.transform.tag == "SummonMirror")
                {
                    _mAmountOfBounces++;
                    start = hit.point;
                    direction = Vector3.Reflect(direction.normalized, hit.normal.normalized);
                    direction.Normalize();

                    Shoot(start, direction);
                }
                else
                {
                    _mHitPositions.Add(hit.point);
                    UpdateLineRenderer();
                    return;
                }
            }
            else
            {
                UpdateLineRenderer();
                return;
            }            
        }
        else
        {
            _mHitPositions.Add(start + direction * _mLaserRange);
            UpdateLineRenderer();
            return;
        }
    }

    private void UpdateLineRenderer()
    {
        if (_mLineRenderer)
        {
            _mLineRenderer.positionCount = _mHitPositions.Count;
            _mLineRenderer.SetPositions(_mHitPositions.ToArray());
        }
        _mHitPositions.Clear();
        _mAmountOfBounces = 0;
    }

    public void ClearLaser()
    {
        _mHitPositions.Clear();
        _mLineRenderer.positionCount = 0;
        _mAmountOfBounces = 0;
    }
}
