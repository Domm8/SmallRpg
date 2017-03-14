using System;

namespace SmallRPGLibrary.Services
{
    public static class UnitAction
    {
         public static bool Random 
         {
             get { return new Random().Next(0, 10) > 4; }
         }
    }
}