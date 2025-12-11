using System;
using System.Linq;

namespace OE.ALGA.Adatszerkezetek
{
    // 12. heti labor feladat - Tesztek: 12_SulyozottGrafTesztek.cs
    public class SulyozottEgeszGrafEl : EgeszGrafEl, SulyozottGrafEl<int>
    {
        public float Suly { get; }
        public SulyozottEgeszGrafEl(int Honnan, int Hova, float Suly) : base(Honnan, Hova)
        {
            this.Suly = Suly;
        }
    }
    public class CsucsmatrixSulyozottEgeszGraf : SulyozottGraf<int, SulyozottEgeszGrafEl>
    {
        int n; //csúcsok száma
        float[,] M; //n*n-es csúcsmátrix

        public CsucsmatrixSulyozottEgeszGraf(int n)
        {
            this.n = n;
            M = new float[n, n];
        }

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
                        db += M[i, j] is 0f ? 0 : 1;
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
                for (int i = 0; i < n; i++) csucsok.Beszur(i);
                return csucsok;
            }
        }

        public Halmaz<SulyozottEgeszGrafEl> Elek
        {
            get
            {
                FaHalmaz<SulyozottEgeszGrafEl> elek = new FaHalmaz<SulyozottEgeszGrafEl>();
                for (int i = 0; i < M.GetLength(0); i++)
                {
                    for (int j = 0; j < M.GetLength(1); j++)
                    {
                        if (M[i, j] is not 0f) elek.Beszur(new SulyozottEgeszGrafEl(i, j, M[i, j]));
                    }
                }
                return elek;
            }
        }

        public float Suly(int honnan, int hova) => M[honnan, hova] is 0f ? throw new NincsElKivetel() : M[honnan, hova];

        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> szomszedok = new FaHalmaz<int>();
            for (int i = 0; i < M.GetLength(1); i++)
            {
                if (M[csucs, i] is not 0f) szomszedok.Beszur(i);
            }
            //for (int i = 0; i < M.GetLength(0); i++)
            //{
            //    if (!float.IsNaN(M[i, csucs]) && !szomszedok.Eleme(i)) szomszedok.Beszur(i);
            //}
            return szomszedok;
        }

        public void UjEl(int honnan, int hova, float suly) => M[honnan, hova] = suly;

        public bool VezetEl(int honnan, int hova) => M[honnan, hova] is not 0f;
    }

    public class Utkereses
    {

        public static Szotar<V, float> Dijkstra<V, E>(SulyozottGraf<V, E> g, V start) where V : IComparable
        //NEM MEGY A TESZT MIATT, AHOL CSÚCSMÁTRIX VAN, OTT A SZOMSZÉDSÁG NEM KÖLCSÖNÖS, A TENGELYESEN TÜKRÖZÖTT CSÚCSMÁTRIXBAN KÖLCSÖNÖS
        //DE A TESZTBEN HIÁNYZIK A (0,4) PÁRJA, EZÉRT A TESZT SZERINT NINCS A 4-NEK SZOMSZÉDJA
        {
            HasitoSzotarTulcsordulasiTerulettel<V, float> L = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            HasitoSzotarTulcsordulasiTerulettel<V, V?> P = new HasitoSzotarTulcsordulasiTerulettel<V, V?>(g.CsucsokSzama);
            KupacPrioritasosSor<V> S = new KupacPrioritasosSor<V>(g.CsucsokSzama, (x, y) => { return L.Kiolvas(x) < L.Kiolvas(y); });
            
            g.Csucsok.Bejar(x =>
            {
                L.Beir(x, float.PositiveInfinity);
                P.Beir(x, default);
                S.Sorba(x);
            });
            L.Beir(start, 0f);
            S.Frissit(start);
            while (!S.Ures)
            {
                V u = S.Sorbol();
                
                g.Szomszedai(u).Bejar(x =>
                {
                    float uert = L.Kiolvas(u);
                    float xert = L.Kiolvas(x);
                    if (uert + g.Suly(u,x) < xert)
                    {
                        L.Beir(x, uert + g.Suly(u, x));
                        P.Beir(x, u);
                        S.Frissit(x);
                    }
                });
            }
            return L;
        }
    }

    public class FeszitofaKereses
    {
        public static Szotar<V, V> Prim<V, E>(SulyozottGraf<V, E> g, V start) where V : IComparable, IComparable<V>
        {
            //melyik csúcsot melyikből kötöttük be
            HasitoSzotarTulcsordulasiTerulettel<V, float> K = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            HasitoSzotarTulcsordulasiTerulettel<V, V> P = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);
            KupacPrioritasosSor<V> S = new KupacPrioritasosSor<V>(g.CsucsokSzama, (x, y) => { return K.Kiolvas(x) < K.Kiolvas(y);});
            Halmaz<V> halm = new FaHalmaz<V>();
            int idx = 0;
            g.Csucsok.Bejar(x =>
            {
                K.Beir(x, float.MaxValue);
                S.Sorba(x);
                halm.Beszur(x);
                idx++;
            });
            K.Beir(start, 0);
            while (!S.Ures)
            {
                V u = S.Sorbol();
                halm.Torol(u);
                g.Szomszedai(u).Bejar(x =>
                {
                    if (halm.Eleme(x) && g.Suly(u, x) < K.Kiolvas(x))
                    {
                        K.Beir(x, g.Suly(u, x));
                        P.Beir(x, u);
                        S.Frissit(x);
                    }
                });
            }
            return P;
        }
        public static Halmaz<E> Kruskal<V, E>(SulyozottGraf<V, E> g) where E : SulyozottGrafEl<V>, IComparable<E> where V : IComparable<V>
        {
            FaHalmaz<E> A = new FaHalmaz<E>();
            LancoltLista<V> CS = new LancoltLista<V>();
            g.Csucsok.Bejar(CS.Hozzafuz);
            FaHalmaz<V> csucshalmazok = new FaHalmaz<V>();
            KupacPrioritasosSor<E> grafelsulyszerint = new KupacPrioritasosSor<E>(g.ElekSzama + 1, (x, y) => x.Suly < y.Suly);
            g.Elek.Bejar(grafelsulyszerint.Sorba);
            while (!grafelsulyszerint.Ures)
            {
                E u = grafelsulyszerint.Sorbol();
                V honnan = u.Honnan;
                V hova = u.Hova;
                if (!honnan.Equals(hova) && !(csucshalmazok.Eleme(honnan) && csucshalmazok.Eleme(hova)))
                {
                    A.Beszur(u);
                    g.Szomszedai(honnan).Bejar(csucshalmazok.Beszur);
                    g.Szomszedai(hova).Bejar(x =>
                    {
                        if (!csucshalmazok.Eleme(x)) csucshalmazok.Beszur(x);
                    });

                }
            }
            return A;
        }
    }
}
