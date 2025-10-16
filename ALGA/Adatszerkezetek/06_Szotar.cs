using System;

namespace OE.ALGA.Adatszerkezetek
{
    // 6. heti labor feladat - Tesztek: 06_SzotarTesztek.cs
    public class SzotarElem<K,T>
    {
        public K kulcs;
        public T tart;
        public SzotarElem(K kulcs, T tart)
        {
            this.kulcs = kulcs;
            this.tart = tart;
        }
    }

    public class HasitoSzotarTulcsordulasiTerulettel<K, T> : Szotar<K, T>
    {
        SzotarElem<K, T>[] E; //elemek...bruh
        Func<K, int> h; //a kulcs helye (index) az E tömbben (hasítófüggvény)
        LancoltLista<SzotarElem<K, T>> U = new LancoltLista<SzotarElem<K, T>>(); //túlcsordulási terület
        public HasitoSzotarTulcsordulasiTerulettel(int meret, Func<K,int> h)
        {
            E = new SzotarElem<K, T>[meret];
            //this.h = (x => h(x) % E.Length);
            this.h = (x => Math.Abs(h(x)) % E.Length);
        }
        public HasitoSzotarTulcsordulasiTerulettel(int meret) : this(meret, x => x.GetHashCode())
        {
            
        }

        private SzotarElem<K,T> KulcsKeres(K kulcs)
        {
            if (E[h(kulcs)] != null && E[h(kulcs)].kulcs.Equals(kulcs)) return E[h(kulcs)];
            SzotarElem<K, T>? e = null;
            U.Bejar(x => { if (x.kulcs.Equals(kulcs)) e = x; });
            return e;
        }
        public void Beir(K kulcs, T ertek)
        {
            SzotarElem<K, T> meglevo = KulcsKeres(kulcs);
            if (meglevo != null) meglevo.tart = ertek;
            else
            {
                SzotarElem<K, T> uj = new SzotarElem<K, T>(kulcs, ertek);
                if (E[h(kulcs)] == null) E[h(kulcs)] = uj;
                else U.Hozzafuz(uj);
            }
        }

        public T Kiolvas(K kulcs)
        {
            SzotarElem<K, T> meglevo = KulcsKeres(kulcs);
            if (meglevo == null) throw new HibasKulcsKivetel();
            return meglevo.tart;
        }

        public void Torol(K kulcs)
        {
            if (E[h(kulcs)] != null && E[h(kulcs)].kulcs.Equals(kulcs)) E[h(kulcs)] = null;
            else
            {
                SzotarElem<K, T> e = null;
                U.Bejar(x => { if (x.kulcs.Equals(kulcs)) e = x; });
                if (e != null) U.Torol(e);
                else throw new HibasKulcsKivetel();
            }
        }
    }
}
