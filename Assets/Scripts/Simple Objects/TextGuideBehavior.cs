using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextGuideBehavior : MonoBehaviour {

	public Text guide;

	private float startTime;

	private string[] lines;
	private float[] delays;
	private bool newGuide = false;
	private int index = 0;

	private string[] OPENING_LINES = {
		"Kurent, a mighty force of nature...",
        "you have descended on winter earth to help life and the living.",
        "Your mission in life is to banish snow from the land.",
        "Try moving around with W, A, S, D",
		"Why don't you check out that shiny bell over there!",
		"Pick up the bell by pressing E.",
		""
	};
	private float[] OPENING_DELAYS = {
		2, 5, 8, 11, 14, 17, 20
	};
	private bool opening = false;

	private string[] DANCING_LINES_ONE = {
		"Great! Now try dancing around by pressing 1.",
        "That's how you cleanse the earth of snow."
	};  
    private float[] DANCING_DELAYS_ONE = {
    	2, 5
    };
	private bool dancingOne = false;

	private string[] DANCING_LINES_TWO = {
		"Now try shaking your hips a little wilder! Press 2"
	};
	private float[] DANCING_DELAYS_TWO = {
		2
	};
	private bool dancingTwo = false;

	private string[] LESS_SNOW_LINES = {
		"You managed to cleanse quite some snow!",
		"See if there's someone in the village that is especially gratefull.",
		"",
		"The villager has a special bell they want to give you!",
		"Press E to accept it"
    };
    private float[] LESS_SNOW_DELAYS = {
    	2, 5, 10, 13, 16
    };
    private bool lessSnow = false;

	private string[] BIG_BELL_LINES = {
		"Now go wild, Kurent! Try dances 3 and 4 ;)"
	};
	private float[] BIG_BELL_DELAY = {
		2
	};
	private bool bigBell = false;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
		StartOpening();
	}
	
	// Update is called once per frame
	void Update () {
		if (newGuide) {
			index = 0;
			newGuide = false;
		}

		if (opening) {
			lines = OPENING_LINES;
			delays = OPENING_DELAYS;
			opening = UpdateGuide();
		} else if (dancingOne) {
			lines = DANCING_LINES_ONE;
			delays = DANCING_DELAYS_ONE;
			dancingOne = UpdateGuide();
		} else if (dancingTwo) {
			lines = DANCING_LINES_TWO;
			delays = DANCING_DELAYS_TWO;
			dancingTwo = UpdateGuide();
		} else if (lessSnow) {
			lines = LESS_SNOW_LINES;
			delays = LESS_SNOW_DELAYS;
			lessSnow = UpdateGuide();
		} else if (bigBell) {
			lines = BIG_BELL_LINES;
			delays = BIG_BELL_DELAY;
			bigBell = UpdateGuide();
		}
	}

	bool UpdateGuide() {
		if (delays[index] < Time.time - startTime) {
			guide.text = lines[index];
			index ++;
			if (index == lines.Length) {
				return false;
			}
		}
		return true;
	}

	void StartOpening() {
		opening = true;
		newGuide = true;
	}

	void StartDancingOne() {
		if (!opening) {
			dancingOne = true;
			newGuide = true;
		}
	}

	void StartDancingTwo() {
		if (!opening && !dancingOne) {
			dancingTwo = true;
			newGuide = true;
		}
	}

	void StartLessSnow() {
		if (!opening && !dancingOne && dancingTwo) {
			lessSnow = true;
			newGuide = true;
		}
	}

	void StartBigBell() {
		if (!opening && !dancingOne && !dancingTwo && !lessSnow) {
			bigBell = true;
			newGuide = true;
		}
	}
}
