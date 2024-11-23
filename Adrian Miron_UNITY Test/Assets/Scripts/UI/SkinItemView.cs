using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.UI
{
    public class SkinItemView : MonoBehaviour
    {
        public event Action<SkinItemView> ButtonClickEvent;
        public int SkinID => _skinID;

        [SerializeField] private BrushMainMenu _brush;
        [SerializeField] private Button _button;

        private int _skinID;

        public void Initialize(int skinID)
        {
            _skinID = skinID;
            _brush.Set(GameManager.Instance.m_Skins[_skinID]);
            _button.onClick.AddListener(OnButtonClick);
        }

        public void ChangeMainColor(Color mainColor)
        {
            _button.image.color = mainColor;
        }

        private void OnButtonClick()
        {
            ButtonClickEvent?.Invoke(this);
        }

        public void HideBrush(bool flag)
        {
            _brush.gameObject.SetActive(flag);
        }
    }
}
