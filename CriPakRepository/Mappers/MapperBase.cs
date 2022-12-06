using CriPakInterfaces;
using CriPakInterfaces.Models.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace CriPakRepository.Mappers
{
    public abstract class MapperBase<T> : IMapper<T>
    {
        public T Map(IEnumerable<Row> rowValue)
        {
            throw new NotImplementedException();
        }
    }
}
