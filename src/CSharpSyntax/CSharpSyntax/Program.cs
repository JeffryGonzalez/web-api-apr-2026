using CSharpSyntax;

Console.WriteLine("Hello, World!");


var age = 56;



if(age.IsEven())
{
    Console.WriteLine("Even");
}
else
{
    Console.WriteLine("Odd");
}

Console.WriteLine(3.DaysFromNow().ToLongDateString());