﻿// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/lock

const int runs = 100;

var unsafeAccount = new UnsafeAccount(0);
var safeAccount = new SafeAccount(0);

var depositTasks = new List<Task>();
var withdrawalTasks = new List<Task>();

// Start unsafe account balance test:

for (int i = 0; i < runs; i++)
    depositTasks.Add(Task.Run(() => Deposit(unsafeAccount)));

await Task.WhenAll(depositTasks);

for (int i = 0; i < runs; i++)
    withdrawalTasks.Add(Task.Run(() => Withdraw(unsafeAccount)));

await Task.WhenAll(withdrawalTasks);

Console.WriteLine($"Unsafe balance: {unsafeAccount.Balance}");

depositTasks.Clear();
withdrawalTasks.Clear();
Console.WriteLine();

// Start safe account balance test:

for (int i = 0; i < runs; i++)
    depositTasks.Add(Task.Run(() => Deposit(safeAccount)));

await Task.WhenAll(depositTasks);

for (int i = 0; i < runs; i++)
    withdrawalTasks.Add(Task.Run(() => Withdraw(safeAccount)));

await Task.WhenAll(withdrawalTasks);

Console.WriteLine($"Safe balance: {safeAccount.Balance}");

void Withdraw(Account account)
{
    try
    {
        account.Withdraw(1);
    }
    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine(e.Message);
    }
}

void Deposit(Account account)
{
    try
    {
        account.Deposit(1);
    }
    catch (ArgumentOutOfRangeException e)
    {
        Console.WriteLine(e.Message);
    }
}
