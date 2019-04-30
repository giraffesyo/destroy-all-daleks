using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuScript : MonoBehaviour
{
    public Text floorNum;
    public Text totalKilled;
    public Text dalekKilledFloor;
    public GameObject hud;
    private PlayerUIScript hudScript;

    private Toggle[] toggles;
    private Toggle musicToggle;
    private Toggle heartBeatToggle;
    private Toggle breathToggle;


    private bool isSettingToggles;

    private void Awake()
    {
        isSettingToggles = false;
        hudScript = hud.GetComponent<PlayerUIScript>();

        
        hud.SetActive(false);
    }

    private void OnEnable()
    {
        floorNum.text = $"Floor {hudScript.currentFloor}";
        totalKilled.text = $"{hudScript.totalDaleksKilled} destroyed overall";
        dalekKilledFloor.text = $"{ hudScript.daleksKilled}/{ hudScript.totalDaleks} destroyed";

        SetToggles();
        Cursor.visible = true;
    }

    private void Start()
    {
        toggles = System.Array.ConvertAll(GetComponentsInChildren(typeof(Toggle), true), (t => (Toggle)t));
        musicToggle = toggles[0];
        heartBeatToggle = toggles[1];
        breathToggle = toggles[2];

        SetToggles();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Unpause();
        }
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

    public void SetToggles()
    {
        if (musicToggle != null)
        {
            bool toCheck = true;
            toCheck = hudScript.playMusic;
            if (musicToggle.isOn != toCheck)
            {
                isSettingToggles = true;
                musicToggle.isOn = toCheck;
            }
            toCheck = hudScript.playBeat;
            if (heartBeatToggle.isOn != toCheck)
            {
                isSettingToggles = true;
                heartBeatToggle.isOn = toCheck;
            }
            toCheck = hudScript.playBreath;
            if (breathToggle.isOn != toCheck)
            {
                isSettingToggles = true;
                breathToggle.isOn = toCheck;
            }
        }
    }

    public void MusicToggle()
    {
        if (!isSettingToggles)
        {
            //hudScript.playMusic = !hudScript.playMusic;
        }
        else
        {
            isSettingToggles = false;
        }
    }

    public void HeartBeatToggle()
    {
        if (!isSettingToggles)
        {
            hudScript.playBeat = !hudScript.playBeat;
        }
        else
        {
            isSettingToggles = false;
        }
    }

    public void BreathToggle()
    {
        if (!isSettingToggles)
        {
            hudScript.playBreath = !hudScript.playBreath;
        }
        else
        {
            isSettingToggles = false;
        }
    }

    private void Unpause()
    {
        hud.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
