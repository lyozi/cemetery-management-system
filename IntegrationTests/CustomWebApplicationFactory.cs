//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.EntityFrameworkCore;
//using Infrastructure.Context;

//public class CustomWebApplicationFactory<Program> : WebApplicationFactory<Program> where Program : class
//{
//  protected override void ConfigureWebHost(IWebHostBuilder builder)
//  {
//    builder.ConfigureServices(services =>
//    {
//      var descriptor = services.SingleOrDefault(
//          d => d.ServiceType ==
//              typeof(DbContextOptions<DatabaseContext>));

//      if (descriptor != null)
//      {
//        services.Remove(descriptor);
//      }

//      services.AddDbContext<DatabaseContext>(options =>
//      {
//        options.UseInMem111oryDatabase("InMemoryDbForTesting");
//      });

//      var serviceProvider = services.BuildServiceProvider();

//      using (var scope = serviceProvider.CreateScope())
//      {
//        var scopedServices = scope.ServiceProvider;
//        var db = scopedServices.GetRequiredService<DatabaseContext>();

//        db.Database.EnsureCreated();
//      }
//    });
//  }
//}