using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace Berry.Maui.Handlers;

public partial class EntryViewHandler : EntryHandler
{
    public EntryViewHandler() { }

    public EntryViewHandler(IPropertyMapper? mapper = null)
        : base(mapper) { }
}
