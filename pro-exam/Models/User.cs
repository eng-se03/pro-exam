using System.ComponentModel.DataAnnotations;

namespace pro_exam.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
         // إضافة خصائص جديدة
        public string Email { get; set; } // البريد الإلكتروني
                                          // OTP يتكون فقط من أرقام
        [RegularExpression(@"^\d+$", ErrorMessage = "OTP must contain only numbers.")]
        public string OTP { get; set; } // رمز التحقق (One-Time Password)

        public DateTime OTPExpirationTime { get; set; } // وقت انتهاء صلاحية OTP
    }
}
