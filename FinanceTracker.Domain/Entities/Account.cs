using FinanceTracker.Domain.Enums;

namespace FinanceTracker.Domain.Entities;

public class Account
{
      public Guid Id { get; private set; }                                                                                                                                                   
      public Guid UserId { get; private set; }                                                                                                                                               
      public string Name { get; private set; }              
      public AccountType Type { get; private set; }
      public decimal Balance { get; private set; }                                                                                                                                           
      public string Currency { get; private set; }
      public bool IsActive { get; private set; }                                                                                                                                             
      public DateTime CreatedAt { get; private set; }       
                                                                                                                                                                                             
      private Account() { }
                                                                                                                                                                                             
      public static Account Create(Guid userId, string name, AccountType type, string currency)
      {
          return new Account
          {
              Id = Guid.NewGuid(),
              UserId = userId,                                                                                                                                                               
              Name = name,
              Type = type,                                                                                                                                                                   
              Balance = 0,                                  
              Currency = currency,
              IsActive = true,
              CreatedAt = DateTime.UtcNow
          };
      }
                                                                                                                                                                                             
      public void UpdateBalance(decimal amount)
      {                                                                                                                                                                                      
          Balance += amount;                                
      }

      public void Deactivate() => IsActive = false;


}