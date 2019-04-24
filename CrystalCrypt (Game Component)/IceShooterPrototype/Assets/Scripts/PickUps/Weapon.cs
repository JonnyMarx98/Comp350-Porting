using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Pick Ups/Weapons")]
public class Weapon : ScriptableObject
{
	[Header ("Generic Weapon Attributes")]
	[Tooltip ("The name of the weapon in-game")]
	public string name;
	[Tooltip("The type of weapon")]
	public WeaponType weaponType;
	[Tooltip("Damage")]
	public float damage;
	[Tooltip("Maximum ammo when the weapon is picked up")]
	public int maxAmmo;
	[Tooltip("Chance for the weapon to spawn, higher this number; the higher the chance of spawning")]
	[Range(0, 1)]
	public float spawnChance;
	[Tooltip("Button pressed to activate weapon")]
	public ItemActivator itemActivator;
    [Tooltip("Audio Clip played upon using weapon")]
    public AudioClip useAudio;

	[Header("Gun Attributes")]
	[Tooltip("Is this weapon the default? (Can only be checked on ONE weapon)")]
	public bool defaultWeapon;
	[Tooltip ("Amount of time you must wait between each shot")]
	public float shotCooldown;
	[Tooltip ("Force applied to player who shoots gun upon firing")]
	public float knockBackForce;
	[Tooltip ("Force applied to Rigidbody hit by bullet")]
	public float hitImpactForce;
	[Tooltip ("How fast the bullet travels")]
	public float bulletVelocity;
	[Tooltip ("The bullet prefab which will be fired by the gun")]
	public GameObject bulletPrefab;
	[Tooltip ("How intense the controller vibration is for shooter")]
	public float vibrateIntensity;
	[Tooltip ("How long the vibration lasts for")]
	public float vibrateDuration;	
	[Tooltip ("The fire rate of the gun (Spread is for shotguns)")]
	public FireRate fireRate;
	[Tooltip("The amount of times the bullet will collide with non-player objects before destroying")]
	public int maxCollisions;


	[Header("Projectile Attributes")]
	[Tooltip ("The diameter of the circle of damage")]
	public float areaOfEffect;
	[Tooltip("How much damage/force decreases over distance from center of impact")]
	public float damageDropoff;
	[Tooltip("Force applied to players hit by explosion")]
	public float explosionForce;
	[Tooltip("Velocity of projectile")]
	public float projectileVelocity;
	[Tooltip("Prefab of projectile")]
	public GameObject projectilePrefab;
	[Tooltip("Prefab of the explosion particle system")]
	public GameObject explosionPrefab;
    [Tooltip("Audio Clip to be played on explosion")]
    public AudioClip explosionAudio;

	[Tooltip ("How long the player can carry the gun for")]
	public float timeActive;

	public enum FireRate
	{
		SemiAutomatic,
		Spread,
		Automatic
	};

	public enum WeaponType
	{
		Gun,
		Projectile
	};
}
