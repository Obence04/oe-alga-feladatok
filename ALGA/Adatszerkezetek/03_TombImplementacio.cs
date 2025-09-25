using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Adatszerkezetek
{
    // 3. heti labor feladat - Tesztek: 03_TombImplementacioTesztek.cs
    public class TombVerem<T> : Verem<T>
    {
        T[] E;
        int n;
        public bool Ures => n == -1;

        public TombVerem(int meret)
        {
            E = new T[meret];
            n = -1;
        }
        public T Felso()
        {
            return E[n];
        }

        public void Verembe(T ertek)
        {
            if (n >= E.Length-1) throw new NincsHelyKivetel();
            n++;
            E[n] = ertek;
        }

        public T Verembol()
        {
            if (n == -1) throw new NincsElemKivetel();
            T ertek = E[n];
            n--;
            return ertek;
        }
    }

    public class TombSor<T> : Sor<T>
    {
        T[] E;
        int e; //utolsó kiolvasott
        int u; //utolsó beszúrt
        int n; //számláló
        public bool Ures => n == -1;
        public TombSor(int meret)
        {
            E = new T[meret];
            n = -1;
            e = -1;
            u = -1;
        }

        
        public T Elso()
        {
            if (n == -1) throw new NincsElemKivetel();
            return E[(e % E.Length) + 1];
        }

        public void Sorba(T ertek)
        {
            if (n >= E.Length - 1) throw new NincsHelyKivetel();
            n++;
            u = ((u+1) % E.Length);
            E[u] = ertek;
        }

        public T Sorbol()
        {
            if (n < 0) throw new NincsElemKivetel();
            n--;
            e = ((e+1) % E.Length);
            return E[e];
        }
    }
    public class TombLista<T> : Lista<T>, IEnumerable<T>
    {
        T[] E;
        int n;
        public int Elemszam => n+1;
        public TombLista(int meret = 1)
        {
            E = new T[meret];
            n = -1;
        }

        public void Bejar(Action<T> muvelet)
        {
            for (int i = 0; i < n+1; i++)
            {
                muvelet.Invoke(E[i]);
            }
        }

        public void Beszur(int index, T ertek)
        {
            if (index < 0 || index > n+1) throw new HibasIndexKivetel();
            if (n+1 == E.Length) MeretNovel();
            n++;
            for (int i = n; i > index; i--)
            {
                E[i] = E[i - 1];
            }
            E[index] = ertek;
        }

        public void Hozzafuz(T ertek)
        {
            Beszur(n + 1, ertek);
        }

        public T Kiolvas(int index)
        {
            if (index < 0 || index > n) throw new HibasIndexKivetel();
            return E[index];
        }

        public void Modosit(int index, T ertek)
        {
            if (index < 0 || index > n)throw new HibasIndexKivetel();
            E[index] = ertek;
        }

        public void Torol(T ertek)
        {
            int db = 0;
            for (int i = 0; i < n + 1; i++)
            {
                if (E[i]!.Equals(ertek)) db++;
                else E[i - db] = E[i];
            }
            n -= db;
        }
        public void MeretNovel()
        {
            T[] tarolo = E;
            E = new T[tarolo.Length * 2];
            for (int i = 0; i < tarolo.Length; i++)
            {
                E[i] = tarolo[i];
            }
        }

        public IEnumerator<T> GetEnumerator() => BejaroLetrehozas();
        public TombListaBejaro<T> BejaroLetrehozas()
        {
            return new TombListaBejaro<T>(E, n);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class TombListaBejaro<T> : IEnumerator<T>
    {
        T[] E;
        int n;
        int aktualisIndex;
        public T Current => E[aktualisIndex];

        object IEnumerator.Current => Current;

        public TombListaBejaro(T[] E, int n)
        {
            this.E = E;
            this.n = n;
            Reset();
        }

        public bool MoveNext()
        {
            aktualisIndex++;
            return aktualisIndex < E.Length;
        }

        public void Reset()
        {
            aktualisIndex = -1;
        }

        public void Dispose()
        {
        }
    }
}
