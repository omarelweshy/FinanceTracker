namespace FinanceTracker.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string FullName { get; private set; }
    public string PasswordHash { get; private set; }                                                                                                                                       
    public string Currency { get; private set; }
    public DateTime CreatedAt { get; private set; }                                                                                                                                        
                                                            
    private User() { }                                                                                                                                                                     
   
    public static User Create(string email, string fullName, string passwordHash, string currency)                                                                                         
    {                                                     
        return new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FullName = fullName,
            PasswordHash = passwordHash,                                                                                                                                                   
            Currency = currency,
            CreatedAt = DateTime.UtcNow                                                                                                                                                    
        };                                                
    }

}