using System.Threading;
using UnityEngine;

namespace Assets.Scripts
{
    internal class SimpleThread : MonoBehaviour
    {
        [SerializeField] private int i;

        private void Start()
        {
            new Thread(() =>
            {
                while (true)
                {
                    i++;
                }
            }).Start();
        }
    }
}