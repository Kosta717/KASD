using System;
using System.Threading;
struct Complex
{
    public double Re;
    public double Im;

    public Complex(double re, double im)
    {
        Re = re;
        Im = im;
    }

    public Complex Add(Complex other)
    {
        return new Complex(Re + other.Re, Im + other.Im);
    }

    public Complex Sub(Complex other)
    {
        return new Complex(Re - other.Re, Im - other.Im);
    }

    public Complex Multiply(Complex other)
    {
        double re = Re * other.Re - Im * other.Im;
        double im = Re * other.Im + Im * other.Re;
        return new Complex(re, im);
    }

    public Complex Divide(Complex other)
    {
        double denominator = other.Re * other.Re + other.Im * other.Im;
        double re = (Re * other.Re + Im * other.Im) / denominator;
        double im = (Im * other.Re - Re * other.Im) / denominator;
        return new Complex(re, im);
    }

    public void OutCon()
    {
        Console.WriteLine($"{Re} + {Im}i");
    }

    public void Argum()
    {
        double arg=Math.Tan(Im/Re);
        Console.WriteLine($"Аргумент: {arg}");
    }
    public void Modull()
    {
        double arg = Math.Sqrt(Im*Im + Re*Re);
        Console.WriteLine($"Модуль: {arg}");
    }
}

class Program
{
    static void Main()
    {
        Complex number = new Complex(0, 0);
        char choice;

        do
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1 - Ввести новое комплексное число");
            Console.WriteLine("2 - Сложить");
            Console.WriteLine("3 - Вычесть");
            Console.WriteLine("4 - Умножить");
            Console.WriteLine("5 - Разделить");
            Console.WriteLine("6 - Текущее число");
            Console.WriteLine("7 - Аргумент");
            Console.WriteLine("8 - Модуль");
            Console.WriteLine("Q - Выход");
            choice = Console.ReadKey().KeyChar;
            Console.WriteLine();

            switch (choice)
            {
                case '1':
                    number = ReadComp();
                    break;
                case '2':
                    number = number.Add(ReadComp());
                    break;
                case '3':
                    number = number.Sub(ReadComp());
                    break;
                case '4':
                    number = number.Multiply(ReadComp());
                    break;
                case '5':
                    number = number.Divide(ReadComp());
                    break;
                case '6':
                    number.OutCon();
                    break;
                case '7':
                    number.Argum();
                    break;
                case '8':
                    number.Modull();
                    break;
                case 'q':
                case 'Q':
                    Console.WriteLine("Выход из программы.");
                    break;
                default:
                    Console.WriteLine("Неизвестная команда.");
                    break;
            }

        } while (choice != 'q' && choice != 'Q');
    }

    static Complex ReadComp()
    {
        Console.Write("Введите вещественную часть: ");
        double re = Convert.ToDouble(Console.ReadLine());
        Console.Write("Введите мнимую часть: ");
        double im = Convert.ToDouble(Console.ReadLine());
        return new Complex(re, im);
        Thread.Sleep(10000);
    }
}
