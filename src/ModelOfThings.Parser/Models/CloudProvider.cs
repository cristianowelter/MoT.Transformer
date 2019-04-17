using System;
using System.ComponentModel.DataAnnotations;

namespace ModelOfThings.Parser.Models
{
    public class CloudProvider
    {
        [Key]
        public string Id { get; set; }
        public string MddApplicationId { get; set; }
        public Provider Provider { get; set; }
        [Required]
        public string AccountId { get; set; }
        [Required]
        public string AccessKey { get; set; }
        [Required]
        public string SecretAccessKey { get; set; }

        public virtual MddApplication MddApplication { get; set; }
    }

    public enum Provider
    {
        Amazon,
        Google,
        Microsoft
    }
}