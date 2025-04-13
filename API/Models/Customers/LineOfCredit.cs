namespace API.Models.Customers;

/// <summary>
/// Represents a line of credit with a balance that can be manipulated through deposits and withdrawals.
/// </summary>
public class LineOfCredit
{
    private decimal _balance;
    private readonly List<CreditTransaction> _transactions = new();
    
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the provider of the line of credit.
    /// </summary>
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the credit limit for this line of credit.
    /// </summary>
    public decimal CreditLimit { get; set; }

    /// <summary>
    /// Gets or sets the annual interest rate for this line of credit.
    /// </summary>
    public decimal AnnualInterestRate { get; set; }

    /// <summary>
    /// Gets or sets the date when this line of credit was opened.
    /// </summary>
    public DateTime OpenDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date when the next payment is due.
    /// </summary>
    public DateTime NextPaymentDueDate { get; set; }

    /// <summary>
    /// Gets or sets the minimum payment amount.
    /// </summary>
    public decimal MinimumPaymentAmount { get; set; }

    /// <summary>
    /// Gets or sets the credit score of the customer at the time of opening the line of credit.
    /// </summary>
    public int CreditScore { get; set; }

    /// <summary>
    /// Gets or sets the status of the line of credit (Active, Suspended, Closed).
    /// </summary>
    public string Status { get; set; } = "Active";

    /// <summary>
    /// Gets or sets the date when the account was last reviewed.
    /// </summary>
    public DateTime LastReviewDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets the current balance of the line of credit.
    /// </summary>
    /// <returns>The current balance.</returns>
    public decimal GetBalance()
    {
        return _balance;
    }

    /// <summary>
    /// Gets the available credit (credit limit minus current balance).
    /// </summary>
    /// <returns>The available credit amount.</returns>
    public decimal GetAvailableCredit()
    {
        return CreditLimit - _balance;
    }

    /// <summary>
    /// Gets a list of all transactions for this line of credit.
    /// </summary>
    /// <returns>An IReadOnlyList of CreditTransaction objects.</returns>
    public IReadOnlyList<CreditTransaction> GetTransactionHistory()
    {
        return _transactions.AsReadOnly();
    }

    /// <summary>
    /// Deposits a specified amount into the line of credit.
    /// </summary>
    /// <param name="amount">The amount to deposit. Must be positive.</param>
    /// <param name="description">Optional description of the transaction.</param>
    /// <exception cref="ArgumentException">Thrown when the deposit amount is not positive.</exception>
    public void Deposit(decimal amount, string description = "Payment")
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Deposit amount must be positive", nameof(amount));
        }
        
        _balance += amount;
        
        _transactions.Add(new CreditTransaction
        {
            TransactionType = "Deposit",
            Amount = amount,
            Description = description,
            TransactionDate = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Withdraws a specified amount from the line of credit.
    /// </summary>
    /// <param name="amount">The amount to withdraw. Must be positive and not exceed the available credit.</param>
    /// <param name="description">Optional description of the transaction.</param>
    /// <exception cref="ArgumentException">Thrown when the withdrawal amount is not positive.</exception>
    /// <exception cref="InvalidOperationException">Thrown when there is insufficient available credit.</exception>
    public void Withdraw(decimal amount, string description = "Purchase")
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Withdrawal amount must be positive", nameof(amount));
        }
        
        if (amount > GetAvailableCredit())
        {
            throw new InvalidOperationException("Insufficient available credit");
        }
        
        _balance += amount; // Increase balance (debt) when withdrawing
        
        _transactions.Add(new CreditTransaction
        {
            TransactionType = "Withdrawal",
            Amount = amount,
            Description = description,
            TransactionDate = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Calculates the interest for the current billing period.
    /// </summary>
    /// <returns>The interest amount.</returns>
    public decimal CalculateInterest()
    {
        // Simple monthly interest calculation (APR / 12)
        return _balance * (AnnualInterestRate / 100 / 12);
    }

    /// <summary>
    /// Applies the calculated interest to the balance.
    /// </summary>
    public void ApplyInterest()
    {
        decimal interestAmount = CalculateInterest();
        
        _balance += interestAmount;
        
        _transactions.Add(new CreditTransaction
        {
            TransactionType = "Interest",
            Amount = interestAmount,
            Description = "Monthly Interest",
            TransactionDate = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Calculates the minimum payment for the current billing period.
    /// </summary>
    /// <returns>The minimum payment amount.</returns>
    public decimal CalculateMinimumPayment()
    {
        // Typically a percentage of the balance or a fixed amount, whichever is greater
        decimal percentageOfBalance = _balance * 0.02m; // 2% of balance
        decimal minimumPayment = Math.Max(percentageOfBalance, 25.0m); // At least $25
        
        return Math.Min(minimumPayment, _balance); // Can't be more than the balance
    }

    /// <summary>
    /// Determines if the account is past due.
    /// </summary>
    /// <returns>True if the account is past due, false otherwise.</returns>
    public bool IsPastDue()
    {
        return DateTime.UtcNow > NextPaymentDueDate && _balance > 0;
    }
}

/// <summary>
/// Represents a transaction in a line of credit.
/// </summary>
public class CreditTransaction
{
    /// <summary>
    /// Gets or sets the unique identifier for the transaction.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the type of transaction (Deposit, Withdrawal, Interest, Fee).
    /// </summary>
    public string TransactionType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the amount of the transaction.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the description of the transaction.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the transaction occurred.
    /// </summary>
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}