using Microsoft.AspNetCore.Identity;
using System;

namespace Village.Data.Domain
{
    public sealed class Role : IdentityRole<Guid>
    {
        public Role() : base()
        {

        }
        public Role(string name)
        {
            Name = name;
        }
    }
}
