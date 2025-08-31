// See https://aka.ms/new-console-template for more information
namespace OOPMidterm;

interface IId
{
    public int id;
}

class SinglyLinkedList<T>
{
    public SinglyLinkedList? Next;
    private T data;
}

class Student : IId
{

}

class Program
{
    static void Main()
    {
        Console.WriteLine("hello");
    }
}
