namespace DeReviewer.Tests.Model.Inheritance
{
    // TODO: fix commented cases
    internal class SpecificClass : AbstractClass//, IInterface
    {
        public override void Foo() {}
        
        //void IInterface.Foo() {}

        public override void Bar() {}

        public virtual void JustVirtual() {}
    }
}