using UnityEngine;

namespace Assets.Scripts
{
    public class EscapePod : MonoBehaviour, IActionable
    {
        public bool Activated;
        private Transform _door;
        private PlayerController _player;
        private float _activated;

        private void Start()
        {
            _door = transform.Find("escape pod door").transform;
            _player = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        public string Action()
        {
            return !Activated ? "engage the escape pod" : null;
        }

        public void Execute()
        {
            if (Activated) return;
            Activated = true;
            _activated = Time.time;
        }

        private void Update()
        {
            if (!Activated) return;
            _door.rotation = Quaternion.Lerp(_door.rotation, transform.rotation * Quaternion.identity, (Time.time - _activated) * 0.01f);
            if (_door.rotation != transform.rotation * Quaternion.identity) return;
            Physics.gravity = Vector3.up * 20;
            _player.Won = true;
        }
    }
}