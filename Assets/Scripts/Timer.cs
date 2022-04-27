using System.Collections;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{    
    float _initialTime;
    float timeRemaining;
    bool timeRunning = false;

    public void Countdown(TextMeshProUGUI textWhereDisplay, float initialTime)
    {   
        timeRunning = true;
        timeRemaining = initialTime;
        _initialTime = initialTime;
        StartCoroutine(CountdownTimer(textWhereDisplay));
    }

    IEnumerator CountdownTimer(TextMeshProUGUI timeText)
    {
        while (timeRunning)
        {
            if (timeRemaining <= 0)
            {
                timeRunning = false;
                timeRemaining = _initialTime;
                ChangeText(timeText, _initialTime);
                yield return null;
            }
            ChangeText(timeText, timeRemaining);
            timeRemaining -= 1f;
            yield return new WaitForSeconds(1f);
        }
    }

    void ChangeText(TextMeshProUGUI timeText, float time)
    {
        timeText.text = Mathf.FloorToInt(time).ToString();
    }
}