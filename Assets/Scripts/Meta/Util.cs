using System;

public static class Util
{
    public static float Remap(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
    
    public static string ConvertSecondsToDurationString(double timeInSeconds)
    {
        int m = (int)Math.Floor(timeInSeconds / 60);
        int s = (int)Math.Floor(timeInSeconds % 60);
        int ms = (int)Math.Floor((timeInSeconds * 100) % 100);

        string mString = m < 10 ? "0" + m : m.ToString();
        string sString = s < 10 ? "0" + s : s.ToString();
        string msString = ms < 10 ? "0" + ms : ms.ToString();
    
        return mString + ":" + sString + "." + msString;
    }
}