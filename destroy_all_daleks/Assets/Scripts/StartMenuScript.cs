using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour
{
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

        //hud.SetActive(false);

        toggles = System.Array.ConvertAll(GetComponentsInChildren(typeof(Toggle), true), (t => (Toggle)t));
        musicToggle = toggles[0];
        heartBeatToggle = toggles[1];
        breathToggle = toggles[2];
        StartCoroutine("DisableHUD");
    }

    private void OnEnable()
    {
        Cursor.visible = true;
    }

    private void Start()
    {
        AudioSource[] toAssign = this.gameObject.GetComponents<AudioSource>();
        AudioSource whoThemeIntro = toAssign[0];
        AudioSource whoThemeLoop = toAssign[1];
        whoThemeIntro.Play();
        whoThemeLoop.PlayDelayed(whoThemeIntro.clip.length);
        SetToggles();
    }

    public void SetToggles()
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

    public void NewGame()
    {
        hud.SetActive(true);
        hudScript.NewGame();
        this.gameObject.SetActive(false);
    }

    public void MusicToggle()
    {
        if (!isSettingToggles)
        {
            hudScript.playMusic = !hudScript.playMusic;
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

    IEnumerator DisableHUD()
    {
        yield return new WaitForSeconds(0.001f);
        this.gameObject.SetActive(false);
    }
}
