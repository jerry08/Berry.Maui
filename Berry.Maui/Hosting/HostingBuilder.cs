using Microsoft.Maui.Hosting;

namespace Berry.Maui;

public class HostingBuilder
{
    private readonly MauiAppBuilder _builder;

    public HostingBuilder(MauiAppBuilder builder)
    {
        _builder = builder;
    }

    public HostingBuilder UseBottomSheet()
    {
        _builder.UseBottomSheet();
        return this;
    }

    public HostingBuilder UseContextMenu()
    {
        _builder.UseContextMenu();
        return this;
    }
    
    public HostingBuilder UseAcrylicView()
    {
        _builder.UseAcrylicView();
        return this;
    }
    
    public HostingBuilder UseMauiPlainer()
    {
        _builder.UseMauiPlainer();
        return this;
    }
    
    public HostingBuilder UseInsets()
    {
        _builder.UseInsets();
        return this;
    }
    
    public HostingBuilder UseMaterialSwitch()
    {
        _builder.UseMaterialSwitch();
        return this;
    }
}
