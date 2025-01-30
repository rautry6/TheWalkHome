using UnityEngine;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine.Rendering.Universal;
using System;
using System.Collections.Generic;

public class EvilGuy : MonoBehaviour
{

    Transform player;
    Vector2 velcocity;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidBody;

    public float MoveSpeed = 7f;
    Flashlight playerFlashlight;
    PlayerController playerController;

    Light2D[] lights;
    List<Light2D> spotLights = new List<Light2D>();

    private float lightSlowdown = 0.75f;
    private float fadeAwaySpeed = 3f;

    public AudioSource DeathSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("Player").transform;
        playerController = player.GetComponent<PlayerController>();
        playerFlashlight = player.GetComponent<Flashlight>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();

        lights = FindObjectsByType<Light2D>(FindObjectsSortMode.None);

        foreach(Light2D light in lights)
        {
            if(light.transform.parent != null && light.transform.parent.parent != null)
            {
                spotLights.Add(light);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(player.position.x > transform.position.x)
        {
            spriteRenderer.flipX = false;
            velcocity = new Vector2(MoveSpeed * 1, 0);
        }
        else
        {
            spriteRenderer.flipX = true;
            velcocity = new Vector2(MoveSpeed * -1, 0);
        }

        float value = spriteRenderer.color.a;


         // player flashlight check
        if (playerFlashlight.FlashlightActive)
        {
            if (playerController.FacingRight && player.position.x < transform.position.x)
            {
                if(Vector2.Distance(transform.position, playerFlashlight.RightFacingLight.transform.position)
                    < playerFlashlight.RightFacingLight.pointLightOuterRadius)
                {
                    velcocity = -velcocity * lightSlowdown;

                    value -= Time.deltaTime * fadeAwaySpeed;

                    if (!DeathSound.isPlaying)
                    {
                        DeathSound.Play();
                    }
                }
            }
            else if (!playerController.FacingRight && player.position.x > transform.position.x)
            {
                if (Vector2.Distance(transform.position, playerFlashlight.LeftFacingLight.transform.position)
                   < playerFlashlight.LeftFacingLight.pointLightOuterRadius)
                {
                    velcocity = -velcocity * lightSlowdown;

                    value -= Time.deltaTime * fadeAwaySpeed;

                    if (!DeathSound.isPlaying)
                    {
                        DeathSound.Play();
                    }
                }
            }
        }

        // other lights check
        foreach(var light in spotLights)
        {
            if (Vector2.Distance(transform.position, light.transform.position)
                   < light.pointLightOuterRadius - 1)
            {
                velcocity = -velcocity * lightSlowdown;

                value -= Time.deltaTime * fadeAwaySpeed;

                if (!DeathSound.isPlaying)
                {
                    DeathSound.Play();
                }
            }
        }


        rigidBody.linearVelocity = velcocity;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, value);

        if(value <= 0)
        {
            Destroy(gameObject);
        }
    }


}
