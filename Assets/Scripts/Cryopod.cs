using UnityEngine;

namespace Assets.Scripts
{
    public class Cryopod : MonoBehaviour, IActionable
    {
        private Transform _door;
        private float _actioned;
        public bool Open;

        private void Start()
        {
            _door = transform.Find("cryopod door").transform;
        }

        private void Update()
        {
            var target = Open ? Quaternion.Euler(70, 0, 0) : Quaternion.Euler(0, 0, 0);
            _door.rotation = Quaternion.Lerp(_door.rotation, transform.rotation * target, (Time.time - _actioned) * 0.01f);
        }

        public void Action()
        {
            Open = !Open;
            _actioned = Time.time;
        }
    }
}