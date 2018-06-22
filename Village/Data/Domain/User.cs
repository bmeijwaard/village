using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Village.Data.Domain
{
    [Table("Users")]
    public class User : IdentityUser<Guid>
    {
        public override Guid Id { get; set; }

        public override string UserName { get; set; }

        public override bool LockoutEnabled { get; set; }

        public override DateTimeOffset? LockoutEnd { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }
    }
}
