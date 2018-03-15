using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace ConsoleApplication1
{
    class TextColor
    {
        public static void ErrorColor(string outString)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(outString);
            Console.ResetColor();
        }
        public static void GreenColor(string outString)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(outString);
            Console.ResetColor();
        }
        public static void BlueColor(Char outString)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(outString);
            Console.ResetColor();
        }
    }
	
    class Program
    {
        static SerialPort port;
        public static void ReadUART()
        {
            while (true)
            {
                try
                {
                    TextColor.BlueColor((char)port.ReadChar());
                }
                catch (Exception e)
                {
                    TextColor.ErrorColor(e.Message);
                    Console.ReadKey();
                    Environment.Exit(-1);
                }
            }
        }
		
        public static void WriteUART()
        {
            while (true)
            {
                try
                {
                    port.Write(SymbolReplace(Console.ReadLine()));
                }
                catch (Exception e)
                {
                    TextColor.ErrorColor(e.Message);
                    Console.ReadKey();
                    Environment.Exit(-1);
                }
            }
        }
		
		public static string SymbolReplace(string stringForChange)
		{
			stringForChange = stringForChange.Replace(@"\n", "\n");
			stringForChange = stringForChange.Replace(@"\r", "\r");
			stringForChange = stringForChange.Replace(@"\0", "\0");
			stringForChange = stringForChange.Replace(@"\t", "\t");
			return stringForChange;
		}
		
        static void Main(string[] args)
        {
            string[] portnames = SerialPort.GetPortNames();
            if (portnames.Length > 0)
            {
                int portNumber = 0, portBitRate = 38400;
                if (portnames.Length >= 2)
                {
                    Console.WriteLine("Доступно портов: {0}", portnames.Length);
                    for (int i = 0; i < portnames.Length; i++)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(i);
                        Console.ResetColor();
                        Console.WriteLine("\t->   {0}", portnames[i]);
                    }
                    Console.Write("Выберите номер порта: ");
                    try
                    {
                        portNumber = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (Exception e)
                    {
                        TextColor.ErrorColor(e.Message);
                        Console.ReadKey();
                        Environment.Exit(-1);
                    }
                    if ((portNumber < 0) || (portNumber >= portnames.Length))
                    {
                        TextColor.ErrorColor("Некорректный номер порта");
                        Console.ReadKey();
                        Environment.Exit(-1);
                    }
                    TextColor.GreenColor("Был выбран порт: " + portnames[portNumber]);
                }
                else
                {
                    Console.Write("Был выбран порт по умолцанию: ");
                    TextColor.GreenColor(portnames[0]);
                }
                Console.Write("Введите скорость (38400 по умолчанию): ");
                Console.ForegroundColor = ConsoleColor.Green;
                try
                {
                    portBitRate = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception e)
                {
                    TextColor.ErrorColor(e.Message);
                    portBitRate = 38400;
                }
                TextColor.GreenColor("Скорость передачи: " + portBitRate);
                Console.ResetColor();
                port = new SerialPort(portnames[portNumber], portBitRate, Parity.None, 8, StopBits.One);
                try
                {
                    port.Open();
                }
                catch (Exception e)
                {
                    TextColor.ErrorColor(e.Message);
                    Console.ReadKey();
                    Environment.Exit(-1);
                }
                Console.WriteLine("__________________________");
                Thread readThread = new Thread(ReadUART);
                readThread.Start();
                WriteUART();
            }
            else
            {
                TextColor.ErrorColor("Нет доступных портов");           
            }
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
