using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace dejtingsajt.Models
{
    public class Message
    {
        [Key]
        public int messageId { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        // AspNetUsers
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
    }
}
