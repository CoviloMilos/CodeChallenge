using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace DistributedExceptionHandler.Models
{
    public class ExceptionModel
    {
        [Key]
        public Guid ExceptionGuid { get; set; }
        [Required]
        public string Origin { get; set; }
        [Required]
        public string Type { get; set; }
        public string Code { get; set; }
        public string Detail { get; set; }
        public string StatusCode { get; set; }
        [Required]
        public DateTime ExceptionDate { get; set; }
        public DateTime ExceptionConsumedDate { get; set; }

        public ExceptionModel() 
        {
            ExceptionGuid = Guid.NewGuid();
            ExceptionConsumedDate = DateTime.Now;
        }

        public static ExceptionModel DeserializeModel(String model)
        {
            return JsonConvert.DeserializeObject<ExceptionModel>(model);
        }
    }
}