using System;
using DataContext.DbConfigurator;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataContextUnitTest
{
    [TestClass]
    public class DatabaseTest
    {
        /// <summary>
        /// ¶ÁÈ¡Á¬½Ó×Ö·û´®²âÊÔ
        /// </summary>
        [TestMethod]
        public void ReadConnectionStrTest()
        {
            bool readSuccessful = false;
            try
            {
                MysqlDbConfigurator mysqlDbConfigurator = new MysqlDbConfigurator();
                readSuccessful = true;
            }
            catch (Exception) { }

            Assert.IsTrue(readSuccessful);
        }
    }
}
