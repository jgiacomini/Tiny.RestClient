namespace Tiny.Http
{
    public interface ICommonResquest
    {
        IRequest AddHeader(string key, string value);
        IRequest AddQueryParameter(string key, string value);
        IRequest AddQueryParameter(string key, int value);
        IRequest AddQueryParameter(string key, uint value);
        IRequest AddQueryParameter(string key, double value);
        IRequest AddQueryParameter(string key, decimal value);
    }
}