using System;
using System.ComponentModel.DataAnnotations;

namespace Village.Models.Example
{
    public class ExampleViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
