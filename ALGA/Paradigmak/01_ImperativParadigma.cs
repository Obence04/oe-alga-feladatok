using System;
using System.Collections;
using System.Collections.Generic;

namespace OE.ALGA.Paradigmak
{
    public interface IVegrehajthato
    {
        void Vegrehajtas();
    }
    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajthato
    {
        protected T[] tarolo;
        protected int n;
        public FeladatTarolo(int meret)
        {
            tarolo = new T[meret];
            n = 0;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return BejaroLetrehozas();
        }
        public void Felvesz(T elem)
        {
            if (n < tarolo.Length)
            {
                tarolo[n] = elem;
                n++;
            }
            else throw new TaroloMegteltKivetel();
        }
        public void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                tarolo[i].Vegrehajtas();
            }
        }

        public IEnumerator<T> BejaroLetrehozas()
        {
            FeladatTaroloBejaro<T> ftb = new FeladatTaroloBejaro<T>(tarolo, n);
            return ftb;
        }

    }

    public interface IFuggo
    {
        public bool FuggosegTeljesul { get; }
    }

    public class FuggoFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato, IFuggo
    {

        public FuggoFeladatTarolo(int meret) : base(meret)
        {

        }
        public void MindentVegrehajt()
        {
            for (int i = 0; i < n; i++)
            {
                if (tarolo[i].FuggosegTeljesul)
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }
    }

    public interface IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator();
    }

    public interface IEnumerator<T>
    {
        public T Current { get; }
        public void Alaphelyzet();
        public bool MoveNext();

    }

    public class FeladatTaroloBejaro<T> : IEnumerator<T>
    {
        T[] tarolo;
        int n;
        int aktualisIndex;
        public T Current { get { return tarolo[aktualisIndex]; } }
        public FeladatTaroloBejaro(T[] tarolo, int n)
        {
            this.tarolo = tarolo;
            this.n = n;
            Alaphelyzet();
        }
        public void Alaphelyzet()
        {
            aktualisIndex = -1;
        }
        public bool MoveNext()
        {
            aktualisIndex++;
            if (aktualisIndex < n)
            {
                return true;
            }
            return false;
        }
    }

    [Serializable]
    public class TaroloMegteltKivetel : Exception
    {
        public TaroloMegteltKivetel()
        {
        }

        public TaroloMegteltKivetel(string? message) : base(message)
        {
        }

        public TaroloMegteltKivetel(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}

