using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider _collider;
    private float _puntForce = 0;
    public float GrabRadius = 1;

    private bool _grabbing;

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
        Punt();
        Inputs();
        UI();
    }

    private void Inputs()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pressed E");
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.rotation * Vector3.forward, out hit, 3, 1 << 9))
            {
                var doorOpener = hit.transform.gameObject.GetComponentInParent<DoorOpener>();
                doorOpener.OpenDoor();
            }
        }
    }

    private void UI()
    {
        if (_grabbing)
        {
            _freefallStatus.text = "GRABBING";
        }
        else if (CanGrab())
        {
            _freefallStatus.text = "";
        }
        else
        {
            _freefallStatus.text = "FLOATING";
        }
        _pushSlider.value = _puntForce;
    }

    private void Aim()
    {
        float mouseX = Input.GetAxis("Mouse X"), mouseY = Input.GetAxis("Mouse Y");
        bool a = Input.GetKey(KeyCode.A), d = Input.GetKey(KeyCode.D);
        _rigidbody.AddRelativeTorque(-mouseY * 40, mouseX * 40, (a ? -20 : (d ? 20 : 0)));
    }

    private bool CanGrab()
    {
        return Physics.CheckSphere(_rigidbody.position, GrabRadius, ~(1 << 8));
    }

    private void Punt()
    {
        var grab = CanGrab();
        if (Input.GetKey(KeyCode.Mouse0))
        {
            _puntForce = Mathf.Min(_puntForce + Time.deltaTime * 30000, 15000);
        }
        else
        {
            if (grab) _rigidbody.AddRelativeForce(0, 0, _puntForce);
            _puntForce = 0;
        }
        if (grab && Input.GetKey(KeyCode.Mouse1))
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
