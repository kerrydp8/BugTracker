using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using BugTracker.Models;
using System.Net.Mail;
using System.Web.Configuration;
using System.Net;

    public class EmailService : IIdentityMessageService
    {
    public async Task SendAsync(IdentityMessage message)
    {
        // Plug in your email service here to send an email.awaitSendMailAsync(message);}
        await SendMailAsync(message);
    }

    public async Task<bool> SendMailAsync(IdentityMessage message)
    {
        //Private.config set up

        var GmailUsername = WebConfigurationManager.AppSettings["username"];
        var GmailPassword = WebConfigurationManager.AppSettings["password"];
        var host = WebConfigurationManager.AppSettings["host"];
        int port = Convert.ToInt32(WebConfigurationManager.AppSettings["port"]);
        var from = new MailAddress(WebConfigurationManager.AppSettings["emailfrom"], "BugTracker");

        //Email object set up
        var email = new MailMessage(from, new MailAddress(message.Destination))
        {
            Subject = message.Subject,
            Body = message.Body,
            IsBodyHtml = true,
        };
        using (var smtp = new SmtpClient()
        {
            Host = host,
            Port = port,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(GmailUsername, GmailPassword)
        })
        {
            try
            {
                await smtp.SendMailAsync(email);
                return true;
            }
            catch
            {
                return false;
            }
        };
    }
}

    
