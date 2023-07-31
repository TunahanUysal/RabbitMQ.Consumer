// Baglantı olusturma

using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Runtime.CompilerServices;
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


// interface bir sınıfın belirli ozelliklere veya davranıslara sahip olması için tanımlanan bir sözleşmedir.İnterface bir sablondur.MEtodları imzalrı zorunlu olur ama işlevselligi belirtilmez.

CustomerManager customerManager = new CustomerManager(new SecondLegislation());

customerManager.Add();

class CustomerManager
{
    private readonly ILegislation _legislation;

    public CustomerManager(ILegislation legislation)
    {
        _legislation = legislation;
    }

    public void Add()
    {
        _legislation.DoTransaction();
    }
}


interface ILegislation
{
    void DoTransaction();
}

class FirstLegislation : ILegislation
{
    public void DoTransaction()
    {
        Console.WriteLine("Processed according to the first legislation.");
        Console.Read();
    }
}

class SecondLegislation : ILegislation
{
    public void DoTransaction()
    {
        Console.WriteLine("Processed according to the second legislation.");
        Console.Read();
    }
}


// Solid  Yazılımın surdurebilirligını saglamak için gerekli prensiplerdir.5 adet prensip

// Single Responsibility Principle ==> Tek sorumluluk 

// Open Closed Principle => uygulamalar yeni bir ozellik eklemeye acık ama bu eklemeleri mevcut kodları degistirerek yapmamam gerrekiyor.

// Liskov'S Subsitituon Principle // Sırf birbirine benziyor diye aynı catı altında tutmuyoruz musteri gercek musteri tuzel musteri isim soyisim ornegi

// İnterface Seggregiration Principle  => interfacelerin dogru parcalara ayrılmasıdır.

interface IBankConnector
{
    void Operation1();
}

interface IPrivateBankConnector : IBankConnector
{
    void Operation2();
}

interface IPublicBankConnector : IBankConnector
{
    void Operation3();
}

class CBank : IPrivateBankConnector
{
    public void Operation1()
    {
        throw new NotImplementedException();
    }

    public void Operation2()
    {
        throw new NotImplementedException();
    }
}

// Dependency Inversion Principle => Yuksek seviyeleri sınıfların dusuk seviyeleri sınıfları somut degil de soyut halleri ile kullanmasıdır.

//DependencInjection tasarım deseni kullanılır dependency inversion principle de


//IOc container inversion of control  Ninject,Structure Map

//AddSingleton,AddScoped,AddTransient  singleton uygulama yasamı boyunca tek br ornegi olusturulur scoped her http isteginde ornegi olusturuu.transient 

// Data=>Business=>Services(Web Api(Json),WCF(Soap(Xml)))=>UI

// Abstract Class

public abstract class LegislationBase
{
    public int Id { get; set; }

    protected LegislationBase()
    {
        Id= 0;
        
    }
    public abstract void Evaluate();

    public virtual void Save()
    {
        Console.WriteLine("Saved");

    }
}

class ALegislation : LegislationBase
{
    public override void Evaluate()
    {
        Console.WriteLine("Assessed according to the alegislation.");
    }
}

class BLegislation : LegislationBase
{
    public override void Evaluate()
    {
        Console.WriteLine("Assessed according to the blegislation.");
    }
}
// virtual metodlar

class CLegislation : LegislationBase
{
    public override void Evaluate()
    {
        Console.WriteLine("Assessed according to the blegislation.");
    }

    public override void Save()
    {
        //  base.Save();

        Console.WriteLine("Saved according to the clegislation.");
    }
}

//Access Modifier => Bir classın erşim bildirgecleri default olarak internaldır.

//internal aynı namespace içinde aynı uygulama içinde kullanılabilir

//public farklı assemblylerde de kullanılabilir farklı namespacelerde

//private sadece kendini kapsayan suslu parantezler içerisinde gecerlidir  private<protected<internal<public

//protected ise privateden artı olarak inheritence verdigi yerde de gecerlı olacaktır.
// MVC arayuz için bir tasarım desenidir

//Polymorphisim  =é  çok biçimlilikdir bir sınıfın farklı biçimdeki implemetasyonları kullanabilmesidir.