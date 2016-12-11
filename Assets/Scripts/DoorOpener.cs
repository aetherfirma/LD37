using System.Reflection;
using UnityEngine;

namespace Assets.Scripts
{
    public class DoorOpener : MonoBehaviour, IActionable
    {
        private Transform _leftDoor, _rightDoor;
        private Vector3 _leftClosed, _rightClosed;
        private float _doorOpened, _doorOpenTime;

        public bool Open;

        private void Start()
        {
            _leftDoor = transform.Find("left door");
            _rightDoor = transform.Find("right door");

            _leftClosed = _leftDoor.position;
            _rightClosed = _rightDoor.position;

            _doorOpenTime = 10 + Random.value * 20;
        }

        private void Update()
        {
            if (Open && Time.time > _doorOpened + _doorOpenTime) Open = false;

            Vector3 leftOpen = _leftDoor.parent.rotation * new Vector3(1, 0, 0),
                rightOpen = _leftDoor.parent.rotation * new Vector3(-1, 0, 0),
                leftDesired, rightDesired;
            if (Open)
            {
                leftDesired = _leftClosed + leftOpen;
                rightDesired = _rightClosed + rightOpen;
            }
            else
            {
                leftDesired = _leftClosed;
                rightDesired = _rightClosed;
            }

            var leftDiff = leftDesired - _leftDoor.position;
            var rightDiff = rightDesired - _rightDoor.position;
            _leftDoor.position += Vector3.ClampMagnitude(leftDiff * Time.deltaTime * 2, leftDiff.magnitude);
            _rightDoor.position += Vector3.ClampMagnitude(rightDiff * Time.deltaTime * 2, rightDiff.magnitude);
        }

        public void Execute()
        {
            Open = true;
            _doorOpened = Time.time;
        }

        public string Action()
        {
            return !Open ? "open the door" : null;
        }
    }
}