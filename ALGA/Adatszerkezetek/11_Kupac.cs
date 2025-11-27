using System;
using System.ComponentModel.DataAnnotations;

namespace OE.ALGA.Adatszerkezetek
{
    // 11. heti labor feladat - Tesztek: 11_KupacTesztek.cs
    public class Kupac<T>
    {
        protected T[] E; //a kupac elemei
        protected int n; //elemek száma
        protected Func<T, T, bool> nagyobbPrioritas;
        public Kupac(T[] E, int n, Func<T, T, bool> nagyobbPrioritas)
        {
            this.E = E;
            this.n = n;
            this.nagyobbPrioritas = nagyobbPrioritas;
            KupacotEpit();
        }
        public static int Bal(int i) => 2 * i;
        public static int Jobb(int i) => 2 * i + 1;
        public static int Szulo(int i) => (i + 1) / 2 - 1;
        protected void Kupacol(int i)
        {
            int max = 0;
            int b = Bal(i + 1) - 1;
            int j = Jobb(i + 1) - 1;
            if (b < n && nagyobbPrioritas.Invoke(E[b], E[i])) max = b;
            else max = i;
            if (j < n && nagyobbPrioritas.Invoke(E[j], E[max])) max = j;
            if(max != i)
            {
                T csere = E[i];
                E[i] = E[max];
                E[max] = csere;
                Kupacol(max);
            }
        }
        protected void KupacotEpit()
        {
            for (int i = n/2; i > -1; i--)
            {
                Kupacol(i);
            }
        }
    }

    public class KupacRendezes<T> : Kupac<T> where T : IComparable
    {
        public KupacRendezes(T[] E) : base(E, E.Length, (x,y) => { return x.CompareTo(y) > 0; })
        {
            
        }
        public void Rendezes()
        {
            KupacotEpit();
            for (int i = n-1; i > -1; i--)
            {
                T csere = E[0];
                E[0] = E[i];
                E[i] = csere;
                n--;
                Kupacol(0);
            }
        }
    }

    public class KupacPrioritasosSor<T> : Kupac<T>, PrioritasosSor<T>
    {
        public bool Ures => n == 0;

        public KupacPrioritasosSor(int meret, Func<T, T, bool> nagyobbPrioritas) : base(new T[meret], 0, nagyobbPrioritas)
        {
        }
        private void KulcsotFelvisz(int i)
        {
            int sz = Szulo(i);
            if (sz >= 0 && nagyobbPrioritas.Invoke(E[i], E[sz]))
            {
                T csere = E[sz];
                E[sz] = E[i];
                E[i] = csere;
                KulcsotFelvisz(sz);
            }
        }
        public T Elso()
        {
            if (Ures) throw new NincsElemKivetel();
            return E[0];
        }

        public void Frissit(T elem)
        {
            int i = 0;
            while (i < n && !E[i].Equals(elem)) i++;
            if (i == n) throw new NincsElemKivetel();
            KulcsotFelvisz(i);
            Kupacol(i);

        }

        public void Sorba(T ertek)
        {
            if (n >= E.Length) throw new NincsHelyKivetel();
            E[n] = ertek;
            KulcsotFelvisz(n);
            n++;
        }

        public T Sorbol()
        {
            if (Ures) throw new NincsElemKivetel();
            T ertek = E[0];
            E[0] = E[n-1];
            Kupacol(0);
            n--;
            return ertek;
        }
    }
}