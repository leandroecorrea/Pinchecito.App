using Microsoft.Extensions.Logging;
using Pinchecito.Core.Interfaces;
using Pinchecito.Infrastructure.DataAccess;
using Pinchecito.Infrastructure.HtmlParsers;
using Pinchecito.Infrastructure.Repositories;
using Pinchecito.Infrastructure.Scrapers;
using Pinchecito.Infrastructure.Services;
using Pinchecito.UI.ViewModel;
using Pinchecito.UI.Views;

namespace Pinchecito.UI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddTransient<TrackFileViewModel>();
		builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<HomePage>();
        builder.Services.AddTransient<TrackFilePage>();
        builder.Services.AddTransient<ILoginService, LoginService>();
		builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<HomeViewModel>();
        builder.Services.AddSingleton<PinchecitoDatabase>();
        builder.Services.AddSingleton<MongoDatabase>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        builder.Services.AddTransient<ISessionCookieRepository, SessionCookieRepository>();
        builder.Services.AddTransient<ITrackFileService, TrackFileService>();
		builder.Services.AddTransient<HtmlDocumentBuilder>();
        builder.Services.AddTransient<SearchResultPageParser>();
        builder.Services.AddTransient<SearchPageScraper>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
