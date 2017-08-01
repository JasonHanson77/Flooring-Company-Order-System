using FlooringMastery.BLL;
using FlooringMastery.Models;
using Ninject;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlooringMastery.Tests
{
    [TestFixture]
    public class TaxInfoProdRepositoryTest
    {
        [Test]
        public void CanLoadTaxInfoFromFileTest()
        {
            ITaxRepository repo = DIContainer.Kernel.Get<ITaxRepository>();

            List<TaxInfo> taxInfo = repo.GetTaxInfo();

            Assert.IsNotNull(taxInfo);

            Assert.AreEqual(taxInfo[0].StateAbbreviation, "OH");
            Assert.AreEqual(taxInfo[0].State, "Ohio");
            Assert.AreEqual(taxInfo[0].TaxRate, 6.25m);

            Assert.AreEqual(taxInfo[1].StateAbbreviation, "PA");
            Assert.AreEqual(taxInfo[1].State, "Pennsylvania");
            Assert.AreEqual(taxInfo[1].TaxRate, 6.75m);

            Assert.AreEqual(taxInfo[2].StateAbbreviation, "MI");
            Assert.AreEqual(taxInfo[2].State, "Michigan");
            Assert.AreEqual(taxInfo[2].TaxRate, 5.75m);

            Assert.AreEqual(taxInfo[3].StateAbbreviation, "IN");
            Assert.AreEqual(taxInfo[3].State, "Indiana");
            Assert.AreEqual(taxInfo[3].TaxRate, 6.00m);

        }
    }
}
