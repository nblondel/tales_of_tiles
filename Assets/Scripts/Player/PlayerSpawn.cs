using System;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour {
    private void Start() {
        GameObject.FindGameObjectWithTag("Player").transform.position = transform.position;
    }
}