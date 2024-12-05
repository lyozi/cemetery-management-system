using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
	public class LoginDTO
	{
		public string UserIdentifier { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;

	}

	public class RegistrationDTO
	{
		public string Username { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;
	}

	public class LoginResponseDTO
	{
		public string Username { get; set; } = string.Empty;
		public string Role { get; set; } = string.Empty;
    public bool Succeeded { get; set; }
    public string Message { get; set; } = string.Empty;
  }
}
