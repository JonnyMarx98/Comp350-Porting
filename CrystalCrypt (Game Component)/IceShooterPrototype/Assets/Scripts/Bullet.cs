using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public int playerID;
    public float forceAmount;
    public int maxCollisions;
    public bool homing;
    public bool freeze;

    private int currentCollisions;
    [SerializeField]
    List<AudioClip> reboundClips;

    AudioSource source;

    int playerIDToTrack;
    GameObject playerToTrack;
    Vector3 nonHomingVelocity;
    float homingVelocity;
    [SerializeField]
    float homingRadius;
    bool currentlyHoming;
    float smoothTime = 0.2f;

    Rigidbody rb;

    Vector3 velocity;

    [SerializeField]
    Color frozenBulletColor;
    Material mat;
    [HideInInspector]
    public float thawTime;

    void Start()
    {
        if (freeze)
        {
            foreach (Transform t in gameObject.transform)
            {
                if (t.GetComponent<MeshRenderer>())
                {
                    t.GetComponent<MeshRenderer>().material.color = frozenBulletColor;
                }
            }
        }

        source = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        currentCollisions = 0;

        if (homing)
        {
            homingVelocity = rb.velocity.magnitude / 2;
            nonHomingVelocity = rb.velocity / 2.5f;
        }
    }

    private void LateUpdate()
    {
        if (homing)
        {
            if (playerToTrack != null && !playerToTrack.GetComponent<PlayerHealth>().alive)
            {
                playerToTrack = null;
            }

            if (playerToTrack == null)
            {
                rb.velocity = nonHomingVelocity;
                playerToTrack = CheckForNearPlayer();
            }

            if (playerToTrack)
            {
                Vector3 dir = (playerToTrack.transform.position - transform.position).normalized * homingVelocity;
                rb.velocity = Vector3.SmoothDamp(rb.velocity, dir, ref velocity, smoothTime / 2);
            }
        }
    }

    void FindNearestPlayer()
    {
        List<GameObject> activePlayers = new List<GameObject>();

        for (int i = 0; i < GameManager.instance.players.Count; i++)
        {
            if (GameManager.instance.players[i].GetComponent<PlayerController>().playerID != playerID)
            {
                activePlayers.Add(GameManager.instance.players[i]);
            }
        }

        float shortestDistance = 5000;
        playerIDToTrack = playerID;

        for (int i = 0; i < activePlayers.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, activePlayers[i].transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                playerIDToTrack = activePlayers[i].GetComponent<PlayerController>().playerID;
            }
        }

        playerToTrack = GameManager.instance.RetrievePlayer(playerIDToTrack);
    }

    GameObject CheckForNearPlayer()
    {
        GameObject objectToTrack = null;

        List<GameObject> activePlayers = new List<GameObject>();

        for (int i = 0; i < GameManager.instance.players.Count; i++)
        {
            if (GameManager.instance.players[i].GetComponent<PlayerController>().playerID != playerID)
            {
                activePlayers.Add(GameManager.instance.players[i]);
            }
        }

        RaycastHit hit;

        if (Physics.SphereCast(transform.position, homingRadius, transform.forward, out hit))
        {
            for (int i = 0; i < activePlayers.Count; i++)
            {
                if (hit.transform.root.gameObject.GetComponent<PlayerController>())
                {
                    if (hit.transform.root.gameObject.GetComponent<PlayerController>().playerID == activePlayers[i].GetComponent<PlayerController>().playerID)
                    {
                        objectToTrack = hit.transform.gameObject;
                    }
                }
            }
        }

        return objectToTrack;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet" || other.gameObject.tag == "PickUp" || other.gameObject.tag == "PickUpSpawn")
            return;

        Rigidbody rb = other.gameObject.GetComponentInParent<Rigidbody>();

        if (rb != null)
        {
            ApplyForce(rb);
        }

        if (other.gameObject.tag == "Puck")
        {
            Destroy(gameObject);
        }

        if (other.gameObject.tag == "Player")
        {
            if (freeze)
            {
                PlayerController playerHit = other.gameObject.GetComponent<PlayerController>();
                playerHit.thawTime = thawTime;
                playerHit.Freeze();
            }
            DamagePlayer(other.gameObject, other.contacts[0].point);
            Destroy(gameObject);
            return;
        }

        if (other.gameObject.tag == "Zombie")
        {
            DamageEnemy(other.gameObject);
            //Destroy(other.gameObject);
            Destroy(gameObject);
            return;
        }

        currentCollisions++;
        CheckCollisions();
        PlayReboundAudio();
    }

    void DamagePlayer(GameObject _obj, Vector3 _contactPoint)
    {
        PlayerHealth playerHealth = _obj.GetComponentInParent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage, _contactPoint, playerID);
        }
    }

    void DamageEnemy(GameObject _obj)
    {
        ZombieHealth zombieHealth = _obj.GetComponentInParent<ZombieHealth>();

        if (zombieHealth != null)
        {
            zombieHealth.TakeDamage(damage * 2.5f);

            if (zombieHealth.health <= 0f)
            {
                for (int i = 0; i < GameManager.instance.players.Count; i++)
                {
                    if (GameManager.instance.players[i].GetComponent<PlayerController>() != null)
                    {
                        if (GameManager.instance.players[i].GetComponent<PlayerController>().playerID == playerID)
                        {
                            //print("player " + i + " killed enemy");
                            GameManager.instance.players[i].GetComponent<PlayerController>().playerScore += 100;
                            GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>().UpdateScore(playerID);
                            //print(GameManager.instance.players[i].GetComponent<PlayerController>().playerScore);
                            if (_obj.GetComponentInParent<PlayerController>().playerID == playerID)
                            {
                                return;
                            }
                        }
                    }
                }
            }
        }
    }

    void CheckCollisions()
    {
        if (currentCollisions >= maxCollisions)
        {
            Destroy(gameObject);
        }
    }

    void ApplyForce(Rigidbody _rb)
    {
        _rb.AddForce(transform.forward * forceAmount);
    }

    void PlayReboundAudio()
    {
        if (currentCollisions < maxCollisions)
        {
            if (source != null)
            {
                int index = Random.Range(0, reboundClips.Count);

                source.clip = reboundClips[index];
                source.Play();
            }
        }
    }
}
