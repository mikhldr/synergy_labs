namespace Part1_Interfaces.Task1;

public interface IMovable
{
    void Move(int x, int y);
}

public class Point : IMovable
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void Move(int x, int y)
    {
        X += x;
        Y += y;
        Console.WriteLine($"Точка перемещена в ({X}, {Y})");
    }
}


public interface IDrawable
{
    void Draw();
}

public class Circle : IDrawable
{
    public double Radius { get; }
    public Circle(double radius) => Radius = radius;
    public void Draw() => Console.WriteLine($"Рисую круг радиусом {Radius}");
}

public class Rectangle : IDrawable
{
    public double Width { get; }
    public double Height { get; }
    public Rectangle(double w, double h) { Width = w; Height = h; }
    public void Draw() => Console.WriteLine($"Рисую прямоугольник {Width}x{Height}");
}

public static class DrawHelper
{
    public static void DrawAll(List<IDrawable> shapes)
    {
        foreach (var shape in shapes)
            shape.Draw();
    }
}
