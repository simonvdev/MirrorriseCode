using UnityEngine;
using UnityEngine.Events;

public class Sensor : MonoBehaviour
{
    private enum TypeOfSensor
    {
        Toggle = 0,
        ContinuousInput = 1,
        OnePress = 2
    }

    [SerializeField] private int laserID = 0;
    [SerializeField] private TypeOfSensor sensorType = TypeOfSensor.OnePress;

    [SerializeField] private UnityEvent onActivate = null;
    [SerializeField] private UnityEvent onDeactivate = null;

    [SerializeField] private AudioClip activationSound = null;
    [SerializeField] private AudioClip deactivationSound = null;

    private AudioSource _audioSource = null;

    public bool Activated { get { return _isActivated; } }

    private bool _isActivated = false;
    private bool _isCalled = false;


    private float _activateDelay = 0.3f;
    private float _timer = 0;
    private float _deactivationTimer = 0;

    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _timer = _activateDelay;
    }


    public void TryActivate(int otherLaserID)
    {
        if (laserID != otherLaserID) return;

        switch (sensorType)
        {
            case TypeOfSensor.Toggle:

                _timer += Time.deltaTime;
                if (_timer > _activateDelay)
                {
                    ToggleActivate();
                    _timer = 0;
                }

                break;
            case TypeOfSensor.ContinuousInput:
                _isCalled = true;
                if (_isActivated == false)
                {
                    onActivate.Invoke();
                    OnActivateThis();
                }
                break;
            case TypeOfSensor.OnePress:
                if(_isActivated == false)
                {                  
                    onActivate.Invoke();
                    OnActivateThis();
                }
                break;
            default:
                break;
        }
    }

    private void ToggleActivate()
    {
        if (!_isActivated)
        {
            OnActivateThis();
            onActivate.Invoke();
        }
        else
        {
            OnDeactivateThis();
            onDeactivate.Invoke();
        }
    }

    private void Update()
    {       
        if(sensorType == TypeOfSensor.ContinuousInput)
        {
            // Every frame check if we are still not activated
            if (_isCalled == false)
            {
                _deactivationTimer += Time.deltaTime;
                if(_deactivationTimer >= _activateDelay)
                {
                    // Deactivate
                    _isCalled = false;
                    onDeactivate.Invoke();
                    OnDeactivateThis();
                }
            }
            else
            {
                _isCalled = false;
                _deactivationTimer = 0;
            }
        }
    }

    private void OnActivateThis()
    {
        if (_isActivated == false)
        {
            _isActivated = true;
            _meshRenderer.material.EnableKeyword("_EMISSION");
            _audioSource.PlayOneShot(activationSound);
        }
    }

    private void OnDeactivateThis()
    {
        if (_isActivated)
        {
            _isActivated = false;
            _meshRenderer.material.DisableKeyword("_EMISSION");
            _audioSource.PlayOneShot(deactivationSound);
        }
    }
}
