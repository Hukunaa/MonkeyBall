using System;
using System.Collections.Generic;
using UnityEngine;
using Voodoo.Tiny.Sauce.Internal.Analytics;
using Voodoo.Tiny.Sauce.Privacy;

public static class TinySauce
{
    private const string TAG = "TinySauce";
    public const string Version = "6.4.2";
    private const string ABCohortKey = "ABCohort";
    private const string DebugCohortKey = "DebugCohortKey";

    public static event Action<bool, bool> ConsentGiven;

    #region GameStart Methods

    /// <summary>
    ///  Method to call whenever the user starts a game.
    /// </summary>
    public static void OnGameStarted(int levelNumber, Dictionary<string, object> eventProperties = null)
    {
        OnGameStarted(levelNumber.ToString(), eventProperties);
    }

    /// <summary>
    ///  Method to call whenever the user starts a game.
    /// </summary>
    /// <param name="levelName">The game Level, this parameter is optional for game without level</param>
    public static void OnGameStarted(string levelName = null, Dictionary<string, object> eventProperties = null)
    {
        AnalyticsManager.TrackGameStarted(levelName, eventProperties);
    }

    #endregion

    #region GameFinish Methods

    /// <summary>
    /// Method to call whenever the user completes a game with levels.
    /// </summary>
    /// <param name="score">The score of the game</param>
    public static void OnGameFinished(float score)
    {
        OnGameFinished(true, score, null, null);
    }

    /// <summary>
    /// Method to call whenever the user finishes a game, even when leaving a game.
    /// </summary>
    /// <param name="levelComplete">Whether the user finished the game</param>
    /// <param name="score">The score of the game</param>
    public static void OnGameFinished(bool levelComplete, float score, Dictionary<string, object> eventProperties = null)
    {
        OnGameFinished(levelComplete, score, null, eventProperties);
    }

    /// <summary>
    /// Method to call whenever the user finishes a game, even when leaving a game.
    /// </summary>
    /// <param name="levelComplete">Whether the user finished the game</param>
    /// <param name="score">The score of the game</param>
    /// <param name="levelNumber">The level number</param>
    public static void OnGameFinished(bool levelComplete, float score, int levelNumber, Dictionary<string, object> eventProperties = null)
    {
        OnGameFinished(levelComplete, score, levelNumber.ToString(), eventProperties);
    }

    /// <summary>
    /// Method to call whenever the user finishes a game, even when leaving a game.
    /// </summary>
    /// <param name="levelComplete">Whether the user finished the game</param>
    /// <param name="score">The score of the game</param>
    /// <param name="levelName">The level name</param>
    public static void OnGameFinished(bool levelComplete, float score, string levelName, Dictionary<string, object> eventProperties = null)
    {
        AnalyticsManager.TrackGameFinished(levelComplete, score, levelName, eventProperties);
    }

    #endregion

    #region CustomEvent Method

    /// <summary>
    /// Call this method to track any custom event you want.
    /// </summary>
    /// <param name="eventName">The name of the event to track</param>
    /// <param name="eventProperties">An optional list of properties to send along with the event</param>
    /// <param name="type">type of the event</param>
    /// <param name="analyticsProviders">The list of analytics provider you want to track your custom event to. If this list is null or empty, the event will be tracked in GameAnalytics and Mixpanel (if the user is in a cohort)</param>
    public static void TrackCustomEvent(string eventName, Dictionary<string, object> eventProperties = null,
        string type = null, List<AnalyticsProvider> analyticsProviders = null)
    {
        AnalyticsManager.TrackCustomEvent(eventName, eventProperties, type, analyticsProviders);
    }

    #endregion

    public static void SubscribeToConsentGiven(Action<bool, bool> onConsentGiven)
    {
        if (!PrivacyManager.ConsentReady)
            PrivacyManager.OnConsentGiven += onConsentGiven;
        else
            onConsentGiven?.Invoke(PrivacyManager.AdConsent, PrivacyManager.AnalyticsConsent);
    }

    public static void UnsubscribeToConsentGiven(Action<bool, bool> onConsentGiven)
    {
        PrivacyManager.OnConsentGiven -= onConsentGiven;
    }

    public static string GetABTestCohort()
    {
#if UNITY_EDITOR
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString(DebugCohortKey)))
        {
            return PlayerPrefs.GetString(DebugCohortKey);
        }
#endif
        return PlayerPrefs.GetString(ABCohortKey);
    }

    public enum AnalyticsProvider
    {
        VoodooAnalytics,
        GameAnalytics,
    }
}