using NUnit.Framework;
using System;
using System.IO;

namespace PokerCalculator.Tests.Integration.HandRankCalculator.IntegerBased
{
	[TestFixture]
	public class PokerListFromCommaSeparatedFileRetrievalServiceIntegrationTests : LocalTestBase
	{
		private PokerListRetrievalRetrievalService _instance;
		private readonly string TestDirectoryPath = $"{TestContext.CurrentContext.WorkDirectory}\\TestDirectory";

		[SetUp]
		protected override void Setup()
		{
			base.Setup();

			_instance = new PokerListRetrievalRetrievalService();

			Directory.CreateDirectory(TestDirectoryPath);
		}

		[TearDown]
		protected void TearDown()
		{
			Directory.Delete(TestDirectoryPath, true);
		}

		#region RetrieveListFromFile

		[Test]
		public void RetrieveListFromFile_WHERE_file_doesnt_exist_SHOULD_throw_error()
		{
			//act + assert
			var actualException = Assert.Throws<ArgumentException>(() => _instance.RetrieveListFromFile("ashfjasdfs.txt"));
			Assert.That(actualException.Message, Is.EqualTo("File does not exist"));
		}

		[Test]
		public void RetrieveListFromFile_WHERE_file_is_empty_SHOULD_return_empty_list()
		{
			//arrange
			var filePath = $"{TestDirectoryPath}\\emptyFile";
			File.WriteAllText(filePath, "");

			//act
			var actual = _instance.RetrieveListFromFile(filePath);

			//assert
			Assert.That(actual, Is.Empty);
		}

		[Test]
		public void RetrieveListFromFile_WHERE_file_is_not_empty_SHOULD_return_values_as_list()
		{
			//arrange
			var filePath = $"{TestDirectoryPath}\\file";
			const string valuesAsString = "1, 2,3,5, 10,   15, 18, 20";
			File.WriteAllText(filePath, valuesAsString);

			//act
			var actual = _instance.RetrieveListFromFile(filePath);

			//assert
			Assert.That(actual, Has.Count.EqualTo(8));
			Assert.That(actual[0], Is.EqualTo(1));
			Assert.That(actual[1], Is.EqualTo(2));
			Assert.That(actual[2], Is.EqualTo(3));
			Assert.That(actual[3], Is.EqualTo(5));
			Assert.That(actual[4], Is.EqualTo(10));
			Assert.That(actual[5], Is.EqualTo(15));
			Assert.That(actual[6], Is.EqualTo(18));
			Assert.That(actual[7], Is.EqualTo(20));
		}

		#endregion
	}
}
