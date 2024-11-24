using System;
using System.Collections.Generic;
using DG.Tweening;
using Gameplay.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class DailyRewardView : View<DailyRewardView>
    {
        [SerializeField] private TMP_Text _moneyTMP;
        [SerializeField] private Image _moneyCoin;
        [SerializeField] private Button _claimRewardButton;
        [SerializeField] private RectTransform _rewardsRoot;
        [SerializeField] private RewardItemView _rewardViewPrefab;
        [SerializeField] private List<RewardData> _rewardData;
        
        [SerializeField] private List<Image> m_ColoredImages;
        [SerializeField] private List<Text> m_ColoredTexts;
        [SerializeField] private List<Image> _satisfyingCoins;

        private List<RewardItemView> _rewards;
        private RewardItemView _currentReward;
        private StatsManager _statsManager;

        private DateTime _lastClaimTime;
        

        protected override void Awake()
        {
            base.Awake();
            
            if(DebugMenuView.Instance.DailyRewardFeature == false)
                return;

            _rewards = new List<RewardItemView>();
            _statsManager = StatsManager.Instance;
            _claimRewardButton.onClick.AddListener(() => OnClaimReward());
            
            _moneyTMP.text = _statsManager.Money.ToString();

            _lastClaimTime = DateTime.Parse(_statsManager.LastClaimTime);
            CalculateLastClaim();
        }

        public void SetMainColor(Color _Color)
        {
            if(DebugMenuView.Instance.DailyRewardFeature == false)
                return;

            for (int i = 0; i < m_ColoredImages.Count; ++i)
                m_ColoredImages[i].color = _Color;

            for (int i = 0; i < m_ColoredTexts.Count; i++)
                m_ColoredTexts[i].color = _Color;
                
            for(int i = 0; i < _rewards.Count; i++)
                _rewards[i].SetColor(_Color);
        }

        private void CalculateLastClaim()
        {
            var timeSinceLastClaim = DateTime.Now - _lastClaimTime;
            if(timeSinceLastClaim.TotalHours >= 24)
            {
                if(timeSinceLastClaim.TotalHours >= 48 || _statsManager.Day == 7 || _statsManager.Day == 0)
                {
                    _statsManager.Day = 1;
                }

                GenerateRewards();
                _currentReward = _rewards[_statsManager.Day - 1];
                _currentReward.OnSelection();
            }
        }

        private void GenerateRewards()
        {
            foreach (var data in _rewardData)
            {
                var rewardView = Instantiate(_rewardViewPrefab, _rewardsRoot);
                rewardView.Initialize(data.Day, data.Reward);
                _rewards.Add(rewardView);
            }

            Transition(true);
        }


        private Sequence OnClaimReward()
        {
            var sequence = DOTween.Sequence();
            var currentMoney = _statsManager.Money;

            sequence
            .AppendCallback(() => 
            {
                _statsManager.Money += _currentReward.Reward;
                _statsManager.Day++;
                _statsManager.LastClaimTime = DateTime.Now.ToString("o");
            })
            .Join(LerpCoinsToMoneyCount())
            .Append(DOVirtual.Float(currentMoney, currentMoney + _currentReward.Reward, 0.5f, value => _moneyTMP.text = value.ToString("N0")))
            .AppendCallback(() => Transition(false));

            return sequence;
        }

        private Sequence LerpCoinsToMoneyCount()
        {
            var sequence = DOTween.Sequence();
            var speedMultiplier = 0.01f;
            foreach (var coin in _satisfyingCoins)
            {
                coin.gameObject.SetActive(true);
                sequence.Join(coin.DOFade(1, 0.025f));
                sequence.AppendInterval(speedMultiplier);
            }
                
            sequence.AppendInterval(speedMultiplier);

            foreach (var coin in _satisfyingCoins)
            {
                sequence
                .Join(coin.transform
                    .DOMove(_moneyCoin.transform.position, .8f)
                    .OnComplete(() => coin
                        .DOFade(0, 0.25f)
                        .OnComplete(() => coin.gameObject.SetActive(false))));
            }
            return sequence;
        }
    }
}