using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Model;

namespace Core
{
    public interface IMeasurementResultDisplayer
    {
        void Display(MeasurementResult message);
    }

    public interface IStorage
    {
        void Save<T>(T data);
    }
}
