using System;
using System.Collections.Generic;
#if _INAPP_
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.Purchasing.Security;
using UnityEngine;
#endif

namespace UnityHelper
{
#if _INAPP_
    public class InAppHelper : MonoSingleton<InAppHelper>, IDetailedStoreListener
    {
        private IStoreController m_storeController = null;
        private IExtensionProvider m_storeExtensionProvider = null;
        /// <summary>
        /// <is success, PurchaseFailureReason, productId>
        /// </summary>
        private Action<bool, PurchaseFailureReason, string> m_purchaseCallback = null;
        private CrossPlatformValidator m_validator = null;

        public void initialize()
        {
            if (isInitialized())
                return;

            UnityPurchasing.Initialize(this, getBuilder());
        }

        protected virtual ConfigurationBuilder getBuilder()
        {
            if (Logx.isActive)
                Logx.error("Must override getBuilder");

            return null;
        }

        private bool isInitialized()
        {
            return m_storeController != null && m_storeExtensionProvider != null;
        }

        /// <param name="callback">isSuccess, failureReason, productId</param>
        public void buy(string productId, Action<bool, PurchaseFailureReason, string> callback = null)
        {
            if (Logx.isActive)
                Logx.trace(">>>>>>>>>>>>>>>>>>>>> IAP store buy {0}", productId);

            
#if UNITY_EDITOR
            if (null != callback)
                callback(true, 0, productId);
            return;
#else
            if (null == m_storeController)
            {
                if (null != callback)
                    callback(false, PurchaseFailureReason.PurchasingUnavailable, productId);
                return;
            }

            Product product = m_storeController.products.WithID(productId);

            if (null != product && product.availableToPurchase)
            {
                m_purchaseCallback = callback;

#if UNITY_ANDROID
                // AndroidBackButton.instance.isPurchasing = true;
#endif
                m_storeController.InitiatePurchase(product);
            }
            else
            {
                if (Logx.isActive)
                    Logx.trace(">>>>>>>>>>>>>>>>>>>>> IAP store failed buy {0}", productId);
            }
#endif
        }

        /// <param name="callback">isSuccess, failureReason, productId</param>
        public void RestorePurchases(Action<bool> callback)
        {
            if (Logx.isActive)
                Logx.trace(">>>>>>>>>>>>>>>>>>>>> IAP restorePurchases");

            if (!SystemHelper.isInternetReachable())
            {
                // openInternetReachableMsgBox();

                if (null != callback)
                    callback(false);
                return;
            }

#if UNITY_EDITOR
            if (null != callback)
            {
                callback(true);
            }
            return;
#else

            if (!isInitialized())
            {
                if (null != callback)
                    callback(false);

                return;
            }

            setActiveBlock(true);

            var apple = m_storeExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((success) =>
            {
                setActiveBlock(false);

                if (Logx.isActive)
                    Logx.trace(">>>>>>>>>>>>>>>>>>>>> IAP RestorePurchases continues : {0}", success);

                callback(success);
            });
#endif
        }

