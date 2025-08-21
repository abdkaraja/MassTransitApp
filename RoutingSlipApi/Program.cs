using MassTransit;
using RoutingSlipApi.Activities;
using RoutingSlipApi.Consumers;
using RoutingSlipApi.Models.Arguments;
using RoutingSlipApi.Models.Logs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// إضافة خدمات Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    // Add consumers
    x.AddConsumer<OrderSubmittedConsumer>();
    x.AddConsumer<RoutingSlipCompletedConsumer>();
    x.AddConsumer<RoutingSlipFaultedConsumer>();

    // Add activities
    x.AddActivity<ProcessPaymentActivity, ProcessPaymentArguments, PaymentProcessedLog>();
    x.AddActivity<SendConfirmationEmailActivity, SendConfirmationEmailArguments, EmailSentLog>();

    x.UsingRabbitMq((context, cfg) =>
    {

        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("process-payment", e =>
        {
            e.ExecuteActivityHost<ProcessPaymentActivity, ProcessPaymentArguments>();
        });

        cfg.ReceiveEndpoint("send-confirmation-email", e =>
        {
            e.ExecuteActivityHost<SendConfirmationEmailActivity, SendConfirmationEmailArguments>();
        });

        // Configure consumer endpoints
        cfg.ReceiveEndpoint("order-processing", e =>
        {
            e.ConfigureConsumer<OrderSubmittedConsumer>(context);
        });

        cfg.ReceiveEndpoint("routing-slip-events", e =>
        {
            e.ConfigureConsumer<RoutingSlipCompletedConsumer>(context);
            e.ConfigureConsumer<RoutingSlipFaultedConsumer>(context);
        });

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
