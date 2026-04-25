using Part1_Interfaces.Task1;
using Part1_Interfaces.Task2;
using Part1_Interfaces.Task3;
using Part1_Interfaces.Task4;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("ЗАДАНИЕ 1: IMovable, IDrawable");
var p = new Part1_Interfaces.Task1.Point(0, 0);
p.Move(5, 3);
p.Move(-2, 1);

var shapes = new List<IDrawable>
{
    new Part1_Interfaces.Task1.Circle(5),
    new Part1_Interfaces.Task1.Rectangle(4, 6),
    new Part1_Interfaces.Task1.Circle(2.5)
};
DrawHelper.DrawAll(shapes);

Console.WriteLine("\nЗАДАНИЕ 2: IShape, I3DShape");
ShapePrinter.PrintShapeInfo(new Part1_Interfaces.Task2.Circle(5));
ShapePrinter.PrintShapeInfo(new Part1_Interfaces.Task2.Rectangle(4, 6));
ShapePrinter.PrintShapeInfo(new Cube(3));

Console.WriteLine("\nЗАДАНИЕ 3: ISP");
IPrinter printer = new Printer();
IScanner scanner = new Scanner();
var mfu = new MultifunctionDevice();

printer.Print("Документ.docx");
scanner.Scan();
mfu.Print("Договор.pdf");
mfu.Scan();
mfu.Fax("+373 22 123456");

Console.WriteLine("\nЗАДАНИЕ 4: IPayable, ILogger");
PaymentProcessor.ProcessPayment(new CreditCard("1234-5678-9012-3456"), 1500m);
PaymentProcessor.ProcessPayment(new Cash(), 200m);

Console.WriteLine("--- ConsoleLogger ---");
Worker.DoWork(new ConsoleLogger());

Console.WriteLine("--- FileLogger ---");
Worker.DoWork(new FileLogger("log.txt"));

Console.WriteLine("\nВсе задания выполнены.");
