﻿.NET MAUI

## Initializing

In order to use the .NET MAUI MediaElement you need to call the extension method in your `MauiProgram.cs` file as follows:

```csharp
using Berry.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			// Initialize the .NET MAUI MediaElement by adding the below line of code
			.UseBerryMediaElement();

		// Continue initializing your .NET MAUI App here

		return builder.Build();
	}
}
```

## XAML usage

In order to make use of the toolkit within XAML you can use this namespace:

xmlns:berry="https://github.com/jerry08/Berry.Maui"

## Further information

For more information please visit:

- GitHub repository: https://github.com/jerry08/Berry.Maui