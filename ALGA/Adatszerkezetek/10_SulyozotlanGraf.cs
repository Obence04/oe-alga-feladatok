using System;

namespace OE.ALGA.Adatszerkezetek
{
    // 10. heti labor feladat - Tesztek: 10_SulyozatlanGrafTesztek.cs
    public class EgeszGrafEl : GrafEl<int>, IComparable<EgeszGrafEl>
    {
        public int Honnan { get; private set; }

        public int Hova { get; private set; }

        public EgeszGrafEl(int Honnan, int Hova)
        {
            this.Honnan = Honnan;
            this.Hova = Hova;
        }

        public int CompareTo(EgeszGrafEl? grafEl)
        {
            if (Honnan != grafEl.Honnan) return Honnan - grafEl.Honnan;
            else if (Honnan == grafEl.Honnan && Hova != grafEl.Hova) return Hova - grafEl.Hova;
            return 0;
        }
    }

    public class CsucsmatrixSulyozatlanEgeszGraf : SulyozatlanGraf<int, EgeszGrafEl>
    {
        int n; //csúcsok száma
        bool[,] M; //n*n csúcsmátrix
        public int CsucsokSzama => n;

        public int ElekSzama
        {
            get
            {
                int db = 0;
                for (int i = 0; i < M.GetLength(0); i++)
                {
                    for (int j = 0; j < M.GetLength(1); j++)
                    {
                        db += M[i, j] ? 1 : 0;
                    }
                }
                return db;
            }
        }

        public Halmaz<int> Csucsok
        {
            get
            {
                FaHalmaz<int> csucsok = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                {
                    csucsok.Beszur(i);
                }
                return csucsok;
            }
        }

        public Halmaz<EgeszGrafEl> Elek
        {
            get
            {
                Halmaz<EgeszGrafEl> elek = new FaHalmaz<EgeszGrafEl>();
                for (int i = 0; i < M.GetLength(0); i++)
                {
                    for (int j = 0; j < M.GetLength(1); j++)
                    {
                        if (M[i, j]) elek.Beszur(new EgeszGrafEl(i, j));
                    }
                }
                return elek;
            }
        }

        public CsucsmatrixSulyozatlanEgeszGraf(int n)
        {
            this.n = n;
            M = new bool[n, n];
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> szomszedok = new FaHalmaz<int>();
            for (int i = 0; i < M.GetLength(0); i++) if (M[csucs, i]) szomszedok.Beszur(i);
            return szomszedok;
        }

        public void UjEl(int honnan, int hova)
        {
            //validáció?
            M[honnan, hova] = true;
        }

        public bool VezetEl(int honnan, int hova) => M[honnan, hova];
    }

    public class GrafBejarasok
    {
        public static Halmaz<V> SzelessegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable<V>
        {
            Sor<V> S = new LancoltSor<V>();
            S.Sorba(start);
            Halmaz<V> F = new FaHalmaz<V>();
            F.Beszur(start);

            while (!S.Ures)
            {
                V k = S.Sorbol();
                muvelet.Invoke(k);
                g.Szomszedai(k).Bejar(x =>
                {
                    if (!F.Eleme(x))
                    {
                        S.Sorba(x);
                        F.Beszur(x);
                    }
                });
            }
            return F;
        }
        public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable<V>
        {
            Halmaz<V> F = new FaHalmaz<V>();
            MelysegiBejarasRekurzio<V, E>(g, start, ref F, muvelet);
            return F;
        }
        public static void MelysegiBejarasRekurzio<V, E>(Graf<V, E> g, V k, ref Halmaz<V> F, Action<V> muvelet)
        {
            F.Beszur(k);
            muvelet.Invoke(k);
            Halmaz<V> G = F;
            g.Szomszedai(k).Bejar(x =>
            {
                if (!G.Eleme(x)) MelysegiBejarasRekurzio<V, E>(g, x, ref G, muvelet);
            });
        }
    }
}