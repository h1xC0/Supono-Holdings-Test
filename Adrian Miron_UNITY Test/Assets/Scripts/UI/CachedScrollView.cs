using System;
using System.Collections.Generic;
using Gameplay.UI;
using UnityEngine;
using UnityEngine.UI;

public class CachedScrollView : MonoBehaviour
{
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private RectTransform _contentRoot;
    [SerializeField] private Image _viewPort;
    private List<RectTransform> _contentList = new();

    private void Awake()
    {
        CacheItems();
        _scrollRect.onValueChanged.AddListener(UpdateItemVisibility);
    }

    public void CacheItems()
    {
        for (int i = 0; i < _contentRoot.transform.childCount; i++)
        {
            var child = _contentRoot.GetChild(i);
            _contentList.Add((RectTransform)child);
        }
    }

    private void UpdateItemVisibility(Vector2 position)
    {
        foreach (var child in _contentList)
        {
            var overlaps = _viewPort.rectTransform.Overlap(child);
            var skinItemView = child.GetComponent<SkinItemView>();
            skinItemView.HideBrush(overlaps);
        }
    }

    private void OnDestroy() 
    {
        _scrollRect.onValueChanged.RemoveListener(UpdateItemVisibility);
    }

}