namespace Infrastructure.IoC
{
	public interface ISimpleContainer
	{
		void Register<TInterface, TClass>() where TClass : TInterface, new();
		TInterface Resolve<TInterface>() where TInterface : class;
		TInterface Singleton<TInterface>() where TInterface : class;
	}
}
