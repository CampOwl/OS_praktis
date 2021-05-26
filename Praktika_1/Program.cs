using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO.Compression;

namespace Siteeva_OS_Praktika_1
{
  class Program
  {
    static void Main(string[] args)
    {
      while (true)
      {
        Console.WriteLine("1- информация о дисках, 2 - работа с файлами, 3- JSON, 4- xml, 5- zip");
        switch (Console.ReadLine())
        {
          case "1": disc(); break;
          case "2": fileWork(); break;
          case "3": jsonWork(); break;
          case "4": xmlWork(); break;
          case "5": zipWork(); break;
        }
      }
    }
    static void disc()
    {
      DriveInfo[] drives = DriveInfo.GetDrives();

      foreach (DriveInfo drive in drives)
      {
        Console.WriteLine($"Название: {drive.Name}");
        Console.WriteLine($"Тип файловой системы: {drive.DriveType}");
        if (drive.IsReady)
        {
          Console.WriteLine($"Объем диска: {drive.TotalSize}");
          Console.WriteLine($"Свободное пространство: {drive.TotalFreeSpace}");
          Console.WriteLine($"Метка тома: {drive.VolumeLabel}");
        }
        Console.WriteLine();
      }
    }
    
    static void fileWork()
    {
     
      Console.WriteLine("введите строку, которую запишем в файл");
      string str = Console.ReadLine();
      string path = @"D:\Documents\Ситеева БББО-05-19";
      DirectoryInfo dirInfo = new DirectoryInfo(path);
      if (!dirInfo.Exists)
      {
        dirInfo.Create();
      }

      using (FileStream fstream = new FileStream(@"D:\Documents\Ситеева БББО-05-19\note.txt", FileMode.OpenOrCreate))
      {
       
        byte[] array = System.Text.Encoding.Default.GetBytes(str);
        
        fstream.Write(array, 0, array.Length);
        Console.WriteLine("Текст записан в файл");
      }

      using (FileStream fstream = File.OpenRead(@"D:\Documents\Ситеева БББО-05-19\note.txt"))
      {
        
        byte[] array = new byte[fstream.Length];
        
        fstream.Read(array, 0, array.Length);
        
        string textFromFile = System.Text.Encoding.Default.GetString(array);
        Console.WriteLine($"Текст из файла: {textFromFile}");
        fstream.Close();
      }
      Console.WriteLine("Удалить файл? (1 - да, 0 - нет)");
      if (Console.ReadLine() == "1")
      {
        File.Delete(@"D:\Documents\Ситеева БББО-05-19\note.txt");
        Console.WriteLine("файл удален");
      }
      else
      {
        Console.WriteLine("файл не был удален");
      }
    }
    class Person
    {
      public string Name { get; set; }
      public int Age { get; set; }
    }
    static void jsonWork()
    {
      string path = @"D:\Documents\Ситеева БББО-05-19";
      DirectoryInfo dirInfo = new DirectoryInfo(path);
      if (!dirInfo.Exists)
      {
        dirInfo.Create();
      }
      var options = new JsonSerializerOptions
      {
        WriteIndented = true
      };
      using (FileStream fs = new FileStream(@"D:\Documents\Ситеева БББО-05-19\user.json", FileMode.OpenOrCreate))
      {
        Person tom = new Person() { Name = "Tom", Age = 35 };
        string json = JsonSerializer.Serialize<Person>(tom, options);
        byte[] array = System.Text.Encoding.Default.GetBytes(json);

        fs.Write(array, 0, array.Length);
        Console.WriteLine("Текст записан в файл");
      }


      string jsonString = File.ReadAllText(@"D:\Documents\Ситеева БББО-05-19\user.json");
      Person restoredPerson = JsonSerializer.Deserialize<Person>(jsonString);
      Console.WriteLine($"Name: {restoredPerson.Name}  Age: {restoredPerson.Age}");

      Console.WriteLine("Удалить файл? (1 - да, 0 - нет)");
      if (Console.ReadLine() == "1")
      {
        File.Delete(@"D:\Documents\Ситеева БББО-05-19\user.json");
        Console.WriteLine("файл удален");
      }
      else
      {
        Console.WriteLine("файл не был удален");
      }

    }
    static void xmlWork()
    {
      
      Console.WriteLine("В файле уже записаны некоторые данные. Желаете записать что-то?(1 - да, 0 - нет)");
      if (Console.ReadLine() == "1")
      {
        Console.WriteLine("В файле записывается информация о телефонах. введите название телефона");
        string userPhone = Console.ReadLine();
        Console.WriteLine("компания");
        string ComUserPhone = Console.ReadLine();
        Console.WriteLine("цена");
        string PriceUserPhone = Console.ReadLine();
        XDocument xdoc = new XDocument(new XElement("phones",
    new XElement("phone",
        new XAttribute("name", "iPhone 6"),
        new XElement("company", "Apple"),
        new XElement("price", "40000")),
    new XElement("phone",
        new XAttribute("name", "Samsung Galaxy S5"),
        new XElement("company", "Samsung"),
        new XElement("price", "33000")),
    new XElement("phone",
        new XAttribute("name", userPhone),
        new XElement("company", ComUserPhone),
        new XElement("price", PriceUserPhone))));
     xdoc.Save(@"D:\Documents\Ситеева БББО-05-19\phones.xml");
      }
      else
      {
        XDocument doc = new XDocument(new XElement("phones",
    new XElement("phone",
        new XAttribute("name", "iPhone 6"),
        new XElement("company", "Apple"),
        new XElement("price", "40000")),
    new XElement("phone",
        new XAttribute("name", "Samsung Galaxy S5"),
        new XElement("company", "Samsung"),
        new XElement("price", "33000"))));
        doc.Save(@"D:\Documents\Ситеева БББО-05-19\phones.xml");
      }
      XDocument docu = XDocument.Load(@"D:\Documents\Ситеева БББО-05-19\phones.xml");
      foreach (XElement phoneElement in docu.Element("phones").Elements("phone"))
      {
        XAttribute nameAttribute = phoneElement.Attribute("name");
        XElement companyElement = phoneElement.Element("company");
        XElement priceElement = phoneElement.Element("price");

        if (nameAttribute != null && companyElement != null && priceElement != null)
        {
          Console.WriteLine($"Смартфон: {nameAttribute.Value}");
          Console.WriteLine($"Компания: {companyElement.Value}");
          Console.WriteLine($"Цена: {priceElement.Value}");
        }
        Console.WriteLine();
      }
      Console.WriteLine("Удалить файл? (1 - да, 0 - нет)");
      if (Console.ReadLine() == "1")
      {
        File.Delete(@"D:\Documents\Ситеева БББО-05-19\phones.xml");
        Console.WriteLine("файл удален");
      }
      else
      {
        Console.WriteLine("файл не был удален");
      }
    }
    static void zipWork()
    {
      string arch = @"D:\Documents\Ситеева БББО-05-19\arch.zip";
      string file = @"D:\Documents\Ситеева БББО-05-19\doc.doc";
      string target = @"D:\Documents\Ситеева БББО-05-19\doc_new.doc";
      Compress(file, arch);
      // чтение из сжатого файла
      Decompress(arch, target);

      Console.WriteLine("Удалить файлы? (1 - да, 0 - нет)");
      if (Console.ReadLine() == "1")
      {
        File.Delete(@"D:\Documents\Ситеева БББО-05-19\arch.zip");
        File.Delete(@"D:\Documents\Ситеева БББО-05-19\doc_new.doc");
        Console.WriteLine("файл удален");
      }
      else
      {
        Console.WriteLine("файл не был удален");
      }
    }

    public static void Compress(string sourceFile, string compressedFile)
    {
      // поток для чтения исходного файла
      using (FileStream sourceStream = new FileStream(sourceFile, FileMode.OpenOrCreate))
      {
        // поток для записи сжатого файла
        using (FileStream targetStream = File.Create(compressedFile))
        {
          // поток архивации
          using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
          {
            sourceStream.CopyTo(compressionStream); // копируем байты из одного потока в другой
            Console.WriteLine("Сжатие файла {0} завершено. Исходный размер: {1}  сжатый размер: {2}.",
                sourceFile, sourceStream.Length.ToString(), targetStream.Length.ToString());
          }
        }
      }
    }

    public static void Decompress(string compressedFile, string targetFile)
    {
      // поток для чтения из сжатого файла
      using (FileStream sourceStream = new FileStream(compressedFile, FileMode.OpenOrCreate))
      {
        // поток для записи восстановленного файла
        using (FileStream targetStream = File.Create(targetFile))
        {
          // поток разархивации
          using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
          {
            decompressionStream.CopyTo(targetStream);
            Console.WriteLine("Восстановлен файл: {0}, размер файла: {1}", targetFile, targetStream.Length.ToString());
          }
        }
      }
    }

  }
}
