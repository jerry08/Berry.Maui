using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace Berry.Maui.Handlers;

public partial class PickerViewHandler : PickerHandler
{
    public PickerViewHandler() { }

    public PickerViewHandler(IPropertyMapper? mapper = null)
        : base(mapper) { }
}
