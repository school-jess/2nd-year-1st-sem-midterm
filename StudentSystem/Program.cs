// See https://aka.ms/new-console-template for more information

namespace StudentSystem;

interface IId
{
    public int Id { get; set; }
    public string Name { get; set; }
}

class DoublyNode<T> where T : class, IId
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

    public DoublyNode<T> Insert(T data)
    {
        DoublyNode<T> newNode = new DoublyNode<T>(data);
        if (this.Prev != null)
        {
            this.Prev.Next = newNode;
        }
        this.Prev = newNode;
        newNode.Next = this;
        return newNode;
    }

    public void Print()
    {
        Console.WriteLine(_data.ToString());
    }

    public bool EqId(int other) => _data.Id == other;

    public void UpdateData(T data) => _data = data;

    public int Id() => _data.Id;

    public bool EqName(string other) => _data.Name == other;
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
            if (curNode.Prev == null)
            {
                DoublyNode<T> newHead = curNode.Insert(data);
                Head = newHead;
            }
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

    public DoublyNode<T>? GetName(string name)
    {
        if (Head != null)
        {
            DoublyNode<T>? curNode = Head;
            while (curNode != null)
            {
                if (curNode.EqName(name)) return curNode;
                curNode = curNode.Next;
            }
        }

        return null;
    }
}

class Student : IId
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

class Program
{
    static int convInt(string inputMsg)
    {
        bool dataConv = false;
        int convData = 0;
        while (!dataConv)
        {
            string? dataStr = null;
            while (dataStr == null)
            {
                Console.Write(inputMsg);
                dataStr = Console.ReadLine();
            }

            try
            {
                convData = Convert.ToInt32(dataStr);
                dataConv = true;
            }
            catch
            {
            }
        }

        return convData;
    }

    static string inputStr(string msg)
    {
        string? strToRet = null;
        while (strToRet == null)
        {
            Console.Write(msg);
            strToRet = Console.ReadLine();
        }

        return strToRet;
    }

    static DateOnly convDateOnly(string msg, string format)
    {
        bool dateOnlyConv = false;
        DateOnly dateOnlyVar = new DateOnly();
        while (!dateOnlyConv)
        {
            string dateOnlyVarStr = inputStr(msg);
            try
            {
                dateOnlyVar = DateOnly.ParseExact(dateOnlyVarStr, format);
                dateOnlyConv = true;
            }
            catch
            {
            }
        }

        return dateOnlyVar;
    }

    static void Main()
    {
        DoublyLinkedList<Student> studentLL = new DoublyLinkedList<Student>();
        string command = "";
        Console.WriteLine(":help to get help");
        do
        {
            command = inputStr("> ");

            Student? data = null;
            bool stat = false;
            switch (command)
            {
                case "add":
                    string name = inputStr("Name: ");
                    int yearLevel = convInt("Year Level: ");
                    string course = inputStr("Course: ");
                    string phoneNumber = inputStr("Phone Number: ");
                    string email = inputStr("Email: ");
                    DateOnly birthday = convDateOnly("Birthday(dd-MM-yyyy): ", "dd-MM-yyyy");
                    string befAft = "";
                    do befAft = inputStr("Location(before or after): ");
                    while (befAft != "before" && befAft != "after");
                    data = new Student(0, name, yearLevel, course, phoneNumber, email, birthday);
                    if (befAft == "before") studentLL.Prepend(data);
                    else if (befAft == "after") studentLL.Append(data);
                    break;
                case "insert":
                    string nameToInsert = inputStr("Name: ");
                    int yearLevelToInsert = convInt("Year Level: ");
                    string courseToInsert = inputStr("Course: ");
                    string phoneNumberToInsert = inputStr("Phone Number: ");
                    string emailToInsert = inputStr("Email: ");
                    DateOnly birthdayToINsert = convDateOnly("Birthday(dd-MM-yyyy): ", "dd-MM-yyyy");
                    data = new Student(0, nameToInsert, yearLevelToInsert, courseToInsert, phoneNumberToInsert,
                        emailToInsert, birthdayToINsert);
                    if (studentLL.Head == null) studentLL.Insert(data, 0);
                    else
                    {
                        DoublyNode<Student> curNode = studentLL.Head;
                        int i = 0;
                        while (true)
                        {
                            curNode.Print();
                            ConsoleKey key = ConsoleKey.A;
                            while (key != ConsoleKey.Y && key != ConsoleKey.N)
                            {
                                Console.Write("Insert here?(y or n):");
                                key = Console.ReadKey().Key;
                                Console.WriteLine();
                                if (key == ConsoleKey.N) i++;
                                if (key == ConsoleKey.Y) break;
                            }

                            if (curNode.Next == null) break;
                            curNode = curNode.Next;
                        }

                        studentLL.Insert(data, i);
                    }

                    break;
                case "get":
                    string idName = "";
                    do idName = inputStr("Location(before or after): ");
                    while (idName != "id" && idName != "name");
                    if (idName == "id")
                    {
                        int idToGet = convInt("Id: ");
                        DoublyNode<Student>? studentNode = studentLL.GetId(idToGet);
                        if (studentNode == null) Console.WriteLine($"Unable to find student with id = {idToGet}");
                        else studentNode.Print();
                    }
                    else
                    {
                        string nameToGet = inputStr("Name: ");
                        DoublyNode<Student>? studentNode = studentLL.GetName(nameToGet);
                        if (studentNode == null) Console.WriteLine($"Unable to find student with name = {nameToGet}");
                        else studentNode.Print();
                    }
                    break;
                case "remove":
                    int idToRemove = convInt("Id: ");
                    stat = studentLL.RemoveId(idToRemove);
                    if (stat) Console.WriteLine($"Successfully deleted student with id = {idToRemove}");
                    else Console.WriteLine($"Unable to delete student with id = {idToRemove}");
                    break;
                case "update":
                    int idToUpdate = convInt("Id: ");
                    int newId = convInt("New Id: ");
                    string nameToUpdate = inputStr("Name to Update: ");
                    int yearLevelToUpdate = convInt("Year Level: ");
                    string courseToUpdate = inputStr("Course to Update: ");
                    string phoneNumberToUpdate = inputStr("Phone Number to Update: ");
                    string emailToUpdate = inputStr("Email to Update: ");
                    DateOnly birthdayToUpdate = convDateOnly("Birthday to Update(dd-MM-yyyy): ", "dd-MM-yyyy");
                    data = new Student(newId, nameToUpdate, yearLevelToUpdate, courseToUpdate, phoneNumberToUpdate,
                        emailToUpdate, birthdayToUpdate);
                    stat = studentLL.UpdateNode(idToUpdate, data);
                    if (stat) Console.WriteLine($"Successfully updated student with id = {idToUpdate}");
                    else Console.WriteLine($"Unable to update student with id = {idToUpdate}");
                    break;
                case "print":
                    studentLL.Print();
                    break;
                case "help":
                    Console.WriteLine("Help:");
                    Console.WriteLine("Commands: add, get, remove, update, print, insert, exit");
                    break;
                case "exit":
                    break;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        } while (command != "exit");

        Console.WriteLine("Goodbye!");
    }
}
