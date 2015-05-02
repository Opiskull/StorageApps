namespace Storage.Common.Interfaces
{
    public interface IConverter<in TFrom, out TTo>
    {
        TTo Convert(TFrom fromtType);
    }
}