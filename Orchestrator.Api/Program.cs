using Account.Api.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Orchestrator.Api.Activities;
using Orchestrator.Api.Contexts;
using Orchestrator.Api.Saga;
using Orchestrator.Api.StateMachineInstances;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        //builder.Services.AddSingleton<IEndpointAddressProvider, RabbitMqEndpointAddressProvider>();
        builder.Services.AddDbContext<OrchestratorContext>(opt =>
        {
            opt.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
        });
        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            //x.AddActivitiesFromNamespaceContaining<OrchestratorNamespace>();
            x.AddActivity<UpdateAmountActivity, UpdateAmountArguments, UpdateAmountLog>();
            x.AddActivity<CalculateCommissionActivity, CalculateCommissionArguments, CalculateCommissionLog>();
            x.AddActivity<AddTransactionLogActivity, AddTransactionLogArguments, AddTransactionLog>();
            x.AddConsumer<TransferConsumer>();
            x.AddSagaStateMachine<TransferStateMachine, TransferStateMachineInstance>()
            .EntityFrameworkRepository(r =>
            {
                r.ExistingDbContext<OrchestratorContext>();
                r.UsePostgres();
            });


            // Configure transport (example with RabbitMQ)
            x.UsingRabbitMq((context, cfg) =>
            {

                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                //cfg.ReceiveEndpoint("update-amount_execute", e =>
                //{
                //    e.ExecuteActivityHost<UpdateAmountActivity, UpdateAmountArguments>(context);
                //});

                //cfg.ReceiveEndpoint("update-amount_compensate", e =>
                //{
                //    e.CompensateActivityHost<UpdateAmountActivity, UpdateAmountLog>(context);
                //});
                //cfg.ReceiveEndpoint("reserve-inventory_execute", e =>
                //{
                //    e.ExecuteActivityHost<ReserveInventoryActivity, ReserveInventoryArguments>();
                //});

                //cfg.ReceiveEndpoint("reserve-inventory_compensate", e =>
                //{
                //    e.CompensateActivityHost<ReserveInventoryActivity, InventoryLog>();
                //});

                //cfg.ReceiveEndpoint("charge-payment_execute", e =>
                //{
                //    e.ExecuteActivityHost<ChargePaymentActivity, ChargePaymentArguments>();
                //});

                //cfg.ReceiveEndpoint("charge-payment_compensate", e =>
                //{
                //    e.CompensateActivityHost<ChargePaymentActivity, PaymentLog>();
                //});

                cfg.ConfigureEndpoints(context);


                //cfg.UseInMemoryOutbox(context);

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