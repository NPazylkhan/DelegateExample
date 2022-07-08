#region AccountHandler delegate
public delegate void AccountHandler(string message);
public class Account
{
    int sum;
    // Создаем переменную делегата
    AccountHandler? taken;
    public Account(int sum) => this.sum = sum;
    // Регистрируем делегат
    public void RegisterHandler(AccountHandler del)
    {
        taken += del;
    }
    // Отмена регистрации делегата
    public void UnregisterHandler(AccountHandler del)
    {
        taken -= del; // удаляем делегат
    }
    public void Add(int sum) => this.sum += sum;
    public void Take(int sum)
    {
        if (this.sum >= sum)
        {
            this.sum -= sum;
            // вызываем делегат, передавая ему сообщение
            taken?.Invoke($"Со счета списано {sum} у.е.");
        }
        else
        {
            taken?.Invoke($"Недостаточно средств. Баланс: {this.sum} у.е.");
        }
    }
}
#endregion

delegate void MessageHandler(string message);

delegate void Message();

delegate void Operation(int x, int y);

delegate bool IsEqual(int x);

#region OperationInt delegate
delegate int OperationInt(int x, int y);

enum OperationType
{
    Add, Subtract, Multiply
}
#endregion

class TestDelegate
{
    static void Main(string[] args)
    {
        #region AccountHandler realizations
        // создаем банковский счет
        Account account = new Account(200);
        // Добавляем в делегат ссылку на метод PrintSimpleMessage
        account.RegisterHandler(PrintSimpleMessage);
        account.RegisterHandler(PrintColorMessage);
        // Два раза подряд пытаемся снять деньги
        account.Take(100);
        account.Take(150);

        // Удаляем делегат
        account.UnregisterHandler(PrintColorMessage);
        // снова пытаемся снять деньги
        account.Take(50);
        #endregion

        #region MessageHandler realizations
        MessageHandler handler = delegate (string mes)
        {
            Console.WriteLine(mes);
        };

        handler("hello world!");

        ShowMessage("hello!", delegate (string mes)
        {
            Console.WriteLine(mes);
        });
        #endregion

        #region Lambda expression Message realizations
        Message hello = () => Console.WriteLine("Hello");
        hello();

        Message hello2 = () =>
        {
            Console.Write("Hello ");
            Console.WriteLine("World");
        };
        hello2();
        #endregion

        #region Lambda expression Operation realizations
        Operation sum = (x, y) => Console.WriteLine($"{x} + {y} = {x + y}");
        sum(1, 2);       
        sum(22, 14);
        #endregion

        #region Lambda expression IsEqual realizations
        int[] integers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        // найдем сумму чисел больше 5
        int result1 = Sum(integers, x => x > 5);
        Console.WriteLine(result1); // 30

        // найдем сумму четных чисел
        int result2 = Sum(integers, x => x % 2 == 0);
        Console.WriteLine(result2);  //20
        #endregion

        #region Lambda expression OperationInt realizations
        OperationInt operation = SelectOperation(OperationType.Add);
        Console.WriteLine(operation(10, 4));    // 14

        operation = SelectOperation(OperationType.Subtract);
        Console.WriteLine(operation(10, 4));    // 6

        operation = SelectOperation(OperationType.Multiply);
        Console.WriteLine(operation(10, 4));    // 40

        OperationInt SelectOperation(OperationType opType)
        {
            switch (opType)
            {
                case OperationType.Add: return (x, y) => x + y;
                case OperationType.Subtract: return (x, y) => x - y;
                default: return (x, y) => x * y;
            }
        }
        #endregion
    }

    #region AccountHandler Methods
    private static void PrintSimpleMessage(string message) => Console.WriteLine(message);
    private static void PrintColorMessage(string message)
    {
        // Устанавливаем красный цвет символов
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        // Сбрасываем настройки цвета
        Console.ResetColor();
    }
    #endregion

    #region MessageHandler Methods
    static void ShowMessage(string message, MessageHandler handler)
    {
        handler(message);
    }
    #endregion

    #region IsEqual Sum
    static int Sum(int[] numbers, IsEqual func)
    {
        int result = 0;
        foreach (int i in numbers)
        {
            if (func(i))
                result += i;
        }
        return result;
    }
    #endregion
}

