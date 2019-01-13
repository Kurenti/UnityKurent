using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuBehavior : MonoBehaviour {

	public GameObject canvas;
	
	public Button newGame;
	public Button howToPlay;
    public Button quit;
	public Button howToPlayBack;
	public Button pauseResume;
    public Button pauseMainMenu;

	public Text titleText;
	public Text madeByText;
	public Text howToPlayTitle;
	public Text howToPlayContent;
	public Text pauseTitle;
	public Text deadTitle;
	public Text deadContent;

	public TextGuideBehavior tgb;

	// Use this for initialization
	void Start () {
		howToPlayBack.gameObject.SetActive(false);
		howToPlayTitle.gameObject.SetActive(false);
		howToPlayContent.gameObject.SetActive(false);

		pauseResume.gameObject.SetActive(false);
        pauseMainMenu.gameObject.SetActive(false);
        pauseTitle.gameObject.SetActive(false);

		deadTitle.gameObject.SetActive(false);
		deadContent.gameObject.SetActive(false);

		newGame.onClick.AddListener(NewGameClick);
		howToPlay.onClick.AddListener(HowToPlayClick);
        quit.onClick.AddListener(QuitClick);
		howToPlayBack.onClick.AddListener(HowToPlayBackClick);
		pauseResume.onClick.AddListener(Unpause);
        pauseMainMenu.onClick.AddListener(MainMenuClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	void NewGameClick() {
		canvas.SetActive(false);
		tgb.StartOpening();

		deadTitle.gameObject.SetActive(false);
		deadContent.gameObject.SetActive(false);
    }

	void HowToPlayClick() {
		newGame.gameObject.SetActive(false);
		howToPlay.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
		madeByText.gameObject.SetActive(false);

		howToPlayBack.gameObject.SetActive(true);
		howToPlayTitle.gameObject.SetActive(true);
		howToPlayContent.gameObject.SetActive(true);
	}

    void QuitClick() {
        Application.Quit();
    }

    void HowToPlayBackClick() {
		newGame.gameObject.SetActive(true);
		howToPlay.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        titleText.gameObject.SetActive(true);
		madeByText.gameObject.SetActive(true);

		howToPlayBack.gameObject.SetActive(false);
		howToPlayTitle.gameObject.SetActive(false);
		howToPlayContent.gameObject.SetActive(false);
	}

    public void Pause() {
		canvas.SetActive(true);

		newGame.gameObject.SetActive(false);
		howToPlay.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
		madeByText.gameObject.SetActive(false);

		howToPlayBack.gameObject.SetActive(false);
		howToPlayTitle.gameObject.SetActive(false);
		howToPlayContent.gameObject.SetActive(false);

		pauseTitle.gameObject.SetActive(true);
        pauseMainMenu.gameObject.SetActive(true);
        pauseResume.gameObject.SetActive(true);
	}

    void Unpause() {
		canvas.SetActive(false);

		newGame.gameObject.SetActive(true);
		howToPlay.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);
        titleText.gameObject.SetActive(true);
		madeByText.gameObject.SetActive(true);

		howToPlayBack.gameObject.SetActive(true);
		howToPlayTitle.gameObject.SetActive(true);
		howToPlayContent.gameObject.SetActive(true);

		pauseTitle.gameObject.SetActive(false);
        pauseMainMenu.gameObject.SetActive(false);
        pauseResume.gameObject.SetActive(false);
	}

    void MainMenuClick() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Died() {
		canvas.SetActive(true);

		newGame.gameObject.SetActive(false);
		howToPlay.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
        titleText.gameObject.SetActive(false);
		madeByText.gameObject.SetActive(false);

		howToPlayBack.gameObject.SetActive(false);
		howToPlayTitle.gameObject.SetActive(false);
		howToPlayContent.gameObject.SetActive(false);

		pauseTitle.gameObject.SetActive(false);
        pauseMainMenu.gameObject.SetActive(true);
        pauseResume.gameObject.SetActive(false);

		deadTitle.gameObject.SetActive(true);
		deadContent.gameObject.SetActive(true);
	}

}
