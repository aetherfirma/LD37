using UnityEngine;

namespace Assets.Scripts
{
    public class TheGame : MonoBehaviour
    {
        private GameObject _player;
        private GameObject[] _cryopods;

        public void GameStart()
        {
            _player = GameObject.Find("Player");
            _cryopods = GameObject.FindGameObjectsWithTag("Cryopod");
            Debug.LogWarning(_cryopods.Length);

            var playersPod = _cryopods[Mathf.RoundToInt(Random.value * (_cryopods.Length - 1))];

            _player.transform.rotation = playersPod.transform.rotation * Quaternion.Euler(0, 180, 0);
            _player.transform.position = playersPod.transform.position + (playersPod.transform.rotation * new Vector3(0.78f, 1.72f, 0.804f));
        }
    }
}