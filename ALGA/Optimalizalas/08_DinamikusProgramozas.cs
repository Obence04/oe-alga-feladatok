using System;

namespace OE.ALGA.Optimalizalas
{
    // 8. heti labor feladat - Tesztek: 08_DinamikusProgramozasTesztek.cs
    public class DinamikusHatizsakPakolas
    {
        HatizsakProblema problema;
        public int LepesSzam { get; private set; }
        public DinamikusHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
            LepesSzam = 0;
        }
        public float[,] TablazatFeltoltes()
        {
            float[,] F = new float[problema.n + 1, problema.Wmax + 1];
            for (int t = 0; t < problema.n; t++) //t, mint tárgy (t-edik tárgy)
            {
                F[t, 0] = 0;
            }
            for (int i = 0; i < problema.Wmax; i++) //h, mint hely (0-tól max helyig)
            {
                F[0, i] = 0;
            }

            for (int t = 1; t < problema.n+1; t++)
            {
                for (int h = 1; h < problema.Wmax+1; h++)
                {
                    LepesSzam++;
                    if (h >= problema.w[t - 1]) F[t, h] = Math.Max(F[t - 1, h], F[t - 1, h - problema.w[t - 1]] + problema.p[t - 1]);
                    else F[t, h] = F[t - 1, h];
                }
            }
            return F;
        }
        public float OptimalisErtek()
        {
            return TablazatFeltoltes()[problema.n, problema.Wmax];
        }
        public bool[] OptimalisMegoldas()
        {
            float[,] F = TablazatFeltoltes();
            bool[] O = new bool[problema.n];
            int t = problema.n;
            int h = problema.Wmax;
            while (t > 0 && h > 0)
            {
                if (F[t, h] != F[t - 1, h])
                {
                    O[t - 1] = true;
                    h -= problema.w[t - 1];
                }
                t--;
            }
            return O;
        }
    }
}
