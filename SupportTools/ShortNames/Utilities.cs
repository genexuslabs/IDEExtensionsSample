using System.Collections.Generic;

namespace Artech.Packages.SupportTools.ShortNames
{
    class Utilities
    {
        // Tomado de http://stackoverflow.com/questions/5654885/casting-ienumerablederived-to-ienumerablebaseclass
        // Nota: en .NET 4 ya no sería necesario.
        public static IEnumerable<TBase> Cast<TDerived, TBase>(IEnumerable<TDerived> source)
            where TDerived : TBase
        {
            foreach (TDerived item in source)
            {
                yield return item;
            }
        }
    }
}
