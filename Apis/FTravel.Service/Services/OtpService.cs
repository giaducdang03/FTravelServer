using FTravel.Repository.EntityModels;
using FTravel.Repository.Repositories;
using FTravel.Repository.Repositories.Interface;
using FTravel.Service.BusinessModels;
using FTravel.Service.Services.Interface;
using FTravel.Service.Utils;
using FTravel.Service.Utils.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Services
{
    public class OtpService : IOtpService
    {
        private readonly IOtpRepository _otpRepository;
        private readonly IMailService _mailService;

        public OtpService(IOtpRepository otpRepository, IMailService mailService)
        {
            _otpRepository = otpRepository;
            _mailService = mailService;
        }

        public async Task<Otp> CreateOtpAsync(string email, string type)
        {
            // default ExpiryTime otp is 5 minutes
            Otp newOtp = new Otp()
            {
                Email = email,
                OtpCode = NumberUtils.GenerateSixDigitNumber().ToString(),
                ExpiryTime = DateTime.UtcNow.AddHours(7).AddMinutes(5)
            };
            await _otpRepository.AddAsync(newOtp);

            if (type == "confirm")
            {
                bool checkSendMail = await SendOtpAsync(newOtp);
                return checkSendMail ? newOtp : null;
            } 
            else
            {
                bool checkSendMail = await SendOtpResetPasswordAsync(newOtp);
                return checkSendMail ? newOtp : null;
            }

        }

        public async Task<bool> ValidateOtpAsync(string email, string otpCode)
        {
            var otpExist = await _otpRepository.GetOtpByCode(otpCode);
            if (otpExist != null)
            {
                if (otpExist.Email == email && otpExist.ExpiryTime > DateTime.UtcNow.AddHours(7)
                    && otpExist.IsUsed == false)
                {
                    otpExist.IsUsed = true;
                    await _otpRepository.UpdateAsync(otpExist);
                    return true;
                }
            }
            return false;
        }

        private async Task<bool> SendOtpAsync(Otp otp)
        {
            // create new email
            MailRequest newEmail = new MailRequest()
            {
                ToEmail = otp.Email,
                Subject = "FTravel Confirmation Email",
                Body = SendOTPTemplate.EmailSendOTP(otp.Email, otp.OtpCode)
            };

            // send mail
            await _mailService.SendEmailAsync(newEmail);
            return true;
        }

        private async Task<bool> SendOtpResetPasswordAsync(Otp otp)
        {
            // create new email
            MailRequest newEmail = new MailRequest()
            {
                ToEmail = otp.Email,
                Subject = "FTravel Reset password",
                Body = SendOTPTemplate.EmailSendOTPResetPassword(otp.Email, otp.OtpCode)
            };

            // send mail
            await _mailService.SendEmailAsync(newEmail);
            return true;
        }
    }
}
