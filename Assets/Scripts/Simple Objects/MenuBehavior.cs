using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBehavior : MonoBehaviour {

	public GameObject canvas;
	
	public Button newGame;
	public Button howToPlay;
	public Button howToPlayBack;

	public Text titleText;
	public Text madeByText;
	public Text howToPlayTitle;
	public Text howToPlayContent;

	public TextGuideBehavior tgb;

	// Use this for initialization
	void Start () {
		howToPlayBack.gameObject.SetActive(false);
		howToPlayTitle.gameObject.SetActive(false);
		howToPlayContent.gameObject.SetActive(false);

		newGame.onClick.AddListener(NewGameClick);
		howToPlay.onClick.AddListener(HowToPlayClick);
		howToPlayBack.onClick.AddListener(HowToPlayBackClick);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void NewGameClick() {
		canvas.SetActive(false);
		tgb.StartOpening();
	}

	void HowToPlayClick() {
		newGame.gameObject.SetActive(false);
		howToPlay.gameObject.SetActive(false);
		titleText.gameObject.SetActive(false);
		madeByText.gameObject.SetActive(false);

		howToPlayBack.gameObject.SetActive(true);
		howToPlayTitle.gameObject.SetActive(true);
		howToPlayContent.gameObject.SetActive(true);
	}

	void HowToPlayBackClick() {
		newGame.gameObject.SetActive(true);
		howToPlay.gameObject.SetActive(true);
		titleText.gameObject.SetActive(true);
		madeByText.gameObject.SetActive(true);

		howToPlayBack.gameObject.SetActive(false);
		howToPlayTitle.gameObject.SetActive(false);
		howToPlayContent.gameObject.SetActive(false);
	}

}
