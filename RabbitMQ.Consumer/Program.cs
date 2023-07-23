// Baglantı olusturma

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();

factory.Uri = new("amqps://pntltxss:IMt0DR7p-3U2a7kzUjFVJCHEw9VttGiF@woodpecker.rmq.cloudamqp.com/pntltxss");


//Baglantı aktiflestirme ve Kanal acma
using IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "direct-exchange-example", type: ExchangeType.Direct);

string queueName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queueName, exchange: "direct-exchange-example", routingKey: "direct-queue-example");

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

consumer.Received += (sender, e) =>
{
    string message = Encoding.UTF8.GetString((e.Body.Span));

    Console.WriteLine(message);
};

//Queue olusturma

//channel.QueueDeclare(queue: "examplequeue", exclusive: false,durable:true); // Consumerda bir kuyruk publisherdaki ile birebir aynı yapılandırma tanımlanmalıdır!

////Queue!dan mesaj okuma 

//EventingBasicConsumer consumer = new(channel);

//var consumerTag=channel.BasicConsume(queue: "examplequeue", autoAck:false, consumer:consumer); // autoAck ozelligi false ise  otomatik mesajı silmez ve consumerdan onay bekler 

//channel.BasicQos(0, 1, false);

////channel.BasicCancel(consumerTag);

//consumer.Received += (sender, e) =>
//{
//    //Kuyruga gelen mesajın işlendigi deger 

//    //e.body:Kuyruktaki mesajın verisini bütünsel getirecek

//    //e.Body.Span veya e.Body.Toarray(): Kuyruktaki mesajın byte verisini getirecektir

//    string message = Encoding.UTF8.GetString(e.Body.Span);

//    Console.WriteLine(message);

//    channel.BasicAck(e.DeliveryTag, multiple: false);// false verilirse sadece bu mesaj için onay verir.True ise budan onceki mesajlar için de onay verir.

//    //channel.BasicNack(deliveryTag: e.DeliveryTag, multiple: false, requeue: true); // RabitMqQ ya bilgi verebilir ve mesajı tekrardan işletebiliriz.Requeue parametresi true olursa mesaj işlenmek üzere tekrardn gonderilecek.Eger false olursa sadece mesajın işlenmedigi bilgisi RabbitMQ ' ya bildirilir.

//    //channel.BasicReject(deliveryTag:3,requeue: true); // kuyrukta bulunan mesajlardan belirli alanların consumer tarafından  işlenmesini istemedigimiz durumlarda BasicReject metodunu kullanabiliriz.
//};




Console.Read();