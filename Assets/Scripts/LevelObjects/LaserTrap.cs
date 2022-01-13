using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
public class LaserTrap : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float lazeRange = 500f;
    [SerializeField] private int maxAmountOfBounces = 5;
    [SerializeField] private int laserID = 0;
    [SerializeField] private bool isActive = true;
    [SerializeField] private LayerMask laserMask = 0;
    [SerializeField] private Transform firePoint = null;
    [SerializeField] private int laserDamage = 2;

    private AudioSource _audioSource = null;

    private Laser _mLaser = null;

    private void Start()
    {
        // Laser
        _mLaser = ScriptableObject.CreateInstance<Laser>();
        _mLaser.Init(lazeRange, maxAmountOfBounces, laserID, laserMask, GetComponent<LineRenderer>(), laserDamage);

        // Audio
        _audioSource = GetComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.playOnAwake = true;
    }

    public void TurnOn()
    {
        isActive = true;
        _audioSource.Pause();
    }

    public void TurnOff()
    {
        isActive = false;
        _mLaser.ClearLaser();
        _audioSource.UnPause();
    }

    // Update is called once per frame
    private void Update()
    {
        if (isActive)
        {
            _mLaser.Shoot(firePoint.position, firePoint.forward);
        }
    }
}
