using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FromQueueMailSender.DTO
{
    internal class MessageDto
    {
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverEmail { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
