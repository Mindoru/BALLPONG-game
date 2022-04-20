using System;
using TMPro;

public class OptionSlider
{
    static string _text;
    public static void SetValue(TextMeshProUGUI text, int value)
    {
        _text = value.ToString();
        text.SetText(_text);
    }

    public static void PreviousValue(int[] values, ref int currentValue, ref int currentValueIndex, TextMeshProUGUI text, Action action)
    {
        int previous;
        if (currentValueIndex <= 0) return;
        for (int i = 0; i <= values.Length; i++)
        {
            if (i == (currentValueIndex - 1))
            {
                previous = values[i];
                currentValue = previous;
                currentValueIndex -= 1;
                SetValue(text, currentValue);
                if (action != null) action();
                break;
            }
        }
    }

    public static void NextValue(int[] values, ref int currentValue, ref int currentValueIndex, TextMeshProUGUI text, Action action = null)
    {
        int next;
        if (currentValueIndex >= (values.Length - 1)) return;
        for (int i = 0; i <= values.Length; i++)
        {
            if (i == (currentValueIndex + 1))
            {
                next = values[i];
                currentValue = next;
                currentValueIndex += 1;
                SetValue(text, currentValue);
                if (action != null) action();
                break;
            }
        }
    }
}
