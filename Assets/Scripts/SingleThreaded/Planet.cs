using UnityEngine;

namespace SingleThreaded
{
    [RequireComponent(typeof(Rigidbody))]
    public class Planet : MonoBehaviour
    {
        private Transform _transfrom;
        private Rigidbody _rigidbody;

        public Vector3 Position => _transfrom.position;
        public float Mass => _rigidbody.mass;
        public Vector3 Force { get; set; }

        public void ApplyForce()
        {
            _rigidbody.AddForce(Force);
        }

        private void Awake()
        {
            _transfrom = GetComponent<Transform>();
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            FindObjectOfType<PlanetMovementSystem>().Planets.Add(this);
        }

        private void OnDisable()
        {
            if(gameObject.scene.isLoaded)
            {
                FindObjectOfType<PlanetMovementSystem>().Planets.Remove(this);
            }
        }
    }
}