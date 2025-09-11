using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Paradigmak
{
    public interface IVegrehajthato
    {
        void Vegrehajtas();
    }
    public class FeladatTarolo<T> : IEnumerable<T> where T : IVegrehajthato //gatya
    {
        protected T[] tarolo;
        int n;
        public FeladatTarolo(int meret)
        {
            tarolo = new T[meret];
            n = 0;
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
            foreach (T item in tarolo)
            {
                item.Vegrehajtas();
            }
        }
        public IEnumerable<T> BejaroLetrehozas()
        {
            FeladatTaroloBejaro<T> ftb = new FeladatTaroloBejaro<T>(tarolo, n);
            return ftb as IEnumerable<T>;
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
            foreach (T item in tarolo)
            {
                if (item.FuggosegTeljesul)
                {
                    item.Vegrehajtas();
                }
            }
        }
    }

    public interface IEnumerable<T>
    {
        public IEnumerator<T> BejaroLetrehozas();
    }

    public interface IEnumerator<T>
    {
        public T Aktualis { get; }
        protected void Alaphelyzet();
        protected bool Kovetkezo();

    }

    public class FeladatTaroloBejaro<T> : IEnumerator<T>
    {
        T[] tarolo;
        int n;
        int aktualisIndex;
        public T Aktualis { get; private set; }
        public FeladatTaroloBejaro(T[] tarolo, int n)
        {
            this.tarolo = tarolo;
            this.n = n;
            Alaphelyzet();
        }
        public void Alaphelyzet()
        {
            aktualisIndex = 0;
            Aktualis = tarolo[aktualisIndex];
        }
        public bool Kovetkezo()
        {
            if (aktualisIndex < n)
            {
                aktualisIndex++;
                Aktualis = tarolo[aktualisIndex];
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
