using UnityEngine;

namespace Gameplay.Data
{
    [CreateAssetMenu(fileName = "Reward", menuName = "Data/Reward", order = 1)]
    public class RewardData : ScriptableObject 
    {
        public int Day => _day;
        public int Reward => _reward;

        [SerializeField] private int _day;
        [SerializeField] private int _reward;
    }
}