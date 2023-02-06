using General.Enums;

namespace Implementation.Data
{
	[DbContextConfiguration("WeaponData")]
	public interface IWeaponData : IUniqueIndex
	{
		/// <summary>
		/// Gets or sets die to roll for attacking with this weapon. 
		/// </summary>
		Die DieToRoll { get; set; }

		/// <summary>
		/// Gets or sets weapon type.
		/// </summary>
		WeaponType Type { get; set; }
		
		//maybe write a method for custom hit boxes for each weapon?
	}
}