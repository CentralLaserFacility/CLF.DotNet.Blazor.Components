//
// ViewModelDiscovery.cs
//

using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;

namespace Clf.LogicSystem.Common.DependencyInjection
{

  public static class ViewModelDiscovery
  {

    public static void AddViewModels ( this IServiceCollection serviceCollection )
    {
      IEnumerable<System.Type> viewModelTypes_implementingINotifyPropertyChanged = (
        System.AppDomain.CurrentDomain.GetAssemblies(
        ).SelectMany(
          assembly => assembly.GetTypes()
        ).Where(
          type => type.IsSubclassOf(
            typeof(System.ComponentModel.INotifyPropertyChanged)
          )
        )
      ) ;
      foreach (
        System.Type viewModelType_implementingINotifyPropertyChanged 
        in viewModelTypes_implementingINotifyPropertyChanged 
      ) {
        serviceCollection.AddTransient(
          serviceType : viewModelType_implementingINotifyPropertyChanged
        ) ;
      }
    }

    // https://learn.microsoft.com/en-us/dotnet/core/extensions/configuration?source=recommendations

    // public static void AddConfiguration ( 
    //   this IServiceCollection serviceCollection, 
    //   string                  fileName = "appsettings.json"
    // ) {
    //   var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
    //       // .SetBasePath(AppContext.BaseDirectory)
    //       .AddJsonFile(fileName, false, true);
    //   var configuration = builder.Build() as Microsoft.Extensions.Configuration.IConfiguration;
    //   serviceCollection.AddSingleton(configuration) ;
    // }

  }

}

