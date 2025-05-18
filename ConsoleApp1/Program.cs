using System;
using System.Collections.Generic;

namespace BankPelita
{

    class Nasabah
    {
        public string NomorRekening { get; private set; }
        public string Nama { get; set; }
        public decimal Saldo { get; private set; }

        public Nasabah(string nomorRekening, string nama, decimal saldoAwal)
        {
            NomorRekening = nomorRekening;
            Nama = nama;
            Saldo = saldoAwal;
        }

        public bool TarikDana(decimal jumlah)
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Jumlah penarikan harus lebih dari 0");
                return false;
            }

            if (jumlah > Saldo)
            {
                Console.WriteLine("Saldo tidak mencukupi");
                return false;
            }

            Saldo -= jumlah;
            Console.WriteLine($"Penarikan Rp {jumlah:N0} berhasil. Saldo saat ini: Rp {Saldo:N0}");
            return true;
        }

        public void SetorTunai(decimal jumlah)
        {
            if (jumlah <= 0)
            {
                Console.WriteLine("Jumlah setoran harus lebih dari 0");
                return;
            }

            Saldo += jumlah;
            Console.WriteLine($"Setoran Rp {jumlah:N0} berhasil. Saldo saat ini: Rp {Saldo:N0}");
        }

        public void TampilkanInfo()
        {
            Console.WriteLine("\n=== INFORMASI REKENING ===");
            Console.WriteLine($"Nomor Rekening: {NomorRekening}");
            Console.WriteLine($"Nama: {Nama}");
            Console.WriteLine($"Saldo: Rp {Saldo:N0}");
            Console.WriteLine("========================\n");
        }
    }

    class Bank
    {
        private Dictionary<string, Nasabah> daftarNasabah;

        public Bank()
        {
            daftarNasabah = new Dictionary<string, Nasabah>();
        }

        public void TambahNasabah(string nomorRekening, string nama, decimal saldoAwal)
        {
            if (daftarNasabah.ContainsKey(nomorRekening))
            {
                Console.WriteLine("Nomor rekening sudah terdaftar");
                return;
            }

            Nasabah nasabahBaru = new Nasabah(nomorRekening, nama, saldoAwal);
            daftarNasabah.Add(nomorRekening, nasabahBaru);
        }

        public Nasabah CariNasabah(string nomorRekening)
        {
            if (daftarNasabah.ContainsKey(nomorRekening))
            {
                return daftarNasabah[nomorRekening];
            }
            return null;
        }

        public bool Transfer(string rekeningAsal, string rekeningTujuan, decimal jumlah)
        {
            if (!daftarNasabah.ContainsKey(rekeningAsal))
            {
                Console.WriteLine("Rekening asal tidak ditemukan");
                return false;
            }

            if (!daftarNasabah.ContainsKey(rekeningTujuan))
            {
                Console.WriteLine("Rekening tujuan tidak ditemukan");
                return false;
            }

            if (rekeningAsal == rekeningTujuan)
            {
                Console.WriteLine("Tidak dapat melakukan transfer ke rekening sendiri");
                return false;
            }

            Nasabah nasabahAsal = daftarNasabah[rekeningAsal];
            Nasabah nasabahTujuan = daftarNasabah[rekeningTujuan];

            if (nasabahAsal.TarikDana(jumlah))
            {
                nasabahTujuan.SetorTunai(jumlah);
                Console.WriteLine($"Transfer Rp {jumlah:N0} dari {nasabahAsal.Nama} ke {nasabahTujuan.Nama} berhasil");
                return true;
            }
            return false;
        }

        public void TampilkanSemuaNasabah()
        {
            Console.WriteLine("\n=== DAFTAR NASABAH BANK PELITA ===");
            if (daftarNasabah.Count == 0)
            {
                Console.WriteLine("Belum ada nasabah terdaftar");
            }
            else
            {
                foreach (var nasabah in daftarNasabah.Values)
                {
                    Console.WriteLine($"Nomor Rekening: {nasabah.NomorRekening}, Nama: {nasabah.Nama}, Saldo: Rp {nasabah.Saldo:N0}");
                }
            }
            Console.WriteLine("================================\n");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Bank bankPelita = new Bank();
            bool aplikasiBerjalan = true;

 
            bankPelita.TambahNasabah("1001", "Budi Santoso", 1000000);
            bankPelita.TambahNasabah("1002", "Siti Aminah", 5000000);
            bankPelita.TambahNasabah("1003", "Ahmad Wijaya", 7500000);

            Console.WriteLine("Selamat Datang di Sistem Layanan Digital Bank Pelita");

            while (aplikasiBerjalan)
            {
                Console.WriteLine("\n=== MENU UTAMA ===");
                Console.WriteLine("1. Informasi Rekening");
                Console.WriteLine("2. Setor Tunai");
                Console.WriteLine("3. Tarik Dana");
                Console.WriteLine("4. Transfer");
                Console.WriteLine("5. Daftar Nasabah");
                Console.WriteLine("0. Keluar");
                Console.Write("Pilih menu (0-5): ");

                string pilihan = Console.ReadLine();

                switch (pilihan)
                {
                    case "1": // Informasi Rekening
                        Console.Write("Masukkan nomor rekening: ");
                        string nomorRekening = Console.ReadLine();
                        Nasabah nasabah = bankPelita.CariNasabah(nomorRekening);
                        if (nasabah != null)
                        {
                            nasabah.TampilkanInfo();
                        }
                        else
                        {
                            Console.WriteLine("Nomor rekening tidak ditemukan");
                        }
                        break;

                    case "2": // Setor Tunai
                        Console.Write("Masukkan nomor rekening: ");
                        nomorRekening = Console.ReadLine();
                        nasabah = bankPelita.CariNasabah(nomorRekening);
                        if (nasabah != null)
                        {
                            Console.Write("Masukkan jumlah setoran: Rp ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal jumlahSetor))
                            {
                                nasabah.SetorTunai(jumlahSetor);
                            }
                            else
                            {
                                Console.WriteLine("Jumlah setoran tidak valid");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nomor rekening tidak ditemukan");
                        }
                        break;

                    case "3": // Tarik Dana
                        Console.Write("Masukkan nomor rekening: ");
                        nomorRekening = Console.ReadLine();
                        nasabah = bankPelita.CariNasabah(nomorRekening);
                        if (nasabah != null)
                        {
                            Console.Write("Masukkan jumlah penarikan: Rp ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal jumlahTarik))
                            {
                                nasabah.TarikDana(jumlahTarik);
                            }
                            else
                            {
                                Console.WriteLine("Jumlah penarikan tidak valid");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nomor rekening tidak ditemukan");
                        }
                        break;

                    case "4": // Transfer
                        Console.Write("Masukkan nomor rekening asal: ");
                        string rekeningAsal = Console.ReadLine();
                        Console.Write("Masukkan nomor rekening tujuan: ");
                        string rekeningTujuan = Console.ReadLine();

                        Console.Write("Masukkan jumlah transfer: Rp ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal jumlahTransfer))
                        {
                            bankPelita.Transfer(rekeningAsal, rekeningTujuan, jumlahTransfer);
                        }
                        else
                        {
                            Console.WriteLine("Jumlah transfer tidak valid");
                        }
                        break;

                    case "5": // Daftar Nasabah
                        bankPelita.TampilkanSemuaNasabah();
                        break;

                    case "0": // Keluar
                        aplikasiBerjalan = false;
                        Console.WriteLine("Terima kasih telah menggunakan Layanan Digital Bank Pelita");
                        break;

                    default:
                        Console.WriteLine("Pilihan tidak valid. Silakan coba lagi.");
                        break;
                }
            }
        }
    }
}