namespace Part1_Interfaces.Task2;

public interface IShape
{
    double GetArea();
    double GetPerimeter();
}

public class Circle : IShape
{
    public double Radius { get; }
    public Circle(double radius) => Radius = radius;

    public double GetArea() => Math.PI * Radius * Radius;
    public double GetPerimeter() => 2 * Math.PI * Radius;
}

public class Rectangle : IShape
{
    public double Width { get; }
    public double Height { get; }
    public Rectangle(double w, double h) { Width = w; Height = h; }

    public double GetArea() => Width * Height;
    public double GetPerimeter() => 2 * (Width + Height);
}


public interface I3DShape : IShape
{
    double GetVolume();
}

public class Cube : I3DShape
{
    public double Side { get; }
    public Cube(double side) => Side = side;

    public double GetArea() => 6 * Side * Side;       
    public double GetPerimeter() => 12 * Side;        
    public double GetVolume() => Side * Side * Side;
}

public static class ShapePrinter
{
    public static void PrintShapeInfo(IShape shape)
    {
        Console.WriteLine($"Фигура: {shape.GetType().Name}");
        Console.WriteLine($"  Площадь:  {shape.GetArea():F2}");
        Console.WriteLine($"  Периметр: {shape.GetPerimeter():F2}");
        if (shape is I3DShape s3d)
            Console.WriteLine($"  Объём:    {s3d.GetVolume():F2}");
    }
}
