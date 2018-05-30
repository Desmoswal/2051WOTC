﻿using UnityEngine;

[System.Serializable]
public class PlayerWeapon {

    public string name = "AKM";

    public int damage = 10;
    public float range = 100f;

    public float fireRate = 0f;

    public int maxBullets = 30;
    [HideInInspector]
    public int bullets;

    public float reloadTime = 3f;

    public GameObject graphics;

    public PlayerWeapon()
    {
        bullets = maxBullets;
    }

    public AudioSource audioSource;
}