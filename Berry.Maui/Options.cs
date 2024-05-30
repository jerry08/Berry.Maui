using Berry.Maui.Behaviors;

namespace Berry.Maui;

public class Options()
{
    internal static bool UsePlainer { get; private set; }

    internal static bool ShouldSuppressExceptionsInBehaviors { get; private set; }

    /// <summary>
    /// Allows to return default value instead of throwing an exception when using <see cref="BaseBehavior{TView}"/>.
    /// </summary>
    /// <remarks>
    /// Default value is false.
    /// </remarks>
    public static void SetShouldSuppressExceptionsInBehaviors(bool value) =>
        ShouldSuppressExceptionsInBehaviors = value;

    public static void SetUsePlainer(bool value) => UsePlainer = value;
}
