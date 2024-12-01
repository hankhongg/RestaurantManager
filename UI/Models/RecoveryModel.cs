using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RestaurantManager.Models
{
    class RecoveryModel
    {
        public string generateCode(int length=5)
        {
            Random random = new Random();
            string code = string.Empty;
            for (int i =0; i < length; i++)
            {
                code += random.Next(0, 9).ToString();
            }
            return code;
        }

        public bool sendPasswordRecoveryEmail(string email, string code)
        {
            try
            {
                var fromAddress = new MailAddress("hankhongg@gmail.com", "Quên mật khẩu phần mềm Quản lý nhà hàng");
                var toAddress = new MailAddress(email);
                const string fromPassword = "vuqd heti kiij yzkj";
                string subject = "Mã xác nhận quên mật khẩu";
                string body = "Mã xác nhận của bạn là: " + code;

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
                };
                smtp.Send(new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                });
                return true;
            }
            catch (SmtpException smtpex)
            {
                MessageBox.Show($"Error sending email: {smtpex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
