using FlooringMastery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Data
{
    public class TaxInfoTestRepository : ITaxRepository
    {
        private static List<TaxInfo> _taxInfo = new List<TaxInfo>();

        private TaxInfo Ohio = new TaxInfo
        {
            State = "Ohio",
            StateAbbreviation = "OH",
            TaxRate = 6.25m
        };

        private TaxInfo Pennsylvania = new TaxInfo
        {
            State = "Pennsylvania",
            StateAbbreviation = "PA",
            TaxRate = 6.75m
        };

        private TaxInfo Michigan = new TaxInfo
        {
            State = "Michigan",
            StateAbbreviation = "MI",
            TaxRate = 5.75m
        };

        private TaxInfo Indiana = new TaxInfo
        {
            State = "Indiana",
            StateAbbreviation = "IN",
            TaxRate = 6.00m
        };

        public TaxInfoTestRepository()
        {
            if (_taxInfo.Count() == 0)
            {
                _taxInfo.Add(Ohio);
                _taxInfo.Add(Pennsylvania);
                _taxInfo.Add(Indiana);
                _taxInfo.Add(Michigan);
            }
        }

        public List<TaxInfo> GetTaxInfo()
        {
            return _taxInfo;
        }
    }
}
