using System;

namespace OE.ALGA.Adatszerkezetek
{
    // 5. heti labor feladat - Tesztek: 05_BinarisKeresoFaTesztek.cs
    public class FaElem<T> where T : IComparable<T>
    {
        public T tart;
        public FaElem<T> bal, jobb;

        public FaElem(T tart, FaElem<T> bal, FaElem<T> jobb)
        {
            this.tart = tart;
            this.bal = bal;
            this.jobb = jobb;
        }
    }

    public class FaHalmaz<T> : Halmaz<T> where T : IComparable<T>
    {
        FaElem<T> gyoker;
        public FaHalmaz()
        {
            
        }
        public void Bejar(Action<T> muvelet)
        {
            ReszfaBejarasPreOrder(gyoker, muvelet);
        }
        protected void ReszfaBejarasPreOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                muvelet(p.tart);
                ReszfaBejarasPreOrder(p.bal, muvelet);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
            }
        }
        protected void ReszfaBejarasInOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasPreOrder(p.bal, muvelet);
                muvelet(p.tart);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
            }
        }
        protected void ReszfaBejarasPostOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasPreOrder(p.bal, muvelet);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
                muvelet(p.tart);
            }
        }

        public void Beszur(T ertek)
        {
            gyoker = ReszFabaBeszur(gyoker, ertek);
        }
        public static FaElem<T> ReszFabaBeszur(FaElem<T> p, T ertek)
        {
            if (p == null)
            {
                FaElem<T> uj = new FaElem<T>(ertek, null, null);
                return uj;
            }
            else
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszFabaBeszur(p.bal, ertek);
                }
                else if (p.tart.CompareTo(ertek) < 0)
                {
                    p.jobb = ReszFabaBeszur(p.jobb, ertek);
                }
                return p;
            }
        }
        public bool Eleme(T ertek)
        {
            return ReszfaEleme(gyoker, ertek);
        }
        protected bool ReszfaEleme(FaElem<T> p, T ertek)
        {
            if (p == null) return false;
            if (p.tart.CompareTo(ertek) > 0) return ReszfaEleme(p.bal, ertek);
            else
            {
                if (p.tart.CompareTo(ertek) < 0) return ReszfaEleme(p.jobb, ertek);
                return true;
            }
        }
        public void Torol(T ertek)
        {
            gyoker = Torol(gyoker, ertek);
        }
        public FaElem<T> Torol(FaElem<T> p, T ertek)
        {
            if (p == null) throw new NincsElemKivetel();
            if (p.tart.CompareTo(ertek) > 0)
            {
                p.bal = Torol(p.bal, ertek);
            }
            else
            {
                if (p.tart.CompareTo(ertek) < 0)
                {
                    p.jobb = Torol(p.jobb, ertek);
                }
                else
                {
                    if (p.bal == null)
                    {
                        FaElem<T> q = p;
                        p = p.jobb;
                    }
                    else
                    {
                        p.bal = KetGyerekTorles(p, p.bal);
                    }
                }
            }
            return p;
        }
        public FaElem<T> KetGyerekTorles(FaElem<T> e, FaElem<T> r)
        {
            if (r.jobb == null)
            {
                r.jobb = KetGyerekTorles(e, r.jobb);
                return r;
            }
            e.tart = r.tart;
            FaElem<T> q = r;
            r = r.bal;
            return r;
        }
    }
}
