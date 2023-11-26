using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace Berry.Maui.Controls;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class UnderlinedTabItem : UnderlinedTabItemBase
{
    public UnderlinedTabItem()
    {
        InitializeComponent();

        InnerLabel.PropertyChanged += ContentImplPropertyChanged;
    }

    protected override Label InnerLabelImpl => InnerLabel;

    protected override Grid GridImpl => Grid;

    protected override BoxView UnderlineImpl => Underline;

    protected override View ContentImpl => InnerLabel;
}
