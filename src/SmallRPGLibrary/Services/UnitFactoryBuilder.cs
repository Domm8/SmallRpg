using System.Diagnostics;
using SmallRPGLibrary.Services.Interfaces;

namespace SmallRPGLibrary.Services
{
    public class UnitFactoryBuilder
    {
        private static IUnitFactory _unitFactory;

         public static IUnitFactory Create()
         {
             if (_unitFactory == null)
             {
                 _unitFactory = new UnitFactory();
             }
             return _unitFactory;
         }

        [Conditional("DEBUG")]
        public static void SetUnitFactory(IUnitFactory unitFactory)
        {
            _unitFactory = unitFactory;
        }
    }
}