global using VirtualLibrary.Models;
global using Serilog;

using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using VirtualLibrary.Repository.Interface;
using VirtualLibrary.Repository.Implementation;
using VirtualLibrary.Logic.Implementation;
using VirtualLibrary.Logic.Interface;

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
        builder.Services.AddSingleton<IRepository<Book, BookDTO>, BookRepository>();
        builder.Services.AddSingleton<IRepository<Article, ArticleDTO>, ArticleRepository>();
        builder.Services.AddSingleton<IRepository<Magazine, MagazineDTO>, MagazineRepository>();
        builder.Services.AddSingleton<IRepository<Publisher, PublisherDTO>, PublisherRepository>();

        builder.Services.AddSingleton<IModelLogic<Book, BookDTO>, BookLogic>();
        builder.Services.AddSingleton<IModelLogic<Article, ArticleDTO>, ArticleLogic>();
        builder.Services.AddSingleton<IModelLogic<Magazine, MagazineDTO>, MagazineLogic>();
        builder.Services.AddSingleton<IModelLogic<Publisher, PublisherDTO>, PublisherLogic>();
    }
}
