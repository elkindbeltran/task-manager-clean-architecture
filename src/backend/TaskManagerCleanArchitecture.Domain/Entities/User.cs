namespace TaskManagerCleanArchitecture.Domain.Entities;

public class User : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    private User() { }

    public User(string name, string email)
    {
        SetName(name);
        SetEmail(email);
    }

    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        if (name.Length > 100)
            throw new ArgumentException("Name cannot exceed 100 characters");

        Name = name;
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        if (email.Length > 150)
            throw new ArgumentException("Email cannot exceed 150 characters");

        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format");

        Email = email;
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}