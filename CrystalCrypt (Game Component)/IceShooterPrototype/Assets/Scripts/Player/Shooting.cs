using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class ShootingForAI : MonoBehaviour
{
    public int remainingAmmo;
    [SerializeField]
    float defaultReloadTime;

    //float weaponTimer;
    float shotTimer;

    public Weapon currentWeapon;
    [SerializeField]
    Weapon defaultWeapon;

    bool reloading;

    [SerializeField]
    Transform bulletSpawnPos;

    [SerializeField]
    AudioClip freezeShot;
    [SerializeField]
    AudioSource source;

    PlayerMotor motor;
    PlayerController controller;

    UIManager uiManager;

    Rewired.Player player;

    [SerializeField]
    public ReloadBar reloadBar;

    [HideInInspector]
    public bool homing;
    [HideInInspector]
    public bool riotShield;
    public GameObject riotShieldObj;

    [HideInInspector]
    public bool freeze;
    [HideInInspector]
    public float thawTime;

    [SerializeField]
    GameObject puckPistol;
    [SerializeField]
    GameObject smg;
    [SerializeField]
    GameObject rifle;
    [SerializeField]
    GameObject shotgun;
    [SerializeField]
    GameObject grenade;

    [SerializeField]
    ParticleSystem weaponChangeSmoke;

    private void Awake()
    {
        riotShieldObj.SetActive(false);
    }

    void Start()
    {
        motor = GetComponent<PlayerMotor>();
        controller = GetComponent<PlayerController>();
        EquipDefaultWeapon();
        player = ReInput.players.GetPlayer(controller.playerID);
        shotTimer = currentWeapon.shotCooldown;
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        reloadBar = GetComponentInChildren<ReloadBar>();
        InitialiseAmmoUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!riotShield)
        {
            shotTimer += Time.deltaTime;

            if (currentWeapon.weaponType == Weapon.WeaponType.Gun)
            {

                if (currentWeapon.fireRate == Weapon.FireRate.Automatic)
                {
                    //shoot every x seconds
                    if (player.GetButtonDown("Fire"))
                    {
                        InvokeRepeating("AutomaticFire", 0f, currentWeapon.shotCooldown);
                    }
                    else if (player.GetButtonUp("Fire"))
                    {
                        CancelInvoke();
                    }
                }
                else
                {
                    if (player.GetButtonDown("Fire"))
                    {
                        Fire();
                    }
                }

                if (player.GetButtonDown("Reload"))
                {
                    if (currentWeapon.defaultWeapon && remainingAmmo < currentWeapon.maxAmmo)
                    {
                        remainingAmmo = 0;
                    }
                }

                if (reloading)
                {
                    reloadBar.ReloadBarFill(defaultReloadTime);
                }

            }
            else if (currentWeapon.weaponType == Weapon.WeaponType.Projectile)
            {
                if (player.GetButtonDown("Fire"))
                {
                    PlayGunShot();
                    LaunchProjectile();
                }
            }
            CheckAmmo();
        }
    }

    public void UpdateWeapon(Weapon _weapon)
    {
        ChangeWeaponModel(CheckCurrentWeapon(_weapon));
        //adjust player weapon stats to that of new equipped weapon
        currentWeapon = _weapon;
        remainingAmmo = currentWeapon.maxAmmo;
        source.clip = currentWeapon.useAudio;
        //weaponTimer = currentWeapon.timeActive;
        shotTimer = currentWeapon.shotCooldown;
        CancelInvoke();
        reloadBar.HideBarImmediately();
    }

    public void EquipDefaultWeapon()
    {
        //adjust player weapon stats to stats of default weapon
        currentWeapon = defaultWeapon;
        UpdateWeapon(currentWeapon);

    }

    void Fire()
    {
        //check to see if shot cooldown has passed
        if (shotTimer >= currentWeapon.shotCooldown && remainingAmmo > 0)
        {
            SpawnBullet();
            controller.ControllerVibrate(currentWeapon.vibrateIntensity, currentWeapon.vibrateDuration);
            motor.ApplyKnockback(currentWeapon.knockBackForce);
            shotTimer = 0f;

            if (freeze)
            {
                PlayFreezeShot();
            }
            else if (homing)
            {
                PlayGunShot();
            }
            else
            {
                PlayGunShot();
            }
            remainingAmmo -= 1;
            uiManager.UpdateAmmo(controller.playerID);
        }
    }

    void AutomaticFire()
    {
        if (remainingAmmo > 0)
        {
            SpawnBullet();
            PlayGunShot();
            controller.ControllerVibrate(currentWeapon.vibrateIntensity, currentWeapon.vibrateDuration);
            motor.ApplyKnockback(currentWeapon.knockBackForce);
            remainingAmmo -= 1;
            uiManager.UpdateAmmo(controller.playerID);
        }
    }

    void LaunchProjectile()
    {
        remainingAmmo -= 1;
        uiManager.UpdateAmmo(controller.playerID);

        SpawnProjectile();
    }

    void SpawnBullet()
    {
        if (currentWeapon.fireRate != Weapon.FireRate.Spread)
        {
            GameObject bulletIns = Instantiate(currentWeapon.bulletPrefab, bulletSpawnPos.position, Quaternion.identity);

            Bullet bulletScript = bulletIns.GetComponent<Bullet>();

            if (homing)
                bulletScript.homing = true;

            if (freeze)
            {
                bulletScript.thawTime = thawTime;
                bulletScript.freeze = true;
            }

            bulletIns.GetComponent<Rigidbody>().velocity = motor.gfx.transform.forward * currentWeapon.bulletVelocity;
            bulletScript.damage = currentWeapon.damage;
            bulletScript.playerID = controller.playerID;
            bulletScript.forceAmount = currentWeapon.hitImpactForce;
            bulletScript.maxCollisions = currentWeapon.maxCollisions;
        }
        else if (currentWeapon.fireRate == Weapon.FireRate.Spread)
        {
            GameObject bulletIns = Instantiate(currentWeapon.bulletPrefab, bulletSpawnPos.position, bulletSpawnPos.transform.rotation);

            Bullet[] bulletScripts = bulletIns.GetComponentsInChildren<Bullet>();
            Rigidbody[] rbs = bulletIns.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody rb in rbs)
            {
                rb.velocity = rb.transform.forward * currentWeapon.bulletVelocity;
            }

            foreach (Bullet bullet in bulletScripts)
            {
                bullet.damage = currentWeapon.damage;
                bullet.playerID = controller.playerID;
                bullet.forceAmount = currentWeapon.hitImpactForce;
                if (homing)
                    bullet.homing = true;

                if (freeze)
                {
                    bullet.thawTime = thawTime;
                    bullet.freeze = true;
                }
            }
        }
    }

    void SpawnProjectile()
    {
        GameObject projectileIns = Instantiate(currentWeapon.projectilePrefab, bulletSpawnPos.position, Quaternion.identity);

        projectileIns.GetComponent<Rigidbody>().velocity = motor.gfx.transform.forward * currentWeapon.projectileVelocity;
        projectileIns.GetComponent<Projectile>().damage = currentWeapon.damage;
        projectileIns.GetComponent<Projectile>().damageDropoff = currentWeapon.damageDropoff;
        projectileIns.GetComponent<Projectile>().areaOfEffect = currentWeapon.areaOfEffect;
        projectileIns.GetComponent<Projectile>().forceAmount = currentWeapon.explosionForce;
        projectileIns.GetComponent<Projectile>().explosionPrefab = currentWeapon.explosionPrefab;
        projectileIns.GetComponent<Projectile>().playerID = controller.playerID;
    }

    void CheckAmmo()
    {
        if (remainingAmmo <= 0f)
        {
            if (currentWeapon.defaultWeapon)
            {
                if (!reloading)
                {
                    StartCoroutine("Reload");
                }
                else
                {
                    return;
                }
            }
            else
            {
                EquipDefaultWeapon();
                uiManager.UpdateAmmo(controller.playerID);
            }
        }
    }

    IEnumerator Reload()
    {
        reloading = true;
        reloadBar.ReloadBarEmpty();
        yield return new WaitForSeconds(defaultReloadTime);
        remainingAmmo = currentWeapon.maxAmmo;
        uiManager.UpdateAmmo(controller.playerID);
        reloading = false;
    }

    void InitialiseAmmoUI()
    {
        uiManager.UpdateAmmo(0);
        uiManager.UpdateAmmo(1);
        uiManager.UpdateAmmo(2);
        uiManager.UpdateAmmo(3);
    }

    void PlayGunShot()
    {
        source.Play();
    }

    void PlayFreezeShot()
    {
        source.clip = freezeShot;
        source.Play();
    }

    void ChangeWeaponModel(GameObject _model)
    {
        weaponChangeSmoke.Play();

        puckPistol.SetActive(false);
        shotgun.SetActive(false);
        grenade.SetActive(false);
        smg.SetActive(false);
        rifle.SetActive(false);

        _model.SetActive(true);
    }

    GameObject CheckCurrentWeapon(Weapon _weapon)
    {
        GameObject currentWeap = null;

        if (_weapon == null)
        {
            return puckPistol;
        }

        if (_weapon.name == "Puck Pistol")
        {
            return puckPistol;
        }
        else if (_weapon.name == "SMG")
        {
            return smg;
        }
        else if (_weapon.name == "Shotgun")
        {
            return shotgun;
        }
        else if (_weapon.name == "Sniper")
        {
            return rifle;
        }
        else if (_weapon.name == "Grenade")
        {
            return grenade;
        }

        return currentWeap;
    }
}
