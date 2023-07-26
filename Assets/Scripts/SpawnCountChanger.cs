using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnCountChanger : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_InputField _countField;

    private void Awake()
    {
        _countField.onSubmit.AddListener(SaveAndExit);
    }

    private void SaveAndExit(string text)
    {
        PlayerPrefs.SetInt("COUNT", int.Parse(text));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}