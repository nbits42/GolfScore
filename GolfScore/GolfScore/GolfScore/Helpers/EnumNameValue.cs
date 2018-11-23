using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper.Mappers;
using TeeScore.Services;

namespace TeeScore.Helpers
{
    public class EnumNameValue<T> 
    {
        public EnumNameValue(T value)
        {
            var key = $"{typeof(T).Name}-{value}";
            Name = TranslationService.Translate(key);
            Value = value;
        }

        public T Value { get; set; }
        public string Name { get; set; }
    }
}
