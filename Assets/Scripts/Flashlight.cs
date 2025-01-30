using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Flashlight : MonoBehaviour
{
    [Header("Flashlights")]
    public Light2D RightFacingLight;
    public Light2D LeftFacingLight;

    PlayerController playerController;

    public bool FlashlightActive = false;

    public float FlashlightEnergy = 100f;

    private float flashlightDrain = 5f;
    private bool flickered = false;

    private bool inPole = false;

    public AudioSource Click;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(FlashlightActive)
        {
            FlashlightEnergy -= flashlightDrain * Time.deltaTime;

            if(!flickered && FlashlightEnergy < 20)
            {
                flickered = true;
                StartCoroutine(Flicker());
            }

            if(FlashlightEnergy <= 0)
            {
                DisableFlashlight();
            }
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        if(inPole && Input.GetKeyDown(KeyCode.Space))
        {
            RechargeFlashlight();
        }
    }

    public void DisableFlashlight()
    {
        FlashlightActive = false;

        RightFacingLight.pointLightOuterRadius = 0;
        LeftFacingLight.pointLightOuterRadius = 0;
    }

    public void ToggleFlashlight()
    {
        if(FlashlightEnergy <= 0)
        {
            return;
        }

        Click.Play();

        if (!FlashlightActive)
        {
            FlashlightActive = true;

            if (playerController.FacingRight)
            {
                RightFacingLight.pointLightOuterRadius = 2.58f;
                LeftFacingLight.pointLightOuterRadius = 0;
            }
            else
            {
                RightFacingLight.pointLightOuterRadius = 0;
                LeftFacingLight.pointLightOuterRadius = 2.58f;
            }
        }
        else
        {
            FlashlightActive = false;

            RightFacingLight.pointLightOuterRadius = 0;
            LeftFacingLight.pointLightOuterRadius = 0;
        }
    }

    public void DirectionChange()
    {
        if(FlashlightActive)
        {
            if (playerController.FacingRight)
            {
                RightFacingLight.pointLightOuterRadius = 2.58f;
                LeftFacingLight.pointLightOuterRadius = 0;
            }
            else
            {
                RightFacingLight.pointLightOuterRadius = 0;
                LeftFacingLight.pointLightOuterRadius = 2.58f;
            }
        }
    }

    private IEnumerator Flicker()
    {
        for(int i = 0; i < 4; i++)
        {
            RightFacingLight.intensity = 0;
            LeftFacingLight.intensity = 0;

            yield return new WaitForSeconds(0.2f);

            RightFacingLight.intensity = 1;
            LeftFacingLight.intensity = 1;

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pole"))
        {
            inPole = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Pole"))
        {
            inPole = false;
        }
    }

    public void RechargeFlashlight()
    {
        DisableFlashlight();

        playerController.StopMovement();

        StartCoroutine(FlashlightShake());
    }


    private IEnumerator FlashlightShake()
    {
        playerController.PlayShakeAnim();

        yield return new WaitForSeconds(1f);

        FlashlightEnergy = 100;
        flickered = false;

        playerController.EnableMovement();
    }
}
