using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    [SerializeField]
    float lookSensitivity = 3f;
    public int playerID;
    public int playerScore;
    [SerializeField]
    AudioSource source;
    [SerializeField]
    float skateAudioDelayMin, skateAudioDelayMax;
    [SerializeField]
    AudioClip[] skateClips;

    [SerializeField]
    bool skateAudioPlaying;

    Rewired.Player player;

    public PowerUp currentPowerUp;
    public PassivePowerUp currentPassivePowerUp;

    [SerializeField]
    float noPowerUpVibrate, powerUpVibrate, powerUpVibrateDuration;

    [HideInInspector]
    public bool charging;
    [HideInInspector]
    public float chargeVelocity, chargeForce, chargeDamage;

    [SerializeField]
    Color frozenMatColor;
    Color standardColor;
    Material gfxMat;
    bool frozen;
    [HideInInspector]
    public float thawTime;

    public Transform riotShieldPos;

    [SerializeField]
    GameObject crown;

    [SerializeField]
    float chargeVibrateIntensity, chargeVibrateDuration;

    UIManager uiManager;

    //Component Caching
    PlayerMotor motor;
    Rigidbody rb;

    public Image powerUpIcon;

    public float maxPowerUpTime;
    public float currentPowerUpTime;
	[SerializeField]
	AudioSource powerUpAudioSource;
	[SerializeField]
	AudioClip frozenAudio;

    void Awake()
    {
        crown.SetActive(false);
    }

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        rb = GetComponent<Rigidbody>();
        SetNameAndMat();
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        gfxMat = gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material;
        standardColor = gfxMat.color;
        powerUpIcon.enabled = false;
    }

    void Update()
    {
        if (!charging && !frozen)
        {
            //Calculate movement velocity as a 3D vector
            float _xMov = CrossPlatformInputManager.GetAxis("MoveHorizontal");
            float _zMov = CrossPlatformInputManager.GetAxis("MoveVertical");
            Vector3 _movHorizontal = transform.right * _xMov;
            Vector3 _movVertical = transform.forward * _zMov;

            //Final movement vector
            Vector3 _velocity = (_movHorizontal + _movVertical) * speed;

            //Apply movement
            motor.Move(_velocity);

            if (_xMov != 0 && !skateAudioPlaying || _zMov != 0 && !skateAudioPlaying)
            {
                skateAudioPlaying = true;
                InvokeRepeating("SkateNoise", 0f, Random.Range(skateAudioDelayMin, skateAudioDelayMax));
            }
            else if (_zMov == 0 || _xMov == 0)
            {
                skateAudioPlaying = false;
                CancelInvoke("SkateNoise");
            }

            float _xRot = CrossPlatformInputManager.GetAxis("LookHorizontal");
            float _zRot = CrossPlatformInputManager.GetAxis("LookVertical");

            if (_xRot != 0 || _zRot != 0)
            {
                motor.Rotate(_xRot, _zRot);
            }
        }
        else if (charging)
        {
            rb.velocity = motor.gfx.transform.forward * chargeVelocity;
        }

        if (player.GetButtonDown("PowerUp"))
        {
            if (currentPowerUp == null)
            {
                ControllerVibrate(noPowerUpVibrate, powerUpVibrateDuration);
                return;
            }

            ControllerVibrate(powerUpVibrate, powerUpVibrateDuration);
            currentPowerUp.playerID = playerID;
            currentPowerUp.playerPos = transform.position;
            currentPowerUp.playerPos.y += 1f;
			currentPowerUp.powerUpAudioSource = powerUpAudioSource;
            currentPowerUp.ActivatePowerUp();
            uiManager.HidePowerUp(playerID);
            StartCoroutine("DeactivatePowerUp");
        }

        if (currentPassivePowerUp && !currentPowerUp)
        {
            IconDecrease();
        }

        CheckScore();
    }

    public void SetNameAndMat()
    {
        player = ReInput.players.GetPlayer(playerID);
        gameObject.name = "player" + playerID + "(" + name + ")";
    }

    public void ControllerVibrate(float _intensity, float _duration)
    {
        player.SetVibration(0, _intensity, _duration);
    }

    void SkateNoise()
    {
        int rand = Random.Range(0, skateClips.Length);
        source.clip = skateClips[rand];
        source.Play();
    }

    IEnumerator DeactivatePowerUp()
    {
        yield return new WaitForSeconds(currentPowerUp.timeActive);
        if (currentPowerUp != null)
        {
            currentPowerUp.DeactivatePowerUp();
            currentPowerUp = null;
            powerUpIcon.enabled = false;
        }
    }

    public IEnumerator DeactivatePassivePowerUp()
    {
        yield return new WaitForSeconds(currentPassivePowerUp.timeActive);

        if (currentPassivePowerUp != null)
        {
            currentPassivePowerUp.DeactivatePowerUp();
            currentPassivePowerUp = null;
            powerUpIcon.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (charging)
        {
            if (collision.gameObject.tag == "Player")
            {
                charging = false;
                rb.velocity = Vector3.zero;
                Vector3 force = motor.gfx.transform.forward * chargeForce;
                collision.gameObject.GetComponent<Rigidbody>().AddForce(force);
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(chargeDamage, Vector3.zero, playerID);
                ControllerVibrate(chargeVibrateIntensity, chargeVibrateDuration);
                GameManager.instance.cam.GetComponent<CameraTargeting>().ShakeCamera(.3f, .2f);
            }

            if (collision.gameObject.tag != "Floor")
            {
                charging = false;
                GetComponent<PlayerHealth>().TakeDamage(chargeDamage, Vector3.zero, playerID);
                ControllerVibrate(chargeVibrateIntensity, chargeVibrateDuration);
            }
        }
    }

    public void Freeze()
    {
        frozen = true;
        gfxMat.color = frozenMatColor;
        GetComponent<Shooting>().enabled = false;
        StartCoroutine("Thaw");
		GameManager.instance.PlaySFX(frozenAudio);
    }

    IEnumerator Thaw()
    {
        yield return new WaitForSeconds(thawTime);
        frozen = false;
        gfxMat.color = standardColor;
        GetComponent<Shooting>().enabled = true;
    }

    void CheckScore()
    {
        if (GetComponent<PlayerHealth>().alive)
        {
            if (GameManager.instance.CheckHighestScore(playerID))
            {
                crown.SetActive(true);
            }
            else if (!GameManager.instance.CheckHighestScore(playerID))
            {
                crown.SetActive(false);
            }
        }
        else
        {
            crown.SetActive(false);
        }
    }

    void IconDecrease()
    {
        currentPowerUpTime -= Time.deltaTime;
        powerUpIcon.fillAmount = currentPowerUpTime / maxPowerUpTime;
    }
}
