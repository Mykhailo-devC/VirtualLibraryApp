global using VirtualLibrary.Models;
global using Serilog;
using Microsoft.EntityFrameworkCore;
using VirtualLibrary.Utilites.Implementations;
using System.Text.Json.Serialization;
using VirtualLibrary.Utilites.Interfaces;
using VirtualLibrary.Utilites.Implementations.DataStore;

class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Host.UseSerilog((context, config) => config
                                .WriteTo.Console());
        builder.Services.AddDbContext<VirtualLibraryDbContext>(opt =>
        {
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
        builder.Services.AddControllers().AddJsonOptions(opt =>
                                           opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        AddUserServices(builder);


        var app = builder.Build();

        // Configure the HTTP request pipeline.

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    private static void AddUserServices(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<RepositoryFactory>();

        builder.Services.AddScoped<IDataStore<Book, BookDTO>, BookDataStore>();
        builder.Services.AddScoped<IDataStore<Article, ArticleDTO>, ArticleDataStore>();
        builder.Services.AddScoped<IDataStore<Magazine, MagazineDTO>, MagazineDataStore>();
        builder.Services.AddScoped<IDataStore<Publisher, PublisherDTO>, PublisherDataStore>();
    }
}
