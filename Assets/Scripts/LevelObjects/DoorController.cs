using UnityEngine;
[RequireComponent(typeof(Animator))]
public class DoorController : MonoBehaviour
{
    private Animator _mAnim;


    [SerializeField] private int amountOfSignalsToOpen = 1;
    [SerializeField] private AudioClip doorMoving = null;

    private AudioSource _doorAudioSource;

    private int _currentAmountOfSignals = 0;
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    private void Start()
    {
        _doorAudioSource = GetComponent<AudioSource>();
        _mAnim = GetComponent<Animator>();
    }

    public void OpenDoor()
    {
        _currentAmountOfSignals++;
        if(_currentAmountOfSignals == amountOfSignalsToOpen)
        {
            _doorAudioSource.PlayOneShot(doorMoving);
            _mAnim.SetBool(IsOpen, true);
        }       
    }

    public void CloseDoor()
    {
        if(_currentAmountOfSignals == amountOfSignalsToOpen)
        {
            _currentAmountOfSignals--;
            _doorAudioSource.PlayOneShot(doorMoving);
            _mAnim.SetBool(IsOpen, false);
        }
    }
}
