public abstract class Account
{
    public Account(decimal initialBalance)
    {
        Balance = initialBalance;
    }

    public decimal Balance { get; protected set; }
    public abstract void Withdraw(decimal amount);
    public abstract void Deposit(decimal amount);
}

public class UnsafeAccount : Account
{
    public UnsafeAccount(decimal initialBalance) : base(initialBalance) { }

    public override void Withdraw(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "The withdrawal amount cannot be negative.");

        if (amount > Balance)
            throw new ArgumentOutOfRangeException(nameof(amount),
                "The withdrawal amount cannot be greater than the balance.");

        Balance -= amount;
    }

    public override void Deposit(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "The deposit amount cannot be negative.");

        Balance += amount;
    }
}

public class SafeAccount : Account
{
    private readonly object _lock = new { };

    public SafeAccount(decimal initialBalance) : base(initialBalance) { }

    public override void Withdraw(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "The withdrawal amount cannot be negative.");

        if (amount > Balance)
            throw new ArgumentOutOfRangeException(nameof(amount),
                "The withdrawal amount cannot be greater than the balance.");

        lock (_lock)
        {
            Balance -= amount;
        }
    }

    public override void Deposit(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), "The deposit amount cannot be negative.");

        lock (_lock)
        {
            Balance += amount;
        }
    }
}