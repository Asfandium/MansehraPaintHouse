//using System.Threading.Tasks;

//namespace MansehraPaintHouse.Core.Interfaces.IServices
//{
//    public interface IEmailService
//    {
//        Task SendEmailAsync(string email, string subject, string message);
//        Task SendPasswordResetEmailAsync(string email, string resetLink);
//        Task SendEmailConfirmationAsync(string email, string confirmationLink);
//    }
//} 

namespace MansehraPaintHouse.Core.Interfaces.IServices
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendPasswordResetEmailAsync(string email, string resetLink);
        Task SendEmailConfirmationAsync(string email, string confirmationLink);
    }
}