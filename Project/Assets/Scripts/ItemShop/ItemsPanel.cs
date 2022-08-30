using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class ItemsPanel : MonoBehaviour, IEnhancedScrollerDelegate
{
   // [SerializeField] private GroupInfo _groupInfo;
    [SerializeField] private EnhancedScroller scroller;
    [SerializeField] private EnhancedScrollerCellView cellViewPrefab;
    private readonly List<BodyCellData> _dataSkins = new List<BodyCellData>();
    [SerializeField] private List<ShopItem> list = new List<ShopItem>();
    private int _reloadScrollerFrameCountLeft = -1;
    [SerializeField] private ShopItem shopItem;
    [SerializeField] private GroupInfo _groupInfo;
    public ShopItem _shopItem
    {
        get => shopItem;
        set => shopItem = value;
    }

    private Callback _close;

    private bool _isScroll;

    
    // Start is called before the first frame update
    public void Enable(Callback close)
    {
        gameObject.SetActive(true);
        _close = close;
     //   ShopItem = GetBodyInfoWithID((BodySkinId) GameData.BodySkin);
        if (_isScroll)
        {
            scroller.RefreshActiveCellViews();
            scroller.JumpToDataIndex(0);
            return;
        }

        _isScroll = true;
    }


    private void Start()
    {
        Enable(null);
        scroller.Delegate = this;
        LoadDataScroll();
        Initialize();
    }

    // Update is called once per frame

    public void Initialize()
    {
        list = new List<ShopItem>();
        foreach (var item in GameControl.Instance.ItemsInfos)
        {
            list.Add(item);
        }
        
        var listConvert = new List<ShopItem>();
        var listClone = new List<ShopItem>();
        listClone.AddRange(list);
        listClone.Reverse();

        for (var i = listClone.Count - 1; i >= 0; i--)
        {
            var item = listClone[i];
         //   if (item.State == State_Man.Locked) continue;
            listConvert.Add(item);
            listClone.Remove(item);
        }

        listClone.Reverse();
        listConvert.AddRange(listClone);
        list = listConvert;
        
        var count = 0;
        var convert = new List<ShopItem>(); //Init cac CellView
        for (var i = 0; i < list.Count; i++)
        {
            convert.Add(list[i]);
            count++;
            if (count < 3) continue;
            var newCell = new BodyCellData();
            newCell.listInfo = new List<ShopItem>();
            newCell.listInfo.AddRange(convert);
            _dataSkins.Add(newCell);
            convert.Clear();
            count = 0;
        }

        if (count <= 0) return;
        {
            var newCell = new BodyCellData();
            newCell.listInfo = new List<ShopItem>();
            newCell.listInfo.AddRange(convert);
            _dataSkins.Add(newCell);
        }
    }

    // private ShopItem GetBodyInfoWithID(BodySkinId id)
    // {
    //     return list.FirstOrDefault(item => item.Id == id);
    // }
    public void ButtonShowShop()
    {
        Enable(null);
    }
    public void ClickBtn(ShopItem bodyinfo)
    {
        _shopItem = bodyinfo;
        _groupInfo.Enable();
    }
   
    /// <summary>
    /// 
    /// </summary>

    #region EnhancedScroller Control

    private void LoadDataScroll()
    {
        // capture the scroller dimensions so that we can reset them when we are done
        var rectTransform = scroller.GetComponent<RectTransform>();
        var size = rectTransform.sizeDelta;

        // set the dimensions to the largest size possible to acommodate all the cells
        rectTransform.sizeDelta = new Vector2(size.x, float.MaxValue);

        // First Pass: reload the scroller so that it can populate the text UI elements in the cell view.
        // The content size fitter will determine how big the cells need to be on subsequent passes
        scroller.ReloadData();

        // reset the scroller size back to what it was originally
        rectTransform.sizeDelta = size;

        // set up our frame countdown so that we can reload the scroller on subsequent frames
        _reloadScrollerFrameCountLeft = 1;
    }

    void LateUpdate()
    {
        // only process if we have a countdown left
        if (_reloadScrollerFrameCountLeft != -1)
        {
            // skip the first frame (frame countdown 1) since it is the one where we set up the scroller text.
            if (_reloadScrollerFrameCountLeft < 1)
            {
                // reload two times, the first to put the newly set content size fitter values into the model,
                // the second to set the scroller's cell sizes based on the model.
                scroller.ReloadData();
            }

            // decrement the frame count
            _reloadScrollerFrameCountLeft--;
        }
    }

    #endregion

    #region EnhancedScroller Handlers

    public int GetNumberOfCells(EnhancedScroller scroll)
    {
        return _dataSkins.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroll, int dataIndex)
    {
        // we pull the size of the cell from the model.
        // First pass (frame countdown 2): this size will be zero as set in the LoadData function
        // Second pass (frame countdown 1): this size will be set to the content size fitter in the cell view
        // Third pass (frmae countdown 0): this set value will be pulled here from the scroller
        return cellViewPrefab.GetComponent<RectTransform>().sizeDelta.y;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroll, int dataIndex, int cellIndex)
    {
        var cellView = scroll.GetCellView(cellViewPrefab) as UIBodyCellView;
        if (cellView == null)
            return null;
        cellView.SetData(_dataSkins[dataIndex], dataIndex, this);
        return cellView;
    }

    #endregion
}

public class BodyCellData
{
    public List<ShopItem> listInfo;
}