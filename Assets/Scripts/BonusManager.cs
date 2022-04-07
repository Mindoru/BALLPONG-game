using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public static BonusManager Instance;

    [SerializeField] PlayerController Player;
    [SerializeField] float PlayerSpeedBonus = 3f;
    Dictionary<string, bool> hasBonus = new Dictionary<string, bool>();
    Dictionary<string, float> bonusTime = new Dictionary<string, float>()
    {
      { "speed", 2.5f },
    };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpeedBonus();
        }
    }

    public void SpeedBonus()
    {
        if (!hasBonus.ContainsKey("speed"))
        {
            Player.Speed += PlayerSpeedBonus;
            hasBonus.Add("speed", true);
            StartCoroutine(RemoveSpeedBonus());
        }
    }

    IEnumerator RemoveSpeedBonus()
    {
        yield return new WaitForSeconds(bonusTime["speed"]);
        Player.Speed -= PlayerSpeedBonus;
        hasBonus.Remove("speed");
    }
}
