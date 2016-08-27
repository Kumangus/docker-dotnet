using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NameDirectoryService.Models
{
    public class NameDirectory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String CreatedTimestamp { get; set; }
   }
}