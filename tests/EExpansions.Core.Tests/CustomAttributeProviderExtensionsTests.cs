using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace EExpansions;

public class CustomAttributeProviderExtensionsTests
{
    [Display(Name = ModelDisplayName)]
    private class VerificationModel
    {
        public const string ModelDisplayName = "VerificationModelDisplayName";
        public const string FieldDisplayName = "VerificationFieldDisplayName";
        public const string PropertyDisplayName = "VerificationPropertyDisplayName";
        public const string MethodDisplayName = "VerificationMethodDisplayName";

        [Display(Name = FieldDisplayName)]
        public string VerificationField = string.Empty;

        [Display(Name = PropertyDisplayName)]
        public virtual string VerificationProperty { get; set; } = string.Empty;

        [Display(Name = MethodDisplayName)]
        public virtual string VerificationMethod => string.Empty;
    }

    private class ChildVerificationModel : VerificationModel
    {
        public override string VerificationProperty {
            get => base.VerificationProperty;
            set => base.VerificationProperty = value;
        }

        public override string VerificationMethod => base.VerificationMethod;
    }

    private class TestDataClass : TestHelper.TestDataClass
    {
        protected override void Initialize()
        {
            var model = new VerificationModel();
            var child = new ChildVerificationModel();
            TestData.Add(new object?[] { null, true, false });
            TestData.Add(new object?[] { model.GetType(), false, true });
            TestData.Add(new object?[] { model.GetType().GetField(nameof(VerificationModel.VerificationField)), false, true });
            TestData.Add(new object?[] { model.GetType().GetProperty(nameof(VerificationModel.VerificationProperty)), false, true });
            TestData.Add(new object?[] { model.GetType().GetMethod(nameof(VerificationModel.VerificationMethod)), false, true });
            TestData.Add(new object?[] { child.GetType(), true, true });
            TestData.Add(new object?[] { child.GetType(), false, false });
            TestData.Add(new object?[] { child.GetType().GetField(nameof(VerificationModel.VerificationField)), true, true });
            TestData.Add(new object?[] { child.GetType().GetField(nameof(VerificationModel.VerificationField)), false, true });
            TestData.Add(new object?[] { child.GetType().GetProperty(nameof(VerificationModel.VerificationProperty)), true, false });
            TestData.Add(new object?[] { child.GetType().GetProperty(nameof(VerificationModel.VerificationProperty)), false, false });
            TestData.Add(new object?[] { child.GetType().GetMethod(nameof(VerificationModel.VerificationMethod)), true, true });
            TestData.Add(new object?[] { child.GetType().GetMethod(nameof(VerificationModel.VerificationMethod)), false, false });
        }
    }

    [Theory]
    [ClassData(typeof(TestDataClass))]
    public void GetCustomAttributes(ICustomAttributeProvider provider, bool inherit, bool shouldBeFound)
    {
        if (provider is null)
        {
            Assert.Throws<ArgumentNullException>(() => provider!.GetCustomAttributes<DisplayAttribute>(inherit));
            return;
        }
        if (shouldBeFound)
        {
            var attrs = provider.GetCustomAttributes<DisplayAttribute>(inherit);
            Assert.NotEmpty(attrs);
            Assert.All(attrs, a =>
                Assert.IsType<DisplayAttribute>(a)
            );
        }
        else
        {
            Assert.Empty(
                provider.GetCustomAttributes<DisplayAttribute>(inherit)
            );
        }
    }

    [Theory]
    [ClassData(typeof(TestDataClass))]
    public void GetCustomAttribute(ICustomAttributeProvider provider, bool inherit, bool shouldBeFound)
    {
        if (provider is null)
        {
            Assert.Throws<ArgumentNullException>(() => provider!.GetCustomAttribute<DisplayAttribute>(inherit));
            return;
        }
        if (shouldBeFound)
        {
            var attr = provider.GetCustomAttribute<DisplayAttribute>(inherit);
            Assert.NotNull(attr);
            Assert.IsType<DisplayAttribute>(attr);
        }
        else
        {
            Assert.Null(provider.GetCustomAttribute<DisplayAttribute>(inherit));
        }
    }
}
