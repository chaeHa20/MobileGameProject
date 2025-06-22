namespace UnityHelper
{
    public class UIPanelData : UIWidgetData
    {

    }

    public class UIPanel : UIWidget
    {
        public override void open()
        {
            base.open();
        }

        public override void onClose()
        {
            UIHelper.instance.closePanel(name);
        }

        public override void onAnimationCloseEndEvent()
        {
            UIHelper.instance.closePanel(name);
        }
    }
}