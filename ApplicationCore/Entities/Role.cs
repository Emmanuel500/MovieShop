using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        // Navigation Propertys
        public ICollection<UserRole> UserRole { get; set; }
    }
}
