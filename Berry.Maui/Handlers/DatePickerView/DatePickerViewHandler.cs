using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace Berry.Maui.Handlers;

public partial class DatePickerViewHandler : DatePickerHandler
{
    public DatePickerViewHandler() { }

    public DatePickerViewHandler(IPropertyMapper mapper)
        : base(mapper) { }
}
