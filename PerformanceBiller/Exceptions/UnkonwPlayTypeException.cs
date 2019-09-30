using PerformanceBiller.Model;
using System;

namespace PerformanceBiller.Exceptions
{
    public class UnkonwPlayTypeException: Exception
    {
        public UnkonwPlayTypeException(Play play) : base($"unknown type: { play.Type}") { }
    }
}
