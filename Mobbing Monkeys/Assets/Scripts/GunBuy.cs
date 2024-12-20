using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunBuy : MonoBehaviour
{
    Transform player;
    bool inDistance = false;

    [SerializeField]
    public int CostOfGun = 9999;
    [SerializeField]
    public int CostOfAmmo = 1;

    bool isOwned = false;

    [SerializeField]
    public TMP_Text interactText;

    [SerializeField]
    public string gunName;

    public GameManagement game;
    public GameObject gunPrefab; // Reference to the gun prefab
    public WeaponSwitch weaponSwitch; // Reference to the WeaponSwitch script

    private GameObject purchasedGunInstance; // Store the instantiated gun instance

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        AttemptPurchase();
    }

    public bool AttemptPurchase()
    {
        if (inDistance)
        {
            if (!isOwned)
            {
                if (Input.GetKeyDown(KeyCode.F) && GameManagement.currentPoints >= CostOfGun)
                {
                    GameManagement.setPoints(GameManagement.currentPoints - CostOfGun); // Deduct points
                    isOwned = true; // Mark the gun as owned

                    // Instantiate the gun and store the reference
                    purchasedGunInstance = Instantiate(gunPrefab, weaponSwitch.transform);
                    purchasedGunInstance.SetActive(false); // Deactivate by default
                    weaponSwitch.AddWeapon(purchasedGunInstance); // Add to WeaponSwitch

                    // Update interact text for ammo purchase
                    interactText.text = $"F to interact: {gunName} ammo ~ ${CostOfAmmo}";
                    return true;
                }
            }
            else
            {
                // Refill ammo for the purchased gun
                if (Input.GetKeyDown(KeyCode.F) && GameManagement.currentPoints >= CostOfAmmo)
                {
                    GameManagement.setPoints(GameManagement.currentPoints - CostOfAmmo); // Deduct points

                    if (purchasedGunInstance != null)
                    {
                        Gun gunScript = purchasedGunInstance.GetComponent<Gun>();
                        if (gunScript != null)
                        {
                            gunScript.reserveAmmo = gunScript.maxReserveAmmo; // Refill ammo
                            gunScript.currentAmmo = gunScript.maxMagSize;
                            Debug.Log($"{gunName} ammo refilled to: {gunScript.reserveAmmo}");
                        }
                    }
                    return true;
                }
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inDistance = true;

            if (isOwned)
            {
                interactText.text = $"F to interact: {gunName} ammo ~ ${CostOfAmmo}";
            }
            else
            {
                interactText.text = $"F to interact: {gunName} ~ ${CostOfGun}";
            }
            interactText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            inDistance = false;
            interactText.gameObject.SetActive(false);
        }
    }
}
