// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;

namespace StudentSystem;

public interface IConsoleService
{
    string ReadLine();
    void WriteLine(string message);
    void Write(string message);
    ConsoleKeyInfo ReadKey();
}

public class ConsoleService : IConsoleService
{
    public string ReadLine() => Console.ReadLine() ?? string.Empty;
    public void WriteLine(string message) => Console.WriteLine(message);
    public void Write(string message) => Console.Write(message);
    public ConsoleKeyInfo ReadKey() => Console.ReadKey();
}

public interface IStudentRepository
{
    void Append(Student student);
    void Prepend(Student student);
    void Insert(Student student, int index);
    void Print();
    DoublyNode<Student>? GetById(int id);
    bool RemoveById(int id);
    bool UpdateNode(int id, Student student);
    List<DoublyNode<Student>> GetByName(string name);
    DoublyNode<Student>? GetHead();
}

public class StudentRepository : IStudentRepository
{
    private readonly DoublyLinkedList<Student> _studentList;

    public StudentRepository()
    {
        _studentList = new DoublyLinkedList<Student>();
    }

    public void Append(Student student) => _studentList.Append(student);
    public void Prepend(Student student) => _studentList.Prepend(student);
    public void Insert(Student student, int index) => _studentList.Insert(student, index);
    public void Print() => _studentList.Print();
    public DoublyNode<Student>? GetById(int id) => _studentList.GetId(id);
    public bool RemoveById(int id) => _studentList.RemoveId(id);
    public bool UpdateNode(int id, Student student) => _studentList.UpdateNode(id, student);
    public List<DoublyNode<Student>> GetByName(string name) => _studentList.GetName(name);
    public DoublyNode<Student>? GetHead() => _studentList.Head;
}

public interface IInputService
{
    int ConvertToInt(string inputMsg);
    string InputString(string msg);
    DateOnly ConvertToDateOnly(string msg, string format);
}

public class InputService : IInputService
{
    private readonly IConsoleService _consoleService;

    public InputService(IConsoleService consoleService)
    {
        _consoleService = consoleService;
    }

    public int ConvertToInt(string inputMsg)
    {
        bool dataConv = false;
        int convData = 0;
        while (!dataConv)
        {
            string? dataStr = null;
            while (dataStr == null || dataStr.Trim() == "")
            {
                _consoleService.Write(inputMsg);
                dataStr = _consoleService.ReadLine();
            }

            try
            {
                convData = Convert.ToInt32(dataStr);
                dataConv = true;
            }
            catch
            {
                _consoleService.WriteLine("Invalid integer format. Please try again.");
            }
        }

        return convData;
    }

    public string InputString(string msg)
    {
        string? strToRet = null;
        while (strToRet == null || strToRet.Trim() == "")
        {
            _consoleService.Write(msg);
            strToRet = _consoleService.ReadLine();
        }

        return strToRet;
    }

    public DateOnly ConvertToDateOnly(string msg, string format)
    {
        bool dateOnlyConv = false;
        DateOnly dateOnlyVar = new DateOnly();
        while (!dateOnlyConv)
        {
            string dateOnlyVarStr = InputString(msg);
            try
            {
                dateOnlyVar = DateOnly.ParseExact(dateOnlyVarStr, format);
                dateOnlyConv = true;
            }
            catch
            {
                _consoleService.WriteLine($"Invalid date format. Please use {format} format.");
            }
        }

        return dateOnlyVar;
    }
}

public interface ICommandProcessor
{
    void ProcessCommands();
}

public class CommandProcessor : ICommandProcessor
{
    private readonly IConsoleService _consoleService;
    private readonly IStudentRepository _studentRepository;
    private readonly IInputService _inputService;

    public CommandProcessor(
        IConsoleService consoleService,
        IStudentRepository studentRepository,
        IInputService inputService)
    {
        _consoleService = consoleService;
        _studentRepository = studentRepository;
        _inputService = inputService;
    }

