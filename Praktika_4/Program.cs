using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace praktika4
{
  class Program
  {
    static List<Thread> func = new List<Thread>();
    static int kvant = 5000;
    static bool zero, first, second;//проверка спят ли потоки
    static bool work_0 = true, work_1 = true, work_2 = true; // состояние потоков
    private static ManualResetEvent mre_zero = new ManualResetEvent(true); // синхронизация потоков
    private static ManualResetEvent mre_first = new ManualResetEvent(false);
    private static ManualResetEvent mre_second = new ManualResetEvent(false);
    static Stopwatch timer_1 = new Stopwatch();
    static Stopwatch timer_2 = new Stopwatch();
    static Stopwatch timer_3 = new Stopwatch();
    static void WordCount()//просто переборка слова из 7 слов
    {
      uint sum = 0;
      string text;
      char[] symbols = new char[7];
     //случайная последовательность, нужно лишь чтобы дольше работало
      for (int j = 65; j <= 82; j++)
        for (int k = 65; k <= 82; k++)
          for (int l = 65; l <= 82; l++)
            for (int m = 65; m <= 82; m++)
              for (int n = 65; n <= 82; n++)
                for (int q = 65; q <= 82; q++)
                {
                  mre_zero.WaitOne();
                  //mre_first.WaitOne();
                  //mre_second.WaitOne();
                  sum++;
                  symbols[1] = Convert.ToChar(j);
                  symbols[2] = Convert.ToChar(k);
                  symbols[3] = Convert.ToChar(l);
                  symbols[4] = Convert.ToChar(m);
                  symbols[5] = Convert.ToChar(n);
                  symbols[6] = Convert.ToChar(q);
                  text = new string(symbols);
                }
              
      Console.WriteLine("результат 0 потока " + sum);
      work_0 = false;
    }
    static void NumberCount()//просто переборка чисел из 9 цифр
    {
      uint sum = 0;
      string text;
      char[] symbols = new char[9];
      //опять же просто числа, 48 это 0, а 57 это 9
      
      for (int k = 48; k <= 57; k++)
        for (int l = 48; l <= 57; l++)
          for (int m = 48; m <= 57; m++)
            for (int p = 48; p <= 57; p++)
              for (int s = 48; s <= 57; s++)
                for (int t = 48; t <= 57; t++)
                  for (int q = 48; q <= 57; q++)
                  {
                    //mre_zero.WaitOne();
                    //mre_second.WaitOne();
                    mre_first.WaitOne();
                    sum++;
                    
                    symbols[2] = Convert.ToChar(k);
                    symbols[3] = Convert.ToChar(l);
                    symbols[4] = Convert.ToChar(m);
                    symbols[5] = Convert.ToChar(p);
                    symbols[6] = Convert.ToChar(s);
                    symbols[7] = Convert.ToChar(t);
                    symbols[8] = Convert.ToChar(q);
                    text = new string(symbols);
                  }
      Console.WriteLine("результат 1 потока " + sum);
      work_1 = false;
    }
    static void Fibonacci()
    {
     //mre_second.WaitOne();
      ulong a = 0;//первый и последний член
      ulong b = 1;//второй и последующий член
      ulong tmp;//хранилище
      ulong n = 40000000;
      for (ulong i = 0; i < n; i++)
      {
        //mre_zero.WaitOne();
        //mre_first.WaitOne();
        mre_second.WaitOne();
        tmp = a;
        a = b;
        b += tmp;
      }
      Console.WriteLine("результат 2 потока " + a);
      work_2 = false;
    }
    static void Main()
    {
      func.Add(new Thread(WordCount)); func[0].Name = "WordCount"; // обьявление потоков
      func.Add(new Thread(NumberCount)); func[1].Name = "NumberCount";
      func.Add(new Thread(Fibonacci)); func[2].Name = "Fibonacci";
      for (int i = 0; i < 3; i++)// запуск
        func[i].Start();
      zero = false; first = true; second = true;
      timer_1.Start();
      Thread.Sleep(kvant);
      while (func[0].IsAlive || func[1].IsAlive || func[2].IsAlive)//проверка на существование потоков
      {
        if (!zero && work_1)
        {
          zero = true;
          mre_zero.Reset();
          timer_1.Stop();
          mre_first.Set();
          timer_2.Start();
          first = false;
          Console.WriteLine("0 заснул, а 1 заработал");
          Console.WriteLine($"{timer_1.ElapsedMilliseconds}");
          Thread.Sleep(kvant);
          continue;
        }
        else if (!first && work_2)
        {

          first = true;
          mre_first.Reset();
          timer_2.Stop();
          mre_second.Set();
          timer_3.Start();
          second = false;
          Console.WriteLine("1 заснул, а 2 заработал");
          Console.WriteLine($"{timer_2.ElapsedMilliseconds}");
          Thread.Sleep(kvant);
          continue;
        }
        else if (!second && work_0)
        {
          second = true;
          mre_second.Reset();
          timer_3.Stop();
          mre_zero.Set();
          timer_1.Start();
          zero = false;
          Console.WriteLine("2 заснул, а 0 заработал");
          Console.WriteLine($"{timer_3.ElapsedMilliseconds}");
          Thread.Sleep(kvant);
          continue;
        }
        else if (!zero && !work_1 && work_2)
        {
          zero = true;
          mre_zero.Reset();
          timer_1.Stop();
          mre_second.Set();
          timer_3.Start();
          second = false;
          Console.WriteLine("0 заснул, а 2 заработал");
          Console.WriteLine($"{timer_1.ElapsedMilliseconds}");
          Thread.Sleep(kvant);
        }
        else if (!first && !work_2 && work_0)
        {
        first = true;
        mre_first.Reset();
        timer_2.Stop();
        mre_zero.Set();
        timer_1.Start();
        zero = false;
        Console.WriteLine("1 заснул, а 0 заработал");
        Console.WriteLine($"{timer_2.ElapsedMilliseconds}");
        Thread.Sleep(kvant);
        continue;
        }
        else if(!second && !work_0 && work_1)
        {
          second = true;
          mre_second.Reset();
          timer_3.Stop();
          mre_first.Set();
          timer_2.Start();
          first = false;
          Console.WriteLine("2 заснул, а 1 заработал");
          Console.WriteLine($"{timer_3.ElapsedMilliseconds}");
          Thread.Sleep(kvant);
          continue;
        }
          else if(!work_0 && !work_1)
        {
          timer_3.Start();
          Console.WriteLine("работает только 2");
          Console.WriteLine($"{timer_3.ElapsedMilliseconds}");
          Thread.Sleep(kvant);
          continue;
        }
        else if (!work_1 && !work_2)
        {
          timer_1.Start();
          Console.WriteLine("работает только 0");
          Console.WriteLine($"{timer_1.ElapsedMilliseconds}");
          Thread.Sleep(kvant);
          continue;
        }
        else if (!work_2 && !work_0)
        {
          timer_2.Start();
          Console.WriteLine("работает только 1");
          Console.WriteLine($"{timer_2.ElapsedMilliseconds}");
          Thread.Sleep(kvant);
          continue;
        }
        else
        {
          Console.WriteLine("А все, программка закончилась");
        }
      }
    }
  }
}
