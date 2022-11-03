using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject _winScreen;
    [SerializeField] private TMP_Text _winText;

    public void Win(string textToDisplay)
    {
        _winText.text = textToDisplay;
        _winScreen.SetActive(true);
    }

    public void Replay()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentScene);
    }
}