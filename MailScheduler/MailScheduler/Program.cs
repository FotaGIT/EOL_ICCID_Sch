using System;
using MailScheduler.BAL;

namespace MailScheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start application excutions");
            ICCIDMailBAL iCCID = new ICCIDMailBAL();
            Console.WriteLine("Calling geticcid()");
            iCCID.getIccid();
            Console.WriteLine("Excution Completed!");
           
        }
    }
}
