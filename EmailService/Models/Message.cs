using MimeKit;

namespace EmailService.Models
{
    public class Message
    {

        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public Message(IEnumerable<(string displayName, string email)> to, string subject, string content)
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x => new MailboxAddress(x.displayName, x.email)));
            Subject = subject;
            Content = content;
        }
    }
}
