using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class Gun : MonoBehaviour
{
    public GameObject gun;

    [SerializeField]
    public float damage = 10f;
    [SerializeField]
    public float range = 100f;
    [SerializeField]
    public int currentAmmo = 12;
    [SerializeField]
    public int reserveAmmo = 60;
    [SerializeField]
    public int maxReserveAmmo = 60;

    public static int staticMaxReserverAmmo;
    public static int staticReserveAmmo;

    public ParticleSystem muzzleFlash;
    public Camera fpsCam;

    public TMP_Text currentAmmoText;
    public TMP_Text reserveAmmoText;

    [SerializeField]
    public int maxMagSize = 12;

    [SerializeField]
    public float reloadTime = 1f;
    [SerializeField]
    public float recoilTime = .07f;

    [SerializeField]
    public bool isAuto = false;
    [SerializeField]
    public float fireCooldown = 2f;
    [SerializeField]
    public float currentCooldown = 0f;

    public static bool isReloading = false;

    [SerializeField]
    public static bool isOwned = false;

    //public GameObject impactEffect;
    //public AudioSource shootSound;
    // Update is called once per frame

    private void Start()
    {
        reserveAmmo = maxReserveAmmo;
        staticMaxReserverAmmo = maxReserveAmmo;
        staticReserveAmmo = reserveAmmo;
    }

    void Update()
    {
        currentAmmoText.text = currentAmmo.ToString();
        reserveAmmoText.text = reserveAmmo.ToString();

        if (isAuto)
        {
            if(Input.GetButton("Fire1") && currentAmmo > 0 && isReloading == false)
            {
                if (currentCooldown <= 0f)
                {
                    Shoot();
                    currentCooldown = fireCooldown;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.R) && reserveAmmo > 0 && currentAmmo < maxMagSize)
                {
                    StartCoroutine(StartReload());
                    int addedAmmo = maxMagSize - currentAmmo;
                    if (reserveAmmo >= addedAmmo)
                    {
                        currentAmmo += addedAmmo;
                        reserveAmmo -= addedAmmo;
                    }
                    else
                    {
                        currentAmmo += reserveAmmo;
                        reserveAmmo = 0;
                    }
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && currentAmmo > 0 && isReloading == false)
            {
                if(currentCooldown <= 0f)
                {
                    Shoot();
                    currentCooldown = fireCooldown;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.R) && reserveAmmo > 0 && currentAmmo < maxMagSize)
                {
                    StartCoroutine(StartReload());
                    int addedAmmo = maxMagSize - currentAmmo;
                    if (reserveAmmo >= addedAmmo)
                    {
                        currentAmmo += addedAmmo;
                        reserveAmmo -= addedAmmo;
                    }
                    else
                    {
                        currentAmmo += reserveAmmo;
                        reserveAmmo = 0;
                    }
                }
            }
        }

        currentCooldown -= Time.deltaTime;

        /*
        if (Input.GetButton("Fire1") && currentAmmo > 0 && isReloading == false)
        {
            Shoot();
        }
        */

        /*
        if (Input.GetKeyDown(KeyCode.R) && reserveAmmo > 0 && currentAmmo < maxMagSize)
        {
            StartCoroutine(StartReload());
            int addedAmmo = maxMagSize - currentAmmo;
            if (reserveAmmo >= addedAmmo)
            {
                currentAmmo += addedAmmo;
                reserveAmmo -= addedAmmo;
            }
            else
            {
                currentAmmo += reserveAmmo;
                reserveAmmo = 0;
            }
        }
        */

    }

    void Shoot()
    {
        muzzleFlash.Play();
        //shootSound.Play();
        StartCoroutine(StartRecoil());
        RaycastHit hit;
        currentAmmo--;
        //Debug.Log(currentAmmo);
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            //Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            //GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            //Destroy(impactGO, 2f);
        }
    }

    IEnumerator StartRecoil()
    { 
        gun.GetComponent<Animator>().Play("Recoil");
        yield return new WaitForSeconds(recoilTime);
        gun.GetComponent<Animator>().Play("New State");
    }

    IEnumerator StartReload()
    {
        isReloading = true;
        gun.GetComponent<Animator>().Play("Reload");
        yield return new WaitForSeconds(reloadTime); // wait time for reload animation (MUST CHANGE MANUALLY DEPENDING ON GUNS ANIMATION)
        gun.GetComponent<Animator>().Play("New State");
        isReloading = false;
    }

    public bool getIsOwned()
    {
        return isOwned;
    }

}
