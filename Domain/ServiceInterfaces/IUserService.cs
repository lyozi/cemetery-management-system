using Domain.DTOs;
using System.Threading.Tasks;

namespace Domain.ServiceInterfaces
{
	public interface IUserService
	{
		Task<(bool Succeeded, string Message, LoginResponseDTO? Response)> LoginAsync(LoginDTO model);
		Task<(bool Succeeded, string Message)> RegisterAsync(RegistrationDTO model);
		Task LogoutAsync();
	}
}
