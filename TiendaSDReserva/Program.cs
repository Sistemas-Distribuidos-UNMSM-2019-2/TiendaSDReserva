using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TiendaSDReserva.ComponenteDatos;
using TiendaSDReserva.Model;

namespace TiendaSDReserva
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConsumerConfig
            {
                GroupId = "re",
                BootstrapServers = "192.168.4.17:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe("reserva");

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true;
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = consumer.Consume(cts.Token);
                            Console.WriteLine(cr.Value);

                            OrdenCompraModel ordenCompra = JsonConvert.DeserializeObject<OrdenCompraModel>(cr.Value);
                            ordenCompra.sEstado = "reservado";

                            ProductoDAO productoDAO = new ProductoDAO();
                            productoDAO.actualizarStock(ordenCompra.lDetalleCompra);

                            OrdenCompraDAO ordenCompraDAO = new OrdenCompraDAO();
                            ordenCompraDAO.registrarOrdenCompra(ordenCompra);

                            var config2 = new ProducerConfig { BootstrapServers = "192.168.4.17:9092" };

                            Action<DeliveryReport<Null, string>> handler = r =>
                                Console.WriteLine(!r.Error.IsError
                                ? $"Delivered message to {r.TopicPartitionOffset}"
                                : $"Delivery Error: {r.Error.Reason}");

                            using (var producer = new ProducerBuilder<Null, string>(config2).Build())
                            {
                                producer.ProduceAsync("factura", new Message<Null, string> { Value = JsonConvert.SerializeObject(ordenCompra) });

                                producer.Flush(TimeSpan.FromSeconds(10));
                            }

                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occured: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    consumer.Close();
                }
            }
        }
    }
}
