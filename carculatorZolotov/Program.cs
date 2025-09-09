using System;
using System.Globalization;
using System.Linq;

namespace CalculatorProgram
{
    class Calculator
    {
        private static NumberFormatInfo numberFormatInfo = new NumberFormatInfo()
        {
            NumberDecimalSeparator = ".",
        };
        
        private double memory = 0;
        private double currentValue = 0;
        private static string[] memoryOperations = { "M+", "M-", "MR", "MC" };

        public void Start()
        {
            bool continueCalculations = true;
            
            Console.WriteLine("Добро пожаловать в калькулятор!");
            Console.WriteLine("Доступные операции: +, -, *, /, ^, %, sqrt");
            Console.WriteLine("Операции с памятью: M+ (добавить), M- (вычесть), MR (вспомнить), MC (очистить)");

            while (continueCalculations)
            {
                try
                {
                    Console.WriteLine("\nВведите выражение (например: 2 + 2) или команду памяти:");
                    Console.WriteLine("Для sqrt введите: sqrt число");
                    
                    string input = Console.ReadLine();
                    string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    if (parts.Length == 0)
                    {
                        Console.WriteLine("ERROR: пустой ввод");
                        continue;
                    }

                    // обработка операций с памятью
                    if (parts.Length == 1 && memoryOperations.Contains(parts[0]))
                    {
                        HandleMemoryOperation(parts[0]);
                        continue;
                    }

                    // обработка унарных операций 
                    if (parts.Length == 2 && parts[0] == "sqrt")
                    {
                        if (double.TryParse(parts[1], NumberStyles.Any, numberFormatInfo, out double num))
                        {
                            if (num < 0)
                                throw new ArgumentException("ERROR: нельзя извлечь корень из отрицательного числа");

                            currentValue = Math.Sqrt(num);
                            Console.WriteLine($"Результат: {ToPointString(currentValue)}");
                        }
                        else
                        {
                            Console.WriteLine("ERROR: неверный формат числа");
                        }
                        continue;
                    }

                    // Обработка бинарных операций
                    if (parts.Length == 3)
                    {
                        if (double.TryParse(parts[0], NumberStyles.Any, numberFormatInfo, out double firstNum) &&
                            double.TryParse(parts[2], NumberStyles.Any, numberFormatInfo, out double secondNum))
                        {
                            HandleBinaryOperation(parts[1], firstNum, secondNum);
                        }
                        else
                        {
                            Console.WriteLine("ERROR: неверный формат чисел");
                        }
                        continue;
                    }

                    Console.WriteLine("ERROR: неверный формат ввода");
                }
                catch (FormatException)
                {
                    Console.WriteLine("ERROR: неверный формат числа");
                }
                catch (OverflowException)
                {
                    Console.WriteLine("ERROR: число слишком большое или слишком маленькое");
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("ERROR: деление на ноль");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"ERROR: {ex.Message}");
                }

                // Запрос на продолжение
                Console.WriteLine("\nНажмите 'q' для выхода или любую другую клавишу для продолжения.");
                if (Console.ReadKey().KeyChar == 'q')
                    continueCalculations = false;

                Console.Clear();
                Console.WriteLine("Добро пожаловать в калькулятор!");
                Console.WriteLine($"Текущее значение: {ToPointString(currentValue)}");
                Console.WriteLine($"Память: {ToPointString(memory)}");
            }
        }

        private void HandleMemoryOperation(string operation)
        {
            switch (operation)
            {
                case "M+":
                    memory += currentValue;
                    Console.WriteLine($"Добавлено в память: {ToPointString(currentValue)}");
                    break;
                case "M-":
                    memory -= currentValue;
                    Console.WriteLine($"Вычтено из памяти: {ToPointString(currentValue)}");
                    break;
                case "MR":
                    currentValue = memory;
                    Console.WriteLine($"Восстановлено из памяти: {ToPointString(memory)}");
                    break;
                case "MC":
                    memory = 0;
                    Console.WriteLine("Память очищена");
                    break;
            }
        }

        private void HandleBinaryOperation(string operation, double firstNum, double secondNum)
        {
            switch (operation)
            {
                case "+":
                    currentValue = firstNum + secondNum;
                    break;
                case "-":
                    currentValue = firstNum - secondNum;
                    break;
                case "*":
                    currentValue = firstNum * secondNum;
                    break;
                case "/":
                    if (secondNum == 0)
                        throw new DivideByZeroException("деление на ноль");
                    currentValue = firstNum / secondNum;
                    break;
                case "%":
                    currentValue = firstNum % secondNum;
                    break;
                case "^":
                    currentValue = Math.Pow(firstNum, secondNum);
                    break;
                default:
                    throw new InvalidOperationException("неверная операция");
            }
            
            Console.WriteLine($"Результат: {ToPointString(currentValue)}");
        }

        public static string ToPointString(double value)
        {
            return value.ToString(numberFormatInfo);
        }
    }

    class Program
    {
        public static void Main(string[] args)
        {
            Calculator calculator = new Calculator();
            calculator.Start();
        }
    }
}