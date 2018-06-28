using System;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net.Mail;
using System.Configuration;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FutureSoft.Util
{
    /// <summary>
    /// Summary description for SendMail.
    /// </summary>
    public class MailComponent
    {

        #region properties and Variables
        /// <summary>
        /// From Mail address
        /// </summary>
        private string _fromUserName;
        public string FromUserName
        {
            get
            {
                return _fromUserName;
            }
            set
            {
                _fromUserName = "InCash::<" + value + ">";
                //_fromUserName=value;

            }
        }

        /// <summary>
        /// To Mail Address
        /// </summary>
        private string _toMailID;
        public string ToMailID
        {
            get
            {
                return _toMailID;
            }
            set
            {
                _toMailID = value;
            }
        }

        /// <summary>
        /// Subject
        /// </summary>
        private string _subject;
        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }
        /// <summary>
        ///Body
        /// </summary>
        private string _bodyContent;
        public string BodyContent
        {
            get
            {
                return _bodyContent;
            }
            set
            {
                _bodyContent = value;
            }
        }
        /// <summary>
        /// CC address
        /// </summary>
        private string _ccMailid;
        public string CCMailId
        {
            get
            {
                return _ccMailid;
            }
            set
            {
                _ccMailid = value;
            }
        }
        /// <summary>
        /// BCC Mail Id
        /// </summary>
        private string _bccMailId;
        public string _BCCMailId
        {
            get
            {
                return _bccMailId;
            }
            set
            {
                _bccMailId = value;
            }
        }
        /// <summary>
        /// Attachements
        /// </summary>
        private string _attachementDocs;
        public string AttachmentDocs
        {
            get
            {
                return _attachementDocs;
            }
            set
            {
                _attachementDocs = value;
            }
        }
        /// <summary>
        /// Attachements
        /// </summary>
        private string _attachementDocs2;
        public string AttachmentDocs2
        {
            get
            {
                return _attachementDocs2;
            }
            set
            {
                _attachementDocs2 = value;
            }
        }
        /// <summary>
        /// Attachements
        /// </summary>
        private string _attachementDocs3;
        public string AttachmentDocs3
        {
            get
            {
                return _attachementDocs3;
            }
            set
            {
                _attachementDocs3 = value;
            }
        }

        #endregion

        #region Declarations
        MailMessage objMailMessage;

        #endregion

        public MailComponent()
        {

        }

        #region Send Mail
        public static void SendEmail(string sToEmail, string sCcEmail, string sBCcEmail, string sSubject, string sBody, Attachment objAttachment, AlternateView objAlternateView, bool bIsHtml, System.IO.Stream attchmentFileStream, string attchmentFileName)
        {
            string SMTPClient = ConfigurationManager.AppSettings["SMTPClient"].ToString();
            string SMTPPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
            bool EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"].ToString());
            string FromMail = ConfigurationManager.AppSettings["FromMail"].ToString();
            string SmtpUserId = ConfigurationManager.AppSettings["SmtpUserId"].ToString();
            string SmtpCredential = ConfigurationManager.AppSettings["SmtpCredential"].ToString();

            int iPort = 0;

            iPort = Convert.ToInt32(SMTPPort);

            if (iPort == 465)
            {
                try
                {
                    var client = new MailKit.Net.Smtp.SmtpClient();
                    client.Connect(SMTPClient, 465, true);

                    // Note: since we don't have an OAuth2 token, disable the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Authenticate(SmtpUserId, SmtpCredential);

                    var msg = new MimeKit.MimeMessage();
                    msg.From.Add(new MimeKit.MailboxAddress(FromMail));

                    string[] ToMuliId = sToEmail.Split(',');
                    foreach (string ToEMailId in ToMuliId)
                    {
                        msg.To.Add(new MimeKit.MailboxAddress(ToEMailId));
                    }

                    if (!string.IsNullOrEmpty(sCcEmail))
                    {
                        string[] CCId = sCcEmail.Split(',');

                        foreach (string CCEmail in CCId)
                        {
                            msg.Cc.Add(new MimeKit.MailboxAddress(CCEmail));
                        }
                    }

                    if (!string.IsNullOrEmpty(sBCcEmail))
                    {
                        string[] bccid = sBCcEmail.Split(',');

                        foreach (string bccEmailId in bccid)
                        {
                            msg.Bcc.Add(new MimeKit.MailboxAddress(bccEmailId));
                        }
                    }

                    var builder = new MimeKit.BodyBuilder();
                    msg.Subject = sSubject;

                    if (bIsHtml)
                    {
                        builder.HtmlBody = sBody;
                    }
                    else
                    {
                        builder.TextBody = sBody;
                    }

                    if (objAttachment != null)
                    {
                        builder.Attachments.Add(attchmentFileName, attchmentFileStream);
                    }

                    // Now we just need to set the message body and we're done
                    msg.Body = builder.ToMessageBody();

                    client.Send(msg);
                    client.Disconnect(true);
                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                    //LogError("An error occurred while [SendEmail] : " + ex.Message.ToString() + " " + ex.StackTrace, "", "SendEmail");
                    throw ex;
                }
            }
            else  //For non 465 ports
            {
                SmtpClient objSmtpClient = null;
                MailMessage objMailMessage = null;
                try
                {
                    objSmtpClient = new SmtpClient(SMTPClient);

                    if (iPort == 25)
                    {
                        objSmtpClient.EnableSsl = false;
                        objSmtpClient.UseDefaultCredentials = true;
                    }
                    else
                    {
                        objSmtpClient.EnableSsl = EnableSSL;
                        if (iPort != 0)
                            objSmtpClient.Port = iPort;

                        if (SmtpCredential != "")
                        {
                            objSmtpClient.Credentials = new System.Net.NetworkCredential(SmtpUserId, SmtpCredential);
                        }
                        else
                        {
                            objSmtpClient.UseDefaultCredentials = true;
                        }
                    }
                    objMailMessage = new MailMessage();
                    string[] ToMuliId = sToEmail.Split(',');
                    foreach (string ToEMailId in ToMuliId)
                    {
                        objMailMessage.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                    }

                    if (sCcEmail != "")
                    {
                        string[] CCId = sCcEmail.Split(',');

                        foreach (string CCEmail in CCId)
                        {
                            objMailMessage.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                        }
                    }

                    if (sBCcEmail != "")
                    {
                        string[] bccid = sBCcEmail.Split(',');

                        foreach (string bccEmailId in bccid)
                        {
                            objMailMessage.Bcc.Add(new MailAddress(bccEmailId)); //Adding Multiple BCC email Id
                        }
                    }
                    //if (iPort != 25)
                    //{
                    MailAddress MailFrom = new MailAddress(FromMail);
                    objMailMessage.From = MailFrom;
                    //}
                    //else
                    //{
                    //    MailAddress MailFrom = new MailAddress();

                    //    MailFrom.DisplayName =  
                    //    objMailMessage.From = MailFrom;
                    //}
                    objMailMessage.Subject = sSubject;
                    objMailMessage.IsBodyHtml = bIsHtml;
                    if (objAlternateView == null)
                    {
                        objMailMessage.Body = sBody;
                    }
                    else
                    {
                        objMailMessage.AlternateViews.Add(objAlternateView);
                    }
                    if (objAttachment != null)
                    {
                        objMailMessage.Attachments.Add(objAttachment);
                    }

                    objSmtpClient.Send(objMailMessage);

                }
                catch (Exception ex)
                {
                    string s = ex.Message;
                    //LogError("An error occurred while [SendEmail] : " + ex.Message.ToString() + " " + ex.StackTrace, "", "SendEmail");
                    throw ex;
                }
                finally
                {
                    if (objMailMessage != null) { objMailMessage.Dispose(); }
                }
            }

        }

        public static void SendEmailNotInUse(string sToEmail, string sCcEmail, string sBCcEmail, string sSubject, string sBody, Attachment objAttachment, AlternateView objAlternateView, bool bIsHtml)
        {
            SmtpClient objSmtpClient = null;
            MailMessage objMailMessage = null;
            int iPort = 0;
            try
            {
                
                string SMTPClient = ConfigurationManager.AppSettings["SMTPClient"].ToString();
                string SMTPPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
                bool EnableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"].ToString());
                string FromMail = ConfigurationManager.AppSettings["FromMail"].ToString();
                string SmtpUserId = ConfigurationManager.AppSettings["SmtpUserId"].ToString();
                string SmtpCredential = ConfigurationManager.AppSettings["SmtpCredential"].ToString();
              
                objSmtpClient = new SmtpClient(SMTPClient);
                iPort = Convert.ToInt32(SMTPPort);

                if (iPort == 25)
                {
                    objSmtpClient.EnableSsl = false;
                    objSmtpClient.UseDefaultCredentials = true;
                }
                else
                {
                    objSmtpClient.EnableSsl = EnableSSL;
                    if (iPort != 0)
                        objSmtpClient.Port = iPort;

                    if (SmtpCredential != "")
                    {
                        objSmtpClient.Credentials = new System.Net.NetworkCredential(SmtpUserId, SmtpCredential);
                    }
                    else
                    {
                        objSmtpClient.UseDefaultCredentials = true;
                    }
                }
                objMailMessage = new MailMessage();
                string[] ToMuliId = sToEmail.Split(',');
                foreach (string ToEMailId in ToMuliId)
                {
                    objMailMessage.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                }

                if (sCcEmail != "")
                {
                    string[] CCId = sCcEmail.Split(',');

                    foreach (string CCEmail in CCId)
                    {
                        objMailMessage.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
                    }
                }

                if (sBCcEmail != "")
                {
                    string[] bccid = sBCcEmail.Split(',');

                    foreach (string bccEmailId in bccid)
                    {
                        objMailMessage.Bcc.Add(new MailAddress(bccEmailId)); //Adding Multiple BCC email Id
                    }
                }
                //if (iPort != 25)
                //{
                MailAddress MailFrom = new MailAddress(FromMail);
                objMailMessage.From = MailFrom;
                //}
                //else
                //{
                //    MailAddress MailFrom = new MailAddress();

                //    MailFrom.DisplayName =  
                //    objMailMessage.From = MailFrom;
                //}
                objMailMessage.Subject = sSubject;
                objMailMessage.IsBodyHtml = bIsHtml;
                if (objAlternateView == null)
                {
                    objMailMessage.Body = sBody;
                }
                else
                {
                    objMailMessage.AlternateViews.Add(objAlternateView);
                }
                if (objAttachment != null)
                {
                    objMailMessage.Attachments.Add(objAttachment);
                }
                objSmtpClient.Send(objMailMessage);
            }
            catch (Exception ex)
            {
               throw ex;
            }
            finally
            {
                if (objMailMessage != null) { objMailMessage.Dispose(); }
            }
        }
  
        #endregion
    }
}
