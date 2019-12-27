using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task.Api.Models
{
    [Table("Cases")]
    public class Case
    {
        [Key]
        public int CaseId { get; set; }
        [Required]
        public int CaseNum { get; set; }
        public TaskModel Task { get; set; }
        [Required]
        [ForeignKey("FK_Task_TaskGuid")]
        public Guid TaskGuid { get; set; }
        [Required]
        public string FirstInputParameter { get; set; }
        public string SecondInputParameter { get; set; }
        [Required]
        public string ValidReturnValue { get; set; }
        [Required]
        public bool SecretTest { get; set; }

    }
}