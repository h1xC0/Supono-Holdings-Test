using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class DebugMenuView : View<DebugMenuView>
    {
        public bool PlayerCollisionFeature
        {
            get => PlayerPrefs.GetInt("PlayerCollisionFeature", 0) == 1 ? true : false;
            set => PlayerPrefs.SetInt("PlayerCollisionFeature", value ? 1 : 0);
        }
        public bool SkinSelectionFeature
        {
            get => PlayerPrefs.GetInt("SkinSelectionFeature", 0) == 1 ? true : false;
            set => PlayerPrefs.SetInt("SkinSelectionFeature", value ? 1 : 0);
        }
        public bool DailyRewardFeature
        {
            get => PlayerPrefs.GetInt("DailyRewardFeature", 0) == 1 ? true : false;
            set => PlayerPrefs.SetInt("DailyRewardFeature", value ? 1 : 0);
        }
        public bool CustomFeature 
        {
            get => PlayerPrefs.GetInt("CustomFeature", 0) == 1 ? true : false;
            set => PlayerPrefs.SetInt("CustomFeature", value ? 1 : 0);
        }

        [SerializeField] private Toggle _playerCollisionFeature;
        [SerializeField] private Toggle _skinSelectionFeature;
        [SerializeField] private Toggle _dailyRewardFeature;
        [SerializeField] private Toggle _customFeature;

        private MainMenuView m_MainMenuView;

        protected override void Awake()
        {
            base.Awake();

            m_MainMenuView = MainMenuView.Instance;
            
            ResetToggles();

            _playerCollisionFeature.onValueChanged.AddListener(OnPlayerCollisionFeature);
            _skinSelectionFeature.onValueChanged.AddListener(OnSkinSelectionFeature);
            _dailyRewardFeature.onValueChanged.AddListener(OnDailyRewardFeature);
            _customFeature.onValueChanged.AddListener(OnCustomFeature);


            _playerCollisionFeature.isOn = PlayerCollisionFeature;
            _skinSelectionFeature.isOn = SkinSelectionFeature;
            _dailyRewardFeature.isOn = DailyRewardFeature;
            _customFeature.isOn = CustomFeature;
        }

        private void ResetToggles()
        {
            _playerCollisionFeature.isOn = !_playerCollisionFeature.isOn;
            _skinSelectionFeature.isOn = !_skinSelectionFeature.isOn ;
            _dailyRewardFeature.isOn = !_dailyRewardFeature.isOn;
            _customFeature.isOn = !_customFeature.isOn;
        }

        public void HideOnSkinSelection(bool flag)
        {
            Transition(!flag);
        }

        protected override void OnGamePhaseChanged(GamePhase _GamePhase)
        {
            base.OnGamePhaseChanged(_GamePhase);

            switch (_GamePhase)
            {
                case GamePhase.MAIN_MENU:
                    Transition(true);
                    break;

                case GamePhase.LOADING:
                    if (m_Visible)
                        Transition(false);
                    break;
            }
        }

        private void OnPlayerCollisionFeature(bool flag)
        {
            PlayerCollisionFeature = flag;
        }

        private void OnSkinSelectionFeature(bool flag)
        {   
            m_MainMenuView.SetBrushSelectionType(flag);
            SkinSelectionFeature = flag;
        }

        private void OnDailyRewardFeature(bool flag)
        {
            DailyRewardFeature = flag;
        }

        private void OnCustomFeature(bool flag)
        {
            CustomFeature = flag;
        }
    }
}