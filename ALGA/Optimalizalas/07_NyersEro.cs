using System;

namespace OE.ALGA.Optimalizalas
{
    // 7. heti labor feladat - Tesztek: 07_NyersEroTesztek.cs
    public class HatizsakProblema
    {
        public int n { get; } //bepakolható tárgyak száma
        public int Wmax { get; } //hátizsák mérete
        public int[] w { get; } //a tárgyak súlyai
        public float[] p { get; } //a tárgyak értékei
        public HatizsakProblema(int n, int Wmax, int[] w, float[] p)
        {
            this.n = n;
            this.Wmax = Wmax;
            this.w = w;
            this.p = p;
        }
        public int OsszSuly(bool[] pakolas)
        {
            int min = Math.Min(pakolas.Length, w.Length);
            int s = 0;
            for (int i = 0; i < min; i++)
            {
                if (pakolas[i]) s += w[i];
            }
            return s;
        }
        public float OsszErtek(bool[] pakolas)
        {
            int min = Math.Min(pakolas.Length, p.Length);
            float s = 0;
            for (int i = 0; i < min; i++)
            {
                if (pakolas[i]) s += p[i];
            }
            return s;
        }
        public bool Ervenyes(bool[] pakolas) => OsszSuly(pakolas) <= Wmax;
    }
    public class NyersEro<T>
    {
        int m; //lehetséges megoldások
        Func<int, T> generator; //lehetséges megoldások generálására referencia
        Func<T, float> josag; //lehetséges megoldások értékeinek generlására referencia
        public int LepesSzam { get; private set; }
        public NyersEro(int m, Func<int, T> generator, Func<T, float> josag)
        {
            this.m = m;
            this.generator = generator;
            this.josag = josag;
            LepesSzam = 0;
        }
        public T OptimalisMegoldas()
        {
            LepesSzam = 0;
            T o = generator.Invoke(1);
            for (int i = 2; i <= m; i++)
            {
                LepesSzam++;
                T megoldas = generator.Invoke(i);
                if (josag.Invoke(megoldas) > josag.Invoke(o)) o = megoldas;
            }
            return o;
        }
    }
    public class NyersEroHatizsakPakolas
    {
        public int LepesSzam { get; private set; }
        HatizsakProblema problema;
        public NyersEroHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
            LepesSzam = 0;
        }
        public bool[] Generator(int i)
        {
            int szam = i - 1;
            bool[] K = new bool[problema.n];
            for (int j = 1; j < problema.n; j++)
            {
                K[j] = (szam / EgeszHatvany(2, (j - 1))) % 2 == 1;
            }
            return K;
        }
        public float Josag(bool[] pakolas) => problema.Ervenyes(pakolas) ? problema.OsszErtek(pakolas) : -1;
        public bool[] OptimalisMegoldas()
        {
            NyersEro<bool[]> pakolas = new NyersEro<bool[]>(EgeszHatvany(2, problema.n), Generator, Josag);
            bool[] optimalis = pakolas.OptimalisMegoldas();
            LepesSzam = pakolas.LepesSzam;
            return optimalis;
        }
        public float OptimalisErtek() => problema.OsszErtek(OptimalisMegoldas());
        static int EgeszHatvany(int alap, int kitevo)
        {
            int ertek = 1;
            for (int i = 0; i < kitevo; i++)
            {
                ertek *= alap;
            }
            return ertek;
        }
    }
}
