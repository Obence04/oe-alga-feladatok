using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Paradigmak
{
    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T>, IEnumerable<T> where T : IVegrehajthato
    {
        public Func<T, bool> BejaroFeltetel { get; set; }
        public FeltetelesFeladatTarolo(int meret) : base(meret)
        {
        }
        public void FeltetelesVegrehajtas(Func<T,bool> feltetel)
        {
            for (int i = 0; i < n; i++)
            {
                if (feltetel(tarolo[i]))
                {
                    tarolo[i].Vegrehajtas();
                }
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return BejaroLetrehozas();
        }
        public FeltetelesFeladatTaroloBejaro<T> BejaroLetrehozas()
        {
            if (BejaroFeltetel is null)
            {
                FeltetelesFeladatTaroloBejaro<T> fftb1 = new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, x => true);
                return fftb1;
            }
            FeltetelesFeladatTaroloBejaro<T> fftb2 = new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, BejaroFeltetel);
            return fftb2;
        }
    }

    public class FeltetelesFeladatTaroloBejaro<T> : IEnumerator<T>
    {
        T[] tarolo;
        int n;
        int aktualisIndex;
        Func<T, bool> feltetel;
        public T Current { get { return tarolo[aktualisIndex]; } }
        public FeltetelesFeladatTaroloBejaro(T[] tarolo, int n, Func<T, bool> feltetel)
        {
            this.tarolo = tarolo;
            this.n = n;
            Alaphelyzet();
            this.feltetel = feltetel;
        }
        public void Alaphelyzet()
        {
            aktualisIndex = -1;
        }

        public bool MoveNext()
        {
            do
            {
                aktualisIndex++;
            }
            while (aktualisIndex < n && !feltetel(tarolo[aktualisIndex]));

            if (aktualisIndex < n)
            {
                return true;
            }
            return false;
        }
    }

}
