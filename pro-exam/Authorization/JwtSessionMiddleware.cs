namespace pro_exam.Authorization;
public class JwtSessionMiddleware
{
    private readonly RequestDelegate _next;

    public JwtSessionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Session.GetString("token");
        if (!string.IsNullOrEmpty(token))
        {
            context.Request.Headers.Add("Authorization", "Bearer " + token);
        }
        await _next(context);
    }
}

