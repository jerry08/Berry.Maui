namespace Berry.Maui.Effects.Extensions;

public class ViewExtensions
{
    internal static NView? GetParent(this NView? view)
    {
        return view?.GetParent() as NView;
    }
}
