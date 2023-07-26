using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _templates;

    [SerializeField] private Transform _parent;

    [SerializeField] private float _randomizePosition;

    private int _count;

    private void Awake()
    {
        _count = PlayerPrefs.GetInt("COUNT");

        for (int i = 0; i < _count; i++)
        {
            var randomizedPosition = transform.position + 
                new Vector3(Random.Range(-_randomizePosition, _randomizePosition),
                Random.Range(-_randomizePosition, _randomizePosition), 
                Random.Range(-_randomizePosition, _randomizePosition));
            
            foreach (var template in _templates)
            {
                Instantiate(template, randomizedPosition, Quaternion.identity, _parent);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, Vector3.one * _randomizePosition);
    }
}