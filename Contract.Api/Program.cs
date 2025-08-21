using Contract.Api;
using Contract.Api.Consumers;
using Contract.Api.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<ContractDbContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
        });

        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.AddConsumers(typeof(CalculateCommissionConsumer).Assembly);

            x.UsingRabbitMq((context, cfg) =>
            {

                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                //cfg.UseInMemoryOutbox(context);
                cfg.ConfigureEndpoints(context);
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}