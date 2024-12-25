using System.Net.Mail;
using System.Net;

namespace pro_exam.email_service
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("y.alshyoukh@gmail.com", "esctfdqiepyqrugo"),
                EnableSsl = true,
            };
        }

        public async Task SendVerificationEmailAsync(string email, string verificationCode)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("y.alshyoukh@gmail.com"),
                Subject = "Email Verification",
                Body = $"Your verification code is: {verificationCode}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            await _smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendPasswordAsync(string email, string Password)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("y.alshyoukh@gmail.com"),
                Subject = "Email Verification",
                Body = $"Your Password is: {Password}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            await _smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendForgetpassworodAsync(string email, string verificationCode)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("y.alshyoukh@gmail.com"),
                Subject = "For Get Password",
                Body = $"Your verification code is: {verificationCode}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            await _smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendStatus(string email, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("y.alshyoukh@gmail.com"),
                Subject = "Respond to the joining request",
                Body = body,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            await _smtpClient.SendMailAsync(mailMessage);
        }
        public async Task ThanksService(string email)
        {
            var imageUrl = "https://localhost:5000/file/Image/hello.png";
            var appDescription = @"
        <h1>Welcome to Laser!</h1>
        <p>Laser is an innovative app that offers a full range of beauty and health services in addition to beauty products, designed to be your first destination for taking care of your beauty and health with ease and comfort.</p>
        <p>The app aims to provide a unique and comfortable experience for users, allowing them to access the best services and products anytime, anywhere.</p>
        <h2>Key Features of the Laser App:</h2>
        <h3>Beauty Services:</h3>
        <ul>
            <li><b>Booking appointments:</b> Users can book appointments for beauty services such as haircuts, hair dyes, facial massages, nail care, and more. The app allows you to choose your preferred salon or expert, with the ability to view other users’ reviews and experiences.</li>
            <li><b>Virtual consultations:</b> The app provides virtual consultations with beauty experts and specialists, where you can get personalized advice on skin and hair care, in addition to recommendations on products suitable for your skin or hair type.</li>
        </ul>
        <h3>Health Services:</h3>
        <ul>
            <li><b>Health consultations:</b> The app allows you to book appointments with doctors and public health specialists, in addition to virtual consultations that help you monitor your health condition and get professional advice.</li>
            <li><b>Personal care sessions:</b> The app provides services such as therapeutic massage, yoga, and physiotherapy, helping you relax and take care of your body.</li>
        </ul>
        <h3>Beauty Products:</h3>
        <ul>
            <li><b>Shopping Beauty Products:</b> The app has a comprehensive store for beauty products, where you can buy skincare, cosmetics, and hair care products from well-known and trusted brands.</li>
            <li><b>Personalized Recommendations:</b> Based on your profile and preferences, the app provides personalized recommendations for products that suit your specific needs, helping you make better purchasing decisions.</li>
        </ul>
        <h3>Additional Features:</h3>
        <ul>
            <li><b>Ratings and Reviews:</b> Users can view other people’s ratings and reviews for services and products, helping them make informed decisions.</li>
            <li><b>Offers and Discounts:</b> The app provides exclusive offers and discounts to users on services and products, making the experience more economical.</li>
            <li><b>Easy User Interface:</b> The app features an intuitive and easy-to-use user interface, ensuring a smooth and convenient experience for users of all ages.</li>
            <li><b>Notifications and Alerts:</b> The app sends notifications and alerts to users about their upcoming appointments, special offers, and new product updates.</li>
        </ul>
        <h3>Conclusion:</h3>
        <p>Laser is a comprehensive and integrated app that aims to improve your life by providing the best beauty and health services, in addition to a variety of beauty products. Whether you are looking for a relaxation session, a health consultation, or a new beauty product, Laser is the perfect solution for you.</p>
    ";

            var mailMessage = new MailMessage
            {
                From = new MailAddress("y.alshyoukh@gmail.com"),
                Subject = "Thanks for joining our family",
                Body = $"<p>Thanks for joining our family!</p><img src='{imageUrl}' alt='Hello' />{appDescription}",
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            await _smtpClient.SendMailAsync(mailMessage);
        }
        public async Task CanselService(string email, decimal Amount)
        {
            var appDescription = $"The appointment was successfully cancelled, and an amount was added to the wallet : {Amount} SAR";

            var mailMessage = new MailMessage
            {
                From = new MailAddress("y.alshyoukh@gmail.com"),
                Subject = "Cancel the appointment",
                Body = appDescription,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            await _smtpClient.SendMailAsync(mailMessage);
        }

        public async Task AddAppointment(string email, DateOnly date, TimeOnly time, decimal Amount)
        {
            var appDescription = $"Dear {email},\n\n" +
                                 $"We hope this message finds you well. This is to inform you that your appointment scheduled for {date:MMMM dd, yyyy} at {time:hh:mm tt} has been successfully cancelled. As a gesture of goodwill, we have credited an amount of {Amount} SAR to your wallet.\n\n" +
                                 "Please note that appointments can be cancelled up to four days before the scheduled date.\n\n" +
                                 "Please let us know if there is anything else we can assist you with.\n\n" +
                                 "Thank you for choosing our services.\n\n" +
                                 "Best regards,\n" +
                                 "Laser";

            var mailMessage = new MailMessage
            {
                From = new MailAddress("y.alshyoukh@gmail.com"),
                Subject = "Appointment Cancellation Confirmation",
                Body = appDescription,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            await _smtpClient.SendMailAsync(mailMessage);
        }

    }
}
