using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextGuideBehavior : MonoBehaviour {

	public Text guide;

    private GameObject player;

	private float startTime;

	private string[] lines;
	private float[] delays;
	private bool newGuide = false;
	private int index = 0;

	private string[] OPENING_LINES = {
		"Kurent, sila narave...",
        "Znajdeš se v zasneženi dolini, ki že predolgo zimuje!",
        "Tvoja naloga je pregnati sneg in mraz iz te bele pokrajine.",
        "",
		"Le kaj je tisti zvonec, ki na palici visi?",
        ""
	};
	private float[] OPENING_DELAYS = {
		2, 5, 8, 11, 14, 17
	};
	private bool opening = false;
    private bool pack1Done = false;

	private string[] DANCING_LINES_ONE = {
		"Novo opasani zvonec ti omogoči, da plešeš in zvoniš.",
        "Tako čistiš zemljo snega.",
        "",
        "Kaj pa je tisti zeleni grmiček ob vznožju gore?",
        ""
    };  
    private float[] DANCING_DELAYS_ONE = {
    	0, 3, 6, 9, 12
    };
	private bool dancingOne = false;
    private bool pack2Done = false;

    private string[] BIG_BELL_LINES = {
		"To je Zeleni Jurij, dobri duh narave!",
        "\"Pogumni kurent, ki koračiš v mraz in sneg!\"",
        "\"Čas je, da se narava prebudi...\"",
        "\"Na, vzemi ta zvonec, preženi mraz!\"",
        "\"Naj trava spet pokrije pobočja te doline!\"",
        ""
	};
	private float[] BIG_BELL_DELAY = {
        0, 3, 6, 9, 12, 15
    };
	private bool bigBell = false;
    private bool pack3Done = false;

    private string[] END_LINES = {
        "Sedaj si popolno opremnljen za premagovanje zime.",
        "Bitja pomladi ti bodo hvaležna, če očistiš dolino snega!",
        ""
    };
    private float[] END_DELAY = {
        0, 3, 6
    };
    private bool endMsg = false;
    private bool pack4Done = false;

    // Use this for initialization
    void Start () {
		startTime = Time.time;
        player = GetComponentInParent<UIcontrol>().player;
		StartOpening();
	}

    // Update is called once per frame
    void Update() {
        if (newGuide) {
            index = 0;
            newGuide = false;
            startTime = Time.time;
        }

        if (player.GetComponent<PlayerBellBehavior>().maxPower >= 1)
            StartDancingOne();
        if (player.GetComponent<PlayerBehavior>().nearJurij)
            StartBigBell();
        if (player.GetComponent<PlayerBellBehavior>().maxPower >= 2)
            StartEnd();

        if (endMsg) {
            lines = END_LINES;
            delays = END_DELAY;
            endMsg = UpdateGuide();
        } else if (bigBell) {
            lines = BIG_BELL_LINES;
            delays = BIG_BELL_DELAY;
            bigBell = UpdateGuide();
		} else if (dancingOne) {
			lines = DANCING_LINES_ONE;
			delays = DANCING_DELAYS_ONE;
			dancingOne = UpdateGuide();
		} else if (opening) {
            lines = OPENING_LINES;
            delays = OPENING_DELAYS;
            opening = UpdateGuide();
        }
	}

	bool UpdateGuide() {
		if (delays[index] < Time.time - startTime) {
			guide.text = lines[index];
			if (index == lines.Length - 1) {
				return false;
			} else {
                index++;
            }
		}
		return true;
	}

	public void StartOpening() {
        if (!pack1Done)
        {
            opening = true;

            newGuide = true;
            pack1Done = true;
        }
	}

	public void StartDancingOne() {
		if (pack1Done && !pack2Done) {
			dancingOne = true;
            opening = false;

            newGuide = true;
            pack2Done = true;

        }
	}

	public void StartBigBell()
    {
        if (pack1Done && pack2Done && !pack3Done) {
			bigBell = true;
            dancingOne = false;
            opening = false;

            newGuide = true;
            pack3Done = true;

        }
    }

    public void StartEnd()
    {
        if (pack1Done && pack2Done && pack3Done && !pack4Done)
        {
            endMsg = true;
            dancingOne = false;
            opening = false;
            bigBell = false;

            newGuide = true;
            pack4Done = true;

        }
    }
}
