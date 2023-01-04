using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowFloor : MonoBehaviour
{
    [SerializeField] private Transform Player;
    [SerializeField] private Material material;
    public float offsetSpeed = 0.25f;

    void Update()
    {
        transform.position = new Vector3(0, 0, Player.transform.position.z);
        material.SetVector("_offset", new Vector2(0, -transform.position.z * offsetSpeed));
    }
}
