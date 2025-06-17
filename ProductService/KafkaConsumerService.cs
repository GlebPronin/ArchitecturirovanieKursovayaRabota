using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class KafkaConsumerService : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		var config = new ConsumerConfig
		{
			GroupId = "product-consumer-group",
			BootstrapServers = "kafka:9092",
			AutoOffsetReset = AutoOffsetReset.Earliest
		};

		using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
		consumer.Subscribe("orders");

		try
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				var result = consumer.Consume(stoppingToken);
				Console.WriteLine($"[ProductService] Получено сообщение: {result.Message.Value}");
			}
		}
		catch (OperationCanceledException)
		{ 
		}
		finally
		{
			consumer.Close();
		}
	}
}
