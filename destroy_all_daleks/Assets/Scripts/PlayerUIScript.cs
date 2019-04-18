using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIScript : MonoBehaviour
{
    public float maxHealth;
    private float health;
    private bool invincible;
    public int maxAmmoGun;
    public int maxAmmoReserve;
    private int ammoGun;
    private int ammoReserve;
    public int totalDaleks;
    [HideInInspector]
    public int daleksKilled;
    [HideInInspector]
    public int totalDaleksKilled;
    private int totalDaleksKilledBeforeFloor;
    [HideInInspector]
    public int currentFloor;

    public Text ammoGunText;
    public Text ammoReserveText;
    public Text totalDaleksText;
    public Text daleksKilledText;

    private float heartTimer;
    public float beatspm;
    public float maxBeatpm;
    private float breathTimer;
    public float breathspm;
    public float maxBreathpm;
    public bool playBeat = true;
    public bool playBreath = true;

    public bool playMusic = true;

    private AudioClip[] maleBreath;
    private AudioClip maleBreathFinal;
    private AudioClip[] femaleBreath;
    private AudioClip femaleBreathFinal;
    private AudioSource heartBeat;
    private AudioSource breath;
    private AudioSource backgroundCalm;
    private AudioSource backgroundEnergetic;
    public bool isMale;

    public Image[] hitIndicators;
    private Sprite[] hitLeftSprites;
    private Sprite[] hitRightSprites;
    private Sprite[] hitForwardSprites;
    private Sprite[] hitBehindSprites;
    private Image hitLeft;
    private Image hitRight;
    private Image hitForward;
    private Image hitBehind;
    private List<Image> toFadeOut;
    public float fadeSpeed;

    public Image tunnelVision;
    private float tunnelVisionTargetAlpha;
    public Slider healthBar;

    public GameObject PauseMenu;
    public GameObject GameOverMenu;


    //sides: 0-left, 1-forward, 2-right, 3-behind, 4-all, 5-heal
    private enum Side { left, forward, right, behind, all, heal }

    private void Awake()
    {
        toFadeOut = new List<Image>();

        AssignAudio();
        AssignHitIndicators();
    }

    void Start()
    {
        NewGame();
    }

    private void OnEnable()
    {
        invincible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            Pause();
        }
        //TestAction();

        Breath();
        HeartBeat();
        FadeHitIndicators(Time.deltaTime);
        SetVision(Time.deltaTime);
    }

    private void TestAction()
    {
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{
        //    FlashDamage(Side.left);
        //}
        //else if (Input.GetKeyDown(KeyCode.RightArrow))
        //{
        //    FlashDamage(Side.right);
        //}
        //else if (Input.GetKeyDown(KeyCode.UpArrow))
        //{
        //    FlashDamage(Side.forward);
        //}
        //else if (Input.GetKeyDown(KeyCode.DownArrow))
        //{
        //    FlashDamage(Side.behind);
        //}
        //else if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    FlashDamage(Side.all);
        //}

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Damage(2, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Damage(2, 2);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Damage(2, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Damage(2, 3);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(-20, 5);
        }
    }


    //to heal, pass in a negative amount, with s=5
    //sides: 0-left, 1-forward, 2-right, 3-behind, 4-all
    public void Damage(float amount, int s = 4)
    {
        if (!invincible)
        {
            if (s > 5 || s < 0)
            {
                s = 4;
            }
            Side side = (Side)s;

            if (side != Side.heal)
            {
                FlashDamage(side);
            }

            if (amount > 0 && health <= amount)
            {
                health = 0f;
                Dead();
            }
            else
            {
                health -= amount;
                healthBar.value = health / maxHealth * 100;
            }

            if (health > maxHealth)
            {
                health = maxHealth;
            }

            if (health / maxHealth <= 0.5)
            {
                SetHeartBeat();
            }
            else
            {
                beatspm = 0;
            }

            if (health / maxHealth <= 0.4)
            {
                float maxAlpha = .9f;
                tunnelVisionTargetAlpha = (1 - (health / maxHealth)) * maxAlpha;
            }
            else
            {
                tunnelVisionTargetAlpha = 0;
            }

            if (health / maxHealth <= .3)
            {
                SetBreath();
            }
            else
            {
                breathspm = 0;
            }
        }
    }

    private void SetVision(float deltaTime)
    {
        Color toSet = tunnelVision.color;
        if (Mathf.Abs(toSet.a - tunnelVisionTargetAlpha) > 0.0001f)
        {

            toSet.a = Mathf.Lerp(tunnelVision.color.a, tunnelVisionTargetAlpha, fadeSpeed / 3 * deltaTime);
            tunnelVision.color = toSet;
        }
    }

    private void SetHeartBeat()
    {
        heartBeat.volume = (1 - (health / maxHealth));
        beatspm = ((1 - (health / maxHealth)) * maxBeatpm) + 30;
    }

    private void SetBreath()
    {
        breathspm = ((1 - (health / maxHealth)) * maxBreathpm);
    }

    public void Fire()
    {
        ammoGun -= 1;
        ammoGunText.text = ammoGun.ToString();
        //if the gun is out of bullets reload
        if (ammoGun <= 0 && ammoReserve > 0)
        {
            Reload();
        }
    }

    public void Reload()
    {
        //need to call reload method for gun animation
        //or call it when reload is called in Fire or AmmoPickup
        if (ammoReserve >= maxAmmoGun)
        {
            int ammoDifference = maxAmmoGun - ammoGun;
            ammoGun = maxAmmoGun;
            ammoReserve -= ammoDifference;
        }
        else if (ammoReserve > 0)
        {
            int ammoDifference = maxAmmoGun - ammoGun;
            int toAdd = 0;
            if (ammoDifference > ammoReserve)
            {
                toAdd = ammoReserve;
            }
            else
            {
                toAdd = ammoDifference;
            }
            ammoGun += toAdd;
            ammoReserve -= toAdd;
        }

        ammoGunText.text = ammoGun.ToString();
        ammoReserveText.text = $"/{ammoReserve.ToString()}";
    }

    public void AmmoPickup(int amount)
    {
        if (maxAmmoReserve > ammoReserve + amount)
        {
            ammoReserve += amount;
        }
        else
        {
            ammoReserve = maxAmmoReserve;
        }

        ammoReserveText.text = $"/{ammoReserve.ToString()}";

        if (ammoGun <= 0)
        {
            Reload();
        }
    }

    public void DalekKilled()
    {
        daleksKilled += 1;
        totalDaleksKilled += 1;
        daleksKilledText.text = daleksKilled.ToString();
        if (daleksKilled >= totalDaleks)
        {
            //call some method for when the last dalek is killed
        }
    }

    private void AssignAudio()
    {
        heartTimer = 0;
        breathTimer = 0;
        AudioSource[] toLoad = this.GetComponents<AudioSource>();
        heartBeat = toLoad[0];
        breath = toLoad[1];
        backgroundCalm = toLoad[2];
        backgroundEnergetic = toLoad[3];
        maleBreath = new AudioClip[]
        {
            Resources.Load<AudioClip>("Sounds/breath_male_1"),
            Resources.Load<AudioClip>("Sounds/breath_male_2"),
            Resources.Load<AudioClip>("Sounds/breath_male_3")
        };
        maleBreathFinal = Resources.Load<AudioClip>("Sounds/breath_male_final");
        femaleBreath = new AudioClip[]
        {
            Resources.Load<AudioClip>("Sounds/breath_female_1"),
            Resources.Load<AudioClip>("Sounds/breath_female_2"),
            Resources.Load<AudioClip>("Sounds/breath_female_3"),
            Resources.Load<AudioClip>("Sounds/breath_female_4")
        };
        femaleBreathFinal = Resources.Load<AudioClip>("Sounds/breath_female_final");
    }

    private void AssignHitIndicators()
    {
        hitLeft = hitIndicators[0];
        hitForward = hitIndicators[1];
        hitRight = hitIndicators[2];
        hitBehind = hitIndicators[3];

        hitLeftSprites = new Sprite[]
        {
            Resources.Load<Sprite>("Images/blood_left_1"),
            Resources.Load<Sprite>("Images/blood_left_2"),
            Resources.Load<Sprite>("Images/blood_left_3")
        };
        hitRightSprites = new Sprite[]
        {
            Resources.Load<Sprite>("Images/blood_right_1"),
            Resources.Load<Sprite>("Images/blood_right_2"),
            Resources.Load<Sprite>("Images/blood_right_3")
        };
        hitForwardSprites = new Sprite[]
        {
            Resources.Load<Sprite>("Images/blood_top_1"),
            Resources.Load<Sprite>("Images/blood_top_2"),
            Resources.Load<Sprite>("Images/blood_top_3")
        };
        hitBehindSprites = new Sprite[]
        {
            Resources.Load<Sprite>("Images/blood_bottom_1"),
            Resources.Load<Sprite>("Images/blood_bottom_2"),
            Resources.Load<Sprite>("Images/blood_bottom_3")
        };
    }

    private void SetUpStats()
    {
        health = maxHealth;
        healthBar.value = (health / maxHealth * 100);
        ammoReserve = 0;
        ammoGun = maxAmmoGun;
        daleksKilled = 0;
        invincible = false;
        ammoGunText.text = ammoGun.ToString();
        ammoReserveText.text = $"/{ammoReserve.ToString()}";
        daleksKilledText.text = daleksKilled.ToString();
        totalDaleksText.text = $"/{totalDaleks.ToString()}";
    }

    private void NewGameStats()
    {
        totalDaleksKilled = 0;
        totalDaleksKilledBeforeFloor = 0;
    }

    private void FlashDamage(Side side)
    {
        switch (side)
        {
            case Side.left:
                hitLeft.sprite = hitLeftSprites[UnityEngine.Random.Range(0, hitLeftSprites.Length - 1)];
                hitLeft.color = new Color(hitLeft.color.r, hitLeft.color.g, hitLeft.color.b, 1);
                if (!toFadeOut.Contains(hitLeft))
                {
                    toFadeOut.Add(hitLeft);
                }
                break;
            case Side.right:
                hitRight.sprite = hitRightSprites[UnityEngine.Random.Range(0, hitRightSprites.Length - 1)];
                hitRight.color = new Color(hitRight.color.r, hitRight.color.g, hitRight.color.b, 1);
                if (!toFadeOut.Contains(hitRight))
                {
                    toFadeOut.Add(hitRight);
                }
                break;
            case Side.forward:
                hitForward.sprite = hitForwardSprites[UnityEngine.Random.Range(0, hitForwardSprites.Length - 1)];
                hitForward.color = new Color(hitForward.color.r, hitForward.color.g, hitForward.color.b, 1);
                if (!toFadeOut.Contains(hitForward))
                {
                    toFadeOut.Add(hitForward);
                }
                break;
            case Side.behind:
                hitBehind.sprite = hitBehindSprites[UnityEngine.Random.Range(0, hitBehindSprites.Length - 1)];
                hitBehind.color = new Color(hitBehind.color.r, hitBehind.color.g, hitBehind.color.b, 1);
                if (!toFadeOut.Contains(hitBehind))
                {
                    toFadeOut.Add(hitBehind);
                }
                break;
            case Side.all:
            default:
                hitLeft.sprite = hitLeftSprites[UnityEngine.Random.Range(0, hitLeftSprites.Length - 1)];
                hitRight.sprite = hitRightSprites[UnityEngine.Random.Range(0, hitRightSprites.Length - 1)];
                hitForward.sprite = hitForwardSprites[UnityEngine.Random.Range(0, hitForwardSprites.Length - 1)];
                hitBehind.sprite = hitBehindSprites[UnityEngine.Random.Range(0, hitBehindSprites.Length - 1)];

                hitLeft.color = new Color(hitLeft.color.r, hitLeft.color.g, hitLeft.color.b, 1);
                hitRight.color = new Color(hitRight.color.r, hitRight.color.g, hitRight.color.b, 1);
                hitForward.color = new Color(hitForward.color.r, hitForward.color.g, hitForward.color.b, 1);
                hitBehind.color = new Color(hitBehind.color.r, hitBehind.color.g, hitBehind.color.b, 1);

                if (!toFadeOut.Contains(hitLeft))
                {
                    toFadeOut.Add(hitLeft);
                }
                if (!toFadeOut.Contains(hitRight))
                {
                    toFadeOut.Add(hitRight);
                }
                if (!toFadeOut.Contains(hitForward))
                {
                    toFadeOut.Add(hitForward);
                }
                if (!toFadeOut.Contains(hitBehind))
                {
                    toFadeOut.Add(hitBehind);
                }
                break;
        }
    }

    private void FadeHitIndicators(float deltaTime)
    {
        Image[] toLoop = new Image[toFadeOut.Count];
        toFadeOut.CopyTo(toLoop);
        foreach (var image in toLoop)
        {
            if (image.color.a > 0.0001f)
            {
                Color toSet = image.color;
                toSet.a = Mathf.Lerp(image.color.a, 0, fadeSpeed * deltaTime);
                image.color = toSet;
            }
            else
            {
                toFadeOut.Remove(image);
            }
        }
    }

    private void HeartBeat()
    {
        if (playBeat && !invincible)
        {
            if (heartTimer <= 0 && beatspm > 0)
            {
                heartBeat.Play();
                heartTimer = 60 / beatspm;
            }
            heartTimer -= Time.deltaTime;
        }
    }

    private void Breath()
    {
        if (playBreath && !invincible)
        {
            if (breathTimer <= 0 && breathspm > 0)
            {
                if (isMale)
                {
                    breath.clip = maleBreath[UnityEngine.Random.Range(0, maleBreath.Length - 1)];
                }
                else
                {
                    breath.clip = femaleBreath[UnityEngine.Random.Range(0, femaleBreath.Length - 1)];
                }

                breath.Play();
                breathTimer = 60 / breathspm;
            }
            breathTimer -= Time.deltaTime;
        }
    }

    public void NewGame()
    {
        currentFloor = 1;

        tunnelVisionTargetAlpha = 0;
        foreach (var image in toFadeOut)
        {
            Color toSet = image.color;
            toSet.a = 0;
            image.color = toSet;
        }
        toFadeOut.Clear();
        
        totalDaleks = GetNumberOfDaleks();

        SetUpStats();
        NewGameStats();
        SetBreath();
        SetHeartBeat();
        //Need to move player back to start position
    }

    public void RestartFloor()
    {
        tunnelVisionTargetAlpha = 0;
        foreach (var image in toFadeOut)
        {
            Color toSet = image.color;
            toSet.a = 0;
            image.color = toSet;
        }
        toFadeOut.Clear();

        SetUpStats();
        SetBreath();
        SetHeartBeat();
        totalDaleksKilled = totalDaleksKilledBeforeFloor;

        //Need to move player back to start position
    }

    public void NextFloor()
    {
        currentFloor += 1;
        totalDaleksKilledBeforeFloor = totalDaleksKilled;

        totalDaleks = GetNumberOfDaleks();
    }

    public void Pause()
    {
        invincible = true;
        PauseMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void Dead()
    {
        invincible = true;
        GameOverMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public int GetNumberOfDaleks()
    {
        //this should return the number of daleks generated for the current floor
        return 12;
    }

    public void SwitchMusic()
    {
        if (backgroundCalm.isPlaying)
        {
            backgroundEnergetic.Play();
            backgroundCalm.Stop();
        }
        else if (backgroundEnergetic.isPlaying)
        {
            backgroundCalm.Play();
            backgroundEnergetic.Stop();
        }
    }
}
