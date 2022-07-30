using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Model
{
    public class MSMQmodel
    {

        MessageQueue messageQueue = new MessageQueue();

        public void sendData2Queue(string Token)
        {

            messageQueue.Path = @".\private$\Token";

            if(!MessageQueue.Exists(messageQueue.Path))
            {
                //Exists
                MessageQueue.Create(messageQueue.Path);
            }
           

            messageQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            messageQueue.ReceiveCompleted += MessageQueue_ReceiveCompleted;
            messageQueue.Send(Token);
            messageQueue.BeginReceive();
            messageQueue.Close();
        }

        private void MessageQueue_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {

            var msg = messageQueue.EndReceive(e.AsyncResult);
            string Token = msg.Body.ToString();
            string subject = "Fundoo Notes reset link";
            string Body = Token;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("testmsmq@gmail.com");
            mail.To.Add("testmsmq@gmail.com");
            mail.Subject = subject;
            mail.IsBodyHtml = true;

            string htmlBody;

            htmlBody = "<body><p>Dear User,<br>" +
                        "Copy below token to reset your password.<br></body>" +
                        Token;

            mail.Body = htmlBody;


            var SMTP = new SmtpClient("smtp.gmail.com")
            {

                Port = 587,
                Credentials = new NetworkCredential("testmsmq@gmail.com", "vtpsrofqdzaissia"),
                EnableSsl = true

            };

            SMTP.Send(mail);

            messageQueue.BeginReceive();

        }
    }
}
