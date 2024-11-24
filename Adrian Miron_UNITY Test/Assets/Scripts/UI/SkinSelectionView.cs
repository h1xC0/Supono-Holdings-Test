using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class SkinSelectionView : View<SkinSelectionView>
    {
        public List<Image> m_ColoredImages;
        public List<Text> m_ColoredTexts;

        public GameObject m_BrushesPrefab;
        public int m_IdSkin = 0;

        [SerializeField] private List<GameObject> _itemsToShow;
        [SerializeField] private Button _backButton;
        [SerializeField] private CachedScrollView _cachedScrollView;
        [SerializeField] private SkinItemView _skinItemViewPrefab;
        [SerializeField] private ScrollRect _verticalScroll;
        private List<SkinItemView> _skinItemViews;


        private StatsManager m_StatsManager;
        private MainMenuView m_MainMenuView;


        private Color _mainColor;

        protected override void Awake()
        {
            base.Awake();

            _skinItemViews = new List<SkinItemView>();
            m_StatsManager = StatsManager.Instance;
            m_MainMenuView = MainMenuView.Instance;
            m_IdSkin = m_StatsManager.FavoriteSkin;

            _backButton.onClick.AddListener(OnBack);
        }


        protected override void OnGamePhaseChanged(GamePhase _GamePhase)
        {
            base.OnGamePhaseChanged(_GamePhase);

            switch (_GamePhase)
            {
                case GamePhase.MAIN_MENU:
                    Transition(true);
                    GenerateBrushes();
                    break;

                case GamePhase.LOADING:
                        m_BrushesPrefab.SetActive(false);

                    if (m_Visible)
                        Transition(false);
                    break;
            }
        }

        public void ShowSkins()
        {
            Transition(true);
            foreach (var item in _itemsToShow)
            {
                item.SetActive(true);
            }
        }

        public void SetMainColor(Color _Color)
        {
            m_BrushesPrefab.SetActive(true);
            int favoriteSkin = Mathf.Min(m_StatsManager.FavoriteSkin, m_GameManager.m_Skins.Count - 1);
            m_BrushesPrefab.GetComponent<BrushMainMenu>().Set(m_GameManager.m_Skins[favoriteSkin]);

            for (int i = 0; i < m_ColoredImages.Count; ++i)
                m_ColoredImages[i].color = _Color;

            for (int i = 0; i < m_ColoredTexts.Count; i++)
                m_ColoredTexts[i].color = _Color;
                
            for (int i = 0; i < _skinItemViews.Count; i++)
                _skinItemViews[i].ChangeMainColor(_Color);
                
            _mainColor = _Color;
        }

        public void ChangeBrush(int _NewBrush)
        {
            _NewBrush = Mathf.Clamp(_NewBrush, 0, GameManager.Instance.m_Skins.Count);
            m_IdSkin = _NewBrush;
            if (m_IdSkin >= GameManager.Instance.m_Skins.Count)
                m_IdSkin = 0;
            GameManager.Instance.m_PlayerSkinID = m_IdSkin;
            int favoriteSkin = Mathf.Min(m_StatsManager.FavoriteSkin, m_GameManager.m_Skins.Count - 1);
            m_BrushesPrefab.GetComponent<BrushMainMenu>().Set(GameManager.Instance.m_Skins[favoriteSkin]);
            m_StatsManager.FavoriteSkin = m_IdSkin;
            GameManager.Instance.SetColor(GameManager.Instance.ComputeCurrentPlayerColor(true, 0));
        }

        private void GenerateBrushes()
        {
            for (int i = 0; i < GameManager.Instance.m_Skins.Count; i++)
            {
                var skinView = Instantiate(_skinItemViewPrefab, _verticalScroll.content);
                skinView.Initialize(i);
                skinView.ChangeMainColor(_mainColor);
                skinView.ButtonClickEvent += SetBrush;
                _skinItemViews.Add(skinView);
            }

            _cachedScrollView.CacheItems();
        }

        private void SetBrush(SkinItemView skinItemView)
        {
            ChangeBrush(skinItemView.SkinID);
        }

        private async void OnBack()
        {
            Transition(false);
            
            var durationToMS = TimeSpan.FromSeconds(m_FadeOutDuration);
            await Task.Delay((int)durationToMS.TotalMilliseconds);
            foreach(var item in _itemsToShow)
            {
                item.SetActive(false);
            }
            m_MainMenuView.ShowSkinsButton();
        }
    }   
}
