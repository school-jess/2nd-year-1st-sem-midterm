// See https://aka.ms/new-console-template for more information
namespace OOPMidterm;

interface IId
{
    public int Id { get; set; }
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

    public void Print()
    {
        Console.WriteLine(_data.ToString());
    }

    public bool EqId(int other) => _data.Id == other;

    public void UpdateData(T data) => _data = data;

    public int Id() => _data.Id;
}

class DoublyLinkedList<T> where T : class, IId
{
    private DoublyNode<T>? _head;
    private HashSet<int> _idIndices;
    public DoublyLinkedList()
    {
        _idIndices = new HashSet<int>();
    }

    public void Append(T data)
    {
        if (_head == null)
        {
            _head = new DoublyNode<T>(data);
            _idIndices.Add(data.Id);
        }
        else
        {
            if (_idIndices.Contains(data.Id))
            {
                Console.WriteLine($"student with id = {data.Id} already in LL");
                return;
            }
            DoublyNode<T> curNode = _head;
            _idIndices.Add(data.Id);
            while (curNode.Next != null)
            {
                curNode = curNode.Next;
            }
            curNode.Append(data);
        }
    }

    public void Print()
    {
        if (_head == null)
        {
            Console.WriteLine("No elements in LL");
        }
        else
        {
            DoublyNode<T>? curNode = _head;
            while (curNode != null)
            {
                curNode.Print();
                curNode = curNode.Next;
            }
        }
    }

    public DoublyNode<T>? GetId(int id)
    {
        if (_head != null)
        {
            DoublyNode<T>? curNode = _head;
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
        if (_head != null)
        {
            DoublyNode<T>? curNode = _head;
            while (curNode != null)
            {
                if (curNode.EqId(id))
                {
                    if (curNode.Prev == null)
                    {
                        _head = curNode.Next;
                    }
                    else
                    {
                        curNode.Prev.Next = curNode.Next;
                    }

                    return true;
                }

                curNode = curNode.Next;
            }
        }

        return false;
    }
    public bool UpdateNode(int id, T data)
    {
        if (_head != null)
        {
            DoublyNode<T>? curNode = _head;
            while (curNode != null)
            {
                if (curNode.EqId(id))
                {
                    _idIndices.Remove(curNode.Id());
                    curNode.UpdateData(data);
                    return true;
                }
                curNode = curNode.Next;
            }
        }

        return false;
    }
}

class Student : IId
{
    public int Id { get; set; }
    public Student(int id)
    {
        Id = id;
    }

    public override string ToString() => $"id = {Id}";
}

class Program
{
    static void Main()
    {
        DoublyLinkedList<Student> studentLL = new DoublyLinkedList<Student>();
        string command = "";
        Console.WriteLine(":help to get help");
        while (command != "exit")
        {
            while (command == "")
            {
                Console.Write("> ");
                command = Console.ReadLine();
            }

            int id = 0;
            bool idConv = false;
            Student data = new Student(0);
            bool stat = false;
            switch (command)
            {
                case "add":
                    while (!idConv)
                    {
                        string idStr = "";
                        while (idStr == "")
                        {
                            Console.Write("Id: ");
                            idStr = Console.ReadLine();
                        }
                        try
                        {
                            id = Convert.ToInt32(idStr);
                            idConv = true;
                        }
                        catch { }
                    }

                    data = new Student(id);
                    studentLL.Append(data);
                    break;
                case "get":
                    while (!idConv)
                    {
                        string idStr = "";
                        while (idStr == "")
                        {
                            Console.Write("Id: ");
                            idStr = Console.ReadLine();
                        }
                        try
                        {
                            id = Convert.ToInt32(idStr);
                            idConv = true;
                        }
                        catch { }
                    }
                    DoublyNode<Student>? studentNode = studentLL.GetId(id);
                    if (studentNode == null)
                    {
                        Console.WriteLine($"Unable to find student with id = {id}");
                    }
                    else
                    {
                        studentNode.Print();
                    }
                    break;
                case "remove":
                    while (!idConv)
                    {
                        string idStr = "";
                        while (idStr == "")
                        {
                            Console.Write("Id: ");
                            idStr = Console.ReadLine();
                        }
                        try
                        {
                            id = Convert.ToInt32(idStr);
                            idConv = true;
                        }
                        catch { }
                    }
                    stat = studentLL.RemoveId(id);
                    if (stat)
                    {
                        Console.WriteLine($"Successfully deleted student with id = {id}");
                    }
                    else
                    {
                        Console.WriteLine($"Unable to delete student with id = {id}");
                    }
                    break;
                case "update":
                    while (!idConv)
                    {
                        string idStr = "";
                        while (idStr == "")
                        {
                            Console.Write("Id: ");
                            idStr = Console.ReadLine();
                        }
                        try
                        {
                            id = Convert.ToInt32(idStr);
                            idConv = true;
                        }
                        catch { }
                    }
                    int newId = 0;
                    bool newIdConv = false;
                    while (!newIdConv)
                    {
                        string newIdStr = "";
                        while (newIdStr == "")
                        {
                            Console.Write("New Id: ");
                            newIdStr = Console.ReadLine();
                        }
                        try
                        {
                            newId = Convert.ToInt32(newIdStr);
                            newIdConv = true;
                        }
                        catch { }
                    }
                    data = new Student(newId);
                    stat = studentLL.UpdateNode(id, data);
                    if (stat)
                    {
                        Console.WriteLine($"Successfully updated student with id = {id}");
                    }
                    else
                    {
                        Console.WriteLine($"Unable to update student with id = {id}");
                    }
                    break;
                case "print":
                    studentLL.Print();
                    break;
                case "help":
                    Console.WriteLine("Help:");
                    Console.WriteLine("Commands: add, get, remove, update, print");
                    break;
                case "exit":
                    goto quit;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
            command = "";
        }
    quit:
        Console.WriteLine("Goodbye!");
    }
}
