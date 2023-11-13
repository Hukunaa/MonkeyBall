using System;
using System.Collections.Generic;

namespace Voodoo.Tiny.Sauce.Internal.Analytics
{
    internal static class AnalyticsManager
    {
        private const string TAG = "AnalyticsManager";
        private const string NO_GAME_LEVEL = "game";

        internal static bool HasGameStarted { get; private set; }

        // Voodoo sauce additional events
        
        #region Progression Events Declaration
        internal static event Action<int, bool> OnGamePlayed;
        internal static event Action<GameStartedParameters> OnGameStartedEvent;
        internal static event Action<GameFinishedParameters> OnGameFinishedEvent;
        
        #endregion

        #region Metrics Event Declaration

        
        private const float PerfMetricsPeriod = 30f;
        internal static event Action<PerformanceMetricsAnalyticsInfo> OnTrackPerformanceMetricsEvent;
        

        #endregion

        #region Application Launch Events Declaration

        internal static event Action OnApplicationFirstLaunchEvent;
        internal static event Action OnApplicationLaunchEvent;
        

        #endregion

        #region Interstitial Events Declaration
        
        internal static event Action<AdShownEventAnalyticsInfo> OnInterstitialShowEvent;
        internal static event Action<AdClickEventAnalyticsInfo> OnInterstitialClickedEvent;

        #endregion
        
        #region Rewarded Events Declaration
        
        internal static event Action<AdShownEventAnalyticsInfo> OnRewardedShowEvent;
        internal static event Action<AdClickEventAnalyticsInfo> OnRewardedClickedEvent;

        #endregion

        #region Custom Event Declaration
        
        internal static event Action<string, Dictionary<string, object>, string, List<TinySauce.AnalyticsProvider>> OnTrackCustomEvent;

        #endregion

        #region Unusued Event Declaration, TO CLEAN
        internal static event Action OnApplicationResumeEvent;
        
        #endregion

        
        private static readonly List<TinySauce.AnalyticsProvider> DefaultAnalyticsProvider = new List<TinySauce.AnalyticsProvider>
            {TinySauce.AnalyticsProvider.GameAnalytics};

        private static List<IAnalyticsProvider> _analyticsProviders;

        internal static void Initialize(TinySauceSettings sauceSettings, bool consent)
        {
            _analyticsProviders = new List<IAnalyticsProvider>()
            {
                new GameAnalyticsProvider(), 
                new VoodooAnalyticsProvider(new VoodooAnalyticsParameters(false, true, ""))
            };
            
            // Initialize providers
            _analyticsProviders.ForEach(provider => provider.Initialize(consent));
            
            PerformanceMetricsManager.Initialize(PerfMetricsPeriod);
        }

        internal static void OnApplicationResume()
        {
            OnApplicationResumeEvent?.Invoke();
        }
        
        // Track game events
        // ================================================
        internal static void TrackApplicationLaunch()
        {
            AnalyticsStorageHelper.IncrementAppLaunchCount();
            //fire app launch events
            if (AnalyticsStorageHelper.IsFirstAppLaunch())
            {
                OnApplicationFirstLaunchEvent?.Invoke();
            }

            OnApplicationLaunchEvent?.Invoke();
        }
        
        #region Track Progression Events
        
        internal static void TrackGameStarted(string level, Dictionary<string, object> eventProperties = null)
        {
            HasGameStarted = true;
            AnalyticsStorageHelper.IncrementGameCount();
            GameStartedParameters gameStartedParameters = new GameStartedParameters
            {
                level = level ?? NO_GAME_LEVEL,
                eventProperties = eventProperties,
            };
            OnGameStartedEvent?.Invoke(gameStartedParameters);
        }

        internal static void TrackGameFinished(bool levelComplete, float score, string level, Dictionary<string, object> eventProperties)
        {
            HasGameStarted = false;
            AnalyticsStorageHelper.UpdateLevel(level);
            if (levelComplete)
            {
                // used to calculate the win rate (for RemoteConfig)
                AnalyticsStorageHelper.IncrementSuccessfulGameCount();
            }

            GameFinishedParameters gameFinishedParameters = new GameFinishedParameters
            {
                level = level ?? NO_GAME_LEVEL,
                status = levelComplete,
                score = score,
                eventProperties = eventProperties,
            };

            OnGamePlayed?.Invoke(AnalyticsStorageHelper.GetGameCount(), AnalyticsStorageHelper.UpdateGameHighestScore(score));
            OnGameFinishedEvent?.Invoke(gameFinishedParameters);
        }
        #endregion

        #region Track Custom Event
        
        internal static void TrackCustomEvent(string eventName,
                                              Dictionary<string, object> eventProperties,
                                              string type = null,
                                              List<TinySauce.AnalyticsProvider> analyticsProviders = null)
        {
            if (analyticsProviders == null || analyticsProviders.Count == 0)
            {
                analyticsProviders = DefaultAnalyticsProvider;
            }

            OnTrackCustomEvent?.Invoke(eventName, eventProperties, type, analyticsProviders);
        }
        
        #endregion
        
        #region Track Metrics Event

        internal static void TrackPerformanceMetrics(PerformanceMetricsAnalyticsInfo info)
        {
            OnTrackPerformanceMetricsEvent?.Invoke(info);
        }
        #endregion
        
        #region Track Interstitial Event
        
        /*
        *
        * AnalyticsManager.TrackInterstitialShow(new AdShownEventAnalyticsInfo {
              AdTag = tag,
              AdNetworkName = MediationAdapter.GetRewardedVideoAdNetworkName(),
              AdLoadingTime = (int) MediationAdapter.GetRewardedVideoLoadingTime().TotalMilliseconds,
              AdCount = AnalyticsStorageHelper.GetShowRewardedVideoCount()
          });
        */
    
        internal static void TrackInterstitialShow(AdShownEventAnalyticsInfo adAnalyticsInfo)
        {
            adAnalyticsInfo.GameCount = AnalyticsStorageHelper.GetGameCount();
            OnInterstitialShowEvent?.Invoke(adAnalyticsInfo);
        }
  
        
        internal static void TrackInterstitialClick(AdClickEventAnalyticsInfo adAnalyticsInfo)
        {
            adAnalyticsInfo.GameCount = AnalyticsStorageHelper.GetGameCount();
            OnInterstitialClickedEvent?.Invoke(adAnalyticsInfo);
        }

        #endregion
        
        #region Track Rewarded Event
        
        internal static void TrackRewardedShow(AdShownEventAnalyticsInfo adAnalyticsInfo)
        {
            adAnalyticsInfo.GameCount = AnalyticsStorageHelper.GetGameCount();
            OnRewardedShowEvent?.Invoke(adAnalyticsInfo);
        }
        
        internal static void TrackRewardedClick(AdClickEventAnalyticsInfo adAnalyticsInfo)
        {
            adAnalyticsInfo.GameCount = AnalyticsStorageHelper.GetGameCount();
            OnRewardedClickedEvent?.Invoke(adAnalyticsInfo);
        }

        #endregion
    }
}