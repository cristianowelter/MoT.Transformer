using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ModelOfThings.Parser.Models
{
    public class UmlModel
    {
        public string Id { get; set; }
        public string MddApplicationId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string UserId { get; set; }

        [NotMapped]
        public IFormFile UmlModelFile { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual MddApplication MddApplication { get; set; }
    }
}