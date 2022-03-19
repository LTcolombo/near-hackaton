using System;
using service;
using utils.injection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace view.behaviours.UI
{
    public class DisplayMatchUsers : InjectableBehaviour
    {
        [Inject]
        private MultiplayerService _multiplayer;

        public GameObject NoMatch;
        public GameObject MatchInfo;

        public Text[] UserNameLabels;

        void OnEnable()
        {
            _multiplayer.UsersUpdated.Add(UpdateViews);
            UpdateViews();
        }

        private void UpdateViews()
        {
            MatchInfo.SetActive(false);
            NoMatch.SetActive(true);

            if (_multiplayer.Users.Count < 2)
            {
                MatchInfo.SetActive(false);
                NoMatch.SetActive(true);
            }
            else
            {
                MatchInfo.SetActive(true);
                NoMatch.SetActive(false);

                var userData = _multiplayer.Users;
                bool allReady = true;
                for (var i = 0; i < Math.Min(UserNameLabels.Length, userData.Count); i++)
                {
                    UserNameLabels[i].text = (userData[i].DisplayName ?? userData[i].Username).Split('.')[0].ToUpper();
                    var isReady = _multiplayer.IsReady(userData[i].Id);
                    UserNameLabels[i].color = isReady ? Color.green : Color.white;
                    
                    allReady &= isReady;
                }

                if (allReady)
                    SceneManager.LoadScene("Hive");
            }
        }

        private void OnDisable()
        {
            _multiplayer.UsersUpdated.Remove(UpdateViews);
        }
    }
}