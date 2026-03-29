using ConducatorModel = Conducator.Conducator;
using MasinaModel = Masina.Masina;
using CursaModel = Cursa.Cursa;

namespace AdministrareDate
{	
	public interface IStocareData
	{
		void AddMasina(MasinaModel m);
		List<MasinaModel> GetMasini();
		void RescrieFisier(List<MasinaModel> masini);
	}
}
