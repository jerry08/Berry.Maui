using Microsoft.Maui;
using Microsoft.Maui.Handlers;

namespace Berry.Maui.Handlers;

public partial class EditorViewHandler : EditorHandler
{
    public EditorViewHandler() { }

    public EditorViewHandler(IPropertyMapper? mapper = null)
        : base(mapper) { }
}
