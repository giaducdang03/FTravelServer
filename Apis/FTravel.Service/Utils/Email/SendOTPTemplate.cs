using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Utils.Email
{
    public class SendOTPTemplate
    {
        public static string EmailSendOTP(string email, string otpCode)
        {
            #region style

            string style =

            @"<style>
                body {
                  font-family: Arial, sans-serif;
                  margin: 0;
                  padding: 0;
                }
                .container {
                  width: 550px;
                  margin: 0 auto;
                  padding: 20px;
                }
                .header {
                  display: flex;
                  align-items: center;
                  background-color: #f7f7f7;
                  padding: 10px;
                }
                .header img {
                  object-fit: cover;
                  padding-top: 10px;
                  width: 100px;
                }
                .otp {
                  border: #0098af solid 2px;
                  padding: 8px;
                  text-align: center;
                  margin: auto 35%;
                }
                .otp span {
                  color: #1cbcd4;
                  font-weight: bold;
                  font-size: 22px;
                  letter-spacing: 3px;
                }
                .footer {
                  background-color: #f7f7f7;
                  padding: 10px;
                }
                .footer img {
                  width: 100px;
                }
                .info p {
                  font-size: 13px;
                }
                .info p:last-child {
                  margin-bottom: 0;
                }
            </style>";
            #endregion style

            #region body

            string body =
                $@"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Confirm Email</title>
                    {style}
                </head>
                <body>
                    <div class=""container"">
                      <div class=""header"">
                        <img
                          src=""https://firebasestorage.googleapis.com/v0/b/swd392-d2c4e.appspot.com/o/FTravel%2FLogo_FTravel.png?alt=media&token=71c03bb9-fbc4-455a-a295-c226d0945a9f""
                          alt
                        />
                        <div style=""border-left: 2px solid #1cbcd4; margin-left: 20px"">
                          <h4 style=""margin-left: 20px"">Xác nhận Email</h4>
                        </div>
                      </div>
                      <div class=""content"">
                        <div>
                          <h3>Xác thực tài khoản FTravel của bạn</h3>
                          <p style=""font-size: 15px"">Bạn đã đăng ký {email} tại FTravel.</p>
                          <p style=""font-size: 15px"">Đây là mã xác thực OTP của bạn</p>
                        </div>
                        <div class=""otp"">
                          <span>{otpCode}</span>
                        </div>
                        <div class=""note"">
                          <p style=""font-size: 15px; font-style: italic; color: red"">
                            * Lưu ý: Tài khoản chỉ có thể đăng nhập được khi đã xác thực.
                          </p>
                        </div>
                      </div>
                      <div class=""footer"">
                        <img
                          src=""https://firebasestorage.googleapis.com/v0/b/swd392-d2c4e.appspot.com/o/FTravel%2FLogo_FTravel.png?alt=media&token=71c03bb9-fbc4-455a-a295-c226d0945a9f""
                          alt=""logo""
                          width=""80px""
                        />
                        <div class=""info"">
                          <p>Điện thoại: +84 988 889 898</p>
                          <p>Email: ftravel.service@gmail.com</p>
                          <p>Địa chỉ: Đại học FPT thành phố Hồ Chí Minh</p>
                        </div>
                      </div>
                    </div>
                </body>
                </html>
                ";
            #endregion body

            return body;
        }

        public static string EmailSendOTPResetPassword(string email, string otpCode)
        {
            #region style

            string style =

            @"<style>
                body {
                  font-family: Arial, sans-serif;
                  margin: 0;
                  padding: 0;
                }
                .container {
                  width: 550px;
                  margin: 0 auto;
                  padding: 20px;
                }
                .header {
                  display: flex;
                  align-items: center;
                  background-color: #f7f7f7;
                  padding: 10px;
                }
                .header img {
                  object-fit: cover;
                  padding-top: 10px;
                  width: 100px;
                }
                .otp {
                  border: #0098af solid 2px;
                  padding: 8px;
                  text-align: center;
                  margin: auto 35%;
                }
                .otp span {
                  color: #1cbcd4;
                  font-weight: bold;
                  font-size: 22px;
                  letter-spacing: 3px;
                }
                .footer {
                  background-color: #f7f7f7;
                  padding: 10px;
                }
                .footer img {
                  width: 100px;
                }
                .info p {
                  font-size: 13px;
                }
                .info p:last-child {
                  margin-bottom: 0;
                }
            </style>";
            #endregion style

            #region body

            string body =
                $@"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Confirm Email</title>
                    {style}
                </head>
                <body>
                    <div class=""container"">
                      <div class=""header"">
                        <img
                          src=""https://firebasestorage.googleapis.com/v0/b/swd392-d2c4e.appspot.com/o/FTravel%2FLogo_FTravel.png?alt=media&token=71c03bb9-fbc4-455a-a295-c226d0945a9f""
                          alt
                        />
                        <div style=""border-left: 2px solid #1cbcd4; margin-left: 20px"">
                          <h4 style=""margin-left: 20px"">Đặt lại mật khẩu</h4>
                        </div>
                      </div>
                      <div class=""content"">
                        <div>
                          <h3>Đặt lại mật khẩu FTravel</h3>
                          <p style=""font-size: 15px"">Bạn vừa thực hiện yêu cầu đặt lại mật khẩu cho tài khoản {email} tại FTravel.</p>
                          <p style=""font-size: 15px"">Đây là mã xác thực OTP của bạn</p>
                        </div>
                        <div class=""otp"">
                          <span>{otpCode}</span>
                        </div>
                        <div class=""note"">
                          <p style=""font-size: 15px; font-style: italic; color: red"">
                            * Lưu ý: Nếu bạn không thực hiện yêu cầu này vui lòng bỏ qua.
                          </p>
                        </div>
                      </div>
                      <div class=""footer"">
                        <img
                          src=""https://firebasestorage.googleapis.com/v0/b/swd392-d2c4e.appspot.com/o/FTravel%2FLogo_FTravel.png?alt=media&token=71c03bb9-fbc4-455a-a295-c226d0945a9f""
                          alt=""logo""
                          width=""80px""
                        />
                        <div class=""info"">
                          <p>Điện thoại: +84 988 889 898</p>
                          <p>Email: ftravel.service@gmail.com</p>
                          <p>Địa chỉ: Đại học FPT thành phố Hồ Chí Minh</p>
                        </div>
                      </div>
                    </div>
                </body>
                </html>
                ";
            #endregion body

            return body;
        }
    }
}
