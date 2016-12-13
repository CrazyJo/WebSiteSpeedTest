using System;
using Core.Model;

namespace UtilitiesPackage
{
    public interface IFormater<in TInput, out TOutput> : IDisposable
    {
        TOutput Format(TInput value);
    }

    public interface IDisplayer
    {
        void Display(MeasurementResult value);
    }
}
