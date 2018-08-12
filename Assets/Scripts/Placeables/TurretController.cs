using System.Collections;
using Resources;
using UnityEngine;

public class TurretController : Placeable, EnergySink
{
#pragma warning disable 0649
    [SerializeField]
	[Tooltip("The barrel of the turret. Should be preconfigured in the Prefab")]
	private GameObject barrel;

	[SerializeField]
	[Tooltip("The bullet Prefab")]
	private GameObject bulletPrefab;

	[SerializeField]
	[Tooltip("The bullet spawn point. Should be preconfigured in the Prefab")]
	private GameObject bulletSpawn;

	[SerializeField]
	[Tooltip("The frequency of bullets shot")]
	private float bulletFrequency;

	[SerializeField]
	[Tooltip("The energy needed to shoot a bullet")]
	private Energy energyPerBullet;
	
	[SerializeField]
	[Tooltip("Standby energy per second")]
	private Energy standbyEnergy;

	[SerializeField]
	[Tooltip("For testing only")]
	private GameObject dummyTarget;
	
	[Tooltip("How much will the sound pitch vary from instance to instance")]
	public float pitchRange = 0.2f;
#pragma warning restore 0649

	
    private float currentAngle;
	private ResourceManager resourceManager;

	private bool hasEnergy;

	private AudioSource audioSource;
	private float originalPitch;

	protected override void Start()
	{
		base.Start();

		resourceManager = FindObjectOfType<ResourceManager>();
		resourceManager.AddSink(this);

		if (dummyTarget != null)
		{
			AimAt(dummyTarget);
		}
		
		audioSource = GetComponent<AudioSource>();
		audioSource.volume = 0.2f;
		audioSource.loop = true;
		audioSource.playOnAwake = false;
		originalPitch = audioSource.pitch;
		audioSource.pitch = UnityEngine.Random.Range(originalPitch - pitchRange, originalPitch + pitchRange);

	}

	public void AimAt(GameObject target)
	{
		StartCoroutine(FireAt(target));
	}

	private IEnumerator FireAt(GameObject target)
	{
		var waitTime = 1 / bulletFrequency;
		
		// ReSharper disable once LoopVariableIsNeverChangedInsideLoop
		while (target != null)
		{
			var fire = true;
			var angleDiff = Vector2.SignedAngle(-barrel.transform.right, target.transform.position - transform.position);
			if (currentAngle + angleDiff > 0)
			{
				fire = false;
				angleDiff = -currentAngle;
			}
			else if (currentAngle + angleDiff < -180)
			{
				fire = false;
				angleDiff = -180 - currentAngle;
			}
			
			barrel.transform.RotateAround(transform.position, Vector3.forward, angleDiff);
			currentAngle += angleDiff;
			
			if (fire && waitTime <= 0 && hasEnergy && resourceManager.TryConsume(energyPerBullet))
			{
				waitTime = 1 / bulletFrequency;
				var bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, Quaternion.identity);
				bullet.GetComponent<Rigidbody2D>().AddForce(-bulletSpawn.transform.right * 10);
				audioSource.Play();
			}
			else
			{
				waitTime -= Time.deltaTime;
			}
			
			yield return null;
		}
	}

	public void ConsumeEnergy(ResourceManager manager)
	{
		hasEnergy = manager.TryConsume(standbyEnergy * Time.fixedDeltaTime);
	}
}
