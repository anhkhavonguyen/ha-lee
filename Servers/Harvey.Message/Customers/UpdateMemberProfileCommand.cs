using System;

namespace Harvey.Message.Customers
{
    public interface UpdateMemberProfileCommand
    {
        string UserId { get; }
        string FirstName { get; }
        string LastName { get; }
        string Email { get; }
        string ProfileImage { get; }
        string UpdatedBy { get; }
        DateTime? DateOfBirth { get; }
        int? Gender { get; }
    }
}
