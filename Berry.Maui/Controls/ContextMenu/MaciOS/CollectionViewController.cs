using Foundation;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Handlers.Items;
using UIKit;

namespace Berry.Maui.Controls;

public class CollectionViewController : ReorderableItemsViewController<ReorderableItemsView>
{
    public CollectionViewController(
        ReorderableItemsView reorderableItemsView,
        ItemsViewLayout layout
    )
        : base(reorderableItemsView, layout) { }

    protected override UICollectionViewDelegateFlowLayout CreateDelegator()
    {
        return new CollectionViewDelegator(ItemsViewLayout, this);
    }

    protected override void UpdateDefaultCell(DefaultCell cell, NSIndexPath indexPath)
    {
        base.UpdateDefaultCell(cell, indexPath);
        if (ItemsView.SelectionMode == SelectionMode.None)
        {
            cell.SelectedBackgroundView = null;
        }
    }

    protected override void UpdateTemplatedCell(TemplatedCell cell, NSIndexPath indexPath)
    {
        base.UpdateTemplatedCell(cell, indexPath);
        if (ItemsView.SelectionMode == SelectionMode.None)
        {
            cell.SelectedBackgroundView = null;
        }
    }
}
