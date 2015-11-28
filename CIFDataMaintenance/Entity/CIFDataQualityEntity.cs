using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityLibrary;

namespace CIFDataMaintenance.Entity
{
    [Table("[Sheet1$]", true, false, "[A]")]
    class CIFDataQualityEntity
    {
        //[Column(name: "GETDATE()", useTabAlias: false)]
        //public string TanggalMonitoring { get; set; }
        [Column(name: "[A]")]
        public string Wilayah { get; set; }
        [Column(name: "[B]")]
        public string Area { get; set; }
        [Column(name: "[C]")]
        public string CabangCIF { get; set; }
        [Column(name: "[D]")]
        public string OpenDate { get; set; }
        [Column(name: "[E]")]
        public string NomorCIF { get; set; }
        [Column(name: "[H]")]
        public string CabangRek { get; set; }
        [Column(name: "[I]")]
        public string NomorRekening { get; set; }
        [Column(name: "[J]")]
        public string JenisRekening { get; set; }
        [Column(name: "[K]")]
        public string MaksudTujuanHubBank { get; set; }
        [Column(name: "[L]")]
        public string Prefix { get; set; }
        [Column(name: "[M]")]
        public string NamaCIFNasabah { get; set; }
        [Column(name: "[N]")]
        public string NamaCIFNasabah2 { get; set; }
        [Column(name: "[O]")]
        public string Suffix { get; set; }
        [Column(name: "[P]")]
        public string NamaPelaporan1 { get; set; }
        [Column(name: "[Q]")]
        public string NamaPelaporan2 { get; set; }
        [Column(name: "[R]")]
        public string GolonganNasabah { get; set; }
        [Column(name: "[S]")]
        public string Golongan_N { get; set; }
        [Column(name: "[T]")]
        public string JenisDebit { get; set; }
        [Column(name: "[U]")]
        public string HubDgnBank { get; set; }
        [Column(name: "[V]")]
        public string TmptKeluarID { get; set; }
        [Column(name: "[W]")]
        public string JenisID { get; set; }
        [Column(name: "[X]")]
        public string NomorID { get; set; }
        [Column(name: "[Y]")]
        public string JenisIDTambahanGiro { get; set; }
        [Column(name: "[Z]")]
        public string NomotIDTambahanGiro { get; set; }
        [Column(name: "[AA]")]
        public string TmptBerdiriPerusahaam { get; set; }
        [Column(name: "[AB]")]
        public string TglBerdiriPerusahaan { get; set; }
        [Column(name: "[AC]")]
        public string JenisKelamin { get; set; }
        [Column(name: "[AD]")]
        public string JenisNasabah { get; set; }
        [Column(name: "[AE]")]
        public string Kewarganegaraan { get; set; }
        [Column(name: "[AF]")]
        public string StatusPerkawinan { get; set; }
        [Column(name: "[AG]")]
        public string BUC { get; set; }
        [Column(name: "[AH]")]
        public string NamaGadisIbuKandung { get; set; }
        [Column(name: "[AI]")]
        public string AlamatBaris1 { get; set; }
        [Column(name: "[AJ]")]
        public string AlamatBaris2 { get; set; }
        [Column(name: "[AK]")]
        public string AlamatBaris3 { get; set; }
        [Column(name: "[AL]")]
        public string AlamatBaris4 { get; set; }
        [Column(name: "[AM]")]
        public string KodePos { get; set; }
        [Column(name: "[AN]")]
        public string JenisAlmtElektronikTlp1 { get; set; }
        [Column(name: "[AO]")]
        public string NomorAlmtElektronikTlp1 { get; set; }
        [Column(name: "[AP]")]
        public string JenisAlmtElektronikTlp2 { get; set; }
        [Column(name: "[AQ]")]
        public string NomorAlmtElektronikTlp2 { get; set; }
        [Column(name: "[AR]")]
        public string JenisAlmtElektronikTlp3 { get; set; }
        [Column(name: "[AS]")]
        public string NomorAlmtElektronikTlp3 { get; set; }
        [Column(name: "[AT]")]
        public string KodePekerja { get; set; }
        [Column(name: "[AU]")]
        public string NamaPemberiKerja { get; set; }
        [Column(name: "[AV]")]
        public string KodeJabatan { get; set; }
        [Column(name: "[AW]")]
        public string JobDesc { get; set; }
        [Column(name: "[AX]")]
        public string GajiPendapatanValuta { get; set; }
        [Column(name: "[AY]")]
        public string GajiPendapatanNominal { get; set; }
        [Column(name: "[AZ]")]
        public string PendapatanLainValuta { get; set; }
        [Column(name: "[BA]")]
        public string PendapatanLainNomila { get; set; }
        [Column(name: "[BB]")]
        public string OmzetValuta { get; set; }
        [Column(name: "[BC]")]
        public string OmzetNominal { get; set; }
    }
}
