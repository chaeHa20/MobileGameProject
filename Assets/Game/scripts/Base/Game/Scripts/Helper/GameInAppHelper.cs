using System;
using System.Collections.Generic;
using UnityHelper;
using System.Diagnostics;

#if _INAPP_
using UnityEngine.Purchasing;
#endif

public class GameInAppHelper : InAppHelper
{
    private bool m_isNoAds = false;
    //private List<int> m_nonConsumableProductRowIds = new List<int>();
    public bool isNoAds => m_isNoAds;

#if _INAPP_

    public static GameInAppHelper getInstance()
    {
        return getInstance<GameInAppHelper>();
    }

    protected override ConfigurationBuilder getBuilder()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        var inAppProductTable = GameTableHelper.instance.getTable<InAppProductTable>((int)eTable.InAppProduct);
        inAppProductTable.forEach<InAppProductRow>((row) =>
        {
            ProductType productType = (!row.isOnlyOneBuy) ? ProductType.Consumable : ProductType.NonConsumable;

            if (Logx.isActive)
                Logx.trace("InApp Product Add {0}, {1}", row.productId, productType);

            builder.AddProduct(row.productId, productType);

            /*
            if (ProductType.NonConsumable == productType)
            {
                m_nonConsumableProductRowIds.Add(row.id);
            }
            */
        });

        return builder;
    }

    protected override void initializedStore(IStoreController controller)
    {
        base.initializedStore(controller);

        // getNoAds();
        //checkIsNoAds(controller);
    }

    /*
     * ¬Õ ¿ÃªÛ«ÿº≠ æ»æ∏
    /// <param name="controller"></param>
    private void checkIsNoAds(IStoreController controller)
    {
        if (Logx.isActive)
            Logx.trace("checkIsNoAds");

        var inAppProductTable = GameTableHelper.instance.getTable<InAppProductTable>((int)eTable.InAppProduct);

        foreach (var rowId in m_nonConsumableProductRowIds)
        {
            var row = inAppProductTable.getRow<InAppProductRow>(rowId);

            var product = controller.products.WithID(row.productId);
            if (null != product && product.hasReceipt)
            {
                if (Logx.isActive)
                {
                    Logx.trace("hasReceipt {0}", row.productId);
                }

                if (row.isNoAds)
                    m_isNoAds = row.isNoAds;
            }
        }
    }
    */

    private void openFailedBuyInAppMsgBox(PurchaseFailureReason failureReason)
    {
        if (PurchaseFailureReason.UserCancelled == failureReason)
            return;

        string title = StringHelper.get("information");
        string body = StringHelper.get("system_failed_buy_inapp", (int)failureReason);

    }

    public void buy(int inAppProductId, Action<bool> callback = null)
    {
        var inAppProductRow = GameTableHelper.instance.getRow<InAppProductRow>((int)eTable.InAppProduct, inAppProductId);

#if UNITY_EDITOR
        if (DebugSettings.instance.isDebug)
        {
            //if (inAppProductRow.isNoAds)
            //    setNoAds();

            callback(true);
            return;
        }
#endif
        GameHelper.ignoreApplicationPauseAction(true);


        buy(inAppProductRow.productId, (isSuccess, failureReason, productId) =>
        {
            callback(isSuccess);
            //var cloudData = new OptionCloudData();
            //cloudData.save(() =>
            //{
            // callback(isSuccess);
            // });
        });
    }

    private void setNoAds()
    {
        var req = new Req_SetNoAds();
        GameLocalDataHelper.getInstance().request<Res_SetNoAds>(req, (res) =>
        {
            if (res.isSuccess)
            {
                m_isNoAds = true;

                if (Logx.isActive)
                    Logx.trace("setNoAds {0}", m_isNoAds);
            }
        });
    }

    private void getNoAds()
    {
        var req = new Req_GetNoAds();
        GameLocalDataHelper.getInstance().request<Res_GetNoAds>(req, (res) =>
        {
            if (res.isSuccess)
            {
                m_isNoAds = res.isNoAds;

                if (Logx.isActive)
                    Logx.trace("getNoAds {0}", m_isNoAds);
            }
        });
    }

    protected override void openInternetReachableMsgBox()
    {
        string title = StringHelper.get("information");
        string body = StringHelper.get("system_internet_connect");
    }

    public string getPrice(int productId)
    {
        var productRow = GameTableHelper.instance.getRow<InAppProductRow>((int)eTable.InAppProduct, productId);
        var price = getPrice(productRow.productId);
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(price))
            return "Free";
#endif
        return price;
    }

#else
    /// <param name="callback">isSuccess</param>
    public void buy(int inAppProductId, Action<bool> callback = null)
    {
        callback?.Invoke(false);
    }

    public string getPrice(int productId)
    {
        return "Error";
    }
#endif
}
