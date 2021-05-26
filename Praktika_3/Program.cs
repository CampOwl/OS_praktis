using System;
using System.Threading;
using System.Collections.Generic;

namespace pr_3_OS
{
  class Program
  {
    static List<Int32> SNumbers = new List<Int32>();//  массив производителей
    static List<Int32> BNumbers = new List<Int32>();// массив  потребителей
    static List<Int32> control = new List<Int32>();// дубликат производителя
    static bool check = true; // состояние потоков
    static object Slocker = new object();
    static object Blocker = new object();
    static void Seller(object i)
    {
      bool sleeping = false;
      int number = (int)i;
      int value;
      bool acquiredLock;
      Random rnd = new Random();
      while (check)
      {
        if (SNumbers.Count >= 100)
        {
          Console.WriteLine($"Продавец {number} спит.");
          sleeping = true;
          Thread.Sleep(1200);
          continue;
        }
        else
          if ((SNumbers.Count <= 80) && (sleeping))
        {
          Console.WriteLine($"Продавец {number} проснулся.");
          sleeping = false;
        }
        value = rnd.Next(1, 100);
        acquiredLock = false;
        try
        {
          Monitor.Enter(Slocker, ref acquiredLock);
          SNumbers.Add(value);
          control.Add(value);
          Thread.Sleep(0);
        }
        finally
        {
          if (acquiredLock) Monitor.Exit(Slocker);
        }
        Console.WriteLine($"Продавец {number} произвел число {value}");
      }
      Console.WriteLine($"Продавец {number} ушел на покой.");
    }
    static void Buyer(object j)
    {
      int value;
      int number = (int)j;
      bool sleeping = false;
      bool acquiredLock;
      while ((SNumbers.Count != 0) || (check))
      {
        if (SNumbers.Count == 0)
        {
          Console.WriteLine($"Покупатель {number} спит.");
          sleeping = true;
          Thread.Sleep(500);
          continue;
        }
        else
          if (sleeping)
        {
          Console.WriteLine($"Покупатель {number} проснулся.");
          sleeping = false;
        }
        acquiredLock = false;
        try
        {
          Monitor.Enter(Blocker, ref acquiredLock);
          if (SNumbers.Count != 0)
          {
            value = SNumbers[0];
            SNumbers.RemoveAt(0);
            Thread.Sleep(0);
          }
          else
          {
            Thread.Sleep(0);
            continue;
          }
        }
        finally
        {
          if (acquiredLock) Monitor.Exit(Blocker);
        }
        BNumbers.Add(value);
        Console.WriteLine($"Покупатель {number} извлек число {value}");
      }
      Console.WriteLine($"Покупатель {number} ушел на отдых.");
    }
    static void Main()
    {
      char for_check;
      bool check_alive = true;
      List<Thread> sell = new List<Thread>();
      List<Thread> buy = new List<Thread>();
      for (int i = 0; i < 3; i++)
      {
        sell.Add(new Thread(new ParameterizedThreadStart(Seller)));
        sell[i].Start(i);
      }
      for (int j = 0; j < 2; j++)
      {
        buy.Add(new Thread(new ParameterizedThreadStart(Buyer)));
        buy[j].Start(j);
      }
      while (check)
      {
        for_check = Console.ReadKey().KeyChar;
        if (for_check == 'q')
          check = false;
      }
      while (check_alive)
        if (!(sell[0].IsAlive || sell[1].IsAlive || sell[2].IsAlive || buy[0].IsAlive || buy[1].IsAlive))
        {
          check_alive = false;
          BNumbers.Sort();
          control.Sort();
          bool correct = true;
          if (BNumbers.Count == control.Count)
          {
            for (int i = 0; i < BNumbers.Count; i++)
              if (BNumbers[i] != control[i])
              {
                Console.WriteLine("Есть расхождения. Что-то пошло не так.");
                correct = false;
                break;
              }
            if (correct)
              Console.WriteLine("Вроде бы все работает");
          }
          else
            Console.WriteLine("В них даже количество элементов не совпадает. Явно что-то не то.");
        }
    }
  }
}
