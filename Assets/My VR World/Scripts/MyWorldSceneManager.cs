using UnityEngine;

public class MyWorldSceneManager : MonoBehaviour
{
    public GameObject Weapon;
    private void Awake()
    {
        Weapon.SetActive(true);
    }
}
