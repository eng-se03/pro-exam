using Nancy.Json;
namespace pro_exam.Authorization
{
    public class SecuredHelper
    {
        public static JwtAuthResponse GetInfoFromToken(string Token)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string[] source = Token.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            var data =  source[1] +source[2];
            JwtAuthResponse authResponse = serializer.Deserialize<JwtAuthResponse>(data);
            return authResponse;
        }
    }
}

