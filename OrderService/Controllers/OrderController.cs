using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IProducer<Null, string> _producer;

    public OrderController(IProducer<Null, string> producer)
    {
        _producer = producer;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder()
    {
        var order = new { OrderId = Guid.NewGuid(), Timestamp = DateTime.UtcNow };
        var json = JsonSerializer.Serialize(order);

        await _producer.ProduceAsync("orders", new Message<Null, string> { Value = json });

        return Ok(new { message = "Order sent to Kafka", order });
    }
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Order service is running");
    }
}
