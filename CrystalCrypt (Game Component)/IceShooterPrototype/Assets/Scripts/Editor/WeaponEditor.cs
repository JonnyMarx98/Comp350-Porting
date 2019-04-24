using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
	private Weapon instance;

	private bool SetInstance()
	{
		instance = (Weapon)target;
		return instance;
	}

	public override void OnInspectorGUI()
	{
		if (instance == null & SetInstance() == false)
			return;

		DrawGeneralSettings();

		EditorGUILayout.BeginVertical("box");
		switch (instance.weaponType)
		{
			case Weapon.WeaponType.Gun:
				DrawGunSettings();
				break;
			case Weapon.WeaponType.Projectile:
				DrawProjectileSettings();
				break;
			default:
				GUILayout.Label("Working on it");
				break;
		}

		if (GUI.changed)
		{
			// writing changes of the testScriptable into Undo
			Undo.RecordObject(instance, "Weapon Scriptable Editor Modify");
			// mark the testScriptable object as "dirty" and save it
			EditorUtility.SetDirty(instance);
		}

		EditorGUILayout.EndVertical();
	}

	private void DrawGeneralSettings()
	{
		instance.weaponType = (Weapon.WeaponType)EditorGUILayout.EnumPopup("Weapon Type", instance.weaponType);

		EditorGUILayout.BeginVertical("box");
		GUILayout.Label("General Settings");

		instance.name = EditorGUILayout.TextField("Weapon Name", instance.name);
		instance.damage = EditorGUILayout.FloatField("Damage", instance.damage);
		instance.maxAmmo = EditorGUILayout.IntField("Max Ammo", instance.maxAmmo);

		EditorGUILayout.BeginHorizontal();
		instance.spawnChance = Mathf.Clamp(EditorGUILayout.FloatField("Spawn Chance", instance.spawnChance), 0, 1);
		instance.spawnChance = GUILayout.HorizontalSlider(instance.spawnChance, 0, 1);
		EditorGUILayout.EndHorizontal();

		instance.itemActivator = (ItemActivator)EditorGUILayout.EnumPopup("Item Activator", instance.itemActivator);
        instance.useAudio = (AudioClip)EditorGUILayout.ObjectField("Use Audio", instance.useAudio, typeof(AudioClip), false);

		EditorGUILayout.EndVertical();
	}

	private void DrawGunSettings()
	{
		GUILayout.Label("Gun Settings");
		instance.defaultWeapon = EditorGUILayout.Toggle("Default Weapon", instance.defaultWeapon);
		instance.shotCooldown = EditorGUILayout.FloatField("Shot Cooldown", instance.shotCooldown);
		instance.knockBackForce = EditorGUILayout.FloatField("Knockback Force", instance.knockBackForce);
		instance.hitImpactForce = EditorGUILayout.FloatField("Hit Impact Force", instance.hitImpactForce);
		instance.bulletVelocity = EditorGUILayout.FloatField("Bullet Velocity", instance.bulletVelocity);
		instance.bulletPrefab = (GameObject)EditorGUILayout.ObjectField("Bullet Prefab", instance.bulletPrefab, typeof(GameObject), false);
		instance.vibrateIntensity = EditorGUILayout.FloatField("Vibration Intensity", instance.vibrateIntensity);
		instance.vibrateDuration = EditorGUILayout.FloatField("Vibration Duration", instance.vibrateDuration);
		instance.fireRate = (Weapon.FireRate)EditorGUILayout.EnumPopup("Fire Rate", instance.fireRate);
		instance.maxCollisions = EditorGUILayout.IntField("Max Collisions", instance.maxCollisions);
	}

	private void DrawProjectileSettings()
	{
		GUILayout.Label("Projectile Settings");
		instance.areaOfEffect = EditorGUILayout.FloatField("Area of Effect", instance.areaOfEffect);
		instance.damageDropoff = EditorGUILayout.FloatField("Damage Dropoff", instance.damageDropoff);
		instance.explosionForce = EditorGUILayout.FloatField("Explosion Force", instance.explosionForce);
		instance.projectileVelocity = EditorGUILayout.FloatField("Projectile Velocity", instance.projectileVelocity);
		instance.projectilePrefab = (GameObject)EditorGUILayout.ObjectField("Projectile Prefab", instance.projectilePrefab, typeof(GameObject), false);
		instance.explosionPrefab = (GameObject)EditorGUILayout.ObjectField("Explosion Prefab", instance.explosionPrefab, typeof(GameObject), false);
        instance.explosionAudio = (AudioClip)EditorGUILayout.ObjectField("Explosion Audio", instance.explosionAudio, typeof(AudioClip), false);
    }
}
