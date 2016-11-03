namespace KKings.Foundation.Popsicle
{
    using Analytics;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;

    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IEventTracker, EventTracker>();
        }
    }
}