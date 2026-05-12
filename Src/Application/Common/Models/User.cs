using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coreapi.Application.Common.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public bool IsManager { get; set; }
    }
	public class UsersInput
	{
		public string Email { get; set; }
		public string Name { get; set; }
		public string UserId { get; set; }
		public bool Organizer { get; set; }

	}
}
