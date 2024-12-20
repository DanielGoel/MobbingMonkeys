using UnityEngine;
using System.Collections.Generic;

public class WeaponSwitch : MonoBehaviour
{
    public List<GameObject> weapons = new List<GameObject>(); // List of all weapon objects
    private int selectedWeapon = 0; // Index of currently selected weapon

    public int SelectedWeapon // Public getter for selectedWeapon
    {
        get { return selectedWeapon; }
    }

    void Start()
    {
        // Deactivate all weapons except the first one in the list
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(i == selectedWeapon); // Activate only the starting weapon
        }
    }

    void Update()
    {
        // Allow switching weapons only if multiple weapons are available
        if (weapons.Count > 1)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                selectedWeapon = (selectedWeapon + 1) % weapons.Count;
                UpdateWeaponSelection();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                selectedWeapon = (selectedWeapon - 1 + weapons.Count) % weapons.Count;
                UpdateWeaponSelection();
            }
        }
    }

    private void UpdateWeaponSelection()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            weapons[i].SetActive(i == selectedWeapon); // Activate only the selected weapon
        }
    }

    public void AddWeapon(GameObject newWeapon)
    {
        if (!weapons.Contains(newWeapon)) // Avoid duplicates
        {
            weapons.Add(newWeapon);
        }
    }
}
