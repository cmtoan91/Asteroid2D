using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUI_Canvas : SimpleSingleton<GUI_Canvas>
{
    [SerializeField]
    TMP_Text _scoreText;

    [SerializeField]
    TMP_Text _gameOverText;

    [SerializeField]
    GameObject _gameFinishPanel;

    [SerializeField]
    GameObject _lifeDisplayPrefab;

    [SerializeField]
    Transform _lifeDisplayPanel;

    [SerializeField]
    Button _resetButton;

    [SerializeField]
    Button _quitButton;

    GameObject[] _lifeDisplay;

    protected override void Awake()
    {
        base.Awake();
        _resetButton.onClick.AddListener(() => ResetGame());
        _quitButton.onClick.AddListener(() => QuitGame());
    }

    public void InitLifeDisplay(int count)
    {
        _lifeDisplay = new GameObject[count];
        for (int i = 0; i < count; i++)
        {
            _lifeDisplay[i] = Instantiate(_lifeDisplayPrefab, _lifeDisplayPanel);
        }
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = score.ToString();
    }

    public void UpdateLife(int life)
    {
        for(int i = 0; i< _lifeDisplay.Length; i++)
        {
            if(i >= life)
            {
                _lifeDisplay[i].SetActive(false);
            }
        }
    }

    public void FinishGame(bool isWin)
    {
        if (isWin) _gameOverText.text = "YOU WIN";
        else _gameOverText.text = "GAME OVER";
        _gameFinishPanel.SetActive(true);
    }

    void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
