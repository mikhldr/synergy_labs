namespace Part1_Interfaces.Task4;


public interface IPayable
{
    void Pay(decimal amount);
}

public class CreditCard : IPayable
{
    public string Number { get; }
    public CreditCard(string number) => Number = number;

    public void Pay(decimal amount)
        => Console.WriteLine($"Оплата {amount:C} с карты {Number}");
}

public class Cash : IPayable
{
    public void Pay(decimal amount)
        => Console.WriteLine($"Оплата {amount:C} наличными");
}

public static class PaymentProcessor
{
    public static void ProcessPayment(IPayable method, decimal amount)
    {
        Console.WriteLine("Обработка платежа...");
        method.Pay(amount);
        Console.WriteLine("Платёж выполнен.\n");
    }
}


public interface ILogger
{
    void Log(string message);
}

public class ConsoleLogger : ILogger
{
    public void Log(string message)
        => Console.WriteLine($"[CONSOLE {DateTime.Now:HH:mm:ss}] {message}");
}

public class FileLogger : ILogger
{
    private readonly string _filePath;
    public FileLogger(string filePath) => _filePath = filePath;

    public void Log(string message)
    {
        var line = $"[FILE {DateTime.Now:HH:mm:ss}] {message}";
        File.AppendAllText(_filePath, line + Environment.NewLine);
        Console.WriteLine($"(в файл {_filePath}): {line}");
    }
}

public static class Worker
{
    public static void DoWork(ILogger logger)
    {
        logger.Log("Начало работы");
        logger.Log("Выполнение задачи...");
        logger.Log("Работа завершена");
    }
}
