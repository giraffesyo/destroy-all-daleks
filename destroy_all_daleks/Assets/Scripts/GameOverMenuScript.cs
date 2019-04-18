using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenuScript : MonoBehaviour
{
    public Text floorNum;
    public Text totalKilled;
    public Text dalekKilledFloor;
    public GameObject hud;
    private PlayerUIScript hudScript;

    private void Awake()
    {
        hudScript = hud.GetComponent<PlayerUIScript>();

        hud.SetActive(false);
    }

    private void OnEnable()
    {
        AudioSource[] toAssign = this.gameObject.GetComponents<AudioSource>();
        AudioSource whoThemeIntro = toAssign[0];
        AudioSource whoThemeLoop = toAssign[1];
        whoThemeIntro.Play();
        whoThemeLoop.PlayDelayed(whoThemeIntro.clip.length);

        floorNum.text = $"Floor {hudScript.currentFloor}";
        totalKilled.text = $"{hudScript.totalDaleksKilled} destroyed overall";
        dalekKilledFloor.text = $"{ hudScript.daleksKilled}/{ hudScript.totalDaleks} destroyed";
    }

    public void NewGame()
    {
        hud.SetActive(true);
        hudScript.NewGame();
        this.gameObject.SetActive(false);
    }

    public void RestartFloor()
    {
        hud.SetActive(true);
        hudScript.RestartFloor();
        this.gameObject.SetActive(false);
    }
}
