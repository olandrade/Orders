using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Repositories.Implementations;
using Orders.Backend.Repositories.Interfaces;
using Orders.Backend.UnitsOfWork.Implementations;
using Orders.Backend.UnitsOfWork.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer("name=DKConn"));
builder.Services.AddTransient<SeedDb>(); //AddTransient - para uso minimo

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));   //       AddScoped - para uso multiproceso
builder.Services.AddScoped(typeof(IGenericUnitsOfWork<>), typeof(GenericUnitsOfWork<>));

builder.Services.AddScoped(typeof(ICountriesRepository), typeof(CountriesRepository));
builder.Services.AddScoped(typeof(IStatesRepository), typeof(StatesRepository));
builder.Services.AddScoped(typeof(ICitiesRepository), typeof(CitiesRepository));

builder.Services.AddScoped(typeof(ICountriesUnitOfWork), typeof(CountriesUnitOfWork));
builder.Services.AddScoped(typeof(IStatesUnitOfWork), typeof(StatesUnitOfWork));
builder.Services.AddScoped(typeof(ICitiesUnitOfWork), typeof(CitiesUnitOfWork));

var app = builder.Build();
SeedDb(app);

void SeedDb(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (var scope = scopedFactory!.CreateScope()) { 
        var service = scope.ServiceProvider.GetService<SeedDb>();
        service!.SeedAsync().Wait();
    }
}

app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .SetIsOriginAllowed(origin => true)
        .AllowCredentials());

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();