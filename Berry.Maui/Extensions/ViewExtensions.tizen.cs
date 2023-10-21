namespace Berry.Maui.Extensions;

public class ViewExtensions
{
    internal static NView? GetParent(this NView? view)
    {
        return view?.GetParent() as NView;
    }
}
