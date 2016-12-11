using Assets.Scripts;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider _collider;
    private float _puntForce = 0;
    public float GrabRadius = 1;

    private GameObject[] _escapePods;
    private bool _grabbing;
    private IActionable _action;

    private UnityEngine.UI.Slider _pushSlider;
    private UnityEngine.UI.Text _freefallStatus;

    // Use this for initialization
    private void Start ()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();

        _pushSlider = GameObject.Find("Push Slider").GetComponent<UnityEngine.UI.Slider>();
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
    }

    private void UI()
    {
        if (_escapePods == null) _escapePods = GameObject.FindGameObjectsWithTag("Escape Pod");

        if (_action != null && _action.Action() != null)
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
        _pushSlider.value = _puntForce;
    }

    private void Aim()
    {
        float mouseX = Input.GetAxis("Mouse X"), mouseY = Input.GetAxis("Mouse Y");
        bool a = Input.GetKey(KeyCode.A), d = Input.GetKey(KeyCode.D);
        _rigidbody.AddRelativeTorque(-mouseY * 40, mouseX * 40, (a ? -20 : (d ? 20 : 0)));
    }

    private void Punt()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _puntForce = Mathf.Min(_puntForce + Time.deltaTime * 30000, 15000);
        }
        else
        {
            _rigidbody.AddRelativeForce(0, 0, _puntForce);
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
