using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _puntForce = 0;
    public float GrabRadius = 1;

    private GameObject[] _escapePods;
    private bool _grabbing;
    private IActionable _action;
    private Light _light;
    private AudioSource _mixer;

    public List<Message> MessageQueue;
    private float _messageDisplayedUntil;
    private Message _currentMessage;

    private UnityEngine.UI.Text _freefallStatus;

    public bool Won;

    // Use this for initialization
    private void Start ()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _light = GetComponentInChildren<Light>();
        _mixer = GetComponentInChildren<AudioSource>();

        _freefallStatus = GameObject.Find("Freefall Status").GetComponent<UnityEngine.UI.Text>();

        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
    private void Update ()
    {
		Aim();
        LookAhead();
        Punt();
        Inputs();
        UI();
    }

    private void LookAhead()
    {
        _action = null;
        RaycastHit hit;
        if (!Physics.Raycast(transform.position, transform.rotation * Vector3.forward, out hit, 3, 1 << 9)) return;
        _action = hit.transform.gameObject.GetComponentInParent<IActionable>();
    }

    private void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.E) && _action != null)
        {
            _action.Execute();
        }

        if (Input.GetKeyDown(KeyCode.F)) _light.enabled = !_light.enabled;
    }

    private void UI()
    {
        if (_escapePods == null) _escapePods = GameObject.FindGameObjectsWithTag("Escape Pod");

        if (Time.time < _messageDisplayedUntil)
        {
            _freefallStatus.text = _currentMessage.Text.ToUpper();
        }
        else if (MessageQueue.Count > 0)
        {
            _currentMessage = MessageQueue[0];
            MessageQueue.RemoveAt(0);
            _messageDisplayedUntil = Time.time + _currentMessage.Audio.length + 2;
            _freefallStatus.text = _currentMessage.Text.ToUpper();
            _mixer.PlayOneShot(_currentMessage.Audio);
        }
        else if (_action != null && _action.Action() != null)
        {
            _freefallStatus.text = ("press e to " + _action.Action()).ToUpper();
        }
        else if (_grabbing)
        {
            _freefallStatus.text = "DECELERATING";
        }
        else
        {
            var minDistance = 1e6f;
            foreach (var escapePod in _escapePods)
            {
                minDistance = Mathf.Min(minDistance, (transform.position - escapePod.transform.position).magnitude);
            }
            _freefallStatus.text = Mathf.Round(minDistance) + "M TO ESCAPE POD";
        }
    }

    private void Aim()
    {
        float mouseX = Input.GetAxis("Mouse X"), mouseY = Input.GetAxis("Mouse Y");
        bool a = Input.GetKey(KeyCode.D), d = Input.GetKey(KeyCode.A);
        _rigidbody.AddRelativeTorque(-mouseY * 40, mouseX * 40, a ? -40 : (d ? 40 : 0));
    }

    private void Punt()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _puntForce = Mathf.Min(_puntForce + Time.deltaTime * 30000, 15000);
        }
        else
        {
            var backwards = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            _rigidbody.AddRelativeForce(0, 0, backwards ? -_puntForce / 2 : _puntForce);
            _puntForce = 0;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            _rigidbody.drag = 2;
            _grabbing = true;
        }
        else
        {
            _rigidbody.drag = 0.1f;
            _grabbing = false;
        }

    }
}
