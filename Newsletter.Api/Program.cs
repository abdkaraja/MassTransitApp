using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api;
using Newsletter.Api.Consumers;
using Newsletter.Api.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// إضافة خدمات Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});

var assembly = typeof(Article).Assembly;

builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumer<GetArticleDetailConsumer>();

    // Configure transport (example with RabbitMQ)
    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        //var endpointNameFormatter = context.GetRequiredService<IEndpointNameFormatter>();
        //EndpointConvention.Map<GetArticleDetailConsumer>(new Uri($"queue:{endpointNameFormatter.Consumer<GetArticleDetailConsumer>()}"));

        cfg.UseInMemoryOutbox(context);
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
