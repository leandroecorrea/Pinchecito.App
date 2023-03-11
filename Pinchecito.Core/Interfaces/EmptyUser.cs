namespace Pinchecito.Core.Interfaces
{
    public class EmptyUser : User
    {
        public EmptyUser()
        {
            Username = string.Empty;
            Fullname = string.Empty;
        }
    }
}
