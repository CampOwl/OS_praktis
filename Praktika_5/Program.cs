using System;
using System.Collections.Generic;
//распределение памяти разделами переменной величины
namespace pls
{
  struct MemoryArea // струкутура области пямяти
  {
    public int start;
    //public int size;
    public int finish;
    public bool state;//состояние, заполнено процессом или нет
    public MemoryArea(int start, int finish, bool state)
    {
      this.start = start;
      this.finish = finish;
      //this.size = size;
      this.state = state;
      //finish = start + size;
    }
    public int Size()
    {
      return finish - start;
    }
  }

  class Program
  {
    int count = 65535;//размер свободной памяти
    static List<MemoryArea> memory = new List<MemoryArea>();//состояния областей памяти
    static void Insert()
    {
      int size;
      Console.WriteLine("Введите размер памяти, которую хотите занять: от 0 до 65535");
      size = Int32.Parse(Console.ReadLine());
      //foreach (MemoryArea area in memory)
      bool flag = false;
      for(int i = 0; i < memory.Count; i++)
      {
        Console.WriteLine(memory[i].start+"---"+ memory[i].finish);
        if ( !memory[i].state && memory[i].Size() >= size)
        {
          MemoryArea temp = memory[i];
          temp.state = true;
          int tempFin = temp.finish;
          temp.finish = temp.start + size;
          memory[i] = temp;
          memory.Insert(i+1, new MemoryArea(temp.start + size, tempFin, false));
          flag = true;
          break;
        }
      }
      if (flag)
      {
        Console.WriteLine("добавление впамять произошло успешно");
        LookAt();
      }
      else
      {
        Console.WriteLine("Упс, не нашлось подходящей области памяти");
      }
    }
    static void Delete()
    {
      int number;
      LookAt();
      Console.WriteLine("введите номер процесса, который необходимо удалить");
      number = Int32.Parse(Console.ReadLine());
      int j = 0;
      for (int i = 0; i < memory.Count; i++)
      {
        if(memory[i].state)
        {
          j++;
        }
        else
        {
          continue;
        }
        if (j == number)
        {
          if(i+1 < memory.Count)
          {
            MemoryArea temp = memory[i];
            if (memory[i + 1].state)
            {
              //MemoryArea temp = memory[i];
              temp.state = false;
              memory[i] = temp;
            }
            else
            {
              temp.finish = memory[i + 1].finish;
              temp.state = false;
              memory[i] = temp;
              memory.RemoveAt(i + 1);
            }
          }
          if(i>0)
          {
            MemoryArea temp = memory[i];
            if (memory[i - 1].state)
            {
              temp.state = false;
              memory[i] = temp;
            }
            else
            {
              temp.start = memory[i - 1].start;
              temp.state = false;
              memory[i] = temp;
              memory.RemoveAt(i - 1);
            }
          }
          if(memory.Count == 1)
          {
            MemoryArea temp = memory[i];
            temp.state = false;
            memory[i] = temp;
          }
        }
      }
      Console.WriteLine("удаление произошло успешно");
    }
    static void LookAt()
    {
      int count = 0;
      bool flag = true;
      for (int i = 0; i < memory.Count; i++)
      {
        if (memory[i].state)
        {
          flag = false;
          Console.WriteLine($"{count+1} процесс занимает {memory[i].Size()} байт и начинается с {memory[i].start} байта.");
          count++;
        }
      }
      if (flag) Console.WriteLine("память пуста\n");
    }
    static void Main()
    {
      bool flag = true;
      memory.Add(new MemoryArea(0, 65535, false));
      while (flag)
      {
        Console.WriteLine
        (
        "-----------------\n1. Добавить задачу\n" +
        "2. Удалить задачу\n" +
        "3. Посмотреть количество свободной памяти\n" +
        "4. Выход\n\n" +
        "Введите действие:"
        );
        string s = Console.ReadLine();
        switch (s)
        {
          case "1":
          {
              Insert();
              //Console.WriteLine("1");
              break;
          }
          case "2":
          {
              Delete();
              //Console.WriteLine("2");
              break;
          }
          case "3":
          {
              LookAt();
              //Console.WriteLine("3");
              break;
          }
          case "4":
          {
              flag = false;
              Console.WriteLine("а все");
              break;
          }
          default:
          {
              Console.WriteLine("Упс, вы ввели неккоректные данные! Повторите ввод\n");
              break;
          }
        }
      }
    }
  }
}
