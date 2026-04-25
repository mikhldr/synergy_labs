namespace Part1_Interfaces.Task3;


public interface IDevice
{
    void Print(string text);
    void Scan();
    void Fax(string number);
}

public class BadPrinter : IDevice
{
    public void Print(string text) => Console.WriteLine($"Печать: {text}");
    public void Scan() => throw new NotSupportedException();
    public void Fax(string number) => throw new NotSupportedException();
}

public class BadScanner : IDevice
{
    public void Print(string text) => throw new NotSupportedException();
    public void Scan() => Console.WriteLine("Сканирование документа");
    public void Fax(string number) => throw new NotSupportedException();
}

public interface IPrinter
{
    void Print(string text);
}

public interface IScanner
{
    void Scan();
}

public interface IFax
{
    void Fax(string number);
}

public class Printer : IPrinter
{
    public void Print(string text) => Console.WriteLine($"[Принтер] печать: {text}");
}

public class Scanner : IScanner
{
    public void Scan() => Console.WriteLine("[Сканер] сканирование");
}

public class MultifunctionDevice : IPrinter, IScanner, IFax
{
    public void Print(string text) => Console.WriteLine($"[МФУ] печать: {text}");
    public void Scan() => Console.WriteLine("[МФУ] сканирование");
    public void Fax(string number) => Console.WriteLine($"[МФУ] отправка факса на {number}");
}
