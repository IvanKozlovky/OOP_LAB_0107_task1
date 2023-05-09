using System;
using System.Collections.Generic;

namespace AvtoSalon
{
    public class KilkistAvtoException : Exception
    {
        public KilkistAvtoException(string message) : base(message) { }
    }

    public class PIBException : Exception
    {
        public PIBException(string message) : base(message) { }
    }

    public class AvtoSalon
    {
        public string Nazva { get; set; }
    }

    public class Avtomobil
    {
        public string Marka { get; set; }
        public int MaksKilkistPasazhyriv { get; set; }
        public decimal Vartist { get; set; }
        public int KilkistNaSkladi { get; set; }
        public bool Nayaunst { get; set; }
    }

    public class ZayavkaNaPokupku
    {
        public string PIBPokuptsya { get; set; }
        public string Nomertelefonu { get; set; }
        private List<Avtomobil> Avtomobili { get; set; } = new List<Avtomobil>();

        public void AddAvtomobil(Avtomobil avtomobil)
        {
            Avtomobili.Add(avtomobil);
        }

        public void RemoveAvtomobil(Avtomobil avtomobil)
        {
            Avtomobili.Remove(avtomobil);
        }

        public decimal RozrahuvatyVartistZamovlennya()
        {
            try
            {
                if (Avtomobili.Count == 0)
                {
                    throw new KilkistAvtoException("Кількість автомобілів дорівнює нулю");
                }

                decimal vartist = 0;
                foreach (Avtomobil avto in Avtomobili)
                {
                    vartist += avto.Vartist;
                }

                return vartist;
            }
            catch (KilkistAvtoException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
                throw;
            }
        }
    }

    public class ZayavkaNaVidkladenuPostavku : ZayavkaNaPokupku
    {
        public int VidsotokZnyzhky { get; set; }

        public new decimal RozrahuvatyVartistZamovlennya()
        {
            decimal vartist = base.RozrahuvatyVartistZamovlennya();
            return vartist * (1 - (decimal)VidsotokZnyzhky / 100);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ZayavkaNaPokupku zayavka = new ZayavkaNaPokupku { PIBPokuptsya = "" };
                Console.WriteLine("Заявка створена");
            }
            catch (PIBException ex)
            {
                Console.WriteLine("Неможливо створити заявку – не вказано ПІБ покупця");
                // повторно створити виняток
                throw;
            }

            ZayavkaNaPokupku zayavka2 = new ZayavkaNaPokupku
            {
                PIBPokuptsya = "Іванов Іван Петрович"
            };
            Avtomobil avto1 = new Avtomobil { Marka = "BMW", MaksKilkistPasazhyriv = 5, Vartist = 50000, KilkistNaSkladi = 3, Nayaunst = true };
            Avtomobil avto2 = new Avtomobil { Marka = "Audi", MaksKilkistPasazhyriv = 5, Vartist = 45000, KilkistNaSkladi = 2, Nayaunst = true };
            zayavka2.AddAvtomobil(avto1);
            zayavka2.AddAvtomobil(avto2);

            try
            {
                decimal vartistZamovlennya = zayavka2.RozrahuvatyVartistZamovlennya();
                Console.WriteLine($"Вартість замовлення: {vartistZamovlennya}");
            }
            catch (KilkistAvtoException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }

            ZayavkaNaVidkladenuPostavku zayavka3 = new ZayavkaNaVidkladenuPostavku { PIBPokuptsya = "Петро Васильович", VidsotokZnyzhky = 10 };
            zayavka3.AddAvtomobil(avto1);
            zayavka3.AddAvtomobil(avto2);

            try
            {
                decimal vartistZamovlennya = zayavka3.RozrahuvatyVartistZamovlennya();
                Console.WriteLine($"Вартість замовлення зі знижкою: {vartistZamovlennya}");
            }
            catch (KilkistAvtoException ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка: {ex.Message}");
            }
        }
    }
}
