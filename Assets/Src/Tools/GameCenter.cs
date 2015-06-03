namespace GlobalPlay.Tools
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.SocialPlatforms;
    using UnityEngine.SocialPlatforms.GameCenter;

    /// <summary>
    /// Can be used on iOS only
    /// </summary>
    public sealed class GameCenter : MonoBehaviour
    {
        private ILeaderboard m_Leaderboard;

        private bool isLoggedIn = false;

        /// <summary>
        /// Set it to appropriate value.
        /// </summary>
        public string leaderboardID = "com.company.game.leaderboardname";

        public event UnityAction LoginComplete = delegate { };

        public static GameCenter Instance { get; private set; }

        /// <summary>
        /// Authenticates user, the LoginComplete event will be fired upon success
        /// </summary>
        public void LogIn()
        {
#if UNITY_IPHONE
            Social.localUser.Authenticate(OnAuthenticationComplete);
#else
            Debug.LogWarning("Game center can be used on iOS only");
#endif
        }

        /// <summary>
        /// Makes request to get achievements.
        /// </summary>
        /// <param name="success"></param>
        public void RequestAchievements()
        {
            if (!this.isLoggedIn)
            {
                Debug.LogWarning("Game center action requested but the user is not authenticated");
                return;
            }

            Social.LoadAchievements(OnAchievementsLoaded);
        }

        /// <summary>
        /// Makes request to get the leaderboard.
        /// </summary>
        public void RequestLeaderboard()
        {
            if (!this.isLoggedIn)
            {
                Debug.LogWarning("Game center action requested but the user is not authenticated");
                return;
            }

            m_Leaderboard = Social.CreateLeaderboard();
            m_Leaderboard.id = leaderboardID;
            m_Leaderboard.LoadScores(OnScoresLoaded);

            //Social.LoadScores(leaderboardID, scores =>
            //{
            //    if (scores.Length > 0)
            //    {
            //        Debug.Log("Received " + scores.Length + " scores");
            //        string myScores = "Leaderboard: \n";

            //        foreach (IScore score in scores)
            //        {
            //            myScores += "\t" + score.userID + " " + score.formattedValue
            //                + " " + score.date + "\n";
            //        }

            //        Debug.Log(myScores);
            //    }
            //    else
            //        Debug.Log("No scores have been loaded.");
            //});
        }

        /// <summary>
        /// Sends achievement progress to Game center
        /// </summary>
        public void ReportAchievement(string achievementId, double progress)
        {
            if (!this.isLoggedIn)
            {
                Debug.LogWarning("Game center action requested but the user is not authenticated");
                return;
            }

            Social.ReportProgress(achievementId, progress, (result) =>
            {
                Debug.Log(result ? string.Format("Successfully reported achievement {0}", achievementId)
                          : string.Format("Failed to report achievement {0}", achievementId));
            });
        }

        /// <summary>
        /// Sends score value to current leaderboardId to Game center
        /// </summary>
        public void ReportScore(long score)
        {
            if (!this.isLoggedIn)
            {
                Debug.LogWarning("Game center action requested but the user is not authenticated");
                return;
            }

            Social.ReportScore(score, leaderboardID, success =>
            {

                Debug.Log(success ? "Reported score to leaderboard successfully" : "Failed to report score");
            });
        }

        public void ResetAchievements()
        {
            if (!this.isLoggedIn)
            {
                Debug.LogWarning("Game center action requested but the user is not authenticated");
                return;
            }

            GameCenterPlatform.ResetAllAchievements((resetResult) =>
            {
                Debug.Log(resetResult ? "Achievements have been Reset" : "Achievement reset failure.");
            });
        }

        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        /// Gets called when authentication completes.
        /// </summary>
        /// <param name="success">If the operation is successful,
        /// Social.localUser will contain data from the Game center</param>
        private void OnAuthenticationComplete(bool success)
        {
            if (success)
            {
                Debug.Log("Authenticated, checking achievements");

                string userInfo = "Username: " + Social.localUser.userName +
                    "\nUser ID: " + Social.localUser.id +
                    "\nIsUnderage: " + Social.localUser.underage;
                Debug.Log(userInfo);

                this.LoginComplete();
            }
            else
                Debug.Log("Failed to authenticate with Game Center.");
        }

        private void OnAchievementsLoaded(IAchievement[] achievements)
        {
            if (achievements.Length == 0)
                Debug.Log("Error: no achievements found");
            else
                Debug.Log("Got " + achievements.Length + " achievements");

            Social.ShowAchievementsUI();
        }

        private void OnScoresLoaded(bool result)
        {
            Debug.Log("Received " + m_Leaderboard.scores.Length + " scores");
            foreach (IScore score in m_Leaderboard.scores)
            {
                Debug.Log(score);
            }

            //GameCenterPlatform.ShowLeaderboardUI(this.leaderboardID, TimeScope.Today);
            Social.ShowLeaderboardUI();
        }

        #region Debug

        private bool active = false;

        private void OnGUI()
        {
#if !UNITY_IPHONE // TODO: swap
            float d = Screen.height * .8f / 1000f;
            float h = 400;
            if (GUI.Button(new Rect(20 * d, (h + 700) * d, 250 * d, 100 * d), "Toggle GC"))
            {
                active = !active;
            }

            if (!active)
            {
                return;
            }

            if (GUI.Button(new Rect(20 * d, (h + 100) * d, 250 * d, 100 * d), "Log in"))
            {
                this.LogIn();
            }

            if (GUI.Button(new Rect(20 * d, (h + 200) * d, 250 * d, 100 * d), "View Leaderboard"))
            {
                this.RequestLeaderboard();
            }

            if (GUI.Button(new Rect(20 * d, (h + 300) * d, 250 * d, 100 * d), "View Achievements"))
            {
                this.RequestAchievements();
            }

            if (GUI.Button(new Rect(20 * d, (h + 400) * d, 250 * d, 100 * d), "Report Score"))
            {
                int highScore = 1000;
                this.ReportScore(highScore);
            }

            if (GUI.Button(new Rect(20 * d, (h + 500) * d, 250 * d, 100 * d), "Report Achievement"))
            {
                this.ReportAchievement("com.compnayname.demo.achievement1", 100.00);
            }

            if (GUI.Button(new Rect(20 * d, (h + 600) * d, 250 * d, 100 * d), "Reset Achievements"))
            {
                GameCenterPlatform.ResetAllAchievements((resetResult) =>
                {
                    Debug.Log(resetResult ? "Achievements have been Reset" : "Achievement reset failure.");
                });
            }
#endif
        }

        #endregion
    }
}