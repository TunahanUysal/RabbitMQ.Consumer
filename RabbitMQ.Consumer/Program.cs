// Baglantı olusturma

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();

factory.Uri = new("amqpuri");


//Baglantı aktiflestirme ve Kanal acma
using IConnection connection = factory.CreateConnection();

IModel channel = connection.CreateModel();

//Queue olusturma

channel.QueueDeclare(queue: "examplequeue", exclusive: false); // Consumerda bir kuyruk publisherdaki ile birebir aynı yapılandırma tanımlanmalıdır!

//Queue!dan mesaj okuma 

EventingBasicConsumer consumer = new(channel);

channel.BasicConsume(queue: "examplequeue", autoAck:false, consumer:consumer); // autoAck ozelligi false ise  otomatik mesajı silmez ve consumerdan onay bekler 

consumer.Received += (sender, e) =>
{
    //Kuyruga gelen mesajın işlendigi deger 

    //e.body:Kuyruktaki mesajın verisini bütünsel getirecek

    //e.Body.Span veya e.Body.Toarray(): Kuyruktaki mesajın byte verisini getirecektir

    string message = Encoding.UTF8.GetString(e.Body.Span);

    Console.WriteLine(message);

    channel.BasicAck(e.DeliveryTag, multiple: false);// false verilirse sadece bu mesaj için onay verir.True ise budan onceki mesajlar için de onay verir.

    channel.BasicNack(deliveryTag: e.DeliveryTag, multiple: false, requeue: true); // RabitMqQ ya bilgi verebilir ve mesajı tekrardan işletebiliriz.Requeue parametresi true olursa mesaj işlenmek üzere tekrardn gonderilecek.Eger false olursa sadece mesajın işlenmedigi bilgisi RabbitMQ ' ya bildirilir.
};




Console.Read();