
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class UIBodyCellView : EnhancedScrollerCellView
{
    [SerializeField] private int _cellId;
    [SerializeField] private List<BodyItems> items;
    private BodyCellData _cellView;
    private ItemsPanel _manager;
    public void SetData(BodyCellData cellView, int cellId, ItemsPanel manager)
    {
        _cellView = cellView;
        _manager = manager;
        _cellId = cellId;
        var count = cellView.listInfo.Count;
        for (var i = 0; i < items.Count; i++)
        {
            if (i < count)
            {
                items[i].gameObject.SetActive(true);
                items[i].SetData(cellView.listInfo[i], _cellId, manager);
                    
            }
            else
                items[i].gameObject.SetActive(false);
        }

    }

    public override void RefreshCellView()
    {
        var count = _cellView.listInfo.Count;
        for (var i = 0; i < items.Count; i++)
        {
            if (i < count)
            {
                items[i].gameObject.SetActive(true);
                items[i].SetData(_cellView.listInfo[i], _cellId, _manager);
                    
            }
            else
                items[i].gameObject.SetActive(false);
        }
    }
}
