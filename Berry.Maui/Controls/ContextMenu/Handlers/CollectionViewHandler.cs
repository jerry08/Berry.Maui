using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers.Items;

namespace Berry.Maui.Controls;

public class CollectionViewHandler : ReorderableItemsViewHandler<ReorderableItemsView>
{
#if IOS
    public CollectionViewHandler()
    {
        ReorderableItemsViewMapper.ModifyMapping(
            SelectableItemsView.SelectionModeProperty.PropertyName,
            MapSelectionMode
        );
    }

    private void MapSelectionMode(
        ReorderableItemsViewHandler<ReorderableItemsView> handler,
        ReorderableItemsView view,
        System.Action<IElementHandler, IElement>? action
    )
    {
        var ctrl = (handler.ViewController as SelectableItemsViewController<ReorderableItemsView>);
        if (ctrl is null)
        {
            return;
        }
        ctrl.CollectionView.AllowsSelection = true;
        ctrl.CollectionView.AllowsMultipleSelection = false;
    }

    protected override CollectionViewController CreateController(
        ReorderableItemsView itemsView,
        ItemsViewLayout layout
    ) => new(itemsView, layout);
#endif
}
