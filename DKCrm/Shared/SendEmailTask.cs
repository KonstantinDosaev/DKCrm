using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Models;

namespace DKCrm.Shared
{
    public class SendEmailTask
    {
        public Guid Id { get; set; }
        [MaxLength(200)]
        public string Emails { get; set; } = null!;
        [MaxLength(200)]
        public string? PhoneNumbers { get; set; }
        [MaxLength(200)]
        public string Subject { get; set; } = null!;
        [MaxLength(1000)]
        public string Message { get; set; } = null!;
        public  byte[]? AttachmentOne { get; set; }
        public byte[]? AttachmentTwo { get; set; }
        public bool SendByUser { get; set; }
        public bool PrivetTask { get; set; }
        public DateTime DateTimeCreate { get; set; }
        public DateTime DateTimeInit { get; set; }
        [MaxLength(60)]
        public string? UseCreatorId { get; set; }
        public virtual ApplicationUser? UseCreator { get; set; }

    }
}
