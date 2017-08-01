using FlooringMastery.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Data
{
    public class TaxInfoRepository: ITaxRepository
    {
        private string _directoryPath;
        private string _taxInfoFile;
        private List<TaxInfo> _taxInfoList = new List<TaxInfo>();

        public TaxInfoRepository(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                _directoryPath = directoryPath;
                _taxInfoFile = _directoryPath + "Taxes.txt";
            }
            else
            {
                Directory.CreateDirectory(directoryPath);
                string[] files = System.IO.Directory.GetFiles(Settings.SeedDirectoryPath);

                string fileName = "Taxes.txt";


                foreach (string s in files)
                {

                    if (fileName == System.IO.Path.GetFileName(s))
                    {
                        _taxInfoFile = System.IO.Path.Combine(directoryPath, fileName);
                        System.IO.File.Copy(s, _taxInfoFile, true);
                    }
                }
            }

            CreateListFromFile(_taxInfoFile);
        }

        public void CreateListFromFile(string filePath)
        {
            
            using (StreamReader sr = new StreamReader(filePath))
            {
                sr.ReadLine();
                string line;


                while ((line = sr.ReadLine()) != null)
                {
                    TaxInfo taxInfo = new TaxInfo();

                    string[] columns = line.Split(',');

                    taxInfo.StateAbbreviation = columns[0];
                    taxInfo.State = columns[1];

                    decimal taxRate = 0m;

                    if (decimal.TryParse(columns[2], out taxRate))
                    {
                        taxInfo.TaxRate = taxRate;
                    }

                    _taxInfoList.Add(taxInfo);
                   
                }
            }
        }

        public List<TaxInfo> GetTaxInfo()
        {
            return _taxInfoList;
        }
    }
}
