using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class Program
{
    static List<Product> urunler = new List<Product>();

    static void Main()
    {
        UrunleriYukle();
        bool devam = true;

        while (devam)
        {
            Console.WriteLine("\n--- ÜRÜN YÖNETİM SİSTEMİ ---");
            Console.WriteLine("1. Ürün Ekle");
            Console.WriteLine("2. Ürünleri Listele");
            Console.WriteLine("3. Ürün Ara");
            Console.WriteLine("4. Ürün Sil");
            Console.WriteLine("5. Çıkış");
            Console.Write("Seçiminiz: ");

            string secim = Console.ReadLine() ?? "";

            switch (secim)
            {
                case "1":
                    UrunEkle();
                    break;

                case "2":
                    UrunleriListele();
                    break;

                case "3":
                    UrunAra();
                    break;

                case "4":
                    UrunSil();    
                    break;

                case "5":
                    devam = false;
                    Console.WriteLine("Program kapatılıyor...");
                    break;

                default:
                    Console.WriteLine("Geçersiz seçim! 1, 2, 3, 4 veya 5 giriniz.");
                    break;
            }
        }
    }

    static void UrunEkle()
    {
        Console.Write("Ürün adını giriniz: ");
        string ad = Console.ReadLine() ?? "";

        if (string.IsNullOrWhiteSpace(ad))
        {
            Console.WriteLine("Ürün adı boş olamaz!");
            return;
        }

        decimal fiyat;

        while (true)
        {
            Console.Write("Ürün fiyatını giriniz: ");

            if (decimal.TryParse(Console.ReadLine(), out fiyat)
                && fiyat >= 0)
            {
                break;
            }

            Console.WriteLine("Geçersiz fiyat! Sıfır veya pozitif bir sayı giriniz.");
        }

        int stok;

        while (true)
        {
            Console.Write("Stok adedini giriniz: ");

            if (int.TryParse(Console.ReadLine(), out stok)
                && stok >= 0)
            {
                break;
            }

            Console.WriteLine("Geçersiz stok! Sıfır veya pozitif tam sayı giriniz.");
        }

        Product yeniUrun = new Product
        {
            Name = ad,
            Price = fiyat,
            Stock = stok
        };

        urunler.Add(yeniUrun);
        UrunleriKaydet();

        Console.WriteLine("Ürün başarıyla eklendi!");
    }

    static void UrunleriListele()
    {
        if (urunler.Count == 0)
        {
            Console.WriteLine("Henüz kayıtlı ürün yok.");
            return;
        }

        Console.WriteLine("\n--- KAYITLI ÜRÜNLER ---");

        foreach (Product urun in urunler)
        {
            Console.WriteLine(
                $"Ürün: {urun.Name} | Fiyat: {urun.Price} | Stok: {urun.Stock}"
            );
        }
    }

    static void UrunAra()
    {

        Console.Write("Aranacak ürünün adını giriniz: ");
        string arananAd = Console.ReadLine() ?? "";

        Product? bulunanUrun = urunler.Find(urun => urun.Name.Equals(arananAd, StringComparison.OrdinalIgnoreCase));

        if (bulunanUrun != null)
        {
        Console.WriteLine("Ürün bulundu!");
        Console.WriteLine(
            $"Ürün: {bulunanUrun.Name} | Fiyat: {bulunanUrun.Price} | Stok: {bulunanUrun.Stock}");
        }
        else
        {
            Console.WriteLine("Ürün bulunamadı.");
        }
    }

    static void UrunSil()
    {
        Console.Write("Silinecek ürünün adını giriniz: ");
        string arananAd = Console.ReadLine() ?? "";

        Product? bulunanUrun = urunler.Find(  urun => urun.Name.Equals(arananAd, StringComparison.OrdinalIgnoreCase));

        if (bulunanUrun != null)
        {
            urunler.Remove(bulunanUrun);
            UrunleriKaydet();
            Console.WriteLine("Ürün başarıyla silindi!");
        }
        else
        {
            Console.WriteLine("Ürün bulunamadı.");
        }
    }

    static void UrunleriYukle()
    {
        if (File.Exists("product.json"))
        {
            string json = File.ReadAllText("product.json");

            List<Product>? kayitliUrunler = JsonSerializer.Deserialize<List<Product>>(json);

            if(kayitliUrunler !=null)
            {
                urunler = kayitliUrunler;
            } 
        }
    }

    static void UrunleriKaydet()
    {
        string json = JsonSerializer.Serialize(urunler);
        File.WriteAllText("product.json", json);
    }
}