    public void ProcessCommands()
    {
        _consoleService.WriteLine(":help to get help");
        string command;

        do
        {
            _consoleService.Write("> ");
            command = _consoleService.ReadLine() ?? string.Empty;

            Student? data = null;
            bool stat = false;
            switch (command.ToLower())
            {
                case "add":
                    string name = _inputService.InputString("Name: ");
                    int yearLevel = _inputService.ConvertToInt("Year Level: ");
                    string course = _inputService.InputString("Course: ");
                    string phoneNumber = _inputService.InputString("Phone Number: ");
                    string email = _inputService.InputString("Email: ");
                    DateOnly birthday = _inputService.ConvertToDateOnly("Birthday(dd-MM-yyyy): ", "dd-MM-yyyy");
                    string location;
                    do
                    {
                        location = _inputService.InputString("Location(before or after or specific): ").ToLower();
                    } while (location != "before" && location != "after" && location != "specific");

                    data = new Student(0, name, yearLevel, course, phoneNumber, email, birthday);
                    if (location == "before")
                        _studentRepository.Prepend(data);
                    else if (location == "after")
                        _studentRepository.Append(data);
                    else
                    {
                        if (_studentRepository.GetHead() == null)
                            _studentRepository.Insert(data, 0);
                        else
                        {
                            var curNode = _studentRepository.GetHead();
                            int i = 0;
                            while (curNode != null)
                            {
                                curNode.Print();
                                ConsoleKey key;
                                do
                                {
                                    _consoleService.Write("Insert here?(y or n): ");
                                    key = _consoleService.ReadKey().Key;
                                } while (key != ConsoleKey.Y && key != ConsoleKey.N);
                                _consoleService.WriteLine("");

                                if (key == ConsoleKey.Y) break;
                                i++;
                                curNode = curNode.Next;
                            }

                            _studentRepository.Insert(data, i);
                        }
                    }

                    break;
                case "get":
                    string idName;
                    do
                        idName = _inputService.InputString("Search by (id or name): ").ToLower();
                    while (idName != "id" && idName != "name");

                    if (idName == "id")
                    {
                        int idToGet = _inputService.ConvertToInt("Id: ");
                        var studentNode = _studentRepository.GetById(idToGet);
                        if (studentNode == null)
                            _consoleService.WriteLine($"Unable to find student with id = {idToGet}");
                        else
                            studentNode.Print();
                    }
                    else
                    {
                        string nameToGet = _inputService.InputString("Name: ");
                        var studentNodes = _studentRepository.GetByName(nameToGet);
                        if (studentNodes.Count() == 0) _consoleService.WriteLine($"Unable to find student with name = {nameToGet}");
                        else foreach (var studentNode in studentNodes) studentNode.Print();
                    }

                    break;

                case "remove":
                    int idToRemove = _inputService.ConvertToInt("Id: ");
                    stat = _studentRepository.RemoveById(idToRemove);
                    if (stat)
                        _consoleService.WriteLine($"Successfully deleted student with id = {idToRemove}");
                    else
                        _consoleService.WriteLine($"Unable to delete student with id = {idToRemove}");
                    break;

                case "update":
                    int idToUpdate = _inputService.ConvertToInt("Id: ");
                    int newId = _inputService.ConvertToInt("New Id: ");
                    string nameToUpdate = _inputService.InputString("Name to Update: ");
                    int yearLevelToUpdate = _inputService.ConvertToInt("Year Level: ");
                    string courseToUpdate = _inputService.InputString("Course to Update: ");
                    string phoneNumberToUpdate = _inputService.InputString("Phone Number to Update: ");
                    string emailToUpdate = _inputService.InputString("Email to Update: ");
                    DateOnly birthdayToUpdate =
                        _inputService.ConvertToDateOnly("Birthday to Update(dd-MM-yyyy): ", "dd-MM-yyyy");

                    data = new Student(newId, nameToUpdate, yearLevelToUpdate, courseToUpdate, phoneNumberToUpdate,
                        emailToUpdate, birthdayToUpdate);

                    stat = _studentRepository.UpdateNode(idToUpdate, data);
                    if (stat)
                        _consoleService.WriteLine($"Successfully updated student with id = {idToUpdate}");
                    else
                        _consoleService.WriteLine($"Unable to update student with id = {idToUpdate}");
                    break;

                case "print":
                    _studentRepository.Print();
                    break;

                case "help":
                    _consoleService.WriteLine("Help:");
                    _consoleService.WriteLine("Commands: add, get, remove, update, print, exit");
                    break;

                case "exit":
                    break;

                default:
                    if (!string.IsNullOrWhiteSpace(command))
                        _consoleService.WriteLine("Unknown command");
                    break;
            }
        } while (command.ToLower() != "exit");
    }
}

public interface IId
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class DoublyNode<T> where T : class, IId
{
    public DoublyNode<T>? Next;
    public DoublyNode<T>? Prev;
    private T _data;

    public DoublyNode(T data)
    {
        _data = data;
    }

    public void Append(T data)
    {
        Next = new DoublyNode<T>(data);
        Next.Prev = this;
    }

    public DoublyNode<T>? Prepend(T data)
    {
        this.Prev = new DoublyNode<T>(data);
        this.Prev.Next = this;
        return this.Prev;
    }

    public void Insert(T data)
    {
        DoublyNode<T> newNode = new DoublyNode<T>(data);
        if (this.Prev != null)
        {
            this.Prev.Next = newNode;
        }

        this.Prev = newNode;
        newNode.Next = this;
    }

    public void Print()
    {
        Console.WriteLine(_data.ToString());
    }

    public bool EqId(int other) => _data.Id == other;

    public void UpdateData(T data) => _data = data;

    public int Id() => _data.Id;

    public bool Contains(string name)
    {
        string[] nameParts = _data.Name.Split();
        bool res = false;
        foreach (var namePart in nameParts)
        {
            if (namePart.ToLower().Trim() == name.ToLower().Trim()) res = true;
        }
        return res;
    }
}

