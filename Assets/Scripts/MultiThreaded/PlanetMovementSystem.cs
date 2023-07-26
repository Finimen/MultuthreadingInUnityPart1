using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

namespace MultiThreaded
{
    public class PlanetMovementSystem : MonoBehaviour
    {
        [SerializeField] private float _strengthMultiplier = 1;

        [SerializeField] private TMPro.TMP_Text _info;

        private const int treads_count = 20;

        private Stopwatch _calculatingWatch = new Stopwatch();

        private Thread[] _updatingThreads = new Thread[treads_count];
        private SemaphoreSlim _startUpdating = new SemaphoreSlim(0);
        private Barrier _updatingBarrier = new Barrier(treads_count);

        public List<Planet> Planets { get; private set; } = new List<Planet>();

        private float _deltaTime;

        private void Start()
        {
            for (int i = 0; i < _updatingThreads.Length; i++)
            {
                _updatingThreads[i] = new Thread(parameter => UpdateThreadStarted((int)parameter, treads_count));
                _updatingThreads[i].Start(i + 1);
            }
        }

        private void FixedUpdate()
        {
            _deltaTime = Time.fixedDeltaTime;

            _calculatingWatch.Start();
            _startUpdating.Release(treads_count);
            _calculatingWatch.Stop();

            ApplyForces();

            _info.text = $"Calculating {_calculatingWatch.ElapsedMilliseconds}ms\n" +
                $"Threads {treads_count}";

            _calculatingWatch.Reset();
        }

        private void UpdateThreadStarted(int index, int count)
        {
            while (true)
            {
                _startUpdating.Wait();
                UpdateThread(index, count);
                _updatingBarrier.SignalAndWait();
            }
        }

        private void UpdateThread(int packIndex, int totalPacks)
        {
            float from = (float)(packIndex - 1) / totalPacks;
            float to = (float)(packIndex) / totalPacks;

            for (int i = (int)(Planets.Count * from); i < (int)(Planets.Count * to); i++)
            {
                foreach (Planet other in Planets)
                {
                    if (other != Planets[i])
                    {
                        var direction = (other.Position - Planets[i].Position).normalized;
                        var distance = Vector3.Distance(Planets[i].Position, other.Position);
                        var strength = Planets[i].Mass * other.Mass / Mathf.Pow(distance, 2) * _deltaTime * _strengthMultiplier;

                        Planets[i].Force += strength * direction;
                    }
                }
            }

            _updatingBarrier.SignalAndWait();
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