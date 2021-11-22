using System;
using System.Collections.Generic;
using Core.Entities.Identity;

namespace Core.Dtos
{
    public class UserToReturnDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public List<string> Roles { get; set; }

    }
}