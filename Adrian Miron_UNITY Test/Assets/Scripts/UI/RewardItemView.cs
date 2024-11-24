using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Gameplay.UI
{
    public class RewardItemView : MonoBehaviour
    {
        public int Day { get; private set; }
        public int Reward { get; private set;}

        [SerializeField] private Image _frame;
        [SerializeField] private Image _background;
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _dayTMP;
        [SerializeField] private TMP_Text _rewardTMP;
        [SerializeField] private List<Image> _coins;

        private EventSystem _eventSystem;    
    
        public void Initialize(int day, int reward)
        {
            _eventSystem = EventSystem.current;
            Day = day;
            Reward = reward;

            _dayTMP.text = $"Day {day.ToString()}";
            _rewardTMP.text = reward.ToString();

            CalculateCoinsCount(reward);
        }

        public void SetColor(Color color)
        {
            _background.color = color;
        }

        public void OnSelection()
        {
            _button.Select();
            _frame.gameObject.SetActive(true);
        }

        public void OnDeselection()
        {
            _button.OnDeselect(new BaseEventData(_eventSystem));
            _frame.gameObject.SetActive(false);
        }

        private void CalculateCoinsCount(int reward)
        {
            if(reward < 150)
            {
                ShowCoins(1);
            }
            else if(reward >= 150 && reward < 200)
            {
                ShowCoins(2);
            }
            else if(reward >= 200)
            {
                ShowCoins(3);
            }
        }

        private void ShowCoins(int count)
        {
            foreach (var coin in _coins)
            {
                coin.gameObject.SetActive(false);
            }

            for (int i = 0; i < count; i++)
            {
                _coins[i].gameObject.SetActive(true);
            }
        }
    }
}