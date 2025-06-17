using Confluent.Kafka;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddSingleton(new ProducerBuilder<Null, string>(
    new ProducerConfig { BootstrapServers = "kafka:9092" }).Build());

var app = builder.Build();
app.MapControllers(); 

app.Run();