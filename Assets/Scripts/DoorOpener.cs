using UnityEngine;

namespace Assets.Scripts
{
    public class DoorOpener : MonoBehaviour
    {
        private Transform _leftDoor, _rightDoor;
        private Vector3 _leftClosed, _rightClosed;

        public bool Open;

        private void Start()
        {
            _leftDoor = transform.Find("left door");
            _rightDoor = transform.Find("right door");

            _leftClosed = _leftDoor.position;
            _rightClosed = _rightDoor.position;
        }

        private void Update()
        {
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
    }
}