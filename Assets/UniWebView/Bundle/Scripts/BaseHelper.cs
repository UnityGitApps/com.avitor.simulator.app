using UnityEngine;

public static class BaseHelper
{
    private static readonly string DopString = "dalvik";
    public static readonly string AppSubArg = "AVITORyu_ALL";
    public static readonly string BundleParam = $"{DopString}_package";
    public static readonly string GAIDParam = $"{DopString}_gaid";
    public static readonly string AppSubParam = $"{DopString}_appsub";

    public static bool IsEnd(this string input)
    {
        return input.Contains(DopString);
    }

    public static string Company(this string input)
    {
        return input;
    }
}
