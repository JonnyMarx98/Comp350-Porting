using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargeting : MonoBehaviour
{
    float currentDistance;
    float largestDistance;
    Vector3 offset;
    [SerializeField]
    float smoothTime;
    [SerializeField]
    float minY;
    [SerializeField]
    float maxY;

    [SerializeField]
    Vector3 startPos;

    [SerializeField]
    float shakeTimer;
    [SerializeField]
    float shakeAmount;

    Vector3 velocity;

    List<Transform> objectsToTrack = new List<Transform>();

    void Start()
    {
        transform.position = startPos;
        GetTrack();
    }

    void Update()
    {
        if (shakeTimer >= 0)
        {
            Vector2 shakePos = Random.insideUnitCircle * shakeAmount;

            transform.position = new Vector3(transform.position.x + shakePos.x, transform.position.y, transform.position.z + shakePos.y);

            shakeTimer -= Time.deltaTime;
        }

        Vector3 position = Vector3.zero;

        foreach (Transform obj in objectsToTrack)
        {
            position += obj.position;
        }

        position /= GameManager.instance.players.Count;

        float distance = ReturnLargestDifference();

        offset.y = Mathf.Clamp(distance, minY, maxY);
        offset.z = -1 * (offset.y / 2.5f);

        transform.position = Vector3.SmoothDamp(transform.position, position + offset, ref velocity, smoothTime);
    }

    float ReturnLargestDifference()
    {
        currentDistance = 0f;
        largestDistance = 0f;

        for (int i = 0; i < GameManager.instance.players.Count; i++)
        {
            for (int x = 0; x < GameManager.instance.players.Count; x++)
            {
                currentDistance = Vector3.Distance(GameManager.instance.players[i].transform.position, GameManager.instance.players[x].transform.position);

                if (currentDistance > largestDistance)
                {
                    largestDistance = currentDistance;
                }
            }
        }

        return largestDistance;
    }

    public void ShakeCamera(float shakePower, float shakeDur)
    {
        shakeAmount = shakePower;
        shakeTimer = shakeDur;
    }

    public void GetTrack()
    {
        objectsToTrack.Clear();

        if (GameManager.instance.hockeyMode)
        {
            GameObject puck = GameObject.FindGameObjectWithTag("Puck");

            if (puck != null)
                objectsToTrack.Add(puck.transform);
        }

        foreach (GameObject player in GameManager.instance.players)
        {
            if (player.GetComponent<PlayerHealth>().alive == true)
                objectsToTrack.Add(player.transform);
        }
    }
}
