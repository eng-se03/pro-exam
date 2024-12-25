namespace pro_exam.Authorization
{
    public interface IAuthentication<T>
    {
        string GetJsonWebToken(T entity);
    }
}

