using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

namespace OE.ALGA.Adatszerkezetek
{
    // 4. heti labor feladat - Tesztek: 04_LancoltImplementacioTesztek.cs
    public class LancElem<T>
    {
        public T tart;
        public LancElem<T> kov;
        public LancElem(T tart, LancElem<T> kov)
        {
            this.tart = tart;
            this.kov = kov;
        }
    }
    public class LancoltVerem<T> : Verem<T>
    {
        LancElem<T> fej;
        public bool Ures => fej is null;
        public LancoltVerem()
        {
            fej = null;
        }
        public T Felso() => fej is null ? throw new NincsElemKivetel() : fej.tart;

        public void Verembe(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, fej);
            fej = uj;
        }

        public T Verembol()
        {
            if (fej is null) throw new NincsElemKivetel();
            T q = fej.tart;
            fej = fej.kov;
            return q;
        }
    }

    public class LancoltSor<T> : Sor<T>
    {
        LancElem<T> fej;
        LancElem<T> vege;
        public bool Ures => fej is null;

        public LancoltSor()
        {
            fej = null;
            vege = null;
        }
        public T Elso() => fej is null ? throw new NincsElemKivetel() : fej.tart;

        public void Sorba(T ertek)
        {
            LancElem<T> uj = new LancElem<T> (ertek, null);
            if (vege is null)
            {
                fej = uj;
            }
            else
            {
                vege.kov = uj;
            }
            vege = uj;
        }

        public T Sorbol()
        {
            if (fej is null) throw new NincsElemKivetel();
            else
            {
                T ertek = fej.tart;
                LancElem<T> q = fej;
                fej = fej.kov;
                if (fej is null) vege = null;
                return ertek;
            }
        }
    }

    public class LancoltLista<T> : Lista<T>, IEnumerable<T>
    {
        LancElem<T> fej;
        public int Elemszam
        {
            get
            {
                int darabszam = 0;
                LancElem<T> p = fej;
                while(p is not null)
                {
                    p = p.kov;
                    darabszam++;
                }
                return darabszam;
            }
        }
        public LancoltLista()
        {
            fej = null;
        }
        public void Bejar(Action<T> muvelet)
        {
            LancElem<T> p = fej;
            while(p is not null)
            {
                muvelet(p.tart);
                p = p.kov;
            }
        }

        public void Beszur(int index, T ertek)
        {
            if (fej is null || index == 0)
            {
                LancElem<T> uj = new LancElem<T>(ertek, fej);
                fej = uj;
            }
            else
            {
                LancElem<T> p = fej;
                int i = 1;
                while(p.kov is not null && i < index)
                {
                    p = p.kov;
                    i++;
                }
                if (i <= index)
                {
                    LancElem<T> uj = new LancElem<T>(ertek, p.kov);
                    p.kov = uj;
                }
                else throw new HibasIndexKivetel();
            }
        }

        public void Hozzafuz(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, null);
            if (fej is null) fej = uj;
            else
            {
                LancElem<T> p = fej;
                while (p.kov != null)
                {
                    p = p.kov;
                }
                p.kov = uj;
            }
        }

        public T Kiolvas(int index)
        {
            LancElem<T> p = fej;
            int i = 0;
            while(p is not null && i < index)
            {
                p = p.kov;
                i++;
            }
            if (p is null) throw new HibasIndexKivetel();
            return p.tart;
        }

        public void Modosit(int index, T ertek)
        {
            LancElem<T> p = fej;
            int i = 0;
            while(p is not null && i < index)
            {
                p = p.kov;
                i++;
            }
            if (p is null) throw new HibasIndexKivetel();
            p.tart = ertek;
        }

        public void Torol(T ertek)
        {
            LancElem<T> p = fej;
            LancElem<T> e = null;
            do
            {
                while (p is not null && !p.tart.Equals(ertek))
                {
                    e = p;
                    p = p.kov;
                } 
                if (p is not null)
                {
                    LancElem<T> q = p.kov;
                    if (e is null) fej = q;
                    else e.kov = q;
                    p = q;
                }
            } while (p is not null);
        }

        public IEnumerator<T> GetEnumerator() => BejaroLetrehozas();
        public LancoltListaBejaro<T> BejaroLetrehozas()
        {
            return new LancoltListaBejaro<T>(fej);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class LancoltListaBejaro<T> : IEnumerator<T>
    {
        LancElem<T> fej;
        LancElem<T> aktualisElem;
        public T Current => aktualisElem.tart;

        object IEnumerator.Current => Current;

        public LancoltListaBejaro(LancElem<T> fej)
        {
            this.fej = fej;
            aktualisElem = new LancElem<T>(fej.tart, fej);
            //aktualisElem = fej;
        }
        public void Dispose()
        {

        }

        public bool MoveNext()
        {
            if (aktualisElem.kov is null) return false;
            aktualisElem = aktualisElem.kov;
            return true;
        }

        public void Reset()
        {
            aktualisElem = fej;
        }
    }
}