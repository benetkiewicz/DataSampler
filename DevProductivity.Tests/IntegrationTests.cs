namespace DevProductivity.Tests
{
    using System;

    using DevProductivity.DataTools;

    using NUnit.Framework;

    [TestFixture]
    public class IntegrationTests
    {
        public class Model
        {
            public int Foo { get; set; }

            public string Bar { get; set; }

            public DateTime Baz { get; set; }

            public Sub Sub { get; set; }
        }

        public class Sub
        {
            public int SubFoo { get; set; }
            public SubSub SubSub { get; set; }
        }

        public class SubSub
        {
            public int SubSubFoo { get; set; }
        }

        [Test]
        public void MixedModelDataShouldBeGenerated()
        {
            int i = 0, j = 0;
            var result = new DataSampler<Model>()
            .AddPropertyConfiguration(x => x.Foo, () => i++)
            .AddPropertyConfiguration(x => x.Baz, () => DateTime.Now.AddDays(j++))
            .AddPropertyConfiguration(x => x.Bar, () => Guid.NewGuid().ToString())
            .GenerateListOf(2);
            Assert.AreEqual(0, result[0].Foo);
            Assert.AreEqual(1, result[1].Foo);
            Assert.AreNotEqual("aaaaaaaa", result[0].Bar);
            Assert.AreEqual(result[0].Baz.Day+1, result[1].Baz.Day);
        }

        [Test]
        public void UnknownPropertyShouldRemainNull()
        {
            var result = new DataSampler<Model>()
                .AddPropertyConfiguration(x => x.Foo, () => 1)
                .GenerateListOf(2);
            Assert.IsNotNull(result);
            Assert.IsNull(result[0].Bar);
        }

        [Test]
        public void UnknownSubTypePropertyShouldRemainNull()
        {
            var result = new DataSampler<Model>()
                .AddPropertyConfiguration(x => x.Foo, () => 1)
                .AddPropertyConfiguration(x => x.Sub.SubFoo, () => 2)
                .GenerateListOf(2);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Foo);
            Assert.IsNull(result[0].Sub);
        }

        [Test]
        public void SimpleInitializerOutOfScopeShouldWork()
        {
            int i = 0;
            var result = new DataSampler<Model>()
                .AddPropertyConfiguration(x => x.Foo, () => i++)
                .GenerateListOf(3);

            Assert.AreEqual(0, result[0].Foo);
            Assert.AreEqual(1, result[1].Foo);
            Assert.AreEqual(2, result[2].Foo);
        }

        [Test]
        public void NestedPropertyShouldWork()
        {
            var result = new DataSampler<Model>()
                .AddPropertyConfiguration(x => x.Sub.SubFoo, () => 3)
                .AddKnownType(typeof(Sub))
                .GenerateListOf(1);
            Assert.AreEqual(3, result[0].Sub.SubFoo);
        }

        [Test]
        public void DoubleNestedPropertyShouldWork()
        {
            var result = new DataSampler<Model>()
                .AddPropertyConfiguration(x => x.Sub.SubFoo, () => 3)
                .AddPropertyConfiguration(x => x.Sub.SubSub.SubSubFoo, () => 5)
                .AddKnownType(typeof(Sub))
                .AddKnownType(typeof(SubSub))
                .GenerateListOf(1);
            Assert.AreEqual(3, result[0].Sub.SubFoo);
            Assert.AreEqual(5, result[0].Sub.SubSub.SubSubFoo);
        }
    }
}