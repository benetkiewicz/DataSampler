namespace DevProductivity.Tests
{
    using System;

    using DevProductivity.DataTools;

    using NUnit.Framework;

    [TestFixture]
    public class ReflectionHelperTests
    {
        private class Sample
        {
            public int Foo { get; set; }

            public int Bar;

            public int Baz()
            {
                return 1;
            }

            public Sub Sub { get; set; }
        }

        private class Sub
        {
            public int SubFoo { get; set; }
            public SubSub SubSub { get; set; }
        }

        private class SubSub
        {
            public int SubSubFoo { get; set; }
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void FieldThrowsExceptionOnlyPropertiesAccepted()
        {
            string result = ReflectionHelper<Sample>.GetFullPropertyName(x => x.Bar);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void MethodThrowsExceptionOnlyPropertiesAccepted()
        {
            string result = ReflectionHelper<Sample>.GetFullPropertyName(x => x.Baz());
        }

        [Test]
        public void PropertyConfigurationAddNewConfigEntry()
        {
            string result = ReflectionHelper<Sample>.GetFullPropertyName(x => x.Foo);
            Assert.AreEqual("Foo", result);
        }

        [Test]
        public void NestedPropertyConfigurationAddNewConfigEntry()
        {
            string result = ReflectionHelper<Sample>.GetFullPropertyName(x => x.Sub.SubFoo);
            Assert.AreEqual("Sub.SubFoo", result);
        }

        [Test]
        public void DoubleNestedPropertyConfigurationAddNewConfigEntry()
        {
            string result = ReflectionHelper<Sample>.GetFullPropertyName(x => x.Sub.SubSub.SubSubFoo);
            Assert.AreEqual("Sub.SubSub.SubSubFoo", result);
        }
    }
}