using System;
using NUnit.Framework;
using Stazh.Core.Helpers;

namespace Stazh.Core.Testing
{
    public class FilerHelperTests   
    {
        [Test]
        public void ObscureFileName_InputValidFilename_ReturnsNewFileNameWithOriginalExtension()
        {
            //Arrange
            var testFileName = "FileA.txt";

            //Act
            var obscuredFileName = FileHelper.ObscureFileName(testFileName);
            Console.WriteLine(obscuredFileName);

            //Assert
            Assert.Contains("txt",obscuredFileName.Split("."));
            Assert.That(testFileName != obscuredFileName);
        }
    }
}