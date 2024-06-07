using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Utils.Email
{
    public class EmailCreateAccount
    {
        public static string EmailSendCreateAccount(string email, string password)
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
                  padding: 3px;
                }
                .info {
                  padding: 5px;
                }
                .info p {
                  font-size: 13px;
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
                    <title>Create Account Email</title>
                    {style}
                </head>
                <body>
                    <div class=""container"">
                      <div class=""header"">
                        <div style=""padding: 15px; padding-bottom: 7px"">
                          <img
                            src=""https://firebasestorage.googleapis.com/v0/b/swd392-d2c4e.appspot.com/o/FTravel%2FLogo_FTravel.png?alt=media&token=7cdfe76e-4911-43f9-8d0c-e5d037df3289""
                            alt
                            width=""100px""
                          />
                        </div>
                      </div>
                      <div class=""content"" style=""padding: 20px"">
                        <div>
                          <h3 style=""color: #1cbcd4; font-size: xx-large"">
                            Tạo tài khoản thành công
                          </h3>
                          <p style=""font-size: 18px; color: gray"">
                            Chúc mừng, bạn đã là thành viên của FTravel.
                          </p>
                        </div>
                        <div
                          style=""
                            display: flex;
                            justify-content: center;
                            flex-direction: column;
                            align-items: center;
                            padding: 20px;
                          ""
                        >
                          <div style=""line-height: 30px"">
                            <div style=""font-weight: 600"">
                              <span>Tài khoản:</span>
                              <span>{email}</span>
                            </div>
                            <div style=""font-weight: 600"">
                              <span>Mật khẩu:</span><span>{password}</span>
                            </div>
                          </div>
                        </div>

                        <div class=""note"">
                          <p style=""font-size: 15px; font-style: italic; color: red"">
                            * Lưu ý: Không cung cấp thông tin tài khoản cho người khác để tránh
                            những thiệt hại không đáng có
                          </p>
                        </div>
                      </div>
                      <div class=""footer"">
                        <img
                          src=""https://firebasestorage.googleapis.com/v0/b/swd392-d2c4e.appspot.com/o/FTravel%2FLogo_FTravel.png?alt=media&token=7cdfe76e-4911-43f9-8d0c-e5d037df3289""
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
