namespace Berry.Maui.Controls;

public partial class FullscreenDetent : Detent
{
    public override double GetHeight(BottomSheet page, double maxSheetHeight)
    {
        return maxSheetHeight;
    }
}
