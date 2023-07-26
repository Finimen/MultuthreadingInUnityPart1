using System.Threading;
using UnityEngine;

namespace Assets.Scripts
{
    internal class MovingThread : MonoBehaviour
    {
        [SerializeField] private Transform _transform;

        private Vector3 _position;

        private void Start()
        {
            new Thread(() =>
            {
                while (true)
                {
                    _position = _position + Vector3.one;
                }
            }).Start();

            _transform.position = _position;
        }
    }
}