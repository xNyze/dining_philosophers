using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//--created by Paul Zänker

namespace Dining_Philosophers
{
    class Program
    {
        const int philosopherCount = 5;

        public Program()
        { }

        public static void eatMeal(int philosopher, object leftFork, object rightFork, int leftForkNumber, int rightForkNumber)
        {
            lock (leftFork) //nehme zuerst linke Gabel
            {
                Console.WriteLine("Philosoph {0} greift nach linker Gabel Nummer {1}", philosopher, leftForkNumber);

                lock (rightFork) //nehme danach rechte Gabel
                {
                    Console.WriteLine("Philosoph {0} greift nach rechter Gabel Nummer {1}", philosopher, rightForkNumber);
                    Console.WriteLine("Philosoph {0} isst", philosopher); //Philosoph isst
                }
                Console.WriteLine("Philosoph {0} legt Gabel Nummer {1} zurück", philosopher, rightForkNumber); //rechte Gabel zurücklegen
            }
            Console.WriteLine("Philosoph {0} legt Gabel Nummer {1} zurück\n", philosopher, leftForkNumber); //linke Gabel zurücklegen
        }

        static void Main(string[] args)
        {
            Task[] t = new Task[philosopherCount]; //5 Threads
            object[] fork = new object[philosopherCount]; //5 Gabeln

            for (int i = 0; i < philosopherCount; i++) //Erzeugt neue Gabel
            { fork[i] = new object(); }


            for (int i = 1; i <= philosopherCount; i++)
            {
                int k = i; //wird benötigt da i sonst bei task.Start() schon den Wert 5 annimmt
                t[i-1] = new Task(() => {
                    eatMeal(k + 1, fork[k - 1], fork[k%philosopherCount], k, k + 1);
                });
            }

            Parallel.ForEach(t, task => //task in t
            {
                task.Start();
            });

            Task.WaitAll(t);
        }
    }
}