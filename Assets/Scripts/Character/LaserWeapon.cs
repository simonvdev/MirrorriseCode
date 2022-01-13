using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class LaserWeapon : MonoBehaviour
{

    [SerializeField] private float lazeRange = 500f;
    [SerializeField] private int maxAmountOfBounces = 5;
    [SerializeField] private int angleIncrement = 15;
    [SerializeField] private int maxAmountOfMirrors = 3;

    [SerializeField] private Transform firePoint = null;
    [SerializeField] private GameObject mirrorObject = null;
    [SerializeField] private GameObject targetObject = null;
    [SerializeField] private GameObject prefabPreview = null;
    [SerializeField] private GameObject buildModeText = null;
    [SerializeField] private Text angleText = null;

    [SerializeField] private AudioClip mirrorSummonClip = null;
    [SerializeField] private AudioClip mirrorDestroyClip = null;
    [SerializeField] private GameObject mirrorDestroyParticle = null;

    [SerializeField] private Material mirrorMaterial = null;
    [SerializeField] private Material highLightMaterial = null;
    [SerializeField] private string summonedMirrorTag = "SummonMirror";

    private AudioSource _weaponAudioSource = null;

    private Transform _selectedMirror = null;
    private List<GameObject> _summonedMirrors = new List<GameObject>();
    private Laser _mLaser = null;
    private Placer _placer;

    private RaycastHit _spawnHit;

    [SerializeField] private LayerMask buildMask = 0;
    [SerializeField] private LayerMask laserMask = 0;

    private bool _buildMode = false;
    private int _laserID = 0;
    private int _spawnAngle = 0;

    private void Awake()
    {
        int laserDamage = 2;
        _mLaser = ScriptableObject.CreateInstance<Laser>();
        _mLaser.Init(lazeRange, maxAmountOfBounces,_laserID, laserMask, GetComponentInChildren<LineRenderer>(), laserDamage);
        _placer = prefabPreview.GetComponent<Placer>();

        _weaponAudioSource = GetComponent<AudioSource>();
        _weaponAudioSource.loop = true;

        
    }

    private void Update()
    {
        if (Input.GetButtonDown("BuildMode"))
            _buildMode = !_buildMode;

        if(_selectedMirror != null)
        {
            Renderer mirrorRenderer = _selectedMirror.GetComponentInChildren<Renderer>();
            if(mirrorRenderer != null)
            {
                mirrorRenderer.material = mirrorMaterial;
                _selectedMirror = null;
            }
        }

        if (_buildMode)
        {

            buildModeText.SetActive(true);

            _placer.SetRenderState(true);

            RotateMirror();

            UpdateBuildPreview();

            if (Input.GetButtonDown("Fire1"))
                SummonMirror();

            if (Input.GetButtonDown("Fire2"))
                DestroyMirror();

            

        }
        else
        {

            if (!targetObject.activeSelf)
            {
                targetObject.SetActive(true);
                
            }

            _placer.SetRenderState(false);
            buildModeText.SetActive(false);

            if (Input.GetButton("Fire1"))
            {
                _weaponAudioSource.UnPause();
                _mLaser.Shoot(firePoint.position, firePoint.forward);
            }
            if (Input.GetButtonUp("Fire1"))
            {
                _weaponAudioSource.Pause();
                _mLaser.ClearLaser();
            }

            UpdateTargetObject();
        }
    }

    private void RotateMirror()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 || Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {

            _spawnAngle += (int)(Mathf.Sign(Input.GetAxisRaw("Mouse ScrollWheel"))) * angleIncrement;

            _spawnAngle = Mathf.Clamp(_spawnAngle, -90, 90);

            if (angleText)
                angleText.text = "MIRROR ANGLE | " + Mathf.Abs(_spawnAngle);
        }
    }

    private void UpdateBuildPreview()
    {
        if (targetObject.activeSelf)
            targetObject.SetActive(false);

        if (Physics.Raycast(firePoint.position, firePoint.forward, out _spawnHit, lazeRange, buildMask))
        {
            if (_spawnHit.transform.tag != summonedMirrorTag)
            {
                _placer.SetRenderState(true);

                prefabPreview.transform.position = _spawnHit.point;
                prefabPreview.transform.rotation = Quaternion.LookRotation(_spawnHit.transform.right, _spawnHit.normal);
                prefabPreview.transform.Rotate(new Vector3(0, _spawnAngle, 0), Space.Self);

            }
            else
            {
                _placer.SetRenderState(false);

                _selectedMirror = _spawnHit.transform;
                _selectedMirror.GetComponentInChildren<Renderer>().material = highLightMaterial;
            }
        }
        else
        {
            _placer.SetRenderState(false);
        }

        if (_placer.canPlace)
            prefabPreview.GetComponentInChildren<Renderer>().material.color = new Color(0.4481131f, 0.9705883f, 1, 0.5f);
        else
            prefabPreview.GetComponentInChildren<Renderer>().material.color = new Color(1f, 0f, 0f, 0.3f);
    }

    private void UpdateTargetObject()
    {
        RaycastHit hit;

        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, lazeRange))
            targetObject.transform.position = hit.point;
        else
        {
            if (targetObject.activeSelf)
                targetObject.SetActive(false);
        }
    }

    private void SummonMirror()
    {
        if (Physics.Raycast(firePoint.position, firePoint.forward, out _spawnHit, lazeRange, buildMask))
        {  
            if (_placer.canPlace)
            {
                // Instantiating the mirror
                Quaternion objectRotation = Quaternion.LookRotation(_spawnHit.transform.right, _spawnHit.normal);
                GameObject mirror = (GameObject)Instantiate(mirrorObject, _placer.transform.position, _placer.transform.rotation);

                // Checking if max mirrors aren't reached
                if (_summonedMirrors.Count < maxAmountOfMirrors)
                {
                    AudioSource.PlayClipAtPoint(mirrorSummonClip, _placer.transform.position);
                    _summonedMirrors.Add(mirror);
                }
                else
                {
                    // Remove first place mirror from list 

                    Instantiate(mirrorDestroyParticle, _summonedMirrors[0].transform.position, _summonedMirrors[0].transform.rotation);
                    AudioSource.PlayClipAtPoint(mirrorDestroyClip, _summonedMirrors[0].transform.position);
                    Destroy(_summonedMirrors[0]);
                    _summonedMirrors.RemoveAt(0);
                    
                    // Adding the new one

                    _summonedMirrors.Add(mirror);
                }

            }
        }
    }

    private void DestroyMirror()
    {
        RaycastHit hit;
        if(Physics.Raycast(firePoint.position, firePoint.forward, out hit, lazeRange,buildMask))
        {
            if(hit.transform.tag == summonedMirrorTag)
            {
                // Remove the mirror object from the list and destroying it
                _placer.canPlace = true;
                _summonedMirrors.Remove(hit.transform.gameObject);
                GameObject spawnedParticle = Instantiate(mirrorDestroyParticle, hit.transform.position, hit.transform.rotation);
                AudioSource.PlayClipAtPoint(mirrorDestroyClip, hit.transform.position);
                Destroy(hit.transform.gameObject);
            }
        }
    }
}
