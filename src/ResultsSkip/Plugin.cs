using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Photon.Pun;
using Zorro.Core;

namespace ResultsSkip;

[BepInAutoPlugin]
public partial class Plugin : BaseUnityPlugin
{
    internal static ManualLogSource Log { get; private set; } = null!;

    private void Awake()
    {
        Log = Logger;

        Log.LogInfo($"Plugin {Name} is loaded!");
        Harmony.CreateAndPatchAll(typeof(ResultsSkip));
    }
    
}

public class ResultsSkip {
    [HarmonyPostfix]
    [HarmonyPatch(typeof(EndScreen), nameof(EndScreen.Next))]
    private static void ResultsSkipPatch() {
        if (!PhotonNetwork.IsMasterClient) {
            Plugin.Log.LogWarning("Only the Master Client can skip results!");
            return;
        }
        Singleton<PeakHandler>.Instance.EndScreenComplete();
        Plugin.Log.LogInfo("Skipped Results Screen for all players.");
    }
}