        private void initializeValidator()
        {
            try
            {
                m_validator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
            }
            catch (NotImplementedException exception)
            {
                if (Logx.isActive)
                    Logx.trace("Cross Platform Validator Not Implemented: " + exception);
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            if (Logx.isActive)
                Logx.trace(">>>>>>>>>>>>>>>>>>>>> IAP OnInitialized");

            m_storeController = controller;
            m_storeExtensionProvider = extensions;

            initializeValidator();
            initializedStore(controller);
        }

        protected virtual void initializedStore(IStoreController controller)
        {

        }

        public void getLocalizedInfo(string productId, out string title, out string price)
        {
            if (Logx.isActive)
                Logx.trace(">>>>>>>>>>>>>>>>>>>>> IAP getLocalizedInfo {0}", productId);

#if UNITY_EDITOR
            title = "";
            price = "Editor Free";
#else
            title = "";
            price = "";

            if (null == m_storeController)
                return;

            foreach (var product in m_storeController.products.all)
            {
                if (string.Equals(product.definition.id, productId, System.StringComparison.Ordinal))
                {
                    title = product.metadata.localizedTitle;
                    price = product.metadata.localizedPriceString;
                    return;
                }
            }
#endif
        }

        public string getPrice(string productId)
        {
            if (Logx.isActive)
                Logx.trace(">>>>>>>>>>>>>>>>>>>>> IAP getPrice {0}", productId);

#if UNITY_EDITOR
            return null;
#else
            if (null == m_storeController)
                return null;

            foreach (var product in m_storeController.products.all)
            {
                if (string.Equals(product.definition.id, productId, System.StringComparison.Ordinal))
                {
                    return product.metadata.localizedPriceString;
                }
            }

            return null;
#endif
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            if (Logx.isActive)
            {
                var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

                if (message != null)
                {
                    errorMessage += $" More details: {message}";
                }

                Logx.trace(errorMessage);
            }
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            if (Logx.isActive)
                Logx.error(">>>>>>>>>>>>>>>>>>>>> IAP OnInitializeFailed {0}", error);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            if (Logx.isActive)
            {
                Logx.trace($"Purchase failed - Product: '{product.definition.id}'," +
                           $" Purchase failure reason: {failureDescription.reason}," +
                           $" Purchase failure details: {failureDescription.message}");
            }

            if (null != m_purchaseCallback)
            {
                m_purchaseCallback(false, failureDescription.reason, product.definition.id);
                m_purchaseCallback = null;
            }

#if UNITY_ANDROID
            // AndroidBackButton.instance.isPurchasing = false;
#endif
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason p)
        {
            string productId = product.definition.id;

            if (Logx.isActive)
                Logx.error(">>>>>>>>>>>>>>>>>>>>> IAP OnPurchaseFailed {0}, resion {1}", productId, p);

            if (null != m_purchaseCallback)
            {
                m_purchaseCallback(false, p, productId);
                m_purchaseCallback = null;
            }

#if UNITY_ANDROID
            // AndroidBackButton.instance.isPurchasing = false;
#endif
        }

        private bool isPurchaseValid(Product product)
        {
            /*
             * 로컬 영수증 인증
             * */
            try
            {
                var result = m_validator.Validate(product.receipt);
                if (Logx.isActive)
                    Logx.trace("Receipt is valid. Contents:");

                foreach (IPurchaseReceipt productReceipt in result)
                {
                    if (Logx.isActive)
                    {
                        Logx.trace("productID {0}", productReceipt.productID);
                        Logx.trace("purchaseDate {0}", productReceipt.purchaseDate);
                        Logx.trace("transactionID {0}", productReceipt.transactionID);
                    }

                    GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
                    if (null != google)
                    {
                        if (Logx.isActive)
                        {
                            Logx.trace("purchaseState {0}", google.purchaseState.ToString());
                            Logx.trace("purchaseToken {0}", google.purchaseToken);
                        }
                    }

                    AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
                    if (null != apple)
                    {
                        if (Logx.isActive)
                        {
                            Logx.trace("originalTransactionIdentifier {0}", apple.originalTransactionIdentifier);
                            Logx.trace("subscriptionExpirationDate {0}", apple.subscriptionExpirationDate);
                            Logx.trace("cancellationDate {0}", apple.cancellationDate);
                            Logx.trace("quantity {0}", apple.quantity);
                        }
                    }
                }
            }
            catch (IAPSecurityException ex)
            {
                if (Logx.isActive)
                    Logx.trace("Invalid receipt, not unlocking content. " + ex);
                return false;
            }

            return true;
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            bool isValidPurchase = isPurchaseValid(e.purchasedProduct);

            string productId = e.purchasedProduct.definition.id;

            if (Logx.isActive)
                Logx.trace(">>>>>>>>>>>>>>>>>>>>> IAP ProcessPurchase {0}", productId);

            if (null != m_purchaseCallback)
            {
                m_purchaseCallback(isValidPurchase, 0, productId);
                m_purchaseCallback = null;
            }

            StartCoroutine(coReleasePurchasePause());

            return PurchaseProcessingResult.Complete;
        }

        /// <summary>
        /// 확인창 때문에 백그라운드로 2번 가는거 같다. 딜레이를 줌..;
        /// </summary>
        /// <returns></returns>
        IEnumerator coReleasePurchasePause()
        {
            yield return new WaitForSeconds(0.5f);

#if UNITY_ANDROID
            // AndroidBackButton.instance.isPurchasing = false;
#endif
        }

        protected virtual void openInternetReachableMsgBox()
        {

        }

        protected virtual void setActiveBlock(bool isActive)
        {

        }
    }
#else
    public class InAppHelper : MonoSingleton<InAppHelper>
    {
        public virtual void initialize() { }
    }
#endif
}