class DoublyLinkedList<T> where T : class, IId
{
    public DoublyNode<T>? Head;
    private int _curId;
    private int _length;

    public DoublyLinkedList()
    {
        _curId = 0;
        _length = 0;
    }

    public void Append(T data)
    {
        _curId += 1;
        _length += 1;
        data.Id = _curId;
        if (Head == null) Head = new DoublyNode<T>(data);
        else
        {
            DoublyNode<T> curNode = Head;
            while (curNode.Next != null) curNode = curNode.Next;
            curNode.Append(data);
        }
    }

    public void Prepend(T data)
    {
        _curId += 1;
        _length += 1;
        data.Id = _curId;
        if (Head == null) Head = new DoublyNode<T>(data);
        else Head = Head.Prepend(data);
    }


    public void Insert(T data, int index)
    {
        _curId += 1;
        _length += 1;
        data.Id = _curId;
        if (Head == null) Head = new DoublyNode<T>(data);
        else
        {
            if (index > _length)
            {
                Console.WriteLine("Selected index greater than length of list");
                return;
            }

            DoublyNode<T>? curNode = Head;
            for (int i = 0; i < index; i++)
                if (curNode.Next != null)
                    curNode = curNode.Next;
            Console.WriteLine();
            if (curNode.Prev == null && index == 0) Head = curNode.Prepend(data);
            else if (curNode.Next == null && index == _length - 1) curNode.Append(data);
            else curNode.Insert(data);
        }
    }

    public void Print()
    {
        if (Head == null) Console.WriteLine("No elements in list");
        else
        {
            DoublyNode<T>? curNode = Head;
            while (curNode != null)
            {
                curNode.Print();

                ConsoleKey key;
                do
                {
                    Console.Write("Continue? (y or n): ");
                    key = Console.ReadKey().Key;
                    Console.WriteLine();
                } while (key != ConsoleKey.Y && key != ConsoleKey.N);
                if (key == ConsoleKey.N) break;

                curNode = curNode.Next;
            }
        }
    }

    public DoublyNode<T>? GetId(int id)
    {
        if (Head != null)
        {
            DoublyNode<T>? curNode = Head;
            while (curNode != null)
            {
                if (curNode.EqId(id)) return curNode;
                curNode = curNode.Next;
            }
        }

        return null;
    }

    public bool RemoveId(int id)
    {
        if (Head != null)
        {
            DoublyNode<T>? curNode = Head;
            while (curNode != null)
            {
                if (curNode.EqId(id))
                {
                    if (curNode.Prev == null) Head = curNode.Next;
                    else curNode.Prev.Next = curNode.Next;

                    return true;
                }

                curNode = curNode.Next;
            }
        }

        return false;
    }

    public bool UpdateNode(int id, T data)
    {
        if (Head != null)
        {
            DoublyNode<T>? curNode = Head;
            while (curNode != null)
            {
                if (curNode.EqId(id))
                {
                    curNode.UpdateData(data);
                    return true;
                }

                curNode = curNode.Next;
            }
        }

        return false;
    }

    public List<DoublyNode<T>> GetName(string name)
    {
        List<DoublyNode<T>> nodes = new List<DoublyNode<T>>();
        if (Head != null)
        {
            DoublyNode<T>? curNode = Head;
            while (curNode != null)
            {
                if (curNode.Contains(name))
                {
                    nodes = nodes.Append(curNode).ToList();
                }
                curNode = curNode.Next;
            }
        }

        return nodes;
    }

    public int Len() => _length;
}

public class Student : IId
{
    public int Id { get; set; }
    public string Name { get; set; }
    private int _yearLevel;
    private string _course;
    private string _phoneNumber;
    private string _email;
    private DateOnly _birthday;

    public Student(int id, string name, int yearLevel, string course, string phoneNumber, string email,
        DateOnly birthday)
    {
        Id = id;
        Name = name;
        _yearLevel = yearLevel;
        _course = course;
        _phoneNumber = phoneNumber;
        _email = email;
        _birthday = birthday;
    }

    public override string ToString() =>
        $"id = {Id}, name = {Name}, year level = {_yearLevel}, course = {_course}, phone number = {_phoneNumber}, email = {_email}, birthday = {_birthday.ToString()}";
}

static class Program
{
    static void Main()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IConsoleService, ConsoleService>();
        services.AddSingleton<IStudentRepository, StudentRepository>();
        services.AddSingleton<IInputService, InputService>();
        services.AddTransient<ICommandProcessor, CommandProcessor>();

        using var serviceProvider = services.BuildServiceProvider();

        var commandProcessor = serviceProvider.GetRequiredService<ICommandProcessor>();
        commandProcessor.ProcessCommands();
    }
}