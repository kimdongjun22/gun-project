using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform weaponHolder;
    private Weapon currentWeapon;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentWeapon != null)
        {
            currentWeapon.Fire();
        }
    }

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (currentWeapon != null)
            Destroy(currentWeapon.gameObject);

        GameObject newWeapon = Instantiate(weaponPrefab, weaponHolder.position, Quaternion.identity, weaponHolder);
        currentWeapon = newWeapon.GetComponent<Weapon>();
    }
}
