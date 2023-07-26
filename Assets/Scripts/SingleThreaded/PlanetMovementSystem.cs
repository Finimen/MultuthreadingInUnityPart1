using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace SingleThreaded
{
    public class PlanetMovementSystem : MonoBehaviour
    {
        [SerializeField] private float _strengthMultiplier = 1;

        [SerializeField] private TMPro.TMP_Text _info;

        private Stopwatch _calculatingWatch = new Stopwatch();

        public List<Planet> Planets { get; private set; } = new List<Planet>();

        private void FixedUpdate()
        {
            _calculatingWatch.Start();
            CalculateForces();
            _calculatingWatch.Stop();

            ApplyForces();

            _info.text = $"Calculating {_calculatingWatch.ElapsedMilliseconds}ms\n" +
                $"Threads {1}";

            _calculatingWatch.Reset();
        }

        private void CalculateForces()
        {
            foreach (Planet current in Planets)
            {
                foreach (Planet other in Planets)
                {
                    if(other != current)
                    {
                        var direction = (other.transform.position - current.transform.position).normalized;
                        var distance = Vector3.Distance(current.Position, other.Position);
                        var strength = current.Mass * other.Mass / Mathf.Pow(distance, 2) * Time.deltaTime * _strengthMultiplier;

                        current.Force += strength * direction;
                    }
                }
            }
        }

        private void ApplyForces()
        {
            foreach(Planet planet in Planets)
            {
                planet.ApplyForce();
                planet.Force = Vector3.zero;
            }
        }
    }
}