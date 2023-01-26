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
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("VirtualLibrary"));
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
        builder.Services.AddScoped<IRepository<BookCopy, BookDTO>, BookRepository>();
        builder.Services.AddScoped<IRepository<ArticleCopy, ArticleDTO>, ArticleRepository>();
        builder.Services.AddScoped<IRepository<MagazineCopy, MagazineDTO>, MagazineRepository>();
        builder.Services.AddScoped<IRepository<Publisher, PublisherDTO>, PublisherRepository>();

        builder.Services.AddScoped<IModelLogic<BookCopy, BookDTO>, BookLogic>();
        builder.Services.AddScoped<IModelLogic<ArticleCopy, ArticleDTO>, ArticleLogic>();
        builder.Services.AddScoped<IModelLogic<MagazineCopy, MagazineDTO>, MagazineLogic>();
        builder.Services.AddScoped<IModelLogic<Publisher, PublisherDTO>, PublisherLogic>();
    }
}
